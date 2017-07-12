///		Author:			
///		Created:		2008-10-31
///		Last Modified:	2014-01-08
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// This control can (and should) be used instead of StyleSheet.ascx
    /// it will use a handler url, the handler will combine the css files in the order they are listed in
    /// the css.config file located in the skin folder
    /// It will also remove white space (and comments?)
    /// This can improve performance as measured using the YSlow Firefox plugin
    /// </summary>
    public partial class StyleSheetCombiner : UserControl
    {
        private bool loadSkinCss = true;
        private bool allowPageOverride = false;
        private string skinBaseUrl = string.Empty;
        private bool includeColorPickerCss = false;
        private string overrideSkinName = string.Empty;
        private string protocol = "http";

        private bool includeYuiLayout = false;
        private bool includeYuiReset = false;
        private bool useFullYuiSam = false;
        private bool includeYuiAccordion = false;
        private bool includeYuiTreeView = false;
        private string treeViewStyle = string.Empty;
        private bool includeYuiMenu = false;
        private bool includejQueryUI = true;
        private string jQueryUIThemeName = "smoothness";
        private bool includeYuiTabs = false;

        private bool includejCrop = false;


        /// <summary>
        /// this is just a flag used within specific features to determine if schema.org attributes can be added
        /// it should always be fine in HTML 5 doctype but
        /// may cause validation errors for XHTML doctypes
        /// so, you have the option to disable it to fix w3c validation if using xhtml
        /// but at the expense of SEO.
        /// </summary>
        private bool useSchemaDotOrgFormats = true;

        public bool UseSchemaDotOrgFormats
        {
            get { return useSchemaDotOrgFormats; }
            set { useSchemaDotOrgFormats = value; }
        }

        private bool addBodyClassForLiveWriter = true;

        public bool AddBodyClassForLiveWriter
        {
            get { return addBodyClassForLiveWriter; }
            set { addBodyClassForLiveWriter = value; }
        }

        private bool addBodyClassForiPad = false;

        public bool AddBodyClassForiPad
        {
            get { return addBodyClassForiPad; }
            set { addBodyClassForiPad = value; }
        }

        private bool useIconsForAdminLinks = true;
        /// <summary>
        /// this property is not used directly by this control but the base page and cms page detect ths setting and use it
        /// so it allows configuring this per skin
        /// </summary>
        public bool UseIconsForAdminLinks
        {
            get { return useIconsForAdminLinks; }
            set { useIconsForAdminLinks = value; }
        }

        private bool useJQueryMobile = false;

        public bool UseJQueryMobile
        {
            get { return useJQueryMobile; }
            set { useJQueryMobile = value; }
        }


        private bool usingjQueryHintsOnRegisterPage = false;
        /// <summary>
        /// set this to true if you are including the css from /Data/style/formvalidation in your style.config
        /// this flag is chacked by code in the register.aspx page and if true then it wires up the script for jquery validation
        /// </summary>
        public bool UsingjQueryHintsOnRegisterPage
        {
            get { return usingjQueryHintsOnRegisterPage; }
            set { usingjQueryHintsOnRegisterPage = value; }
        }

        private bool useTextLinksForFeatureSettings = true;
        /// <summary>
        /// this property is not used directly by this control but the base page and cms page detect ths setting and use it
        /// so it allows configuring this per skin
        /// </summary>
        public bool UseTextLinksForFeatureSettings
        {
            get { return useTextLinksForFeatureSettings; }
            set { useTextLinksForFeatureSettings = value; }
        }

        private bool siteMapPopulateOnDemand = false;
        /// <summary>
        /// this property is not used directly by this control but the base page and cms page detect ths setting and use it
        /// so it allows configuring this per skin
        /// </summary>
        public bool SiteMapPopulateOnDemand
        {
            get { return siteMapPopulateOnDemand; }
            set { siteMapPopulateOnDemand = value; }
        }

        private int siteMapExpandDepth = -1;
        /// <summary>
        /// this property is not used directly by this control but the base page and cms page detect ths setting and use it
        /// so it allows configuring this per skin
        /// the default -1 means fully expanded, when using SiteMapPopulateOnDemand=true you would typically set this to 0
        /// </summary>
        public int SiteMapExpandDepth
        {
            get { return siteMapExpandDepth; }
            set { siteMapExpandDepth = value; }
        }


        /// <summary>
        /// valid options are: base, black-tie, blitzer, cupertino, dot-luv, excite-bike, hot-sneaks, humanity, mint-choc,
        /// redmond, smoothness, south-street, start, swanky-purse, trontastic, ui-darkness, ui-lightness, vader
        /// </summary>
        public string JQueryUIThemeName
        {
            get { return jQueryUIThemeName; }
            set { jQueryUIThemeName = value; }
        }

        public bool IncludejQueryUI
        {
            get { return includejQueryUI; }
            set { includejQueryUI = value; }
        }

        public bool IncludejCrop
        {
            get { return includejCrop; }
            set { includejCrop = value; }
        }

        public bool AllowPageOverride
        {
            get { return allowPageOverride; }
            set { allowPageOverride = value; }
        }

        public bool LoadSkinCss
        {
            get { return loadSkinCss; }
            set { loadSkinCss = value; }
        }

        //public string CSSConfigFile
        //{
        //    get { return cssConfigFile; }
        //    set { cssConfigFile = value; }
        //}

        public string OverrideSkinName
        {
            get { return overrideSkinName; }
            set { overrideSkinName = value; }
        }

        public bool IncludeColorPickerCss
        {
            get { return includeColorPickerCss; }
            set { includeColorPickerCss = value; }
        }

        public bool IncludeYuiLayout
        {
            get { return includeYuiLayout; }
            set { includeYuiLayout = value; }
        }

        public bool IncludeYuiReset
        {
            get { return includeYuiReset; }
            set { includeYuiReset = value; }
        }

        public bool IncludeYuiTabs
        {
            get { return includeYuiTabs; }
            set { includeYuiTabs = value; }
        }

        public bool IncludeYuiAccordion
        {
            get { return includeYuiAccordion; }
            set { includeYuiAccordion = value; }
        }

        public bool IncludeYuiTreeView
        {
            get { return includeYuiTreeView; }
            set { includeYuiTreeView = value; }
        }

        /// <summary>
        /// valid options are empty for default, "folders", and "menu"
        /// </summary>
        public string TreeViewStyle
        {
            get { return treeViewStyle; }
            set { treeViewStyle = value; }
        }

        public bool IncludeYuiMenu
        {
            get { return includeYuiMenu; }
            set { includeYuiMenu = value; }
        }

        private bool includeTwitterCss = false;
        public bool IncludeTwitterCss
        {
            get { return includeTwitterCss; }
            set { includeTwitterCss = value; }
        }

        private bool includeGreyBoxCss = false;
        public bool IncludeGreyBoxCss
        {
            get { return includeGreyBoxCss; }
            set { includeGreyBoxCss = value; }
        }

        private bool alwaysShowLeftColumn = false;
        public bool AlwaysShowLeftColumn
        {
            get { return alwaysShowLeftColumn; }
            set { alwaysShowLeftColumn = value; }
        }

        private bool alwaysShowRightColumn = false;
        public bool AlwaysShowRightColumn
        {
            get { return alwaysShowRightColumn; }
            set { alwaysShowRightColumn = value; }
        }

        private bool includeGoogleCustomSearchCss = false;
        public bool IncludeGoogleCustomSearchCss
        {
            get { return includeGoogleCustomSearchCss; }
            set { includeGoogleCustomSearchCss = value; }
        }

        private bool disableCssHandler = false;

        public bool DisableCssHandler
        {
            get { return disableCssHandler; }
            set { disableCssHandler = value; }
        }

        #region Property Bag settings stored here but not used in this control

        private bool enableNonClickablePageLinks = true;
        /// <summary>
        /// it is possible for dynamic menus like jQuery superfish to make unclickable menu links
        /// the menu controls will check this setting to determine whether to enable it
        /// the property is stored here just to make it controlled by the skin since 
        /// other menus don't makesense with unclickable links
        /// doesn't work for TreeView
        /// 2014-01-08 changed the default from false to to true because all newer skins use FlexMenu
        /// which does support it, TreeView is now legacy
        /// </summary>
        public bool EnableNonClickablePageLinks
        {
            get { return enableNonClickablePageLinks; }
            set { enableNonClickablePageLinks = value; }
        }

        private bool useMenuTooltipForCustomCss = true;
        /// <summary>
        /// there are no good ways to expand MenuItem with additional properties so we are using a property for something other than its intended purposes
        /// the MenuAdapterArtisteer is used to override the rendering and there we can use the tooltip property as a way to add a custom css class to soecific menu items.
        /// Admittedly an ugly solution but no other solutions seem feasible
        /// </summary>
        public bool UseMenuTooltipForCustomCss
        {
            get { return useMenuTooltipForCustomCss; }
            set { useMenuTooltipForCustomCss = value; }
        }

        private bool useArtisteer3 = false;

        public bool UseArtisteer3
        {
            get { return useArtisteer3; }
            set { useArtisteer3 = value; }
        }

        private bool hideEmptyAlt1 = true;

        public bool HideEmptyAlt1
        {
            get { return hideEmptyAlt1; }
            set { hideEmptyAlt1 = value; }
        }

        private bool hideEmptyAlt2 = true;

        public bool HideEmptyAlt2
        {
            get { return hideEmptyAlt2; }
            set { hideEmptyAlt2 = value; }
        }

        private bool addQtFileCss = false;

        public bool AddQtFileCss
        {
            get { return addQtFileCss; }
            set { addQtFileCss = value; }
        }

        private bool qtFileCssIsInMainCss = true;
        /// <summary>
        /// historically we have included qtfile css in the style.config
        /// so this is set to true
        /// set it to false if you want qtfile to be loaded separately only on the
        /// file manager pages where it is needed
        /// this can reduce the sixe of the main css a little
        /// </summary>
        public bool QtFileCssIsInMainCss
        {
            get { return qtFileCssIsInMainCss; }
            set { qtFileCssIsInMainCss = value; }
        }

        private string media = string.Empty;

        public string Media
        {
            get { return media; }
            set { media = value; }
        }

        #endregion

        private string FormatMedia()
        {
            if (media.Length > 0)
            {
                return " media='" + media + "' ";
            }

            return string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (WebConfigSettings.AlwaysLoadYuiTabs) { IncludeYuiTabs = true; }

            if ((addBodyClassForiPad)&&(Page is mojoBasePage))
            {
                if (BrowserHelper.IsIpad())
                {
                    mojoBasePage basePage = Page as mojoBasePage;
                    basePage.AddClassToBody("ipad");
                }
            }

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (SiteUtils.IsSecureRequest()) { protocol = "https"; }

            SetupYuiCss();

            if (includejQueryUI) 
            { 
                SetupjQueryUICss(); 
            }
            if (includejCrop) { SetupjCropCss(); }
            //if (includeGreyBoxCss) { SetupGreyBox(); }
            if (includeTwitterCss) { SetupTwitter(); }
            if (includeGoogleCustomSearchCss) { SetupGoogleSearch(); }
            if (includeMediaElement) { SetupMediaElement(); }

            if (addQtFileCss) { SetupQtFile(); }

            if (!loadSkinCss) { return; }

            if (overrideSkinName.Length > 0)
            {
                skinBaseUrl = SiteUtils.DetermineSkinBaseUrl(overrideSkinName);
                

            }
            else
            {
                //skinBaseUrl = SiteUtils.GetSkinBaseUrl(allowPageOverride, Page);
                skinBaseUrl = SiteUtils.DetermineSkinBaseUrl(allowPageOverride, false, Page); //changed to relative url

            }

            if (WebConfigSettings.CombineCSS)
            {
                SetupCombinedCssUrl();
            }
            else
            {
                AddCssLinks();
            }

            
            
        }

        

        private void SetupCombinedCssUrl()
        {
            if (disableCssHandler) { return; }
            Literal cssLink = new Literal();

            string siteRoot = string.Empty;
            if (WebConfigSettings.UseFullUrlsForSkins)
            {
                siteRoot = SiteUtils.GetNavigationSiteRoot();
            }
            else
            {
                siteRoot = SiteUtils.GetRelativeNavigationSiteRoot();
            }
            
            string siteParam = "&amp;s=-1";
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (WebConfigSettings.IncludeVersionInCssUrl)
            {
                if (siteSettings != null) { siteParam = "&amp;s=" + siteSettings.SiteId.ToInvariantString() + "&amp;v=" + Server.UrlEncode(DatabaseHelper.DBCodeVersion().ToString()) + "&amp;sv=" + siteSettings.SkinVersion; }
            }
            else
            {
                if (siteSettings != null) { siteParam = "&amp;s=" + siteSettings.SiteId.ToInvariantString() + "&amp;sv=" + siteSettings.SkinVersion; }
            }

            
            if (SiteUtils.UseMobileSkin())
            {
                if (siteSettings.MobileSkin.Length > 0)
                {
                    overrideSkinName = siteSettings.MobileSkin;
                }
                //web.config setting trumps site setting
                if (WebConfigSettings.MobilePhoneSkin.Length > 0)
                {
                    overrideSkinName = WebConfigSettings.MobilePhoneSkin;
                }
            }

           

            if (overrideSkinName.Length > 0)
            {
                cssLink.Text = "\n<link rel='stylesheet'  type='text/css'" + FormatMedia() + " href='" + siteRoot
                    + "/csshandler.ashx?skin=" + overrideSkinName + siteParam
                    + "' />\n";
            }
            else
            {
                cssLink.Text = "\n<link rel='stylesheet' type='text/css'" + FormatMedia() + " href='" + siteRoot
                    + "/csshandler.ashx?skin=" + SiteUtils.GetSkinName(allowPageOverride, Page) + siteParam
                    + "' />\n";
            }

            this.Controls.Add(cssLink);

        }


        private void SetupQtFile()
        {
            if (qtFileCssIsInMainCss) { return; }

            Literal cssLink = new Literal();
            cssLink.ID = "qtfile";
            cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='/Data/style/qtfile/default.css' />";
            Controls.Add(cssLink);

        }

        //http://mediaelementjs.com/
        private bool includeMediaElement = false;
        public bool IncludeMediaElement
        {
            get { return includeMediaElement; }
            set { includeMediaElement = value; }
        }

        private bool assumeMediaElementIsLoaded = false;
        public bool AssumeMediaElementIsLoaded
        {
            get { return assumeMediaElementIsLoaded; }
            set { assumeMediaElementIsLoaded = value; }
        }

        private string mediaElementCssPath = "~/ClientScript/mediaelement2-13-1/mediaelementplayer.min.css";

        public string MediaElementCssPath
        {
            get { return mediaElementCssPath; }
            set { mediaElementCssPath = value; }
        }

        private void SetupMediaElement()
        {
            if (assumeMediaElementIsLoaded) { return; }

            Literal cssLink = new Literal();
            cssLink.ID = "mediaelementcss";
            cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='" + Page.ResolveUrl(mediaElementCssPath) + "' />";
            Controls.Add(cssLink);

        }


        private void SetupTwitter()
        {
            Literal cssLink = new Literal();
            cssLink.ID = "twittercsss";
            cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='//widgets.twimg.com/j/1/widget.css' />";
            Controls.Add(cssLink);

        }

        private void SetupGoogleSearch()
        {
            Literal cssLink = new Literal();
            cssLink.ID = "googlesearchcsss";
            cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='" + protocol + "://www.google.com/cse/style/look/default.css' />";
            Controls.Add(cssLink);

        }

        

        private void SetupjQueryUICss()
        {
            if (WebConfigSettings.DisablejQueryUI) { return; }

            string jQueryUIBasePath;
            string jQueryUIVersion = WebConfigSettings.GoogleCDNjQueryUIVersion;

            //string jQueryCssAllName = "jquery.ui.all.css"; //this was renamed as of 1.8.0 http://blog.jqueryui.com/2010/03/jquery-ui-18/
            //if (jQueryUIVersion == "1.7.2") { jQueryCssAllName = "ui.all.css"; }
            string jQueryCssAllName = "jquery-ui.css"; //the jquery.ui.all.css uses @imports so it loads 15 separate style sheets, whereas jquery-ui.css is all in one file

            if ((WebConfigSettings.UseGoogleCDN) || (ConfigurationManager.AppSettings["jQueryUIBasePath"] == null))
            {
                jQueryUIBasePath = WebConfigSettings.GoogleCDNJQueryUIBaseUrl + jQueryUIVersion + "/";
            }
            else
            {
                jQueryUIBasePath = Page.ResolveUrl(ConfigurationManager.AppSettings["jQueryUIBasePath"]);
            }


            Literal cssLink = new Literal();
            cssLink.ID = "jqueryui-css";
            cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
            + jQueryUIBasePath + "themes/" + jQueryUIThemeName + "/" + jQueryCssAllName + "' />";
            this.Controls.Add(cssLink);


        }

        //private void SetupjQueryMobileCss()
        //{
        //   // if (WebConfigSettings.DisablejQueryUI) { return; }

        //    //TODO: make it possible to host the files locally

        //    Literal cssLink = new Literal();
        //    cssLink.ID = "jquerymobile-css";
        //    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
        //    + WebConfigSettings.JQueryMobileCssCdnUrl + "' />";
        //    this.Controls.Add(cssLink);


        //}

        private void SetupjCropCss()
        {
            Literal cssLink = new Literal();
            cssLink.ID = "jcrop-css";
            cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='" + Page.ResolveUrl("~/ClientScript/jcrop0912/jquery.Jcrop.css") + "' />";
            this.Controls.Add(cssLink);
        }

        private void AddCssLinks()
        {
            
            string configFilePath;
            if (overrideSkinName.Length > 0)
            {
                configFilePath = Server.MapPath(SiteUtils.DetermineSkinBaseUrl(SiteUtils.SanitizeSkinParam(overrideSkinName)) + "style.config");
            }
            else
            {
                configFilePath = Server.MapPath(SiteUtils.DetermineSkinBaseUrl(allowPageOverride, false, Page) + "style.config");
            }

            if (File.Exists(configFilePath))
            {
                using (XmlReader reader = new XmlTextReader(new StreamReader(configFilePath)))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if (("file" == reader.Name) && (reader.NodeType != XmlNodeType.EndElement))
                        {
                            string csswebconfigkey = reader.GetAttribute("csswebconfigkey");
                            string cssVPath = reader.GetAttribute("cssvpath");

                            if ((!string.IsNullOrEmpty(csswebconfigkey)))
                            {
                                if ((ConfigurationManager.AppSettings[csswebconfigkey] != null))
                                {
                                    AddCssLink(Page.ResolveUrl(ConfigurationManager.AppSettings[csswebconfigkey]));
                                }
                            }
                            else if ((!string.IsNullOrEmpty(cssVPath)))
                            {
                                AddCssLink(Page.ResolveUrl("~" + cssVPath));
                            }
                            else
                            {
                                string cssFile = reader.ReadElementContentAsString();
                                AddCssLink(skinBaseUrl + cssFile);

                            }
                        }
                    }
                }
            }
            else
            {
                // style.config is missing
               
                AddCssLink(skinBaseUrl + "style.css");
                //AddCssLink(skinBaseUrl + "stylecolors.css");
                //AddCssLink(skinBaseUrl + "styleimages.css");
                //AddCssLink(skinBaseUrl + "styleborders.css");
                //AddCssLink(skinBaseUrl + "style-gridview.css");
               // AddCssLink(skinBaseUrl + "styletext.css");
                AddCssLink(skinBaseUrl + "stylemenu.css");
                AddCssLink(skinBaseUrl + "styletreeview.css");
                AddCssLink(skinBaseUrl + "styleblog.css");
                AddCssLink(skinBaseUrl + "styleforum.css");
                AddCssLink(skinBaseUrl + "stylefeedmanager.css");
                AddCssLink(skinBaseUrl + "styleformwizard.css");
                AddCssLink(skinBaseUrl + "styleaspcalendar.css");
                AddCssLink(skinBaseUrl + "styledatacalendar.css");
              
            }

        }

        

        private void AddCssLink(string cssUrl)
        {
            // don't add .less files
            if (!cssUrl.EndsWith(".css")) { return; }

            Literal cssLink = new Literal();
            cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='" + cssUrl + "' />";
            this.Controls.Add(cssLink);

        }

        private void SetupYuiCss()
        {
            if (WebConfigSettings.DisableYUI) { return; }

            string yuiVersion = "2.6.0";

            if (ConfigurationManager.AppSettings["GoogleCDNYUIVersion"] != null)
            {
                yuiVersion = ConfigurationManager.AppSettings["GoogleCDNYUIVersion"];
            }

            string yuiBasePath;
            if ((WebConfigSettings.UseGoogleCDN) || (ConfigurationManager.AppSettings["YUIBasePath"] == null))
            {
                yuiBasePath = protocol + "://ajax.googleapis.com/ajax/libs/yui/" + yuiVersion + "/build/";
            }
            else
            {
                yuiBasePath = Page.ResolveUrl(ConfigurationManager.AppSettings["YUIBasePath"]);
            }

            string yuiAddOnBasePath = Page.ResolveUrl("~/ClientScript/yuiaddons/");
            if (ConfigurationManager.AppSettings["YUIAddOnsBasePath"] != null)
            {
                yuiAddOnBasePath = Page.ResolveUrl(ConfigurationManager.AppSettings["YUIAddOnsBasePath"]);
            }


            Literal cssLink;

            if (includeYuiLayout)
            {
                if (includeYuiReset)
                {
                    cssLink = new Literal();
                    cssLink.ID = "yui-reset-fonts-grids";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + yuiBasePath + "reset-fonts-grids/reset-fonts-grids.css' />";
                    this.Controls.Add(cssLink);
                }

                if (!useFullYuiSam)
                {
                    cssLink = new Literal();
                    cssLink.ID = "yui-container";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + yuiBasePath + "container/assets/skins/sam/container.css' />";
                    this.Controls.Add(cssLink);

                    cssLink = new Literal();
                    cssLink.ID = "yui-resize";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + yuiBasePath + "resize/assets/skins/sam/resize.css' />";
                    this.Controls.Add(cssLink);

                    cssLink = new Literal();
                    cssLink.ID = "yui-layout";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + yuiBasePath + "layout/assets/skins/sam/layout.css' />";
                    this.Controls.Add(cssLink);

                    cssLink = new Literal();
                    cssLink.ID = "yui-button";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + yuiBasePath + "button/assets/skins/sam/button.css' />";
                    this.Controls.Add(cssLink);

                }

            }

            if (useFullYuiSam)
            {
                cssLink = new Literal();
                cssLink.ID = "yui-samskin";
                cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                    + yuiBasePath + "assets/skins/sam/skin.css' />";
                this.Controls.Add(cssLink);

            }
            else
            {
                // always include at least tabs
                if (includeYuiTabs)
                {
                    cssLink = new Literal();
                    cssLink.ID = "yui-tabview";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + yuiBasePath + "tabview/assets/skins/sam/tabview.css' />";
                    this.Controls.Add(cssLink);
                }

                if (includeColorPickerCss)
                {

                    cssLink = new Literal();
                    cssLink.ID = "yui-colorpickercss";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                    + yuiBasePath + "colorpicker/assets/skins/sam/colorpicker.css' />";
                    this.Controls.Add(cssLink);

                    cssLink = new Literal();
                    cssLink.ID = "dd-windowcss";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + Page.ResolveUrl("~/ClientScript/ddwindow/dhtmlwindow.css")
                         + "' />";
                    this.Controls.Add(cssLink);
                }

                if (includeYuiTreeView)
                {
                    cssLink = new Literal();
                    cssLink.ID = "yui-treeview-css";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                    + yuiBasePath + "treeview/assets/skins/sam/treeview.css' />";
                    this.Controls.Add(cssLink);

                    if (treeViewStyle == "folders")
                    {
                        cssLink = new Literal();
                        cssLink.ID = "yui-treeview-folder-css";
                        cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + Page.ResolveUrl("~/Data/style/treeviewcss/folders/tree.css") + "' />";
                        this.Controls.Add(cssLink);
                    }

                    if (treeViewStyle == "menu")
                    {
                        cssLink = new Literal();
                        cssLink.ID = "yui-treeview-menu-css";
                        cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                        + Page.ResolveUrl("~/Data/style/treeviewcss/menu/tree.css") + "' />";
                        this.Controls.Add(cssLink);
                    }

                }

                if (includeYuiMenu)
                {
                    cssLink = new Literal();
                    cssLink.ID = "yui-menu-css";
                    cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                    + yuiBasePath + "menu/assets/skins/sam/menu.css' />";
                    this.Controls.Add(cssLink);
                }

            }





            if (includeYuiAccordion)
            {
                cssLink = new Literal();
                cssLink.ID = "yui-accordioncss";
                cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
                + yuiAddOnBasePath + "accordionview/assets/skins/sam/accordionview.css' />";
                this.Controls.Add(cssLink);
            }

        }

        
    }
}