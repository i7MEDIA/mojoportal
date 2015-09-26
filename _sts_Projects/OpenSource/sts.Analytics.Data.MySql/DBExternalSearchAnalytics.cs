// Author:					Joe Audette
// Created:					2010-09-08
// Last Modified:			2010-10-05
// 
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace sts.Analytics.Data
{
    public static class DBExternalSearchAnalytics
    {
        /// <summary>
        /// Inserts a row in the sts_ga_ExternalSearchData table. Returns rows affected count.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="profileId"> profileId </param>
        /// <param name="analyticsDate"> analyticsDate </param>
        /// <param name="searchTerm"> searchTerm </param>
        /// <param name="pageViews"> pageViews </param>
        /// <param name="visits"> visits </param>
        /// <param name="newVisits"> newVisits </param>
        /// <param name="bounces"> bounces </param>
        /// <param name="capturedUtc"> capturedUtc </param>
        /// <returns>int</returns>
        public static int Create(
            Guid siteGuid,
            string profileId,
            DateTime analyticsDate,
            string searchTerm,
            int pageViews,
            int visits,
            int newVisits,
            decimal bounceRate,
            DateTime capturedUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_ga_ExternalSearchData (");
     
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ProfileId, ");
            sqlCommand.Append("AnalyticsDate, ");
            sqlCommand.Append("ADate, ");
            sqlCommand.Append("SearchTerm, ");
            sqlCommand.Append("PageViews, ");
            sqlCommand.Append("Visits, ");
            sqlCommand.Append("NewVisits, ");
            sqlCommand.Append("BounceRate, ");
            sqlCommand.Append("CapturedUtc )");

            sqlCommand.Append(" VALUES (");
       
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?ProfileId, ");
            sqlCommand.Append("?AnalyticsDate, ");
            sqlCommand.Append("?ADate, ");
            sqlCommand.Append("?SearchTerm, ");
            sqlCommand.Append("?PageViews, ");
            sqlCommand.Append("?Visits, ");
            sqlCommand.Append("?NewVisits, ");
            sqlCommand.Append("?BounceRate, ");
            sqlCommand.Append("?CapturedUtc )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[10];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            arParams[2] = new MySqlParameter("?AnalyticsDate", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = analyticsDate;

            if (searchTerm.Length > 255) { searchTerm = searchTerm.Substring(0, 255); }

            arParams[3] = new MySqlParameter("?SearchTerm", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = searchTerm;

            arParams[4] = new MySqlParameter("?PageViews", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageViews;

            arParams[5] = new MySqlParameter("?Visits", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = visits;

            arParams[6] = new MySqlParameter("?NewVisits", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = newVisits;

            arParams[7] = new MySqlParameter("?BounceRate", MySqlDbType.Decimal);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = bounceRate;

            arParams[8] = new MySqlParameter("?CapturedUtc", MySqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = capturedUtc;

            arParams[9] = new MySqlParameter("?ADate", MySqlDbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = Utility.DateTonInteger(analyticsDate);

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_ga_ExternalSearchData ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static void Truncate()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("TRUNCATE TABLE sts_ga_ExternalSearchData ");
            sqlCommand.Append(";");

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                null);

        }

        public static void ReIndex()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("OPTIMIZE TABLE sts_ga_ExternalSearchData ");
            sqlCommand.Append(";");

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                null);
        }

        public static bool EntryExists(Guid siteGuid, string profileId, DateTime analyticsDate, string searchTerm)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_ga_ExternalSearchData ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ProfileId = ?ProfileId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ADate = ?ADate ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SearchTerm = ?SearchTerm ");
            
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            arParams[2] = new MySqlParameter("?ADate", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = Utility.DateTonInteger(analyticsDate);

            if (searchTerm.Length > 255) { searchTerm = searchTerm.Substring(0, 255); }

            arParams[3] = new MySqlParameter("?SearchTerm", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = searchTerm;

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static IDataReader GetMostRecent(Guid siteGuid, string profileId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_ga_ExternalSearchData ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ProfileId = ?ProfileId ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("ADate DESC ");
            sqlCommand.Append("LIMIT 1");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetTopReport(bool tracking1ProfileOnly, Guid siteGuid, string profileId, DateTime beginDate, DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");

            sqlCommand.Append("SearchTerm, ");
            sqlCommand.Append("SUM(PageViews) As PageViews, ");
            sqlCommand.Append("SUM(Visits) As Visits, ");
            sqlCommand.Append("SUM(NewVisits) As NewVisits, ");
            sqlCommand.Append("AVG(BounceRate) As BounceRate ");
            
            sqlCommand.Append("FROM	sts_ga_ExternalSearchData ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ADate >= ?BeginDate ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ADate <= ?EndDate ");

            if (!tracking1ProfileOnly)
            {
                sqlCommand.Append("AND ");
                sqlCommand.Append("SiteGuid = ?SiteGuid ");
                sqlCommand.Append("AND ");
                sqlCommand.Append("ProfileId = ?ProfileId ");
            }

            sqlCommand.Append("GROUP BY ");

            sqlCommand.Append("SearchTerm ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("SUM(PageViews) DESC  ");

            sqlCommand.Append("LIMIT 20  ");

            sqlCommand.Append(";");


            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            arParams[2] = new MySqlParameter("?BeginDate", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = Utility.DateTonInteger(beginDate);

            arParams[3] = new MySqlParameter("?EndDate", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = Utility.DateTonInteger(endDate);

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetCount(bool tracking1ProfileOnly, Guid siteGuid, string profileId, DateTime beginDate, DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	 ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT COALESCE(SearchTerm,'') As SearchTerm, Count(*) As TheCount  ");
            sqlCommand.Append("FROM sts_ga_ExternalSearchData ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ADate >= ?BeginDate ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ADate <= ?EndDate ");

            if (!tracking1ProfileOnly)
            {
                sqlCommand.Append("AND ");
                sqlCommand.Append("SiteGuid = ?SiteGuid ");
                sqlCommand.Append("AND ");
                sqlCommand.Append("ProfileId = ?ProfileId ");
            }

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("COALESCE(SearchTerm,'') ");

            sqlCommand.Append(") s ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            arParams[2] = new MySqlParameter("?BeginDate", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = Utility.DateTonInteger(beginDate);

            arParams[3] = new MySqlParameter("?EndDate", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = Utility.DateTonInteger(endDate);

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetPage(
            bool tracking1ProfileOnly,
            Guid siteGuid,
            string profileId,
            DateTime beginDate,
            DateTime endDate,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(tracking1ProfileOnly, siteGuid, profileId, beginDate, endDate);

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
            sqlCommand.Append("SELECT ");

            sqlCommand.Append("SearchTerm, ");
            sqlCommand.Append("SUM(PageViews) As PageViews, ");
            sqlCommand.Append("SUM(Visits) As Visits, ");
            sqlCommand.Append("SUM(NewVisits) As NewVisits, ");
            sqlCommand.Append("AVG(BounceRate) As BounceRate ");

            sqlCommand.Append("FROM	sts_ga_ExternalSearchData  ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ADate >= ?BeginDate ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ADate <= ?EndDate ");

            if (!tracking1ProfileOnly)
            {
                sqlCommand.Append("AND ");
                sqlCommand.Append("SiteGuid = ?SiteGuid ");
                sqlCommand.Append("AND ");
                sqlCommand.Append("ProfileId = ?ProfileId ");
            }

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("SearchTerm ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("SUM(PageViews) DESC  ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            arParams[2] = new MySqlParameter("?BeginDate", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = Utility.DateTonInteger(beginDate);

            arParams[3] = new MySqlParameter("?EndDate", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = Utility.DateTonInteger(endDate);

            arParams[4] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageSize;

            arParams[5] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static int GetCount(bool tracking1ProfileOnly, Guid siteGuid, string profileId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	 ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT COALESCE(SearchTerm,'') As SearchTerm, Count(*) As TheCount  ");
            sqlCommand.Append("FROM sts_ga_ExternalSearchData ");

            if (!tracking1ProfileOnly)
            {
                sqlCommand.Append("WHERE ");
                sqlCommand.Append("SiteGuid = ?SiteGuid ");
                sqlCommand.Append("AND ");
                sqlCommand.Append("ProfileId = ?ProfileId ");
            }

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("COALESCE(SearchTerm,'') ");

            sqlCommand.Append(") s ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetPage(
            bool tracking1ProfileOnly,
            Guid siteGuid,
            string profileId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(tracking1ProfileOnly, siteGuid, profileId);

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
            sqlCommand.Append("SELECT ");

            sqlCommand.Append("SearchTerm, ");
            sqlCommand.Append("SUM(PageViews) As PageViews, ");
            sqlCommand.Append("SUM(Visits) As Visits, ");
            sqlCommand.Append("SUM(NewVisits) As NewVisits, ");
            sqlCommand.Append("AVG(BounceRate) As BounceRate ");

            sqlCommand.Append("FROM	sts_ga_ExternalSearchData  ");

            if (!tracking1ProfileOnly)
            {
                sqlCommand.Append("WHERE ");
                sqlCommand.Append("SiteGuid = ?SiteGuid ");
                sqlCommand.Append("AND ");
                sqlCommand.Append("ProfileId = ?ProfileId ");
            }

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("SearchTerm ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("SUM(PageViews) DESC  ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            arParams[2] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

    }
}
