//	Created:			    2010-01-04
//	Last Modified:		    2011-05-16
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
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// a convenience link for the File manager. The link renders only for those in roles that can use it
    /// </summary>
    public class FileManagerLink : HyperLink
    {
        private mojoBasePage basePage = null;

        private bool renderAsListItem = false;
        public bool RenderAsListItem
        {
            get { return renderAsListItem; }
            set { renderAsListItem = value; }
        }

        private string listItemCSS = string.Empty;
        public string ListItemCss
        {
            get { return listItemCSS; }
            set { listItemCSS = value; }
        }

        private bool ShouldRender()
        {
            if (basePage == null) { return false; }
            if (!Page.Request.IsAuthenticated) { return false; }
            if (!WebConfigSettings.ShowFileManagerLink) { return false; }
            if (WebConfigSettings.DisableFileManager) { return false; }
            if (basePage.SiteInfo == null) { return false; }

            if(SiteUtils.UserIsSiteEditor()) { return true; }

            // only roles that can delete can use file manager
            if (!WebUser.IsInRoles(basePage.SiteInfo.RolesThatCanDeleteFilesInEditor)) { return false; }

            if (
                (WebUser.IsInRoles(basePage.SiteInfo.UserFilesBrowseAndUploadRoles))
                || (WebUser.IsInRoles(basePage.SiteInfo.GeneralBrowseAndUploadRoles))
                ) 
            { return true; }

            

            return false;
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (HttpContext.Current == null) { return; }
            EnableViewState = false;
            basePage = Page as mojoBasePage;


            Visible = ShouldRender();
            if (!Visible) { return; }
            if (basePage == null) { return; }

            basePage.ScriptConfig.IncludeColorBox = true; 

            if (CssClass.Length > 0)
            {
                CssClass = "adminlink filemanlink cblink " + CssClass;
            }
            else
            {
                CssClass = "adminlink filemanlink cblink";
            }

            Text = Resource.AdminMenuFileManagerLink;
            ToolTip = Resource.AdminMenuFileManagerLink;

            if (SiteUtils.SslIsAvailable())
            {
                if (WebConfigSettings.UseAlternateFileManagerAsDefault)
                {
                    NavigateUrl = basePage.SiteRoot + "/Dialog/FileManagerAltDialog.aspx";
                }
                else
                {
                    NavigateUrl = basePage.SiteRoot + "/Dialog/FileManagerDialog.aspx";
                }
            }
            else
            {
                if (WebConfigSettings.UseAlternateFileManagerAsDefault)
                {
                    NavigateUrl = basePage.RelativeSiteRoot + "/Dialog/FileManagerAltDialog.aspx";
                }
                else
                {
                    NavigateUrl = basePage.RelativeSiteRoot + "/Dialog/FileManagerDialog.aspx";
                }
            }
//#if MONO

//            NavigateUrl = basePage.SiteRoot + "/Dialog/FileManagerAltDialog.aspx";
//#endif

            if (basePage.UseIconsForAdminLinks)
            {
                ImageUrl = Page.ResolveUrl("~/Data/SiteImages/folder_explore.png");
               
            }
            //ClientClick = "GB_showFullScreen(this.title, this.href); return false;";
            //DialogCloseText = Resource.CloseDialogButton;
            
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            if (renderAsListItem)
            {
                if (listItemCSS.Length > 0)
                {
                    writer.Write("<li class='" + listItemCSS + "'>");
                }
                else
                {
                    writer.Write("<li>");
                }
            }

            base.Render(writer);

            if (renderAsListItem)
            {
                writer.Write("</li>");
            }
        }


    }

}
