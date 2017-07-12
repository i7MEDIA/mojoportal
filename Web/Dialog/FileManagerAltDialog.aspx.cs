//  Author:                     
//  Created:                    2009-12-30
//	Last Modified:              2015-09-04 (i7MEDIA)
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

// Use mojoDialogBasePage instead of Page

using System;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.Dialog
{
    public partial class FileManagerAltDialog : mojoDialogBasePage
    {
        private SiteSettings siteSettings = null;
        private bool canAccess = false;

        protected void Page_Load(object sender, EventArgs e)
        {
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

            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuFileManagerLink);
            lnkAltFileManager.Text = Resources.Resource.FileManagerAlternateLink;
            lnkAltFileManager.NavigateUrl = SiteUtils.GetNavigationSiteRoot() + "/Dialog/FileManagerDialog.aspx";
#if MONO
            lnkAltFileManager.Visible = false;
#endif
        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (WebUser.IsAdminOrContentAdmin || WebUser.IsInRoles(siteSettings.RolesThatCanDeleteFilesInEditor) || SiteUtils.UserIsSiteEditor())
            {
                canAccess = true;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);
            
        }


    }
}
