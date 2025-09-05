﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using SuperFlexiBusiness;

namespace SuperFlexiUI
{
	public class SuperFlexiHelpers
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(SuperFlexiHelpers));

		public static string GetModuleLinks(ModuleConfiguration config, SuperFlexiDisplaySettings displaySettings, int moduleId, int pageId)
		{
			StringBuilder litExtraMarkup = new StringBuilder();
			string add = string.Empty;
			string header = string.Empty;
			string footer = string.Empty;
			string import = string.Empty;
			string export = string.Empty;
			try
			{
				string settings = string.Format(
					displaySettings.ModuleSettingsLinkFormat,
					SiteUtils.GetNavigationSiteRoot() + "/Admin/ModuleSettings.aspx?pageid=" + pageId.ToString() + "&amp;mid=" + moduleId.ToString(),
					SuperFlexiResources.SettingsLinkLabel);

				if (!string.IsNullOrWhiteSpace(config.MarkupDefinitionName) && config.MarkupDefinitionName != "0")
				{
					if (!config.IsGlobalView && (config.MaxItems == -1 || Item.GetCountForModule(moduleId) < config.MaxItems))
					{
						add = string.Format(
							displaySettings.AddItemLinkFormat,
							SiteUtils.GetNavigationSiteRoot() + "/SuperFlexi/Edit.aspx?pageid=" + pageId.ToString() + "&amp;mid=" + moduleId.ToString(),
							SuperFlexiResources.AddItem);
					}

					if (config.UseHeader)
					{
						header = string.Format(
							displaySettings.EditHeaderLinkFormat,
							SiteUtils.GetNavigationSiteRoot() + "/SuperFlexi/EditHeader.aspx?pageid=" + pageId.ToString() + "&amp;mid=" + moduleId.ToString(),
							SuperFlexiResources.EditHeader);
					}

					if (config.UseFooter)
					{
						footer = string.Format(
							displaySettings.EditFooterLinkFormat,
							SiteUtils.GetNavigationSiteRoot() + "/SuperFlexi/EditHeader.aspx?f=true&pageid=" + pageId.ToString() + "&amp;mid=" + moduleId.ToString(),
							SuperFlexiResources.EditFooter);
					}

					if (config.AllowImport)
					{
						import = string.Format(
							displaySettings.ImportLinkFormat,
							SiteUtils.GetNavigationSiteRoot() + "/SuperFlexi/Import.aspx?pageid=" + pageId.ToString() + "&amp;mid=" + moduleId.ToString(),
							SuperFlexiResources.ImportTitle);
					}

					if (config.AllowExport)
					{
						export = string.Format(
							displaySettings.ExportLinkFormat,
							SiteUtils.GetNavigationSiteRoot() + "/SuperFlexi/Export.aspx?pageid=" + pageId.ToString() + "&amp;mid=" + moduleId.ToString(),
							SuperFlexiResources.ExportTitle);
					}
				}

				litExtraMarkup.AppendFormat(displaySettings.ModuleLinksFormat, settings, add, header, footer, import, export);
			}
			catch (FormatException ex)
			{
				Module module = new Module(moduleId);
				string moduleTitle = "unknown";
				if (module != null) moduleTitle = module.ModuleTitle;
				log.ErrorFormat("Error rendering \"{0}\", with moduleID={1}, pageid={2}. Error was:\r\n{3}", moduleTitle, moduleId.ToString(), pageId.ToString(), ex);
			}
			return litExtraMarkup.ToString();
		}

		public static string GetHelpText(string helpKey, ModuleConfiguration config)
		{

			FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
			if (p == null)
			{
				log.Error("File System Provider Could Not Be Loaded.");
				return string.Empty;
			}
			IFileSystem fileSystem = p.GetFileSystem();
			if (fileSystem == null)
			{
				log.Error("File System Could Not Be Loaded.");
				return string.Empty;
			}

			string helpText = string.Empty;
			WebFile helpFile;
			if (helpKey.ToLower().EndsWith(".sfhelp") ||
				helpKey.ToLower().EndsWith(".config") ||
				helpKey.ToLower().EndsWith(".html"))
			{
				if (helpKey.IndexOf("$_FlexiHelp_$") >= 0)
				{
					string path = fileSystem.CombinePath("~/Data/SuperFlexi/Help/", helpKey.Replace("$_FlexiHelp_$", string.Empty));
					helpFile = fileSystem.RetrieveFile(path);

				}
				else if (helpKey.IndexOf("$_SitePath_$") >= 0)
				{
					string path = helpKey.Replace("$_SitePath_$", "~/Data/Sites/" + CacheHelper.GetCurrentSiteSettings().SiteId.ToInvariantString());
					helpFile = fileSystem.RetrieveFile(path);
				}
				else if (helpKey.IndexOf("$_Data_$") >= 0)
				{
					string path = helpKey.Replace("$_Data_$", "~/Data");
					helpFile = fileSystem.RetrieveFile(path);
				}
				else if (helpKey.IndexOf("~/") >= 0)
				{
					helpFile = fileSystem.RetrieveFile(helpKey);
				}
				else
				{
					string path = fileSystem.CombinePath(config.RelativeSolutionLocation, helpKey);
					helpFile = fileSystem.RetrieveFile(path);
				}
			}
			else
			{
				helpText = ResourceHelper.GetHelpFileText(helpKey);
				helpFile = null;
			}

			if (helpFile != null && fileSystem.FileExists(helpFile.VirtualPath))
			{
				StreamReader sr = new StreamReader(fileSystem.GetAsStream(helpFile.VirtualPath));
				helpText = sr.ReadToEnd();
				sr.Close();
			}

			return helpText;
		}

		public static void ReplaceStaticTokens(
			StringBuilder stringBuilder,
			ModuleConfiguration config,
			bool isEditable,
			SuperFlexiDisplaySettings displaySettings,
			Module module,
			PageSettings pageSettings,
			SiteSettings siteSettings,
			out StringBuilder sb)
		{
			sb = stringBuilder;
			int moduleId = module.ModuleId;

			var featuredImageUrl = string.IsNullOrWhiteSpace(config.InstanceFeaturedImage) ? string.Empty : WebUtils.GetApplicationRoot() + config.InstanceFeaturedImage;
			var friendlyName = string.IsNullOrWhiteSpace(config.ModuleFriendlyName) ? module.ModuleTitle : config.ModuleFriendlyName;
			var jsonObjName = Invariant($"sflexi{moduleId}{(config.IsGlobalView ? "Modules" : "Items")}");
			var currentSkin = string.Empty;
			var siteRoot = SiteUtils.GetNavigationSiteRoot();
			var publishedOnCurrentPage = true;
			siteSettings = new SiteSettings(module.SiteGuid);

			if (HttpContext.Current != null && HttpContext.Current.Request.Params.Get("skin") != null)
			{
				currentSkin = SiteUtils.SanitizeSkinParam(HttpContext.Current.Request.Params.Get("skin")) + "/";
			}

			if (isEditable)
			{
				var pageModules = PageModule.GetPageModulesByModule(moduleId);
				if (pageModules.Where(pm => pm.PageId == pageSettings.PageId).ToList().Count() == 0)
				{
					publishedOnCurrentPage = false;
				}
			}

			sb.Replace("$_ModuleTitle_$", module.ShowTitle ? string.Format(displaySettings.ModuleTitleFormat, module.ModuleTitle) : string.Empty);
			sb.Replace("$_RawModuleTitle_$", module.ModuleTitle);
			sb.Replace("$_ModuleGuid_$", module.ModuleGuid.ToString());
			sb.Replace("$_FriendlyName_$", friendlyName);
			sb.Replace("$_FeaturedImageUrl_$", featuredImageUrl);
			sb.Replace("$_ModuleID_$", moduleId.ToString());
			sb.Replace("$_PageID_$", pageSettings.PageId.ToString());
			sb.Replace("$_PageUrl_$", siteRoot + pageSettings.Url.Replace("~/", "/"));
			sb.Replace("$_PageUrlRelative_$", pageSettings.Url.Replace("~/", "/"));
			sb.Replace("$_PageName_$", siteRoot + pageSettings.PageName);
			//sb.Replace("$_ModuleLinks_$", isEditable ? SuperFlexiHelpers.GetModuleLinks(config, displaySettings, moduleId, pageSettings.PageId) : string.Empty);
			sb.Replace("$_ModuleLinks_$", isEditable ? SuperFlexiHelpers.GetModuleLinks(config, displaySettings, moduleId, publishedOnCurrentPage ? pageSettings.PageId : -1) : string.Empty);
			sb.Replace("$_JSONNAME_$", jsonObjName);
			sb.Replace("$_ModuleClass_$", SiteUtils.IsMobileDevice() && !string.IsNullOrWhiteSpace(config.ModuleMobileCssClass) ? config.ModuleMobileCssClass : config.ModuleCssClass);
			sb.Replace("$_ModuleTitleElement_$", module.HeadElement);
			sb.Replace("$_SiteID_$", siteSettings.SiteId.ToString());
			sb.Replace("$_SiteRoot_$", string.IsNullOrWhiteSpace(siteRoot) ? "/" : siteRoot);
			sb.Replace("$_SitePath_$", string.IsNullOrWhiteSpace(siteRoot) ? "/" : Invariant($"{WebUtils.GetApplicationRoot()}/Data/Sites/{CacheHelper.GetCurrentSiteSettings().SiteId}"));
			sb.Replace("$_SkinPath_$", SiteUtils.DetermineSkinBaseUrl(currentSkin));
			sb.Replace("$_CustomSettings_$", config.CustomizableSettings); //this needs to be enhanced, a lot, right now we just dump the 'settings' where ever this token exists.
			sb.Replace("$_EditorType_$", siteSettings.EditorProviderName);
			sb.Replace("$_EditorSkin_$", siteSettings.EditorSkin.ToString());
			sb.Replace("$_EditorBasePath_$", WebUtils.ResolveUrl(ConfigurationManager.AppSettings["CKEditor:BasePath"]));
			sb.Replace("$_EditorConfigPath_$", WebUtils.ResolveUrl(ConfigurationManager.AppSettings["CKEditor:ConfigPath"]));
			sb.Replace("$_EditorToolbarSet_$", mojoPortal.Web.Editor.ToolBar.FullWithTemplates.ToString());
			sb.Replace("$_EditorTemplatesUrl_$", $"{siteRoot}/Services/CKeditorTemplates.ashx?cb={Guid.NewGuid()}");
			sb.Replace("$_EditorStylesUrl_$", $"{siteRoot}/Services/CKeditorStyles.ashx?cb={Guid.NewGuid().ToString().Replace("-", string.Empty)}");
			sb.Replace("$_DropFileUploadUrl_$", $"{siteRoot}/Services/FileService.ashx?cmd=uploadfromeditor&rz=true&ko={WebConfigSettings.KeepFullSizeImagesDroppedInEditor.ToString().ToLower()}&t={Global.FileSystemToken}");
			sb.Replace("$_FileBrowserUrl_$", $"{siteRoot}{WebConfigSettings.FileDialogRelativeUrl}");
			sb.Replace("$_HeaderContent_$", config.HeaderContent);
			sb.Replace("$_FooterContent_$", config.FooterContent);
			sb.Replace("$_SkinVersionGuid_$", siteSettings.SkinVersion.ToString());
		}

		public static Module GetSuperFlexiModule(int moduleId)
		{
			var bp = new mojoBasePage();
			var m = bp.GetModule(moduleId);
			if (m != null)
			{
				return m;
			}

			bool isSiteEditor = SiteUtils.UserIsSiteEditor();

			// these extra checks allow for editing an instance from modulewrapper
			m = new Module(moduleId);
			if (m is null
				|| m.SiteId != CacheHelper.GetCurrentSiteSettings().SiteId
				|| m.ModuleId == -1
				|| ((!WebUser.IsInRoles(m.AuthorizedEditRoles)) && (!WebUser.IsAdminOrContentAdmin) && (!isSiteEditor))
				)
			{
				m = null;
			}

			return m;
		}

		public static List<MarkupScript> ParseScriptsFromXmlNode(XmlNode childNode)
		{
			// Script Positions:
			// inHead
			// inBody (register script) (default)
			// aboveMarkupDefinition
			// belowMarkupDefinition
			// bottomStartup(register startup script)

			var workingMarkupScripts = new List<MarkupScript>();

			if (childNode.Name != "Scripts")
			{
				return workingMarkupScripts;
			}

			foreach (XmlNode child in childNode)
			{
				if (child.Name != "Script")
				{
					continue;
				}

				var script = new MarkupScript { };

				XmlAttributeCollection childAttrs = child.Attributes;

				foreach (XmlAttribute attr in childAttrs)
				{
					if (attr is null)
					{
						continue;
					}

					switch (attr.Name)
					{
						case "src":
							script.Url = attr.Value;
							break;
						case "position":
							script.Position = attr.Value;
							break;
						case "name":
							script.ScriptName = attr.Value;
							break;
						default:
							script.Attributes.Add(attr.Name, attr.Value);
							break;
					}
				}

				// if neither src or inner text is specified, skip it
				if (string.IsNullOrWhiteSpace(script.Url) && string.IsNullOrWhiteSpace(child.InnerText))
				{
					continue;
				}

				// if no src is specified, but there is inner text, assume it's raw script
				if (string.IsNullOrWhiteSpace(script.Url) && !string.IsNullOrWhiteSpace(child.InnerText))
				{
					script.RawScript = child.InnerText.Trim();
				}

				workingMarkupScripts.Add(script);
			}

			return workingMarkupScripts;
		}


		public static string GetPathToFile(ModuleConfiguration config, string path, bool forceHttps = false)
		{
			if (path.StartsWith("/") ||
				path.StartsWith("http://") ||
				path.StartsWith("https://"))
			{
				return path;
			}
			else if (path.StartsWith("~/"))
			{
				return WebUtils.ResolveServerUrl(path, forceHttps);
			}
			else if (path.StartsWith("$_SitePath_$"))
			{
				return path.Replace("$_SitePath_$", "/Data/Sites/" + CacheHelper.GetCurrentSiteSettings().SiteId.ToString() + "/");
			}
			else
			{
				return new Uri(config.SolutionLocationUrl, path).ToString();
			}
		}

		public static void SetupScripts(
			List<MarkupScript> markupScripts,
			ModuleConfiguration config,
			SuperFlexiDisplaySettings displaySettings,
			bool isEditable,
			bool isPostBack,
			string clientID,
			SiteSettings siteSettings,
			Module module,
			PageSettings pageSettings,
			Page page,
			Control control)
		{
			string scriptRefFormat = "\n<script src=\"{0}\" data-name=\"{1}\" {2}></script>";
			string rawScriptFormat = "\n<script data-name=\"{1}\" {2}>\n{0}\n</script>";

			foreach (MarkupScript script in markupScripts)
			{
				StringBuilder sbScriptText = new StringBuilder();
				StringBuilder sbScriptName = new StringBuilder();

				sbScriptName.Append(string.IsNullOrWhiteSpace(script.ScriptName) ? clientID + "flexiScript_" + markupScripts.IndexOf(script) : "flexiScript_" + script.ScriptName);
				ReplaceStaticTokens(sbScriptName, config, isEditable, displaySettings, module, pageSettings, siteSettings, out sbScriptName);

				var attributesString = new StringBuilder();
				foreach (string key in script.Attributes.Keys)
				{
					attributesString.Append($"{key}=\"{script.Attributes[key]}\" ");
				}

				string scriptName = sbScriptName.ToString();
				if (!string.IsNullOrWhiteSpace(script.Url))
				{
					string scriptUrl = GetPathToFile(config, script.Url, WebConfigSettings.SslisAvailable);
					sbScriptText.Append(string.Format(scriptRefFormat, scriptUrl, scriptName, attributesString));
				}
				else if (!string.IsNullOrWhiteSpace(script.RawScript))
				{
					sbScriptText.Append(string.Format(rawScriptFormat, script.RawScript, scriptName, attributesString));
				}

				ReplaceStaticTokens(sbScriptText, config, isEditable, displaySettings, module, pageSettings, siteSettings, out sbScriptText);

				// script position options
				// inHead
				// inBody (register script) (default)
				// aboveMarkupDefinition
				// belowMarkupDefinition
				// bottomStartup (register startup script)
				switch (script.Position)
				{
					case "inHead":
						if (!isPostBack && !page.IsCallback)
						{
							if (page.Header.FindControl(scriptName) == null)
							{
								LiteralControl headLit = new LiteralControl();
								headLit.ID = scriptName;
								headLit.Text = sbScriptText.ToString();
								headLit.ClientIDMode = ClientIDMode.Static;
								headLit.EnableViewState = false;
								page.Header.Controls.Add(headLit);
							}
						}
						break;

					case "aboveMarkupDefinition":
						if (control == null) goto case "bottomStartup";
						if (control.FindControlRecursive(scriptName) == null)
						{
							Control aboveMarkupDefinitionScripts = control.FindControlRecursive("aboveMarkupDefinitionScripts");
							if (aboveMarkupDefinitionScripts != null)
							{
								LiteralControl aboveLit = new LiteralControl
								{
									ID = scriptName,
									Text = sbScriptText.ToString()
								};
								aboveMarkupDefinitionScripts.Controls.Add(aboveLit);
							}
							else
							{
								goto case "bottomStartup";
							}
						}
						break;

					case "belowMarkupDefinition":
						if (control == null) goto case "bottomStartup";
						if (control.FindControlRecursive(scriptName) == null)
						{
							Control belowMarkupDefinitionScripts = control.FindControlRecursive("belowMarkupDefinitionScripts");
							if (belowMarkupDefinitionScripts != null)
							{
								LiteralControl belowLit = new LiteralControl
								{
									ID = scriptName,
									Text = sbScriptText.ToString()
								};
								belowMarkupDefinitionScripts.Controls.Add(belowLit);
							}
							else
							{
								goto case "bottomStartup";
							}
						}
						//strBelowMarkupScripts.AppendLine(scriptText);
						break;

					case "bottomStartup":
						if (!page.ClientScript.IsStartupScriptRegistered(scriptName))
						{
							ScriptManager.RegisterStartupScript(
								page,
								typeof(Page),
								scriptName,
								sbScriptText.ToString(),
								false);
						}
						break;

					case "inBody":
					default:
						if (!page.ClientScript.IsClientScriptBlockRegistered(scriptName))
						{
							ScriptManager.RegisterClientScriptBlock(
								page,
								typeof(Page),
								scriptName,
								sbScriptText.ToString(),
								false);
						}
						break;
				}
			}
		}

		public static void SetupStyle(
			List<MarkupCss> markupCss,
			ModuleConfiguration config,
			SuperFlexiDisplaySettings displaySettings,
			bool isEditable,
			string clientID,
			SiteSettings siteSettings,
			Module module,
			PageSettings pageSettings,
			Page page,
			Control control)
		{
			string styleLinkFormat = "\n<link rel=\"stylesheet\" href=\"{0}\" media=\"{2}\" data-name=\"{1}\" {3}>";
			string rawCSSFormat = "\n<style data-name=\"{1}\" media=\"{2}\" {3}>\n{0}\n</style>";

			foreach (var style in markupCss)
			{
				var sbStyleText = new StringBuilder();
				var sbStyleName = new StringBuilder();

				sbStyleName.Append(string.IsNullOrWhiteSpace(style.Name) ? clientID + "flexiStyle_" + markupCss.IndexOf(style) : "flexiStyle_" + style.Name);
				ReplaceStaticTokens(sbStyleName, config, false, displaySettings, module, pageSettings, siteSettings, out sbStyleName);
				string styleName = sbStyleName.ToString();

				var attributesString = new StringBuilder();
				foreach (string key in style.Attributes.Keys)
				{
					string keyValueMarkup = string.Empty;
					if (!string.IsNullOrWhiteSpace(style.Attributes[key]))
					{
						keyValueMarkup = $"=\"{style.Attributes[key]}\"";
					}

					attributesString.Append($"{key}{keyValueMarkup} ");
				}

				if (!string.IsNullOrWhiteSpace(style.Url))
				{
					string styleUrl;

					if (style.Url.StartsWith("/") ||
						style.Url.StartsWith("http://") ||
						style.Url.StartsWith("https://"))
					{
						styleUrl = style.Url;
					}
					else if (style.Url.StartsWith("~/"))
					{
						styleUrl = WebUtils.ResolveServerUrl(style.Url, siteSettings.UseSslOnAllPages);
					}
					//else if (style.Url.StartsWith("$_SitePath_$"))
					//{
					//	styleUrl = style.Url.Replace("$_SitePath_$", $"/Data/Sites/{CacheHelper.GetCurrentSiteSettings().SiteId}/");
					//}
					else
					{
						styleUrl = new Uri(config.SolutionLocationUrl, style.Url).ToString();
					}

					sbStyleText.Append(string.Format(styleLinkFormat, styleUrl, styleName, style.Media, attributesString));
				}
				else if (!string.IsNullOrWhiteSpace(style.CSS))
				{
					sbStyleText.Append(string.Format(rawCSSFormat, style.CSS, styleName, style.Media, attributesString));
				}

				ReplaceStaticTokens(sbStyleText, config, false, displaySettings, module, pageSettings, siteSettings, out sbStyleText);

				var styleElement = new LiteralControl
				{
					Text = sbStyleText.ToString()
				};

				var ssc = (StyleSheetCombiner)page.Header.FindControl("StyleSheetCombiner");

				if (ssc != null)
				{
					var sscIndex = page.Header.Controls.IndexOf(ssc);
					if (style.RenderAboveSSC)
					{
						page.Header.Controls.AddAt(sscIndex, styleElement);
					}
					else
					{
						page.Header.Controls.AddAt(sscIndex + 1, styleElement);
					}
				}
				else
				{
					page.Header.Controls.AddAt(0, styleElement);
				}
			}
		}

		internal static List<MarkupCss> ParseCssFromXmlNode(XmlNode childNode)
		{
			var markupCss = new List<MarkupCss>();

			if (childNode.Name != "Styles")
			{
				return markupCss;
			}

			foreach (XmlNode child in childNode)
			{
				if (child.Name == "Style")
				{
					var style = new MarkupCss();

					XmlAttributeCollection childAttrs = child.Attributes;

					foreach (XmlAttribute attr in childAttrs)
					{
						if (attr == null)
						{
							continue;
						}

						switch (attr.Name)
						{
							case "href":
								style.Url = attr.Value;
								break;
							case "name":
								style.Name = attr.Value;
								break;
							case "media":
								style.Media = attr.Value;
								break;
							case "renderAboveSSC":
								style.RenderAboveSSC = Convert.ToBoolean(attr.Value);
								break;
							default:
								style.Attributes.Add(attr.Name, attr.Value);
								break;
						}
					}

					if (string.IsNullOrWhiteSpace(style.Url) && string.IsNullOrWhiteSpace(child.InnerText))
					{
						continue;
					}

					if (string.IsNullOrWhiteSpace(style.Url) && !string.IsNullOrWhiteSpace(child.InnerText))
					{
						style.CSS = child.InnerText.Trim();
					}

					markupCss.Add(style);
				}
			}

			return markupCss;
		}

		public static void ParseSearchDefinition(XmlNode searchNode, Guid fieldDefinitionGuid, Guid siteGuid)
		{
			ModuleConfiguration config = new ModuleConfiguration();
			if (searchNode != null)
			{
				//XmlAttributeCollection attrCollection = node.Attributes;
				//if (attrCollection["fieldDefinitionGuid"] != null) fieldDefinitionGuid = Guid.Parse(attrCollection["fieldDefinitionGuid"].Value);
				//if (fieldDefinitionGuid == Guid.Empty) return;

				bool emptySearchDef = false;
				bool searchDefExists = true;
				SearchDef searchDef = SearchDef.GetByFieldDefinition(fieldDefinitionGuid);
				if (searchDef == null)
				{
					searchDefExists = false;
					emptySearchDef = true;

					searchDef = new SearchDef();
					searchDef.FieldDefinitionGuid = fieldDefinitionGuid;
					searchDef.SiteGuid = siteGuid;
					searchDef.FeatureGuid = config.FeatureGuid;
				}

				foreach (XmlNode childNode in searchNode)
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

					//}
				}
				if (searchDefExists && emptySearchDef)
				{
					SearchDef.DeleteByFieldDefinition(fieldDefinitionGuid);
				}
				else if (!emptySearchDef)
				{
					searchDef.Save();
				}
			}
		}

		public static ExpandoObject GetExpandoForItem(Item item)
		{
			var fields = Field.GetAllForDefinition(item.DefinitionGuid);

			if (fields == null || item == null)
			{
				return null;
			}

			dynamic itemExpando = new ExpandoObject();
			itemExpando.Guid = item.ItemGuid;
			itemExpando.Id = item.ItemID;
			itemExpando.SortOrder = item.SortOrder;

			List<ItemFieldValue> fieldValues = ItemFieldValue.GetItemValues(item.ItemGuid);

			foreach (Field field in fields)
			{
				var found = false;

				foreach (ItemFieldValue fieldValue in fieldValues)
				{
					if (field.FieldGuid == fieldValue.FieldGuid)
					{
						found = true;
						((IDictionary<String, Object>)itemExpando)[field.Name] = fieldValue.FieldValue;
					}
				}

				if (!found)
				{
					((IDictionary<String, Object>)itemExpando)[field.Name] = field.DefaultValue;
				}
			}

			return itemExpando;
		}
		public static ExpandoObject GetExpandoForModuleItems(Module module, ModuleConfiguration config, bool allForDefinition = false)
		{
			var fields = Field.GetAllForDefinition(config.FieldDefinitionGuid);
			List<Item> items;

			if (allForDefinition)
			{
				items = Item.GetForDefinition(config.FieldDefinitionGuid, module.SiteGuid, config.DescendingSort);
			}
			else
			{
				items = Item.GetForModule(module.ModuleId, config.DescendingSort);
			}

			if (fields == null || items == null)
			{
				return null;
			}

			dynamic expando = new ExpandoObject();

			expando.Definition = config.MarkupDefinitionName;
			expando.ModuleName = module.ModuleTitle;
			expando.Items = new List<dynamic>();

			foreach (Item item in items)
			{
				expando.Items.Add(GetExpandoForItem(item));
			}

			return expando;
		}

		public static object GetFieldValueFromKVPWithType(KeyValuePair<string, object> fieldValue, Field field)
		{
			object theValue;
			var _fieldValue = fieldValue.Value.ToString();
			var emptyValue = string.IsNullOrWhiteSpace(_fieldValue);

			switch (field.DataType)
			{
				case "int":
					if (field.IsList ||
						field.IsCheckBoxListField ||
						field.IsDynamicListField)
					{
						goto case "string";
					}

					if (!emptyValue && int.TryParse(_fieldValue, out int intVal))
					{
						theValue = intVal;
					}
					else
					{
						theValue = field.DefaultValue;
					}

					break;
				case "bool":
				case "boolean":
					if (bool.TryParse(_fieldValue, out bool boolVal) && !emptyValue)
					{
						theValue = boolVal;
					}
					else
					{
						theValue = field.DefaultValue;
					}
					break;
				case "string":
				default:
					if (field.ControlType == "CheckBox")
					{
						goto case "bool";
					}

					theValue = _fieldValue;
					break;
					//case "float":
					//	theValue = Convert.ToSingle(emptyValue ? float.MinValue : fieldValue.Value);
					//	break;
					//case "decimal":
					//	theValue = Convert.ToDecimal(fieldValue.Value);
					//	break;
					//case "double":
					//	theValue = Convert.ToDouble(fieldValue.Value);
					//	break;
					//case "byte":
					//	theValue = Convert.ToByte(fieldValue.Value);
					//	break;
					//case "char":
					//	theValue = Convert.ToChar(fieldValue.Value);
					//	break;
			}

			return theValue;

		}

	}
}