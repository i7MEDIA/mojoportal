// Author:					
// Created:				    2005-04-10
// Last Modified:			2009-06-22
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Data;
using log4net;
using mojoPortal.Data;


namespace mojoPortal.Business
{
    /// <summary>
    /// Represents an event calendar
    /// </summary>
    public class CalendarEvent : IIndexableContent
    {
        private const string featureGuid = "c5e6a5df-ac2a-43d3-bb7f-9739bc47194e";

        public static Guid FeatureGuid
        {
            get { return new Guid(featureGuid); }
        }

        #region Constructors

        public CalendarEvent()
        { }


        public CalendarEvent(int itemId)
        {
            GetCalendarEvent(itemId);
        }

        #endregion

        #region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(CalendarEvent));

        private Guid itemGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private int itemID = -1;
        private int moduleID = -1;
        private string title = string.Empty;
        private string description = string.Empty;
        private string location = string.Empty;
        private string imageName = string.Empty;
        private DateTime eventDate = DateTime.Now;
        private DateTime startTime = DateTime.Now;
        private DateTime endTime = DateTime.Now.AddHours(1);
        private DateTime createdDate = DateTime.UtcNow;
        private int userID = -1;
        private Guid userGuid = Guid.Empty;

        private Guid lastModUserGuid = Guid.Empty;
        private DateTime lastModUtc = DateTime.UtcNow;
        private bool requiresTicket = false;
        private decimal ticketPrice = 0;

        private int siteId = -1;
        private string searchIndexPath = string.Empty;
		private bool showMap = true;

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

        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        public int ItemId
        {
            get { return itemID; }
            set { itemID = value; }
        }
        public int ModuleId
        {
            get { return moduleID; }
            set { moduleID = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        public string ImageName
        {
            get { return imageName; }
            set { imageName = value; }
        }
        public DateTime EventDate
        {
            get { return eventDate; }
            set { eventDate = value; }
        }
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }
        public int UserId
        {
            get { return userID; }
            set { userID = value; }
        }

        public Guid LastModUserGuid
        {
            get { return lastModUserGuid; }
            set { lastModUserGuid = value; }
        }

        public DateTime LastModUtc
        {
            get { return lastModUtc; }
            set { lastModUtc = value; }
        }

        public bool RequiresTicket
        {
            get { return requiresTicket; }
            set { requiresTicket = value; }
        }

        public decimal TicketPrice
        {
            get { return ticketPrice; }
            set { ticketPrice = value; }
        }

        /// <summary>
        /// This is not persisted to the db. It is only set and used when indexing forum threads in the search index.
        /// Its a convenience because when we queue the task to index on a new thread we can only pass one object.
        /// So we store extra properties here so we don't need any other objects.
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        /// <summary>
        /// This is not persisted to the db. It is only set and used when indexing forum threads in the search index.
        /// Its a convenience because when we queue the task to index on a new thread we can only pass one object.
        /// So we store extra properties here so we don't need any other objects.
        /// </summary>
        public string SearchIndexPath
        {
            get { return searchIndexPath; }
            set { searchIndexPath = value; }
        }


		public bool ShowMap
		{
			get { return showMap; }
			set { showMap = value; }
		}
        #endregion

        #region Private Methods

        private void GetCalendarEvent(int itemId)
        {
            using (IDataReader reader = DBEvents.GetCalendarEvent(itemId))
            {
                if (reader.Read())
                {
                    this.itemID = Convert.ToInt32(reader["ItemID"], CultureInfo.InvariantCulture);
                    this.moduleID = Convert.ToInt32(reader["ModuleID"], CultureInfo.InvariantCulture);
                    this.title = reader["Title"].ToString();
                    this.description = reader["Description"].ToString();
                    this.imageName = reader["ImageName"].ToString();
                    this.eventDate = Convert.ToDateTime(reader["EventDate"], CultureInfo.CurrentCulture);
                    this.startTime = Convert.ToDateTime(reader["StartTime"], CultureInfo.CurrentCulture);
                    this.endTime = Convert.ToDateTime(reader["EndTime"], CultureInfo.CurrentCulture);
                    this.createdDate = Convert.ToDateTime(reader["CreatedDate"], CultureInfo.CurrentCulture);
                    this.userID = Convert.ToInt32(reader["UserID"], CultureInfo.InvariantCulture);

                    this.itemGuid = new Guid(reader["ItemGuid"].ToString());
                    this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                    this.location = reader["Location"].ToString();
                    string u = reader["UserGuid"].ToString();
                    if (u.Length == 36) this.userGuid = new Guid(u);

                    u = reader["LastModUserGuid"].ToString();
                    if (u.Length == 36) this.lastModUserGuid = new Guid(u);

                    if (reader["LastModUtc"] != DBNull.Value)
                    {
                        this.lastModUtc = Convert.ToDateTime(reader["LastModUtc"], CultureInfo.CurrentCulture);

                    }

                    this.requiresTicket = Convert.ToBoolean(reader["RequiresTicket"], CultureInfo.InvariantCulture);
                    this.ticketPrice = Convert.ToDecimal(reader["TicketPrice"], CultureInfo.InvariantCulture);
					this.showMap = Convert.ToBoolean(reader["ShowMap"], CultureInfo.InvariantCulture);
                }

            }

        }

        private bool Create()
        {
            int newID;

            this.itemGuid = Guid.NewGuid();
            this.createdDate = DateTime.UtcNow;

            newID = DBEvents.AddCalendarEvent(
                this.itemGuid,
                this.moduleGuid,
                this.moduleID,
                this.title,
                this.description,
                this.imageName,
                this.eventDate,
                this.startTime,
                this.endTime,
                this.userID,
                this.userGuid,
                this.location,
                this.requiresTicket,
                this.ticketPrice,
                this.createdDate,
				this.showMap);

            this.itemID = newID;

            bool result = (newID > -1);

            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

            return result;

        }

        private bool Update()
        {
            this.lastModUtc = DateTime.UtcNow;

            bool result = DBEvents.UpdateCalendarEvent(
                this.itemID,
                this.moduleID,
                this.title,
                this.description,
                this.imageName,
                this.eventDate,
                this.startTime,
                this.endTime,
                this.location,
                this.requiresTicket,
                this.ticketPrice,
                this.lastModUtc,
                this.lastModUserGuid,
				this.showMap);

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
            if (this.itemID > 0)
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
            //IndexHelper.RemoveIndexItem(moduleID, itemID);
            bool result = DBEvents.DeleteCalendarEvent(itemID);

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

        //public static bool DeleteEvent(int moduleID, int itemID) 
        //{
        //    IndexHelper.RemoveIndexItem(moduleID, itemID);
        //    return dbEventsDeleteCalendarEvent(itemID);
        //}

        public static bool DeleteByModule(int moduleId)
        {
            return DBEvents.DeleteByModule(moduleId);
        }

        public static bool DeleteBySite(int siteId)
        {
            return DBEvents.DeleteBySite(siteId);
        }


        public static DataSet GetEvents(int moduleId, DateTime beginDate, DateTime endDate)
        {
            return DBEvents.GetEvents(moduleId, beginDate, endDate);
        }

        public static DataTable GetEventsTable(int moduleId, DateTime beginDate, DateTime endDate)
        {
            return DBEvents.GetEventsTable(moduleId, beginDate, endDate);
        }


        public static DataTable GetEventsByPage(int siteId, int pageId)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ItemID", typeof(int));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("ViewRoles", typeof(string));

            dataTable.Columns.Add("CreatedDate", typeof(DateTime));
            dataTable.Columns.Add("LastModUtc", typeof(DateTime));
			dataTable.Columns.Add("ShowMap", typeof(bool));
            using (IDataReader reader = DBEvents.GetEventsByPage(siteId, pageId))
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
                    row["LastModUtc"] = Convert.ToDateTime(reader["LastModUtc"]);
					row["ShowMap"] = Convert.ToBoolean(reader["ShowMap"]);
                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;

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
