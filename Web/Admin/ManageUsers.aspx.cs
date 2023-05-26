// Author:					    
// Created:				        2004-08-29
// Last Modified:			    2019-10-08 (i7MEDIA)
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.ProfileUpdatedHandlers;
using mojoPortal.Business.WebHelpers.UserRegisteredHandlers;
using mojoPortal.Net;
using mojoPortal.Web.Configuration;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.AdminUI
{
	public partial class ManageUsers : NonCmsBasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(ManageUsers));
        private Guid userGuid = Guid.Empty;
        private int pageID = -1;
        private int userID = -1;
        //private string AvatarPath = string.Empty;
        private SiteUser siteUser = null;
        private SiteUser currentUser = null;
        protected Double TimeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        private bool isAdmin = false;
       
        //Gravatar public enum RatingType { G, PG, R, X }
        private mojoPortal.Web.UI.Avatar.RatingType MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();
        private bool allowGravatars = false;
        private bool disableAvatars = true;
        private CommerceConfiguration commerceConfig = null;
        protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;
        private SubscriberRepository subscriptions = new SubscriberRepository();

		
        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            //this.addExisting.Click += new EventHandler(this.AddRole_Click);
            //this.userRoles.ItemDataBound += new DataListItemEventHandler(userRoles_ItemDataBound);
            //this.userRoles.ItemCommand += new DataListCommandEventHandler(this.UserRoles_ItemCommand);
            this.btnUnlockUser.Click += new EventHandler(btnUnlockUser_Click);
            this.btnLockUser.Click += new EventHandler(btnLockUser_Click);
            this.btnConfirmEmail.Click += new EventHandler(btnConfirmEmail_Click);
            btnApprove.Click += new EventHandler(btnApprove_Click);
            btnPurgeUserLocations.Click += new EventHandler(btnPurgeUserLocations_Click);
            btnResendConfirmationEmail.Click += new EventHandler(btnResendConfirmationEmail_Click);
            //btnUploadAvatar.Click += new EventHandler(btnUploadAvatar_Click);

            SuppressMenuSelection();
            SuppressPageMenu();
            EnableViewState = true;

            
        }

        

        #endregion

        private void Page_Load(object sender, EventArgs e)
		{
            
            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            SecurityHelper.DisableBrowserCache();

            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();

            if (this.userID > -1)
            {
                siteUser = new SiteUser(siteSettings, this.userID);
                newsLetterPrefs.UserGuid = siteUser.UserGuid;
                purchaseHx.UserGuid = siteUser.UserGuid;
                purchaseHx.ShowAdminOrderLink = true;
                UserRolesControl.UserId = userID;

                
            }
            else
            {
                if (userGuid != Guid.Empty)
                {
                    siteUser = new SiteUser(siteSettings, userGuid);
                    newsLetterPrefs.UserGuid = siteUser.UserGuid;
                    purchaseHx.UserGuid = siteUser.UserGuid;
                    purchaseHx.ShowAdminOrderLink = true;
                    UserRolesControl.UserId = siteUser.UserId;

                }
            }

			//someone is trying to edit a user from another site or a non-existent user
			if (siteUser != null && siteUser.UserId == -1)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);
				return;
			}


            if (userID == -1)
            {
                if ((!WebUser.IsInRoles(siteSettings.RolesThatCanCreateUsers))&&(!WebUser.IsInRoles(siteSettings.RolesThatCanManageUsers)))
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                    return;
                }

            }
            else
            {
                if (
                    (WebUser.IsInRoles(siteSettings.RolesThatCanManageUsers)||WebUser.IsInRoles(siteSettings.RolesThatCanCreateUsers))
                    && !isAdmin
                    )
                {
                    // only admins can edit admins
                    if (siteUser.IsInRoles("Admins"))
                    {
                        SiteUtils.RedirectToAccessDeniedPage(this);
                        return;
                    }
                    
                    HideAdminControls();
                   
                }
                else
                {
                    if (!isAdmin)
                    {
                        

                        SiteUtils.RedirectToAccessDeniedPage(this);
                        return;
                    }
                }
            }

            //SetupAvatarScript();

			this.divUserGuid.Visible = false;
            divProfileApproved.Visible = false;
            divApprovedForLogin.Visible = false;

            if (
                (siteUser != null) 
                && (siteSettings.RequireApprovalBeforeLogin) 
                && (!siteUser.ApprovedForLogin) 
                && ((WebUser.IsInRoles(siteSettings.RolesThatCanCreateUsers)) || (WebUser.IsInRoles(siteSettings.RolesThatCanManageUsers)))
                )
            {
                divApprovedForLogin.Visible = true;
            }
            
            divOpenID.Visible = ((WebConfigSettings.EnableOpenIdAuthentication && siteSettings.AllowOpenIdAuth) || siteSettings.RpxNowApiKey.Length > 0);

            divWindowsLiveID.Visible = WebConfigSettings.EnableWindowsLiveAuthentication && siteSettings.AllowWindowsLiveAuth;
           
            PopulateProfileControls();
			PopulateLabels();
           
            if (!IsPostBack)
			{
                PopulateControls();
            }

            
        }

	
		private void PopulateControls()
		{
            if (!siteSettings.RequiresQuestionAndAnswer)
            {
                divSecurityQuestion.Visible = false;
                divSecurityAnswer.Visible = false;
            }

            if ((siteUser != null)&&(siteUser.UserId > -1))
			{
				heading.Text = $"{Resource.ManageUsersTitleLabel} {siteUser.Name}";

				if (siteUser.IsDeleted)
                {
                    btnDelete.Text = Resource.Undelete;
                    btnDelete.ToolTip = Resource.UndeleteUserTooltip;
					UIHelper.RemoveConfirmationDialog(btnDelete);
					UIHelper.AddConfirmationDialog(btnDelete, Resource.ManageUsersUnDeleteUserWarning);
					heading.Text += string.Format(displaySettings.IsDeletedUserNoteFormat, Resource.ManageUsersIsDeleted);
				}

				//spnTitle.InnerText = Resource.ManageUsersTitleLabel + " " + siteUser.Name;

                txtName.Text = SecurityHelper.RemoveMarkup(siteUser.Name);
                txtLoginName.Text = SecurityHelper.RemoveMarkup(siteUser.LoginName);
                txtFirstName.Text = SecurityHelper.RemoveMarkup(siteUser.FirstName);
                txtLastName.Text = SecurityHelper.RemoveMarkup(siteUser.LastName);
               // lnkAvatarUpload.ClientClick = "return GB_showPage('" + Page.Server.HtmlEncode(string.Format(CultureInfo.InvariantCulture, Resource.UploadAvatarForUserFormat, siteUser.Name)) + "', this.href, GBCallback)";
                txtEmail.Text = siteUser.Email;
                txtOpenIDURI.Text = siteUser.OpenIdUri;
                txtWindowsLiveID.Text = siteUser.WindowsLiveId;
                txtLiveMessengerCID.Text = siteUser.LiveMessengerId;
                chkEnableLiveMessengerOnProfile.Checked = siteUser.EnableLiveMessengerOnProfile;
                //gravatar1.Email = siteUser.Email;
                //gravatar1.MaxAllowedRating = MaxAllowedGravatarRating;

                if (siteUser.LastActivityDate > DateTime.MinValue)
                {
                    if (timeZone != null)
                    {
                        lblLastActivityDate.Text = siteUser.LastActivityDate.ToLocalTime(timeZone).ToString();
                    }
                    else
                    {
                        lblLastActivityDate.Text = siteUser.LastActivityDate.AddHours(TimeOffset).ToString();
                    }
                }

                if (siteUser.LastLoginDate > DateTime.MinValue)
                {
                    if (timeZone != null)
                    {
                        lblLastLoginDate.Text = siteUser.LastLoginDate.ToLocalTime(timeZone).ToString();
                    }
                    else
                    {
                        lblLastLoginDate.Text = siteUser.LastLoginDate.AddHours(TimeOffset).ToString();
                    }
                }

                if (siteUser.LastPasswordChangedDate > DateTime.MinValue)
                {
                    if (timeZone != null)
                    {
                        lblLastPasswordChangeDate.Text = siteUser.LastPasswordChangedDate.ToLocalTime(timeZone).ToString();
                    }
                    else
                    {
                        lblLastPasswordChangeDate.Text = siteUser.LastPasswordChangedDate.AddHours(TimeOffset).ToString();
                    }
                }

                if (siteUser.LastLockoutDate > DateTime.MinValue)
                {
                    if (timeZone != null)
                    {
                        lblLastLockoutDate.Text = siteUser.LastLockoutDate.ToLocalTime(timeZone).ToString();
                    }
                    else
                    {
                        lblLastLockoutDate.Text = siteUser.LastLockoutDate.AddHours(TimeOffset).ToString();
                    }
                }
                lblFailedPasswordAttemptCount.Text = siteUser.FailedPasswordAttemptCount.ToString();
                lblFailedPasswordAnswerAttemptCount.Text = siteUser.FailedPasswordAnswerAttemptCount.ToString();
                chkIsLockedOut.Checked = siteUser.IsLockedOut;
                btnLockUser.Visible = !siteUser.IsLockedOut;
                btnUnlockUser.Visible = siteUser.IsLockedOut;

                if (siteSettings.UseSecureRegistration)
                {
                    if (siteUser.RegisterConfirmGuid == Guid.Empty)
                    {
                        chkEmailIsConfirmed.Checked = true;
                        btnConfirmEmail.Enabled = false;
                    }
                }
                else
                {
                    divEmailConfirm.Visible = false;
                }
                
                txtComment.Text = siteUser.Comment;
                txtPasswordQuestion.Text = siteUser.PasswordQuestion;
                txtPasswordAnswer.Text = siteUser.PasswordAnswer;
                chkRequirePasswordChange.Checked = siteUser.MustChangePwd;

                

                if (!siteSettings.UseLdapAuth || (siteSettings.UseLdapAuth && siteSettings.AllowDbFallbackWithLdap))
                {
                    if (siteSettings.PasswordFormat == 0)
                    { //Clear
                        this.txtPassword.Text = siteUser.Password;

                    }
                    else if (siteSettings.PasswordFormat == 2)
                    {
                        try
                        {
                         
                            mojoMembershipProvider mojoMembership = (mojoMembershipProvider)Membership.Provider;
                            string password = mojoMembership.UnencodePassword(siteUser.Password, MembershipPasswordFormat.Encrypted);
                            if (siteUser.PasswordSalt.Length > 0)
                            {
                                password = password.Replace(siteUser.PasswordSalt, string.Empty);
                            }
                            this.txtPassword.Text = password;
                        }
                        catch (FormatException ex)
                        {
                            log.Error("Error decoding password for user " + siteUser.Email + " on manage users page.", ex);
                            // TODO: should we generate a random password and fix it here?
                        }
                    }

                }

                if (timeZone != null)
                {
                    lblCreatedDate.Text = siteUser.DateCreated.ToLocalTime(timeZone).ToString();
                }
                else
                {
                    lblCreatedDate.Text = siteUser.DateCreated.AddHours(TimeOffset).ToString();
                }
                lblUserGuid.Text = siteUser.UserGuid.ToString();
                lblTotalPosts.Text = siteUser.TotalPosts.ToInvariantString();
                lnkUserPosts.UserId = siteUser.UserId;
                lnkUserPosts.TotalPosts = siteUser.TotalPosts;
                lnkUnsubscribeFromForums.NavigateUrl = SiteRoot + "/Forums/UnsubscribeForum.aspx?ue=" + Page.Server.UrlEncode(siteUser.Email);

                chkProfileApproved.Checked = siteUser.ProfileApproved;
                
                chkTrusted.Checked = siteUser.Trusted;
                chkDisplayInMemberList.Checked = siteUser.DisplayInMemberList;

                //if ((!allowGravatars)&&(!disableAvatars))
                //{
                //    if (siteUser.AvatarUrl.Length > 0)
                //    {
                //        imgAvatar.Src = ImageSiteRoot + "/Data/Sites/"
                //            + siteSettings.SiteId.ToInvariantString() + "/useravatars/" + siteUser.AvatarUrl;
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

                if ((siteSettings.AllowUserEditorPreference) && (siteUser != null) && (siteUser.EditorPreference.Length > 0))
                {

                    ListItem listItem = ddEditorProviders.Items.FindByValue(siteUser.EditorPreference);
                    if (listItem != null)
                    {
                        ddEditorProviders.ClearSelection();
                        listItem.Selected = true;
                    }

                }

#if!MONO
                ISettingControl setting = timeZoneSetting as ISettingControl;
                if (setting != null)
                {
                    setting.SetValue(siteUser.TimeZoneId);
                }

#endif

               
                List<UserLocation> userLocations = UserLocation.GetByUser(siteUser.UserGuid);
                grdUserLocation.DataSource = userLocations;
                grdUserLocation.DataBind();

			}
			else
			{
                //spnTitle.InnerText = Resource.ManageUsersAddUserLabel;
                heading.Text = Resource.ManageUsersAddUserLabel;
				HideExtendedProfileControls();
			}

		}

        protected bool CanDeleteUserFromRole(string roleName)
        {
            if (WebUser.IsAdmin) { return true; }

            if (roleName == "Admins") { return false; }
            if (roleName == "Role Admins") { return false; }

            return true;
        }

        private void HideAdminControls()
        {
            if (!WebUser.IsInRoles(siteSettings.RolesThatCanManageUsers))
            {
                divDisplayInMemberList.Visible = false;
                liProfile.Visible = false;
                tabProfile.Visible = false;
                liLocation.Visible = false;
                tabLocation.Visible = false;

                if (userID != -1)
                {
                    btnUpdate.Enabled = false;
                    btnDelete.Enabled = false;
                    txtPassword.TextMode = TextBoxMode.Password;
                    liActivity.Visible = false;
                    tabActivity.Visible = false;
                    liNewsletters.Visible = false;
                    tabNewsletters.Visible = false;
                }
            }

            if (!WebUser.IsRoleAdmin)
            {
                liRoles.Visible = false;
                tabRoles.Visible = false;
            }

            if (!WebUser.IsInRoles(siteSettings.CommerceReportViewRoles))
            {
                liOrderHistory.Visible = false;
                tabOrderHistory.Visible = false;
            }
           
           
            
        }

        private void PopulateProfileControls()
        {
            if (siteUser == null) { return; }
            if ((!isAdmin)&&!WebUser.IsInRoles(siteSettings.RolesThatCanManageUsers)) { return; }
            
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

                    // we allow this to be configured as a profile property so it can be required for registration
                    // but we don't need to load it here because we have a dedicated control for the property already
                    if (propertyDefinition.Name == "FirstName" || propertyDefinition.Name == "LastName") { continue; }

                    if (
                        (propertyDefinition.OnlyAvailableForRoles.Length == 0)
                        ||(siteUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles))
                        )
                    {
                        object propValue = siteUser.GetProperty(propertyDefinition.Name, propertyDefinition.SerializeAs, propertyDefinition.LazyLoad);
                        if (propValue != null)
                        {
                            mojoProfilePropertyDefinition.SetupPropertyControl(
                                this,
                                pnlProfileProperties,
                                propertyDefinition,
                                propValue.ToString(),
                                TimeOffset,
                                timeZone,
                                SiteRoot);
                        }
                        else
                        {
                            mojoProfilePropertyDefinition.SetupPropertyControl(
                                this,
                                pnlProfileProperties,
                                propertyDefinition,
                                propertyDefinition.DefaultValue,
                                TimeOffset,
                                timeZone,
                                SiteRoot);
                        }
                    }
                }
            }

           
        }

       
     
        private void btnUpdate_Click(Object sender, EventArgs e)
		{
            Page.Validate("profile");
            if (Page.IsValid)
            {
                if (this.userID > -1)
                {
                    UpdateUser();
                }
                else
                {
                    CreateUser();
                }
            }
			
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if((siteUser != null)&&(this.userID > -1))
			{
                if(siteUser.IsDeleted)
                {
                    siteUser.UndeleteUser();
                    WebUtils.SetupRedirect(this, Request.RawUrl);
                    return;

                }


                //try
                //{
                    UserPreDeleteEventArgs u = new UserPreDeleteEventArgs(siteUser, !siteSettings.ReallyDeleteUsers);
                    OnDeletingUser(u);
                //}
                //catch (Exception e)
                //{
                //    log.Error(e);
                //}

                siteUser.DeleteUser();

                

				WebUtils.SetupRedirect(this, SiteRoot + WebConfigSettings.MemberListUrl);
                return;

			}

		}

        protected void OnDeletingUser(UserPreDeleteEventArgs e)
        {
            foreach (UserPreDeleteHandlerProvider handler in UserPreDeleteHandlerProviderManager.Providers)
            {
                handler.UserPreDeleteHandler(null, e);
            }


        }

		private void UpdateUser()
		{
            if (siteUser == null) { return; }

            
            if (
                (siteUser.Email != txtEmail.Text)
                && (SiteUser.EmailExistsInDB(siteSettings.SiteId, siteUser.UserId, txtEmail.Text))
                )
            {
                lblErrorMessage.Text = Resource.DuplicateEmailMessage;
                return;
            }

            if (
                (siteUser.LoginName != txtLoginName.Text)
                && (SiteUser.LoginExistsInDB(siteSettings.SiteId, txtLoginName.Text))
                )
            {
                lblErrorMessage.Text = Resource.DuplicateUserNameMessage;
                return;
            }

            string oldEmail = siteUser.Email;
            siteUser.Name = txtName.Text;
            siteUser.LoginName = txtLoginName.Text;
            siteUser.Email = txtEmail.Text;
            siteUser.FirstName = txtFirstName.Text;
            siteUser.LastName = txtLastName.Text;

            if (WebConfigSettings.LogIpAddressForEmailChanges)
            {
                if ((siteUser.UserId != -1) && (oldEmail != siteUser.Email))
                {
                    log.Info("email for user changed from " + oldEmail + " to " + siteUser.Email + " from ip address " + SiteUtils.GetIP4Address());
                }
            }

            if (divOpenID.Visible)
            {
                siteUser.OpenIdUri = txtOpenIDURI.Text;
            }

            string oldPassword = siteUser.Password;

            if (!siteSettings.UseLdapAuth || (siteSettings.UseLdapAuth && siteSettings.AllowDbFallbackWithLdap))
            {
                if (txtPassword.Text.Length > 0)
                {
                    mojoMembershipProvider mojoMembership = (mojoMembershipProvider) Membership.Provider;
                    siteUser.Password = mojoMembership.EncodePassword(siteSettings, siteUser, txtPassword.Text);
                }
            }

            if (WebConfigSettings.LogIpAddressForPasswordChanges)
            {
                if((siteUser.UserId != -1)&&(oldPassword != siteUser.Password))
                {
                    log.Info("password for user " + siteUser.Name + " was changed by an admin user from ip address " + SiteUtils.GetIP4Address());
                }
            }

            siteUser.ProfileApproved = chkProfileApproved.Checked;
            
            siteUser.Trusted = chkTrusted.Checked;
            siteUser.DisplayInMemberList = chkDisplayInMemberList.Checked;
            //siteUser.AvatarUrl = ddAvatars.SelectedValue;

            siteUser.MustChangePwd = chkRequirePasswordChange.Checked;
            siteUser.Comment = txtComment.Text;
            siteUser.PasswordQuestion = txtPasswordQuestion.Text;
            siteUser.PasswordAnswer = txtPasswordAnswer.Text;
            siteUser.PasswordFormat = siteSettings.PasswordFormat;
            siteUser.WindowsLiveId = txtWindowsLiveID.Text;
            siteUser.LiveMessengerId = txtLiveMessengerCID.Text;
            siteUser.EnableLiveMessengerOnProfile = chkEnableLiveMessengerOnProfile.Checked;

            if ((siteSettings.AllowUserEditorPreference) && (divEditorPreference.Visible))
            {
                siteUser.EditorPreference = ddEditorProviders.SelectedValue;
            }

#if!MONO
            ISettingControl setting = timeZoneSetting as ISettingControl;
            if (setting != null)
            {
                siteUser.TimeZoneId = setting.GetValue();
            }

#endif

            if (siteUser.Save())
            {
                mojoProfileConfiguration profileConfig = mojoProfileConfiguration.GetConfig();

                foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
                {

                    mojoProfilePropertyDefinition.SaveProperty(
                        siteUser,
                        pnlProfileProperties,
                        propertyDefinition,
                        TimeOffset,
                        timeZone);
                }

                
                if ((currentUser != null) && (currentUser.UserId == siteUser.UserId))
                {
                    if ((siteSettings.UseEmailForLogin) && (siteUser.Email != currentUser.Email))
                    {
                        FormsAuthentication.SetAuthCookie(siteUser.Email, false);
                    }

                    if ((!siteSettings.UseEmailForLogin) && (siteUser.LoginName != currentUser.LoginName))
                    {
                        FormsAuthentication.SetAuthCookie(siteUser.LoginName, false);
                    }

                }

                ProfileUpdatedEventArgs u = new ProfileUpdatedEventArgs(siteUser, true);
                OnUserUpdated(u);

                WebUtils.SetupRedirect(this, Request.RawUrl);
            }
            
		}

		private void CreateUser()
		{
			
            if (SiteUser.EmailExistsInDB(siteSettings.SiteId, txtEmail.Text))
            {
                lblErrorMessage.Text = Resource.DuplicateEmailMessage;
                return ;
            }

            if (SiteUser.LoginExistsInDB(siteSettings.SiteId, txtLoginName.Text))
            {
                lblErrorMessage.Text = Resource.DuplicateUserNameMessage;
                return;
            }

            SiteUser user = new SiteUser(siteSettings);
			user.Name = txtName.Text;
			user.LoginName = txtLoginName.Text;
			user.Email = txtEmail.Text;
            user.TimeZoneId = siteSettings.TimeZoneId;
            mojoMembershipProvider mojoMembership = (mojoMembershipProvider)Membership.Provider;
		    user.Password = mojoMembership.EncodePassword(siteSettings, user, txtPassword.Text);
            user.MustChangePwd = chkRequirePasswordChange.Checked;
			user.FirstName = txtFirstName.Text;
			user.LastName = txtLastName.Text;

			if(user.Save())
			{
				user.PasswordQuestion = this.txtPasswordQuestion.Text;
                user.PasswordAnswer = this.txtPasswordAnswer.Text;
                user.Save();

                mojoProfileConfiguration profileConfig = mojoProfileConfiguration.GetConfig();
                // set default values
                foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
                {
                    if (propertyDefinition.Name == mojoProfilePropertyDefinition.TimeZoneIdKey) { continue; }
					if (propertyDefinition.Name == "FirstName" || propertyDefinition.Name == "LastName") { continue; }
					mojoProfilePropertyDefinition.SavePropertyDefault(user, propertyDefinition);
                }

                CacheHelper.ClearMembershipStatisticsCache();

                if (WebConfigSettings.NewsletterAutoSubscribeUsersCreatedByAdmin)
                {
                    DoSubscribe(user);
                }

                UserRegisteredEventArgs u = new UserRegisteredEventArgs(user);
                OnUserRegistered(u);

				WebUtils.SetupRedirect(this, SiteRoot 
                    + "/Admin/ManageUsers.aspx?userId=" + user.UserId.ToInvariantString() 
					+ "&username=" + user.Email + "&pageid=" + pageID.ToInvariantString());
				return;
 
			}

		}

        private void DoSubscribe(SiteUser siteUser)
        {
            List<LetterInfo> siteAvailableSubscriptions = NewsletterHelper.GetAvailableNewslettersForSiteMembers(siteSettings.SiteGuid);

            foreach (LetterInfo available in siteAvailableSubscriptions)
            {

                if (available.ProfileOptIn)
                {
                    DoSubscribe(available, siteUser);
                }
            }

            List<LetterSubscriber> memberSubscriptions = subscriptions.GetListByUser(siteUser.SiteGuid, siteUser.UserGuid);
            NewsletterHelper.RemoveDuplicates(memberSubscriptions);


        }

        private void DoSubscribe(LetterInfo letter, SiteUser siteUser)
        {

            LetterSubscriber s = new LetterSubscriber();
            s.SiteGuid = siteSettings.SiteGuid;
            s.EmailAddress = siteUser.Email;
            s.UserGuid = siteUser.UserGuid;
            s.LetterInfoGuid = letter.LetterInfoGuid;
            s.UseHtml = true;
            s.IsVerified = true;
            s.IpAddress = SiteUtils.GetIP4Address();
            subscriptions.Save(s);

            LetterInfo.UpdateSubscriberCount(s.LetterInfoGuid);

        }

        protected void OnUserRegistered(UserRegisteredEventArgs e)
        {
            foreach (UserRegisteredHandlerProvider handler in UserRegisteredHandlerProviderManager.Providers)
            {
                handler.UserRegisteredHandler(null, e);
            }

           
        }

        protected void OnUserUpdated(ProfileUpdatedEventArgs e)
        {
            foreach (ProfileUpdatedHandlerProvider handler in ProfileUpdatedHandlerProviderManager.Providers)
            {
                handler.ProfileUpdatedHandler(null, e);
            }


        }

        void btnResendConfirmationEmail_Click(object sender, EventArgs e)
        {
            if (userID > -1)
            {
                SiteUser user = new SiteUser(siteSettings, userID);
                if (user.RegisterConfirmGuid != Guid.Empty)
                {
                    Notification.SendRegistrationConfirmationLink(
                    SiteUtils.GetSmtpSettings(),
                    ResourceHelper.GetMessageTemplate("RegisterConfirmEmailMessage.config"),
                    siteSettings.DefaultEmailFromAddress,
                    siteSettings.DefaultFromEmailAlias,
                    user.Email,
                    siteSettings.SiteName,
                    SiteRoot + "/ConfirmRegistration.aspx?ticket=" +
                    user.RegisterConfirmGuid.ToString());

                }
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
            return;

        }

        void btnApprove_Click(object sender, EventArgs e)
        {
            if (this.userID > -1)
            {
                SiteUser user = new SiteUser(siteSettings, this.userID);
                user.ApprovedForLogin = true;
                user.Save();

                //send user notification of approval
                if (WebConfigSettings.NotifyUsersOnAccountApproval)
                {
                    CultureInfo defaultCulture = SiteUtils.GetDefaultUICulture();
                    string signInLink = SiteUtils.GetNavigationSiteRoot() + "/Secure/Login.aspx";
                    SmtpSettings smtpSettings = SiteUtils.GetSmtpSettings();

                    //EmailMessageTask messageTask = new EmailMessageTask(smtpSettings);
                    //messageTask.EmailFrom = siteSettings.DefaultEmailFromAddress;
                    //messageTask.EmailFromAlias = siteSettings.DefaultFromEmailAlias;
                    //messageTask.EmailTo = user.Email;

                    string subjectFormat = ResourceHelper.GetResourceString("Resource", "AccountApprovedSubjectformat", defaultCulture, true);
                    //messageTask.Subject = string.Format(defaultCulture, subjectFormat, siteSettings.SiteName);

                    string textBodyTemplate = ResourceHelper.GetMessageTemplate(defaultCulture, "AccountApprovedMessage.config");
                    //messageTask.TextBody = string.Format(
                    //    defaultCulture,
                    //    textBodyTemplate,
                    //    siteSettings.SiteName,
                    //    signInLink
                    //    );

                    //messageTask.SiteGuid = siteSettings.SiteGuid;
                    //messageTask.QueueTask();
                    //WebTaskManager.StartOrResumeTasks();

                    Email.Send(
                        smtpSettings,
                        siteSettings.DefaultEmailFromAddress,
                        siteSettings.DefaultFromEmailAlias,
                        string.Empty,
                        user.Email,
                        string.Empty,
                        string.Empty,
                        string.Format(defaultCulture, subjectFormat, siteSettings.SiteName),
                        string.Format(defaultCulture, textBodyTemplate, siteSettings.SiteName, signInLink),
                        false,
                        Email.PriorityNormal);

                }
                

            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
            return;
        }

        protected void btnUnlockUser_Click(object sender, EventArgs e)
        {
            if (this.userID > -1)
            {
                SiteUser user = new SiteUser(siteSettings, this.userID);
                user.UnlockAccount();
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
            return;
        }

        protected void btnLockUser_Click(object sender, EventArgs e)
        {
            if (this.userID > -1)
            {
                SiteUser user = new SiteUser(siteSettings, this.userID);
                user.LockoutAccount();
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
            return;
        }

        void btnPurgeUserLocations_Click(object sender, EventArgs e)
        {
            if (this.userID > -1)
            {
                SiteUser user = new SiteUser(siteSettings, this.userID);
                UserLocation.DeleteByUser(user.UserGuid);
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
            return;
        }

        //private void AddRole_Click(Object sender, EventArgs e) 
        //{
        //    if (this.userID > -1)
        //    {
        //        SiteUser user = new SiteUser(siteSettings, this.userID);
        //        int roleID = int.Parse(allRoles.SelectedItem.Value, CultureInfo.InvariantCulture);
        //        Role role = new Role(roleID);
        //        Role.AddUser(roleID, userID, role.RoleGuid, user.UserGuid);
        //        user.RolesChanged = true;
        //        user.Save();
                
        //    }
			
        //    WebUtils.SetupRedirect(this, Request.RawUrl);
        //}


        //private void UserRoles_ItemCommand(object sender, DataListCommandEventArgs e) 
        //{
        //    int roleID = Convert.ToInt32(userRoles.DataKeys[e.Item.ItemIndex]);
        //    SiteUser user = new SiteUser(siteSettings, userID);

        //    Role.RemoveUser(roleID,userID);
        //    userRoles.EditItemIndex = -1;
        //    if (user.UserId > -1)
        //    {
        //        user.RolesChanged = true;
        //        user.Save();
        //    }
            
        //    WebUtils.SetupRedirect(this, Request.RawUrl);
        //    return;

        //}


        void userRoles_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            ImageButton btnRemoveRole = e.Item.FindControl("btnRemoveRole") as ImageButton;
            UIHelper.AddConfirmationDialog(btnRemoveRole, Resource.ManageUsersRemoveRoleWarning);
        }


        void btnConfirmEmail_Click(object sender, EventArgs e)
        {
            if (this.userID > 0)
            {
                SiteUser user = new SiteUser(siteSettings, this.userID);
                SiteUser.ConfirmRegistration(user.RegisterConfirmGuid);
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
            return;
            
        }

        private void PopulateLabels()
        {

            if (userID > -1)
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, Resource.ManageUsersPageTitle);
                if (WebUser.IsInRoles(siteSettings.RolesThatCanManageUsers))
                {
                    liNewsletters.Visible = WebConfigSettings.EnableNewsletter;
                    tabNewsletters.Visible = liNewsletters.Visible;
                    newsLetterPrefs.Visible = liNewsletters.Visible;
                }
                btnUpdate.Text = Resource.ManageUsersUpdateButton;
                btnUpdate.ToolTip = Resource.ManageUsersUpdateButton;
            }
            else
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, Resource.ManageUsersAddUserLabel);
                btnUpdate.Text = Resource.ManageUsersCreateButton;
                btnUpdate.ToolTip = Resource.ManageUsersCreateButton;
            }

            litSecurityTab.Text = "<a href='#tabSecurity'>" + Resource.ManageUsersSecurityTab + "</a>";
            litProfileTab.Text = "<a href='#" + tabProfile.ClientID + "'>" + Resource.ManageUsersProfileTab + "</a>";
            litOrderHistoryTab.Text = "<a href='#" + tabOrderHistory.ClientID + "'>" + Resource.CommerceOrderHistoryTab + "</a>";
            litNewsletterTab.Text = "<a href='#" + tabNewsletters.ClientID + "'>" + Resource.ManageUsersNewslettersTab + "</a>";
            litRolesTab.Text = "<a href='#" + tabRoles.ClientID + "'>" + Resource.ManageUsersRolesTab + "</a>";
            litActivityTab.Text = "<a href='#" + tabActivity.ClientID + "'>" + Resource.ManageUsersActivityTab + "</a>";
            litLocationTab.Text = "<a href='#" + tabLocation.ClientID + "'>" + Resource.ManageUsersLocationTab + "</a>";


            btnApprove.Text = Resource.ApproveUserButton;
            btnApprove.ToolTip = Resource.ApproveUserButton;

            btnUnlockUser.Text = Resource.UserUnlockUserButton;
            btnUnlockUser.ToolTip = Resource.UserUnlockUserButton;
            SiteUtils.SetButtonAccessKey(btnUnlockUser, AccessKeys.UserUnlockUserButtonAccessKey);

            btnLockUser.Text = Resource.UserLockUserButton;
            btnLockUser.ToolTip = Resource.UserLockUserButton;
            SiteUtils.SetButtonAccessKey(btnLockUser, AccessKeys.UserLockUserButtonAccessKey);

            btnConfirmEmail.Text = Resource.ManageUsersConfirmEmailButton;
            btnResendConfirmationEmail.Text = Resource.ResendConfirmationEmailButton;

            
            SiteUtils.SetButtonAccessKey(btnUpdate, AccessKeys.ManageUsersUpdateButtonAccessKey);

            btnDelete.Text = Resource.ManageUsersDeleteButton;
            btnDelete.ToolTip = Resource.ManageUsersDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, AccessKeys.ManageUsersDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(btnDelete, Resource.ManageUsersDeleteUserWarning);

            //addExisting.Text = Resource.ManageUsersAddToRoleButton;
            //addExisting.ToolTip = Resource.ManageUsersAddToRoleButton;
            //SiteUtils.SetButtonAccessKey(addExisting, AccessKeys.ManageUsersAddToRoleButtonAccessKey);

            lnkUnsubscribeFromForums.Text = Resource.ManageUsersUnsubscribeForumsLink;

            //lnkAvatarUpload.Text = Resource.UploadAvatarLink;
            lnkAvatarUpld.Text = Resource.UploadAvatarLink;
            lnkAvatarUpld.ToolTip = Resource.UploadAvatarLink;

            if (!(this.userID > -1))
            {
                this.btnUnlockUser.Enabled = false;
            }

            if (!IsPostBack)
            {
                txtPasswordQuestion.Text = Resource.ManageUsersDefaultSecurityQuestion;
                txtPasswordAnswer.Text = Resource.ManageUsersDefaultSecurityAnswer;
            }

            rfvName.ErrorMessage = Resource.UserProfileNameRequired;
            rfvLoginName.ErrorMessage = Resource.UserProfileLoginNameRequired;

            regexEmail.ErrorMessage = Resource.UserProfileEmailValidation;
            rfvEmail.ErrorMessage = Resource.UserProfileEmailRequired;

            if (siteSettings.UseLdapAuth && !siteSettings.AllowDbFallbackWithLdap)
            {
                divPassword.Visible = false;
                divReqPasswordChange.Visible = false;
            }

            if (allowGravatars)
            {
                //gravatar1.Visible = true;
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
                    divAvatarUrl.Visible = false;
                   // imgAvatar.Visible = false;
                  //  lnkAvatarUpload.Visible = false;
                    lnkAvatarUpld.Visible = false;
                    avatarHelp.Visible = false;
             
                }
                else
                {
                   // imgAvatar.Visible = true;
                    avatarHelp.Visible = true;
                }

            }

            grdUserLocation.Columns[0].HeaderText = Resource.ManageUsersLocationGridIPAddressHeading;
            grdUserLocation.Columns[1].HeaderText = Resource.ManageUsersLocationGridHostnameHeading;
            grdUserLocation.Columns[2].HeaderText = Resource.ManageUsersLocationGridISPHeading;
            grdUserLocation.Columns[3].HeaderText = Resource.ManageUsersLocationGridContinentHeading;
            grdUserLocation.Columns[4].HeaderText = Resource.ManageUsersLocationGridCountryHeading;
            grdUserLocation.Columns[5].HeaderText = Resource.ManageUsersLocationGridRegionHeading;
            grdUserLocation.Columns[6].HeaderText = Resource.ManageUsersLocationGridCityHeading;
            grdUserLocation.Columns[7].HeaderText = Resource.ManageUsersLocationGridTimeZoneHeading;
            grdUserLocation.Columns[8].HeaderText = Resource.ManageUsersLocationGridCaptureCountHeading;
            grdUserLocation.Columns[9].HeaderText = Resource.ManageUsersLocationGridFirstCaptureHeading;
            grdUserLocation.Columns[10].HeaderText = Resource.ManageUsersLocationGridLastCaptureHeading;

            btnPurgeUserLocations.Text = Resource.PurgeUserLocations;
            UIHelper.AddConfirmationDialog(btnPurgeUserLocations, Resource.PurgeUserLocationsWarning);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkMemberList.Text = Resource.MemberListLink;
            lnkMemberList.ToolTip = Resource.MemberListLink;
            lnkMemberList.NavigateUrl = SiteRoot + WebConfigSettings.MemberListUrl;

            lnkManageUser.Text = this.userID > -1 ? Resource.ManageUsersTitleLabel : Resource.ManageUsersAddUserLabel;
            lnkManageUser.ToolTip = this.userID > -1 ? Resource.ManageUsersTitleLabel : Resource.ManageUsersAddUserLabel;
            lnkManageUser.NavigateUrl = SiteRoot + "/Admin/ManageUsers.aspx?userId=" + this.userID;

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
           
        }

        protected void HideExtendedProfileControls()
        {
            tabActivity.Visible = false;
            tabProfile.Visible = false;
            tabRoles.Visible = false;
            liProfile.Visible = false;
            liOrderHistory.Visible = false;
            tabOrderHistory.Visible = false;
            liNewsletters.Visible = false;
            liRoles.Visible = false;
            liActivity.Visible = false;
            liLocation.Visible = false;


            divCreatedDate.Visible = false;
            divUserGuid.Visible = false;
            divTotalPosts.Visible = false;
            divProfileApproved.Visible = false;
            divApprovedForLogin.Visible = false;
            divTrusted.Visible = false;
            divDisplayInMemberList.Visible = false;
            divAvatarUrl.Visible = false;
            UserRolesControl.Visible = false;
            //divRoles.Visible = false;
            //divUserRoles.Visible = false;
            
            btnDelete.Visible = false;
            divLiveMessenger.Visible = false;
            
            divLastActivity.Visible = false;
            divLastLogin.Visible = false;
            divPasswordChanged.Visible = false;
            divLockoutDate.Visible = false;
            divEmailConfirm.Visible = false;
            divFailedPasswordAttempt.Visible = false;
            divFailedPasswordAnswerAttempt.Visible = false;
            divLockout.Visible = false;
            divComment.Visible = false;
            divOpenID.Visible = false;
            divWindowsLiveID.Visible = false;
            tabNewsletters.Visible = false;
            newsLetterPrefs.Visible = false;
            tabLocation.Visible = false;
            divEditorPreference.Visible = false;


        }

        private void LoadSettings()
        {
            
            currentUser = SiteUtils.GetCurrentSiteUser();
            isAdmin = WebUser.IsAdmin;
            pageID = WebUtils.ParseInt32FromQueryString("pageid", -1);
            userID = WebUtils.ParseInt32FromQueryString("userid", true, userID);
            TimeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            userGuid = WebUtils.ParseGuidFromQueryString("u", Guid.Empty);
            UserRolesControl.SiteRoot = SiteRoot;
#if!MONO
            divTimeZone.Visible = true;

#endif

            switch (siteSettings.AvatarSystem)
            {
                case "gravatar":
                    allowGravatars = true;
                    disableAvatars = false;
                    break;

                case "internal":
                    allowGravatars = false;
                    disableAvatars = false;
                    lnkAvatarUpld.NavigateUrl = SiteRoot + "/Dialog/AvatarUploadDialog.aspx?u=" + userID.ToInvariantString();
					lnkAvatarUpld.Attributes.Add("data-size", "fluid-xlarge");
					lnkAvatarUpld.Attributes.Add("data-modal", string.Empty);
					lnkAvatarUpld.Attributes.Add("data-close-text", Resource.CloseDialogButton);
					lnkAvatarUpld.Attributes.Add("data-modal-type", "iframe");
					lnkAvatarUpld.Attributes.Add("data-height", "full");

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


            lnkUnsubscribeFromForums.Visible = WebConfigSettings.ShowForumUnsubscribeLinkInUserManagement;
            // a way to block from purging user locations/ip address history
            // needed for the demo site
            btnPurgeUserLocations.Visible = WebConfigSettings.ShowPurgeUserLocationsInUserManagement;

            if (WebConfigSettings.MaskPasswordsInUserAdmin)
            {
                txtPassword.TextMode = TextBoxMode.Password;
            }

            

            if (WebConfigSettings.UseRelatedSiteMode)
            {
                divTotalPosts.Visible = false;
            }
            if (displaySettings.HidePostCount) { divTotalPosts.Visible = false; }

            commerceConfig = SiteUtils.GetCommerceConfig();
            if (!commerceConfig.IsConfigured)
            {
                liOrderHistory.Visible = false;
                tabOrderHistory.Visible = false;
            }

            string wlAppId = siteSettings.WindowsLiveAppId;
            if (ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"] != null)
            {
                wlAppId = ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"];
                if (wlAppId.Length == 0) { wlAppId = siteSettings.WindowsLiveAppId; }
            }

            if (
                (WebConfigSettings.GloballyDisableMemberUseOfWindowsLiveMessenger)
                || (!siteSettings.AllowWindowsLiveMessengerForMembers)
                || (wlAppId.Length == 0)
                )
            {
                divLiveMessenger.Visible = false;
            }

            divEditorPreference.Visible = siteSettings.AllowUserEditorPreference;

            if (userID == -1) { HideExtendedProfileControls(); }

            divReqPasswordChange.Visible = WebConfigSettings.AllowRequiringPasswordChange;

            // don't let an admin user lock himself out
            if ((userID == currentUser.UserId)&&(!currentUser.IsLockedOut)) { btnLockUser.Enabled = false; }

            AddClassToBody("administration");
            AddClassToBody("manageusers");
        }

        private void SetupAvatarScript()
        {

            //string callback = "<script type=\"text/javascript\">"
            //    + "function GBCallback() { "
            //    + " window.location.reload(true); "
            //    + "}</script>";

            //this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "gbcallback", callback);

            //string init = "<script type=\"text/javascript\">"
            //    + "$('#" + lnkAvatarUpld.ClientID + "').colorbox({width:\"80%\", height:\"80%\", iframe:true, onClosed:GBCallback}); function GBCallback() { window.location.reload(true); } "
            //    + "</script>";

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "cbupinit", init, false);
        }
        

	}


    

}

namespace mojoPortal.Web.UI
{

	public class ProfileDisplaySettings : WebControl
    {
		public string OverrideAvatarLabel { get; set; } = string.Empty;

		public bool HidePostCount { get; set; } = false;

		public string IsDeletedUserNoteFormat { get; set; } = "<span class='txterror'>{0}</span>";

		protected override void Render(HtmlTextWriter writer)
        {

            // nothing to render
        }
    }

}




