using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.SiteCreatedEventHandlers;
using mojoPortal.Net;
using mojoPortal.Web.Controls.Captcha;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
namespace mojoPortal.Web.AdminUI
{

	public partial class SiteSettingsPage : NonCmsBasePage
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(SiteSettingsPage));
		private string logoPath = string.Empty;
		private bool sslIsAvailable = false;
		private bool IsServerAdmin = false;
		private int currentSiteID = 0;
		private Guid currentSiteGuid = Guid.Empty;
		private int selectedSiteID = -2;
		private bool isAdmin = false;
		private bool isContentAdmin = false;
		private bool allowPasswordFormatChange = false;
		private bool useFolderForSiteDetection = false;
		private SiteSettings selectedSite;
		protected string lastGroupValue = string.Empty;
		private bool enableSiteSettingsSmtpSettings = false;
		private bool maskSMTPPassword = true;
		private string requestedTab = string.Empty;
		protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;

		protected void Page_Load(object sender, EventArgs e)
		{
			SecurityHelper.DisableBrowserCache();
			if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();

			CheckAuthentication();

			if(SiteUtils.IsFishyPost(this))
			{
				SiteUtils.RedirectToAccessDeniedPage(this);
				return;
			}
			
			if (ScriptController != null)
			{	
				ScriptController.RegisterAsyncPostBackControl(btnAddHost);
				ScriptController.RegisterAsyncPostBackControl(rptHosts);

				ScriptController.RegisterAsyncPostBackControl(btnAddFolder);
				ScriptController.RegisterAsyncPostBackControl(rptFolderNames);

				ScriptController.RegisterAsyncPostBackControl(btnAddFeature);
				ScriptController.RegisterAsyncPostBackControl(btnRemoveFeature);
			}

			PopulateLabels();
			SetupScripts();
			
			if (!Page.IsPostBack)
			{
				hdnCurrentSkin.Value = selectedSite.Skin;
				BindGeoLists();
				PopulateControls();
			}
		}

		private void CheckAuthentication()
		{
			if ((!isAdmin) && (!isContentAdmin))
			{
				SiteUtils.RedirectToAccessDeniedPage(this);
				return;
			}
		}

		private void PopulateControls()
		{
			if (IsServerAdmin && WebConfigSettings.AllowMultipleSites)
			{
				PopulateMultiSiteControls();
			}
			else if (!WebConfigSettings.AllowMultipleSites)
			{
				litHostListHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsMultiTenancyTurnedOffLabel, string.Empty);
				litHostMessage.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsMultiTenancyTurnedOff, string.Empty);
				pnlAddFolder.Visible = false;
				pnlAddHostName.Visible = false;
				rptFolderNames.Visible = false;
				rptHosts.Visible = false;
			}

			if (selectedSiteID == -1)
			{
				heading.Text = Resource.CreateNewSite;
				txtSiteName.Text = Resource.SiteSettingsNewSiteLabel;
			}
			else
			{
				heading.Text = string.Format(CultureInfo.InvariantCulture, Resource.SiteSettingsFormat, SecurityHelper.RemoveMarkup(selectedSite.SiteName));
				txtSiteName.Text = selectedSite.SiteName;

				if (!siteSettings.IsServerAdminSite && WebConfigSettings.HideGoogleAnalyticsInChildSites)
				{
					fgpGAnalytics.Visible = false;
				}

				fgpSiteId.Visible = (siteSettings.IsServerAdminSite && WebConfigSettings.ShowSiteGuidInSiteSettings);
				lblSiteId.Text = selectedSite.SiteId.ToString(CultureInfo.InvariantCulture);
				lblSiteGuid.Text = selectedSite.SiteGuid.ToString();
			}

			if (timeZone is ISettingControl setting)
			{
				setting.SetValue(selectedSite.TimeZoneId);
			}

			txtSlogan.Text = selectedSite.Slogan;
			txtCompanyName.Text = selectedSite.CompanyName;
			txtStreetAddress.Text = selectedSite.CompanyStreetAddress;
			txtStreetAddress2.Text = selectedSite.CompanyStreetAddress2;
			txtLocality.Text = selectedSite.CompanyLocality;
			txtRegion.Text = selectedSite.CompanyRegion;
			txtPostalCode.Text = selectedSite.CompanyPostalCode;
			txtCountry.Text = selectedSite.CompanyCountry;
			txtPhone.Text = selectedSite.CompanyPhone;
			txtFax.Text = selectedSite.CompanyFax;
			txtPublicEmail.Text = selectedSite.CompanyPublicEmail;

			//lblPrivacySiteRoot.Text = selectedSite.SiteRoot;
			lblPrivacySiteRoot.Text = SiteUtils.GetNavigationSiteRoot(selectedSite);
			txtPrivacyPolicyUrl.Text = selectedSite.PrivacyPolicyUrl;
			txtRpxNowApiKey.Text = selectedSite.RpxNowApiKey.Trim();
			txtRpxNowApplicationName.Text = selectedSite.RpxNowApplicationName;
			lnkRpxAdmin.NavigateUrl = siteSettings.RpxNowAdminUrl;
			lnkRpxAdmin.Visible = (selectedSite.RpxNowAdminUrl.Length > 0);
			txtOpenSearchName.Text = selectedSite.OpenSearchName;
			txtMetaProfile.Text = selectedSite.MetaProfile;

			DirectoryInfo[] skins;

			if (selectedSiteID > -1)
			{
				skins = SiteUtils.GetSkinList(this.selectedSite);
				SkinSetting.SelectedSite = selectedSite;
			}
			else
			{
				skins = SiteUtils.GetSkinCatalogList();
				SkinSetting.UseCatalog = true;
			}

			ddMobileSkin.DataSource = skins;
			ddMobileSkin.DataBind();

			//ddMyPageSkin.DataSource = skins;
			//ddMyPageSkin.DataBind();

			//ListItem listItem = new ListItem();
			ListItem listItem = new ListItem
			{
				Value = "",
				Text = Resource.PageLayoutDefaultSkinLabel
			};
			ddMobileSkin.Items.Insert(0, listItem);

			listItem = ddMobileSkin.Items.FindByValue("printerfriendly");
			if (listItem != null)
			{
				ddMobileSkin.Items.Remove(listItem);
			}

			listItem = ddMobileSkin.Items.FindByValue(selectedSite.MobileSkin);
			if (listItem != null)
			{
				ddMobileSkin.ClearSelection();
				listItem.Selected = true;
			}

			chkSiteIsClosed.Checked = selectedSite.SiteIsClosed;
			
			txtBingSearchAPIKey.Text = selectedSite.BingAPIId;
			txtGoogleCustomSearchId.Text = selectedSite.GoogleCustomSearchId;
			chkShowAlternateSearchIfConfigured.Checked = selectedSite.ShowAlternateSearchIfConfigured;

			ListItem item;
			if (selectedSite.PrimarySearchEngine.Length > 0)
			{
				item = ddSearchEngine.Items.FindByValue(selectedSite.PrimarySearchEngine);
				if (item != null)
				{
					ddSearchEngine.ClearSelection();
					item.Selected = true;
				}
			}

			if (selectedSite.Skin.Length > 0)
			{
				SkinSetting.SetValue(selectedSite.Skin);
			}

			item = ddAvatarSystem.Items.FindByValue(selectedSite.AvatarSystem);
			if (item != null)
			{
				ddAvatarSystem.ClearSelection();
				item.Selected = true;
			}

			item = ddCommentSystem.Items.FindByValue(selectedSite.CommentProvider);
			if (item != null)
			{
				ddCommentSystem.ClearSelection();
				item.Selected = true;
			}

			if (selectedSite.SiteGuid != siteSettings.SiteGuid)
			{ 
				SkinSetting.ShowPreviewLink = false;
			}

			ddLogos.DataSource = SiteUtils.GetLogoList(selectedSite);
			ddLogos.DataBind();

			if (selectedSite.Logo.Length > 0)
			{
				item = ddLogos.Items.FindByValue(selectedSite.Logo);
				if (item != null)
				{
					ddLogos.ClearSelection();
					item.Selected = true;
				}

				imgLogo.Src = $"{logoPath}{selectedSite.Logo}";
			}

			item = ddEditorProviders.Items.FindByValue(selectedSite.EditorProviderName);
			if (item != null)
			{
				ddEditorProviders.ClearSelection();
				item.Selected = true;
			}

			item = ddNewsletterEditor.Items.FindByValue(selectedSite.NewsletterEditor);
			if (item != null)
			{
				ddNewsletterEditor.ClearSelection();
				item.Selected = true;
			}
			
			item = ddDefaultFriendlyUrlPattern.Items.FindByValue(selectedSite.DefaultFriendlyUrlPattern.ToString());
			if (item != null)
			{
				ddDefaultFriendlyUrlPattern.ClearSelection();
				item.Selected = true;
			}

			item = ddCaptchaProviders.Items.FindByValue(selectedSite.CaptchaProvider);
			if (item != null)
			{
				ddCaptchaProviders.ClearSelection();
				item.Selected = true;
			}

			item = ddDefaultCountry.Items.FindByValue(selectedSite.DefaultCountryGuid.ToString());
			if (item != null)
			{
				ddDefaultCountry.ClearSelection();
				item.Selected = true;
				BindZoneList();
			}

			item = ddDefaultGeoZone.Items.FindByValue(selectedSite.DefaultStateGuid.ToString());
			if (item != null)
			{
				ddDefaultGeoZone.ClearSelection();
				item.Selected = true;	
			}

			txtRecaptchaPrivateKey.Text = selectedSite.RecaptchaPrivateKey;
			txtRecaptchaPublicKey.Text = selectedSite.RecaptchaPublicKey;
			rbRecaptchaHcaptcha.SelectedValue = selectedSite.CaptchaReCaptchaHCaptcha;

			txtCaptchaClientScriptUrl.Text = selectedSite.CaptchaClientScriptUrl;
			txtCaptchaVerifyUrl.Text = selectedSite.CaptchaVerifyUrl;
			txtCaptchaTheme.Text = selectedSite.CaptchaTheme;
			txtCaptchaParam.Text = selectedSite.CaptchaParam;
			txtCaptchaResponseField.Text = selectedSite.CaptchaResponseField;


			pnlRecaptchaSettings.Enabled = selectedSite.CaptchaProvider == "RecaptchaCaptchaProvider";

			//if (selectedSite.CaptchaReCaptchaHCaptcha == "recaptcha")
			//{
			//	if (selectedSite)
			//}



			txtBadWordList.Text = selectedSite.BadWordList;
			chkForceBadWordChecking.Checked = selectedSite.BadWordCheckingEnforced;
			txtGmapApiKey.Text = selectedSite.GmapApiKey;
			txtAddThisUserId.Text = selectedSite.AddThisDotComUsername;
			txtGoogleAnayticsAccountCode.Text = selectedSite.GoogleAnalyticsAccountCode;
			txtOpenIdSelectorCode.Text = selectedSite.OpenIdSelectorId;
			chkEnableWoopra.Checked = selectedSite.EnableWoopra;
			txtIntenseDebateAccountId.Text = selectedSite.IntenseDebateAccountId;
			txtDisqusSiteShortName.Text = selectedSite.DisqusSiteShortName;
			txtFacebookAppId.Text = selectedSite.FacebookAppId;


			txtHeaderContent.Text = selectedSite.SiteWideHeaderContent;
			txtFooterContent.Text = selectedSite.SiteWideFooterContent;
			txtHeaderAdminContent.Text = selectedSite.SiteWideHeaderAdminContent;
			txtFooterAdminContent.Text = selectedSite.SiteWideFooterAdminContent;
						  
			ISettingControl currencySetting = SiteCurrencySetting as ISettingControl;
			currencySetting.SetValue(selectedSite.CurrencyGuid.ToString());

			if (WebConfigSettings.EnableOpenIdAuthentication)
			{
				chkAllowOpenIDAuth.Checked = selectedSite.AllowOpenIdAuth;
			}
			
			if (WebConfigSettings.EnableWindowsLiveAuthentication)
			{
				chkAllowWindowsLiveAuth.Checked = selectedSite.AllowWindowsLiveAuth;
				txtWindowsLiveAppID.Text = selectedSite.WindowsLiveAppId;
				txtWindowsLiveKey.Text = selectedSite.WindowsLiveKey;
			}
			else
			{
				chkAllowWindowsLiveAuth.Checked = false;
				chkAllowWindowsLiveAuth.Enabled = false;
				txtWindowsLiveAppID.Enabled = false;
				txtWindowsLiveKey.Enabled = false;
			}

			txtAppLogoForWindowsLive.Text = selectedSite.AppLogoForWindowsLive;
			pickAppLogoForWindowsLive.Text = Resource.Browse;
			pickAppLogoForWindowsLive.ToolTip = Resource.Browse;
			pickAppLogoForWindowsLive.TextBoxClientId = txtAppLogoForWindowsLive.ClientID;

			chkAllowWindowsLiveMessengerForMembers.Checked = selectedSite.AllowWindowsLiveMessengerForMembers;

			if (!selectedSite.UseLdapAuth || (selectedSite.UseLdapAuth && selectedSite.AllowDbFallbackWithLdap))
			{
				chkAllowRegistration.Checked = selectedSite.AllowNewRegistration;
				chkSecureRegistration.Checked = selectedSite.UseSecureRegistration;
				chkRequireApprovalForLogin.Checked = selectedSite.RequireApprovalBeforeLogin;
				
				chkAllowUserToChangeName.Checked = selectedSite.AllowUserFullNameChange;
				chkReallyDeleteUsers.Checked = selectedSite.ReallyDeleteUsers;
				if (!selectedSite.UseLdapAuth)
				{
					chkUseEmailForLogin.Checked = selectedSite.UseEmailForLogin;
				}
				else
				{
					chkUseEmailForLogin.Enabled = false;
				}
				chkAllowPasswordRetrieval.Checked = selectedSite.AllowPasswordRetrieval;
				chkRequirePasswordChangeAfterRecovery.Checked = selectedSite.RequirePasswordChangeOnResetRecover;
				chkRequiresQuestionAndAnswer.Checked = selectedSite.RequiresQuestionAndAnswer;
				chkAllowPasswordReset.Checked = selectedSite.AllowPasswordReset;
				txtMinimumPasswordLength.Text = selectedSite.MinRequiredPasswordLength.ToInvariantString();
				txtMinRequiredNonAlphaNumericCharacters.Text = selectedSite.MinRequiredNonAlphanumericCharacters.ToInvariantString();
				txtPasswordStrengthRegularExpression.Text = selectedSite.PasswordStrengthRegularExpression;
				txtPasswordStrengthErrorMessage.Text = selectedSite.PasswordRegexWarning;
				chkShowPasswordStrength.Checked = selectedSite.ShowPasswordStrengthOnRegistration;
				chkRequireCaptcha.Checked = selectedSite.RequireCaptchaOnRegistration;
				chkRequireEmailTwice.Checked = selectedSite.RequireEnterEmailTwiceOnRegistration;

				chkAllowDbFallbackWithLdap.Checked = selectedSite.AllowDbFallbackWithLdap;
				chkAllowEmailLoginWithLdapDbFallback.Checked = selectedSite.AllowEmailLoginWithLdapDbFallback;
			}
			else
			{
				chkAllowRegistration.Enabled = false;
				chkSecureRegistration.Enabled = false;
				chkRequireApprovalForLogin.Enabled = false;
				
				chkAllowUserToChangeName.Enabled = false;
				chkReallyDeleteUsers.Enabled = false;
				chkUseEmailForLogin.Enabled = false;
				chkAllowPasswordRetrieval.Enabled = false;
				chkRequiresQuestionAndAnswer.Enabled = false;
				chkAllowPasswordReset.Enabled = false;
				txtMinimumPasswordLength.Enabled = false;
				txtMinRequiredNonAlphaNumericCharacters.Enabled = false;
				txtPasswordStrengthRegularExpression.Enabled = false;
				txtPasswordStrengthErrorMessage.Enabled = false;
				chkShowPasswordStrength.Enabled = false;
				chkRequireCaptcha.Enabled = false;
				chkRequireEmailTwice.Enabled = false;
				//chkAllowPersistentLogin.Enabled = false;
				
			}

			txtEmailAdressesForUserApprovalNotification.Text = selectedSite.EmailAdressesForUserApprovalNotification;
			txtEmailAdressesForUserApprovalNotification.Enabled = chkRequireApprovalForLogin.Checked;
			fgpDisableDbAuthentication.Visible = !WebConfigSettings.HideDisableDbAuthenticationSetting;
			chkDisableDbAuthentication.Checked = selectedSite.DisableDbAuth;

			chkAllowUserEditorChoice.Checked = selectedSite.AllowUserEditorPreference;
			chkAllowUserSkins.Checked = selectedSite.AllowUserSkins;
			chkAllowPageSkins.Checked = selectedSite.AllowPageSkins;
			chkAllowHideMenuOnPages.Checked = selectedSite.AllowHideMenuOnPages;

			if (WebConfigSettings.ForceSslOnAllPages)
			{
				chkRequireSSL.Checked = true;
				chkRequireSSL.Enabled = false;
			}
			else
			{
				chkRequireSSL.Checked = selectedSite.UseSslOnAllPages;
			}

			chkAllowPersistentLogin.Checked = selectedSite.AllowPersistentLogin;

			txtPreferredHostName.Text = selectedSite.PreferredHostName;
			chkForceContentVersioning.Checked = selectedSite.ForceContentVersioning;
			chkEnableContentWorkflow.Checked = selectedSite.EnableContentWorkflow;

			if (WebConfigSettings.EnforceContentVersioningGlobally)
			{
				chkForceContentVersioning.Checked = true;
				chkForceContentVersioning.Enabled = false;
			}
		   
			if (!isAdmin)
			{
				//ddSiteList.Visible = false;
				tabSiteMappings.Visible = false;
				liSiteMappings.Visible = false;
				//tabFolderNames.Visible = false;
				//liFolderNames.Visible = false;
				tabSiteFeatures.Visible = false;
				liSecurity.Visible = false;
				tabSecurity.Visible = false;
				liCommerce.Visible = false;
				tabCommerce.Visible = false;
				liAdvanced.Visible = false;
				tabAdvanced.Visible = false;
				liContent.Visible = false;
				tabContent.Visible = false;
				//divCommerceRoles.Visible = false;
				chkForceContentVersioning.Visible = false;
				chkEnableContentWorkflow.Visible = false;
			}

			chkUseLdapAuth.Checked = selectedSite.UseLdapAuth;
			chkAutoCreateLdapUserOnFirstLogin.Checked = selectedSite.AutoCreateLdapUserOnFirstLogin;
			txtLdapServer.Text = selectedSite.SiteLdapSettings.Server;
			txtLdapPort.Text = selectedSite.SiteLdapSettings.Port.ToString();
			txtLdapDomain.Text = selectedSite.SiteLdapSettings.Domain;
			txtLdapRootDN.Text = selectedSite.SiteLdapSettings.RootDN;

			item = ddLdapUserDNKey.Items.FindByValue(selectedSite.SiteLdapSettings.UserDNKey);
			if (item != null)
			{
				ddLdapUserDNKey.ClearSelection();
				item.Selected = true;
			}

			txtSiteEmailFromAddress.Text = selectedSite.DefaultEmailFromAddress;
			txtSiteEmailFromAlias.Text = selectedSite.DefaultFromEmailAlias;
			txtSMTPHeaders.Text = selectedSite.SMTPCustomHeaders;

			item = ddPasswordFormat.Items.FindByValue(selectedSite.PasswordFormat.ToString(CultureInfo.InvariantCulture));

			if (item != null)
			{
				ddPasswordFormat.ClearSelection();
				item.Selected = true;
			}

			btnEnablePasswordFormatChange.Enabled = allowPasswordFormatChange;
			txtMaxInvalidPasswordAttempts.Text = selectedSite.MaxInvalidPasswordAttempts.ToInvariantString();
			txtPasswordAttemptWindowMinutes.Text = selectedSite.PasswordAttemptWindowMinutes.ToInvariantString();
			chkRequireCaptchaOnLogin.Checked = selectedSite.RequireCaptchaOnLogin;
			

			if (
				(siteSettings.IsServerAdminSite)
				&& (!selectedSite.IsServerAdminSite)
				&& (isAdmin)
				&& (WebConfigSettings.AllowDeletingChildSites)
				)
			{
				btnDelete.Visible = true;
		   
			}

			if (
				(siteSettings.IsServerAdminSite)
				&& (isAdmin)
				&&(selectedSiteID > -1)
				&& (WebConfigSettings.ShowSkinRestoreButtonInSiteSettings)
				)
			{
				btnRestoreSkins.Visible = true;
			}

			if (siteSettings.IsServerAdminSite && isAdmin && WebConfigSettings.AllowForcingPreferredHostName)
			{
				txtPreferredHostName.Enabled = true;

			}
			else
			{
				txtPreferredHostName.Enabled = false;
			}

			if (
				(WebConfigSettings.UseRelatedSiteMode)
				&&((selectedSite.SiteId != WebConfigSettings.RelatedSiteID)||(selectedSiteID == -1))
				)
			{
				if (WebConfigSettings.UseFolderBasedMultiTenants)
				{
					liGeneralSecurity.Visible = false;
					tabGeneralSecurity.Visible = false;
					liLDAP.Visible = false;
					tabLDAP.Visible = false;
					lithirdpartyauth.Visible = false;
					tabthirdpartyauth.Visible = false;
				}
				else
				{
					liGeneralSecurity.Visible = false;
					tabGeneralSecurity.Visible = false;
					liLDAP.Visible = false;
					tabLDAP.Visible = false;
				}

				fgpReallyDeleteUsers.Visible = false; 
			}

			if (txtRpxNowApiKey.Text.Length == 0)
			{
				UIHelper.AddConfirmationDialog(btnSetupRpx, Resource.RpxButtonConfirm);
			}

			DoTabSelection();
			PopulateMailSettings();
			fgpTestSMTPSettings.Visible = !WebConfigSettings.IsDemoSite;

			pageSelector.SetValue(selectedSite.HomePageOverride.ToString());
		}


		private void DoTabSelection()
		{
			switch (requestedTab)
			{
				case "oid":
					if (tabSecurity.Visible)
					{
						liSecurity.Attributes.Add("class", "selected");
						lithirdpartyauth.Attributes.Add("class", "selected");
					}
					else
					{
						liGeneral.Attributes.Add("class", "selected");
					}

					break;
				default:
					liGeneral.Attributes.Add("class", "selected");
					break;
			}
		}

		private void BindGeoLists()
		{
			BindCountryList();
			BindZoneList();
		}

		private void BindCountryList()
		{
			DataTable dataTable = GeoCountry.GetList();
			ddDefaultCountry.DataSource = dataTable;
			ddDefaultCountry.DataBind();

		}

		void ddDefaultCountry_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindZoneList();

		}

		private void BindZoneList()
		{
			if (ddDefaultCountry.SelectedIndex > -1)
			{
				Guid countryGuid = new Guid(ddDefaultCountry.SelectedValue);
				using (IDataReader reader = GeoZone.GetByCountry(countryGuid))
				{
					ddDefaultGeoZone.DataSource = reader;
					ddDefaultGeoZone.DataBind();
				}
			}
		}

		private void DdCaptchaProviders_SelectedIndexChanged(object sender, EventArgs e)
		{
			pnlRecaptchaSettings.Enabled = ddCaptchaProviders.SelectedValue == "RecaptchaCaptchaProvider";
			updCaptcha.Update();
		}

		private void RbRecaptchaHcaptcha_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (selectedSite.CaptchaReCaptchaHCaptcha == rbRecaptchaHcaptcha.SelectedValue)
			{
				txtCaptchaClientScriptUrl.Text = selectedSite.CaptchaClientScriptUrl;
				txtCaptchaVerifyUrl.Text = selectedSite.CaptchaVerifyUrl;
				txtCaptchaTheme.Text = selectedSite.CaptchaTheme;
				txtCaptchaParam.Text = selectedSite.CaptchaParam;
				txtCaptchaResponseField.Text = selectedSite.CaptchaResponseField;
			}
			else if (rbRecaptchaHcaptcha.SelectedValue == "hcaptcha")
			{
				txtCaptchaClientScriptUrl.Text = WebConfigSettings.HCaptchaDefaultClientScriptUrl;
				txtCaptchaVerifyUrl.Text = WebConfigSettings.HCaptchaDefaultVerifyUrl;
				txtCaptchaTheme.Text = WebConfigSettings.HCaptchaDefaultTheme;
				txtCaptchaParam.Text = WebConfigSettings.HCaptchaDefaultParam;
				txtCaptchaResponseField.Text = WebConfigSettings.HCaptchaDefaultResponseField;
			}
			else if (rbRecaptchaHcaptcha.SelectedValue == "recaptcha")
			{
				txtCaptchaClientScriptUrl.Text = WebConfigSettings.ReCaptchaDefaultClientScriptUrl;
				txtCaptchaVerifyUrl.Text = WebConfigSettings.ReCaptchaDefaultVerifyUrl;
				txtCaptchaTheme.Text = WebConfigSettings.ReCaptchaDefaultTheme;
				txtCaptchaParam.Text = WebConfigSettings.ReCaptchaDefaultParam;
				txtCaptchaResponseField.Text = WebConfigSettings.ReCaptchaDefaultResponseField;
			}

			updCaptcha.Update();
		}
		private void btnResetRecaptchaHcaptchaDefaults_Click(object sender, EventArgs e)
		{
			if (rbRecaptchaHcaptcha.SelectedValue == "hcaptcha")
			{
				txtCaptchaClientScriptUrl.Text = WebConfigSettings.HCaptchaDefaultClientScriptUrl;
				txtCaptchaVerifyUrl.Text = WebConfigSettings.HCaptchaDefaultVerifyUrl;
				txtCaptchaTheme.Text = WebConfigSettings.HCaptchaDefaultTheme;
				txtCaptchaParam.Text = WebConfigSettings.HCaptchaDefaultParam;
				txtCaptchaResponseField.Text = WebConfigSettings.HCaptchaDefaultResponseField;
			}
			else if (rbRecaptchaHcaptcha.SelectedValue == "recaptcha")
			{
				txtCaptchaClientScriptUrl.Text = WebConfigSettings.ReCaptchaDefaultClientScriptUrl;
				txtCaptchaVerifyUrl.Text = WebConfigSettings.ReCaptchaDefaultVerifyUrl;
				txtCaptchaTheme.Text = WebConfigSettings.ReCaptchaDefaultTheme;
				txtCaptchaParam.Text = WebConfigSettings.ReCaptchaDefaultParam;
				txtCaptchaResponseField.Text = WebConfigSettings.ReCaptchaDefaultResponseField;
			}

			updCaptcha.Update();
		}

		private void PopulateMultiSiteControls()
		{
			int countOfOtherSites = SiteSettings.GetCountOfOtherSites(siteSettings.SiteId);

			if(countOfOtherSites > 0 || selectedSiteID == -1)
			{
				
				if (WebConfigSettings.UseFolderBasedMultiTenants)
				{
					PopulateFolderList();
					litHostMessage.Text = string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsHostNamesTurnedOff);
					pnlAddHostName.Visible = false;
					rptHosts.Visible = false;
				}
				else
				{
					PopulateHostList();
					litFolderMessage.Text = string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsFolderNamesTurnedOff);
					pnlAddFolder.Visible = false;
					rptFolderNames.Visible = false;
				}

				if (!selectedSite.IsServerAdminSite)
				{
					PopulateFeatures();
				}
			}
			else
			{
				pnlAddHostName.Visible = false;
				rptHosts.Visible = false;
				upFolderNames.Visible = false; // we can hide the entire thing because we're not using any of it

				//set the host list header and message to state there aren't other sites.
				litHostListHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsMultiTenancyNoOtherSitesLabel, string.Empty);
				litHostMessage.Text = string.Format(displaySettings.AlertNoticeMarkup, string.Format(Resource.SiteSettingsMultiTenancyNoOtherSites, SiteRoot + "/Admin/SiteSettings.aspx?SiteID=-1"));
			}
		}

		private void PopulateFeatures()
		{
			liFeatures.Visible = true;
			tabSiteFeatures.Visible = true;
			lstAllFeatures.Items.Clear();
			lstSelectedFeatures.Items.Clear();

			ListItem listItem;
			using (IDataReader reader = ModuleDefinition.GetModuleDefinitions(this.currentSiteGuid))
			{
				while (reader.Read())
				{
					listItem = new ListItem(
						ResourceHelper.GetResourceString(
						reader["ResourceFile"].ToString(),
						reader["FeatureName"].ToString()),
						reader["Guid"].ToString());

					lstAllFeatures.Items.Add(listItem);
				}
			}

			using (IDataReader reader = ModuleDefinition.GetModuleDefinitions(selectedSite.SiteGuid))
			{
				while (reader.Read())
				{
					ListItem matchItem = lstAllFeatures.Items.FindByValue(reader["Guid"].ToString());
					if (matchItem != null)
					{
						lstSelectedFeatures.Items.Add(matchItem);
						lstAllFeatures.Items.Remove(matchItem);
					}
				}
			}

			btnAddFeature.Enabled = lstAllFeatures.Items.Count > 0;
			btnRemoveFeature.Enabled = lstSelectedFeatures.Items.Count > 0;
		}

		private void PopulateHostList()
		{
			if (selectedSiteID == -1)
			{
				// site must be created first
				litHostMessage.Text = string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsHostNamesAfterSiteCreated);
				rptHosts.Visible = false;
				pnlAddHostName.Visible = false;
				return;
			} 

			using (IDataReader reader = SiteSettings.GetHostList(selectedSite.SiteId))
			{
				rptHosts.DataSource = reader;
				rptHosts.DataBind();
			}
			if (rptHosts.Items.Count > 0)
			{
				rptHosts.Visible = true;
			}
			else
			{
				litHostMessage.Text = string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsNoHostsFound);
				rptHosts.Visible = false;
			}
		}

		private void PopulateFolderList()
		{
			if (selectedSiteID == -1)
			{
				// site must be created first
				litFolderMessage.Text = string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsFolderNamesAfterSiteCreated);
				rptFolderNames.Visible = false;
				pnlAddFolder.Visible = false;

				return;
			}

			if (selectedSite.IsServerAdminSite)
			{
				// no folders can map to root site
				rptFolderNames.Visible = false;
				pnlAddFolder.Visible = false;

				litFolderMessage.Text = string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsFolderNamesNotAllowedOnAdminSite);
			}
			else
			{
				List<SiteFolder> siteFolders = SiteFolder.GetBySite(selectedSite.SiteGuid);
				rptFolderNames.DataSource = siteFolders;
				rptFolderNames.DataBind();
				if (rptFolderNames.Items.Count > 0)
				{
					rptFolderNames.Visible = true;
				}
				else
				{
					litFolderMessage.Text = string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsNoFolderNames);
					rptFolderNames.Visible = false;
				}
			}
		}

		private void PopulateMailSettings()
		{
			if (this.selectedSiteID == -1)
			{
				//new site
				pnlSMTPSettingsWrapper.Visible = false;
				pnlSMTPSettingsWrapper.DontRender = true;
				if (enableSiteSettingsSmtpSettings)
				{
					litSMTPSettingsHeader.Text += string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsSMTPSettingsAfterSiteCreated);
				}
				else
				{
					litSMTPSettingsHeader.Text += string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsSMTPSettingsDisabled);
				}
				return;
			}

			if (enableSiteSettingsSmtpSettings)
			{
				fgpSMTPEncoding.Visible = WebConfigSettings.ShowSmtpEncodingOption;

				if (WebConfigSettings.UseLegacyCryptoHelper)
				{
					if (selectedSite.SMTPUser.Length > 0)
					{
						try
						{
							txtSMTPUser.Text = SiteUtils.Decrypt(selectedSite.SMTPUser);
						}
						catch (System.Security.Cryptography.CryptographicException)
						{
							txtSMTPUser.Text = selectedSite.SMTPUser;
						}
					}
					if (selectedSite.SMTPPassword.Length > 0)
					{
						try
						{
							txtSMTPPassword.Text = SiteUtils.Decrypt(selectedSite.SMTPPassword);
						}
						catch (System.Security.Cryptography.CryptographicException)
						{
							txtSMTPPassword.Text = selectedSite.SMTPPassword;
						}
					}
				}
				else
				{
					if (selectedSite.SMTPUser.Length > 0)
					{
						try
						{
							txtSMTPUser.Text = SiteUtils.Decrypt(selectedSite.SMTPUser);
						}
						catch (FormatException)
						{
							txtSMTPUser.Text = selectedSite.SMTPUser;
						}
					}
					if (selectedSite.SMTPPassword.Length > 0)
					{
						try
						{
							txtSMTPPassword.Text = SiteUtils.Decrypt(selectedSite.SMTPPassword);
						}
						catch (FormatException)
						{
							txtSMTPPassword.Text = selectedSite.SMTPPassword;
						}
					}
				}

				txtSMTPServer.Text = selectedSite.SMTPServer;
				txtSMTPPort.Text = selectedSite.SMTPPort.ToString();
				chkSMTPUseSsl.Checked = selectedSite.SMTPUseSsl;
				txtSMTPPreferredEncoding.Text = selectedSite.SMTPPreferredEncoding;

				if (selectedSite.SMTPRequiresAuthentication)
				{
					chkSMTPRequiresAuthentication.Checked = true;
					txtSMTPPassword.Enabled = true;
					txtSMTPUser.Enabled = true;
				}
				else
				{
					chkSMTPRequiresAuthentication.Checked = false;
					txtSMTPPassword.Enabled = false;
					txtSMTPUser.Enabled = false;
				}
			}
			else
			{
				pnlSMTPSettingsWrapper.Visible = false;
				litSMTPSettingsHeader.Text += string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsSMTPSettingsDisabled);
			}


		}

		private void SetMailSettings()
		{
			if (selectedSite.SiteId == -1) { return; }

			selectedSite.DefaultEmailFromAddress = txtSiteEmailFromAddress.Text;
			selectedSite.DefaultFromEmailAlias = txtSiteEmailFromAlias.Text;
			selectedSite.SMTPCustomHeaders = txtSMTPHeaders.Text;

			if (!enableSiteSettingsSmtpSettings) { return; }

			if (WebConfigSettings.UseLegacyCryptoHelper)
			{
				if (txtSMTPUser.Text.Length > 0)
				{
					selectedSite.SMTPUser = SiteUtils.Encrypt(txtSMTPUser.Text);
				}
				else
				{
					selectedSite.SMTPUser = string.Empty;
				}
				if (txtSMTPPassword.Text.Length > 0)
				{
					selectedSite.SMTPPassword = SiteUtils.Encrypt(txtSMTPPassword.Text);
				}
				else
				{
					if (!maskSMTPPassword)
					{
						selectedSite.SMTPPassword = string.Empty;
					}
				}
			}
			else
			{
				if (txtSMTPUser.Text.Length > 0)
				{
					selectedSite.SMTPUser = SiteUtils.Encrypt(txtSMTPUser.Text);
				}
				else
				{
					selectedSite.SMTPUser = string.Empty;
				}
				if (txtSMTPPassword.Text.Length > 0)
				{
					selectedSite.SMTPPassword = SiteUtils.Encrypt(txtSMTPPassword.Text);
				}
				else
				{
					if (!maskSMTPPassword)
					{
						selectedSite.SMTPPassword = string.Empty;
					}
				}
			}

			selectedSite.SMTPServer = txtSMTPServer.Text;
			int port = 25;
			int.TryParse(txtSMTPPort.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out port);
			selectedSite.SMTPPort = port;
			selectedSite.SMTPRequiresAuthentication = chkSMTPRequiresAuthentication.Checked;
			selectedSite.SMTPUseSsl = chkSMTPUseSsl.Checked;
			selectedSite.SMTPPreferredEncoding = txtSMTPPreferredEncoding.Text;
		}
		
		protected void btnSave_Click(Object sender, EventArgs e)
		{
			Page.Validate("sitesettings");
			if (!Page.IsValid) { return; }

			bool creatingNewSite = false;
			if (IsServerAdmin)
			{
				if (isAdmin)
				{
					if (selectedSiteID == -1)
					{
						selectedSite = new SiteSettings(selectedSiteID);
						creatingNewSite = true;
					}
				}
			}
			
			selectedSite.SiteName = txtSiteName.Text.Trim();
			selectedSite.Slogan = txtSlogan.Text;
			selectedSite.CompanyName = txtCompanyName.Text;
			selectedSite.CompanyStreetAddress = txtStreetAddress.Text;
			selectedSite.CompanyStreetAddress2 = txtStreetAddress2.Text;
			selectedSite.CompanyLocality = txtLocality.Text;
			selectedSite.CompanyRegion = txtRegion.Text;
			selectedSite.CompanyPostalCode = txtPostalCode.Text;
			selectedSite.CompanyCountry = txtCountry.Text;
			selectedSite.CompanyPhone = txtPhone.Text;
			selectedSite.CompanyFax = txtFax.Text;
			selectedSite.CompanyPublicEmail = txtPublicEmail.Text;

			selectedSite.PrivacyPolicyUrl = txtPrivacyPolicyUrl.Text;

			selectedSite.BingAPIId = txtBingSearchAPIKey.Text;
			selectedSite.GoogleCustomSearchId = txtGoogleCustomSearchId.Text;
			selectedSite.ShowAlternateSearchIfConfigured = chkShowAlternateSearchIfConfigured.Checked;
			selectedSite.PrimarySearchEngine = ddSearchEngine.SelectedValue;

			ISettingControl setting = timeZone as ISettingControl;
			if (setting != null)
			{
				selectedSite.TimeZoneId = setting.GetValue();
			}
			
			selectedSite.Logo = ddLogos.SelectedValue;
			selectedSite.Skin = SkinSetting.GetValue();

			if (ddMobileSkin.Enabled)
			{
				selectedSite.MobileSkin = ddMobileSkin.SelectedValue;
			}

			if (ddEditorProviders.SelectedIndex > -1)
			{
				selectedSite.EditorProviderName = ddEditorProviders.SelectedValue;
			}

			if (ddNewsletterEditor.SelectedIndex > -1)
			{
				selectedSite.NewsletterEditor = ddNewsletterEditor.SelectedValue;
			}
			
			selectedSite.AvatarSystem = ddAvatarSystem.SelectedValue;

			selectedSite.DefaultFriendlyUrlPattern = (SiteSettings.FriendlyUrlPattern)Enum.Parse(typeof(SiteSettings.FriendlyUrlPattern), ddDefaultFriendlyUrlPattern.SelectedValue);

			if (ddCaptchaProviders.SelectedIndex > -1)
			{
				selectedSite.CaptchaProvider = ddCaptchaProviders.SelectedValue;
			}

			if (ddDefaultCountry.SelectedValue.Length == 36)
			{
				selectedSite.DefaultCountryGuid = new Guid(ddDefaultCountry.SelectedValue);
			}

			if (ddDefaultGeoZone.SelectedValue.Length == 36)
			{
				selectedSite.DefaultStateGuid = new Guid(ddDefaultGeoZone.SelectedValue);
			}

			selectedSite.RecaptchaPrivateKey = txtRecaptchaPrivateKey.Text;
			selectedSite.RecaptchaPublicKey = txtRecaptchaPublicKey.Text;
			selectedSite.CaptchaReCaptchaHCaptcha = rbRecaptchaHcaptcha.SelectedValue;
			selectedSite.CaptchaClientScriptUrl = txtCaptchaClientScriptUrl.Text;
			selectedSite.CaptchaVerifyUrl = txtCaptchaVerifyUrl.Text;
			selectedSite.CaptchaParam = txtCaptchaParam.Text;
			selectedSite.CaptchaResponseField = txtCaptchaResponseField.Text;
			selectedSite.CaptchaTheme = txtCaptchaTheme.Text;
			selectedSite.BadWordList = txtBadWordList.Text;
			selectedSite.GmapApiKey = txtGmapApiKey.Text;
			selectedSite.AddThisDotComUsername = txtAddThisUserId.Text;
			selectedSite.GoogleAnalyticsAccountCode = txtGoogleAnayticsAccountCode.Text;
			selectedSite.OpenIdSelectorId = txtOpenIdSelectorCode.Text;
			selectedSite.CommentProvider = ddCommentSystem.SelectedValue;
			selectedSite.IntenseDebateAccountId = txtIntenseDebateAccountId.Text;
			selectedSite.DisqusSiteShortName = txtDisqusSiteShortName.Text;
			selectedSite.FacebookAppId = txtFacebookAppId.Text;
			selectedSite.SiteWideHeaderContent = txtHeaderContent.Text;
			selectedSite.SiteWideFooterContent = txtFooterContent.Text;
			selectedSite.SiteWideHeaderAdminContent = txtHeaderAdminContent.Text;
			selectedSite.SiteWideFooterAdminContent = txtFooterAdminContent.Text;

			if (fgpWoopra.Visible)
			{
				selectedSite.EnableWoopra = chkEnableWoopra.Checked;
			}

			if (fgpSiteIsClosed.Visible)
			{
				selectedSite.SiteIsClosed = chkSiteIsClosed.Checked;
			}

			// keep track if password format changed then we need to update passwords to new format
			int previousPasswordFormat = selectedSite.PasswordFormat;

			if (isAdmin)
			{
				selectedSite.PreferredHostName = txtPreferredHostName.Text.Replace("https://", string.Empty).Replace("http://",string.Empty).Replace("/", string.Empty);
				selectedSite.HomePageOverride = Convert.ToInt32(pageSelector.GetValue());
				if (WebConfigSettings.EnableOpenIdAuthentication)
				{
					selectedSite.AllowOpenIdAuth = chkAllowOpenIDAuth.Checked;
				}
				//selectedSite.HomePageOverride = 1;
				if (WebConfigSettings.EnableWindowsLiveAuthentication)
				{
					selectedSite.AllowWindowsLiveAuth = chkAllowWindowsLiveAuth.Checked;
					selectedSite.WindowsLiveAppId = txtWindowsLiveAppID.Text;
					selectedSite.WindowsLiveKey = txtWindowsLiveKey.Text;
				}

				selectedSite.DisableDbAuth = chkDisableDbAuthentication.Checked;

				selectedSite.AllowWindowsLiveMessengerForMembers = chkAllowWindowsLiveMessengerForMembers.Checked;
				selectedSite.AppLogoForWindowsLive = txtAppLogoForWindowsLive.Text;

				selectedSite.RpxNowApiKey = txtRpxNowApiKey.Text;
				selectedSite.RpxNowApplicationName = txtRpxNowApplicationName.Text;
				if (selectedSite.RpxNowApiKey.Length == 0) { selectedSite.RpxNowAdminUrl = string.Empty; }

				selectedSite.OpenSearchName = txtOpenSearchName.Text;

				selectedSite.AllowUserSkins = chkAllowUserSkins.Checked;
				selectedSite.AllowPageSkins = chkAllowPageSkins.Checked;
				selectedSite.AllowHideMenuOnPages = chkAllowHideMenuOnPages.Checked;
				selectedSite.UseSecureRegistration = chkSecureRegistration.Checked;
				selectedSite.RequireApprovalBeforeLogin = chkRequireApprovalForLogin.Checked;
				selectedSite.EmailAdressesForUserApprovalNotification = txtEmailAdressesForUserApprovalNotification.Text;
				selectedSite.AllowPersistentLogin = chkAllowPersistentLogin.Checked;
				selectedSite.ForceContentVersioning = chkForceContentVersioning.Checked;
				selectedSite.EnableContentWorkflow = chkEnableContentWorkflow.Checked;

				ISettingControl currencySetting = SiteCurrencySetting as ISettingControl;
				string currencyGuidString = currencySetting.GetValue();
				if (currencyGuidString.Length == 36)
				{
					selectedSite.CurrencyGuid = new Guid(currencyGuidString);
				}

				if (sslIsAvailable)
				{
					selectedSite.UseSslOnAllPages = WebConfigSettings.ForceSslOnAllPages || chkRequireSSL.Checked;
				}	

				if (chkAllowRegistration.Enabled && fgpAllowRegistration.Visible && tabGeneralSecurity.Visible)
				{
					selectedSite.AllowNewRegistration = chkAllowRegistration.Checked;
				}
				else
				{
					if (chkUseLdapAuth.Checked && !selectedSite.AllowDbFallbackWithLdap)
					{
						selectedSite.AllowNewRegistration = false;
					}
				}

				if (WebConfigSettings.UseRelatedSiteMode
				&& (selectedSite.SiteId != WebConfigSettings.RelatedSiteID) && (selectedSiteID != -1)
				)
				{
					//don't change this on child sites in related sites mode
				}
				else
				{
					if (chkAllowUserToChangeName.Enabled && fgpAllowUserToChangeName.Visible)
					{
						selectedSite.AllowUserFullNameChange = chkAllowUserToChangeName.Checked;
					}

					if (chkUseEmailForLogin.Enabled && fgpUseEmailForLogin.Visible)
					{
						selectedSite.UseEmailForLogin = chkUseEmailForLogin.Checked;
					}
				}

				selectedSite.AutoCreateLdapUserOnFirstLogin = chkAutoCreateLdapUserOnFirstLogin.Checked;
				selectedSite.AllowDbFallbackWithLdap = chkAllowDbFallbackWithLdap.Checked;
				selectedSite.AllowEmailLoginWithLdapDbFallback = chkAllowEmailLoginWithLdapDbFallback.Checked;

				if (!selectedSite.UseLdapAuth && chkUseLdapAuth.Checked && !creatingNewSite)
				{
					LdapSettings testLdapSettings = new LdapSettings
					{
						Server = txtLdapServer.Text,
						Port = Convert.ToInt32(txtLdapPort.Text),
						Domain = txtLdapDomain.Text,
						RootDN = txtLdapRootDN.Text,
						UserDNKey = ddLdapUserDNKey.SelectedValue
					};

					if (!TestCurrentUserLdap(testLdapSettings))
					{
						lblErrorMessage.Text += "  " + Resource.SiteSettingsLDAPAdminUserNotFound;
						btnSave.Text = Resource.SiteSettingsApplyChangesButton;
						btnSave.Enabled = true;
						return;
					}
				}

				if (WebConfigSettings.UseRelatedSiteMode && selectedSite.SiteId != WebConfigSettings.RelatedSiteID && selectedSiteID != -1)
				{
					tabLDAP.Visible = false;
				}

				if (selectedSite.SiteId > -1)
				{
					if (tabLDAP.Visible)
					{
						if (fgpUseLdap.Visible)
						{
							selectedSite.UseLdapAuth = chkUseLdapAuth.Checked;
						}

						if (fgpLdapServer.Visible)
						{
							selectedSite.SiteLdapSettings.Server = txtLdapServer.Text;
						}

						if (fgpLdapPort.Visible && (txtLdapPort.Text.Length > 0))
						{
							int port = 389;
							int.TryParse(txtLdapPort.Text, out port);
							selectedSite.SiteLdapSettings.Port = port;
						}

						if (fgpLdapDomain.Visible)
						{
							selectedSite.SiteLdapSettings.Domain = txtLdapDomain.Text;
						}

						if (fgpLdapRootDn.Visible)
						{
							selectedSite.SiteLdapSettings.RootDN = txtLdapRootDN.Text;
						}

						if (fgpLdapUserDNKey.Visible)
						{
							selectedSite.SiteLdapSettings.UserDNKey = ddLdapUserDNKey.SelectedValue;
						}
					}
				}

				if (selectedSite.UseLdapAuth && !selectedSite.AllowDbFallbackWithLdap)
				{
					selectedSite.ReallyDeleteUsers = false;
				}
				else
				{
					selectedSite.ReallyDeleteUsers = chkReallyDeleteUsers.Checked;
				}

				if (WebConfigSettings.UseRelatedSiteMode && selectedSite.SiteId != WebConfigSettings.RelatedSiteID && selectedSiteID != -1)
				{
					//don't change this on child sites in related sites mode
				}
				else
				{
					if (allowPasswordFormatChange && int.Parse(ddPasswordFormat.SelectedValue) != selectedSite.PasswordFormat && ddPasswordFormat.Enabled || (selectedSite.SiteGuid == Guid.Empty))
					{
						try
						{
							selectedSite.PasswordFormat = int.Parse(ddPasswordFormat.SelectedValue);
						}
						catch (ArgumentException) { }
						catch (FormatException) { }
					}
					 
					selectedSite.AllowPasswordRetrieval = chkAllowPasswordRetrieval.Checked;
					selectedSite.RequiresQuestionAndAnswer = chkRequiresQuestionAndAnswer.Checked;
					selectedSite.AllowPasswordReset = chkAllowPasswordReset.Checked;
					selectedSite.RequirePasswordChangeOnResetRecover = chkRequirePasswordChangeAfterRecovery.Checked;

					int MaxInvalidPasswordAttempts = selectedSite.MaxInvalidPasswordAttempts;
					int.TryParse(txtMaxInvalidPasswordAttempts.Text, out MaxInvalidPasswordAttempts);
					selectedSite.MaxInvalidPasswordAttempts = MaxInvalidPasswordAttempts;

					int PasswordAttemptWindowMinutes = selectedSite.PasswordAttemptWindowMinutes;
					int.TryParse(txtPasswordAttemptWindowMinutes.Text, out PasswordAttemptWindowMinutes);
					selectedSite.PasswordAttemptWindowMinutes = PasswordAttemptWindowMinutes;

					int MinRequiredPasswordLength = selectedSite.MinRequiredPasswordLength;
					int.TryParse(txtMinimumPasswordLength.Text, out MinRequiredPasswordLength);
					selectedSite.MinRequiredPasswordLength = MinRequiredPasswordLength;

					int MinRequiredNonAlphanumericCharacters = selectedSite.MinRequiredNonAlphanumericCharacters;
					int.TryParse(txtMinRequiredNonAlphaNumericCharacters.Text, out MinRequiredNonAlphanumericCharacters);
					selectedSite.MinRequiredNonAlphanumericCharacters = MinRequiredNonAlphanumericCharacters;

					selectedSite.PasswordStrengthRegularExpression = txtPasswordStrengthRegularExpression.Text.Trim();
					selectedSite.PasswordRegexWarning = txtPasswordStrengthErrorMessage.Text.Trim();
					selectedSite.ShowPasswordStrengthOnRegistration = chkShowPasswordStrength.Checked;
					selectedSite.RequireCaptchaOnRegistration = chkRequireCaptcha.Checked;
					selectedSite.RequireCaptchaOnLogin = chkRequireCaptchaOnLogin.Checked;
					selectedSite.BadWordCheckingEnforced = chkForceBadWordChecking.Checked;
					selectedSite.RequireEnterEmailTwiceOnRegistration = chkRequireEmailTwice.Checked;
				}
			} //end isAdmin

			selectedSite.AllowUserEditorPreference = chkAllowUserEditorChoice.Checked;
			selectedSite.MetaProfile = txtMetaProfile.Text;

			SetMailSettings();

			// the site may previously have been using email for login
			//but we need to make sure it uses login name in case using ldap as fallback authentication
			if (selectedSite.UseLdapAuth) { selectedSite.UseEmailForLogin = false; }

			if (creatingNewSite)
			{
				selectedSite.SiteCreated += new SiteCreatedEventHandler(siteSettings_SiteCreated);
			}

			selectedSite.Save();

			if (creatingNewSite)
			{
				mojoSetup.CreateNewSiteData(selectedSite);
			}

			CacheHelper.ClearSiteSettingsCache(selectedSite.SiteId);

			mojoMembershipProvider mojoMembership = (mojoMembershipProvider)Membership.Provider;
			
			if ((!creatingNewSite) && (previousPasswordFormat != selectedSite.PasswordFormat))
			{
				// this is not something you want to change very often
				mojoMembership.ChangeUserPasswordFormat(selectedSite, previousPasswordFormat);
				CacheHelper.ClearSiteSettingsCache(selectedSite.SiteId);
			}
			
			string oldSkin = hdnCurrentSkin.Value;
			if ((oldSkin != selectedSite.Skin)&&(WebConfigSettings.UseCacheDependencyFiles))
			{
				CacheHelper.ResetThemeCache();
			}

			if (WebConfigSettings.UseRelatedSiteMode)
			{
				// need to propagate any security changes to all child sites
				// reset the sitesettings cache for each site
				if (creatingNewSite)
				{
					SiteSettings masterSite = CacheHelper.GetSiteSettings(WebConfigSettings.RelatedSiteID);
					// siteSettings is the master site we need some permissions from it synced to the new site
					SiteSettings.SyncRelatedSites(masterSite, WebConfigSettings.UseFolderBasedMultiTenants);
				}
				else
				{
					SiteSettings.SyncRelatedSites(selectedSite, WebConfigSettings.UseFolderBasedMultiTenants);
				}

				// reset the sitesettings cache for each site
				CacheHelper.ClearRelatedSiteCache(-1);
			}

			string redirectUrl = SiteRoot + "/Admin/SiteSettings.aspx?SiteID=" + selectedSite.SiteId.ToString();

			if (selectedSite.SiteId == currentSiteID)
			{
				redirectUrl = Request.RawUrl;
			}

			WebUtils.SetupRedirect(this, redirectUrl);
		}

		void siteSettings_SiteCreated(object sender, SiteCreatedEventArgs e)
		{
			// this is a hook so that custom code can be fired when sites are created
			// implement a SiteCreatedEventHandlerProvider and put a config file for it in
			// /Setup/ProviderConfig/sitecreatedeventhandlers
			try
			{
				foreach (SiteCreatedEventHandlerProvider handler in SiteCreatedEventHandlerProviderManager.Providers)
				{
					handler.SiteCreatedHandler(sender, e);
				}
			}
			catch (TypeInitializationException ex)
			{
				log.Error(ex);
			}
		}

		private bool TestCurrentUserLdap(LdapSettings testLdapSettings)
		{
			string uid = Context.User.Identity.Name;
			SiteUser user = new SiteUser(this.selectedSite, uid);
			return LdapHelper.TestUser(testLdapSettings, user.LoginName, txtLdapTestPassword.Text);
		}

		private void btnTestSMTPSettings_Click(object sender, EventArgs e)
		{
			string validFormat = displaySettings.AlertNoticeMarkup;
			string invalidFormat = displaySettings.AlertErrorMarkup;

			btnTestSMTPSettings.Text = Resource.SiteSettingsTestSMTPSettingsButtonSending;
			btnTestSMTPSettings.Enabled = false;

			SmtpSettings smtpSettings = new();
			SmtpSettings savedSmtpSettings = SiteUtils.GetSmtpSettings();

			if (!WebConfigSettings.EnableSiteSettingsSmtpSettings)
			{
				smtpSettings = savedSmtpSettings;
			} 
			else if (WebConfigSettings.EnableSiteSettingsSmtpSettings)
			{
				smtpSettings = new SmtpSettings
				{
					Server = txtSMTPServer.Text,
					PreferredEncoding = txtSMTPPreferredEncoding.Text,
					UseSsl = chkSMTPUseSsl.Checked
				};

				try
				{
					smtpSettings.Port = Convert.ToInt32(txtSMTPPort.Text);
				}
				catch (FormatException)
				{
					litTestSMTPResult.Text = string.Format(invalidFormat, $"{Resource.SiteSettingsTestSMTPSettingsInvalidMessageDetailed}: 'Port invalid'");
					ResetButton();
					return;
				}

				if (chkSMTPRequiresAuthentication.Checked)
				{
					smtpSettings.RequiresAuthentication = true;
					smtpSettings.User = txtSMTPUser.Text;
					if (string.IsNullOrWhiteSpace(txtSMTPPassword.Text))
					{
						smtpSettings.Password = savedSmtpSettings.Password;
					}
					else
					{
						smtpSettings.Password = txtSMTPPassword.Text;
					}
				}
			}



			foreach (var header in txtSMTPHeaders.Text.SplitOnNewLineAndTrim())
			{
				var keyFinalIndex = header.IndexOf(':');
				var key = header.Substring(0, keyFinalIndex).Trim();
				var val = header.Substring(keyFinalIndex + 1).Trim();
				smtpSettings.AdditionalHeaders.Add(new SmtpHeader { Name = key, Value = val});
				log.Info($"smtp header info [{key},{val}]");
			}

			if (smtpSettings.IsValid)
			{
				string msg = ResourceHelper.GetMessageTemplate("TestEmailSettings.config");
				string subj = ResourceHelper.GetMessageTemplate("TestEmailSettingsSubject.config");
				if (string.IsNullOrWhiteSpace(subj))
				{
					subj = $"{siteSettings.SiteName} Email Test";
				}
				else
				{
					subj = subj.Replace("{SiteName}", siteSettings.SiteName);
				}

				StringBuilder message = new StringBuilder();
				message.Append(string.IsNullOrWhiteSpace(msg) ? "If you're reading this, your email settings on your website are working fine." : msg);
				message.Replace("{SiteName}", siteSettings.SiteName);
				message.Replace("{AdminEmail}", txtSiteEmailFromAddress.Text);
				string resultMessage = string.Empty;
				bool result = Email.Send(
					smtpSettings,
					txtSiteEmailFromAddress.Text,
					txtSiteEmailFromAlias.Text,
					string.Empty,
					txtTestSMTPEmailAddress.Text,
					string.Empty,
					string.Empty,
					subj,
					message.ToString(),
					false,
					"Normal",
					out resultMessage);
				if (result)
				{
					litTestSMTPResult.Text = string.Format(validFormat, Resource.SiteSettingsTestSMTPSettingsValidMessage);
					ResetButton();
					return;
				}
				else
				{
					//the mojoPortal.Net.Email class returns the messagebody with the error message so we need to strip it with a Replace below
					litTestSMTPResult.Text = string.Format(invalidFormat, $"{Resource.SiteSettingsTestSMTPSettingsInvalidMessageDetailed}<br>{resultMessage.Replace(message.ToString(),"")}");
					ResetButton();
					return;
				}
			}
			else
			{
				litTestSMTPResult.Text = string.Format(invalidFormat, Resource.SiteSettingsTestSMTPSettingsInvalidMessage);
				ResetButton();
				return;
			}

			void ResetButton()
			{
				btnTestSMTPSettings.Text = Resource.SiteSettingsTestSMTPSettingsButton;
				btnTestSMTPSettings.Enabled = true;
			}
		}

		private void btnEnablePasswordFormatChange_Click(object sender, EventArgs e)
		{
			if (allowPasswordFormatChange)
			{
				ddPasswordFormat.Enabled = true;
				upPasswordFormat.Update();
				return;
			}
		}
		private void btnAddFeature_Click(object sender, EventArgs e)
		{
			if (lstAllFeatures.SelectedIndex > -1)
			{
				foreach (ListItem item in lstAllFeatures.Items)
				{
					if(item.Selected)
						SiteSettings.AddFeature(selectedSite.SiteGuid, new Guid(item.Value));
				}

				PopulateFeatures();
				upFeatures.Update();
			}
			else
			{
				litFeatureMessage.Text = string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsSelectFeatureToAddWarning);
			}
		}

		private void btnRemoveFeature_Click(object sender, EventArgs e)
		{
			if (lstSelectedFeatures.SelectedIndex > -1)
			{
				foreach (ListItem item in lstSelectedFeatures.Items)
				{
					if (item.Selected)
						SiteSettings.RemoveFeature(selectedSite.SiteGuid, new Guid(item.Value));
				}

				PopulateFeatures();
				upFeatures.Update();
			}
			else
			{
				litFeatureMessage.Text = string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsSelectFeatureToRemoveWarning);
			}
		}

		private void btnAddHost_Click(object sender, EventArgs e)
		{
			if (selectedSite == null) { return; }

			litHostMessage.Text = string.Empty;

			if (this.txtHostName.Text.Length == 0)
			{
				litHostMessage.Text = string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsHostNameRequiredMessage);
				return;
			}

			if (SiteSettings.HostNameExists(txtHostName.Text))
			{
				litHostMessage.Text = string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsDuplicateHostsWarning);
				return;
			}

			try
			{
				SiteSettings.AddHost(selectedSite.SiteGuid, selectedSite.SiteId, this.txtHostName.Text.ToLower());
				CacheHelper.ClearSiteSettingsCache(selectedSite.SiteId);
				CacheHelper.ClearSiteSettingsCache(siteSettings.SiteId); // this clears it from the defaut site which would use any host if it is not already assigned
			}
			catch (DbException)
			{
				litHostMessage.Text = string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsDuplicateHostsWarning);
			}

			PopulateHostList(); 

			upHosts.Update();
		}

		void rptHosts_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			int hostID = int.Parse(e.CommandArgument.ToString());

			switch (e.CommandName)
			{
				case "delete":
					SiteSettings.RemoveHost(hostID);
					if (selectedSite != null)
					{
						CacheHelper.ClearSiteSettingsCache(selectedSite.SiteId);
						CacheHelper.ClearSiteSettingsCache(siteSettings.SiteId); // this clears it from the defaut site which would use any host if it is not already assigned
					}
					break;

				default:
					break;
			}

			PopulateHostList();
			upHosts.Update();
			

		}

		void btnAddFolder_Click(object sender, EventArgs e)
		{
			litFolderMessage.Text = string.Empty;

			if (txtFolderName.Text.Length > 0)
			{
				if (SiteFolder.Exists(txtFolderName.Text))
				{
					litFolderMessage.Text = string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsFolderNameAlreadyInUseWarning);
					return;
				}

				if (!SiteFolder.IsAllowedFolder(txtFolderName.Text))
				{
					litFolderMessage.Text = string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsFolderNameNotAllowedWarning);
					return;
				}

				if (SiteFolder.HasInvalidChars(txtFolderName.Text))
				{
					litFolderMessage.Text = string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsFolderNameInvalidCharsWarning);
					return;
				}

				SiteFolder siteFolder = new SiteFolder
				{
					SiteGuid = selectedSite.SiteGuid,
					FolderName = txtFolderName.Text
				};
				siteFolder.Save();

				PopulateFolderList();
				upFolderNames.Update();
			}
			else
			{
				litFolderMessage.Text = string.Format(displaySettings.AlertNoticeMarkup, Resource.SiteSettingsFolderNameBlankWarning);
			}
		}

		void rptFolderNames_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			Guid folderGuid = new Guid(e.CommandArgument.ToString());

			switch (e.CommandName)
			{
				case "delete":
					SiteFolder.Delete(folderGuid);
					break;

				default:
					break;
			}

			PopulateFolderList();
			upFolderNames.Update();
		}

		void rptFolderNames_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			ImageButton btnDelete = e.Item.FindControl("btnDeleteFolder") as ImageButton;
			UIHelper.AddConfirmationDialog(btnDelete, Resource.SiteSettingsDeleteFolderMappingWarning);
		}

		void rptHosts_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			ImageButton btnDelete = e.Item.FindControl("btnDeleteHost") as ImageButton;
			UIHelper.AddConfirmationDialog(btnDelete, Resource.SiteSettingsHostDeleteWarning);
		}

		void btnRestoreSkins_Click(object sender, EventArgs e)
		{
			if (selectedSite == null) { return; }
			mojoSetup.CreateOrRestoreSiteSkins(selectedSite.SiteId);
			WebUtils.SetupRedirect(this, Request.RawUrl);
		}

		void btnDelete_Click(object sender, EventArgs e)
		{
			if (WebConfigSettings.AllowDeletingChildSites)
			{
				if ((selectedSite != null) && (!selectedSite.IsServerAdminSite))
				{
					try
					{
						DeleteSiteContent(selectedSite.SiteId);
						CommentRepository commentRepository = new CommentRepository();
						commentRepository.DeleteBySite(selectedSite.SiteGuid);
					}
					catch (Exception ex)
					{
						log.Error("error deleting site content ", ex);
					}

					SiteSettings.Delete(selectedSite.SiteId);
					WebUtils.SetupRedirect(this, SiteRoot + "/Admin/SiteList.aspx");
				}
			}
		}

		private void DeleteSiteContent(int siteId)
		{
			if (siteId == -1) { return; }

			foreach(SitePreDeleteHandlerProvider contentDeleter in SitePreDeleteHandlerProviderManager.Providers)
			{
				try
				{
					contentDeleter.DeleteSiteContent(siteId);
				}
				catch (Exception ex)
				{
					log.Error("SiteSettings.aspx.cs.DeleteSiteContent ", ex);
				}
			}

			if (WebConfigSettings.DeleteSiteFolderWhenDeletingSites)
			{
				FolderDeleteTask task = new FolderDeleteTask();
				task.SiteGuid = siteSettings.SiteGuid;
				SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
				if (currentUser != null)
				{
					task.QueuedBy = currentUser.UserGuid;
				}
				task.FolderToDelete = Server.MapPath("~/Data/Sites/" + siteId.ToInvariantString() + "/");
				task.QueueTask();

				WebTaskManager.StartOrResumeTasks();
			}
		}

		void btnSetupRpx_Click(object sender, EventArgs e)
		{
			if (HttpContext.Current == null) { return; }

			if (txtRpxNowApiKey.Text.Length == 0)
			{
				string redirectUrl = OpenIdRpxHelper.GetPluginApiRedirectUrl(
					HttpContext.Current,
					SiteRoot,
					SiteRoot + "/Services/RpxPluginResponseHandler.ashx",
					siteSettings.SiteGuid.ToString());

				WebUtils.SetupRedirect(this, redirectUrl);

				return;
			}

			OpenIdRpxAccountInfo rpxAccount = OpenIdRpxHelper.LookupRpxAccount(txtRpxNowApiKey.Text, false);
			if (rpxAccount == null)
			{
				WebUtils.SetupRedirect(this, OpenIdRpxHelper.GetPluginApiRedirectUrl(
					HttpContext.Current,
					SiteRoot,
					SiteRoot + "/Services/RpxPluginResponseHandler.ashx",
					siteSettings.SiteGuid.ToString()));

				return;
			}

			siteSettings.RpxNowAdminUrl = rpxAccount.AdminUrl;
			siteSettings.RpxNowApiKey = rpxAccount.ApiKey;
			siteSettings.RpxNowApplicationName = rpxAccount.Realm;
			siteSettings.Save();
			CacheHelper.ClearSiteSettingsCache(siteSettings.SiteId);
			
			WebUtils.SetupRedirect(this, SiteRoot + "/Admin/SiteSettings.aspx?t=oid");
		}
		private void chkRequireApprovalForLogin_Changed(object sender, EventArgs e)
		{
			txtEmailAdressesForUserApprovalNotification.Enabled = chkRequireApprovalForLogin.Checked;
			upLoginApproval.Update();
		}

		private void chkSMTPRequiresAuthentication_Changed(object sender, EventArgs e)
		{
			txtSMTPUser.Enabled = chkSMTPRequiresAuthentication.Checked;
			txtSMTPPassword.Enabled = chkSMTPRequiresAuthentication.Checked;
			upSMTPAuth.Update();
		}

		private void SetupScripts()
		{
			string logoScript = "<script type=\"text/javascript\">"
				+ "function showLogo(listBox) { if(!document.images) return; "
				+ "var logoPath = '" + logoPath + "'; "
				+ "document.images." + imgLogo.ClientID + ".src = logoPath + listBox.value;"
				+ "}</script>";

			Page.ClientScript.RegisterClientScriptBlock(GetType(), "showLogo", logoScript);
		}

		private void PopulateLabels()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuSiteSettingsLink);

			txtSMTPPassword.TextMode = maskSMTPPassword ? TextBoxMode.Password : TextBoxMode.SingleLine;

			litMainSettingsHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsMainSettingsLabel, Resource.SiteSettingsMainSettingsDescription);
			fgpMainSettings.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;
			
			litSkinSettingsHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsSkinSettingsLabel, Resource.SiteSettingsSkinSettingsDescription);
			fgpSkinSettings.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			litContentEditorSettingsHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsContentEditorSettingsLabel, Resource.SiteSettingsContentEditorSettingsDescription);
			fgpEditorSettings.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			litRegistrationSettingsHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsSecurityRegistrationSettingsLabel, Resource.SiteSettingsSecurityRegistrationSettingsDescription);
			fgpRegistrationOptions.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			litUserAccountSettingsHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsSecurityUserAccountSettingsLabel, Resource.SiteSettingsSecurityUserAccountSettingsDescription);
			fgpUserAccountSettings.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			litPasswordSettingsHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsSecurityPasswordSettingsLabel, Resource.SiteSettingsSecurityPasswordSettingsDescription);
			fgpPasswordSettings.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			litPasswordRecoverySettingsHeader.Text = string.Format(adminDisplaySettings.SubPanelHeadingMarkup, Resource.SiteSettingsSecurityPasswordRecoverySettingsLabel, Resource.SiteSettingsSecurityPasswordRecoverySettingsDescription);
			fgpPasswordRecovery.OutsideBottomMarkup += adminDisplaySettings.SubPanelBottomMarkup;

			litOpenIDSettingsHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsSecurityOpenIDSettingsLabel, Resource.SiteSettingsSecurityOpenIDSettingsDescription);
			fgpOpenIDSettings.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			litWindowsLiveIDSettingsHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsSecurityWindowsLiveIDSettingsLabel, Resource.SiteSettingsSecurityWindowsLiveIDSettingsDescription);
			fgpWinLiveID.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			litSMTPSettingsHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsSMTPSettingsLabel, Resource.SiteSettingsSMTPSettingsDescription);
			fgpSMTPSettings.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			litSMTPHeadersHeading.Text = string.Format(adminDisplaySettings.SubPanelHeadingMarkup, Resource.SiteSettingsSMTPHeaders, Resource.SiteSettingsSMTPHeadersDescription);
			fgpSMTPHeaders.OutsideBottomMarkup += adminDisplaySettings.SubPanelBottomMarkup;

			litTestSMTPSettingsHeader.Text = string.Format(adminDisplaySettings.SubPanelHeadingMarkup, Resource.SiteSettingsTestSMTPSettingsLabel, Resource.SiteSettingsTestSMTPSettingsDescription);
			fgpTestSMTPSettings.OutsideBottomMarkup += adminDisplaySettings.SubPanelBottomMarkup;
			btnTestSMTPSettings.Text = Resource.SiteSettingsTestSMTPSettingsButton;

			litHostListHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsExistingHostsLabel, Resource.SiteSettingsExistingHostsDescription);
			fgpHostNames.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			litFolderNamesListHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsExistingFolderMappingsLabel, Resource.SiteSettingsExistingFolderMappingsDescription);
			fgpFolderNames.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			litRecaptchaSettingsHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsSiteRecaptchaSettingsLabel, Resource.SiteSettingsSiteRecaptchaSettingsDescription);
			pnlRecaptchaSettings.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			litRecaptchaAdvancedSettingsHeader.Text = string.Format(adminDisplaySettings.SubPanelHeadingMarkupCollapsible, Resource.SiteSettingsSiteRecaptchaAdvancedSettingsLabel, Resource.SiteSettingsSiteRecaptchaAdvancedSettingsDescription, litRecaptchaAdvancedSettingsHeader.ClientID);
			pnlRecaptchaAdvancedSettings.OutsideBottomMarkup += adminDisplaySettings.SubPanelBottomMarkupCollapsible;

			btnResetRecaptchaHcaptchaDefaults.Text = Resource.SiteSettingsSiteRecaptchaHCaptchaDefaultResetButton;

			litCaptchaVerifyDefault.Text = string.Format(adminDisplaySettings.HelpBlockMarkup, string.Format(Resource.SiteSettingsSiteCaptchaSettingDefault, WebConfigSettings.ReCaptchaDefaultVerifyUrl, WebConfigSettings.HCaptchaDefaultVerifyUrl));
			litCaptchaScriptDefault.Text = string.Format(adminDisplaySettings.HelpBlockMarkup, string.Format(Resource.SiteSettingsSiteCaptchaSettingDefault, WebConfigSettings.ReCaptchaDefaultClientScriptUrl, WebConfigSettings.HCaptchaDefaultClientScriptUrl));
			litCaptchaParamDefault.Text = string.Format(adminDisplaySettings.HelpBlockMarkup, string.Format(Resource.SiteSettingsSiteCaptchaSettingDefault, WebConfigSettings.ReCaptchaDefaultParam, WebConfigSettings.HCaptchaDefaultParam));
			litCaptchaResponseDefault.Text = string.Format(adminDisplaySettings.HelpBlockMarkup, string.Format(Resource.SiteSettingsSiteCaptchaSettingDefault, WebConfigSettings.ReCaptchaDefaultResponseField, WebConfigSettings.HCaptchaDefaultResponseField));
			litCaptchaThemeDefault.Text = string.Format(adminDisplaySettings.HelpBlockMarkup, string.Format(Resource.SiteSettingsSiteCaptchaThemeValidOptions, WebConfigSettings.ReCaptchaDefaultResponseField));

			litSettingsTab.Text = Resource.SiteSettingsGeneralSettingsTab;

			litSecurityTabLink.Text = $"<a href='#{tabSecurity.ClientID}'>{Resource.SiteSettingsSecurityTab}</a>";

			litAdvancedTabLink.Text = $"<a href='#{tabAdvanced.ClientID}'>{Resource.SiteSettingsAdvancedTab}</a>";
			litAdvSettingsHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsAdvancedLabel, Resource.SiteSettingsAdvancedDescription);
			fgpAdvancedSettings.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			litDefaultCountryHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.DefaultCountryStateLabel, string.Empty);
			fgpDefaultCountry.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			litDefaultCurrencyHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.DefaultCurrency, string.Empty);
			fgpDefaultCurrency.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			litCompanyInfoHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsCompanyInfoHeader, string.Empty);
			litCompanyInfoQuickHelp.Text = string.Format(adminDisplaySettings.HelpBlockMarkup, Resource.SiteSettingsCompanyInfoQuickHelp);
			fgpCompanyInfo.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			litBadWordHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsBadWordHeader, string.Empty);
			litBadWordQuickHelp.Text = string.Format(adminDisplaySettings.HelpBlockMarkup, Resource.SiteSettingsBadWordQuickHelp);
			fgpBadWordSettings.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			litContentTabLink.Text = $"<a href='#{tabContent.ClientID}'>{Resource.SiteSettingsContentLink}</a>";
			litScriptsHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsContentHeader, Resource.SiteSettingsContentDescription);
			litHeaderContentQuickHelp.Text = string.Format(adminDisplaySettings.HelpBlockMarkup, Resource.SiteSettingsContentHeaderQuickHelp);
			litFooterContentQuickHelp.Text = string.Format(adminDisplaySettings.HelpBlockMarkup, Resource.SiteSettingsContentFooterQuickHelp);
			fgpScripts.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			litAdminScriptsHeader.Text = string.Format(adminDisplaySettings.PanelHeadingMarkup, Resource.SiteSettingsContentAdminHeader, Resource.SiteSettingsContentAdminDescription);
			litHeaderAdminContentQuickHelp.Text = string.Format(adminDisplaySettings.HelpBlockMarkup, Resource.SiteSettingsContentHeaderQuickHelp);
			litFooterAdminContentQuickHelp.Text = string.Format(adminDisplaySettings.HelpBlockMarkup, Resource.SiteSettingsContentFooterQuickHelp);
			fgpAdminContent.OutsideBottomMarkup += adminDisplaySettings.PanelBottomMarkup;

			lnkCountryAdmin.Text = Resource.CountryAdministrationLink;
			lnkCountryAdmin.NavigateUrl = "~/Admin/AdminCountry.aspx";
			lnkCountryAdmin.Target = "_blank";

			lnkStateAdmin.Text = Resource.GeoZoneAdministrationLink;
			lnkStateAdmin.NavigateUrl = "~/Admin/AdminGeoZone.aspx";
			lnkStateAdmin.Target = "_blank";

			lnkCurrencyAdmin.Text = Resource.CurrencyAdministrationLink;
			lnkCurrencyAdmin.NavigateUrl = "~/Admin/AdminCurrency.aspx";
			lnkCurrencyAdmin.Target = "_blank";

			litCompanyInfoTab.Text = Resource.CompanyInfo;

			litCommerceTabLink.Text = $"<a href='#{tabCommerce.ClientID}'>{Resource.CommerceTab}</a>";

			litGeneralSecurityTabLink.Text = $"<a href='#{tabGeneralSecurity.ClientID}'>{Resource.SiteSettingsSecurityMainTab}</a>";
			litLDAPTabLink.Text = $"<a href='#{tabLDAP.ClientID}'>{Resource.SiteSettingsLdapSettingsLabel}</a>";
			litthirdpartyauthtabLink.Text = $"<a href='#{tabthirdpartyauth.ClientID}'>{Resource.SiteSettingsSecurityThirdPartyAuthTab}</a>";
			litAntiSpamTab.Text = Resource.SiteSettingsSecurityAntiSPAMTab;


			litAPIKeysTab.Text = Resource.SiteSettingsApiKeysTab;
			litMailSettingsTabLink.Text = $"<a href='#{tabMailSettings.ClientID}'>{Resource.MailSettingsTab}</a>";
			litFeaturesTabLink.Text = $"<a href='#{tabSiteFeatures.ClientID}'>{Resource.SiteSettingsFeaturesAllowedLabel}</a>";
			litSiteMappingsTabLink.Text = $"<a href='#{tabSiteMappings.ClientID}'>{Resource.SiteSettingsSiteMappingsLabel}</a>";
			
			btnAddFeature.ToolTip = Resource.SiteSettingsAddFeatureTooltip;
			btnRemoveFeature.ToolTip = Resource.SiteSettingsRemoveFeatureTooltip;

			litFeatureMessage.Text = string.Empty;
			
			lnkAdminMenu.Text = Resource.AdminMenuLink;
			lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
			lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

			lnkSiteList.Visible = ((WebConfigSettings.AllowMultipleSites) && (IsServerAdmin) && isAdmin);
			lnkSiteList.Text = Resource.SiteList;
			lnkSiteList.NavigateUrl = SiteRoot + "/Admin/SiteList.aspx";
			litLinkSeparator2.Visible = lnkSiteList.Visible;
			
			lnkSiteSettings.Text = Resource.AdminMenuSiteSettingsLink;
			lnkSiteSettings.ToolTip = Resource.AdminMenuSiteSettingsLink;
			lnkSiteSettings.NavigateUrl = SiteRoot + "/Admin/SiteSettings.aspx";

			lnkEditClosedMessage.Text = Resource.SiteClosedMessageEditLink;
			lnkEditClosedMessage.NavigateUrl = $"{SiteRoot}/Admin/EditSiteClosedMessage.aspx";

			if (selectedSite.SiteId != siteSettings.SiteId)
			{
				lnkEditClosedMessage.NavigateUrl += $"?SiteId={selectedSite.SiteId}";
			}

			if (ddEditorProviders.Items.Count == 0)
			{
				ddEditorProviders.DataSource = EditorManager.Providers;
				ddEditorProviders.DataBind();
				foreach (ListItem providerItem in ddEditorProviders.Items)
				{
					providerItem.Text = providerItem.Text.Replace("Provider", string.Empty);
				}
			}

			if (ddNewsletterEditor.Items.Count == 0)
			{
				ddNewsletterEditor.DataSource = EditorManager.Providers;
				ddNewsletterEditor.DataBind();
				foreach (ListItem providerItem in ddNewsletterEditor.Items)
				{
					providerItem.Text = providerItem.Text.Replace("Provider", string.Empty);
				}
			}

			if (ddCaptchaProviders.Items.Count == 0)
			{
				ddCaptchaProviders.DataSource = CaptchaManager.Providers;
				ddCaptchaProviders.DataBind();
				foreach (ListItem providerItem in ddCaptchaProviders.Items)
				{
					providerItem.Text = providerItem.Text.Replace("CaptchaProvider", string.Empty);
					providerItem.Text = providerItem.Text.Replace("Provider", string.Empty);
					providerItem.Text = providerItem.Text.Replace("Recaptcha", "reCaptcha/hCaptcha");
				}
			}

			btnAddFolder.Text = Resource.SiteSettingsAddFolderMappingButton;
			btnAddFolder.ToolTip = Resource.SiteSettingsAddFolderMappingButton;

			btnSave.Text = Resource.SiteSettingsApplyChangesButton;
			UIHelper.DisableButtonAfterClick(
				btnSave,
				Resource.PleaseWaitButton,
				Page.ClientScript.GetPostBackEventReference(this.btnSave, string.Empty)
				);

			btnSave.ToolTip = Resource.SiteSettingsApplyChangesButton;
			btnDelete.Text = Resource.SiteSettingsDeleteSiteButton;
			btnDelete.ToolTip = Resource.SiteSettingsDeleteSiteButton;
			UIHelper.AddConfirmationDialog(btnDelete, Resource.SiteSettingsDeleteWarning);

			imgLogo.Alt = Resource.SiteSettingsLogoAltText;
			ddLogos.Attributes.Add("onchange", "javascript:showLogo(this);");
			ddLogos.Attributes.Add("size", "6");

			if (ddPasswordFormat.Items.Count == 0)
			{
				ListItem listItem = new ListItem(Resource.SiteSettingsClearTextPasswordLabel, "0");
				ddPasswordFormat.Items.Add(listItem);
				listItem = new ListItem(Resource.SiteSettingsEncryptedPasswordLabel, "2");
				ddPasswordFormat.Items.Add(listItem);
				listItem = new ListItem(Resource.SiteSettingsHashedPasswordLabel, "1");
				ddPasswordFormat.Items.Add(listItem);
			}

			btnAddHost.Text = Resource.SiteSettingsAddHostButtonLabel;
			btnAddHost.ToolTip = Resource.SiteSettingsAddHostButtonLabel;

			ddDefaultFriendlyUrlPattern.Enabled = WebConfigSettings.AllowChangingFriendlyUrlPattern;

			regexMaxInvalidPasswordAttempts.ErrorMessage = Resource.MaxInvalidPasswordAttemptsRegexWarning;
			regexMinPasswordLength.ErrorMessage = Resource.MinPasswordLengthRegexWarning;
			regexPasswordAttemptWindow.ErrorMessage = Resource.PasswordAttemptWindowRegexWarning;
			regexPasswordMinNonAlphaNumeric.ErrorMessage = Resource.PasswordMinNonAlphaNumericRegexWarning;
			regexWinLiveSecret.ErrorMessage = Resource.WindowsLiveSecretKeyMinLengthWarning;

			btnRestoreSkins.Text = Resource.CopyRestoreSkinsButton;

			btnSetupRpx.Text = Resource.SetupRpxButton;
			lnkRpxAdmin.Text = Resource.RpxAdminLink;
			btnEnablePasswordFormatChange.Text = Resource.AllowPasswordFormatChange;
			if (WebConfigSettings.EnableWoopraGlobally || WebConfigSettings.DisableWoopraGlobally) { fgpWoopra.Visible = false; }
		}

		private void LoadSettings()
		{
			lblErrorMessage.Text = String.Empty;
			isAdmin = WebUser.IsAdmin;
			isContentAdmin = WebUser.IsContentAdmin || SiteUtils.UserIsSiteEditor();
			useFolderForSiteDetection = WebConfigSettings.UseFolderBasedMultiTenants;
			fgpShowPasswordStrength.Visible = WebConfigSettings.EnableAjaxControlPasswordStrength;

			fgpTimeZone.Visible = true;

			if (Request.QueryString["t"] != null)
			{
				requestedTab = Request.QueryString["t"];
			}

			if (SiteUtils.SslIsAvailable())
			{
				sslIsAvailable = true;
				fgpSSL.Visible = true;
			}
			else
			{
				fgpSSL.Visible = false;
			}

			lblSiteRoot.Text = SiteRoot;

			if (WebConfigSettings.GloballyDisableMemberUseOfWindowsLiveMessenger)
			{
				fgpLiveMessenger.Visible = false;
			}

			fgpApprovalsWorkflow.Visible = WebConfigSettings.EnableContentWorkflow;

			enableSiteSettingsSmtpSettings = WebConfigSettings.EnableSiteSettingsSmtpSettings;
			maskSMTPPassword = WebConfigSettings.MaskSmtpPasswordInSiteSettings;

			fgpOpenID.Visible = WebConfigSettings.EnableOpenIdAuthentication;
			fgpOpenIDSelector.Visible = WebConfigSettings.ShowLegacyOpenIDSelector;

			IsServerAdmin = siteSettings.IsServerAdminSite;

			fgpSiteIsClosed.Visible = (WebConfigSettings.AllowClosingSites) && (isAdmin || WebUser.IsContentAdmin);

			currentSiteID = siteSettings.SiteId;
			currentSiteGuid = siteSettings.SiteGuid;

			selectedSiteID = siteSettings.SiteId;

			if (IsServerAdmin && (Page.Request.Params.Get("SiteID") != null))
			{
				selectedSiteID  = WebUtils.ParseInt32FromQueryString("SiteID", selectedSiteID);
			}

			if ((selectedSiteID != siteSettings.SiteId) && (selectedSiteID > -1))
			{
				selectedSite = new SiteSettings((selectedSiteID));
			}
			else
			{
				selectedSite = siteSettings;
			}

			logoPath = ImageSiteRoot + "/Data/Sites/" + selectedSite.SiteId.ToString() + (WebConfigSettings.SiteLogoUseMediaFolder ? "/media/logos/" : "/logos/");

			imgLogo.Src = ImageSiteRoot
				+ "/Data/SitesImages/1x1.gif";

			allowPasswordFormatChange
				= ((IsServerAdmin && WebConfigSettings.AllowPasswordFormatChange) || ((IsServerAdmin) && (selectedSiteID == -1)));

			if ((!IsServerAdmin) && (!WebConfigSettings.AllowPasswordFormatChangeInChildSites))
			{
				allowPasswordFormatChange = false;
			}

			if (!WebConfigSettings.AllowMultipleSites)
			{
				this.IsServerAdmin = false;
			}

			try
			{
				// this keeps the action from changing during ajax postback in folder based sites
				SiteUtils.SetFormAction(Page, Request.RawUrl);
			}
			catch (MissingMethodException)
			{
				//this method was introduced in .NET 3.5 SP1
			}

			// prevent users from changing the mobile skin on the demo site
			ddMobileSkin.Enabled = siteSettings.IsServerAdminSite || WebConfigSettings.AllowSettingMobileSkinInChildSites || WebConfigSettings.MobilePhoneSkin.Length > 0;

			AddClassToBody("administration");
			AddClassToBody("sitesettings");
		}

		#region OnInit

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += new EventHandler(Page_Load);

			btnSave.Click += new EventHandler(btnSave_Click);

			btnAddFeature.Click += new EventHandler(btnAddFeature_Click);
			btnRemoveFeature.Click += new EventHandler(btnRemoveFeature_Click);

			btnAddHost.Click += new EventHandler(btnAddHost_Click);
			btnAddFolder.Click += new EventHandler(btnAddFolder_Click);
			rptHosts.ItemCommand += new RepeaterCommandEventHandler(rptHosts_ItemCommand);
			rptHosts.ItemDataBound += new RepeaterItemEventHandler(rptHosts_ItemDataBound);
			rptFolderNames.ItemCommand += new RepeaterCommandEventHandler(rptFolderNames_ItemCommand);
			rptFolderNames.ItemDataBound += new RepeaterItemEventHandler(rptFolderNames_ItemDataBound);

			rbRecaptchaHcaptcha.SelectedIndexChanged += new EventHandler(RbRecaptchaHcaptcha_SelectedIndexChanged);
			ddCaptchaProviders.SelectedIndexChanged += new EventHandler(DdCaptchaProviders_SelectedIndexChanged);
			btnResetRecaptchaHcaptchaDefaults.Click += new EventHandler(btnResetRecaptchaHcaptchaDefaults_Click);

			btnDelete.Click += new EventHandler(btnDelete_Click);

			btnRestoreSkins.Click += new EventHandler(btnRestoreSkins_Click);

			btnSetupRpx.Click += new EventHandler(btnSetupRpx_Click);


			chkRequireApprovalForLogin.CheckedChanged += new EventHandler(chkRequireApprovalForLogin_Changed);
			
			ddDefaultCountry.SelectedIndexChanged += new EventHandler(ddDefaultCountry_SelectedIndexChanged);

			chkSMTPRequiresAuthentication.CheckedChanged += new EventHandler(chkSMTPRequiresAuthentication_Changed);
			btnTestSMTPSettings.Click += new EventHandler(btnTestSMTPSettings_Click);

			btnEnablePasswordFormatChange.Click += new EventHandler(btnEnablePasswordFormatChange_Click);

			SuppressMenuSelection();
			SuppressPageMenu();
		}



		protected override void OnPreInit(EventArgs e)
		{
			base.OnPreInit(e);

			// commented out for now because it causes this error
			//http://www.mojoportal.com/Forums/Thread.aspx?thread=6281&mid=34&pageid=5&ItemID=2&pagenumber=1#post25767
			//#if !NET35
			//            //http://www.4guysfromrolla.com/articles/071410-1.aspx
			//            //optimize viewstate for .NET 4
			//            this.ViewStateMode = ViewStateMode.Disabled;
			//            ddLdapUserDNKey.ViewStateMode = ViewStateMode.Enabled;
			//            ddDefaultFriendlyUrlPattern.ViewStateMode = ViewStateMode.Enabled;
			//            ddDefaultGeoZone.ViewStateMode = ViewStateMode.Enabled;
			//            ddEditorProviders.ViewStateMode = ViewStateMode.Enabled;
			//            ddMyPageSkin.ViewStateMode = ViewStateMode.Enabled;
			//            ddLogos.ViewStateMode = ViewStateMode.Enabled;
			//            ddNewsletterEditor.ViewStateMode = ViewStateMode.Enabled;
			//            ddSkins.ViewStateMode = ViewStateMode.Enabled;
			//            ddSiteList.ViewStateMode = ViewStateMode.Enabled;
			//            ddDefaultCountry.ViewStateMode = ViewStateMode.Enabled;
			//            ddPasswordFormat.ViewStateMode = ViewStateMode.Enabled;
			//            ddSearchEngine.ViewStateMode = ViewStateMode.Enabled;
			//            ddAvatarSystem.ViewStateMode = ViewStateMode.Enabled;
			//            ddCaptchaProviders.ViewStateMode = ViewStateMode.Enabled;
			//            ddCommentSystem.ViewStateMode = ViewStateMode.Enabled;

			//            chkListEditRoles.ViewStateMode = ViewStateMode.Enabled;
			//            chkGeneralBrowseAndUploadRoles.ViewStateMode = ViewStateMode.Enabled;
			//            chkUserFilesBrowseAndUploadRoles.ViewStateMode = ViewStateMode.Enabled;
			//            chkRolesNotAllowedToEditModuleSettings.ViewStateMode = ViewStateMode.Enabled;
			//            chkRolesThatCanCreateRootPages.ViewStateMode = ViewStateMode.Enabled;
			//            chkRolesThatCanDeleteFilesInEditor.ViewStateMode = ViewStateMode.Enabled;
			//            chkRolesThatCanEditContentTemplates.ViewStateMode = ViewStateMode.Enabled;
			//            chkRolesThatCanLookupUsers.ViewStateMode = ViewStateMode.Enabled;
			//            chkRolesThatCanManageUsers.ViewStateMode = ViewStateMode.Enabled;
			//            chkRolesThatCanViewMemberList.ViewStateMode = ViewStateMode.Enabled;
			//            chkRolesThatCanViewMyPage.ViewStateMode = ViewStateMode.Enabled;

			//            //upFeatures.ViewStateMode = ViewStateMode.Enabled;
			//            lstAllFeatures.ViewStateMode = ViewStateMode.Enabled;
			//            lstSelectedFeatures.ViewStateMode = ViewStateMode.Enabled;
			//            lstAllWebParts.ViewStateMode = ViewStateMode.Enabled;
			//            lstSelectedWebParts.ViewStateMode = ViewStateMode.Enabled;


			//            hdnCurrentSkin.ViewStateMode = ViewStateMode.Enabled;
			//            rptHosts.ViewStateMode = ViewStateMode.Enabled;
			//            rptFolderNames.ViewStateMode = ViewStateMode.Enabled;


			//#endif
		}
		#endregion
	}
}
