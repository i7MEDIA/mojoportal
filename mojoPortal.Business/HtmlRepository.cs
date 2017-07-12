// Author:					
// Created:				    2009-10-02
// Last Modified:		    2013-04-18
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using mojoPortal.Data;

namespace mojoPortal.Business
{

    public class HtmlRepository
    {
        public HtmlRepository()
		{}

        public HtmlContent Fetch(int moduleId)
        {
            if (moduleId < 0) { return null; }

            using (IDataReader reader = DBHtmlContent.GetHtmlContent(moduleId, DateTime.UtcNow))
            {
                if (reader.Read())
                {
                    HtmlContent content = new HtmlContent();
                    LoadFromReader(content, reader);
                    return content;
                }
            }

            return null;
        }

        public HtmlContent Fetch(int moduleId, int itemId)
        {
            if (moduleId < 0) { return null; }
            if (itemId < 0) { return null; }

            using (IDataReader reader = DBHtmlContent.GetHtmlContent(moduleId, itemId))
            {
                if (reader.Read())
                {
                    HtmlContent content = new HtmlContent();
                    LoadFromReader(content, reader);
                    return content;
                }
            }

            return null;
        }

        private void LoadFromReader(HtmlContent content, IDataReader reader)
        {
            if (content == null) { return; }
            if (reader == null) { return; }

            content.ItemId = Convert.ToInt32(reader["ItemID"]);
            content.ModuleId = Convert.ToInt32(reader["ModuleID"]);
            content.Title = reader["Title"].ToString();
            
            content.Body = reader["Body"].ToString();

            //legacy fields not used
            //content.Excerpt = reader["Excerpt"].ToString();
            //content.MoreLink = reader["MoreLink"].ToString();
            //content.SortOrder = Convert.ToInt32(reader["SortOrder"]);
            //content.BeginDate = Convert.ToDateTime(reader["BeginDate"]);
            //content.EndDate = Convert.ToDateTime(reader["EndDate"]);


            content.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);

            //if (reader["UserID"] != DBNull.Value)
            //content.CreatedBy = Convert.ToInt32(reader["UserID"]);

            content.ItemGuid = new Guid(reader["ItemGuid"].ToString());
            content.ModuleGuid = new Guid(reader["ModuleGuid"].ToString());
            string user = reader["UserGuid"].ToString();
            if (user.Length == 36) content.UserGuid = new Guid(user);

            user = reader["LastModUserGuid"].ToString();
            if (user.Length == 36) content.LastModUserGuid = new Guid(user);

            if (reader["LastModUtc"] != DBNull.Value)
                content.LastModUtc = Convert.ToDateTime(reader["LastModUtc"]);

            content.CreatedByName = reader["CreatedByName"].ToString();
            content.CreatedByFirstName = reader["CreatedByFirstName"].ToString();
            content.CreatedByLastName = reader["CreatedByLastName"].ToString();
            content.CreatedByEmail = reader["CreatedByEmail"].ToString();

            content.AuthorAvatar = reader["AvatarUrl"].ToString();
            content.AuthorBio = reader["AuthorBio"].ToString();
            content.AuthorUserId = Convert.ToInt32(reader["AuthorUserID"]);


            content.LastModByName = reader["LastModByName"].ToString();
            content.LastModByFirstName = reader["LastModByFirstName"].ToString();
            content.LastModByLastName = reader["LastModByLastName"].ToString();
            content.LastModByEmail = reader["LastModByEmail"].ToString();
            content.ExcludeFromRecentContent = Convert.ToBoolean(reader["ExcludeFromRecentContent"]);
        }

        public bool Save(HtmlContent content)
        {
            bool result = false;
            if (content == null) { return result; }

            content.LastModUtc = DateTime.UtcNow;

            if (content.ItemId > -1)
            {
                result = DBHtmlContent.UpdateHtmlContent(
                content.ItemId,
                content.ModuleId,
                content.Title,
                content.Excerpt,
                content.Body,
                content.MoreLink,
                content.SortOrder,
                content.BeginDate,
                content.EndDate,
                content.LastModUtc,
                content.LastModUserGuid,
                content.ExcludeFromRecentContent);

            }
            else
            {
                content.ItemGuid = Guid.NewGuid();

                int newId = DBHtmlContent.AddHtmlContent(
                    content.ItemGuid,
                    content.ModuleGuid,
                    content.ModuleId,
                    content.Title,
                    content.Excerpt,
                    content.Body,
                    content.MoreLink,
                    content.SortOrder,
                    content.BeginDate,
                    content.EndDate,
                    content.CreatedDate,
                    content.CreatedBy,
                    content.UserGuid,
                    content.ExcludeFromRecentContent);

                content.ItemId = newId;

                result = (newId > -1);

            }

            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                content.OnContentChanged(e);
            }

            return result;
        }


        public bool Delete(HtmlContent content)
        {
            if (content == null) { return false; }

            bool result = DBHtmlContent.DeleteHtmlContent(content.ItemId);

            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                e.IsDeleted = true;
                content.OnContentChanged(e);
            }

            return result;

        }


        public IDataReader GetHtml(int moduleId, DateTime beginDate)
        {
            return DBHtmlContent.GetHtmlContent(moduleId, beginDate);
        }

        public IDataReader GetHtmlForMetaWeblogApi(int siteId)
        {
            return DBHtmlContent.GetHtmlForMetaWeblogApi(siteId);
        }

        public bool DeleteByModule(int moduleId)
        {
            return DBHtmlContent.DeleteByModule(moduleId);
        }

        public bool DeleteBySite(int siteId)
        {
            return DBHtmlContent.DeleteBySite(siteId);
        }

        public DataTable GetHtmlContentByPage(int siteId, int pageId)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ItemID", typeof(int));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Body", typeof(string));
            dataTable.Columns.Add("ViewRoles", typeof(string));
            dataTable.Columns.Add("IncludeInSearch", typeof(bool));

            dataTable.Columns.Add("CreatedByName", typeof(string));
            dataTable.Columns.Add("CreatedByFirstName", typeof(string));
            dataTable.Columns.Add("CreatedByLastName", typeof(string));

            dataTable.Columns.Add("CreatedDate", typeof(DateTime));
            dataTable.Columns.Add("LastModUtc", typeof(DateTime));
            dataTable.Columns.Add("ExcludeFromRecentContent", typeof(bool));

            

            using (IDataReader reader = DBHtmlContent.GetHtmlContentByPage(siteId, pageId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();

                    row["ItemID"] = reader["ItemID"];
                    row["ModuleID"] = reader["ModuleID"];
                    row["ModuleTitle"] = reader["ModuleTitle"];
                    row["Title"] = reader["Title"];
                    row["Body"] = reader["Body"];
                    row["ViewRoles"] = reader["ViewRoles"];

                    row["CreatedByName"] = reader["CreatedByName"];
                    row["CreatedByFirstName"] = reader["CreatedByFirstName"];
                    row["CreatedByLastName"] = reader["CreatedByLastName"];

                    row["IncludeInSearch"] = Convert.ToBoolean(reader["IncludeInSearch"]);

                    row["CreatedDate"] = Convert.ToDateTime(reader["CreatedDate"]);
                    row["LastModUtc"] = Convert.ToDateTime(reader["LastModUtc"]);
                    row["ExcludeFromRecentContent"] = Convert.ToBoolean(reader["ExcludeFromRecentContent"]);

                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }


    }
}
