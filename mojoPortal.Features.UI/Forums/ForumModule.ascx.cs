using System;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ForumUI;

public partial class ForumModule : SiteModuleControl
{

	protected ForumConfiguration config = null;

	protected void Page_Load(object sender, EventArgs e)
	{
		Title1.EditUrl = $"{SiteRoot}/Forums/EditForum.aspx";
		Title1.EditText = ForumResources.ForumEditLabel;

		if (ModuleConfiguration != null)
		{
			Title = ModuleConfiguration.ModuleTitle;
			Description = ModuleConfiguration.FeatureName;
		}
		LoadSettings();
	}


	private void LoadSettings()
	{
		config = new ForumConfiguration(Settings);

		forumList.Config = config;
		forumList.ModuleId = ModuleId;
		forumList.PageId = PageId;
		forumList.SiteRoot = SiteRoot;
		forumList.NonSslSiteRoot = SiteUtils.GetInSecureNavigationSiteRoot();
		forumList.ImageSiteRoot = ImageSiteRoot;
		forumList.IsEditable = IsEditable;

		forumListAlt.Config = config;
		forumListAlt.ModuleId = ModuleId;
		forumListAlt.PageId = PageId;
		forumListAlt.SiteRoot = SiteRoot;
		forumListAlt.NonSslSiteRoot = forumList.NonSslSiteRoot;
		forumListAlt.ImageSiteRoot = ImageSiteRoot;
		forumListAlt.IsEditable = IsEditable;

		if (displaySettings.UseAltForumList)
		{
			forumList.Visible = false;
			forumListAlt.Visible = true;
		}

		if (!config.ShowForumSearchBox) { searchBoxTop.Visible = false; }
		if (displaySettings.HideSearchOnForumList) { searchBoxTop.Visible = false; }

		if (searchBoxTop.Visible && displaySettings.UseBottomSearchOnForumList)
		{
			searchBoxTop.Visible = false;
			searchBoxBottom.Visible = true;
		}

		if (config.InstanceCssClass.Length > 0) { pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass); }

		if (Page is mojoBasePage)
		{
			mojoBasePage basePage = Page as mojoBasePage;
			if (basePage != null)
			{
				basePage.ScriptConfig.IncludeColorBox = true;
				if (basePage.AnalyticsSection.Length == 0)
				{
					basePage.AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsForumSection", "forums");
				}
			}
		}
	}


	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}
}
