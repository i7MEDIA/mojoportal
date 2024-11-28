using System;
using System.Web.UI.WebControls;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.ELetterUI;

public partial class DefaultPage : NonCmsBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		PopulateLabels();
		AddCanonicalUrl();
	}

	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.NewslettersLink);
		lnkLetterAdmin.Text = Resource.NewsLetterAdministrationHeading;

		litAnonymousHeader.Text = Resource.NewsletterPreferencesHeader;

		lnkThisPage.Text = Resource.NewslettersLink;

		MetaDescription = Resource.NewsletterSignUpPageMetaDescription;

		AddClassToBody("eletterdefault");
	}

	private void AddCanonicalUrl()
	{
		if (Page.Header == null)
		{
			return;
		}

		Page.Header.Controls.Add(new Literal()
		{
			ID = "threadurl",
			Text = $"\n<link rel=\"canonical\" href=\"{"eletter/Default.aspx".ToLinkBuilder()}\" />",
			EnableViewState = false
		});

	}

	private void LoadSettings()
	{
		newsLetterPrefs.Visible = Request.IsAuthenticated;
		pnlAnonymousSubscriber.Visible = !newsLetterPrefs.Visible;
		anonymousSubscribe.Visible = pnlAnonymousSubscriber.Visible;
		spnAdmin.Visible = WebUser.IsNewsletterAdmin;
		lnkLetterAdmin.NavigateUrl = $"eletter/Admin.aspx".ToLinkBuilder().ToString();
		lnkThisPage.NavigateUrl = $"eletter/Default.aspx".ToLinkBuilder().ToString();
		AddClassToBody("administration");
		AddClassToBody("eletter");
	}

	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		SuppressMenuSelection();
	}
	#endregion
}