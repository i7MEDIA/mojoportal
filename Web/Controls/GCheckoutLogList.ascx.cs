/// Author:					
/// Created:				2008-04-06
/// Last Modified:			2009-12-17
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using Resources;


namespace mojoPortal.Web.UI
{
    public partial class GCheckoutLogList : UserControl
    {
        private int pageId = -1;
        private int moduleId = -1;
        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 100;
        private Guid storeGuid = Guid.Empty;
        private Guid cartGuid = Guid.Empty;

        public Guid StoreGuid
        {
            get { return storeGuid; }
            set { storeGuid = value; }
        }

        public Guid CartGuid
        {
            get { return cartGuid; }
            set { cartGuid = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();

            BindGrid();
        }

        private void BindGrid()
        {
            if (cartGuid != Guid.Empty)
            {
                BindGridForCart();
            }
            else if (storeGuid != Guid.Empty)
            {
                BindGridForStore();

            }

        }

        private void BindGridForCart()
        {
            if (cartGuid == Guid.Empty) 
            {
                pgrGoogleCheckoutLog.Visible = false;
                return; 
            }

            using(IDataReader reader = GoogleCheckoutLog.GetPageByCart(
                    cartGuid,
                    pageNumber,
                    pageSize,
                    out totalPages))
           {
            
                if (this.totalPages > 1)
                {

                    string pageUrl = Page.Request.CurrentExecutionFilePath
                        + "?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
                        + "&amp;mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                        + "&amp;pagenumber={0}";

                    pgrGoogleCheckoutLog.PageURLFormat = pageUrl;
                    pgrGoogleCheckoutLog.ShowFirstLast = true;
                    pgrGoogleCheckoutLog.CurrentIndex = pageNumber;
                    pgrGoogleCheckoutLog.PageSize = pageSize;
                    pgrGoogleCheckoutLog.PageCount = totalPages;

                }
                else
                {
                    pgrGoogleCheckoutLog.Visible = false;
                }

                grdGoogleCheckoutLog.PageIndex = pageNumber;
                grdGoogleCheckoutLog.PageSize = pageSize;
                grdGoogleCheckoutLog.DataSource = reader;
                grdGoogleCheckoutLog.DataBind();
                
            }
        

            if (grdGoogleCheckoutLog.Rows.Count == 0)
            {
                this.Visible = false;
            }

        }

        private void BindGridForStore()
        {
            if (storeGuid == Guid.Empty)
            {
                pgrGoogleCheckoutLog.Visible = false;
                return;
            }

            using(IDataReader reader = GoogleCheckoutLog.GetPageByStore(
                    storeGuid,
                    pageNumber,
                    pageSize,
                    out totalPages))
            {
            
                if (this.totalPages > 1)
                {

                    string pageUrl = Page.Request.CurrentExecutionFilePath
                        + "?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
                        + "&amp;mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                        + "&amp;pagenumber={0}";

                    pgrGoogleCheckoutLog.PageURLFormat = pageUrl;
                    pgrGoogleCheckoutLog.ShowFirstLast = true;
                    pgrGoogleCheckoutLog.CurrentIndex = pageNumber;
                    pgrGoogleCheckoutLog.PageSize = pageSize;
                    pgrGoogleCheckoutLog.PageCount = totalPages;

                }
                else
                {
                    pgrGoogleCheckoutLog.Visible = false;
                }

                grdGoogleCheckoutLog.PageIndex = pageNumber;
                grdGoogleCheckoutLog.PageSize = pageSize;
                grdGoogleCheckoutLog.DataSource = reader;
                grdGoogleCheckoutLog.DataBind();
                
            }

            if (grdGoogleCheckoutLog.Rows.Count == 0)
            {
                this.Visible = false;
            }

        }

        private void PopulateLabels()
        {
            litHeading.Text = Resource.GCheckoutNotificationHeading;
            grdGoogleCheckoutLog.Columns[0].HeaderText = Resource.GCheckoutNotificationType;
            grdGoogleCheckoutLog.Columns[1].HeaderText = Resource.GCheckoutSerialNumber;
            grdGoogleCheckoutLog.Columns[2].HeaderText = Resource.GCheckoutOrderNumber;
            grdGoogleCheckoutLog.Columns[3].HeaderText = Resource.GCheckoutBuyerId;
            grdGoogleCheckoutLog.Columns[4].HeaderText = Resource.GCheckoutFullfillState;
            grdGoogleCheckoutLog.Columns[5].HeaderText = Resource.GCheckoutFinanceState;
            grdGoogleCheckoutLog.Columns[6].HeaderText = Resource.GCheckoutCreatedUtc;

        }


        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);

        }


    }
}
