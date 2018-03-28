/// Last Modifed: 2018-03-28

using System;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class FileManagerPage : NonCmsBasePage
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

            heading.LiteralExtraMarkup = "&nbsp;<a href='" + SiteRoot + "/Admin/FileManagerAlt.aspx"
                   + "' class='ModuleEditLink' title='" + Resource.FileManagerAlternateLink + "'>" + Resource.FileManagerAlternateLink + "</a>";

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkFileManager.Text = Resource.AdminMenuFileManagerLink;
            lnkFileManager.ToolTip = Resource.AdminMenuFileManagerLink;
            lnkFileManager.NavigateUrl = SiteRoot + "/Admin/FileManager.aspx";

            
            
           
        }

        private void LoadSettings()
        {
            if (WebUser.IsAdminOrContentAdmin || WebUser.IsInRoles(siteSettings.RolesThatCanDeleteFilesInEditor) || SiteUtils.UserIsSiteEditor())
            {
                canAccess = true;
            }

            AddClassToBody("administration");
            AddClassToBody("fileadmin");
            ScriptConfig.IncludeQtFile = true;
            if (StyleCombiner != null) { StyleCombiner.AddQtFileCss = true; }
        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            this.Load += new EventHandler(Page_Load);
       
            SuppressMenuSelection();
            SuppressPageMenu();
    
        }

       
        #endregion
    }
}
