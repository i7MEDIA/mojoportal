using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using SuperFlexiBusiness;
using SuperFlexiUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuperFlexiUI
{
	[ToolboxData("<{0}:WidgetRazor runat=server></{0}:WidgetRazor>")]
	public class WidgetRazor : WebControl
	{
		#region Properties
		private static readonly ILog log = LogManager.GetLogger(typeof(WidgetRazor));
		protected TimeZoneInfo timeZone = null;

		private int pageNumber = 0;
		private int pageSize = 0;
		private int totalPages = -1;
		private int totalRows = -1;
		private int itemId = 0;
		private bool getDynamicListValuesFromReturnedItems = false;

		private SuperFlexiDisplaySettings displaySettings { get; set; }

		//StringBuilder strOutput = new StringBuilder();
		//StringBuilder strAboveMarkupScripts = new StringBuilder();
		//StringBuilder strBelowMarkupScripts = new StringBuilder();
		private List<Item> items = new List<Item>();
		private List<Field> fields = new List<Field>();
		private List<Field> dynamicLists = new List<Field>();
		private List<ItemFieldValue> dynamicListValues = new List<ItemFieldValue>();
		private Dictionary<string, string> dynamicQueryParams = new Dictionary<string, string>();
		List<ItemWithValues> itemsWithValues = new List<ItemWithValues>();
		//List<ItemFieldValue> fieldValues = new List<ItemFieldValue>();
		SiteSettings siteSettings;
		Module module;

		public ModuleConfiguration Config { get; set; } = new ModuleConfiguration();
		public string SiteRoot { get; set; } = string.Empty;
		public string ImageSiteRoot { get; set; } = string.Empty;
		public bool IsEditable { get; set; } = false;
		public int ModuleId { get; set; } = -1;
		public Guid ModuleGuid { get; set; }
		public int PageId { get; set; } = -1;
		public PageSettings CurrentPage { get; set; }

		#endregion


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			LoadSettings();

			if (module == null)
			{
				//Visible = false;
				return;
			}

			if (Page.IsPostBack)
			{
				return;
			}
		}

		protected virtual void LoadSettings()
		{
			module = new Module(ModuleId);
			if (module == null)
			{
				return;
			}
			//moduleTitle = module.ModuleTitle;

			pageNumber = WebUtils.ParseInt32FromQueryString($"sf{ModuleId}_PageNumber", pageNumber);
			pageSize = WebUtils.ParseInt32FromQueryString($"sf{ModuleId}_PageSize", Config.PageSize);
			itemId = WebUtils.ParseInt32FromQueryString($"sf{ModuleId}_ItemId", itemId);

			foreach (string param in HttpContext.Current.Request.QueryString.Keys)
			{
				if (param.StartsWith($"sf{ModuleId}_") && param != $"sf{ModuleId}_PageNumber" && param != $"sf{ModuleId}_PageSize" && param != $"sf{ModuleId}_ItemId")
				{
					dynamicQueryParams.Add(param, HttpContext.Current.Request.QueryString[param]);
				}
			}

			siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (CurrentPage == null)
			{
				CurrentPage = CacheHelper.GetCurrentPage();
				if (CurrentPage == null)
				{
					log.Debug("Can't use CacheHelper.GetCurrentPage() here.");
					CurrentPage = new PageSettings(siteSettings.SiteId, PageId);
				}
			}

			timeZone = SiteUtils.GetUserTimeZone();

			if (Config.MarkupDefinition != null)
			{
				displaySettings = Config.MarkupDefinition;
			}

			fields = Field.GetAllForDefinition(Config.FieldDefinitionGuid);

			dynamicLists = fields.Where(f => f.IsDynamicListField()).ToList();

			if (itemId > 0)
			{
				var item = new Item(itemId);
				if (!Config.IsGlobalView && item.ModuleID != ModuleId)
				{
					GetItems();
				}
				else
				{
					items.Add(item);
				}
			}
			else
			{
				GetItems();
			}

			if (SiteUtils.IsMobileDevice() && Config.MobileMarkupDefinition != null)
			{
				displaySettings = Config.MobileMarkupDefinition;
			}

			if (Config.MarkupScripts.Count > 0 || (SiteUtils.IsMobileDevice() && Config.MobileMarkupScripts.Count > 0))
			{

				if (SiteUtils.IsMobileDevice() && Config.MobileMarkupScripts.Count > 0)
				{
					SuperFlexiHelpers.SetupScripts(Config.MobileMarkupScripts, Config, displaySettings, IsEditable, Page.IsPostBack, ClientID, siteSettings, module, CurrentPage, Page, Parent);
				}
				else
				{
					SuperFlexiHelpers.SetupScripts(Config.MarkupScripts, Config, displaySettings, IsEditable, Page.IsPostBack, ClientID, siteSettings, module, CurrentPage, Page, Parent);
				}

			}

			if (Config.MarkupCSS.Count > 0)
			{
				SuperFlexiHelpers.SetupStyle(Config.MarkupCSS, Config, displaySettings, IsEditable, ClientID, siteSettings, module, CurrentPage, Page, this);
			}
		}

		private void GetItems()
		{

			if (dynamicQueryParams.Count > 0)
			{
				foreach (var set in dynamicQueryParams)
				{
					if (set.Value.Contains(";"))
					{
						foreach (var setValue in set.Value.SplitOnCharAndTrim(';'))
						{
							itemsWithValues.AddRange(GetItemsWithValues(setValue, set.Key));
						}
					}
				}

				itemsWithValues = itemsWithValues.Distinct(new SimpleItemWithValuesComparer()).ToList();

				if (totalPages > 1 && dynamicLists.Count > 0)
				{
					foreach (var dynamicListField in dynamicLists)
					{
						if (Config.IsGlobalView)
						{
							dynamicListValues.AddRange(ItemFieldValue.GetByFieldGuid(dynamicListField.FieldGuid));
						}
						else
						{
							dynamicListValues.AddRange(ItemFieldValue.GetByFieldGuidForModule(dynamicListField.FieldGuid, module.ModuleGuid));
						}
					}
				}
			}
			else if (Config.ProcessItems)
			{
				itemsWithValues.AddRange(GetItemsWithValues());
				getDynamicListValuesFromReturnedItems = true;
			}

			//if (Config.IsGlobalView)
			//{
			//	if (pageNumber > -1)
			//	{
			//		items = Item.GetForDefinition(Config.FieldDefinitionGuid, siteSettings.SiteGuid, pageNumber, pageSize, out totalPages, out totalRows, Config.DescendingSort);

			//		if (totalPages > 1 && dynamicLists.Count > 0)
			//		{
			//			foreach (var dynamicListField in dynamicLists)
			//			{
			//				dynamicListValues.AddRange(ItemFieldValue.GetByFieldGuid(dynamicListField.FieldGuid));
			//			}
			//		}
			//	}
			//	else
			//	{
			//		items = Item.GetForDefinition(Config.FieldDefinitionGuid, siteSettings.SiteGuid, Config.DescendingSort);
			//		getDynamicListValuesFromReturnedItems = true;
			//	}
			//}
			//else
			//{
			//	if (pageNumber > -1)
			//	{
			//		items = Item.GetForModule(ModuleId, pageNumber, pageSize, out totalPages, out totalRows, Config.DescendingSort);

			//		if (totalPages > 1 && dynamicLists.Count > 0)
			//		{
			//			foreach (var dynamicListField in dynamicLists)
			//			{
			//				dynamicListValues.AddRange(ItemFieldValue.GetByFieldGuidForModule(dynamicListField.FieldGuid, module.ModuleGuid));
			//			}
			//		}
			//	}
			//	else
			//	{
			//		items = Item.GetForModule(ModuleId, Config.DescendingSort);
			//		getDynamicListValuesFromReturnedItems = true;
			//	}
			//}
		}

		private List<ItemWithValues> GetItemsWithValues(string searchTerm = "", string searchField = "")
		{
			if (Config.IsGlobalView)
			{
				return ItemWithValues.GetListForDefinition(
					Config.FieldDefinitionGuid,
					siteSettings.SiteGuid,
					out totalPages,
					out totalRows,
					pageNumber,
					pageSize,
					searchTerm,
					searchField,
					Config.DescendingSort
				);
			}
			else
			{
				return ItemWithValues.GetListForModule(
						ModuleGuid,
						out totalPages,
						out totalRows,
						pageNumber,
						pageSize,
						searchTerm,
						searchField,
						Config.DescendingSort
					);
			}
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			string featuredImageUrl = string.Empty;

			featuredImageUrl = String.IsNullOrWhiteSpace(Config.InstanceFeaturedImage) ? featuredImageUrl : SiteUtils.GetNavigationSiteRoot() + Config.InstanceFeaturedImage;

			bool publishedToCurrentPage = true;

			var pageModules = PageModule.GetPageModulesByModule(module.ModuleId);
			if (pageModules.Where(pm => pm.PageId == CurrentPage.PageId).ToList().Count() == 0)
			{
				publishedToCurrentPage = false;
			}

			var superFlexiItemClass = new ClassBuilder(itemsWithValues)
			{
				IsEditable = IsEditable,
				PageId = PageId
			}.Init();

			//var itemModels = new List<object> ();

			WidgetModel model = new WidgetModel
			{
				Module = new ModuleModel
				{
					Id = module.ModuleId,
					Guid = module.ModuleGuid,
					IsEditable = IsEditable,
					Pane = module.PaneName,
					PublishedToPageId = publishedToCurrentPage ? CurrentPage.PageId : -1,
					ShowTitle = module.ShowTitle,
					Title = module.ModuleTitle,
					TitleElement = module.HeadElement
				},
				Config = Config,
				//DynamicLists = new KeyValuePair<string, List<string>>(),
				Page = new PageModel
				{
					Id = CurrentPage.PageId,
					Url = CurrentPage.Url,
					Name = CurrentPage.PageName
				},
				Site = new SiteModel
				{
					Id = module.SiteId,
					Guid = siteSettings.SiteGuid,
					CacheGuid = siteSettings.SkinVersion,
					CacheKey = SiteUtils.GetCssCacheCookieName(siteSettings),
					PhysAppRoot = WebUtils.GetApplicationRoot(),
					SitePath = WebUtils.GetApplicationRoot() + "/Data/Sites/" + module.SiteId,
					SiteUrl = SiteUtils.GetNavigationSiteRoot(),
					SkinPath = SiteUtils.DetermineSkinBaseUrl(SiteUtils.GetSkinName(true, this.Page)),
					TimeZone = SiteUtils.GetSiteTimeZone()
				},
				Pagination = new PaginationModel
				{
					PageCount = totalPages > -1 ? totalPages : 1,
					PageNumber = pageNumber,
					PageSize = pageSize,
					TotalItems = totalRows > -1 ? totalRows : items.Count
				}
			};


			//        foreach (Item item in items)
			//        {
			//            var itemObject = Activator.CreateInstance(superFlexiItemClass);

			//            bool itemIsEditable = IsEditable || WebUser.IsInRoles(item.EditRoles);
			//            bool itemIsViewable = WebUser.IsInRoles(item.ViewRoles) || itemIsEditable;

			//            if (!itemIsViewable)
			//            {
			//                continue;
			//            }

			//            string itemEditUrl = SiteUtils.GetNavigationSiteRoot() + "/SuperFlexi/Edit.aspx?pageid=" + PageId + "&mid=" + item.ModuleID + "&itemid=" + item.ItemID;

			//            SetItemClassProperty(itemObject, "Id", item.ItemID);
			//            SetItemClassProperty(itemObject, "Guid", item.ItemGuid);
			//            SetItemClassProperty(itemObject, "SortOrder", item.SortOrder);
			//            SetItemClassProperty(itemObject, "EditUrl", itemEditUrl);
			//            SetItemClassProperty(itemObject, "IsEditable", itemIsEditable);

			//            List<ItemFieldValue> fieldValues = ItemFieldValue.GetItemValues(item.ItemGuid);

			//foreach (Field field in fields)
			//            {
			//                foreach (ItemFieldValue fieldValue in fieldValues)
			//                {
			//                    if (field.FieldGuid == fieldValue.FieldGuid)
			//                    {
			//                        if (getDynamicListValuesFromReturnedItems && field.IsDynamicListField())
			//                        {
			//                            dynamicListValues.Add(fieldValue);
			//                        }

			//                        switch (field.ControlType)
			//                        {
			//                            case "CheckBox":
			//                                if (field.CheckBoxReturnBool)
			//                                {
			//                                    SetItemClassProperty(itemObject, field.Name, Convert.ToBoolean(fieldValue.FieldValue));
			//                                }
			//                                else
			//                                {
			//                                    goto default;
			//                                }

			//                                break;
			//                            case "CheckBoxList":
			//                            case "DynamicCheckBoxList":
			//                                SetItemClassProperty(itemObject, field.Name, fieldValue.FieldValue.SplitOnCharAndTrim(';'));
			//                                break;
			//                            case "DateTime":
			//                            case "Date":
			//                                if (!string.IsNullOrWhiteSpace(fieldValue.FieldValue))
			//                                {
			//                                    DateTime.TryParse(fieldValue.FieldValue, out DateTime dt);
			//                                    SetItemClassProperty(itemObject, field.Name, TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeToUtc(dt), DateTimeKind.Utc), timeZone));
			//                                    SetItemClassProperty(itemObject, field.Name + "UTC", TimeZoneInfo.ConvertTimeToUtc(dt));

			//                                    //var dt2 = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(dt, DateTimeKind.Utc));
			//                                }
			//                                break;
			//                            case "TextBox":
			//                            default:
			//                                if (field.IsDateField()) goto case "Date";

			//                                SetItemClassProperty(itemObject, field.Name, fieldValue.FieldValue);
			//                                break;
			//                        }
			//                    }
			//                }
			//            }

			//            itemModels.Add(itemObject);
			//        }

			foreach (var dv in dynamicListValues)
			{

				var values = dynamicListValues.Where(list => list.FieldName == dv.FieldName).Select(v => v.FieldValue).ToList();
				model.DynamicLists.Add(dv.FieldName, values);

			}

			//        foreach (var iwv in itemsWithValues)
			//        {

			//            bool itemIsEditable = IsEditable || WebUser.IsInRoles(iwv.Item.EditRoles);
			//            bool itemIsViewable = WebUser.IsInRoles(iwv.Item.ViewRoles) || itemIsEditable;

			//            if (!itemIsViewable)
			//            {
			//                continue;
			//            }

			//            //var itemObject = Activator.CreateInstance(superFlexiItemClass.Class);

			//            string itemEditUrl =SiteUtils.GetNavigationSiteRoot() + "/SuperFlexi/Edit.aspx?pageid=" + PageId + "&mid=" + iwv.Item.ModuleID + "&itemid=" + iwv.Item.ItemID;

			//            superFlexiItemClass.SetItemClassProperty("Id", iwv.Item.ItemID);
			//superFlexiItemClass.SetItemClassProperty("Guid", iwv.Item.ItemGuid);
			//superFlexiItemClass.SetItemClassProperty("SortOrder", iwv.Item.SortOrder);
			//superFlexiItemClass.SetItemClassProperty("EditUrl", itemIsEditable? itemEditUrl : string.Empty);
			//superFlexiItemClass.SetItemClassProperty("IsEditable", itemIsEditable);

			//            //List<ItemFieldValue> fieldValues = ItemFieldValue.GetItemValues(iwv.Item.ItemGuid);

			//            foreach (var fieldValue in iwv.Values)
			//            {
			//                var field = fields.FirstOrDefault(f => f.Name == fieldValue.Key);
			//                if (field != null)
			//                {
			//                    switch (field.ControlType)
			//                    {
			//                        case "CheckBox":
			//                            if (!field.CheckBoxReturnBool) goto default;

			//				superFlexiItemClass.SetItemClassProperty(field.Name, Convert.ToBoolean(fieldValue.Value.ToString()));

			//                            break;
			//                        case "CheckBoxList":
			//                        case "DynamicCheckBoxList":
			//				superFlexiItemClass.SetItemClassProperty(field.Name, fieldValue.Value.ToString().SplitOnCharAndTrim(';'));
			//                            break;
			//                        case "DateTime":
			//                        case "Date":
			//                            if (!string.IsNullOrWhiteSpace(fieldValue.Value.ToString()))
			//                            {
			//                                DateTime.TryParse(fieldValue.Value.ToString(), out DateTime dt);
			//					superFlexiItemClass.SetItemClassProperty(field.Name, TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeToUtc(dt), DateTimeKind.Utc), timeZone));
			//					superFlexiItemClass.SetItemClassProperty(field.Name + "UTC", TimeZoneInfo.ConvertTimeToUtc(dt));
			//                            }
			//                            break;
			//                        case "TextBox":
			//                        default:
			//				if (field.IsDateField()) goto case "Date";

			//				superFlexiItemClass.SetItemClassProperty(field.Name, fieldValue.Value.ToString());
			//                            break;
			//                    }
			//                }
			//            }

			//            itemModels.Add(superFlexiItemClass.Item);
			//        }

			model.Items = superFlexiItemClass.Items;

			var viewPath = Config.RelativeSolutionLocation + "/" + Config.ViewName;

			mojoViewEngine mve = new mojoViewEngine();

			model.Site.SkinViewPath = model.Site.SkinPath + "Views/" + Config.RelativeSolutionLocation.Replace("~/Data/", string.Empty).Replace("sites/" + model.Site.Id, string.Empty) + "/" + Config.ViewName;

			List<string> masterLocationFormats = new List<string>(mve.MasterLocationFormats);
			masterLocationFormats.Insert(0, "~/Data/Sites/$SiteId$/skins/$Skin$/Views/SuperFlexi/{0}.cshtml");
			mve.MasterLocationFormats = masterLocationFormats.ToArray();

			List<string> partialViewLocationFormats = new List<string>(mve.PartialViewLocationFormats);
			partialViewLocationFormats.Insert(0, model.Site.SkinViewPath.Replace(Config.ViewName, string.Empty) + "/{0}.cshtml");
			mve.PartialViewLocationFormats = partialViewLocationFormats.ToArray();

			List<string> viewLocationFormats = new List<string>(mve.ViewLocationFormats);
			viewLocationFormats.Insert(0, model.Site.SkinViewPath.Replace(Config.ViewName, string.Empty) + "/{0}.cshtml");
			mve.ViewLocationFormats = viewLocationFormats.ToArray();


			string content;

			try
			{
				content = RazorBridge.RenderPartialToString(model.Site.SkinViewPath, model, "SuperFlexi");
			}
			catch (Exception ex)
			{

				log.DebugFormat(
					"chosen layout ({0}) for _SuperFlexiRazor was not found in skin {1} or SuperFlexi Solution. Perhaps it is in a different skin or Solution. \nError was: {2}",
					Config.ViewName,
					SiteUtils.GetSkinBaseUrl(true, Page),
					ex
				);

				try
				{
					content = RazorBridge.RenderPartialToString(viewPath, model, "SuperFlexi");
				}
				catch (Exception ex2)
				{
					renderDefaultView(ex2.ToString());
				}
			}

			void renderDefaultView(string error = "")
			{
				if (!string.IsNullOrWhiteSpace(error))
				{
					log.ErrorFormat(
						"chosen layout ({0}) for _SuperFlexiRazor was not found in skin {1} or SuperFlexi Solution. Perhaps it is in a different skin or Solution. \nError was: {2}",
						Config.ViewName,
						SiteUtils.GetSkinBaseUrl(true, Page),
						error
					);
				}
				content = RazorBridge.RenderPartialToString("_SuperFlexiRazor", model, "SuperFlexi");
			}

			writer.Write(content);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			this.RenderContents(writer);
		}

		//     private Type CreateClass()
		//     {
		//         var className = "_" + Config.FieldDefinitionGuid.ToString("N");
		//         var solutionName = Config.MarkupDefinitionName;

		//         var classCode = $@"
		//                 using System;
		//                 using System.Collections.Generic;
		//                 //using mojoPortal.Web.ModelBinders;
		//                 /// <summary>
		//                 /// Dynamically generated class for {solutionName}
		//                 /// </summary>
		//                 public class {className} {{
		//                     public int Id {{get;set;}}
		//                     public Guid Guid {{get;set;}}
		//                     public int SortOrder {{get;set;}}
		//                     public string EditUrl {{get;set;}}
		//                     public bool IsEditable {{get;set;}}
		//                     {getFields()}                        
		//                 }}";

		//string getFields()
		//{

		//	var sb = new StringBuilder();
		//	var sbConstructor = new StringBuilder();
		//	sbConstructor.AppendLine($"public {className}(){{");

		//	foreach (Field field in fields)
		//	{
		//		switch (field.ControlType)
		//		{
		//			case "CheckBox":
		//				if (field.CheckBoxReturnBool)
		//				{
		//					sb.AppendLine($"public bool {field.Name.Replace(" ", string.Empty)} {{get;set;}}");
		//				}
		//				else
		//				{
		//					goto default;
		//				}
		//				break;
		//			case "CheckBoxList":
		//			case "DynamicCheckBoxList":
		//				sb.AppendLine($"public List<string> {field.Name.Replace(" ", string.Empty)} {{get;set;}}");
		//				sbConstructor.AppendLine(field.Name + " = new List<string>();");
		//				break;
		//			case "DateTime":
		//			case "Date":
		//				sb.AppendLine($"public DateTime {field.Name.Replace(" ", string.Empty)} {{get;set;}}");
		//				sb.AppendLine($"public DateTime {field.Name.Replace(" ", string.Empty)}UTC {{get;set;}}");
		//				break;
		//			case "TextBox":
		//			default:
		//                         if (field.IsDateField()) goto case "Date";

		//				sb.AppendLine($"public string {field.Name.Replace(" ", string.Empty)} {{get;set;}}");
		//				break;
		//		}
		//	}
		//	if (sbConstructor.Length > 1)
		//	{
		//		sbConstructor.AppendLine("}");
		//		sb.AppendLine(sbConstructor.ToString());
		//	}
		//	return sb.ToString();
		//}

		//log.Debug(classCode);
		//         var options = new CompilerParameters
		//         {
		//             GenerateExecutable = false,
		//             GenerateInMemory = true,

		//         };
		//         //options.ReferencedAssemblies.Add(System.Web.Hosting.HostingEnvironment.MapPath("~/bin/System.dll"));
		//         options.ReferencedAssemblies.Add(Reflection.Assembly.GetExecutingAssembly().CodeBase.Substring(8));
		//         //options.ReferencedAssemblies.Add(AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "mojoPortal.Web").FullName);
		//         options.ReferencedAssemblies.Add(Reflection.Assembly.GetAssembly(typeof(Global)).CodeBase.Substring(8));

		//         var provider = new CSharpCodeProvider();
		//         var compile = provider.CompileAssemblyFromSource(options, classCode);

		//         if (compile != null)
		//         {
		//             if (compile.Errors.Count > 0)
		//             {
		//                 log.Error(compile.Errors);
		//             }

		//             return compile.CompiledAssembly.GetType(className);

		//             //return Activator.CreateInstance(type);
		//         }
		//         else
		//         {
		//             log.Error("could not compile");
		//             return null;
		//         }
		//     }

		//public void SetItemClassProperty(object itemObject, string propName, object propValue)
		//{
		//    itemObject.GetType()
		//        .GetProperty(propName, Reflection.BindingFlags.Public | Reflection.BindingFlags.Instance)
		//        .SetValue(itemObject, propValue);
		//}
	}


}
