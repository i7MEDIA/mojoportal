// Author:				    i7MEDIA (joe davis)
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Core = mojoPortal.Core;
using mojoPortal.Web.Framework;
using Resources;
using SuperFlexiBusiness;
using mojoPortal.FileSystem;
using mojoPortal.Web;
using mojoPortal.Core.Helpers;

namespace SuperFlexiUI
{
	public class ModuleConfiguration
	{

		private static readonly ILog log = LogManager.GetLogger(typeof(ModuleConfiguration));
		private Module module;
		private Hashtable settings;
		//private SiteSettings siteSettings;
		private int siteId = -1;

		FileSystemProvider fsProvider;
		IFileSystem fileSystem;

		#region contstructors
		public ModuleConfiguration()
		{ }

		public ModuleConfiguration(Module module, bool reloadDefinitionFromDisk = false)
		{
			if (module != null)
			{
				//log.Info($"module {module.ModuleId} has siteid={module.SiteId}");
				if (module.SiteId < 1)
				{
					Module m2 = new Module(module.ModuleId);
					if (m2 != null)
					{
						module = m2;
					}
				}

				this.module = module;
				this.siteId = module.SiteId;
				featureGuid = module.FeatureGuid;
				settings = ModuleSettings.GetModuleSettings(module.ModuleId);

				//if (siteId < 1)
				//{
				//	if (siteSettings == null)
				//	{
				//		siteSettings = CacheHelper.GetCurrentSiteSettings();
				//	}
				//	siteId = siteSettings.SiteId;
				//}
				try
				{
					fsProvider = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
					if (fsProvider == null)
					{
						log.Error("File System Provider Could Not Be Loaded.");
						return;
					}
					fileSystem = fsProvider.GetFileSystem(siteId);
					if (fileSystem == null)
					{
						log.Error("File System Could Not Be Loaded.");
						return;
					}
				}
				catch (TypeInitializationException ex)
				{
					log.Error(ex);
					return;
				}
				catch (NullReferenceException ex)
				{
					log.Error(ex);
					return;
				}
				LoadSettings(settings, reloadDefinitionFromDisk);
			}
		}

		//public ModuleConfiguration(Module module, int siteId, bool reloadDefinitionFromDisk = false)
		//{
		//	siteSettings = new SiteSettings(siteId);
		//}
		#endregion
		#region public methods
		/// <summary>
		/// Copies markup definition xml to settings. Must instantiate ModuleConfiguration first.
		/// </summary>
		/// <returns>bool</returns>
		public void CopyMarkupDefinitionToDatabase()
		{

			string virtualPath = fileSystem.VirtualRoot;

			string mdContent = string.Empty;
			bool changed = false;
			//if (settings.Contains("MarkupDefinitionFile"))
			//{
			//    markupDefinitionFile = settings["MarkupDefinitionFile"].ToString();
			//    if (markupDefinitionFile.IndexOf("~", 0) < 0) markupDefinitionFile = "~" + markupDefinitionFile;

			// can't use HttpContext because we use this method in SuperFlexiIndexBuilderProvider.RebuildIndex which doesn't have HttpContext
			//string fullPath = HttpContext.Current.Server.MapPath(markupDefinitionFile); 
			//string fullPath = System.Web.Hosting.HostingEnvironment.MapPath(markupDefinitionFile);
			var sfMarkup = fileSystem.RetrieveFile(markupDefinitionFile);

			if (sfMarkup != null)
			{
				StreamReader sr = new StreamReader(fileSystem.GetAsStream(sfMarkup.VirtualPath));
				mdContent = sr.ReadToEnd();
				sr.Close();
				if (mdContent != markupDefinitionContent)
				{
					changed = true;
					markupDefinitionContent = mdContent;
				}
			}
			//}

			if (changed && !ModuleSettings.UpdateModuleSetting(module.ModuleGuid, module.ModuleId, "MarkupDefinitionContent", markupDefinitionContent))
			{
				log.ErrorFormat(@"
					Could not save MarkupDefinitionContent to module settings
					siteId={0}
					moduleId={1}
					moduleTitle={2}
					",
					module.SiteId, module.ModuleId, module.ModuleTitle);
			}
		}
		#endregion
		#region private methods
		private void LoadSettings(Hashtable settings, bool reloadDefinitionFromDisk = false)
		{
			if (settings == null)
			{ throw new ArgumentException("must pass in a hashtable of settings"); }

			if (settings.Contains("ExtraCssClassSetting"))
			{
				ModuleCssClass = settings["ExtraCssClassSetting"].ToString().Trim();
			}

			isGlobalView = WebUtils.ParseBoolFromHashtable(settings, "IsGlobalView", isGlobalView);

			includeInSearch = WebUtils.ParseBoolFromHashtable(settings, "IncludeInSearch", includeInSearch);

			if (settings.Contains("ModuleFriendlyName"))
			{
				moduleFriendlyName = settings["ModuleFriendlyName"].ToString();
			}

			globalViewSortOrder = WebUtils.ParseInt32FromHashtable(settings, "GlobalViewSortOrder", globalViewSortOrder);


			//useFooter = WebUtils.ParseBoolFromHashtable(settings, "UseFooter", useFooter);
			descendingSort = WebUtils.ParseBoolFromHashtable(settings, "DescendingSortOrder", descendingSort);

			if (settings.Contains("InstanceFeaturedImage"))
			{
				instanceFeaturedImage = settings["InstanceFeaturedImage"].ToString();
			}

			if (settings.Contains("HeaderContent"))
			{
				headerContent = settings["HeaderContent"].ToString();
			}

			if (settings.Contains("FooterContent"))
			{
				footerContent = settings["FooterContent"].ToString();
			}

			if (settings.Contains("CustomizableSettings"))
			{
				customizableSettings = settings["CustomizableSettings"].ToString();
			}

			if (settings.Contains("ItemEditRoles"))
			{
				itemEditRoles = settings["ItemEditRoles"].ToString();
			}

			if (settings.Contains("ItemCreateRoles"))
			{
				itemCreateRoles = settings["ItemCreateRoles"].ToString();
			}

			if (settings.Contains("ItemDeleteRoles"))
			{
				itemDeleteRoles = settings["ItemDeleteRoles"].ToString();
			}

			if (settings.Contains("MaxItems"))
			{
				var maxItemsFromModuleSettings = settings["MaxItems"].ToString();

				if (maxItemsFromModuleSettings.ToUpper() != "ALL")
				{
					if (!int.TryParse(maxItemsFromModuleSettings, out maxItems))
					{
						maxItems = -1;
					}
				}
			}

			PageSize = WebUtils.ParseInt32FromHashtable(settings, "PageSize", PageSize);

			maxItems = WebUtils.ParseInt32FromHashtable(settings, "MaxItems", maxItems);

			if (settings.Contains("MarkupDefinitionFile"))
			{
				markupDefinitionFile = settings["MarkupDefinitionFile"].ToString();
				if (markupDefinitionFile.IndexOf("~", 0) < 0)
					markupDefinitionFile = "~" + markupDefinitionFile;
			}
			if (fileSystem != null)
			{
				if (fileSystem.FileExists(markupDefinitionFile))
				{
					WebFile sfMarkupFile = fileSystem.RetrieveFile(markupDefinitionFile);
					//FileInfo sfMarkupFile = new FileInfo(System.Web.Hosting.HostingEnvironment.MapPath(markupDefinitionFile));

					solutionLocation = sfMarkupFile.FolderVirtualPath;
				}
			}
			//useRazor = WebUtils.ParseBoolFromHashtable(settings, "UseRazor", useRazor);
			#region MarkupDefinition
			if (settings.Contains("MarkupDefinitionContent"))
			{
				markupDefinitionContent = settings["MarkupDefinitionContent"].ToString();
				if (string.IsNullOrWhiteSpace(markupDefinitionContent) || AlwaysLoadMarkupDefinitionFromDisk || reloadDefinitionFromDisk)
				{
					CopyMarkupDefinitionToDatabase();
				}

				if (!string.IsNullOrWhiteSpace(markupDefinitionContent))
				{
					var doc = Core.Helpers.XmlHelper.GetXmlDocumentFromString(markupDefinitionContent);

					XmlNode node = doc.DocumentElement.SelectSingleNode("/Definitions/MarkupDefinition");
					if (node != null)
						MapDefinedMarkup(node);

					XmlNode mobileNode = doc.DocumentElement.SelectSingleNode("/Definitions/MobileMarkupDefinition");
					if (mobileNode != null)
						MapDefinedMarkup(mobileNode, true);

					XmlNode searchNode = doc.DocumentElement.SelectSingleNode("/Definitions/SearchDefinition");
					if (searchNode != null)
						SetupSearchDefinition(searchNode);
				}


			}
			else
			{
				//this is for legacy purposes. Superflexi instances created before 12/1/2016 did not have the markupDefinitionContent setting
				//once the settings link is clicked on one of those instances, the instance will get the markupDefinitionContent setting and then
				//this code will not be used anymore.




				// can't use HttpContext because we use this method in SuperFlexiIndexBuilderProvider.RebuildIndex which doesn't have HttpContext
				//string fullPath = HttpContext.Current.Server.MapPath(markupDefinitionFile); 

				//string fullPath = System.Web.Hosting.HostingEnvironment.MapPath(markupDefinitionFile);
				if (fileSystem != null && fileSystem.FileExists(markupDefinitionFile))
				{
					//FileInfo fileInfo = new FileInfo(fullPath);

					WebFile webFile = fileSystem.RetrieveFile(markupDefinitionFile);

					var doc = Core.Helpers.XmlHelper.GetXmlDocument(webFile.Path);

					XmlNode node = doc.DocumentElement.SelectSingleNode("/Definitions/MarkupDefinition");
					if (node != null)
						MapDefinedMarkup(node);

					XmlNode mobileNode = doc.DocumentElement.SelectSingleNode("/Definitions/MobileMarkupDefinition");
					if (mobileNode != null)
						MapDefinedMarkup(mobileNode, true);

					XmlNode searchNode = doc.DocumentElement.SelectSingleNode("/Definitions/SearchDefinition");
					if (searchNode != null)
						SetupSearchDefinition(searchNode);
				}
			}
			#endregion
		}



		private void SetupSearchDefinition(XmlNode node)
		{
			if (node != null)
			{
				XmlAttributeCollection attrCollection = node.Attributes;
				if (attrCollection["fieldDefinitionGuid"] != null)
					fieldDefinitionGuid = Guid.Parse(attrCollection["fieldDefinitionGuid"].Value);
				if (fieldDefinitionGuid == Guid.Empty)
					return;
				SearchDef searchDef = SearchDef.GetByFieldDefinition(fieldDefinitionGuid);
				if (searchDef == null)
				{
					searchDef = new SearchDef();
					searchDef.FieldDefinitionGuid = fieldDefinitionGuid;
					searchDef.SiteGuid = CacheHelper.GetCurrentSiteSettings().SiteGuid;
					searchDef.FeatureGuid = FeatureGuid;
				}
				bool emptySearchDef = true;
				foreach (XmlNode childNode in node)
				{
					//if (!String.IsNullOrWhiteSpace(childNode.InnerText) || !String.IsNullOrWhiteSpace(childNode.InnerXml))
					//{

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

					//}
				}
				if (!emptySearchDef)
					searchDef.Save();
			}
		}

		private void MapDefinedMarkup(XmlNode node, bool isMobile = false)
		{
			if (node != null)
			{
				//bool desktopOnly = false;
				XmlAttributeCollection attrCollection = node.Attributes;

				if (attrCollection["name"] != null)
					markupDefinitionName = attrCollection["name"].Value;

				if (attrCollection["moduleClass"] != null)
				{

					SolutionCssClass = attrCollection["moduleClass"].Value.Trim();

					var classes = ModuleCssClass.SplitOnCharAndTrim(' ');
					classes.AddRange(SolutionCssClass.SplitOnCharAndTrim(' '));

					ModuleCssClass = string.Join(" ", classes);

					if (isMobile)
					{
						ModuleMobileCssClass = ModuleCssClass;
					}

				}
				useStandardMarkupOnDesktopOnly = XmlUtils.ParseBoolFromAttribute(attrCollection, "desktopOnly", useStandardMarkupOnDesktopOnly);
				useHeader = XmlUtils.ParseBoolFromAttribute(attrCollection, "useHeader", useHeader);
				useFooter = XmlUtils.ParseBoolFromAttribute(attrCollection, "useFooter", useFooter);
				allowImport = XmlUtils.ParseBoolFromAttribute(attrCollection, "allowImport", allowImport);
				allowExport = XmlUtils.ParseBoolFromAttribute(attrCollection, "allowExport", allowExport);
				Debug = XmlUtils.ParseBoolFromAttribute(attrCollection, "debug", Debug);
				GetDynamicListsInRazor = XmlUtils.ParseBoolFromAttribute(attrCollection, "getDynamicListsInRazor", GetDynamicListsInRazor);

				if (attrCollection["itemViewRolesFieldName"] != null)
					ItemViewRolesFieldName = attrCollection["itemViewRolesFieldName"].Value;
				if (attrCollection["itemEditRolesFieldName"] != null)
					ItemEditRolesFieldName = attrCollection["itemEditRolesFieldName"].Value;
				//renderModuleLinksWithModuleTitle = XmlUtils.ParseBoolFromAttribute(attrCollection, "renderModuleLinksWithModuleTitle", renderModuleLinksWithModuleTitle);

				if (attrCollection["editPageClass"] != null)
					editPageCssClass += " " + attrCollection["editPageClass"].Value;
				if (attrCollection["editPageTitle"] != null)
					editPageTitle = attrCollection["editPageTitle"].Value;
				if (attrCollection["editPageUpdateButtonText"] != null)
					editPageUpdateButtonText = attrCollection["editPageUpdateButtonText"].Value;
				if (attrCollection["editPageSaveButtonText"] != null)
					editPageSaveButtonText = attrCollection["editPageSaveButtonText"].Value;
				if (attrCollection["editPageDeleteButtonText"] != null)
					editPageDeleteButtonText = attrCollection["editPageDeleteButtonText"].Value;
				if (attrCollection["editPageCancelLinkText"] != null)
					editPageCancelLinkText = attrCollection["editPageCancelLinkText"].Value;
				if (attrCollection["editPageDeleteWarning"] != null)
					editPageDeleteWarning = attrCollection["editPageDeleteWarning"].Value;
				if (attrCollection["importPageTitle"] != null)
					importPageTitle = attrCollection["importPageTitle"].Value;
				if (attrCollection["exportPageTitle"] != null)
					exportPageTitle = attrCollection["exportPageTitle"].Value;
				if (attrCollection["importPageCancelLinkText"] != null)
					importPageCancelLinkText = attrCollection["importPageCancelLinkText"].Value;
				log.Debug($"current siteid={siteId}. invariant siteid={siteId.ToInvariantString()}");
				if (attrCollection["fieldDefinitionSrc"] != null)
					fieldDefinitionSrc = attrCollection["fieldDefinitionSrc"].Value.Replace("$_SitePath_$", "/Data/Sites/" + siteId.ToInvariantString());
				if (attrCollection["fieldDefinitionGuid"] != null)
					fieldDefinitionGuid = Guid.Parse(attrCollection["fieldDefinitionGuid"].Value);
				if (attrCollection["jsonRenderLocation"] != null)
					jsonRenderLocation = attrCollection["jsonRenderLocation"].Value;
				if (attrCollection["jsonLabelObjects"] != null)
					jsonLabelObjects = Convert.ToBoolean(attrCollection["jsonLabelObjects"].Value);
				if (attrCollection["headerLocation"] != null)
					headerLocation = attrCollection["headerLocation"].Value;
				if (attrCollection["footerLocation"] != null)
					footerLocation = attrCollection["footerLocation"].Value;
				if (attrCollection["hideOuterWrapperPanel"] != null)
					hideOuterWrapperPanel = Convert.ToBoolean(attrCollection["hideOuterWrapperPanel"].Value);
				if (attrCollection["hideInnerWrapperPanel"] != null)
					hideInnerWrapperPanel = Convert.ToBoolean(attrCollection["hideInnerWrapperPanel"].Value);
				if (attrCollection["hideOuterBodyPanel"] != null)
					hideOuterBodyPanel = Convert.ToBoolean(attrCollection["hideOuterBodyPanel"].Value);
				if (attrCollection["hideInnerBodyPanel"] != null)
					hideInnerBodyPanel = Convert.ToBoolean(attrCollection["hideInnerBodyPanel"].Value);
				if (attrCollection["showSaveAsNewButton"] != null)
					showSaveAsNew = Convert.ToBoolean(attrCollection["showSaveAsNewButton"].Value);
				if (attrCollection["maxItems"] != null)
					maxItems = Convert.ToInt32(attrCollection["maxItems"].Value);
				if (attrCollection["processItems"] != null)
					processItems = Convert.ToBoolean(attrCollection["processItems"].Value);
				if (attrCollection["viewName"] != null && !string.IsNullOrWhiteSpace(attrCollection["viewName"].Value))
				{
					UseRazor = true;
					ViewName = attrCollection["viewName"].Value;
				}

				MarkupDefinition workingMarkupDefinition = new MarkupDefinition();
				if (isMobile && !useStandardMarkupOnDesktopOnly)
				{
					// do this so mobile settings are added to desktop
					workingMarkupDefinition = (MarkupDefinition)MarkupDefinition.Clone();
				}

				foreach (XmlNode childNode in node)
				{
					if (!String.IsNullOrWhiteSpace(childNode.InnerText) || !String.IsNullOrWhiteSpace(childNode.InnerXml))
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
								if (repeaterAttribs["itemsPerGroup"] != null)
								{
									workingMarkupDefinition.ItemsPerGroup = XmlUtils.ParseInt32FromAttribute(repeaterAttribs, "itemsPerGroup", workingMarkupDefinition.ItemsPerGroup);
								}
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
								importInstructions = childNode.InnerText.Trim();
								break;
							case "ExportInstructions":
								exportInstructions = childNode.InnerText.Trim();
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
								CheckBoxListMarkup cblm = new CheckBoxListMarkup();

								XmlAttributeCollection cblmAttribs = childNode.Attributes;
								if (cblmAttribs["field"] != null)
									cblm.Field = cblmAttribs["field"].Value;
								if (cblmAttribs["token"] != null)
									cblm.Token = cblmAttribs["token"].Value;

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
								checkBoxListMarkups.Add(cblm);
								break;
							case "Scripts":
								if (isMobile)
								{
									mobileMarkupScripts = SuperFlexiHelpers.ParseScriptsFromXmlNode(childNode);
								}
								else
								{
									markupScripts = SuperFlexiHelpers.ParseScriptsFromXmlNode(childNode);
								}
								//SetupDefinedScripts(childNode, isMobile);
								//rawScript = childNode.InnerText;
								break;
							case "Styles":
								markupCSS = SuperFlexiHelpers.ParseCssFromXmlNode(childNode);
								break;
						}
					}
				}

				if (isMobile)
				{
					mobileMarkupDefinition = workingMarkupDefinition;
				}
				else
				{
					MarkupDefinition = workingMarkupDefinition;
				}
			}
		}

		#endregion private methods

		#region properties
		private Guid featureGuid = Guid.Parse("4FF93793-1187-4022-899C-C3E9096A855F");
		public Guid FeatureGuid
		{
			get { return featureGuid; }
			set { featureGuid = value; }
		}
		public bool DeleteOrphanedFieldValues
		{
			get { return ConfigHelper.GetBoolProperty("SuperFlexi:DeleteOrphanedFieldValues", false); }
		}
		public bool AlwaysLoadMarkupDefinitionFromDisk
		{
			get { return ConfigHelper.GetBoolProperty("SuperFlexi:AlwaysLoadMarkupDefinitionFromDisk", false); }
		}
		private bool useStandardMarkupOnDesktopOnly = false;

		private string fieldDefinitionSrc = string.Empty;
		public string FieldDefinitionSrc { get { return fieldDefinitionSrc; } }

		private Guid fieldDefinitionGuid = Guid.Empty;
		public Guid FieldDefinitionGuid { get { return fieldDefinitionGuid; } }


		// Script Positions:
		// inHead               
		// inBody (register script) (default)               
		// aboveMarkupDefinition               
		// belowMarkupDefinition               
		// bottomStartup(register startup script) 
		private string jsonRenderLocation = string.Empty;
		/// <summary>
		/// Where the json should be rendered.
		/// Valid options are: 
		///     inHead               
		///     inBody (register script) (default)      
		///     aboveMarkupDefinition               
		///     belowMarkupDefinition               
		///     bottomStartup(register startup script) 
		/// </summary>
		public string JsonRenderLocation { get { return jsonRenderLocation; } }


		public bool RenderJSONOfData
		{
			get
			{
				if (processItems && !String.IsNullOrWhiteSpace(jsonRenderLocation))
					return true;

				return false;
			}
		}


		private bool jsonLabelObjects = false;
		public bool JsonLabelObjects { get { return jsonLabelObjects; } }
		//private bool renderModuleLinksWithModuleTitle = true;
		//public bool RenderModuleLinksWithModuleTitle { get { return renderModuleLinksWithModuleTitle; } }

		private bool descendingSort = false;
		public bool DescendingSort { get { return descendingSort; } }

		private bool useHeader = false;
		public bool UseHeader { get { return useHeader; } }

		private string headerLocation = "InnerBodyPanel";
		/// <summary>
		/// Where the header content should be rendered.
		/// Valid options are:
		///     InnerBodyPanel (default)
		///     OuterBodyPanel
		///     InnerWrapperPanel
		///     OuterWrapperPanel
		///     Outside
		/// </summary>
		public string HeaderLocation { get { return headerLocation; } }

		private string headerContent = string.Empty;
		public string HeaderContent { get { return headerContent; } }

		private bool useFooter = false;
		public bool UseFooter { get { return useFooter; } }

		private string footerLocation = "InnerBodyPanel";
		/// <summary>
		/// Where the header content should be rendered.
		/// Valid options are:
		///     InnerBodyPanel (default)
		///     OuterBodyPanel
		///     InnerWrapperPanel
		///     OuterWrapperPanel
		///     Outside
		/// </summary>
		public string FooterLocation { get { return footerLocation; } }

		private string footerContent = string.Empty;
		public string FooterContent { get { return footerContent; } }

		private bool hideOuterWrapperPanel = false;
		public bool HideOuterWrapperPanel { get { return hideOuterWrapperPanel; } }

		private bool hideInnerWrapperPanel = false;
		public bool HideInnerWrapperPanel { get { return hideInnerWrapperPanel; } }

		private bool hideOuterBodyPanel = false;
		public bool HideOuterBodyPanel { get { return hideOuterBodyPanel; } }

		private bool hideInnerBodyPanel = false;
		public bool HideInnerBodyPanel { get { return hideInnerBodyPanel; } }

		private string instanceFeaturedImage = string.Empty;
		public string InstanceFeaturedImage { get { return instanceFeaturedImage; } }

		private string featuredImageEmptyUrl = "/Data/SiteImages/1x1.gif";
		public string FeaturedImageEmptyUrl { get { return WebUtils.GetRelativeSiteRoot() + featuredImageEmptyUrl; } }

		private string solutionLocation = string.Empty;
		public string SolutionLocation { get { return solutionLocation; } }

		public Uri SolutionLocationUrl
		{
			get
			{
				return new Uri(WebUtils.ResolveServerUrl(solutionLocation.Replace(System.Web.Hosting.HostingEnvironment.MapPath("~"), "~/") + "/"));
			}
		}

		public string RelativeSolutionLocation
		{
			get
			{
				return solutionLocation.Replace(System.Web.Hosting.HostingEnvironment.MapPath("~"), "~/");
			}
		}

		private string markupDefinitionFile = string.Empty;
		public string MarkupDefinitionFile { get { return markupDefinitionFile; } }

		public MarkupDefinition MarkupDefinition { get; private set; } = null;

		private string markupDefinitionContent = string.Empty;
		public string MarkupDefinitionContent { get { return markupDefinitionContent; } }

		private string markupDefinitionName = string.Empty;
		public string MarkupDefinitionName { get { return markupDefinitionName; } }

		private MarkupDefinition mobileMarkupDefinition = null;
		public MarkupDefinition MobileMarkupDefinition { get { return mobileMarkupDefinition; } }

		private List<MarkupScript> mobileMarkupScripts = new List<MarkupScript>();
		public List<MarkupScript> MobileMarkupScripts { get { return mobileMarkupScripts; } }

		private List<MarkupScript> markupScripts = new List<MarkupScript>();
		public List<MarkupScript> MarkupScripts { get { return markupScripts; } }

		private List<MarkupScript> editPageScripts = new List<MarkupScript>();
		public List<MarkupScript> EditPageScripts { get { return editPageScripts; } set { editPageScripts = value; } }

		private List<MarkupCss> markupCSS = new List<MarkupCss>();
		public List<MarkupCss> MarkupCSS { get { return markupCSS; } }

		private List<MarkupCss> editPageCSS = new List<MarkupCss>();
		public List<MarkupCss> EditPageCSS { get { return editPageCSS; } set { editPageCSS = value; } }

		private List<CheckBoxListMarkup> checkBoxListMarkups = new List<CheckBoxListMarkup>();
		public List<CheckBoxListMarkup> CheckBoxListMarkups { get { return checkBoxListMarkups; } }

		private string addItemText = SuperFlexiResources.AddItem;
		public string AddItemText { get { return addItemText; } }

		public string SolutionCssClass { get; private set; } = string.Empty;

		public string ModuleCssClass { get; private set; } = string.Empty;

		public string ModuleMobileCssClass { get; private set; } = string.Empty;

		private string editPageCssClass = string.Empty;
		public string EditPageCssClass { get { return editPageCssClass; } }

		private string editPageTitle = SuperFlexiResources.EditItemTitle;
		public string EditPageTitle { get { return editPageTitle; } }

		private string editPageUpdateButtonText = SuperFlexiResources.UpdateButton;
		public string EditPageUpdateButtonText { get { return editPageUpdateButtonText; } }

		private string editPageSaveButtonText = SuperFlexiResources.UpdateButton;
		public string EditPageSaveButtonText { get { return editPageSaveButtonText; } }

		private string editPageDeleteButtonText = SuperFlexiResources.DeleteButton;
		public string EditPageDeleteButtonText { get { return editPageDeleteButtonText; } }

		private string editPageCancelLinkText = SuperFlexiResources.CancelButton;
		public string EditPageCancelLinkText { get { return editPageCancelLinkText; } }

		private string editPageDeleteWarning = SuperFlexiResources.ItemDeleteWarning;
		public string EditPageDeleteWarning { get { return editPageDeleteWarning; } }

		private string importPageTitle = SuperFlexiResources.ImportTitle;
		public string ImportPageTitle { get { return importPageTitle; } }

		private string importPageCancelLinkText = SuperFlexiResources.CancelButton;
		public string ImportPageCancelLinkText { get { return importPageCancelLinkText; } }

		private string importInstructions = SuperFlexiResources.ImportInstructions;
		public string ImportInstructions { get { return importInstructions; } }

		private string exportInstructions = SuperFlexiResources.ExportInstructions;
		public string ExportInstructions { get { return exportInstructions; } }

		private string exportPageTitle = SuperFlexiResources.ExportTitle;
		public string ExportPageTitle { get { return exportPageTitle; } }

		private bool allowImport = false;
		public bool AllowImport { get { return allowImport; } }

		private bool allowExport = false;
		public bool AllowExport { get { return allowExport; } }

		public bool Debug { get; private set; } = false;

		private bool showSaveAsNew = true;
		public bool ShowSaveAsNew { get => showSaveAsNew; }

		private int maxItems = -1;
		public int MaxItems { get => maxItems; }

		private string customizableSettings = string.Empty;
		public string CustomizableSettings { get { return customizableSettings; } }

		private string moduleFriendlyName = string.Empty;
		public string ModuleFriendlyName { get { return moduleFriendlyName; } }

		private int globalViewSortOrder = 0;
		public int GlobalViewSortOrder { get { return globalViewSortOrder; } }

		private bool isGlobalView = false;
		public bool IsGlobalView { get { return isGlobalView; } }

		private bool includeInSearch = false;
		public bool IncludeInSearch { get { return includeInSearch; } }

		private SearchDef searchDefinition = new SearchDef();
		public SearchDef SearchDefinition { get { return searchDefinition; } set { searchDefinition = value; } }

		private string itemEditRoles = string.Empty;
		public string ItemEditRoles { get { return itemEditRoles; } }

		private string itemCreateRoles = string.Empty;
		public string ItemCreateRoles { get { return itemCreateRoles; } }

		private string itemDeleteRoles = string.Empty;
		public string ItemDeleteRoles { get { return itemDeleteRoles; } }

		public bool UseRazor { get; private set; } = false;

		public string ViewName { get; set; }

		public string SkinViewLocation { get; set; }

		private bool processItems = true;
		/// <summary>
		/// set in the markup definition. 
		/// used when populating data via api
		/// </summary>
		public bool ProcessItems { get { return processItems; } }

		public string ItemViewRolesFieldName { get; set; } = string.Empty;
		public string ItemEditRolesFieldName { get; set; } = string.Empty;

		public int PageSize { get; set; } = 0;
		public bool GetDynamicListsInRazor = false;
		#endregion
	}

	public class MarkupScript
	{
		private string url = string.Empty;
		public string Url
		{
			get { return url; }
			set { url = value; }
		}

		private string rawScript = string.Empty;
		public string RawScript
		{
			get { return rawScript; }
			set { rawScript = value; }
		}

		private string scriptName = string.Empty;
		public string ScriptName
		{
			get { return scriptName; }
			set { scriptName = value; }
		}


		private string position = "inBody";

		/// <summary>
		///		Position of rendered script.
		///		<para>inHead</para>
		///		<para>inBody (register script) (default)</para>
		///		<para>aboveMarkupDefinition</para>
		///		<para>belowMarkupDefinition</para>        
		///		<para>bottomStartup(register startup script)</para>
		///		<para>inHead</para>
		/// </summary>
		public string Position
		{
			get { return position; }
			set { position = value; }
		}
	}

	public class MarkupCss
	{
		private string url = string.Empty;
		public string Url
		{
			get { return url; }
			set { url = value; }
		}

		private string css = string.Empty;
		public string CSS
		{
			get { return css; }
			set { css = value; }
		}

		private string name = string.Empty;
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string media = string.Empty;
		public string Media { get { return media; } set { media = value; } }

		private bool renderAboveSSC = true;
		public bool RenderAboveSSC { get { return renderAboveSSC; } set { renderAboveSSC = value; } }
	}


	public class CheckBoxListMarkup
	{
		private string markup = string.Empty;
		public string Markup { get { return markup; } set { markup = value; } }

		private string field = string.Empty;
		public string Field { get { return field; } set { field = value; } }

		private string token = string.Empty;
		public string Token { get { return token; } set { token = value; } }

		private List<SelectedValue> selectedValues = new List<SelectedValue>();
		public List<SelectedValue> SelectedValues { get { return selectedValues; } set { selectedValues = value; } }

		private string separator = string.Empty;
		public string Separator { get { return separator; } set { separator = value; } }

		//private bool useOptionNames = false;
		//public bool UseOptionNames { get { return useOptionNames; } set { useOptionNames = value; } }

		public class SelectedValue
		{
			private string val = string.Empty;
			public string Value { get { return val; } set { val = value; } }

			private int itemID = -1;
			public int ItemID { get { return itemID; } set { itemID = value; } }

			private int count = 1;
			public int Count { get { return count; } set { count = value; } }
			public SelectedValue selectedValue;
		}
	}
}