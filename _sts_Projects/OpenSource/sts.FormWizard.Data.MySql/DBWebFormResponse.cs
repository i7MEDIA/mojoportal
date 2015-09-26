/// Author:					Joe Audette
/// Created:				2008-09-26
/// Last Modified:			2011-10-20
/// 
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using MySql.Data.MySqlClient;


namespace sts.FormWizard.Data
{
    public static class DBWebFormResponse
    {
        private static String GetReadConnectionString()
        {
            return ConfigurationManager.AppSettings["MySqlConnectionString"];

        }

        private static String GetWriteConnectionString()
        {
            if (ConfigurationManager.AppSettings["MySqlWriteConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["MySqlWriteConnectionString"];
            }

            return ConfigurationManager.AppSettings["MySqlConnectionString"];
        }

        /// <summary>
        /// Inserts a row in the sts_WebFormResponse table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="responseSetGuid"> responseSetGuid </param>
        /// <param name="questionGuid"> questionGuid </param>
        /// <param name="response"> response </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid responseSetGuid,
            Guid questionGuid,
            string response)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_WebFormResponse (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("ResponseSetGuid, ");
            sqlCommand.Append("QuestionGuid, ");
            sqlCommand.Append("Response )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?ResponseSetGuid, ");
            sqlCommand.Append("?QuestionGuid, ");
            sqlCommand.Append("?Response )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?ResponseSetGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = responseSetGuid.ToString();

            arParams[2] = new MySqlParameter("?QuestionGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = questionGuid.ToString();

            arParams[3] = new MySqlParameter("?Response", MySqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = response;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the sts_WebFormResponse table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="response"> response </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            string response)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_WebFormResponse ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Response = ?Response ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?Response", MySqlDbType.Text);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = response;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the sts_WebFormResponse table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_WebFormResponse ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByResponseSet(Guid responseSetGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_WebFormResponse ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseSetGuid = ?ResponseSetGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ResponseSetGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseSetGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the sts_WebFormResponse table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByForm(Guid formGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_WebFormResponse ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseSetGuid ");
            sqlCommand.Append("IN ");
            sqlCommand.Append("( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM sts_WebFormResponseSet ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FormGuid = ?FormGuid ");
            sqlCommand.Append(") ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?FormGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = formGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        //this has a syntax error
        //public static bool DeleteOrphansByForm(Guid formGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM sts_WebFormResponse ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("ResponseSetGuid ");
        //    sqlCommand.Append("IN ( SELECT Guid FROM sts_WebFormResponseSet WHERE FormGuid = ?FormGuid) ");
        //    sqlCommand.Append("AND ");

        //    sqlCommand.Append("ResponseSetGuid ");
        //    sqlCommand.Append("NOT IN ");
        //    sqlCommand.Append("( ");
        //    sqlCommand.Append("SELECT wfr.ResponseSetGuid ");
        //    sqlCommand.Append("FROM sts_WebFormResponse wfr ");
        //    sqlCommand.Append("JOIN ");
        //    sqlCommand.Append("sts_WebFormQuestion wfq ");
        //    sqlCommand.Append("ON ");
        //    sqlCommand.Append("wfr.QuestionGuid = wfq.Guid ");
        //    sqlCommand.Append("AND ");
        //    sqlCommand.Append("wfq.FormGuid = ?FormGuid ");
        //    sqlCommand.Append(") ");
        //    sqlCommand.Append(";");

        //    MySqlParameter[] arParams = new MySqlParameter[1];

        //    arParams[0] = new MySqlParameter("?FormGuid", MySqlDbType.VarChar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = formGuid.ToString();

        //    int rowsAffected = MySqlHelper.ExecuteNonQuery(
        //        GetWriteConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > 0);
        //}

        /// <summary>
        /// Deletes rows from the sts_WebFormResponse table that do not correspond to current form questions, ie the questiosn were deletedfrom the form. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrphansByForm(Guid formGuid)
        {
            DataTable dataTable = GetOrphansByFormTable(formGuid);
            bool result = false;

            foreach (DataRow row in dataTable.Rows)
            {
                string guidString = row["ResponseSetGuid"].ToString();
                if (guidString.Length == 36)
                {
                    Guid responseSetGuid = new Guid(guidString);
                    result = DeleteByResponseSet(responseSetGuid);

                }

            }

            return result;

        }

        private static DataTable GetOrphansByFormTable(Guid formGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ResponseSetGuid FROM sts_WebFormResponse ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseSetGuid ");
            sqlCommand.Append("IN ( SELECT Guid FROM sts_WebFormResponseSet WHERE FormGuid = ?FormGuid) ");
            sqlCommand.Append("AND ");

            sqlCommand.Append("ResponseSetGuid ");
            sqlCommand.Append("NOT IN ");
            sqlCommand.Append("( ");
            sqlCommand.Append("SELECT wfr.ResponseSetGuid ");
            sqlCommand.Append("FROM sts_WebFormResponse wfr ");
            sqlCommand.Append("JOIN ");
            sqlCommand.Append("sts_WebFormQuestion wfq ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("wfr.QuestionGuid = wfq.Guid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("wfq.FormGuid = ?FormGuid ");
            sqlCommand.Append(") ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?FormGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = formGuid.ToString();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ResponseSetGuid", typeof(string));

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["ResponseSetGuid"] = reader["ResponseSetGuid"];
                    dataTable.Rows.Add(row);

                }

            }

            return dataTable;
            
        }

        /// <summary>
        /// Gets an IDataReader with one row from the sts_WebFormResponse table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  wfr.*, ");
            sqlCommand.Append("rs.UserGuid, ");
            sqlCommand.Append("rs.CreatedUtc ");

            sqlCommand.Append("FROM	sts_WebFormResponse wfr ");
            sqlCommand.Append("JOIN ");
            sqlCommand.Append("sts_WebFormResponseSet rs ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("rs.Guid = wfr.ResponseSetGuid ");
            
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("wfr.Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the sts_WebFormResponse table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByResponseSet(Guid responseSetGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  wfr.*, ");
            sqlCommand.Append("rs.UserGuid, ");
            sqlCommand.Append("rs.CreatedUtc ");

            sqlCommand.Append("FROM	sts_WebFormResponse wfr ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("sts_WebFormQuestion wfq ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("wfr.QuestionGuid = wfq.Guid ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("sts_WebFormResponseSet rs ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("rs.Guid = wfr.ResponseSetGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("wfr.ResponseSetGuid = ?ResponseSetGuid ");
            sqlCommand.Append("ORDER BY wfq.SortOrder ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ResponseSetGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseSetGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the sts_WebFormResponse table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByForm(Guid formGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("wfq.QuestionAlias, ");
            sqlCommand.Append("wfr.*, ");
            sqlCommand.Append("rs.UserGuid, ");
            sqlCommand.Append("rs.CreatedUtc ");

            sqlCommand.Append("FROM	sts_WebFormResponse wfr ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("sts_WebFormQuestion wfq ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("wfr.QuestionGuid = wfq.Guid ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("sts_WebFormResponseSet rs ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("rs.Guid = wfr.ResponseSetGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("wfq.FormGuid = ?FormGuid ");
            sqlCommand.Append("ORDER BY rs.CreatedUtc, wfr.ResponseSetGuid, wfq.SortOrder ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?FormGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = formGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with rows from the sts_WebFormResponse table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByFormForWord(Guid formGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("wfq.QuestionAlias, ");
            sqlCommand.Append("wfr.Response, ");
            sqlCommand.Append("rs.CreatedUtc ");

            sqlCommand.Append("FROM	sts_WebFormResponse wfr ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("sts_WebFormQuestion wfq ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("wfr.QuestionGuid = wfq.Guid ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("sts_WebFormResponseSet rs ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("rs.Guid = wfr.ResponseSetGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("wfq.FormGuid = ?FormGuid ");
            sqlCommand.Append("ORDER BY rs.CreatedUtc, wfr.ResponseSetGuid, wfq.SortOrder ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?FormGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = formGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets an IDataReader with row from the sts_WebFormResponse table that do not corespond to existing questions on the form, ie deleted questions.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOrphansByForm(Guid formGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  wfr.*, ");
            sqlCommand.Append("rs.UserGuid, ");
            sqlCommand.Append("rs.CreatedUtc ");

            sqlCommand.Append("FROM	sts_WebFormResponse wfr ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("sts_WebFormResponseSet rs ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("rs.Guid = wfr.ResponseSetGuid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("sts_WebFormQuestion wfq ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("wfr.QuestionGuid = wfq.Guid ");

            
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("rs.FormGuid = ?FormGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("wfq.Guid IS NULL ");

            sqlCommand.Append("ORDER BY wfr.ResponseSetGuid ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?FormGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = formGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


    }
}
