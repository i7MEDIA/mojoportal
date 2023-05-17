using log4net;
using SuperFlexiData;
using System;
using System.Collections.Generic;
using System.Data;

namespace SuperFlexiBusiness
{

	public class ItemFieldValue
    {

        #region Constructors

        public ItemFieldValue()
        { }


        public ItemFieldValue(Guid valueGuid)
        {
            GetValue(valueGuid);
        }

		#endregion
		#region Private Properties
		//used to output total number of rows which match a query when using paging
		private static int _totalRows;

        private static readonly ILog log = LogManager.GetLogger(typeof(ItemFieldValue));

        #endregion

        #region Public Properties

        public Guid ValueGuid { get; set; } = Guid.Empty;
		public Guid SiteGuid { get; set; } = Guid.Empty;
		public Guid FeatureGuid { get; set; } = Guid.Empty;
		public Guid ModuleGuid { get; set; } = Guid.Empty;
		public Guid ItemGuid { get; set; } = Guid.Empty;

		public Guid FieldGuid { get; set; } = Guid.Empty;
		public string FieldValue { get; set; } = string.Empty;
		public string FieldName { get; internal set; } = string.Empty;
		#endregion

		#region Private Methods

		/// <summary>
		/// Gets an instance of value.
		/// </summary>
		/// <param name="valueGuid"> valueGuid </param>
		private void GetValue(Guid valueGuid)
        {
            using (IDataReader reader = DBItemFieldValues.GetOne(valueGuid))
            {
                PopulateFromReader(reader);
            }

        }

        private void PopulateFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                this.ValueGuid = new Guid(reader["ValueGuid"].ToString());
                this.SiteGuid = new Guid(reader["SiteGuid"].ToString());
                this.FeatureGuid = new Guid(reader["FeatureGuid"].ToString());
                this.ModuleGuid = new Guid(reader["ModuleGuid"].ToString());
                this.ItemGuid = new Guid(reader["ItemGuid"].ToString());
                this.FieldGuid = new Guid(reader["FieldGuid"].ToString());
                this.FieldValue = reader["FieldValue"].ToString();
				this.FieldName = reader["FieldName"].ToString();

            }

        }

        /// <summary>
        /// Persists a new instance of value. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.ValueGuid = Guid.NewGuid();

            int rowsAffected = DBItemFieldValues.Create(
                this.ValueGuid,
                this.SiteGuid,
                this.FeatureGuid,
                this.ModuleGuid,
                this.ItemGuid,
                this.FieldGuid,
                this.FieldValue);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of value. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBItemFieldValues.Update(
                this.ValueGuid,
                this.SiteGuid,
                this.FeatureGuid,
                this.ModuleGuid,
                this.ItemGuid,
                this.FieldGuid,
                this.FieldValue);

        }





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of value. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.ValueGuid != Guid.Empty)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }

		#endregion

		#region Static Methods

		/// <summary>
		/// Gets all values for item with ItemGuid. Returns List<ItemFieldValue>
		/// </summary>
		/// <param name="itemGuid"></param>
		/// <returns></returns>
		public static List<ItemFieldValue> GetItemValues(Guid itemGuid)
        {
            return LoadListFromReader(DBItemFieldValues.GetByItemGuid(itemGuid));
        }

		/// <summary>
		/// Gets all values for items with ModuleGuid. Returns List<ItemFieldValue>
		/// </summary>
		/// <param name="moduleGuid"></param>
		/// <returns></returns>
		public static List<ItemFieldValue> GetItemValuesByModule(Guid moduleGuid)
		{
			return LoadListFromReader(DBItemFieldValues.GetByModuleGuid(moduleGuid));
		}

		/// <summary>
		/// Gets all values for items with ModuleGuid. Returns List<ItemFieldValue>
		/// </summary>
		/// <param name="moduleGuid"></param>
		/// <returns></returns>
		public static List<ItemFieldValue> GetItemValuesByDefinition(Guid definitionGuid)
		{
			return LoadListFromReader(DBItemFieldValues.GetByDefinitionGuid(definitionGuid));
		}

		/// <summary>
		/// Deletes an instance of value. Returns true on success.
		/// </summary>
		/// <param name="valueGuid"> valueGuid </param>
		/// <returns>bool</returns>
		public static bool Delete(
            Guid valueGuid)
        {
            return DBItemFieldValues.Delete(
                valueGuid);
        }

        /// <summary>
        /// Deletes Items by Site. Returns true on success.
        /// </summary>
        public static bool DeleteBySite(Guid siteGuid)
        {
            return DBItemFieldValues.DeleteBySite(siteGuid);
        }

        /// <summary>
        /// Deletes Items by Module. Returns true on success.
        /// </summary>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            return DBItemFieldValues.DeleteByModule(moduleGuid);
        }

        /// <summary>
        /// Deletes Item Values by FieldGuild. Returns true on success.
        /// </summary>
        /// <param name="fieldGuid"></param>
        /// <returns></returns>
        public static bool DeleteByField(Guid fieldGuid)
        {
            return DBItemFieldValues.DeleteByField(fieldGuid);
        }

        /// <summary>
        /// Deletes Item Values by ItemGuild. Returns true on success.
        /// </summary>
        /// <param name="itemGuid"></param>
        /// <returns></returns>
        public static bool DeleteByItem(Guid itemGuid)
        {
            return DBItemFieldValues.DeleteByItem(itemGuid);
        }

        /// <summary>
        /// Gets a count of value. 
        /// </summary>
        public static int GetCount()
        {
            return DBItemFieldValues.GetCount();
        }

		private static List<ItemFieldValue> LoadListFromReader(IDataReader reader, bool getTotalRows = false)
        {
            List<ItemFieldValue> valueList = new List<ItemFieldValue>();
            try
            {
                while (reader.Read())
                {
                    ItemFieldValue value = new ItemFieldValue
                    {
                        ValueGuid = new Guid(reader["ValueGuid"].ToString()),
                        SiteGuid = new Guid(reader["SiteGuid"].ToString()),
                        FeatureGuid = new Guid(reader["FeatureGuid"].ToString()),
                        ModuleGuid = new Guid(reader["ModuleGuid"].ToString()),
                        ItemGuid = new Guid(reader["ItemGuid"].ToString()),
                        FieldGuid = new Guid(reader["FieldGuid"].ToString()),
                        FieldValue = reader["FieldValue"].ToString()
                    };


                    try
					{
                        if (reader["FieldName"] != DBNull.Value)
                        {
                            value.FieldName = reader["FieldName"].ToString();
                        }
                    }
                    catch(IndexOutOfRangeException ex)
					{
                        log.Debug($"FieldName isn't used here. Might want to fix that.\n{ex}");
					}
                    
					
                    // Not all methods will use TotalRows but there is no sense in having an extra method to load the reader
					// so, we'll catch the error and do nothing with it because we are expecting it
					// the if statement should keep any problems at bay but we still use try/catch in case someone inadvertently 
					// set getTotalRows = true
					if (getTotalRows)
					{
						try
						{
							if (reader["TotalRows"] != DBNull.Value)
							{
								_totalRows = Convert.ToInt32(reader["TotalRows"]);
							}
						}
						catch (IndexOutOfRangeException ex)
						{
                            log.Debug($"TotalRows isn't used by here. This is probably expected.\n{ex}");
                        }
                    }

					valueList.Add(value);
                }
            }
            finally
            {
                reader.Close();
            }

            return valueList;

        }

		/// <summary>
		/// Gets a List of ItemFieldValue using an array of ItemID
		/// </summary>
		/// <param name="itemIds"></param>
		/// <returns></returns>
		public static List<ItemFieldValue> GetByItemGuids(List<Guid> itemGuids)
		{
			IDataReader reader = DBItemFieldValues.GetByItemGuids(itemGuids);
			return LoadListFromReader(reader);
		}

		/// <summary>
		/// Gets an IList with all instances of value.
		/// </summary>
		public static List<ItemFieldValue> GetAll()
        {
            IDataReader reader = DBItemFieldValues.GetAll();
            return LoadListFromReader(reader);

        }


		public static List<ItemFieldValue> GetPageOfValues(
			Guid moduleGuid,
			Guid definitionGuid,
			string field,
			int pageNumber,
			int pageSize,
			out int totalPages,
			out int totalRows,
			string searchTerm = "",
			//string searchField = "",
			bool descending = false)
		{
			totalPages = 1;

			IDataReader reader = DBItemFieldValues.GetPageOfValuesForField(moduleGuid, definitionGuid, field, pageNumber, pageSize, searchTerm, descending);

			var values = LoadListFromReader(reader, true);

			totalRows = _totalRows;

			if (pageSize > 0)
			{
				totalPages = totalRows / pageSize;
			}
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

			return values;
		}

		/// <summary>
		/// Gets an IList with page of instances of value.
		/// </summary>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="totalPages">total pages</param>
		public static List<ItemFieldValue> GetPage(int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBItemFieldValues.GetPage(pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }

        public static List<ItemFieldValue> GetByFieldGuid(Guid fieldGuid)
        {
            IDataReader reader = DBItemFieldValues.GetByFieldGuid(fieldGuid);
            return LoadListFromReader(reader);
        }

        public static List<ItemFieldValue> GetByFieldGuidForModule(Guid fieldGuid, Guid moduleGuid)
        {
            IDataReader reader = DBItemFieldValues.GetByGuidForModule(fieldGuid, moduleGuid);
            return LoadListFromReader(reader);
        }

        public static List<ItemFieldValue> GetByFieldGuidForModule(Guid fieldGuid, int moduleId)
        {
            IDataReader reader = DBItemFieldValues.GetByGuidForModule(fieldGuid, moduleId);
            return LoadListFromReader(reader);
        }

		public static List<ItemFieldValue> GetByFieldGuidForDefinition(Guid fieldGuid)
		{
			IDataReader reader = DBItemFieldValues.GetByFieldGuid(fieldGuid);
			return LoadListFromReader(reader);
		}

		#endregion

		#region Comparison Methods

		/// <summary>
		/// Compares 2 instances of value.
		/// </summary>
		public static int CompareByFieldValue(ItemFieldValue value1, ItemFieldValue value2)
        {
            return value1.FieldValue.CompareTo(value2.FieldValue);
        }

        #endregion




    }

}





