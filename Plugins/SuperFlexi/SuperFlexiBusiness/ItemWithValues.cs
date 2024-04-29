using SuperFlexiData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace SuperFlexiBusiness
{
	public class ItemWithValues
	{
		public Item Item { get; set; }
		public List<Field> Fields { get; set; }
		public Dictionary<string, object> Values { get; set; }
		//public Dictionary<string, Guid> FieldGuids { get; set; } 

		public ItemWithValues() { }

		public ItemWithValues(int itemID)
		{
			var reader = DBItems.GetOneWithValues(itemID);
			var list = LoadListFromReader(reader, 0, out _, out _);

			if (list != null && list.Count > 0)
			{
				Item = list[0].Item;
				Fields = list[0].Fields;
				Values = list[0].Values;
			}
		}

		public int CompareTo(ItemWithValues other)
		{
			return this.Item.ItemID.CompareTo(other.Item.ItemID);
		}


		public static List<ItemWithValues> GetListByIDs(Guid defGuid, Guid siteGuid, List<int> itemIDs)
		{
			return GetListByIDs(defGuid, siteGuid, itemIDs, out _, out _);
		}

		public static List<ItemWithValues> GetListByIDs(Guid defGuid, Guid siteGuid, List<int> itemIDs, out int totalPages, out int totalRows, int pageSize = 1999999999)
		{
			var reader = DBItems.GetByIDsWithValues(defGuid, siteGuid, itemIDs);

			return LoadListFromReader(reader, pageSize, out totalPages, out totalRows);
		}


		/// <summary>
		/// Gets a list of Items within a "page"
		/// </summary>
		/// <param name="moduleId"></param>
		/// <param name="pageNumber"></param>
		/// <param name="pageSize"></param>
		/// <param name="totalPages"></param>
		/// <param name="searchTerm"></param>
		/// <param name="searchField"></param>
		/// <param name="descending"></param>
		/// <returns></returns>
		public static List<ItemWithValues> GetListForModule(
			Guid moduleGuid,
			out int totalPages,
			out int totalRows,
			int pageNumber = 1,
			int pageSize = 20,
			string searchTerm = "",
			string searchField = "",
			bool descending = false)
		{
			return LoadListFromReader(
				DBItems.GetForModuleWithValues(moduleGuid, pageNumber, pageSize, searchTerm, searchField, descending ? "DESC" : "ASC"),
				pageSize,
				out totalPages,
				out totalRows);
		}


		/// <summary>
		/// Gets a list of Items for a Definition.
		/// </summary>
		/// <param name="moduleId"></param>
		/// <param name="pageNumber"></param>
		/// <param name="pageSize"></param>
		/// <param name="totalPages"></param>
		/// <param name="searchTerm"></param>
		/// <param name="searchField"></param>
		/// <param name="descending"></param>
		/// <returns></returns>
		public static List<ItemWithValues> GetListForDefinition(
			Guid defGuid,
			Guid siteGuid,
			out int totalPages,
			out int totalRows,
			int pageNumber = 1,
			int pageSize = 20,
			string searchTerm = "",
			string searchField = "",
			bool descending = false)
		{
			return LoadListFromReader(
				DBItems.GetForDefinitionWithValues(defGuid, siteGuid, pageNumber, pageSize, searchTerm, searchField, descending ? "DESC" : "ASC"),
				pageSize,
				out totalPages,
				out totalRows);
		}

		private static List<ItemWithValues> LoadListFromReader(IDataReader reader, int pageSize, out int totalPages, out int totalRows)
		{
			List<ItemWithValues> itemList = new();
			List<Field> fields = new();
			totalRows = 0;
			totalPages = 1;

			// for each distinct item i need a list of values (fieldname=>fieldvalue)
			try
			{

				while (reader.Read())
				{
					ItemWithValues itemWithValues = new()
					{
						Item = new Item
						{
							SiteGuid = new Guid(reader["SiteGuid"].ToString()),
							FeatureGuid = new Guid(reader["FeatureGuid"].ToString()),
							ModuleGuid = new Guid(reader["ModuleGuid"].ToString()),
							ModuleID = Convert.ToInt32(reader["ModuleID"]),
							DefinitionGuid = new Guid(reader["DefinitionGuid"].ToString()),
							ItemGuid = new Guid(reader["ItemGuid"].ToString()),
							ItemID = Convert.ToInt32(reader["ItemID"]),
							SortOrder = Convert.ToInt32(reader["SortOrder"]),
							CreatedUtc = Convert.ToDateTime(reader["CreatedUtc"]),
							LastModUtc = Convert.ToDateTime(reader["LastModUtc"]),
							ViewRoles = reader["ViewRoles"].ToString(),
							EditRoles = reader["EditRoles"].ToString()
						},
						Values = new Dictionary<string, object>(),
						//FieldGuids = new Dictionary<string, Guid>()
					};

					if (fields.Count == 0)
					{
						fields = Field.GetAllForDefinition(itemWithValues.Item.DefinitionGuid);
					}

					itemWithValues.Fields = fields;

					// Not all methods will use TotalRows but there is no sense in having an extra method to load the reader
					// so, we'll catch the error and do nothing with it because we are expecting it
					// the if statement should keep any problems at bay but we still use try/catch in case someone inadvertently 

					if (pageSize > 0)
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


					var fieldName = reader["FieldName"].ToString();
					var fieldValue = reader["FieldValue"].ToString();

					try
					{
						//first, we'll try to add the value to the list with the corresponding item
						//itemList.Where(i => i.Item.ItemGuid == itemWithValues.Item.ItemGuid).First().Values
						//	.Add(reader["FieldName"].ToString(), 
						//	new { GUID = reader["FieldGuid"].ToString(), Value = reader["FieldValue"].ToString() });

						itemList.Where(i => i.Item.ItemGuid == itemWithValues.Item.ItemGuid).First().Values
							.Add(fieldName, fieldValue);
					}
					catch (Exception)
					{
						//corresponding item not found, we'll add our value to the current item and then add that item to our list
						//itemWithValues.Values.Add(reader["FieldName"].ToString(),
						//	new { GUID = reader["FieldGuid"].ToString(), Value = reader["FieldValue"].ToString() });
						itemWithValues.Values.Add(fieldName, fieldValue);

						itemList.Add(itemWithValues);
					}
				}
			}
			finally
			{
				reader.Close();
			}

			if (totalRows == 0) { totalRows = itemList.Count; }

			if (totalRows <= pageSize || pageSize == 0)
			{
				totalPages = 1;
			}
			else
			{
				//totalPages = totalRows / pageSize;

				//Math.DivRem(totalRows, pageSize, out int remainder);

				//if (remainder > 0)
				//{
				//	totalPages += 1;
				//}

				totalPages = (int)decimal.Ceiling((decimal)totalRows / pageSize);
			}

			return itemList;
		}

	}

	public class SimpleItemWithValuesComparer : IEqualityComparer<ItemWithValues>
	{
		//Items are equal if their ItemIDs and ItemGuids are equal.
		public bool Equals(ItemWithValues x, ItemWithValues y)
		{
			//check whether the compared objects reference the same data.
			if (Object.ReferenceEquals(x.Item, y.Item)) return true;

			//check for nulls
			if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
			{
				return false;
			}
			
			if (Object.ReferenceEquals(x.Item, null) || Object.ReferenceEquals(y.Item, null))
			{
				return false;
			}


			//check if properties are equal
			return x.Item.ItemGuid == y.Item.ItemGuid && x.Item.ItemID == y.Item.ItemID;
		}

		// If Equals() returns true for a pair of objects 
		// then GetHashCode() must return the same value for these objects.
		public int GetHashCode(ItemWithValues itemWithValues)
		{
			//Check whether the object is null
			if (Object.ReferenceEquals(itemWithValues, null)) return 0;
			if (object.ReferenceEquals(itemWithValues.Item, null)) return 0;

			int hashItemID = itemWithValues.Item.ItemID.GetHashCode();
			int hashItemGuid = itemWithValues.Item.ItemGuid.GetHashCode();

			return hashItemID ^ hashItemGuid;
		}
	}
}
