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
// Modifications and adaptation for Firebird Sql by Joe Audette
// Last Modified 2010-01-28

using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Isql;
using log4net;

namespace mojoPortal.Data
{
    public sealed class FBSqlHelper
    {
        private FBSqlHelper() { }

        private static readonly ILog log = LogManager.GetLogger(typeof(FBSqlHelper));

        public static String GetParamString(Int32 count)
        {
            if (count <= 1) { return count < 1 ? "" : "?"; }
            return "?," + GetParamString(count - 1);
        }

        private static void PrepareCommand(
            FbCommand command,
            FbConnection connection,
            FbTransaction transaction,
            CommandType commandType,
            string commandText,
            FbParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            command.Connection = connection;
            command.CommandText = commandText;
            command.CommandType = commandType;

            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        private static void AttachParameters(FbCommand command, FbParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (FbParameter p in commandParameters)
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

        public static bool ExecuteBatchScript(
            string connectionString,
            string pathToScriptFile)
        {
            FbScript script = new FbScript(pathToScriptFile);
            if (script.Parse() > 0)
            {
                using (FbConnection connection = new FbConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        FbBatchExecution batch = new FbBatchExecution(connection, script);
                        batch.Execute(true);


                    }
                    catch (FbException ex)
                    {
                        log.Error(ex);
                        throw new Exception(pathToScriptFile, ex);
                    }
                    finally
                    {
                        connection.Close();
                    }

                   
                }

            }

            return true;

        }

        public static int ExecuteNonQuery(string connectionString, string commandText, params FbParameter[] commandParameters)
        {
            return ExecuteNonQuery(connectionString, CommandType.Text, commandText, commandParameters);
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params FbParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) { throw new ArgumentNullException("connectionString"); }

            using (FbConnection connection = new FbConnection(connectionString))
            {
                connection.Open();
                using (FbTransaction transaction = connection.BeginTransaction())
                {
                    using (FbCommand cmd = new FbCommand())
                    {
                        PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        transaction.Commit();
                        return rowsAffected;
                    }
                }
            }
        }

        public static int ExecuteNonQuery(FbTransaction transaction, CommandType commandType, string commandText, params FbParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");

            FbCommand cmd = new FbCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
            int retval = cmd.ExecuteNonQuery();
            return retval;
        }

        public static FbDataReader ExecuteReader(string connectionString, string commandText, params FbParameter[] commandParameters)
        {
            return ExecuteReader(connectionString, CommandType.Text, commandText, commandParameters);
        }

        public static FbDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params FbParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            FbConnection connection = null;
            try
            {
                connection = new FbConnection(connectionString);
                connection.Open();

                FbCommand command = new FbCommand();

                PrepareCommand(
                    command,
                    connection,
                    null,
                    commandType,
                    commandText,
                    commandParameters);

                return command.ExecuteReader(CommandBehavior.CloseConnection);

            }
            catch
            {
                if ((connection != null) && (connection.State == ConnectionState.Open)) { connection.Close(); }
                throw;
            }
        }

        public static object ExecuteScalar(string connectionString, string commandText, params FbParameter[] commandParameters)
        {
            return ExecuteScalar(connectionString, CommandType.Text, commandText, commandParameters);
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params FbParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            using (FbConnection connection = new FbConnection(connectionString))
            {
                connection.Open();
                FbTransaction transaction = null;
                bool useTransaction = (commandText.Contains("EXECUTE") || commandText.Contains("INSERT"));
                if (useTransaction) { transaction = connection.BeginTransaction(); }

                using (FbCommand command = new FbCommand())
                {
                    PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters);
                    object result = command.ExecuteScalar();

                    if (transaction != null)
                    {
                        transaction.Commit();
                        transaction.Dispose();
                        transaction = null;

                    }

                    if (connection.State == ConnectionState.Open)
                        connection.Close();

                    return result;

                }
            }
        }

        public static DataSet ExecuteDataset(string connectionString, string commandText, params FbParameter[] commandParameters)
        {
            return ExecuteDataset(connectionString, CommandType.Text, commandText, commandParameters);
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params FbParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            using (FbConnection connection = new FbConnection(connectionString))
            {
                connection.Open();
                using (FbCommand command = new FbCommand())
                {
                    PrepareCommand(command, connection, (FbTransaction)null, commandType, commandText, commandParameters);

                    using (FbDataAdapter adapter = new FbDataAdapter(command))
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
