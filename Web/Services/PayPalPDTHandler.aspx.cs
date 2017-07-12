/// Author:                     
/// Created:                    2008-07-05
///	Last Modified:              2010-11-10
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
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.Services
{
    public partial class PayPalPDTHandler : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PayPalPDTHandler));

        private string transactionId = string.Empty;
        private string custom = string.Empty;
        private string lastResortRedirectUrl = string.Empty;
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
            LoadParams();
            HandleRequest();

        }

        private void HandleRequest()
        {
            if (transactionId.Length == 0)
            {
                
                WebUtils.SetupRedirect(this, lastResortRedirectUrl);
                return;
            }

            try
            {
                //Log the querystring in case we have to investigate
                //Logger.Information(Request.QueryString.ToString());

                transactionId = HttpUtility.UrlDecode(transactionId);
                custom = HttpUtility.UrlDecode(custom);
                
                string pdtResponse = Verify(transactionId);
                if (pdtResponse.StartsWith("SUCCESS"))
                {
                    
                    string redirectUrl = string.Empty;

                    Guid logGuid = Guid.Empty;
                    if (custom.Length == 36)
                    {
                        logGuid = new Guid(custom);
                    }

                    PayPalLog standardCheckoutLog = new PayPalLog(logGuid);

                    if ((standardCheckoutLog != null)&&(standardCheckoutLog.PDTProviderName.Length > 0))
                    {
                        PayPalPDTHandlerProvider provider 
                            = PayPalPDTHandlerProviderManager.Providers[standardCheckoutLog.PDTProviderName];

                        if (provider != null)
                        {
                            redirectUrl = provider.HandleRequestAndReturnUrlForRedirect(
                                pdtResponse,
                                PayPalStandardPaymentGateway.GetPDTValues(pdtResponse),
                                transactionId,
                                standardCheckoutLog);

                            if (redirectUrl.Length > 0)
                            {
                                WebUtils.SetupRedirect(this, redirectUrl);
                                return;

                            }
                            else
                            {
                                // no redeirectUrl returned from provider
                                //TODO: what? log  it?

                                WebUtils.SetupRedirect(this, lastResortRedirectUrl);
                                return;
                            }
                        }
                        else
                        {
                            // provider not found
                            //log  it
                            PayPalLog unhandledLog = new PayPalLog();
                            unhandledLog.ProviderName = "unhandled";
                            unhandledLog.RawResponse = pdtResponse;
                            unhandledLog.Save();

                            log.Info("invalid ptd request no valid provider found " + Request.Url.ToString());

                            WebUtils.SetupRedirect(this, lastResortRedirectUrl);
                            return;

                        }


                    }
                    else
                    {
                        // provider not specified on StandardCheckoutLog
                        //TODO: what? log  it?
                        PayPalLog unhandledLog = new PayPalLog();
                        unhandledLog.ProviderName = "unhandled";
                        unhandledLog.RawResponse = pdtResponse;
                        unhandledLog.Save();

                        log.Info("invalid ptd request no valid provider found " + Request.Url.ToString());

                        WebUtils.SetupRedirect(this, lastResortRedirectUrl);
                        return;

                    }
                   
                    


                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                //TODO: show generic error on the page

                
            }


        }

        public string Verify(string transactionId)
        {
            
            PayPalStandardPaymentGateway gateway 
                = new PayPalStandardPaymentGateway(
                    CommerceConfig.PayPalStandardUrl,
                    CommerceConfig.PayPalStandardEmailAddress,
                    CommerceConfig.PayPalStandardPDTId);

            gateway.TransactionId = transactionId;

            return gateway.ValidatePDT();
        }

        

        
        private void LoadParams()
        {
            lastResortRedirectUrl = SiteUtils.GetNavigationSiteRoot();

            if (Request.QueryString["tx"] != null)
            {
                transactionId = Request.QueryString["tx"];
            }

            if (Request.QueryString["cm"] != null)
            {
                custom = Request.QueryString["cm"];
            }

            //try to solve this paypal bug
            //http://www.mojoportal.com/Forums/Thread.aspx?thread=6755&mid=34&pageid=5&ItemID=5&pagenumber=1#post27617
            // example malformed url from paypal note that the query string params are duplicated:
            //https://siteroot/Services/PayPalPDTHandler.aspx?tx=9K579114XU325245S&st=Completed&amt=9.90&cc=USD&cm=3c58cf8e%2d09e3%2d44af%2dae46%2d3ffb7f682820&item_number=&tx=9K579114XU325245S&st=Completed&amt=9.90&cc=USD&cm=3c58cf8e%2d09e3%2d44af%2dae46%2d3ffb7f682820&item_number=
            
            if (transactionId.Contains(","))
            {
                string[] trArray = transactionId.Split(',');
                transactionId = trArray[0];
            }

            if (custom.Contains(","))
            {
                string[] customArray = custom.Split(',');
                custom = customArray[0];
            }

        }


    }
}
