/// Author:					
/// Created:				2008-07-01
/// Last Modified:			2009-12-16
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
using System.Web.UI;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using Resources;

namespace mojoPortal.Web.UI
{
    public partial class PayPalLogList : UserControl
    {
        private int pageId = -1;
        private int moduleId = -1;
        //private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 10;
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
            if (cartGuid == Guid.Empty)
            {
                this.Visible = false;
                return;
            }

            using (IDataReader reader = PayPalLog.GetByCart(cartGuid))
            {
                pgrCheckoutLog.Visible = false;

                grdCheckoutLog.PageIndex = pageNumber;
                grdCheckoutLog.PageSize = pageSize;
                grdCheckoutLog.DataSource = reader;
                grdCheckoutLog.DataBind();
            }

            if (grdCheckoutLog.Rows.Count == 0)
            {
                this.Visible = false;
            }

        }

        private void PopulateLabels()
        {
            litHeading.Text = Resource.PayPalLogHeading;
            grdCheckoutLog.Columns[0].HeaderText = Resource.PayPalLogRequestType;
            grdCheckoutLog.Columns[1].HeaderText = Resource.PayPalPaymentType;
            grdCheckoutLog.Columns[2].HeaderText = Resource.PayPalPaymentStatus;
            grdCheckoutLog.Columns[3].HeaderText = Resource.PayPalTransactionId;
            grdCheckoutLog.Columns[4].HeaderText = Resource.PayPalCartTotal;
            grdCheckoutLog.Columns[5].HeaderText = Resource.PayPalTaxAmount;
            grdCheckoutLog.Columns[6].HeaderText = Resource.PayPalChargeTotal;
            grdCheckoutLog.Columns[7].HeaderText = Resource.PayPalFeeAmount;
            grdCheckoutLog.Columns[8].HeaderText = Resource.PayPalSettlementAmount;
            grdCheckoutLog.Columns[9].HeaderText = Resource.PayPalCreatedUtc;

        }


        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);

        }

    }
}