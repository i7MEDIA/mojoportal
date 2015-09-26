// Author:					Joe Audette
// Created:				    2008-09-26
// Last Modified:			2014-05-21
// 
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using MySql.Data.MySqlClient;


namespace sts.FormWizard.Data
{
    public static class DBWebFormQuestion
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
        /// Inserts a row in the sts_WebFormQuestion table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="formGuid"> formGuid </param>
        /// <param name="questionTypeId"> questionTypeId </param>
        /// <param name="questionText"> questionText </param>
        /// <param name="isRequired"> isRequired </param>
        /// <param name="validationExpression"> validationExpression </param>
        /// <param name="invalidMessage"> invalidMessage </param>
        /// <param name="sortOrder"> sortOrder </param>
        /// <param name="pageNumber"> pageNumber </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid formGuid,
            int questionTypeId,
            string questionText,
            string questionAlias,
            bool isRequired,
            string validationExpression,
            string invalidMessage,
            int sortOrder,
            int pageNumber,
            int minRange,
            int maxRange,
            string cssClass)
        {
            #region Bit Conversion

            int intIsRequired = 0;
            if (isRequired)
            {
                intIsRequired = 1;
            }
            
            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_WebFormQuestion (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("FormGuid, ");
            sqlCommand.Append("QuestionTypeId, ");
            sqlCommand.Append("QuestionText, ");
            sqlCommand.Append("QuestionAlias, ");
            sqlCommand.Append("IsRequired, ");
            sqlCommand.Append("ValidationExpression, ");
            sqlCommand.Append("InvalidMessage, ");
            sqlCommand.Append("CssClass, ");
            sqlCommand.Append("SortOrder, ");
            sqlCommand.Append("PageNumber, ");
            sqlCommand.Append("MinRange, ");
            sqlCommand.Append("MaxRange )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?FormGuid, ");
            sqlCommand.Append("?QuestionTypeId, ");
            sqlCommand.Append("?QuestionText, ");
            sqlCommand.Append("?QuestionAlias, ");
            sqlCommand.Append("?IsRequired, ");
            sqlCommand.Append("?ValidationExpression, ");
            sqlCommand.Append("?InvalidMessage, ");
            sqlCommand.Append("?CssClass, ");
            sqlCommand.Append("?SortOrder, ");
            sqlCommand.Append("?PageNumber, ");
            sqlCommand.Append("?MinRange, ");
            sqlCommand.Append("?MaxRange )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[13];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?FormGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = formGuid.ToString();

            arParams[2] = new MySqlParameter("?QuestionTypeId", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = questionTypeId;

            arParams[3] = new MySqlParameter("?QuestionText", MySqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = questionText;

            arParams[4] = new MySqlParameter("?IsRequired", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intIsRequired;

            arParams[5] = new MySqlParameter("?ValidationExpression", MySqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = validationExpression;

            arParams[6] = new MySqlParameter("?InvalidMessage", MySqlDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = invalidMessage;

            arParams[7] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = sortOrder;

            arParams[8] = new MySqlParameter("?PageNumber", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = pageNumber;

            arParams[9] = new MySqlParameter("?MinRange", MySqlDbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = minRange;

            arParams[10] = new MySqlParameter("?MaxRange", MySqlDbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = maxRange;

            arParams[11] = new MySqlParameter("?CssClass", MySqlDbType.VarChar, 100);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = cssClass;

            arParams[12] = new MySqlParameter("?QuestionAlias", MySqlDbType.Text);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = questionAlias;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;



        }

        /// <summary>
        /// Updates a row in the sts_WebFormQuestion table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="questionTypeId"> questionTypeId </param>
        /// <param name="questionText"> questionText </param>
        /// <param name="isRequired"> isRequired </param>
        /// <param name="validationExpression"> validationExpression </param>
        /// <param name="invalidMessage"> invalidMessage </param>
        /// <param name="sortOrder"> sortOrder </param>
        /// <param name="pageNumber"> pageNumber </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            int questionTypeId,
            string questionText,
            string questionAlias,
            bool isRequired,
            string validationExpression,
            string invalidMessage,
            int sortOrder,
            int pageNumber,
            int minRange,
            int maxRange,
            string cssClass)
        {
            #region Bit Conversion

            int intIsRequired = 0;
            if (isRequired)
            {
                intIsRequired = 1;
            }
            
            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_WebFormQuestion ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("QuestionTypeId = ?QuestionTypeId, ");
            sqlCommand.Append("QuestionText = ?QuestionText, ");
            sqlCommand.Append("QuestionAlias = ?QuestionAlias, ");
            sqlCommand.Append("IsRequired = ?IsRequired, ");
            sqlCommand.Append("ValidationExpression = ?ValidationExpression, ");
            sqlCommand.Append("InvalidMessage = ?InvalidMessage, ");
            sqlCommand.Append("CssClass = ?CssClass, ");
            sqlCommand.Append("SortOrder = ?SortOrder, ");
            sqlCommand.Append("PageNumber = ?PageNumber, ");
            sqlCommand.Append("MinRange = ?MinRange, ");
            sqlCommand.Append("MaxRange = ?MaxRange ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[12];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?QuestionTypeId", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = questionTypeId;

            arParams[2] = new MySqlParameter("?QuestionText", MySqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = questionText;

            arParams[3] = new MySqlParameter("?IsRequired", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = intIsRequired;

            arParams[4] = new MySqlParameter("?ValidationExpression", MySqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = validationExpression;

            arParams[5] = new MySqlParameter("?InvalidMessage", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = invalidMessage;

            arParams[6] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = sortOrder;

            arParams[7] = new MySqlParameter("?PageNumber", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = pageNumber;

            arParams[8] = new MySqlParameter("?MinRange", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = minRange;

            arParams[9] = new MySqlParameter("?MaxRange", MySqlDbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = maxRange;

            arParams[10] = new MySqlParameter("?CssClass", MySqlDbType.VarChar, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = cssClass;

            arParams[11] = new MySqlParameter("?QuestionAlias", MySqlDbType.Text);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = questionAlias;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the sts_WebFormQuestion table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_WebFormQuestion ");
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
            sqlCommand.Append("DELETE FROM sts_WebFormQuestion ");
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
        /// Gets an IDataReader with one row from the sts_WebFormQuestion table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_WebFormQuestion ");
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
        /// Gets an IDataReader with rows from the sts_WebFormQuestion table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByForm(Guid formGuid, int pageNumber)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_WebFormQuestion ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FormGuid = ?FormGuid ");
            if (pageNumber > -1)
            {
                sqlCommand.Append("AND PageNumber = ?PageNumber ");
            }
            sqlCommand.Append("ORDER BY PageNumber, SortOrder ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?FormGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = formGuid.ToString();

            arParams[1] = new MySqlParameter("?PageNumber", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetMaxSortOrder(Guid formGuid, int pageNumber)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  COALESCE(MAX(SortOrder),0) ");
            sqlCommand.Append("FROM	sts_WebFormQuestion ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FormGuid = ?FormGuid ");
            sqlCommand.Append("AND PageNumber = ?PageNumber ");
            
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?FormGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = formGuid.ToString();

            arParams[1] = new MySqlParameter("?PageNumber", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

    }
}
