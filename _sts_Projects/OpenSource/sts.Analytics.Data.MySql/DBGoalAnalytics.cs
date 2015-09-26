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
    public static class DBGoalAnalytics
    {

        /// <summary>
        /// Inserts a row in the sts_ga_GoalData table. Returns rows affected count.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="profileId"> profileId </param>
        /// <param name="analyticsDate"> analyticsDate </param>
        /// <param name="medium"> medium </param>
        /// <param name="goal1Starts"> goal1Starts </param>
        /// <param name="goal1Completions"> goal1Completions </param>
        /// <param name="goal1Value"> goal1Value </param>
        /// <param name="goal2Starts"> goal2Starts </param>
        /// <param name="goal2Completions"> goal2Completions </param>
        /// <param name="goal2Value"> goal2Value </param>
        /// <param name="goal3Starts"> goal3Starts </param>
        /// <param name="goal3Completions"> goal3Completions </param>
        /// <param name="goal3Value"> goal3Value </param>
        /// <param name="goal4Starts"> goal4Starts </param>
        /// <param name="goal4Completions"> goal4Completions </param>
        /// <param name="goal4Value"> goal4Value </param>
        /// <param name="capturedUtc"> capturedUtc </param>
        /// <returns>int</returns>
        public static int Create(
            Guid siteGuid,
            string profileId,
            DateTime analyticsDate,
            string medium,
            int goal1Starts,
            int goal1Completions,
            decimal goal1Value,
            int goal2Starts,
            int goal2Completions,
            decimal goal2Value,
            int goal3Starts,
            int goal3Completions,
            decimal goal3Value,
            int goal4Starts,
            int goal4Completions,
            decimal goal4Value,
            DateTime capturedUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_ga_GoalData (");
       
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ProfileId, ");
            sqlCommand.Append("AnalyticsDate, ");
            sqlCommand.Append("ADate, ");
            sqlCommand.Append("Medium, ");
            sqlCommand.Append("Goal1Starts, ");
            sqlCommand.Append("Goal1Completions, ");
            sqlCommand.Append("Goal1Value, ");
            sqlCommand.Append("Goal2Starts, ");
            sqlCommand.Append("Goal2Completions, ");
            sqlCommand.Append("Goal2Value, ");
            sqlCommand.Append("Goal3Starts, ");
            sqlCommand.Append("Goal3Completions, ");
            sqlCommand.Append("Goal3Value, ");
            sqlCommand.Append("Goal4Starts, ");
            sqlCommand.Append("Goal4Completions, ");
            sqlCommand.Append("Goal4Value, ");
            sqlCommand.Append("CapturedUtc )");

            sqlCommand.Append(" VALUES (");
       
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?ProfileId, ");
            sqlCommand.Append("?AnalyticsDate, ");
            sqlCommand.Append("?ADate, ");
            sqlCommand.Append("?Medium, ");
            sqlCommand.Append("?Goal1Starts, ");
            sqlCommand.Append("?Goal1Completions, ");
            sqlCommand.Append("?Goal1Value, ");
            sqlCommand.Append("?Goal2Starts, ");
            sqlCommand.Append("?Goal2Completions, ");
            sqlCommand.Append("?Goal2Value, ");
            sqlCommand.Append("?Goal3Starts, ");
            sqlCommand.Append("?Goal3Completions, ");
            sqlCommand.Append("?Goal3Value, ");
            sqlCommand.Append("?Goal4Starts, ");
            sqlCommand.Append("?Goal4Completions, ");
            sqlCommand.Append("?Goal4Value, ");
            sqlCommand.Append("?CapturedUtc )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[18];

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

            arParams[4] = new MySqlParameter("?Goal1Starts", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = goal1Starts;

            arParams[5] = new MySqlParameter("?Goal1Completions", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = goal1Completions;

            arParams[6] = new MySqlParameter("?Goal1Value", MySqlDbType.Decimal);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = goal1Value;

            arParams[7] = new MySqlParameter("?Goal2Starts", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = goal2Starts;

            arParams[8] = new MySqlParameter("?Goal2Completions", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = goal2Completions;

            arParams[9] = new MySqlParameter("?Goal2Value", MySqlDbType.Decimal);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = goal2Value;

            arParams[10] = new MySqlParameter("?Goal3Starts", MySqlDbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = goal3Starts;

            arParams[11] = new MySqlParameter("?Goal3Completions", MySqlDbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = goal3Completions;

            arParams[12] = new MySqlParameter("?Goal3Value", MySqlDbType.Decimal);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = goal3Value;

            arParams[13] = new MySqlParameter("?Goal4Starts", MySqlDbType.Int32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = goal4Starts;

            arParams[14] = new MySqlParameter("?Goal4Completions", MySqlDbType.Int32);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = goal4Completions;

            arParams[15] = new MySqlParameter("?Goal4Value", MySqlDbType.Decimal);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = goal4Value;

            arParams[16] = new MySqlParameter("?CapturedUtc", MySqlDbType.DateTime);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = capturedUtc;

            arParams[17] = new MySqlParameter("?ADate", MySqlDbType.Int32);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = Utility.DateTonInteger(analyticsDate);

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        public static bool UpdateGoal4(
            Guid siteGuid,
            string profileId,
            DateTime analyticsDate,
            string medium,
            int goal4Starts,
            int goal4Completions,
            decimal goal4Value)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_ga_GoalData ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("Goal4Starts = ?Goal4Starts, ");
            sqlCommand.Append("Goal4Completions = ?Goal4Completions, ");
            sqlCommand.Append("Goal4Value = ?Goal4Value ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ProfileId = ?ProfileId ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ADate = ?ADate ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("Medium = ?Medium ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("Goal4Starts = -1 ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[7];

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

            arParams[4] = new MySqlParameter("?Goal4Starts", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = goal4Starts;

            arParams[5] = new MySqlParameter("?Goal4Completions", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = goal4Completions;

            arParams[6] = new MySqlParameter("?Goal4Value", MySqlDbType.Decimal);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = goal4Value;

           

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool EntryExists(Guid siteGuid, string profileId, string medium, DateTime analyticsDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_ga_GoalData ");
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

            arParams[3] = new MySqlParameter("?Medium", MySqlDbType.VarChar, 50);
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
            sqlCommand.Append("FROM	sts_ga_GoalData ");
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

        /// <summary>
        /// gets the oldest row where we have not yet updated group 4 as indicated by Group4Starts = -1
        /// </summary>
        /// <param name="siteGuid"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public static IDataReader GetOldestUnprocessedGroup4(Guid siteGuid, string profileId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_ga_GoalData ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ProfileId = ?ProfileId ");
            sqlCommand.Append("AND Goal4Starts = -1 ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("ADate ");
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

    }
}
