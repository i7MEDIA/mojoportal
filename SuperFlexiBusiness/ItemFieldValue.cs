// Author:					i7MEDIA
// Created:					2015-3-6
// Last Modified:			2017-11-06
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using SuperFlexiData;

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

        /// <summary>
        /// Item Field Value by ItemGuid and FieldGuid
        /// </summary>
        /// <param name="itemGuid"></param>
        /// <param name="fieldGuid"></param>
        public ItemFieldValue(Guid itemGuid, Guid fieldGuid)
        {
            GetValueByItemField(itemGuid, fieldGuid);
        }

        #endregion

        #region Private Properties

        private Guid valueGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid featureGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private Guid itemGuid = Guid.Empty;
        private Guid fieldGuid = Guid.Empty;
        private string fieldValue = string.Empty;
		//used to output total number of rows which match a query when using paging
		private static int _totalRows;
		#endregion

		#region Public Properties

		public Guid ValueGuid
        {
            get { return valueGuid; }
            set { valueGuid = value; }
        }
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
        public Guid ItemGuid
        {
            get { return itemGuid; }
            set { itemGuid = value; }
        }

		public Guid FieldGuid
        {
            get { return fieldGuid; }
            set { fieldGuid = value; }
        }
        public string FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }

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

        private void GetValueByItemField(Guid itemGuid, Guid fieldGuid)
        {
            using (IDataReader reader = DBItemFieldValues.GetByItemField(itemGuid, fieldGuid))
            {
                //need to do something here
            }
        }


        private void PopulateFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                this.valueGuid = new Guid(reader["ValueGuid"].ToString());
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                this.featureGuid = new Guid(reader["FeatureGuid"].ToString());
                this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                this.itemGuid = new Guid(reader["ItemGuid"].ToString());
                this.fieldGuid = new Guid(reader["FieldGuid"].ToString());
                this.fieldValue = reader["FieldValue"].ToString();

            }

        }

        /// <summary>
        /// Persists a new instance of value. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.valueGuid = Guid.NewGuid();

            int rowsAffected = DBItemFieldValues.Create(
                this.valueGuid,
                this.siteGuid,
                this.featureGuid,
                this.moduleGuid,
                this.itemGuid,
                this.fieldGuid,
                this.fieldValue);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of value. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBItemFieldValues.Update(
                this.valueGuid,
                this.siteGuid,
                this.featureGuid,
                this.moduleGuid,
                this.itemGuid,
                this.fieldGuid,
                this.fieldValue);

        }





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of value. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.valueGuid != Guid.Empty)
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
                    ItemFieldValue value = new ItemFieldValue();
                    value.valueGuid = new Guid(reader["ValueGuid"].ToString());
                    value.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    value.featureGuid = new Guid(reader["FeatureGuid"].ToString());
                    value.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                    value.itemGuid = new Guid(reader["ItemGuid"].ToString());
                    value.fieldGuid = new Guid(reader["FieldGuid"].ToString());
                    value.fieldValue = reader["FieldValue"].ToString();

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
						catch (System.IndexOutOfRangeException ex)
						{

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

        public static List<ItemFieldValue> GetByGuid(Guid fieldGuid)
        {
            IDataReader reader = DBItemFieldValues.GetByGuid(fieldGuid);
            return LoadListFromReader(reader);
        }

        public static List<ItemFieldValue> GetByGuidForModule(Guid fieldGuid, Guid moduleGuid)
        {
            IDataReader reader = DBItemFieldValues.GetByGuidForModule(fieldGuid, moduleGuid);
            return LoadListFromReader(reader);
        }

        public static List<ItemFieldValue> GetByGuidForModule(Guid fieldGuid, int moduleId)
        {
            IDataReader reader = DBItemFieldValues.GetByGuidForModule(fieldGuid, moduleId);
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





