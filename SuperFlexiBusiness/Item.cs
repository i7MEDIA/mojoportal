using mojoPortal.Business;
using SuperFlexiData;
using System;
using System.Collections.Generic;
using System.Data;

namespace SuperFlexiBusiness
{

	public class Item : IIndexableContent, IComparable<Item>
    {

        #region Constructors

        public Item()
        { }

        public Item(int itemID)
        {
            GetItem(itemID);
        }

        public Item(Guid itemGuid)
        {
            GetItem(itemGuid);
        }

        #endregion

        #region Private Properties

        private Guid siteGuid = Guid.Empty;
        private Guid featureGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private int moduleID = -1;
        private string moduleFriendlyName = string.Empty;
        private int globalViewSortOrder = 0;
        private Guid definitionGuid = Guid.Empty;
        private Guid itemGuid = Guid.Empty;
        private int itemID = -1;
        private int sortOrder = -1;
        private string viewRoles = "All Users;";
        private string editRoles = "";
        private DateTime createdUtc = DateTime.UtcNow;
        private DateTime lastModUtc = DateTime.UtcNow;
        #endregion

        #region Public Properties

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }
        public Guid FeatureGuid
        {
            get { return featureGuid; }
            set { featureGuid = value; }
        }
        public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }
        public int ModuleID
        {
            get { return moduleID; }
            set { moduleID = value; }
        }

        public string ModuleFriendlyName
        {
            get { return moduleFriendlyName; }
            set { moduleFriendlyName = value; }
        }
        public int GlobalViewSortOrder
        {
            get { return globalViewSortOrder; }
            set { globalViewSortOrder = value; }
        }
        public Guid DefinitionGuid
        {
            get { return definitionGuid; }
            set { definitionGuid = value; }
        }
        public Guid ItemGuid
        {
            get { return itemGuid; }
            set { itemGuid = value; }
        }
        public int ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }
        public int SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }

        public string ViewRoles
        {
            get { return viewRoles; }
            set { viewRoles = value; }
        }

        public string EditRoles
        {
            get { return editRoles; }
            set { editRoles = value; }
        }

		public DateTime CreatedUtc
        {
            get { return createdUtc; }
            set { createdUtc = value; }
        }
        public DateTime LastModUtc
        {
            get { return lastModUtc; }
            set { lastModUtc = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of item.
        /// </summary>
        /// <param name="itemID"> itemID </param>
        private void GetItem(int itemID)
        {
            using (IDataReader reader = DBItems.GetOne(itemID))
            {
                PopulateFromReader(reader);
            }

        }

        /// <summary>
        /// Gets an instance of item.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        private void GetItem(Guid itemGuid)
        {
            using (IDataReader reader = DBItems.GetOne(itemGuid))
            {
                PopulateFromReader(reader);
            }

        }


        private void PopulateFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
				if (Guid.TryParse(reader["ItemGuid"].ToString(), out this.itemGuid))
				{
					this.siteGuid = new Guid(reader["SiteGuid"].ToString());
					this.featureGuid = new Guid(reader["FeatureGuid"].ToString());
					this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
					this.moduleID = Convert.ToInt32(reader["ModuleID"]);
					this.definitionGuid = new Guid(reader["DefinitionGuid"].ToString());
					//this.itemGuid = new Guid(reader["ItemGuid"].ToString());
					this.itemID = Convert.ToInt32(reader["ItemID"]);
					this.sortOrder = Convert.ToInt32(reader["SortOrder"]);
					this.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
					this.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
					viewRoles = reader["ViewRoles"].ToString();
					editRoles = reader["EditRoles"].ToString();
				}

            }

        }

        /// <summary>
        /// Persists a new instance of item. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID;

            newID = DBItems.Create(
                siteGuid,
                featureGuid,
                moduleGuid,
                moduleID,
                definitionGuid,
                itemGuid,
                sortOrder,
                createdUtc,
                lastModUtc,
				viewRoles,
				editRoles);

            this.itemID = newID;

            bool result = (newID > 0);

            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

            return result;

        }


        /// <summary>
        /// Updates this instance of item. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            bool result = DBItems.Update(
                siteGuid,
                featureGuid,
                moduleGuid,
                moduleID,
                definitionGuid,
                itemGuid,
                sortOrder,
                createdUtc,
                lastModUtc,
				viewRoles,
				editRoles);

            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

            return result;

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of item. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
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
            if (this.itemID > 0)
            {
                bool result = Delete(this.itemID);

                if (result)
                {
                    ContentChangedEventArgs e = new ContentChangedEventArgs();
                    e.IsDeleted = true;
                    OnContentChanged(e);
                }

                return result;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Deletes an instance of item. Returns true on success.
        /// </summary>
        /// <param name="itemID"> itemID </param>
        /// <returns>bool</returns>
        public static bool Delete(int itemID)
        {
            return DBItems.Delete(itemID);
        }


        /// <summary>
        /// Deletes Items by Site. Returns true on success.
        /// </summary>
        public static bool DeleteBySite(Guid siteGuid)
        {
            return DBItems.DeleteBySite(siteGuid);
        }

        /// <summary>
        /// Deletes Items by Module. Returns true on success.
        /// </summary>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            return DBItems.DeleteByModule(moduleGuid);
        }

		/// <summary>
		/// Gets a count of all items for a module
		/// </summary>
		public static int GetCountForModule(int moduleId)
		{
			return DBItems.GetCountForModule(moduleId);
		}

        /// <summary>
        /// Gets an IList with all items for a module instance
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>IList</returns>
        public static List<Item> GetForModule(int moduleId, bool descending = false)
        {
            return GetForModule(moduleId, totalPages: out _, totalRows: out _, pageSize: -1, descending: descending);
        }

		/// <summary>
		/// Gets an IList with all items for a module instance
		/// </summary>
		/// <param name="moduleId"></param>
		/// <returns>IList</returns>
		public static List<Item> GetForModule(int moduleId, out int totalPages, out int totalRows, int pageNumber = 1, int pageSize = 20, bool descending = false)
		{
			return LoadListFromReader(
                DBItems.GetForModule(moduleId, pageNumber, pageSize, descending ? "DESC" : "ASC"),
                pageSize,
                out totalPages,
                out totalRows);
		}

        public static List<Item> GetForDefinition(Guid defGuid, Guid siteGuid, bool descending = false)
        {
            return GetForDefinition(defGuid, siteGuid, out _, out _, descending: descending);
        }

		/// <summary>
		/// Gets an IList with all items for a single definition
		/// </summary>
		/// <param name="fieldDefinitionGuid"></param>
		/// <param name="descending"></param>
		/// <returns></returns>
		public static List<Item> GetForDefinition(Guid defGuid, Guid siteGuid, out int totalPages, out int totalRows, int pageNumber = 1, int pageSize = 20, bool descending = false)
	    {
		    return LoadListFromReader(
			    DBItems.GetForDefinition(defGuid, siteGuid, pageNumber, pageSize, descending ? "DESC" : "ASC"),
                pageSize, 
                out totalPages, 
                out totalRows);
	    }

		/// <summary>
		/// Gets a DataTable with items on a specific page
		/// </summary>
		/// <param name="siteGuid"></param>
		/// <param name="pageId"></param>
		/// <returns>DataTable</returns>
		public static DataTable GetByCMSPage(Guid siteGuid, int pageId)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ItemGuid", typeof(Guid));
            dataTable.Columns.Add("ItemID", typeof(int));
            dataTable.Columns.Add("SortOrder", typeof(int));
            dataTable.Columns.Add("CreatedUtc", typeof(DateTime));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("ModuleViewRoles", typeof(string));
            dataTable.Columns.Add("ItemViewRoles", typeof(string));
			dataTable.Columns.Add("PublishBeginDate", typeof(DateTime));
            dataTable.Columns.Add("PublishEndDate", typeof(DateTime));
            using (IDataReader reader = DBItems.GetByCMSPage(siteGuid, pageId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();

                    row["ModuleID"] = reader["moduleId"];
                    row["ItemGuid"] = Guid.Parse(reader["itemGuid"].ToString());
                    row["ItemID"] = reader["itemId"];
                    row["SortOrder"] = reader["sortOrder"];
                    row["CreatedUtc"] = Convert.ToDateTime(reader["createdUtc"]);
                    row["ModuleTitle"] = reader["moduleTitle"];
                    row["ModuleViewRoles"] = reader["moduleViewRoles"];
                    row["ItemViewRoles"] = reader["itemViewRoles"];

					if (reader["publishBeginDate"] != DBNull.Value)
                    {
                        row["PublishBeginDate"]
                            = Convert.ToDateTime(reader["publishBeginDate"]);
                    }

                    if (reader["publishEndDate"] != DBNull.Value)
                    {
                        row["PublishEndDate"]
                            = Convert.ToDateTime(reader["publishEndDate"]);
                    }

                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }

        /// <summary>
        /// Gets highest (largest) SortOrder for items for a module instance
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>int</returns>
        public static int GetHighestSortOrder(int moduleId)
        {
			return DBItems.GetHighestSortOrder(moduleId);
        }



		#endregion
		
		private static List<Item> LoadListFromReader(IDataReader reader, int pageSize, out int totalPages, out int totalRows)
		{
			List<Item> itemList = new List<Item>();

			totalRows = 0;
			totalPages = 1;

			try
			{
				while (reader.Read())
				{
					Item item = new Item
					{
						siteGuid = new Guid(reader["SiteGuid"].ToString()),
						featureGuid = new Guid(reader["FeatureGuid"].ToString()),
						moduleGuid = new Guid(reader["ModuleGuid"].ToString()),
						moduleID = Convert.ToInt32(reader["ModuleID"]),
						definitionGuid = new Guid(reader["DefinitionGuid"].ToString()),
						itemGuid = new Guid(reader["ItemGuid"].ToString()),
						itemID = Convert.ToInt32(reader["ItemID"]),
						sortOrder = Convert.ToInt32(reader["SortOrder"]),
						createdUtc = Convert.ToDateTime(reader["CreatedUtc"]),
						lastModUtc = Convert.ToDateTime(reader["LastModUtc"]),
						viewRoles = reader["ViewRoles"].ToString(),
						editRoles = reader["EditRoles"].ToString()
					};
					// Not all methods will use TotalRows but there is no sense in having an extra method to load the reader
					// so, we'll catch the error and do nothing with it because we are expecting it
					// the if statement should keep any problems at bay but we still use try/catch in case someone inadvertently 
					// set getTotalRows = true
					if (pageSize > -1)
					{
						//try
						//{
							if (reader["TotalRows"] != DBNull.Value)
							{
								//totalRows = Convert.ToInt32(reader["TotalRows"]);
						    	int.TryParse(reader["TotalRows"].ToString(), out totalRows);
						    }
						//}
						//catch (IndexOutOfRangeException) { }
					}
					itemList.Add(item);
				}
			}
			finally
			{
				reader.Close();
			}

			if (totalRows == 0) { totalRows = itemList.Count; }

			if (totalRows <= pageSize)
			{
				totalPages = 1;
			}
			else
			{
				Math.DivRem(totalRows, pageSize, out int remainder);
				if (remainder > 0)
				{
					totalPages += 1;
				}
			}

			return itemList;
		}

		#region Comparison Methods

		/// <summary>
		/// Compares 2 instances of item.
		/// </summary>
		public static int CompareByModuleID(Item item1, Item item2)
        {
            return item1.ModuleID.CompareTo(item2.ModuleID);
        }
        /// <summary>
        /// Compares 2 instances of item.
        /// </summary>
        public static int CompareByItemID(Item item1, Item item2)
        {
            return item1.ItemID.CompareTo(item2.ItemID);
        }
        /// <summary>
        /// Compares 2 instances of item.
        /// </summary>
        public static int CompareBySortOrder(Item item1, Item item2)
        {
            return item1.SortOrder.CompareTo(item2.SortOrder);
        }
        /// <summary>
        /// Compares 2 instances of item.
        /// </summary>
        public static int CompareByCreatedUtc(Item item1, Item item2)
        {
            return item1.CreatedUtc.CompareTo(item2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of item.
        /// </summary>
        public static int CompareByLastModUtc(Item item1, Item item2)
        {
            return item1.LastModUtc.CompareTo(item2.LastModUtc);
        }

        public int CompareTo(Item other)
        {
            return this.sortOrder.CompareTo(other.sortOrder);
        }
        #endregion

        #region IIndexableContent

        public event ContentChangedEventHandler ContentChanged;

        protected void OnContentChanged(ContentChangedEventArgs e)
        {
			ContentChanged?.Invoke(this, e);
		}
        #endregion
        
        
    }

	public class SimpleItemComparer : IEqualityComparer<Item>
	{
		//Items are equal if their ItemIDs and ItemGuids are equal.
		public bool Equals(Item x, Item y)
		{
			//check whether the compared objects reference the same data.
			if (Object.ReferenceEquals(x, y)) return true;

			//check for nulls
			if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
			{
				return false;
			}

			//check if properties are equal
			return x.ItemGuid == y.ItemGuid && x.ItemID == y.ItemID;
		}

		// If Equals() returns true for a pair of objects 
		// then GetHashCode() must return the same value for these objects.
		public int GetHashCode(Item item)
		{
			//Check whether the object is null
			if (Object.ReferenceEquals(item, null)) return 0;

			int hashItemID = item.ItemID.GetHashCode();
			int hashItemGuid = item.ItemGuid.GetHashCode();

			return hashItemID ^ hashItemGuid;
		}
	}

	public class Item_SortForGlobalView : IComparer<Item>
    {
        public int Compare(Item x, Item y)
        {
            if (x.GlobalViewSortOrder > y.GlobalViewSortOrder) return 1;
            else if (x.GlobalViewSortOrder < y.GlobalViewSortOrder) return -1;
            else return 0;
        }
    }
    public class Item_SortDescendingOrder : IComparer<Item>
    {
        public int Compare(Item x, Item y)
        {
            if (x.SortOrder < y.SortOrder) return 1;
            else if (x.SortOrder > y.SortOrder) return -1;
            else return 0;
        }

    }
}





