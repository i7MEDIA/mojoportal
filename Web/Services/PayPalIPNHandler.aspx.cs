/// Author:                     
/// Created:                    2008-07-05
///	Last Modified:              2008-07-05
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Web;
using System.Web.UI;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers.PaymentGateway;

namespace mojoPortal.Web.Services
{
    public partial class PayPalIPNHandler : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PayPalIPNHandler));
        private CommerceConfiguration commerceConfig = null;

        protected CommerceConfiguration CommerceConfig
        {
            get
            {
                if (commerceConfig == null)
                {
                    commerceConfig = SiteUtils.GetCommerceConfig();
                }
                return commerceConfig;
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            HandleRequest();
        }

        private void HandleRequest()
        {
            string formValues = string.Empty;
            try
            {
                
                //Per PayPal Order Management / Integration Guide Pg.25
                //we have to validate the price, transactionId, etc.
                string transactionId = HttpUtility.UrlDecode(Request.Form["txn_id"].ToString());
                string custom = HttpUtility.UrlDecode(Request.Form["custom"].ToString());
                
                byte[] buffer = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
                formValues = System.Text.Encoding.ASCII.GetString(buffer);

                string response = Verify(formValues);
                
                if (response.StartsWith("VERIFIED"))
                {

                    Guid standardCheckoutLogGuid = Guid.Empty;
                    if (custom.Length == 36)
                    {
                        standardCheckoutLogGuid = new Guid(custom);
                    }

                    PayPalLog standardCheckoutLog = new PayPalLog(standardCheckoutLogGuid);

                    bool result = false;

                    if ((standardCheckoutLog != null) && (standardCheckoutLog.IPNProviderName.Length > 0))
                    {
                        PayPalIPNHandlerProvider provider
                            = PayPalIPNHandlerProviderManager.Providers[standardCheckoutLog.IPNProviderName];

                        if (provider != null)
                        {
                            result = provider.HandleRequest(
                                transactionId,
                                Request.Form,
                                standardCheckoutLog);

                        }
                    }

                    //if (result)
                    //{

                    //    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    //    Response.Flush();
                    //    Response.End();
                    //}
                    //else
                    //{
                    //    // TODO: what? log it?

                    //    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    //    Response.Flush();
                    //    Response.End();

                    //}


                    
                }
                else
                {
                    // failed verification 
                    // TODO: what log it?


                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                log.Error(formValues, ex);
            }

        }

        public string Verify(string ipnForm)
        {
           
            PayPalStandardPaymentGateway gateway
                = new PayPalStandardPaymentGateway(
                    CommerceConfig.PayPalStandardUrl,
                    CommerceConfig.PayPalStandardEmailAddress,
                    CommerceConfig.PayPalStandardPDTId);

            gateway.IPNForm = ipnForm;

            return gateway.ValidateIPN();
        }

        

    }
}
