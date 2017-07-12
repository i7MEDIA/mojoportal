// Author:		        
// Created:             2012-09-22
// Last Modified:       2012-10-01
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Web.Framework;
using System;
using System.Globalization;
using System.Web;

namespace mojoPortal.Web.Commerce
{
    /// <summary>
    /// represents the form post variables posted back from WorldPay
    /// </summary>
    public class WorldPayPaymentResponse
    {
        private WorldPayPaymentResponse()
        {

        }

        public static WorldPayPaymentResponse ParseRequest()
        {
            if (HttpContext.Current == null) { return null; }
            if (HttpContext.Current.Request == null) { return null; }

            WorldPayPaymentResponse wpResponse = new WorldPayPaymentResponse();

            WebUtils.TryLoadRequestParam<string>("instId", out wpResponse.installationId);
            WebUtils.TryLoadRequestParam<string>("cartId", out wpResponse.cartId);
            WebUtils.TryLoadRequestParam<string>("M_custom", out wpResponse.customData);

            WebUtils.TryLoadRequestParam<string>("currency", out wpResponse.currency);

            CultureInfo currencyCulture = CurrencyHelper.CultureInfoFromCurrencyISO(wpResponse.currency);
            if (currencyCulture == null) { currencyCulture = CultureInfo.CurrentCulture; }

            string amountString;
            WebUtils.TryLoadRequestParam<string>("amount", out amountString);
            if (!string.IsNullOrEmpty(amountString))
            {
                try
                {
                    wpResponse.amount = Convert.ToDecimal(amountString, currencyCulture);
                }
                catch (FormatException) { }
                catch (OverflowException) { }
            }

            WebUtils.TryLoadRequestParam<string>("authAmountString", out wpResponse.authAmountString);
            WebUtils.TryLoadRequestParam<string>("authMode", out wpResponse.authMode);
            WebUtils.TryLoadRequestParam<string>("testMode", out wpResponse.testMode);
            WebUtils.TryLoadRequestParam<string>("name", out wpResponse.name);
            WebUtils.TryLoadRequestParam<string>("address1", out wpResponse.address1);
            WebUtils.TryLoadRequestParam<string>("address2", out wpResponse.address2);
            WebUtils.TryLoadRequestParam<string>("address3", out wpResponse.address3);
            WebUtils.TryLoadRequestParam<string>("town", out wpResponse.town);
            WebUtils.TryLoadRequestParam<string>("region", out wpResponse.region);
            WebUtils.TryLoadRequestParam<string>("postcode", out wpResponse.postcode);
            WebUtils.TryLoadRequestParam<string>("country", out wpResponse.country);
            WebUtils.TryLoadRequestParam<string>("countryString", out wpResponse.countryString);
            WebUtils.TryLoadRequestParam<string>("tel", out wpResponse.tel);
            WebUtils.TryLoadRequestParam<string>("fax", out wpResponse.fax);
            WebUtils.TryLoadRequestParam<string>("email", out wpResponse.email);
            WebUtils.TryLoadRequestParam<string>("delvName", out wpResponse.delvName);
            WebUtils.TryLoadRequestParam<string>("delvAddress1", out wpResponse.delvAddress1);
            WebUtils.TryLoadRequestParam<string>("delvAddress2", out wpResponse.delvAddress2);
            WebUtils.TryLoadRequestParam<string>("delvAddress1", out wpResponse.delvAddress3);
            WebUtils.TryLoadRequestParam<string>("delvTown", out wpResponse.delvTown);
            WebUtils.TryLoadRequestParam<string>("delvRegion", out wpResponse.delvRegion);
            WebUtils.TryLoadRequestParam<string>("delvPostcode", out wpResponse.delvPostcode);
            WebUtils.TryLoadRequestParam<string>("delvCountry", out wpResponse.delvCountry);
            WebUtils.TryLoadRequestParam<string>("delvCountryString", out wpResponse.delvCountryString);
            WebUtils.TryLoadRequestParam<string>("compName", out wpResponse.compName);
            WebUtils.TryLoadRequestParam<string>("transId", out wpResponse.transId);
            WebUtils.TryLoadRequestParam<string>("transStatus", out wpResponse.transStatus);
            WebUtils.TryLoadRequestParam<string>("transTime", out wpResponse.transTime);
            WebUtils.TryLoadRequestParam<string>("authCurrency", out wpResponse.authCurrency);

            CultureInfo authCurrencyCulture = CurrencyHelper.CultureInfoFromCurrencyISO(wpResponse.authCurrency);
            if (authCurrencyCulture == null) { authCurrencyCulture = CultureInfo.CurrentCulture; }

            string authAmountString;
            WebUtils.TryLoadRequestParam<string>("authAmount", out authAmountString);
            if (!string.IsNullOrEmpty(authAmountString))
            {
                try
                {
                    wpResponse.authAmount = Convert.ToDecimal(amountString, authCurrencyCulture);
                }
                catch (FormatException) { }
                catch (OverflowException) { }
            }

            //
            

            WebUtils.TryLoadRequestParam<string>("rawAuthMessage", out wpResponse.rawAuthMessage);
            WebUtils.TryLoadRequestParam<string>("callbackPW", out wpResponse.callbackPW);
            WebUtils.TryLoadRequestParam<string>("cardType", out wpResponse.cardType);
            WebUtils.TryLoadRequestParam<string>("AVS", out wpResponse.avs);
            WebUtils.TryLoadRequestParam<string>("wafMerchMessage", out wpResponse.wafMerchMessage);
            WebUtils.TryLoadRequestParam<string>("authentication", out wpResponse.authentication);
            WebUtils.TryLoadRequestParam<string>("ipAddress", out wpResponse.ipAddress);
            WebUtils.TryLoadRequestParam<string>("charenc", out wpResponse.charenc);
            WebUtils.TryLoadRequestParam<string>("futurePayId", out wpResponse.futurePayId);
            WebUtils.TryLoadRequestParam<string>("futurePayStatusChange", out wpResponse.futurePayStatusChange);


            if(IsValidResponse(wpResponse))
            {
                return wpResponse;
            }

            return null;
        }

        //private static string GetFormParameter(HttpRequest request, string paramName)
        //{

        //    return string.Empty;
        //}

        private static bool IsValidResponse(WorldPayPaymentResponse wpResponse)
        {
            //TODO: make sure expected params exist and are valid

            return true;
        }

        private string installationId = string.Empty;

        public string InstallationId
        {
            get { return installationId; }
        }

        private string cartId = string.Empty;
        /// <summary>
        /// note that we actually pass the PayPalLog guid not the cart id
        /// we then deserialize the cart form tha PayPsalLog to ensure it has not been modified
        /// since the user left our site and went to WorldPay
        /// </summary>
        public string CartId
        {
            get { return cartId; }
        }

        private string customData = string.Empty;

        public string CustomData
        {
            get { return customData; }
        }

        private decimal amount = 0;
        /// <summary>
        /// A decimal number giving the cost of the purchase in terms of the major currency unit e.g. 12.56 
        /// would mean 12 pounds and 56 pence if the currency were GBP (Pounds Sterling).
        /// </summary>
        public decimal Amount
        {
            get { return amount; }
        }

        private string authAmountString = string.Empty;

        /// <summary>
        /// An HTML string produced from the amount and currency that were submitted to initiate the payment.
        /// </summary>
        public string AuthAmountString
        {
            get { return authAmountString; }
        }

        private string currency = string.Empty;
        /// <summary>
        /// 3 letter ISO code for the currency of this payment.
        /// </summary>
        public string Currency
        {
            get { return currency; }
        }

        private string authMode = string.Empty;
        /// <summary>
        /// Specifies the authorisation mode used. The values are "A" for a full auth, or "E" for a pre-auth.
        /// </summary>
        public string AuthMode
        {
            get { return authMode; }
        }

        private string testMode = "0";
        /// <summary>
        /// A value of 100 specifies a test payment and a value of 0 (zero) specifies a live payment. 
        /// Specify the test result you want by entering REFUSED, AUTHORISED, ERROR or CAPTURED in the name parameter.
        /// </summary>
        public string TestMode
        {
            get { return testMode; }
        }

        private string name = string.Empty;
        /// <summary>
        /// The Shopper's full name, including any title, personal name and family name. 
        /// Note: If your purchase token does not contain a name value, 
        /// the name that the cardholder enters on the payment page will be returned to you.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        private string address1 = string.Empty;
        /// <summary>
        /// The first line of the shopper's address. 
        /// Separators (including new line) used in this parameter are encoded as ASCII characters.
        /// </summary>
        public string Address1
        {
            get { return address1; }
        }

        private string address2 = string.Empty;
        /// <summary>
        /// The second line of the shopper's address.
        /// </summary>
        public string Address2
        {
            get { return address2; }
        }

        private string address3 = string.Empty;
        /// <summary>
        /// The third line of the shopper's address.
        /// </summary>
        public string Address3
        {
            get { return address3; }
        }

        private string town = string.Empty;
        /// <summary>
        /// Shopper’s city or town.
        /// </summary>
        public string Town
        {
            get { return town; }
        }


        private string region = string.Empty;
        /// <summary>
        /// Shopper’s country/region/state or area
        /// </summary>
        public string Region
        {
            get { return region; }
        }

        private string postcode = string.Empty;
        /// <summary>
        /// Shopper's postcode.
        /// </summary>
        public string Postcode
        {
            get { return postcode; }
        }

        private string country = string.Empty;
        /// <summary>
        /// Shopper's country, as 2 character ISO code, uppercase.
        /// </summary>
        public string Country
        {
            get { return country; }
        }

        private string countryString = string.Empty;
        /// <summary>
        /// The full name of the country, derived from the country code submitted 
        /// or supplied by the shopper in the language used by the shopper on the payment page.
        /// </summary>
        public string CountryString
        {
            get { return countryString; }
        }

        private string tel = string.Empty;
        /// <summary>
        /// Shopper's telephone number.
        /// </summary>
        public string Tel
        {
            get { return tel; }
        }

        private string fax = string.Empty;
        /// <summary>
        /// Shopper's fax number.
        /// </summary>
        public string Fax
        {
            get { return fax; }
        }

        private string email = string.Empty;
        /// <summary>
        /// Shopper's email address.
        /// </summary>
        public string Email
        {
            get { return email; }
        }

        private string delvName = string.Empty;
        /// <summary>
        /// Shopper's delivery name. 
        /// Note: The withDelivery parameter must be submitted in the purchase 
        /// token for you to receive this parameter in the Payment Message.
        /// </summary>
        public string DelvName
        {
            get { return delvName; }
        }

        private string delvAddress1 = string.Empty;
        /// <summary>
        /// Shopper's delivery address1. 
        /// Note: The withDelivery parameter must be submitted in the 
        /// purchase token for you to receive this parameter in the Payment Message.
        /// </summary>
        public string DelvAddress1
        {
            get { return delvAddress1; }
        }

        private string delvAddress2 = string.Empty;
        /// <summary>
        /// Shopper's delivery address 2. 
        /// Note: The withDelivery parameter must be submitted in the 
        /// purchase token for you to receive this parameter in the Payment Message.
        /// </summary>
        public string DelvAddress2
        {
            get { return delvAddress2; }
        }

        private string delvAddress3 = string.Empty;
        /// <summary>
        /// Shopper's delivery address 3. 
        /// Note: The withDelivery parameter must be submitted in the 
        /// purchase token for you to receive this parameter in the Payment Message.
        /// </summary>
        public string DelvAddress3
        {
            get { return delvAddress3; }
        }

        private string delvTown = string.Empty;
        /// <summary>
        /// Shopper's delivery town or city. 
        /// Note: The withDelivery parameter must be submitted in the 
        /// purchase token for you to receive this parameter in the Payment Message.
        /// </summary>
        public string DelvTown
        {
            get { return delvTown; }
        }

        private string delvRegion = string.Empty;

        /// <summary>
        /// Shopper's delivery county/state/region. 
        /// Note: The withDelivery parameter must be submitted in the 
        /// purchase token for you to receive this parameter in the Payment Message.
        /// </summary>
        public string DelvRegion
        {
            get { return delvRegion; }
        }

        private string delvPostcode = string.Empty;
        /// <summary>
        /// Shopper's delivery postcode. 
        /// Note: The withDelivery parameter must be submitted in the 
        /// purchase token for you to receive this parameter in the Payment Message.
        /// </summary>
        public string DelvPostcode
        {
            get { return delvPostcode; }
        }

        private string delvCountry = string.Empty;
        /// <summary>
        /// Shopper's delivery country, as 2 character ISO code, uppercase. 
        /// Note: The withDelivery parameter must be submitted in the 
        /// purchase token for you to receive this parameter in the Payment Message.
        /// </summary>
        public string DelvCountry
        {
            get { return delvCountry; }
        }

        private string delvCountryString = string.Empty;
        /// <summary>
        /// The full name of the country, derived from the country code submitted 
        /// or supplied by the shopper for the delivery address in the language used by the shopper on the payment page.
        /// </summary>
        public string DelvCountryString
        {
            get { return delvCountryString; }
        }

        private string compName = string.Empty;
        /// <summary>
        /// Name of the company associated with this installation.
        /// </summary>
        public string CompName
        {
            get { return compName; }
        }


        private string transId = string.Empty;
        /// <summary>
        /// The ID for the transaction.
        /// </summary>
        public string TransId
        {
            get { return transId; }
        }

        private string transStatus = string.Empty;
        /// <summary>
        /// Result of the transaction - "Y" for a successful payment authorisation, "C" for a cancelled payment.
        /// </summary>
        public string TransStatus
        {
            get { return transStatus; }
        }

        private string transTime = string.Empty;
        /// <summary>
        /// Time of the transaction in milliseconds since the start of 1970 GMT. 
        /// This is the standard system date in Java, and is also 1000x the standard C time_t time.
        /// </summary>
        public string TransTime
        {
            get { return transTime; }
        }

        private decimal authAmount = 0;
        /// <summary>
        /// Amount that the transaction was authorised for, in the currency given as authCurrency.
        /// </summary>
        public decimal AuthAmount
        {
            get { return authAmount; }
        }

        private string authCurrency = string.Empty;
        /// <summary>
        /// The currency used for authorisation.
        /// </summary>
        public string AuthCurrency
        {
            get { return authCurrency; }
        }


        private string rawAuthMessage = string.Empty;
        /// <summary>
        /// The text received from the bank summarising the different states listed below:
        /// cardbe.msg.authorised - Make Payment (test or live)
        /// trans.cancelled - Cancel Purchase (test or live)
        /// </summary>
        public string RawAuthMessage
        {
            get { return rawAuthMessage; }
        }

        //private string rawAuthCode = string.Empty;
        ///// <summary>
        ///// A single-character bank authorisation code. This is retained for backward compatibility. 
        ///// 'A' means 'authorised' and is directly equivalent to transStatus='Y'.
        ///// </summary>
        //public string RawAuthCode
        //{
        //    get { return rawAuthCode; }
        //}

        private string callbackPW = string.Empty;
        /// <summary>
        /// The Payment Response password set in the Merchant Interface.
        /// </summary>
        public string CallbackPW
        {
            get { return callbackPW; }
        }

        private string cardType = string.Empty;
        /// <summary>
        /// The type of payment method used by the shopper.
        /// </summary>
        public string CardType
        {
            get { return cardType; }
        }

        //private string countryMatch = string.Empty;
        ///// <summary>
        ///// A single character describing the result of the comparison of the cardholder country and the issue country of the card used by the shopper (where available). Note that this parameter is retained for backward compatibility - equivalent information is now provided as part of the AVS results. The result possible values are:
        ///// </summary>
        //public string CountryMatch
        //{
        //    get { return countryMatch; }
        //}

        private string avs = string.Empty;
        /// <summary>
        /// A 4-character string giving the results of 4 internal fraud-related checks. 
        /// The characters respectively give the results of the following checks:
        /// 1st character - Card Verification Value check
        /// 2nd character - postcode AVS check
        /// 3rd character - address AVS check
        /// 4th character - country comparison check (see also countryMatch)
        /// The possible values for each result character are:
        /// 0 - Not supported
        /// 1 - Not checked
        /// 2 - Matched
        /// 4 - Not matched
        /// 8 - Partially matched
        /// </summary>
        public string AVS
        {
            get { return avs; }
        }

        private string wafMerchMessage = string.Empty;
        /// <summary>
        /// If you have the Risk Management service enabled, you will receive 
        /// one of the fraud messages listed below:
        /// waf.warning = Warning
        /// waf.caution = Caution
        /// For more detailed explanation about the fraud message, refer to the Risk Management Service Guide.
        /// </summary>
        public string WafMerchMessage
        {
            get { return wafMerchMessage; }
        }

        private string authentication = string.Empty;
        /// <summary>
        /// If you have enrolled to the Verified By Visa, MasterCard SecureCode or 
        /// American Express SafeKey authentication schemes you will receive one of 
        /// the authentication messages listed below:
        /// ARespH.card.authentication.0 = Cardholder authenticated
        /// ARespH.card.authentication.1 = Cardholder/Issuing bank not enrolled for authentication
        /// ARespH.card.authentication.6 = Cardholder authentication not available
        /// ARespH.card.authentication.7 = Cardholder did not complete authentication
        /// ARespH.card.authentication.9 = Cardholder authentication failed
        /// For more detailed explanation about the authentication messages, refer to
        /// the Card Authentication Guide.
        /// </summary>
        public string Authentication
        {
            get { return authentication; }
        }

        private string ipAddress = string.Empty;
        /// <summary>
        /// The IP address from which the purchase token was submitted.
        /// </summary>
        public string IpAddress
        {
            get { return ipAddress; }
        }

        private string charenc = string.Empty;
        /// <summary>
        /// The character encoding used to display the payment page to the shopper.
        /// </summary>
        public string Charenc
        {
            get { return charenc; }
        }

        private string futurePayId = string.Empty;
        /// <summary>
        /// The ID for the Recurring Payments agreement.
        /// </summary>
        public string FuturePayId
        {
            get { return futurePayId; }
        }

        private string futurePayStatusChange = string.Empty;
        /// <summary>
        /// The status of the agreement, set to either Merchant Cancelled or 
        /// Customer Cancelled depending if the merchant or the shopper has cancelled the agreement.
        /// </summary>
        public string FuturePayStatusChange
        {
            get { return futurePayStatusChange; }
        }


         
    }
}