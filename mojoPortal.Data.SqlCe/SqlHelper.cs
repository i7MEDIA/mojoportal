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
// Forked From Enterprise Library licensed under Ms-Pl http://www.codeplex.com/entlib
// but implementing a sub set of the API from the 2.0 Application Blocks SqlHelper
// using implementation from the newer Ms-Pl version
// Modifications by Joe Audette
// Adapted for SqlCe 2010-08-09 by Joe Audette
// Last Modified 2010-08-09
// 2010-07-07


using System;
using System.Data;
using System.Data.Common;
using System.Xml;
using System.Data.SqlServerCe;
using System.Collections;


namespace mojoPortal.Data
{
    public static class SqlHelper
    {
        

        private static void PrepareCommand(
            SqlCeCommand command,
            SqlCeConnection connection,
            SqlCeTransaction transaction,
            CommandType commandType,
            string commandText,
            SqlCeParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");

            command.CommandType = commandType;
            command.CommandText = commandText;
            command.Connection = connection;

            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            if (commandParameters != null) { AttachParameters(command, commandParameters); }
        }

        private static void AttachParameters(SqlCeCommand command, SqlCeParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlCeParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand command = new SqlCeCommand())
                {
                    PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
                    return command.ExecuteNonQuery();
                }
            }
        }

        public static object DoInsertGetIdentitiy(string connectionString, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                int rowsAffected = 0;
                using (SqlCeCommand command = new SqlCeCommand())
                {
                    PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
                    rowsAffected = command.ExecuteNonQuery();
                }
                if (rowsAffected == 0) { return -1; }
                using (SqlCeCommand command = new SqlCeCommand())
                {
                    PrepareCommand(command, connection, (SqlCeTransaction)null, CommandType.Text, "SELECT @@IDENTITY", null);
                    return command.ExecuteScalar();
                }
            }
        }

        public static int ExecuteNonQuery(
            SqlCeTransaction transaction,
            CommandType commandType,
            string commandText,
            params SqlCeParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            using (SqlCeCommand command = new SqlCeCommand())
            {
                PrepareCommand(
                    command,
                    (SqlCeConnection)transaction.Connection,
                    transaction,
                    CommandType.Text,
                    commandText,
                    commandParameters);

                return command.ExecuteNonQuery();
            }
        }

        public static SqlCeDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            SqlCeConnection connection = null;
            try
            {
                connection = new SqlCeConnection(connectionString);
                try
                {
                    connection.Open();
                }
                catch (SqlCeInvalidDatabaseFormatException)
                {
                    using (SqlCeEngine engine = new SqlCeEngine())
                    {
                        engine.Upgrade(connectionString);
                    }

                    connection = new SqlCeConnection(connectionString);
                    connection.Open();

                }

                using (SqlCeCommand command = new SqlCeCommand())
                {
                    PrepareCommand(
                        command,
                        connection,
                        null,
                        commandType,
                        commandText,
                        commandParameters);

                    return command.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch
            {
                if ((connection != null) && (connection.State == ConnectionState.Open)) { connection.Close(); }
                throw;
            }
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                
                using (SqlCeCommand command = new SqlCeCommand())
                {
                    PrepareCommand(command, connection, (SqlCeTransaction)null, commandType, commandText, commandParameters);
                    return command.ExecuteScalar();
                }
            }
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connectionString, commandType, commandText, (SqlCeParameter[])null);
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlCeParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                using (SqlCeCommand command = new SqlCeCommand())
                {
                    PrepareCommand(command, connection, (SqlCeTransaction)null, commandType, commandText, commandParameters);
                    using (SqlCeDataAdapter adpater = new SqlCeDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();
                        adpater.Fill(dataSet);
                        return dataSet;
                    }
                }
            }
        }
    }
}
