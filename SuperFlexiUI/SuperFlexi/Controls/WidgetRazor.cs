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

		private int pageNumber = 1;
		private int pageSize = 0;
		private int totalPages = -1;
		private int totalRows = -1;
		private int itemId = 0;
		private bool getDynamicListValuesFromReturnedItems = false;
		private bool publishedToCurrentPage = false;

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
			module = new Module(ModuleId, PageId);
			if (module == null)
			{
				publishedToCurrentPage = false; 
				module = new Module(ModuleId);
				if (module == null)
				{
					return;
				}
			}
			else
			{
				publishedToCurrentPage = true;
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
			}
			else if (Config.ProcessItems)
			{
				itemsWithValues.AddRange(GetItemsWithValues());
				//getDynamicListValuesFromReturnedItems = true;
			}

			if (Config.GetDynamicListsInRazor)
			{
				dynamicLists = Field.GetAllForDefinition(Config.FieldDefinitionGuid).Where(f => f.IsDynamicListField()).ToList();

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

			//var pageModules = PageModule.GetPageModulesByModule(module.ModuleId);
			//if (pageModules.Where(pm => pm.PageId == CurrentPage.PageId).ToList().Count() == 0)
			//{
			//	publishedToCurrentPage = false;
			//}

			var superFlexiItemClass = new ClassBuilder(itemsWithValues)
			{
				IsEditable = IsEditable,
				PageId = PageId
			}.Init();

			//var itemModels = new List<object> ();

			WidgetModel model = new()
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
				Page = new PageModel
				{
					Id = CurrentPage.PageId,
					Url = CurrentPage.Url,
					FullUrl = SiteUtils.GetNavigationSiteRoot() + CurrentPage.Url.Replace("~/","/"),
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

			foreach (var dv in dynamicListValues)
			{

				var values = dynamicListValues.Where(list => list.FieldName == dv.FieldName).Select(v => v.FieldValue).ToList();
				model.DynamicLists.Add(dv.FieldName, values);
			}

			model.Items = superFlexiItemClass.Items;

			var viewPath = Config.RelativeSolutionLocation + "/" + Config.ViewName;

			mojoViewEngine mve = new();

			model.Site.SkinViewPath = model.Site.SkinPath + "Views/" + Config.RelativeSolutionLocation.Replace("~/Data/", string.Empty).Replace("sites/" + model.Site.Id, string.Empty) + "/" + Config.ViewName;

			List<string> masterLocationFormats = new(mve.MasterLocationFormats);
			masterLocationFormats.Insert(0, "~/Data/Sites/$SiteId$/skins/$Skin$/Views/SuperFlexi/{0}.cshtml");
			mve.MasterLocationFormats = masterLocationFormats.ToArray();

			List<string> partialViewLocationFormats = new(mve.PartialViewLocationFormats);
			partialViewLocationFormats.Insert(0, model.Site.SkinViewPath.Replace(Config.ViewName, string.Empty) + "/{0}.cshtml");
			mve.PartialViewLocationFormats = partialViewLocationFormats.ToArray();

			List<string> viewLocationFormats = new(mve.ViewLocationFormats);
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
	}
}
