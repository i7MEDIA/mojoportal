// Author:					
// Created:					2009-06-01
// Last Modified:			2018-03-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Web.UI;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.AdminUI
{

    public partial class ContentTemplatesPage : NonCmsBasePage
    {

        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = WebConfigSettings.ContentTemplatePageSize;
        private bool userCanEdit = false;
        protected bool showBody = WebConfigSettings.ContentTemplateShowBodyInAdminList;
        private string imagePath = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();
            if (!userCanEdit)
            {
                SiteUtils.RedirectToAccessDeniedPage(this, false);
                return;
            }

            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            BindGrid();

        }

        private void BindGrid()
        {
            List <ContentTemplate> ContentTemplateList
                = ContentTemplate.GetPage(
                siteSettings.SiteGuid,
                pageNumber, 
                pageSize, 
                out totalPages);
 
            string pageUrl = SiteRoot + "/Admin/ContentTemplates.aspx?pagenumber={0}";

            pgrTop.PageURLFormat = pageUrl;
            pgrTop.ShowFirstLast = true;
            pgrTop.CurrentIndex = pageNumber;
            pgrTop.PageSize = pageSize;
            pgrTop.PageCount = totalPages;
            pgrTop.Visible = (totalPages > 1);

            pgrBottom.PageURLFormat = pageUrl;
            pgrBottom.ShowFirstLast = true;
            pgrBottom.CurrentIndex = pageNumber;
            pgrBottom.PageSize = pageSize;
            pgrBottom.PageCount = totalPages;
            pgrBottom.Visible = (totalPages > 1);

            rptTemplates.DataSource = ContentTemplateList;
            rptTemplates.DataBind();
            
        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.ContentTemplates);
            heading.Text = Resource.ContentTemplates;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkThisPage.Text = Resource.ContentTemplates;
            lnkThisPage.ToolTip = Resource.ContentTemplates;
            lnkThisPage.NavigateUrl = SiteRoot + "/Admin/ContentTemplates.aspx";

            lnkAddNewTop.Text = Resource.ContentTemplateAddNewLink;
            lnkAddNewTop.NavigateUrl = SiteRoot + "/Admin/ContentTemplateEdit.aspx";

            lnkAddNewBottom.Text = Resource.ContentTemplateAddNewLink;
            lnkAddNewBottom.NavigateUrl = SiteRoot + "/Admin/ContentTemplateEdit.aspx";

            lnkExportTop.Text = lnkExportBottom.Text = Resource.ExportTemplatesForCKEditor;
            lnkExportTop.NavigateUrl = lnkExportBottom.NavigateUrl = $"{SiteRoot}/Services/CKeditorTemplates.ashx?cb={Guid.NewGuid()}&e=true";

            lnkAddNewBottom.Text = Resource.ContentTemplateAddNewLink;
            lnkAddNewBottom.NavigateUrl = SiteRoot + "/Admin/ContentTemplateEdit.aspx";
        }

        
        private void LoadSettings()
        {
            if (WebUser.IsAdminOrContentAdmin){ userCanEdit = true; }
            else if (SiteUtils.UserIsSiteEditor()) { userCanEdit = true; }
            else if (WebUser.IsInRoles(siteSettings.RolesThatCanEditContentTemplates)) { userCanEdit = true; }

            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);

            AddClassToBody("administration");
            AddClassToBody("templateadmin");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            SuppressMenuSelection();
            SuppressPageMenu();


        }

        #endregion
    }
}
