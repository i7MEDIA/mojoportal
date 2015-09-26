// Author:					Joe Audette
// Created:					2010-09-08
// Last Modified:			2010-09-08
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
    public static class DBSiteAnalytics
    {
        /// <summary>
        /// Inserts a row in the sts_ga_SiteData table. Returns rows affected count.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="profileId"> profileId </param>
        /// <param name="date"> date </param>
        /// <param name="pageViews"> pageViews </param>
        /// <param name="visits"> visits </param>
        /// <param name="visitors"> visitors </param>
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
            string segmentId,
            DateTime analyticsDate,
            int pageViews,
            int visits,
            int visitors,
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
            sqlCommand.Append("INSERT INTO sts_ga_SiteData (");
         
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ProfileId, ");
            sqlCommand.Append("SegmentId, ");
            sqlCommand.Append("AnalyticsDate, ");
            sqlCommand.Append("ADate, ");
            sqlCommand.Append("PageViews, ");
            sqlCommand.Append("Visits, ");
            sqlCommand.Append("Visitors, ");
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
            sqlCommand.Append("?SegmentId, ");
            sqlCommand.Append("?AnalyticsDate, ");
            sqlCommand.Append("?ADate, ");
            sqlCommand.Append("?PageViews, ");
            sqlCommand.Append("?Visits, ");
            sqlCommand.Append("?Visitors, ");
            sqlCommand.Append("?NewVisits, ");
            sqlCommand.Append("?Bounces, ");
            sqlCommand.Append("?Entrances, ");
            sqlCommand.Append("?Exits, ");
            sqlCommand.Append("?PagesPerVisit, ");
            sqlCommand.Append("?TimeOnPage, ");
            sqlCommand.Append("?TimeOnSite, ");
            sqlCommand.Append("?CapturedUtc )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[16];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            arParams[2] = new MySqlParameter("?SegmentId", MySqlDbType.VarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = segmentId;

            arParams[3] = new MySqlParameter("?AnalyticsDate", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = analyticsDate;

            arParams[4] = new MySqlParameter("?PageViews", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageViews;

            arParams[5] = new MySqlParameter("?Visits", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = visits;

            arParams[6] = new MySqlParameter("?Visitors", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = visitors;

            arParams[7] = new MySqlParameter("?NewVisits", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = newVisits;

            arParams[8] = new MySqlParameter("?Bounces", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = bounces;

            arParams[9] = new MySqlParameter("?Entrances", MySqlDbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = entrances;

            arParams[10] = new MySqlParameter("?Exits", MySqlDbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = exits;

            arParams[11] = new MySqlParameter("?PagesPerVisit", MySqlDbType.Decimal, 9);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = pagesPerVisit;

            arParams[12] = new MySqlParameter("?TimeOnPage", MySqlDbType.Decimal, 18);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = timeOnPage;

            arParams[13] = new MySqlParameter("?TimeOnSite", MySqlDbType.Decimal, 18);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = timeOnSite;

            arParams[14] = new MySqlParameter("?CapturedUtc", MySqlDbType.DateTime);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = capturedUtc;

            arParams[15] = new MySqlParameter("?ADate", MySqlDbType.Int32);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = Utility.DateTonInteger(analyticsDate);

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }

        public static bool EntryExists(Guid siteGuid, string profileId, string segmentId, DateTime analyticsDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_ga_SiteData ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ProfileId = ?ProfileId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ADate = ?ADate ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SegmentId = ?SegmentId ");

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

            arParams[3] = new MySqlParameter("?SegmentId", MySqlDbType.VarChar, 50);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = segmentId;

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static IDataReader GetMostRecent(Guid siteGuid, string profileId, string segmentId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_ga_SiteData ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ProfileId = ?ProfileId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SegmentId = ?SegmentId ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("ADate DESC ");
            sqlCommand.Append("LIMIT 1");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            arParams[2] = new MySqlParameter("?SegmentId", MySqlDbType.VarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = segmentId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }



        public static IDataReader GetSegmentMonthReport(bool tracking1ProfileOnly, Guid siteGuid, string profileId, string segmentId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("s.SegmentName, ");
            sqlCommand.Append("YEAR(sd.AnalyticsDate) As Y,  ");
            sqlCommand.Append("MONTH(sd.AnalyticsDate) As M, ");
            sqlCommand.Append("SUM(sd.PageViews) As PageViews, ");
            sqlCommand.Append("SUM(sd.Visits) As Visits, ");
            sqlCommand.Append("SUM(sd.NewVisits) As NewVisits, ");
            sqlCommand.Append("SUM(sd.Bounces) As Bounces, ");
            sqlCommand.Append("SUM(sd.Entrances) As Entrances, ");
            sqlCommand.Append("SUM(sd.Exits) As Exits, ");
            sqlCommand.Append("(AVG(TimeOnPage)/AVG(Visits)) As TimeOnPage, ");
            sqlCommand.Append("(AVG(TimeOnSite)/AVG(Visits)) As TimeOnSite, ");
            sqlCommand.Append("(AVG(PageViews * 1.0)/AVG(Visits * 1.0)) As PagesPerVisit ");

            sqlCommand.Append("FROM	sts_ga_SiteData sd ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("sts_ga_Segments s ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("s.SegmentId = sd.SegmentId ");

            sqlCommand.Append("WHERE ");
            
            sqlCommand.Append("sd.SegmentId = ?SegmentId ");

            if (!tracking1ProfileOnly)
            {
                sqlCommand.Append("AND ");
                sqlCommand.Append("sd.SiteGuid = ?SiteGuid ");
                sqlCommand.Append("AND ");
                sqlCommand.Append("sd.ProfileId = ?ProfileId ");
            }
            

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("s.SegmentName, ");
            sqlCommand.Append("YEAR(sd.AnalyticsDate),  ");
            sqlCommand.Append("MONTH(sd.AnalyticsDate)  ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("YEAR(sd.AnalyticsDate),  ");
            sqlCommand.Append("MONTH(sd.AnalyticsDate)  ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            arParams[2] = new MySqlParameter("?SegmentId", MySqlDbType.VarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = segmentId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetSegmentMonthReport(bool tracking1ProfileOnly, Guid siteGuid, string profileId, string segmentId, DateTime beginDate, DateTime endDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("s.SegmentName, ");
            sqlCommand.Append("YEAR(sd.AnalyticsDate) As Y,  ");
            sqlCommand.Append("MONTH(sd.AnalyticsDate) As M, ");
            sqlCommand.Append("SUM(sd.PageViews) As PageViews, ");
            sqlCommand.Append("SUM(sd.Visits) As Visits, ");
            sqlCommand.Append("SUM(sd.NewVisits) As NewVisits, ");
            sqlCommand.Append("SUM(sd.Bounces) As Bounces, ");
            sqlCommand.Append("SUM(sd.Entrances) As Entrances, ");
            sqlCommand.Append("SUM(sd.Exits) As Exits, ");
            sqlCommand.Append("(AVG(TimeOnPage)/AVG(Visits)) As TimeOnPage, ");
            sqlCommand.Append("(AVG(TimeOnSite)/AVG(Visits)) As TimeOnSite, ");
            sqlCommand.Append("(AVG(PageViews * 1.0)/AVG(Visits * 1.0)) As PagesPerVisit ");

            sqlCommand.Append("FROM	sts_ga_SiteData sd ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("sts_ga_Segments s ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("s.SegmentId = sd.SegmentId ");

            sqlCommand.Append("WHERE ");
           
            sqlCommand.Append("sd.SegmentId = ?SegmentId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("sd.ADate >= ?BeginDate ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("sd.ADate <= ?EndDate ");

            if (!tracking1ProfileOnly)
            {
                sqlCommand.Append("AND ");
                sqlCommand.Append("sd.SiteGuid = ?SiteGuid ");
                sqlCommand.Append("AND ");
                sqlCommand.Append("sd.ProfileId = ?ProfileId ");
            }

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("s.SegmentName, ");
            sqlCommand.Append("YEAR(sd.AnalyticsDate),  ");
            sqlCommand.Append("MONTH(sd.AnalyticsDate)  ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("YEAR(sd.AnalyticsDate),  ");
            sqlCommand.Append("MONTH(sd.AnalyticsDate)  ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            arParams[2] = new MySqlParameter("?SegmentId", MySqlDbType.VarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = segmentId;

            arParams[3] = new MySqlParameter("?BeginDate", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = Utility.DateTonInteger(beginDate);

            arParams[4] = new MySqlParameter("?EndDate", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = Utility.DateTonInteger(endDate);

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


    }
}
