using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls.google;

public class GMapBasePanel : Panel
{
	public string Protocol { get; set; } = "http";

	/// <summary>
	/// message to display if the api key is not found
	/// </summary>
	public string NoApiKeyWarning { get; set; } = "Could not find Google Maps API Key in Site Settings or Web.config";

	/// <summary>
	/// default is 13, lower number zooms out higher number zooms in
	/// </summary>
	public int ZoomLevel { get; set; } = 13;


	public MapType GmapType { get; set; } = MapType.G_NORMAL_MAP;

	/// <summary>
	/// This should be the path to the folder containing mojoPortal custom javascript for working with google APIs
	/// i.e ~/ClientScript/google/
	/// This is NOT the path to the google server.
	/// </summary>
	public string GoogleJsBasePath { get; set; } = "~/ClientScript/google/";

	/// <summary>
	/// To use google maps you need an API key.
	/// Get it here: http://code.google.com/apis/maps/signup.html
	/// put it in Web.config as <add key="GoogleMapsAPIKey" value="" />
	/// </summary>
	public string GMapApiKey { get; set; } = string.Empty;

	/// <summary>
	/// You can set this on the control if you need to use a different Web.config key for some reason.
	/// </summary>
	protected string GMapWebConfigKey { get; set; } = "GoogleMapsAPIKey";

	/// <summary>
	/// Indicates whether the GMapApiKey was initialized.
	/// Initialization happens in on pre-render
	/// </summary>
	protected bool ApiKeyFound { get; private set; } = false;

	/// <summary>
	/// Hooks up the main GMap script.
	/// SubClasses should override but call the base method before doing anything else.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		LoadSettings();
		if (ApiKeyFound)
		{
			IncludeGMapScript();
		}
		else
		{
			Controls.Add(new Literal
			{
				Text = NoApiKeyWarning
			});
		}
	}

	private void LoadSettings()
	{
		if (GMapApiKey.Length > 0)
		{
			// don't look in Web.config if setting has been already made.
			ApiKeyFound = true;
			return;
		}

		if (ConfigurationManager.AppSettings[GMapWebConfigKey] != null)
		{
			GMapApiKey = ConfigurationManager.AppSettings[GMapWebConfigKey];
			if (GMapApiKey.Length > 0)
			{
				ApiKeyFound = true;
			}
		}
	}

	private void IncludeGMapScript()
	{
		Page.ClientScript.RegisterClientScriptBlock(
			typeof(Page),
			"gmap", $"\n<script src=\"{Protocol}://maps.google.com/maps?file=api&amp;v=2&amp;key={GMapApiKey}\" data-loader=\"gMapBasePanel\"></script>");

		Page.ClientScript.RegisterClientScriptBlock(
			typeof(Page),
			"mojogmaputils", $"\n<script src=\"{Page.ResolveUrl($"{GoogleJsBasePath}mojogmaputils.js")}\" data-loader=\"gMapBasePanel\"></script>");
	}
}