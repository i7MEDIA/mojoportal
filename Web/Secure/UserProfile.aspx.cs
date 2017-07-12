/// Author:				        
/// Created:			        2004-09-26
///	Last Modified:              2014-03-28
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Text;
using System.Web.Security;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.ProfileUpdatedHandlers;
using mojoPortal.Web.Configuration;
using mojoPortal.Web.Editor;
using mojoPortal.FileSystem;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI.Pages
{

    public partial class UserProfile : NonCmsBasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(UserProfile));
        private string userEmail = string.Empty;
        //private string avatarPath = string.Empty;
        private SiteUser siteUser;
        private Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;
        //Gravatar public enum RatingType { G, PG, R, X }
        private Avatar.RatingType MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();
        private bool allowGravatars = false;
        private bool disableAvatars = true;
        private CommerceConfiguration commerceConfig = null;
        private string rpxApiKey = string.Empty;
        private string rpxApplicationName = string.Empty;
        //private string avatarBasePath = string.Empty;
        private bool allowUserSkin = false;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            
        }

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            btnUpdateAvartar.Click += new System.Web.UI.ImageClickEventHandler(btnUpdateAvartar_Click);
           // btnUploadAvatar.Click += new EventHandler(btnUploadAvatar_Click);

            if (WebConfigSettings.HideAllMenusOnProfilePage)
            {
                SuppressAllMenus();
            }
            else if (WebConfigSettings.HidePageMenuOnProfilePage) { SuppressPageMenu(); }

            SuppressMenuSelection();
            SetupAvatarScript();
            

            
        }

        

        

        #endregion

        private void Page_Load(object sender, EventArgs e)
		{
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }
            
            
            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            SecurityHelper.DisableBrowserCache();

            if (!WebConfigSettings.AllowUserProfilePage)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            siteUser = SiteUtils.GetCurrentSiteUser();
            LoadSettings();

            PopulateProfileControls();
            if (siteUser != null)
            {
                purchaseHx.UserGuid = siteUser.UserGuid;
            }

			PopulateLabels();
            //SetupAvatarScript();

			if (!IsPostBack)
			{
				PopulateControls();
			}
		}
		

		private void PopulateControls()
		{
            
            this.lnkChangePassword.NavigateUrl = SiteRoot + "/Secure/ChangePassword.aspx";
            this.lnkChangePassword.Text = Resource.UserChangePasswordLabel;
            ListItem listItem;

            if (allowUserSkin)
            {
                if (siteUser != null)
                {
                    SkinSetting.SetValue(siteUser.Skin);
                }
            }

            if ((siteSettings.AllowUserEditorPreference) && (siteUser != null) && (siteUser.EditorPreference.Length > 0))
            {

                listItem = ddEditorProviders.Items.FindByValue(siteUser.EditorPreference);
                if (listItem != null)
                {
                    ddEditorProviders.ClearSelection();
                    listItem.Selected = true;
                }

            }


			
			if(siteUser != null)
			{
#if!MONO
                ISettingControl setting = timeZoneSetting as ISettingControl;
                if (setting != null)
                {
                    setting.SetValue(siteUser.TimeZoneId);
                }

#endif

                txtName.Text = SecurityHelper.RemoveMarkup(siteUser.Name);
                txtName.Enabled = siteSettings.AllowUserFullNameChange;
                lblLoginName.Text = SecurityHelper.RemoveMarkup(siteUser.LoginName);
                txtEmail.Text = siteUser.Email;
                //gravatar1.Email = siteUser.Email;
                lblOpenID.Text = siteUser.OpenIdUri;
                txtPasswordQuestion.Text = siteUser.PasswordQuestion;
                txtPasswordAnswer.Text = siteUser.PasswordAnswer;
                lblCreatedDate.Text = siteUser.DateCreated.AddHours(timeOffset).ToString();
                lblTotalPosts.Text = siteUser.TotalPosts.ToString();
                lnkUserPosts.UserId = siteUser.UserId;
                lnkUserPosts.TotalPosts = siteUser.TotalPosts;
                //lnkPublicProfile.NavigateUrl = SiteRoot + "/ProfileView.aspx?userid=" + siteUser.UserId.ToInvariantString();
                lnkPubProfile.NavigateUrl = SiteRoot + "/ProfileView.aspx?userid=" + siteUser.UserId.ToInvariantString();

                if (divLiveMessenger.Visible)
                {
                    WindowsLiveLogin wl = WindowsLiveHelper.GetWindowsLiveLogin();
                    WindowsLiveMessenger m = new WindowsLiveMessenger(wl);

                    if (WebConfigSettings.TestLiveMessengerDelegation)
                    {
                        lnkAllowLiveMessenger.NavigateUrl = m.ConsentOptInUrl;
                    }
                    else
                    {
                        lnkAllowLiveMessenger.NavigateUrl = m.NonDelegatedSignUpUrl;
                    }

                    if (siteUser.LiveMessengerId.Length > 0)
                    {
                        chkEnableLiveMessengerOnProfile.Checked = siteUser.EnableLiveMessengerOnProfile;
                        chkEnableLiveMessengerOnProfile.Enabled = true;
                    }
                    else
                    {
                        chkEnableLiveMessengerOnProfile.Checked = false;
                        chkEnableLiveMessengerOnProfile.Enabled = false;
                    }
                }


                //if ((!allowGravatars)&&(!disableAvatars))
                //{
                //    if (siteUser.AvatarUrl.Length > 0)
                //    {
                //        imgAvatar.Src = avatarPath + siteUser.AvatarUrl;
                //    }
                //    else
                //    {
                //        imgAvatar.Src = Page.ResolveUrl(WebConfigSettings.DefaultBlankAvatarPath);
                //    }
                //}

                userAvatar.UseGravatar = allowGravatars;
                userAvatar.Email = siteUser.Email;
                userAvatar.UserName = siteUser.Name;
                userAvatar.UserId = siteUser.UserId;
                userAvatar.AvatarFile = siteUser.AvatarUrl;
                userAvatar.MaxAllowedRating = MaxAllowedGravatarRating;
                userAvatar.Disable = disableAvatars;
                userAvatar.SiteId = siteSettings.SiteId;
                userAvatar.UseLink = false;

                
			}
            
            // this doesn't work
            //DoTabSelection();

            
			
		}

        private void PopulateProfileControls()
        {
            if (siteUser == null) { return; }
            
            mojoProfileConfiguration profileConfig = mojoProfileConfiguration.GetConfig();
            if (profileConfig != null)
            {
                foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
                {
#if!MONO
                    // we are using the new TimeZoneInfo list but it doesn't work under Mono
                    // this makes us skip the TimeOffsetHours setting from mojoProfile.config which is not used under windows
                    if (propertyDefinition.Name == mojoProfilePropertyDefinition.TimeOffsetHoursKey) { continue; }
#endif

                    // we allow this to be configured as a profile property so it can be required for registration
                    // but we don't need to load it here because we have a dedicated control for the property already
                    if (propertyDefinition.Name == mojoProfilePropertyDefinition.TimeZoneIdKey) { continue; }

                    if (
                         (propertyDefinition.VisibleToUser)
                      &&    (
                            (propertyDefinition.OnlyAvailableForRoles.Length == 0)
                            ||(siteUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles))
                            )
                        )
                    {
                        object propValue = siteUser.GetProperty(propertyDefinition.Name, propertyDefinition.SerializeAs, propertyDefinition.LazyLoad);
                        if (propValue != null)
                        {
                            if (propertyDefinition.EditableByUser)
                            {
                                mojoProfilePropertyDefinition.SetupPropertyControl(
                                    this,
                                    pnlProfileProperties,
                                    propertyDefinition,
                                    propValue.ToString(),
                                    timeOffset,
                                    timeZone,
                                    SiteRoot);
                            }
                            else
                            {
                                mojoProfilePropertyDefinition.SetupReadOnlyPropertyControl(
                                    pnlProfileProperties,
                                    propertyDefinition,
                                    propValue.ToString(),
                                    timeOffset,
                                    timeZone);
                            }
                        }
                        else
                        {
                            if (propertyDefinition.EditableByUser)
                            {
                                mojoProfilePropertyDefinition.SetupPropertyControl(
                                    this,
                                    pnlProfileProperties,
                                    propertyDefinition,
                                    propertyDefinition.DefaultValue,
                                    timeOffset,
                                    timeZone,
                                    SiteRoot);
                            }
                            else
                            {
                                mojoProfilePropertyDefinition.SetupReadOnlyPropertyControl(
                                    pnlProfileProperties,
                                    propertyDefinition,
                                    propertyDefinition.DefaultValue,
                                    timeOffset,
                                    timeZone);
                            }

                        }
                    }

                }
            }

           
        }

        

		
		private void btnUpdate_Click(Object sender, EventArgs e)
		{
			if(siteUser != null)
			{
				Page.Validate("profile");
				if(Page.IsValid)
				{
					UpdateUser();
				}
			}
			
			
		}

		private void UpdateUser()
		{
            userEmail = siteUser.Email;

            if (
                (siteUser.Email != txtEmail.Text)
                && (SiteUser.EmailExistsInDB(siteSettings.SiteId, siteUser.UserId, txtEmail.Text))
                )
            {
                lblErrorMessage.Text = Resource.DuplicateEmailMessage;
                return;
            }

            if ((siteSettings.AllowUserEditorPreference)&&(divEditorPreference.Visible))
            {
                siteUser.EditorPreference = ddEditorProviders.SelectedValue;
            }

            if (siteSettings.AllowUserFullNameChange)
            {
                siteUser.Name = txtName.Text;
            }
            siteUser.Email = txtEmail.Text;

            if (WebConfigSettings.LogIpAddressForEmailChanges)
            {
                if ((siteUser.UserId != -1) && (userEmail != siteUser.Email))
                {
                    log.Info("email for user changed from " + userEmail + " to " + siteUser.Email + " from ip address " + SiteUtils.GetIP4Address());
                }
            }

            if (pnlSecurityQuestion.Visible)
            {
                siteUser.PasswordQuestion = this.txtPasswordQuestion.Text;
                siteUser.PasswordAnswer = this.txtPasswordAnswer.Text;
            }
            else
            {
                //in case it is ever changed later to require password question and answer after making it not required
                // we need to ensure there is some question and answer.
                if (siteUser.PasswordQuestion.Length == 0) 
                { 
                    siteUser.PasswordQuestion = Resource.ManageUsersDefaultSecurityQuestion;
                    siteUser.PasswordAnswer = Resource.ManageUsersDefaultSecurityAnswer;
                }


            }


            if (siteUser.LiveMessengerId.Length > 0)
            {
                siteUser.EnableLiveMessengerOnProfile = chkEnableLiveMessengerOnProfile.Checked;
            }
            else
            {
                siteUser.EnableLiveMessengerOnProfile = false;
            }

            if (allowUserSkin)
			{
                siteUser.Skin = SkinSetting.GetValue();
                //if (ddSkins.SelectedValue != "printerfriendly")
                //{
                //    siteUser.Skin = ddSkins.SelectedValue;
                //}

			}

#if!MONO
            ISettingControl setting = timeZoneSetting as ISettingControl;
            if (setting != null)
            {
                siteUser.TimeZoneId = setting.GetValue();
            }
            

#endif

            //if ((!disableOldAvatars)&&(!WebConfigSettings.OnlyAdminsCanEditCheesyAvatars))
            //{ siteUser.AvatarUrl = ddAvatars.SelectedValue; }
            siteUser.PasswordFormat = siteSettings.PasswordFormat;

            if (siteUser.Save())
			{
                mojoProfileConfiguration profileConfig = mojoProfileConfiguration.GetConfig();

                foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
                {
                    if (
                        (propertyDefinition.EditableByUser)
                         && (
                              (propertyDefinition.OnlyAvailableForRoles.Length == 0)
                              ||(WebUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles))
                            )
                        )
                    {
                        mojoProfilePropertyDefinition.SaveProperty(
                            siteUser, 
                            pnlProfileProperties, 
                            propertyDefinition,
                            timeOffset,
                            timeZone);
                    }
                }

                siteUser.UpdateLastActivityTime();
                if ((userEmail != siteUser.Email)&&(siteSettings.UseEmailForLogin)&&(!siteSettings.UseLdapAuth))
				{
                    FormsAuthentication.SetAuthCookie(siteUser.Email, false);
				}

                ProfileUpdatedEventArgs u = new ProfileUpdatedEventArgs(siteUser, false);
                OnUserUpdated(u);

                SiteUtils.SetSkinCookie(siteUser);
				WebUtils.SetupRedirect(this, Request.RawUrl);
				return;
			}

		}

        protected void OnUserUpdated(ProfileUpdatedEventArgs e)
        {
            foreach (ProfileUpdatedHandlerProvider handler in ProfileUpdatedHandlerProviderManager.Providers)
            {
                handler.ProfileUpdatedHandler(null, e);
            }


        }

        

        private void LoadSettings()
        {
            
            //avatarPath = Page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/useravatars/");
#if!MONO
            divTimeZone.Visible = true;

#endif

            QuestionRequired.Enabled = siteSettings.RequiresQuestionAndAnswer;
            AnswerRequired.Enabled = siteSettings.RequiresQuestionAndAnswer;

            //lnkAvatarUpload.Visible = false;

            switch (siteSettings.AvatarSystem)
            {
                case "gravatar":
                    allowGravatars = true;
                    disableAvatars = false;
                    break;

                case "internal":
                    allowGravatars = false;
                    disableAvatars = false;

                    
                    if (siteUser != null)
                    {
                       // lnkAvatarUpload.NavigateUrl = SiteRoot + "/Dialog/AvatarUploadDialog.aspx?u=" + siteUser.UserId.ToInvariantString();
                       // lnkAvatarUpload.ClientClick = "return GB_showPage('" + Page.Server.HtmlEncode(Resource.UploadAvatarHeading) + "', this.href, GBCallback)";
                        lnkAvatarUpld.NavigateUrl = SiteRoot + "/Dialog/AvatarUploadDialog.aspx?u=" + siteUser.UserId.ToInvariantString();
                    }

                    if (WebConfigSettings.AvatarsCanOnlyBeUploadedByAdmin)
                    {
                        //lnkAvatarUpload.Visible = false;
                        lnkAvatarUpld.Visible = false;
                        //lblMaxAvatarSize.Visible = false;
                        //avatarFile.Visible = false;
                       // progressBar.Visible = false;
                        //btnUploadAvatar.Visible = false;
                       // regexAvatarFile.Visible = false;
                       // regexAvatarFile.Enabled = false;
                        avatarHelp.Visible = false;
                    }

                    break;

                case "none":
                default:
                    allowGravatars = false;
                    disableAvatars = true;
                    break;

            }

            if (displaySettings.OverrideAvatarLabel.Length > 0)
            {
                lblAvatar.ConfigKey = displaySettings.OverrideAvatarLabel;
            }

            if (displaySettings.HidePostCount) { divForumPosts.Visible = false; }

            allowUserSkin = ((siteSettings.AllowUserSkins) || ((WebConfigSettings.AllowEditingSkins) && (WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins))));

            if (allowUserSkin)
            {
                this.divSkin.Visible = true;
                ScriptConfig.IncludeColorBox = true;
            }
            else
            {
                this.divSkin.Visible = false;
                SkinSetting.Visible = false;
            }

            if (siteSettings.UseLdapAuth && !siteSettings.AllowDbFallbackWithLdap)
            {
                this.lnkChangePassword.Visible = false;
            }

            divEditorPreference.Visible = siteSettings.AllowUserEditorPreference;

            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            divOpenID.Visible = WebConfigSettings.EnableOpenIdAuthentication && siteSettings.AllowOpenIdAuth;

            rpxApiKey = siteSettings.RpxNowApiKey;
            rpxApplicationName = siteSettings.RpxNowApplicationName;

            if (WebConfigSettings.UseOpenIdRpxSettingsFromWebConfig)
            {
                if (WebConfigSettings.OpenIdRpxApiKey.Length > 0)
                {
                    rpxApiKey = WebConfigSettings.OpenIdRpxApiKey;
                }

                if (WebConfigSettings.OpenIdRpxApplicationName.Length > 0)
                {
                    rpxApplicationName = WebConfigSettings.OpenIdRpxApplicationName;
                }

            }

            if (rpxApiKey.Length > 0)
            {
                lnkOpenIDUpdate.Visible = false;
                rpxLink.Visible = divOpenID.Visible;
            }

            if (!mojoSetup.RunningInFullTrust())
            {
                divOpenID.Visible = false;
            }

            if (
                (WebConfigSettings.GloballyDisableMemberUseOfWindowsLiveMessenger)
                ||(!siteSettings.AllowWindowsLiveMessengerForMembers)
                || ((siteSettings.WindowsLiveAppId.Length == 0) && (ConfigurationManager.AppSettings["GlobalWindowsLiveAppKey"] == null))
                )
            {
                divLiveMessenger.Visible = false;
            }

            int countOfNewsLetters = LetterInfo.GetCount(siteSettings.SiteGuid);

            liNewsletters.Visible = (WebConfigSettings.EnableNewsletter && (countOfNewsLetters > 0));
            tabNewsletters.Visible = liNewsletters.Visible;
            newsLetterPrefs.Visible = liNewsletters.Visible;

            commerceConfig = SiteUtils.GetCommerceConfig();
            if (!commerceConfig.IsConfigured)
            {
                liOrderHistory.Visible = false;
                tabOrderHistory.Visible = false;

            }

            if ((WebConfigSettings.UserNameValidationExpression.Length > 0) && (siteSettings.AllowUserFullNameChange))
            {
                regexUserName.ValidationExpression = WebConfigSettings.UserNameValidationExpression;
                regexUserName.ErrorMessage = WebConfigSettings.UserNameValidationWarning;
                regexUserName.Enabled = true;

            }

            FailSafeUserNameValidator.ErrorMessage = Resource.UserNameHasInvalidCharsWarning;
            FailSafeUserNameValidator.ServerValidate += new ServerValidateEventHandler(FailSafeUserNameValidator_ServerValidate);
            
            AddClassToBody("userprofile");
        }

        void FailSafeUserNameValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args.Value.Contains("<")) { args.IsValid = false; }
            if (args.Value.Contains(">")) { args.IsValid = false; }
            if (args.Value.Contains("/")) { args.IsValid = false; }
            if (args.Value.IndexOf("script", StringComparison.InvariantCultureIgnoreCase) > -1) { args.IsValid = false; }
            //if (args.Value.IndexOf("java", StringComparison.InvariantCultureIgnoreCase) > -1) { args.IsValid = false; }
        }

       
        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.ProfileLink);

            litSecurityTab.Text = "<a href='#tabSecurity'>" + Resource.UserProfileSecurityTab + "</a>";
            litProfileTab.Text = "<a href='#" + tabProfile.ClientID + "'>" + Resource.UserProfileProfileTab + "</a>";
            litOrderHistoryTab.Text = "<a href='#" + tabOrderHistory.ClientID + "'>" + Resource.CommerceOrderHistoryTab + "</a>";
            litNewsletterTab.Text = "<a href='#" + tabNewsletters.ClientID + "'>" + Resource.UserProfileNewslettersTab + "</a>";

            lnkAllowLiveMessenger.Text = Resource.EnableLiveMessengerLink;

            btnUpdate.Text = Resource.UserProfileUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, AccessKeys.UserProfileSaveButtonAccessKey);

            if ((siteSettings.AllowUserEditorPreference) && (ddEditorProviders.Items.Count == 0))
            {
                ddEditorProviders.DataSource = EditorManager.Providers;
                ddEditorProviders.DataBind();
                foreach (ListItem providerItem in ddEditorProviders.Items)
                {
                    providerItem.Text = providerItem.Text.Replace("Provider", string.Empty);
                }

                ListItem listItem = new ListItem();
                listItem.Value = string.Empty;
                listItem.Text = Resource.SiteDefaultEditor;
                ddEditorProviders.Items.Insert(0, listItem);
            }

            //if ((!allowGravatars)&&(!disableOldAvatars)&&(!Page.IsPostBack))
            //{
            //    ddAvatars.DataSource = SiteUtils.GetAvatarList(this.siteSettings);
            //    ddAvatars.DataBind();

            //    ddAvatars.Items.Insert(0, new ListItem(Resource.UserProfileNoAvatarLabel, "blank.gif"));
            //    ddAvatars.Attributes.Add("onChange", "javascript:showAvatar(this);");
            //    ddAvatars.Attributes.Add("size", "6");
            //}

           // lnkPublicProfile.DialogCloseText = Resource.DialogCloseLink;
            //lnkPublicProfile.Text = Resource.PublicProfileLink;

            lnkPubProfile.Text = Resource.PublicProfileLink;
            lnkPubProfile.ToolTip = Resource.PublicProfileLink;

            rfvName.ErrorMessage = Resource.UserProfileNameRequired;
            regexEmail.ErrorMessage = Resource.UserProfileEmailValidation;
            rfvEmail.ErrorMessage = Resource.UserProfileEmailRequired;

            QuestionRequired.ErrorMessage = Resource.RegisterSecurityQuestionRequiredMessage;
            AnswerRequired.ErrorMessage = Resource.RegisterSecurityAnswerRequiredMessage;
            //regexAvatarFile.ErrorMessage = Resource.FileTypeNotAllowed;
            //regexAvatarFile.ValidationExpression = SecurityHelper.GetRegexValidationForAllowedExtensions(SiteUtils.ImageFileExtensions());

            //lnkAvatarUpload.Text = Resource.UploadAvatarLink;
            lnkAvatarUpld.Text = Resource.UploadAvatarLink;
            lnkAvatarUpld.ToolTip = Resource.UploadAvatarLink;


            btnUpdateAvartar.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/1x1.gif");
            btnUpdateAvartar.Attributes.Add("tabIndex", "-1");
           

            if (allowGravatars)
            {
                //gravatar1.Visible = true;
                //ddAvatars.Visible = false;
                //imgAvatar.Visible = false;
                avatarHelp.Visible = false;
                //lnkAvatarUpload.Visible = false;
                lnkAvatarUpld.Visible = false;
                
            }
            else 
            {
                //gravatar1.Visible = false;

                if (disableAvatars)
                {
                    divAvatar.Visible = false;
                    //imgAvatar.Visible = false;
                    avatarHelp.Visible = false;
                    //lnkAvatarUpload.Visible = false;
                    lnkAvatarUpld.Visible = false;
                   
                }
                else
                {
                    //lblMaxAvatarSize.Text = string.Format(Resource.AvatarMaxSizeLabelFormat, WebConfigSettings.AvatarMaxWidth.ToInvariantString(), WebConfigSettings.AvatarMaxHeight.ToInvariantString());
                    //imgAvatar.Visible = true;

                    if (!WebConfigSettings.AvatarsCanOnlyBeUploadedByAdmin)
                    {
                        avatarHelp.Visible = true;
                 
                    }
                }
               
            }

            lnkOpenIDUpdate.Text = Resource.OpenIDUpdateButton;
            lnkOpenIDUpdate.ToolTip = Resource.OpenIDUpdateButton;
            lnkOpenIDUpdate.NavigateUrl = SiteRoot + "/Secure/UpdateOpenID.aspx";

            rpxLink.OverrideText = Resource.OpenIDUpdateButton;

            pnlSecurityQuestion.Visible = siteSettings.RequiresQuestionAndAnswer;
            

        }

        void btnUpdateAvartar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
   
            // this is fired when the avatar upload dialog is closed
            // we don't really know for sure if the image was updated
            // but if it was we should rename it since the previous version may be cached by web browsers
            // so we'll check if the files was modified recently, and if so rename it
            if ((siteUser != null) && siteUser.AvatarUrl.Length > 0)
            {
                FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
                if (p != null)
                {
                    IFileSystem fileSystem = p.GetFileSystem();
                    string avatarBasePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/useravatars/";
                    WebFile avatarFile = fileSystem.RetrieveFile(avatarBasePath + siteUser.AvatarUrl);
                    if (avatarFile != null)
                    {
                        if (avatarFile.Modified > DateTime.Today)
                        {
                            // it was updated today so we'll assume it was just now since the avatar dialog just closed
                            string newfileName = "user" 
                                + siteUser.UserId.ToInvariantString() 
                                + "-" + siteUser.Name.ToCleanFileName() 
                                + "-" + DateTime.UtcNow.Millisecond.ToInvariantString()
                                + System.IO.Path.GetExtension(siteUser.AvatarUrl);

                            fileSystem.MoveFile(avatarBasePath + siteUser.AvatarUrl, avatarBasePath + newfileName, true);
                            siteUser.AvatarUrl = newfileName;
                            siteUser.Save();

                        }

                    }
                }


            }

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        
        private void SetupAvatarScript()
        {
            StringBuilder script = new StringBuilder();

            script.Append("<script type=\"text/javascript\">");
            script.Append("$('#" + lnkAvatarUpld.ClientID + "').colorbox({width:\"80%\", height:\"80%\", iframe:true, onClosed:CBCallback}); ");

            script.Append("function CBCallback() {");

            //script.Append(" window.location.reload(true); ");
            script.Append("var btn = document.getElementById('" + this.btnUpdateAvartar.ClientID + "');  ");
            script.Append("btn.click(); ");
            //script.Append("$.colorbox.close(); ");

            script.Append("}");

            script.Append("</script>");
            

            //string init = "<script type=\"text/javascript\">"
            //    + "$('#" + lnkAvatarUpld.ClientID + "').colorbox({width:\"80%\", height:\"80%\", iframe:true, onClosed:GBCallback}); function GBCallback() { window.location.reload(true); } "
            //    + "</script>";

            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "cbupinit", script.ToString());
        }

	}
}
