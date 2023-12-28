using System;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI;

public partial class BingSearchPage : NonCmsBasePage
{
	string bingApiId = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();

		if (string.IsNullOrEmpty(bingApiId))
		{
			WebUtils.SetupRedirect(this, SiteRoot + "/SearchResults.aspx");
			return;

		}

		PopulateLabels();
	}

	private void PopulateLabels()
	{
		if (siteSettings == null) return;

		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.SearchPageTitle);

		heading.Text = Resource.SearchPageTitle;
	}

	private void LoadSettings()
	{
		bingApiId = SiteUtils.GetBingApiId();

		AddClassToBody("bingsearch");
	}

	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.Load += new EventHandler(this.Page_Load);

		SuppressMenuSelection();
		SuppressPageMenu();
	}

	#endregion
}