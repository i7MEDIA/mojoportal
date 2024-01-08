using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySqlConnector;

namespace mojoPortal.Data;

public static class CommandHelper
{
	public static int ExecuteNonQuery(string connString, string sqlCommand, IEnumerable<MySqlParameter> parameters = null)
	{
		return ExecuteNonQuery(new MySqlConnection(connString), sqlCommand, parameters);
	}

	public static int ExecuteNonQuery(string connString, string sqlCommand, MySqlParameter parameter)
	{
		return ExecuteNonQuery(new MySqlConnection(connString), sqlCommand, new List<MySqlParameter>() { parameter });
	}

	public static int ExecuteNonQuery(MySqlConnection mySqlConnection, string sqlCommand, IEnumerable<MySqlParameter> parameters = null)
	{
		using var command = new MySqlCommand();
		command.Connection = mySqlConnection;
		command.CommandText = sqlCommand;
		command.Parameters.AddRange(parameters.ToArray());
		return command.ExecuteNonQuery();
	}

	public static IDataReader ExecuteReader(string connString, string sqlCommand, MySqlParameter parameter)
	{
		return ExecuteReader(connString, sqlCommand, new List<MySqlParameter>() { parameter });
	}

	public static IDataReader ExecuteReader(string connString, string sqlCommand, IEnumerable<MySqlParameter> parameters = null)
	{
		using var command = new MySqlCommand();
		command.Connection = new(connString);
		command.CommandText = sqlCommand;
		command.Parameters.AddRange(parameters.ToArray());
		return command.ExecuteReader();
	}

	public static object ExecuteScalar(string connString, string sqlCommand, MySqlParameter parameter)
	{
		return ExecuteScalar(connString, sqlCommand, new List<MySqlParameter>() { parameter });
	}

	public static object ExecuteScalar(string connString, string sqlCommand, IEnumerable<MySqlParameter> parameters = null)
	{
		using var command = new MySqlCommand();
		command.Connection = new(connString);
		command.CommandText = sqlCommand;
		command.Parameters.AddRange(parameters.ToArray());
		return command.ExecuteScalar();
	}

	public static object ExecuteDataset(string connString, string sqlCommand, MySqlParameter parameter)
	{
		return ExecuteDataset(connString, sqlCommand, new List<MySqlParameter>() { parameter });
	}

	public static DataSet ExecuteDataset(string connString, string sqlCommand, IEnumerable<MySqlParameter> parameters = null)
	{
		using var command = new MySqlCommand();
		command.Connection = new(connString);
		command.CommandText = sqlCommand;
		command.CommandType = CommandType.Text;
		command.Parameters.AddRange(parameters.ToArray());

		var mySqlDataAdapter = new MySqlDataAdapter(sqlCommand, command.Connection);
		var dataSet = new DataSet();
		mySqlDataAdapter.Fill(dataSet);
		return dataSet;
	}
}
