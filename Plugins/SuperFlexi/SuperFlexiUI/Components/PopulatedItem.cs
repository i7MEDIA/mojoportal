using mojoPortal.Business.WebHelpers;
using SuperFlexiBusiness;
using SuperFlexiUI.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperFlexiUI;

public class PopulatedItem
{
	public Guid Guid { get; set; }
	public int Id { get; set; }
	public int SortOrder { get; set; }
	public DateTime CreatedUTC { get; set; }
	public DateTime UpdatedUTC { get; set; }
	public Dictionary<string, object> Values { get; set; }


	public PopulatedItem(
		Item item,
		List<Field> fields,
		List<ItemFieldValue> values,
		bool canEdit = false
	)
	{
		Guid = item.ItemGuid;
		Id = item.ItemID;
		SortOrder = item.SortOrder;
		CreatedUTC = item.CreatedUtc;
		UpdatedUTC = item.LastModUtc;
		Values = [];

		foreach (var field in fields)
		{
			if (string.IsNullOrWhiteSpace(field.Token))
			{
				field.Token = "$_NONE_$";
			}

			if (
				!WebUser.IsAdmin &&
				!WebUser.IsInRoles(field.ViewRoles) &&
				!WebUser.IsInRoles(field.EditRoles)
			)
			{
				Values.Add(field.Name, null);

				continue;
			}

			var thisValue = values.Where(v => v.FieldGuid == field.FieldGuid).FirstOrDefault();
			var fieldValueFound = thisValue != null;

			if (
				fieldValueFound &&
				!string.IsNullOrWhiteSpace(thisValue.FieldValue) &&
				!thisValue.FieldValue.StartsWith("&deleted&") &&
				!thisValue.FieldValue.StartsWith("&amp;deleted&amp;") &&
				!thisValue.FieldValue.StartsWith("<p>&deleted&</p>") &&
				!thisValue.FieldValue.StartsWith("<p>&amp;deleted&amp;</p>")
			)
			{
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
	}


	public PopulatedItem(Item item)
	{
		var fields = SuperFlexiCache.GetFields(item.DefinitionGuid);
		var values = ItemFieldValue.GetByItemGuids([item.ItemGuid]);

		new PopulatedItem(item, fields, values);
	}


	public PopulatedItem(Item item, List<Field> fields)
	{
		var values = ItemFieldValue.GetByItemGuids([item.ItemGuid]);

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
}
