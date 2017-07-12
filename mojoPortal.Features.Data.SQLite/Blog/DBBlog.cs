// Author:					
// Created:				    2007-11-03
// Last Modified:			2017-06-07
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


using Mono.Data.Sqlite;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text;

namespace mojoPortal.Data
{
	// <summary>
	/// 
	/// </summary>
	public static class DBBlog
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(DBBlog));
        
        
        private static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];
            if (connectionString == "defaultdblocation")
            {

                connectionString = "version=3,URI=file:"
                    + System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config");

            }
            return connectionString;
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
            sqlCommand.Append("AND ModuleID = :ModuleID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsToShow = int.Parse(ConfigurationManager.AppSettings["DefaultBlogPageSize"]);

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    if (reader["SettingValue"] != DBNull.Value)
                    {
                        try
                        {
                            rowsToShow = Convert.ToInt32(reader["SettingValue"]);
                        }
                        catch (ArgumentException) { }
                        catch (FormatException) { }
                        catch (OverflowException) { }

                    }

                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT b.*, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	mp_Blogs b ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
            sqlCommand.Append("AND :BeginDate >= b.StartDate  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > :CurrentTime)  ");


            sqlCommand.Append("ORDER BY b.StartDate DESC  ");
            sqlCommand.Append("LIMIT " + rowsToShow.ToString() + ";");

            arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":BeginDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return SqliteHelper.ExecuteReader(
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
            sqlCommand.Append("SELECT b.*, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ItemID <> :ItemID  ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > :CurrentTime)  ");

            sqlCommand.Append("AND b.ItemID IN (");
            sqlCommand.Append("SELECT ItemID FROM mp_BlogItemCategories WHERE CategoryID IN ( ");
            sqlCommand.Append("SELECT CategoryID FROM mp_BlogItemCategories WHERE ItemID = :ItemID ");
            sqlCommand.Append(") ) ");

            sqlCommand.Append("ORDER BY  b.StartDate DESC  ");
            sqlCommand.Append("LIMIT 20  ");
            sqlCommand.Append(" ;  ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return SqliteHelper.ExecuteReader(
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
            sqlCommand.Append("AND ModuleID = :ModuleID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsToShow = 20;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    if (reader["SettingValue"] != DBNull.Value)
                    {
                        try
                        {
                            rowsToShow = Convert.ToInt32(reader["SettingValue"]);
                        }
                        catch (ArgumentException) { }
                        catch (FormatException) { }
                        catch (OverflowException) { }

                    }

                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT b.*, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	mp_Blogs b ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
            sqlCommand.Append("AND :BeginDate >= b.StartDate  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > :CurrentTime)  ");


            sqlCommand.Append("ORDER BY b.StartDate DESC  ");
            sqlCommand.Append("LIMIT " + rowsToShow.ToString() + ";");

            arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":BeginDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return SqliteHelper.ExecuteReader(
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
            sqlCommand.Append("AND ModuleID = :ModuleID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsToShow = 100;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    if (reader["SettingValue"] != DBNull.Value)
                    {
                        try
                        {
                            rowsToShow = Convert.ToInt32(reader["SettingValue"]);
                        }
                        catch (ArgumentException) { }
                        catch (FormatException) { }
                        catch (OverflowException) { }

                    }

                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT b.*, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.LoginName, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM	mp_Blogs b ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON b.UserGuid = u.UserGuid ");

            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
            sqlCommand.Append("AND :BeginDate >= b.StartDate  ");
            //sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime  ");
            


            sqlCommand.Append("ORDER BY b.StartDate DESC  ");
            sqlCommand.Append("LIMIT " + rowsToShow.ToString() + ";");

            arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":BeginDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return SqliteHelper.ExecuteReader(
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
            sqlCommand.Append("AND ModuleID = :ModuleID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsToShow = 100;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    if (reader["SettingValue"] != DBNull.Value)
                    {
                        try
                        {
                            rowsToShow = Convert.ToInt32(reader["SettingValue"]);
                        }
                        catch (ArgumentException) { }
                        catch (FormatException) { }
                        catch (OverflowException) { }

                    }

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

            sqlCommand.Append("SELECT b.ItemID ");
            
            sqlCommand.Append("FROM	mp_Blogs b ");
            
            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
            sqlCommand.Append("AND :BeginDate >= b.StartDate  ");
            //sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime  ");


            sqlCommand.Append("ORDER BY b.StartDate DESC  ");
            sqlCommand.Append("LIMIT " + rowsToShow.ToString() + " ");

            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY bc.Category ");
            sqlCommand.Append(" ;  ");

            arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":BeginDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return SqliteHelper.ExecuteReader(
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
            sqlCommand.Append("WHERE ModuleID = :ModuleID  ");
            sqlCommand.Append("AND EndDate < :CurrentTime  ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
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

            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
            sqlCommand.Append("AND b.EndDate < :CurrentTime  ");


            sqlCommand.Append("ORDER BY b.StartDate DESC  ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            arParams[2] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            return SqliteHelper.ExecuteReader(
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

            sqlCommand.Append("SELECT b.BlogGuid ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
            sqlCommand.Append("AND b.EndDate < :CurrentTime  ");


            sqlCommand.Append("ORDER BY b.StartDate DESC  ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(") ");
           
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            arParams[2] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

          
            return SqliteHelper.ExecuteReader(
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

            sqlCommand.Append("SELECT b.ItemID ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  "); 
            sqlCommand.Append("AND b.EndDate < :CurrentTime  ");


            sqlCommand.Append("ORDER BY b.StartDate DESC  ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(") ");
            sqlCommand.Append("ORDER BY bc.Category ");

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            arParams[2] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            
            return SqliteHelper.ExecuteReader(
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
            sqlCommand.Append("WHERE ModuleID = :ModuleID  ");
            sqlCommand.Append("AND (:UserGuid = '00000000-0000-0000-0000-000000000000' OR UserGuid  = :UserGuid)  ");
            sqlCommand.Append("AND ((StartDate > :CurrentTime) OR (IsPublished = 0)) ");
            
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
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

            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
            sqlCommand.Append("AND (:UserGuid = '00000000-0000-0000-0000-000000000000' OR b.UserGuid  = :UserGuid)  ");
            sqlCommand.Append("AND ((b.StartDate > :CurrentTime) OR (b.IsPublished = 0)) ");


            sqlCommand.Append("ORDER BY b.StartDate DESC  ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            arParams[3] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageSize;

            arParams[4] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageLowerBound;

            //log.Info("pageSize " + pageSize.ToString());
            //log.Info("pageLowerBound " + pageLowerBound.ToString());

            return SqliteHelper.ExecuteReader(
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
            sqlCommand.Append("WHERE ModuleID = :ModuleID  ");
            sqlCommand.Append("AND :BeginDate >= StartDate  ");
            sqlCommand.Append("AND IsPublished = 1 ");
            sqlCommand.Append("AND StartDate <= :CurrentTime  ");
            sqlCommand.Append("AND (EndDate IS NULL OR EndDate > :CurrentTime)  ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":BeginDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
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

            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
            sqlCommand.Append("AND :BeginDate >= b.StartDate  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > :CurrentTime)  ");


            sqlCommand.Append("ORDER BY b.StartDate DESC  ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":BeginDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            arParams[3] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageSize;

            arParams[4] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageLowerBound;

            //log.Info("pageSize " + pageSize.ToString());
            //log.Info("pageLowerBound " + pageLowerBound.ToString());

            return SqliteHelper.ExecuteReader(
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

            sqlCommand.Append("SELECT b.BlogGuid ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
            sqlCommand.Append("AND :BeginDate >= b.StartDate  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > :CurrentTime)  ");


            sqlCommand.Append("ORDER BY b.StartDate DESC  ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(") ");
            //sqlCommand.Append("ORDER BY bc.Category ");

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":BeginDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            arParams[3] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageSize;

            arParams[4] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageLowerBound;

            //log.Info("pageSize " + pageSize.ToString());
            //log.Info("pageLowerBound " + pageLowerBound.ToString());

            return SqliteHelper.ExecuteReader(
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

            sqlCommand.Append("SELECT b.BlogGuid ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("JOIN	mp_BlogItemCategories bic2 ");
            sqlCommand.Append("ON b.ItemID = bic2.ItemID ");

            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > :CurrentTime)  ");
            sqlCommand.Append("AND  bic2.CategoryID = :CategoryID   ");


            sqlCommand.Append("ORDER BY b.StartDate DESC  ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(") ");
            //sqlCommand.Append("ORDER BY bc.Category ");

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":CategoryID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            arParams[3] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageSize;

            arParams[4] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageLowerBound;

            
            return SqliteHelper.ExecuteReader(
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

            sqlCommand.Append("SELECT b.BlogGuid ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            

            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > :CurrentTime)  ");

            sqlCommand.Append("AND strftime('%Y', b.StartDate) = :Year  ");
            sqlCommand.Append(" AND strftime('%m', b.StartDate)  = :Month  ");


            sqlCommand.Append("ORDER BY b.StartDate DESC  ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(") ");
            //sqlCommand.Append("ORDER BY bc.Category ");

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[6];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":Year", DbType.String);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = year.ToString(CultureInfo.InvariantCulture);

            arParams[2] = new SqliteParameter(":Month", DbType.String);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = month.ToString("00", CultureInfo.InvariantCulture);

            arParams[3] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currentTime;

            arParams[4] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageSize;

            arParams[5] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = pageLowerBound;

            return SqliteHelper.ExecuteReader(
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

            sqlCommand.Append("SELECT b.ItemID ");
            
            sqlCommand.Append("FROM	mp_Blogs b ");
            
            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
            sqlCommand.Append("AND :BeginDate >= b.StartDate  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > :CurrentTime)  ");


            sqlCommand.Append("ORDER BY b.StartDate DESC  ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(") ");
            sqlCommand.Append("ORDER BY bc.Category ");

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":BeginDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            arParams[3] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageSize;

            arParams[4] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageLowerBound;

            //log.Info("pageSize " + pageSize.ToString());
            //log.Info("pageLowerBound " + pageLowerBound.ToString());

            return SqliteHelper.ExecuteReader(
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
            sqlCommand.Append("WHERE ModuleID = :ModuleID  ");
            sqlCommand.Append("AND IsPublished = 1 ");
            sqlCommand.Append("AND StartDate <= :CurrentTime ");
            sqlCommand.Append("AND (EndDate IS NULL OR EndDate > :CurrentTime)  ");
            sqlCommand.Append("AND strftime('%Y', StartDate) = :Year  ");
            sqlCommand.Append(" AND strftime('%m', StartDate)  = :Month  ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":Year", DbType.String);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = year.ToString(CultureInfo.InvariantCulture);

            arParams[2] = new SqliteParameter(":Month", DbType.String);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = month.ToString("00", CultureInfo.InvariantCulture);

            arParams[3] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currentTime;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
               GetConnectionString(),
               sqlCommand.ToString(),
               arParams));

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

            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > :CurrentTime)  ");

            sqlCommand.Append("AND strftime('%Y', b.StartDate) = :Year  ");
            sqlCommand.Append(" AND strftime('%m', b.StartDate)  = :Month  ");

            sqlCommand.Append("ORDER BY b.StartDate DESC ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }

            sqlCommand.Append(";");


            SqliteParameter[] arParams = new SqliteParameter[6];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":Year", DbType.String);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = year.ToString(CultureInfo.InvariantCulture);

            arParams[2] = new SqliteParameter(":Month", DbType.String);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = month.ToString("00", CultureInfo.InvariantCulture);

            arParams[3] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currentTime;

            arParams[4] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageSize;

            arParams[5] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = pageLowerBound;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

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

            sqlCommand.Append("SELECT b.ItemID ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            //sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
            //sqlCommand.Append("AND :BeginDate >= b.StartDate  ");
            //sqlCommand.Append("AND b.IsPublished = 1 ");
            //sqlCommand.Append("AND b.StartDate <= :CurrentTime  ");

            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > :CurrentTime)  ");

            sqlCommand.Append("AND strftime('%Y', b.StartDate) = :Year  ");
            sqlCommand.Append(" AND strftime('%m', b.StartDate)  = :Month  ");


            sqlCommand.Append("ORDER BY b.StartDate DESC  ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(") ");
            sqlCommand.Append("ORDER BY bc.Category ");

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[6];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":Year", DbType.String);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = year.ToString(CultureInfo.InvariantCulture);

            arParams[2] = new SqliteParameter(":Month", DbType.String);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = month.ToString("00", CultureInfo.InvariantCulture);

            arParams[3] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currentTime;

            arParams[4] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageSize;

            arParams[5] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = pageLowerBound;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetBlogEntriesByMonth(int month, int year, int moduleId, DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");

            sqlCommand.Append("FROM	mp_Blogs ");

            sqlCommand.Append("WHERE ModuleID = :ModuleID  ");
            sqlCommand.Append("AND IsPublished = 1 ");
            sqlCommand.Append("AND StartDate <= :CurrentTime ");
            sqlCommand.Append("AND (EndDate IS NULL OR EndDate > :CurrentTime)  ");

            sqlCommand.Append("AND strftime('%Y', StartDate) = :Year  ");
            sqlCommand.Append(" AND strftime('%m', StartDate)  = :Month  ");

            sqlCommand.Append("ORDER BY StartDate DESC ;");


            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":Year", DbType.String);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = year.ToString(CultureInfo.InvariantCulture);

            arParams[2] = new SqliteParameter(":Month", DbType.String);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = month.ToString("00", CultureInfo.InvariantCulture);

            arParams[3] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currentTime;

            return SqliteHelper.ExecuteReader(
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

            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
           
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > :CurrentTime)  ");
            sqlCommand.Append("AND  bic.CategoryID = :CategoryID   ");

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":CategoryID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
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

            sqlCommand.Append("WHERE b.ModuleID = :ModuleID   ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > :CurrentTime)  ");
            sqlCommand.Append("AND  bic.CategoryID = :CategoryID   ");
            sqlCommand.Append("ORDER BY b.StartDate DESC ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":CategoryID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            arParams[3] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageSize;

            arParams[4] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageLowerBound;

            return SqliteHelper.ExecuteReader(
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

            sqlCommand.Append("SELECT b.ItemID ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("JOIN	mp_BlogItemCategories bic2 ");
            sqlCommand.Append("ON b.ItemID = bic2.ItemID ");

            sqlCommand.Append("WHERE b.ModuleID = :ModuleID  ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > :CurrentTime)  ");
            sqlCommand.Append("AND  bic2.CategoryID = :CategoryID   ");


            sqlCommand.Append("ORDER BY b.StartDate DESC  ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(") ");
            sqlCommand.Append("ORDER BY bc.Category ");

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":CategoryID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            arParams[3] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageSize;

            arParams[4] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageLowerBound;

            //log.Info("pageSize " + pageSize.ToString());
            //log.Info("pageLowerBound " + pageLowerBound.ToString());

            return SqliteHelper.ExecuteReader(
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
            sqlCommand.Append("WHERE b.ModuleID = :ModuleID   ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentTime ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > :CurrentTime)  ");
            sqlCommand.Append("AND  bic.CategoryID = :CategoryID   ");
            sqlCommand.Append("ORDER BY b.StartDate DESC;");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":CategoryID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetBlogsForSiteMap(int siteId, DateTime currentUtcDateTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("b.ItemUrl AS ItemUrl, ");
            sqlCommand.Append("b.LastModUtc AS LastModUtc, ");
            sqlCommand.Append("b.ItemID AS ItemID, ");
            sqlCommand.Append("b.ModuleID AS ModuleID, ");
            sqlCommand.Append("pm.PageID AS PageID  ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("JOIN mp_Modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN mp_PageModules pm ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b.ModuleID = pm.ModuleID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("m.SiteID = :SiteID ");
            sqlCommand.Append("AND b.IncludeInSiteMap = 1 ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentDateTime  ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > :CurrentDateTime)  ");
            sqlCommand.Append("AND b.ItemUrl <> ''  ");
            sqlCommand.Append("ORDER BY b.StartDate DESC  ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":CurrentDateTime", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentUtcDateTime;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetBlogsForNewsMap(int siteId, DateTime utcThresholdTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("b.ItemUrl AS ItemUrl, ");
            sqlCommand.Append("b.LastModUtc AS LastModUtc, ");
            sqlCommand.Append("b.ItemID AS ItemID, ");
            sqlCommand.Append("b.ModuleID AS ModuleID, ");

            sqlCommand.Append("b.HeadlineImageUrl AS HeadlineImageUrl, ");
            sqlCommand.Append("b.PubAccess AS PubAccess AS PubAccess, ");
            sqlCommand.Append("b.PubGenres AS PubGenres, ");
            sqlCommand.Append("b.PubGeoLocations AS PubGeoLocations, ");
            sqlCommand.Append("b.PubKeyWords AS PubKeyWords, ");
            sqlCommand.Append("b.PubLanguage AS PubLanguage,");
            sqlCommand.Append("b.PubName AS PubName, ");
            sqlCommand.Append("b.PubStockTickers AS PubStockTickers, ");
            sqlCommand.Append("b.StartDate AS StartDate,");
            sqlCommand.Append("b.Title AS Title, ");
            sqlCommand.Append("b.Heading AS Heading, ");

            sqlCommand.Append("pm.PageID AS PageID  ");

            sqlCommand.Append("FROM	mp_Blogs b ");

            sqlCommand.Append("JOIN mp_Modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN mp_PageModules pm ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b.ModuleID = pm.ModuleID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("m.SiteID = :SiteID ");
            sqlCommand.Append("AND b.IncludeInNews = 1 ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate >= :UtcThresholdTime  ");
            
            sqlCommand.Append("AND b.ItemUrl <> ''  ");
            sqlCommand.Append("ORDER BY b.StartDate DESC  ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":UtcThresholdTime", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = utcThresholdTime;

            return SqliteHelper.ExecuteReader(
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
            sqlCommand.Append("WHERE ModuleID = :ModuleID  ");
            sqlCommand.Append("AND ((StartDate > :BeginDate) OR (IsPublished = 0))  ");

            sqlCommand.Append("ORDER BY StartDate DESC  ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":BeginDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return SqliteHelper.ExecuteReader(
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
            sqlCommand.Append("p.SiteID = :SiteID ");
            sqlCommand.Append("AND pm.PageID = :PageID ");
            //sqlCommand.Append("AND pm.PublishBeginDate < datetime('now','localtime') ");
            //sqlCommand.Append("AND (pm.PublishEndDate IS NULL OR pm.PublishEndDate > datetime('now','localtime'))  ;"); 
            sqlCommand.Append(" ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":PageID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return SqliteHelper.ExecuteReader(
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

            sqlCommand.Append("WHERE ModuleID = :ModuleID  ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetBlogMonthArchive(int moduleId, DateTime currentTime)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("strftime('%m', StartDate) AS Month, ");
            sqlCommand.Append("strftime('%m', StartDate) AS MonthName, ");
            sqlCommand.Append("strftime('%Y', StartDate) AS Year, ");
            sqlCommand.Append("1 AS Day, ");
            sqlCommand.Append("count(*) AS Count ");

            sqlCommand.Append("FROM	mp_Blogs ");

            sqlCommand.Append("WHERE ModuleID = :ModuleID  ");
            sqlCommand.Append("AND IsPublished = 1 ");
            sqlCommand.Append("AND StartDate <= :CurrentDate  ");
            sqlCommand.Append("AND ((EndDate IS NULL) OR (EndDate > :CurrentDate))  ");
            sqlCommand.Append("GROUP BY strftime('%Y', StartDate),  ");
            sqlCommand.Append("strftime('%m', StartDate),  ");
            sqlCommand.Append("strftime('%m', StartDate)  ");

            sqlCommand.Append("ORDER BY strftime('%Y', StartDate) desc, strftime('%m', StartDate)  desc ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":CurrentDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        


        public static IDataReader GetSingleBlog(int itemId, DateTime currentTime)
        {
            //sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > :CurrentTime)  ");

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  b.*, ");

            sqlCommand.Append("(SELECT b2.ItemUrl FROM mp_Blogs b2 WHERE b2.IsPublished = 1 AND b2.StartDate <= :CurrentTime AND (b2.EndDate IS NULL OR b2.EndDate > :CurrentTime) AND b2.StartDate > b.StartDate AND b2.ModuleID = b.ModuleID AND b2.ItemUrl IS NOT NULL AND b2.ItemUrl <> '' ORDER BY b2.StartDate LIMIT 1 ) AS NextPost, ");
            sqlCommand.Append("(SELECT b4.Title FROM mp_Blogs b4 WHERE b4.IsPublished = 1 AND b4.StartDate <= :CurrentTime AND (b4.EndDate IS NULL OR b4.EndDate > :CurrentTime) AND b4.StartDate > b.StartDate AND b4.ModuleID = b.ModuleID AND b4.ItemUrl IS NOT NULL AND b4.ItemUrl <> '' ORDER BY b4.StartDate LIMIT 1 ) AS NextPostTitle, ");
            sqlCommand.Append("COALESCE((SELECT b6.ItemID FROM mp_Blogs b6 WHERE b6.IsPublished = 1 AND b6.StartDate <= :CurrentTime AND (b6.EndDate IS NULL OR b6.EndDate > :CurrentTime) AND b6.StartDate > b.StartDate AND b6.ModuleID = b.ModuleID  ORDER BY b6.StartDate LIMIT 1 ),-1) AS NextItemID, ");



            sqlCommand.Append(" (SELECT b3.ItemUrl FROM mp_Blogs b3 WHERE b3.IsPublished = 1 AND b3.StartDate <= :CurrentTime AND (b3.EndDate IS NULL OR b3.EndDate > :CurrentTime) AND b3.StartDate < b.StartDate AND b3.ModuleID = b.ModuleID AND b3.ItemUrl IS NOT NULL AND b3.ItemUrl <> '' ORDER BY b3.StartDate DESC LIMIT 1 ) AS PreviousPost, ");
            sqlCommand.Append(" (SELECT b5.Title FROM mp_Blogs b5 WHERE b5.IsPublished = 1 AND b5.StartDate <= :CurrentTime AND (b5.EndDate IS NULL OR b5.EndDate > :CurrentTime) AND b5.StartDate < b.StartDate AND b5.ModuleID = b.ModuleID AND b5.ItemUrl IS NOT NULL AND b5.ItemUrl <> '' ORDER BY b5.StartDate DESC LIMIT 1 ) AS PreviousPostTitle,  ");

            sqlCommand.Append(" COALESCE((SELECT b7.ItemID FROM mp_Blogs b7 WHERE b7.IsPublished = 1 AND b7.StartDate <= :CurrentTime AND (b7.EndDate IS NULL OR b7.EndDate > :CurrentTime) AND b7.StartDate < b.StartDate AND b7.ModuleID = b.ModuleID  ORDER BY b7.StartDate DESC LIMIT 1 ),-1) AS PreviousItemID,  ");


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

            sqlCommand.Append("WHERE b.ItemID = :ItemID ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }



        public static bool DeleteBlog(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Blogs ");
            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByModule(int moduleId)
        {
          
            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogItemCategories ");
            sqlCommand.Append("WHERE ItemID IN (SELECT ItemID FROM mp_Blogs WHERE ModuleID  ");
            sqlCommand.Append(" = :ModuleID ) ");
            sqlCommand.Append(";");

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE PageGuid IN (SELECT BlogGuid FROM mp_Blogs WHERE ModuleID  ");
            sqlCommand.Append(" = :ModuleID ) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentHistory ");
            sqlCommand.Append("WHERE ContentGuid IN (SELECT BlogGuid FROM mp_Blogs WHERE ModuleID  ");
            sqlCommand.Append(" = :ModuleID ) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentRating ");
            sqlCommand.Append("WHERE ContentGuid IN (SELECT BlogGuid FROM mp_Blogs WHERE ModuleID  ");
            sqlCommand.Append(" = :ModuleID ) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogCategories ");
            sqlCommand.Append("WHERE ModuleID  = :ModuleID ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogStats ");
            sqlCommand.Append("WHERE ModuleID = :ModuleID ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogComments ");
            sqlCommand.Append("WHERE ModuleID  = :ModuleID ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Blogs ");
            sqlCommand.Append("WHERE ModuleID  = :ModuleID ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(int siteId)
        {

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogItemCategories ");
            sqlCommand.Append("WHERE ItemID IN (SELECT ItemID FROM mp_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID) ) ");
            sqlCommand.Append(";");

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE PageGuid IN (SELECT ModuleGuid FROM mp_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID) ) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE PageGuid IN (SELECT BlogGuid FROM mp_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID) ) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentHistory ");
            sqlCommand.Append("WHERE ContentGuid IN (SELECT BlogGuid FROM mp_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID) ) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentRating ");
            sqlCommand.Append("WHERE ContentGuid IN (SELECT BlogGuid FROM mp_Blogs WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID) ) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogCategories ");
            sqlCommand.Append("WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogStats ");
            sqlCommand.Append("WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogComments ");
            sqlCommand.Append("WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Blogs ");
            sqlCommand.Append("WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID) ");
            sqlCommand.Append(";");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
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
            bool includeImageInExcerpt,
			bool includeImageInPost
		)
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

			int intincludeImageInPost = 0;
			if (includeImageInPost) { intincludeImageInPost = 1; }


			#endregion


			StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Blogs (");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("CreatedDate, ");
            sqlCommand.Append("Heading, ");
            sqlCommand.Append("Abstract, ");
            sqlCommand.Append("Excerpt, ");
            sqlCommand.Append("StartDate, ");
            sqlCommand.Append("IsInNewsletter, ");
            sqlCommand.Append("IsPublished, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("CommentCount, ");
            sqlCommand.Append("TrackBackCount, ");
            sqlCommand.Append("IncludeInFeed, ");
            sqlCommand.Append("AllowCommentsForDays, ");
            sqlCommand.Append("BlogGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("Location, ");
            sqlCommand.Append("MetaKeywords, ");
            sqlCommand.Append("MetaDescription, ");
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
			sqlCommand.Append("IncludeImageInPost, ");

            sqlCommand.Append("ItemUrl, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("LastModUserGuid, ");
            sqlCommand.Append("LastModUtc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":ModuleID, ");
            sqlCommand.Append(":CreatedDate, ");
            sqlCommand.Append(":Heading, ");
            sqlCommand.Append(":Abstract, ");
            sqlCommand.Append("'', ");
            sqlCommand.Append(":StartDate, ");
            sqlCommand.Append(":IsInNewsletter, ");
            sqlCommand.Append(":IsPublished, ");
            sqlCommand.Append(":Description, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append(":IncludeInFeed, ");
            sqlCommand.Append(":AllowCommentsForDays, ");
            sqlCommand.Append(":BlogGuid, ");
            sqlCommand.Append(":ModuleGuid, ");
            sqlCommand.Append(":Location, ");
            sqlCommand.Append(":MetaKeywords, ");
            sqlCommand.Append(":MetaDescription, ");
            sqlCommand.Append(":CompiledMeta, ");

            sqlCommand.Append(":SubTitle, ");
            sqlCommand.Append(":EndDate, ");
            sqlCommand.Append(":Approved, ");
            sqlCommand.Append(":ApprovedBy, ");
            sqlCommand.Append(":ApprovedDate, ");

            sqlCommand.Append(":ShowAuthorName, ");
            sqlCommand.Append(":ShowAuthorAvatar, ");
            sqlCommand.Append(":ShowAuthorBio, ");
            sqlCommand.Append(":IncludeInSearch, ");
            sqlCommand.Append(":IncludeInSiteMap, ");
            sqlCommand.Append(":UseBingMap, ");
            sqlCommand.Append(":MapHeight, ");
            sqlCommand.Append(":MapWidth, ");
            sqlCommand.Append(":ShowMapOptions, ");
            sqlCommand.Append(":ShowZoomTool, ");
            sqlCommand.Append(":ShowLocationInfo, ");
            sqlCommand.Append(":UseDrivingDirections, ");
            sqlCommand.Append(":MapType, ");
            sqlCommand.Append(":MapZoom, ");
            sqlCommand.Append(":ShowDownloadLink, ");
            sqlCommand.Append(":ExcludeFromRecentContent, ");

            sqlCommand.Append(":IncludeInNews, ");
            sqlCommand.Append(":PubName, ");
            sqlCommand.Append(":PubLanguage, ");
            sqlCommand.Append(":PubAccess, ");
            sqlCommand.Append(":PubGenres, ");
            sqlCommand.Append(":PubKeyWords, ");
            sqlCommand.Append(":PubGeoLocations, ");
            sqlCommand.Append(":PubStockTickers, ");
            sqlCommand.Append(":HeadlineImageUrl, ");
            sqlCommand.Append(":IncludeImageInExcerpt, ");
            sqlCommand.Append(":IncludeImageInPost, ");

			sqlCommand.Append(":ItemUrl, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":CreatedDate )");
            sqlCommand.Append(";");


            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[50];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":CreatedDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = createdDate;

            arParams[2] = new SqliteParameter(":Heading", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqliteParameter(":Abstract", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = excerpt;

            arParams[4] = new SqliteParameter(":StartDate", DbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = startDate;

            arParams[5] = new SqliteParameter(":IsInNewsletter", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intIsInNewsletter;

            arParams[6] = new SqliteParameter(":Description", DbType.Object);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = description;

            arParams[7] = new SqliteParameter(":IncludeInFeed", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intIncludeInFeed;

            arParams[8] = new SqliteParameter(":AllowCommentsForDays", DbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = allowCommentsForDays;

            arParams[9] = new SqliteParameter(":BlogGuid", DbType.String, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = blogGuid.ToString();

            arParams[10] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = moduleGuid.ToString();

            arParams[11] = new SqliteParameter(":Location", DbType.Object);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = location;

            arParams[12] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = userGuid.ToString();

            arParams[13] = new SqliteParameter(":ItemUrl", DbType.String, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = itemUrl;

            arParams[14] = new SqliteParameter(":MetaKeywords", DbType.String, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = metaKeywords;

            arParams[15] = new SqliteParameter(":MetaDescription", DbType.String, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = metaDescription;

            arParams[16] = new SqliteParameter(":CompiledMeta", DbType.Object);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = compiledMeta;

            arParams[17] = new SqliteParameter(":IsPublished", DbType.Int32);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = intIsPublished;

            arParams[18] = new SqliteParameter(":SubTitle", DbType.String, 500);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = subTitle;

            arParams[19] = new SqliteParameter(":EndDate", DbType.DateTime);
            arParams[19].Direction = ParameterDirection.Input;
            if (endDate < DateTime.MaxValue)
            {
                arParams[19].Value = endDate;
            }
            else
            {
                arParams[19].Value = DBNull.Value;
            }

            arParams[20] = new SqliteParameter(":Approved", DbType.Int32);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = intApproved;

            arParams[21] = new SqliteParameter(":ApprovedDate", DbType.DateTime);
            arParams[21].Direction = ParameterDirection.Input;
            if (approvedDate < DateTime.MaxValue)
            {
                arParams[21].Value = approvedDate;
            }
            else
            {
                arParams[21].Value = DBNull.Value;
            }

            arParams[22] = new SqliteParameter(":ApprovedBy", DbType.String, 36);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = approvedBy.ToString();

            arParams[23] = new SqliteParameter(":ShowAuthorName", DbType.Int32);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = intshowAuthorName;

            arParams[24] = new SqliteParameter(":ShowAuthorAvatar", DbType.Int32);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = intshowAuthorAvatar;

            arParams[25] = new SqliteParameter(":ShowAuthorBio", DbType.Int32);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = intshowAuthorBio;

            arParams[26] = new SqliteParameter(":IncludeInSearch", DbType.Int32);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = intincludeInSearch;

            arParams[27] = new SqliteParameter(":IncludeInSiteMap", DbType.Int32);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = intincludeInSiteMap;

            arParams[28] = new SqliteParameter(":UseBingMap", DbType.Int32);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = intuseBingMap;

            arParams[29] = new SqliteParameter(":MapHeight", DbType.String, 10);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = mapHeight;

            arParams[30] = new SqliteParameter(":MapWidth", DbType.String, 10);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = mapWidth;

            arParams[31] = new SqliteParameter(":ShowMapOptions", DbType.Int32);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = intshowMapOptions;

            arParams[32] = new SqliteParameter(":ShowZoomTool", DbType.Int32);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = intshowZoomTool;

            arParams[33] = new SqliteParameter(":ShowLocationInfo", DbType.Int32);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = intshowLocationInfo;

            arParams[34] = new SqliteParameter(":UseDrivingDirections", DbType.Int32);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = intuseDrivingDirections;

            arParams[35] = new SqliteParameter(":MapType", DbType.String, 20);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = mapType;

            arParams[36] = new SqliteParameter(":MapZoom", DbType.Int32);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = mapZoom;

            arParams[37] = new SqliteParameter(":ShowDownloadLink", DbType.Int32);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = intshowDownloadLink;

            arParams[38] = new SqliteParameter(":ExcludeFromRecentContent", DbType.Int32);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = intExcludeRecent;

            arParams[39] = new SqliteParameter(":IncludeInNews", DbType.Int32);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = intincludeInNews;

            arParams[40] = new SqliteParameter(":PubName", DbType.String, 255);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = pubName;

            arParams[41] = new SqliteParameter(":PubLanguage", DbType.String, 7);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = pubLanguage;

            arParams[42] = new SqliteParameter(":PubAccess", DbType.String, 20);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = pubAccess;

            arParams[43] = new SqliteParameter(":PubGenres", DbType.String, 255);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = pubGenres;

            arParams[44] = new SqliteParameter(":PubKeyWords", DbType.String, 255);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = pubKeyWords;

            arParams[45] = new SqliteParameter(":PubGeoLocations", DbType.String, 255);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = pubGeoLocations;

            arParams[46] = new SqliteParameter(":PubStockTickers", DbType.String, 255);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = pubStockTickers;

            arParams[47] = new SqliteParameter(":HeadlineImageUrl", DbType.String, 255);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = headlineImageUrl;

			arParams[48] = new SqliteParameter(":IncludeImageInExcerpt", DbType.Int32);
			arParams[48].Direction = ParameterDirection.Input;
			arParams[48].Value = intincludeImageInExcerpt;

			arParams[49] = new SqliteParameter(":IncludeImageInPost", DbType.Int32);
			arParams[49].Direction = ParameterDirection.Input;
			arParams[49].Value = intincludeImageInPost;




			int newID = 0;

            newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT count(*) FROM mp_BlogStats WHERE ModuleID = :ModuleID ;");

            arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowCount = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            if (rowCount > 0)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("UPDATE mp_BlogStats ");
                sqlCommand.Append("SET EntryCount = EntryCount + 1 ");
                sqlCommand.Append("WHERE ModuleID = :ModuleID ;");

                arParams = new SqliteParameter[1];

                arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                SqliteHelper.ExecuteNonQuery(
                    GetConnectionString(),
                    sqlCommand.ToString(),
                    arParams);


            }
            else
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("INSERT INTO mp_BlogStats(ModuleGuid, ModuleID, EntryCount, CommentCount, TrackBackCount) ");
                sqlCommand.Append("VALUES (:ModuleGuid, :ModuleID, 1, 0, 0); ");


                arParams = new SqliteParameter[2];

                arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                arParams[1] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = moduleGuid.ToString();

                SqliteHelper.ExecuteNonQuery(
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
            bool includeImageInExcerpt,
            bool includeImageInPost
		)
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

			int intincludeImageInPost = 0;
			if (includeImageInPost) { intincludeImageInPost = 1; }


			#endregion

			StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Blogs ");
            sqlCommand.Append("SET CreatedByUser = :UserName  , ");
            sqlCommand.Append("CreatedDate = datetime('now','localtime') , ");
            sqlCommand.Append("Heading = :Heading  , ");
            sqlCommand.Append("Abstract = :Abstract  , ");
            sqlCommand.Append("ItemUrl = :ItemUrl  , ");
            sqlCommand.Append("Description = :Description , ");
            sqlCommand.Append("IsInNewsletter = " + inNews + " , ");
            sqlCommand.Append("IncludeInFeed = " + inFeed + " , ");
            sqlCommand.Append("IsPublished = " + isPub + " , ");
            sqlCommand.Append("Description = :Description  , ");
            sqlCommand.Append("AllowCommentsForDays = :AllowCommentsForDays  , ");
            sqlCommand.Append("StartDate = :StartDate,   ");
            sqlCommand.Append("Location = :Location, ");
            sqlCommand.Append("MetaKeywords = :MetaKeywords, ");
            sqlCommand.Append("MetaDescription = :MetaDescription, ");
            sqlCommand.Append("CompiledMeta = :CompiledMeta, ");

            sqlCommand.Append("SubTitle = :SubTitle, ");
            sqlCommand.Append("EndDate = :EndDate, ");
            sqlCommand.Append("Approved = :Approved, ");
            sqlCommand.Append("ApprovedBy = :ApprovedBy, ");
            sqlCommand.Append("ApprovedDate = :ApprovedDate, ");

            sqlCommand.Append("ShowAuthorName = :ShowAuthorName, ");
            sqlCommand.Append("ShowAuthorAvatar = :ShowAuthorAvatar, ");
            sqlCommand.Append("ShowAuthorBio = :ShowAuthorBio, ");
            sqlCommand.Append("IncludeInSearch = :IncludeInSearch, ");
            sqlCommand.Append("IncludeInSiteMap = :IncludeInSiteMap, ");
            sqlCommand.Append("UseBingMap = :UseBingMap, ");
            sqlCommand.Append("MapHeight = :MapHeight, ");
            sqlCommand.Append("MapWidth = :MapWidth, ");
            sqlCommand.Append("ShowMapOptions = :ShowMapOptions, ");
            sqlCommand.Append("ShowZoomTool = :ShowZoomTool, ");
            sqlCommand.Append("ShowLocationInfo = :ShowLocationInfo, ");
            sqlCommand.Append("UseDrivingDirections = :UseDrivingDirections, ");
            sqlCommand.Append("MapType = :MapType, ");
            sqlCommand.Append("MapZoom = :MapZoom, ");
            sqlCommand.Append("ShowDownloadLink = :ShowDownloadLink, ");
            sqlCommand.Append("ExcludeFromRecentContent = :ExcludeFromRecentContent, ");

            sqlCommand.Append("IncludeInNews = :IncludeInNews, ");
            sqlCommand.Append("PubName = :PubName, ");
            sqlCommand.Append("PubLanguage = :PubLanguage, ");
            sqlCommand.Append("PubAccess = :PubAccess, ");
            sqlCommand.Append("PubGenres = :PubGenres, ");
            sqlCommand.Append("PubKeyWords = :PubKeyWords, ");
            sqlCommand.Append("PubGeoLocations = :PubGeoLocations, ");
            sqlCommand.Append("PubStockTickers = :PubStockTickers, ");
            sqlCommand.Append("HeadlineImageUrl = :HeadlineImageUrl, ");
            sqlCommand.Append("IncludeImageInExcerpt = :IncludeImageInExcerpt, ");
            sqlCommand.Append("IncludeImageInPost = :IncludeImageInPost, ");

			sqlCommand.Append("LastModUserGuid = :LastModUserGuid, ");
            sqlCommand.Append("LastModUtc = :LastModUtc ");

            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[46];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqliteParameter(":UserName", DbType.String, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userName;

            arParams[2] = new SqliteParameter(":Heading", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqliteParameter(":Abstract", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = excerpt;

            arParams[4] = new SqliteParameter(":Description", DbType.Object);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = description;

            arParams[5] = new SqliteParameter(":StartDate", DbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = startDate;

            arParams[6] = new SqliteParameter(":AllowCommentsForDays", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = allowCommentsForDays;

            arParams[7] = new SqliteParameter(":Location", DbType.Object);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = location;

            arParams[8] = new SqliteParameter(":LastModUserGuid", DbType.String, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModUserGuid.ToString();

            arParams[9] = new SqliteParameter(":LastModUtc", DbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModUtc;

            arParams[10] = new SqliteParameter(":ItemUrl", DbType.String, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = itemUrl;

            arParams[11] = new SqliteParameter(":MetaKeywords", DbType.String, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = metaKeywords;

            arParams[12] = new SqliteParameter(":MetaDescription", DbType.String, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = metaDescription;

            arParams[13] = new SqliteParameter(":CompiledMeta", DbType.Object);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = compiledMeta;

            arParams[14] = new SqliteParameter(":SubTitle", DbType.String, 500);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = subTitle;

            arParams[15] = new SqliteParameter(":EndDate", DbType.DateTime);
            arParams[15].Direction = ParameterDirection.Input;
            if (endDate < DateTime.MaxValue)
            {
                arParams[15].Value = endDate;
            }
            else
            {
                arParams[15].Value = DBNull.Value;
            }

            arParams[16] = new SqliteParameter(":Approved", DbType.Int32);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = intApproved;

            arParams[17] = new SqliteParameter(":ApprovedDate", DbType.DateTime);
            arParams[17].Direction = ParameterDirection.Input;
            if (approvedDate < DateTime.MaxValue)
            {
                arParams[17].Value = approvedDate;
            }
            else
            {
                arParams[17].Value = DBNull.Value;
            }

            arParams[18] = new SqliteParameter(":ApprovedBy", DbType.String, 36);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = approvedBy.ToString();

            arParams[19] = new SqliteParameter(":ShowAuthorName", DbType.Int32);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = intshowAuthorName;

            arParams[20] = new SqliteParameter(":ShowAuthorAvatar", DbType.Int32);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = intshowAuthorAvatar;

            arParams[21] = new SqliteParameter(":ShowAuthorBio", DbType.Int32);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = intshowAuthorBio;

            arParams[22] = new SqliteParameter(":IncludeInSearch", DbType.Int32);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = intincludeInSearch;

            arParams[23] = new SqliteParameter(":IncludeInSiteMap", DbType.Int32);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = intincludeInSiteMap;

            arParams[24] = new SqliteParameter(":UseBingMap", DbType.Int32);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = intuseBingMap;

            arParams[25] = new SqliteParameter(":MapHeight", DbType.String, 10);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = mapHeight;

            arParams[26] = new SqliteParameter(":MapWidth", DbType.String, 10);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = mapWidth;

            arParams[27] = new SqliteParameter(":ShowMapOptions", DbType.Int32);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = intshowMapOptions;

            arParams[28] = new SqliteParameter(":ShowZoomTool", DbType.Int32);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = intshowZoomTool;

            arParams[29] = new SqliteParameter(":ShowLocationInfo", DbType.Int32);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = intshowLocationInfo;

            arParams[30] = new SqliteParameter(":UseDrivingDirections", DbType.Int32);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = intuseDrivingDirections;

            arParams[31] = new SqliteParameter(":MapType", DbType.String, 20);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = mapType;

            arParams[32] = new SqliteParameter(":MapZoom", DbType.Int32);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = mapZoom;

            arParams[33] = new SqliteParameter(":ShowDownloadLink", DbType.Int32);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = intshowDownloadLink;

            arParams[34] = new SqliteParameter(":ExcludeFromRecentContent", DbType.Int32);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = intExcludeRecent;

            arParams[35] = new SqliteParameter(":IncludeInNews", DbType.Int32);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = intincludeInNews;

            arParams[36] = new SqliteParameter(":PubName", DbType.String, 255);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = pubName;

            arParams[37] = new SqliteParameter(":PubLanguage", DbType.String, 7);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = pubLanguage;

            arParams[38] = new SqliteParameter(":PubAccess", DbType.String, 20);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = pubAccess;

            arParams[39] = new SqliteParameter(":PubGenres", DbType.String, 255);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = pubGenres;

            arParams[40] = new SqliteParameter(":PubKeyWords", DbType.String, 255);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = pubKeyWords;

            arParams[41] = new SqliteParameter(":PubGeoLocations", DbType.String, 255);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = pubGeoLocations;

            arParams[42] = new SqliteParameter(":PubStockTickers", DbType.String, 255);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = pubStockTickers;

            arParams[43] = new SqliteParameter(":HeadlineImageUrl", DbType.String, 255);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = headlineImageUrl;

			arParams[44] = new SqliteParameter(":IncludeImageInExcerpt", DbType.Int32);
			arParams[44].Direction = ParameterDirection.Input;
			arParams[44].Value = intincludeImageInExcerpt;

			arParams[45] = new SqliteParameter(":IncludeImageInPost", DbType.Int32);
			arParams[45].Direction = ParameterDirection.Input;
			arParams[45].Value = intincludeImageInPost;


			int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdateCommentCount(Guid blogGuid, int commentCount)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Blogs ");
            sqlCommand.Append("SET CommentCount = :CommentCount   ");
            sqlCommand.Append("WHERE BlogGuid = :BlogGuid ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":BlogGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = blogGuid.ToString();

            arParams[1] = new SqliteParameter(":CommentCount", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = commentCount;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("INSERT INTO mp_BlogComments (ModuleID, ItemID, Name, Title, URL, Comment, DateCreated)");
            sqlCommand.Append(" VALUES (");

            sqlCommand.Append(" :ModuleID , ");
            sqlCommand.Append(" :ItemID  , ");
            sqlCommand.Append(" :Name , ");
            sqlCommand.Append(" :Title , ");
            sqlCommand.Append(" :URL , ");
            sqlCommand.Append(" :Comment  , ");
            sqlCommand.Append(" :DateCreated ");

            sqlCommand.Append(");    ");

            SqliteParameter[] arParams = new SqliteParameter[7];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            arParams[2] = new SqliteParameter(":Name", DbType.String, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = name;

            arParams[3] = new SqliteParameter(":Title", DbType.String, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = title;

            arParams[4] = new SqliteParameter(":URL", DbType.String, 200);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = url;

            arParams[5] = new SqliteParameter(":Comment", DbType.Object);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = comment;

            arParams[6] = new SqliteParameter(":DateCreated", DbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = dateCreated;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("Update mp_Blogs ");
            sqlCommand.Append("SET CommentCount = CommentCount + 1 ");
            sqlCommand.Append("WHERE ModuleID = :ModuleID AND ItemID = :ItemID ;");

            arParams = new SqliteParameter[2];
            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("Update mp_BlogStats ");
            sqlCommand.Append("SET CommentCount = CommentCount + 1 ");
            sqlCommand.Append("WHERE ModuleID = :ModuleID  ;");

            arParams = new SqliteParameter[1];
            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public static bool DeleteAllCommentsForBlog(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE ");
            sqlCommand.Append("FROM	mp_BlogComments ");

            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateCommentStats(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_BlogStats ");
            sqlCommand.Append("SET 	CommentCount = (SELECT COUNT(*) FROM mp_BlogComments WHERE ModuleID = :ModuleID) ");

            sqlCommand.Append("WHERE ModuleID = :ModuleID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateEntryStats(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_BlogStats ");
            sqlCommand.Append("SET 	EntryCount = (SELECT COUNT(*) FROM mp_Blogs WHERE ModuleID = :ModuleID) ");

            sqlCommand.Append("WHERE ModuleID = :ModuleID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
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

            sqlCommand.Append("WHERE BlogCommentID = :BlogCommentID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":BlogCommentID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = blogCommentId;

            int moduleId = 0;
            int itemId = 0;

            using (IDataReader reader = SqliteHelper.ExecuteReader(
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
            sqlCommand.Append("WHERE BlogCommentID = :BlogCommentID ;");

            arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":BlogCommentID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = blogCommentId;

            int rowsAffected = 0;

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            if (moduleId > 0)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("UPDATE mp_Blogs ");
                sqlCommand.Append("SET CommentCount = CommentCount - 1 ");
                sqlCommand.Append("WHERE ModuleID = :ModuleID AND ItemID = :ItemID ;");

                sqlCommand.Append("UPDATE mp_BlogStats ");
                sqlCommand.Append("SET CommentCount = CommentCount - 1 ");
                sqlCommand.Append("WHERE ModuleID = :ModuleID  ;");

                arParams = new SqliteParameter[2];

                arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                arParams[1] = new SqliteParameter(":ItemID", DbType.Int32);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = itemId;

                SqliteHelper.ExecuteNonQuery(
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
            sqlCommand.Append("WHERE ModuleID = :ModuleID AND ItemID = :ItemID  ");
            sqlCommand.Append("ORDER BY BlogCommentID,  DateCreated DESC  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }


        public static int AddBlogCategory(
            int moduleId,
            string category)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_BlogCategories (ModuleID, Category)");
            sqlCommand.Append(" VALUES (");

            sqlCommand.Append(" :ModuleID , ");
            sqlCommand.Append(" :Category   ");

            sqlCommand.Append(");    ");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");


            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":Category", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = category;

            int newID = 0;

            newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

        }

        public static bool UpdateBlogCategory(
            int categoryId,
            string category)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_BlogCategories ");
            sqlCommand.Append(" SET  ");
            sqlCommand.Append("Category =  :Category   ");

            sqlCommand.Append("WHERE CategoryID = :CategoryID;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":CategoryID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;

            arParams[1] = new SqliteParameter(":Category", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = category;

            int rowsAffected = 0;

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteCategory(int categoryId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogItemCategories ");
            sqlCommand.Append("WHERE CategoryID = :CategoryID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":CategoryID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;

            int rowsAffected = 0;

            SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogCategories ");
            sqlCommand.Append("WHERE CategoryID = :CategoryID ;");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
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
            sqlCommand.Append("WHERE CategoryID = :CategoryID;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":CategoryID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetCategories(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bc.CategoryID As CategoryID, ");
            sqlCommand.Append("bc.Category As Category, ");
            sqlCommand.Append("COUNT(bic.ItemID) As PostCount ");
            sqlCommand.Append("FROM mp_BlogCategories bc ");
            sqlCommand.Append("JOIN	mp_BlogItemCategories bic ");
            sqlCommand.Append("ON bc.CategoryID = bic.CategoryID ");

            sqlCommand.Append("JOIN	mp_Blogs b ");
            sqlCommand.Append("ON b.ItemID = bic.ItemID ");

            sqlCommand.Append("WHERE bc.ModuleID = :ModuleID ");
            sqlCommand.Append("AND b.IsPublished = 1 ");
            sqlCommand.Append("AND b.StartDate <= :CurrentDate ");
            sqlCommand.Append("AND (b.EndDate IS NULL OR b.EndDate > :CurrentDate) ");

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append(" bc.CategoryID, bc.Category ");
            sqlCommand.Append("ORDER BY bc.Category ");
            sqlCommand.Append(" ; ");


            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":CurrentDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetCategoriesList(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("bc.CategoryID As CategoryID, ");
            sqlCommand.Append("bc.Category As Category, ");
            sqlCommand.Append("COUNT(bic.ItemID) As PostCount ");
            sqlCommand.Append("FROM mp_BlogCategories bc ");
            sqlCommand.Append("LEFT OUTER JOIN mp_BlogItemCategories bic ");
            sqlCommand.Append("ON bc.CategoryID = bic.CategoryID ");
            sqlCommand.Append("WHERE bc.ModuleID = :ModuleID  ");
            sqlCommand.Append("GROUP BY bc.CategoryID, bc.Category;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static int AddBlogItemCategory(
            int itemId,
            int categoryId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_BlogItemCategories (ItemID, CategoryID)");
            sqlCommand.Append(" VALUES (");

            sqlCommand.Append(" :ItemID , ");
            sqlCommand.Append(" :CategoryID   ");

            sqlCommand.Append(");    ");
            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");


            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqliteParameter(":CategoryID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            int newID = 0;

            newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

        }


        public static bool DeleteItemCategories(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_BlogItemCategories ");
            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = 0;

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static IDataReader GetBlogItemCategories(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  bic.ItemID As ItemID, ");
            sqlCommand.Append("bic.CategoryID As CategoryID, ");
            sqlCommand.Append("bc.Category As Category ");
            sqlCommand.Append("FROM	mp_BlogItemCategories bic ");
            sqlCommand.Append("JOIN	mp_BlogCategories bc ");
            sqlCommand.Append("ON bc.CategoryID = bic.CategoryID ");
            sqlCommand.Append("WHERE bic.ItemID = :ItemID   ");
            sqlCommand.Append("ORDER BY bc.Category;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        
    }
}
