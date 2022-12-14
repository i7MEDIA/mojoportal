using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperFlexiData;
namespace SuperFlexiBusiness
{
	public class ItemWithValues 
	{
		public Item Item { get; set; }
		public List<Field> Fields { get; set; }
		public Dictionary<string, object> Values { get; set; }
		//public Dictionary<string, Guid> FieldGuids { get; set; } 

		public ItemWithValues() { }

		public ItemWithValues (int itemID)
		{
			var reader = DBItems.GetOneWithValues(itemID);
			while (reader.Read())
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
				};
				Values = new Dictionary<string, object>();
			}
		}



		public int CompareTo(ItemWithValues other)
		{
			return this.Item.ItemID.CompareTo(other.Item.ItemID);
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
