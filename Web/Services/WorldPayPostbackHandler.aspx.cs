//  Author:                     
//  Created:                    2012-09-20
//	Last Modified:              2012-10-05
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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Web.Commerce;

namespace mojoPortal.Web.Services
{
    public partial class WorldPayPostbackHandler : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WorldPayPostbackHandler));
        //private CommerceConfiguration commerceConfig = null;

        //protected CommerceConfiguration CommerceConfig
        //{
        //    get
        //    {
        //        if (commerceConfig == null)
        //        {
        //            commerceConfig = SiteUtils.GetCommerceConfig();
        //        }
        //        return commerceConfig;
        //    }
        //}
       

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack) 
            //{
            //    // not expecting get requests here
            //    SiteUtils.RedirectToAccessDeniedPage(this);
            //    return; 
            //} 

            HandleRequest();
        }

        private void HandleRequest()
        {
            // the handler will return html that worldpay will display on their own site so make sure this page doesn't write to the response
            Response.Clear();
            Response.Buffer = true;

            log.Info("Received a post");

            WorldPayPaymentResponse wpResponse = WorldPayPaymentResponse.ParseRequest();

            if (wpResponse == null)
            {
                log.Info("wpResponse object was null");
                
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            CommerceConfiguration commerceConfig = SiteUtils.GetCommerceConfig();
            if (
                (commerceConfig.WorldPayResponsePassword.Length > 0)
                &&(wpResponse.CallbackPW != commerceConfig.WorldPayResponsePassword)
                )
            {
                log.Info("recieved post but the response password was not correct. so redirecting to access denied.");

                //TODO: should we log what was posted?

                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            Guid logGuid = Guid.Empty;
            if (wpResponse.CartId.Length == 36)
            {
                log.Info("wpResponse.CartId was 36 chars");
                logGuid = new Guid(wpResponse.CartId);
            }

            PayPalLog worldPayLog = new PayPalLog(logGuid);

            if (worldPayLog.RowGuid == Guid.Empty)
            {
                // log was not found
                log.Info("WorldPay/PayPal log not found ");
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            log.Info("Provider name is " + worldPayLog.ProviderName);

            WorldPayResponseHandlerProvider handler = WorldPayResponseHandlerProviderManager.Providers[worldPayLog.ProviderName];

            if (handler == null)
            {
                //log the details of the request.

                string serializedResponse = SerializationHelper.SerializeToString(wpResponse);
                log.Info("failed to find a handler for worldpay postback, xml to follow");
                log.Info(serializedResponse);

                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            

            handler.HandleRequest(wpResponse, worldPayLog, this);

        }
    }
}