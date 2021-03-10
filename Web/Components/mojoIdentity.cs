// Author:					
// Created:				    2007-04-24
// Last Modified:		    2011-11-11
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Web.Security;
using System.Security.Principal;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.Security
{
    /// <summary>
    ///
    /// </summary>
    [Serializable()]
    public class mojoIdentity : MarshalByRefObject, IIdentity
    {
        public mojoIdentity()
        {

        }

        //public mojoIdentity(string username, string password)
        //{
        //    if (string.IsNullOrEmpty(username))
        //        throw new ArgumentNullException("username");

        //    if (string.IsNullOrEmpty(password))
        //        throw new ArgumentNullException("password");

        //    if (!Membership.ValidateUser(username, password)) { return; }

        //    isAuthenticated = true;
        //    name = username;
        //}

        public mojoIdentity(IIdentity innerIdentity)
        {
            this.innerIdentity = innerIdentity;
            name = this.innerIdentity.Name;
            isAuthenticated = this.innerIdentity.IsAuthenticated;
        }

        
        private IIdentity innerIdentity;
        private string name = string.Empty;
        //private string authenticationType = "Forms";
        private bool isAuthenticated = false;
        private bool alreadyChecked = false;

        public string Name
        {
            get 
            {
                return name;
            }
        }

        public string AuthenticationType
        {
            //get { return authenticationType; }
            get { return innerIdentity.AuthenticationType; }
        }

        public bool IsAuthenticated
        {
            get 
            {
                if (!alreadyChecked)
                {
                    bool useFolderForSiteDetection = WebConfigSettings.UseFolderBasedMultiTenants;
                    
                    if (
                        (isAuthenticated) 
                        &&(!WebConfigSettings.UseRelatedSiteMode)
                        && (useFolderForSiteDetection)
                        )
                    {
                        //string virtualFolder = VirtualFolderEvaluator.VirtualFolderName();

                        isAuthenticated = false;
                        SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

                        if (siteSettings == null) return isAuthenticated;

                        string cookieName = "siteguid" + siteSettings.SiteGuid.ToString();

                        if (!CookieHelper.CookieExists(cookieName)) return isAuthenticated;
                        
                        string cookieValue = CookieHelper.GetCookieValue(cookieName);
                        bool bypassAuthCheck = true;
                        SiteUser siteUser = null;
                        try
                        {
                            // errors can happen here during upgrades if a new column was added for mp_Users
                            siteUser = SiteUtils.GetCurrentSiteUser(bypassAuthCheck);
                        }
                        catch 
                        {
                            return false;
                        }
                        if (siteUser == null) return isAuthenticated;

                        if (siteUser.UserGuid.ToString() == cookieValue) isAuthenticated = true;

                        //if ((virtualFolder.Length == 0) && (cookieValue == "root"))
                        //{
                        //    isAuthenticated = true;
                        //}
                        //if (virtualFolder == cookieValue)
                        //{
                        //    isAuthenticated = true;
                        //}

                        alreadyChecked = true;

                    }
                }
                return isAuthenticated; 
            }
        }

    }
}
