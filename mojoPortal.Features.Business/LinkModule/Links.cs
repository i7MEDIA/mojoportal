// Author:					
// Created:				    2004-12-24
// Last Modified:			2010-01-07
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
using System.Globalization;
using log4net;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// Represents an instance of a links module
    /// </summary>
    public class Link : IIndexableContent
    {
        private const string featureGuid = "74bdbcc2-0e79-47ff-bcd4-a159270bf36e";

        public static Guid FeatureGuid
        {
            get { return new Guid(featureGuid); }
        }

        #region Constructors

        public Link()
        { }


        public Link(int itemId)
        {
            if (itemId > -1)
            {
                GetLink(itemId);
            }

        }

        #endregion

        #region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(Link));

        private Guid itemGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private Int32 itemID = -1;
        private Int32 moduleID = -1;
        private Int32 createdByUser = -1;
        private DateTime createdDate = DateTime.Now;
        private String title = string.Empty;
        private String url = string.Empty;
        private String target = String.Empty;
        private Int32 viewOrder = 500;
        private String description = string.Empty;
        private Guid userGuid = Guid.Empty;


        #endregion

        #region Public Properties

        public Guid ItemGuid
        {
            get { return itemGuid; }
        }

        public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }

        public Int32 ItemId
        {
            get { return itemID; }
            set { itemID = value; }
        }

        public Int32 ModuleId
        {
            get { return moduleID; }
            set { moduleID = value; }
        }

        public Int32 CreatedByUser
        {
            get { return createdByUser; }
            set { createdByUser = value; }
        }

        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public String Title
        {
            get { return title; }
            set { title = value; }
        }

        public String Url
        {
            get { return url; }
            set { url = value; }
        }

        public String Target
        {
            get { return target; }
            set { target = value.Trim(); }
        }

        public Int32 ViewOrder
        {
            get { return viewOrder; }
            set { viewOrder = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }


        #endregion

        #region Private Methods

        private void GetLink(Int32 itemId)
        {
            using (IDataReader reader = DBLinks.GetLink(itemId))
            {
                if (reader.Read())
                {
                    this.itemID = Convert.ToInt32(reader["ItemID"], CultureInfo.InvariantCulture);
                    this.moduleID = Convert.ToInt32(reader["ModuleID"], CultureInfo.InvariantCulture);
                    this.createdByUser = Convert.ToInt32(reader["CreatedBy"], CultureInfo.InvariantCulture);
                    this.createdDate = Convert.ToDateTime(reader["CreatedDate"], CultureInfo.CurrentCulture);
                    this.title = reader["Title"].ToString();
                    this.url = reader["Url"].ToString();
                    this.viewOrder = Convert.ToInt32(reader["ViewOrder"]);
                    this.description = reader["Description"].ToString();
                    this.target = reader["Target"].ToString();

                    string test = reader["ItemGuid"].ToString();

                    this.itemGuid = new Guid(reader["ItemGuid"].ToString());
                    this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                    string user = reader["UserGuid"].ToString();
                    if (user.Length == 36) this.userGuid = new Guid(user);

                }

            }

        }

        private bool Create()
        {
            int newID = -1;
            this.itemGuid = Guid.NewGuid();

            newID = DBLinks.AddLink(
                this.itemGuid,
                this.moduleGuid,
                this.moduleID,
                this.title,
                this.url,
                this.viewOrder,
                this.description,
                this.createdDate,
                this.createdByUser,
                this.target,
                this.userGuid);

            this.itemID = newID;

            bool result = (newID > 0);

            //IndexHelper.IndexItem(this);
            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

            return result;

        }

        private bool Update()
        {

            bool result = DBLinks.UpdateLink(
                this.itemID,
                this.moduleID,
                this.title,
                this.url,
                this.viewOrder,
                this.description,
                this.createdDate,
                this.target,
                this.createdByUser);

            //IndexHelper.IndexItem(this);
            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

            return result;

        }


        #endregion

        #region Public Methods



        public bool Save()
        {
            if (this.itemID > -1)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }

        public bool Delete()
        {
            bool result = DBLinks.DeleteLink(itemID);

            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                e.IsDeleted = true;
                OnContentChanged(e);
            }


            return result;
        }

        #endregion

        #region Static Methods

        public static IDataReader GetLinks(Int32 moduleId)
        {
            return DBLinks.GetLinks(moduleId);
        }

        public static IDataReader GetPage(
            int moduleId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBLinks.GetPage(moduleId, pageNumber, pageSize, out totalPages);
        }

        


        public static DataTable GetLinksByPage(int siteId, int pageId)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ItemID", typeof(int));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("ViewRoles", typeof(string));
            dataTable.Columns.Add("CreatedDate", typeof(DateTime));

            using (IDataReader reader = DBLinks.GetLinksByPage(siteId, pageId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();

                    row["ItemID"] = reader["ItemID"];
                    row["ModuleID"] = reader["ModuleID"];
                    row["ModuleTitle"] = reader["ModuleTitle"];
                    row["Title"] = reader["Title"];
                    row["Description"] = reader["Description"];
                    row["ViewRoles"] = reader["ViewRoles"];
                    row["CreatedDate"] = Convert.ToDateTime(reader["CreatedDate"]);

                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }


        public static bool DeleteByModule(int moduleId)
        {
            return DBLinks.DeleteByModule(moduleId);
        }

        public static bool DeleteBySite(int siteId)
        {
            return DBLinks.DeleteBySite(siteId);
        }



        #endregion

        #region IIndexableContent

        public event ContentChangedEventHandler ContentChanged;

        protected void OnContentChanged(ContentChangedEventArgs e)
        {
            if (ContentChanged != null)
            {
                ContentChanged(this, e);
            }
        }




        #endregion


    }

}
