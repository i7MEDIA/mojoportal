// Author:					Joe Audette
// Created:					2011-11-02
// Last Modified:			2014-01-28
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

    public static class DBMembershipProduct
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
        /// Inserts a row in the sts_MembershipProduct table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="memberDefGuid"> memberDefGuid </param>
        /// <param name="title"> title </param>
        /// <param name="description"> description </param>
        /// <param name="durationInDays"> durationInDays </param>
        /// <param name="price"> price </param>
        /// <param name="isAvailable"> isAvailable </param>
        /// <param name="isTaxable"> isTaxable </param>
        /// <param name="sortRank"> sortRank </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid memberDefGuid,
            string title,
            string description,
            int durationInDays,
            int gracePeriodDays,
            DateTime offerBeginUtc,
            DateTime offerEndUtc,
            decimal price,
            bool isAvailable,
            bool isTaxable,
            bool newMemberOnly,
            int sortRank,
            DateTime createdUtc,
            Guid createdBy,
            Guid siteGuid,
            decimal renewPrice,
            bool renewAnyDef,
            bool renewExpired)
        {

            #region Bit Conversion

            int intIsAvailable = 0;
            if (isAvailable) { intIsAvailable = 1; }
            int intIsTaxable = 0;
            if (isTaxable) { intIsTaxable = 1; }
            int intNewMemberOnly = 0;
            if (newMemberOnly) { intNewMemberOnly = 1; }

            int intrenewAnyDef = 0;
            if (renewAnyDef) { intrenewAnyDef = 1; }
            int intrenewExpired = 0;
            if (renewExpired) { intrenewExpired = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_MembershipProduct (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("MemberDefGuid, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("DurationInDays, ");
            sqlCommand.Append("GracePeriodDays, ");
            sqlCommand.Append("OfferBeginUtc, ");
            sqlCommand.Append("OfferEndUtc, ");
            sqlCommand.Append("Price, ");
            sqlCommand.Append("IsAvailable, ");
            sqlCommand.Append("IsTaxable, ");
            sqlCommand.Append("NewMemberOnly, ");
            sqlCommand.Append("QtySold, ");
            sqlCommand.Append("SortRank, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("ModifiedUtc, ");
            sqlCommand.Append("ModifiedBy, ");
            sqlCommand.Append("SiteGuid, ");

            sqlCommand.Append("RenewPrice, ");
            sqlCommand.Append("RenewAnyDef, ");
            sqlCommand.Append("RenewExpired ");

            sqlCommand.Append(") ");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?MemberDefGuid, ");
            sqlCommand.Append("?Title, ");
            sqlCommand.Append("?Description, ");
            sqlCommand.Append("?DurationInDays, ");
            sqlCommand.Append("?GracePeriodDays, ");
            sqlCommand.Append("?OfferBeginUtc, ");
            sqlCommand.Append("?OfferEndUtc, ");
            sqlCommand.Append("?Price, ");
            sqlCommand.Append("?IsAvailable, ");
            sqlCommand.Append("?IsTaxable, ");
            sqlCommand.Append("?NewMemberOnly, ");
            sqlCommand.Append("?QtySold, ");
            sqlCommand.Append("?SortRank, ");
            sqlCommand.Append("?CreatedUtc, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?ModifiedUtc, ");
            sqlCommand.Append("?ModifiedBy, ");
            sqlCommand.Append("?SiteGuid, ");

            sqlCommand.Append("?RenewPrice, ");
            sqlCommand.Append("?RenewAnyDef, ");
            sqlCommand.Append("?RenewExpired ");

            sqlCommand.Append(") ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[22];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?MemberDefGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = memberDefGuid.ToString();

            arParams[2] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new MySqlParameter("?DurationInDays", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = durationInDays;

            arParams[5] = new MySqlParameter("?GracePeriodDays", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = gracePeriodDays;

            arParams[6] = new MySqlParameter("?OfferBeginUtc", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = offerBeginUtc;

            arParams[7] = new MySqlParameter("?OfferEndUtc", MySqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            if (offerEndUtc > DateTime.MinValue)
            {
                arParams[7].Value = offerEndUtc;
            }
            else
            {
                arParams[7].Value = DBNull.Value;
            }

            arParams[8] = new MySqlParameter("?Price", MySqlDbType.Decimal);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = price;

            arParams[9] = new MySqlParameter("?IsAvailable", MySqlDbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = intIsAvailable;

            arParams[10] = new MySqlParameter("?IsTaxable", MySqlDbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = intIsTaxable;

            arParams[11] = new MySqlParameter("?NewMemberOnly", MySqlDbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = intNewMemberOnly;

            arParams[12] = new MySqlParameter("?QtySold", MySqlDbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = 0;

            arParams[13] = new MySqlParameter("?SortRank", MySqlDbType.Int32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = sortRank;

            arParams[14] = new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = createdUtc;

            arParams[15] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = createdBy.ToString();

            arParams[16] = new MySqlParameter("?ModifiedUtc", MySqlDbType.DateTime);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = createdUtc;

            arParams[17] = new MySqlParameter("?ModifiedBy", MySqlDbType.VarChar, 36);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = createdBy.ToString();

            arParams[18] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = siteGuid.ToString();

            arParams[19] = new MySqlParameter("?RenewPrice", MySqlDbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = renewPrice;

            arParams[20] = new MySqlParameter("?RenewAnyDef", MySqlDbType.Int32);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = intrenewAnyDef;

            arParams[21] = new MySqlParameter("?RenewExpired", MySqlDbType.Int32);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = intrenewExpired;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the sts_MembershipProduct table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="memberDefGuid"> memberDefGuid </param>
        /// <param name="title"> title </param>
        /// <param name="description"> description </param>
        /// <param name="durationInDays"> durationInDays </param>
        /// <param name="price"> price </param>
        /// <param name="isAvailable"> isAvailable </param>
        /// <param name="isTaxable"> isTaxable </param>
        /// <param name="sortRank"> sortRank </param>
        /// <param name="modifiedUtc"> modifiedUtc </param>
        /// <param name="modifiedBy"> modifiedBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            Guid memberDefGuid,
            string title,
            string description,
            int durationInDays,
            int gracePeriodDays,
            DateTime offerBeginUtc,
            DateTime offerEndUtc,
            decimal price,
            bool isAvailable,
            bool isTaxable,
             bool newMemberOnly,
            int sortRank,
            DateTime modifiedUtc,
            Guid modifiedBy,
            decimal renewPrice,
            bool renewAnyDef,
            bool renewExpired)
        {
            #region Bit Conversion

            int intIsAvailable = 0;
            if (isAvailable) { intIsAvailable = 1; }
            int intIsTaxable = 0;
            if (isTaxable) { intIsTaxable = 1; }
            int intNewMemberOnly = 0;
            if (newMemberOnly) { intNewMemberOnly = 1; }
            int intrenewAnyDef = 0;
            if (renewAnyDef) { intrenewAnyDef = 1; }
            int intrenewExpired = 0;
            if (renewExpired) { intrenewExpired = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_MembershipProduct ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("MemberDefGuid = ?MemberDefGuid, ");
            sqlCommand.Append("Title = ?Title, ");
            sqlCommand.Append("Description = ?Description, ");
            sqlCommand.Append("DurationInDays = ?DurationInDays, ");
            sqlCommand.Append("GracePeriodDays = ?GracePeriodDays, ");
            sqlCommand.Append("OfferBeginUtc = ?OfferBeginUtc, ");
            sqlCommand.Append("OfferEndUtc = ?OfferEndUtc, ");
            sqlCommand.Append("Price = ?Price, ");
            sqlCommand.Append("IsAvailable = ?IsAvailable, ");
            sqlCommand.Append("IsTaxable = ?IsTaxable, ");
            sqlCommand.Append("NewMemberOnly = ?NewMemberOnly, ");
           
            sqlCommand.Append("SortRank = ?SortRank, ");
            
            sqlCommand.Append("ModifiedUtc = ?ModifiedUtc, ");
            sqlCommand.Append("ModifiedBy = ?ModifiedBy ");
            

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[18];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?MemberDefGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = memberDefGuid.ToString();

            arParams[2] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new MySqlParameter("?DurationInDays", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = durationInDays;

            arParams[5] = new MySqlParameter("?GracePeriodDays", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = gracePeriodDays;

            arParams[6] = new MySqlParameter("?OfferBeginUtc", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = offerBeginUtc;

            arParams[7] = new MySqlParameter("?OfferEndUtc", MySqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            if (offerEndUtc > DateTime.MinValue)
            {
                arParams[7].Value = offerEndUtc;
            }
            else
            {
                arParams[7].Value = DBNull.Value;
            }
            

            arParams[8] = new MySqlParameter("?Price", MySqlDbType.Decimal);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = price;

            arParams[9] = new MySqlParameter("?IsAvailable", MySqlDbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = intIsAvailable;

            arParams[10] = new MySqlParameter("?IsTaxable", MySqlDbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = intIsTaxable;

            arParams[11] = new MySqlParameter("?NewMemberOnly", MySqlDbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = intNewMemberOnly;

            arParams[12] = new MySqlParameter("?SortRank", MySqlDbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = sortRank;

            arParams[13] = new MySqlParameter("?ModifiedUtc", MySqlDbType.DateTime);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = modifiedUtc;

            arParams[14] = new MySqlParameter("?ModifiedBy", MySqlDbType.VarChar, 36);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = modifiedBy.ToString();

            arParams[15] = new MySqlParameter("?RenewPrice", MySqlDbType.Decimal);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = renewPrice;

            arParams[16] = new MySqlParameter("?RenewAnyDef", MySqlDbType.Int32);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = intrenewAnyDef;

            arParams[17] = new MySqlParameter("?RenewExpired", MySqlDbType.Int32);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = intrenewExpired;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the sts_MembershipProduct table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_MembershipProduct ");
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
        /// Deletes rows from the sts_MembershipProduct table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByMemberDef(Guid memberDefGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_MembershipProduct ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("MemberDefGuid = ?MemberDefGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?MemberDefGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = memberDefGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the sts_MembershipProduct table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_MembershipProduct ");
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

        /// <summary>
        /// Gets an IDataReader with one row from the sts_MembershipProduct table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  smp.*, ");
            sqlCommand.Append("smd.GrantedRoles ");
            sqlCommand.Append("FROM	sts_MembershipProduct smp ");
            sqlCommand.Append("LEFT OUTER JOIN	sts_MemberDefinition smd ");
            sqlCommand.Append("ON smp.MemberDefGuid = smd.Guid ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("smp.Guid = ?Guid ");
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
        /// Gets an IDataReader with rows from the sts_MembershipProduct table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetAll(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  smp.*, ");
            sqlCommand.Append("smd.GrantedRoles ");
            sqlCommand.Append("FROM	sts_MembershipProduct smp ");
            sqlCommand.Append("LEFT OUTER JOIN	sts_MemberDefinition smd ");
            sqlCommand.Append("ON smp.MemberDefGuid = smd.Guid ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("smp.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("smp.Title ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetAvailable(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("smp.Guid, ");
            sqlCommand.Append("smp.MemberDefGuid, ");
            sqlCommand.Append("smp.Title, ");
            sqlCommand.Append("smp.Description AS OfferDescription, ");
            sqlCommand.Append("smp.DurationInDays, ");
            sqlCommand.Append("smp.GracePeriodDays, ");
            sqlCommand.Append("smp.OfferBeginUtc, ");
            sqlCommand.Append("smp.OfferEndUtc, ");
            sqlCommand.Append("smp.Price, ");
            sqlCommand.Append("smp.RenewPrice, ");
            sqlCommand.Append("smp.IsTaxable, ");
            sqlCommand.Append("smp.NewMemberOnly, ");
            sqlCommand.Append("smp.RenewAnyDef, ");
            sqlCommand.Append("smp.RenewExpired, ");

            sqlCommand.Append("smd.GrantedRoles, ");
            sqlCommand.Append("smd.DefinitionName, ");
            sqlCommand.Append("smd.Description ");

            sqlCommand.Append("FROM	sts_MembershipProduct smp ");
            sqlCommand.Append("LEFT OUTER JOIN	sts_MemberDefinition smd ");
            sqlCommand.Append("ON smp.MemberDefGuid = smd.Guid ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("smp.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND smp.IsAvailable = 1 ");
            sqlCommand.Append("AND smp.OfferBeginUtc <= ?CurrentUtcTime ");
            sqlCommand.Append("AND ((smp.OfferEndUtc IS NULL) OR (smp.OfferEndUtc > ?CurrentUtcTime)) ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("smp.Title ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?CurrentUtcTime", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetByReminderTemplate(Guid siteGuid, Guid templateGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  smp.*, ");
            sqlCommand.Append("smd.GrantedRoles ");
            sqlCommand.Append("FROM	sts_MembershipProduct smp ");
            sqlCommand.Append("LEFT OUTER JOIN	sts_MemberDefinition smd ");
            sqlCommand.Append("ON smp.MemberDefGuid = smd.Guid ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("smp.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND smp.Guid IN ");
            sqlCommand.Append("(SELECT ProductGuid ");
            sqlCommand.Append("FROM sts_MembershipReminder ");
            sqlCommand.Append("WHERE TemplateGuid = ?TemplateGuid) ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("smp.Title ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?TemplateGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = templateGuid.ToString();

            return MySqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets a count of rows in the sts_MembershipProduct table.
        /// </summary>
        public static int GetCount(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_MembershipProduct ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }
    }
}
