/// Author:				        
/// Created:			        2004-10-03
/// Last Modified:		        2013-09-12
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Configuration;
using mojoPortal.Web.Controls;
using mojoPortal.Web.Framework;
using Resources;
using ConsentToken = mojoPortal.Web.WindowsLiveLogin.ConsentToken;

namespace mojoPortal.Web.UI.Pages
{

    public partial class ProfileView : NonCmsBasePage
	{
        private Guid userGuid = Guid.Empty;
        private int userID = -1;
        private Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;
        //Gravatar public enum RatingType { G, PG, R, X }
        private Avatar.RatingType MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();
        private bool allowGravatars = false;
        private bool disableAvatars = true;
        private string avatarPath = string.Empty;
        private SiteUser siteUser = null;
        private bool allowView = false;
        protected string allowedImageUrlRegexPattern = SecurityHelper.RegexRelativeImageUrlPatern;
        

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            
            SuppressMenuSelection();
            SuppressPageMenu();
        }
        #endregion


		private void Page_Load(object sender, EventArgs e)
		{
            LoadSettings();

            if (!allowView)
            {
                if (!Request.IsAuthenticated)
                {
                    SiteUtils.RedirectToLoginPage(this);
                    return;
                }
                else
                {
                    WebUtils.SetupRedirect(this, SiteRoot);
                    return;
                }
            }

            if (SiteUtils.SslIsAvailable() && WebConfigSettings.ForceSslOnProfileView) { SiteUtils.ForceSsl(); }

            

			PopulateControls();
		}

		private void PopulateControls()
		{
           
            if (siteUser != null)
            {
                this.lblCreatedDate.Text = siteUser.DateCreated.AddHours(timeOffset).ToString();
                this.lblTotalPosts.Text = siteUser.TotalPosts.ToString(CultureInfo.InvariantCulture);
                
                this.lblUserName.Text = Server.HtmlEncode(siteUser.Name);

                Title = SiteUtils.FormatPageTitle(siteSettings, string.Format(CultureInfo.InvariantCulture,
                    Resource.PageTitleFormatProfilePage, Server.HtmlEncode(siteUser.Name)));

                MetaDescription = string.Format(CultureInfo.InvariantCulture,
                    Resource.ProfileViewMetaFormat, Server.HtmlEncode(siteUser.Name));

                userAvatar.UseGravatar = allowGravatars;
                userAvatar.Email = siteUser.Email;
                userAvatar.UserName = siteUser.Name;
                userAvatar.UserId = siteUser.UserId;
                userAvatar.AvatarFile = siteUser.AvatarUrl;
                userAvatar.MaxAllowedRating = MaxAllowedGravatarRating;
                userAvatar.Disable = disableAvatars;
                userAvatar.SiteId = siteSettings.SiteId;
                userAvatar.UseLink = false;

                if (disableAvatars) {  divAvatar.Visible = false; }
                

                //if (allowGravatars)
                //{
                //    imgAvatar.Visible = false;
                //    gravatar1.Visible = true;
                //    gravatar1.Email = siteUser.Email;
                //    //gravatar1.MaxAllowedRating = MaxAllowedGravatarRating;
                //}
                //else
                //{
                //    gravatar1.Visible = false;
                //    if (disableAvatars)
                //    {
                //        divAvatar.Visible = false;
                //    }
                //    else
                //    {
                //        if (siteUser.AvatarUrl.Length > 0)
                //        {
                //            imgAvatar.Src = avatarPath + siteUser.AvatarUrl;
                //        }
                //        else
                //        {
                //            imgAvatar.Src = Page.ResolveUrl(WebConfigSettings.DefaultBlankAvatarPath);
                //        }
                //    }
                //}

                lnkUserPosts.UserId = siteUser.UserId;
                lnkUserPosts.TotalPosts = siteUser.TotalPosts;

                if (siteUser.TimeZoneId.Length > 0)
                {
                    TimeZoneInfo userTz = SiteUtils.GetTimeZone(siteUser.TimeZoneId);
                    if (userTz != null) 
                    {
                        pnlTimeZone.Visible = true;

                        if (userTz.IsDaylightSavingTime(DateTime.UtcNow))
                            lblTimeZone.Text = userTz.DaylightNameWithOffset();
                        else
                            lblTimeZone.Text = userTz.DisplayName; 
                    }

                }

                if (WebConfigSettings.UseRelatedSiteMode)
                {
                    // this can't be used in related site mode
                    // because we can't assume forum posts were in this site.
                    divForumPosts.Visible = false;
                }

                if (Request.IsAuthenticated)
                {
                    ShowAuthenticatedProperties(siteUser);
                }
                else
                {
                    ShowAnonymousProperties(siteUser);
                }


                PopulateMessenger();
            }
            else
            {
                this.lblUserName.Text = "User not found";
                divAvatar.Visible = false;
            }
		
            

		}

        private void LoadSettings()
        {
            //avatarPath = Page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/useravatars/");

            UntrustedContent2.TrustedImageUrlPattern = allowedImageUrlRegexPattern;

            allowView = WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList);
            userID = WebUtils.ParseInt32FromQueryString("userid", true, userID);
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            userGuid = WebUtils.ParseGuidFromQueryString("u", Guid.Empty);

            if (userID > -1)
            {
                siteUser = new SiteUser(siteSettings, userID);
                if (siteUser.UserGuid == Guid.Empty) { siteUser = null; }
            }
            else if(userGuid != Guid.Empty)
            {
                siteUser = new SiteUser(siteSettings, userGuid);
                if (siteUser.UserGuid == Guid.Empty) { siteUser = null; }
            }

            switch (siteSettings.AvatarSystem)
            {
                case "gravatar":
                    allowGravatars = true;
                    disableAvatars = false;
                    break;

                case "internal":
                    allowGravatars = false;
                    disableAvatars = false;
                    
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

            AddClassToBody("profileview");

        }

        private void PopulateMessenger()
        {
            if (WebConfigSettings.GloballyDisableMemberUseOfWindowsLiveMessenger) { return; }
            if (!siteSettings.AllowWindowsLiveMessengerForMembers) { return; }
            if (siteUser == null) { return; }
            if (!siteUser.EnableLiveMessengerOnProfile) { return; }
            if (siteUser.LiveMessengerId.Length == 0) { return; }

            divLiveMessenger.Visible = true;
            chat1.Invitee = siteUser.LiveMessengerId;
            //chat1.InviteeDisplayName = siteUser.Name;

            if (WebConfigSettings.TestLiveMessengerDelegation)
            {
                WindowsLiveLogin wl = WindowsLiveHelper.GetWindowsLiveLogin();
                WindowsLiveMessenger m = new WindowsLiveMessenger(wl);
                ConsentToken token = m.DecodeToken(siteUser.LiveMessengerDelegationToken);
                ConsentToken refreshedToken = m.RefreshConsent(token);
                if (refreshedToken != null)
                {


                    chat1.DelegationToken = refreshedToken.DelegationToken;
                    string signedParams = WindowsLiveMessenger.SignParameters(
                        refreshedToken.SessionKey,
                        siteUser.Name,
                        string.Empty,
                        string.Empty);
                    chat1.SignedParams = signedParams;

                }
                else
                {
                    //chat1.DelegationToken = siteUser.LiveMessengerDelegationToken;
                    chat1.DelegationToken = token.DelegationToken;
                    string signedParams = WindowsLiveMessenger.SignParameters(
                        token.SessionKey,
                        siteUser.Name,
                        string.Empty,
                        string.Empty);

                    chat1.SignedParams = signedParams;
                }


            }
            

        }

        private void ShowAuthenticatedProperties(SiteUser siteUser)
        {
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
                         (propertyDefinition.VisibleToAuthenticated)
                            && (
                                    (propertyDefinition.OnlyAvailableForRoles.Length == 0)
                                    || (siteUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles))
                                )
                                &&(
                                    (propertyDefinition.OnlyVisibleForRoles.Length == 0)
                                        || (WebUser.IsInRoles(propertyDefinition.OnlyVisibleForRoles))
                                  )
                            
                        )
                    {
                        object propValue = siteUser.GetProperty(propertyDefinition.Name, propertyDefinition.SerializeAs, propertyDefinition.LazyLoad);
                        if (propValue != null)
                        {
                            mojoProfilePropertyDefinition.SetupReadOnlyPropertyControl(
                                pnlProfileProperties,
                                propertyDefinition,
                                propValue.ToString(),
                                timeOffset,
                                timeZone);
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

        private void ShowAnonymousProperties(SiteUser siteUser)
        {
            bool wouldSeeMoreIfAuthenticated = false;

            mojoProfileConfiguration profileConfig = mojoProfileConfiguration.GetConfig();
            if (profileConfig != null)
            {
                foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
                {
                    if (
                        (propertyDefinition.VisibleToAnonymous)
                        && (propertyDefinition.OnlyVisibleForRoles.Length == 0)
                        &&(
                        (propertyDefinition.OnlyAvailableForRoles.Length == 0)
                            || (siteUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles))
                            )
                        )
                    {
                        object propValue = siteUser.GetProperty(propertyDefinition.Name, propertyDefinition.SerializeAs, propertyDefinition.LazyLoad);
                        if (propValue != null)
                        {
                            mojoProfilePropertyDefinition.SetupReadOnlyPropertyControl(
                                pnlProfileProperties,
                                propertyDefinition,
                                propValue.ToString(),
                                timeOffset,
                                timeZone);
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
                    else
                    {
                        if (
                            (propertyDefinition.VisibleToAuthenticated)
                            && (propertyDefinition.OnlyVisibleForRoles.Length == 0)
                            &&(
                            (propertyDefinition.OnlyAvailableForRoles.Length == 0)
                                || (siteUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles))
                                )
                            )
                        {
                            wouldSeeMoreIfAuthenticated = true;
                        }

                    }

                }
            }

            if (wouldSeeMoreIfAuthenticated)
            {
                lblMessage.Text = ProfileResource.WouldSeeMoreIfAuthenticatedMessage;
            }

        }

		
	}
}
