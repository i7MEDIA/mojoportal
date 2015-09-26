// Author:					Joe Audette
// Created:				    2010-07-02
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

using System;
using System.Globalization;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
//using log4net;

//examples of the new fetch offset syntax, note thatan ORDER BY clause must be used or an error happens
//SELECT * FROM mp_GeoCountry ORDER BY Name OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;
//http://technet.microsoft.com/en-us/library/ms171807(SQL.100).aspx
//http://technet.microsoft.com/en-us/library/ms173318(SQL.100).aspx
//http://msdn.microsoft.com/en-us/library/bb896140.aspx
//http://msdn.microsoft.com/en-us/library/bb686896.aspx

namespace mojoPortal.Data
{
    public static class DBBlog
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(DBBlog));

        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

        
        public static int GetBlogPageSize(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT SettingValue ");
            sqlCommand.Append("FROM mp_ModuleSettings ");
            sqlCommand.Append("WHERE SettingName = 'BlogEntriesToShowSetting' ");
            sqlCommand.Append("AND ModuleID = @ModuleID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsToShow = 10;

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            return rowsToShow;

        }

        public static IDataReader GetBlogs(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            int rowsToShow = GetBlogPageSize(moduleId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(" + rowsToShow.ToString() + ") b.*, ");
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
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@BeginDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("SELECT TOP (20) b.*, ");
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

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static int GetBlogFeedSize(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT SettingValue ");
            sqlCommand.Append("FROM mp_ModuleSettings ");
            sqlCommand.Append("WHERE SettingName = 'MaxFeedItems' ");
            sqlCommand.Append("AND ModuleID = @ModuleID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsToShow = 20;

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            return rowsToShow;

        }

        public static IDataReader GetBlogsForFeed(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            int rowsToShow = GetBlogFeedSize(moduleId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(" + rowsToShow.ToString() + ") b.*, ");
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
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@BeginDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetMetaWeblogMaxItemsSetting(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT SettingValue ");
            sqlCommand.Append("FROM mp_ModuleSettings ");
            sqlCommand.Append("WHERE SettingName = 'MaxMetaweblogRecentItems' ");
            sqlCommand.Append("AND ModuleID = @ModuleID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsToShow = 100;

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            return rowsToShow;

        }

        public static IDataReader GetBlogsForMetaWeblogApi(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            int rowsToShow = GetMetaWeblogMaxItemsSetting(moduleId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(" + rowsToShow.ToString() + ") b.*, ");
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
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@BeginDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetBlogCategoriesForMetaWeblogApi(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            int rowsToShow = GetMetaWeblogMaxItemsSetting(moduleId);

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

            sqlCommand.Append("SELECT TOP(" + rowsToShow.ToString() + ") b.ItemID ");
            
            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND @BeginDate >= b.StartDate  ");
            //sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY bc.Category ");
            sqlCommand.Append(" ;  ");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@BeginDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
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

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT b.*, ");
            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.AvatarUrl, ");
            sqlCommand.Append("u.AuthorBio ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND b.EndDate < @CurrentTime  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetAttachmentsForClosed(
            int moduleId,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

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

            sqlCommand.Append("SELECT b.BlogGuid ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND b.EndDate < @CurrentTime  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(") ");

          
            sqlCommand.Append(" ;  ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetCategoriesForClosed(
            int moduleId,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

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

            sqlCommand.Append("SELECT b.ItemID ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
           
            sqlCommand.Append("AND b.EndDate < @CurrentTime  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY bc.Category ");
            sqlCommand.Append(" ;  ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
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

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT b.*, ");
            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.AvatarUrl, ");
            sqlCommand.Append("u.AuthorBio ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND (@UserGuid = '00000000-0000-0000-0000-000000000000' OR b.UserGuid  = @UserGuid)  ");
            sqlCommand.Append("AND ((b.StartDate > @CurrentTime) OR (b.IsPublished = 0)) ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid;

            arParams[2] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@BeginDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
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

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT b.*, ");
            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.AvatarUrl, ");
            sqlCommand.Append("u.AuthorBio ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND @BeginDate >= b.StartDate  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");
            
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@BeginDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

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

            sqlCommand.Append("SELECT b.BlogGuid ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND @BeginDate >= b.StartDate  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(") ");

            //sqlCommand.Append("ORDER BY bc.Category ");
            sqlCommand.Append(" ;  ");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@BeginDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

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

            sqlCommand.Append("SELECT b.BlogGuid ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("JOIN	mp_BlogItemCategories bic2 ");
            sqlCommand.Append("ON b.ItemID = bic2.ItemID ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND  bic2.CategoryID = @CategoryID   ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(") ");

            //sqlCommand.Append("ORDER BY bc.Category ");
            sqlCommand.Append(" ;  ");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CategoryID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

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

            sqlCommand.Append("SELECT b.BlogGuid ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");
            sqlCommand.Append("AND DatePart(year, b.StartDate) = @Year  ");
            sqlCommand.Append("AND DatePart(month, b.StartDate) = @Month  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(") ");

            //sqlCommand.Append("ORDER BY bc.Category ");
            sqlCommand.Append(" ;  ");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            arParams[2] = new SqlCeParameter("@Year", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = year;

            arParams[3] = new SqlCeParameter("@Month", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = month;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            
            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

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

            sqlCommand.Append("SELECT b.ItemID ");
            
            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND @BeginDate >= b.StartDate  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY bc.Category ");
            sqlCommand.Append(" ;  ");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@BeginDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("AND DatePart(year, StartDate) = @Year  ");
            sqlCommand.Append("AND DatePart(month, StartDate) = @Month  ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            arParams[2] = new SqlCeParameter("@Year", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = year;

            arParams[3] = new SqlCeParameter("@Month", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = month;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
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

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

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

            sqlCommand.Append("SELECT b.ItemID ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");
            sqlCommand.Append("AND DatePart(year, b.StartDate) = @Year  ");
            sqlCommand.Append("AND DatePart(month, b.StartDate) = @Month  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY bc.Category ");
            sqlCommand.Append(" ;  ");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            arParams[2] = new SqlCeParameter("@Year", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = year;

            arParams[3] = new SqlCeParameter("@Month", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = month;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT b.*, ");
            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.AvatarUrl, ");
            sqlCommand.Append("u.AuthorBio ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");
            sqlCommand.Append("AND DatePart(year, b.StartDate) = @Year  ");
            sqlCommand.Append("AND DatePart(month, b.StartDate) = @Month  ");

            sqlCommand.Append("ORDER BY b.StartDate DESC ");

            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            arParams[2] = new SqlCeParameter("@Year", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = year;

            arParams[3] = new SqlCeParameter("@Month", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = month;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("AND DatePart(year, StartDate) = @Year  ");
            sqlCommand.Append("AND DatePart(month, StartDate) = @Month  ");

            sqlCommand.Append("ORDER BY StartDate DESC ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            arParams[2] = new SqlCeParameter("@Year", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = year;

            arParams[3] = new SqlCeParameter("@Month", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = month;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID   ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");
            sqlCommand.Append("AND  bic.CategoryID = @CategoryID   ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CategoryID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
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

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT b.*, ");
            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.AvatarUrl, ");
            sqlCommand.Append("u.AuthorBio ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("JOIN	mp_BlogItemCategories bic ");
            sqlCommand.Append("ON b.ItemID = bic.ItemID ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID   ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");
            sqlCommand.Append("AND  bic.CategoryID = @CategoryID   ");

            sqlCommand.Append("ORDER BY b.StartDate DESC   ");

            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            arParams[2] = new SqlCeParameter("@CategoryID", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = categoryId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

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

            sqlCommand.Append("SELECT b.ItemID ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("JOIN	mp_BlogItemCategories bic2 ");
            sqlCommand.Append("ON b.ItemID = bic2.ItemID ");

            sqlCommand.Append("WHERE b.ModuleID = @ModuleID  ");
            sqlCommand.Append("AND  bic2.CategoryID = @CategoryID   ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY bc.Category ");
            sqlCommand.Append(" ;  ");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CategoryID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            arParams[2] = new SqlCeParameter("@CategoryID", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = categoryId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("AND b.StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime)  ");
            sqlCommand.Append("AND b.ItemUrl <> ''  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append(" ;  ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentUtcDateTime;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("AND b.StartDate >= @UtcThresholdTime  ");
            
            sqlCommand.Append("AND b.ItemUrl <> ''  ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append(" ;  ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@UtcThresholdTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = utcThresholdTime;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetDrafts(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Blogs ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID  ");
            sqlCommand.Append("AND ((StartDate > @BeginDate) OR (IsPublished = 0))  ");
            sqlCommand.Append("ORDER BY  StartDate DESC  ");
            sqlCommand.Append(" ;  ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@BeginDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            sqlCommand.Append(" ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            sqlCommand.Append("WHERE ModuleID = @ModuleID");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            
            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetBlogMonthArchive(int moduleId, DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("DatePart(month, StartDate) As Month, ");
            sqlCommand.Append("DateName(month, StartDate) As MonthName, ");
            sqlCommand.Append("DatePart(year, StartDate) As Year, ");
            sqlCommand.Append("1 AS Day, ");
            sqlCommand.Append("count(*) AS Count ");

            sqlCommand.Append("FROM	mp_Blogs ");

            sqlCommand.Append("WHERE ModuleID = @ModuleID  ");
            sqlCommand.Append("AND IsPublished = 1 ");
            sqlCommand.Append("AND StartDate <= @CurrentTime  ");
            sqlCommand.Append("AND (EndDate IS NULL OR EndDate > @CurrentTime)  ");

            sqlCommand.Append("GROUP BY DatePart(year, StartDate),  ");
            sqlCommand.Append("DatePart(month, StartDate),  ");
            sqlCommand.Append("DateName(month, StartDate)  ");

            sqlCommand.Append("ORDER BY DatePart(year, StartDate) desc, DatePart(month, StartDate)  desc ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        

        

        public static IDataReader GetSingleBlog(int itemId, DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  c.*, ");
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

            sqlCommand.Append("FROM	mp_Blogs c ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Blogs p ");
            sqlCommand.Append("ON p.ItemID <> c.ItemID ");
            sqlCommand.Append("AND p.ItemID IN ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP(1) ItemID FROM mp_Blogs ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = c.ModuleID AND IsPublished = 1 ");
            sqlCommand.Append("AND StartDate <= @CurrentTime ");
            sqlCommand.Append("AND (EndDate IS NULL OR EndDate > @CurrentTime)  ");
            sqlCommand.Append("AND StartDate < c.StartDate ");
            sqlCommand.Append("AND ItemUrl IS NOT NULL AND ItemUrl <> '' ");
            sqlCommand.Append("ORDER BY StartDate DESC ");
            sqlCommand.Append(") ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Blogs n ");
            sqlCommand.Append("ON n.ItemID <> c.ItemID ");
            sqlCommand.Append("AND n.ItemID IN ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP(1) ItemID FROM mp_Blogs ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = c.ModuleID AND IsPublished = 1 ");
            sqlCommand.Append("AND StartDate <= @CurrentTime ");
            sqlCommand.Append("AND (EndDate IS NULL OR EndDate > @CurrentTime)  ");
            sqlCommand.Append("AND StartDate > c.StartDate ");
            sqlCommand.Append("AND ItemUrl IS NOT NULL AND ItemUrl <> '' ");
            sqlCommand.Append("ORDER BY StartDate ");
            sqlCommand.Append(") ");


            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON c.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE c.ItemID = @ItemID ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            //log.Info(sqlCommand.ToString());

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static bool DeleteBlog(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Blogs ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Blogs ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogItemCategories ");
            sqlCommand.Append("WHERE ItemID IN (SELECT ItemID FROM mp_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID) ) ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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

            StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("INSERT INTO mp_Blogs ");
			sqlCommand.Append("(");
			sqlCommand.Append("ModuleID, ");
			sqlCommand.Append("CreatedByUser, ");
			sqlCommand.Append("CreatedDate, ");
			sqlCommand.Append("Title, ");
			sqlCommand.Append("StartDate, ");
			sqlCommand.Append("IsInNewsletter, ");
			sqlCommand.Append("Description, ");
			sqlCommand.Append("CommentCount, ");
			sqlCommand.Append("TrackBackCount, ");
	
			sqlCommand.Append("IncludeInFeed, ");
			sqlCommand.Append("AllowCommentsForDays, ");
			sqlCommand.Append("BlogGuid, ");
			sqlCommand.Append("ModuleGuid, ");
			sqlCommand.Append("Location, ");
			sqlCommand.Append("UserGuid, ");
			sqlCommand.Append("LastModUserGuid, ");
			sqlCommand.Append("LastModUtc, ");
			sqlCommand.Append("ItemUrl, ");
			sqlCommand.Append("Heading, ");
			sqlCommand.Append("MetaKeywords, ");
			sqlCommand.Append("MetaDescription, ");
			sqlCommand.Append("Abstract, ");
			sqlCommand.Append("CompiledMeta, ");

            sqlCommand.Append("SubTitle, ");
            sqlCommand.Append("EndDate, ");
            sqlCommand.Append("Approved, ");
            sqlCommand.Append("ApprovedBy, ");
            sqlCommand.Append("ApprovedDate, ");

            sqlCommand.Append("ShowAuthorName, ");
            sqlCommand.Append("ShowAuthorAvatar, ");
            sqlCommand.Append("ShowAuthorBio, ");
            sqlCommand.Append("IncludeInSearch, ");
            sqlCommand.Append("IncludeInSiteMap, ");
            sqlCommand.Append("UseBingMap, ");
            sqlCommand.Append("MapHeight, ");
            sqlCommand.Append("MapWidth, ");
            sqlCommand.Append("ShowMapOptions, ");
            sqlCommand.Append("ShowZoomTool, ");
            sqlCommand.Append("ShowLocationInfo, ");
            sqlCommand.Append("UseDrivingDirections, ");
            sqlCommand.Append("MapType, ");
            sqlCommand.Append("MapZoom, ");
            sqlCommand.Append("ShowDownloadLink, ");
            sqlCommand.Append("ExcludeFromRecentContent, ");

            sqlCommand.Append("IncludeInNews, ");
            sqlCommand.Append("PubName, ");
            sqlCommand.Append("PubLanguage, ");
            sqlCommand.Append("PubAccess, ");
            sqlCommand.Append("PubGenres, ");
            sqlCommand.Append("PubKeyWords, ");
            sqlCommand.Append("PubGeoLocations, ");
            sqlCommand.Append("PubStockTickers, ");
            sqlCommand.Append("HeadlineImageUrl, ");
            sqlCommand.Append("IncludeImageInExcerpt, ");

			sqlCommand.Append("IsPublished "); 
			sqlCommand.Append(")");
			
			sqlCommand.Append(" VALUES ");
			sqlCommand.Append("(");
			sqlCommand.Append("@ModuleID, ");
			sqlCommand.Append("@CreatedByUser, ");
			sqlCommand.Append("@CreatedDate, ");
			sqlCommand.Append("@Title, ");
			sqlCommand.Append("@StartDate, ");
			sqlCommand.Append("@IsInNewsletter, ");
			sqlCommand.Append("@Description, ");
			sqlCommand.Append("@CommentCount, ");
			sqlCommand.Append("@TrackBackCount, ");

			sqlCommand.Append("@IncludeInFeed, ");
			sqlCommand.Append("@AllowCommentsForDays, ");
			sqlCommand.Append("@BlogGuid, ");
			sqlCommand.Append("@ModuleGuid, ");
			sqlCommand.Append("@Location, ");
			sqlCommand.Append("@UserGuid, ");
			sqlCommand.Append("@LastModUserGuid, ");
			sqlCommand.Append("@LastModUtc, ");
			sqlCommand.Append("@ItemUrl, ");
			sqlCommand.Append("@Heading, ");
			sqlCommand.Append("@MetaKeywords, ");
			sqlCommand.Append("@MetaDescription, ");
			sqlCommand.Append("@Abstract, ");
			sqlCommand.Append("@CompiledMeta, ");
            sqlCommand.Append("@SubTitle, ");
            sqlCommand.Append("@EndDate, ");
            sqlCommand.Append("@Approved, ");
            sqlCommand.Append("@ApprovedBy, ");
            sqlCommand.Append("@ApprovedDate, ");

            sqlCommand.Append("@ShowAuthorName, ");
            sqlCommand.Append("@ShowAuthorAvatar, ");
            sqlCommand.Append("@ShowAuthorBio, ");
            sqlCommand.Append("@IncludeInSearch, ");
            sqlCommand.Append("@IncludeInSiteMap, ");
            sqlCommand.Append("@UseBingMap, ");
            sqlCommand.Append("@MapHeight, ");
            sqlCommand.Append("@MapWidth, ");
            sqlCommand.Append("@ShowMapOptions, ");
            sqlCommand.Append("@ShowZoomTool, ");
            sqlCommand.Append("@ShowLocationInfo, ");
            sqlCommand.Append("@UseDrivingDirections, ");
            sqlCommand.Append("@MapType, ");
            sqlCommand.Append("@MapZoom, ");
            sqlCommand.Append("@ShowDownloadLink, ");
            sqlCommand.Append("@ExcludeFromRecentContent, ");

            sqlCommand.Append("@IncludeInNews, ");
            sqlCommand.Append("@PubName, ");
            sqlCommand.Append("@PubLanguage, ");
            sqlCommand.Append("@PubAccess, ");
            sqlCommand.Append("@PubGenres, ");
            sqlCommand.Append("@PubKeyWords, ");
            sqlCommand.Append("@PubGeoLocations, ");
            sqlCommand.Append("@PubStockTickers, ");
            sqlCommand.Append("@HeadlineImageUrl, ");
            sqlCommand.Append("@IncludeImageInExcerpt, ");

			sqlCommand.Append("@IsPublished "); 
			sqlCommand.Append(")");
			sqlCommand.Append(";");
	
			SqlCeParameter[] arParams = new SqlCeParameter[55];
			
			arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;
			
			arParams[1] = new SqlCeParameter("@CreatedByUser", SqlDbType.NVarChar, 100);
			arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userName;
			
			arParams[2] = new SqlCeParameter("@CreatedDate", SqlDbType.DateTime);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = createdDate;
			
			arParams[3] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 100);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = title;
			
			arParams[4] = new SqlCeParameter("@StartDate", SqlDbType.DateTime);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = startDate;
			
			arParams[5] = new SqlCeParameter("@IsInNewsletter", SqlDbType.Bit);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = isInNewsletter;
			
			arParams[6] = new SqlCeParameter("@Description", SqlDbType.NText);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = description;
			
			arParams[7] = new SqlCeParameter("@CommentCount", SqlDbType.Int);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = 0;
			
			arParams[8] = new SqlCeParameter("@TrackBackCount", SqlDbType.Int);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = 0;
			
			arParams[9] = new SqlCeParameter("@IncludeInFeed", SqlDbType.Bit);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = includeInFeed;
			
			arParams[10] = new SqlCeParameter("@AllowCommentsForDays", SqlDbType.Int);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = allowCommentsForDays;
			
			arParams[11] = new SqlCeParameter("@BlogGuid", SqlDbType.UniqueIdentifier);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = blogGuid;
			
			arParams[12] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = moduleGuid;
			
			arParams[13] = new SqlCeParameter("@Location", SqlDbType.NText);
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = location;
			
			arParams[14] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
			arParams[14].Direction = ParameterDirection.Input;
			arParams[14].Value = userGuid;
			
			arParams[15] = new SqlCeParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier);
			arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = userGuid;
			
			arParams[16] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
			arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = createdDate;
			
			arParams[17] = new SqlCeParameter("@ItemUrl", SqlDbType.NVarChar, 255);
			arParams[17].Direction = ParameterDirection.Input;
			arParams[17].Value = itemUrl;
			
			arParams[18] = new SqlCeParameter("@Heading", SqlDbType.NVarChar, 255);
			arParams[18].Direction = ParameterDirection.Input;
			arParams[18].Value = title;
			
			arParams[19] = new SqlCeParameter("@MetaKeywords", SqlDbType.NVarChar, 255);
			arParams[19].Direction = ParameterDirection.Input;
			arParams[19].Value = metaKeywords;
			
			arParams[20] = new SqlCeParameter("@MetaDescription", SqlDbType.NVarChar, 255);
			arParams[20].Direction = ParameterDirection.Input;
			arParams[20].Value = metaDescription;
			
			arParams[21] = new SqlCeParameter("@Abstract", SqlDbType.NText);
			arParams[21].Direction = ParameterDirection.Input;
			arParams[21].Value = excerpt;
			
			arParams[22] = new SqlCeParameter("@CompiledMeta", SqlDbType.NText);
			arParams[22].Direction = ParameterDirection.Input;
			arParams[22].Value = compiledMeta;
			
			arParams[23] = new SqlCeParameter("@IsPublished", SqlDbType.Bit);
			arParams[23].Direction = ParameterDirection.Input;
			arParams[23].Value = isPublished;

            arParams[24] = new SqlCeParameter("@SubTitle", SqlDbType.NVarChar, 500);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = subTitle;

            arParams[25] = new SqlCeParameter("@EndDate", SqlDbType.DateTime);
            arParams[25].Direction = ParameterDirection.Input;
            if (endDate < DateTime.MaxValue)
            {
                arParams[25].Value = endDate;
            }
            else
            {
                arParams[25].Value = DBNull.Value;
            }

            arParams[26] = new SqlCeParameter("@Approved", SqlDbType.Bit);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = approved;

            arParams[27] = new SqlCeParameter("@ApprovedDate", SqlDbType.DateTime);
            arParams[27].Direction = ParameterDirection.Input;
            if (approvedDate < DateTime.MaxValue)
            {
                arParams[27].Value = approvedDate;
            }
            else
            {
                arParams[27].Value = DBNull.Value;
            }

            arParams[28] = new SqlCeParameter("@ApprovedBy", SqlDbType.UniqueIdentifier);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = approvedBy;

            arParams[29] = new SqlCeParameter("@ShowAuthorName", SqlDbType.Bit);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = showAuthorName;

            arParams[30] = new SqlCeParameter("@ShowAuthorAvatar", SqlDbType.Bit);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = showAuthorAvatar;

            arParams[31] = new SqlCeParameter("@ShowAuthorBio", SqlDbType.Bit);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = showAuthorBio;

            arParams[32] = new SqlCeParameter("@IncludeInSearch", SqlDbType.Bit);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = includeInSearch;

            arParams[33] = new SqlCeParameter("@IncludeInSiteMap", SqlDbType.Bit);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = includeInSiteMap;

            arParams[34] = new SqlCeParameter("@UseBingMap", SqlDbType.Bit);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = useBingMap;

            arParams[35] = new SqlCeParameter("@MapHeight", SqlDbType.NVarChar, 10);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = mapHeight;

            arParams[36] = new SqlCeParameter("@MapWidth", SqlDbType.NVarChar, 10);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = mapWidth;

            arParams[37] = new SqlCeParameter("@ShowMapOptions", SqlDbType.Bit);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = showMapOptions;

            arParams[38] = new SqlCeParameter("@ShowZoomTool", SqlDbType.Bit);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = showZoomTool;

            arParams[39] = new SqlCeParameter("@ShowLocationInfo", SqlDbType.Bit);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = showLocationInfo;

            arParams[40] = new SqlCeParameter("@UseDrivingDirections", SqlDbType.Bit);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = useDrivingDirections;

            arParams[41] = new SqlCeParameter("@MapType", SqlDbType.NVarChar, 20);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = mapType;

            arParams[42] = new SqlCeParameter("@MapZoom", SqlDbType.Int);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = mapZoom;

            arParams[43] = new SqlCeParameter("@ShowDownloadLink", SqlDbType.Bit);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = showDownloadLink;

            arParams[44] = new SqlCeParameter("@ExcludeFromRecentContent", SqlDbType.Bit);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = excludeFromRecentContent;

            arParams[45] = new SqlCeParameter("@IncludeInNews", SqlDbType.Bit);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = includeInNews;

            arParams[46] = new SqlCeParameter("@PubName", SqlDbType.NVarChar, 255);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = pubName;

            arParams[47] = new SqlCeParameter("@PubLanguage", SqlDbType.NVarChar, 7);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = pubLanguage;

            arParams[48] = new SqlCeParameter("@PubAccess", SqlDbType.NVarChar, 20);
            arParams[48].Direction = ParameterDirection.Input;
            arParams[48].Value = pubAccess;

            arParams[49] = new SqlCeParameter("@PubGenres", SqlDbType.NVarChar, 255);
            arParams[49].Direction = ParameterDirection.Input;
            arParams[49].Value = pubGenres;

            arParams[50] = new SqlCeParameter("@PubKeyWords", SqlDbType.NVarChar, 255);
            arParams[50].Direction = ParameterDirection.Input;
            arParams[50].Value = pubKeyWords;

            arParams[51] = new SqlCeParameter("@PubGeoLocations", SqlDbType.NVarChar, 255);
            arParams[51].Direction = ParameterDirection.Input;
            arParams[51].Value = pubGeoLocations;

            arParams[52] = new SqlCeParameter("@PubStockTickers", SqlDbType.NVarChar, 255);
            arParams[52].Direction = ParameterDirection.Input;
            arParams[52].Value = pubStockTickers;

            arParams[53] = new SqlCeParameter("@HeadlineImageUrl", SqlDbType.NVarChar, 255);
            arParams[53].Direction = ParameterDirection.Input;
            arParams[53].Value = headlineImageUrl;

            arParams[54] = new SqlCeParameter("@IncludeImageInExcerpt", SqlDbType.Bit);
            arParams[54].Direction = ParameterDirection.Input;
            arParams[54].Value = includeImageInExcerpt;


			int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT count(*) FROM mp_BlogStats WHERE ModuleID = @ModuleID ;");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowCount = Convert.ToInt32(SqlHelper.ExecuteScalar(
				GetConnectionString(), 
				CommandType.Text,
				sqlCommand.ToString(), 
				arParams));

            if (rowCount > 0)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("UPDATE mp_BlogStats ");
                sqlCommand.Append("SET EntryCount = EntryCount + 1 ");
                sqlCommand.Append("WHERE ModuleID = @ModuleID ;");

                arParams = new SqlCeParameter[1];

                arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                SqlHelper.ExecuteScalar(
                    GetConnectionString(),
                    CommandType.Text,
                    sqlCommand.ToString(),
                    arParams);


            }
            else
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("INSERT INTO mp_BlogStats(ModuleGuid, ModuleID, EntryCount, CommentCount, TrackBackCount) ");
                sqlCommand.Append("VALUES (@ModuleGuid, @ModuleID, 1, 0, 0); ");

                arParams = new SqlCeParameter[2];

                arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                arParams[1] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = moduleGuid;

                SqlHelper.ExecuteScalar(
                        GetConnectionString(),
                        CommandType.Text,
                        sqlCommand.ToString(),
                        arParams);


            }

			
			return newId;

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
            StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE mp_Blogs ");
			sqlCommand.Append("SET  ");
			
			sqlCommand.Append("StartDate = @StartDate, ");
			sqlCommand.Append("IsInNewsletter = @IsInNewsletter, ");
			sqlCommand.Append("Description = @Description, ");
			
			sqlCommand.Append("IncludeInFeed = @IncludeInFeed, ");
			sqlCommand.Append("AllowCommentsForDays = @AllowCommentsForDays, ");
			
			sqlCommand.Append("Location = @Location, ");
			
			sqlCommand.Append("LastModUserGuid = @LastModUserGuid, ");
			sqlCommand.Append("LastModUtc = @LastModUtc, ");
			sqlCommand.Append("ItemUrl = @ItemUrl, ");
			sqlCommand.Append("Heading = @Heading, ");
			sqlCommand.Append("MetaKeywords = @MetaKeywords, ");
			sqlCommand.Append("MetaDescription = @MetaDescription, ");
			sqlCommand.Append("Abstract = @Abstract, ");
			sqlCommand.Append("CompiledMeta = @CompiledMeta, ");

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

			sqlCommand.Append("IsPublished = @IsPublished "); 
			
			sqlCommand.Append("WHERE  ");
			sqlCommand.Append("ItemID = @ItemID "); 
			sqlCommand.Append(";");
		
			SqlCeParameter[] arParams = new SqlCeParameter[47];
			
			arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = itemId;
			
			arParams[1] = new SqlCeParameter("@StartDate", SqlDbType.DateTime);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = startDate;
			
			arParams[2] = new SqlCeParameter("@IsInNewsletter", SqlDbType.Bit);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = isInNewsletter;
			
			arParams[3] = new SqlCeParameter("@Description", SqlDbType.NText);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = description;
			
			arParams[4] = new SqlCeParameter("@IncludeInFeed", SqlDbType.Bit);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = includeInFeed;
			
			arParams[5] = new SqlCeParameter("@AllowCommentsForDays", SqlDbType.Int);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = allowCommentsForDays;
			
			arParams[6] = new SqlCeParameter("@Location", SqlDbType.NText);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = location;
			
			arParams[7] = new SqlCeParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = lastModUserGuid;
			
			arParams[8] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = lastModUtc;
			
			arParams[9] = new SqlCeParameter("@ItemUrl", SqlDbType.NVarChar, 255);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = itemUrl;
			
			arParams[10] = new SqlCeParameter("@Heading", SqlDbType.NVarChar, 255);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = title;
			
			arParams[11] = new SqlCeParameter("@MetaKeywords", SqlDbType.NVarChar, 255);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = metaKeywords;
			
			arParams[12] = new SqlCeParameter("@MetaDescription", SqlDbType.NVarChar, 255);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = metaDescription;
			
			arParams[13] = new SqlCeParameter("@Abstract", SqlDbType.NText);
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = excerpt;
			
			arParams[14] = new SqlCeParameter("@CompiledMeta", SqlDbType.NText);
			arParams[14].Direction = ParameterDirection.Input;
			arParams[14].Value = compiledMeta;
			
			arParams[15] = new SqlCeParameter("@IsPublished", SqlDbType.Bit);
			arParams[15].Direction = ParameterDirection.Input;
			arParams[15].Value = isPublished;

            arParams[16] = new SqlCeParameter("@SubTitle", SqlDbType.NVarChar, 500);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = subTitle;

            arParams[17] = new SqlCeParameter("@EndDate", SqlDbType.DateTime);
            arParams[17].Direction = ParameterDirection.Input;
            if (endDate < DateTime.MaxValue)
            {
                arParams[17].Value = endDate;
            }
            else
            {
                arParams[17].Value = DBNull.Value;
            }

            arParams[18] = new SqlCeParameter("@Approved", SqlDbType.Bit);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = approved;

            arParams[19] = new SqlCeParameter("@ApprovedDate", SqlDbType.DateTime);
            arParams[19].Direction = ParameterDirection.Input;
            if (approvedDate < DateTime.MaxValue)
            {
                arParams[19].Value = approvedDate;
            }
            else
            {
                arParams[19].Value = DBNull.Value;
            }

            arParams[20] = new SqlCeParameter("@ApprovedBy", SqlDbType.UniqueIdentifier);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = approvedBy;

            arParams[21] = new SqlCeParameter("@ShowAuthorName", SqlDbType.Bit);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = showAuthorName;

            arParams[22] = new SqlCeParameter("@ShowAuthorAvatar", SqlDbType.Bit);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = showAuthorAvatar;

            arParams[23] = new SqlCeParameter("@ShowAuthorBio", SqlDbType.Bit);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = showAuthorBio;

            arParams[24] = new SqlCeParameter("@IncludeInSearch", SqlDbType.Bit);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = includeInSearch;

            arParams[25] = new SqlCeParameter("@IncludeInSiteMap", SqlDbType.Bit);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = includeInSiteMap;

            arParams[26] = new SqlCeParameter("@UseBingMap", SqlDbType.Bit);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = useBingMap;

            arParams[27] = new SqlCeParameter("@MapHeight", SqlDbType.NVarChar, 10);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = mapHeight;

            arParams[28] = new SqlCeParameter("@MapWidth", SqlDbType.NVarChar, 10);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = mapWidth;

            arParams[29] = new SqlCeParameter("@ShowMapOptions", SqlDbType.Bit);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = showMapOptions;

            arParams[30] = new SqlCeParameter("@ShowZoomTool", SqlDbType.Bit);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = showZoomTool;

            arParams[31] = new SqlCeParameter("@ShowLocationInfo", SqlDbType.Bit);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = showLocationInfo;

            arParams[32] = new SqlCeParameter("@UseDrivingDirections", SqlDbType.Bit);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = useDrivingDirections;

            arParams[33] = new SqlCeParameter("@MapType", SqlDbType.NVarChar, 20);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = mapType;

            arParams[34] = new SqlCeParameter("@MapZoom", SqlDbType.Int);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = mapZoom;

            arParams[35] = new SqlCeParameter("@ShowDownloadLink", SqlDbType.Bit);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = showDownloadLink;

            arParams[36] = new SqlCeParameter("@ExcludeFromRecentContent", SqlDbType.Bit);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = excludeFromRecentContent;

            arParams[37] = new SqlCeParameter("@IncludeInNews", SqlDbType.Bit);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = includeInNews;

            arParams[38] = new SqlCeParameter("@PubName", SqlDbType.NVarChar, 255);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = pubName;

            arParams[39] = new SqlCeParameter("@PubLanguage", SqlDbType.NVarChar, 7);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = pubLanguage;

            arParams[40] = new SqlCeParameter("@PubAccess", SqlDbType.NVarChar, 20);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = pubAccess;

            arParams[41] = new SqlCeParameter("@PubGenres", SqlDbType.NVarChar, 255);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = pubGenres;

            arParams[42] = new SqlCeParameter("@PubKeyWords", SqlDbType.NVarChar, 255);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = pubKeyWords;

            arParams[43] = new SqlCeParameter("@PubGeoLocations", SqlDbType.NVarChar, 255);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = pubGeoLocations;

            arParams[44] = new SqlCeParameter("@PubStockTickers", SqlDbType.NVarChar, 255);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = pubStockTickers;

            arParams[45] = new SqlCeParameter("@HeadlineImageUrl", SqlDbType.NVarChar, 255);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = headlineImageUrl;

            arParams[46] = new SqlCeParameter("@IncludeImageInExcerpt", SqlDbType.Bit);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = includeImageInExcerpt;
			
			int rowsAffected = SqlHelper.ExecuteNonQuery(
				GetConnectionString(), 
				CommandType.Text,
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
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("BlogGuid = @BlogGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@BlogGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = blogGuid;

            arParams[1] = new SqlCeParameter("@CommentCount", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = commentCount;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_BlogComments ");
            sqlCommand.Append("(");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("ItemID, ");
            sqlCommand.Append("Comment, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("URL, ");
            sqlCommand.Append("DateCreated ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ModuleID, ");
            sqlCommand.Append("@ItemID, ");
            sqlCommand.Append("@Comment, ");
            sqlCommand.Append("@Title, ");
            sqlCommand.Append("@Name, ");
            sqlCommand.Append("@URL, ");
            sqlCommand.Append("@DateCreated ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[7];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            arParams[2] = new SqlCeParameter("@Comment", SqlDbType.NText);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = comment;

            arParams[3] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = title;

            arParams[4] = new SqlCeParameter("@Name", SqlDbType.NVarChar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = name;

            arParams[5] = new SqlCeParameter("@URL", SqlDbType.NVarChar, 200);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = url;

            arParams[6] = new SqlCeParameter("@DateCreated", SqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = dateCreated;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append(" COALESCE(COUNT(*), 0) FROM mp_BlogComments ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID AND ItemID = @ItemID ;");

            arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            sqlCommand = new StringBuilder();
            sqlCommand.Append("Update mp_Blogs ");
            sqlCommand.Append("SET CommentCount = @CommentCount ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID AND ItemID = @ItemID;");

            arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            arParams[2] = new SqlCeParameter("@CommentCount", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = count;

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("COALESCE(COUNT(*), 0) FROM mp_BlogComments ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID  ;");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            sqlCommand = new StringBuilder();
            sqlCommand.Append("Update mp_BlogStats ");
            sqlCommand.Append("SET CommentCount =  @CommentCount ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID ;");

            arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CommentCount", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = count;

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


            return (newId > -1);

        }

        public static bool DeleteAllCommentsForBlog(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogComments  ");
            sqlCommand.Append("WHERE ItemID = @ItemID  ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdateCommentStats(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_BlogComments WHERE ModuleID = @ModuleID; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_BlogStats  ");
            sqlCommand.Append("SET 	CommentCount = @CommentCount   ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID  ");
            sqlCommand.Append(";");

            arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CommentCount", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = count;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdateEntryStats(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) FROM mp_Blogs WHERE ModuleID = @ModuleID; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_BlogStats  ");
            sqlCommand.Append("SET 	EntryCount = @EntryCount   ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID  ");
            sqlCommand.Append(";");

            arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@EntryCount", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = count;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static bool DeleteBlogComment(int commentId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ModuleID, ItemID ");
            sqlCommand.Append("FROM	mp_BlogComments ");

            sqlCommand.Append("WHERE BlogCommentID = @BlogCommentID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@BlogCommentID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = commentId;

            int moduleId = -1;
            int itemId = -1;

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("BlogCommentID = @BlogCommentID ");
            sqlCommand.Append(";");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@BlogCommentID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = commentId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            if (moduleId > -1)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("SELECT COUNT(*) FROM mp_BlogComments WHERE ModuleID = @ModuleID AND ItemID = @ItemID; ");

                arParams = new SqlCeParameter[2];

                arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                arParams[1] = new SqlCeParameter("@ItemID", SqlDbType.Int);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = itemId;

                int itemCommentCount = Convert.ToInt32(SqlHelper.ExecuteScalar(
                    GetConnectionString(),
                    CommandType.Text,
                    sqlCommand.ToString(),
                    arParams));

                sqlCommand = new StringBuilder();
                sqlCommand.Append("SELECT COUNT(*) FROM mp_BlogComments WHERE ModuleID = @ModuleID; ");

                arParams = new SqlCeParameter[1];

                arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                int moduleCommentCount = Convert.ToInt32(SqlHelper.ExecuteScalar(
                    GetConnectionString(),
                    CommandType.Text,
                    sqlCommand.ToString(),
                    arParams));



                sqlCommand = new StringBuilder();
                sqlCommand.Append("UPDATE mp_Blogs ");
                sqlCommand.Append("SET CommentCount = @CommentCount ");
                sqlCommand.Append("WHERE ModuleID = @ModuleID AND ItemID = @ItemID ;");

                arParams = new SqlCeParameter[3];

                arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                arParams[1] = new SqlCeParameter("@ItemID", SqlDbType.Int);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = itemId;

                arParams[2] = new SqlCeParameter("@CommentCount", SqlDbType.Int);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = itemCommentCount;

                SqlHelper.ExecuteNonQuery(
                    GetConnectionString(),
                    CommandType.Text,
                    sqlCommand.ToString(),
                    arParams);

                sqlCommand = new StringBuilder();
                sqlCommand.Append("UPDATE mp_BlogStats ");
                sqlCommand.Append("SET CommentCount = @CommentCount ");
                sqlCommand.Append("WHERE ModuleID = @ModuleID  ;");

                arParams = new SqlCeParameter[2];

                arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                arParams[1] = new SqlCeParameter("@CommentCount", SqlDbType.Int);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = moduleCommentCount;

                SqlHelper.ExecuteNonQuery(
                    GetConnectionString(),
                    CommandType.Text,
                    sqlCommand.ToString(),
                    arParams);

            }

            return (rowsAffected > -1);

        }

        public static IDataReader GetBlogComments(int moduleId, int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_BlogComments ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID AND ItemID = @ItemID  ");
            sqlCommand.Append("ORDER BY BlogCommentID,  DateCreated DESC  ;");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int AddBlogCategory(
          int moduleId,
          string category)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_BlogCategories ");
            sqlCommand.Append("(");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("Category ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ModuleID, ");
            sqlCommand.Append("@Category ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@Category", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = category;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;

        }

        public static bool UpdateBlogCategory(
          int categoryId,
          string category)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_BlogCategories ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Category = @Category ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("CategoryID = @CategoryID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@CategoryID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;

            arParams[1] = new SqlCeParameter("@Category", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = category;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteCategory(int categoryId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogCategories ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CategoryID = @CategoryID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@CategoryID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetCategory(int categoryId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_BlogCategories ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CategoryID = @CategoryID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@CategoryID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("AND b.StartDate <= @CurrentTime ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > @CurrentTime) ");

            sqlCommand.Append("GROUP BY bc.CategoryID, bc.Category ");
            sqlCommand.Append("ORDER BY bc.Category; ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int AddBlogItemCategory(
          int itemId,
          int categoryId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_BlogItemCategories ");
            sqlCommand.Append("(");
            sqlCommand.Append("ItemID, ");
            sqlCommand.Append("CategoryID ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ItemID, ");
            sqlCommand.Append("@CategoryID ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqlCeParameter("@CategoryID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;

        }

        public static bool DeleteItemCategories(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogItemCategories ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }
    }
}
