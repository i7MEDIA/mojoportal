// Author:					Joe Audette
// Created:					2011-11-02
// Last Modified:			2012-04-03
// 


using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;

namespace sts.SiteMembership.Data
{
    public static class DBMembershipReminder
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
        /// Inserts a row in the sts_MembershipReminder table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="productGuid"> productGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="templateGuid"> templateGuid </param>
        /// <param name="reminderType"> reminderType </param>
        /// <param name="targetDateType"> targetDateType </param>
        /// <param name="days"> days </param>
        /// <param name="enabled"> enabled </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdBy"> createdBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid productGuid,
            Guid siteGuid,
            Guid templateGuid,
            string reminderType,
            string targetDateType,
            int days,
            bool enabled,
            string productPageUrl,
            DateTime createdUtc,
            Guid createdBy)
        {
            #region Bit Conversion

            int intEnabled = 0;
            if (enabled) { intEnabled = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_MembershipReminder (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("ProductGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("TemplateGuid, ");
            sqlCommand.Append("ReminderType, ");
            sqlCommand.Append("TargetDateType, ");
            sqlCommand.Append("Days, ");
            sqlCommand.Append("Enabled, ");
            sqlCommand.Append("ProductPageUrl, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("LastModBy )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?ProductGuid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?TemplateGuid, ");
            sqlCommand.Append("?ReminderType, ");
            sqlCommand.Append("?TargetDateType, ");
            sqlCommand.Append("?Days, ");
            sqlCommand.Append("?Enabled, ");
            sqlCommand.Append("?ProductPageUrl, ");
            sqlCommand.Append("?CreatedUtc, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?LastModUtc, ");
            sqlCommand.Append("?LastModBy )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[13];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = productGuid.ToString();

            arParams[2] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new MySqlParameter("?TemplateGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = templateGuid.ToString();

            arParams[4] = new MySqlParameter("?ReminderType", MySqlDbType.VarChar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = reminderType;

            arParams[5] = new MySqlParameter("?TargetDateType", MySqlDbType.VarChar, 50);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = targetDateType;

            arParams[6] = new MySqlParameter("?Days", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = days;

            arParams[7] = new MySqlParameter("?Enabled", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intEnabled;

            arParams[8] = new MySqlParameter("?ProductPageUrl", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = productPageUrl;

            arParams[9] = new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = createdUtc;

            arParams[10] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = createdBy.ToString();

            arParams[11] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = createdUtc;

            arParams[12] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = createdBy.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the sts_MembershipReminder table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="templateGuid"> templateGuid </param>
        /// <param name="reminderType"> reminderType </param>
        /// <param name="targetDateType"> targetDateType </param>
        /// <param name="days"> days </param>
        /// <param name="enabled"> enabled </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            Guid templateGuid,
            string reminderType,
            string targetDateType,
            int days,
            bool enabled,
            string productPageUrl,
            DateTime lastModUtc,
            Guid lastModBy)
        {
            #region Bit Conversion

            int intEnabled = 0;
            if (enabled) { intEnabled = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_MembershipReminder ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("TemplateGuid = ?TemplateGuid, ");
            sqlCommand.Append("ReminderType = ?ReminderType, ");
            sqlCommand.Append("TargetDateType = ?TargetDateType, ");
            sqlCommand.Append("Days = ?Days, ");
            sqlCommand.Append("Enabled = ?Enabled, ");
            sqlCommand.Append("ProductPageUrl = ?ProductPageUrl, ");
            
            sqlCommand.Append("LastModUtc = ?LastModUtc, ");
            sqlCommand.Append("LastModBy = ?LastModBy ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[9];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?TemplateGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = templateGuid.ToString();

            arParams[2] = new MySqlParameter("?ReminderType", MySqlDbType.VarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = reminderType;

            arParams[3] = new MySqlParameter("?TargetDateType", MySqlDbType.VarChar, 50);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = targetDateType;

            arParams[4] = new MySqlParameter("?Days", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = days;

            arParams[5] = new MySqlParameter("?Enabled", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intEnabled;

            arParams[6] = new MySqlParameter("?ProductPageUrl", MySqlDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = productPageUrl;

            arParams[7] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = lastModUtc;

            arParams[8] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModBy.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the sts_MembershipReminder table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_MembershipReminder ");
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

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_MembershipReminder ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByProduct(Guid productGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_MembershipReminder ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ProductGuid = ?ProductGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = productGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the sts_MembershipReminder table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  mr.*, ");
            sqlCommand.Append("s.SiteID, ");
            sqlCommand.Append("et.Name AS TemplateName ");

            sqlCommand.Append("FROM	sts_MembershipReminder mr ");

            sqlCommand.Append("JOIN	mp_Sites s ");
            sqlCommand.Append("ON mr.SiteGuid = s.SiteGuid ");

            sqlCommand.Append("LEFT OUTER JOIN mp_EmailTemplate et ");
            sqlCommand.Append("ON mr.TemplateGuid = et.Guid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mr.Guid = ?Guid ");
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

        public static IDataReader GetByProduct(Guid productGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  mr.*, ");
            sqlCommand.Append("s.SiteID, ");
            sqlCommand.Append("et.Name AS TemplateName ");

            sqlCommand.Append("FROM	sts_MembershipReminder mr ");

            sqlCommand.Append("JOIN	mp_Sites s ");
            sqlCommand.Append("ON mr.SiteGuid = s.SiteGuid ");

            sqlCommand.Append("LEFT OUTER JOIN mp_EmailTemplate et ");
            sqlCommand.Append("ON mr.TemplateGuid = et.Guid ");


            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mr.ProductGuid = ?ProductGuid ");

            sqlCommand.Append("ORDER BY mr.Days, mr.ReminderType DESC ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = productGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetAllActive()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  mr.*, ");
            sqlCommand.Append("s.SiteID, ");
            sqlCommand.Append("et.Name AS TemplateName ");

            sqlCommand.Append("FROM	sts_MembershipReminder mr ");

            sqlCommand.Append("JOIN	mp_Sites s ");
            sqlCommand.Append("ON mr.SiteGuid = s.SiteGuid ");

            sqlCommand.Append("LEFT OUTER JOIN mp_EmailTemplate et ");
            sqlCommand.Append("ON mr.TemplateGuid = et.Guid ");


            sqlCommand.Append("WHERE ");
            sqlCommand.Append("mr.Enabled = 1 ");

            sqlCommand.Append(";");

          
            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                null);

        }

        public static int CountByTemplate(Guid templateGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_MembershipReminder ");
            sqlCommand.Append("WHERE TemplateGuid = ?TemplateGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?TemplateGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = templateGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Inserts a row in the sts_MembershipReminderLog table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="reminderGuid"> reminderGuid </param>
        /// <param name="ticketGuid"> ticketGuid </param>
        /// <param name="sentUtc"> sentUtc </param>
        /// <returns>int</returns>
        public static int CreateLog(
            Guid guid,
            Guid siteGuid,
            Guid reminderGuid,
            Guid ticketGuid,
            DateTime sentUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_MembershipReminderLog (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ReminderGuid, ");
            sqlCommand.Append("TicketGuid, ");
            sqlCommand.Append("SentUtc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?ReminderGuid, ");
            sqlCommand.Append("?TicketGuid, ");
            sqlCommand.Append("?SentUtc )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?ReminderGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = reminderGuid.ToString();

            arParams[3] = new MySqlParameter("?TicketGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = ticketGuid.ToString();

            arParams[4] = new MySqlParameter("?SentUtc", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = sentUtc;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }

        public static bool DeleteLog(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_MembershipReminderLog ");
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

        public static bool DeleteLogBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_MembershipReminderLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteLogByTicket(Guid ticketGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_MembershipReminderLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("TicketGuid = ?TicketGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?TicketGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = ticketGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteLogByReminder(Guid reminderGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_MembershipReminderLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ReminderGuid = ?ReminderGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ReminderGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = reminderGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

    }
}
