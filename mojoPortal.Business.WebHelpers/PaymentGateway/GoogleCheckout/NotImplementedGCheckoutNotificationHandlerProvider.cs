//  Author:                     
//  Created:                    2008-06-25
//	Last Modified:              2008-06-25
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using GCheckout.AutoGen;
using GCheckout.AutoGen.Extended;
using mojoPortal.Business.Commerce;

namespace mojoPortal.Business.WebHelpers.PaymentGateway
{
    /// <summary>
    /// This provider does nothing. It exists only because there must always be at least one 
    /// provider in the collection or an error occurs.
    /// </summary>
    public class NotImplementedGCheckoutNotificationHandlerProvider : GCheckoutNotificationHandlerProvider
    {
        public NotImplementedGCheckoutNotificationHandlerProvider()
        { }

        public override void HandleNewOrderNotificationExtended(
            string requestXml,
            NewOrderNotificationExtended newOrder,
            MerchantData merchantData)
        {

        }


        public override void HandleOrderStateChangeNotification(
            string requestXml,
            OrderStateChangeNotification notification)
        {

        }

        public override void HandleRiskInformationNotification(
            string requestXml,
            RiskInformationNotification notification)
        {


        }


        public override void HandleChargeAmountNotification(
            string requestXml,
            ChargeAmountNotification notification)
        {


        }

        public override void HandleChargebackAmountNotification(
            string requestXml,
            ChargebackAmountNotification notification)
        {

        }


        public override void HandleAuthorizationAmountNotification(
            string requestXml,
            AuthorizationAmountNotification notification)
        {


        }


        public override void HandleRefundAmountNotification(
            string requestXml,
            RefundAmountNotification notification)
        {


        }


    }
}
