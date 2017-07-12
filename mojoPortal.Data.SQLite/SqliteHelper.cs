//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// 
// original copyright above
// Forked From Enterprise Library licensed under Ms-Pl http://www.codeplex.com/entlib
// but implementing a sub set of the API from the 2.0 Application Blocks SqlHelper
// using implementation from the newer Ms-Pl version
// Modifications and adaptation for SQLite by 
// Last Modified 2010-01-28

using System;
using System.Data;
using Mono.Data.Sqlite;


namespace mojoPortal.Data 
{
	/// <summary>
	/// Helper class that makes it easier to work with the provider.
	/// </summary>
	public sealed class SqliteHelper
	{
		private SqliteHelper(){}

        public static int ExecuteNonQuery(IDbConnection connection, string commandText, params IDataParameter[] parameters)
		{
            using (SqliteCommand cmd = new SqliteCommand())
            {
                cmd.Connection = (SqliteConnection)connection;
                cmd.CommandText = commandText;
                cmd.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    foreach (IDataParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                }

                return cmd.ExecuteNonQuery();
            }
		}

		public static int ExecuteNonQuery(string connectionString, string commandText, params IDataParameter[] parameters )
		{
			using (SqliteConnection connection = new SqliteConnection(connectionString))
			{
                connection.Open();
                int returnVal = ExecuteNonQuery(connection, commandText, parameters);
                connection.Close();
                return returnVal;
			}
		}

        public static IDataReader ExecuteReader(string connectionString, string commandText, params IDataParameter[] commandParameters)
        {
            SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            try
            {
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = (SqliteConnection)connection;
                    command.CommandText = commandText;
                    command.CommandType = CommandType.Text;

                    if (commandParameters != null)
                    {
                        foreach (IDataParameter p in commandParameters)
                        {
                            command.Parameters.Add(p);
                        }
                    }

                    return command.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
                throw;
            }
        }
        
        public static object ExecuteScalar(string connectionString, string commandText, params IDataParameter[] commandParameters)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = commandText;
                    command.CommandType = CommandType.Text;

                    if (commandParameters != null)
                    {
                        foreach (IDataParameter p in commandParameters)
                        {
                            command.Parameters.Add(p);
                        }
                    }

                    return command.ExecuteScalar();
                }
            }
        }

		public static DataSet ExecuteDataset(string connectionString, string commandText, params IDataParameter[] commandParameters)
		{
            using (SqliteConnection connection = new SqliteConnection(connectionString))
			{
                connection.Open();
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = commandText;
                    command.CommandType = CommandType.Text;

                    if (commandParameters != null)
                    {
                        foreach (IDataParameter p in commandParameters)
                        {
                            command.Parameters.Add(p);
                        }
                    }

                    SqliteDataAdapter adapter = new SqliteDataAdapter(command);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    if (dataSet.Tables.Count == 0)
                    {
                        adapter.FillSchema(dataSet, SchemaType.Source);
                    }

                    return dataSet;
                }
			}
		}

	}
}
