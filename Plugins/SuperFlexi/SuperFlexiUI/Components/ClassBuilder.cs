using log4net;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Components;
using SuperFlexiBusiness;
using SuperFlexiUI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SuperFlexiUI;

public class ClassBuilder
{
	#region Fields

	private static readonly ILog _log = LogManager.GetLogger(typeof(ClassBuilder));

	private string _classname = string.Empty;
	private string _defName;
	private Guid _defGuid;
	private List<Field> _fields;
	private object _item;
	private readonly List<ItemWithValues> _itemsWithValues;

	#endregion


	#region Properties

	public Type Class { get; private set; }
	public List<dynamic> Items { get; private set; } = [];
	public bool IsEditable { get; set; }
	public int PageId { get; set; }

	#endregion


	#region Constructors

	// Must use Init
	private ClassBuilder(
		List<ItemWithValues> items,
		bool isEditable,
		int pageId
	)
	{
		_itemsWithValues = items;
		IsEditable = isEditable;
		PageId = pageId;
		Class = GetSolutionClassFromCache();

		PopulateItemValues();
	}

	#endregion


	#region Factory Methods

	public static ClassBuilder Init(List<ItemWithValues> items, bool isEditable, int pageId) =>
		new(items, isEditable, pageId);

	#endregion


	#region Public Methods

	public static string GetClassName(Guid fieldDefinitionGuid)
	{
		return $"SuperFlexiSolution_{fieldDefinitionGuid:N}";
	}


	public Type CreateSolutionClass()
	{
		if (_itemsWithValues is not null && _itemsWithValues.Count > 0)
		{
			_fields = _itemsWithValues[0].Fields;
		}

		if (_fields is not null && _fields.Count > 0)
		{
			_defName = _fields[0].DefinitionName;
			_defGuid = _fields[0].DefinitionGuid;
		}

		if (_itemsWithValues is null || _fields is null)
		{
			return GetType();
		}

		var dynamicType = DynamicTypeGenerator
			.Init(_classname)
			.AddProperties(new()
			{
				["Id"] = typeof(int),
				["Guid"] = typeof(Guid),
				["SortOrder"] = typeof(int),
				["EditUrl"] = typeof(string),
				["IsEditable"] = typeof(bool),
			});

		foreach (var field in _fields)
		{
			var fieldName = field.Name.Replace(" ", string.Empty);

			switch (field.ControlType)
			{
				case "CheckBox":
					if (field.CheckBoxReturnBool)
					{
						dynamicType.AddProperty(fieldName, typeof(bool));
					}
					else
					{
						goto default;
					}
					break;
				case "List":
				case "CheckBoxList":
				case "DynamicCheckBoxList":
					dynamicType.AddProperty(fieldName, GetTypeFromName(field.DataType, isList: true));
					break;
				case "DateTime":
				case "Date":
					dynamicType.AddProperty(fieldName, typeof(DateTime));
					dynamicType.AddProperty(fieldName + "UTC", typeof(DateTime));
					break;
				case "TextBox":
				default:
					if (field.IsDateField) goto case "Date";
					if (field.IsList) goto case "List";
					dynamicType.AddProperty(fieldName, GetTypeFromName(field.DataType));
					break;
			}
		}

		return dynamicType.CreateType();
	}

	#endregion


	#region Private Methods

	private Type GetSolutionClassFromCache()
	{
		if (_itemsWithValues is { Count: > 0 })
		{
			_fields = SuperFlexiCache.GetFields(_itemsWithValues[0].Item.DefinitionGuid);
		}

		if (_fields is { Count: > 0 })
		{
			_defName = _fields[0].DefinitionName;
			_defGuid = _fields[0].DefinitionGuid;
		}

		_classname = GetClassName(_defGuid);

		if (SuperFlexiCache.ClassCache.TryGetValue(_classname, out Type cachedType))
		{
			return cachedType;
		}
		else
		{
			var newType = CreateSolutionClass();

			if (newType is not null)
			{
				SuperFlexiCache.ClassCache.TryAdd(_classname, newType);

				return newType;
			}
			else
			{
				_log.Error($"failed to create class for {_classname}");

				throw new InvalidOperationException($"failed to create class for {_defName} [FieldDefinitionGuid: {_defGuid}]");
			}
		}
	}


	private static Type GetTypeFromName(string typeName, bool isList = false) => typeName switch
	{
		var _ when isList => typeof(List<>).MakeGenericType(GetTypeFromName(typeName)),
		"int" => typeof(int),
		"bool" or "boolean" => typeof(bool),
		"DateTime" => typeof(DateTime),
		"string" or _ => typeof(string),
	};


	private void SetItemClassProperty(string propName, object propValue)
	{
		_item.GetType()
			.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance)
			.SetValue(_item, propValue);
	}


	private void PopulateItemValues()
	{
		foreach (var iwv in _itemsWithValues)
		{

			bool itemIsEditable = IsEditable || WebUser.IsInRoles(iwv.Item.EditRoles);
			bool itemIsViewable = WebUser.IsInRoles(iwv.Item.ViewRoles) || itemIsEditable;

			if (!itemIsViewable)
			{
				continue;
			}

			_item = Activator.CreateInstance(Class);

			string itemEditUrl = Invariant($"{SiteUtils.GetNavigationSiteRoot()}/SuperFlexi/Edit.aspx?pageid={PageId}&mid={iwv.Item.ModuleID}&itemid={iwv.Item.ItemID}");

			SetItemClassProperty("Id", iwv.Item.ItemID);
			SetItemClassProperty("Guid", iwv.Item.ItemGuid);
			SetItemClassProperty("SortOrder", iwv.Item.SortOrder);
			SetItemClassProperty("EditUrl", itemIsEditable ? itemEditUrl : string.Empty);
			SetItemClassProperty("IsEditable", itemIsEditable);

			foreach (var fieldValue in iwv.Values)
			{
				var field = _fields.FirstOrDefault(f => f.Name == fieldValue.Key);
				if (field is not null)
				{
					var fieldName = field.Name.Replace(" ", string.Empty);

					object theValue = SuperFlexiHelpers.GetFieldValueFromKVPWithType(fieldValue, field);

					switch (field.ControlType)
					{
						case "CheckBox":
							if (!field.CheckBoxReturnBool)
							{
								goto default;
							}

							SetItemClassProperty(fieldName, theValue);

							break;
						case "List":
						case "CheckBoxList":
						case "DynamicCheckBoxList":

							if (field.DataType == "int")
							{
								SetItemClassProperty(fieldName, fieldValue.Value.ToString().SplitIntStringOnCharAndTrim(';'));
							}
							else
							{
								SetItemClassProperty(fieldName, fieldValue.Value.ToString().SplitOnCharAndTrim(';'));
							}

							break;
						case "DateTime":
						case "Date":
							if (!string.IsNullOrWhiteSpace(fieldValue.Value.ToString()))
							{
								DateTime.TryParse(fieldValue.Value.ToString(), out DateTime dt);
								SetItemClassProperty(fieldName, TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeToUtc(dt), DateTimeKind.Utc), SiteUtils.GetUserTimeZone()));
								SetItemClassProperty(fieldName + "UTC", TimeZoneInfo.ConvertTimeToUtc(dt));
							}
							break;
						case "TextBox":
						default:
							if (field.IsDateField)
							{
								goto case "Date";
							}

							if (field.IsList)
							{
								goto case "List";
							}

							if (field.DataType == "int")
							{
								if (int.TryParse(theValue.ToString(), out int intVal))
								{
									SetItemClassProperty(fieldName, intVal);
								}
								else
								{
									SetItemClassProperty(fieldName, field.DefaultValue);
								}
							}
							else
							{
								SetItemClassProperty(fieldName, theValue);

							}

							break;
					}
				}
			}
			Items.Add(_item);
		}
	}


	// Someday move to this, it would allow us to automatically update the cache based on the fields

	//var properties = newType
	//	.GetProperties()
	//	.Select(x => new KeyValuePair<string, Type>(x.Name, x.GetType()))
	//	.ToDictionary(x => x.Key, x => x.Value);
	//var cacheName = GenerateCacheKey(_classname, properties);

	//private static string GenerateCacheKey(string baseClassName, Dictionary<string, Type> properties)
	//{
	//	// Sort properties by name to ensure the same schema always produces the same key
	//	var sortedProperties = properties.OrderBy(p => p.Key);

	//	var sb = new StringBuilder();

	//	sb.Append(baseClassName).Append('|');

	//	foreach (var prop in sortedProperties)
	//	{
	//		sb.Append($"{prop.Key}:{prop.Value.FullName};");
	//	}

	//	return sb.ToString();
	//}

	#endregion
}
