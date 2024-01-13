using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Microsoft.CodeAnalysis;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using SuperFlexiBusiness;
using Westwind.Scripting;

namespace SuperFlexiUI;

public class ClassBuilder
{
	private static readonly ILog log = LogManager.GetLogger(typeof(ClassBuilder));
	private string _defName;
	private Guid _defGuid;
	private List<Field> _fields;
	private object _item;
	private List<ItemWithValues> ItemsWithValues { get; set; }

	public Type Class { get; private set; }
	public List<object> Items { get; private set; } = new List<object>();
	public bool IsEditable { get; set; }
	public int PageId { get; set; }

	public ClassBuilder(List<ItemWithValues> items)
	{
		ItemsWithValues = items;
		Class = CreateSolutionClass();
	}

	public ClassBuilder(int itemId)
	{
		var list = new List<ItemWithValues> {
			new(itemId)
		};

		ItemsWithValues = list;
		Class = CreateSolutionClass();
	}

	public ClassBuilder Init()
	{

		PopulateItemValues();
		return this;
	}

	public Type CreateSolutionClass()
	{

		if (ItemsWithValues is not null && ItemsWithValues.Count > 0)
		{
			_fields = ItemsWithValues[0].Fields;
		}

		if (_fields is not null && _fields.Count > 0)
		{
			_defName = _fields[0].DefinitionName;
			_defGuid = _fields[0].DefinitionGuid;
		}

		if (ItemsWithValues is null || _fields is null)
		{
			return this.GetType();
		}

		var className = $"_{_defGuid:N}";
		var classCode = $@"
                    using System;
                    using System.Collections.Generic;
                    /// <summary>
                    /// Dynamically generated class for {_defName}
                    /// </summary>
                    public class {className} {{
                        public int Id {{get;set;}}
                        public Guid Guid {{get;set;}}
                        public int SortOrder {{get;set;}}
                        public string EditUrl {{get;set;}}
                        public bool IsEditable {{get;set;}}
                        {getFields()}                        
                    }}";

		string getFields()
		{
			var sb = new StringBuilder();
			var sbConstructor = new StringBuilder();
			sbConstructor.AppendLine($"public {className}(){{");

			foreach (Field field in _fields)
			{
				var fieldName = field.Name.Replace(" ", string.Empty);

				switch (field.ControlType)
				{
					case "CheckBox":
						if (field.CheckBoxReturnBool)
						{
							sb.AppendLine($"public bool {fieldName} {{get;set;}}");
						}
						else
						{
							goto default;
						}
						break;
					case "List":
					case "CheckBoxList":
					case "DynamicCheckBoxList":
						sb.AppendLine($"public List<{field.DataType}> {fieldName} {{get;set;}}");
						sbConstructor.AppendLine($"{fieldName} = new List<{field.DataType}>();");
						break;
					case "DateTime":
					case "Date":
						sb.AppendLine($"public DateTime {field.Name.Replace(" ", string.Empty)} {{get;set;}}");
						sb.AppendLine($"public DateTime {field.Name.Replace(" ", string.Empty)}UTC {{get;set;}}");
						break;
					case "TextBox":
					default:
						if (field.IsDateField) goto case "Date";
						if (field.IsList) goto case "List";
						sb.AppendLine($"public {field.DataType} {fieldName} {{get;set;}}");
						break;
				}
			}
			if (sbConstructor.Length > 1)
			{
				sbConstructor.AppendLine("}");
				sb.AppendLine(sbConstructor.ToString());
			}
			return sb.ToString();
		}

		log.Debug(classCode);

		var script = new CSharpScriptExecution()
		{
			SaveGeneratedCode = true,
			GeneratedClassName = className,
			GeneratedNamespace = "SuperFlexiUI.Solutions",
		};
		script.AddDefaultReferencesAndNamespaces();
		script.AddAssembly("SuperFlexiUI.dll");
		script.AddAssembly("mojoPortal.Web.dll");
		return script.CompileClassToType(classCode);
		// CodeDom Compiler

		//var options = new CompilerParameters
		//{
		//	GenerateExecutable = false,
		//	GenerateInMemory = true,

		//};
		//options.ReferencedAssemblies.Add(System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Substring(8));
		//options.ReferencedAssemblies.Add(System.Reflection.Assembly.GetAssembly(typeof(Global)).CodeBase.Substring(8));
		//var provider = new CSharpCodeProvider();
		//var compile = provider.CompileAssemblyFromSource(options, classCode);
		//var path = compile.PathToAssembly;
		//var v = compile.CompiledAssembly;
		//if (compile is not null)
		//{
		//	if (compile.Errors.Count > 0)
		//	{
		//		log.Error(compile.Errors);
		//	}
		//	type = compile.CompiledAssembly.GetType(className);
		//	return type;
		//}
		//else
		//{
		//	log.Error("could not compile");
		//	return null;
		//}
	}

	internal void SetItemClassProperty(string propName, object propValue)
	{
		_item.GetType()
			.GetProperty(propName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
			.SetValue(_item, propValue);
	}

	internal void PopulateItemValues()
	{
		foreach (var iwv in ItemsWithValues)
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

							SetItemClassProperty(fieldName, theValue);

							break;
					}
				}
			}
			Items.Add(_item);
		}
	}
}