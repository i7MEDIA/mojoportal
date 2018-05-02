// Author:					
// Created:					2011-03-21
// Last Modified:			2018-05-02
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using mojoPortal.Web;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;
using System.Text;

namespace mojoPortal.Web.AdminUI
{

    public partial class ManageSkinPage : NonCmsBasePage
    {
        protected string skinName = string.Empty;
        private string skinBasePath = string.Empty;
        protected bool allowEditing = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins)))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();

            if (!allowEditing)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/DesignTools/SkinList.aspx");
                return;
            }

            if (!Directory.Exists(Server.MapPath(skinBasePath + skinName)))
            {   //skin doesn't exist go back to skin list
                WebUtils.SetupRedirect(this, SiteRoot + "/DesignTools/SkinList.aspx");
                return;
            }

            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {

            BindSkin();

        }

        private void BindSkin()
        {
            
            string title = string.Format(CultureInfo.InvariantCulture, Resource.ManageSkinFormat, skinName);
            Title = SiteUtils.FormatPageTitle(siteSettings, title);

            heading.Text = title;
            lnkThisPage.Text = skinName;
			string skinFolderPath = Server.MapPath(skinBasePath + skinName);
			var files = SkinHelper.GetCssFileList(skinFolderPath: skinFolderPath, recursive: true);
			//List<object> fileObjs = new List<object>();
			StringBuilder sb = new StringBuilder();

			foreach (var file in files)
			{
				//we want to have just the name and the directory starting at the skin path
				//fileObjs.Add(new { Name = file.Name, Directory = file.DirectoryName.Replace(skinFolderPath, "") });
				string thisPath = file.FullName.Replace(skinFolderPath, "").TrimStart('/').TrimStart('\\').Replace('\\', '/');
				sb.Append($"<li class='simplelist'><a href='{SiteRoot}/DesignTools/CssEditor.aspx?s={skinName}&f={thisPath}'>{thisPath}</a></li>");
			}

			litCssFiles.Text = $"<ul class='simplelist'>{sb.ToString()}</ul>";

			//rptCss.DataSource = fileObjs;
   //         rptCss.DataBind();

        }

        void btnCopy_Click(object sender, EventArgs e)
        {
            string newSkinName = txtCopyAs.Text.ToCleanFolderName(true);
            if ((newSkinName.Length > 0)&&( newSkinName != skinName))
            {
                SkinHelper.CopySkin(Server.MapPath(skinBasePath + skinName), Server.MapPath(skinBasePath + newSkinName));

                WebUtils.SetupRedirect(this, SiteRoot + "/DesignTools/ManageSkin.aspx?s=" + newSkinName);
            }
            else
            {
                WebUtils.SetupRedirect(this, Request.RawUrl);
            }


        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.ManageSkin);

            heading.Text = Resource.ManageSkin;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkDesignerTools.Text = DevTools.DesignTools;
            lnkDesignerTools.NavigateUrl = SiteRoot + "/DesignTools/Default.aspx";

            lnkSkinList.Text = DevTools.SkinManagement;
            lnkSkinList.NavigateUrl = SiteRoot + "/DesignTools/SkinList.aspx";

            lnkThisPage.Text = Resource.ManageSkin;
            lnkThisPage.NavigateUrl = Request.RawUrl;

            btnCopy.Text = Resource.CopySkinAs;

        }

        private void LoadSettings()
        {
            skinBasePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/skins/";
            skinName = WebUtils.ParseStringFromQueryString("s", string.Empty);

            allowEditing = WebConfigSettings.AllowEditingSkins && (WebConfigSettings.AllowEditingSkinsInChildSites || siteSettings.IsServerAdminSite);

            AddClassToBody("administration");
            AddClassToBody("designtools");
        }

       


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            btnCopy.Click += new EventHandler(btnCopy_Click);

            SuppressMenuSelection();
            SuppressPageMenu();

        }

        

        #endregion
    }
}
