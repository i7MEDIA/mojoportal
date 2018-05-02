// Author:					
// Created:					2011-03-14
// Last Modified:			2018-05-02
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Web;



namespace mojoPortal.Web.AdminUI
{

	public partial class CacheToolPage : NonCmsBasePage
    {
        private string cssCacheCookieName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins)))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            lblSkinGuid.Text = siteSettings.SkinVersion.ToString();

            if (CookieHelper.CookieExists(cssCacheCookieName))
            {
                btnCssCacheToggle.Text = DevTools.EnableCssCaching;
            }
            else
            {
                btnCssCacheToggle.Text = DevTools.DisableCssCaching;
            }

        }

        void btnCssCacheToggle_Click(object sender, EventArgs e)
        {
            if (CookieHelper.CookieExists(cssCacheCookieName))
            {
                HttpCookie cssCookie = new HttpCookie(cssCacheCookieName, string.Empty);
                cssCookie.Expires = DateTime.Now.AddMinutes(-10);
                cssCookie.Path = "/";
                Response.Cookies.Add(cssCookie);
            }
            else
            {
                CookieHelper.SetPersistentCookie(cssCacheCookieName, "true");
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);


        }

        void btnResetSkinVersionGuid_Click(object sender, EventArgs e)
        {
            siteSettings.SkinVersion = Guid.NewGuid();
            siteSettings.Save();
            CacheHelper.ClearSiteSettingsCache(siteSettings.SiteId);

            WebUtils.SetupRedirect(this, Request.RawUrl);
            
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, DevTools.CacheTool);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkDesignerTools.Text = DevTools.DesignTools;
            lnkDesignerTools.NavigateUrl = SiteRoot + "/DesignTools/Default.aspx";

            lnkThisPage.Text = DevTools.CacheTool;
            lnkThisPage.NavigateUrl = SiteRoot + "/DesignTools/CacheTool.aspx";

            btnResetSkinVersionGuid.Text = Resource.ResetSkinVersionGuid;

        }

        private void LoadSettings()
        {
            cssCacheCookieName = SiteUtils.GetCssCacheCookieName(siteSettings);

            AddClassToBody("administration");
            AddClassToBody("designtools");
        }

       
        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            btnCssCacheToggle.Click += new EventHandler(btnCssCacheToggle_Click);
            btnResetSkinVersionGuid.Click += new EventHandler(btnResetSkinVersionGuid_Click);

            SuppressMenuSelection();
            SuppressPageMenu();


        }

        

        #endregion
    }
}
