using System;
using System.Web.UI;

namespace mojoPortal.Web.UI;

public partial class ChildPageSiteMapModule : SiteModuleControl
{
	#region OnInit

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}

	#endregion

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		PopulateControls();
	}

	private void PopulateControls()
	{
		Title1.Visible = !RenderInWebPartMode;

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