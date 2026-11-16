using System;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.MapUI;

public partial class GoogleMapModule : SiteModuleControl
{
	private GoogleMapConfiguration config = new();

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		PopulateLabels();
		PopulateControls();
	}

	private void PopulateControls()
	{
		gmap.Location = config.Location;

		if (config.GoogleApiKey.Length > 0)
		{
			gmap.GMapApiKey = config.GoogleApiKey;
		}
		else
		{
			gmap.GMapApiKey = SiteUtils.GetGmapApiKey();
		}

		gmap.EnableMapType = config.EnableMapType;
		gmap.EnableZoom = config.EnableZoom;
		gmap.ShowInfoWindow = config.ShowInfoWindow;
		gmap.EnableLocalSearch = config.EnableLocalSearch;
		gmap.MapHeight = config.MapHeight;
		gmap.MapWidth = config.MapWidth;
		gmap.UseIframe = config.UseIframe;
		gmap.MapRatio = config.MapRatio;
		gmap.EnableDrivingDirections = config.EnableDrivingDirections;
		gmap.GmapType = config.GoogleMapType;
		gmap.ZoomLevel = config.ZoomSetting;

		if (config.Caption.Length > 0)
		{
			litCaption.Text = config.Caption;
		}

		if (config.UseLocationAsCaption)
		{
			litCaption.Text = Server.HtmlEncode(gmap.Location);
		}
	}


	private void PopulateLabels()
	{
		if (ModuleConfiguration != null)
		{
			if (config.UseLocationAsTitle)
			{
				ModuleConfiguration.ModuleTitle = config.Location;
			}

			Title = ModuleConfiguration.ModuleTitle;
			Description = ModuleConfiguration.FeatureName;
		}

		gmap.DirectionsButtonText = GMapResources.GoogleMapGetDirectionsFromButton;
		gmap.DirectionsButtonToolTip = GMapResources.GoogleMapGetDirectionsFromButtonToolTip;
		gmap.NoApiKeyWarning = GMapResources.NoApiKeyWarning;
	}

	private void LoadSettings()
	{
		config = new GoogleMapConfiguration(Settings);

		if (config.InstanceCssClass.Length > 0)
		{
			pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass);
		}
	}

	#region OnInit

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}

	#endregion
}