//  Author:                     
//  Created:                    2008-07-05
//	Last Modified:              2008-07-27
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Collections.Specialized;

namespace mojoPortal.Business.WebHelpers.PaymentGateway
{
    /// <summary>
    /// This only exists because a provider collection can never be empty
    ///  
    /// </summary>
    public class DoNothingPayPalPDTHandlerProvider : PayPalPDTHandlerProvider
    {
        public override string HandleRequestAndReturnUrlForRedirect(
            string rawResponse,
            StringDictionary pdtItems,
            string transactionId,
            PayPalLog standardCheckoutLog)
        {
            // do nothing
            return string.Empty;

        }
    }
}
