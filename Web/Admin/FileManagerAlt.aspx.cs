//  Author:                     
//  Created:                    2009-12-30
//	Last Modified:              2018-03-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Web;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class FileManagerAlt : NonCmsBasePage
    {
        private bool canAccess = false;

        protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();
            // if the user has no upload permissions the file manager control will handle blocking access
            // but upload permissions doesn't guarantee delete permission
            // only users who are trusted to delete should be able to use the file manager
            if (
                WebConfigSettings.DisableFileManager
                || (!canAccess)
                )
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            
            PopulateLabels();

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuFileManagerLink);
            heading.Text = Resource.AdminMenuFileManagerLink;
            heading.LiteralExtraMarkup = "&nbsp;<a href='" + SiteRoot + "/Admin/FileManager.aspx"
                   + "' class='ModuleEditLink' title='" + Resource.FileManagerAlternateLink + "'>" + Resource.FileManagerAlternateLink + "</a>";

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkFileManager.Text = Resource.AdminMenuFileManagerLink;
            lnkFileManager.ToolTip = Resource.AdminMenuFileManagerLink;
            lnkFileManager.NavigateUrl = SiteRoot + "/Admin/FileManagerAlt.aspx";

            
            
//#if MONO
//            lnkAltFileManager.Visible = false;
//#endif

        }

        private void LoadSettings()
        {
            if (WebUser.IsAdminOrContentAdmin || WebUser.IsInRoles(siteSettings.RolesThatCanDeleteFilesInEditor) || SiteUtils.UserIsSiteEditor())
            {
                canAccess = true;
            }

            AddClassToBody("administration");
            AddClassToBody("altfileadmin");
        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            if (HttpContext.Current == null) { return; }
            base.OnInit(e);

            this.Load += new EventHandler(Page_Load);
            DoInit();
            

        }

        private void DoInit()
        {
            SuppressMenuSelection();
            SuppressPageMenu();

            ScriptConfig.IncludeJQTable = true;
        }


        #endregion
    }
}
