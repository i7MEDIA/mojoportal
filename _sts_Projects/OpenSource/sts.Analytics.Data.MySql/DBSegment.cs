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
    public static class DBSegment
    {
        /// <summary>
        /// Inserts a row in the sts_ga_Segments table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="analyticsUser"> analyticsUser </param>
        /// <param name="segmentId"> segmentId </param>
        /// <param name="segmentName"> segmentName </param>
        /// <param name="definition"> definition </param>
        /// <param name="captureStats"> captureStats </param>
        /// <param name="sortRank"> sortRank </param>
        /// <returns>int</returns>
        public static int Create(
            Guid rowGuid,
            Guid siteGuid,
            string analyticsUser,
            string segmentId,
            string segmentName,
            string definition,
            bool captureStats,
            int sortRank)
        {
            #region Bit Conversion

            int intCaptureStats = 0;
            if (captureStats) { intCaptureStats = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_ga_Segments (");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("AnalyticsUser, ");
            sqlCommand.Append("SegmentId, ");
            sqlCommand.Append("SegmentName, ");
            sqlCommand.Append("Definition, ");
            sqlCommand.Append("CaptureStats, ");
            sqlCommand.Append("SortRank )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?RowGuid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?AnalyticsUser, ");
            sqlCommand.Append("?SegmentId, ");
            sqlCommand.Append("?SegmentName, ");
            sqlCommand.Append("?Definition, ");
            sqlCommand.Append("?CaptureStats, ");
            sqlCommand.Append("?SortRank )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[8];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?AnalyticsUser", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = analyticsUser;

            arParams[3] = new MySqlParameter("?SegmentId", MySqlDbType.VarChar, 50);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = segmentId;

            arParams[4] = new MySqlParameter("?SegmentName", MySqlDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = segmentName;

            arParams[5] = new MySqlParameter("?Definition", MySqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = definition;

            arParams[6] = new MySqlParameter("?CaptureStats", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = intCaptureStats;

            arParams[7] = new MySqlParameter("?SortRank", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = sortRank;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the sts_ga_Segments table. Returns true if row updated.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="captureStats"> captureStats </param>
        /// <param name="sortRank"> sortRank </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid rowGuid,
            bool captureStats,
            int sortRank)
        {
            #region Bit Conversion

            int intCaptureStats = 0;
            if (captureStats) { intCaptureStats = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_ga_Segments ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("CaptureStats = ?CaptureStats, ");
            sqlCommand.Append("SortRank = ?SortRank ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowGuid = ?RowGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new MySqlParameter("?CaptureStats", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = intCaptureStats;

            arParams[2] = new MySqlParameter("?SortRank", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = sortRank;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        /// <summary>
        /// Deletes a row from the sts_ga_Segments table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_ga_Segments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = ?RowGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);
        }

        public static bool Exists(Guid siteGuid, string analyticsUser, string segmentId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_ga_Segments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("AnalyticsUser = ?AnalyticsUser ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SegmentId = ?SegmentId ");
            
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?AnalyticsUser", MySqlDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = analyticsUser;

            arParams[2] = new MySqlParameter("?SegmentId", MySqlDbType.VarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = segmentId;

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the sts_ga_Segments table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_ga_Segments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = ?RowGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets an IDataReader with all rows from the sts_ga_Segments table matching the passed in params.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetAll(Guid siteGuid, string analyticsUser)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_ga_Segments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("AnalyticsUser = ?AnalyticsUser ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?AnalyticsUser", MySqlDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = analyticsUser;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

    }
}
