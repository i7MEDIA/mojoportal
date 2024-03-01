using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Controls.Editors;
using Newtonsoft.Json;

namespace mojoPortal.Web.Components;

public class SkinConfig
{
	public string Name { get; set; } = WebConfigSettings.DefaultInitialSkin;
	public string Version { get; set; } = "0";
	public string Author { get; set; } = string.Empty;
	public string License { get; set; } = "EPL";
	public string SupportUrl { get; set; } = "https://www.mojoportal.com";
	public string HelpLinkScriptPath { get; set; } = "~/ClientScript/mojoHelpLinkScript.js";
	public string ModalTemplatePath { get; set; } = "~/Content/Templates/mojoModal.html";
	public string ModalScriptPath { get; set; } = "~/ClientScript/mojoModalScript.js";
	public string CompatibleWith { get; set; } = "n/a";
	public MenuOptions Menu { get; set; } = new MenuOptions();
	public MenuOptions PageMenu { get; set; } = new MenuOptions();
	public List<SkinContentTemplate> Templates { get; set; } = [];
	public List<EditorStyle> EditorStyles { get; set; } = [];
	public List<PanelOption> PanelOptions { get; set; } = [];
}
public class SkinContentTemplate
{
	public string SysName { get; set; }
	public string Name { get; set; }
	public string Content { get; set; }
}

//public class SkinStyle
//{
//	public string Name { get; set; }
//	public string Place { get; set; }
//	public string @Class { get; set; }
//	public string Element { get; set; }
//}

public class PanelOption
{
	public string Name { get; set; }
	public string @Class { get; set; }
}

public class MenuOptions
{
	public bool UseDescriptions { get; set; }

	public bool UseImages { get; set; }

	public bool UnclickableLinks { get; set; }

	//public bool HideOnSiteClosed { get; set; } = WebConfigSettings.HideAllMenusOnSiteClosedPage;

	//public bool HideOnLogin { get; set; } = WebConfigSettings.HideMenusOnLoginPage;

	//public bool HideOnSiteMap { get; set; } = WebConfigSettings.HideMenusOnSiteMap;

	//public bool HideOnRegister { get; set; } = WebConfigSettings.HideMenusOnRegisterPage;

	//public bool HideOnPasswordRecovery { get; set; } = WebConfigSettings.HideMenusOnPasswordRecoveryPage;

	//public bool HideOnChangePassword { get; set; } = WebConfigSettings.HideMenusOnChangePasswordPage;

	//public bool HideOnProfile { get; set; } = WebConfigSettings.HideAllMenusOnProfilePage;

	//public bool HideOnMemberList { get; set; } = WebConfigSettings.HidePageMenuOnMemberListPage;
}

public class SkinConfigManager
{
	private ConcurrentDictionary<string, SkinConfig> configs = [];

	/// <summary>
	/// Called from global.asax. Should not be called from anywhere else
	/// </summary>
	public SkinConfigManager()
	{
		ensureSkinConfig();
	}
	/// <summary>
	/// Called from global.asax. Should not be called from anywhere else
	/// </summary>
	/// <returns></returns>
	public SkinConfig GetConfig()
	{
		ensureSkinConfig();
		string skinName = SiteUtils.GetSkinName(true);
		if (!configs.ContainsKey(skinName))
		{
			return configs.GetOrAdd(skinName, getSkinConfig(skinName));
		}
		return configs[skinName];
	}

	public void RefreshSkinConfig(string skinName)
	{
		configs.AddOrUpdate(skinName, getSkinConfig(skinName), (key, oldValue) => getSkinConfig(skinName));
	}

	private void ensureSkinConfig()
	{
		string skinName = SiteUtils.GetSkinName(true);
		if (!configs.ContainsKey(skinName))
		{
			configs.AddOrUpdate(skinName, getSkinConfig(skinName), (key, oldValue) => getSkinConfig(skinName));
		}
	}

	public void ClearAll()
	{
		configs.Clear();
		ensureSkinConfig();
	}

	private SkinConfig getSkinConfig(string skinName)
	{
		var siteSettings = CacheHelper.GetCurrentSiteSettings();
		var skinConfig = new SkinConfig();

		if (siteSettings is null)
		{
			return skinConfig;
		}

		string skinUrlPath = SiteUtils.DetermineSkinBaseUrl(skinName);

		string configFilePath = HttpContext.Current.Server.MapPath($"{skinUrlPath}/config/config.json");
		var configFile = new FileInfo(configFilePath);

		if (configFile.Exists)
		{
			var content = File.ReadAllText(configFile.FullName);

			skinConfig = JsonConvert.DeserializeObject<SkinConfig>(content);
			skinConfig.HelpLinkScriptPath = resolveFilePath(skinConfig.HelpLinkScriptPath, skinUrlPath);
			skinConfig.ModalScriptPath = resolveFilePath(skinConfig.ModalScriptPath, skinUrlPath);
			skinConfig.ModalTemplatePath = resolveFilePath(skinConfig.ModalTemplatePath, skinUrlPath);
		}
		else
		{
			skinConfig.HelpLinkScriptPath = VirtualPathUtility.ToAbsolute(skinConfig.HelpLinkScriptPath);
			skinConfig.ModalScriptPath = VirtualPathUtility.ToAbsolute(skinConfig.ModalScriptPath);
			skinConfig.ModalTemplatePath = VirtualPathUtility.ToAbsolute(skinConfig.ModalTemplatePath);
		}

		#region Editor Styles
		var editorStylesFile = new FileInfo(HttpContext.Current.Server.MapPath($"{skinUrlPath}/config/editorstyles.json"));
		var systemEditorStylesFile = new FileInfo(HttpContext.Current.Server.MapPath("~/data/style/editorstyles.json"));

		var systemStylesExists = systemEditorStylesFile.Exists;

		var styles = new List<EditorStyle>();

		//add system styles first, if WebConfig says so
		if (WebConfigSettings.AddSystemStyleTemplatesAboveSiteTemplates && systemStylesExists)
		{
			styles.AddRange(EditorStyle.GetEditorStyles(systemEditorStylesFile));
		}

		//add site styles
		using (IDataReader reader = ContentStyle.GetAllActive(siteSettings.SiteGuid))
		{
			while (reader.Read())
			{
				styles.Add(new EditorStyle
				{
					Name = reader["Name"].ToString(),
					Element = [reader["Element"].ToString()],
					Attributes = new Dictionary<string, string>() { { "class", reader["CssClass"].ToString() } }
				});
			}
		}

		//add skin styles
		if (editorStylesFile.Exists)
		{
			styles.AddRange(EditorStyle.GetEditorStyles(editorStylesFile));
		}

		//add system styles last, if WebConfig says so
		if (WebConfigSettings.AddSystemStyleTemplatesBelowSiteTemplates && systemStylesExists)
		{
			styles.AddRange(EditorStyle.GetEditorStyles(systemEditorStylesFile));
		}

		skinConfig.EditorStyles = styles;

		#endregion

		return skinConfig;
	}

	private string resolveFilePath(string path, string skinUrlPath)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			return VirtualPathUtility.ToAbsolute(path.Replace("$SkinPath$/", skinUrlPath).Replace("$SkinPath$", skinUrlPath));
		}

		return path;
	}
}