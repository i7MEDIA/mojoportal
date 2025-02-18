using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AutoMapper.Internal;
using Newtonsoft.Json;

namespace mojoPortal.Web;

public class BaseDisplaySettings : WebControl
{
	private static string defaultSkinPath = $"/Data/skins/{WebConfigSettings.DefaultInitialSkin}/";
	private string siteSkinPath = defaultSkinPath;
	private string skinName = WebConfigSettings.DefaultInitialSkin;
	private const string defaultConfigName = "display";
	private bool initialized;
	public virtual string FeatureName => GetType().Name.Replace("DisplaySettings", string.Empty);
	public virtual string SubFeatureName => string.Empty;
	public virtual bool IsPlugin => false;

	public BaseDisplaySettings()
	{
		if (HttpContext.Current is null)
		{
			return;
		}
		siteSkinPath = SiteUtils.DetermineSkinBaseUrl(true, Page);
		skinName = SiteUtils.GetSkinName(true);

		InitConfig();
	}


	private void InitConfig()
	{
		if (!string.IsNullOrWhiteSpace(SkinID))
		{
			if (!populateFromCache(true))
			{
				getOverrideConfig(SkinID);

				Global.SkinConfigManager.SetDisplaySettings(skinName, FeatureName + SkinID, this);
			}
		}
		else
		{
			if (!populateFromCache(false))
			{
				getDefaultConfig();
				Global.SkinConfigManager?.SetDisplaySettings(skinName, FeatureName + SubFeatureName, this);
			}
		}
	}

	private bool populateFromCache(bool useSkinId)
	{
		var cachedDisplaySettings = Global.SkinConfigManager?.GetDisplaySettings(skinName, FeatureName + SubFeatureName + (useSkinId ? SkinID : string.Empty));
		if (cachedDisplaySettings != null)
		{
			var props = GetType().GetProperties();

			foreach (var prop in props)
			{
				if (!prop.CanBeSet())
				{
					continue;
				}

				var val = cachedDisplaySettings.GetType().GetProperty(prop.Name).GetValue(cachedDisplaySettings, null);

				if (val is not null)
				{
					prop.SetValue(this, val);
				}
			}

			return true;
		}
		return false;
	}


	private bool getOverrideConfig(string overrideName)
	{
		if (ParseConfig(overrideName))
		{
			return true;
		}

		if (ParseConfig(overrideName, defaultSkinPath))
		{
			return true;
		}

		return getDefaultConfig();
	}


	private bool getDefaultConfig()
	{
		if (ParseConfig(defaultConfigName))
		{
			return true;
		}

		return ParseConfig(defaultConfigName, defaultSkinPath);
	}


	protected bool ParseConfig(string configName, string skinPath = null)
	{
		skinPath ??= siteSkinPath;

		//string featureName = string.IsNullOrWhiteSpace(FeatureName) ? string.Empty : $"{FeatureName}";
		string subFeatureName = string.IsNullOrWhiteSpace(SubFeatureName) ? string.Empty : $"{SubFeatureName}-";
		string plugins = IsPlugin ? "plugins" : string.Empty;

		//var configFile = new FileInfo(HttpContext.Current.Server.MapPath($"{skinPath}/config/{plugins}{featureName}{subFeatureName}{configName}.json"));

		if (skinPath.StartsWith("http"))
		{
			skinPath = new Uri(skinPath).LocalPath;
		}

		var relativeSkinPath = HttpContext.Current.Server.MapPath(skinPath);

		//var configPath = Path.Combine(new Uri(skinPath).LocalPath, "config", plugins, FeatureName, $"{subFeatureName}{configName}.json");
		var configPath = Path.Combine(relativeSkinPath, "config", plugins, FeatureName, $"{subFeatureName}{configName}.json");
		//data/sites/1/skins/framework/config/plugsin/EventCalendarPro/MonthViewModule-config.json
		var configFile = new FileInfo(configPath);

		if (configFile.Exists)
		{
			var content = File.ReadAllText(configFile.FullName);
			JsonConvert.PopulateObject(content, this);
			return true;
		}
		return false;
	}


	public new string SkinID
	{
		get
		{
			return base.SkinID;
		}
		set
		{
			base.SkinID = value;

			//if (!initialized && !string.IsNullOrWhiteSpace(value))
			//{
			//	InitConfig();
			//	initialized = true;
			//}
		}
	}

	protected override void Render(HtmlTextWriter writer)
	{
		// nothing to render
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		EnableViewState = false;
	}

}

public class BasePluginDisplaySettings : BaseDisplaySettings
{
	public override bool IsPlugin => true;
	public BasePluginDisplaySettings() : base() { }
}

