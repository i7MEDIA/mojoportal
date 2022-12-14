using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.UI;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web
{
    
    public class layout : MasterPage
    {
        #region declarations moved here from designer.cs 2012-09-16
        

        protected SiteMenu SiteMenu1;
        protected Panel divLeft;
        protected PageMenuControl PageMenu1;
        protected ContentPlaceHolder leftContent;
        protected Panel divCenter;
        protected ContentPlaceHolder mainContent;
        protected Panel divRight;
        protected ContentPlaceHolder rightContent;
        protected ContentPlaceHolder pageEditContent;

        #endregion

        private int leftModuleCount = 0;
        private int centerModuleCount = 0;
        private int rightModuleCount = 0;
        private int alt1ModuleCount = 0;
        private int alt2ModuleCount = 0;
        protected SiteSettings siteSettings;
        protected PageSettings currentPage = null;
        private SiteMapDataSource siteMapDataSource = null;
        private SiteMapNode rootNode = null;
        protected string SkinBaseUrl = string.Empty;

        private bool useArtisteer3 = false;
        private bool hideEmptyAlt1 = true;
        private bool hideEmptyAlt2 = true;
        private string leftSideNoRightSideCss = "art-layout-cell art-sidebar1 leftside left2column";
        private string rightSideNoLeftSideCss = "art-layout-cell art-sidebar2 rightside right2column";
        private string leftAndRightNoCenterCss = string.Empty;
        private string leftOnlyCss = string.Empty;
        private string rightOnlyCss = string.Empty;
        private string centerNoLeftSideCss = "art-layout-cell art-content center-rightmargin cmszone";
        private string centerNoRightSideCss = "art-layout-cell art-content center-leftmargin cmszone";
        private string centerNoLeftOrRightSideCss = "art-layout-cell art-content-wide center-nomargins cmszone";
        private string centerWithLeftAndRightSideCss = "art-layout-cell  art-content-narrow center-rightandleftmargins cmszone";
        private string emptyCenterCss = string.Empty;
        private bool hideEmptyCenterIfOnlySidesHaveContent = false;

        protected bool isCmsPage = false;
		protected bool isNonCmsBasePage = false;
        protected bool isMobileDevice = false;
        private int mobileOnly = (int)ContentPublishMode.MobileOnly;
        private int webOnly = (int)ContentPublishMode.WebOnly;
        
		protected virtual void OnPreInit(EventArgs e)
		{
			// this is here to allow adding logic before the Page_Load from the skin layout.master
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current == null) 
            { 
                return; 
            }

            siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (siteSettings == null) 
            { 
                return; 
            }

			SkinBaseUrl = SiteUtils.GetSkinBaseUrl(Page);

            isMobileDevice = SiteUtils.IsMobileDevice();

			isNonCmsBasePage = Page is NonCmsBasePage;

            if (Page is CmsPage) 
            {
                isCmsPage = true;
                currentPage = CacheHelper.GetCurrentPage(); 
            }
			else if (!isNonCmsBasePage)
			{
				currentPage = CacheHelper.GetPage(WebUtils.ParseInt32FromQueryString("pageid", -1));
			}
			
            siteMapDataSource = (SiteMapDataSource)this.FindControl("SiteMapData");
            if(siteMapDataSource == null){ return;}

            siteMapDataSource.SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString();

            try 
            {
                rootNode = siteMapDataSource.Provider.RootNode;
            }
            catch(HttpException) 
            {
                return;
            }

            Control c = this.FindControl("StyleSheetCombiner");
            if ((c != null) && (c is StyleSheetCombiner))
            {
                StyleSheetCombiner style = c as StyleSheetCombiner;
                useArtisteer3 = style.UseArtisteer3;
                hideEmptyAlt1 = style.HideEmptyAlt1;
                hideEmptyAlt2 = style.HideEmptyAlt2;
            }

            if (!useArtisteer3)
            {
                Control l = this.FindControl("LayoutDisplaySettings1");
                if ((l != null) && (l is LayoutDisplaySettings))
                {
                    LayoutDisplaySettings layoutSettings = l as LayoutDisplaySettings;
                    leftSideNoRightSideCss = layoutSettings.LeftSideNoRightSideCss;
                    rightSideNoLeftSideCss = layoutSettings.RightSideNoLeftSideCss;
                    leftAndRightNoCenterCss = layoutSettings.LeftAndRightNoCenterCss;
                    leftOnlyCss = layoutSettings.LeftOnlyCss;
                    rightOnlyCss = layoutSettings.RightOnlyCss;
                    centerNoLeftSideCss = layoutSettings.CenterNoLeftSideCss;
                    centerNoRightSideCss = layoutSettings.CenterNoRightSideCss;
                    centerNoLeftOrRightSideCss = layoutSettings.CenterNoLeftOrRightSideCss;
                    centerWithLeftAndRightSideCss = layoutSettings.CenterWithLeftAndRightSideCss;
                    emptyCenterCss = layoutSettings.EmptyCenterCss;
                    hideEmptyCenterIfOnlySidesHaveContent = layoutSettings.HideEmptyCenterIfOnlySidesHaveContent;

                }
            }
			
            SetupLayout();
        }


        /// <summary>
        /// Count items in each of the 3 columns to determine what css class to assign to center and whether to hide side columns.
        /// This gives us automatic adjustment of column layout from 1 to 3 columns for the main layout.
        /// </summary>
        private void SetupLayout()
        {
            // Count menus if they exist within a content pane and are visible
            CountVisibleMenus();
            CountContentInstances();

            if ((hideEmptyAlt1) && (alt1ModuleCount == 0)) {
                Control a1 = FindControl("divAlt1");
                if (a1 != null) {
                    a1.Visible = false;
                }
                else {
                    a1 = FindControl("divAltContent1");
                    if (a1 != null) { a1.Visible = false; }
                }
            }

            if ((hideEmptyAlt2) && (alt2ModuleCount == 0)) {
                Control a2 = FindControl("divAltContent2");
                if (a2 != null) { a2.Visible = false; }
            }

            // Set css classes based on count of items in each column panel
            divLeft.Visible = (leftModuleCount > 0);
            divRight.Visible = (rightModuleCount > 0);

            if((divLeft.Visible)&&(!divRight.Visible)) {
                divLeft.CssClass = leftSideNoRightSideCss;
            }

            if ((divRight.Visible) && (!divLeft.Visible)) {
                divRight.CssClass = rightSideNoLeftSideCss;
            }

            if (useArtisteer3)
            {
                divCenter.CssClass =
                    divLeft.Visible
                        ? (divRight.Visible ? "art-layout-cell art-content art-content-narrow center-rightandleftmargins cmszone" : "art-layout-cell art-content center-leftmargin cmszone")
                        : (divRight.Visible ? "art-layout-cell art-content center-rightmargin cmszone" : "art-layout-cell art-content-wide center-nomargins cmszone");
            }
            else {
                divCenter.CssClass =
                    divLeft.Visible
                        ? (divRight.Visible ? centerWithLeftAndRightSideCss : centerNoRightSideCss)
                        : (divRight.Visible ? centerNoLeftSideCss : centerNoLeftOrRightSideCss);

            }

            //https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11210~1#post46748
            if ((isCmsPage)&&(centerModuleCount == 0))
            {
                if ((leftModuleCount > 0) && (rightModuleCount > 0))
                {
                    if (emptyCenterCss.Length > 0) { divCenter.CssClass = emptyCenterCss; }

                    divCenter.Visible = !hideEmptyCenterIfOnlySidesHaveContent;

                    if (leftAndRightNoCenterCss.Length > 0)
                    {
                        divLeft.CssClass = leftAndRightNoCenterCss;
                        divRight.CssClass = leftAndRightNoCenterCss;
                    }
                    
                }
                else if (leftModuleCount > 0)
                {
                    if (emptyCenterCss.Length > 0) { divCenter.CssClass = emptyCenterCss; }

                    if (leftOnlyCss.Length > 0)
                    {
                        divLeft.CssClass = leftOnlyCss;
                    }
                }
                else if (rightModuleCount > 0)
                {
                    if (emptyCenterCss.Length > 0) { divCenter.CssClass = emptyCenterCss; }

                    if (rightOnlyCss.Length > 0)
                    {
                        divRight.CssClass = rightOnlyCss;
                    }
                }

            }
            

            if (!IsPostBack) {

                divLeft.CssClass += " cmszone";
                divRight.CssClass += " cmszone";

                // these are optional panels that may exist in some skins
                // but are not part of the automatic column layout scheme
                Control alt = this.FindControl("divAlt1");
                if ((alt != null) && (alt is Panel))
                {
                    ((Panel)alt).CssClass += " cmszone";
                }

                alt = this.FindControl("divAlt2");
                if ((alt != null) && (alt is Panel))
                {
                    ((Panel)alt).CssClass += " cmszone";
                }

                alt = this.FindControl("divAltContent2");
                if ((alt != null) && (alt is Panel))
                {
                    ((Panel)alt).CssClass += " cmszone";
                }
            }

            
        }

        private void CountContentInstances()
        {
            if ((Page is CmsPage) && (currentPage != null))
            {
                foreach (Module module in currentPage.Modules)
                {
                    //if ((module.ControlSource == "Modules/LoginModule.ascx") && (Request.IsAuthenticated)) { continue; }
                    if (module.ControlSource == "Modules/LoginModule.ascx")
                    {
                        LoginModuleDisplaySettings loginSettings = new LoginModuleDisplaySettings();
                        this.Controls.Add(loginSettings);
                        if ((Request.IsAuthenticated) && (loginSettings.HideWhenAuthenticated)) { continue; }
                    }

                    if (ModuleIsVisible(module))
                    {
                        if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "leftpane"))
                        {
                            leftModuleCount++;
                        }

                        if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "rightpane"))
                        {
                            rightModuleCount++;
                        }

                        if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "contentpane"))
                        {
                            centerModuleCount++;
                        }

                        if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "altcontent1"))
                        {
                            alt1ModuleCount++;
                        }

                        if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "altcontent2"))
                        {
                            alt2ModuleCount++;
                        }
                    }
                }

                // this is to make room for ModuleWrapper or custom usercontrols if they exsits anywhere in left or right
                foreach (Control c in divRight.Controls)
                {
                    if (c is mojoUserControl) { rightModuleCount++; }
                }

                foreach (Control c in divLeft.Controls)
                {
                    if (c is mojoUserControl) { leftModuleCount++; }
                }
            }
        }

        private void CountVisibleMenus()
        {
            // Count menus if they exist within a content pane and are visible
            if ((SiteMenu1 != null) && SiteMenu1.Visible)
            {
                // printable view skin doesn't have a menu so it is null there
                if (SiteMenu1.Parent.ID == "divLeft") leftModuleCount++;
                if (SiteMenu1.Parent.ID == "divRight") rightModuleCount++;
            }

            Control c = this.FindControl("PageMenu1");
            if (
                (c != null)
                && (c.Visible)
                )
            {
                PageMenuControl p = (PageMenuControl)c;
                if ((!p.IsSubMenu)||(SiteUtils.TopPageHasChildren(rootNode, p.StartingNodeOffset)))
                {
                    if (c.Parent.ID == "divLeft") leftModuleCount++;
                    if (c.Parent.ID == "divRight") rightModuleCount++;
                }

            }

            c = this.FindControl("PageMenu2");
            if (
                (c != null)
                && (c.Visible)
                )
            {
                PageMenuControl p = (PageMenuControl)c;
                if (SiteUtils.TopPageHasChildren(rootNode, p.StartingNodeOffset))
                {
                    if (c.Parent.ID == "divLeft") leftModuleCount++;
                    if (c.Parent.ID == "divRight") rightModuleCount++;
                }
            }

            c = this.FindControl("PageMenu3");
            if (
                (c != null)
                && (c.Visible)
                )
            {
                PageMenuControl p = (PageMenuControl)c;
                if (SiteUtils.TopPageHasChildren(rootNode, p.StartingNodeOffset))
                {
                    if (c.Parent.ID == "divLeft") leftModuleCount++;
                    if (c.Parent.ID == "divRight") rightModuleCount++;
                }
            }

            c = this.FindControl("pnlMenu");
            if ((c != null) && (c.Parent.ID == "divLeft")) leftModuleCount++;

            c = this.FindControl("StyleSheetCombiner");
            if ((c != null) && (c is StyleSheetCombiner))
            {
                StyleSheetCombiner style = c as StyleSheetCombiner;
                if (style.AlwaysShowLeftColumn) { leftModuleCount++; }
                if (style.AlwaysShowRightColumn) { rightModuleCount++; }
            }

           

        }

        private bool ModuleIsVisible(Module module)
        {
            if ((module.HideFromAuthenticated) && (Request.IsAuthenticated)) { return false; }
            if ((module.HideFromUnauthenticated) && (!Request.IsAuthenticated)) { return false; }
            if (isMobileDevice && module.PublishMode == webOnly) { return false; }
            if (!isMobileDevice && module.PublishMode == mobileOnly) 
            {
                if (WebConfigSettings.RolesThatAlwaysViewMobileContent.Length > 0)
                {
                    if (WebUser.IsInRoles(WebConfigSettings.RolesThatAlwaysViewMobileContent)) { return true; }
                }
                return false; 
            }

            return true;
        }

       
    }
}
