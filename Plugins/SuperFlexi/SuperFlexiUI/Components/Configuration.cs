using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using Resources;
using SuperFlexiBusiness;

namespace SuperFlexiUI;

public class ModuleConfiguration
{
	private static readonly ILog log = LogManager.GetLogger(typeof(ModuleConfiguration));
	private readonly Module module;
	private readonly int siteId;
	private readonly Hashtable settings;
	//private readonly FileSystemProvider fsProvider;
	private readonly IFileSystem fileSystem;

	#region contstructors
	public ModuleConfiguration()
	{ }

	public ModuleConfiguration(Module module, bool reloadDefinitionFromDisk = false)
	{
		if (module is not null)
		{
			//log.Info($"module {module.ModuleId} has siteid={module.SiteId}");
			if (module.SiteId < 1)
			{
				var m2 = new Module(module.ModuleId);
				if (m2 is not null)
				{
					module = m2;
				}
			}

			this.module = module;
			siteId = module.SiteId;
			FeatureGuid = module.FeatureGuid;
			settings = ModuleSettings.GetModuleSettings(module.ModuleId);

			fileSystem = FileSystemHelper.LoadFileSystem(module.SiteId);

			LoadSettings(settings, reloadDefinitionFromDisk);
		}
	}

	#endregion
	#region public methods
	/// <summary>
	/// Copies markup definition xml to settings. Must instantiate ModuleConfiguration first.
	/// </summary>
	/// <returns>bool</returns>
	public void CopyMarkupDefinitionToDatabase()
	{
		bool changed = false;
		var sfMarkup = fileSystem.RetrieveFile(MarkupDefinitionFile);

		if (sfMarkup is not null)
		{
			var sr = new StreamReader(fileSystem.GetAsStream(sfMarkup.VirtualPath));
			string mdContent = sr.ReadToEnd();
			sr.Close();
			if (mdContent != MarkupDefinitionContent)
			{
				changed = true;
				MarkupDefinitionContent = mdContent;
			}
		}

		if (changed && !ModuleSettings.UpdateModuleSetting(module.ModuleGuid, module.ModuleId, "MarkupDefinitionContent", MarkupDefinitionContent))
		{
			log.Error($"Could not save MarkupDefinitionContent to module settings siteId={module.SiteId}, moduleId={module.ModuleId}, moduleTitle={module.ModuleTitle}");
		}
	}
	#endregion
	#region private methods
	private void LoadSettings(Hashtable settings, bool reloadDefinitionFromDisk = false)
	{
		if (settings is null)
		{
			throw new ArgumentException("must pass in a hashtable of settings");
		}

		ModuleCssClass = WebUtils.ParseStringFromHashtable(settings, "ExtraCssClassSetting", ModuleCssClass).ToString().Trim();
		IsGlobalView = WebUtils.ParseBoolFromHashtable(settings, "IsGlobalView", IsGlobalView);
		IncludeInSearch = WebUtils.ParseBoolFromHashtable(settings, "IncludeInSearch", IncludeInSearch);
		ModuleFriendlyName = WebUtils.ParseStringFromHashtable(settings, "ModuleFriendlyName", ModuleFriendlyName);
		GlobalViewSortOrder = WebUtils.ParseInt32FromHashtable(settings, "GlobalViewSortOrder", GlobalViewSortOrder);
		DescendingSort = WebUtils.ParseBoolFromHashtable(settings, "DescendingSortOrder", DescendingSort);
		InstanceFeaturedImage = WebUtils.ParseStringFromHashtable(settings, "InstanceFeaturedImage", InstanceFeaturedImage);
		HeaderContent = WebUtils.ParseStringFromHashtable(settings, "HeaderContent", HeaderContent);
		FooterContent = WebUtils.ParseStringFromHashtable(settings, "FooterContent", FooterContent);
		CustomizableSettings = WebUtils.ParseStringFromHashtable(settings, "CustomizableSettings", CustomizableSettings);
		ItemEditRoles = WebUtils.ParseStringFromHashtable(settings, "ItemEditRoles", ItemEditRoles);
		ItemCreateRoles = WebUtils.ParseStringFromHashtable(settings, "ItemCreateRoles", ItemCreateRoles);
		ItemDeleteRoles = WebUtils.ParseStringFromHashtable(settings, "ItemDeleteRoles", ItemDeleteRoles);
		PageSize = WebUtils.ParseInt32FromHashtable(settings, "PageSize", PageSize);

		var maxItemsFromModuleSettings = WebUtils.ParseStringFromHashtable(settings, "MaxItems", "ALL").ToUpper();
		if (maxItemsFromModuleSettings != "ALL")
		{
			if (int.TryParse(maxItemsFromModuleSettings, out int maxItems))
			{
				MaxItems = maxItems;
			}
			else
			{
				MaxItems = -1;
			}
		}

		MarkupDefinitionFile = WebUtils.ParseStringFromHashtable(settings, "MarkupDefinitionFile", MarkupDefinitionFile);
		if (MarkupDefinitionFile.IndexOf("~", 0) < 0)
		{
			MarkupDefinitionFile = $"~{MarkupDefinitionFile}";
		}

		if (fileSystem.FileExists(MarkupDefinitionFile))
		{
			WebFile sfMarkupFile = fileSystem.RetrieveFile(MarkupDefinitionFile);
			SolutionLocation = sfMarkupFile.FolderVirtualPath;
		}

		#region MarkupDefinition
		if (settings.Contains("MarkupDefinitionContent"))
		{
			MarkupDefinitionContent = settings["MarkupDefinitionContent"].ToString();
			if (string.IsNullOrWhiteSpace(MarkupDefinitionContent) || AlwaysLoadMarkupDefinitionFromDisk || reloadDefinitionFromDisk)
			{
				CopyMarkupDefinitionToDatabase();
			}

			if (!string.IsNullOrWhiteSpace(MarkupDefinitionContent))
			{
				MapNodes(XmlHelper.GetXmlDocumentFromString(MarkupDefinitionContent));
			}
		}
		else
		{
			//this is for legacy purposes. Superflexi instances created before 12/1/2016 did not have the markupDefinitionContent setting
			//once the settings link is clicked on one of those instances, the instance will get the markupDefinitionContent setting and then
			//this code will not be used anymore.

			// can't use HttpContext because we use this method in SuperFlexiIndexBuilderProvider.RebuildIndex which doesn't have HttpContext
			//string fullPath = HttpContext.Current.Server.MapPath(markupDefinitionFile); 

			if (fileSystem.FileExists(MarkupDefinitionFile))
			{
				var webFile = fileSystem.RetrieveFile(MarkupDefinitionFile);
				MapNodes(XmlHelper.GetXmlDocument(webFile.Path));
			}
		}

		void MapNodes(XmlDocument doc)
		{
			if (doc.DocumentElement.SelectSingleNode("/Definitions/MarkupDefinition") is XmlNode node)
			{
				MapDefinedMarkup(node);
			}

			if (doc.DocumentElement.SelectSingleNode("/Definitions/MobileMarkupDefinition") is XmlNode mobileNode)
			{
				MapDefinedMarkup(mobileNode, true);
			}

			if (doc.DocumentElement.SelectSingleNode("/Definitions/SearchDefinition") is XmlNode searchNode)
			{
				SetupSearchDefinition(searchNode);
			}
		}
		#endregion
	}

	private void SetupSearchDefinition(XmlNode node)
	{
		if (node is not null)
		{
			var attrCollection = node.Attributes;
			FieldDefinitionGuid = attrCollection.ParseGuidFromAttribute("fieldDefinitionGuid", FieldDefinitionGuid);
			if (FieldDefinitionGuid == Guid.Empty)
			{
				return;
			}

			var searchDef = SearchDef.GetByFieldDefinition(FieldDefinitionGuid);
			searchDef ??= new SearchDef
			{
				FieldDefinitionGuid = FieldDefinitionGuid,
				SiteGuid = CacheHelper.GetCurrentSiteSettings().SiteGuid,
				FeatureGuid = FeatureGuid
			};
			bool emptySearchDef = true;
			foreach (XmlNode childNode in node)
			{
				//need to find a way to clear out the searchdef if needed
				switch (childNode.Name)
				{
					case "Title":
						searchDef.Title = childNode.InnerText.Trim();
						emptySearchDef = false;
						break;
					case "Keywords":
						searchDef.Keywords = childNode.InnerText.Trim();
						emptySearchDef = false;
						break;
					case "Description":
						searchDef.Description = childNode.InnerText.Trim();
						emptySearchDef = false;
						break;
					case "Link":
						searchDef.Link = childNode.InnerText.Trim();
						emptySearchDef = false;
						break;
					case "LinkQueryAddendum":
						searchDef.LinkQueryAddendum = childNode.InnerText.Trim();
						emptySearchDef = false;
						break;
				}
			}

			if (!emptySearchDef)
			{
				searchDef.Save();
			}
		}
	}

	private void MapDefinedMarkup(XmlNode node, bool isMobile = false)
	{
		if (node is not null)
		{
			var attrCollection = node.Attributes;
			MarkupDefinitionName = attrCollection.ParseStringFromAttribute("name", MarkupDefinitionName);


			SolutionCssClass = attrCollection.ParseStringFromAttribute("moduleClass", SolutionCssClass).Trim();

			var classes = ModuleCssClass.SplitOnCharAndTrim(' ');
			classes.AddRange(SolutionCssClass.SplitOnCharAndTrim(' '));

			ModuleCssClass = string.Join(" ", classes);

			if (isMobile)
			{
				ModuleMobileCssClass = ModuleCssClass;
			}

			useStandardMarkupOnDesktopOnly = attrCollection.ParseBoolFromAttribute("desktopOnly", useStandardMarkupOnDesktopOnly);
			UseHeader = attrCollection.ParseBoolFromAttribute("useHeader", UseHeader);
			UseFooter = attrCollection.ParseBoolFromAttribute("useFooter", UseFooter);
			AllowImport = attrCollection.ParseBoolFromAttribute("allowImport", AllowImport);
			AllowExport = attrCollection.ParseBoolFromAttribute("allowExport", AllowExport);
			Debug = attrCollection.ParseBoolFromAttribute("debug", Debug);
			GetDynamicListsInRazor = attrCollection.ParseBoolFromAttribute("getDynamicListsInRazor", GetDynamicListsInRazor);
			ItemViewRolesFieldName = attrCollection.ParseStringFromAttribute("itemViewRolesFieldName", ItemViewRolesFieldName);
			ItemEditRolesFieldName = attrCollection.ParseStringFromAttribute("itemEditRolesFieldName", ItemEditRolesFieldName);
			EditPageCssClass += " " + attrCollection.ParseStringFromAttribute("editPageClass", string.Empty);
			EditPageTitle = attrCollection.ParseStringFromAttribute("editPageTitle", EditPageTitle);
			EditPageUpdateButtonText = attrCollection.ParseStringFromAttribute("editPageUpdateButtonText", EditPageUpdateButtonText);
			EditPageSaveButtonText = attrCollection.ParseStringFromAttribute("editPageSaveButtonText", EditPageSaveButtonText);
			EditPageDeleteButtonText = attrCollection.ParseStringFromAttribute("editPageDeleteButtonText", EditPageDeleteButtonText);
			EditPageCancelLinkText = attrCollection.ParseStringFromAttribute("editPageCancelLinkText", EditPageCancelLinkText);
			EditPageDeleteWarning = attrCollection.ParseStringFromAttribute("editPageDeleteWarning", EditPageDeleteWarning);
			ImportPageTitle = attrCollection.ParseStringFromAttribute("importPageTitle", ImportPageTitle);
			ExportPageTitle = attrCollection.ParseStringFromAttribute("exportPageTitle", ExportPageTitle);
			ImportPageCancelLinkText = attrCollection.ParseStringFromAttribute("importPageCancelLinkText", ImportPageCancelLinkText);
			FieldDefinitionSrc = attrCollection.ParseStringFromAttribute("fieldDefinitionSrc", FieldDefinitionSrc).Replace("$_SitePath_$", Invariant($"/Data/Sites/{siteId}"));
			FieldDefinitionGuid = attrCollection.ParseGuidFromAttribute("fieldDefinitionGuid", FieldDefinitionGuid);
			JsonRenderLocation = attrCollection.ParseStringFromAttribute("jsonRenderLocation", JsonRenderLocation);
			JsonLabelObjects = attrCollection.ParseBoolFromAttribute("jsonLabelObjects", JsonLabelObjects);
			HeaderLocation = attrCollection.ParseStringFromAttribute("headerLocation", HeaderLocation);
			FooterLocation = attrCollection.ParseStringFromAttribute("footerLocation", FooterLocation);
			HideOuterWrapperPanel = attrCollection.ParseBoolFromAttribute("hideOuterWrapperPanel", HideOuterWrapperPanel);
			HideInnerWrapperPanel = attrCollection.ParseBoolFromAttribute("hideInnerWrapperPanel", HideInnerWrapperPanel);
			HideOuterBodyPanel = attrCollection.ParseBoolFromAttribute("hideOuterBodyPanel", HideOuterBodyPanel);
			HideInnerBodyPanel = attrCollection.ParseBoolFromAttribute("hideInnerBodyPanel", HideInnerBodyPanel);
			ShowSaveAsNew = attrCollection.ParseBoolFromAttribute("showSaveAsNewButton", ShowSaveAsNew);
			MaxItems = attrCollection.ParseInt32FromAttribute("maxItems", MaxItems);
			ProcessItems = attrCollection.ParseBoolFromAttribute("processItems", ProcessItems);

			ViewName = attrCollection.ParseStringFromAttribute("viewName", ViewName);
			UseRazor = !string.IsNullOrWhiteSpace(ViewName);

			var workingMarkupDefinition = new MarkupDefinition();
			if (isMobile && !useStandardMarkupOnDesktopOnly)
			{
				// do this so mobile settings are added to desktop
				workingMarkupDefinition = (MarkupDefinition)MarkupDefinition.Clone();
			}

			foreach (XmlNode childNode in node)
			{
				if (!string.IsNullOrWhiteSpace(childNode.InnerText) || !string.IsNullOrWhiteSpace(childNode.InnerXml))
				{
					switch (childNode.Name)
					{
						case "ModuleTitleMarkup":
							workingMarkupDefinition.ModuleTitleMarkup = childNode.InnerText.Trim();
							break;
						case "ModuleTitleFormat":
							workingMarkupDefinition.ModuleTitleFormat = childNode.InnerText.Trim();
							break;
						case "ModuleLinksFormat":
							workingMarkupDefinition.ModuleLinksFormat = childNode.InnerText.Trim();
							break;
						case "ModuleInstanceMarkupTop":
							workingMarkupDefinition.ModuleInstanceMarkupTop = childNode.InnerText.Trim();
							break;
						case "ModuleInstanceMarkupBottom":
							workingMarkupDefinition.ModuleInstanceMarkupBottom = childNode.InnerText.Trim();
							break;
						case "InstanceFeaturedImageFormat":
							workingMarkupDefinition.InstanceFeaturedImageFormat = childNode.InnerText.Trim();
							break;
						case "HeaderContentFormat":
							workingMarkupDefinition.HeaderContentFormat = childNode.InnerText.Trim();
							break;
						case "FooterContentFormat":
							workingMarkupDefinition.FooterContentFormat = childNode.InnerText.Trim();
							break;
						case "ItemMarkup":
							workingMarkupDefinition.ItemMarkup = childNode.InnerText.Trim();
							break;
						case "ItemsRepeaterMarkup":
							workingMarkupDefinition.ItemsRepeaterMarkup = childNode.InnerText.Trim();
							XmlAttributeCollection repeaterAttribs = childNode.Attributes;
							workingMarkupDefinition.ItemsPerGroup = repeaterAttribs.ParseInt32FromAttribute("itemsPerGroup", workingMarkupDefinition.ItemsPerGroup);

							break;
						case "ItemsWrapperFormat":
							workingMarkupDefinition.ItemsWrapperFormat = childNode.InnerText.Trim();
							break;
						case "ModuleSettingsLinkFormat":
							workingMarkupDefinition.ModuleSettingsLinkFormat = childNode.InnerText.Trim();
							break;
						case "AddItemLinkFormat":
							workingMarkupDefinition.AddItemLinkFormat = childNode.InnerText.Trim();
							break;
						case "EditHeaderLinkFormat":
							workingMarkupDefinition.EditHeaderLinkFormat = childNode.InnerText.Trim();
							break;
						case "EditFooterLinkFormat":
							workingMarkupDefinition.EditFooterLinkFormat = childNode.InnerText.Trim();
							break;
						case "ItemEditLinkFormat":
							workingMarkupDefinition.ItemEditLinkFormat = childNode.InnerText.Trim();
							break;
						case "ImportInstructions":
							ImportInstructions = childNode.InnerText.Trim();
							break;
						case "ExportInstructions":
							ExportInstructions = childNode.InnerText.Trim();
							break;
						case "ImportLinkFormat":
							workingMarkupDefinition.ImportLinkFormat = childNode.InnerText.Trim();
							break;
						case "ExportLinkFormat":
							workingMarkupDefinition.ExportLinkFormat = childNode.InnerText.Trim();
							break;
						case "GlobalViewMarkup":
							workingMarkupDefinition.GlobalViewMarkup = childNode.InnerText.Trim();
							break;
						case "GlobalViewItemMarkup":
							workingMarkupDefinition.GlobalViewItemMarkup = childNode.InnerText.Trim();
							break;
						case "CheckBoxListMarkup":
						case "RadioButtonListMarkup":
							var cblm = new CheckBoxListMarkup();

							var cblmAttribs = childNode.Attributes;
							if (cblmAttribs["field"] is not null)
							{
								cblm.Field = cblmAttribs["field"].Value;
							}

							if (cblmAttribs["token"] is not null)
							{
								cblm.Token = cblmAttribs["token"].Value;
							}

							foreach (XmlNode cblmNode in childNode)
							{
								switch (cblmNode.Name)
								{
									case "Separator":
										cblm.Separator = cblmNode.InnerText.Trim();
										break;
									case "Content":
										cblm.Markup = cblmNode.InnerText.Trim();
										break;
								}
							}
							CheckBoxListMarkups.Add(cblm);
							break;
						case "Scripts":
							if (isMobile)
							{
								MobileMarkupScripts = SuperFlexiHelpers.ParseScriptsFromXmlNode(childNode);
							}
							else
							{
								MarkupScripts = SuperFlexiHelpers.ParseScriptsFromXmlNode(childNode);
							}
							break;
						case "Styles":
							MarkupCSS = SuperFlexiHelpers.ParseCssFromXmlNode(childNode);
							break;
					}
				}
			}

			if (isMobile)
			{
				MobileMarkupDefinition = workingMarkupDefinition;
			}
			else
			{
				MarkupDefinition = workingMarkupDefinition;
			}
		}
	}

	#endregion private methods

	#region properties
	public Guid FeatureGuid { get; private set; } = Guid.Parse("4FF93793-1187-4022-899C-C3E9096A855F");
	public bool DeleteOrphanedFieldValues => mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("SuperFlexi:DeleteOrphanedFieldValues", false);
	public bool AlwaysLoadMarkupDefinitionFromDisk => mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("SuperFlexi:AlwaysLoadMarkupDefinitionFromDisk", false);

	private bool useStandardMarkupOnDesktopOnly = false;
	public string FieldDefinitionSrc { get; private set; } = string.Empty;
	public Guid FieldDefinitionGuid { get; private set; }

	/// <summary>
	/// Where the json should be rendered.
	/// Valid options are: 
	///     inHead               
	///     inBody (register script) (default)      
	///     aboveMarkupDefinition               
	///     belowMarkupDefinition               
	///     bottomStartup(register startup script) 
	/// </summary>
	public string JsonRenderLocation { get; private set; } = string.Empty;
	public bool RenderJSONOfData => ProcessItems && !string.IsNullOrWhiteSpace(JsonRenderLocation);
	public bool JsonLabelObjects { get; private set; } = false;
	public bool DescendingSort { get; private set; } = false;
	public bool UseHeader { get; private set; } = false;
	/// <summary>
	/// Where the header content should be rendered.
	/// Valid options are:
	///     InnerBodyPanel (default)
	///     OuterBodyPanel
	///     InnerWrapperPanel
	///     OuterWrapperPanel
	///     Outside
	/// </summary>
	public string HeaderLocation { get; private set; } = "InnerBodyPanel";
	public string HeaderContent { get; private set; } = string.Empty;
	public bool UseFooter { get; private set; } = false;
	/// <summary>
	/// Where the header content should be rendered.
	/// Valid options are:
	///     InnerBodyPanel (default)
	///     OuterBodyPanel
	///     InnerWrapperPanel
	///     OuterWrapperPanel
	///     Outside
	/// </summary>
	public string FooterLocation { get; private set; } = "InnerBodyPanel";
	public string FooterContent { get; private set; } = string.Empty;
	public bool HideOuterWrapperPanel { get; private set; } = false;
	public bool HideInnerWrapperPanel { get; private set; } = false;
	public bool HideOuterBodyPanel { get; private set; } = false;
	public bool HideInnerBodyPanel { get; private set; } = false;
	public string InstanceFeaturedImage { get; private set; } = string.Empty;
	public string FeaturedImageEmptyUrl => $"{WebUtils.GetRelativeSiteRoot()}/Data/SiteImages/1x1.gif";
	public string SolutionLocation { get; private set; } = string.Empty;
	public Uri SolutionLocationUrl => new(WebUtils.ResolveServerUrl($"{SolutionLocation.Replace(System.Web.Hosting.HostingEnvironment.MapPath("~"), "~/")}/"));
	public string RelativeSolutionLocation => SolutionLocation.Replace(System.Web.Hosting.HostingEnvironment.MapPath("~"), "~/");
	public string MarkupDefinitionFile { get; private set; } = string.Empty;
	public MarkupDefinition MarkupDefinition { get; private set; } = null;
	public string MarkupDefinitionContent { get; private set; } = string.Empty;
	public string MarkupDefinitionName { get; private set; } = string.Empty;
	public MarkupDefinition MobileMarkupDefinition { get; private set; } = null;
	public List<MarkupScript> MobileMarkupScripts { get; private set; } = [];
	public List<MarkupScript> MarkupScripts { get; private set; } = [];
	public List<MarkupScript> EditPageScripts { get; set; } = [];
	public List<MarkupCss> MarkupCSS { get; private set; } = [];
	public List<MarkupCss> EditPageCSS { get; set; } = [];
	public List<CheckBoxListMarkup> CheckBoxListMarkups { get; } = [];
	public string AddItemText { get; private set; } = SuperFlexiResources.AddItem;
	public string SolutionCssClass { get; private set; } = string.Empty;
	public string ModuleCssClass { get; private set; } = string.Empty;
	public string ModuleMobileCssClass { get; private set; } = string.Empty;
	public string EditPageCssClass { get; private set; } = string.Empty;
	public string EditPageTitle { get; private set; } = SuperFlexiResources.EditItemTitle;
	public string EditPageUpdateButtonText { get; private set; } = SuperFlexiResources.UpdateButton;
	public string EditPageSaveButtonText { get; private set; } = SuperFlexiResources.UpdateButton;
	public string EditPageDeleteButtonText { get; private set; } = SuperFlexiResources.DeleteButton;
	public string EditPageCancelLinkText { get; private set; } = SuperFlexiResources.CancelButton;
	public string EditPageDeleteWarning { get; private set; } = SuperFlexiResources.ItemDeleteWarning;
	public string ImportPageTitle { get; private set; } = SuperFlexiResources.ImportTitle;
	public string ImportPageCancelLinkText { get; private set; } = SuperFlexiResources.CancelButton;
	public string ImportInstructions { get; private set; } = SuperFlexiResources.ImportInstructions;
	public string ExportInstructions { get; private set; } = SuperFlexiResources.ExportInstructions;
	public string ExportPageTitle { get; private set; } = SuperFlexiResources.ExportTitle;
	public bool AllowImport { get; private set; } = false;
	public bool AllowExport { get; private set; } = false;
	public bool Debug { get; private set; } = false;
	public bool ShowSaveAsNew { get; private set; } = true;
	public int MaxItems { get; private set; } = -1;
	public string CustomizableSettings { get; private set; } = string.Empty;
	public string ModuleFriendlyName { get; private set; } = string.Empty;
	public int GlobalViewSortOrder { get; private set; } = 0;
	public bool IsGlobalView { get; private set; } = false;
	public bool IncludeInSearch { get; private set; } = false;
	public SearchDef SearchDefinition { get; set; } = [];
	public string ItemEditRoles { get; private set; } = string.Empty;
	public string ItemCreateRoles { get; private set; } = string.Empty;
	public string ItemDeleteRoles { get; private set; } = string.Empty;
	public bool UseRazor { get; private set; } = false;
	public string ViewName { get; set; }
	/// <summary>
	/// set in the markup definition. 
	/// used when populating data via api
	/// </summary>
	public bool ProcessItems { get; private set; } = true;
	public string ItemViewRolesFieldName { get; set; } = string.Empty;
	public string ItemEditRolesFieldName { get; set; } = string.Empty;
	public int PageSize { get; set; } = 0;
	public bool GetDynamicListsInRazor = false;
	#endregion
}

public class MarkupScript
{
	public string Url { get; set; } = string.Empty;
	public string RawScript { get; set; } = string.Empty;
	public string ScriptName { get; set; } = string.Empty;
	/// <summary>
	///		Position of rendered script.
	///		<para>inHead</para>
	///		<para>inBody (register script) (default)</para>
	///		<para>aboveMarkupDefinition</para>
	///		<para>belowMarkupDefinition</para>        
	///		<para>bottomStartup(register startup script)</para>
	///		<para>inHead</para>
	/// </summary>
	public string Position { get; set; } = "inBody";
}

public class MarkupCss
{
	public string Url { get; set; } = string.Empty;
	public string CSS { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string Media { get; set; } = string.Empty;
	public bool RenderAboveSSC { get; set; } = true;
}


public class CheckBoxListMarkup
{
	public string Markup { get; set; } = string.Empty;
	public string Field { get; set; } = string.Empty;
	public string Token { get; set; } = string.Empty;
	public List<SelectedValue> SelectedValues { get; set; } = [];
	public string Separator { get; set; } = string.Empty;

	public class SelectedValue
	{
		public string Value { get; set; } = string.Empty;
		public int ItemID { get; set; } = -1;
		public int Count { get; set; } = 1;
		public SelectedValue selectedValue;
	}
}