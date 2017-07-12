// Author:					
// Created:					2009-11-03
// Last Modified:			2010-06-15
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
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;



namespace mojoPortal.Web.AdminUI
{

    public partial class ContentRatingsDialog : mojoDialogBasePage
    {
        private int pageNumber = 1;
        private int totalPages = 1;
        private int pageSize = WebConfigSettings.ContentRatingListPageSize;
        private Guid contentGuid = Guid.Empty;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!WebUser.IsAdminOrContentAdmin)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            LoadSettings();
            PopulateLabels();
            if (!Page.IsPostBack) { PopulateControls(); }

        }

        private void PopulateControls()
        {
            List<ContentRating> ratings = ContentRating.GetPage(contentGuid, pageNumber, pageSize, out totalPages);

            string pageUrl = SiteRoot + "/Dialog/ContentRatingsDialog.aspx?c=" + contentGuid.ToString()
                + "&amp;pagenumber={0}"; ;

            pgr.PageURLFormat = pageUrl;
            pgr.ShowFirstLast = true;
            pgr.CurrentIndex = pageNumber;
            pgr.PageSize = pageSize;
            pgr.PageCount = totalPages;
            pgr.Visible = (totalPages > 1);

            grdContentRating.DataSource = ratings;
            grdContentRating.DataBind();

        }

        void grdContentRating_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string s = e.CommandArgument.ToString();
            Guid ratingGuid = Guid.Empty;
            if (s.Length == 36)
            {
                try
                {
                    ratingGuid = new Guid(s);
                }
                catch (FormatException) { }

            }


            switch (e.CommandName)
            {
                case "DeleteRating":

                    if (ratingGuid != Guid.Empty)
                    {
                        ContentRating.Delete(ratingGuid);
                    }
                    
                    
                    break;

            }

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }


        private void PopulateLabels()
        {
             grdContentRating.Columns[0].HeaderText = Resource.Email;
             grdContentRating.Columns[1].HeaderText = Resource.IpAddress;
             grdContentRating.Columns[2].HeaderText = Resource.Date;
             grdContentRating.Columns[3].HeaderText = Resource.Rating;
             grdContentRating.Columns[4].HeaderText = Resource.Comments;
        }

        private void LoadSettings()
        {
            contentGuid = WebUtils.ParseGuidFromQueryString("c", contentGuid);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

        }

        


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            grdContentRating.RowCommand += new GridViewCommandEventHandler(grdContentRating_RowCommand);
            

        }

        

        #endregion
    }
}
