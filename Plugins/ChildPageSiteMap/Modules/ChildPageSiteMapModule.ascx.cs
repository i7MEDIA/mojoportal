using System;
using System.Web.UI;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI;

public partial class ChildPageSiteMapModule : SiteModuleControl
{
	protected string customCssClass = string.Empty;

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}


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
		pnlOuterWrap.SetOrAppendCss(customCssClass);
	}


	private void LoadSettings()
	{
		ChildPageMenu1.ForceDisplay = !WebConfigSettings.EnforcePageSettingsInChildPageSiteMapModule;
		customCssClass = Settings.ParseString("CustomCssClassSetting", customCssClass);

		if (WebConfigSettings.HideMasterPageChildSiteMapWhenUsingModule)
		{
			Control c = Page.Master.FindControl("ChildPageMenu");
			if (c != null) { c.Visible = false; }
		}
	}
}