// Author:					Joe Audette
// Created:					2010-04-04
// Last Modified:			2013-04-18
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
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Data.SqlServerCe;

namespace mojoPortal.Data
{
    public static class DBHtmlContent
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_HtmlContent ");
            sqlCommand.Append("(");
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
            sqlCommand.Append("ExcludeFromRecentContent, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("LastModUserGuid, ");
            sqlCommand.Append("LastModUtc ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ModuleID, ");
            sqlCommand.Append("@Title, ");
            sqlCommand.Append("@Excerpt, ");
            sqlCommand.Append("@Body, ");
            sqlCommand.Append("@MoreLink, ");
            sqlCommand.Append("@SortOrder, ");
            sqlCommand.Append("@BeginDate, ");
            sqlCommand.Append("@EndDate, ");
            sqlCommand.Append("@CreatedDate, ");
            sqlCommand.Append("@UserID, ");
            sqlCommand.Append("@ExcludeFromRecentContent, ");
            sqlCommand.Append("@ItemGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@LastModUserGuid, ");
            sqlCommand.Append("@LastModUtc ");
            sqlCommand.Append(")");
            
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[16];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new SqlCeParameter("@Excerpt", SqlDbType.NText);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = excerpt;

            arParams[3] = new SqlCeParameter("@Body", SqlDbType.NText);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = body;

            arParams[4] = new SqlCeParameter("@MoreLink", SqlDbType.NVarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = moreLink;

            arParams[5] = new SqlCeParameter("@SortOrder", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sortOrder;

            arParams[6] = new SqlCeParameter("@BeginDate", SqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = beginDate;

            arParams[7] = new SqlCeParameter("@EndDate", SqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = endDate;

            arParams[8] = new SqlCeParameter("@CreatedDate", SqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdDate;

            arParams[9] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userId;

            arParams[10] = new SqlCeParameter("@ItemGuid", SqlDbType.UniqueIdentifier);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = itemGuid;

            arParams[11] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = moduleGuid;

            arParams[12] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = userGuid;

            arParams[13] = new SqlCeParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = userGuid;

            arParams[14] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = createdDate;

            arParams[15] = new SqlCeParameter("@ExcludeFromRecentContent", SqlDbType.Bit);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = excludeFromRecentContent;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;

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
            sqlCommand.Append("UPDATE mp_HtmlContent ");
            sqlCommand.Append("SET  ");
            //sqlCommand.Append("ModuleID = @ModuleID, ");
            sqlCommand.Append("Title = @Title, ");
            sqlCommand.Append("Excerpt = @Excerpt, ");
            sqlCommand.Append("Body = @Body, ");
            sqlCommand.Append("MoreLink = @MoreLink, ");
            sqlCommand.Append("SortOrder = @SortOrder, ");
            sqlCommand.Append("BeginDate = @BeginDate, ");
            sqlCommand.Append("EndDate = @EndDate, ");
            sqlCommand.Append("ExcludeFromRecentContent = @ExcludeFromRecentContent, ");
            sqlCommand.Append("LastModUserGuid = @LastModUserGuid, ");
            sqlCommand.Append("LastModUtc = @LastModUtc ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[12];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqlCeParameter("@Excerpt", SqlDbType.NText);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = excerpt;

            arParams[4] = new SqlCeParameter("@Body", SqlDbType.NText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = body;

            arParams[5] = new SqlCeParameter("@MoreLink", SqlDbType.NVarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = moreLink;

            arParams[6] = new SqlCeParameter("@SortOrder", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = sortOrder;

            arParams[7] = new SqlCeParameter("@BeginDate", SqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = beginDate;

            arParams[8] = new SqlCeParameter("@EndDate", SqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = endDate;

            arParams[9] = new SqlCeParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModUserGuid;

            arParams[10] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = lastModUtc;

            arParams[11] = new SqlCeParameter("@ExcludeFromRecentContent", SqlDbType.Bit);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = excludeFromRecentContent;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteHtmlContent(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_HtmlContent ");
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

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_ContentHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid IN (SELECT ItemGuid FROM mp_HtmlContent WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID) ");
            sqlCommand.Append(")");
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


            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentRating ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid IN (SELECT ItemGuid FROM mp_HtmlContent WHERE ModuleID IN ");
            sqlCommand.Append("(SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID) ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_HtmlContent ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID) ");
            sqlCommand.Append(";");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_HtmlContent ");
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

        public static IDataReader GetHtmlContent(int moduleId, int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");

            sqlCommand.Append("h.ItemID AS ItemID, ");
            sqlCommand.Append("h.ModuleID AS ModuleID, ");
            sqlCommand.Append("h.Title AS Title, ");
            sqlCommand.Append("h.Body AS Body, ");
            sqlCommand.Append("h.CreatedDate AS CreatedDate, ");
            sqlCommand.Append("h.ItemGuid AS ItemGuid, ");
            sqlCommand.Append("h.ModuleGuid AS ModuleGuid, ");
            sqlCommand.Append("h.UserGuid AS UserGuid, ");
            sqlCommand.Append("h.LastModUserGuid AS LastModUserGuid, ");
            sqlCommand.Append("h.LastModUtc AS LastModUtc, ");
            sqlCommand.Append("h.ExcludeFromRecentContent AS ExcludeFromRecentContent, ");

            sqlCommand.Append("u1.[Name] AS CreatedByName, ");
            sqlCommand.Append("u1.FirstName AS CreatedByFirstName, ");
            sqlCommand.Append("u1.LastName AS CreatedByLastName, ");
            sqlCommand.Append("u1.Email AS CreatedByEmail, ");
            sqlCommand.Append("u1.AuthorBio, ");
            sqlCommand.Append("u1.AvatarUrl, ");
            sqlCommand.Append("COALESCE(u1.UserID, -1) As AuthorUserID, ");
            sqlCommand.Append("u2.[Name] AS LastModByName, ");
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

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("h.ItemID = @ItemID ");
            sqlCommand.Append(";");

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

            sqlCommand.Append("WHERE p.SiteID = @SiteID  ");

            sqlCommand.Append("AND ");
            sqlCommand.Append("md.Guid = '881e4e00-93e4-444c-b7b0-6672fb55de10' ");

            sqlCommand.Append("AND ");
            sqlCommand.Append("pm.PaneName = 'contentpane' ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("p.PageName, pm.ModuleOrder ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("h.Body AS Body, ");
            sqlCommand.Append("h.CreatedDate AS CreatedDate, ");
            sqlCommand.Append("h.ItemGuid AS ItemGuid, ");
            sqlCommand.Append("h.ModuleGuid AS ModuleGuid, ");
            sqlCommand.Append("h.UserGuid AS UserGuid, ");
            sqlCommand.Append("h.LastModUserGuid AS LastModUserGuid, ");
            sqlCommand.Append("h.LastModUtc AS LastModUtc, ");
            sqlCommand.Append("h.ExcludeFromRecentContent AS ExcludeFromRecentContent, ");

            sqlCommand.Append("u1.[Name] AS CreatedByName, ");
            sqlCommand.Append("u1.FirstName AS CreatedByFirstName, ");
            sqlCommand.Append("u1.LastName AS CreatedByLastName, ");
            sqlCommand.Append("u1.Email AS CreatedByEmail, ");
            sqlCommand.Append("u1.AuthorBio, ");
            sqlCommand.Append("u1.AvatarUrl, ");
            sqlCommand.Append("COALESCE(u1.UserID, -1) As AuthorUserID, ");
            sqlCommand.Append("u2.[Name] AS LastModByName, ");
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


            sqlCommand.Append("WHERE ");
            sqlCommand.Append("h.ModuleID = @ModuleID ");
            sqlCommand.Append("AND h.BeginDate <= @BeginDate ");
            sqlCommand.Append("AND h.EndDate >= @BeginDate ");
            sqlCommand.Append("ORDER BY h.BeginDate DESC ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@BeginDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginDate;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetHtmlContentByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  h.*, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles, ");
            sqlCommand.Append("m.IncludeInSearch, ");
            sqlCommand.Append("md.FeatureName, ");
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
            sqlCommand.Append("ON pm.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN	mp_Pages p ");
            sqlCommand.Append("ON p.PageID = pm.PageID ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_Users u1 ");
            sqlCommand.Append("ON h.UserGuid = u1.UserGuid ");

            sqlCommand.Append("WHERE ");

            sqlCommand.Append("p.SiteID = @SiteID ");
            sqlCommand.Append("AND pm.PageID = @PageID ");

            sqlCommand.Append(";");

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

    }
}
