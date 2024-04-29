// Author:					i7MEDIA (Joe Davis)
// Created:				    2015-12-22
// Last Modified:			2015-02-01
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.Helpers;
using log4net;
using mojoPortal.Business;
using mojoPortal.Data;
using Newtonsoft.Json;
namespace SuperFlexiBusiness
{
    /// <summary>
    /// Represents an instance of a SuperFlexi module
    /// </summary>
    public class SuperFlexiItem : IIndexableContent
    {
        private const string featureGuid = "DE2DEA8A-0905-4F60-9073-D50DEECB6BFF";


        /// <summary>
        /// FeatureGuid for SuperFlexi {DE2DEA8A-0905-4F60-9073-D50DEECB6BFF}
        /// </summary>
        public static Guid FeatureGuid
        {
            get { return new Guid(featureGuid); }
        }

        #region Constructors

        public SuperFlexiItem()
        { }


        public SuperFlexiItem(int itemId)
        {
            if (itemId > -1)
            {
                GetItem(itemId);
            }

        }

        #endregion

        #region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(SuperFlexiItem));

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
        private OtherSettings otherSettings = new OtherSettings();
        private String otherSettingsJson = string.Empty;
        private String content = string.Empty;
        private String cssClass = string.Empty;
        private String mobileCssClass = string.Empty;
        private String featuredImage = string.Empty;
        private Guid userGuid = Guid.Empty;
        private List<ContentItem> contentItems = new List<ContentItem>();

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

        public OtherSettings OtherSettings
        {
            get { return otherSettings; }
            set { otherSettings = value; }
        }

        public String Content
        {
            get { return content; }
            set { content = value; }
        }

        public String CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        public String MobileCssClass
        {
            get { return mobileCssClass; }
            set { mobileCssClass = value; }
        }

        public string FeaturedImage
        {
            get { return featuredImage; }
            set { featuredImage = value; }
        }

        public List<ContentItem> ContentItems { get { return contentItems; } set { contentItems = value; } }

        #endregion

        #region Private Methods

        private void GetItem(Int32 itemId)
        {
            string content = string.Empty;

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

                    content = reader["Description"].ToString();
                    this.target = reader["Target"].ToString();

                    string test = reader["ItemGuid"].ToString();

                    this.itemGuid = new Guid(reader["ItemGuid"].ToString());
                    this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                    string user = reader["UserGuid"].ToString();
                    if (user.Length == 36) this.userGuid = new Guid(user);

                }
            }
            OtherSettings others = ParseOtherSettings(content);
            if (others != null)
            {
                this.content = others.Content;
                this.cssClass = others.CssClass;
                this.mobileCssClass = others.MobileCssClass;
                this.featuredImage = others.FeaturedImage;
            }
            else
            {
                try
                {
                    this.contentItems = JsonConvert.DeserializeObject<List<ContentItem>>(content);
                }
                catch (JsonSerializationException ex)
                {
                    this.content = content;
                }
            }

        }

        private bool Create()
        {
            string content = string.Empty;
            int newID = -1;
            this.itemGuid = Guid.NewGuid();

            if (this.ContentItems.Count < 1)
            {
                otherSettings.Content = this.content;
                otherSettings.CssClass = this.cssClass;
                otherSettings.MobileCssClass = this.mobileCssClass;
                otherSettings.FeaturedImage = this.featuredImage;
                try
                {
                    content = Json.Encode(otherSettings);
                }
                catch (System.ArgumentException ex)
                {
                    content = this.content;
                }
            }
            else
            {
                try
                {
                    content = JsonConvert.SerializeObject(this.ContentItems);
                }
                catch (JsonSerializationException ex)
                {
                    content = this.content;
                }
            }

            newID = DBLinks.AddLink(
                this.itemGuid,
                this.moduleGuid,
                this.moduleID,
                this.title,
                this.url,
                this.viewOrder,
                content,
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
            string content = string.Empty;

            if (this.ContentItems.Count < 1)
            {
                //use legacy items
                otherSettings.Content = this.content;
                otherSettings.CssClass = this.cssClass;
                otherSettings.MobileCssClass = this.mobileCssClass;
                otherSettings.FeaturedImage = this.featuredImage;
                try
                {
                    content = Json.Encode(otherSettings);
                }
                catch (System.ArgumentException ex)
                {
                    content = this.content;
                }
            }

            else
            {
                try
                {
                    content = JsonConvert.SerializeObject(this.ContentItems);
                }
                catch (JsonSerializationException ex)
                {
                    content = this.content;
                }
            }

            bool result = DBLinks.UpdateLink(
                    this.itemID,
                    this.moduleID,
                    this.title,
                    this.url,
                    this.viewOrder,
                    content,
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
        public static List<SuperFlexiItem> GetModuleItems(Int32 moduleId)
        {
            string content = string.Empty;
            List<SuperFlexiItem> items = new List<SuperFlexiItem>();
            using (IDataReader reader = DBLinks.GetLinks(moduleId))
            {
                while (reader.Read())
                {
                    SuperFlexiItem item = new SuperFlexiItem();

                    item.itemID = Convert.ToInt32(reader["ItemID"], CultureInfo.InvariantCulture);
                    item.moduleID = Convert.ToInt32(reader["ModuleID"], CultureInfo.InvariantCulture);
                    item.createdByUser = Convert.ToInt32(reader["CreatedBy"], CultureInfo.InvariantCulture);
                    item.createdDate = Convert.ToDateTime(reader["CreatedDate"], CultureInfo.CurrentCulture);
                    item.title = reader["Title"].ToString();
                    item.url = reader["Url"].ToString();
                    item.viewOrder = Convert.ToInt32(reader["ViewOrder"]);
                    content = reader["Description"].ToString();
                    OtherSettings others = ParseOtherSettings(content);
                    if (others != null)
                    {
                        item.content = others.Content;
                        item.cssClass = others.CssClass;
                        item.mobileCssClass = others.MobileCssClass;
                        item.featuredImage = others.FeaturedImage;
                    }
                    else
                    {
                        try
                        {
                            item.contentItems = JsonConvert.DeserializeObject<List<ContentItem>>(content);
                        }
                        catch (JsonSerializationException ex)
                        {
                            item.content = content;
                        }
                    }
                    item.target = reader["Target"].ToString();

                    //string test = reader["ItemGuid"].ToString();

                    item.itemGuid = new Guid(reader["ItemGuid"].ToString());
                    item.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                    string user = reader["UserGuid"].ToString();
                    if (user.Length == 36) item.userGuid = new Guid(user);
                    items.Add(item);
                }
            }
            return items;
        }

        //public static IDataReader GetWidgets(Int32 moduleId)
        //{
        //    return DBLinks.GetLinks(moduleId);
        //}

        public static IDataReader GetPage(
            int moduleId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBLinks.GetPage(moduleId, pageNumber, pageSize, out totalPages);
        }


        public static OtherSettings ParseOtherSettings(string otherSettingsString)
        {
            OtherSettings others = new OtherSettings();
            if (otherSettingsString != string.Empty)
            {
                try
                {
                    dynamic otherSettingsObject = Json.Decode(otherSettingsString);
                    if (otherSettingsObject.Content == null) return null;
                    others.Content = otherSettingsObject.Content;
                    others.CssClass = otherSettingsObject.CssClass != null ? otherSettingsObject.CssClass : string.Empty;
                    others.MobileCssClass = otherSettingsObject.MobileCssClass != null ? otherSettingsObject.MobileCssClass : string.Empty;
                    others.FeaturedImage = otherSettingsObject.FeaturedImage != null ? otherSettingsObject.FeaturedImage : string.Empty;
                }
                catch (System.ArgumentException ex)
                {
                    others.Content = otherSettingsString;
                }
            }

            return others;
        }


        //public static DataTable GetWidgetsByPage(int siteId, int pageId)
        //{
        //    DataTable dataTable = new DataTable();

        //    dataTable.Columns.Add("ItemID", typeof(int));
        //    dataTable.Columns.Add("ModuleID", typeof(int));
        //    dataTable.Columns.Add("ModuleTitle", typeof(string));
        //    dataTable.Columns.Add("Title", typeof(string));
        //    dataTable.Columns.Add("Description", typeof(string));
        //    dataTable.Columns.Add("ViewRoles", typeof(string));
        //    dataTable.Columns.Add("CreatedDate", typeof(DateTime));

        //    using (IDataReader reader = DBLinks.GetLinksByPage(siteId, pageId))
        //    {
        //        while (reader.Read())
        //        {
        //            DataRow row = dataTable.NewRow();

        //            row["ItemID"] = reader["ItemID"];
        //            row["ModuleID"] = reader["ModuleID"];
        //            row["ModuleTitle"] = reader["ModuleTitle"];
        //            row["Title"] = reader["Title"];
        //            row["Description"] = reader["Description"];
        //            row["ViewRoles"] = reader["ViewRoles"];
        //            row["CreatedDate"] = Convert.ToDateTime(reader["CreatedDate"]);

        //            dataTable.Rows.Add(row);
        //        }
        //    }

        //    return dataTable;
        //}


        public static bool DeleteByModule(int moduleId)
        {
            return DBLinks.DeleteByModule(moduleId);
        }

        public static bool DeleteBySite(int siteId)
        {
            return DBLinks.DeleteBySite(siteId);
        }

        public static int GetHighestSortRank(int moduleId)
        {
            List<SuperFlexiItem> items = SuperFlexiItem.GetModuleItems(moduleId);
            SuperFlexiItem lastItem = items[items.Count - 1];
            return lastItem.ViewOrder;
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


    public class ContentItem
    {

        private string content = string.Empty;
        public string Content { get { return content; } set { content = value; } }

        //private int id = -1;
        //public int ID { get { return id; } set { id = value; } }

        private string itemDefName = string.Empty;
        public string ItemDefName { get { return itemDefName; } set { itemDefName = value; } }
    }

    public class OtherSettings
    {
        private string content = string.Empty;
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private string cssClass = string.Empty;
        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        private string mobileCssClass = string.Empty;
        public string MobileCssClass
        {
            get { return mobileCssClass; }
            set { mobileCssClass = value; }
        }

        private string featuredImage = string.Empty;
        public string FeaturedImage
        {
            get { return featuredImage; }
            set { featuredImage = value; }
        }

    }

}
