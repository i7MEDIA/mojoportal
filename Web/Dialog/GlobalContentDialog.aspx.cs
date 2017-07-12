// Author:					
// Created:				    2009-04-25
// Last Modified:			2009-05-02
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    public partial class GlobalContentDialog : mojoDialogBasePage
    {
        private int pageId = -1;
        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 15;

        //private bool sortByFeature = false;
        //private bool sortByAuthor = false;
        private int moduleDefId = -1;
        private string title = string.Empty;
        
        private SiteSettings siteSettings = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if (!UserCanEditPage(pageId))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();
            if (!IsPostBack)
            {
                PopulateControls();
            }
        }

        private void PopulateControls()
        {
            if (siteSettings == null) { return; }

                BindList();
            

        }

        private void BindList()
        {

            DataTable dt = Module.SelectGlobalPage(
               siteSettings.SiteId,
               moduleDefId,
               pageId,
               pageNumber,
               pageSize,
               out totalPages);

            if (pageNumber > totalPages)
            {
                pageNumber = 1;

            }

            string pageUrl = SiteRoot
                + "/Dialog/GlobalContentDialog.aspx?pageid=" + pageId.ToInvariantString()
                + "&amp;pagenumber={0}";

            pgr.PageURLFormat = pageUrl;
            pgr.ShowFirstLast = true;
            pgr.CurrentIndex = pageNumber;
            pgr.PageSize = pageSize;
            pgr.PageCount = totalPages;
            pgr.Visible = (totalPages > 1);


            grid.DataSource = dt;
            grid.DataBind();


        }

        void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnSelect = (Button)e.Row.Cells[1].FindControl("btnSelect");
                if (btnSelect == null) { return; }

                //string[] pair = btnSelect.CommandArgument.Split('|');
                //if (pair.Length != 2) { return; }
                //if (pair[0].Length != 36) { return; }


                btnSelect.Attributes.Add("onclick", GetClientScriptForButton(btnSelect.CommandArgument.ToString()));

            }

        }

        private string GetClientScriptForButton(string moduleId)
        {
            return "top.window.AddModule(" + moduleId + ");  return false;";
        }

        //void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    Button btnSelect = (Button)e.Item.FindControl("btnSelect");
        //    if (btnSelect == null) { return; }

        //    string[] pair = btnSelect.CommandArgument.Split('|');
        //    if (pair.Length != 2) { return; }
        //    if (pair[0].Length != 36) { return; }


        //    btnSelect.Attributes.Add("onclick", GetClientScriptForButton(pair[0], pair[1]));

        //}

        

        //void btnSearchUser_Click(object sender, EventArgs e)
        //{
        //    string pageUrl = SiteRoot
        //         + "/Dialog/UserSelectorDialog.aspx?"
        //         + "letter=" + Server.HtmlEncode(txtSearchUser.Text)
        //         + "&pagenumber=";

        //    WebUtils.SetupRedirect(this, pageUrl);

        //}




        private void PopulateLabels()
        {
            grid.Columns[0].HeaderText = Resource.ContentManagerContentTitleColumnHeader;
            grid.Columns[1].HeaderText = Resource.ContentManagerFeatureTypeColumnHeader;
            grid.Columns[2].HeaderText = Resource.ContentManagerAuthorColumnHeader;
            
        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
            


        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            grid.RowDataBound += new GridViewRowEventHandler(grid_RowDataBound);
        }

        

    }
}