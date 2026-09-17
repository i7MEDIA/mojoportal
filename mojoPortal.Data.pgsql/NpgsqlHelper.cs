#nullable enable
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;

public sealed class NpgsqlHelper
{
	static NpgsqlHelper()
	{
		// Ensures CommandType.StoredProcedure executes functions via SELECT rather than CALL
		AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);

		// Ensures DateTime values with Unspecified or Local kind map safely to timestamp columns
		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
	}

	private NpgsqlHelper()
	{ }


	private static void PrepareCommand(
		NpgsqlCommand command,
		NpgsqlConnection connection,
		NpgsqlTransaction? transaction,
		CommandType commandType,
		string commandText,
		NpgsqlParameter[] commandParameters
	)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}

		if (string.IsNullOrEmpty(commandText))
		{
			throw new ArgumentNullException("commandText");
		}

		if (commandType == CommandType.StoredProcedure)
		{
			commandType = CommandType.Text;
			string trimmedText = commandText.Trim();
			if (trimmedText.EndsWith(";"))
			{
				trimmedText = trimmedText.Substring(0, trimmedText.Length - 1).Trim();
			}

			if (trimmedText.Contains("("))
			{
				if (!trimmedText.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase)
					&& !trimmedText.StartsWith("CALL", StringComparison.OrdinalIgnoreCase))
				{
					commandText = "SELECT * FROM " + trimmedText;
				}
				else
				{
					commandText = trimmedText;
				}
			}
			else
			{
				var paramPlaceholders = new List<string>();
				if (commandParameters != null)
				{
					for (int i = 0; i < commandParameters.Length; i++)
					{
						var p = commandParameters[i];
						if (p != null)
						{
							if (string.IsNullOrEmpty(p.ParameterName))
							{
								p.ParameterName = $"p{i}";
							}

							string pName = p.ParameterName;
							if (!pName.StartsWith(":") && !pName.StartsWith("@"))
							{
								pName = ":" + pName;
							}
							paramPlaceholders.Add(pName);
						}
					}
				}
				commandText = "SELECT * FROM " + trimmedText + "(" + string.Join(", ", paramPlaceholders) + ")";
			}
		}

		command.Connection = connection;
		command.CommandType = commandType;
		command.CommandText = commandText;

		if (transaction is not null)
		{
			if (transaction.Connection is null)
			{
				throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
			}

			command.Transaction = transaction;
		}

		if (commandParameters != null)
		{
			AttachParameters(command, commandParameters);
		}

		return;
	}


	private static void AttachParameters(
		NpgsqlCommand command,
		NpgsqlParameter[] commandParameters
	)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}

		if (commandParameters != null)
		{
			foreach (NpgsqlParameter p in commandParameters)
			{
				if (p != null)
				{
					if (
						(
							p.Direction == ParameterDirection.InputOutput ||
							p.Direction == ParameterDirection.Input
						) &&
						p.Value == null
					)
					{
						p.Value = DBNull.Value;
					}

					command.Parameters.Add(p);
				}
			}
		}
	}


	public static int ExecuteNonQuery(
		string connectionString,
		CommandType commandType,
		string commandText,
		params NpgsqlParameter[] commandParameters
	)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new ArgumentNullException("connectionString");
		}

		connectionString = ConnectionString.CleanConnectionString(connectionString);

		using NpgsqlConnection connection = new NpgsqlConnection(connectionString);

		connection.Open();

		using NpgsqlCommand command = new NpgsqlCommand();

		bool isStoredProcedure = commandType == CommandType.StoredProcedure;

		PrepareCommand(
			command,
			connection,
			null,
			commandType,
			commandText,
			commandParameters
		);

		if (isStoredProcedure)
		{
			object? scalar = command.ExecuteScalar();
			if (scalar != null && scalar != DBNull.Value && int.TryParse(scalar.ToString(), out int affected))
			{
				return affected;
			}
			return 0;
		}

		return command.ExecuteNonQuery();
	}


	public static int ExecuteNonQuery(
		NpgsqlTransaction transaction,
		CommandType commandType,
		string commandText,
		params NpgsqlParameter[] commandParameters
	)
	{
		if (transaction is null)
		{
			throw new ArgumentNullException("transaction");
		}

		if (transaction is not null && transaction.Connection == null)
		{
			throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
		}

		var command = new NpgsqlCommand();
		bool isStoredProcedure = commandType == CommandType.StoredProcedure;

		if (transaction != null)
		{
			PrepareCommand(
				command,
				transaction.Connection,
				transaction,
				commandType,
				commandText,
				commandParameters
			);
		}

		if (isStoredProcedure)
		{
			object? scalar = command.ExecuteScalar();
			if (scalar != null && scalar != DBNull.Value && int.TryParse(scalar.ToString(), out int affected))
			{
				return affected;
			}
			return 0;
		}

		return command.ExecuteNonQuery();
	}


	public static NpgsqlDataReader ExecuteReader(
		string connectionString,
		CommandType commandType,
		string commandText,
		params NpgsqlParameter[] commandParameters
	)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new ArgumentNullException("connectionString");
		}

		connectionString = ConnectionString.CleanConnectionString(connectionString);

		NpgsqlConnection? connection = null;

		try
		{
			var cmd = new NpgsqlCommand();

			connection = new NpgsqlConnection(connectionString);
			connection.Open();

			PrepareCommand(
				cmd,
				connection,
				null,
				commandType,
				commandText,
				commandParameters
			);

			return cmd.ExecuteReader(CommandBehavior.CloseConnection);
		}
		catch
		{
			if (connection is not null && connection.State == ConnectionState.Open)
			{
				connection.Close();
			}

			throw;
		}
	}


	public static object ExecuteScalar(
		string connectionString,
		CommandType commandType,
		string commandText,
		params NpgsqlParameter[] commandParameters
	)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new ArgumentNullException("connectionString");
		}

		connectionString = ConnectionString.CleanConnectionString(connectionString);

		using var connection = new NpgsqlConnection(connectionString);

		connection.Open();

		using var command = new NpgsqlCommand();

		PrepareCommand(
			command,
			connection,
			null,
			commandType,
			commandText,
			commandParameters
		);

		return command.ExecuteScalar();
	}


	public static DataSet ExecuteDataset(
		string connectionString,
		CommandType commandType,
		string commandText,
		params NpgsqlParameter[] commandParameters
	)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new ArgumentNullException("connectionString");
		}

		connectionString = ConnectionString.CleanConnectionString(connectionString);

		using var connection = new NpgsqlConnection(connectionString);

		connection.Open();

		using var command = new NpgsqlCommand();

		PrepareCommand(
			command,
			connection,
			null,
			commandType,
			commandText,
			commandParameters
		);

		using var adapter = new NpgsqlDataAdapter(command);
		var dataSet = new DataSet();

		adapter.Fill(dataSet);

		return dataSet;
	}
}
