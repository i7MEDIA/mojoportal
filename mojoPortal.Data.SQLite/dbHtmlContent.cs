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
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using Mono.Data.Sqlite;

namespace mojoPortal.Data
{
   
    public static class DBHtmlContent
    {
        
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
            int exclude = 0;
            if (excludeFromRecentContent) { exclude = 1; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_HtmlContent (");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Excerpt, ");
            sqlCommand.Append("Body, ");
            sqlCommand.Append("MoreLink, ");
            sqlCommand.Append("SortOrder, ");
            sqlCommand.Append("BeginDate, ");
            sqlCommand.Append("EndDate, ");
            sqlCommand.Append("CreatedDate, ");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("ExcludeFromRecentContent, ");
            sqlCommand.Append("LastModUserGuid, ");
            sqlCommand.Append("LastModUtc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":ModuleID, ");
            sqlCommand.Append(":Title, ");
            sqlCommand.Append(":Excerpt, ");
            sqlCommand.Append(":Body, ");
            sqlCommand.Append(":MoreLink, ");
            sqlCommand.Append(":SortOrder, ");
            sqlCommand.Append(":BeginDate, ");
            sqlCommand.Append(":EndDate, ");
            sqlCommand.Append(":CreatedDate, ");
            sqlCommand.Append(":UserID, ");
            sqlCommand.Append(":ItemGuid, ");
            sqlCommand.Append(":ModuleGuid, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":ExcludeFromRecentContent, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":CreatedDate )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[14];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":Title", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new SqliteParameter(":Excerpt", DbType.Object);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = excerpt;

            arParams[3] = new SqliteParameter(":Body", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = body;

            arParams[4] = new SqliteParameter(":MoreLink", DbType.String, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = moreLink;

            arParams[5] = new SqliteParameter(":BeginDate", DbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = beginDate;

            arParams[6] = new SqliteParameter(":EndDate", DbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = endDate;

            arParams[7] = new SqliteParameter(":CreatedDate", DbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdDate;

            arParams[8] = new SqliteParameter(":UserID", DbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = userId;

            arParams[9] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = sortOrder;

            arParams[10] = new SqliteParameter(":ItemGuid", DbType.String, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = itemGuid.ToString();

            arParams[11] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = moduleGuid.ToString();

            arParams[12] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = userGuid.ToString();

            arParams[13] = new SqliteParameter(":ExcludeFromRecentContent", DbType.Int32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = exclude;

            int newID = 0;

            newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

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
            int exclude = 0;
            if (excludeFromRecentContent) { exclude = 1; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_HtmlContent ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("BeginDate = :BeginDate  , ");
            sqlCommand.Append("EndDate = :EndDate  , ");
            sqlCommand.Append("Title = :Title  , ");
            sqlCommand.Append("Excerpt = :Excerpt  , ");
            sqlCommand.Append("Body = :Body , ");
            sqlCommand.Append("MoreLink = :MoreLink, ");
            sqlCommand.Append("ExcludeFromRecentContent = :ExcludeFromRecentContent, ");
            sqlCommand.Append("LastModUserGuid = :LastModUserGuid, ");
            sqlCommand.Append("LastModUtc = :LastModUtc "); 

            sqlCommand.Append("WHERE ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[11];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqliteParameter(":Title", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new SqliteParameter(":Excerpt", DbType.Object);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = excerpt;

            arParams[3] = new SqliteParameter(":Body", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = body;

            arParams[4] = new SqliteParameter(":MoreLink", DbType.String, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = moreLink;

            arParams[5] = new SqliteParameter(":BeginDate", DbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = beginDate;

            arParams[6] = new SqliteParameter(":EndDate", DbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = endDate;

            arParams[7] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = sortOrder;

            arParams[8] = new SqliteParameter(":LastModUserGuid", DbType.String, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModUserGuid.ToString();

            arParams[9] = new SqliteParameter(":LastModUtc", DbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModUtc;

            arParams[10] = new SqliteParameter(":ExcludeFromRecentContent", DbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = exclude;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteHtmlContent(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_HtmlContent ");
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
            sqlCommand.Append("DELETE FROM mp_ContentHistory ");
            sqlCommand.Append("WHERE ContentGuid IN (SELECT ItemGuid FROM mp_HtmlContent WHERE ModuleID  ");
            sqlCommand.Append(" = :ModuleID ); ");

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentRating ");
            sqlCommand.Append("WHERE ContentGuid IN (SELECT ItemGuid FROM mp_HtmlContent WHERE ModuleID  ");
            sqlCommand.Append(" = :ModuleID ); ");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_HtmlContent ");
            sqlCommand.Append("WHERE ModuleID = :ModuleID ;");

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
            sqlCommand.Append("DELETE FROM mp_ContentHistory ");
            sqlCommand.Append("WHERE ContentGuid IN (SELECT ItemGuid FROM mp_HtmlContent WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID) ); ");

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentRating ");
            sqlCommand.Append("WHERE ContentGuid IN (SELECT ItemGuid FROM mp_HtmlContent WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID) ); ");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_HtmlContent ");
            sqlCommand.Append("WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID) ;");

            rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static IDataReader GetHtmlContent(int moduleId, int itemId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("h.ItemID AS ItemID, ");
            sqlCommand.Append("h.ModuleID AS ModuleID, ");
            sqlCommand.Append("h.Title AS Title, ");
            //sqlCommand.Append("h.Excerpt AS Excerpt, ");
            sqlCommand.Append("h.Body AS Body, ");
            sqlCommand.Append("h.MoreLink AS MoreLink, ");
            sqlCommand.Append("h.SortOrder AS SortOrder, ");
            //sqlCommand.Append("h.BeginDate AS BeginDate, ");
            //sqlCommand.Append("h.EndDate AS EndDate, ");
            sqlCommand.Append("h.CreatedDate AS CreatedDate, ");
            sqlCommand.Append("h.UserID AS UserID, ");
            sqlCommand.Append("h.ItemGuid AS ItemGuid, ");
            sqlCommand.Append("h.ModuleGuid AS ModuleGuid, ");
            sqlCommand.Append("h.UserGuid AS UserGuid, ");
            sqlCommand.Append("h.LastModUserGuid AS LastModUserGuid, ");
            sqlCommand.Append("h.LastModUtc AS LastModUtc, ");
            sqlCommand.Append("h.ExcludeFromRecentContent AS ExcludeFromRecentContent, ");

            sqlCommand.Append("u1.Name AS CreatedByName, ");
            sqlCommand.Append("u1.FirstName AS CreatedByFirstName, ");
            sqlCommand.Append("u1.LastName AS CreatedByLastName, ");
            sqlCommand.Append("u1.Email AS CreatedByEmail, ");
            sqlCommand.Append("u1.AuthorBio, ");
            sqlCommand.Append("u1.AvatarUrl, ");
            sqlCommand.Append("COALESCE(u1.UserID, -1) As AuthorUserID, ");
            sqlCommand.Append("u2.Name AS LastModByName, ");
            sqlCommand.Append("u2.FirstName AS LastModByFirstName, ");
            sqlCommand.Append("u2.LastName AS LastModByLastName, ");
            sqlCommand.Append("u2.Email AS LastModByEmail ");

            sqlCommand.Append("FROM	mp_HtmlContent h ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users u1 ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("h.UserGuid = u1.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users u2 ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("h.LastModUserGuid = u2.UserGuid ");

            sqlCommand.Append("WHERE h.ItemID = :ItemID  ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetHtmlForMetaWeblogApi(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("pm.*, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.AuthorizedEditRoles, ");
            sqlCommand.Append("m.IsGlobal, ");
            sqlCommand.Append("h.Body, ");
            sqlCommand.Append("h.ItemID, ");
            sqlCommand.Append("h.ItemGuid, ");
            sqlCommand.Append("h.LastModUserGuid, ");
            sqlCommand.Append("h.LastModUtc, ");
            sqlCommand.Append("p.PageGuid, ");
            sqlCommand.Append("p.ParentID, ");
            sqlCommand.Append("p.ParentGuid, ");
            sqlCommand.Append("p.PageName, ");
            sqlCommand.Append("p.UseUrl, ");
            sqlCommand.Append("p.Url, ");
            sqlCommand.Append("p.EditRoles, ");
            sqlCommand.Append("p.PageOrder, ");
            sqlCommand.Append("p.EnableComments, ");
            sqlCommand.Append("p.IsPending, ");
            sqlCommand.Append("pp.PageName As ParentName ");


            sqlCommand.Append("FROM	mp_PageModules pm ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pm.ModuleID = m.ModuleID ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_HtmlContent h ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("h.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_ModuleDefinitions md ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("md.ModuleDefID = m.ModuleDefID ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Pages p ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pm.PageID = p.PageID ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Pages pp ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pp.PageID = p.ParentID ");

            sqlCommand.Append("WHERE p.SiteID = :SiteID  ");

            sqlCommand.Append("AND ");
            sqlCommand.Append("md.Guid = '881e4e00-93e4-444c-b7b0-6672fb55de10' ");

            sqlCommand.Append("AND ");
            sqlCommand.Append("pm.PaneName = 'contentpane' ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("p.PageName, pm.ModuleOrder ");

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetHtmlContent(
            int moduleId,
            DateTime beginDate)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("h.ItemID AS ItemID, ");
            sqlCommand.Append("h.ModuleID AS ModuleID, ");
            sqlCommand.Append("h.Title AS Title, ");
            //sqlCommand.Append("h.Excerpt AS Excerpt, ");
            sqlCommand.Append("h.Body AS Body, ");
            sqlCommand.Append("h.MoreLink AS MoreLink, ");
            sqlCommand.Append("h.SortOrder AS SortOrder, ");
            //sqlCommand.Append("h.BeginDate AS BeginDate, ");
            //sqlCommand.Append("h.EndDate AS EndDate, ");
            sqlCommand.Append("h.CreatedDate AS CreatedDate, ");
            sqlCommand.Append("h.UserID AS UserID, ");
            sqlCommand.Append("h.ItemGuid AS ItemGuid, ");
            sqlCommand.Append("h.ModuleGuid AS ModuleGuid, ");
            sqlCommand.Append("h.UserGuid AS UserGuid, ");
            sqlCommand.Append("h.LastModUserGuid AS LastModUserGuid, ");
            sqlCommand.Append("h.LastModUtc AS LastModUtc, ");
            sqlCommand.Append("h.ExcludeFromRecentContent AS ExcludeFromRecentContent, ");

            sqlCommand.Append("u1.Name AS CreatedByName, ");
            sqlCommand.Append("u1.FirstName AS CreatedByFirstName, ");
            sqlCommand.Append("u1.LastName AS CreatedByLastName, ");
            sqlCommand.Append("u1.Email AS CreatedByEmail, ");
            sqlCommand.Append("u1.AuthorBio, ");
            sqlCommand.Append("u1.AvatarUrl, ");
            sqlCommand.Append("COALESCE(u1.UserID, -1) As AuthorUserID, ");
            sqlCommand.Append("u2.Name AS LastModByName, ");
            sqlCommand.Append("u2.FirstName AS LastModByFirstName, ");
            sqlCommand.Append("u2.LastName AS LastModByLastName, ");
            sqlCommand.Append("u2.Email AS LastModByEmail ");

            sqlCommand.Append("FROM	mp_HtmlContent h ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users u1 ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("h.UserGuid = u1.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users u2 ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("h.LastModUserGuid = u2.UserGuid ");


            sqlCommand.Append("WHERE h.ModuleID = :ModuleID  ");
            sqlCommand.Append("AND h.BeginDate <= :BeginDate  ");
            sqlCommand.Append("AND h.EndDate >= :BeginDate  ");
            sqlCommand.Append("ORDER BY h.BeginDate DESC ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":BeginDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetHtmlContentByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  h.*, ");

            sqlCommand.Append("m.ModuleTitle As ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles As ViewRoles, ");
            sqlCommand.Append("m.IncludeInSearch As IncludeInSearch, ");
            sqlCommand.Append("md.FeatureName As FeatureName, ");
            sqlCommand.Append("u1.Name AS CreatedByName, ");
            sqlCommand.Append("u1.FirstName AS CreatedByFirstName, ");
            sqlCommand.Append("u1.LastName AS CreatedByLastName, ");
            sqlCommand.Append("u1.Email AS CreatedByEmail, ");
            sqlCommand.Append("u1.AuthorBio, ");
            sqlCommand.Append("u1.AvatarUrl, ");
            sqlCommand.Append("COALESCE(u1.UserID, -1) As AuthorUserID ");

            sqlCommand.Append("FROM	mp_HtmlContent h ");

            sqlCommand.Append("JOIN	mp_Modules m ");
            sqlCommand.Append("ON h.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN	mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("JOIN	mp_PageModules pm ");
            sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

            sqlCommand.Append("JOIN	mp_Pages p ");
            sqlCommand.Append("ON p.PageID = pm.PageID ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users u1 ");
            sqlCommand.Append("ON h.UserGuid = u1.UserGuid ");

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



        


    }
}
