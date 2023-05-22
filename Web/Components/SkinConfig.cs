using mojoPortal.Business;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;

namespace mojoPortal.Web.Components
{
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
		public List<SkinContentTemplate> Templates { get; set; } = new();
		public List<SkinStyle> Styles { get; set; } = new();
		public List<PanelOption> PanelOptions { get; set; } = new();
	}
	public class SkinContentTemplate
	{
		public string SysName { get; set; }
		public string Name { get; set; }
		public string Content { get; set; }
	}

	public class SkinStyle
	{
		public string Name { get; set; }
		public string Place { get; set; }
		public string @Class { get; set; }
		public string Element { get; set; }
	}

	public class PanelOption
	{
		public string Name { get; set; }
		public string @Class { get; set; }
	}

	public class SkinConfigManager
	{
		private Dictionary<string, SkinConfig> configs = new();

		/// <summary>
		/// Called from global.asax. Should not be called from anywhere else
		/// </summary>
		public SkinConfigManager()
		{
			ensureSkinConfig();
		}
		/// <summary>
		/// Called from global.asx. Should not be called from anywhere else
		/// </summary>
		/// <returns></returns>
		public SkinConfig GetConfig()
		{
			ensureSkinConfig();
			return configs[SiteUtils.GetSkinName(true)];
		}

		//public SkinConfig GetConfig(string skinName)
		//{

		//}

		public void RefreshSkinConfig(string skinName)
		{
			configs.Remove(skinName);
			configs.Add(skinName, getSkinConfig(skinName));
		}

		private void ensureSkinConfig()
		{
			string skinName = SiteUtils.GetSkinName(true);
			if (!configs.ContainsKey(skinName))
			{
				configs.Add(skinName, getSkinConfig(skinName));
			}
		}

		public void ClearAll()
		{
			configs = new Dictionary<string, SkinConfig>();
			ensureSkinConfig();
		}

		private SkinConfig getSkinConfig(string skinName)
		{
			//siteSettings = CacheHelper.GetCurrentSiteSettings();
			SkinConfig skinConfig = new();
			string skinUrlPath = SiteUtils.DetermineSkinBaseUrl(skinName);

			string configFilePath = HttpContext.Current.Server.MapPath(skinUrlPath + "/config/config.json");

			FileInfo configFile = new(configFilePath);

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
}