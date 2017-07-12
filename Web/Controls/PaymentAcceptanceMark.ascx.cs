/// Author:					
/// Created:				2008-07-15
/// Last Modified:		    2008-07-20
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace mojoPortal.Web.UI
{
    [Themeable(true)]
    public partial class PaymentAcceptanceMark : UserControl
    {
        private string gCheckoutStyle = "horizontal";
        private CommerceConfiguration commerceConfig = null;
        private bool suppressGoogleCheckout = false;

        //https://www.paypal.com/us/cgi-bin/webscr?cmd=xpt/Marketing/popup/OLCWhatIsPayPaloutside

        public string GCheckoutStyle
        {
            get { return gCheckoutStyle; }
            set { gCheckoutStyle = value; }
        }

        public bool SuppressGoogleCheckout
        {
            get { return suppressGoogleCheckout; }
            set { suppressGoogleCheckout = value; }
        }

        private bool showVisa = true;

        public bool ShowVisa
        {
            get { return showVisa; }
            set { showVisa = value; }
        }


        private bool showMasterCard = true;

        public bool ShowMasterCard
        {
            get { return showMasterCard; }
            set { showMasterCard = value; }
        }

        private bool showAmex = true;

        public bool ShowAmex
        {
            get { return showAmex; }
            set { showAmex = value; }
        }

        private bool showDiscover = true;

        public bool ShowDiscover
        {
            get { return showDiscover; }
            set { showDiscover = value; }
        }

        private bool alwaysShowPayPal = true;
        public bool AlwaysShowPayPal
        {
            get { return alwaysShowPayPal; }
            set { alwaysShowPayPal = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);

            //SetupMarks();

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetupMarks();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void SetupMarks()
        {
            if (commerceConfig == null)
            {
                commerceConfig = SiteUtils.GetCommerceConfig();
            }

            if (commerceConfig == null) { return; }

            if (commerceConfig.CanProcessStandardCards)
            {
                SetupStandardCards();
            }

            // we already have a button for paypal if its enabled and this is sufficient for an acceptance mark
            if (commerceConfig.PayPalIsEnabled)
            {
                SetupPayPal();
            }

            
            

        }

        

        private void SetupPayPal()
        {
            if ((alwaysShowPayPal)||(!Request.IsAuthenticated))
            {
                // only show this if the user is not authenticated
                // because when he is authenticated we show the button
                tblPayPal.Visible = true;
            }
        }

        private void SetupStandardCards()
        {
            // these booleans can be overridden from theme.skin
            imgVisaCard.Visible = showVisa;
            imgMasterCard.Visible = showMasterCard;
            imgAmexCard.Visible = showAmex;
            imgDiscover.Visible = showDiscover;

        }

        //this is just to make it themeable
        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        
    }
}