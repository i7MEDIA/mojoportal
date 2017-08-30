//	Created:			    2008-08-18
//	Last Modified:		    2015-04-27
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using System;
using System.Globalization;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    
    public class ScriptLoader : WebControl
    {
   
        #region Properties

        ScriptManager scriptManager = null;
        private string protocol = "http";
        //private bool isIpad = false;
        //private bool isSmartPhone = false;

        private bool addMSAjaxScriptReference = true;
        /// <summary>
        /// if you set this to false then you need this in layout.master ScriptManagerDeclaration:
        /// <asp:ScriptManager runat="server">
        ///    <Scripts>
        ///        <asp:ScriptReference Name="MsAjaxBundle" />
        ///        ...
        ///    </Scripts>
        ///</asp:ScriptManager>
        ///with this as true we are just doing this programatically
        ///because most existing skins won't have the above
        /// </summary>
        public bool AddMSAjaxScriptReference
        {
            get { return addMSAjaxScriptReference; }
            set { addMSAjaxScriptReference = value; }
        }

        

        //TODO: when loading remote javascripts should we use the default culture or the current culture
        // or let it be specified on the ScriptLoader as it is here?

        private string languageCode = "en";

        public string LanguageCode
        {
            get { return languageCode; }
            set { languageCode = value; }
        }

        private bool requireExitPrompt = false;
        /// <summary>
        /// this is used to set a javascript variable to enable prompting a user if they try to navigate away from a page with unsaved content
        /// it does not do anything by itself it still requires a call to the hookupGoodbyePrompt method which is included in mojocombined.js
        /// TinyMCE and CKeditor will call that method when content in the editor has changed
        /// Note that the save button or any button that does postback will also trigger the prompt so we have to add extra javascript to the save buttons
        /// in features to set the variable to false in order to prevent the prompt when the user does click the button to save the content
        /// So edit pages should set ScriptConfig.EnableExitPromptForUnsavedContent = true; and should also add script to their save buttons to implement this feature
        /// we have a helper mthod for that: UIHelper.AddClearPageExitCode(btnUpdate);
        /// study the edit pages for existing features to learn more
        /// </summary>
        public bool EnableExitPromptForUnsavedContent
        {
            get { return requireExitPrompt; }
            set { requireExitPrompt = value; }
        }

        private bool includeGoogleGeoLocator = false;

        public bool IncludeGoogleGeoLocator
        {
            get { return includeGoogleGeoLocator; }
            set
            {
                includeGoogleGeoLocator = value;
                if (includeGoogleGeoLocator) { includeGoogleMaps = true; }
            }
        }

        private bool includeMediaElement = false;

        public bool IncludeMediaElement
        {
            get { return includeMediaElement; }
            set { includeMediaElement = value; }
        }

        private bool includeGoogleMaps = false;

        public bool IncludeGoogleMaps
        {
            get { return includeGoogleMaps; }
            set { includeGoogleMaps = value; }
        }

        private bool includeGoogleSearch = false;

        public bool IncludeGoogleSearch
        {
            get { return includeGoogleSearch; }
            set { includeGoogleSearch = value; }
        }

        private bool includeGoogleSearchV2 = false;

        public bool IncludeGoogleSearchV2
        {
            get { return includeGoogleSearchV2; }
            set { includeGoogleSearchV2 = value; }
        }

        private string googleSearchV2Id = string.Empty;
        public string GoogleSearchV2Id
        {
            get { return googleSearchV2Id; }
            set { googleSearchV2Id = value; }
        }

        private bool includeWebSnapr = false;

        public bool IncludeWebSnapr
        {
            get { return includeWebSnapr; }
            set { includeWebSnapr = value; }
        }

        private bool includeClueTip = false;

        public bool IncludeClueTip
        {
            get { return includeClueTip; }
            set 
            { 
                includeClueTip = value;
                if (includeClueTip) { includeJQuery = true; }
            }
        }

        private bool includeSimpleFaq = true;

        public bool IncludeSimpleFaq
        {
            get { return includeSimpleFaq; }
            set
            {
                includeSimpleFaq = value;
                if (includeSimpleFaq) { includeJQuery = true; }
            }
        }

        private bool includeMarkitUpHtml = false;

        public bool IncludeMarkitUpHtml
        {
            get { return includeMarkitUpHtml; }
            set
            {
                includeMarkitUpHtml = value;
                if (includeMarkitUpHtml) { includeJQuery = true; }
            }
        }

        //private bool includeOomph = true;

        //public bool IncludeOomph
        //{
        //    get { return includeOomph; }
        //    set { includeOomph = value; }
        //}

        private bool includeYahooMediaPlayer = false;

        public bool IncludeYahooMediaPlayer
        {
            get { return includeYahooMediaPlayer; }
            set { includeYahooMediaPlayer = value; }
        }

        private bool includeSwfObject = false;

        public bool IncludeSwfObject
        {
            get { return includeSwfObject; }
            set { includeSwfObject = value; }
        }

        private bool includeJQuery = true;

        public bool IncludeJQuery
        {
            get { return includeJQuery; }
            set { includeJQuery = value; }
        }

        //http://www.jplayer.org/
        private bool includejPlayer = false;

        public bool IncludejPlayer
        {
            get { return includejPlayer; }
            set
            {
                includejPlayer = value;
                if (includejPlayer) { includeJQuery = true; }
            }
        }

        private bool includejQueryCycle = false;

        public bool IncludejQueryCycle
        {
            get { return includejQueryCycle; }
            set
            {
                includejQueryCycle = value;
                if (includejQueryCycle) { includeJQuery = true; }
            }
        }

        private bool includeNivoSlider = false;

        public bool IncludeNivoSlider
        {
            get { return includeNivoSlider; }
            set
            {
                includeNivoSlider = value;
                if (includeNivoSlider) { includeJQuery = true; }
            }
        }

        private bool includejQueryValidator = false;

        public bool IncludejQueryValidator
        {
            get { return includejQueryValidator; }
            set
            {
                includejQueryValidator = value;
                if (includejQueryValidator) { includeJQuery = true; }
            }
        }

        private bool includejQueryUICore = true;

        public bool IncludejQueryUICore
        {
            get { return includejQueryUICore; }
            set { includejQueryUICore = value; }
        }

        //private bool useJQueryMobile = false;
        //public bool UseJQueryMobile
        //{
        //    get { return useJQueryMobile; }
        //    set
        //    {
        //        useJQueryMobile = value;
        //        if (useJQueryMobile) { includeJQuery = true; }
        //    }
        //}

        private bool includejQueryAccordion = true;

        public bool IncludejQueryAccordion
        {
            get { return includejQueryAccordion; }
            set 
            { 
                includejQueryAccordion = value;
                if (includejQueryAccordion) { includejQueryUICore = true; }
            }
        }

        //private bool includejQueryFileTree = false;

        //public bool IncludejQueryFileTree
        //{
        //    get { return includejQueryFileTree; }
        //    set
        //    {
        //        includejQueryFileTree = value;
        //        if (includejQueryFileTree) { includeJQuery = true; }
        //    }
        //}

        //private bool includejQueryLayout = false;

        //public bool IncludejQueryLayout
        //{
        //    get { return includejQueryLayout; }
        //    set
        //    {
        //        includejQueryLayout = value;
        //        if (includejQueryLayout) { includeJQuery = true; includejQueryUICore = true; }
        //    }
        //}

        private string jQueryTabConfig = "{}";

        /// <summary>
        /// can be an empty string or a json string with configuration for the jqueryui tabs example: { fx: { opacity: 'toggle', duration: 'fast'} }
        /// </summary>
        public string JQueryTabConfig
        {
            get { return jQueryTabConfig; }
            set { jQueryTabConfig = value; }
        }

        private string jQueryAccordionConfig = "{}";

        /// <summary>
        /// can be an empty string or a json string with configuration for the jqueryui tabs example: { fx: { opacity: 'toggle', duration: 'fast'} }
        /// </summary>
        public string JQueryAccordionConfig
        {
            get { return jQueryAccordionConfig; }
            set { jQueryAccordionConfig = value; }
        }

        //private string jQueryAccordionNoHeightConfig = "{fx:{opacity:'toggle',duration:'fast'},autoHeight:false}";
        // autoHeight is now deprectated in jqueryui
        private string jQueryAccordionNoHeightConfig = "{heightStyle:'content',animate:{opacity:'toggle',duration:'400'}}";

        /// <summary>
        /// can be an empty string or a json string with configuration for the jqueryui tabs example: { fx: { opacity: 'toggle', duration: 'fast'} }
        /// </summary>
        public string JQueryAccordionNoHeightConfig
        {
            get { return jQueryAccordionNoHeightConfig; }
            set { jQueryAccordionNoHeightConfig = value; }
        }

        private bool includeQtFile = false;

        public bool IncludeQtFile
        {
            get { return includeQtFile; }
            set { 
                includeQtFile = value;
                if (includeQtFile) { includeImpromtu = true; includeJQuery = true; includejQueryUICore = true; }
            }
        }

        private bool includeImpromtu = false;

        public bool IncludeImpromtu
        {
            get { return includeImpromtu; }
            set
            {
                includeImpromtu = value;
                if (includeImpromtu) { includeJQuery = true; includejQueryUICore = true; }
            }
        }

        private bool includeJqueryScroller = false;

        public bool IncludeJqueryScroller
        {
            get { return includeJqueryScroller; }
            set
            {
                includeJqueryScroller = value;
                if (includeJqueryScroller) { includeJQuery = true; }
            }
        }

        //private bool includejQueryExtruder = false;

        //public bool IncludejQueryExtruder
        //{
        //    get { return includejQueryExtruder; }
        //    set
        //    {
        //        includejQueryExtruder = value;
        //        if (includejQueryExtruder)
        //        {
        //            includeJQuery = true;
        //            includejQueryHoverIntent = true;
        //            includejQueryMetaData = true;
        //            includejQueryFlipText = true;
        //        }
        //    }
        //}

        //private bool includejQueryHoverIntent = false;

        //public bool IncludejQueryHoverIntent
        //{
        //    get { return includejQueryHoverIntent; }
        //    set
        //    {
        //        includejQueryHoverIntent = value;
        //        if (includejQueryHoverIntent) { includeJQuery = true; }
        //    }
        //}

        //private bool includejQueryMetaData = false;

        //public bool IncludejQueryMetaData
        //{
        //    get { return includejQueryMetaData; }
        //    set
        //    {
        //        includejQueryMetaData = value;
        //        if (includejQueryMetaData) { includeJQuery = true; }
        //    }
        //}

        //private bool includejQueryFlipText = false;

        //public bool IncludejQueryFlipText
        //{
        //    get { return includejQueryFlipText; }
        //    set
        //    {
        //        includejQueryFlipText = value;
        //        if (includejQueryFlipText) { includeJQuery = true; }
        //    }
        //}

        private bool includeFlickrGallery = false;
        public bool IncludeFlickrGallery
        {
            get { return includeFlickrGallery; }
            set
            {
                includeFlickrGallery = value;
                if (includeFlickrGallery) { includeJQuery = true; }
            }
        }

        // only difference is the positioning og the lightbox
        private bool useMobileVersionOfFlickrGallery = false;
        public bool UseMobileVersionOfFlickrGallery
        {
            get { return useMobileVersionOfFlickrGallery; }
            set { useMobileVersionOfFlickrGallery = value; }
        }

        private bool includeColorBox = false;
        public bool IncludeColorBox
        {
            get { return includeColorBox; }
            set
            {
                includeColorBox = value;
                if (includeColorBox) { includeJQuery = true; }
            }
        }

        private bool includeGreyBox = false;

        public bool IncludeGreyBox
        {
            get { return includeGreyBox; }
            set { includeGreyBox = value; }
        }

        private bool includeSizzle = false;
        public bool IncludeSizzle
        {
            get { return includeSizzle; }
            set { includeSizzle = value; }
        }

        
        private bool includeColorPicker = false;
        private bool includeSlider = false;
        private bool includeYuiDom = false;

        public bool IncludeColorPicker
        {
            get { return includeColorPicker; }
            set
            {
                includeColorPicker = value;
                if (includeColorPicker)
                {
                    includeYuiDom = true;
                    includeSlider = true;
                    
                }
            }
        }

        private bool includeYuiTabs = false;
       
        public bool IncludeYuiTabs
        {
            get { return includeYuiTabs; }
            set 
            { 
                includeYuiTabs = value;
                if (includeYuiTabs)
                {
                    includeYuiDom = true;
                }
            }
        }

       
        private bool includeYuiAccordion = false;

        public bool IncludeYuiAccordion
        {
            get { return includeYuiAccordion; }
            set 
            { 
                includeYuiAccordion = value;
                if (includeYuiAccordion) { includeYuiDom = true; }
            }
        }

        private bool renderjQueryInHead = true;

        public bool RenderjQueryInHead
        {
            get { return renderjQueryInHead; }
            set { renderjQueryInHead = value; }
        }

        private bool assumejQueryIsLoaded = false;

        public bool AssumejQueryIsLoaded
        {
            get { return assumejQueryIsLoaded; }
            set { assumejQueryIsLoaded = value; }
        }

        private bool assumejQueryUiIsLoaded = false;

        public bool AssumejQueryUiIsLoaded
        {
            get { return assumejQueryUiIsLoaded; }
            set { assumejQueryUiIsLoaded = value; }
        }

        private bool assumeMarkitupIsLoaded = false;

        public bool AssumeMarkitupIsLoaded
        {
            get { return assumeMarkitupIsLoaded; }
            set { assumeMarkitupIsLoaded = value; }
        }

        private bool assumeMojoCombinedIsLoaded = false;

        public bool AssumeMojoCombinedIsLoaded
        {
            get { return assumeMojoCombinedIsLoaded; }
            set { assumeMojoCombinedIsLoaded = value; }
        }

        private bool includeAspTreeView = false;

        public bool IncludeAspTreeView
        {
            get { return includeAspTreeView; }
            set { includeAspTreeView = value; }
        }

        private bool includeAspMenu = false;

        public bool IncludeAspMenu
        {
            get { return includeAspMenu; }
            set { includeAspMenu = value; }
        }

        private bool includeAjaxToolkit = false;
        /// <summary>
        /// if either includeAjaxToolkit or alwaysIncludeAjaxToolkit is true
        /// a script reference will be added to script manager
        /// equivalent to this in the layout.master file:
        /// <asp:ScriptReference Name="MsAjaxBundle" />
        /// 
        /// includeAjaxToolkit is meant to be set programatically to true per page if ajaxcontroltoolkit is needed
        /// alwaysIncludeAjaxToolkit is false by default but if you want to force it to load the scripts for ajaxtoolkit 
        /// you can set it true ie if your custom code that depends on ajaxtoolkit is not setting includeAjaxToolkit 
        /// programtically for you. Typically you would get a reference to mojoBasePage.ScriptConfig.IncludeAjaxToolkit = true;
        /// </summary>
        public bool IncludeAjaxToolkit
        {
            get { return includeAjaxToolkit; }
            set { includeAjaxToolkit = value; }
        }

        private bool alwaysIncludeAjaxToolkit = false;
        /// <summary>
        /// if you set this as false then you would need to add this in the <Scripts></Scripts>
        /// element inside <ScriptManager></ScriptManager> in the layout.master file:
        /// <asp:ScriptReference Name="MsAjaxBundle" />
        /// with this true we are just adding it programatically because most existing skins don't have that
        /// </summary>
        public bool AlwaysIncludeAjaxToolkit
        {
            get { return alwaysIncludeAjaxToolkit; }
            set { alwaysIncludeAjaxToolkit = value; }
        }

        private bool autoAddAjaxToolkitCss = true;
        /// <summary>
        /// if this is true in addition to either includeAjaxToolkit or alwaysIncludeAjaxToolkit
        /// then the css for the toolkit will be rendered in the render method of this control
        /// </summary>
        public bool AutoAddAjaxToolkitCss
        {
            get { return autoAddAjaxToolkitCss; }
            set { autoAddAjaxToolkitCss = value; }
        }
        

        private bool includeJqueryTmpl = false;

        public bool IncludeJqueryTmpl
        {
            get { return includeJqueryTmpl; }
            set { 
                includeJqueryTmpl = value;
                if (includeJqueryTmpl) { includeJQuery = true; }
            }
        }

        private bool includeKnockoutJs = false;

        public bool IncludeKnockoutJs
        {
            get { return includeKnockoutJs; }
            set { 

                includeKnockoutJs = value; 
            }
        }

        private bool includeImageFit = false;

        public bool IncludeImageFit
        {
            get { return includeImageFit; }
            set { 
                includeImageFit = value;
                if (includeImageFit) { includeJQuery = true; }
            }
        }

        private string imageFitSelector = string.Empty;
        public string ImageFitSelector
        {
            get { return imageFitSelector; }
            set { imageFitSelector = value; }
        }

        private string mojoCombinedFullScript = "/mojocombined/mojocombinedfull.js";

        /// <summary>
        ///  in case you need to override with a different script file to include more things
        /// </summary>
        public string MojoCombinedFullScript
        {
            get { return mojoCombinedFullScript; }
            set { mojoCombinedFullScript = value; }
        }

        private bool renderCombinedInHead = false;

        public bool RenderCombinedInHead
        {
            get { return renderCombinedInHead; }
            set
            {
                renderCombinedInHead = value;
                //if (renderCombinedInHead) { usedMojoCombinedFull = true; }
            }
        }

        //private bool usedMojoCombinedFull = false;

        private bool combineScriptsWithScriptManager = true;

        public bool CombineScriptsWithScriptManager
        {
            get { return combineScriptsWithScriptManager; }
            set { combineScriptsWithScriptManager = value; }
        }

        //http://www.gettopup.com/
        //bool includeTopup = ConfigHelper.GetBoolProperty("UseTopup", false);

        private bool includeColorboxHelp = true;

        public bool IncludeColorboxHelp
        {
            get { return includeColorboxHelp; }
            set { includeColorboxHelp = value; }
        }

        private string colorBoxConfig = "{width:'85%', height:'85%', iframe:true}";

        public string ColorBoxConfig
        {
            get { return colorBoxConfig; }
            set { colorBoxConfig = value; }
        }

        private bool includeModernizr = false;
        public bool IncludeModernizr
        {
            get { return includeModernizr; }
            set { includeModernizr = value; }
        }

        private string modernizrFileName = "modernizr-2.5.3.min.js";
        public string ModernizrFileName
        {
            get { return modernizrFileName; }
            set { modernizrFileName = value; }
        }


        private bool includeJQueryMigrate = false;

        public bool IncludeJQueryMigrate
        {
            get { return includeJQueryMigrate; }
            set { includeJQueryMigrate = value; }
        }

        private string jQueryMigrateUrl = "~/ClientScript/jqmojo/jquery-migrate1-0-0.js";

        public string JQueryMigrateUrl
        {
            get { return jQueryMigrateUrl; }
            set { jQueryMigrateUrl = value; }
        }
        

        

        #endregion


        protected override void Render(HtmlTextWriter writer)
        {
            // Modernizr needs to be the first script in the head
            if (includeModernizr)
            {
                writer.Write("\n<script src=\"" + Page.ResolveUrl("~/ClientScript/" + modernizrFileName)  + "\" type=\"text/javascript\" ></script>");
            }
            
            if (renderjQueryInHead)
            {
                if (WebConfigSettings.DisablejQuery) { return; }

                if (!assumejQueryIsLoaded)
                {
                    if (includeJQuery)
                    {
                        string jqueryBasePath = GetJQueryBasePath();

                        writer.Write("\n<script src=\"" + jqueryBasePath + "jquery.min.js" + "\" type=\"text/javascript\" ></script>");

                        if (includeJQueryMigrate)
                        {
                            writer.Write("\n<script src=\"" + Page.ResolveUrl(jQueryMigrateUrl) + "\" type=\"text/javascript\" ></script>");
                        }
                    }
 
                }

                if (!assumejQueryUiIsLoaded)
                {
                    if (includejQueryUICore)
                    {
                        string jqueryUIBasePath = GetJQueryUIBasePath();
                        writer.Write("\n<script src=\"" + jqueryUIBasePath + "jquery-ui.min.js" + "\" type=\"text/javascript\" ></script>");


                    }
                }

               

            }

            if (!assumeMojoCombinedIsLoaded && (renderCombinedInHead))
            {
                writer.Write("\n<script  src=\""
                        + Page.ResolveUrl("~/ClientScript/" + mojoCombinedFullScript + "?" + WebConfigSettings.mojoCombinedScriptVersionParam)
                        + "\" type=\"text/javascript\" ></script>");
            }

            if ((includeGoogleSearchV2) && (googleSearchV2Id.Length > 0))
            {
                writer.Write(BuildGoogleSearchV2Script());
            }

            
            if(includeAjaxToolkit || alwaysIncludeAjaxToolkit)
            {
                if (!WebConfigSettings.DisableAjaxToolkitBundlesAndScriptReferences)
                {
                    if (autoAddAjaxToolkitCss)
                    {
                        writer.Write("\n");
                        writer.Write(System.Web.Optimization.Styles.Render("~/Content/AjaxControlToolkit/Styles/Bundle").ToHtmlString());
                    }

                }
                
                
            }
        }

        private string BuildGoogleSearchV2Script()
        {
            StringBuilder script = new StringBuilder();

            script.Append("<script>");

            script.Append("(function() {");
            script.Append("var cx = '" + googleSearchV2Id + "';");
            script.Append("var gcse = document.createElement('script'); gcse.type = 'text/javascript'; gcse.async = true; ");
            script.Append("gcse.src = (document.location.protocol == 'https:' ? 'https:' : 'http:') + ");
            script.Append("'//www.google.com/cse/cse.js?cx=' + cx; ");
            script.Append("var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(gcse, s); ");
            script.Append("})();");

            script.Append("</script>");

            return script.ToString();

        }
        

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            //if (WebConfigSettings.AlwaysLoadYuiTabs) { IncludeYuiTabs = true; }

            //if (!assumeMarkitupIsLoaded)
            //{
            //    isIpad = BrowserHelper.IsIpad();
            //    isSmartPhone = SiteUtils.IsMobileDevice();
            //    if (
            //        (isIpad && WebConfigSettings.UseMarkitUpForiPad)
            //        || (isSmartPhone && WebConfigSettings.UseMarkitUpForSmartPhones)
            //        )
            //    { includeMarkitUpHtml = true; }
            //}

#if NET35
            combineScriptsWithScriptManager = false;
#endif

            if (Page.Master != null)
            {
                scriptManager = Page.Master.FindControl("ScriptManager1") as ScriptManager;
                if (scriptManager == null)
                {
                    scriptManager = Page.Form.FindControl("ScriptManager1") as ScriptManager;
                }
            }

            //if (WebConfigSettings.AssumejQueryIsLoaded)
            //{
            //    assumejQueryIsLoaded = true;
            //    assumejQueryUiIsLoaded = true;
            //}

            //if (WebConfigSettings.mojoCombinedScript.Length > 0)
            //{
            //    mojoCombinedFullScript = WebConfigSettings.mojoCombinedScript;
            //}

            //setup these in page load to ensure the come in before other scripts that may depend on them
            SetupCoreScripts();
            
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);

            if (HttpContext.Current == null) { return ; }
            if (HttpContext.Current.Request == null) { return ; }

            if (SiteUtils.IsSecureRequest()) { protocol = "https"; }

            SetupScripts();
        }

        private void SetupCoreScripts()
        {
            SetupmojoCombined();
            if (!WebConfigSettings.DisablejQuery)
            {
                SetupJQuery();
                if (!WebConfigSettings.DisablejQueryUI) { SetupJQueryUICore(); }  
            }

           

        }

        
        private void SetupScripts()
        {

            //SetupJqueryScriptManagerReference(scriptManager);
            //SetupJqueryUIScriptManagerReference(scriptManager);
            //SetupmojoCombined();

            
            if (addMSAjaxScriptReference) { SetupMSAjaxScripts(scriptManager); }

            if (includeAjaxToolkit || alwaysIncludeAjaxToolkit) { SetupAjaxControlToolkitScripts(scriptManager); }
                
            
            if (includeAspTreeView) { SetupAspTreeView(); }
            if (includeAspMenu) { SetupAspMenu(); }

            if (!WebConfigSettings.DisablejQuery) 
            { 
                if (WebConfigSettings.DisablejQueryUI) { return; }
               
                SetupInitScript();
                if (includeImpromtu) { SetupImpromtu(); }
                if (includeQtFile) { SetupQtFile(); }
				//if (includejQueryLayout) { SetupJqueryLayout(); }
				//if (includejQueryFileTree) { SetupJQueryFileTree(); }
                if (includejQueryValidator) { SetupJQueryValidate(); }
                if (includeFlickrGallery) { SetupFlickrGallery(); }
                if (includeColorBox) { SetupColorBox(); }
                if (includeMarkitUpHtml) { SetupMarkitUpHtml(); }
                if (includejPlayer) { SetupjPlayer(); }
                if (includeJqueryTmpl) { SetupJQueryTmpl(); }
                if (includeJqueryScroller) { SetupScroller(); }
                if (includeClueTip) { SetupClueTip(); }
                if (includejQueryCycle) { SetupjQueryCycle(); }
                if (includeNivoSlider) { SetupNivoSlider(); }
                if (includeMediaElement) { SetupMediaElement(); }
            }
            
            if (!WebConfigSettings.DisableYUI) { SetupYui(); }

            if (includeKnockoutJs) { SetupKnockoutJs(); }
            if (includeImageFit) { SetupJQueryImageFit(); }
            if (!WebConfigSettings.DisableWebSnapr && (includeWebSnapr)) { SetupWebSnapr(); }
            if (includeYahooMediaPlayer) { SetupYahooMediaPlayer(); }
            if (includeSwfObject) { SetupSwfObject(); }
            SetupBrowserSpecificScripts();

            if (includeGreyBox) { SetupGreyBox(); }
            SetupGoogleAjax();
            if (includeSizzle) { SetupSizzle(); }

            if (requireExitPrompt)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page),
                  "requireExitPrompt", "\n<script type=\"text/javascript\">\n requireExitPrompt = true;\n</script>", false);
            }
           
        }

        private bool autoWireJQueryUITooltip = true;

        public bool AutoWireJQueryUITooltip
        {
            get { return autoWireJQueryUITooltip; }
            set { autoWireJQueryUITooltip = value; }
        }

        private bool includeJQTable = false;

        public bool IncludeJQTable
        {
            get { return includeJQTable; }
            set { includeJQTable = value; }
        }

        private bool disableJQTable = true;

        public bool DisableJQTable
        {
            get { return disableJQTable; }
            set { disableJQTable = value; }
        }

        private void SetupInitScript()
        {
            if (includejQueryAccordion)
            {
                // this also includes jqueryui tabs

                StringBuilder initAutoScript = new StringBuilder();
                initAutoScript.Append(" $('div.mojo-accordion').accordion(" + jQueryAccordionConfig + ");");
                initAutoScript.Append("$('div.mojo-accordion-nh').accordion(" + jQueryAccordionNoHeightConfig + "); ");
                initAutoScript.Append("$('div.mojo-tabs').tabs(" + jQueryTabConfig + "); $('input.jqbutton').button(); ");

                if (autoWireJQueryUITooltip)
                {
                    initAutoScript.Append("$('.jqtt').tooltip(); ");
                }

                if (includeColorBox && includeColorboxHelp)
                {
                    initAutoScript.Append("$('a.cblink').colorbox(" + colorBoxConfig + ");");
                }


                if (includeSimpleFaq)
                {
                    initAutoScript.Append("$('.faqs dd').hide();"); // Hide all DDs inside .faqs
                    initAutoScript.Append("$('.faqs dt').hover(function(){$(this).addClass('hover')},function(){$(this).removeClass('hover')}).click(function(){ "); // Add class "hover" on dt when hover
                    initAutoScript.Append("$(this).next().slideToggle('normal'); });  "); // Toggle dd when the respective dt is clicked
                    //initAutoScript.Append(" ");
                    //initAutoScript.Append(" ");
                    //initAutoScript.Append(" ");
                }

                if (includeImageFit && imageFitSelector.Length > 0)
                {
                    initAutoScript.Append("$('" + imageFitSelector + "').imagefit();");
                }

                if (includeJQTable && !disableJQTable)
                {
                   // initAutoScript.Append("function setupJTable() {");

                    initAutoScript.Append("$('table.jqtable th').each(function(){ ");
                    initAutoScript.Append("$(this).addClass('ui-state-default'); ");
                    initAutoScript.Append("}); ");
                    initAutoScript.Append("$('table.jqtable td').each(function(){ ");
                    initAutoScript.Append("$(this).addClass('ui-widget-content'); ");
                    initAutoScript.Append("}); ");
                    initAutoScript.Append("$('table.jqtable tr').hover( ");
                    initAutoScript.Append("function() {");
                    initAutoScript.Append("$(this).children('td').addClass('ui-state-hover'); ");
                    initAutoScript.Append("},function() {");
                    initAutoScript.Append("$(this).children('td').removeClass('ui-state-hover'); ");
                    initAutoScript.Append("} ");
                    initAutoScript.Append("); ");
                    initAutoScript.Append("$('table.jqtable tr').click(function() { ");
                    initAutoScript.Append("$(this).children('td').toggleClass('ui-state-highlight'); ");
                    initAutoScript.Append("}); ");
                 //   initAutoScript.Append("} "); // end function

                 //   initAutoScript.Append("setupJTable(); ");

                }


                //
                if (Page is mojoBasePage)
                {
                    mojoBasePage basePage = Page as mojoBasePage;
                    if (basePage.StyleCombiner.EnableNonClickablePageLinks)
                    {
                        initAutoScript.Append("$(\"a.unclickable\").click(function(){ return false; });");
                    }
                }

                //string initAutoScript = "$(document).ready(function () { $('div.mojo-accordion').accordion(); $('div.mojo-accordion-nh').accordion({autoHeight: false});  $('div.mojo-tabs').tabs();  }); ";

                ScriptManager.RegisterStartupScript(this, typeof(Page),
                   "jui-init", "\n<script type=\"text/javascript\" >"
                   + initAutoScript.ToString() + "</script>", false);
            }
        }

        private void SetupWebFormsScripts(ScriptManager scriptManager)
        {
            if (scriptManager == null) { return; }

            var script = new ScriptReference("WebForms.js", "System.Web");
            script.Path = "~/Scripts/WebForms/WebForms.js";
            //scriptManager.CompositeScript.Scripts.Add(script);
            scriptManager.Scripts.Add(script);


            script = new ScriptReference("WebUIValidation.js", "System.Web");
            script.Path = "~/Scripts/WebForms/WebUIValidation.js";
            scriptManager.Scripts.Add(script);

            script = new ScriptReference("MenuStandards.js", "System.Web");
            script.Path = "~/Scripts/WebForms/MenuStandards.js";
            scriptManager.Scripts.Add(script);

            script = new ScriptReference("GridView.js", "System.Web");
            script.Path = "~/Scripts/WebForms/GridView.js";
            scriptManager.Scripts.Add(script);

            script = new ScriptReference("DetailsView.js", "System.Web");
            script.Path = "~/Scripts/WebForms/DetailsView.js";
            scriptManager.Scripts.Add(script);

            script = new ScriptReference("TreeView.js", "System.Web");
            script.Path = "~/Scripts/WebForms/TreeView.js";
            scriptManager.Scripts.Add(script);

            script = new ScriptReference("WebParts.js", "System.Web");
            script.Path = "~/Scripts/WebForms/WebParts.js";
            scriptManager.Scripts.Add(script);

            script = new ScriptReference("Focus.js", "System.Web");
            script.Path = "~/Scripts/WebForms/Focus.js";
            scriptManager.Scripts.Add(script);

        }

        private void SetupMSAjaxScripts(ScriptManager scriptManager)
        {

            if (scriptManager == null) { return; }

            SetupWebFormsScripts(scriptManager);

           

            ScriptReference scriptReference = new ScriptReference("~/bundles/WebFormsJs");
            scriptReference.Name = "WebFormsBundle";
            scriptManager.Scripts.Add(scriptReference);

            scriptReference = new ScriptReference("~/bundles/MsAjaxJs");
            scriptReference.Name = "MsAjaxBundle";
            scriptManager.Scripts.Add(scriptReference);

            if (WebConfigSettings.BundlesUseCdn)
            {
                scriptManager.EnableCdn = true;
                // for some reason with true it was using the cdn url as the fallback url
                // which is redundant
                scriptManager.EnableCdnFallback = false;
                
            }

            //ScriptReference scriptReference = new ScriptReference("MsAjaxBundle","");
            //AddBundleScriptReference(scriptManager, scriptReference);

            //ScriptReference scriptReference = new ScriptReference("MicrosoftAjax.js", "System.Web.Extensions");
            //AddNamedScriptReference(scriptManager, scriptReference);

            //scriptReference = new ScriptReference("MicrosoftAjaxWebForms.js", "System.Web.Extensions");
            //AddNamedScriptReference(scriptManager, scriptReference);

        }

        private void SetupAjaxControlToolkitScripts(ScriptManager scriptManager)
        {
            if (WebConfigSettings.DisableAjaxToolkitBundlesAndScriptReferences) { return; }
            
            if (scriptManager == null) { return; }

            ScriptReference scriptReference = new ScriptReference("~/Scripts/AjaxControlToolkit/Bundle");
            scriptManager.Scripts.Add(scriptReference);



            // below is not working/needed with the latest toolkit

            //ScriptReference scriptReference = new ScriptReference("Common.Common.js", "AjaxControlToolkit");
            //AddNamedScriptReference(scriptManager, scriptReference);

            //scriptReference = new ScriptReference("Compat.Timer.Timer.js", "AjaxControlToolkit");
            //AddNamedScriptReference(scriptManager, scriptReference);

            //scriptReference = new ScriptReference("Animation.Animations.js", "AjaxControlToolkit");
            //AddNamedScriptReference(scriptManager, scriptReference);

            //scriptReference = new ScriptReference("Compat.DragDrop.DragDropScripts.js", "AjaxControlToolkit");
            //AddNamedScriptReference(scriptManager, scriptReference);

            //scriptReference = new ScriptReference("ExtenderBase.BaseScripts.js", "AjaxControlToolkit");
            //AddNamedScriptReference(scriptManager, scriptReference);

            //scriptReference = new ScriptReference("Slider.SliderBehavior_resource.js", "AjaxControlToolkit");
            //AddNamedScriptReference(scriptManager, scriptReference);

            //scriptReference = new ScriptReference("Common.DateTime.js", "AjaxControlToolkit");
            //AddNamedScriptReference(scriptManager, scriptReference);

            //scriptReference = new ScriptReference("Animation.AnimationBehavior.js", "AjaxControlToolkit");
            //AddNamedScriptReference(scriptManager, scriptReference);

            //scriptReference = new ScriptReference("PopupExtender.PopupBehavior.js", "AjaxControlToolkit");
            //AddNamedScriptReference(scriptManager, scriptReference);

            //scriptReference = new ScriptReference("Common.Threading.js", "AjaxControlToolkit");
            //AddNamedScriptReference(scriptManager, scriptReference);

            //scriptReference = new ScriptReference("Calendar.CalendarBehavior.js", "AjaxControlToolkit");
            //AddNamedScriptReference(scriptManager, scriptReference);

            //scriptReference = new ScriptReference("ReorderList.DraggableListItemBehavior.js", "AjaxControlToolkit");
            //AddNamedScriptReference(scriptManager, scriptReference);

            //scriptReference = new ScriptReference("ReorderList.DropWatcherBehavior.js", "AjaxControlToolkit");
            //AddNamedScriptReference(scriptManager, scriptReference);




        }

        private void SetupJqueryScriptManagerReference(ScriptManager scriptManager)
        {

            if (scriptManager == null) { return; }

            ScriptReference scriptReference = new ScriptReference("jquery", "");
            AddBundleScriptReference(scriptManager, scriptReference);

        }

        private void SetupJqueryUIScriptManagerReference(ScriptManager scriptManager)
        {

            if (scriptManager == null) { return; }

            ScriptReference scriptReference = new ScriptReference("jquery.ui.combined", "");
            AddBundleScriptReference(scriptManager, scriptReference);

        }

        private void AddBundleScriptReference(ScriptManager scriptManager, ScriptReference scriptReference)
        {
            if (scriptManager == null) { return; }
            if (scriptReference == null) { return; }
           
            scriptManager.Scripts.Add(scriptReference);
        }

        private void AddNamedScriptReference(ScriptManager scriptManager, ScriptReference scriptReference)
        {
            if (scriptManager == null) { return; }
            if (scriptReference == null) { return; }
            //if (scriptManager.CompositeScript.Scripts.Contains(scriptReference)) { return; }
            foreach (ScriptReference s in scriptManager.CompositeScript.Scripts)
            {
                if (s.Name == scriptReference.Name) { return; }
            }

            scriptManager.CompositeScript.Scripts.Add(scriptReference);
        }

        private void AddPathScriptReference(ScriptReference scriptReference)
        {
            AddPathScriptReference(scriptManager, scriptReference);
        }

        private void AddPathScriptReference(ScriptManager scriptManager, ScriptReference scriptReference)
        {
            if (scriptManager == null) { return; }
            if (scriptReference == null) { return; }
            //if (scriptManager.CompositeScript.Scripts.Contains(scriptReference)) { return; }
            foreach (ScriptReference s in scriptManager.CompositeScript.Scripts)
            {
                if (s.Path == scriptReference.Path) { return; }
            }

            scriptManager.CompositeScript.Scripts.Add(scriptReference);
        }

        /// <summary>
        /// should be in the format ~/path/to/yourscript.js
        /// </summary>
        /// <param name="scriptRelativeUrl"></param>
        public void AddPathScriptReference(string scriptRelativeUrl)
        {
            ScriptReference script = new ScriptReference();
            script.Path = scriptRelativeUrl;
            AddPathScriptReference(scriptManager, script);

        }

        
        #region jQuery

        private string GetJQueryBasePath()
        {
            if (WebConfigSettings.UseGoogleCDN)
            {
                return WebConfigSettings.GoogleCDNJQueryBaseUrl + WebConfigSettings.GoogleCDNjQueryVersion + "/";
            }
            return Page.ResolveUrl(WebConfigSettings.jQueryBasePath);

        }

        private void SetupJQuery()
        {
            if (renderjQueryInHead) { return; }
            if (assumejQueryIsLoaded) { return; }

            string jqueryBasePath = GetJQueryBasePath();

            if (WebConfigSettings.UseGoogleCDN)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                "jquery", "\n<script src=\""
                + jqueryBasePath + "jquery.min.js" + "\" type=\"text/javascript\" ></script>");

                
           
            }
            else
            {
                if (combineScriptsWithScriptManager)
                {
                    ScriptReference script = new ScriptReference();
                    script.Path = jqueryBasePath + "jquery.min.js";
                    AddPathScriptReference(scriptManager, script);
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "jquery", "\n<script src=\""
                    + jqueryBasePath + "jquery.min.js" + "\" type=\"text/javascript\" ></script>");
                }
            }

            
        }

        private string GetJQueryUIBasePath()
        {

            if (WebConfigSettings.UseGoogleCDN)
            {
                return WebConfigSettings.GoogleCDNJQueryUIBaseUrl + WebConfigSettings.GoogleCDNjQueryUIVersion + "/";
            }

            return Page.ResolveUrl(WebConfigSettings.jQueryUIBasePath);

        }

        private void SetupJQueryUICore()
        {
            if (renderjQueryInHead) { return; }

            string jqueryUIBasePath = GetJQueryUIBasePath();

            if (includejQueryUICore && !assumejQueryUiIsLoaded)
            {
                if (WebConfigSettings.UseGoogleCDN)
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "jqueryui-core", "\n<script src=\""
                    + jqueryUIBasePath + "jquery-ui.min.js" + "\" type=\"text/javascript\"></script>");
                }
                else
                {
                    if (combineScriptsWithScriptManager)
                    {
                        ScriptReference script = new ScriptReference();
                        script.Path = jqueryUIBasePath + "jquery-ui.min.js";
                        AddPathScriptReference(scriptManager, script);
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                        "jqueryui-core", "\n<script src=\""
                        + jqueryUIBasePath + "jquery-ui.min.js" + "\" type=\"text/javascript\"></script>");
                    }
                }
            }


        }

        private void SetupAspTreeView()
        {
           
            if (combineScriptsWithScriptManager)
            {
                ScriptReference script = new ScriptReference();
                script.Path = "~/ClientScript/CSSFriendly/AdapterUtils.min.js";
                AddPathScriptReference(scriptManager, script);

                script = new ScriptReference();
                script.Path = "~/ClientScript/CSSFriendly/TreeViewAdapter.min.js";
                AddPathScriptReference(scriptManager, script);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                        "cssadapterutils", "\n<script src=\""
                        + Page.ResolveUrl("~/ClientScript/CSSFriendly/AdapterUtils.min.js") + "\" type=\"text/javascript\"></script>");

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                        "treeviewadapter", "\n<script src=\""
                        + Page.ResolveUrl("~/ClientScript/CSSFriendly/TreeViewAdapter.min.js") + "\" type=\"text/javascript\"></script>");
            }
        }

        private void SetupAspMenu()
        {
          
            if (combineScriptsWithScriptManager)
            {
                ScriptReference script = new ScriptReference();
                script.Path = "~/ClientScript/CSSFriendly/AdapterUtils.min.js";
                AddPathScriptReference(scriptManager, script);

                script = new ScriptReference();
                script.Path = "~/ClientScript/CSSFriendly/MenuAdapter.min.js";
                AddPathScriptReference(scriptManager, script);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                        "cssadapterutils", "\n<script src=\""
                        + Page.ResolveUrl("~/ClientScript/CSSFriendly/AdapterUtils.min.js") + "\" type=\"text/javascript\"></script>");

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                        "aspmenuadapter", "\n<script src=\""
                        + Page.ResolveUrl("~/ClientScript/CSSFriendly/MenuAdapter.min.js") + "\" type=\"text/javascript\"></script>");
            }
        }

        private void SetupjQueryCycle()
        {
            if (WebConfigSettings.DisablejQueryUI) { return; }

            //jquery.cycle.all.min

            //Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
            //        "jqcycle", "\n<script src=\""
            //        + Page.ResolveUrl("~/ClientScript/jqmojo/cycle.js") + "\" type=\"text/javascript\"></script>");

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "jqcycle", "\n<script src=\""
                    + Page.ResolveUrl("~/ClientScript/jqmojo/jquery.cycle.all.min.js") + "\" type=\"text/javascript\"></script>");
            
        }

        private void SetupNivoSlider()
        {
            if (WebConfigSettings.DisablejQueryUI) { return; }

            ScriptManager.RegisterClientScriptBlock(this, typeof(Page),
                    "nivoslidermain", "\n<script src=\""
                    + Page.ResolveUrl("~/ClientScript/jqmojo/jquery.nivo.slider.pack3-2.js") + "\" type=\"text/javascript\"></script>", false);
        }

        private string mediaElementScriptPath = "~/ClientScript/mediaelement2-13-1/mediaelement-and-player.min.js";

        public string MediaElementScriptPath
        {
            get { return mediaElementScriptPath; }
            set { mediaElementScriptPath = value; }
        }

        //http://mediaelementjs.com/
        private void SetupMediaElement()
        {
            if (WebConfigSettings.DisablejQuery) { return; }

            ScriptManager.RegisterClientScriptBlock(this, typeof(Page),
                    "mediaelementmain", "\n<script src=\""
                    + Page.ResolveUrl(mediaElementScriptPath) + "\" type=\"text/javascript\"></script>", false);
        }

        private void SetupJQueryTmpl()
        {
            if (WebConfigSettings.DisablejQueryUI) { return; }

            if (combineScriptsWithScriptManager)
            {
                ScriptReference script = new ScriptReference();
                script.Path = "~/ClientScript/jqmojo/jquery.tmpl.js";
                AddPathScriptReference(scriptManager, script);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                        "jqtmpl", "\n<script src=\""
                        + Page.ResolveUrl("~/ClientScript/jqmojo/jquery.tmpl.js") + "\" type=\"text/javascript\"></script>");
            }
        }

        private void SetupJQueryImageFit()
        {
            if (WebConfigSettings.DisablejQueryUI) { return; }

            if (combineScriptsWithScriptManager)
            {
                ScriptReference script = new ScriptReference();
                script.Path = "~/ClientScript/jqmojo/jquery.imagefit.min.js";
                AddPathScriptReference(scriptManager, script);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                        "jqimagefit", "\n<script src=\""
                        + Page.ResolveUrl("~/ClientScript/jqmojo/jquery.imagefit.min.js") + "\" type=\"text/javascript\"></script>");
            }
        }

        private string knockouJsPath = "~/ClientScript/jqmojo/knockout-1.2.0.js";

        public string KnockouJsPath
        {
            get { return knockouJsPath; }
            set { knockouJsPath = value; }
        }


        private void SetupKnockoutJs()
        {
            if (WebConfigSettings.DisablejQueryUI) { return; }

            if (combineScriptsWithScriptManager)
            {
                ScriptReference script = new ScriptReference();
                script.Path = knockouJsPath;
                AddPathScriptReference(scriptManager, script);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                        "knockoutjs", "\n<script src=\""
                        + Page.ResolveUrl(knockouJsPath) + "\" type=\"text/javascript\"></script>");
            }
        }

        private void SetupJQueryValidate()
        {
            if (combineScriptsWithScriptManager)
            {
                ScriptReference script = new ScriptReference();
                script.Path = "~/ClientScript/jqmojo/jquery.validate.min.js";
                AddPathScriptReference(scriptManager, script);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                         "jqvalidate", "\n<script src=\""
                         + Page.ResolveUrl("~/ClientScript/jqmojo/jquery.validate.min.js") + "\" type=\"text/javascript\"></script>");
            }


        }

        private void SetupQtFile()
        {
            CultureInfo defaultCulture = SiteUtils.GetDefaultUICulture();

            bool loadedQtLangFile = false;
            if (defaultCulture.TwoLetterISOLanguageName != "en")
            {
                if (File.Exists(HostingEnvironment.MapPath("~/ClientScript/jqmojo/" + defaultCulture.TwoLetterISOLanguageName + ".qtfile.js")))
                {
                    if (combineScriptsWithScriptManager)
                    {
                        ScriptReference script = new ScriptReference();
                        script.Path = "~/ClientScript/jqmojo/" + defaultCulture.TwoLetterISOLanguageName + ".qtfile.js";
                        AddPathScriptReference(scriptManager, script);
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                         "qtfilelocalize", "\n<script src=\""
                         + Page.ResolveUrl("~/ClientScript/jqmojo/" + defaultCulture.TwoLetterISOLanguageName + ".qtfile.js") + "\" type=\"text/javascript\"></script>");
                    }
                    loadedQtLangFile = true;
                }
            }

            if (!loadedQtLangFile)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                     "qtfilelocalize", "\n<script src=\""
                     + Page.ResolveUrl("~/ClientScript/jqmojo/en.qtfile.js") + "\" type=\"text/javascript\"></script>");
            }

            //if (combineScriptsWithScriptManager)
            //{
            //    ScriptReference script = new ScriptReference();
            //    script.Path = "~/ClientScript/jqmojo/mojoqtfilev2.js";
            //    AddPathScriptReference(scriptManager, script);
            //}
            //else
            //{

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                     "qtfile", "\n<script src=\""
                     + Page.ResolveUrl("~/ClientScript/jqmojo/mojoqtfilev2.js") + "\" type=\"text/javascript\"></script>");
            //}
        }

        

        private void SetupFlickrGallery()
        {
            
            if (useMobileVersionOfFlickrGallery)
            {
                // only difference is the positioning of the lightbox
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                        "jqflickrgal", "\n<script src=\""
                        + Page.ResolveUrl("~/ClientScript/jqueryflickr/inc/flickrGallery-mobile-min.js") + "\" type=\"text/javascript\"></script>");
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                         "jqflickrgal", "\n<script src=\""
                         + Page.ResolveUrl("~/ClientScript/jqueryflickr/inc/flickrGallery-min.js") + "\" type=\"text/javascript\"></script>");
            }

        }

        //private void SetupJqueryLayout()
        //{
        //    Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
        //                 "jqlayout", "\n<script src=\""
        //                 + Page.ResolveUrl("~/ClientScript/jqmojo/jquery.layout.min.js") + "\" type=\"text/javascript\"></script>");
        //}

        private void SetupSizzle()
        {
            if (combineScriptsWithScriptManager)
            {
                ScriptReference script = new ScriptReference();
                script.Path = "~/ClientScript/jqmojo/sizzle.js";
                AddPathScriptReference(scriptManager, script);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                         "sizzle", "\n<script src=\""
                         + Page.ResolveUrl("~/ClientScript/jqmojo/sizzle.js") + "\" type=\"text/javascript\"></script>");
            }


        }

        private void SetupImpromtu()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                            "jqprompt", "\n<script src=\""
                            + Page.ResolveUrl("~/ClientScript/jqmojo/jquery-impromptu42.min.js") + "\" type=\"text/javascript\"></script>");
        }

        private void SetupColorBox()
        {
            if (combineScriptsWithScriptManager)
            {
                ScriptReference script = new ScriptReference();
                script.Path = "~/ClientScript/colorbox/jquery.colorbox-min.js";
                AddPathScriptReference(scriptManager, script);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                         "jqcolorbox", "\n<script src=\""
                         + Page.ResolveUrl("~/ClientScript/colorbox/jquery.colorbox-min.js") + "\" type=\"text/javascript\"></script>");
            }

        }

        private void SetupScroller()
        {
            if (combineScriptsWithScriptManager)
            {
                ScriptReference script = new ScriptReference();
                script.Path = "~/ClientScript/jqmojo/jquery.scroller.min.js";
                AddPathScriptReference(scriptManager, script);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                         "jqscroller", "\n<script src=\""
                         + Page.ResolveUrl("~/ClientScript/jqmojo/jquery.scroller.min.js") + "\" type=\"text/javascript\"></script>");
            }

        }

        private bool includejPlayerPlaylist = false;

        public bool IncludejPlayerPlaylist
        {
            get { return includejPlayerPlaylist; }
            set
            {
                includejPlayerPlaylist = value;
                if (includejPlayerPlaylist)
                {
                    includeJQuery = true;
                    includejPlayer = true;
                    includeJQueryMigrate = true;
                }

            }
        }

       

        private void SetupjPlayer()
        {
            //Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
            //         "jplayer", "\n<script src=\""
            //         + Page.ResolveUrl("~/ClientScript/jplayer/jquery.jplayer.min.js") + "\" type=\"text/javascript\"></script>");

            //if (combineScriptsWithScriptManager)
            //{
            //    ScriptReference script = new ScriptReference();
            //    script.Path = "~/ClientScript/jplayer-2-1-0/jquery.jplayer.min.js";
            //    AddPathScriptReference(scriptManager, script);

            //    if (includejPlayerPlaylist)
            //    {
            //        ScriptReference scriptPl = new ScriptReference();
            //        script.Path = "~/ClientScript/jplayer-2-1-0/add-on/jplayer.playlist.min.js";
            //        AddPathScriptReference(scriptManager, scriptPl);
            //    }
            //}
            //else
            //{

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                         "jplayer", "\n<script src=\""
                         + Page.ResolveUrl(WebConfigSettings.JPlayerBasePath + "jquery.jplayer.min.js") + "\" type=\"text/javascript\"></script>");

                if (includejPlayerPlaylist)
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                         "jplayer-playlist", "\n<script src=\""
                         + Page.ResolveUrl(WebConfigSettings.JPlayerBasePath + "add-on/jplayer.playlist.min.js") + "\" type=\"text/javascript\"></script>");
                }
            //}
        }

       

        //private void SetupJQueryFileTree()
        //{

        //    Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
        //             "jqfiletree", "\n<script src=\""
        //             + Page.ResolveUrl("~/ClientScript/jqueryFileTree/jqueryFileTree.js") + "\" type=\"text/javascript\"></script>");


        //}



        
        

        //private void SetupJQueryUI()
        //{
        //    if (!renderjQueryInHead)
        //    {
        //        string jqueryUIBasePath = GetJQueryUIBasePath();

        //        if (includejQueryUICore && !assumejQueryUiIsLoaded)
        //        {
        //            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
        //            "jqueryui-core", "\n<script src=\""
        //            + jqueryUIBasePath + "jquery-ui.min.js" + "\" type=\"text/javascript\"></script>");
        //        }
        //    }

            

        //}

        // TODO when jQueryUI ships it will have a built in tooltip so we can get rid of this/
        private void SetupClueTip()
        {
            if (WebConfigSettings.DisablejQueryUI) { return; }

            if (combineScriptsWithScriptManager)
            {
                ScriptReference script = new ScriptReference();
                script.Path = "~/ClientScript/jqmojo/jquery.cluetip.min.js";
                AddPathScriptReference(scriptManager, script);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                        "cluetip", "\n<script src=\""
                        + Page.ResolveUrl("~/ClientScript/jqmojo/jquery.cluetip.min.js") + "\" type=\"text/javascript\"></script>");
            }

            string initAutoScript = "$(document).ready(function () { $('a.cluetiplink').cluetip({attribute:'href', topOffset:25, leftOffset:25}); }); ";

            Page.ClientScript.RegisterStartupScript(typeof(Page),
                   "cluetip-init", "\n<script type=\"text/javascript\" >"
                   + initAutoScript + "</script>");


        }

        private void SetupMarkitUpHtml()
        {
            if (assumeMarkitupIsLoaded) { return; }

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                        "markitup", "\n<script src=\""
                        + Page.ResolveUrl("~/ClientScript/markitup/jquery.markitup-html.js") + "\" type=\"text/javascript\"></script>");

           
            string initAutoScript = "$(document).ready(function () { $('textarea.markituphtml').markItUp(htmlSettings); }); ";

            Page.ClientScript.RegisterStartupScript(typeof(Page),
                    "markituphtml-init", "\n<script type=\"text/javascript\" >"
                    + initAutoScript + "</script>");
            
            
        }

        #endregion



        
        #region swfobject

        private string GetSwfObjectUrl()
        {
            string version = "2.2";
            if (ConfigurationManager.AppSettings["GoogleCDNSwfObjectVersion"] != null)
            {
                version = ConfigurationManager.AppSettings["GoogleCDNSwfObjectVersion"];
            }

            if (WebConfigSettings.UseGoogleCDN)
            {
                return protocol + "://ajax.googleapis.com/ajax/libs/swfobject/" + version + "/swfobject.js";
            }

            if (ConfigurationManager.AppSettings["SwfObjectUrl"] != null)
            {
                string surl = ConfigurationManager.AppSettings["SwfObjectUrl"];
                return Page.ResolveUrl(surl);

            }

            return string.Empty;
        }

        private void SetupSwfObject()
        {
            if (HttpContext.Current == null) { return; }
            if (HttpContext.Current.Request == null) { return; }
            if (WebConfigSettings.DisableSwfObject) { return; }

            //http://ajax.googleapis.com/ajax/libs/swfobject/2.2/swfobject.js

            string swfUrl = GetSwfObjectUrl();
            if (swfUrl.Length > 0)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "swfobject", "\n<script src=\"" + swfUrl + "\" type=\"text/javascript\" ></script>");
            }


        }


        #endregion

        private void SetupWebSnapr()
        {
            if (HttpContext.Current == null) { return; }
            if (HttpContext.Current.Request == null) { return; }

            // this script doesn't support https as far as I know
            if (SiteUtils.IsSecureRequest()) { return; }

            //Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
            //"websnapr", "\n<script src=\"http://bubble.websnapr.com/"
            //+ webSnaprKey + "/swh/" + "\" type=\"text/javascript\" ></script>");

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
            "websnapr", "\n<script src=\"http://www.websnapr.com/js/websnapr.js"
            + "\" type=\"text/javascript\" ></script>");

        }

        private void SetupYahooMediaPlayer()
        {
            if (HttpContext.Current == null) { return; }
            if (HttpContext.Current.Request == null) { return; }
            if (SiteUtils.IsSecureRequest()) { return; } //https not supported

            //yahoo media player doesn't seem to work on localhost so usethe delicious one
            if (HttpContext.Current.Request.Url.ToString().Contains("localhost"))
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                "dmedia", "\n<script type=\"text/javascript\" src=\"http://static.delicious.com/js/playtagger.js\"></script>\n");
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                "yahoomedia", "\n<script type=\"text/javascript\" src=\"http://mediaplayer.yahoo.com/js\"></script>\n");
            }

            //<script type="text/javascript" src="http://static.delicious.com/js/playtagger.js"></script>


        }

        private void SetupGreyBox()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "gbVar", "\n<script  type=\"text/javascript\">"
                    + "var GB_ROOT_DIR = '" + Page.ResolveUrl("~/ClientScript/greybox/") + "'; var GBCloseText = '" + Resource.CloseDialogButton + "';" + " </script>");

            //Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
            //        "GreyBoxJs", "\n<script  src=\""
            //        + Page.ResolveUrl(scriptBaseUrl + "gbcombined.js") + "\" type=\"text/javascript\" ></script>");

            //The commented version above is the preferred syntax, the uncommented one below is the deprecated version.
            //There is a reason we are using the deprecated version, greybox is registered also by NeatUpload using the deprecated
            //syntax, the reason they use the older syntax is because they also support .NET v1.1
            //We use the old syntax here for compatibility with NeatUpload so that it does not get registered more than once
            // on pages that use NeatUpload. Otherwise we would have to always modify our copy of NeatUpload.
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),"GreyBoxJs", "\n<script  src=\""
                    + Page.ResolveUrl("~/ClientScript/greybox/gbcombined.js") + "\" type=\"text/javascript\" ></script>");
        }

        

        private void SetupmojoCombined()
        {
            if (renderCombinedInHead) { return; }
            
            if (!assumeMojoCombinedIsLoaded)
            {
                if (combineScriptsWithScriptManager)
                {
                    ScriptReference script = new ScriptReference();
                    script.Path = "~/ClientScript" + mojoCombinedFullScript;
                    AddPathScriptReference(scriptManager, script);
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                        "mojocombined", "\n<script  src=\""
                        + Page.ResolveUrl("~/ClientScript" + mojoCombinedFullScript)
                        + "\" type=\"text/javascript\" ></script>");
                }
            }
            

            //usedMojoCombinedFull = true;

        }

        


        private void SetupGoogleAjax()
        {
            if ((!includeGoogleMaps) && (!includeGoogleSearch)) { return; }
            string googleApiKey = SiteUtils.GetGmapApiKey();
            //if (string.IsNullOrEmpty(googleApiKey)) { return; }

            if (googleApiKey.Length > 0)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                        "gajaxmain", "\n<script  src=\"" + protocol + "://www.google.com/jsapi?key=" + googleApiKey + "\" type=\"text/javascript\" ></script>");
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                        "gajaxmain", "\n<script  src=\"" + protocol + "://www.google.com/jsapi"  + "\" type=\"text/javascript\" ></script>");
            }

            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\">\n");

            if (includeGoogleMaps)
            {
                script.Append("google.load(\"maps\", \"2\");");
            }
            if (includeGoogleSearch)
            {
                script.Append("google.load(\"search\", \"1\", {language : '" + languageCode + "'});");
            }

            if ((includeGoogleGeoLocator) && (Page.Request.IsAuthenticated))
            {
                script.Append("\n function trackLocation() { ");
                script.Append("var location = google.loader.ClientLocation; ");
                // this method is in mojocombined.js
                script.Append("if(location != null){ trackUserLocation(location); }");
                //script.Append("else{alert('location was null'); }");

                script.Append("}");

                script.Append("google.setOnLoadCallback(trackLocation);");
            }

            script.Append("\n</script>");


            Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "gloader",
                script.ToString());


        }



        private void SetupBrowserSpecificScripts()
        {
#if !MONO
            string loweredBrowser = string.Empty;

            if (HttpContext.Current.Request.UserAgent != null)
            {
                loweredBrowser = HttpContext.Current.Request.UserAgent.ToLower();
            }

            if (WebConfigSettings.UseSafariWebKitHack)
            {
                if (loweredBrowser.Contains("webkit"))
                {
                    //this fixes some ajax updatepanel issues in webkit
                    //http://forums.asp.net/p/1252014/2392110.aspx
                    try
                    {
                        ScriptReference scriptReference = new ScriptReference();
                        scriptReference.Path = Page.ResolveUrl("~/ClientScript/AjaxWebKitFix.js");
                        ScriptManager ajax = ScriptManager.GetCurrent(Page);
                        if (ajax != null)
                        {
                            ajax.Scripts.Add(scriptReference);
                        }
                    }
                    catch (TypeLoadException)
                    { }// this can happen if SP1 is not installed for .NET 3.5
                }
            }
#endif

        }


        #region YUI

        private string GetYuiBasePath()
        {
            string yuiVersion = "2.6.0";

            if (ConfigurationManager.AppSettings["GoogleCDNYUIVersion"] != null)
            {
                yuiVersion = ConfigurationManager.AppSettings["GoogleCDNYUIVersion"];
            }

            if (WebConfigSettings.UseGoogleCDN)
            {
                return protocol + "://ajax.googleapis.com/ajax/libs/yui/" + yuiVersion + "/build/";
            }

            if (ConfigurationManager.AppSettings["YUIBasePath"] != null)
            {
                string yuiBasePath = ConfigurationManager.AppSettings["YUIBasePath"];
                return Page.ResolveUrl(yuiBasePath);
            }

            return string.Empty;
        }


        private void SetupYui()
        {

            string scriptBaseUrl = GetYuiBasePath();
            string yuiAddOnBaseUrl = Page.ResolveUrl("~/ClientScript/yuiaddons/");
            if (ConfigurationManager.AppSettings["YUIAddOnsBasePath"] != null)
            {
                yuiAddOnBaseUrl = Page.ResolveUrl(ConfigurationManager.AppSettings["YUIAddOnsBasePath"]);
            }

            if (includeYuiDom)
            {

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-utilities", "\n<script src=\""
                    + scriptBaseUrl + "utilities/utilities.js" + "\" type=\"text/javascript\"></script>");
            }

            //if (includeYuiLayout)
            //{
            //    Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
            //        "yui-container-js", "\n<script src=\""
            //        + scriptBaseUrl + "container/container-min.js" + "\" type=\"text/javascript\"></script>");

            //    Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
            //        "yui-resize-js", "\n<script src=\""
            //        + scriptBaseUrl + "resize/resize-min.js" + "\" type=\"text/javascript\"></script>");

            //    Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
            //        "yui-layout-js", "\n<script src=\""
            //        + scriptBaseUrl + "layout/layout-min.js" + "\" type=\"text/javascript\"></script>");

            //}

            if (includeSlider)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-slider", "\n<script src=\""
                    + scriptBaseUrl + "slider/slider-min.js" + "\" type=\"text/javascript\"></script>");
            }

            if (includeColorPicker)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-colorpicker", "\n<script src=\""
                    + scriptBaseUrl + "colorpicker/colorpicker-min.js" + "\" type=\"text/javascript\"></script>");

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "dd-window", "\n<script src=\""
                    + Page.ResolveUrl("~/ClientScript/ddwindow/dhtmlwindow.js") + "\" type=\"text/javascript\"></script>");

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "dd-colorpicker", "\n<script src=\""
                    + Page.ResolveUrl("~/ClientScript/ddcolorpicker.js") + "\" type=\"text/javascript\"></script>");
            }

            

            if (includeYuiTabs)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-tabview", "\n<script src=\""
                    + scriptBaseUrl + "tabview/tabview-min.js" + "\" type=\"text/javascript\"></script>");

                string initTabsScript = "$('div.yui-navset').each(function(n){var myTabs = new YAHOO.widget.TabView(this);})";

                Page.ClientScript.RegisterStartupScript(typeof(Page),
                   "yui-tabinit", "\n<script type=\"text/javascript\" >"
                   + initTabsScript + "</script>");
            }

            if (includeYuiAccordion)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "yui-accordionview", "\n<script src=\""
                    + yuiAddOnBaseUrl + "accordionview/accordionview-min.js" + "\" type=\"text/javascript\"></script>");

            }

            //if (includeYuiTreeView)
            //{
            //    Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
            //        "yui-treeview-js", "\n<script src=\""
            //        + scriptBaseUrl + "treeview/treeview-min.js" + "\" type=\"text/javascript\"></script>");
            //}

            //if (includeYuiMenu)
            //{
            //    Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
            //        "yui-menu-js", "\n<script src=\""
            //        + scriptBaseUrl + "menu/menu-min.js" + "\" type=\"text/javascript\"></script>");
            //}

        }

        #endregion


    }
}
