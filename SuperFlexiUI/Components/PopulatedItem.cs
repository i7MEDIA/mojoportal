using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperFlexiBusiness;
using mojoPortal.Web.Framework;
using mojoPortal.Business.WebHelpers;

namespace SuperFlexiUI
{
	public class PopulatedItem
	{
		public Guid Guid { get; set; }
		public int Id { get; set; }
		public int SortOrder { get; set; }
		public DateTime CreatedUTC { get; set; }
		public DateTime UpdatedUTC { get; set; }
		//public List<PopulatedValue> Values { get; set; }
		public Dictionary<string,object> Values { get; set; }

		public PopulatedItem (Item item, List<Field> fields, List<ItemFieldValue> values, bool canEdit = false)
		{
			//List<PopulatedValue> popValues = new List<PopulatedValue>();

			//Item = item;
			Guid = item.ItemGuid;
			Id = item.ItemID;
			SortOrder = item.SortOrder;
			CreatedUTC = item.CreatedUtc;
			UpdatedUTC = item.LastModUtc;
			Values = new Dictionary<string, object>();
			foreach (Field field in fields)
			{
				if (String.IsNullOrWhiteSpace(field.Token)) field.Token = "$_NONE_$";
				if (!WebUser.IsAdmin && !WebUser.IsInRoles(field.ViewRoles) && !WebUser.IsInRoles(field.EditRoles))
				{
					Values.Add(field.Name, null);
					continue;
				}

				var thisValue = values.Where(v => v.FieldGuid == field.FieldGuid).FirstOrDefault();

				bool fieldValueFound = thisValue != null;

				if (fieldValueFound && (!String.IsNullOrWhiteSpace(thisValue.FieldValue) &&
								!thisValue.FieldValue.StartsWith("&deleted&") &&
								!thisValue.FieldValue.StartsWith("&amp;deleted&amp;") &&
								!thisValue.FieldValue.StartsWith("<p>&deleted&</p>") &&
								!thisValue.FieldValue.StartsWith("<p>&amp;deleted&amp;</p>")))
				{
					//popValues.Add(new PopulatedValue
					//{
					//	Name = field.Name,
					//	Token = field.Token,
					//	Value = thisValue.FieldValue,
					//	ValueGuid = thisValue.ValueGuid,
					//	FieldGuid = thisValue.FieldGuid
					//});
					if (field.ControlType == "DynamicCheckBoxList" || field.ControlType == "CheckBoxList")
					{
						Values.Add(field.Name, thisValue.FieldValue.SplitOnCharAndTrim(';'));
					}
					else if (field.ControlType == "CheckBox" && field.CheckBoxReturnBool == true)
					{
						Values.Add(field.Name, Convert.ToBoolean(thisValue.FieldValue));
					}
					else
					{
						Values.Add(field.Name, thisValue.FieldValue);
					}

				}
				else
				{
					Values.Add(field.Name, null);
				}
			}
			//Values = popValues;
			// if (field.ControlType != "InstructionBlock")
		}
		public PopulatedItem (Item item)
		{
			
			List<Field> fields = Field.GetAllForDefinition(item.DefinitionGuid);
			//var itemGuids = items.Select(x => x.ItemGuid).ToList();
			List<ItemFieldValue> values = ItemFieldValue.GetByItemGuids(new List<Guid> { item.ItemGuid });

			new PopulatedItem(item, fields, values);
		}

		public PopulatedItem(Item item, List<Field> fields)
		{
			List<ItemFieldValue> values = ItemFieldValue.GetByItemGuids(new List<Guid> { item.ItemGuid });

			new PopulatedItem(item, fields, values);
		}
	}

	public class PopulatedValue
	{
		public string Name { get; set; }
		public string Token { get; set; }
		public string Value { get; set; }
		public Guid FieldGuid { get; set; }
		public Guid ValueGuid { get; set; }
		//public ItemFieldValue ItemFieldValue { get; set; }
	}
}
