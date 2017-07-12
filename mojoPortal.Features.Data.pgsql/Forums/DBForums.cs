// Author:					
// Created:				    2007-11-03
// Last Modified:			2014-07-06
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
using System.Data;
using System.Globalization;
using System.Text;
using Npgsql;

namespace mojoPortal.Data
{
   
    public static class DBForums
    {
        
        public static int Create(
            Guid forumGuid,
            int moduleId,
            int userId,
            string title,
            string description,
            bool isModerated,
            bool isActive,
            int sortOrder,
            int postsPerPage,
            int threadsPerPage,
            bool allowAnonymousPosts,
            string rolesThatCanPost,
            string rolesThatCanModerate,
            string moderatorNotifyEmail,
            bool includeInGoogleMap,
            bool addNoIndexMeta,
            bool closed,
            bool visible,
            bool requireModeration,
            bool requireModForNotify,
            bool allowTrustedDirectPosts,
            bool allowTrustedDirectNotify
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_forums (");
            sqlCommand.Append("moduleid, ");
            sqlCommand.Append("createddate, ");
            sqlCommand.Append("createdby, ");
            sqlCommand.Append("title, ");
            sqlCommand.Append("description, ");
            sqlCommand.Append("ismoderated, ");
            sqlCommand.Append("isactive, ");
            sqlCommand.Append("sortorder, ");
            sqlCommand.Append("threadcount, ");
            sqlCommand.Append("postcount, ");

            sqlCommand.Append("rolesthatcanpost, ");
            sqlCommand.Append("rolesthatcanmoderate, ");
            sqlCommand.Append("moderatornotifyemail, ");
            sqlCommand.Append("includeingooglemap, ");
            sqlCommand.Append("addnoindexmeta, ");
            sqlCommand.Append("closed, ");
            sqlCommand.Append("visible, ");
            sqlCommand.Append("requiremoderation, ");
            sqlCommand.Append("requiremodfornotify, ");
            sqlCommand.Append("allowtrusteddirectposts, ");
            sqlCommand.Append("allowtrusteddirectnotify, ");
            
            sqlCommand.Append("postsperpage, ");
            sqlCommand.Append("threadsperpage, ");
            sqlCommand.Append("allowanonymousposts, ");
            sqlCommand.Append("forumguid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":moduleid, ");
            sqlCommand.Append(":createddate, ");
            sqlCommand.Append(":createdby, ");
            sqlCommand.Append(":title, ");
            sqlCommand.Append(":description, ");
            sqlCommand.Append(":ismoderated, ");
            sqlCommand.Append(":isactive, ");
            sqlCommand.Append(":sortorder, ");
            sqlCommand.Append(":threadcount, ");
            sqlCommand.Append(":postcount, ");

            sqlCommand.Append(":rolesthatcanpost, ");
            sqlCommand.Append(":rolesthatcanmoderate, ");
            sqlCommand.Append(":moderatornotifyemail, ");
            sqlCommand.Append(":includeingooglemap, ");
            sqlCommand.Append(":addnoindexmeta, ");
            sqlCommand.Append(":closed, ");
            sqlCommand.Append(":visible, ");
            sqlCommand.Append(":requiremoderation, ");
            sqlCommand.Append(":requiremodfornotify, ");
            sqlCommand.Append(":allowtrusteddirectposts, ");
            sqlCommand.Append(":allowtrusteddirectnotify, ");
            
            sqlCommand.Append(":postsperpage, ");
            sqlCommand.Append(":threadsperpage, ");
            sqlCommand.Append(":allowanonymousposts, ");
            sqlCommand.Append(":forumguid )");
            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT currval('mp_forums_itemid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[25];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("createddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            arParams[2] = new NpgsqlParameter("createdby", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userId;

            arParams[3] = new NpgsqlParameter("title", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = title;

            arParams[4] = new NpgsqlParameter("description", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = description;

            arParams[5] = new NpgsqlParameter("ismoderated", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = isModerated;

            arParams[6] = new NpgsqlParameter("isactive", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = isActive;

            arParams[7] = new NpgsqlParameter("sortorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = sortOrder;

            arParams[8] = new NpgsqlParameter("threadcount", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = 0;

            arParams[9] = new NpgsqlParameter("postcount", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = 0;

            arParams[10] = new NpgsqlParameter("postsperpage", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = postsPerPage;

            arParams[11] = new NpgsqlParameter("threadsperpage", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = threadsPerPage;

            arParams[12] = new NpgsqlParameter("allowanonymousposts", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = allowAnonymousPosts;

            arParams[13] = new NpgsqlParameter("forumguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = forumGuid.ToString();

            arParams[14] = new NpgsqlParameter("rolesthatcanpost", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = rolesThatCanPost;

            arParams[15] = new NpgsqlParameter("rolesthatcanmoderate", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = rolesThatCanModerate;

            arParams[16] = new NpgsqlParameter("moderatornotifyemail", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = moderatorNotifyEmail;

            arParams[17] = new NpgsqlParameter("includeingooglemap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = includeInGoogleMap;

            arParams[18] = new NpgsqlParameter("addnoindexmeta", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = addNoIndexMeta;

            arParams[19] = new NpgsqlParameter("closed", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = closed;

            arParams[20] = new NpgsqlParameter("visible", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = visible;

            arParams[21] = new NpgsqlParameter("requiremoderation", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = requireModeration;

            arParams[22] = new NpgsqlParameter("requiremodfornotify", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = requireModForNotify;

            arParams[23] = new NpgsqlParameter("allowtrusteddirectposts", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = allowTrustedDirectPosts;

            arParams[24] = new NpgsqlParameter("allowtrusteddirectnotify", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = allowTrustedDirectNotify;


            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));


            return newID;

           

        }

        public static bool Update(
            int itemId,
            int userId,
            string title,
            string description,
            bool isModerated,
            bool isActive,
            int sortOrder,
            int postsPerPage,
            int threadsPerPage,
            bool allowAnonymousPosts,
            string rolesThatCanPost,
            string rolesThatCanModerate,
            string moderatorNotifyEmail,
            bool includeInGoogleMap,
            bool addNoIndexMeta,
            bool closed,
            bool visible,
            bool requireModeration,
            bool requireModForNotify,
            bool allowTrustedDirectPosts,
            bool allowTrustedDirectNotify)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[20];
            
            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new NpgsqlParameter("title", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new NpgsqlParameter("description", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            arParams[3] = new NpgsqlParameter("ismoderated", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = isModerated;

            arParams[4] = new NpgsqlParameter("isactive", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = isActive;

            arParams[5] = new NpgsqlParameter("sortorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sortOrder;

            arParams[6] = new NpgsqlParameter("postsperpage", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = postsPerPage;

            arParams[7] = new NpgsqlParameter("threadsperpage", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = threadsPerPage;

            arParams[8] = new NpgsqlParameter("allowanonymousposts", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = allowAnonymousPosts;

            arParams[9] = new NpgsqlParameter("rolesthatcanpost", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = rolesThatCanPost;

            arParams[10] = new NpgsqlParameter("rolesthatcanmoderate", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = rolesThatCanModerate;

            arParams[11] = new NpgsqlParameter("moderatornotifyemail", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = moderatorNotifyEmail;

            arParams[12] = new NpgsqlParameter("includeingooglemap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = includeInGoogleMap;

            arParams[13] = new NpgsqlParameter("addnoindexmeta", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = addNoIndexMeta;

            arParams[14] = new NpgsqlParameter("closed", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = closed;

            arParams[15] = new NpgsqlParameter("visible", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = visible;

            arParams[16] = new NpgsqlParameter("requiremoderation", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = requireModeration;

            arParams[17] = new NpgsqlParameter("requiremodfornotify", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = requireModForNotify;

            arParams[18] = new NpgsqlParameter("allowtrusteddirectposts", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = allowTrustedDirectPosts;

            arParams[19] = new NpgsqlParameter("allowtrusteddirectnotify", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = allowTrustedDirectNotify;

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_forums ");
            sqlCommand.Append("SET title = :title, ");
            sqlCommand.Append("description = :description, ");
            sqlCommand.Append("ismoderated = :ismoderated, ");
            sqlCommand.Append("isactive = :isactive, ");
            sqlCommand.Append("sortorder = :sortorder, ");
            sqlCommand.Append("postsperpage = :postsperpage, ");
            sqlCommand.Append("threadsperpage = :threadsperpage, ");

            sqlCommand.Append("rolesthatcanpost = :rolesthatcanpost, ");
            sqlCommand.Append("rolesthatcanmoderate = :rolesthatcanmoderate, ");
            sqlCommand.Append("moderatornotifyemail = :moderatornotifyemail, ");
            sqlCommand.Append("includeingooglemap = :includeingooglemap, ");
            sqlCommand.Append("addnoindexmeta = :addnoindexmeta, ");
            sqlCommand.Append("closed = :closed, ");
            sqlCommand.Append("visible = :visible, ");
            sqlCommand.Append("requiremoderation = :requiremoderation, ");
            sqlCommand.Append("requiremodfornotify = :requiremodfornotify, ");
            sqlCommand.Append("allowtrusteddirectposts = :allowtrusteddirectposts, ");
            sqlCommand.Append("allowtrusteddirectnotify = :allowtrusteddirectnotify, ");

            sqlCommand.Append("allowanonymousposts = :allowanonymousposts ");

            sqlCommand.Append("WHERE itemid = :itemid ;");


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
            
            //int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
            //    ConnectionString.GetWriteConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_forums_update(:itemid,:title,:description,:ismoderated,:isactive,:sortorder,:postsperpage,:threadsperpage,:allowanonymousposts)",
            //    arParams));

            //return (rowsAffected > -1);

        }

        public static bool Delete(int itemId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;
           
            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_forum_delete(:itemid)",
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
            sqlCommand.Append("DELETE FROM mp_forumposts WHERE threadid IN (SELECT threadid FROM mp_forumthreads WHERE forumid IN (SELECT itemid FROM mp_forums WHERE moduleid = :moduleid) );");
            sqlCommand.Append("DELETE FROM mp_forumthreadsubscriptions WHERE threadid IN (SELECT threadid FROM mp_forumthreads WHERE forumid IN (SELECT itemid FROM mp_forums WHERE moduleid = :moduleid) );");
            sqlCommand.Append("DELETE FROM mp_forumthreads WHERE forumid IN (SELECT itemid FROM mp_forums WHERE moduleid = :moduleid);");
            sqlCommand.Append("DELETE FROM mp_forumsubscriptions WHERE forumid IN (SELECT itemid FROM mp_forums WHERE moduleid = :moduleid) ;");
            sqlCommand.Append("DELETE FROM mp_forums WHERE moduleid = :moduleid;");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("DELETE FROM mp_forumposts WHERE threadid IN (SELECT threadid FROM mp_forumthreads WHERE forumid IN (SELECT itemid FROM mp_forums WHERE moduleid IN  (SELECT moduleid FROM mp_modules WHERE siteid = :siteid)) );");
            sqlCommand.Append("DELETE FROM mp_forumthreadsubscriptions WHERE threadid IN (SELECT threadid FROM mp_forumthreads WHERE forumid IN (SELECT itemid FROM mp_forums WHERE moduleid IN  (SELECT moduleid FROM mp_modules WHERE siteid = :siteid)) );");
            sqlCommand.Append("DELETE FROM mp_forumthreads WHERE forumid IN (SELECT itemid FROM mp_forums WHERE moduleid IN  (SELECT moduleid FROM mp_modules WHERE siteid = :siteid));");
            sqlCommand.Append("DELETE FROM mp_forumsubscriptions WHERE forumid IN (SELECT itemid FROM mp_forums WHERE moduleid IN  (SELECT moduleid FROM mp_modules WHERE siteid = :siteid)) ;");
            sqlCommand.Append("DELETE FROM mp_forums WHERE moduleid IN (SELECT moduleid FROM mp_modules WHERE siteid = :siteid);");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetForums(int moduleId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("f.*, ");
            //sqlCommand.Append("f.moduleid, ");
            //sqlCommand.Append("f.createddate, ");
            //sqlCommand.Append("f.createdby, ");
            //sqlCommand.Append("f.title, ");
            //sqlCommand.Append("f.description, ");
            //sqlCommand.Append("f.ismoderated, ");
            //sqlCommand.Append("f.isactive, ");
            //sqlCommand.Append("f.sortorder, ");
            //sqlCommand.Append("f.threadcount, ");
            //sqlCommand.Append("f.postcount, ");
            //sqlCommand.Append("f.mostrecentpostdate, ");
            //sqlCommand.Append("f.mostrecentpostuserid, ");
            //sqlCommand.Append("f.postsperpage, ");
            //sqlCommand.Append("f.threadsperpage, ");
            //sqlCommand.Append("f.allowanonymousposts, ");
            sqlCommand.Append("u.name as mostrecentpostuser, ");
            sqlCommand.Append("s.subscribedate is not null and s.unsubscribedate is null as subscribed, ");

            sqlCommand.Append("(SELECT COUNT(fs.*) FROM mp_forumsubscriptions fs WHERE fs.forumid = f.itemid AND fs.unsubscribedate IS NULL) As subscribercount  ");

            sqlCommand.Append("FROM	mp_forums f ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("f.mostrecentpostuserid = u.userid ");
            sqlCommand.Append(" ");

            sqlCommand.Append("LEFT OUTER JOIN mp_forumsubscriptions s ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("f.itemid = s.forumid and s.userid = :userid AND s.unsubscribedate IS NULL ");
            sqlCommand.Append(" ");


            sqlCommand.Append("WHERE ");
            sqlCommand.Append("f.moduleid = :moduleid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("f.isactive = true ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("f.sortorder, f.itemid ");

            sqlCommand.Append(";");



            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
            
            //return NpgsqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_forums_select(:moduleid,:userid)",
            //    arParams);

        }

        public static IDataReader GetForum(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT f.*, ");
            sqlCommand.Append("u.name As createdbyuser, ");
            sqlCommand.Append("up.name As mostrecentpostuser ");
            sqlCommand.Append("FROM	mp_forums f ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON f.createdby = u.userid ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_users up ");
            sqlCommand.Append("ON f.mostrecentpostuserid = up.userid ");
            sqlCommand.Append("WHERE f.itemid	= :itemid ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            //NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            //arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = itemId;
           
            //return NpgsqlHelper.ExecuteReader(
            //    ConnectionString.GetReadConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_forums_selectone(:itemid)",
            //    arParams);

        }

        public static bool ForumUpdatePostStats(int forumId, int mostRecentPostUserId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new NpgsqlParameter("mostrecentpostuserid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostUserId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_forums_updatepoststats(:forumid,:mostrecentpostuserid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool IncrementThreadCount(int forumId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_forums_incrementthreadcount(:forumid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool DecrementThreadCount(int forumId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_forums_decrementthreadcount(:forumid)",
                arParams));

            return (rowsAffected > -1);

        }


        public static bool IncrementPostCount(
            int forumId,
            int mostRecentPostUserId,
            DateTime mostRecentPostDate)
        {

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];
            
            arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new NpgsqlParameter("mostrecentpostuserid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostUserId;

            arParams[2] = new NpgsqlParameter("mostrecentpostdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostDate;
            
            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_forums_incrementpostcount(:forumid,:mostrecentpostuserid,:mostrecentpostdate)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool UpdateUserStats(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_users ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("totalposts = (SELECT COUNT(*) FROM mp_forumposts WHERE mp_forumposts.userid = mp_users.userid) ");
            if (userId > -1)
            {
                sqlCommand.Append("WHERE userid = :userid ");
            }
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool IncrementPostCount(int forumId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;
            
            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_forums_incrementpostcountonly(:forumid)",
                arParams));

            return (rowsAffected > -1);

        }


        public static bool DecrementPostCount(int forumId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;
            
            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_forums_decrementpostcount(:forumid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool RecalculatePostStats(int forumId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;
            
            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_forums_recalculatepoststats(:forumid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static int GetUserThreadCount(int userId, int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");

            sqlCommand.Append("FROM	mp_forumthreads ft ");

            sqlCommand.Append("JOIN mp_forums f ");
            sqlCommand.Append("ON ft.forumid = f.itemid ");

            sqlCommand.Append("JOIN mp_modules m ");
            sqlCommand.Append("ON f.moduleid = m.moduleid ");

            sqlCommand.Append("WHERE m.siteid = :siteid AND ft.threadid IN (Select threadid FROM mp_forumposts WHERE mp_forumposts.userid = :userid) ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteId;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }


        public static IDataReader GetThreadPageByUser(
            int userId,
            int siteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetUserThreadCount(userId, siteId);

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
            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" t.*, ");
            sqlCommand.Append("f.title As forum, ");
            sqlCommand.Append("f.moduleid, ");
            sqlCommand.Append("(SELECT pageid FROM mp_pagemodules WHERE mp_pagemodules.moduleid = f.moduleid AND (publishenddate IS NULL OR publishenddate > :currentdate) LIMIT 1) As pageid, ");
            sqlCommand.Append("COALESCE(u.name, 'Guest') As mostrecentpostuser, ");
            sqlCommand.Append("s.name As startedby ");

            sqlCommand.Append("FROM	mp_forumthreads t ");

            sqlCommand.Append("JOIN	mp_forums f ");
            sqlCommand.Append("ON t.forumid = f.itemid ");

            sqlCommand.Append("JOIN mp_modules m ");
            sqlCommand.Append("ON f.moduleid = m.moduleid ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON t.mostrecentpostuserID = u.userid ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users s ");
            sqlCommand.Append("ON t.startedbyuserID = s.userid ");

            sqlCommand.Append("WHERE m.siteid = :siteid AND t.threadid IN (Select threadid FROM mp_forumposts WHERE mp_forumposts.userid = :userid) ");

            sqlCommand.Append("ORDER BY	t.mostrecentpostdate DESC  ");

            sqlCommand.Append("LIMIT " + pageSize.ToString(CultureInfo.InvariantCulture));

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");


            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageLowerBound;

            arParams[2] = new NpgsqlParameter("currentdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            arParams[3] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = siteId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetThreads(int forumId, int pageNumber)
        {
            int threadsPerPage = 1;
            int totalThreads = 0;
            using (IDataReader reader = GetForum(forumId))
            {
                if (reader.Read())
                {
                    threadsPerPage = Convert.ToInt32(reader["ThreadsPerPage"]);
                    totalThreads = Convert.ToInt32(reader["ThreadCount"]);
                }
            }

            int pageLowerBound = (threadsPerPage * pageNumber) - threadsPerPage;

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT	t.*, ");
            sqlCommand.Append("COALESCE(u.name, 'Guest') As mostrecentpostuser, ");
            sqlCommand.Append("s.name As startedby ");
            sqlCommand.Append("FROM	mp_forumthreads t ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON t.mostrecentpostuserid = u.userid ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_users s ");
            sqlCommand.Append("ON t.startedbyuserid = s.userid ");
            sqlCommand.Append("WHERE	t.forumid = :forumid ");
            sqlCommand.Append("ORDER BY t.sortorder, t.mostrecentpostdate DESC ");
            sqlCommand.Append("LIMIT		" + threadsPerPage + " ");
            sqlCommand.Append("OFFSET		" + pageLowerBound + " ");
            sqlCommand.Append(" ; ");


            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new NpgsqlParameter("pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            
            //return NpgsqlHelper.ExecuteReader(
            //    ConnectionString.GetReadConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_forumthreads_selectbyforumdesc_v2(:forumid,:pagenumber)",
            //    arParams);

        }

        public static int GetSubscriberCount(int forumId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_forumsubscriptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("forumid = :forumid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("unsubscribedate IS NULL");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }

        public static IDataReader GetSubscriberPage(
            int forumId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetSubscriberCount(forumId);

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

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("fs.subscriptionid, ");
            sqlCommand.Append("fs.subscribedate, ");
            sqlCommand.Append("u.name, ");
            sqlCommand.Append("u.loginname, ");
            sqlCommand.Append("u.email ");

            sqlCommand.Append("FROM	mp_forumsubscriptions fs  ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.userid = fs.userid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("fs.forumid = :forumid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("fs.unsubscribedate IS NULL ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("u.name  ");
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


        public static bool AddSubscriber(int forumId, int userId, Guid subGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COUNT(*) As subscriptioncount ");
            sqlCommand.Append("FROM mp_forumsubscriptions  ");
            sqlCommand.Append("WHERE forumid = :forumid AND userid = :userid AND unsubscribedate IS NULL ; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int subscriptionCount = 0;
            int rowsAffected = -1;

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    subscriptionCount = Convert.ToInt32(reader["SubscriptionCount"]);
                }
            }


            if (subscriptionCount > 0)
            {


                arParams = new NpgsqlParameter[3];

                arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = forumId;

                arParams[1] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = userId;

                arParams[2] = new NpgsqlParameter("subscribedate", NpgsqlTypes.NpgsqlDbType.Timestamp);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = DateTime.UtcNow;

                sqlCommand = new StringBuilder();

                sqlCommand.Append("UPDATE mp_forumsubscriptions ");
                sqlCommand.Append("SET unsubscribedate = :subscribedate ");
                sqlCommand.Append("WHERE forumid = :forumid AND userid = :userid AND unsubscribedate IS NULL ;");

                rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));


            }


            arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new NpgsqlParameter("subscribedate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            arParams[3] = new NpgsqlParameter("subguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = subGuid.ToString();


            sqlCommand = new StringBuilder();


            sqlCommand.Append("INSERT INTO	mp_forumsubscriptions ( ");

            sqlCommand.Append("forumid, ");
            sqlCommand.Append("userid, ");
            sqlCommand.Append("subguid, ");
            sqlCommand.Append("subscribedate");

            sqlCommand.Append(") ");

            sqlCommand.Append("VALUES ( ");

            sqlCommand.Append(":forumid, ");
            sqlCommand.Append(":userid, ");
            sqlCommand.Append(":subguid, ");
            sqlCommand.Append(":subscribedate");

            sqlCommand.Append(") ;");

            rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (rowsAffected > 0);

        }

        public static bool DeleteSubscription(int subscriptionId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_forumsubscriptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("subscriptionid = :subscriptionid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("subscriptionid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subscriptionId;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Unsubscribe(Guid subGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_forumsubscriptions ");
            sqlCommand.Append("SET unsubscribedate = :unsubscribedate ");
            sqlCommand.Append("WHERE subguid = :subguid ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("subguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subGuid.ToString();

            arParams[1] = new NpgsqlParameter("unsubscribedate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        public static IDataReader GetForumSubscription(Guid subGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_forumsubscriptions ");

            sqlCommand.Append("WHERE subguid = :subguid ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("subguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static bool Unsubscribe(int forumId, int userId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;
            
            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_forumsubscriptions_unsubscribe(:forumid,:userid)",
                arParams));

            return (rowsAffected > 0);

        }

        public static bool UnsubscribeAll(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_forumsubscriptions ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("unsubscribedate = :unsubscribedate ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("userid = :userid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            arParams[1] = new NpgsqlParameter("unsubscribedate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool ForumSubscriptionExists(int forumId, int userId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_forumsubscriptions_exists(:forumid,:userid)",
                arParams));

            return (count > 0);

        }

        public static bool ForumThreadSubscriptionExists(int threadId, int userId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_forumthreadsubscriptions_exists(:threadid,:userid)",
                arParams));

            return (count > 0);

        }

        public static IDataReader GetThreadsForSiteMap(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ft.threadid, ");
            sqlCommand.Append("ft.mostrecentpostdate, ");
            sqlCommand.Append("f.moduleid, ");
            sqlCommand.Append("m.viewroles, ");
            sqlCommand.Append("p.pageid, ");
            sqlCommand.Append("p.authorizedroles ");


            sqlCommand.Append("FROM	mp_forumthreads ft ");

            sqlCommand.Append("JOIN	mp_forums f ");
            sqlCommand.Append("ON f.itemid = ft.forumid ");

            sqlCommand.Append("JOIN	mp_modules m ");
            sqlCommand.Append("ON f.moduleid = m.moduleid ");

            sqlCommand.Append("JOIN	mp_pagemodules pm ");
            sqlCommand.Append("ON pm.moduleid = m.moduleid ");

            sqlCommand.Append("JOIN	mp_pages p ");
            sqlCommand.Append("ON p.pageid = pm.pageid ");


            sqlCommand.Append("WHERE p.siteid = :siteid ");
            sqlCommand.Append("AND ft.includeinsitemap = true ");

            sqlCommand.Append("ORDER BY ft.threadid DESC ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader ForumThreadGetThread(int threadId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	t.*, ");
            sqlCommand.Append("COALESCE(u.name, 'Guest') As mostrecentpostuser, ");
            sqlCommand.Append("COALESCE(s.name, 'Guest') As startedby, ");
            sqlCommand.Append("f.postsperpage, ");
            sqlCommand.Append("f.moduleid ");

            sqlCommand.Append("FROM	mp_forumthreads t ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON t.mostrecentpostuserid = u.userid ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_users s ");
            sqlCommand.Append("ON t.startedbyuserid = s.userid ");
            sqlCommand.Append("JOIN	mp_forums f ");
            sqlCommand.Append("ON f.itemid = t.forumid ");
            sqlCommand.Append("WHERE t.threadid = :threadid ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;
            
            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPost(int postId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("postid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;
           
            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_forumposts_selectone(:postid)",
                arParams);

        }

        public static int ForumThreadGetPostCount(int threadId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_forumposts_countbythread(:threadid)",
                arParams));

            return count;

        }

        public static int ForumThreadCreate(
            int forumId,
            string threadSubject,
            int sortOrder,
            bool isLocked,
            int startedByUserId,
            DateTime threadDate,
            Guid threadGuid,
            bool isQuestion,
            bool includeInSiteMap,
            bool setNoIndexMeta,
            string pageTitleOverride,
            int modStatus,
            string threadType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            int forumSequence = 1;
            sqlCommand.Append("SELECT COALESCE(Max(forumsequence) + 1,1) As forumsequence FROM mp_forumthreads WHERE forumid = :forumid ; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    forumSequence = Convert.ToInt32(reader["ForumSequence"]);
                }
            }


            sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_forumthreads (");
            sqlCommand.Append("forumid, ");
            sqlCommand.Append("threadsubject, ");
            sqlCommand.Append("threaddate, ");
            sqlCommand.Append("totalviews, ");
            sqlCommand.Append("totalreplies, ");
            sqlCommand.Append("sortorder, ");
            sqlCommand.Append("islocked, ");
            sqlCommand.Append("forumsequence, ");
            sqlCommand.Append("mostrecentpostdate, ");
            sqlCommand.Append("mostrecentpostuserid, ");
            sqlCommand.Append("startedbyuserid, ");
            sqlCommand.Append("threadguid, ");
            sqlCommand.Append("isquestion, ");
            sqlCommand.Append("includeinsitemap, ");
            sqlCommand.Append("setnoindexmeta, ");

            sqlCommand.Append("modstatus, ");
            sqlCommand.Append("threadtype, ");

            sqlCommand.Append("ptitleoverride )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":forumid, ");
            sqlCommand.Append(":threadsubject, ");
            sqlCommand.Append(":threaddate, ");
            sqlCommand.Append(":totalviews, ");
            sqlCommand.Append(":totalreplies, ");
            sqlCommand.Append(":sortorder, ");
            sqlCommand.Append(":islocked, ");
            sqlCommand.Append(":forumsequence, ");
            sqlCommand.Append(":mostrecentpostdate, ");
            sqlCommand.Append(":mostrecentpostuserid, ");
            sqlCommand.Append(":startedbyuserid, ");
            sqlCommand.Append(":threadguid, ");
            sqlCommand.Append(":isquestion, ");
            sqlCommand.Append(":includeinsitemap, ");
            sqlCommand.Append(":setnoindexmeta, ");

            sqlCommand.Append(":modstatus, ");
            sqlCommand.Append(":threadtype, ");

            sqlCommand.Append(":ptitleoverride )");
            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT currval('mp_forumthreads_threadid_seq');");

            arParams = new NpgsqlParameter[18];
            arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new NpgsqlParameter("threadsubject", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadSubject;

            arParams[2] = new NpgsqlParameter("threaddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = threadDate;

            arParams[3] = new NpgsqlParameter("totalviews", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = 0;

            arParams[4] = new NpgsqlParameter("totalreplies", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = 0;

            arParams[5] = new NpgsqlParameter("sortorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sortOrder;

            arParams[6] = new NpgsqlParameter("islocked", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = isLocked;

            arParams[7] = new NpgsqlParameter("forumsequence", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = forumSequence;

            arParams[8] = new NpgsqlParameter("mostrecentpostdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = threadDate;

            arParams[9] = new NpgsqlParameter("mostrecentpostuserid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = startedByUserId;

            arParams[10] = new NpgsqlParameter("startedbyuserid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = startedByUserId;

            arParams[11] = new NpgsqlParameter("threadguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = threadGuid.ToString();

            arParams[12] = new NpgsqlParameter("isquestion", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = isQuestion;

            arParams[13] = new NpgsqlParameter("includeinsitemap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = includeInSiteMap;

            arParams[14] = new NpgsqlParameter("setnoindexmeta", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = setNoIndexMeta;

            arParams[15] = new NpgsqlParameter("ptitleoverride", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = pageTitleOverride;

            arParams[16] = new NpgsqlParameter("modstatus", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = modStatus;

            arParams[17] = new NpgsqlParameter("threadtype", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = threadType;


            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));


            return newID;

            

        }

        public static bool ForumThreadDelete(int threadId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_forumthreads_delete(:threadid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool ForumThreadUpdate(
            int threadId,
            int forumId,
            string threadSubject,
            int sortOrder,
            bool isLocked,
            bool isQuestion,
            bool includeInSiteMap,
            bool setNoIndexMeta,
            string pageTitleOverride,

            int modStatus,
            string threadType,
            Guid assignedTo,
            Guid lockedBy,
            string lockedReason,
            DateTime lockedUtc)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_forumthreads ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("forumid = :forumid, ");
            sqlCommand.Append("threadsubject = :threadsubject, ");
           
            sqlCommand.Append("sortorder = :sortorder, ");
            sqlCommand.Append("islocked = :islocked, ");
            sqlCommand.Append("isquestion = :isquestion, ");
            sqlCommand.Append("includeinsitemap = :includeinsitemap, ");
            sqlCommand.Append("setnoindexmeta = :setnoindexmeta, ");

            sqlCommand.Append("modstatus = :modstatus, ");
            sqlCommand.Append("threadtype = :threadtype, ");
            sqlCommand.Append("assignedto = :assignedto, ");
            sqlCommand.Append("lockedby = :lockedby, ");
            sqlCommand.Append("lockedreason = :lockedreason, ");
            sqlCommand.Append("lockedutc = :lockedutc, ");

            sqlCommand.Append("ptitleoverride = :ptitleoverride ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("threadid = :threadid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[15];

            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = forumId;

            arParams[2] = new NpgsqlParameter("threadsubject", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = threadSubject;

            arParams[3] = new NpgsqlParameter("sortorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new NpgsqlParameter("islocked", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = isLocked;

            arParams[5] = new NpgsqlParameter("isquestion", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = isQuestion;

            arParams[6] = new NpgsqlParameter("includeinsitemap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = includeInSiteMap;

            arParams[7] = new NpgsqlParameter("setnoindexmeta", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = setNoIndexMeta;

            arParams[8] = new NpgsqlParameter("ptitleoverride", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = pageTitleOverride;

            arParams[9] = new NpgsqlParameter("modstatus", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = modStatus;

            arParams[10] = new NpgsqlParameter("threadtype", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = threadType;

            arParams[11] = new NpgsqlParameter("assignedto", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = assignedTo.ToString();

            arParams[12] = new NpgsqlParameter("lockedby", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = lockedBy.ToString();

            arParams[13] = new NpgsqlParameter("lockedreason", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = lockedReason;

            arParams[14] = new NpgsqlParameter("lockedutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[14].Direction = ParameterDirection.Input;
            if (lockedUtc < DateTime.MaxValue)
            {
                arParams[14].Value = lockedUtc;
            }
            else
            {
                arParams[14].Value = DBNull.Value;
            }


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
            
            
            

        }



        public static bool ForumThreadIncrementReplyStats(
            int threadId,
            int mostRecentPostUserId,
            DateTime mostRecentPostDate)
        {

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];
            
            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new NpgsqlParameter("mostrecentpostuserid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = mostRecentPostUserId;

            arParams[2] = new NpgsqlParameter("mostrecentpostdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = mostRecentPostDate;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_forumthreads_incrementreplycount(:threadid,:mostrecentpostuserid,:mostrecentpostdate)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool ForumThreadDecrementReplyStats(int threadId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_forumthreads_decrementreplycount(:threadid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool ForumThreadUpdateViewStats(int threadId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_forumthreads_updateviewstats(:threadid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static IDataReader ForumThreadGetPosts(int threadId, int pageNumber)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            StringBuilder sqlCommand = new StringBuilder();

            int postsPerPage = 10;

            sqlCommand.Append("SELECT f.postsperpage ");
            sqlCommand.Append("FROM	mp_forumthreads ft ");
            sqlCommand.Append("JOIN	mp_forums f ");
            sqlCommand.Append("ON ft.forumid = f.itemid ");
            sqlCommand.Append("WHERE ft.threadid = :threadid ;");

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {

                if (reader.Read())
                {
                    postsPerPage = Convert.ToInt32(reader["PostsPerPage"]);
                }
            }

            
            int currentPageMaxThreadSequence = postsPerPage * pageNumber;
            int beginSequence = 0;

            if (currentPageMaxThreadSequence > postsPerPage)
            {
                beginSequence = currentPageMaxThreadSequence - postsPerPage;

            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	p.*, ");

            sqlCommand.Append("ft.forumid As forumid, ");
            sqlCommand.Append("ft.islocked As islocked, ");
            // TODO:
            //using 'Guest' here is not culture neutral, need to pass in a label
            sqlCommand.Append("COALESCE(u.name, 'Guest') As mostrecentpostuser, ");
            sqlCommand.Append("COALESCE(s.name, 'Guest') As startedby, ");
            sqlCommand.Append("COALESCE(up.name, 'Guest') As postauthor, ");
            sqlCommand.Append("COALESCE(up.email, '') As authoremail, ");
            sqlCommand.Append("COALESCE(up.totalposts, 0) As postauthortotalposts, ");
            sqlCommand.Append("COALESCE(up.totalrevenue, 0) As userrevenue, ");
            sqlCommand.Append("COALESCE(up.trusted, true) As trusted, ");
            sqlCommand.Append("COALESCE(up.avatarurl, 'blank.gif') As postauthoravatar, ");
            sqlCommand.Append("up.websiteurl As postauthorwebsiteurl, ");
            sqlCommand.Append("up.signature As postauthorsignature ");

            sqlCommand.Append("FROM	mp_forumposts p ");

            sqlCommand.Append("JOIN	mp_forumthreads ft ");
            sqlCommand.Append("ON p.threadid = ft.threadid ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON ft.mostrecentpostuserid = u.userid ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users s ");
            sqlCommand.Append("ON ft.startedbyuserid = s.userid ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users up ");
            sqlCommand.Append("ON up.userid = p.userid ");

            sqlCommand.Append("WHERE ft.threadid = :threadid ");

            sqlCommand.Append("ORDER BY	p.sortorder, p.postid ");
            sqlCommand.Append("LIMIT " + postsPerPage + " ");
            sqlCommand.Append("OFFSET " + beginSequence + " ; ");
            
            
            
            
            
            arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new NpgsqlParameter("pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
            
            //return NpgsqlHelper.ExecuteReader(
            //    ConnectionString.GetReadConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_forumposts_selectbythread2(:threadid,:pagenumber)",
            //    arParams);

        }

        public static IDataReader ForumThreadGetPosts(int threadId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT	p.*, ");

            sqlCommand.Append("ft.forumid As forumid, ");
            sqlCommand.Append("ft.islocked As islocked, ");
            // TODO:
            //using 'Guest' here is not culture neutral, need to pass in a label
            sqlCommand.Append("COALESCE(u.name, 'Guest') As mostrecentpostuser, ");
            sqlCommand.Append("COALESCE(s.name, 'Guest') As startedby, ");
            sqlCommand.Append("COALESCE(up.name, 'Guest') As postauthor, ");
            sqlCommand.Append("COALESCE(up.email, '') As authoremail, ");
            sqlCommand.Append("COALESCE(up.totalposts, 0) As postauthortotalposts, ");
            sqlCommand.Append("COALESCE(up.totalrevenue, 0) As userrevenue, ");
            sqlCommand.Append("COALESCE(up.trusted, true) As trusted, ");
            sqlCommand.Append("COALESCE(up.avatarurl, 'blank.gif') As postauthoravatar, ");
            sqlCommand.Append("up.websiteurl As postauthorwebsiteurl, ");
            sqlCommand.Append("up.signature As postauthorsignature ");

            sqlCommand.Append("FROM	mp_forumposts p ");

            sqlCommand.Append("JOIN	mp_forumthreads ft ");
            sqlCommand.Append("ON p.threadid = ft.threadid ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON ft.mostrecentpostuserid = u.userid ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users s ");
            sqlCommand.Append("ON ft.startedbyuserid = s.userid ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users up ");
            sqlCommand.Append("ON up.userid = p.userid ");

            sqlCommand.Append("WHERE ft.threadid = :threadid ");

            sqlCommand.Append("ORDER BY p.postid   ;");


            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader ForumThreadGetPostsReverseSorted(int threadId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT	p.*, ");

            sqlCommand.Append("ft.forumid As forumid, ");
            sqlCommand.Append("ft.islocked As islocked, ");
            // TODO:
            //using 'Guest' here is not culture neutral, need to pass in a label
            sqlCommand.Append("COALESCE(u.name, 'Guest') As mostrecentpostuser, ");
            sqlCommand.Append("COALESCE(s.name, 'Guest') As startedby, ");
            sqlCommand.Append("COALESCE(up.name, 'Guest') As postauthor, ");
            sqlCommand.Append("COALESCE(up.email, '') As authoremail, ");
            sqlCommand.Append("COALESCE(up.totalposts, 0) As postauthortotalposts, ");
            sqlCommand.Append("COALESCE(up.totalrevenue, 0) As userrevenue, ");
            sqlCommand.Append("COALESCE(up.trusted, true) As trusted, ");
            sqlCommand.Append("COALESCE(up.avatarurl, 'blank.gif') As postauthoravatar, ");
            sqlCommand.Append("up.websiteurl As postauthorwebsiteurl, ");
            sqlCommand.Append("up.signature As postauthorsignature ");

            sqlCommand.Append("FROM	mp_forumposts p ");

            sqlCommand.Append("JOIN	mp_forumthreads ft ");
            sqlCommand.Append("ON p.threadid = ft.threadid ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON ft.mostrecentpostuserid = u.userid ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users s ");
            sqlCommand.Append("ON ft.startedbyuserid = s.userid ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users up ");
            sqlCommand.Append("ON up.userid = p.userid ");

            sqlCommand.Append("WHERE ft.threadid = :threadid ");

            sqlCommand.Append("ORDER BY p.threadsequence DESC  ;");
            
            
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            //return NpgsqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_forumposts_selectallbythreadreversesorted(:threadid)",
            //    arParams);

        }

        public static IDataReader ForumThreadGetSortedPosts(int threadId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_forumposts_selectsortedthread(:threadid)",
                arParams);

        }

        public static IDataReader ForumThreadGetPostsByPage(int siteId, int pageId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            
            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("pageid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  fp.*, ");
            sqlCommand.Append("f.moduleid, ");
            sqlCommand.Append("f.itemid , ");
            sqlCommand.Append("m.moduletitle , ");
            sqlCommand.Append("m.viewroles, ");
            sqlCommand.Append("md.featurename, ");
            sqlCommand.Append("md.resourcefile  ");
            sqlCommand.Append("FROM	mp_forumposts fp ");
            sqlCommand.Append("JOIN	mp_forumthreads ft ");
            sqlCommand.Append("ON fp.threadid = ft.threadid ");
            sqlCommand.Append("JOIN	mp_forums f ");
            sqlCommand.Append("ON f.itemid = ft.forumid ");
            sqlCommand.Append("JOIN	mp_modules m ");
            sqlCommand.Append("ON f.moduleid = m.moduleid ");
            sqlCommand.Append("JOIN	mp_moduledefinitions md ");
            sqlCommand.Append("ON m.moduledefid = md.moduledefid ");
            sqlCommand.Append("JOIN	mp_pagemodules pm ");
            sqlCommand.Append("ON m.moduleid = pm.moduleid ");
            sqlCommand.Append("JOIN	mp_pages p ");
            sqlCommand.Append("ON p.pageid = pm.pageid ");
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

        public static IDataReader ForumThreadGetThreadsByPage(int siteId, int pageId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];


            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("pageid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ft.*, ");
            sqlCommand.Append("f.moduleid, ");
            sqlCommand.Append("f.itemid , ");
            sqlCommand.Append("m.moduletitle , ");
            sqlCommand.Append("m.viewroles, ");
            sqlCommand.Append("md.featurename, ");
            sqlCommand.Append("md.resourcefile  ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_forumthreads ft ");
           
            sqlCommand.Append("JOIN	mp_forums f ");
            sqlCommand.Append("ON f.itemid = ft.forumid ");
            sqlCommand.Append("JOIN	mp_modules m ");
            sqlCommand.Append("ON f.moduleid = m.moduleid ");
            sqlCommand.Append("JOIN	mp_moduledefinitions md ");
            sqlCommand.Append("ON m.moduledefid = md.moduledefid ");
            sqlCommand.Append("JOIN	mp_pagemodules pm ");
            sqlCommand.Append("ON m.moduleid = pm.moduleid ");
            sqlCommand.Append("JOIN	mp_pages p ");
            sqlCommand.Append("ON p.pageid = pm.pageid ");
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

        public static IDataReader ForumThreadGetPostsForRss(int siteId, int pageId, int moduleId, int itemId, int threadId, int maximumDays)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[6];
            
            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("pageid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            arParams[2] = new NpgsqlParameter("moduleId", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleId;

            arParams[3] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = itemId;

            arParams[4] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = threadId;

            arParams[5] = new NpgsqlParameter("maximumdays", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = maximumDays;

            DateTime currentTime = DateTime.UtcNow;
            if (maximumDays > -1)
            {
                currentTime = DateTime.UtcNow.AddDays(-maximumDays);
            }

            arParams[6] = new NpgsqlParameter("currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = currentTime;

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT fp.*, ");
            sqlCommand.Append("ft.threadsubject, ");
            sqlCommand.Append("ft.forumid, ");
            sqlCommand.Append("p.pageid, ");
            sqlCommand.Append("p.authorizedroles, ");
            sqlCommand.Append("m.moduleid, ");
            sqlCommand.Append("m.viewroles, ");

            sqlCommand.Append("COALESCE(s.name,'Guest') as startedby, ");
            sqlCommand.Append("COALESCE(up.Name, 'Guest') as postauthor, ");
            sqlCommand.Append("up.totalposts as postauthortotalposts,");
            sqlCommand.Append("up.avatarurl as postauthoravatar, ");
            sqlCommand.Append("up.websiteurl as postauthorwebsiteurl, ");
            sqlCommand.Append("up.signature as postauthorsignature ");

            sqlCommand.Append("FROM	mp_forumposts fp ");

            sqlCommand.Append("JOIN	mp_forumthreads ft ");
            sqlCommand.Append("ON fp.threadid = ft.threadid ");

            sqlCommand.Append("JOIN mp_forums f ");
            sqlCommand.Append("ON ft.forumid = f.itemid ");

            sqlCommand.Append("JOIN	mp_modules m ");
            sqlCommand.Append("ON f.moduleid = m.moduleid ");

            sqlCommand.Append("JOIN mp_pagemodules pm ");
            sqlCommand.Append("ON pm.moduleid = m.moduleid ");

            sqlCommand.Append("JOIN mp_pages p ");
            sqlCommand.Append("ON pm.pageid = p.pageid ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users u ");
            sqlCommand.Append("ON ft.mostrecentpostuserid = u.userid ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users s ");
            sqlCommand.Append("ON ft.startedbyuserid = s.userid ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users up ");
            sqlCommand.Append("ON up.userid = fp.userid ");

            sqlCommand.Append("WHERE p.siteid = :siteid ");
            sqlCommand.Append("AND	(:pageid = -1 OR p.pageid = :pageid) ");
            sqlCommand.Append("AND	(:moduleid = -1 OR m.moduleid = :moduleid) ");
            sqlCommand.Append("AND	(:itemid = -1 OR f.itemid = :itemid) ");
            sqlCommand.Append("AND	(:threadid = -1 OR ft.threadid = :threadid) ");
            sqlCommand.Append("AND	(:maximumdays = -1 OR fp.postdate >= :currenttime) ");

            sqlCommand.Append("ORDER BY	fp.postdate DESC ; ");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


            //return NpgsqlHelper.ExecuteReader(
            //    ConnectionString.GetReadConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_forumposts_selectforrss",
            //    arParams);

        }

        public static DataSet ForumThreadGetSubscribers(int forumId, int threadId, int currentPostUserId, bool includeCurrentUser)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT u.email As email, ");
            sqlCommand.Append("COALESCE(fts.threadsubscriptionid, -1) AS threadsubid, ");
            sqlCommand.Append("COALESCE(fs.subscriptionid, -1) AS forumsubid, ");
            sqlCommand.Append("COALESCE(fts.subguid, '00000000-0000-0000-0000-000000000000') AS threadsubguid, ");
            sqlCommand.Append("COALESCE(fs.subguid, '00000000-0000-0000-0000-000000000000') AS forumsubguid ");


            sqlCommand.Append("FROM	mp_users u ");

            sqlCommand.Append("LEFT OUTER JOIN mp_forumthreadsubscriptions fts ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("fts.userid = u.userid ");
            sqlCommand.Append("AND fts.threadid = :threadid ");
            sqlCommand.Append("AND fts.unsubscribedate IS NULL ");

            sqlCommand.Append("LEFT OUTER JOIN mp_forumsubscriptions fs ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("fs.userid = u.userid ");
            sqlCommand.Append("AND fs.forumid = :forumid ");
            sqlCommand.Append("AND fs.unsubscribedate IS NULL ");

            sqlCommand.Append("WHERE  ");

            if (!includeCurrentUser)
            {
                sqlCommand.Append(" u.userid <> :currentpostuserid ");
                sqlCommand.Append("AND ");
            }

            sqlCommand.Append("(");

            sqlCommand.Append("(");
            sqlCommand.Append("fts.threadsubscriptionid IS NOT NULL ");
            sqlCommand.Append(")");

            sqlCommand.Append("OR ");

            sqlCommand.Append("(");
            sqlCommand.Append("fs.subscriptionid IS NOT NULL ");
            sqlCommand.Append(")");

            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("forumid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = forumId;

            arParams[1] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadId;

            arParams[2] = new NpgsqlParameter("currentpostuserid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = currentPostUserId;

            return NpgsqlHelper.ExecuteDataset(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
            
            //return NpgsqlHelper.ExecuteDataset(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_forumthreadsubscribers_selectbythread(:threadid,:currentpostuserid)",
            //    arParams);

        }

        public static bool ForumThreadAddSubscriber(int threadId, int userId, Guid subGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COUNT(*) As subscriptioncount ");
            sqlCommand.Append("FROM mp_forumthreadsubscriptions  ");
            sqlCommand.Append("WHERE threadid = :threadid AND userid = :userid AND unsubscribedate IS NULL ; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int subscriptionCount = 0;
            int rowsAffected = -1;

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    subscriptionCount = Convert.ToInt32(reader["SubscriptionCount"]);
                }
            }

            if (subscriptionCount > 0)
            {
                sqlCommand = new StringBuilder();
                sqlCommand.Append("UPDATE mp_forumthreadsubscriptions ");
                sqlCommand.Append("SET unsubscribedate = :subscribedate ");

                sqlCommand.Append("WHERE threadid = :threadid AND userid = :userid AND unsubscribedate IS NULL ;");

                arParams = new NpgsqlParameter[3];

                arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = threadId;

                arParams[1] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = userId;

                arParams[2] = new NpgsqlParameter("subscribedate", NpgsqlTypes.NpgsqlDbType.Timestamp);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = DateTime.UtcNow;

                rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));


            }


            sqlCommand = new StringBuilder();

            sqlCommand.Append("INSERT INTO	mp_forumthreadsubscriptions ( ");
            sqlCommand.Append("threadid, ");
            sqlCommand.Append("userid, ");
            sqlCommand.Append("subguid, ");
            sqlCommand.Append("subscribedate ");
            sqlCommand.Append(") ");

            sqlCommand.Append("VALUES ( ");
            sqlCommand.Append(":threadid, ");
            sqlCommand.Append(":userid, ");
            sqlCommand.Append(":subguid, ");
            sqlCommand.Append(":subscribedate ");
            sqlCommand.Append(") ;");

            arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new NpgsqlParameter("subscribedate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            arParams[3] = new NpgsqlParameter("subguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = subGuid.ToString();


            rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (rowsAffected > 0);

        }


        public static bool ForumThreadUnSubscribe(Guid subGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_forumthreadsubscriptions ");
            sqlCommand.Append("SET unsubscribedate = :subscribedate ");
            sqlCommand.Append("WHERE subguid = :subguid  ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("subguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subGuid.ToString();

            arParams[1] = new NpgsqlParameter("subscribedate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (rowsAffected > 0);


        }

        public static IDataReader ForumThreadGetSubscriber(Guid subGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_forumthreadsubscriptions ");

            sqlCommand.Append("WHERE subguid = :subguid ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("subguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = subGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static bool ForumThreadUNSubscribe(int threadId, int userId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;
            
            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_forumthreadsubscriptions_unsubscribethread(:threadid,:userid)",
                arParams));

            return (rowsAffected > 0);

        }

        public static bool ForumThreadUnsubscribeAll(int userId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;
            
            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_forumthreadsubscriptions_unsubscribeallthreads(:userid)",
                arParams));

            return (rowsAffected > 0);

        }

        public static int ForumPostCreate(
            int threadId,
            string subject,
            string post,
            bool approved,
            int userId,
            DateTime postDate,
            Guid postGuid,
            Guid approvedBy,
            DateTime approvedUtc,
            string userIp,
            bool notificationSent,
            int modStatus)
        {

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT COALESCE(Max(threadsequence) + 1,1) As threadsequence FROM mp_forumposts WHERE threadid = :threadid ; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[13];
            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            int threadSequence = 1;

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    threadSequence = Convert.ToInt32(reader["ThreadSequence"]);
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_forumposts (");
            sqlCommand.Append("threadid, ");
            sqlCommand.Append("threadsequence, ");
            sqlCommand.Append("subject, ");
            sqlCommand.Append("postdate, ");
            sqlCommand.Append("approved, ");
            sqlCommand.Append("userid, ");
            sqlCommand.Append("post, ");
            sqlCommand.Append("postguid, ");
            sqlCommand.Append("answervotes, ");
            sqlCommand.Append("approvedby, ");
            sqlCommand.Append("approvedutc, ");

            sqlCommand.Append("notificationsent, ");
            sqlCommand.Append("modstatus, ");

            sqlCommand.Append("userip )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":threadid, ");
            sqlCommand.Append(":threadsequence, ");
            sqlCommand.Append(":subject, ");
            sqlCommand.Append(":postdate, ");
            sqlCommand.Append(":approved, ");
            sqlCommand.Append(":userid, ");
            sqlCommand.Append(":post, ");
            sqlCommand.Append(":postguid, ");
            sqlCommand.Append(":answervotes, ");
            sqlCommand.Append(":approvedby, ");
            sqlCommand.Append(":approvedutc, ");

            sqlCommand.Append(":notificationsent, ");
            sqlCommand.Append(":modstatus, ");

            sqlCommand.Append(":userip )");
            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT currval('mp_forumposts_postid_seq');");

            arParams = new NpgsqlParameter[14];

            arParams[0] = new NpgsqlParameter("threadid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = threadId;

            arParams[1] = new NpgsqlParameter("threadsequence", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadSequence;

            arParams[2] = new NpgsqlParameter("subject", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = subject;

            arParams[3] = new NpgsqlParameter("postdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = postDate;

            arParams[4] = new NpgsqlParameter("approved", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = approved;

            arParams[5] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = userId;

            arParams[6] = new NpgsqlParameter("post", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = post;

            arParams[7] = new NpgsqlParameter("postguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = postGuid.ToString();

            arParams[8] = new NpgsqlParameter("answervotes", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = 0;

            arParams[9] = new NpgsqlParameter("approvedby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = approvedBy.ToString();

            arParams[10] = new NpgsqlParameter("approvedutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[10].Direction = ParameterDirection.Input;
            if (approvedUtc > DateTime.MinValue)
            {
                arParams[10].Value = approvedUtc;
            }
            else
            {
                arParams[10].Value = DBNull.Value;
            }

            arParams[11] = new NpgsqlParameter("userip", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = userIp;

            arParams[12] = new NpgsqlParameter("notificationsent", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = notificationSent;

            arParams[13] = new NpgsqlParameter("modstatus", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = modStatus;


            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));


            return newID;
            
            
            

        }

        public static bool ForumPostUpdate(
            int postId,
            string subject,
            string post,
            int sortOrder,
            bool approved,
            Guid approvedBy,
            DateTime approvedUtc,
            bool notificationSent,
            int modStatus)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_forumposts ");
            sqlCommand.Append("SET ");
            sqlCommand.Append("subject = :subject, ");
           
            sqlCommand.Append("approved = :approved, ");
            sqlCommand.Append("sortorder = :sortorder, ");
            sqlCommand.Append("post = :post, ");
            sqlCommand.Append("notificationsent = :notificationsent, ");
            sqlCommand.Append("modstatus = :modstatus,");
            sqlCommand.Append("approvedby = :approvedby, ");
            sqlCommand.Append("approvedutc = :approvedutc ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("postid = :postid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[9];

            arParams[0] = new NpgsqlParameter("postid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            arParams[1] = new NpgsqlParameter("subject", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = subject;

            arParams[2] = new NpgsqlParameter("approved", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = approved;

            arParams[3] = new NpgsqlParameter("sortorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new NpgsqlParameter("post", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = post;

            arParams[5] = new NpgsqlParameter("approvedby", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = approvedBy.ToString();

            arParams[6] = new NpgsqlParameter("approvedutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[6].Direction = ParameterDirection.Input;
            if (approvedUtc > DateTime.MinValue)
            {
                arParams[6].Value = approvedUtc;
            }
            else
            {
                arParams[6].Value = DBNull.Value;
            }

            arParams[7] = new NpgsqlParameter("notificationsent", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = notificationSent;

            arParams[8] = new NpgsqlParameter("modstatus", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = modStatus;

            
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
            
            

        }

        public static bool ForumPostUpdateThreadSequence(
            int postId,
            int threadSequence)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter("postid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            arParams[1] = new NpgsqlParameter("threadsequence", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = threadSequence;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_forumposts_updatethreadsequence(:postid,:threadsequence)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool ForumPostDelete(int postId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("postid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = postId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_forumposts_delete(:postid)",
                arParams));

            return (rowsAffected > -1);

        }

    }
}
