// Created:       2010-12-11
// Last Modified: 2013-09-17
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Modules
{
	public partial class LoginModule : SiteModuleControl
	{
		// FeatureGuid 12c68a12-ceea-4d29-8a81-2db8f2e9d29b

		#region OnInit

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Load += new EventHandler(Page_Load);
		}

		#endregion


		protected void Page_Load(object sender, EventArgs e)
		{
			if ((Request.IsAuthenticated) && (displaySettings.HideWhenAuthenticated))
			{
				this.Visible = false;

				return;
			}

			if (WebConfigSettings.ForceSslInLoginModule)
			{
				if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
			}

			LoadSettings();
			PopulateLabels();
			PopulateControls();
		}


		private void PopulateControls()
		{
			TitleControl.Visible = !this.RenderInWebPartMode;

			if (this.ModuleConfiguration != null)
			{
				this.Title = this.ModuleConfiguration.ModuleTitle;
				this.Description = this.ModuleConfiguration.FeatureName;
			}

			if (displaySettings.DisableModuleChrome)
			{
				TitleControl.Visible = false;
				pnlOuterWrap.RenderContentsOnly = true;
				pnlInnerWrap.RenderContentsOnly = true;
				pnlOuterBody.RenderContentsOnly = true;
				pnlInnerBody.RenderContentsOnly = true;
				divCleared.Visible = false;
			}
		}


		private void PopulateLabels()
		{ }


		private void LoadSettings()
		{
			if (!SiteUtils.IsSecureRequest())
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

			if ((WebConfigSettings.PageToRedirectToAfterSignIn.Length > 0) && (WebConfigSettings.UseRedirectInSignInModule))
			{
				login1.SetRedirectUrl = true;
				UpdatePanel1.ChildrenAsTriggers = false;
			}
		}
	}
}



namespace mojoPortal.Web.UI
{
	public class LoginModuleDisplaySettings : WebControl
	{
		private bool hideWhenAuthenticated = false;
		public bool HideWhenAuthenticated
		{
			get { return hideWhenAuthenticated; }
			set { hideWhenAuthenticated = value; }
		}

		private bool includeSocialLogin = false; // false by default because its too wide for most side columns where this module is likely to be rendered
		public bool IncludeSocialLogin
		{
			get { return includeSocialLogin; }
			set { includeSocialLogin = value; }
		}

		private bool onlyIncludeSocialLogin = false;
		public bool OnlyIncludeSocialLogin
		{
			get { return onlyIncludeSocialLogin; }
			set { onlyIncludeSocialLogin = value; }
		}

		private string linkSeparator = "<br />";
		public string LinkSeparator
		{
			get { return linkSeparator; }
			set { linkSeparator = value; }
		}

		private bool useOverlayForSocialLogin = false;
		public bool UseOverlayForSocialLogin
		{
			get { return useOverlayForSocialLogin; }
			set { useOverlayForSocialLogin = value; }
		}

		private string socialLoginLinkText = string.Empty;
		public string SocialLoginLinkText
		{
			get { return socialLoginLinkText; }
			set { socialLoginLinkText = value; }
		}

		private bool showAvatar = true;
		public bool ShowAvatar
		{
			get { return showAvatar; }
			set { showAvatar = value; }
		}

		private bool useProfileLink = true;
		public bool UseProfileLink
		{
			get { return useProfileLink; }
			set { useProfileLink = value; }
		}

		private bool disableModuleChrome = false;
		public bool DisableModuleChrome
		{
			get { return disableModuleChrome; }
			set { disableModuleChrome = value; }
		}


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			EnableViewState = false;
		}


		protected override void Render(HtmlTextWriter writer)
		{
			if (HttpContext.Current == null)
			{
				writer.Write("[" + this.ID + "]");
				return;
			}
		}
	}
}
