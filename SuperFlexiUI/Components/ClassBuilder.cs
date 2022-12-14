using log4net;
using Microsoft.CSharp;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using SuperFlexiBusiness;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace SuperFlexiUI
{
	public class ClassBuilder
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ClassBuilder));


		private readonly ModuleConfiguration _config;
		private readonly List<Field> _fields;
		private object _item;

		public Type Class { get; private set; }
		public List<object> Items { get; private set; } = new List<object>();
		public bool IsEditable { get; set; }
		public int PageId { get; set; }
		public List<ItemWithValues> ItemsWithValues { get; set; }

		public ClassBuilder(ModuleConfiguration config, List<Field> fields)
		{
			_config = config;
			_fields = fields;
			
			Class = CreateSolutionClass();
		}

		public ClassBuilder Init()
		{
			//_item = Activator.CreateInstance(Class);
			PopulateItemValues();
			return this;
		}

		public Type CreateSolutionClass()
		{
			var className = "_" + _config.FieldDefinitionGuid.ToString("N");
			var solutionName = _config.MarkupDefinitionName;

			var classCode = $@"
                    using System;
                    using System.Collections.Generic;
                    //using mojoPortal.Web.ModelBinders;
                    /// <summary>
                    /// Dynamically generated class for {solutionName}
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
					switch (field.ControlType)
					{
						case "CheckBox":
							if (field.CheckBoxReturnBool)
							{
								sb.AppendLine($"public bool {field.Name.Replace(" ", string.Empty)} {{get;set;}}");
							}
							else
							{
								goto default;
							}
							break;
						case "CheckBoxList":
						case "DynamicCheckBoxList":
							sb.AppendLine($"public List<string> {field.Name.Replace(" ", string.Empty)} {{get;set;}}");
							sbConstructor.AppendLine(field.Name + " = new List<string>();");
							break;
						case "DateTime":
						case "Date":
							sb.AppendLine($"public DateTime {field.Name.Replace(" ", string.Empty)} {{get;set;}}");
							sb.AppendLine($"public DateTime {field.Name.Replace(" ", string.Empty)}UTC {{get;set;}}");
							break;
						case "TextBox":
						default:
							if (field.IsDateField()) goto case "Date";

							sb.AppendLine($"public string {field.Name.Replace(" ", string.Empty)} {{get;set;}}");
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
			var options = new CompilerParameters
			{
				GenerateExecutable = false,
				GenerateInMemory = true,

			};
			options.ReferencedAssemblies.Add(System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Substring(8));
			options.ReferencedAssemblies.Add(System.Reflection.Assembly.GetAssembly(typeof(Global)).CodeBase.Substring(8));

			var provider = new CSharpCodeProvider();
			var compile = provider.CompileAssemblyFromSource(options, classCode);

			if (compile != null)
			{
				if (compile.Errors.Count > 0)
				{
					log.Error(compile.Errors);
				}

				return compile.CompiledAssembly.GetType(className);
			}
			else
			{
				log.Error("could not compile");
				return null;
			}
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

				string itemEditUrl = SiteUtils.GetNavigationSiteRoot() + "/SuperFlexi/Edit.aspx?pageid=" + PageId + "&mid=" + iwv.Item.ModuleID + "&itemid=" + iwv.Item.ItemID;

				SetItemClassProperty("Id", iwv.Item.ItemID);
				SetItemClassProperty("Guid", iwv.Item.ItemGuid);
				SetItemClassProperty("SortOrder", iwv.Item.SortOrder);
				SetItemClassProperty("EditUrl", itemIsEditable ? itemEditUrl : string.Empty);
				SetItemClassProperty("IsEditable", itemIsEditable);

				//List<ItemFieldValue> fieldValues = ItemFieldValue.GetItemValues(iwv.Item.ItemGuid);

				foreach (var fieldValue in iwv.Values)
				{
					var field = _fields.FirstOrDefault(f => f.Name == fieldValue.Key);
					if (field != null)
					{
						switch (field.ControlType)
						{
							case "CheckBox":
								if (!field.CheckBoxReturnBool) goto default;

								SetItemClassProperty(field.Name, Convert.ToBoolean(fieldValue.Value.ToString()));

								break;
							case "CheckBoxList":
							case "DynamicCheckBoxList":
								SetItemClassProperty(field.Name, fieldValue.Value.ToString().SplitOnCharAndTrim(';'));
								break;
							case "DateTime":
							case "Date":
								if (!string.IsNullOrWhiteSpace(fieldValue.Value.ToString()))
								{
									DateTime.TryParse(fieldValue.Value.ToString(), out DateTime dt);
									SetItemClassProperty(field.Name, TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeToUtc(dt), DateTimeKind.Utc), SiteUtils.GetUserTimeZone()));
									SetItemClassProperty(field.Name + "UTC", TimeZoneInfo.ConvertTimeToUtc(dt));
								}
								break;
							case "TextBox":
							default:
								if (field.IsDateField()) goto case "Date";

								SetItemClassProperty(field.Name, fieldValue.Value.ToString());
								break;
						}
					}
				}
				Items.Add(_item);
			}
		}
	}
}