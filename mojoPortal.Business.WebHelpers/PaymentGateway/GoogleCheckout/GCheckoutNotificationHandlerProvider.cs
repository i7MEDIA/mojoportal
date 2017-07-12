//  Author:                     
//  Created:                    2008-06-24
//	Last Modified:              2008-06-24
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Configuration.Provider;
using GCheckout.AutoGen;
using GCheckout.AutoGen.Extended;
using mojoPortal.Business.Commerce;

namespace mojoPortal.Business.WebHelpers.PaymentGateway
{
    /// <summary>
    ///  
    /// </summary>
    public abstract class GCheckoutNotificationHandlerProvider : ProviderBase
    {
        //public abstract void RebuildIndex(
        //    PageSettings pageSettings,
        //    string indexPath);

        public abstract void HandleNewOrderNotificationExtended(
            string requestXml,
            NewOrderNotificationExtended newOrder, 
            MerchantData merchantData);

        public abstract void HandleOrderStateChangeNotification(
            string requestXml,
            OrderStateChangeNotification notification);

        public abstract void HandleRiskInformationNotification(
            string requestXml,
            RiskInformationNotification notification);

        public abstract void HandleChargeAmountNotification(
            string requestXml,
            ChargeAmountNotification notification);


        public abstract void HandleChargebackAmountNotification(
            string requestXml,
            ChargebackAmountNotification notification);

        public abstract void HandleAuthorizationAmountNotification(
            string requestXml,
            AuthorizationAmountNotification notification);

        public abstract void HandleRefundAmountNotification(
            string requestXml,
            RefundAmountNotification notification);

    }
}
