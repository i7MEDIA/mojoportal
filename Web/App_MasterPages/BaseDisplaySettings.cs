using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace mojoPortal.Web;

public class BaseDisplaySettings : WebControl
{
	private static string defaultSkinPath = $"/Data/skins/{WebConfigSettings.DefaultInitialSkin}/";
	private string siteSkinPath = defaultSkinPath;
	private const string defaultConfigName = "config";
	private bool initialized;
	private string skinID = string.Empty;
	public string FeatureName => GetType().Name.Replace("DisplaySettings", string.Empty);


	public BaseDisplaySettings()
	{
		if (HttpContext.Current is null)
		{
			return;
		}
		siteSkinPath = SiteUtils.DetermineSkinBaseUrl(true, false, Page);

		InitConfig();

	}

	private void InitConfig()
	{

		if (!string.IsNullOrWhiteSpace(SkinID))
		{
			getOverrideConfig(SkinID);
		}
		else
		{
			getDefaultConfig();
		}
	}

	private bool getDefaultConfig()
	{
		if (ParseConfig(defaultConfigName))
		{
			return true;
		}

		return ParseConfig(defaultConfigName, defaultSkinPath);
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


	protected bool ParseConfig(string configName, string skinPath = null)
	{
		skinPath ??= siteSkinPath;

		var configFile = new FileInfo(HttpContext.Current.Server.MapPath($"{skinPath}/config/plugins/{FeatureName}/{configName}.json"));

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

			if (!initialized)
			{
				InitConfig();
				initialized = true;
			}
		}
	}

	protected override void Render(HtmlTextWriter writer)
	{
		// nothing to render
	}

}