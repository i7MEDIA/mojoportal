

using System.Web;
using System.Web.Script.Serialization;
using mojoPortal.Web;
using log4net;
using mojoPortal.Web.ForumUI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.ForumUI.API
{
    public class ModResult
    {
        public string Msg { get; set; }
    }
    /// <summary>
    /// Summary description for ModHandler
    /// </summary>
    public class ModHandler : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ModHandler));

        private string cmd = string.Empty;
        private int pageId = -1;
        private int moduleId = -1;
        private int threadId = -1;
        private int postId = -1;
        //private int forumId = -1;
        private int pageNumber = 1;
        private Forum forum = null;
        private ForumThread thread = null;
        private Module module = null;
        private SiteSettings siteSettings = null;
        private PageSettings currentPage = null;
        private SiteUser postUser = null;
        private ForumConfiguration config = new ForumConfiguration();


        public void ProcessRequest(HttpContext context)
        {
            LoadSettings(context);

            switch (cmd)
            {
                case "sendnotification":
                    SendNotification(context);
                    break;

                case "marksent":
                    MarkAsSent(context);
                    break;
            }
        }

        private void SendNotification(HttpContext context)
        {
            //log.Info("theadId = " + threadId);
            //log.Info("postId = " + postId);
            //log.Info("pageId = " + pageId);
            //log.Info("mid = " + moduleId);

            bool notifyModeratorOnly = false;
            ModResult result = new ModResult();

            if (LoadAndValidateForumObjects())
            {
                thread.NotificationSent = true;
                thread.UpdatePost();

                ForumNotification.NotifySubscribers(
                forum,
                thread,
                module,
                postUser,
                siteSettings,
                config,
                SiteUtils.GetNavigationSiteRoot(),
                pageId,
                pageNumber,
                SiteUtils.GetDefaultCulture(),
                ForumConfiguration.GetSmtpSettings(),
                notifyModeratorOnly
                );

                result.Msg = "success";

            }
            else
            {
                log.Info("Send forum notification request rejected due to invalid params");
               
                result.Msg = "rejected";
            }

            context.Response.ContentType = "application/json";
            JavaScriptSerializer s = new JavaScriptSerializer();
            context.Response.Write(s.Serialize(result));
            
            

        }

        private void MarkAsSent(HttpContext context)
        {
            ModResult result = new ModResult();

            if (LoadAndValidateForumObjects())
            {
                thread.NotificationSent = true;
                thread.UpdatePost();
                
                result.Msg = "success";

                //System.Threading.Thread.Sleep(3000);
                
                //context.Response.Write("{\"Msg\":\"success\"}");
                //context.Response.Write("{\"Msg\":\"rejected\"}");
            }
            else
            {
                //context.Response.Write("{\"Msg\":\"rejected\"}");
                result.Msg = "rejected";
            }

            context.Response.ContentType = "application/json";
            JavaScriptSerializer s = new JavaScriptSerializer();
            context.Response.Write(s.Serialize(result));

        }

        private bool LoadAndValidateForumObjects()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if(siteSettings == null)
            {
                //log.Info("SiteSettings was null");
                return false;
            }
            currentPage = CacheHelper.GetPage(pageId);
            if (
                (currentPage.PageId != pageId)
                || (currentPage.SiteId != siteSettings.SiteId) 
                )
            {
                log.Info("request rejected - pageid did not match");
                return false; 
            }

            thread = new ForumThread(threadId, postId);
            
            if (thread.ModuleId != moduleId) 
            {
                log.Info("thread module id did not match");
                return false; 
            }
            forum = new Forum(thread.ForumId);

            module = GetModule(thread.ModuleId);
            if (module == null) 
            { 
                log.Info("module not found in page modules");
                return false; 
            }
            config = new ForumConfiguration(ModuleSettings.GetModuleSettings(module.ModuleId));
            if(thread.PostUserId > -1)
            {
                postUser = new SiteUser(siteSettings, thread.PostUserId);
            }

            return UserCanModerate();
        }

        private Module GetModule(int moduleId)
        {
            if (currentPage == null) { return null; }
            foreach(Module m in currentPage.Modules)
            {
                if((m.ModuleId == moduleId)&&(m.FeatureGuid == Forum.FeatureGuid))
                {
                    return m;
                }
            }

            return null;

        }

        private bool UserCanModerate()
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

        



        private void LoadSettings(HttpContext context)
        {
            //logAllFileSystemActivity = WebConfigSettings.LogAllFileManagerActivity;

            if (context.Request.Form["cmd"] != null) { cmd = context.Request.Form["cmd"]; }

            if (context.Request.Form["pageId"] != null) 
            {
                int.TryParse(context.Request.Form["pageId"], out pageId); 
            }

            if (context.Request.Form["moduleId"] != null)
            {
                int.TryParse(context.Request.Form["moduleId"], out moduleId);
            }

            if (context.Request.Form["threadId"] != null)
            {
                int.TryParse(context.Request.Form["threadId"], out threadId);
            }

            if (context.Request.Form["postId"] != null)
            {
                int.TryParse(context.Request.Form["postId"], out postId);
            }

            if (context.Request.Form["pageNumber"] != null)
            {
                int.TryParse(context.Request.Form["pageNumber"], out pageNumber);
            }

            
           
            
            
            
           

            //if (context.Request.Form["theadId"] != null) { threadId = context.Request.Form["theadId"]; }

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