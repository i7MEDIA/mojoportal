/// Author:					Joe Audette
/// Created:				2008-09-26
/// Last Modified:			2012-06-25
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
    public static class DBWebFormResponseSet
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
        /// Inserts a row in the sts_WebFormResponseSet table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="formGuid"> formGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdFromIP"> createdFromIP </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid formGuid,
            Guid userGuid,
            DateTime createdUtc,
            string createdFromIP,
            string emailTo,
            string emailToAlias,
            int uploadCount)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_WebFormResponseSet (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("FormGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("EmailTo, ");
            sqlCommand.Append("EmailToAlias, ");
            sqlCommand.Append("UploadCount, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("CreatedFromIP )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?FormGuid, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?EmailTo, ");
            sqlCommand.Append("?EmailToAlias, ");
            sqlCommand.Append("?UploadCount, ");
            sqlCommand.Append("?CreatedUtc, ");
            sqlCommand.Append("?CreatedFromIP )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[8];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?FormGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = formGuid.ToString();

            arParams[2] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid.ToString();

            arParams[3] = new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = createdUtc;

            arParams[4] = new MySqlParameter("?CreatedFromIP", MySqlDbType.VarChar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = createdFromIP;

            arParams[5] = new MySqlParameter("?EmailTo", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = emailTo;

            arParams[6] = new MySqlParameter("?UploadCount", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = uploadCount;

            arParams[7] = new MySqlParameter("?EmailToAlias", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = emailToAlias;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the sts_WebFormResponseSet table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="formGuid"> formGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdFromIP"> createdFromIP </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            Guid userGuid,
            string emailTo,
            string emailToAlias,
            int uploadCount)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_WebFormResponseSet ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("UserGuid = ?UserGuid, ");
            sqlCommand.Append("EmailTo = ?EmailTo, ");
            sqlCommand.Append("EmailToAlias = ?EmailToAlias, ");
            sqlCommand.Append("UploadCount = ?UploadCount ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new MySqlParameter("?EmailTo", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = emailTo;

            arParams[3] = new MySqlParameter("?UploadCount", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = uploadCount;

            arParams[4] = new MySqlParameter("?EmailToAlias", MySqlDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = emailToAlias;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the sts_WebFormResponseSet table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_WebFormResponseSet ");
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

        /// <summary>
        /// Deletes a row from the sts_WebFormResponseSet table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByForm(Guid formGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_WebFormResponseSet ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FormGuid = ?FormGuid ");
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

        /// <summary>
        /// Deletes rows from the sts_WebFormResponseSet table that have no responses for cuurnet form questions, ie the questions were deleted after form submission. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteOrphansByForm(Guid formGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_WebFormResponseSet ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FormGuid = ?FormGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("Guid NOT IN (SELECT wfr.ResponseSetGuid FROM sts_WebFormResponse wfr ");
            sqlCommand.Append("JOIN sts_WebFormQuestion wfq ");
            sqlCommand.Append("ON wfr.QuestionGuid = wfq.Guid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("wfq.FormGuid = ?FormGuid) ");
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

        /// <summary>
        /// Gets an IDataReader with one row from the sts_WebFormResponseSet table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_WebFormResponseSet ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
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
        /// Gets an IDataReader with one row from the sts_WebFormResponseSet table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByForm(Guid formGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_WebFormResponseSet ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FormGuid = ?FormGuid ");
            sqlCommand.Append("ORDER BY CreatedUtc ");
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
        /// Gets a count of rows in the sts_WebFormResponseSet table.
        /// </summary>
        public static int GetCount(Guid formGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_WebFormResponseSet ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FormGuid = ?FormGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("Guid IN (SELECT wfr.ResponseSetGuid FROM sts_WebFormResponse wfr ");
            sqlCommand.Append("JOIN sts_WebFormQuestion wfq ");
            sqlCommand.Append("ON wfr.QuestionGuid = wfq.Guid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("wfq.FormGuid = ?FormGuid) ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?FormGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = formGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets a count of rows in the sts_WebFormResponseSet table that are not attached to any current form questions.
        /// </summary>
        public static int GetOrphanCount(Guid formGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_WebFormResponseSet ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FormGuid = ?FormGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("Guid NOT IN (SELECT wfr.ResponseSetGuid FROM sts_WebFormResponse wfr ");
            sqlCommand.Append("JOIN sts_WebFormQuestion wfq ");
            sqlCommand.Append("ON wfr.QuestionGuid = wfq.Guid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("wfq.FormGuid = ?FormGuid) ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?FormGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = formGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets a page of data from the sts_WebFormResponseSet table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid formGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(formGuid);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	sts_WebFormResponseSet  ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("FormGuid = ?FormGuid ");
            sqlCommand.Append("ORDER BY CreatedUtc ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?FormGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = formGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }


    }
}
