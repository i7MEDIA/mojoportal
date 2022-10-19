/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2013-04-18
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Text;
using Npgsql;

namespace mojoPortal.Data
{
    
    public static class DBHtmlContent
    {
        
        public static int AddHtmlContent(
            Guid itemGuid,
            Guid moduleGuid,
            int moduleId,
            string title,
            string excerpt,
            string body,
            string moreLink,
            int sortOrder,
            DateTime beginDate,
            DateTime endDate,
            DateTime createdDate,
            int userId,
            Guid userGuid,
            bool excludeFromRecentContent)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_htmlcontent (");
            sqlCommand.Append("moduleid, ");
            sqlCommand.Append("title, ");
            sqlCommand.Append("excerpt, ");
            sqlCommand.Append("body, ");
            sqlCommand.Append("morelink, ");
            sqlCommand.Append("sortorder, ");
            sqlCommand.Append("begindate, ");
            sqlCommand.Append("enddate, ");
            sqlCommand.Append("createddate, ");
            sqlCommand.Append("userid, ");
            sqlCommand.Append("itemguid, ");
            sqlCommand.Append("moduleguid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("excludefromrecentcontent, ");
            sqlCommand.Append("lastmoduserguid, ");
            sqlCommand.Append("lastmodutc ) ");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":moduleid, ");
            sqlCommand.Append(":title, ");
            sqlCommand.Append(":excerpt, ");
            sqlCommand.Append(":body, ");
            sqlCommand.Append(":morelink, ");
            sqlCommand.Append(":sortorder, ");
            sqlCommand.Append(":begindate, ");
            sqlCommand.Append(":enddate, ");
            sqlCommand.Append(":createddate, ");
            sqlCommand.Append(":userid, ");
            sqlCommand.Append(":itemguid, ");
            sqlCommand.Append(":moduleguid, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":excludefromrecentcontent, ");
            sqlCommand.Append(":lastmoduserguid, ");
            sqlCommand.Append(":lastmodutc ) ");
            sqlCommand.Append("; ");
            sqlCommand.Append(" SELECT CURRVAL('mp_htmlcontent_itemid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[16];

            arParams[0] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter(":title", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new NpgsqlParameter(":excerpt", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = excerpt;

            arParams[3] = new NpgsqlParameter(":body", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = body;

            arParams[4] = new NpgsqlParameter(":morelink", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = moreLink;

            arParams[5] = new NpgsqlParameter(":sortorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sortOrder;

            arParams[6] = new NpgsqlParameter(":begindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = beginDate;

            arParams[7] = new NpgsqlParameter(":enddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = endDate;

            arParams[8] = new NpgsqlParameter(":createddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdDate;

            arParams[9] = new NpgsqlParameter(":userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userId;

            arParams[10] = new NpgsqlParameter(":itemguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = itemGuid.ToString();

            arParams[11] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = moduleGuid.ToString();

            arParams[12] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = userGuid.ToString();

            arParams[13] = new NpgsqlParameter(":lastmoduserguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = userGuid.ToString();

            arParams[14] = new NpgsqlParameter(":lastmodutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = createdDate;

            arParams[15] = new NpgsqlParameter(":excludefromrecentcontent", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = excludeFromRecentContent;

            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            //int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
            //    ConnectionString.GetWriteConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_htmlcontent_insert(:moduleid,:title,:excerpt,:body,:morelink,:sortorder,:begindate,:enddate,:createddate,:userid,:itemguid,:moduleguid,:userguid)",
            //    arParams));

            return newID;

        }

        public static bool UpdateHtmlContent(
          int itemId,
          int moduleId,
          string title,
          string excerpt,
          string body,
          string moreLink,
          int sortOrder,
          DateTime beginDate,
          DateTime endDate,
          DateTime lastModUtc,
          Guid lastModUserGuid,
          bool excludeFromRecentContent)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_htmlcontent ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("title = :title, ");
            sqlCommand.Append("excerpt = :excerpt, ");
            sqlCommand.Append("body = :body, ");
            sqlCommand.Append("morelink = :morelink, ");
            sqlCommand.Append("sortorder = :sortorder, ");
            sqlCommand.Append("begindate = :begindate, ");
            sqlCommand.Append("enddate = :enddate, ");
            sqlCommand.Append("excludefromrecentcontent = :excludefromrecentcontent, ");
            sqlCommand.Append("lastmoduserguid = :lastmoduserguid, ");
            sqlCommand.Append("lastmodutc = :lastmodutc ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("itemid = :itemid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[11];
            
            arParams[0] = new NpgsqlParameter(":itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new NpgsqlParameter(":title", NpgsqlTypes.NpgsqlDbType.Text, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new NpgsqlParameter(":excerpt", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = excerpt;

            arParams[3] = new NpgsqlParameter(":body", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = body;

            arParams[4] = new NpgsqlParameter(":morelink", NpgsqlTypes.NpgsqlDbType.Text, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = moreLink;

            arParams[5] = new NpgsqlParameter(":sortorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sortOrder;

            arParams[6] = new NpgsqlParameter(":begindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = beginDate;

            arParams[7] = new NpgsqlParameter(":enddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = endDate;

            arParams[8] = new NpgsqlParameter(":lastmoduserguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModUserGuid.ToString();

            arParams[9] = new NpgsqlParameter(":lastmodutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModUtc;

            arParams[10] = new NpgsqlParameter(":excludefromrecentcontent", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = excludeFromRecentContent;

            //int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
            //    ConnectionString.GetWriteConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_htmlcontent_update(:itemid,:moduleid,:title,:excerpt,:body,:morelink,:sortorder,:begindate,:enddate,:lastmoduserguid,:lastmodutc)",
            //    arParams));

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (rowsAffected > -1);

        }

        public static bool DeleteHtmlContent(int itemId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_htmlcontent_delete(:itemid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(int moduleId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_contenthistory ");
            sqlCommand.Append("WHERE contentguid IN (SELECT itemguid FROM mp_htmlcontent WHERE moduleid  ");
            sqlCommand.Append(" = :moduleid ); ");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_contentrating ");
            sqlCommand.Append("WHERE contentguid IN (SELECT itemguid FROM mp_htmlcontent WHERE moduleid  ");
            sqlCommand.Append(" = :moduleid ); ");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_htmlcontent ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid  = :moduleid ");
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

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_contenthistory ");
            sqlCommand.Append("WHERE contentguid IN (SELECT itemguid FROM mp_htmlcontent WHERE moduleid IN ");
            sqlCommand.Append("(SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ); ");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_contentrating ");
            sqlCommand.Append("WHERE contentguid IN (SELECT itemguid FROM mp_htmlcontent WHERE moduleid IN ");
            sqlCommand.Append("(SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ); ");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_htmlcontent ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid IN (SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ");
            sqlCommand.Append(";");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static IDataReader GetHtmlContent(int moduleId, int itemId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT h.*, ");
            sqlCommand.Append("u1.name AS createdbyname, ");
            sqlCommand.Append("u1.firstname AS createdbyfirstname, ");
            sqlCommand.Append("u1.lastname AS createdbylastname, ");
            sqlCommand.Append("u1.email AS createdbyemail, ");

            sqlCommand.Append("u1.authorbio, ");
            sqlCommand.Append("u1.avatarurl, ");
            sqlCommand.Append("COALESCE(u1.userid, -1) As authoruserid, ");

            sqlCommand.Append("u2.name AS lastmodbyname, ");
            sqlCommand.Append("u2.firstname AS lastmodbyfirstname, ");
            sqlCommand.Append("u2.lastname AS lastmodbylastname, ");
            sqlCommand.Append("u2.email AS lastmodByemail ");

            sqlCommand.Append("FROM	mp_htmlcontent h ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_users u1 ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("h.userguid = u1.userguid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_users u2 ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("h.lastmoduserguid = u2.userguid ");

            sqlCommand.Append("WHERE h.itemid = :itemid  ;");



            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetHtmlForMetaWeblogApi(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("pm.*, ");
            sqlCommand.Append("m.moduletitle, ");
            sqlCommand.Append("m.sauthorizededitroles, ");
            sqlCommand.Append("m.isglobal, ");
            sqlCommand.Append("h.body, ");
            sqlCommand.Append("h.itemid, ");
            sqlCommand.Append("h.itemguid, ");
            sqlCommand.Append("h.lastmoduserguid, ");
            sqlCommand.Append("h.lastmodutc, ");
            sqlCommand.Append("p.pageguid, ");
            sqlCommand.Append("p.parentid, ");
            sqlCommand.Append("p.parentguid, ");
            sqlCommand.Append("p.pagename, ");
            sqlCommand.Append("p.useurl, ");
            sqlCommand.Append("p.url, ");
            sqlCommand.Append("p.reditroles, ");
            sqlCommand.Append("p.pageorder, ");
            sqlCommand.Append("p.enablecomments, ");
            sqlCommand.Append("p.ispending, ");
            sqlCommand.Append("pp.pagename As parentname ");


            sqlCommand.Append("FROM	mp_pagemodules pm ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pm.moduleid = m.moduleid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_htmlcontent h ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("h.moduleid = m.moduleid ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_moduledefinitions md ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("md.moduledefid = m.moduledefid ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_pages p ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pm.pageid = p.pageid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_pages pp ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pp.pageid = p.parentid ");

            sqlCommand.Append("WHERE p.siteid = :siteid  ");

            sqlCommand.Append("AND ");
            sqlCommand.Append("md.guid = '881e4e00-93e4-444c-b7b0-6672fb55de10' ");

            sqlCommand.Append("AND ");
            sqlCommand.Append("pm.panename = 'contentpane' ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("p.pagename, pm.moduleorder ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetHtmlContent(
            int moduleId,
            DateTime beginDate)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter(":begindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT h.*, ");
            sqlCommand.Append("u1.name AS createdbyname, ");
            sqlCommand.Append("u1.firstname AS createdbyfirstname, ");
            sqlCommand.Append("u1.lastname AS createdbylastname, ");
            sqlCommand.Append("u1.email AS createdbyemail, ");
            sqlCommand.Append("u1.authorbio, ");
            sqlCommand.Append("u1.avatarurl, ");
            sqlCommand.Append("COALESCE(u1.userid, -1) As authoruserid, ");
            sqlCommand.Append("u2.name AS lastmodbyname, ");
            sqlCommand.Append("u2.firstname AS lastmodbyfirstname, ");
            sqlCommand.Append("u2.lastname AS lastmodbylastname, ");
            sqlCommand.Append("u2.email AS lastmodByemail ");

            sqlCommand.Append("FROM	mp_htmlcontent h ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_users u1 ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("h.userguid = u1.userguid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_users u2 ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("h.lastmoduserguid = u2.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("h.moduleid = :moduleid ");
            sqlCommand.Append("AND h.begindate <= :begindate  ");
            sqlCommand.Append("AND h.enddate >= :begindate  ");
            sqlCommand.Append("ORDER BY h.BeginDate DESC ");
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetHtmlContentByPage(int siteId, int pageId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  h.*, ");

            sqlCommand.Append("m.moduletitle, ");
            sqlCommand.Append("m.viewroles, ");
            sqlCommand.Append("m.IncludeInSearch, ");
            sqlCommand.Append("md.featurename, ");

            sqlCommand.Append("u1.name AS createdbyname, ");
            sqlCommand.Append("u1.firstname AS createdbyfirstname, ");
            sqlCommand.Append("u1.lastname AS createdbylastname, ");
            sqlCommand.Append("u1.email AS createdbyemail, ");
            sqlCommand.Append("u1.authorbio, ");
            sqlCommand.Append("u1.avatarurl, ");
            sqlCommand.Append("COALESCE(u1.userid, -1) As authoruserid ");

            sqlCommand.Append("FROM	mp_htmlcontent h ");

            sqlCommand.Append("JOIN	mp_modules m ");
            sqlCommand.Append("ON h.moduleid = m.moduleid ");

            sqlCommand.Append("JOIN	mp_moduledefinitions md ");
            sqlCommand.Append("ON m.moduledefid = md.moduledefid ");

            sqlCommand.Append("JOIN	mp_pagemodules pm ");
            sqlCommand.Append("ON m.moduleid = pm.moduleid ");

            sqlCommand.Append("JOIN	mp_pages p ");
            sqlCommand.Append("ON p.pageid = pm.pageid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_users u1 ");
            sqlCommand.Append("ON h.userguid = u1.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.siteid = :siteid ");
            sqlCommand.Append("AND pm.pageid = :pageid ");

            sqlCommand.Append(" ; ");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
            
            
            //return NpgsqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_htmlcontent_selectbypage(:siteid,:pageid)",
            //    arParams);

        }

    }
}
