using mojoPortal.Web.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace mojoPortal.Web.XmlUI;

public class XmlConfiguration
{
	private const string _featureGuid = "fa969c0a-6d02-4dcb-86b8-ac69d80c1fb1";

	public static Guid FeatureGuid => new(_featureGuid);
	public string XmlUrl { get; private set; } = string.Empty;
	public string XslUrl { get; private set; } = string.Empty;
	public string XmlFileSource { get; private set; } = string.Empty;
	public string XslFileSource { get; private set; } = string.Empty;
	public string InstanceCssClass { get; private set; } = string.Empty;
	public bool AllowExternalImages { get; private set; } = false;
	public bool TrustContent { get; private set; } = false;
	public List<string> AllowedHostnames { get; set; } = [];


	public XmlConfiguration()
	{ }


	public XmlConfiguration(Hashtable settings)
	{
		LoadSettings(settings);
	}


	private void LoadSettings(Hashtable settings)
	{
		if (settings == null)
		{
			throw new ArgumentException("must pass in a hashtable of settings");
		}

		if (settings.Contains("XmlModuleXmlSourceSetting"))
		{
			XmlFileSource = settings["XmlModuleXmlSourceSetting"].ToString();
		}

		if (settings.Contains("XmlModuleXslSourceSetting"))
		{
			XslFileSource = settings["XmlModuleXslSourceSetting"].ToString();
		}

		if (settings.Contains("CustomCssClassSetting"))
		{
			InstanceCssClass = settings["CustomCssClassSetting"].ToString();
		}

		if (settings.Contains("XmlUrl"))
		{
			XmlUrl = settings["XmlUrl"].ToString();
		}

		if (settings.Contains("XslUrl"))
		{
			XslUrl = settings["XslUrl"].ToString();
		}

		AllowExternalImages = WebUtils.ParseBoolFromHashtable(settings, "AllowExternalImages", AllowExternalImages);
		TrustContent = WebUtils.ParseBoolFromHashtable(settings, "TrustContent", TrustContent);

		if (settings.Contains("AllowedHostnames"))
		{
			AllowedHostnames = HostnameHelper.ParseMultilineHostnameList(settings["AllowedHostnames"].ToString());
		}
	}
}
