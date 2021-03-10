//previously this functionality was in Global.asax.cs
// moved here 2009-05-30
// Last Modified 2011-11-17

using System;
using System.Data.Common;
using System.IO;
using System.Web;
using System.Web.Security;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.UserRegisteredHandlers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Security;

namespace mojoPortal.Web
{
    public class AuthHandlerHttpModule : IHttpModule
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AuthHandlerHttpModule));
        private static bool debugLog = log.IsDebugEnabled;

        public void Init(HttpApplication application)
        {
            
            application.AuthenticateRequest += new EventHandler(application_AuthenticateRequest);
        }

        void application_AuthenticateRequest(object sender, EventArgs e)
        {
            //if (debugLog) log.Debug("AuthHandlerHttpModule Application_AuthenticateRequest");

            if (sender == null) return;

            HttpApplication app = (HttpApplication)sender;
            if (app.Request == null) { return; }
            if (!app.Request.IsAuthenticated) { return; }

            if(WebUtils.IsRequestForStaticFile(app.Request.Path)) { return; }
            if (app.Request.Path.ContainsCaseInsensitive(".ashx")) { return; }
            if (app.Request.Path.ContainsCaseInsensitive(".axd")) { return; }
            if (app.Request.Path.ContainsCaseInsensitive("setup/default.aspx")) { return; }

            
            //if (debugLog) log.Debug("IsAuthenticated == true");
            SiteSettings siteSettings;
            try
            {
                siteSettings = CacheHelper.GetCurrentSiteSettings();
            }
            catch (System.Data.Common.DbException ex)
            {
                // can happen during upgrades
                log.Error(ex);
                return;
            }
			catch (InvalidOperationException ex)
			{
				log.Error(ex);
				return;
			}
            catch (Exception ex)
            {
                // hate to trap System.Exception but SqlCeException doe snot inherit from DbException as it should
                if (DatabaseHelper.DBPlatform() != "SqlCe") { throw; }
                log.Error(ex);
                return;
            }
            bool useFolderForSiteDetection = WebConfigSettings.UseFolderBasedMultiTenants;

            // Added by Haluk Eryuksel - 2006-01-23
            // support for Windows authentication
            if (
                (app.User.Identity.AuthenticationType == "NTLM")
                || (app.User.Identity.AuthenticationType == "Negotiate")
                // || ( Context.User.Identity.AuthenticationType == "Windows" )
                )
            {
                //Added by Benedict Chan - 2008-08-05
                //Added Cookie here so that we don't have to check the users in every page, also to authenticate under NTLM with "useFolderForSiteDetection == true"
                string cookieName = "siteguid" + siteSettings.SiteGuid;
                if (!CookieHelper.CookieExists(cookieName))
                {
                    bool existsInDB;
                    existsInDB = SiteUser.LoginExistsInDB(siteSettings.SiteId, app.Context.User.Identity.Name);

                    if (!existsInDB)
                    {
                        SiteUser u = new SiteUser(siteSettings);
                        u.Name = app.Context.User.Identity.Name;
                        u.LoginName = app.Context.User.Identity.Name;
                        u.Email = GuessEmailAddress(u.Name);
                        u.Password = SiteUser.CreateRandomPassword(7, WebConfigSettings.PasswordGeneratorChars);

                        mojoMembershipProvider m = Membership.Provider as mojoMembershipProvider;
                        if (m != null)
                        {
                            u.Password = m.EncodePassword(siteSettings, u, u.Password);
                        }

                        u.Save();
                        NewsletterHelper.ClaimExistingSubscriptions(u);

                        UserRegisteredEventArgs args = new UserRegisteredEventArgs(u);
                        OnUserRegistered(args);
                           
                    }

                    SiteUser siteUser = new SiteUser(siteSettings, app.Context.User.Identity.Name);
                    CookieHelper.SetCookie(cookieName, siteUser.UserGuid.ToString(), true);

                    //Copied logic from SiteLogin.cs  Since we will skip them if we use NTLM
                    if (siteUser.UserId > -1 && siteSettings.AllowUserSkins && siteUser.Skin.Length > 0)
                    {
                        SiteUtils.SetSkinCookie(siteUser);
                    }

                    // track user ip address
                    try
                    {
                        UserLocation userLocation = new UserLocation(siteUser.UserGuid, SiteUtils.GetIP4Address());
                        userLocation.SiteGuid = siteSettings.SiteGuid;
                        userLocation.Hostname = app.Request.UserHostName;
                        userLocation.Save();
                        log.Info("Set UserLocation : " + app.Request.UserHostName + ":" + SiteUtils.GetIP4Address());
                    }
                    catch (Exception ex)
                    {
                        log.Error(SiteUtils.GetIP4Address(), ex);
                    }
                }

                //End-Added by Benedict Chan

            }
            // End-Added by Haluk Eryuksel


            if ((useFolderForSiteDetection) && (!WebConfigSettings.UseRelatedSiteMode))
            {
                // replace GenericPrincipal with custom one
                //string roles = string.Empty;
                if (!(app.Context.User is mojoIdentity))
                {
                    app.Context.User = new mojoPrincipal(app.Context.User);
                }
            }

           


        }

        private string GuessEmailAddress(string userName)
        {
            if (WebConfigSettings.GuessEmailForWindowsAuth)
            {
                if (userName.Contains("\\"))
                {
                    string domain = userName.Substring(0, userName.IndexOf("\\"));
                    string user = userName.Replace(domain + "\\", string.Empty);
                    return user + "@" + domain + WebConfigSettings.WindowsAuthDomainExtension;
                }
            }

            return string.Empty;
        }


        private void OnUserRegistered(UserRegisteredEventArgs e)
        {
            foreach (UserRegisteredHandlerProvider handler in UserRegisteredHandlerProviderManager.Providers)
            {
                handler.UserRegisteredHandler(null, e);
            }
        }
       

        public void Dispose() { }
    }
}
