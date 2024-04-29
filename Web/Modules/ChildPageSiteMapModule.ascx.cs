using System;
using System.Web.UI;

namespace mojoPortal.Web.UI;

public partial class ChildPageSiteMapModule : SiteModuleControl
{

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
	}


	private void LoadSettings()
	{
		ChildPageMenu1.ForceDisplay = !WebConfigSettings.EnforcePageSettingsInChildPageSiteMapModule;
		if (WebConfigSettings.HideMasterPageChildSiteMapWhenUsingModule)
		{
			Control c = Page.Master.FindControl("ChildPageMenu");
			if (c != null) { c.Visible = false; }
		}
	}
}