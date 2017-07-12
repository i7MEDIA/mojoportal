// Author:		        
// Created:             2009-05-16
// Last Modified:       2009-05-16
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
using System.Web;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.Services
{
    /// <summary>
    /// Handles the redirect from rpxnow.com when using the plugin api
    /// </summary>
    public class RpxPluginResponseHandler : IHttpHandler
    {
        private SiteSettings siteSettings = null;
        private string rpxToken = string.Empty;
        private string siteRoot = "/";

        public void ProcessRequest(HttpContext context)
        {
            
            LoadSettings(context);

            if (!WebUser.IsAdmin)
            {
                context.Response.Redirect(siteRoot);
                return;
            }

            if (rpxToken.Length == 0)
            {
                context.Response.Redirect(siteRoot);
                return;
            }

            ProcessToken(context);

        }

        private void ProcessToken(HttpContext context)
        {
            OpenIdRpxAccountInfo rpxAccount = OpenIdRpxHelper.LookupRpxAccount(rpxToken, true);
            if (rpxAccount == null)
            {
                context.Response.Redirect(siteRoot + "/Admin/SiteSettings.aspx");
                return;
            }

            siteSettings.RpxNowAdminUrl = rpxAccount.AdminUrl;
            siteSettings.RpxNowApiKey = rpxAccount.ApiKey;
            siteSettings.RpxNowApplicationName = rpxAccount.Realm;
            if (siteSettings.SiteGuid.ToString() == rpxAccount.RequestId)
            {
                siteSettings.Save();
                CacheHelper.ClearSiteSettingsCache(siteSettings.SiteId);
            }

            context.Response.Redirect(siteRoot + "/Admin/SiteSettings.aspx?t=oid");

        }

        private void LoadSettings(HttpContext context)
        {
            rpxToken = context.Request.Params.Get("token");

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            

            //if (siteSettings.SiteRoot.Length > 0)
            //{
            //    siteRoot = siteSettings.SiteRoot;
            //}
            //else
            //{
                siteRoot = SiteUtils.GetNavigationSiteRoot();
            //}


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
