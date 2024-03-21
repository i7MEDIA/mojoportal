using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySqlConnector;

namespace mojoPortal.Data;

public static class CommandHelper
{
	#region NonQuery
	/// <summary>
	/// Executes as single command against a MySQL database.
	/// </summary>
	/// <param name="connectionString"><see cref="MySqlConnection.ConnectionString"/> to use.</param>
	/// <param name="commandText">The SQL command to be executed.</param>
	/// <param name="commandParameters">An IEnumberable of <see cref="MySqlParameter"/> objects to use with the command.</param>
	/// <returns>The number of rows affected.</returns>
	/// <remarks>For UPDATE, INSERT, and DELETE statements, the return value is the number of rows affected by the command.
	/// For stored procedures, the return value is the number of rows affected by the last statement in the stored procedure,
	/// or zero if the last statement is a SELECT. For all other types of statements, the return value is -1.</remarks>
	public static int ExecuteNonQuery(string connectionString, string commandText, IEnumerable<MySqlParameter> commandParameters = null) => ExecuteNonQuery(new MySqlConnection(connectionString), commandText, commandParameters);

	/// <summary>
	/// Executes as single command against a MySQL database.
	/// </summary>
	/// <param name="connectionString"><see cref="MySqlConnection.ConnectionString"/> to use.</param>
	/// <param name="commandText">The SQL command to be executed.</param>
	/// <param name="commandParameter">A single <see cref="MySqlParameter"/> object to use with the command.</param>
	/// <returns>The number of rows affected.</returns>
	/// <remarks>For UPDATE, INSERT, and DELETE statements, the return value is the number of rows affected by the command.
	/// For stored procedures, the return value is the number of rows affected by the last statement in the stored procedure,
	/// or zero if the last statement is a SELECT. For all other types of statements, the return value is -1.</remarks>
	public static int ExecuteNonQuery(string connectionString, string commandText, MySqlParameter commandParameter) => ExecuteNonQuery(new MySqlConnection(connectionString), commandText, [commandParameter]);

	/// <summary>
	/// Executes this command on the associated <see cref="MySqlConnection"/>.
	/// </summary>
	/// <param name="mySqlConnection"><see cref="MySqlConnection"/></param>
	/// <param name="commandText">The SQL command to be executed.</param>
	/// <param name="commandParameters">An IEnumberable of <see cref="MySqlParameter"/> objects to use with the command.</param>/// <returns>The number of rows affected.</returns>
	/// <remarks>For UPDATE, INSERT, and DELETE statements, the return value is the number of rows affected by the command.
	/// For stored procedures, the return value is the number of rows affected by the last statement in the stored procedure,
	/// or zero if the last statement is a SELECT. For all other types of statements, the return value is -1.</remarks>
	public static int ExecuteNonQuery(MySqlConnection mySqlConnection, string commandText, IEnumerable<MySqlParameter> commandParameters = null)
	{
		using (mySqlConnection)
		{
			mySqlConnection.Open();
			using var command = new MySqlCommand();
			command.Connection = mySqlConnection;
			command.CommandText = commandText;
			if (commandParameters is not null)
			{
				command.Parameters.AddRange(commandParameters.ToArray());
			}

			return command.ExecuteNonQuery();
		}
	}
	#endregion

	#region Reader

	/// <summary>
	/// Executes a single command against a MySQL database.
	/// </summary>
	/// <param name="connectionString"><see cref="MySqlConnection.ConnectionString"/> to use.</param>
	/// <param name="commandText">The SQL command to be executed.</param>
	/// <param name="commandParameter">A single <see cref="MySqlParameter"/> object to use with the command</param>
	/// <returns><see cref="MySqlDataReader"/> object ready to read the results of the command</returns>
	public static IDataReader ExecuteReader(string connectionString, string commandText, MySqlParameter commandParameter) => ExecuteReader(connectionString, commandText, [commandParameter]);

	/// <summary>
	/// Executes a single command against a MySQL database.
	/// </summary>
	/// <param name="connectionString"><see cref="MySqlConnection.ConnectionString"/> to use.</param>
	/// <param name="commandText">The SQL command to be executed.</param>
	/// <param name="commandParameters">An IEnumberable of <see cref="MySqlParameter"/> objects to use with the command</param>
	/// <returns><see cref="MySqlDataReader"/> object ready to read the results of the command</returns>
	public static IDataReader ExecuteReader(string connectionString, string commandText, IEnumerable<MySqlParameter> commandParameters = null)
	{
		MySqlDataReader dr;
		var connection = new MySqlConnection(connectionString);

		connection.Open();

		var command = new MySqlCommand
		{
			Connection = connection,
			CommandText = commandText
		};

		if (commandParameters is not null)
		{
			command.Parameters.AddRange(commandParameters.ToArray());
		}

		dr = command.ExecuteReader(CommandBehavior.CloseConnection);
		return dr;
	}
	#endregion

	#region Scalar
	/// <summary>
	/// Execute a single command against a MySQL database.
	/// </summary>
	/// <param name="connectionString"><see cref="MySqlConnection.ConnectionString"/> to use.</param>
	/// <param name="commandText">The SQL command to be executed.</param>
	/// <param name="commandParameter">A single <see cref="MySqlParameter"/> object to use with the command</param>
	/// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
	public static object ExecuteScalar(string connectionString, string commandText, MySqlParameter commandParameter) => ExecuteScalar(connectionString, commandText, [commandParameter]);

	/// <summary>
	/// Execute a single command against a MySQL database.
	/// </summary>
	/// <param name="connectionString"><see cref="MySqlConnection.ConnectionString"/> to use.</param>
	/// <param name="commandText">The SQL command to be executed.</param>
	/// <param name="commandParameters">An IEnumerable of <see cref="MySqlParameter"/> objects to use with the command</param>
	/// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
	public static object ExecuteScalar(string connectionString, string commandText, IEnumerable<MySqlParameter> commandParameters = null)
	{
		using var connection = new MySqlConnection(connectionString);
		connection.Open();

		using var command = new MySqlCommand();
		command.Connection = connection;
		command.CommandText = commandText;
		if (commandParameters is not null)
		{
			command.Parameters.AddRange(commandParameters.ToArray());
		}
		return command.ExecuteScalar();
	}
	#endregion

	#region DataSet
	public static DataSet ExecuteDataset(string connectionString, string commandText, MySqlParameter commandParameter) => ExecuteDataset(connectionString, commandText, [commandParameter]);

	/// <summary>
	/// Execute a single command against a MySQL database.
	/// </summary>
	/// <param name="connectionString"><see cref="MySqlConnection.ConnectionString"/> to use.</param>
	/// <param name="commandText">The SQL command to be executed.</param>
	/// <param name="commandParameters">An IEnumerable of <see cref="MySqlParameter"/> objects to use with the command</param>
	/// <returns>A <see cref="System.Data.DataSet"/> object of the data returned by the command.</returns>
	public static DataSet ExecuteDataset(string connectionString, string commandText, IEnumerable<MySqlParameter> commandParameters = null)
	{
		using var connection = new MySqlConnection(connectionString);
		connection.Open();

		using var command = new MySqlCommand();
		command.Connection = connection;
		command.CommandText = commandText;
		command.CommandType = CommandType.Text;
		if (commandParameters is not null)
		{
			command.Parameters.AddRange(commandParameters.ToArray());
		}

		var mySqlDataAdapter = new MySqlDataAdapter(commandText, command.Connection);
		var dataSet = new DataSet();
		mySqlDataAdapter.Fill(dataSet);
		return dataSet;
	}
	#endregion
}