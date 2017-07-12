// Author:				    
// Created:			        2013-02-21
// Last Modified:		    2013-04-02
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
using mojoPortal.SearchIndex;
using mojoPortal.Web.ContentUI;
using mojoPortal.Web.Framework;
using System;
using System.Collections;
using System.IO;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

namespace mojoPortal.Web.Services
{
    public partial class HtmlEditService : mojoEditServiceBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HtmlEditService));
        private Hashtable moduleSettings;
        private HtmlContent html;
        protected int pageId = -1;
        protected int moduleId = -1;
        protected Module module = null;
        private bool enableContentVersioning = false;
        private SiteUser currentUser = null;
        protected bool isFullEditor = false;
        private ContentWorkflow workInProgress;
        //private bool userCanEdit = false;
        private bool userCanOnlyEditAsDraft = false;
        private HtmlRepository repository = null;
        private string submittedContent = string.Empty;
        protected HtmlConfiguration config = null;
        private bool editDraft = false;

        protected void Page_Load(object sender, EventArgs e)
        {

            LoadSettings();
            UpdateContent();
        }

        private void UpdateContent()
        {
            if (string.IsNullOrEmpty(submittedContent)) { return; }

            if (!EditIsAllowed()) { return; }

            if ((ViewMode == PageViewMode.WorkInProgress)&&(WebConfigSettings.EnableContentWorkflow) && (CurrentSite.EnableContentWorkflow))
            {
                if (workInProgress != null)
                {
                    if (!editDraft) { return; }

                    workInProgress.ContentText = submittedContent;
                    workInProgress.LastModUserGuid = currentUser.UserGuid;
                    workInProgress.Save();
                }
                else
                {
                    //draft version doesn't exist - create it:
                    ContentWorkflow.CreateDraftVersion(
                        CurrentSite.SiteGuid,
                        submittedContent,
                        string.Empty,
                        -1,
                        Guid.Empty,
                        this.module.ModuleGuid,
                        currentUser.UserGuid);
                }
            }
            else 
            {
                // edit live content
                if (userCanOnlyEditAsDraft) { return; }

                html = repository.Fetch(module.ModuleId);
                if (html == null)
                {
                    html = new HtmlContent();
                    html.ModuleId = module.ModuleId;
                    html.ModuleGuid = module.ModuleGuid;
                }

                html.Body = submittedContent;
                //these will really only be saved if it is a new html instance
                html.CreatedDate = DateTime.UtcNow;
                html.UserGuid = currentUser.UserGuid;
                html.LastModUserGuid = currentUser.UserGuid;

                if (module != null)
                {
                    html.ModuleGuid = module.ModuleGuid;
                }
                html.LastModUtc = DateTime.UtcNow;


                if ((enableContentVersioning) && (html.ItemId > -1))
                {
                    html.CreateHistory(CurrentSite.SiteGuid);
                }

                html.ContentChanged += new ContentChangedEventHandler(html_ContentChanged);

                repository.Save(html);

                CurrentPage.UpdateLastModifiedTime();
                CacheHelper.ClearModuleCache(module.ModuleId);

                SiteUtils.QueueIndexing();

            }

            

        }

        void html_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["HtmlContentIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        private bool EditIsAllowed()
        {
            if (module == null) { return false; } // will be null if user doesn't have permission
            if (currentUser == null) { return false; }


            return true;
        }

        private void LoadSettings()
        {

            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);

            //if (Request.Form.Count > 0)
            //{
            //    submittedContent = Server.UrlDecode(Request.Form.ToString()); // this gets the full content of the post
                
            //}

            if (Request.Form["html"] != null)
            {
                submittedContent = Request.Form["html"];
                //log.Info("html does = " + Request.Form["html"]);
            }


            //using (Stream s = Request.InputStream)
            //{
            //    using (StreamReader sr = new StreamReader(s))
            //    {
            //        string requestBody = sr.ReadToEnd();
            //        requestBody = Server.UrlDecode(requestBody);
            //        log.Info("requestBody was " + requestBody);
            //        JObject jObj = JObject.Parse(requestBody);
            //        submittedContent = (string)jObj["html"];
            //    }
            //}

            module = GetHtmlModule();

            if (module == null) { return; }

            currentUser = SiteUtils.GetCurrentSiteUser();
            repository = new HtmlRepository();
            moduleSettings = ModuleSettings.GetModuleSettings(module.ModuleId);
            config = new HtmlConfiguration(moduleSettings);

            enableContentVersioning = config.EnableContentVersioning;

            if ((CurrentSite.ForceContentVersioning) || (WebConfigSettings.EnforceContentVersioningGlobally))
            {
                enableContentVersioning = true;
            }

            userCanOnlyEditAsDraft = UserCanOnlyEditModuleAsDraft(module.ModuleId, HtmlContent.FeatureGuid);

            if ((WebConfigSettings.EnableContentWorkflow) && (CurrentSite.EnableContentWorkflow))
            {
                workInProgress = ContentWorkflow.GetWorkInProgress(module.ModuleGuid);
            }

            if (workInProgress != null)
            {
                switch (workInProgress.Status)
                {
                    case ContentWorkflowStatus.Draft:

                        //there is a draft version currently available, therefore dont allow the non draft version to be edited:
                       
                        if (ViewMode == PageViewMode.WorkInProgress)
                        {
                            editDraft = true;
                        }

                        break;

                    case ContentWorkflowStatus.ApprovalRejected:
                        //rejected content - allow update as draft only
                        
                        if (ViewMode == PageViewMode.WorkInProgress)
                        {
                            editDraft = true;
                        }
                        break;

                    case ContentWorkflowStatus.AwaitingApproval:
                        //pending approval - dont allow any edited:
                        // 2010-01-18 let editors update the draft if they want to before approving it.
                        editDraft = !userCanOnlyEditAsDraft;
                        break;
                }
            }
            

            //for (int i = 0; i < Request.QueryString.Count; i++)
            //{
            //    log.Info(Request.QueryString.GetKey(i) + " query param = " + Request.QueryString.Get(i));
            //}

            //if (Request.Form["c"] != null)
            //{
            //    submittedContent = Request.Form["c"];
            //    log.Info("c does = " + Request.Form["c"]);
            //}

            // this shows that a large html content post appears as multiple params
            //for (int i = 0; i < Request.Form.Count; i++)
            //{
            //    log.Info(Request.Form.GetKey(i) + " form param " + i.ToInvariantString() + " = " + Request.Form.Get(i));
            //}

        }

        private Module GetHtmlModule()
        {
            if (CurrentSite == null) { return null; }

            Module m = GetModule(moduleId, HtmlContent.FeatureGuid);
            if (m != null) { return m; }

           

            // these extra checks allow for editing an html instance from modulewrapper
            m = new Module(moduleId);
            if (
                (m.FeatureGuid != HtmlContent.FeatureGuid)
                || (m.SiteId != CurrentSite.SiteId)
                || (m.ModuleId == -1)
                || ((!WebUser.IsInRoles(m.AuthorizedEditRoles)) && (!UserIsAdmin) && (!UserIsContentAdmin) && (!UserIsSiteEditor))
                )
            { m = null; }


            return m;
        }



    }
}