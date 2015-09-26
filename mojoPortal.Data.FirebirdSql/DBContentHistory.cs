// Author:					Joe Audette
// Created:					2009-03-31
// Last Modified:			2009-04-07
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using FirebirdSql.Data.FirebirdClient;

namespace mojoPortal.Data
{
    public static class DBContentHistory
    {
        public static String DBPlatform()
        {
            return "FirebirdSql";
        }

        private static String GetReadConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }

        private static String GetWriteConnectionString()
        {
            if (ConfigurationManager.AppSettings["FirebirdWriteConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["FirebirdWriteConnectionString"];
            }

            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }


        /// <summary>
        /// Inserts a row in the mp_ContentHistory table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="contentGuid"> contentGuid </param>
        /// <param name="title"> title </param>
        /// <param name="contentText"> contentText </param>
        /// <param name="customData"> customData </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="historyUtc"> historyUtc </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid userGuid,
            Guid contentGuid,
            string title,
            string contentText,
            string customData,
            DateTime createdUtc,
            DateTime historyUtc)
        {

            FbParameter[] arParams = new FbParameter[9];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid.ToString();

            arParams[3] = new FbParameter("@ContentGuid", FbDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = contentGuid.ToString();

            arParams[4] = new FbParameter("@Title", FbDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = title;

            arParams[5] = new FbParameter("@ContentText", FbDbType.VarChar);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = contentText;

            arParams[6] = new FbParameter("@CustomData", FbDbType.VarChar);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = customData;

            arParams[7] = new FbParameter("@CreatedUtc", FbDbType.TimeStamp);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdUtc;

            arParams[8] = new FbParameter("@HistoryUtc", FbDbType.TimeStamp);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = historyUtc;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_ContentHistory (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("ContentGuid, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("ContentText, ");
            sqlCommand.Append("CustomData, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("HistoryUtc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@ContentGuid, ");
            sqlCommand.Append("@Title, ");
            sqlCommand.Append("@ContentText, ");
            sqlCommand.Append("@CustomData, ");
            sqlCommand.Append("@CreatedUtc, ");
            sqlCommand.Append("@HistoryUtc )");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }


        

        /// <summary>
        /// Deletes a row from the mp_ContentHistory table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes rows from the mp_ContentHistory table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> contentGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByContent(Guid contentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid = @ContentGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ContentGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_ContentHistory table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ch.*, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	mp_ContentHistory ch ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.UserGuid = ch.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ch.Guid = @Guid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_ContentHistory table.
        /// </summary>
        public static int GetCount(Guid contentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_ContentHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid = @ContentGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ContentGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        


        /// <summary>
        /// Gets a page of data from the mp_ContentHistory table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid contentGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(contentGuid);

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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }

            sqlCommand.Append(" ch.*, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	mp_ContentHistory ch ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.UserGuid = ch.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ch.ContentGuid = @ContentGuid ");
            
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("ch.HistoryUtc DESC ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ContentGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

           

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }
    }
}
