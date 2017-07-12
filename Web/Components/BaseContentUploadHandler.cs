//  Author:                     
//  Created:                    2013-04-01
//	Last Modified:              2014-04-22
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web.Framework;
using System;
using System.Web;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// provides some common functionality for upload handlers to use
    /// </summary>
    public class BaseContentUploadHandler 
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(BaseContentUploadHandler));

        protected HttpRequest Request;
        protected HttpResponse Response;
        protected HttpServerUtility Server;
        protected IFileSystem FileSystem = null;
        protected int PageId = -1;
        protected int ModuleId = -1;
        protected SiteSettings CurrentSite = null;
        protected PageSettings CurrentPage = null;
        protected SiteUser CurrentUser = null;

        private bool userIsAdmin = false;

        public bool UserIsAdmin
        {
            get { return userIsAdmin; }
        }

        private bool userIsContentAdmin = false;

        public bool UserIsContentAdmin
        {
            get { return userIsContentAdmin; }
        }

        private bool userIsSiteEditor = false;

        public bool UserIsSiteEditor
        {
            get { return userIsSiteEditor; }
        }

        public void Initialize(HttpContext context)
        {
            Request = context.Request;
            Response = context.Response;
            Server = context.Server;

            PageId = WebUtils.ParseInt32FromQueryString("pageid", PageId);
            ModuleId = WebUtils.ParseInt32FromQueryString("mid", ModuleId);
            CurrentSite = CacheHelper.GetCurrentSiteSettings();
            CurrentUser = SiteUtils.GetCurrentSiteUser();

            userIsAdmin = WebUser.IsAdmin;
            if (!userIsAdmin)
            {
                userIsContentAdmin = WebUser.IsContentAdmin;
                if (!userIsContentAdmin)
                {
                    userIsSiteEditor = SiteUtils.UserIsSiteEditor();
                }
            }

            FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
            if (p == null) { return; }

            FileSystem = p.GetFileSystem();

            

        }

        public bool UserCanEditModule(int moduleId, Guid featureGuid)
        {
            if (!Request.IsAuthenticated) { return false; }

            if (WebConfigSettings.FileServiceRejectFishyPosts)
            {
                if (SiteUtils.IsFishyPost(Request))
                {
                    return false;
                }
            }

            if (CurrentPage == null) { CurrentPage = CacheHelper.GetCurrentPage(); }
            if (CurrentPage == null) { return false; }

            if ((!userIsAdmin) && (CurrentPage.EditRoles == "Admins;")) { return false; }

            bool moduleFoundOnPage = false;
            string moduleEditRoles = string.Empty;

            foreach (Module m in CurrentPage.Modules)
            {
                if (
                    (m.ModuleId == moduleId)
                    && ((featureGuid == Guid.Empty) || (m.FeatureGuid == featureGuid))
                    )
                {
                    moduleFoundOnPage = true;
                    moduleEditRoles = m.AuthorizedEditRoles;
                }
            }

            if (!moduleFoundOnPage) return false;

            if ((!userIsAdmin) && (moduleEditRoles == "Admins;")) { return false; }

            if (userIsAdmin || userIsContentAdmin || userIsSiteEditor) return true;

            if (WebUser.IsInRoles(moduleEditRoles)) return true;

            if (WebUser.IsInRoles(CurrentPage.EditRoles)) return true;

            return false;

        }


        public Module GetModule(int moduleId, Guid featureGuid)
        {
            if (CurrentPage == null) { CurrentPage = CacheHelper.GetCurrentPage(); }

            if (CurrentPage == null) { return null; }

            foreach (Module m in CurrentPage.Modules)
            {
                if (
                    (m.ModuleId == moduleId)
                    && ((featureGuid == Guid.Empty) || (m.FeatureGuid == featureGuid))
                    )
                { return m; }
            }

            return null;
        }

    }

    public class UploadFilesResult
    {
        public string Thumbnail_url { get; set; }
        public string FileUrl { get; set; }
        public string FullSizeUrl { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
        public string ReturnValue { get; set; }
        public string ErrorMessage { get; set; }
    }
}