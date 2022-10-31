/// Author:					
/// Created:				2007-03-09
/// Last Modified:			2013-12-19
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
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers.PaymentGateway;

namespace mojoPortal.Web.Services
{
   
    /// <summary>
    /// the return url for paypal express checkout
    /// the token provided by paypal is used to lookup the paypal log which contains information about the provider that should handle
    /// this request as well as the feature specific serialized cart. This allows different features to use the same return url but each can process their own results
    /// by implementing a provider and registering it by config under /Setup/ProviderConfig/paypalreturnhandlers
    /// and by logging the needed info into the paypal log prior to redirecting the user to paypal
    /// webstore feature has a reference implementation
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class PayPalReturnHandler : IHttpHandler
    {
        private string payPalToken = string.Empty;
        private string payPalPayerId = string.Empty;
        private static readonly ILog log = LogManager.GetLogger(typeof(PayPalReturnHandler));

        public void ProcessRequest(HttpContext context)
        {
            LoadParams(context);
            HandleRequest(context);
        }

        private void HandleRequest(HttpContext context)
        {

            if (payPalToken.Length == 0)
            {
                log.Info("invalid request no PayPalLog token provided");
                SiteUtils.RedirectToSiteRoot();
                return;
            }

            PayPalLog setExpressCheckoutLog = PayPalLog.GetSetExpressCheckout(payPalToken);

            if (setExpressCheckoutLog == null)
            {
                log.Info("invalid request no PayPalLog found for token " + payPalToken);
                SiteUtils.RedirectToSiteRoot();
                return;

            }

            
            PayPalReturnHandlerProvider provider = null;
            string returnUrl = string.Empty;

            try
            {
                provider = PayPalReturnHandlerManager.Providers[setExpressCheckoutLog.ProviderName];
            }
            catch(TypeInitializationException ex)
            {
                log.Error(ex);
            }
            
            if (provider != null)
            {
                returnUrl = provider.HandleRequestAndReturnUrlForRedirect(
                    context,
                    payPalToken,
                    payPalPayerId,
                    setExpressCheckoutLog);

            }
            else
            {
                log.Info("could not find PayPalReturnHandlerProvider " + setExpressCheckoutLog.ProviderName);

            }

            if (returnUrl.Length == 0) 
            {
                log.Info("no return url determined so redirecting to site root");
                returnUrl = SiteUtils.GetNavigationSiteRoot();
                
            }

            context.Response.Redirect(returnUrl);



        }

        private void LoadParams(HttpContext context)
        {
            if (context.Request.Params["token"] != null)
            {
                payPalToken = context.Request.Params["token"];
                if (payPalToken.Length > 0)
                {
                    payPalToken = HttpUtility.UrlDecode(payPalToken);
                }
            }

            if (context.Request.Params["PayerID"] != null)
            {
                payPalPayerId = context.Request.Params["PayerID"];
                if (payPalPayerId.Length > 0)
                {
                    payPalPayerId = HttpUtility.UrlDecode(payPalPayerId);
                }
            }

            //commerceConfig = StoreHelper.GetCommerceConfig();

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
