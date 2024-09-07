using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using Resources;
using System;
using System.Web.UI;

namespace mojoPortal.Web.UI;

public partial class SignInOrRegisterPrompt : UserControl
{
	public string Instructions { get; set; } = string.Empty;

	public bool ShowJanrainWidget { get; set; } = false;


	protected void Page_Load(object sender, EventArgs e)
	{
		if (Request.IsAuthenticated)
		{
			Visible = false;

			return;
		}

		PopulateControls();
	}


	private void PopulateControls()
	{
		if (Instructions.Length > 0)
		{
			litLoginInstructions.Text = Instructions;
		}
		else
		{
			litLoginInstructions.Text = Resource.DefaultSignInPrompt;
		}

		litLoginPrompt.Text = Resource.AlreadyRegistered;
		litRegisterPrompt.Text = Resource.NotYetRegistered;

		lnkLogin.Text = Resource.LoginLink;
		lnkLogin.NavigateUrl = PageUrlService.GetLoginLink();

		lnkRegister.Text = Resource.RegisterLink;
		lnkRegister.NavigateUrl = PageUrlService.GetRegisterLink(Request.RawUrl);

		string siteRoot = SiteUtils.GetNavigationSiteRoot();

		SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

		if (siteSettings == null) { return; }

		lnkRegister.Visible = siteSettings.AllowNewRegistration;

		lnkRegisterWithOpenId.Text = Resource.RegisterWithOpenIDLink;
		lnkRegisterWithOpenId.NavigateUrl = siteRoot + "/Secure/RegisterWithOpenID.aspx";

		if (siteSettings.AllowOpenIdAuth && WebConfigSettings.EnableOpenIdAuthentication)
		{
			litAdditionalRegisterOptions.Text = Resource.RegisterWithOpenIdOrWindowsLiveInstructions;
			divAdditionalRegisterOptions.Visible = true;

		}
		else if (siteSettings.AllowOpenIdAuth && WebConfigSettings.EnableOpenIdAuthentication)
		{
			litAdditionalRegisterOptions.Text = Resource.RegisterWithOpenIDInstructions;
			divAdditionalRegisterOptions.Visible = true;
		}

		divOpenId.Visible = siteSettings.AllowOpenIdAuth && WebConfigSettings.EnableOpenIdAuthentication;

		if (ShowJanrainWidget)
		{
			pnlJanrain.Visible = true;
			janrainWidet.Visible = !Request.IsAuthenticated;
			janrainWidet.Embed = true;
			janrainWidet.AutoDetectReturnUrl = true;
		}
		else
		{
			pnlJanrain.Visible = false;
			janrainWidet.Visible = false;
		}
	}
}
