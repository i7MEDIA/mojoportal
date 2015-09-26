// Author:					Joe Audette
// Created:				    2004-08-28
// Last Modified:			2014-01-10
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 2010-12-18 modifications by Jamie Eubanks to better support ldap fallback
// 2011-03-01 improvements for multi site management accessibility, got rid of the autopostback dropdown now uses the SiteList.aspx page to select sites

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
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Controls.Captcha;
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
        //private bool siteIsCommerceEnabled = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();

            LoadSettings();

            if ((!isAdmin) && (!isContentAdmin))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

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
                ScriptController.RegisterAsyncPostBackControl(btnAddWebPart);
                ScriptController.RegisterAsyncPostBackControl(btnRemoveFeature);
                ScriptController.RegisterAsyncPostBackControl(btnRemoveWebPart);

              
            }

            PopulateLabels();
            SetupScripts();
            

            if (!Page.IsPostBack)
            {
                //ViewState["skin"] = selectedSite.Skin;
                hdnCurrentSkin.Value = selectedSite.Skin;
                BindGeoLists();
                PopulateControls();
                
            }

        }

        private void PopulateControls()
        {
            if (
                (this.IsServerAdmin)
                && (WebConfigSettings.AllowMultipleSites)
                )
            {
                PopulateMultiSiteControls();
            }

            if (this.selectedSiteID == -1)
            {
                heading.Text = Resource.CreateNewSite;
                txtSiteName.Text = Resource.SiteSettingsNewSiteLabel;
            }
            else
            {
                heading.Text = string.Format(CultureInfo.InvariantCulture, Resource.SiteSettingsFormat, SecurityHelper.RemoveMarkup(selectedSite.SiteName));
                txtSiteName.Text = selectedSite.SiteName;

                if ((!siteSettings.IsServerAdminSite) && (WebConfigSettings.HideGoogleAnalyticsInChildSites))
                {
                    divGAnalytics.Visible = false;
                }

                divSiteId.Visible = (siteSettings.IsServerAdminSite && WebConfigSettings.ShowSiteGuidInSiteSettings);
                lblSiteId.Text = selectedSite.SiteId.ToString(CultureInfo.InvariantCulture);
                lblSiteGuid.Text = selectedSite.SiteGuid.ToString();
            }

#if!MONO
            ISettingControl setting = timeZone as ISettingControl;
            if (setting != null)
            {
                setting.SetValue(selectedSite.TimeZoneId);
            }

#endif

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

            ddMyPageSkin.DataSource = skins;
            ddMyPageSkin.DataBind();

            ListItem listItem = new ListItem();
            listItem.Value = "";
            listItem.Text = Resource.PageLayoutDefaultSkinLabel;
            ddMyPageSkin.Items.Insert(0, listItem);

            listItem = new ListItem();
            listItem.Value = "";
            listItem.Text = Resource.PageLayoutDefaultSkinLabel;
            ddMobileSkin.Items.Insert(0, listItem);

            listItem = ddMyPageSkin.Items.FindByValue("printerfriendly");
            if (listItem != null)
            {
                ddMyPageSkin.Items.Remove(listItem);
            }

            listItem = ddMobileSkin.Items.FindByValue("printerfriendly");
            if (listItem != null)
            {
                ddMobileSkin.Items.Remove(listItem);
            }

            listItem = ddMyPageSkin.Items.FindByValue(selectedSite.MyPageSkin);
            if (listItem != null)
            {
                ddMyPageSkin.ClearSelection();
                listItem.Selected = true;
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
                //item = ddSkins.Items.FindByValue(selectedSite.Skin.Replace(".ascx", ""));
                //if (item != null)
                //{
                //    ddSkins.ClearSelection();
                //    item.Selected = true;
                //}

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

            if (selectedSite.SiteGuid == siteSettings.SiteGuid)
            {
                //SetupSkinPreviewScript();
            }
            else
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

                if (WebConfigSettings.SiteLogoUseMediaFolder)
                {
                    imgLogo.Src = ImageSiteRoot + "/Data/Sites/" + selectedSite.SiteId.ToInvariantString() + "/media/logos/" + selectedSite.Logo;
                }
                else
                {
                    imgLogo.Src = ImageSiteRoot + "/Data/Sites/" + selectedSite.SiteId.ToInvariantString() + "/logos/" + selectedSite.Logo;
                }
                
                
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
            txtGmapApiKey.Text = selectedSite.GmapApiKey;
            txtAddThisUserId.Text = selectedSite.AddThisDotComUsername;
            txtGoogleAnayticsAccountCode.Text = selectedSite.GoogleAnalyticsAccountCode;
            txtOpenIdSelectorCode.Text = selectedSite.OpenIdSelectorId;
            chkEnableWoopra.Checked = selectedSite.EnableWoopra;
            txtIntenseDebateAccountId.Text = selectedSite.IntenseDebateAccountId;
            txtDisqusSiteShortName.Text = selectedSite.DisqusSiteShortName;
            txtFacebookAppId.Text = selectedSite.FacebookAppId;

            ISettingControl currencySetting = SiteCurrencySetting as ISettingControl;
            currencySetting.SetValue(selectedSite.CurrencyGuid.ToString());

            if (WebConfigSettings.EnableOpenIdAuthentication)
            {
                chkAllowOpenIDAuth.Checked = selectedSite.AllowOpenIdAuth;
            }
            //else
            //{
            //    tabOpenID.Visible = false;
            //}

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

            divDisableDbAuthentication.Visible = !WebConfigSettings.HideDisableDbAuthenticationSetting;
            chkDisableDbAuthentication.Checked = selectedSite.DisableDbAuth;

            chkAllowUserEditorChoice.Checked = selectedSite.AllowUserEditorPreference;
            chkEnableMyPageFeature.Checked = selectedSite.EnableMyPageFeature;
            chkAllowUserSkins.Checked = selectedSite.AllowUserSkins;
            chkAllowPageSkins.Checked = selectedSite.AllowPageSkins;
            chkAllowHideMenuOnPages.Checked = selectedSite.AllowHideMenuOnPages;

            chkRequireSSL.Checked = selectedSite.UseSslOnAllPages;
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
                tabHosts.Visible = false;
                liHosts.Visible = false;
                tabFolderNames.Visible = false;
                liFolderNames.Visible = false;
                tabSiteFeatures.Visible = false;
                liSecurity.Visible = false;
                tabSecurity.Visible = false;
                liCommerce.Visible = false;
                tabCommerce.Visible = false;
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
            
            item = ddPasswordFormat.Items.FindByValue(selectedSite.PasswordFormat.ToString(CultureInfo.InvariantCulture));

            if (item != null)
            {
                ddPasswordFormat.ClearSelection();
                item.Selected = true;
            }

            ddPasswordFormat.Enabled = allowPasswordFormatChange;
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

            if (
                (siteSettings.IsServerAdminSite)
                && (isAdmin)
                && (WebConfigSettings.AllowForcingPreferredHostName)
                )
            {
                divPreferredHostName.Visible = true;

            }
            else
            {
                divPreferredHostName.Visible = false;
            }

            if (
                (WebConfigSettings.UseRelatedSiteMode)
                &&((selectedSite.SiteId != WebConfigSettings.RelatedSiteID)||(selectedSiteID == -1))
                )
            {
                if (WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
                {
                    liGeneralSecurity.Visible = false;
                    tabGeneralSecurity.Visible = false;
                    liLDAP.Visible = false;
                    tabLDAP.Visible = false;
                    liOpenID.Visible = false;
                    tabOpenID.Visible = false;
                    liWindowsLive.Visible = false;
                    tabWindowsLiveID.Visible = false;  
                    
                }
                else
                {
                    liGeneralSecurity.Visible = false;
                    tabGeneralSecurity.Visible = false;
                    liLDAP.Visible = false;
                    tabLDAP.Visible = false;
                    
                }

                divReallyDeleteUsers.Visible = false; 
            }

            if (txtRpxNowApiKey.Text.Length == 0)
            {
                UIHelper.AddConfirmationDialog(btnSetupRpx, Resource.RpxButtonConfirm);
            }

            DoTabSelection();
            PopulateMailSettings();
          

        }

        

        private void DoTabSelection()
        {

            switch (requestedTab)
            {
                case "oid":
                    if (tabSecurity.Visible)
                    {
                        liSecurity.Attributes.Add("class", "selected");
                        liOpenID.Attributes.Add("class", "selected");
                    }
                    else
                    {
                        liGeneral.Attributes.Add("class", "selected");
                    }

                    break;

                default:

                    liGeneral.Attributes.Add("class", "selected");

                    //if (
                    //    (WebConfigSettings.UseRelatedSiteMode) 
                    //    && ((selectedSite.SiteId != WebConfigSettings.RelatedSiteID)||(selectedSiteID == -1))
                    //    )
                    //{
                    //    liPermissions.Attributes.Add("class", "selected");
                        

                    //}

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


        //private void SetupSiteEditRolesList()
        //{
        //    if (selectedSite == null) { return; }

        //    //divSiteEditRoles.Visible = true;
        //    //h3SiteEditRoles.Visible = true;

           

        //}

        private void PopulateMultiSiteControls()
        {
            int countOfOtherSites = SiteSettings.GetCountOfOtherSites(siteSettings.SiteId);

            if(countOfOtherSites > 0)
            {

                if (WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites)
                {
                    PopulateFolderList();

                }
                else
                {
                    PopulateHostList();

                }

                if (!selectedSite.IsServerAdminSite)
                {
                    PopulateFeatures();
                    PopulateWebParts();

                }
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

            btnAddFeature.Enabled = (lstAllFeatures.Items.Count > 0);
            btnRemoveFeature.Enabled = (lstSelectedFeatures.Items.Count > 0);

        }

        private void PopulateWebParts()
        {
 #if !MONO
            liWebParts.Visible = true;
            tabWebParts.Visible = true;
            lstAllWebParts.Items.Clear();
            lstSelectedWebParts.Items.Clear();


            using (IDataReader reader = WebPartContent.SelectBySite(this.currentSiteID))
            {
                lstAllWebParts.DataSource = reader;
                lstAllWebParts.DataTextField = "ClassName";
                lstAllWebParts.DataValueField = "WebPartID";
                lstAllWebParts.DataBind();
            }

            using (IDataReader reader = WebPartContent.SelectBySite(selectedSite.SiteId))
            {
                while (reader.Read())
                {
                    ListItem matchItem = lstAllWebParts.Items.FindByText(reader["ClassName"].ToString());
                    if (matchItem != null)
                    {
                        lstAllWebParts.Items.Remove(matchItem);

                        matchItem.Value = reader["WebPartID"].ToString();
                        lstSelectedWebParts.Items.Add(matchItem);

                    }
                }
            }

            btnAddWebPart.Enabled = (lstAllWebParts.Items.Count > 0);
            btnRemoveWebPart.Enabled = (lstSelectedWebParts.Items.Count > 0);
#endif

        }

        private void PopulateHostList()
        {
            if (selectedSiteID == -1)  { return; } // site must be created first
            liHosts.Visible = true;
            tabHosts.Visible = true;
            using (IDataReader reader = SiteSettings.GetHostList(selectedSite.SiteId))
            {
                rptHosts.DataSource = reader;
                rptHosts.DataBind();
            }
            if (rptHosts.Items.Count > 0)
            {
                rptHosts.Visible = true;
                litHostListHeader.Text = Resource.SiteSettingsExistingHostsLabel;
            }
            else
            {
                rptHosts.Visible = false;
            }
        }

        private void PopulateFolderList()
        {
            if (selectedSiteID == -1) { return; } // site must be created first

            // no folders can map to root site
            if (!selectedSite.IsServerAdminSite)
            {
                liFolderNames.Visible = true;
                tabFolderNames.Visible = true;
                //fldFolderNames.Visible = true;
                List<SiteFolder> siteFolders = SiteFolder.GetBySite(selectedSite.SiteGuid);
                rptFolderNames.DataSource = siteFolders;
                rptFolderNames.DataBind();
                if (rptFolderNames.Items.Count > 0)
                {
                    rptFolderNames.Visible = true;
                    litFolderNamesListHeading.Text = Resource.SiteSettingsExistingFolderMappingsLabel;
                }
                else
                {
                    rptFolderNames.Visible = false;
                }

            }
        }

        private void PopulateMailSettings()
        {
            if (selectedSite.SiteId == -1)
            {
                //new site
                liMailSettings.Visible = false;
                tabMailSettings.Visible = false;
                return;
            }

            if (!enableSiteSettingsSmtpSettings) { return; }

            divSMTPEncoding.Visible = WebConfigSettings.ShowSmtpEncodingOption;

            if (WebConfigSettings.UseLegacyCryptoHelper)
            {

                if (selectedSite.SMTPUser.Length > 0)
                {
                    try
                    {
                        txtSMTPUser.Text = CryptoHelper.Decrypt(selectedSite.SMTPUser);
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
                        txtSMTPPassword.Text = CryptoHelper.Decrypt(selectedSite.SMTPPassword);
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
            chkSMTPRequiresAuthentication.Checked = selectedSite.SMTPRequiresAuthentication;
            chkSMTPUseSsl.Checked = selectedSite.SMTPUseSsl;
            txtSMTPPreferredEncoding.Text = selectedSite.SMTPPreferredEncoding;
            

        }

        private void SetMailSettings()
        {
            if (selectedSite.SiteId == -1) { return; }
            if (!enableSiteSettingsSmtpSettings) { return; }

            if (WebConfigSettings.UseLegacyCryptoHelper)
            {
                if (txtSMTPUser.Text.Length > 0)
                {
                    selectedSite.SMTPUser = CryptoHelper.Encrypt(txtSMTPUser.Text);
                }
                else
                {
                    selectedSite.SMTPUser = string.Empty;
                }
                if (txtSMTPPassword.Text.Length > 0)
                {
                    selectedSite.SMTPPassword = CryptoHelper.Encrypt(txtSMTPPassword.Text);
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
            if (this.IsServerAdmin)
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

#if!MONO
            ISettingControl setting = timeZone as ISettingControl;
            if (setting != null)
            {
                selectedSite.TimeZoneId = setting.GetValue();
            }
#endif
            
            selectedSite.Logo = ddLogos.SelectedValue;
            selectedSite.Skin = SkinSetting.GetValue();

            if (ddMobileSkin.Enabled)
            {
                selectedSite.MobileSkin = ddMobileSkin.SelectedValue;
            }

            selectedSite.MyPageSkin = ddMyPageSkin.SelectedValue;
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
            selectedSite.GmapApiKey = txtGmapApiKey.Text;
            selectedSite.AddThisDotComUsername = txtAddThisUserId.Text;
            selectedSite.GoogleAnalyticsAccountCode = txtGoogleAnayticsAccountCode.Text;
            selectedSite.OpenIdSelectorId = txtOpenIdSelectorCode.Text;
            selectedSite.CommentProvider = ddCommentSystem.SelectedValue;
            selectedSite.IntenseDebateAccountId = txtIntenseDebateAccountId.Text;
            selectedSite.DisqusSiteShortName = txtDisqusSiteShortName.Text;
            selectedSite.FacebookAppId = txtFacebookAppId.Text;

            if (divWoopra.Visible)
            {
                selectedSite.EnableWoopra = chkEnableWoopra.Checked;
            }

            if (divSiteIsClosed.Visible)
            {
                selectedSite.SiteIsClosed = chkSiteIsClosed.Checked;
            }

            // keep track if password format changed then we need to update passwords to new format
            int previousPasswordFormat = selectedSite.PasswordFormat;

            if (isAdmin)
            {
                selectedSite.PreferredHostName = txtPreferredHostName.Text.Replace("https://", string.Empty).Replace("http://",string.Empty).Replace("/", string.Empty);

                if (WebConfigSettings.EnableOpenIdAuthentication)
                {
                    selectedSite.AllowOpenIdAuth = chkAllowOpenIDAuth.Checked;
                }
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

                //ISettingControl commerceReportRoles = CommerceReportRolesSetting as ISettingControl;
                //selectedSite.RolesThatCanCreateRootPages = chkRolesThatCanCreateRootPages.Items.SelectedItemsToSemiColonSeparatedString();
                //selectedSite.CommerceReportViewRoles = chkCommerceReportRoles.Items.SelectedItemsToSemiColonSeparatedString();
                //selectedSite.GeneralBrowseAndUploadRoles = chkGeneralBrowseAndUploadRoles.Items.SelectedItemsToSemiColonSeparatedString();
                //selectedSite.UserFilesBrowseAndUploadRoles = chkUserFilesBrowseAndUploadRoles.Items.SelectedItemsToSemiColonSeparatedString();
                //selectedSite.RolesThatCanEditContentTemplates = chkRolesThatCanEditContentTemplates.Items.SelectedItemsToSemiColonSeparatedString();
                //selectedSite.RolesNotAllowedToEditModuleSettings = chkRolesNotAllowedToEditModuleSettings.Items.SelectedItemsToSemiColonSeparatedString();
                //selectedSite.RolesThatCanCreateUsers = chkRolesThatCanCreateUsers.Items.SelectedItemsToSemiColonSeparatedString();
                //selectedSite.RolesThatCanManageUsers = chkRolesThatCanManageUsers.Items.SelectedItemsToSemiColonSeparatedString();
                //selectedSite.RolesThatCanLookupUsers = chkRolesThatCanLookupUsers.Items.SelectedItemsToSemiColonSeparatedString();
                //selectedSite.RolesThatCanViewMemberList = chkRolesThatCanViewMemberList.Items.SelectedItemsToSemiColonSeparatedString();
                //selectedSite.RolesThatCanViewMyPage = chkRolesThatCanViewMyPage.Items.SelectedItemsToSemiColonSeparatedString();
                //selectedSite.RolesThatCanDeleteFilesInEditor = chkRolesThatCanDeleteFilesInEditor.Items.SelectedItemsToSemiColonSeparatedString();
                //selectedSite.RolesThatCanManageSkins = chkRolesThatCanManageSkins.Items.SelectedItemsToSemiColonSeparatedString();
                //selectedSite.RolesThatCanAssignSkinsToPages = chkRolesThatCanAssignSkinsToPages.Items.SelectedItemsToSemiColonSeparatedString();

                //if (divDefaultRootPageViewRoles.Visible)
                //{
                //    selectedSite.DefaultRootPageViewRoles = chkDefaultRootPageViewRoles.Items.SelectedItemsToSemiColonSeparatedString();
                //}

                //if (divDefaultRootPageEditRoles.Visible)
                //{
                //    selectedSite.DefaultRootPageEditRoles = chkDefaultRootPageEditRoles.Items.SelectedItemsToSemiColonSeparatedString();
                //}

                //if (divDefaultRootPageCreateChildPageRoles.Visible)
                //{
                //    selectedSite.DefaultRootPageCreateChildPageRoles = chkDefaultRootPageCreateChildPageRoles.Items.SelectedItemsToSemiColonSeparatedString();
                //}



                if (sslIsAvailable)
                {
                    selectedSite.UseSslOnAllPages = chkRequireSSL.Checked;
                }

                

                if ((chkAllowRegistration.Enabled) && (divAllowRegistration.Visible)&&(tabGeneralSecurity.Visible))
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

                if (
                (WebConfigSettings.UseRelatedSiteMode)
                && ((selectedSite.SiteId != WebConfigSettings.RelatedSiteID) && (selectedSiteID != -1))
                )
                {
                    //don't change this on child sites in related sites mode
                }
                else
                {
                    if ((chkAllowUserToChangeName.Enabled) && (divAllowUserToChangeName.Visible))
                    {
                        selectedSite.AllowUserFullNameChange = chkAllowUserToChangeName.Checked;
                    }

                    if ((chkUseEmailForLogin.Enabled) && (divUseEmailForLogin.Visible))
                    {
                        selectedSite.UseEmailForLogin = chkUseEmailForLogin.Checked;

                    }

                    
                }

                selectedSite.AutoCreateLdapUserOnFirstLogin = chkAutoCreateLdapUserOnFirstLogin.Checked;
                selectedSite.AllowDbFallbackWithLdap = chkAllowDbFallbackWithLdap.Checked;
                selectedSite.AllowEmailLoginWithLdapDbFallback = chkAllowEmailLoginWithLdapDbFallback.Checked;

                if ((!selectedSite.UseLdapAuth) && (chkUseLdapAuth.Checked) && (!creatingNewSite))
                {
                    LdapSettings testLdapSettings = new LdapSettings();
                    testLdapSettings.Server = txtLdapServer.Text;
                    testLdapSettings.Port = Convert.ToInt32(txtLdapPort.Text);
                    testLdapSettings.Domain = txtLdapDomain.Text;
                    testLdapSettings.RootDN = txtLdapRootDN.Text;
                    testLdapSettings.UserDNKey = ddLdapUserDNKey.SelectedValue;
                    if (!TestCurrentUserLdap(testLdapSettings))
                    {
                        lblErrorMessage.Text += "  " + Resource.SiteSettingsLDAPAdminUserNotFound;
                        btnSave.Text = Resource.SiteSettingsApplyChangesButton;
                        btnSave.Enabled = true;
                        return;
                    }
                }

                if (
                (WebConfigSettings.UseRelatedSiteMode)
                && ((selectedSite.SiteId != WebConfigSettings.RelatedSiteID) && (selectedSiteID != -1))
                )
                {
                    tabLDAP.Visible = false;
                }

                if (selectedSite.SiteId > -1)
                {
                    if (tabLDAP.Visible)
                    {
                        if (divUseLdap.Visible)
                        {
                            selectedSite.UseLdapAuth = chkUseLdapAuth.Checked;
                        }
                        if (divLdapServer.Visible)
                        {
                            selectedSite.SiteLdapSettings.Server = txtLdapServer.Text;
                        }
                        if ((divLdapPort.Visible)&&(txtLdapPort.Text.Length > 0))
                        {
                            int port = 389;
                            int.TryParse(txtLdapPort.Text, out port);
                            selectedSite.SiteLdapSettings.Port = port;
                        }

                        if (divLdapDomain.Visible)
                        {
                            selectedSite.SiteLdapSettings.Domain = txtLdapDomain.Text;
                        }

                        if (divLdapRootDn.Visible)
                        {
                            selectedSite.SiteLdapSettings.RootDN = txtLdapRootDN.Text;
                        }
                        if (divLdapUserDNKey.Visible)
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

                if (
                (WebConfigSettings.UseRelatedSiteMode)
                && ((selectedSite.SiteId != WebConfigSettings.RelatedSiteID) && (selectedSiteID != -1))
                )
                {
                    //don't change this on child sites in related sites mode
                }
                else
                {
                    if (
                    (allowPasswordFormatChange)
                    || (selectedSite.SiteGuid == Guid.Empty) // new site
                    )
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
                    selectedSite.RequireEnterEmailTwiceOnRegistration = chkRequireEmailTwice.Checked;
                }


                //if (IsServerAdmin
                //&& (WebConfigSettings.UseRelatedSiteMode)
                //&& (selectedSite.SiteId != WebConfigSettings.RelatedSiteID)
                //&& (chkListEditRoles.Items.Count > 0)
                //)
                //{
                //    selectedSite.SiteRootEditRoles = chkListEditRoles.Items.SelectedItemsToSemiColonSeparatedString(); 
                //}


            } //end isAdmin

            selectedSite.AllowUserEditorPreference = chkAllowUserEditorChoice.Checked;
            selectedSite.MetaProfile = txtMetaProfile.Text;
            selectedSite.DefaultEmailFromAddress = txtSiteEmailFromAddress.Text;
            selectedSite.DefaultFromEmailAlias = txtSiteEmailFromAlias.Text;
            selectedSite.EnableMyPageFeature = chkEnableMyPageFeature.Checked;

            SetMailSettings();

            // the site may previously have been using email for login
            //but we need to make sure it uses loging name in case usinh ldap as fallback authentication
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
            

            if (
                (!creatingNewSite)
                && (previousPasswordFormat != selectedSite.PasswordFormat)
                )
            {
                // this is not something you want to change very often
                mojoMembership.ChangeUserPasswordFormat(selectedSite, previousPasswordFormat);
                CacheHelper.ClearSiteSettingsCache(selectedSite.SiteId);

            }

            //String oldSkin = ViewState["skin"].ToString();
            string oldSkin = hdnCurrentSkin.Value;
            if ((oldSkin != selectedSite.Skin)&&(WebConfigSettings.UseCacheDependencyFiles))
            {
                CacheHelper.ResetThemeCache();
            }

            //if ((WebConfigSettings.UseRelatedSiteMode)&&(selectedSite.SiteId == WebConfigSettings.RelatedSiteID))
            if (WebConfigSettings.UseRelatedSiteMode)
            {
                // need to propagate any security changes to all child sites
                // reset the sitesettings cache for each site
                if (creatingNewSite)
                {
                    SiteSettings masterSite = CacheHelper.GetSiteSettings(WebConfigSettings.RelatedSiteID);
                    // siteSettings is the master site we need some permissions from it synced to the new site
                    SiteSettings.SyncRelatedSites(masterSite, WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites);
                }
                else
                {
                    SiteSettings.SyncRelatedSites(selectedSite, WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites);
                }

                // reset the sitesettings cache for each site
                CacheHelper.ClearRelatedSiteCache(-1);

                


            }

            String redirectUrl = SiteRoot
                + "/Admin/SiteSettings.aspx?SiteID=" + selectedSite.SiteId.ToString();

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
            String uid = Context.User.Identity.Name;
            SiteUser user = new SiteUser(this.selectedSite, uid);
            return LdapHelper.TestUser(testLdapSettings, user.LoginName, txtLdapTestPassword.Text);
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
                lblFeatureMessage.Text = Resource.SiteSettingsSelectFeatureToAddWarning;
            }
        }

        private void btnRemoveFeature_Click(object sender, EventArgs e)
        {
            //if (selectedSite == null) { return; }

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
                lblFeatureMessage.Text = Resource.SiteSettingsSelectFeatureToRemoveWarning;
            }
        }

        protected void btnRemoveWebPart_Click(object sender, EventArgs e)
        {
            if (lstSelectedWebParts.SelectedIndex > -1)
            {
                foreach (ListItem item in lstSelectedFeatures.Items)
                {
                    if (item.Selected)
                    {
                        Guid webPartID = new Guid(item.Value);
                        WebPartContent.DeleteWebPart(webPartID);

                    }

                }

                PopulateWebParts();
                upWebParts.Update();
                
            }
            else
            {
                lblWebPartMessage.Text = Resource.SiteSettingsSelectWebPartToRemoveWarning;
            }
        }

        protected void btnAddWebPart_Click(object sender, EventArgs e)
        {
            if (lstAllWebParts.SelectedIndex > -1)
            {
                foreach (ListItem item in lstAllWebParts.Items)
                {
                    if (item.Selected)
                    {
                        Guid webPartID = new Guid(item.Value);
                        WebPartContent baseSiteWebPart = new WebPartContent(webPartID);

                        WebPartContent childSiteWebPart = new WebPartContent();
                        childSiteWebPart.SiteId = selectedSite.SiteId;
                        childSiteWebPart.SiteGuid = selectedSite.SiteGuid;
                        childSiteWebPart.AllowMultipleInstancesOnMyPage = baseSiteWebPart.AllowMultipleInstancesOnMyPage;
                        childSiteWebPart.AssemblyName = baseSiteWebPart.AssemblyName;
                        childSiteWebPart.AvailableForContentSystem = baseSiteWebPart.AvailableForContentSystem;
                        childSiteWebPart.AvailableForMyPage = baseSiteWebPart.AvailableForMyPage;
                        childSiteWebPart.ClassName = baseSiteWebPart.ClassName;
                        childSiteWebPart.Description = baseSiteWebPart.Description;
                        childSiteWebPart.ImageUrl = baseSiteWebPart.ImageUrl;
                        childSiteWebPart.Title = baseSiteWebPart.Title;
                        childSiteWebPart.Save();

                    }

                }

                PopulateWebParts();
                upWebParts.Update();
            }
            else
            {
                lblWebPartMessage.Text = Resource.SiteSettingsSelectWebPartToAddWarning;
            }

        }

        private void btnAddHost_Click(object sender, EventArgs e)
        {
            if (selectedSite == null) { return; }

            lblHostMessage.Text = string.Empty;

            if (this.txtHostName.Text.Length == 0)
            {
                lblHostMessage.Text = Resource.SiteSettingsHostNameRequiredMessage;
                return;
            }

            try
            {
                SiteSettings.AddHost(selectedSite.SiteGuid, selectedSite.SiteId, this.txtHostName.Text.ToLower());
                
                CacheHelper.ClearSiteSettingsCache(selectedSite.SiteId);
                CacheHelper.ClearSiteSettingsCache(siteSettings.SiteId); // this clears it from the defaut site which would use any host if it is not already assigned
                //CacheHelper.ResetSiteMapCache(selectedSite.SiteId);
                //CacheHelper.ResetSiteMapCache(1);
               
            }
            catch (DbException)
            {
                lblHostMessage.Text = Resource.SiteSettingsDuplicateHostsWarning;
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
                        //CacheHelper.ResetSiteMapCache(selectedSite.SiteId);
                        //CacheHelper.ResetSiteMapCache(1);
                    }
                    break;

                default:

                    break;
            }

            //WebUtils.SetupRedirect(this, Request.RawUrl);
            PopulateHostList();
            upHosts.Update();
            

        }

        void btnAddFolder_Click(object sender, EventArgs e)
        {
            lblFolderMessage.Text = string.Empty;

            if (txtFolderName.Text.Length > 0)
            {
                if (SiteFolder.Exists(txtFolderName.Text))
                {
                    lblFolderMessage.Text = Resource.SiteSettingsFolderNameAlreadyInUseWarning;
                    return;
                }

                if (!SiteFolder.IsAllowedFolder(txtFolderName.Text))
                {
                    lblFolderMessage.Text = Resource.SiteSettingsFolderNameNotAllowedWarning;
                    return;
                }

                if (SiteFolder.HasInvalidChars(txtFolderName.Text))
                {
                    lblFolderMessage.Text = Resource.SiteSettingsFolderNameInvalidCharsWarning;
                    return;
                }
                
                SiteFolder siteFolder = new SiteFolder();
                siteFolder.SiteGuid = selectedSite.SiteGuid;
                siteFolder.FolderName = txtFolderName.Text;
                siteFolder.Save();

                PopulateFolderList();
                upFolderNames.Update();

            }
            else
            {
                lblFolderMessage.Text = Resource.SiteSettingsFolderNameBlankWarning;
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


        private void SetupScripts()
        {
            string logoScript = "<script type=\"text/javascript\">"
                + "function showLogo(listBox) { if(!document.images) return; "
                + "var logoPath = '" + logoPath + "'; "
                + "document.images." + imgLogo.ClientID + ".src = logoPath + listBox.value;"
                + "}</script>";

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "showLogo", logoScript);

            

        }

        

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuSiteSettingsLink);

            

            if (enableSiteSettingsSmtpSettings)
            {
                liMailSettings.Visible = true;
                tabMailSettings.Visible = true;
            }
            else
            {
                liMailSettings.Visible = false;
                tabMailSettings.Visible = false;
            }

            if (maskSMTPPassword)
            {
                txtSMTPPassword.TextMode = TextBoxMode.Password;
            }
            else
            {
                txtSMTPPassword.TextMode = TextBoxMode.SingleLine;
            }

            litSettingsTab.Text = Resource.SiteSettingsGeneralSettingsTab;
            litSecurityTabLink.Text = "<a href='#" + tabSecurity.ClientID + "'>" +  Resource.SiteSettingsSecurityTab + "</a>";
            litCompanyInfoTab.Text = Resource.CompanyInfo;
            litCommerceTabLink.Text = "<a href='#" + tabCommerce.ClientID + "'>" + Resource.CommerceTab + "</a>";
            litGeneralSecurityTabLink.Text = "<a href='#" + tabGeneralSecurity.ClientID + "'>" + Resource.SiteSettingsSecurityMainTab + "</a>";
            //litPermissionsTabLink.Text = "<a href='#" + tabPermissions.ClientID + "'>" + Resource.SiteSettingsPermissionsTab + "</a>";
            litLDAPTabLink.Text = "<a href='#" + tabLDAP.ClientID + "'>" + Resource.SiteSettingsLdapSettingsLabel + "</a>";
            litOpenIDTabLink.Text = "<a href='#" + tabOpenID.ClientID + "'>" + Resource.SiteSettingsSecurityOpenIDTab + "</a>";
            litWindowsLiveTabLink.Text = "<a href='#" + tabWindowsLiveID.ClientID + "'>" + Resource.SiteSettingsSecurityWindowsLiveTab + "</a>";
            litAntiSpamTab.Text = Resource.SiteSettingsSecurityAntiSPAMTab;
            litAPIKeysTab.Text = Resource.SiteSettingsApiKeysTab;
            litMailSettingsTabLink.Text = "<a href='#" + tabMailSettings.ClientID + "'>" + Resource.MailSettingsTab + "</a>";
            litFeaturesTabLink.Text = "<a href='#" + tabSiteFeatures.ClientID + "'>" + Resource.SiteSettingsFeaturesAllowedLabel + "</a>";
            litWebPartsTabLink.Text = "<a href='#" + tabWebParts.ClientID + "'>" + Resource.SiteSettingsWebPartTab + "</a>";
            litHostsTabLink.Text = "<a href='#" + tabHosts.ClientID + "'>" + Resource.SiteSettingsHostNameMappingLabel + "</a>";
            litFolderNamesTabLink.Text = "<a href='#" + tabFolderNames.ClientID + "'>" + Resource.SiteSettingsFolderMappingLabel + "</a>";

            
            btnAddWebPart.ToolTip = Resource.SiteSettignsAddWebPartTooltip;
            btnAddFeature.ToolTip = Resource.SiteSettingsAddFeatureTooltip;
            btnRemoveWebPart.ToolTip = Resource.SiteSettingsRemoveWebPartTooltip;
            btnRemoveFeature.ToolTip = Resource.SiteSettingsRemoveFeatureTooltip;

            lblFeatureMessage.Text = string.Empty;
            lblWebPartMessage.Text = string.Empty;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkSiteList.Visible = ((WebConfigSettings.AllowMultipleSites) && (IsServerAdmin) && isAdmin);
            lnkSiteList.Text = Resource.SiteList;
            lnkSiteList.NavigateUrl = SiteRoot + "/Admin/SiteList.aspx";
            litLinkSeparator2.Visible = lnkSiteList.Visible;
            
            lnkNewSite.Visible = lnkSiteList.Visible;
            lnkNewSite.Text = Resource.SiteSettingsNewSiteLabel;
            lnkNewSite.ToolTip = Resource.CreateNewSite;
            lnkNewSite.NavigateUrl = SiteRoot + "/Admin/SiteSettings.aspx?SiteID=-1";

            lnkSiteSettings.Text = Resource.AdminMenuSiteSettingsLink;
            lnkSiteSettings.ToolTip = Resource.AdminMenuSiteSettingsLink;
            lnkSiteSettings.NavigateUrl = SiteRoot + "/Admin/SiteSettings.aspx";

            lnkEditClosedMessage.Text = Resource.SiteClosedMessageEditLink;
            lnkEditClosedMessage.NavigateUrl = SiteRoot + "/Admin/EditSiteClosedMessage.aspx";

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
                    providerItem.Text = providerItem.Text.Replace("Provider", string.Empty);
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

            litHostListHeader.Text = Resource.SiteSettingsNoHostsFound;
            btnAddHost.Text = Resource.SiteSettingsAddHostButtonLabel;
            btnAddHost.ToolTip = Resource.SiteSettingsAddHostButtonLabel;
            litFolderNamesListHeading.Text = Resource.SiteSettingsNoFolderNames;
            divFriendlyUrlPattern.Visible = WebConfigSettings.AllowChangingFriendlyUrlPattern;

            regexMaxInvalidPasswordAttempts.ErrorMessage = Resource.MaxInvalidPasswordAttemptsRegexWarning;
            regexMinPasswordLength.ErrorMessage = Resource.MinPasswordLengthRegexWarning;
            regexPasswordAttemptWindow.ErrorMessage = Resource.PasswordAttemptWindowRegexWarning;
            regexPasswordMinNonAlphaNumeric.ErrorMessage = Resource.PasswordMinNonAlphaNumericRegexWarning;
            regexWinLiveSecret.ErrorMessage = Resource.WindowsLiveSecretKeyMinLengthWarning;

            btnRestoreSkins.Text = Resource.CopyRestoreSkinsButton;

            btnSetupRpx.Text = Resource.SetupRpxButton;
            lnkRpxAdmin.Text = Resource.RpxAdminLink;

            //gbSkinPreview.Text = Resource.SkinPreviewLink;
            

            if (WebConfigSettings.EnableWoopraGlobally || WebConfigSettings.DisableWoopraGlobally) { divWoopra.Visible = false; }

            // only need to set this for first instance of helplinkbutton on the page
            

#if MONO
                divMyPage.Visible = false;

#endif

        }

        

        private void LoadSettings()
        {
            lblErrorMessage.Text = String.Empty;
            isAdmin = WebUser.IsAdmin;
            isContentAdmin = WebUser.IsContentAdmin || SiteUtils.UserIsSiteEditor();
            useFolderForSiteDetection = WebConfigSettings.UseFoldersInsteadOfHostnamesForMultipleSites;
            divShowPasswordStrength.Visible = WebConfigSettings.EnableAjaxControlPasswordStrength;

#if!MONO
            divTimeZone.Visible = true;

#endif
            if (Request.QueryString["t"] != null)
            {
                requestedTab = Request.QueryString["t"];
            }

            divMyPage.Visible = WebConfigSettings.MyPageIsInstalled;
            divMyPageSkin.Visible = WebConfigSettings.MyPageIsInstalled;
            //myPagePermissionHeading.Visible = WebConfigSettings.MyPageIsInstalled;
            //myPagePermissionsDiv.Visible = WebConfigSettings.MyPageIsInstalled;

            if (SiteUtils.SslIsAvailable())
            {
                this.sslIsAvailable = true;
                this.divSSL.Visible = true;
            }
            else
            {
                this.divSSL.Visible = false;
            }

            lblSiteRoot.Text = SiteRoot;
            if (WebConfigSettings.GloballyDisableMemberUseOfWindowsLiveMessenger)
            {
                divLiveMessenger.Visible = false;
            }

            divApprovalsWorkflow.Visible = WebConfigSettings.EnableContentWorkflow;

            enableSiteSettingsSmtpSettings = WebConfigSettings.EnableSiteSettingsSmtpSettings;
            maskSMTPPassword = WebConfigSettings.MaskSmtpPasswordInSiteSettings;

            divOpenIDSelector.Visible = WebConfigSettings.ShowLegacyOpenIDSelector;
            divOpenID.Visible = WebConfigSettings.EnableOpenIdAuthentication;

            IsServerAdmin = siteSettings.IsServerAdminSite;

            divSiteIsClosed.Visible = (WebConfigSettings.AllowClosingSites) && (isAdmin || WebUser.IsContentAdmin);
            

            //divAdditionalMeta.Visible = WebConfigSettings.ShowAdditionalMeta;
            //divPageEncoding.Visible = WebConfigSettings.ShowPageEncoding;
            //txtDefaultPageEncoding.Text = mojoSetup.DefaultPageEncoding;

            

            currentSiteID = siteSettings.SiteId;
            currentSiteGuid = siteSettings.SiteGuid;

            selectedSiteID = siteSettings.SiteId;

            if ((IsServerAdmin) 
                && (Page.Request.Params.Get("SiteID") != null)
                )
            {
                selectedSiteID  = WebUtils.ParseInt32FromQueryString("SiteID", selectedSiteID);

            }

            if ((selectedSiteID != siteSettings.SiteId)
                && (selectedSiteID > -1))
            {
                selectedSite = new SiteSettings((selectedSiteID));
                
            }
            else
            {
                selectedSite = siteSettings;
            }

            if (WebConfigSettings.SiteLogoUseMediaFolder)
            {
                logoPath = ImageSiteRoot
                + "/Data/Sites/" + selectedSite.SiteId.ToString() + "/media/logos/";
            }
            else
            {
                logoPath = ImageSiteRoot
                + "/Data/Sites/" + selectedSite.SiteId.ToString() + "/logos/";
            }

            

            imgLogo.Src = ImageSiteRoot
                + "/Data/SitesImages/1x1.gif";

            allowPasswordFormatChange
                = ((IsServerAdmin && WebConfigSettings.AllowPasswordFormatChange) || ((IsServerAdmin) && (selectedSiteID == -1)));

            if ((!IsServerAdmin) && (!WebConfigSettings.AllowPasswordFormatChangeInChildSites))
            {
                allowPasswordFormatChange = false;
            }

           
                //h3DefaultRootPageViewRoles.Visible = true;
                //divDefaultRootPageViewRoles.Visible = true;
                //h3DefaultRootPageEditRoles.Visible = true;
                //divDefaultRootPageEditRoles.Visible = true;
                //h3DefaultRootPageCreateChildPageRoles.Visible = true;
                //divDefaultRootPageCreateChildPageRoles.Visible = true;
            

                if (!WebConfigSettings.AllowMultipleSites)
                {
                    this.IsServerAdmin = false;
                }
                //else
                //{
                //    if ((!IsServerAdmin) && (WebConfigSettings.UseRelatedSiteMode))
                //    {
                //        // in related sites mode these are propagated from the master site
                //        // and should not be editable here
                //        chkRolesThatCanCreateUsers.Enabled = false;
                //        chkRolesThatCanLookupUsers.Enabled = false;
                //        chkRolesThatCanManageUsers.Enabled = false;
                //        chkRolesThatCanViewMemberList.Enabled = false;
                        

                //    }

                //}

            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            // I use this to prevent users from changing the mobile skin on the demo site
            ddMobileSkin.Enabled = siteSettings.IsServerAdminSite || WebConfigSettings.AllowSettingMobileSkinInChildSites || WebConfigSettings.MobilePhoneSkin.Length > 0;

            AddClassToBody("administration");
            AddClassToBody("sitesettings");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnAddFeature.Click += new EventHandler(btnAddFeature_Click);
            this.btnRemoveFeature.Click += new EventHandler(btnRemoveFeature_Click);
            this.btnAddHost.Click += new EventHandler(btnAddHost_Click);
            this.btnAddFolder.Click += new EventHandler(btnAddFolder_Click);
            this.rptHosts.ItemCommand += new RepeaterCommandEventHandler(rptHosts_ItemCommand);
            this.rptHosts.ItemDataBound += new RepeaterItemEventHandler(rptHosts_ItemDataBound);
            this.rptFolderNames.ItemCommand += new RepeaterCommandEventHandler(rptFolderNames_ItemCommand);
            this.rptFolderNames.ItemDataBound += new RepeaterItemEventHandler(rptFolderNames_ItemDataBound);
            this.btnAddWebPart.Click += new EventHandler(btnAddWebPart_Click);
            this.btnRemoveWebPart.Click += new EventHandler(btnRemoveWebPart_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            btnRestoreSkins.Click += new EventHandler(btnRestoreSkins_Click);
            btnSetupRpx.Click += new EventHandler(btnSetupRpx_Click);

            //ddSiteList.SelectedIndexChanged += new EventHandler(ddSiteList_SelectedIndexChanged);
            ddDefaultCountry.SelectedIndexChanged += new EventHandler(ddDefaultCountry_SelectedIndexChanged);

            SuppressMenuSelection();
            SuppressPageMenu();
            //ScriptConfig.IncludeYuiTabs = true;
            //IncludeYuiTabsCss = true;

            //JQueryUIThemeName = "base";
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
