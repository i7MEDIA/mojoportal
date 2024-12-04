using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using System;

namespace mojoPortal.Web.Modules;

public partial class LoginModule : SiteModuleControl
{
	public string customCssClass { get; private set; }

	// FeatureGuid 12c68a12-ceea-4d29-8a81-2db8f2e9d29b

	#region OnInit

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}

	#endregion


	protected void Page_Load(object sender, EventArgs e)
	{
		if (Request.IsAuthenticated && displaySettings.HideWhenAuthenticated)
		{
			Visible = false;

			return;
		}

		SiteUtils.ForceSsl();

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

		if (displaySettings.DisableModuleChrome)
		{
			TitleControl.Visible = false;
			pnlOuterWrap.RenderContentsOnly = true;
			pnlInnerWrap.RenderContentsOnly = true;
			pnlOuterBody.RenderContentsOnly = true;
			pnlInnerBody.RenderContentsOnly = true;
		}

		pnlOuterWrap.SetOrAppendCss(customCssClass);
	}

	private void LoadSettings()
	{
		customCssClass = Settings.ParseString("CustomCssClassSetting", customCssClass);

		if (!WebHelper.IsSecureRequest())
		{
			if (WebConfigSettings.ShowWarningWhenSslIsAvailableButNotUsedWithLoginModule)
			{
				bool sslIsAvailable = SiteUtils.SslIsAvailable();
				sslWarning.Visible = sslIsAvailable;
			}
		}
		else
		{
			sslWarning.Visible = false;
		}

		janrainWidet.Visible = displaySettings.IncludeSocialLogin && !Request.IsAuthenticated;
		janrainWidet.Embed = !displaySettings.UseOverlayForSocialLogin;
		janrainWidet.AutoDetectReturnUrl = true;
		janrainWidet.OverrideText = displaySettings.SocialLoginLinkText;

		if (Request.IsAuthenticated)
		{
			litBreak.Text = displaySettings.LinkSeparator;
			avatar1.Disable = !displaySettings.ShowAvatar;

			WelcomeMessage.WrapInProfileLink = displaySettings.UseProfileLink;
		}
		else
		{
			avatar1.Disable = true;
		}

		if (displaySettings.IncludeSocialLogin && displaySettings.OnlyIncludeSocialLogin)
		{
			UpdatePanel1.Visible = false;
			login1.Visible = false;
		}

		if (!string.IsNullOrWhiteSpace(PageUrlService.GetLoginRedirectLink()) && WebConfigSettings.UseRedirectInSignInModule)
		{
			login1.SetRedirectUrl = true;
			UpdatePanel1.ChildrenAsTriggers = false;
		}
	}
}
