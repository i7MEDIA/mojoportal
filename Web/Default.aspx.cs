/// Author:				
/// Created:			2004-08-22
/// Last Modified:	    2013-04-07
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using mojoPortal.Core.Configuration;
using mojoPortal.Core.Helpers;
using mojoPortal.Core.API;
namespace mojoPortal.Web.UI
{

    public partial class CmsPage : mojoBasePage
	{
		private static readonly ILog log  = LogManager.GetLogger(typeof(CmsPage));

		private HyperLink lnkEditPageSettings = new HyperLink();
        private HyperLink lnkEditPageContent = new HyperLink();
        
        //private bool isAdmin = false;
        //private bool isContentAdmin = false;
        //private bool isSiteEditor = false;
        //private bool allowPageOverride = true;
        private int countOfIWorkflow = 0;
        //private bool userCanEditPage = false;

        private bool forceShowWorkflow = false;

        override protected void OnPreInit(EventArgs e)
        {

            //SetMasterInBasePage = false;

            AllowSkinOverride = true;
            base.OnPreInit(e);

            if(WebConfigSettings.SetMaintainScrollPositionOnPostBackTrueOnCmsPages)
            {
                MaintainScrollPositionOnPostBack = true;
            }

            //try
            //{
            //    SetupMasterPage();

            //}
            //catch (HttpException ex)
            //{
            //    log.Error(SiteUtils.GetIP4Address() + " - Error setting master page. Will try settingto default skin", ex);
            //    SetupFailsafeMasterPage();
            //}
            
            
            //StyleSheetCombiner styleCombiner = (StyleSheetCombiner)Master.FindControl("StyleSheetCombiner");
            //if (styleCombiner != null) { styleCombiner.AllowPageOverride = allowPageOverride; }

            
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        void Page_Load(object sender, EventArgs e)
        {
            
            LoadPage();

            
        }


        private void LoadPage()
        {
            EnsurePageAndSite();
            if (CurrentPage == null) return;
            LoadSettings();
            EnforceSecuritySettings();
            bool redirected = RedirectIfNeeded();
            if (redirected) { return; }
            
            if (CurrentPage.PageId == -1)
            {
                SetupAdminLinks();
                return;
            }

            if ((CurrentPage.ShowChildPageMenu)
                    || (CurrentPage.ShowBreadcrumbs)
                    || (CurrentPage.ShowChildPageBreadcrumbs)
                    )
            {
                // this is needed to override some hide logic in
                // layout.Master.cs
                this.MPContent.Visible = true;
                this.MPContent.Parent.Visible = true;
            }

            if (CurrentPage.BodyCssClass.Length > 0)
            {
                AddClassToBody(CurrentPage.BodyCssClass);
            }

            // solves background problems with some skin in WLW
            if ((StyleCombiner != null) && (StyleCombiner.AddBodyClassForLiveWriter))
            {
                if (BrowserHelper.IsWindowsLiveWriter())
                {
                    AddClassToBody("wysiwygeditor");
                }
            }
            
            if (CurrentPage.PageTitle.Length > 0)
            {
                if (WebConfigSettings.FormatOverridePageTitle)
                {
                    Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageTitle);
                }
                else
                {
                    Title = CurrentPage.PageTitle;
                }
            }
            else
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageName);
            }

            if (CurrentPage.PageMetaKeyWords.Length > 0)
            {
                MetaKeywordCsv = CurrentPage.PageMetaKeyWords;
            }
            

            if (CurrentPage.PageMetaDescription.Length > 0)
            {
                MetaDescription = CurrentPage.PageMetaDescription;
            }
            

            if (CurrentPage.CompiledMeta.Length > 0)
            {
                AdditionalMetaMarkup = CurrentPage.CompiledMeta;
            }

            if (WebConfigSettings.AutomaticallyAddCanonicalUrlToCmsPages)
            {
                if ((Page.Header != null) && (CurrentPage.UseUrl) && (CurrentPage.Url.Length > 0))
                {
                    string urlToUse;
                    if (CurrentPage.CanonicalOverride.Length > 0)
                    {
                        urlToUse = CurrentPage.CanonicalOverride;
                    }
                    else
                    {
                        if (CurrentPage.Url.StartsWith("http"))
                        {
                            urlToUse = CurrentPage.Url;
                        }
                        else
                        {
                            if (CurrentPage.UrlHasBeenAdjustedForFolderSites)
                            {
                                urlToUse = WebUtils.GetSiteRoot() + CurrentPage.Url.Replace("~/", "/");
                            }
                            else
                            {
                                urlToUse = SiteRoot + CurrentPage.Url.Replace("~/", "/");
                            }

                            if (WebConfigSettings.ForceHttpForCanonicalUrlsThatDontRequireSsl)
							{
								if (WebHelper.IsSecureRequest() && (!CurrentPage.RequireSsl) && (!siteSettings.UseSslOnAllPages))
                                {
                                    urlToUse = urlToUse.Replace("https:", "http:");
                                }
                            }
                        }
                    }

                    Literal link = new Literal();
                    link.ID = "pageurl";
                    link.Text = "\n<link rel='canonical' href='"
                        + urlToUse
                        + "' />";

                    Page.Header.Controls.Add(link);

                }
            }

           // if (CurrentPage.Modules.Count == 0) { return; }

            bool isAdmin = WebUser.IsAdmin;
            bool isContentAdmin = false;
            bool isSiteEditor = false;
            if (!isAdmin)
            {
                isContentAdmin = WebUser.IsContentAdmin;
                isSiteEditor = SiteUtils.UserIsSiteEditor();
            }

            if (CurrentPage.Modules.Count == 0)
            {
                if (!CurrentPage.ShowChildPageMenu) // we'll consider Child Page Menu as a feature for our purpose here
                {
                    if (
                        (WebConfigSettings.UsePageContentWizard)
                        &&
                        (isAdmin || isContentAdmin || isSiteEditor || WebUser.IsInRoles(CurrentPage.EditRoles))
                        )
                    {
                        // this is to make it a little more intuitive
                        // sometimes users create pages but fail to see the link in pagesettings to 
                        // add features so if they visit the empty page give them an easy opportunity
                        // to add a feature
                        Control wiz = Page.LoadControl("~/Admin/Controls/PageContentWizard.ascx");
                        MPContent.Controls.Add(wiz);
                    }
                }

                return;
            }

            foreach (Module module in CurrentPage.Modules)
            {
                if (!ModuleIsVisible(module)) { continue; }

                if(
                    (!WebUser.IsInRoles(module.ViewRoles))
                    && (!isContentAdmin)
                    && (!isSiteEditor)
                    )
                { 
                    continue; 
                }

                if ((module.ViewRoles == "Admins;") && (!isAdmin)) { continue; }

                
                if (module.ControlSource == "Modules/LoginModule.ascx")
                {
                    LoginModuleDisplaySettings loginSettings = new LoginModuleDisplaySettings();
                    this.MPContent.Controls.Add(loginSettings); ///theme is not applied until its loaded
                    if ((Request.IsAuthenticated) && (loginSettings.HideWhenAuthenticated)) { continue; }
                }

                Control parent = this.MPContent;

                if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "leftpane"))
                {
                    parent = this.MPLeftPane;

                }

                if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "rightpane"))
                {
                    parent = this.MPRightPane;

                }

                if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "altcontent1"))
                {
                    if (AltPane1 != null)
                    {
                        parent = this.AltPane1;
                    }
                    else
                    {
                        log.Error("Content is assigned to altcontent1 placeholder but it does not exist in layout.master so using center.");
                        parent = this.MPContent;
                    }

                }

                if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "altcontent2"))
                {
                    if (AltPane2 != null)
                    {
                        parent = this.AltPane2;
                    }
                    else
                    {
                        log.Error("Content is assigned to altcontent2 placeholder but it does not exist in layout.master so using center.");
                        parent = this.MPContent;
                    }

                }

                // this checks if we are using the mobile skin and whether the content is for all web only or mobile only
                if (!ShouldShowModule(module)) { continue; }

                // 2008-10-04 since more an more of our features use postback via ajax
                // its not feasible to use output caching as this breaks postback,
                // so I changed the default the use of WebConfigSettings.DisableContentCache to true
                // this also reduces the memory consumption footprint

                if ((module.CacheTime == 0) || (WebConfigSettings.DisableContentCache))
                {
                    //2008-10-16 in ulu's blog post:http://dotfresh.blogspot.com/2008/10/in-search-of-developer-friendly-cms.html
                    // he complains about having to inherit from a base class (SiteModuleControl) to make a plugin.
                    // he wishes he could just use a UserControl
                    // While SiteModuleControl "is a" UserControl that provides additional functionality
                    // Its easy enough to support using
                    // a plain UserControl so I'm making the needed change here now.
                    // The drawback of a plain UserControl is that is not reusable in the same way as SiteModuleControl.
                    // If you use a plain UserControl, its going to be exactly the same on any page you use it on.
                    // It has no instance specific properties.
                    // SiteModuleControl gives you instance specific ids and internal tracking of which instance this is so 
                    // that you can have different instances.
                    // For example the Blog is instance specific, if you put a blog on one page and then put a blog on another page
                    // those are 2 different blogs with different content.
                    // However, if you don't need a re-usable feature with instance specific properties
                    // you are now free to use a plain old UserControl and I think freedom is a good thing
                    // so this was valuable feedback from ulu.
                    // Those who do need instance specific features should read my developer Guidelines for building one:
                    //http://www.mojoportal.com/addingfeatures.aspx

                    try
                    {
                        Control c = Page.LoadControl("~/" + module.ControlSource);
                        if (c == null) { continue; }

                        if (c is SiteModuleControl)
                        {
                            SiteModuleControl siteModule = (SiteModuleControl)c;
                            siteModule.SiteId = siteSettings.SiteId;
                            siteModule.ModuleConfiguration = module;

                            if (siteModule is IWorkflow)
                            {
                                if (WebUser.IsInRoles(module.AuthorizedEditRoles)) { forceShowWorkflow = true; }
                                if (WebUser.IsInRoles(module.DraftEditRoles)) { forceShowWorkflow = true; }

                                countOfIWorkflow += 1;
                            }
                        }

                        parent.Controls.Add(c);
                    }
                    catch (HttpException ex)
                    {
                        log.Error("failed to load control " + module.ControlSource, ex);
                    }
                    
                }
                else
                {
                    CachedSiteModuleControl siteModule = new CachedSiteModuleControl();

                    siteModule.SiteId = siteSettings.SiteId;
                    siteModule.ModuleConfiguration = module;
                    parent.Controls.Add(siteModule);
                }

                parent.Visible = true;
                parent.Parent.Visible = true;

               
            } //end foreach

            SetupAdminLinks();

            if ((!WebConfigSettings.DisableExternalCommentSystems) && (siteSettings != null) && (CurrentPage != null) && (CurrentPage.EnableComments))
            {
                switch (siteSettings.CommentProvider)
                {
                    case "disqus":

                        if (siteSettings.DisqusSiteShortName.Length > 0)
                        {
                            DisqusWidget disqus = new DisqusWidget();
                            disqus.SiteShortName = siteSettings.DisqusSiteShortName;
                            disqus.WidgetPageUrl = WebUtils.ResolveServerUrl(SiteUtils.GetCurrentPageUrl());
                            if (disqus.WidgetPageUrl.StartsWith("https"))
                            {
                                disqus.WidgetPageUrl = disqus.WidgetPageUrl.Replace("https", "http");
                            }
                            disqus.RenderWidget = true;
                            MPContent.Controls.Add(disqus);
                        }

                        break;

                    case "intensedebate":

                        if (siteSettings.IntenseDebateAccountId.Length > 0)
                        {
                            IntenseDebateDiscussion d = new IntenseDebateDiscussion();
                            d.AccountId = siteSettings.IntenseDebateAccountId;
                            d.PostUrl = SiteUtils.GetCurrentPageUrl();
                            MPContent.Controls.Add(d);

                        }

                        

                        break;

                    case "facebook":
                        FacebookCommentWidget fbComments = new FacebookCommentWidget();
                        fbComments.AutoDetectUrl = true;
                        MPContent.Controls.Add(fbComments);

                        break;

                }

                


            }

            if (WebConfigSettings.HidePageViewModeIfNoWorkflowItems && (countOfIWorkflow == 0))
            {
                HideViewSelector();
            }

            // (to show the last mnodified time of a page we may have this control in layout.master, but I set it invisible by default
            // because we only want to show it on content pages not edit pages
            // since Default.aspx.cs is the handler for content pages, we look for it here and make it visible.
            Control pageLastMod = Master.FindControl("pageLastMod");
            if (pageLastMod != null) { pageLastMod.Visible = true; }
            
        }

        

        

        private void LoadSettings()
        {
            //isAdmin = WebUser.IsAdmin;
            //if (!isAdmin) { isContentAdmin = WebUser.IsContentAdmin; }

           // isSiteEditor = SiteUtils.UserIsSiteEditor();

            //userCanEditPage = WebUser.IsInRoles(CurrentPage.EditRoles);

            //if((isContentAdmin && !isAdmin)&&(CurrentPage.EditRoles == "Admins;"))
            //{
            //    userCanEditPage = false;
            //}

            //if ((userCanEditPage) && (WebConfigSettings.EnableDragDropPageLayout))
            //{
            //    ScriptConfig.IncludeInterface = true;
            //    SetupDragDropScript();
            //}

            //we need it enabled always in .NET 4 in order for viewstatemode to work
            //#if NET35
            //            if (WebConfigSettings.DisablePageViewStateByDefault)
            //            {
            //                this.EnableViewState = false;
            //            }
            //#endif
        }


        /// <summary>
        /// this is just an experiment looking into possibilities of drag drop page re-arrangement
        /// </summary>
        private void SetupDragDropScript()
        {
            StringBuilder script = new StringBuilder();

            script.Append("$(document).ready(");
            script.Append("function()");
            script.Append("{");

            script.Append("$('div.panelwrapper').each(function(){");
            script.Append("$(this).Draggable();");
            script.Append("});");

            script.Append("$('div.cmszone').each(function(){");
            //script.Append("$(this).Sortable(");
            script.Append("$(this).Droppable(");
            script.Append("{");
            script.Append("accept:'panelwrapper'");
            script.Append(",helperclass:'sortHelper'");
            script.Append(",activeclass:'cmsdropactive'");
            script.Append(",hoverclass:'cmsdrophover'");
            script.Append(",tolerance:'intersect'");
            

            // Droppable
            script.Append(",ondrop:	function (drag)");
            script.Append("{");
            script.Append("ModuleDrop(this,drag);");
            script.Append("}");

            // sortable
            ////script.Append(",handle:'div.panelwrapper'");
            //script.Append(",onChange:function (drag)");
            //script.Append("{");
            //script.Append("ModuleDrop(this,drag);");
            //script.Append("}");
            //script.Append(",onStart:function ()");
            //script.Append("{");
            //script.Append("$.iAutoscroller.start(this, document.getElementsByTagName('body'));");
            //script.Append("}");
            //script.Append(",onStop:function ()");
            //script.Append("{");
            //script.Append("$.iAutoscroller.stop();");
            //script.Append("}");
            


            script.Append("}");
            script.Append(");");//end sortable


            script.Append("}");
            script.Append(");");//end each

            script.Append("});");//end document.ready

           

            script.Append("function ModuleDrop(droppable, dragable)");
            script.Append("{");

            script.Append("alert(droppable);");
            script.Append("alert(dragable);");

            script.Append("}");

            


            Page.ClientScript.RegisterStartupScript(typeof(Page),
                   "droppanels", "\n<script type=\"text/javascript\">"
                   + script.ToString() + "</script>");

        }


        private void EnsurePageAndSite()
        {
            if (CurrentPage == null)
            {
                int siteCount = SiteSettings.SiteCount();
                
                
                if (siteCount == 0)
                {
                    // no site data so redirect to setup
                    HttpContext.Current.Response.Redirect(WebUtils.GetSiteRoot() + "/Setup/Default.aspx");
                }

                
            }


        }

        private bool RedirectIfNeeded()
        {
            if(!UserCanViewPage())
            {
                if (!Request.IsAuthenticated)
                {
                    if (WebConfigSettings.UseRawUrlForCmsPageLoginRedirects)
                    {
                        SiteUtils.RedirectToLoginPage(this);
                    }
                    else
                    {
                        SiteUtils.RedirectToLoginPage(this, SiteUtils.GetCurrentPageUrl());
                    }
                    return true;

                }
                else
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                    return true;
                }
            }

            return false;
        }

       

        private void EnforceSecuritySettings()
        {
            if (CurrentPage.PageId == -1) { return; }

            if (!CurrentPage.AllowBrowserCache)
            {
                SecurityHelper.DisableBrowserCache();
            }

            bool useSsl = false;
            
            if (SiteUtils.SslIsAvailable())
            {
                if (WebConfigSettings.ForceSslOnAllPages || siteSettings.UseSslOnAllPages || CurrentPage.RequireSsl)
                {
                    useSsl = true;
                }
            }

            if (useSsl)
            {
                SiteUtils.ForceSsl();
            }
            else
            {
               SiteUtils.ClearSsl();
            }

            

        }

//        private void SetupMasterPage()
//        {
//            // allow content system pages to use page settings
//            bool allowPageOverride = true;
//            if (
//                (siteSettings != null)
//                && (siteSettings.SiteGuid != Guid.Empty)
//                )
//            {
                
//                MasterPageFile = SiteUtils.GetMasterPage(this, siteSettings, allowPageOverride);
//                bool registeredVirtualThemes = Global.RegisteredVirtualThemes; //should always be true under .NET 4

//                if (registeredVirtualThemes)
//                {
//                    this.Theme = "default" + siteSettings.SiteId.ToInvariantString() + siteSettings.Skin;  
//                }


//                if ((registeredVirtualThemes)
//                     && ((siteSettings.AllowUserSkins) || ((WebConfigSettings.AllowEditingSkins) && (WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins))))
//                    )
//                {
//                    SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
//                    if((currentUser != null) &&(currentUser.Skin.Length > 0))
//                    {
//                        this.Theme = "userpersonal" + currentUser.Skin;
//                    }
//                }
                
//                if (
//                    (siteSettings.AllowPageSkins)
//                    &&(CurrentPage != null)
//                    &&(CurrentPage.Skin.Length > 0)
//                    )
//                {
//                    if (registeredVirtualThemes)
//                    {
//                        this.Theme = "pageskin-" + siteSettings.SiteId.ToInvariantString() + CurrentPage.Skin; 
//                    }
//                    else
//                    {
//                        this.Theme = "pageskin";
//                    }
//                }

//                if (registeredVirtualThemes)
//                {
//                    string previewSkin = SiteUtils.GetSkinPreviewParam(siteSettings);
//                    if (previewSkin.Length > 0)
//                    {
//                        this.Theme = "preview_" + previewSkin;
//                    }

//                    if(UseMobileSkin)   
//                    {
//                        this.Theme = "mobile" + siteSettings.SiteId.ToInvariantString();
//                    }
//                }

//#if MONO
//                this.Theme = "default";
//#endif

//            }

//            MPLeftPane = (ContentPlaceHolder)Master.FindControl("leftContent");
//            MPContent = (ContentPlaceHolder)Master.FindControl("mainContent");
//            MPRightPane = (ContentPlaceHolder)Master.FindControl("rightContent");
//            MPPageEdit = (ContentPlaceHolder)Master.FindControl("pageEditContent");

//            AltPane1 = (ContentPlaceHolder)Master.FindControl("altContent1");
//            AltPane2 = (ContentPlaceHolder)Master.FindControl("altContent2");

//            StyleSheetControl = (StyleSheet)Master.FindControl("StyleSheet");

//        }

        

        private void SetupAdminLinks()
        {
            
            // 2010-01-04 made it possible to add these links directly in layout.master so they can be arranged and styled easier
            if (Page.Master.FindControl("lnkPageContent") == null)
            {
                this.MPPageEdit.Controls.Add(new PageEditFeaturesLink());
            }

            if (Page.Master.FindControl("lnkPageSettings") == null)
            {
                this.MPPageEdit.Controls.Add(new PageEditSettingsLink());
            }

            SetupWorkflowControls(forceShowWorkflow);
            
        }

        

        

       


    }
}
