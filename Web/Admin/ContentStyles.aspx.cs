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
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI
{

    public partial class ContentStylesPage : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ContentStylesPage));
        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = WebConfigSettings.ContentStyleTemplatePageSize;
        private bool userCanEdit = false;

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

            if (!IsPostBack) { BindGrid(); }

        }

        private void BindGrid()
        {
            List<ContentStyle> styles = ContentStyle.GetPage(siteSettings.SiteGuid, pageNumber, pageSize, out totalPages);

            string pageUrl = SiteRoot + "/Admin/ContentStyles.aspx?pagenumber={0}";
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

            grdStyles.DataSource = styles;
            grdStyles.DataBind();

            if (styles.Count() <= 0)
            {
                btnExportStyles.Enabled = false;
            }

        }


        void btnAddNew_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("SiteGuid", typeof(Guid));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("SkinName", typeof(string));
            dataTable.Columns.Add("Element", typeof(string));
            dataTable.Columns.Add("CssClass", typeof(string));
            dataTable.Columns.Add("IsActive", typeof(bool));
           
            DataRow row = dataTable.NewRow();

            row["Guid"] = Guid.Empty;
            row["SiteGuid"] = siteSettings.SiteGuid;
            row["Name"] = string.Empty;
            row["SkinName"] = siteSettings.Skin;
            row["Element"] = string.Empty;
            row["CssClass"] = string.Empty;
            row["IsActive"] = true;

            dataTable.Rows.Add(row);

            grdStyles.EditIndex = 0;
            grdStyles.DataSource = dataTable.DefaultView;
            grdStyles.DataBind();

            pgrTop.Visible = false;
            pgrBottom.Visible = false;
            
            btnAddNew.Visible = false;
            btnImportStyles.Visible = false;
            btnExportStyles.Visible = false;

        }

        void grdStyles_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());

            TextBox txtName = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtName");
            TextBox txtElement = (TextBox)grid.Rows[e.RowIndex].Cells[2].FindControl("txtElement");
            TextBox txtCssClass = (TextBox)grid.Rows[e.RowIndex].Cells[3].FindControl("txtCssClass");
            CheckBox chkIsActive = (CheckBox)grid.Rows[e.RowIndex].Cells[4].FindControl("chkIsActive");

            ContentStyle style;


            if (guid != Guid.Empty)
            {
                style = ContentStyle.Get(guid);
            }
            else
            {
                style = ContentStyle.GetNew(siteSettings.SiteGuid);
                style.SkinName = siteSettings.Skin;
            }

            if (style != null)
            {
                SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser != null) { style.LastModBy = currentUser.UserGuid; }
                style.Name = txtName.Text;
                style.Element = txtElement.Text;
                style.CssClass = txtCssClass.Text;
                style.IsActive = chkIsActive.Checked;
                style.Save();
            }

           
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        void grdStyles_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;
            BindGrid();

            Button btnDelete = (Button)grid.Rows[e.NewEditIndex].Cells[0].FindControl("btnGridDelete");
            if (btnDelete != null)
            {
                btnDelete.Attributes.Add("OnClick", "return confirm('"
                    + Resource.ContentStyleDeleteWarning + "');");
            }
        }

        void grdStyles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            ContentStyle.Delete(guid);
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        void btnExportStyles_Click(object sender, EventArgs e)
        {
            string fileName = "ContentStyles-" + siteSettings.SiteName + "-" + DateTimeHelper.GetDateTimeStringForFileName() + ".xml";

            ExportHelper.ExportStringAsFile(HttpContext.Current, Encoding.Unicode, "text/xml", SkinHelper.GetStyleExportString(siteSettings.SiteGuid), fileName);
        }

        void btnImportStyles_Click(object sender, EventArgs e)
        {
            if (uploader.HasFile)
            {
                if (uploader.FileName.EndsWith(".xml"))
                {
                    SkinHelper.ImportStyles(uploader.FileContent, siteSettings.SiteGuid);
                }
            }
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.ContentStyleTemplates);
            heading.Text = Resource.ContentStyleTemplates;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkThisPage.Text = Resource.ContentStyleTemplates;
            lnkThisPage.ToolTip = Resource.ContentStyleTemplates;
            lnkThisPage.NavigateUrl = SiteRoot + "/Admin/ContentStyles.aspx";

            btnAddNew.Text = Resource.ContentStyleAddNew;
            btnImportStyles.Text = Resource.ImportStyles;
            btnExportStyles.Text = Resource.ExportStyles;

            grdStyles.Columns[1].HeaderText = Resource.ContentStyleName;
            grdStyles.Columns[2].HeaderText = Resource.ContentStyleElement;
            grdStyles.Columns[3].HeaderText = Resource.ContentStyleCssClass;
            grdStyles.Columns[4].HeaderText = Resource.ContentStyleIsActive;

        }

        private void LoadSettings()
        {
            if (WebUser.IsAdminOrContentAdmin) { userCanEdit = true; }
            else if (SiteUtils.UserIsSiteEditor()) { userCanEdit = true; }
            else if (WebUser.IsInRoles(siteSettings.RolesThatCanEditContentTemplates)) { userCanEdit = true; }

            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);

            AddClassToBody("administration");
            AddClassToBody("styleadmin");

        }

        


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            btnAddNew.Click += new EventHandler(btnAddNew_Click);
            btnExportStyles.Click += new EventHandler(btnExportStyles_Click);
            btnImportStyles.Click += new EventHandler(btnImportStyles_Click);
            grdStyles.RowEditing += new GridViewEditEventHandler(grdStyles_RowEditing);
            grdStyles.RowDeleting += new GridViewDeleteEventHandler(grdStyles_RowDeleting);
            grdStyles.RowUpdating += new GridViewUpdateEventHandler(grdStyles_RowUpdating);

            SuppressMenuSelection();
            SuppressPageMenu();
            ScriptConfig.IncludeJQTable = true;
        }

        

        #endregion
    }
}
