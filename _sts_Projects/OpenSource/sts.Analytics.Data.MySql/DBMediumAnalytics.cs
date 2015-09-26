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
    public static class DBMediumAnalytics
    {
        /// <summary>
        /// Inserts a row in the sts_ga_MediumData table. Returns rows affected count.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="profileId"> profileId </param>
        /// <param name="analyticsDate"> analyticsDate </param>
        /// <param name="medium"> medium </param>
        /// <param name="pageViews"> pageViews </param>
        /// <param name="visits"> visits </param>
        /// <param name="newVisits"> newVisits </param>
        /// <param name="bounces"> bounces </param>
        /// <param name="entrances"> entrances </param>
        /// <param name="exits"> exits </param>
        /// <param name="timeOnPage"> timeOnPage </param>
        /// <param name="timeOnSite"> timeOnSite </param>
        /// <param name="capturedUtc"> capturedUtc </param>
        /// <returns>int</returns>
        public static int Create(
            Guid siteGuid,
            string profileId,
            DateTime analyticsDate,
            string medium,
            int pageViews,
            int visits,
            int newVisits,
            int bounces,
            int entrances,
            int exits,
            decimal pagesPerVisit,
            decimal timeOnPage,
            decimal timeOnSite,
            DateTime capturedUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_ga_MediumData (");
        
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ProfileId, ");
            sqlCommand.Append("AnalyticsDate, ");
            sqlCommand.Append("ADate, ");
            sqlCommand.Append("Medium, ");
            sqlCommand.Append("PageViews, ");
            sqlCommand.Append("Visits, ");
            sqlCommand.Append("NewVisits, ");
            sqlCommand.Append("Bounces, ");
            sqlCommand.Append("Entrances, ");
            sqlCommand.Append("Exits, ");
            sqlCommand.Append("PagesPerVisit, ");
            sqlCommand.Append("TimeOnPage, ");
            sqlCommand.Append("TimeOnSite, ");
            sqlCommand.Append("CapturedUtc )");

            sqlCommand.Append(" VALUES (");
         
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?ProfileId, ");
            sqlCommand.Append("?AnalyticsDate, ");
            sqlCommand.Append("?ADate, ");
            sqlCommand.Append("?Medium, ");
            sqlCommand.Append("?PageViews, ");
            sqlCommand.Append("?Visits, ");
            sqlCommand.Append("?NewVisits, ");
            sqlCommand.Append("?Bounces, ");
            sqlCommand.Append("?Entrances, ");
            sqlCommand.Append("?Exits, ");
            sqlCommand.Append("?PagesPerVisit, ");
            sqlCommand.Append("?TimeOnPage, ");
            sqlCommand.Append("?TimeOnSite, ");
            sqlCommand.Append("?CapturedUtc )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[15];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            arParams[2] = new MySqlParameter("?AnalyticsDate", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = analyticsDate;

            arParams[3] = new MySqlParameter("?Medium", MySqlDbType.VarChar, 15);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = medium;

            arParams[4] = new MySqlParameter("?PageViews", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageViews;

            arParams[5] = new MySqlParameter("?Visits", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = visits;

            arParams[6] = new MySqlParameter("?NewVisits", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = newVisits;

            arParams[7] = new MySqlParameter("?Bounces", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = bounces;

            arParams[8] = new MySqlParameter("?Entrances", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = entrances;

            arParams[9] = new MySqlParameter("?Exits", MySqlDbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = exits;

            arParams[10] = new MySqlParameter("?TimeOnPage", MySqlDbType.Decimal);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = timeOnPage;

            arParams[11] = new MySqlParameter("?TimeOnSite", MySqlDbType.Decimal);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = timeOnSite;

            arParams[12] = new MySqlParameter("?CapturedUtc", MySqlDbType.DateTime);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = capturedUtc;

            arParams[13] = new MySqlParameter("?ADate", MySqlDbType.Int32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = Utility.DateTonInteger(analyticsDate);

            arParams[14] = new MySqlParameter("?PagesPerVisit", MySqlDbType.Decimal);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = pagesPerVisit;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_ga_MediumData ");
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
            sqlCommand.Append("TRUNCATE TABLE sts_ga_MediumData ");
            sqlCommand.Append(";");

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                null);

        }

        public static void ReIndex()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("OPTIMIZE TABLE sts_ga_MediumData ");
            sqlCommand.Append(";");

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                null);
        }

        public static bool EntryExists(Guid siteGuid, string profileId, string medium, DateTime analyticsDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_ga_MediumData ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ProfileId = ?ProfileId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ADate = ?ADate ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("Medium = ?Medium ");

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

            arParams[3] = new MySqlParameter("?Medium", MySqlDbType.VarChar, 15);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = medium;

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
            sqlCommand.Append("FROM	sts_ga_MediumData ");
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

        public static IDataReader GetMonthReport(bool tracking1ProfileOnly, Guid siteGuid, string profileId, DateTime beginDate, DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("Medium, ");
            sqlCommand.Append("YEAR(AnalyticsDate) As Y,  ");
            sqlCommand.Append("MONTH(AnalyticsDate) As M, ");
            sqlCommand.Append("SUM(PageViews) As PageViews, ");
            sqlCommand.Append("SUM(Visits) As Visits, ");
            sqlCommand.Append("SUM(NewVisits) As NewVisits, ");
            sqlCommand.Append("SUM(Bounces) As Bounces, ");
            sqlCommand.Append("SUM(Entrances) As Entrances, ");
            sqlCommand.Append("SUM(Exits) As Exits, ");
            sqlCommand.Append("(AVG(TimeOnPage)/AVG(Visits)) As TimeOnPage, ");
            sqlCommand.Append("(AVG(TimeOnSite)/AVG(Visits)) As TimeOnSite, ");
            sqlCommand.Append("AVG(PagesPerVisit) As PagesPerVisit ");

            sqlCommand.Append("FROM	sts_ga_MediumData ");

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
            sqlCommand.Append("Medium, ");
            sqlCommand.Append("YEAR(AnalyticsDate),  ");
            sqlCommand.Append("MONTH(AnalyticsDate)  ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("YEAR(AnalyticsDate),  ");
            sqlCommand.Append("MONTH(AnalyticsDate)  ");

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

        public static IDataReader GetDayReport(bool tracking1ProfileOnly, Guid siteGuid, string profileId, DateTime beginDate, DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("Medium, ");
            sqlCommand.Append("YEAR(AnalyticsDate) As Y,  ");
            sqlCommand.Append("MONTH(AnalyticsDate) As M, ");
            sqlCommand.Append("DAY(AnalyticsDate) As D, ");
            sqlCommand.Append("SUM(PageViews) As PageViews, ");
            sqlCommand.Append("SUM(Visits) As Visits, ");
            sqlCommand.Append("SUM(NewVisits) As NewVisits, ");
            sqlCommand.Append("SUM(Bounces) As Bounces, ");
            sqlCommand.Append("SUM(Entrances) As Entrances, ");
            sqlCommand.Append("SUM(Exits) As Exits, ");
            sqlCommand.Append("(AVG(TimeOnPage)/AVG(Visits)) As TimeOnPage, ");
            sqlCommand.Append("(AVG(TimeOnSite)/AVG(Visits)) As TimeOnSite, ");
            sqlCommand.Append("AVG(PagesPerVisit) As PagesPerVisit ");

            sqlCommand.Append("FROM	sts_ga_MediumData ");

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
            sqlCommand.Append("Medium, ");
            sqlCommand.Append("YEAR(AnalyticsDate),  ");
            sqlCommand.Append("MONTH(AnalyticsDate),  ");
            sqlCommand.Append("DAY(AnalyticsDate) ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("YEAR(AnalyticsDate),  ");
            sqlCommand.Append("MONTH(AnalyticsDate),  ");
            sqlCommand.Append("DAY(AnalyticsDate)  ");

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
            sqlCommand.Append("Medium, ");
            sqlCommand.Append("SUM(PageViews) As PageViews, ");
            sqlCommand.Append("SUM(Visits) As Visits, ");
            sqlCommand.Append("SUM(NewVisits) As NewVisits, ");
            sqlCommand.Append("SUM(Bounces) As Bounces, ");
            sqlCommand.Append("SUM(Entrances) As Entrances, ");
            sqlCommand.Append("SUM(Exits) As Exits, ");
            sqlCommand.Append("(AVG(TimeOnPage)/AVG(Visits)) As TimeOnPage, ");
            sqlCommand.Append("(AVG(TimeOnSite)/AVG(Visits)) As TimeOnSite, ");
            sqlCommand.Append("AVG(PagesPerVisit) As PagesPerVisit ");

            sqlCommand.Append("FROM	sts_ga_MediumData ");

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
            sqlCommand.Append("Medium ");
            
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("Medium  ");
            
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

        public static IDataReader GetBasicStatsReport(bool tracking1ProfileOnly, Guid siteGuid, string profileId, DateTime beginDate, DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("SUM(PageViews) As PageViews, ");
            sqlCommand.Append("SUM(Visits) As Visits, ");
            sqlCommand.Append("SUM(NewVisits) As NewVisits, ");
            sqlCommand.Append("(AVG(Bounces * 1.0)/AVG(Visits * 1.0)) As BounceRate, ");
            sqlCommand.Append("SUM(Entrances) As Entrances, ");
            sqlCommand.Append("SUM(Exits) As Exits, ");
            sqlCommand.Append("(AVG(TimeOnPage)/AVG(Visits)) As TimeOnPage, ");
            sqlCommand.Append("(AVG(TimeOnSite)/AVG(Visits)) As TimeOnSite, ");
            sqlCommand.Append("AVG(PagesPerVisit) As PagesPerVisit ");

            sqlCommand.Append("FROM	sts_ga_MediumData ");

            sqlCommand.Append("WHERE ");
           
            sqlCommand.Append("ADate >= ?BeginDate ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ADate <= ?EndDate ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("PageViews > 0 ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("Visits > 0 ");

            if (!tracking1ProfileOnly)
            {
                sqlCommand.Append("AND ");
                sqlCommand.Append("SiteGuid = ?SiteGuid ");
                sqlCommand.Append("AND ");
                sqlCommand.Append("ProfileId = ?ProfileId ");
            }

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
