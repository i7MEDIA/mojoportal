using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using PaymentGateway.Common;

namespace WebStore.Business
{
    /// <summary>
    /// Author:					Joe Audette
    /// Created:				2/13/2007
    /// Last Modified:		    2/14/2007
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public static class PaymentGatewayFactory
    {
        public static IPaymentGateway GetPaymentGateway(Guid storeGuid)
        {
            // TODO: implement alternate gateways and lookup for store specific gateway
            // for now just Authorize.NET

            return GetAuthorizeNetGateway(storeGuid);


        }

        #region Private Methods

        private static IPaymentGateway GetAuthorizeNetGateway(Guid storeGuid)
        {
            string gatewayUserName = ConfigurationManager.AppSettings["AuthorizeNetAPILogin"];
            string gatewayTransactionKey = ConfigurationManager.AppSettings["AuthorizeNetAPITransactionKey"];

            mojoGateway.AuthorizeNet.PaymentGateway gateway
                = new mojoGateway.AuthorizeNet.PaymentGateway(gatewayUserName, gatewayTransactionKey);

            return gateway;

        }

        #endregion

    }
}
