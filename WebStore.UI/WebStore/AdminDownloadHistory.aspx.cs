// Author:					Joe Audette
// Created:					2009-07-01
// Last Modified:			2009-12-17
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using WebStore.Business;
using Resources;

namespace WebStore.UI
{

    public partial class AdminDownloadHistoryPage : mojoDialogBasePage
    {
        //private Store store = null;
        private Guid orderGuid = Guid.Empty;
        private Guid ticketGuid = Guid.Empty;
        private int pageId = -1;
        private int moduleId = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParams();

            if (!UserCanEditModule(moduleId))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            LoadSettings();
            PopulateLabels();
            PopulateControls();
            

        }

        private void PopulateControls()
        {
            if (ticketGuid == Guid.Empty) { return; }

            using (IDataReader reader = FullfillDownloadTicket.GetDownloadHistory(ticketGuid))
            {
                grdDownloadHistory.DataSource = reader;
                grdDownloadHistory.DataBind();

            }

        }


        private void PopulateLabels()
        {

            grdDownloadHistory.Columns[0].HeaderText = WebStoreResources.DownloadHistoryUtcHeader;
            grdDownloadHistory.Columns[1].HeaderText = WebStoreResources.DownloadHistoryIPAddressHeader;
        }

       

        private void LoadSettings()
        {
            //if (CurrentPage.ContainsModule(moduleId))
            //{
            //    //store = StoreHelper.GetStore(siteSettings.SiteGuid, moduleId);
            //    store = StoreHelper.GetStore();
            //}
           
        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            orderGuid = WebUtils.ParseGuidFromQueryString("order", orderGuid);
            ticketGuid = WebUtils.ParseGuidFromQueryString("t", ticketGuid);

        }


        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            //AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            ScriptConfig.IncludeJQTable = true;

        }

        #endregion
    }
}
