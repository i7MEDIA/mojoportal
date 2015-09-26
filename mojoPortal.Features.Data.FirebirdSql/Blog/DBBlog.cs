// Author:					Joe Audette
// Created:				    2007-11-03
// Last Modified:			2014-02-10
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 
// Note moved into separate class file from dbPortal 2007-11-03
// 

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
    /// <summary>
    /// 
    /// </summary>
    public static class DBBlog
    {
        

        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }


        public static IDataReader GetBlogs(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT SettingValue ");
            sqlCommand.Append("FROM mp_ModuleSettings ");
            sqlCommand.Append("WHERE SettingName = 'BlogEntriesToShowSetting' ");
            sqlCommand.Append("AND ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsToShow = int.Parse(ConfigurationManager.AppSettings["DefaultBlogPageSize"]);

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    try
                    {
                        rowsToShow = Convert.ToInt32(reader["SettingValue"]);
                    }
                    catch { }

                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST " + rowsToShow.ToString() + " b.*, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND @BeginDate >= b.StartDate  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append(" ;  ");

            arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@BeginDate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetBlogsForFeed(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT SettingValue ");
            sqlCommand.Append("FROM mp_ModuleSettings ");
            sqlCommand.Append("WHERE SettingName = 'MaxFeedItems' ");
            sqlCommand.Append("AND ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsToShow = 20;

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    try
                    {
                        rowsToShow = Convert.ToInt32(reader["SettingValue"]);
                    }
                    catch { }

                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST " + rowsToShow.ToString() + " b.*, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND @BeginDate >= b.StartDate  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append(" ;  ");

            arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@BeginDate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// gets top 20 related posts ordered by created date desc
        /// based on categories of current post itemid
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static IDataReader GetRelatedPosts(int itemId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST 20 b.*, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ItemID <> @ItemID  ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");

            sqlCommand.Append("AND b.ItemID IN (");
            sqlCommand.Append("SELECT ItemID FROM mp_BlogItemCategories WHERE CategoryID IN ( ");
            sqlCommand.Append("SELECT CategoryID FROM mp_BlogItemCategories WHERE ItemID = @ItemID ");
            sqlCommand.Append(") ) ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append(" ;  ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetBlogsForMetaWeblogApi(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT SettingValue ");
            sqlCommand.Append("FROM mp_ModuleSettings ");
            sqlCommand.Append("WHERE SettingName = 'MaxMetaweblogRecentItems' ");
            sqlCommand.Append("AND ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsToShow = 100;

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    try
                    {
                        rowsToShow = Convert.ToInt32(reader["SettingValue"]);
                    }
                    catch { }

                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST " + rowsToShow.ToString() + " b.*, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND @BeginDate >= b.StartDate  ");
            //sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append(" ;  ");

            arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@BeginDate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetBlogCategoriesForMetaWeblogApi(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT SettingValue ");
            sqlCommand.Append("FROM mp_ModuleSettings ");
            sqlCommand.Append("WHERE SettingName = 'MaxMetaweblogRecentItems' ");
            sqlCommand.Append("AND ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsToShow = 100;

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    try
                    {
                        rowsToShow = Convert.ToInt32(reader["SettingValue"]);
                    }
                    catch { }

                }
            }

            sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.ID, ");
            sqlCommand.Append("bic.ItemID, ");
            sqlCommand.Append("bic.CategoryID, ");
            sqlCommand.Append("bc.Category ");

            sqlCommand.Append("FROM mp_BlogItemCategories bic ");

            sqlCommand.Append("JOIN	mp_BlogCategories bc ");
            sqlCommand.Append("ON bc.CategoryID = bic.CategoryID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.ItemID IN (");

            sqlCommand.Append("SELECT FIRST " + rowsToShow.ToString() + " b.ItemID ");
           
            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND @BeginDate >= b.StartDate  ");
            //sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append(")");

            sqlCommand.Append("ORDER BY bc.Category ");
            sqlCommand.Append(" ;  ");

            arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@BeginDate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static int GetCountClosed(
            int moduleId,
            DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Blogs ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID  ");
            sqlCommand.Append("AND EndDate < @CurrentTime  ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetClosed(
            int moduleId,
            DateTime currentTime,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {

            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountClosed(moduleId, currentTime);

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
            sqlCommand.Append("	b.*, ");
            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.AvatarUrl, ");
            sqlCommand.Append("u.AuthorBio ");

            sqlCommand.Append("FROM	mp_Blogs b  ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND b.EndDate < @CurrentTime  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetAttachmentsForClosed(
            int moduleId,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {

            int pageLowerBound = (pageSize * pageNumber) - pageSize;


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.*, ");
            sqlCommand.Append("b2.ShowDownloadLink ");

            sqlCommand.Append("FROM mp_FileAttachment bic ");

            sqlCommand.Append("JOIN mp_Blogs b2 ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b2.BlogGuid = bic.ItemGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.ItemGuid IN (");

            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	b.BlogGuid ");

            sqlCommand.Append("FROM	mp_Blogs b  ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND b.EndDate < @CurrentTime  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");

            sqlCommand.Append(") ");
            //sqlCommand.Append("ORDER BY bc.Category ");

            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetCategoriesForClosed(
            int moduleId,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {

            int pageLowerBound = (pageSize * pageNumber) - pageSize;


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.ID, ");
            sqlCommand.Append("bic.ItemID, ");
            sqlCommand.Append("bic.CategoryID, ");
            sqlCommand.Append("bc.Category ");

            sqlCommand.Append("FROM mp_BlogItemCategories bic ");

            sqlCommand.Append("JOIN	mp_BlogCategories bc ");
            sqlCommand.Append("ON bc.CategoryID = bic.CategoryID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.ItemID IN (");

            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	b.ItemID ");

            sqlCommand.Append("FROM	mp_Blogs b  ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");  
            sqlCommand.Append("AND b.EndDate < @CurrentTime  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");

            sqlCommand.Append(") ");
            sqlCommand.Append("ORDER BY bc.Category ");

            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static int GetCountOfDrafts(
            int moduleId,
            Guid userGuid,
            DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Blogs ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID  ");
            sqlCommand.Append("AND (@UserGuid = '00000000-0000-0000-0000-000000000000' OR UserGuid  = @UserGuid)  ");
            sqlCommand.Append("AND ((StartDate > @CurrentTime) OR (IsPublished = 0)) ");
            
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetPageOfDrafts(
            int moduleId,
            Guid userGuid,
            DateTime currentTime,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {

            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountOfDrafts(moduleId, userGuid, currentTime);

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
            sqlCommand.Append("	b.*, ");
            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.AvatarUrl, ");
            sqlCommand.Append("u.AuthorBio ");

            sqlCommand.Append("FROM	mp_Blogs b  ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND (@UserGuid = '00000000-0000-0000-0000-000000000000' OR b.UserGuid  = @UserGuid)  ");
            sqlCommand.Append("AND ((b.StartDate > @CurrentTime) OR (b.IsPublished = 0)) ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@UserGuid", FbDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }





        public static int GetCount(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Blogs ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID  ");
            sqlCommand.Append("AND @BeginDate >= StartDate  ");
            sqlCommand.Append("AND IsPublished = 1 ");
            sqlCommand.Append("AND StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (EndDate IS NULL OR EndDate > @CurrentTime)  ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@BeginDate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetPage(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {

            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(moduleId, beginDate, currentTime);

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
            sqlCommand.Append("	b.*, ");
            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.AvatarUrl, ");
            sqlCommand.Append("u.AuthorBio ");

            sqlCommand.Append("FROM	mp_Blogs b  ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND @BeginDate >= b.StartDate  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@BeginDate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetAttachmentsForPage(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {

            int pageLowerBound = (pageSize * pageNumber) - pageSize;


            StringBuilder sqlCommand = new StringBuilder();
  
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.*, ");
            sqlCommand.Append("b2.ShowDownloadLink ");

            sqlCommand.Append("FROM mp_FileAttachment bic ");

            sqlCommand.Append("JOIN mp_Blogs b2 ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b2.BlogGuid = bic.ItemGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.ItemGuid IN (");

            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	b.BlogGuid ");

            sqlCommand.Append("FROM	mp_Blogs b  ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND @BeginDate >= b.StartDate  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");

            sqlCommand.Append(") ");
            //sqlCommand.Append("ORDER BY bc.Category ");

            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@BeginDate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetAttachmentsForPage(
            int moduleId,
            int categoryId,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {

            int pageLowerBound = (pageSize * pageNumber) - pageSize;


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.*, ");
            sqlCommand.Append("b2.ShowDownloadLink ");

            sqlCommand.Append("FROM mp_FileAttachment bic ");

            sqlCommand.Append("JOIN mp_Blogs b2 ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b2.BlogGuid = bic.ItemGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.ItemGuid IN (");

            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	b.BlogGuid ");

            sqlCommand.Append("FROM	mp_Blogs b  ");

            sqlCommand.Append("JOIN	mp_BlogItemCategories bic2 ");
            sqlCommand.Append("ON b.ItemID = bic2.ItemID ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");

            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");
            sqlCommand.Append("AND  bic2.CategoryID = @CategoryID   ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");

            sqlCommand.Append(") ");
            //sqlCommand.Append("ORDER BY bc.Category ");

            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@CategoryID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetAttachmentsForPage(
            int month,
            int year,
            int moduleId,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {

            int pageLowerBound = (pageSize * pageNumber) - pageSize;


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.*, ");
            sqlCommand.Append("b2.ShowDownloadLink ");

            sqlCommand.Append("FROM mp_FileAttachment bic ");

            sqlCommand.Append("JOIN mp_Blogs b2 ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b2.BlogGuid = bic.ItemGuid ");

           
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.ItemGuid IN (");

            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	b.BlogGuid ");

            sqlCommand.Append("FROM	mp_Blogs b  ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");
            sqlCommand.Append("AND EXTRACT(YEAR FROM b.STARTDATE) = @Year  ");
            sqlCommand.Append(" AND EXTRACT(MONTH FROM b.STARTDATE)  = @Month  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");

            sqlCommand.Append(") ");
            //sqlCommand.Append("ORDER BY bc.Category ");

            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@Year", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = year;

            arParams[2] = new FbParameter("@Month", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = month;

            arParams[3] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetCategoriesForPage(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {

            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.ID, ");
            sqlCommand.Append("bic.ItemID, ");
            sqlCommand.Append("bic.CategoryID, ");
            sqlCommand.Append("bc.Category ");

            sqlCommand.Append("FROM mp_BlogItemCategories bic ");

            sqlCommand.Append("JOIN	mp_BlogCategories bc ");
            sqlCommand.Append("ON bc.CategoryID = bic.CategoryID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.ItemID IN (");

            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	b.ItemID ");
            
            sqlCommand.Append("FROM	mp_Blogs b  ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND @BeginDate >= b.StartDate  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");

            sqlCommand.Append(") ");
            sqlCommand.Append("ORDER BY bc.Category ");

            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@BeginDate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetCountByMonth(
            int month,
            int year,
            int moduleId,
            DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Blogs ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID  ");
            sqlCommand.Append("AND IsPublished = 1 ");
            sqlCommand.Append("AND StartDate <= @CurrentTime ");
            sqlCommand.Append("AND (EndDate IS NULL OR EndDate > @CurrentTime)  ");

            sqlCommand.Append("AND EXTRACT(YEAR FROM STARTDATE) = @Year  ");
            sqlCommand.Append(" AND EXTRACT(MONTH FROM STARTDATE)  = @Month  ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@Year", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = year;

            arParams[2] = new FbParameter("@Month", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = month;

            arParams[3] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currentTime;

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetCategoriesForPage(
            int month,
            int year,
            int moduleId,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {

            int pageLowerBound = (pageSize * pageNumber) - pageSize;


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.ID, ");
            sqlCommand.Append("bic.ItemID, ");
            sqlCommand.Append("bic.CategoryID, ");
            sqlCommand.Append("bc.Category ");

            sqlCommand.Append("FROM mp_BlogItemCategories bic ");

            sqlCommand.Append("JOIN	mp_BlogCategories bc ");
            sqlCommand.Append("ON bc.CategoryID = bic.CategoryID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.ItemID IN (");

            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	b.ItemID ");

            sqlCommand.Append("FROM	mp_Blogs b  ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");
            sqlCommand.Append("AND EXTRACT(YEAR FROM b.STARTDATE) = @Year  ");
            sqlCommand.Append(" AND EXTRACT(MONTH FROM b.STARTDATE)  = @Month  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");

            sqlCommand.Append(") ");
            sqlCommand.Append("ORDER BY bc.Category ");

            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@Year", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = year;

            arParams[2] = new FbParameter("@Month", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = month;

            arParams[3] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetBlogEntriesByMonth(
            int month,
            int year,
            int moduleId,
            DateTime currentTime,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountByMonth(month, year, moduleId, currentTime);

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
            sqlCommand.Append("	b.*, ");
            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.AvatarUrl, ");
            sqlCommand.Append("u.AuthorBio ");

            sqlCommand.Append("FROM	mp_Blogs b  ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");

            sqlCommand.Append("AND EXTRACT(YEAR FROM b.STARTDATE) = @Year  ");
            sqlCommand.Append(" AND EXTRACT(MONTH FROM b.STARTDATE)  = @Month  ");

            sqlCommand.Append("ORDER BY b.StartDate DESC ;");


            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@Year", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = year;

            arParams[2] = new FbParameter("@Month", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = month;

            arParams[3] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetBlogEntriesByMonth(int month, int year, int moduleId, DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");

            sqlCommand.Append("FROM	mp_Blogs ");

            sqlCommand.Append("WHERE ModuleID = @ModuleID  ");
            sqlCommand.Append("AND IsPublished = 1 ");
            sqlCommand.Append("AND StartDate <= @CurrentTime ");
            sqlCommand.Append("AND (EndDate IS NULL OR EndDate > @CurrentTime)  ");

            sqlCommand.Append("AND EXTRACT(YEAR FROM STARTDATE) = @Year  ");
            sqlCommand.Append(" AND EXTRACT(MONTH FROM STARTDATE)  = @Month  ");

            sqlCommand.Append("ORDER BY StartDate DESC ;");


            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@Year", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = year;

            arParams[2] = new FbParameter("@Month", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = month;

            arParams[3] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetCountByCategory(
            int moduleId,
            int categoryId,
            DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("JOIN	mp_BlogItemCategories bic ");
            sqlCommand.Append("ON b.ItemID = bic.ItemID ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");
            sqlCommand.Append("AND  bic.CategoryID = @CategoryID   ");

            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@CategoryID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetEntriesByCategory(
            int moduleId,
            int categoryId,
            DateTime currentTime,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountByCategory(moduleId, categoryId, currentTime);

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
            sqlCommand.Append("	b.*, ");
            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.AvatarUrl, ");
            sqlCommand.Append("u.AuthorBio ");

            sqlCommand.Append("FROM	mp_Blogs b  ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("JOIN	mp_BlogItemCategories bic ");
            sqlCommand.Append("ON b.ItemID = bic.ItemID ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID   ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");

            sqlCommand.Append("AND  bic.CategoryID = @CategoryID   ");

            sqlCommand.Append("ORDER BY b.StartDate DESC ;  ");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@CategoryID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);



        }

        public static IDataReader GetCategoriesForPage(
            int moduleId,
            int categoryId,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {

            int pageLowerBound = (pageSize * pageNumber) - pageSize;


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.ID, ");
            sqlCommand.Append("bic.ItemID, ");
            sqlCommand.Append("bic.CategoryID, ");
            sqlCommand.Append("bc.Category ");

            sqlCommand.Append("FROM mp_BlogItemCategories bic ");

            sqlCommand.Append("JOIN	mp_BlogCategories bc ");
            sqlCommand.Append("ON bc.CategoryID = bic.CategoryID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.ItemID IN (");

            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	b.ItemID ");

            sqlCommand.Append("FROM	mp_Blogs b  ");

            sqlCommand.Append("JOIN	mp_BlogItemCategories bic2 ");
            sqlCommand.Append("ON b.ItemID = bic2.ItemID ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
     
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");
            sqlCommand.Append("AND  bic2.CategoryID = @CategoryID   ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");

            sqlCommand.Append(") ");
            sqlCommand.Append("ORDER BY bc.Category ");

            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@CategoryID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetEntriesByCategory(int moduleId, int categoryId, DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  b.* ");

            sqlCommand.Append("FROM	mp_Blogs b ");
            sqlCommand.Append("JOIN	mp_BlogItemCategories bic ");
            sqlCommand.Append("ON b.ItemID = bic.ItemID ");
            sqlCommand.Append("WHERE b.ModuleID = @ModuleID   ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");

            sqlCommand.Append("AND  bic.CategoryID = @CategoryID   ");
            sqlCommand.Append("ORDER BY b.StartDate DESC ;  ");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@CategoryID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);



        }

        public static IDataReader GetBlogsForSiteMap(int siteId, DateTime currentUtcDateTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("b.ItemUrl, ");
            sqlCommand.Append("b.LastModUtc, ");
            sqlCommand.Append("b.ItemID, ");
            sqlCommand.Append("b.ModuleID, ");
            sqlCommand.Append("pm.PageID ");


            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("JOIN mp_Modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN mp_PageModules pm ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b.ModuleID = pm.ModuleID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("m.SiteID = @SiteID ");
            sqlCommand.Append("AND b.IncludeInSiteMap = 1 ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND  b.StartDate <= @CurrentDateTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentDateTime)  ");
            sqlCommand.Append("AND b.ItemUrl <> ''  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append(" ;  ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@CurrentDateTime", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentUtcDateTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetBlogsForNewsMap(int siteId, DateTime utcThresholdTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("b.ItemUrl, ");
            sqlCommand.Append("b.LastModUtc, ");
            sqlCommand.Append("b.ItemID, ");
            sqlCommand.Append("b.ModuleID, ");

            sqlCommand.Append("b.HeadlineImageUrl, ");
            sqlCommand.Append("b.PubAccess, ");
            sqlCommand.Append("b.PubGenres, ");
            sqlCommand.Append("b.PubGeoLocations, ");
            sqlCommand.Append("b.PubKeyWords, ");
            sqlCommand.Append("b.PubLanguage,");
            sqlCommand.Append("b.PubName, ");
            sqlCommand.Append("b.PubStockTickers, ");
            sqlCommand.Append("b.StartDate,");
            sqlCommand.Append("b.Title, ");
            sqlCommand.Append("b.Heading, ");
            //sqlCommand.Append("");
            //sqlCommand.Append("");
            //sqlCommand.Append("");




            sqlCommand.Append("pm.PageID ");


            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("JOIN mp_Modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN mp_PageModules pm ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b.ModuleID = pm.ModuleID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("m.SiteID = @SiteID ");
            sqlCommand.Append("AND b.IncludeInNews = 1 ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND  b.StartDate >= @UtcThresholdTime  ");
           
            sqlCommand.Append("AND b.ItemUrl <> ''  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append(" ;  ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@UtcThresholdTime", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = utcThresholdTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }



        public static IDataReader GetDrafts(
            int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Blogs ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID  ");
            sqlCommand.Append("AND ((StartDate > @BeginDate) OR (IsPublished = 0))");
            sqlCommand.Append("ORDER BY  StartDate DESC  ");
            sqlCommand.Append(" ;  ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@BeginDate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }





        public static IDataReader GetBlogsByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  b.*, ");

            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles, ");
            sqlCommand.Append("md.FeatureName, ");

            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.AvatarUrl ");
            

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("JOIN	mp_Modules m ");
            sqlCommand.Append("ON b.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN	mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("JOIN	mp_PageModules pm ");
            sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

            sqlCommand.Append("JOIN	mp_Pages p ");
            sqlCommand.Append("ON p.PageID = pm.PageID ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.SiteID = @SiteID ");
            sqlCommand.Append("AND pm.PageID = @PageID ");
            //sqlCommand.Append("AND pm.PublishBeginDate < now() ");
            //sqlCommand.Append("AND (pm.PublishEndDate IS NULL OR pm.PublishEndDate > now())  ;");
            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetBlogStats(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("EntryCount, ");
            sqlCommand.Append("CommentCount ");

            sqlCommand.Append("FROM	mp_BlogStats ");

            sqlCommand.Append("WHERE ModuleID = @ModuleID  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetBlogMonthArchive(int moduleId, DateTime currentTime)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("EXTRACT(MONTH FROM StartDate) \"MONTH\", ");
            sqlCommand.Append("'' \"MONTHNAME\", ");
            sqlCommand.Append("EXTRACT(YEAR FROM STARTDATE)	\"YEAR\", ");
            sqlCommand.Append("1 \"DAY\", ");
            sqlCommand.Append("COUNT(*)	\"COUNT\" ");

            sqlCommand.Append("FROM	mp_Blogs ");

            sqlCommand.Append("WHERE ModuleID = @ModuleID  ");
            sqlCommand.Append("AND IsPublished = 1 ");
            sqlCommand.Append("AND StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (EndDate IS NULL OR EndDate > @CurrentTime)  ");
            sqlCommand.Append("GROUP BY   ");
            sqlCommand.Append("\"MONTH\", ");
            sqlCommand.Append("\"MONTHNAME\", ");
            sqlCommand.Append("\"YEAR\" ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("\"YEAR\" DESC, ");
            sqlCommand.Append("\"MONTH\" DESC ");
            sqlCommand.Append("; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSingleBlog(int itemId, DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  b.*, ");
            sqlCommand.Append("p.ItemUrl AS PreviousPost, ");
            sqlCommand.Append("p.Heading AS PreviousPostTitle, ");
            sqlCommand.Append("COALESCE(p.ItemID, -1) AS PreviousItemID, ");
            sqlCommand.Append("n.ItemUrl AS NextPost, ");
            sqlCommand.Append("n.Heading AS NextPostTitle, ");
            sqlCommand.Append("COALESCE(n.ItemID, -1) AS NextItemID, ");

            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.AvatarUrl, ");
            sqlCommand.Append("u.AuthorBio ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Blogs p ");
            sqlCommand.Append("ON p.ItemID <>  @ItemID ");
            sqlCommand.Append("AND p.ItemID IN ");
            sqlCommand.Append("(SELECT FIRST 1 x.ItemID ");
            sqlCommand.Append("FROM mp_Blogs x ");
            sqlCommand.Append("WHERE x.ModuleID = b.ModuleID AND x.IsPublished = 1 ");
            sqlCommand.Append("AND x.StartDate <= @CurrentTime ");
            sqlCommand.Append("AND (x.EndDate IS NULL OR x.EndDate > @CurrentTime)  ");
            sqlCommand.Append("AND x.StartDate < b.StartDate ");
            sqlCommand.Append("ORDER BY x.StartDate DESC ) ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Blogs n ");
            sqlCommand.Append("ON n.ItemID <>  @ItemID ");
            sqlCommand.Append("AND n.ItemID IN ");
            sqlCommand.Append("(SELECT FIRST 1 z.ItemID ");
            sqlCommand.Append("FROM mp_Blogs z ");
            sqlCommand.Append("WHERE z.ModuleID = b.ModuleID AND z.IsPublished = 1 ");
            sqlCommand.Append("AND z.StartDate <= @CurrentTime ");
            sqlCommand.Append("AND (z.EndDate IS NULL OR z.EndDate > @CurrentTime)  ");
            sqlCommand.Append("AND z.StartDate > b.StartDate ");
            sqlCommand.Append("ORDER BY z.StartDate ) ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ItemID = @ItemID ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new FbParameter("@CurrentTime", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }



        public static bool DeleteBlog(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Blogs ");
            sqlCommand.Append("WHERE ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByModule(int moduleId)
        {
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogItemCategories ");
            sqlCommand.Append("WHERE ItemID IN (SELECT ItemID FROM mp_Blogs WHERE ModuleID  ");
            sqlCommand.Append(" = @ModuleID ) ");
            sqlCommand.Append(";");


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE PageGuid IN (SELECT BlogGuid FROM mp_Blogs WHERE ModuleID  ");
            sqlCommand.Append(" = @ModuleID ) ");
            sqlCommand.Append(";");


            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogCategories ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogStats ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogComments ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Blogs ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(int siteId)
        {
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogItemCategories ");
            sqlCommand.Append("WHERE ItemID IN (SELECT ItemID FROM mp_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID) ) ");
            sqlCommand.Append(";");
            

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE PageGuid IN (SELECT ModuleGuid FROM mp_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID) ) ");
            sqlCommand.Append(";");


            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE PageGuid IN (SELECT BlogGuid FROM mp_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID) ) ");
            sqlCommand.Append(";");


            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogCategories ");
            sqlCommand.Append("WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID) ");
            sqlCommand.Append(";");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogStats ");
            sqlCommand.Append("WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID) ");
            sqlCommand.Append(";");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogComments ");
            sqlCommand.Append("WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID) ");
            sqlCommand.Append(";");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Blogs ");
            sqlCommand.Append("WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID) ");
            sqlCommand.Append(";");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public static int AddBlog(
            Guid blogGuid,
            Guid moduleGuid,
            int moduleId,
            string userName,
            string title,
            string excerpt,
            string description,
            DateTime startDate,
            bool isInNewsletter,
            bool includeInFeed,
            int allowCommentsForDays,
            string location,
            Guid userGuid,
            DateTime createdDate,
            string itemUrl,
            string metaKeywords,
            string metaDescription,
            string compiledMeta,
            bool isPublished,
            string subTitle,
            DateTime endDate,
            bool approved,
            Guid approvedBy,
            DateTime approvedDate,
            bool showAuthorName,
            bool showAuthorAvatar,
            bool showAuthorBio,
            bool includeInSearch,
            bool useBingMap,
            string mapHeight,
            string mapWidth,
            bool showMapOptions,
            bool showZoomTool,
            bool showLocationInfo,
            bool useDrivingDirections,
            string mapType,
            int mapZoom,
            bool showDownloadLink,
            bool includeInSiteMap,
            bool excludeFromRecentContent,

            bool includeInNews,
            string pubName,
            string pubLanguage,
            string pubAccess,
            string pubGenres,
            string pubKeyWords,
            string pubGeoLocations,
            string pubStockTickers,
            string headlineImageUrl,
            bool includeImageInExcerpt)
        {
            #region Bit Conversion

            int intIsInNewsletter;
            if (isInNewsletter)
            {
                intIsInNewsletter = 1;
            }
            else
            {
                intIsInNewsletter = 0;
            }

            int intIncludeInFeed;
            if (includeInFeed)
            {
                intIncludeInFeed = 1;
            }
            else
            {
                intIncludeInFeed = 0;
            }

            int intIsPublished = 0;
            if (isPublished) { intIsPublished = 1; }

            int intApproved = 0;
            if (approved) { intApproved = 1; }

            int intshowAuthorName = 0;
            if(showAuthorName) {intshowAuthorName = 1;}

            int intshowAuthorAvatar = 0;
            if(showAuthorAvatar) {intshowAuthorAvatar = 1;}

            int intshowAuthorBio = 0;
            if(showAuthorBio) {intshowAuthorBio = 1;}

            int intincludeInSearch = 0;
            if(includeInSearch) {intincludeInSearch = 1;}

            int intuseBingMap = 0;
            if(useBingMap) {intuseBingMap = 1;}

            int intshowMapOptions = 0;
            if(showMapOptions) {intshowMapOptions = 1;}

            int intshowZoomTool = 0;
            if(showZoomTool) {intshowZoomTool = 1;}

            int intshowLocationInfo = 0;
            if(showLocationInfo) {intshowLocationInfo = 1;}

            int intuseDrivingDirections = 0;
            if(useDrivingDirections) {intuseDrivingDirections = 1;}

            int intshowDownloadLink = 0;
            if(showDownloadLink) {intshowDownloadLink = 1;}

            int intincludeInSiteMap = 0;
            if (includeInSiteMap) { intincludeInSiteMap = 1; }

            int intExcludeRecent = 0;
            if (excludeFromRecentContent) { intExcludeRecent = 1; }

            int intincludeInNews = 0;
            if (includeInNews) { intincludeInNews = 1; }

            int intincludeImageInExcerpt = 0;
            if (includeImageInExcerpt) { intincludeImageInExcerpt = 1; }

            #endregion

            FbParameter[] arParams = new FbParameter[52];

            arParams[0] = new FbParameter(":ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter(":CreatedByUser", FbDbType.VarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = string.Empty;

            arParams[2] = new FbParameter(":CreatedDate", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            arParams[3] = new FbParameter(":Heading", FbDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = title;

            arParams[4] = new FbParameter(":Abstract", FbDbType.VarChar);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = excerpt;

            arParams[5] = new FbParameter(":StartDate", FbDbType.TimeStamp);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = startDate;

            arParams[6] = new FbParameter(":IsInNewsletter", FbDbType.SmallInt);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = intIsInNewsletter;

            arParams[7] = new FbParameter(":Description", FbDbType.VarChar);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = description;

            arParams[8] = new FbParameter(":CommentCount", FbDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = 0;

            arParams[9] = new FbParameter(":TrackBackCount", FbDbType.Integer);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = 0;

            arParams[10] = new FbParameter(":IncludeInFeed", FbDbType.SmallInt);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = intIncludeInFeed;

            arParams[11] = new FbParameter(":AllowCommentsForDays", FbDbType.Integer);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = allowCommentsForDays;

            arParams[12] = new FbParameter(":BlogGuid", FbDbType.Char, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = blogGuid.ToString();

            arParams[13] = new FbParameter(":ModuleGuid", FbDbType.Char, 36);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = moduleGuid.ToString();

            arParams[14] = new FbParameter(":Location", FbDbType.VarChar);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = location;

            arParams[15] = new FbParameter(":UserGuid", FbDbType.Char, 36);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = userGuid.ToString();

            arParams[16] = new FbParameter(":ItemUrl", FbDbType.VarChar, 255);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = itemUrl;

            arParams[17] = new FbParameter(":MetaKeywords", FbDbType.VarChar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = metaKeywords;

            arParams[18] = new FbParameter(":MetaDescription", FbDbType.VarChar, 255);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = metaDescription;

            arParams[19] = new FbParameter(":CompiledMeta", FbDbType.VarChar);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = compiledMeta;

            arParams[20] = new FbParameter(":IsPublished", FbDbType.SmallInt);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = intIsPublished;

            arParams[21] = new FbParameter(":SubTitle", FbDbType.VarChar, 500);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = subTitle;

            arParams[22] = new FbParameter(":EndDate", FbDbType.TimeStamp);
            arParams[22].Direction = ParameterDirection.Input;
            if (endDate < DateTime.MaxValue)
            {
                arParams[22].Value = endDate;
            }
            else
            {
                arParams[22].Value = DBNull.Value;
            }

            arParams[23] = new FbParameter(":Approved", FbDbType.SmallInt);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = intApproved;

            arParams[24] = new FbParameter(":ApprovedDate", FbDbType.TimeStamp);
            arParams[24].Direction = ParameterDirection.Input;
            if (approvedDate < DateTime.MaxValue)
            {
                arParams[24].Value = approvedDate;
            }
            else
            {
                arParams[24].Value = DBNull.Value;
            }

            arParams[25] = new FbParameter(":ApprovedBy", FbDbType.Char, 36);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = approvedBy.ToString();

            arParams[26] = new FbParameter(":ShowAuthorName", FbDbType.SmallInt);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = intshowAuthorName;

            arParams[27] = new FbParameter(":ShowAuthorAvatar", FbDbType.SmallInt);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = intshowAuthorAvatar;

            arParams[28] = new FbParameter(":ShowAuthorBio", FbDbType.SmallInt);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = intshowAuthorBio;

            arParams[29] = new FbParameter(":IncludeInSearch", FbDbType.SmallInt);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = intincludeInSearch;

            arParams[30] = new FbParameter(":IncludeInSiteMap", FbDbType.SmallInt);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = intincludeInSiteMap;

            arParams[31] = new FbParameter(":UseBingMap", FbDbType.SmallInt);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = intuseBingMap;

            arParams[32] = new FbParameter(":MapHeight", FbDbType.VarChar, 10);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = mapHeight;

            arParams[33] = new FbParameter(":MapWidth", FbDbType.VarChar, 10);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = mapWidth;

            arParams[34] = new FbParameter(":ShowMapOptions", FbDbType.SmallInt);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = intshowMapOptions;

            arParams[35] = new FbParameter(":ShowZoomTool", FbDbType.SmallInt);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = intshowZoomTool;

            arParams[36] = new FbParameter(":ShowLocationInfo", FbDbType.SmallInt);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = intshowLocationInfo;

            arParams[37] = new FbParameter(":UseDrivingDirections", FbDbType.SmallInt);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = intuseDrivingDirections;

            arParams[38] = new FbParameter(":MapType", FbDbType.VarChar, 20);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = mapType;

            arParams[39] = new FbParameter(":MapZoom", FbDbType.SmallInt);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = mapZoom;

            arParams[40] = new FbParameter(":ShowDownloadLink", FbDbType.SmallInt);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = intshowDownloadLink;

            arParams[41] = new FbParameter(":ExcludeFromRecentContent", FbDbType.SmallInt);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = intExcludeRecent;

            arParams[42] = new FbParameter(":IncludeInNews", FbDbType.SmallInt);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = intincludeInNews;

            arParams[43] = new FbParameter(":PubName", FbDbType.VarChar, 255);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = pubName;

            arParams[44] = new FbParameter(":PubLanguage", FbDbType.VarChar, 7);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = pubLanguage;

            arParams[45] = new FbParameter(":PubAccess", FbDbType.VarChar, 20);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = pubAccess;

            arParams[46] = new FbParameter(":PubGenres", FbDbType.VarChar, 255);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = pubGenres;

            arParams[47] = new FbParameter(":PubKeyWords", FbDbType.VarChar, 255);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = pubKeyWords;

            arParams[48] = new FbParameter(":PubGeoLocations", FbDbType.VarChar, 255);
            arParams[48].Direction = ParameterDirection.Input;
            arParams[48].Value = pubGeoLocations;

            arParams[49] = new FbParameter(":PubStockTickers", FbDbType.VarChar, 255);
            arParams[49].Direction = ParameterDirection.Input;
            arParams[49].Value = pubStockTickers;

            arParams[50] = new FbParameter(":HeadlineImageUrl", FbDbType.VarChar, 255);
            arParams[50].Direction = ParameterDirection.Input;
            arParams[50].Value = headlineImageUrl;

            arParams[51] = new FbParameter(":IncludeImageInExcerpt", FbDbType.SmallInt);
            arParams[51].Direction = ParameterDirection.Input;
            arParams[51].Value = intincludeImageInExcerpt;


            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_BLOGS_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));

            //return newID;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT count(*) FROM mp_BlogStats WHERE ModuleID = @ModuleID ;");

            arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowCount = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            if (rowCount > 0)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("UPDATE mp_BlogStats ");
                sqlCommand.Append("SET EntryCount = EntryCount + 1 ");
                sqlCommand.Append("WHERE ModuleID = @ModuleID ;");

                arParams = new FbParameter[1];

                arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                FBSqlHelper.ExecuteNonQuery(
                    GetConnectionString(),
                    sqlCommand.ToString(),
                    arParams);


            }
            else
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("INSERT INTO mp_BlogStats(ModuleID, ModuleGuid, EntryCount, CommentCount, TrackBackCount) ");
                sqlCommand.Append("VALUES (@ModuleID, @ModuleGuid, 1, 0, 0); ");

                arParams = new FbParameter[2];

                arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                arParams[1] = new FbParameter("@ModuleGuid", FbDbType.Char, 36);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = moduleGuid.ToString();

                FBSqlHelper.ExecuteNonQuery(
                    GetConnectionString(),
                    sqlCommand.ToString(),
                    arParams);


            }

            return newID;

        }




        public static bool UpdateBlog(
            int moduleId,
            int itemId,
            string userName,
            string title,
            string excerpt,
            string description,
            DateTime startDate,
            bool isInNewsletter,
            bool includeInFeed,
            int allowCommentsForDays,
            string location,
            Guid lastModUserGuid,
            DateTime lastModUtc,
            string itemUrl,
            string metaKeywords,
            string metaDescription,
            string compiledMeta,
            bool isPublished,
            string subTitle,
            DateTime endDate,
            bool approved,
            Guid approvedBy,
            DateTime approvedDate,

            bool showAuthorName,
            bool showAuthorAvatar,
            bool showAuthorBio,
            bool includeInSearch,
            bool useBingMap,
            string mapHeight,
            string mapWidth,
            bool showMapOptions,
            bool showZoomTool,
            bool showLocationInfo,
            bool useDrivingDirections,
            string mapType,
            int mapZoom,
            bool showDownloadLink,
            bool includeInSiteMap,
            bool excludeFromRecentContent,

            bool includeInNews,
            string pubName,
            string pubLanguage,
            string pubAccess,
            string pubGenres,
            string pubKeyWords,
            string pubGeoLocations,
            string pubStockTickers,
            string headlineImageUrl,
            bool includeImageInExcerpt)
        {
            #region bit conversion

            string inNews;
            if (isInNewsletter)
            {
                inNews = "1";
            }
            else
            {
                inNews = "0";
            }

            string inFeed;
            if (includeInFeed)
            {
                inFeed = "1";
            }
            else
            {
                inFeed = "0";
            }

            string isPub;
            if (isPublished)
            {
                isPub = "1";
            }
            else
            {
                isPub = "0";
            }

            int intApproved = 0;
            if (approved) { intApproved = 1; }

            int intshowAuthorName = 0;
            if (showAuthorName) { intshowAuthorName = 1; }

            int intshowAuthorAvatar = 0;
            if (showAuthorAvatar) { intshowAuthorAvatar = 1; }

            int intshowAuthorBio = 0;
            if (showAuthorBio) { intshowAuthorBio = 1; }

            int intincludeInSearch = 0;
            if (includeInSearch) { intincludeInSearch = 1; }

            int intuseBingMap = 0;
            if (useBingMap) { intuseBingMap = 1; }

            int intshowMapOptions = 0;
            if (showMapOptions) { intshowMapOptions = 1; }

            int intshowZoomTool = 0;
            if (showZoomTool) { intshowZoomTool = 1; }

            int intshowLocationInfo = 0;
            if (showLocationInfo) { intshowLocationInfo = 1; }

            int intuseDrivingDirections = 0;
            if (useDrivingDirections) { intuseDrivingDirections = 1; }

            int intshowDownloadLink = 0;
            if (showDownloadLink) { intshowDownloadLink = 1; }

            int intincludeInSiteMap = 0;
            if (includeInSiteMap) { intincludeInSiteMap = 1; }

            int intExcludeRecent = 0;
            if (excludeFromRecentContent) { intExcludeRecent = 1; }

            int intincludeInNews = 0;
            if (includeInNews) { intincludeInNews = 1; }

            int intincludeImageInExcerpt = 0;
            if (includeImageInExcerpt) { intincludeImageInExcerpt = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Blogs ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append(" Heading = @Heading, ");
            sqlCommand.Append("Abstract = @Abstract, ");
            sqlCommand.Append("Description = @Description, ");
            sqlCommand.Append("ItemUrl = @ItemUrl, ");
            sqlCommand.Append("IsInNewsletter = " + inNews + ", ");
            sqlCommand.Append("IncludeInFeed = " + inFeed + ", ");
            sqlCommand.Append("IsPublished = " + isPub + ", ");
            sqlCommand.Append("AllowCommentsForDays = @AllowCommentsForDays, ");
            sqlCommand.Append("StartDate = @StartDate,   ");
            sqlCommand.Append("Location = @Location, ");
            sqlCommand.Append("MetaKeywords = @MetaKeywords, ");
            sqlCommand.Append("MetaDescription = @MetaDescription, ");
            sqlCommand.Append("CompiledMeta = @CompiledMeta, ");
            sqlCommand.Append("LastModUserGuid = @LastModUserGuid, ");

            sqlCommand.Append("SubTitle = @SubTitle, ");
            sqlCommand.Append("EndDate = @EndDate, ");
            sqlCommand.Append("Approved = @Approved, ");
            sqlCommand.Append("ApprovedBy = @ApprovedBy, ");
            sqlCommand.Append("ApprovedDate = @ApprovedDate, ");

            sqlCommand.Append("ShowAuthorName = @ShowAuthorName, ");
            sqlCommand.Append("ShowAuthorAvatar = @ShowAuthorAvatar, ");
            sqlCommand.Append("ShowAuthorBio = @ShowAuthorBio, ");
            sqlCommand.Append("IncludeInSearch = @IncludeInSearch, ");
            sqlCommand.Append("IncludeInSiteMap = @IncludeInSiteMap, ");
            sqlCommand.Append("UseBingMap = @UseBingMap, ");
            sqlCommand.Append("MapHeight = @MapHeight, ");
            sqlCommand.Append("MapWidth = @MapWidth, ");
            sqlCommand.Append("ShowMapOptions = @ShowMapOptions, ");
            sqlCommand.Append("ShowZoomTool = @ShowZoomTool, ");
            sqlCommand.Append("ShowLocationInfo = @ShowLocationInfo, ");
            sqlCommand.Append("UseDrivingDirections = @UseDrivingDirections, ");
            sqlCommand.Append("MapType = @MapType, ");
            sqlCommand.Append("MapZoom = @MapZoom, ");
            sqlCommand.Append("ShowDownloadLink = @ShowDownloadLink, ");
            sqlCommand.Append("ExcludeFromRecentContent = @ExcludeFromRecentContent, ");

            sqlCommand.Append("IncludeInNews = @IncludeInNews, ");
            sqlCommand.Append("PubName = @PubName, ");
            sqlCommand.Append("PubLanguage = @PubLanguage, ");
            sqlCommand.Append("PubAccess = @PubAccess, ");
            sqlCommand.Append("PubGenres = @PubGenres, ");
            sqlCommand.Append("PubKeyWords = @PubKeyWords, ");
            sqlCommand.Append("PubGeoLocations = @PubGeoLocations, ");
            sqlCommand.Append("PubStockTickers = @PubStockTickers, ");
            sqlCommand.Append("HeadlineImageUrl = @HeadlineImageUrl, ");
            sqlCommand.Append("IncludeImageInExcerpt = @IncludeImageInExcerpt, ");


            sqlCommand.Append("LastModUtc = @LastModUtc ");

            sqlCommand.Append("WHERE ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[44];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new FbParameter("@Heading", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new FbParameter("@Abstract", FbDbType.VarChar);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = excerpt;

            arParams[3] = new FbParameter("@Description", FbDbType.VarChar);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new FbParameter("@StartDate", FbDbType.TimeStamp);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = startDate;

            arParams[5] = new FbParameter("@AllowCommentsForDays", FbDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = allowCommentsForDays;

            arParams[6] = new FbParameter("@Location", FbDbType.VarChar);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = location;

            arParams[7] = new FbParameter("@LastModUserGuid", FbDbType.Char, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = lastModUserGuid.ToString();

            arParams[8] = new FbParameter("@LastModUtc", FbDbType.TimeStamp);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModUtc;

            arParams[9] = new FbParameter("@ItemUrl", FbDbType.VarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = itemUrl;

            arParams[10] = new FbParameter("@MetaKeywords", FbDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = metaKeywords;

            arParams[11] = new FbParameter("@MetaDescription", FbDbType.VarChar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = metaDescription;

            arParams[12] = new FbParameter("@CompiledMeta", FbDbType.VarChar);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = compiledMeta;

            arParams[13] = new FbParameter("@SubTitle", FbDbType.VarChar, 500);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = subTitle;

            arParams[14] = new FbParameter("@EndDate", FbDbType.TimeStamp);
            arParams[14].Direction = ParameterDirection.Input;
            if (endDate < DateTime.MaxValue)
            {
                arParams[14].Value = endDate;
            }
            else
            {
                arParams[14].Value = DBNull.Value;
            }

            arParams[15] = new FbParameter("@Approved", FbDbType.SmallInt);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = intApproved;

            arParams[16] = new FbParameter("@ApprovedDate", FbDbType.TimeStamp);
            arParams[16].Direction = ParameterDirection.Input;
            if (approvedDate < DateTime.MaxValue)
            {
                arParams[16].Value = approvedDate;
            }
            else
            {
                arParams[16].Value = DBNull.Value;
            }

            arParams[17] = new FbParameter("@ApprovedBy", FbDbType.Char, 36);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = approvedBy.ToString();

            arParams[18] = new FbParameter("@ShowAuthorName", FbDbType.SmallInt);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = intshowAuthorName;

            arParams[19] = new FbParameter("@ShowAuthorAvatar", FbDbType.SmallInt);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = intshowAuthorAvatar;

            arParams[20] = new FbParameter("@ShowAuthorBio", FbDbType.SmallInt);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = intshowAuthorBio;

            arParams[21] = new FbParameter("@IncludeInSearch", FbDbType.SmallInt);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = intincludeInSearch;

            arParams[22] = new FbParameter("@IncludeInSiteMap", FbDbType.SmallInt);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = intincludeInSiteMap;

            arParams[23] = new FbParameter("@UseBingMap", FbDbType.SmallInt);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = intuseBingMap;

            arParams[24] = new FbParameter("@MapHeight", FbDbType.VarChar, 10);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = mapHeight;

            arParams[25] = new FbParameter("@MapWidth", FbDbType.VarChar, 10);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = mapWidth;

            arParams[26] = new FbParameter("@ShowMapOptions", FbDbType.SmallInt);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = intshowMapOptions;

            arParams[27] = new FbParameter("@ShowZoomTool", FbDbType.SmallInt);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = intshowZoomTool;

            arParams[28] = new FbParameter("@ShowLocationInfo", FbDbType.SmallInt);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = intshowLocationInfo;

            arParams[29] = new FbParameter("@UseDrivingDirections", FbDbType.SmallInt);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = intuseDrivingDirections;

            arParams[30] = new FbParameter("@MapType", FbDbType.VarChar, 20);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = mapType;

            arParams[31] = new FbParameter("@MapZoom", FbDbType.SmallInt);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = mapZoom;

            arParams[32] = new FbParameter("@ShowDownloadLink", FbDbType.SmallInt);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = intshowDownloadLink;

            arParams[33] = new FbParameter("@ExcludeFromRecentContent", FbDbType.SmallInt);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = intExcludeRecent;

            arParams[34] = new FbParameter("@IncludeInNews", FbDbType.SmallInt);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = intincludeInNews;

            arParams[35] = new FbParameter("@PubName", FbDbType.VarChar, 255);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = pubName;

            arParams[36] = new FbParameter("@PubLanguage", FbDbType.VarChar, 7);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = pubLanguage;

            arParams[37] = new FbParameter("@PubAccess", FbDbType.VarChar, 20);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = pubAccess;

            arParams[38] = new FbParameter("@PubGenres", FbDbType.VarChar, 255);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = pubGenres;

            arParams[39] = new FbParameter("@PubKeyWords", FbDbType.VarChar, 255);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = pubKeyWords;

            arParams[40] = new FbParameter("@PubGeoLocations", FbDbType.VarChar, 255);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = pubGeoLocations;

            arParams[41] = new FbParameter("@PubStockTickers", FbDbType.VarChar, 255);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = pubStockTickers;

            arParams[42] = new FbParameter("@HeadlineImageUrl", FbDbType.VarChar, 255);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = headlineImageUrl;

            arParams[43] = new FbParameter("@IncludeImageInExcerpt", FbDbType.SmallInt);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = intincludeImageInExcerpt;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdateCommentCount(Guid blogGuid, int commentCount)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Blogs ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("CommentCount = @CommentCount ");
            sqlCommand.Append("WHERE BlogGuid = @BlogGuid ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@BlogGuid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = blogGuid.ToString(); ;

            arParams[1] = new FbParameter("@CommentCount", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = commentCount;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }


        public static bool AddBlogComment(
            int moduleId,
            int itemId,
            string name,
            string title,
            string url,
            string comment,
            DateTime dateCreated)
        {
            FbParameter[] arParams = new FbParameter[7];

            arParams[0] = new FbParameter(":ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter(":ItemID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            arParams[2] = new FbParameter(":Comment", FbDbType.VarChar);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = comment;

            arParams[3] = new FbParameter(":Title", FbDbType.VarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = title;

            arParams[4] = new FbParameter(":Name", FbDbType.VarChar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = name;

            arParams[5] = new FbParameter(":URL", FbDbType.VarChar, 200);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = url;

            arParams[6] = new FbParameter(":DateCreated", FbDbType.TimeStamp);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = dateCreated;

            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_BLOGCOMMENTS_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("Update mp_Blogs ");
            sqlCommand.Append("SET CommentCount = CommentCount + 1 ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID AND ItemID = @ItemID ;");

            arParams = new FbParameter[2];
            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(), arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("Update mp_BlogStats ");
            sqlCommand.Append("SET CommentCount = CommentCount + 1 ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID  ;");

            arParams = new FbParameter[1];
            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(), arParams);

            return (newID > -1);

        }

        public static bool DeleteAllCommentsForBlog(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE   ");
            sqlCommand.Append("FROM	mp_BlogComments ");
            sqlCommand.Append("WHERE ItemID = @ItemID  ");
            sqlCommand.Append("  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static bool UpdateCommentStats(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_BlogStats ");
            sqlCommand.Append("SET 	CommentCount = (SELECT COUNT(*) FROM mp_BlogComments WHERE ModuleID = @ModuleID) ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID  ");
            sqlCommand.Append("  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateEntryStats(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_BlogStats ");
            sqlCommand.Append("SET 	EntryCount = (SELECT COUNT(*) FROM mp_Blogs WHERE ModuleID = @ModuleID) ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID  ");
            sqlCommand.Append("  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public static bool DeleteBlogComment(int blogCommentId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ModuleID, ItemID ");
            sqlCommand.Append("FROM	mp_BlogComments ");

            sqlCommand.Append("WHERE BlogCommentID = @BlogCommentID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@BlogCommentID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = blogCommentId;

            int moduleId = 0;
            int itemId = 0;

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    moduleId = (int)reader["ModuleID"];
                    itemId = (int)reader["ItemID"];
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogComments ");
            sqlCommand.Append("WHERE BlogCommentID = @BlogCommentID ;");

            arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@BlogCommentID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = blogCommentId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            if (moduleId > 0)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("UPDATE mp_Blogs ");
                sqlCommand.Append("SET CommentCount = CommentCount - 1 ");
                sqlCommand.Append("WHERE ModuleID = @ModuleID AND ItemID = @ItemID ;");

                sqlCommand.Append("UPDATE mp_BlogStats ");
                sqlCommand.Append("SET CommentCount = CommentCount - 1 ");
                sqlCommand.Append("WHERE ModuleID = @ModuleID  ;");

                arParams = new FbParameter[2];

                arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                arParams[1] = new FbParameter("@ItemID", FbDbType.Integer);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = itemId;

                FBSqlHelper.ExecuteNonQuery(
                    GetConnectionString(),
                    sqlCommand.ToString(),
                    arParams);


                return (rowsAffected > 0);


            }

            return (rowsAffected > 0);

        }


        public static IDataReader GetBlogComments(int moduleId, int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_BlogComments ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID AND ItemID = @ItemID  ");
            sqlCommand.Append("ORDER BY BlogCommentID,  DateCreated DESC  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }


        public static int AddBlogCategory(
            int moduleId,
            string category)
        {
            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter(":ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter(":Category", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = category;

            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_BLOGCATEGORIES_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));

            return newID;

        }

        public static bool UpdateBlogCategory(
            int categoryId,
            string category)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_BlogCategories ");
            sqlCommand.Append(" SET  ");
            sqlCommand.Append("Category =  @Category   ");

            sqlCommand.Append("WHERE CategoryID = @CategoryID ;    ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@CategoryID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;

            arParams[1] = new FbParameter("@Category", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = category;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteCategory(int categoryId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogItemCategories ");
            sqlCommand.Append("WHERE CategoryID = @CategoryID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@CategoryID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;



            FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogCategories ");
            sqlCommand.Append("WHERE CategoryID = @CategoryID ;");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }



        public static IDataReader GetCategory(int categoryId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_BlogCategories ");
            sqlCommand.Append("WHERE CategoryID = @CategoryID ;  ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@CategoryID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetCategories(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  bc.CategoryID, bc.Category, COUNT(bic.ItemID) As PostCount ");
            sqlCommand.Append("FROM	mp_BlogCategories bc ");
            sqlCommand.Append("JOIN	mp_BlogItemCategories bic ");
            sqlCommand.Append("ON bc.CategoryID = bic.CategoryID ");

            sqlCommand.Append("JOIN	mp_Blogs b ");
            sqlCommand.Append("ON b.ItemID = bic.ItemID ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentDate ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentDate) ");
            
            sqlCommand.Append("GROUP BY bc.CategoryID, bc.Category ");
            sqlCommand.Append("ORDER BY bc.Category; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@CurrentDate", FbDbType.Date);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetCategoriesList(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  bc.CategoryID, bc.Category, COUNT(bic.ItemID) As PostCount ");
            sqlCommand.Append("FROM	mp_BlogCategories bc ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_BlogItemCategories bic ");
            sqlCommand.Append("ON bc.CategoryID = bic.CategoryID ");
            sqlCommand.Append("WHERE bc.ModuleID = @ModuleID  ");
            sqlCommand.Append("GROUP BY bc.CategoryID, bc.Category; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static int AddBlogItemCategory(
            int itemId,
            int categoryId)
        {
            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter(":ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new FbParameter(":CategoryID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_BLOGITEMCATEGORIES_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));

            return newID;

        }


        public static bool DeleteItemCategories(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogItemCategories ");
            sqlCommand.Append("WHERE ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static IDataReader GetBlogItemCategories(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  bic.ItemID, ");
            sqlCommand.Append("bic.CategoryID, ");
            sqlCommand.Append("bc.Category ");
            sqlCommand.Append("FROM	mp_BlogItemCategories bic ");
            sqlCommand.Append("JOIN	mp_BlogCategories bc ");
            sqlCommand.Append("ON bc.CategoryID = bic.CategoryID ");
            sqlCommand.Append("WHERE bic.ItemID = @ItemID   ");
            sqlCommand.Append("ORDER BY bc.Category ;  ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        






    }
}
