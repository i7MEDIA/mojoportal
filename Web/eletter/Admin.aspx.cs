/// Author:					Joe Audette
/// Created:				2007-09-23
/// Last Modified:			2011-03-15
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
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ELetterUI
{

    public partial class AdminPage : NonCmsBasePage
    {
        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 20;
        private bool isSiteEditor = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            if ((!isSiteEditor) && (!WebUser.IsNewsletterAdmin))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
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
            List<LetterInfo> letterInfoList
                        = LetterInfo.GetPage(
                        siteSettings.SiteGuid,
                        pageNumber,
                        pageSize,
                        out totalPages);

            if (this.totalPages > 1)
            {
                string pageUrl = SiteUtils.GetNavigationSiteRoot() 
                    + "/eletter/Admin.aspx?pagenumber={0}";

                pgrLetterInfo.PageURLFormat = pageUrl;
                pgrLetterInfo.ShowFirstLast = true;
                pgrLetterInfo.CurrentIndex = pageNumber;
                pgrLetterInfo.PageSize = pageSize;
                pgrLetterInfo.PageCount = totalPages;

            }
            else
            {
                pgrLetterInfo.Visible = false;
            }

            grdLetterInfo.DataSource = letterInfoList;
            grdLetterInfo.PageIndex = pageNumber;
            grdLetterInfo.PageSize = pageSize;
            grdLetterInfo.DataBind();

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuNewsletterAdminLabel);

            heading.Text = Resource.AdminMenuNewsletterAdminLabel;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkThisPage.Text = Resource.NewsLetterAdministrationHeading;
            lnkThisPage.NavigateUrl = SiteRoot + "/eletter/Admin.aspx";

            
            lnkAddNew.Text = Resource.NewsLetterAddNewLink;
            lnkAddNew.NavigateUrl = SiteRoot + "/eletter/LetterInfoEdit.aspx";
            //grdLetterInfo.Columns[0].HeaderText = Resource.NewsLetterTitleLabel;

            lnkManageTemplates.Text = Resource.LetterEditManageTemplatesLink;
            lnkManageTemplates.ToolTip = Resource.LetterEditManageTemplatesToolTip;
            lnkManageTemplates.NavigateUrl = SiteRoot + "/eletter/LetterTemplates.aspx";


        }

        private void LoadSettings()
        {
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            isSiteEditor = SiteUtils.UserIsSiteEditor();

            lnkAdminMenu.Visible = WebUser.IsAdminOrContentAdmin;
            litLinkSeparator1.Visible = lnkAdminMenu.Visible;

            AddClassToBody("administration");
            AddClassToBody("eletteradmin");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            SuppressMenuSelection();
            SuppressPageMenu();
            ScriptConfig.IncludeJQTable = true;
            
        }

        #endregion
    }
}
