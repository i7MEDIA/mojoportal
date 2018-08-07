// ===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
// ===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================
// 
// Original copyright above
//
// Forked From Enterprise Library licensed under Ms-Pl http://www.codeplex.com/entlib
// but implementing a sub set of the API from the 2.0 Application Blocks SqlHelper
// using implementation from the newer Ms-Pl version
// Modified and adaptated for NpgSql
// 
// Last Modified 2010-01-28

using Npgsql;
using System;
using System.Data;

namespace mojoPortal.Data
{
	public sealed class NpgsqlHelper
	{
		private NpgsqlHelper()
		{ }


		private static void PrepareCommand(
			NpgsqlCommand command,
			NpgsqlConnection connection,
			NpgsqlTransaction transaction,
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

			command.Connection = connection;
			command.CommandType = commandType;
			command.CommandText = commandText;

			if (transaction != null)
			{
				if (transaction.Connection == null)
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

			using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
			{
				connection.Open();

				using (NpgsqlCommand command = new NpgsqlCommand())
				{
					PrepareCommand(
						command,
						connection,
						null,
						commandType,
						commandText,
						commandParameters
					);

					return command.ExecuteNonQuery();
				}

			}
		}


		public static int ExecuteNonQuery(
			NpgsqlTransaction transaction,
			CommandType commandType,
			string commandText,
			params NpgsqlParameter[] commandParameters
		)
		{
			if (transaction == null)
			{
				throw new ArgumentNullException("transaction");
			}

			if (transaction != null && transaction.Connection == null)
			{
				throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
			}

			NpgsqlCommand command = new NpgsqlCommand();

			PrepareCommand(
				command,
				transaction.Connection,
				transaction,
				commandType,
				commandText,
				commandParameters
			);

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

			NpgsqlConnection connection = null;

			try
			{
				NpgsqlCommand cmd = new NpgsqlCommand();

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
				if (connection != null && connection.State == ConnectionState.Open)
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

			using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
			{
				connection.Open();

				using (NpgsqlCommand command = new NpgsqlCommand())
				{
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
			}
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

			using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
			{
				connection.Open();

				using (NpgsqlCommand command = new NpgsqlCommand())
				{
					PrepareCommand(
						command,
						connection,
						null,
						commandType,
						commandText,
						commandParameters
					);

					using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
					{
						DataSet dataSet = new DataSet();

						adapter.Fill(dataSet);

						return dataSet;
					}
				}
			}
		}
	}
}