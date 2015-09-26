/// Author:					Joe Audette
/// Created:				2008-09-26
/// Last Modified:			2010-01-20
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
    
    public static class DBWebForm
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
        /// Inserts a row in the sts_WebForm table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="moduleId"> moduleId </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="name"> name </param>
        /// <param name="title"> title </param>
        /// <param name="instructions"> instructions </param>
        /// <param name="completedMessage"> completedMessage </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="creatorId"> creatorId </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            int moduleId,
            Guid moduleGuid,
            Guid siteGuid,
            string name,
            string title,
            string instructions,
            string completedMessage,
            int totalPages,
            DateTime createdUtc,
            Guid creatorId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_WebForm (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("ModuleId, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Instructions, ");
            sqlCommand.Append("CompletedMessage, ");
            sqlCommand.Append("TotalPages, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("CreatorId, ");
            sqlCommand.Append("LastModifiedUtc, ");
            sqlCommand.Append("LastModifiedBy )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?ModuleId, ");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?Name, ");
            sqlCommand.Append("?Title, ");
            sqlCommand.Append("?Instructions, ");
            sqlCommand.Append("?CompletedMessage, ");
            sqlCommand.Append("?TotalPages, ");
            sqlCommand.Append("?CreatedUtc, ");
            sqlCommand.Append("?CreatorId, ");
            sqlCommand.Append("?LastModifiedUtc, ");
            sqlCommand.Append("?LastModifiedBy )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[13];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?ModuleId", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = siteGuid.ToString();

            arParams[4] = new MySqlParameter("?Name", MySqlDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = name;

            arParams[5] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = title;

            arParams[6] = new MySqlParameter("?Instructions", MySqlDbType.Text);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = instructions;

            arParams[7] = new MySqlParameter("?CompletedMessage", MySqlDbType.Text);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = completedMessage;

            arParams[8] = new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdUtc;

            arParams[9] = new MySqlParameter("?CreatorId", MySqlDbType.VarChar, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = creatorId.ToString();

            arParams[10] = new MySqlParameter("?LastModifiedUtc", MySqlDbType.DateTime);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = createdUtc;

            arParams[11] = new MySqlParameter("?LastModifiedBy", MySqlDbType.VarChar, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = creatorId.ToString();

            arParams[12] = new MySqlParameter("?TotalPages", MySqlDbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = totalPages;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the sts_WebForm table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="moduleId"> moduleId </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="name"> name </param>
        /// <param name="title"> title </param>
        /// <param name="instructions"> instructions </param>
        /// <param name="completedMessage"> completedMessage </param>
        /// <param name="lastModifiedUtc"> lastModifiedUtc </param>
        /// <param name="lastModifiedBy"> lastModifiedBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            string name,
            string title,
            string instructions,
            string completedMessage,
            int totalPages,
            DateTime lastModifiedUtc,
            Guid lastModifiedBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_WebForm ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Name = ?Name, ");
            sqlCommand.Append("Title = ?Title, ");
            sqlCommand.Append("Instructions = ?Instructions, ");
            sqlCommand.Append("CompletedMessage = ?CompletedMessage, ");
            sqlCommand.Append("TotalPages = ?TotalPages, ");
            sqlCommand.Append("LastModifiedUtc = ?LastModifiedUtc, ");
            sqlCommand.Append("LastModifiedBy = ?LastModifiedBy ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[8];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?Name", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = name;

            arParams[2] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new MySqlParameter("?Instructions", MySqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = instructions;

            arParams[4] = new MySqlParameter("?CompletedMessage", MySqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = completedMessage;

            arParams[5] = new MySqlParameter("?LastModifiedUtc", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = lastModifiedUtc;

            arParams[6] = new MySqlParameter("?LastModifiedBy", MySqlDbType.VarChar, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = lastModifiedBy.ToString();

            arParams[7] = new MySqlParameter("?TotalPages", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = totalPages;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the sts_WebForm table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_WebForm ");
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

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_WebFormResponse ");
            sqlCommand.Append("WHERE ResponseSetGuid IN (SELECT Guid ");
            sqlCommand.Append("FROM sts_WebFormResponseSet ");
            sqlCommand.Append("WHERE FormGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM sts_WebForm ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append(")); ");

            sqlCommand.Append("DELETE FROM sts_WebFormResponseSet ");
            sqlCommand.Append("WHERE FormGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM sts_WebForm ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM sts_WebFormOption ");
            sqlCommand.Append("WHERE QuestionGuid IN (SELECT Guid ");
            sqlCommand.Append("FROM sts_WebFormQuestion ");
            sqlCommand.Append("WHERE FormGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM sts_WebForm ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append(")); ");

            sqlCommand.Append("DELETE FROM sts_WebFormQuestion ");
            sqlCommand.Append("WHERE FormGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM sts_WebForm ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM sts_WebForm ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append("; ");
            

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the sts_WebForm table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_WebForm ");
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


    }
}
