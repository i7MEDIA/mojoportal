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
    public static class DBBrowserAnalytics
    {
        /// <summary>
        /// Inserts a row in the sts_ga_BrowserData table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="profileId"> profileId </param>
        /// <param name="analyticsDate"> analyticsDate </param>
        /// <param name="browser"> browser </param>
        /// <param name="visits"> visits </param>
        /// <param name="capturedUtc"> capturedUtc </param>
        /// <returns>int</returns>
        public static int Create(
            Guid siteGuid,
            string profileId,
            DateTime analyticsDate,
            string browser,
            string browserVersion,
            string operatingSystem,
            int pageViews,
            int visits,
            int newVisits,
            decimal pagesPerVisit,
            decimal bounceRate,
            DateTime capturedUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_ga_BrowserData (");
    
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ProfileId, ");
            sqlCommand.Append("AnalyticsDate, ");
            sqlCommand.Append("ADate, ");
            sqlCommand.Append("Browser, ");
            sqlCommand.Append("BrowserVersion, ");
            sqlCommand.Append("OperatingSystem, ");
            sqlCommand.Append("Visits, ");
            sqlCommand.Append("NewVisits, ");
            sqlCommand.Append("PageViews, ");
            sqlCommand.Append("PagesPerVisit, ");
            sqlCommand.Append("BounceRate, ");
            sqlCommand.Append("CapturedUtc )");

            sqlCommand.Append(" VALUES (");
   
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?ProfileId, ");
            sqlCommand.Append("?AnalyticsDate, ");
            sqlCommand.Append("?ADate, ");
            sqlCommand.Append("?Browser, ");
            sqlCommand.Append("?BrowserVersion, ");
            sqlCommand.Append("?OperatingSystem, ");
            sqlCommand.Append("?Visits, ");
            sqlCommand.Append("?NewVisits, ");
            sqlCommand.Append("?PageViews, ");
            sqlCommand.Append("?PagesPerVisit, ");
            sqlCommand.Append("?BounceRate, ");
            sqlCommand.Append("?CapturedUtc )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[13];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            arParams[2] = new MySqlParameter("?AnalyticsDate", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = analyticsDate;

            arParams[3] = new MySqlParameter("?Browser", MySqlDbType.VarChar, 50);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = browser;

            arParams[4] = new MySqlParameter("?BrowserVersion", MySqlDbType.VarChar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = browserVersion;

            arParams[5] = new MySqlParameter("?OperatingSystem", MySqlDbType.VarChar, 50);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = operatingSystem;

            arParams[6] = new MySqlParameter("?Visits", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = visits;

            arParams[7] = new MySqlParameter("?NewVisits", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = newVisits;

            arParams[8] = new MySqlParameter("?PageViews", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = pageViews;

            arParams[9] = new MySqlParameter("?PagesPerVisit", MySqlDbType.Decimal, 9);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = pagesPerVisit;

            arParams[10] = new MySqlParameter("?BounceRate", MySqlDbType.Decimal, 7);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = bounceRate;

            arParams[11] = new MySqlParameter("?CapturedUtc", MySqlDbType.DateTime);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = capturedUtc;

            arParams[12] = new MySqlParameter("?ADate", MySqlDbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = Utility.DateTonInteger(analyticsDate);

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_ga_BrowserData ");
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
            sqlCommand.Append("TRUNCATE TABLE sts_ga_BrowserData ");
            sqlCommand.Append(";");

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                null);

        }

        public static void ReIndex()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("OPTIMIZE TABLE sts_ga_BrowserData ");
            sqlCommand.Append(";");

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                null);
        }

        public static bool EntryExists(Guid siteGuid, string profileId, string browser, string browserVersion, string operatingSystem, DateTime analyticsDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_ga_BrowserData ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ProfileId = ?ProfileId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ADate = ?ADate ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("Browser = ?Browser ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("BrowserVersion = ?BrowserVersion ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("OperatingSystem = ?OperatingSystem ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            arParams[2] = new MySqlParameter("?ADate", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = Utility.DateTonInteger(analyticsDate);

            arParams[3] = new MySqlParameter("?Browser", MySqlDbType.VarChar, 50);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = browser;

            arParams[4] = new MySqlParameter("?BrowserVersion", MySqlDbType.VarChar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = browserVersion;

            arParams[5] = new MySqlParameter("?OperatingSystem", MySqlDbType.VarChar, 50);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = operatingSystem;

            int count =  Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);
        }

        public static IDataReader GetMostRecent(Guid siteGuid, string profileId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_ga_BrowserData ");
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

        public static IDataReader GetTopOperatingSystemReport(bool tracking1ProfileOnly, Guid siteGuid, string profileId, DateTime beginDate, DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("OperatingSystem, ");
            sqlCommand.Append("SUM(PageViews) As PageViews, ");
            sqlCommand.Append("SUM(Visits) As Visits, ");
            sqlCommand.Append("SUM(NewVisits) As NewVisits, ");
            sqlCommand.Append("AVG(BounceRate) As BounceRate, ");
            sqlCommand.Append("AVG(PagesPerVisit) As PagesPerVisit ");

            sqlCommand.Append("FROM	sts_ga_BrowserData ");

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
            sqlCommand.Append("OperatingSystem ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("SUM(PageViews) DESC  ");

            sqlCommand.Append("LIMIT 10 ");

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

        public static IDataReader GetOperatingSystemReport(bool tracking1ProfileOnly, Guid siteGuid, string profileId, DateTime beginDate, DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("OperatingSystem, ");
            sqlCommand.Append("SUM(PageViews) As PageViews, ");
            sqlCommand.Append("SUM(Visits) As Visits, ");
            sqlCommand.Append("SUM(NewVisits) As NewVisits, ");
            sqlCommand.Append("AVG(BounceRate) As BounceRate, ");
            sqlCommand.Append("AVG(PagesPerVisit) As PagesPerVisit ");

            sqlCommand.Append("FROM	sts_ga_BrowserData ");

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
            sqlCommand.Append("OperatingSystem ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("SUM(PageViews) DESC  ");

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

        public static IDataReader GetReport(bool tracking1ProfileOnly, Guid siteGuid, string profileId, DateTime beginDate, DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("Browser, ");
            sqlCommand.Append("SUM(PageViews) As PageViews, ");
            sqlCommand.Append("SUM(Visits) As Visits, ");
            sqlCommand.Append("SUM(NewVisits) As NewVisits, ");
            sqlCommand.Append("AVG(BounceRate) As BounceRate, ");
            sqlCommand.Append("AVG(PagesPerVisit) As PagesPerVisit ");

            sqlCommand.Append("FROM	sts_ga_BrowserData ");

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
            sqlCommand.Append("Browser ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("SUM(PageViews) DESC  ");

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

    }
}
