/// Author:					Joe Audette
/// Created:				2008-09-26
/// Last Modified:			2013-09-04
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
    public static class DBWebFormOption
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
        /// Inserts a row in the sts_WebFormOption table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="questionGuid"> questionGuid </param>
        /// <param name="optionText"> optionText </param>
        /// <param name="sortOrder"> sortOrder </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid questionGuid,
            string optionText,
            string optionValue,
            string optionGroup,
            int sortOrder)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_WebFormOption (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("QuestionGuid, ");
            sqlCommand.Append("OptionText, ");
            sqlCommand.Append("OptionValue, ");
            sqlCommand.Append("OptionGroup, ");
            sqlCommand.Append("SortOrder )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?QuestionGuid, ");
            sqlCommand.Append("?OptionText, ");
            sqlCommand.Append("?OptionValue, ");
            sqlCommand.Append("?OptionGroup, ");
            sqlCommand.Append("?SortOrder )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?QuestionGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = questionGuid.ToString();

            arParams[2] = new MySqlParameter("?OptionText", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = optionText;

            arParams[3] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new MySqlParameter("?OptionValue", MySqlDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = optionValue;

            arParams[5] = new MySqlParameter("?OptionGroup", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = optionGroup;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the sts_WebFormOption table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="optionText"> optionText </param>
        /// <param name="sortOrder"> sortOrder </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            string optionText,
            string optionValue,
            string optionGroup,
            int sortOrder)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_WebFormOption ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("OptionText = ?OptionText, ");
            sqlCommand.Append("OptionValue = ?OptionValue, ");
            sqlCommand.Append("OptionGroup = ?OptionGroup, ");
            sqlCommand.Append("SortOrder = ?SortOrder ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?OptionText", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = optionText;

            arParams[2] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = sortOrder;

            arParams[3] = new MySqlParameter("?OptionValue", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = optionValue;

            arParams[4] = new MySqlParameter("?OptionGroup", MySqlDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = optionGroup;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the sts_WebFormOption table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_WebFormOption ");
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

        public static bool DeleteByForm(Guid formGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_WebFormOption ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionGuid IN (SELECT Guid FROM sts_WebFormQuestion WHERE FormGuid = ?FormGuid) ");
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
        /// Deletes a row from the sts_WebFormOption table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByQuestion(Guid questionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_WebFormOption ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionGuid = ?QuestionGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?QuestionGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the sts_WebFormOption table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_WebFormOption ");
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
        /// Gets an IDataReader with one row from the sts_WebFormOption table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByQuestion(Guid questionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_WebFormOption ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionGuid = ?QuestionGuid ");
            sqlCommand.Append("ORDER BY SortOrder ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?QuestionGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

    }
}
