// Author:					
// Created:				    2014-07-11
// Last Modified:			2014-07-12
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
//

using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.ForumUI;
using mojoPortal.Web.Framework;
using System.Web.Http;

namespace mojoPortal.ForumUI.API
{

    // the web api default binders and formatters
    // will automatically create an instance of our ModerationRequest class
    // and pass it into the Post method below so long as
    // there are submitted form params that match 
    // the requied property names and data types
    //http://www.asp.net/web-api/overview/formats-and-model-binding/parameter-binding-in-aspnet-web-api

    public class ModerationRequest
    {
        [HttpBindRequired]
        public int PageId { get; set; }
        [HttpBindRequired]
        public int ModuleId { get; set; }
        [HttpBindRequired]
        public int PageNumber { get; set; }
        [HttpBindRequired]
        public int ThreadId { get; set; }
        [HttpBindRequired]
        public int PostId { get; set; }
        [HttpBindRequired]
        public string Cmd { get; set; }
    }

    public class ModerationResult
    {
        public string Msg { get; set; }
    }

    public class ForumModController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ForumModController));

        private Forum forum = null;
        private ForumThread thread = null;
        private Module module = null;
        private SiteSettings siteSettings = null;
        private PageSettings currentPage = null;
        private SiteUser postUser = null;
        private ForumConfiguration config = new ForumConfiguration();



        // POST /api/forummod
        public ModerationResult Post(ModerationRequest modReq)
        {
            //log.Info("cmd = " + modReq.Cmd);
            //log.Info("pageId = " + modReq.PageId.ToInvariantString());
            //log.Info("moduleId = " + modReq.ModuleId.ToInvariantString());
            //log.Info("pageNumber = " + modReq.PageNumber.ToInvariantString());
            //log.Info("threadId = " + modReq.ThreadId.ToInvariantString());
            //log.Info("postId = " + modReq.PostId.ToInvariantString());


            ModerationResult result = new ModerationResult();
            result.Msg = "rejected";
            if (IsAllowed(modReq))
            {
                switch(modReq.Cmd)
                {
                    case "sendnotification":

                        

                        bool notifyModeratorOnly = false;

                        ForumNotification.NotifySubscribers(
                            forum,
                            thread,
                            module,
                            postUser,
                            siteSettings,
                            config,
                            SiteUtils.GetNavigationSiteRoot(),
                            modReq.PageId,
                            modReq.PageNumber,
                            SiteUtils.GetDefaultCulture(),
                            ForumConfiguration.GetSmtpSettings(),
                            notifyModeratorOnly
                            );

                        thread.NotificationSent = true;
                        thread.UpdatePost();

                        result.Msg = "success";
                        
                        break;

                    case "marksent":

                        thread.NotificationSent = true;
                        thread.UpdatePost();

                        //System.Threading.Thread.Sleep(7000);
                
                        result.Msg = "success";
                        
                        break;


                }

            }

            return result;

        }

        private bool IsAllowed(ModerationRequest modReq)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null)
            {
                //log.Info("SiteSettings was null");
                return false;
            }
            currentPage = CacheHelper.GetPage(modReq.PageId);
            if (
                (currentPage.PageId != modReq.PageId)
                || (currentPage.SiteId != siteSettings.SiteId)
                )
            {
                log.Info("request rejected - pageid did not match");
                return false;
            }

            thread = new ForumThread(modReq.ThreadId, modReq.PostId);

            if (thread.ModuleId != modReq.ModuleId)
            {
                log.Info("thread module id did not match");
                return false;
            }
            forum = new Forum(thread.ForumId);

            module = GetModule(currentPage, thread.ModuleId);
            if (module == null)
            {
                log.Info("module not found in page modules");
                return false;
            }

            config = new ForumConfiguration(ModuleSettings.GetModuleSettings(module.ModuleId));
            if (thread.PostUserId > -1)
            {
                postUser = new SiteUser(siteSettings, thread.PostUserId);
            }

            return UserCanModerate(currentPage, module, forum);
        }

        private bool UserCanModerate(PageSettings currentPage, Module module, Forum forum)
        {
            if (currentPage == null) { return false; }
            if (module == null) { return false; }
            if (forum == null) { return false; }

            if (WebUser.IsAdminOrContentAdmin) { return true; }
            if (WebUser.IsInRoles(currentPage.EditRoles)) { return true; }
            if (WebUser.IsInRoles(module.AuthorizedEditRoles)) { return true; }
            if (WebUser.IsInRoles(forum.RolesThatCanModerate)) { return true; }
            if (SiteUtils.UserIsSiteEditor()) { return true; }

            log.Info("user can't moderate");
            return false;
        }

        private Module GetModule(PageSettings currentPage, int moduleId)
        {
            if (currentPage == null) { return null; }
            foreach (Module m in currentPage.Modules)
            {
                if ((m.ModuleId == moduleId) && (m.FeatureGuid == Forum.FeatureGuid))
                {
                    return m;
                }
            }

            return null;

        }


        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

        //    if (siteSettings != null)
        //    {
        //        return new string[] { "forummod says hello site", siteSettings.SiteId.ToInvariantString() };
        //    }

        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}