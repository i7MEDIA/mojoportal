/// Author:					
/// Created:				2007-12-29
/// Last Modified:			2009-11-04
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
using System.Web.UI;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.ELetterUI
{
    public partial class LetterTemplatesPage : NonCmsBasePage
    {
        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 20;
        private bool isSiteEditor = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            if ((!isSiteEditor) && (!WebUser.IsNewsletterAdmin))
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
            if (Page.IsPostBack) return;
            BindGrid();

        }

        private void BindGrid()
        {

            List<LetterHtmlTemplate> LetterHtmlTemplateList
                        = LetterHtmlTemplate.GetPage(
                        siteSettings.SiteGuid,
                        pageNumber,
                        pageSize,
                        out totalPages);

            if (LetterHtmlTemplateList.Count == 0)
            {
                mojoSetup.CreateDefaultLetterTemplates(siteSettings.SiteGuid);

                LetterHtmlTemplateList
                        = LetterHtmlTemplate.GetPage(
                        siteSettings.SiteGuid,
                        pageNumber,
                        pageSize,
                        out totalPages);

            }


            if (this.totalPages > 1)
            {
               
                string pageUrl = SiteUtils.GetNavigationSiteRoot() 
                    + "/eletter/LetterTemplates.aspx?pagenumber={0}";

                pgrLetterHtmlTemplate.PageURLFormat = pageUrl;
                pgrLetterHtmlTemplate.ShowFirstLast = true;
                pgrLetterHtmlTemplate.CurrentIndex = pageNumber;
                pgrLetterHtmlTemplate.PageSize = pageSize;
                pgrLetterHtmlTemplate.PageCount = totalPages;

            }
            else
            {
                pgrLetterHtmlTemplate.Visible = false;
            }

            grdLetterHtmlTemplate.DataSource = LetterHtmlTemplateList;
            grdLetterHtmlTemplate.PageIndex = pageNumber;
            grdLetterHtmlTemplate.PageSize = pageSize;
            grdLetterHtmlTemplate.DataBind();

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuNewsletterAdminLabel);

            lnkAddNew.Text = Resource.LetterHtmlTemplateAddNewLink;
            lnkAddNew.ToolTip = Resource.LetterHtmlTemplateAddNewToolTip;
            lnkAddNew.NavigateUrl = SiteRoot + "/eletter/LetterTemplateEdit.aspx";

            heading.Text = Resource.LetterTemplatesHeading;

            // grdLetterHtmlTemplate.Columns[0].HeaderText = Resource.

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";


            lnkLetterAdmin.Text = Resource.NewsLetterAdministrationHeading;
            lnkLetterAdmin.ToolTip = Resource.NewsLetterAdministrationHeading;
            lnkLetterAdmin.NavigateUrl = SiteRoot + "/eletter/Admin.aspx";

           
            lnkTemplates.Text = Resource.LetterEditManageTemplatesLink;
            lnkTemplates.ToolTip = Resource.LetterEditManageTemplatesToolTip;
            lnkTemplates.NavigateUrl = SiteRoot + "/eletter/LetterTemplates.aspx";
            
        }

        private void LoadSettings()
        {
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            lnkAdminMenu.Visible = WebUser.IsAdminOrContentAdmin;
            litLinkSeparator1.Visible = lnkAdminMenu.Visible;

            AddClassToBody("administration");
            AddClassToBody("lettertemplates");
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
