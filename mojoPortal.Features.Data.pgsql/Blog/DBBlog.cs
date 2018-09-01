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

using Npgsql;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text;

namespace mojoPortal.Data
{
	public static class DBBlog
    {

        public static IDataReader GetBlogs(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT settingvalue ");
            sqlCommand.Append("FROM mp_modulesettings ");
            sqlCommand.Append("WHERE settingname = 'BlogEntriesToShowSetting' ");
            sqlCommand.Append("AND moduleid = :moduleid ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsToShow = int.Parse(ConfigurationManager.AppSettings["DefaultBlogPageSize"]);

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  b.*, ");
            sqlCommand.Append("u.name, ");
            sqlCommand.Append("u.loginname, ");
            sqlCommand.Append("u.email ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON b.userguid = u.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("b.moduleid = :moduleid ");
            sqlCommand.Append("AND :begindate >= b.startdate  ");
            sqlCommand.Append("AND b.ispublished = true ");
            sqlCommand.Append("AND b.startdate <= :currenttime  ");
            sqlCommand.Append("AND (b.enddate IS NULL OR b.enddate > :currenttime) ");

            sqlCommand.Append("ORDER BY  b.startdate DESC  ");
            sqlCommand.Append("LIMIT " + rowsToShow.ToString(CultureInfo.InvariantCulture) + " ; ");

            sqlCommand.Append(";");

            arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("begindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("SELECT b.*, ");
            sqlCommand.Append("u.name, ");
            sqlCommand.Append("u.loginname, ");
            sqlCommand.Append("u.email ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON b.userguid = u.userguid ");

            sqlCommand.Append("WHERE b.itemid <> :itemid  ");
            sqlCommand.Append("AND b.startdate <= :currenttime  ");
            sqlCommand.Append("AND (b.enddate IS NULL OR b.enddate > :currenttime) ");

            sqlCommand.Append("AND b.itemid IN (");
            sqlCommand.Append("SELECT itemid FROM mp_blogitemcategories WHERE categoryid IN ( ");
            sqlCommand.Append("SELECT categoryid FROM mp_blogitemcategories WHERE itemid = :itemid ");
            sqlCommand.Append(") ) ");

            sqlCommand.Append("ORDER BY  b.startdate DESC  ");
            sqlCommand.Append("LIMIT 20  ");
            sqlCommand.Append(" ;  ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetBlogsForFeed(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT settingvalue ");
            sqlCommand.Append("FROM mp_modulesettings ");
            sqlCommand.Append("WHERE settingname = 'MaxFeedItems' ");
            sqlCommand.Append("AND moduleid = :moduleid ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsToShow = 20;

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  b.*, ");
            sqlCommand.Append("u.name, ");
            sqlCommand.Append("u.loginname, ");
            sqlCommand.Append("u.email ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON b.userguid = u.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("b.moduleid = :moduleid ");
            sqlCommand.Append("AND :begindate >= b.startdate  ");
            sqlCommand.Append("AND b.ispublished = true ");
            sqlCommand.Append("AND b.startdate <= :currenttime  ");
            sqlCommand.Append("AND (b.enddate IS NULL OR b.enddate > :currenttime) ");

            sqlCommand.Append("ORDER BY  b.startdate DESC  ");
            sqlCommand.Append("LIMIT " + rowsToShow.ToString(CultureInfo.InvariantCulture) + " ; ");

            sqlCommand.Append(";");

            arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("begindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetBlogsForMetaWeblogApi(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT settingvalue ");
            sqlCommand.Append("FROM mp_modulesettings ");
            sqlCommand.Append("WHERE settingname = 'MaxMetaweblogRecentItems' ");
            sqlCommand.Append("AND moduleid = :moduleid ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsToShow = 100;

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  b.*, ");
            sqlCommand.Append("u.name, ");
            sqlCommand.Append("u.loginname, ");
            sqlCommand.Append("u.email ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON b.userguid = u.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("b.moduleid = :moduleid ");
            sqlCommand.Append("AND :begindate >= b.startdate  ");
            //sqlCommand.Append("AND b.ispublished = true ");
            sqlCommand.Append("AND b.startdate <= :currenttime  ");

            sqlCommand.Append("ORDER BY  b.startdate DESC  ");
            sqlCommand.Append("LIMIT " + rowsToShow.ToString(CultureInfo.InvariantCulture) + " ; ");

            sqlCommand.Append(";");

            arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("begindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetBlogCategoriesForMetaWeblogApi(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT settingvalue ");
            sqlCommand.Append("FROM mp_modulesettings ");
            sqlCommand.Append("WHERE settingname = 'MaxMetaweblogRecentItems' ");
            sqlCommand.Append("AND moduleid = :moduleid ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsToShow = 100;

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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

            sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.id, ");
            sqlCommand.Append("bic.itemid, ");
            sqlCommand.Append("bic.categoryid, ");
            sqlCommand.Append("bc.category ");

            sqlCommand.Append("FROM mp_blogitemcategories bic ");

            sqlCommand.Append("JOIN	mp_blogcategories bc ");
            sqlCommand.Append("ON bc.categoryid = bic.categoryid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.itemid IN (");

            sqlCommand.Append("SELECT  b.itemid ");
            
            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("b.moduleid = :moduleid ");
            sqlCommand.Append("AND :begindate >= b.startdate  ");
            //sqlCommand.Append("AND b.ispublished = true ");
            sqlCommand.Append("AND b.startdate <= :currenttime  ");

            sqlCommand.Append("ORDER BY  b.startdate DESC  ");
            sqlCommand.Append("LIMIT " + rowsToShow.ToString(CultureInfo.InvariantCulture) + " ");
            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY bc.category ");
            sqlCommand.Append(" ;  ");

            arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("begindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("FROM	mp_blogs ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid = :moduleid ");
            sqlCommand.Append("AND enddate < :currenttime ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            arParams[2] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  b.*, ");
            sqlCommand.Append("COALESCE(u.userid, -1) AS userid, ");
            sqlCommand.Append("u.name, ");
            sqlCommand.Append("u.firstname, ");
            sqlCommand.Append("u.lastname, ");
            sqlCommand.Append("u.loginname, ");
            sqlCommand.Append("u.email, ");
            sqlCommand.Append("u.avatarurl, ");
            sqlCommand.Append("u.authorbio ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON b.userguid = u.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("b.moduleid = :moduleid ");
            sqlCommand.Append("AND b.enddate < :currenttime ");

            sqlCommand.Append("ORDER BY  b.startdate DESC  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            arParams[2] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.*, ");
            sqlCommand.Append("b2.showdownloadlink ");

            sqlCommand.Append("FROM mp_fileattachment bic ");

            sqlCommand.Append("JOIN mp_blogs b2 ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b2.blogguid = bic.itemguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.itemguid IN (");

            sqlCommand.Append("SELECT  b.blogguid ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("b.moduleid = :moduleid ");
           
            sqlCommand.Append("AND b.enddate < :currenttime ");

            sqlCommand.Append("ORDER BY  b.startdate DESC  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(") ");


            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            arParams[2] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.id, ");
            sqlCommand.Append("bic.itemid, ");
            sqlCommand.Append("bic.categoryid, ");
            sqlCommand.Append("bc.category ");

            sqlCommand.Append("FROM mp_blogitemcategories bic ");

            sqlCommand.Append("JOIN	mp_blogcategories bc ");
            sqlCommand.Append("ON bc.categoryid = bic.categoryid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.itemid IN (");

            sqlCommand.Append("SELECT  b.itemid ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("b.moduleid = :moduleid ");
            sqlCommand.Append("AND b.enddate < :currenttime ");

            sqlCommand.Append("ORDER BY  b.startdate DESC  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY bc.category ");
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("FROM	mp_blogs ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid = :moduleid ");
            sqlCommand.Append("AND (:userguid = '00000000-0000-0000-0000-000000000000' OR userguid  = :userguid)  ");
            sqlCommand.Append("AND ((startdate > :currenttime) OR (ispublished = false)) ");
            
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid;

            arParams[2] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            arParams[3] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageSize;

            arParams[4] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  b.*, ");
            sqlCommand.Append("COALESCE(u.userid, -1) AS userid, ");
            sqlCommand.Append("u.name, ");
            sqlCommand.Append("u.firstname, ");
            sqlCommand.Append("u.lastname, ");
            sqlCommand.Append("u.loginname, ");
            sqlCommand.Append("u.email, ");
            sqlCommand.Append("u.avatarurl, ");
            sqlCommand.Append("u.authorbio ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON b.userguid = u.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("b.moduleid = :moduleid ");
            sqlCommand.Append("AND (:userguid = '00000000-0000-0000-0000-000000000000' OR b.userguid  = :userguid)  ");
            sqlCommand.Append("AND ((b.startdate > :currenttime) OR (b.ispublished = false)) ");

            sqlCommand.Append("ORDER BY  b.startdate DESC  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("FROM	mp_blogs ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid = :moduleid ");
            sqlCommand.Append("AND :begindate >= startdate  ");
            sqlCommand.Append("AND ispublished = true ");
            sqlCommand.Append("AND startdate <= :currenttime  ");
            sqlCommand.Append("AND (enddate IS NULL OR enddate > :currenttime) ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("begindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("begindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            arParams[3] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageSize;

            arParams[4] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  b.*, ");
            sqlCommand.Append("COALESCE(u.userid, -1) AS userid, ");
            sqlCommand.Append("u.name, ");
            sqlCommand.Append("u.firstname, ");
            sqlCommand.Append("u.lastname, ");
            sqlCommand.Append("u.loginname, ");
            sqlCommand.Append("u.email, ");
            sqlCommand.Append("u.avatarurl, ");
            sqlCommand.Append("u.authorbio ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON b.userguid = u.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("b.moduleid = :moduleid ");
            sqlCommand.Append("AND :begindate >= b.startdate  ");
            sqlCommand.Append("AND b.ispublished = true ");
            sqlCommand.Append("AND b.startdate <= :currenttime  ");
            sqlCommand.Append("AND (b.enddate IS NULL OR b.enddate > :currenttime) ");

            sqlCommand.Append("ORDER BY  b.startdate DESC  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("begindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            arParams[3] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageSize;

            arParams[4] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.*, ");
            sqlCommand.Append("b2.showdownloadlink ");

            sqlCommand.Append("FROM mp_fileattachment bic ");

            sqlCommand.Append("JOIN mp_blogs b2 ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b2.blogguid = bic.itemguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.itemguid IN (");

            sqlCommand.Append("SELECT  b.blogguid ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("b.moduleid = :moduleid ");
            sqlCommand.Append("AND :begindate >= b.startdate  ");
            sqlCommand.Append("AND b.ispublished = true ");
            sqlCommand.Append("AND b.startdate <= :currenttime  ");
            sqlCommand.Append("AND (b.enddate IS NULL OR b.enddate > :currenttime) ");

            sqlCommand.Append("ORDER BY  b.startdate DESC  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(") ");

           
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("categoryid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            arParams[3] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageSize;

            arParams[4] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.*, ");
            sqlCommand.Append("b2.showdownloadlink ");

            sqlCommand.Append("FROM mp_fileattachment bic ");

            sqlCommand.Append("JOIN mp_blogs b2 ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b2.blogguid = bic.itemguid ");

           
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.itemguid IN (");

            sqlCommand.Append("SELECT  b.blogguid ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("b.moduleid = :moduleid ");
            sqlCommand.Append("AND b.itemid IN (SELECT itemid FROM mp_blogitemcategories WHERE categoryid = :categoryid ) ");
            sqlCommand.Append("AND b.ispublished = true ");
            sqlCommand.Append("AND b.startdate <= :currenttime  ");
            sqlCommand.Append("AND (b.enddate IS NULL OR b.enddate > :currenttime) ");

            sqlCommand.Append("ORDER BY  b.startdate DESC  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(") ");

            //sqlCommand.Append("ORDER BY bc.category ");
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            NpgsqlParameter[] arParams = new NpgsqlParameter[6];

            arParams[0] = new NpgsqlParameter("month", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = month;

            arParams[1] = new NpgsqlParameter("year", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = year;

            arParams[2] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleId;

            arParams[3] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currentTime;

            arParams[4] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageSize;

            arParams[5] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.*, ");
            sqlCommand.Append("b2.showdownloadlink ");

            sqlCommand.Append("FROM mp_fileattachment bic ");

            sqlCommand.Append("JOIN mp_blogs b2 ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b2.blogguid = bic.itemguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.itemguid IN (");

            sqlCommand.Append("SELECT  b.blogguid ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("WHERE b.moduleid = :moduleid  ");
            sqlCommand.Append("AND b.ispublished = true ");
            sqlCommand.Append("AND b.startdate <= :currenttime ");
            sqlCommand.Append("AND (b.enddate IS NULL OR b.enddate > :currenttime) ");

            sqlCommand.Append("AND date_part('year', b.startdate) = :year  ");
            sqlCommand.Append(" AND date_part('month', b.startdate)  = :month  ");

            sqlCommand.Append("ORDER BY  b.startdate DESC  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(") ");

            //sqlCommand.Append("ORDER BY bc.category ");
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("begindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            arParams[2] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            arParams[3] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageSize;

            arParams[4] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.id, ");
            sqlCommand.Append("bic.itemid, ");
            sqlCommand.Append("bic.categoryid, ");
            sqlCommand.Append("bc.category ");

            sqlCommand.Append("FROM mp_blogitemcategories bic ");

            sqlCommand.Append("JOIN	mp_blogcategories bc ");
            sqlCommand.Append("ON bc.categoryid = bic.categoryid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.itemid IN (");

            sqlCommand.Append("SELECT  b.itemid ");
            
            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("b.moduleid = :moduleid ");
            sqlCommand.Append("AND :begindate >= b.startdate  ");
            sqlCommand.Append("AND b.ispublished = true ");
            sqlCommand.Append("AND b.startdate <= :currenttime  ");
            sqlCommand.Append("AND (b.enddate IS NULL OR b.enddate > :currenttime) ");

            sqlCommand.Append("ORDER BY  b.startdate DESC  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY bc.category ");
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("FROM	mp_blogs ");

            sqlCommand.Append("WHERE moduleid = :moduleid  ");
            sqlCommand.Append("AND ispublished = true ");
            sqlCommand.Append("AND startdate <= :currenttime ");
            sqlCommand.Append("AND (enddate IS NULL OR enddate > :currenttime) ");

            sqlCommand.Append("AND date_part('year', startdate) = :year  ");
            sqlCommand.Append(" AND date_part('month', startdate)  = :month  ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("month", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = month;

            arParams[1] = new NpgsqlParameter("year", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = year;

            arParams[2] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleId;

            arParams[3] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currentTime;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            NpgsqlParameter[] arParams = new NpgsqlParameter[6];

            arParams[0] = new NpgsqlParameter("month", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = month;

            arParams[1] = new NpgsqlParameter("year", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = year;

            arParams[2] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleId;

            arParams[3] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currentTime;

            arParams[4] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageSize;

            arParams[5] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.id, ");
            sqlCommand.Append("bic.itemid, ");
            sqlCommand.Append("bic.categoryid, ");
            sqlCommand.Append("bc.category ");

            sqlCommand.Append("FROM mp_blogitemcategories bic ");

            sqlCommand.Append("JOIN	mp_blogcategories bc ");
            sqlCommand.Append("ON bc.categoryid = bic.categoryid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.itemid IN (");

            sqlCommand.Append("SELECT  b.itemid ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("WHERE b.moduleid = :moduleid  ");
            sqlCommand.Append("AND b.ispublished = true ");
            sqlCommand.Append("AND b.startdate <= :currenttime ");
            sqlCommand.Append("AND (b.enddate IS NULL OR b.enddate > :currenttime) ");

            sqlCommand.Append("AND date_part('year', b.startdate) = :year  ");
            sqlCommand.Append(" AND date_part('month', b.startdate)  = :month  ");

            sqlCommand.Append("ORDER BY  b.startdate DESC  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY bc.category ");
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            
            sqlCommand.Append("SELECT  b.*, ");
            sqlCommand.Append("COALESCE(u.userid, -1) AS userid, ");
            sqlCommand.Append("u.name, ");
            sqlCommand.Append("u.firstname, ");
            sqlCommand.Append("u.lastname, ");
            sqlCommand.Append("u.loginname, ");
            sqlCommand.Append("u.email, ");
            sqlCommand.Append("u.avatarurl, ");
            sqlCommand.Append("u.authorbio ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON b.userguid = u.userguid ");

            sqlCommand.Append("WHERE b.moduleid = :moduleid  ");
            sqlCommand.Append("AND b.ispublished = true ");
            sqlCommand.Append("AND b.startdate <= :currenttime ");
            sqlCommand.Append("AND (b.enddate IS NULL OR b.enddate > :currenttime) ");

            sqlCommand.Append("AND date_part('year', b.startdate) = :year  ");
            sqlCommand.Append(" AND date_part('month', b.startdate)  = :month  ");

            sqlCommand.Append("ORDER BY startdate DESC ");

            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[6];

            arParams[0] = new NpgsqlParameter("month", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = month;

            arParams[1] = new NpgsqlParameter("year", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = year;

            arParams[2] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleId;

            arParams[3] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currentTime;

            arParams[4] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageSize;

            arParams[5] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = pageLowerBound;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetBlogEntriesByMonth(int month, int year, int moduleId, DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");

            sqlCommand.Append("FROM	mp_blogs ");

            sqlCommand.Append("WHERE moduleid = :moduleid  ");
            sqlCommand.Append("AND ispublished = true ");
            sqlCommand.Append("AND startdate <= :currenttime ");
            sqlCommand.Append("AND (enddate IS NULL OR enddate > :currenttime) ");

            sqlCommand.Append("AND date_part('year', startdate) = :year  ");
            sqlCommand.Append(" AND date_part('month', startdate)  = :month  ");

            sqlCommand.Append("ORDER BY startdate DESC ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("month", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = month;

            arParams[1] = new NpgsqlParameter("year", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = year;

            arParams[2] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleId;

            arParams[3] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = currentTime;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("FROM	mp_blogs ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid = :moduleid ");
            sqlCommand.Append("AND ispublished = true ");
            sqlCommand.Append("AND startdate <= :currenttime ");
            sqlCommand.Append("AND (enddate IS NULL OR enddate > :currenttime) ");
            sqlCommand.Append("AND itemid IN (SELECT itemid FROM mp_blogitemcategories WHERE categoryid = :categoryid ) ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("categoryid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("SELECT  b.*, ");
            sqlCommand.Append("COALESCE(u.userid, -1) AS userid, ");
            sqlCommand.Append("u.name, ");
            sqlCommand.Append("u.firstname, ");
            sqlCommand.Append("u.lastname, ");
            sqlCommand.Append("u.loginname, ");
            sqlCommand.Append("u.email, ");
            sqlCommand.Append("u.avatarurl, ");
            sqlCommand.Append("u.authorbio ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON b.userguid = u.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("b.moduleid = :moduleid ");
            sqlCommand.Append("AND b.ispublished = true ");
            sqlCommand.Append("AND b.startdate <= :currenttime ");
            sqlCommand.Append("AND (b.enddate IS NULL OR b.enddate > :currenttime) ");
            sqlCommand.Append("AND b.itemid IN (SELECT itemid FROM mp_blogitemcategories WHERE categoryid = :categoryid ) ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("b.startdate DESC ");

            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("categoryid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            arParams[3] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageSize;

            arParams[4] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageLowerBound;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("categoryid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            arParams[3] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageSize;

            arParams[4] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bic.id, ");
            sqlCommand.Append("bic.itemid, ");
            sqlCommand.Append("bic.categoryid, ");
            sqlCommand.Append("bc.category ");

            sqlCommand.Append("FROM mp_blogitemcategories bic ");

            sqlCommand.Append("JOIN	mp_blogcategories bc ");
            sqlCommand.Append("ON bc.categoryid = bic.categoryid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("bic.itemid IN (");

            sqlCommand.Append("SELECT  b.itemid ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("b.moduleid = :moduleid ");
            sqlCommand.Append("AND b.itemid IN (SELECT itemid FROM mp_blogitemcategories WHERE categoryid = :categoryid ) ");
            sqlCommand.Append("AND b.ispublished = true ");
            sqlCommand.Append("AND b.startdate <= :currenttime  ");
            sqlCommand.Append("AND (b.enddate IS NULL OR b.enddate > :currenttime) ");

            sqlCommand.Append("ORDER BY  b.startdate DESC  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY bc.category ");
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetEntriesByCategory(int moduleId, int categoryId, DateTime currentTime)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_blogs ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid = :moduleid ");
            sqlCommand.Append("AND ispublished = true ");
            sqlCommand.Append("AND startdate <= :currenttime ");
            sqlCommand.Append("AND (enddate IS NULL OR enddate > :currenttime) ");
            sqlCommand.Append("AND itemid IN (SELECT itemid FROM mp_blogitemcategories WHERE categoryid = :categoryid ) ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("startdate DESC ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("categoryid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            arParams[2] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentTime;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }



        public static IDataReader GetBlogsForSiteMap(int siteId, DateTime currentUtcDateTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("b.itemurl, ");
            sqlCommand.Append("b.lastmodutc, ");
            sqlCommand.Append("b.itemid, ");
            sqlCommand.Append("b.moduleid, ");
            sqlCommand.Append("pm.pageid ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("JOIN mp_modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b.moduleid = m.moduleid ");

            sqlCommand.Append("JOIN mp_pagemodules pm ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b.moduleid = pm.moduleid ");

            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("m.siteid = :siteid ");
            sqlCommand.Append("AND b.includeinsitemap = true ");
            sqlCommand.Append("AND b.IsPublished = true ");
            sqlCommand.Append("AND b.startdate <= :begindate  ");
            sqlCommand.Append("AND (b.enddate IS NULL OR b.enddate > :begindate) ");
            sqlCommand.Append("AND b.itemurl <> ''  ");

            sqlCommand.Append("ORDER BY b.startdate DESC  ");
            sqlCommand.Append("");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("begindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentUtcDateTime;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetBlogsForNewsMap(int siteId, DateTime utcThresholdTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("b.itemurl, ");
            sqlCommand.Append("b.lastmodutc, ");
            sqlCommand.Append("b.itemid, ");
            sqlCommand.Append("b.moduleid, ");

            sqlCommand.Append("b.headlineimageurl, ");
            sqlCommand.Append("b.pubaccess, ");
            sqlCommand.Append("b.pubgenres, ");
            sqlCommand.Append("b.pubgeolocations, ");
            sqlCommand.Append("b.pubkeywords, ");
            sqlCommand.Append("b.publanguage,");
            sqlCommand.Append("b.pubname, ");
            sqlCommand.Append("b.pubstocktickers, ");
            sqlCommand.Append("b.startdate,");
            sqlCommand.Append("b.title, ");
            sqlCommand.Append("b.heading, ");

            sqlCommand.Append("pm.pageid ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("JOIN mp_modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b.moduleid = m.moduleid ");

            sqlCommand.Append("JOIN mp_pagemodules pm ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("b.moduleid = pm.moduleid ");

            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("m.siteid = :siteid ");
            sqlCommand.Append("AND b.includeinnews = true ");
            sqlCommand.Append("AND b.ispublished = true ");
            sqlCommand.Append("AND b.startdate >= :begindate  ");
            
            sqlCommand.Append("AND b.itemurl <> ''  ");

            sqlCommand.Append("ORDER BY b.startdate DESC  ");
            sqlCommand.Append("");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("begindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = utcThresholdTime;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetDrafts(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_blogs ");
            sqlCommand.Append("WHERE moduleid = :moduleid  ");
            sqlCommand.Append("AND ((startdate > :begindate) OR (ispublished = false))  ");

            sqlCommand.Append("ORDER BY startdate DESC  ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("begindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

           
        }

        public static IDataReader GetBlogsByPage(int siteId, int pageId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("pageid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  b.*, ");

            sqlCommand.Append("m.moduletitle, ");
            sqlCommand.Append("m.viewroles, ");
            sqlCommand.Append("md.featurename, ");

            sqlCommand.Append("COALESCE(u.userid, -1) AS userid, ");
            sqlCommand.Append("u.name, ");
            sqlCommand.Append("u.firstname, ");
            sqlCommand.Append("u.lastname, ");
            sqlCommand.Append("u.loginname, ");
            sqlCommand.Append("u.email, ");
            sqlCommand.Append("u.avatarurl ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("JOIN	mp_modules m ");
            sqlCommand.Append("ON b.moduleid = m.moduleid ");

            sqlCommand.Append("JOIN	mp_moduledefinitions md ");
            sqlCommand.Append("ON m.moduledefid = md.moduledefid ");

            sqlCommand.Append("JOIN	mp_pagemodules pm ");
            sqlCommand.Append("ON m.moduleid = pm.moduleid ");

            sqlCommand.Append("JOIN	mp_pages p ");
            sqlCommand.Append("ON p.pageid = pm.pageid ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON b.userguid = u.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.siteid = :siteid ");
            sqlCommand.Append("AND pm.pageid = :pageid ");
            
            sqlCommand.Append(" ; ");
            
            
            
            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetBlogStats(int moduleId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_blogstats_select(:moduleid)",
                arParams);
        }


        public static IDataReader GetBlogMonthArchive(int moduleId, DateTime currentTime)
        {
            
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("currentdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_blog_selectarchivebymonth(:moduleid,:currentdate)",
                arParams);
        }


        

        


        public static IDataReader GetSingleBlog(int itemId, DateTime currentTime)
        {
            //sqlCommand.Append("AND (b.enddate IS NULL OR b.enddate > :currenttime) ");
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT b.*, ");

            sqlCommand.Append("(SELECT b2.itemurl FROM mp_blogs b2 WHERE b2.ispublished = true AND b2.startdate <= :currenttime AND (b2.enddate IS NULL OR b2.enddate > :currenttime) AND b2.startdate > b.startdate AND b2.moduleid = b.moduleid AND b2.itemurl IS NOT NULL AND b2.itemurl <> '' ORDER BY b2.startdate LIMIT 1) AS nextpost, ");
            sqlCommand.Append("(SELECT b2.title FROM mp_blogs b2 WHERE b2.ispublished = true AND b2.startdate <= :currenttime AND (b2.enddate IS NULL OR b2.enddate > :currenttime) AND b2.startdate > b.startdate AND b2.moduleid = b.moduleid AND b2.itemurl IS NOT NULL AND b2.itemurl <> '' ORDER BY b2.startdate LIMIT 1) AS nextposttitle, ");
            sqlCommand.Append("COALESCE((SELECT b4.itemid FROM mp_blogs b4 WHERE b4.ispublished = true AND b4.startdate <= :currenttime AND (b4.enddate IS NULL OR b4.enddate > :currenttime) AND b4.startdate > b.startdate AND b4.moduleid = b.moduleid  ORDER BY b4.startdate LIMIT 1),-1) AS nextitemid, ");


            sqlCommand.Append(" (SELECT b3.itemurl FROM mp_blogs b3 WHERE b3.ispublished = true AND b3.startdate <= :currenttime AND (b3.enddate IS NULL OR b3.enddate > :currenttime) AND b3.startdate < b.startdate AND b3.moduleid = b.moduleid AND b3.itemurl IS NOT NULL AND b3.itemurl <> '' ORDER BY b3.startdate DESC LIMIT 1) As previouspost, ");
            sqlCommand.Append(" (SELECT b3.title FROM mp_blogs b3 WHERE b3.ispublished = true AND b3.startdate <= :currenttime AND (b3.enddate IS NULL OR b3.enddate > :currenttime) AND b3.startdate < b.startdate AND b3.moduleid = b.moduleid AND b3.itemurl IS NOT NULL AND b3.itemurl <> '' ORDER BY b3.startdate DESC LIMIT 1) As previousposttitle, ");
            sqlCommand.Append(" COALESCE((SELECT b5.itemid FROM mp_blogs b5 WHERE b5.ispublished = true AND b5.startdate <= :currenttime AND (b5.enddate IS NULL OR b5.enddate > :currenttime) AND b5.startdate < b.startdate AND b5.moduleid = b.moduleid  ORDER BY b5.startdate DESC LIMIT 1),-1) As previousitemid, ");


            sqlCommand.Append("COALESCE(u.userid, -1) AS userid, ");
            sqlCommand.Append("u.name, ");
            sqlCommand.Append("u.firstname, ");
            sqlCommand.Append("u.lastname, ");
            sqlCommand.Append("u.loginname, ");
            sqlCommand.Append("u.email, ");
            sqlCommand.Append("u.avatarurl, ");
            sqlCommand.Append("u.authorbio ");

            sqlCommand.Append("FROM	mp_blogs b ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON b.userguid = u.userguid ");

            sqlCommand.Append("WHERE b.itemid = :itemid  ");

            sqlCommand.Append("");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = currentTime;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public static bool DeleteBlog(int itemId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_blog_delete(:itemid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(int moduleId)
        {
            
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_blogitemcategories ");
            sqlCommand.Append("WHERE itemid IN (SELECT itemid FROM mp_blogs WHERE moduleid ");
            sqlCommand.Append(" = :moduleid ) ");
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_friendlyurls ");
            sqlCommand.Append("WHERE pageguid IN (SELECT blogguid FROM mp_blogs WHERE moduleid ");
            sqlCommand.Append(" = :moduleid ) ");
            sqlCommand.Append(";");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_contenthistory ");
            sqlCommand.Append("WHERE contentguid IN (SELECT blogguid FROM mp_blogs WHERE moduleid ");
            sqlCommand.Append(" = :moduleid ) ");
            sqlCommand.Append(";");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_contentrating ");
            sqlCommand.Append("WHERE contentguid IN (SELECT blogguid FROM mp_blogs WHERE moduleid ");
            sqlCommand.Append(" = :moduleid ) ");
            sqlCommand.Append(";");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_blogcategories ");
            sqlCommand.Append("WHERE moduleid  = :moduleid ");
            sqlCommand.Append(";");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_blogstats ");
            sqlCommand.Append("WHERE moduleid = :moduleid ");
            sqlCommand.Append(";");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_blogcomments ");
            sqlCommand.Append("WHERE moduleid  = :moduleid ");
            sqlCommand.Append(";");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_blogs ");
            sqlCommand.Append("WHERE moduleid  = :moduleid ");
            sqlCommand.Append(";");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(int siteId)
        {

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_blogitemcategories ");
            sqlCommand.Append("WHERE itemid IN (SELECT itemid FROM mp_blogs WHERE moduleid IN ");
            sqlCommand.Append("(SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ) ");
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_friendlyurls ");
            sqlCommand.Append("WHERE pageguid IN (SELECT moduleguid FROM mp_blogs WHERE moduleid IN ");
            sqlCommand.Append("(SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ) ");
            sqlCommand.Append(";");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_friendlyurls ");
            sqlCommand.Append("WHERE pageguid IN (SELECT blogguid FROM mp_blogs WHERE moduleid IN ");
            sqlCommand.Append("(SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ) ");
            sqlCommand.Append(";");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_contenthistory ");
            sqlCommand.Append("WHERE contentguid IN (SELECT blogguid FROM mp_blogs WHERE moduleid IN ");
            sqlCommand.Append("(SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ) ");
            sqlCommand.Append(";");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_contentrating ");
            sqlCommand.Append("WHERE contentguid IN (SELECT blogguid FROM mp_blogs WHERE moduleid IN ");
            sqlCommand.Append("(SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ) ");
            sqlCommand.Append(";");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_blogcategories ");
            sqlCommand.Append("WHERE moduleid IN (SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ");
            sqlCommand.Append(";");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_blogstats ");
            sqlCommand.Append("WHERE moduleid IN (SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ");
            sqlCommand.Append(";");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_blogcomments ");
            sqlCommand.Append("WHERE moduleid IN (SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ");
            sqlCommand.Append(";");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_blogs ");
            sqlCommand.Append("WHERE moduleid IN (SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ");
            sqlCommand.Append(";");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
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
            bool includeImageInExcerpt,
			bool includeImageInPost
		)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_blogs (");
            sqlCommand.Append("moduleid, ");
            sqlCommand.Append("createdbyuser, ");
            sqlCommand.Append("createddate, ");
            sqlCommand.Append("title, ");
            sqlCommand.Append("startdate, ");
            sqlCommand.Append("isinnewsletter, ");
            sqlCommand.Append("description, ");
            sqlCommand.Append("commentcount, ");
            sqlCommand.Append("trackbackcount, ");

            sqlCommand.Append("includeinfeed, ");
            sqlCommand.Append("ispublished, ");
            sqlCommand.Append("allowcommentsfordays, ");
            sqlCommand.Append("blogguid, ");
            sqlCommand.Append("moduleguid, ");
            sqlCommand.Append("location, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("lastmoduserguid, ");
            sqlCommand.Append("lastmodutc, ");
            sqlCommand.Append("itemurl, ");
            sqlCommand.Append("heading, ");
            sqlCommand.Append("metakeywords, ");
            sqlCommand.Append("metadescription, ");
            sqlCommand.Append("compiledmeta, ");

            sqlCommand.Append("subtitle, ");
            sqlCommand.Append("enddate, ");
            sqlCommand.Append("approved, ");
            sqlCommand.Append("approveddate, ");
            sqlCommand.Append("approvedby, ");

            sqlCommand.Append("showauthorname, ");
            sqlCommand.Append("showauthoravatar, ");
            sqlCommand.Append("showauthorbio, ");
            sqlCommand.Append("includeinsearch, ");
            sqlCommand.Append("excludefromrecentcontent, ");

            sqlCommand.Append("includeinnews, ");
            sqlCommand.Append("pubname, ");
            sqlCommand.Append("publanguage, ");
            sqlCommand.Append("pubaccess, ");
            sqlCommand.Append("pubgenres, ");
            sqlCommand.Append("pubkeywords, ");
            sqlCommand.Append("pubgeolocations, ");
            sqlCommand.Append("pubstocktickers, ");
            sqlCommand.Append("headlineimageurl, ");
            sqlCommand.Append("includeimageinexcerpt, ");
            sqlCommand.Append("includeimageinpost, ");

			sqlCommand.Append("includeinsitemap, ");
            sqlCommand.Append("usebingmap, ");
            sqlCommand.Append("mapheight, ");
            sqlCommand.Append("mapwidth, ");
            sqlCommand.Append("showmapoptions, ");
            sqlCommand.Append("showzoomtool, ");
            sqlCommand.Append("showlocationinfo, ");
            sqlCommand.Append("usedrivingdirections, ");
            sqlCommand.Append("maptype, ");
            sqlCommand.Append("mapzoom, ");
            sqlCommand.Append("showdownloadlink, ");


            sqlCommand.Append("abstract )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":moduleid, ");
            sqlCommand.Append(":createdbyuser, ");
            sqlCommand.Append(":createddate, ");
            sqlCommand.Append(":title, ");
            sqlCommand.Append(":startdate, ");
            sqlCommand.Append(":isinnewsletter, ");
            sqlCommand.Append(":description, ");
            sqlCommand.Append(":commentcount, ");
            sqlCommand.Append(":trackbackcount, ");

            sqlCommand.Append(":includeinfeed, ");
            sqlCommand.Append(":ispublished, ");
            sqlCommand.Append(":allowcommentsfordays, ");
            sqlCommand.Append(":blogguid, ");
            sqlCommand.Append(":moduleguid, ");
            sqlCommand.Append(":location, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":lastmoduserguid, ");
            sqlCommand.Append(":lastmodutc, ");
            sqlCommand.Append(":itemurl, ");
            sqlCommand.Append(":heading, ");
            sqlCommand.Append(":metakeywords, ");
            sqlCommand.Append(":metadescription, ");
            sqlCommand.Append(":compiledmeta, ");
            sqlCommand.Append(":subtitle, ");
            sqlCommand.Append(":enddate, ");
            sqlCommand.Append(":approved, ");
            sqlCommand.Append(":approveddate, ");
            sqlCommand.Append(":approvedby, ");

            sqlCommand.Append(":showauthorname, ");
            sqlCommand.Append(":showauthoravatar, ");
            sqlCommand.Append(":showauthorbio, ");
            sqlCommand.Append(":includeinsearch, ");
            sqlCommand.Append(":excludefromrecentcontent, ");

            sqlCommand.Append(":includeinnews, ");
            sqlCommand.Append(":pubname, ");
            sqlCommand.Append(":publanguage, ");
            sqlCommand.Append(":pubaccess, ");
            sqlCommand.Append(":pubgenres, ");
            sqlCommand.Append(":pubkeywords, ");
            sqlCommand.Append(":pubgeolocations, ");
            sqlCommand.Append(":pubstocktickers, ");
            sqlCommand.Append(":headlineimageurl, ");
            sqlCommand.Append(":includeimageinexcerpt, ");
            sqlCommand.Append(":includeimageinpost, ");

			sqlCommand.Append(":includeinsitemap, ");
            sqlCommand.Append(":usebingmap, ");
            sqlCommand.Append(":mapheight, ");
            sqlCommand.Append(":mapwidth, ");
            sqlCommand.Append(":showmapoptions, ");
            sqlCommand.Append(":showzoomtool, ");
            sqlCommand.Append(":showlocationinfo, ");
            sqlCommand.Append(":usedrivingdirections, ");
            sqlCommand.Append(":maptype, ");
            sqlCommand.Append(":mapzoom, ");
            sqlCommand.Append(":showdownloadlink, ");

            sqlCommand.Append(":abstract )");
            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT CURRVAL('mp_blogs_itemid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[56];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("createdbyuser", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userName;

            arParams[2] = new NpgsqlParameter("createddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            arParams[3] = new NpgsqlParameter("title", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = title;

            arParams[4] = new NpgsqlParameter("startdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = startDate;

            arParams[5] = new NpgsqlParameter("isinnewsletter", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = isInNewsletter;

            arParams[6] = new NpgsqlParameter("description", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = description;

            arParams[7] = new NpgsqlParameter("commentcount", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = 0;

            arParams[8] = new NpgsqlParameter("trackbackcount", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = 0;

            arParams[9] = new NpgsqlParameter("includeinfeed", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = includeInFeed;

            arParams[10] = new NpgsqlParameter("allowcommentsfordays", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = allowCommentsForDays;

            arParams[11] = new NpgsqlParameter("blogguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = blogGuid.ToString();

            arParams[12] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = moduleGuid.ToString();

            arParams[13] = new NpgsqlParameter("location", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = location;

            arParams[14] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = userGuid.ToString();

            arParams[15] = new NpgsqlParameter("lastmoduserguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = userGuid.ToString();

            arParams[16] = new NpgsqlParameter("lastmodutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = DateTime.UtcNow;

            arParams[17] = new NpgsqlParameter("itemurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = itemUrl;

            arParams[18] = new NpgsqlParameter("heading", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = title;

            arParams[19] = new NpgsqlParameter("metakeywords", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = metaKeywords;

            arParams[20] = new NpgsqlParameter("metadescription", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = metaDescription;

            arParams[21] = new NpgsqlParameter("abstract", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = excerpt;

            arParams[22] = new NpgsqlParameter("compiledmeta", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = compiledMeta;

            arParams[23] = new NpgsqlParameter("ispublished", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = isPublished;

            arParams[24] = new NpgsqlParameter("subtitle", NpgsqlTypes.NpgsqlDbType.Varchar, 500);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = subTitle;

            arParams[25] = new NpgsqlParameter("enddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[25].Direction = ParameterDirection.Input;
            if (endDate < DateTime.MaxValue)
            {
                arParams[25].Value = endDate;
            }
            else
            {
                arParams[25].Value = DBNull.Value;
            }

            arParams[26] = new NpgsqlParameter("approved", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = approved;

            arParams[27] = new NpgsqlParameter("approveddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[27].Direction = ParameterDirection.Input;
            if (approvedDate < DateTime.MaxValue)
            {
                arParams[27].Value = approvedDate;
            }
            else
            {
                arParams[27].Value = DBNull.Value;
            }

            arParams[28] = new NpgsqlParameter("approvedby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = approvedBy.ToString();

            arParams[29] = new NpgsqlParameter("showauthorname", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = showAuthorName;

            arParams[30] = new NpgsqlParameter("showauthoravatar", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = showAuthorAvatar;

            arParams[31] = new NpgsqlParameter("showauthorbio", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = showAuthorBio;

            arParams[32] = new NpgsqlParameter("includeinsearch", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = includeInSearch;

            arParams[33] = new NpgsqlParameter("includeinsitemap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = includeInSiteMap;

            arParams[34] = new NpgsqlParameter("usebingmap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = useBingMap;

            arParams[35] = new NpgsqlParameter("mapheight", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = mapHeight;

            arParams[36] = new NpgsqlParameter("mapwidth", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = mapWidth;

            arParams[37] = new NpgsqlParameter("showmapoptions", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = showMapOptions;

            arParams[38] = new NpgsqlParameter("showzoomtool", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = showZoomTool;

            arParams[39] = new NpgsqlParameter("showlocationinfo", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = showLocationInfo;

            arParams[40] = new NpgsqlParameter("usedrivingdirections", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = useDrivingDirections;

            arParams[41] = new NpgsqlParameter("maptype", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = mapType;

            arParams[42] = new NpgsqlParameter("mapzoom", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = mapZoom;

            arParams[43] = new NpgsqlParameter("showdownloadlink", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = showDownloadLink;

            arParams[44] = new NpgsqlParameter("excludefromrecentcontent", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = excludeFromRecentContent;

            arParams[45] = new NpgsqlParameter("includeinnews", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = includeInNews;

            arParams[46] = new NpgsqlParameter("pubname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = pubName;

            arParams[47] = new NpgsqlParameter("publanguage", NpgsqlTypes.NpgsqlDbType.Varchar, 7);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = pubLanguage;

            arParams[48] = new NpgsqlParameter("pubaccess", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[48].Direction = ParameterDirection.Input;
            arParams[48].Value = pubAccess;

            arParams[49] = new NpgsqlParameter("pubgenres", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[49].Direction = ParameterDirection.Input;
            arParams[49].Value = pubGenres;

            arParams[50] = new NpgsqlParameter("pubkeywords", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[50].Direction = ParameterDirection.Input;
            arParams[50].Value = pubKeyWords;

            arParams[51] = new NpgsqlParameter("pubgeolocations", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[51].Direction = ParameterDirection.Input;
            arParams[51].Value = pubGeoLocations;

            arParams[52] = new NpgsqlParameter("pubstocktickers", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[52].Direction = ParameterDirection.Input;
            arParams[52].Value = pubStockTickers;

            arParams[53] = new NpgsqlParameter("headlineimageurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[53].Direction = ParameterDirection.Input;
            arParams[53].Value = headlineImageUrl;

			arParams[54] = new NpgsqlParameter("includeimageinexcerpt", NpgsqlTypes.NpgsqlDbType.Boolean);
			arParams[54].Direction = ParameterDirection.Input;
			arParams[54].Value = includeImageInExcerpt;

			arParams[55] = new NpgsqlParameter("includeimageinpost", NpgsqlTypes.NpgsqlDbType.Boolean);
			arParams[55].Direction = ParameterDirection.Input;
			arParams[55].Value = includeImageInPost;



			int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

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

            StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE mp_blogs ");
			sqlCommand.Append("SET  ");
			
			sqlCommand.Append("heading = :heading, ");
			sqlCommand.Append("startdate = :startdate, ");
			sqlCommand.Append("isinnewsletter = :isinnewsletter, ");
            sqlCommand.Append("ispublished = :ispublished, ");
			sqlCommand.Append("description = :description, ");
			sqlCommand.Append("includeinfeed = :includeinfeed, ");
			sqlCommand.Append("allowcommentsfordays = :allowcommentsfordays, ");
			sqlCommand.Append("location = :location, ");
			sqlCommand.Append("lastmoduserguid = :lastmoduserguid, ");
			sqlCommand.Append("lastmodutc = :lastmodutc, ");
			sqlCommand.Append("itemurl = :itemurl, ");
			sqlCommand.Append("metakeywords = :metakeywords, ");
			sqlCommand.Append("metadescription = :metadescription, ");
            sqlCommand.Append("compiledmeta = :compiledmeta, ");

            sqlCommand.Append("subtitle = :subtitle, ");
            sqlCommand.Append("enddate = :enddate, ");
            sqlCommand.Append("approved = :approved, ");
            sqlCommand.Append("approveddate = :approveddate, ");
            sqlCommand.Append("approvedby = :approvedby, ");

            sqlCommand.Append("showauthorname = :showauthorname, ");
            sqlCommand.Append("showauthoravatar = :showauthoravatar, ");
            sqlCommand.Append("showauthorbio = :showauthorbio, ");
            sqlCommand.Append("includeinsearch = :includeinsearch, ");
            sqlCommand.Append("excludefromrecentcontent = :excludefromrecentcontent, ");
            sqlCommand.Append("includeinsitemap = :includeinsitemap, ");
            sqlCommand.Append("usebingmap = :usebingmap, ");
            sqlCommand.Append("mapheight = :mapheight, ");
            sqlCommand.Append("mapwidth = :mapwidth, ");
            sqlCommand.Append("showmapoptions = :showmapoptions, ");
            sqlCommand.Append("showzoomtool = :showzoomtool, ");
            sqlCommand.Append("showlocationinfo = :showlocationinfo, ");
            sqlCommand.Append("usedrivingdirections = :usedrivingdirections, ");
            sqlCommand.Append("maptype = :maptype, ");
            sqlCommand.Append("mapzoom = :mapzoom, ");
            sqlCommand.Append("showdownloadlink = :showdownloadlink, ");

            sqlCommand.Append("includeinnews = :includeinnews, ");
            sqlCommand.Append("pubname = :pubname, ");
            sqlCommand.Append("publanguage = :publanguage, ");
            sqlCommand.Append("pubaccess = :pubaccess, ");
            sqlCommand.Append("pubgenres = :pubgenres, ");
            sqlCommand.Append("pubkeywords = :pubkeywords, ");
            sqlCommand.Append("pubgeolocations = :pubgeolocations, ");
            sqlCommand.Append("pubstocktickers = :pubstocktickers, ");
            sqlCommand.Append("headlineimageurl = :headlineimageurl, ");
            sqlCommand.Append("includeimageinexcerpt = :includeimageinexcerpt, ");
            sqlCommand.Append("includeimageinpost = :includeimageinpost, ");

			sqlCommand.Append("abstract = :abstract "); 
			
			sqlCommand.Append("WHERE  ");
			sqlCommand.Append("itemid = :itemid "); 
			sqlCommand.Append(";");
			
			NpgsqlParameter[] arParams = new NpgsqlParameter[48];
			
			arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer); 
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = itemId;
			
			arParams[1] = new NpgsqlParameter("startdate", NpgsqlTypes.NpgsqlDbType.Timestamp); 
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = startDate;
			
			arParams[2] = new NpgsqlParameter("isinnewsletter", NpgsqlTypes.NpgsqlDbType.Boolean); 
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = isInNewsletter;
			
			arParams[3] = new NpgsqlParameter("description", NpgsqlTypes.NpgsqlDbType.Text); 
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = description;
			
			arParams[4] = new NpgsqlParameter("includeinfeed", NpgsqlTypes.NpgsqlDbType.Boolean); 
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = includeInFeed;
			
			arParams[5] = new NpgsqlParameter("allowcommentsfordays", NpgsqlTypes.NpgsqlDbType.Integer); 
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = allowCommentsForDays;
			
			arParams[6] = new NpgsqlParameter("location", NpgsqlTypes.NpgsqlDbType.Text); 
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = location;
			
			arParams[7] = new NpgsqlParameter("lastmoduserguid", NpgsqlTypes.NpgsqlDbType.Char, 36); 
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = lastModUserGuid.ToString();
			
			arParams[8] = new NpgsqlParameter("lastmodutc", NpgsqlTypes.NpgsqlDbType.Timestamp); 
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = lastModUtc;
			
			arParams[9] = new NpgsqlParameter("itemurl", NpgsqlTypes.NpgsqlDbType.Varchar,255); 
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = itemUrl;
			
			arParams[10] = new NpgsqlParameter("heading", NpgsqlTypes.NpgsqlDbType.Varchar,255); 
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = title;
			
			arParams[11] = new NpgsqlParameter("metakeywords", NpgsqlTypes.NpgsqlDbType.Varchar,255); 
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = metaKeywords;
			
			arParams[12] = new NpgsqlParameter("metadescription", NpgsqlTypes.NpgsqlDbType.Varchar,255); 
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = metaDescription;
			
			arParams[13] = new NpgsqlParameter("abstract", NpgsqlTypes.NpgsqlDbType.Text); 
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = excerpt;

            arParams[14] = new NpgsqlParameter("compiledmeta", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = compiledMeta;

            arParams[15] = new NpgsqlParameter("ispublished", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = isPublished;

            arParams[16] = new NpgsqlParameter("subtitle", NpgsqlTypes.NpgsqlDbType.Varchar, 500);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = subTitle;

            arParams[17] = new NpgsqlParameter("enddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[17].Direction = ParameterDirection.Input;
            if (endDate < DateTime.MaxValue)
            {
                arParams[17].Value = endDate;
            }
            else
            {
                arParams[17].Value = DBNull.Value;
            }

            arParams[18] = new NpgsqlParameter("approved", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = approved;

            arParams[19] = new NpgsqlParameter("approveddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[19].Direction = ParameterDirection.Input;
            if (approvedDate < DateTime.MaxValue)
            {
                arParams[19].Value = approvedDate;
            }
            else
            {
                arParams[19].Value = DBNull.Value;
            }

            arParams[20] = new NpgsqlParameter("approvedby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = approvedBy.ToString();


            arParams[21] = new NpgsqlParameter("showauthorname", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = showAuthorName;

            arParams[22] = new NpgsqlParameter("showauthoravatar", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = showAuthorAvatar;

            arParams[23] = new NpgsqlParameter("showauthorbio", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = showAuthorBio;

            arParams[24] = new NpgsqlParameter("includeinsearch", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = includeInSearch;

            arParams[25] = new NpgsqlParameter("includeinsitemap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = includeInSiteMap;

            arParams[26] = new NpgsqlParameter("usebingmap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = useBingMap;

            arParams[27] = new NpgsqlParameter("mapheight", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = mapHeight;

            arParams[28] = new NpgsqlParameter("mapwidth", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = mapWidth;

            arParams[29] = new NpgsqlParameter("showmapoptions", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = showMapOptions;

            arParams[30] = new NpgsqlParameter("showzoomtool", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = showZoomTool;

            arParams[31] = new NpgsqlParameter("showlocationinfo", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = showLocationInfo;

            arParams[32] = new NpgsqlParameter("usedrivingdirections", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = useDrivingDirections;

            arParams[33] = new NpgsqlParameter("maptype", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = mapType;

            arParams[34] = new NpgsqlParameter("mapzoom", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = mapZoom;

            arParams[35] = new NpgsqlParameter("showdownloadlink", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = showDownloadLink;

            arParams[36] = new NpgsqlParameter("excludefromrecentcontent", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = excludeFromRecentContent;

            arParams[37] = new NpgsqlParameter("includeinnews", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = includeInNews;

            arParams[38] = new NpgsqlParameter("pubname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = pubName;

            arParams[39] = new NpgsqlParameter("publanguage", NpgsqlTypes.NpgsqlDbType.Varchar, 7);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = pubLanguage;

            arParams[40] = new NpgsqlParameter("pubaccess", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = pubAccess;

            arParams[41] = new NpgsqlParameter("pubgenres", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = pubGenres;

            arParams[42] = new NpgsqlParameter("pubkeywords", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = pubKeyWords;

            arParams[43] = new NpgsqlParameter("pubgeolocations", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = pubGeoLocations;

            arParams[44] = new NpgsqlParameter("pubstocktickers", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = pubStockTickers;

            arParams[45] = new NpgsqlParameter("headlineimageurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = headlineImageUrl;

			arParams[46] = new NpgsqlParameter("includeimageinexcerpt", NpgsqlTypes.NpgsqlDbType.Boolean);
			arParams[46].Direction = ParameterDirection.Input;
			arParams[46].Value = includeImageInExcerpt;

			arParams[47] = new NpgsqlParameter("includeimageinpost", NpgsqlTypes.NpgsqlDbType.Boolean);
			arParams[47].Direction = ParameterDirection.Input;
			arParams[47].Value = includeImageInPost;


			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
				CommandType.Text, 
				sqlCommand.ToString(), 
				arParams);
	
			return (rowsAffected > -1);

        }

        public static bool UpdateCommentCount(Guid blogGuid, int commentCount)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_blogs ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("commentcount = :commentcount ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("blogguid = :blogguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("blogguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = blogGuid.ToString();

            arParams[1] = new NpgsqlParameter("commentcount", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = commentCount;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[7];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            arParams[2] = new NpgsqlParameter("name", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = name;

            arParams[3] = new NpgsqlParameter("title", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = title;

            arParams[4] = new NpgsqlParameter("url", NpgsqlTypes.NpgsqlDbType.Text, 200);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = url;

            arParams[5] = new NpgsqlParameter("comment", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = comment;

            arParams[6] = new NpgsqlParameter("datecreated", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = dateCreated;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_blogcomment_insert(:moduleid,:itemid,:name,:title,:url,:comment,:datecreated)",
                arParams));

            return (rowsAffected > 0);

        }

        public static bool DeleteAllCommentsForBlog(int itemId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE ");
            sqlCommand.Append("FROM	mp_blogcomments ");
            sqlCommand.Append("WHERE itemid = " + itemId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                 ConnectionString.GetWriteConnectionString(),
                 CommandType.Text,
                 sqlCommand.ToString());

            return (rowsAffected > 0);
        }

        public static bool UpdateCommentStats(int moduleId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_blogstats ");
            sqlCommand.Append("SET 	commentcount = (SELECT COUNT(*) FROM mp_blogcomments WHERE moduleid = "
                + moduleId.ToString(CultureInfo.InvariantCulture) + ") ");
            sqlCommand.Append("WHERE ModuleID = " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                 ConnectionString.GetWriteConnectionString(),
                 CommandType.Text,
                 sqlCommand.ToString());

            return (rowsAffected > 0);
        }

        public static bool UpdateEntryStats(int moduleId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_blogstats ");
            sqlCommand.Append("SET 	entrycount = (SELECT COUNT(*) FROM mp_blogs WHERE moduleid = "
                + moduleId.ToString(CultureInfo.InvariantCulture) + ") ");
            sqlCommand.Append("WHERE ModuleID = " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                 ConnectionString.GetWriteConnectionString(),
                 CommandType.Text,
                 sqlCommand.ToString());

            return (rowsAffected > 0);
        }


        public static bool DeleteBlogComment(int commentId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("blogcommentid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = commentId;
           
            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_blogcomment_delete(:blogcommentid)",
                arParams));

            return (rowsAffected > -1);

        }


        public static IDataReader GetBlogComments(int moduleId, int itemId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_blogcomments_select(:moduleid,:itemid)",
                arParams);

        }



        // Blog Categories

        public static int AddBlogCategory(
            int moduleId,
            string category)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("category", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = category;

            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_blogcategories_insert(:moduleid,:category)",
                arParams));

            return newID;

        }

        public static bool UpdateBlogCategory(
            int categoryId,
            string category)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("categoryid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;

            arParams[1] = new NpgsqlParameter("category", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = category;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_blogcategories_update(:categoryid,:category)",
                arParams));

            return (rowsAffected > 0);

        }



        public static bool DeleteCategory(int categoryId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("categoryid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_blogcategories_delete(:categoryid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static IDataReader GetCategory(int categoryId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("categoryid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = categoryId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_blogcategories_select_one(:categoryid)",
                arParams);

        }


        public static IDataReader GetCategories(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("bc.categoryid As categoryid, ");
            sqlCommand.Append("bc.category As category, ");
            sqlCommand.Append("COUNT(bic.itemid) As postcount ");
            sqlCommand.Append("FROM mp_blogcategories bc ");
            sqlCommand.Append("JOIN	mp_blogitemcategories bic ");
            sqlCommand.Append("ON bc.categoryid = bic.categoryid ");

            sqlCommand.Append("JOIN	mp_blogs b ");
            sqlCommand.Append("ON b.itemid = bic.itemid ");

            sqlCommand.Append("WHERE bc.moduleid = :moduleid ");
            sqlCommand.Append("AND b.ispublished = true ");
            sqlCommand.Append("AND b.startdate <= :currentdate ");
            sqlCommand.Append("AND (b.enddate IS NULL OR b.enddate > :currentdate) ");

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append(" bc.categoryid, bc.category ");
            sqlCommand.Append("ORDER BY bc.category ");
            sqlCommand.Append(" ; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("currentdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            //return NpgsqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_blogcategories_select_bymodule(:moduleid,:currentdate)",
            //    arParams);

        }

        public static IDataReader GetCategoriesList(int moduleId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_blogcategories_selectlist_bymodule(:moduleid)",
                arParams);

        }

        public static int AddBlogItemCategory(
            int itemId,
            int categoryId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new NpgsqlParameter("categoryid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = categoryId;

            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_blogitemcategories_insert(:itemid,:categoryid)",
                arParams));

            return newID;

        }


        public static bool DeleteItemCategories(int itemId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_blogitemcategories_delete(:itemid)",
                arParams));

            return (rowsAffected > -1);

        }


        public static IDataReader GetBlogItemCategories(int itemId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_blogitemcategories_select_byitem(:itemid)",
                arParams);

        }


    }
}
