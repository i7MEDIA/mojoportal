using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using SuperFlexiBusiness;
using SuperFlexiUI.Models;

namespace SuperFlexiUI;

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
	private readonly List<Item> items = [];
	private readonly List<Field> fields = [];
	private List<Field> dynamicLists = [];
	private readonly List<ItemFieldValue> dynamicListValues = [];
	private readonly Dictionary<string, string> dynamicQueryParams = [];
	private List<ItemWithValues> itemsWithValues = [];
	private SiteSettings siteSettings;
	private Module module;
	private WidgetModel model;

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

		if (module is null || module.ModuleId == -1)
		{
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
		if (module.ModuleId == -1)
		{
			module = new Module(ModuleId);
			if (module.ModuleId == -1)
			{
				return;
			}
			publishedToCurrentPage = false;
		}
		else if (module is not null)
		{
			publishedToCurrentPage = true;
		}

		pageNumber = WebUtils.ParseInt32FromQueryString($"sf{ModuleId}_PageNumber", pageNumber);
		pageSize = WebUtils.ParseInt32FromQueryString($"sf{ModuleId}_PageSize", Config.PageSize);
		itemId = WebUtils.ParseInt32FromQueryString($"sf{ModuleId}_ItemId", itemId);

		foreach (string param in HttpContext.Current.Request.QueryString.Keys)
		{
			//the keys (param) can be null because some dev somewhere is an asshole
			if (param is not null && param.StartsWith($"sf{ModuleId}")
				&& param != $"sf{ModuleId}_PageNumber"
				&& param != $"sf{ModuleId}_PageSize"
				&& param != $"sf{ModuleId}_ItemId")
			{
				dynamicQueryParams.Add(param, HttpContext.Current.Request.QueryString[param]);
			}
		}

		siteSettings = CacheHelper.GetCurrentSiteSettings();

		if (CurrentPage is null)
		{
			CurrentPage = CacheHelper.GetCurrentPage();
			if (CurrentPage is null)
			{
				log.Debug("Can't use CacheHelper.GetCurrentPage() here.");
				CurrentPage = new PageSettings(siteSettings.SiteId, PageId);
			}
		}

		timeZone = SiteUtils.GetUserTimeZone();

		if (Config.MarkupDefinition is not null)
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

		if (SiteUtils.IsMobileDevice() && Config.MobileMarkupDefinition is not null)
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
						if (set.Key.ToLower() == $"sf{ModuleId}")
						{
							itemsWithValues.AddRange(GetItemsWithValues(set.Value));
						}
						else
						{
							itemsWithValues.AddRange(GetItemsWithValues(setValue, set.Key));
						}
					}
				}
				else if (set.Key.ToLower() == $"sf{ModuleId}")
				{
					itemsWithValues.AddRange(GetItemsWithValues(set.Value));
				}
				else
				{
					itemsWithValues.AddRange(GetItemsWithValues(set.Value, set.Key));
				}
			}

			itemsWithValues = itemsWithValues.Distinct(new SimpleItemWithValuesComparer()).ToList();
		}
		else if (Config.ProcessItems)
		{
			itemsWithValues.AddRange(GetItemsWithValues());
		}

		if (Config.GetDynamicListsInRazor)
		{
			dynamicLists = Field.GetAllForDefinition(Config.FieldDefinitionGuid).Where(f => f.IsDynamicListField).ToList();

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
		prepareModel();	
		writer.Write(getViewContent());
	}

	private void prepareModel()
	{
		string featuredImageUrl = string.Empty;

		featuredImageUrl = string.IsNullOrWhiteSpace(Config.InstanceFeaturedImage) ? featuredImageUrl : SiteUtils.GetNavigationSiteRoot() + Config.InstanceFeaturedImage;

		var superFlexiItemClass = new ClassBuilder(itemsWithValues)
		{
			IsEditable = IsEditable,
			PageId = PageId
		}.Init();

		model = new WidgetModel()
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
				FullUrl = SiteUtils.GetNavigationSiteRoot() + CurrentPage.Url.Replace("~/", "/"),
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
				SkinPath = SiteUtils.DetermineSkinBaseUrl(SiteUtils.GetSkinName(true)),
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
	}

	private string getViewContent()
	{
		var viewPath = $"{Config.RelativeSolutionLocation}/{Config.ViewName}";

		var viewEngine = new mojoViewEngine();

		var solutionDirectory = new DirectoryInfo(Config.RelativeSolutionLocation);
		model.Site.SkinViewPath = $"{model.Site.SkinPath}Views/SuperFlexi/{solutionDirectory.Name}/{Config.ViewName}";

		var masterLocationFormats = new List<string>(viewEngine.MasterLocationFormats);
		masterLocationFormats.Insert(0, $"~{model.Site.SkinPath}Views/SuperFlexi/{{0}}.cshtml");
		viewEngine.MasterLocationFormats = masterLocationFormats.ToArray();

		var partialViewLocationFormats = new List<string>(viewEngine.PartialViewLocationFormats);
		partialViewLocationFormats.Insert(0, model.Site.SkinViewPath.Replace(Config.ViewName, string.Empty) + "/{0}.cshtml");
		viewEngine.PartialViewLocationFormats = partialViewLocationFormats.ToArray();

		var viewLocationFormats = new List<string>(viewEngine.ViewLocationFormats);
		viewLocationFormats.Insert(0, model.Site.SkinViewPath.Replace(Config.ViewName, string.Empty) + "/{0}.cshtml");
		viewEngine.ViewLocationFormats = viewLocationFormats.ToArray();

		try
		{
			return RazorBridge.RenderPartialToString(model.Site.SkinViewPath, model, "SuperFlexi");
		}
		catch (Exception ex)
		{
			log.Debug($"chosen layout ({Config.ViewName}) for ({Config.RelativeSolutionLocation}) was not found in skin {model.Site.SkinPath}. Perhaps it is in a different skin. \nError was: {ex}");

			try
			{
				return RazorBridge.RenderPartialToString(viewPath, model, "SuperFlexi");
			}
			catch (Exception ex2)
			{
				log.Error($"chosen layout ({Config.ViewName}) was not found in skin ({model.Site.SkinPath}) or SuperFlexi Solution ({Config.RelativeSolutionLocation}). Perhaps it is in a different skin or Solution. \nError was: {ex2}");
				return RazorBridge.RenderPartialToString("_SuperFlexiRazor", model, "SuperFlexi");
			}
		}
	}

	protected override void Render(HtmlTextWriter writer)
	{
		this.RenderContents(writer);
	}
}