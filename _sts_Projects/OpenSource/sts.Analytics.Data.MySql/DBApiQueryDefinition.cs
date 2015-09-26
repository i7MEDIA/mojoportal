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
    public static class DBApiQueryDefinition
    {
        /// <summary>
        /// Inserts a row in the sts_ga_ApiQueryDefinition table. Returns rows affected count.
        /// </summary>
        /// <param name="queryDefId"> queryDefId </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="profileId"> profileId </param>
        /// <param name="title"> title </param>
        /// <param name="segmentId"> segmentId </param>
        /// <param name="metrics"> metrics </param>
        /// <param name="dimensions"> dimensions </param>
        /// <param name="filters"> filters </param>
        /// <param name="sort"> sort </param>
        /// <param name="beginDate"> beginDate </param>
        /// <param name="endDate"> endDate </param>
        /// <param name="maxRows"> maxRows </param>
        /// <param name="sartIndex"> sartIndex </param>
        /// <param name="about"> about </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <returns>int</returns>
        public static int Create(
            Guid queryDefId,
            Guid siteGuid,
            string profileId,
            string title,
            string segmentId,
            string metrics,
            string dimensions,
            string filters,
            string sort,
            DateTime beginDate,
            DateTime endDate,
             byte dateMode,
            int beginDateOffset,
            int endDateOffset,
            int maxRows,
            int startIndex,
            string about,
            Guid createdBy,
            DateTime createdUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_ga_ApiQueryDefinition (");
            sqlCommand.Append("QueryDefId, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ProfileId, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("SegmentId, ");
            sqlCommand.Append("Metrics, ");
            sqlCommand.Append("Dimensions, ");
            sqlCommand.Append("Filters, ");
            sqlCommand.Append("Sort, ");
            sqlCommand.Append("BeginDate, ");
            sqlCommand.Append("EndDate, ");
            sqlCommand.Append("DateMode, ");
            sqlCommand.Append("BeginDateOffset, ");
            sqlCommand.Append("EndDateOffset, ");
            sqlCommand.Append("MaxRows, ");
            sqlCommand.Append("StartIndex, ");
            sqlCommand.Append("About, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("LastModBy, ");
            sqlCommand.Append("LastModUtc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?QueryDefId, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?ProfileId, ");
            sqlCommand.Append("?Title, ");
            sqlCommand.Append("?SegmentId, ");
            sqlCommand.Append("?Metrics, ");
            sqlCommand.Append("?Dimensions, ");
            sqlCommand.Append("?Filters, ");
            sqlCommand.Append("?Sort, ");
            sqlCommand.Append("?BeginDate, ");
            sqlCommand.Append("?EndDate, ");
            sqlCommand.Append("?DateMode, ");
            sqlCommand.Append("?BeginDateOffset, ");
            sqlCommand.Append("?EndDateOffset, ");
            sqlCommand.Append("?MaxRows, ");
            sqlCommand.Append("?StartIndex, ");
            sqlCommand.Append("?About, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?CreatedUtc, ");
            sqlCommand.Append("?LastModBy, ");
            sqlCommand.Append("?LastModUtc )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[21];

            arParams[0] = new MySqlParameter("?QueryDefId", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = queryDefId.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = profileId;

            arParams[3] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = title;

            arParams[4] = new MySqlParameter("?SegmentId", MySqlDbType.VarChar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = segmentId;

            arParams[5] = new MySqlParameter("?Metrics", MySqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = metrics;

            arParams[6] = new MySqlParameter("?Dimensions", MySqlDbType.Text);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = dimensions;

            arParams[7] = new MySqlParameter("?Filters", MySqlDbType.Text);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = filters;

            arParams[8] = new MySqlParameter("?Sort", MySqlDbType.Text);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = sort;

            arParams[9] = new MySqlParameter("?BeginDate", MySqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = beginDate;

            arParams[10] = new MySqlParameter("?EndDate", MySqlDbType.DateTime);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = endDate;

            arParams[11] = new MySqlParameter("?MaxRows", MySqlDbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = maxRows;

            arParams[12] = new MySqlParameter("?StartIndex", MySqlDbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = startIndex;

            arParams[13] = new MySqlParameter("?About", MySqlDbType.Text);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = about;

            arParams[14] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = createdBy.ToString();

            arParams[15] = new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = createdUtc;

            arParams[16] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = createdBy.ToString();

            arParams[17] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = createdUtc;

            arParams[18] = new MySqlParameter("?DateMode", MySqlDbType.Int16);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = dateMode;

            arParams[19] = new MySqlParameter("?BeginDateOffset", MySqlDbType.Int32);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = beginDateOffset;

            arParams[20] = new MySqlParameter("?EndDateOffset", MySqlDbType.Int32);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = endDateOffset;


            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;
        }

        /// <summary>
        /// Updates a row in the sts_ga_ApiQueryDefinition table. Returns true if row updated.
        /// </summary>
        /// <param name="queryDefId"> queryDefId </param>
        /// <param name="profileId"> profileId </param>
        /// <param name="title"> title </param>
        /// <param name="segmentId"> segmentId </param>
        /// <param name="metrics"> metrics </param>
        /// <param name="dimensions"> dimensions </param>
        /// <param name="filters"> filters </param>
        /// <param name="sort"> sort </param>
        /// <param name="beginDate"> beginDate </param>
        /// <param name="endDate"> endDate </param>
        /// <param name="maxRows"> maxRows </param>
        /// <param name="sartIndex"> sartIndex </param>
        /// <param name="about"> about </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid queryDefId,
            string profileId,
            string title,
            string segmentId,
            string metrics,
            string dimensions,
            string filters,
            string sort,
            DateTime beginDate,
            DateTime endDate,
             byte dateMode,
            int beginDateOffset,
            int endDateOffset,
            int maxRows,
            int startIndex,
            string about,
            Guid lastModBy,
            DateTime lastModUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_ga_ApiQueryDefinition ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("ProfileId = ?ProfileId, ");
            sqlCommand.Append("Title = ?Title, ");
            sqlCommand.Append("SegmentId = ?SegmentId, ");
            sqlCommand.Append("Metrics = ?Metrics, ");
            sqlCommand.Append("Dimensions = ?Dimensions, ");
            sqlCommand.Append("Filters = ?Filters, ");
            sqlCommand.Append("Sort = ?Sort, ");
            sqlCommand.Append("BeginDate = ?BeginDate, ");
            sqlCommand.Append("EndDate = ?EndDate, ");
            sqlCommand.Append("DateMode = ?DateMode, ");
            sqlCommand.Append("BeginDateOffset = ?BeginDateOffset, ");
            sqlCommand.Append("EndDateOffset = ?EndDateOffset, ");
            sqlCommand.Append("MaxRows = ?MaxRows, ");
            sqlCommand.Append("StartIndex = ?StartIndex, ");
            sqlCommand.Append("About = ?About, ");
            
            sqlCommand.Append("LastModBy = ?LastModBy, ");
            sqlCommand.Append("LastModUtc = ?LastModUtc ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("QueryDefId = ?QueryDefId ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[18];

            arParams[0] = new MySqlParameter("?QueryDefId", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = queryDefId.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            arParams[2] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new MySqlParameter("?SegmentId", MySqlDbType.VarChar, 50);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = segmentId;

            arParams[4] = new MySqlParameter("?Metrics", MySqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = metrics;

            arParams[5] = new MySqlParameter("?Dimensions", MySqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = dimensions;

            arParams[6] = new MySqlParameter("?Filters", MySqlDbType.Text);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = filters;

            arParams[7] = new MySqlParameter("?Sort", MySqlDbType.Text);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = sort;

            arParams[8] = new MySqlParameter("?BeginDate", MySqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = beginDate;

            arParams[9] = new MySqlParameter("?EndDate", MySqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = endDate;

            arParams[10] = new MySqlParameter("?MaxRows", MySqlDbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = maxRows;

            arParams[11] = new MySqlParameter("?StartIndex", MySqlDbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = startIndex;

            arParams[12] = new MySqlParameter("?About", MySqlDbType.Text);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = about;

            arParams[13] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = lastModBy.ToString();

            arParams[14] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = lastModUtc;

            arParams[15] = new MySqlParameter("?DateMode", MySqlDbType.Int16);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = dateMode;

            arParams[16] = new MySqlParameter("?BeginDateOffset", MySqlDbType.Int32);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = beginDateOffset;

            arParams[17] = new MySqlParameter("?EndDateOffset", MySqlDbType.Int32);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = endDateOffset;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        /// <summary>
        /// Deletes a row from the sts_ga_ApiQueryDefinition table. Returns true if row deleted.
        /// </summary>
        /// <param name="queryDefId"> queryDefId </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid queryDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_ga_ApiQueryDefinition ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QueryDefId = ?QueryDefId ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?QueryDefId", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = queryDefId.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_ga_ApiQueryDefinition ");
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

        /// <summary>
        /// Gets an IDataReader with one row from the sts_ga_ApiQueryDefinition table.
        /// </summary>
        /// <param name="queryDefId"> queryDefId </param>
        public static IDataReader GetOne(Guid queryDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_ga_ApiQueryDefinition ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QueryDefId = ?QueryDefId ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?QueryDefId", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = queryDefId.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the sts_ga_ApiQueryDefinition table.
        /// </summary>
        public static int GetCount(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_ga_ApiQueryDefinition ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        public static IDataReader GetPage(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(siteGuid);

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
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	sts_ga_ApiQueryDefinition  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("Title  ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


    }
}
