using System;
using mojoPortal.Web.Framework;

namespace mojoPortal.Features.UI;

public partial class GoogleTranslateModule : SiteModuleControl
{
	// FeatureGuid 5cb1a666-3de9-47df-8b52-ec0df2deeacf

	#region OnInit

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}

	#endregion

	private bool UseSimpleLayout = false;
	private bool ShowToolbar = false;
	private bool TrackInGoogleAnalytics = false;
	private bool AllowOnSecurePages = true;
	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		PopulateControls();
	}

	private void PopulateControls()
	{
		if (ModuleConfiguration != null)
		{
			Title = ModuleConfiguration.ModuleTitle;
			Description = ModuleConfiguration.FeatureName;
		}

		gt1.ShowToolbar = ShowToolbar;
		gt1.TrackInGoogleAnalytics = TrackInGoogleAnalytics;
		gt1.AllowSecurePageTranslation = AllowOnSecurePages;
		if (UseSimpleLayout)
		{
			gt1.Layout = "SIMPLE";
		}
		else
		{
			gt1.Layout = "HORIZONTAL";
		}

	}

	private void LoadSettings()
	{
		UseSimpleLayout = WebUtils.ParseBoolFromHashtable(Settings, "UseSimpleLayout", UseSimpleLayout);
		ShowToolbar = WebUtils.ParseBoolFromHashtable(Settings, "ShowToolbar", ShowToolbar);
		TrackInGoogleAnalytics = WebUtils.ParseBoolFromHashtable(Settings, "TrackInGoogleAnalytics", TrackInGoogleAnalytics);
		AllowOnSecurePages = WebUtils.ParseBoolFromHashtable(Settings, "AllowOnSecurePages", AllowOnSecurePages);
	}
}
