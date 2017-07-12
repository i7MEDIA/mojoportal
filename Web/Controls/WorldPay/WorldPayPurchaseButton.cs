//  Author:                     
//  Created:                    2012-09-20
//	Last Modified:              2013-10-07
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using Resources;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    public class WorldPayPurchaseButton : mojoButton
    {
        private string amountString = string.Empty;

        private CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        public CultureInfo CurrencyCulture
        {
            get { return currencyCulture; }
            set { currencyCulture = value; }
        }

        private string instId = string.Empty;
        [Themeable(false)]
        public string InstId
        {
            get { return instId; }
            set { instId = value; }
        }

        private string merchantCode = string.Empty;

        /// <summary>
        /// As a general rule, we open one merchant code (account) per currency set that you
        /// process. However, you may need to consider using preferred merchant codes if you
        /// have a number of merchant codes with identical characteristics but where they will
        /// be used for different purposes.
        /// For instance, you may have a merchant code for software sales and another for
        /// hardware sales - so, order details submitted to us for software will need to specify
        /// the software merchant code, and order details submitted for hardware will need to
        /// specify the hardware merchant code.
        /// </summary>
        [Themeable(false)]
        public string MerchantCode
        {
            get { return merchantCode; }
            set { merchantCode = value; }
        }

        private string cartId = string.Empty;
        [Themeable(false)]
        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }

        private decimal amount = 0;
        [Themeable(false)]
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        private string currencyCode = "USD";
        [Themeable(false)]
        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }
        

        private bool useTestServer = false;
        [Themeable(false)]
        public bool UseTestServer
        {
            get { return useTestServer; }
            set { useTestServer = value; }
        }

        private string testServerPostbackUrl = "https://secure-test.worldpay.com/wcc/purchase";

        public string TestServerPostbackUrl
        {
            get { return testServerPostbackUrl; }
            set { testServerPostbackUrl = value; }
        }

        private string productionServerPostbackUrl = "https://secure.worldpay.com/wcc/purchase";

        public string ProductionServerPostbackUrl
        {
            get { return productionServerPostbackUrl; }
            set { productionServerPostbackUrl = value; }
        }

        private int authValidFrom = -1;

        /// <summary>
        /// This specifies a time window within
        /// which the purchase must (or must not)
        /// be completed, eg. if the purchase is a
        /// time-limited special offer. Each of these
        /// parameters is a time in milliseconds
        /// since 1 January 1970 GMT - a Java
        /// long date value (as from
        /// System.currentTimeMillis() or Date.getTime()), or 1000* a C
        /// time_t. If from to, then the authorisation must complete between
        /// those two times. If tofrom, then the authorisation must complete either
        /// before the to time or after the from time. Either may be zero or omitted to
        /// give the effect of a simple "not before" or "not after" constraint. If both are zero
        /// or omitted, there are no restrictions on how long a shopper can spend making
        /// their purchase (although our server will time-out their session if it is idle for too long).
        /// </summary>
        [Themeable(false)]
        public int AuthValidFrom
        {
            get { return authValidFrom; }
            set { authValidFrom = value; }
        }

        private int authValidTo = -1;
        [Themeable(false)]
        public int AuthValidTo
        {
            get { return authValidTo; }
            set { authValidTo = value; }
        }

        private string lang = string.Empty;
        /// <summary>
        /// 6 char
        /// The shopper's language choice, as a 2-character ISO 639 code, with optional regionalisation using 2-
        /// character country code separated by hyphen. For example "en-GB" specifies UK English. The shopper
        /// can always choose a language on our pages or via browser preferences but if your site has already
        /// made this choice then you can make things more convenient by submitting it to us.
        /// </summary>
        public string Lang
        {
            get { return lang; }
            set { lang = value; }
        }

        private bool noLanguageMenu = false;
        /// <summary>
        /// needs no value
        /// This suppresses the display of the language menu if you have a choice of languages enabled for your
        /// installation but want the choice to be defined by the value of the lang parameter that you submit. Please
        /// contact your local Technical Support department if you would like this facility enabled on your account.
        /// </summary>
        [Themeable(false)]
        public bool NoLanguageMenu
        {
            get { return noLanguageMenu; }
            set { noLanguageMenu = value; }
        }

        private bool withDelivery = false;
        /// <summary>
        /// needs no value
        /// Displays input fields for delivery address and mandate that they be filled in.
        /// </summary>
        [Themeable(false)]
        public bool WithDelivery
        {
            get { return withDelivery; }
            set { withDelivery = value; }
        }


        private string md5Secret = string.Empty;
        [Themeable(false)]
        public string Md5Secret
        {
            get { return md5Secret; }
            set { md5Secret = value; }
        }

        private string orderDescription = string.Empty;
        [Themeable(false)]
        public string OrderDescription
        {
            get { return orderDescription; }
            set { orderDescription = value; }
        }

        private string customerName = string.Empty;
        [Themeable(false)]
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }

        private string customerEmail = string.Empty;
        [Themeable(false)]
        public string CustomerEmail
        {
            get { return customerEmail; }
            set { customerEmail = value; }
        }

        private string address1 = string.Empty;
        [Themeable(false)]
        public string Address1
        {
            get { return address1; }
            set { address1 = value; }
        }

        private string address2 = string.Empty;
        [Themeable(false)]
        public string Address2
        {
            get { return address2; }
            set { address2 = value; }
        }

        private string address3 = string.Empty;
        [Themeable(false)]
        public string Address3
        {
            get { return address3; }
            set { address3 = value; }
        }

        private string town = string.Empty;
        [Themeable(false)]
        public string Town
        {
            get { return town; }
            set { town = value; }
        }

        private string region = string.Empty;
        [Themeable(false)]
        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        private string postalCode = string.Empty;
        [Themeable(false)]
        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }

        private string country = string.Empty;
        [Themeable(false)]
        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        private string customerPhone = string.Empty;
        [Themeable(false)]
        public string CustomerPhone
        {
            get { return customerPhone; }
            set { customerPhone = value; }
        }

        private bool fixContact = false;
        /// <summary>
        /// if true locks the contact information in the payment page so the customer cannot change it at the worldpay site
        /// </summary>
        [Themeable(false)]
        public bool FixContact
        {
            get { return fixContact; }
            set { fixContact = value; }
        }

        private bool hideContact = false;
        /// <summary>
        /// if true hides the contact details from the shopper when they reach the payment pages
        /// </summary>
        [Themeable(false)]
        public bool HideContact
        {
            get { return hideContact; }
            set { hideContact = value; }
        }

        private string customData = string.Empty;
        /// <summary>
        /// can be used with md5 hash secret to prevent tampering
        /// for example could use a comma separated string of cartoffer guids
        /// then if the user added items to the cart after leaving the site to world pay
        /// by using 2 browser tabs
        /// we can restore the cart offers to the original value when we get the payment response from world pay
        /// since it will include our custom value
        /// </summary>
        [Themeable(false)]
        public string CustomData
        {
            get { return customData; }
            set { customData = value; }
        }

        //private string providerName = string.Empty;
        ///// <summary>
        ///// the provider name of the feature that will handle the shopper response
        ///// </summary>
        //[Themeable(false)]
        //public string ProviderName
        //{
        //    get { return providerName; }
        //    set { providerName = value; }
        //}


        #region FuturePay/Recurring Payment Properties
        //http://culttt.com/2012/07/18/getting-started-with-worldpay-integration/

        private string futurePayType = string.Empty;
        /// <summary>
        /// WorldPay supports limited or regular
        /// Limited agreements, where you can take variable payments at any time, within limits that you place on: a) the total amount payable, and b) the interval of payments.
        /// Regular agreements, where payments occur at regular fixed intervals and you can fix or vary the amount paid.
        /// 
        /// The value of this should be “regular”
        /// 
        /// The default value is empty string so you MUST set this in order to
        /// render the futurePay form elements
        /// </summary>
        [Themeable(false)]
        public string FuturePayType
        {
            get { return futurePayType; }
            set { futurePayType = value; }
        }

        private string option = "0";
        /// <summary>
        /// 0 – With option 0, the individual payments for the duration of the agreement are at a fixed price. 
        /// However, you can set an initial payment at any price.
        /// For example, you can have an initial payment of £100 and then 5 payments of £20. 
        /// At no point in the agreement can you change the £20 payments.
        /// 
        /// 
        /// 1 – With option 1 you must set the normal payments at the start of the agreement and you can set an initial payment if you wish. 
        /// During the course of the agreement you can adjust the recurring payment amounts.
        /// 
        /// 2 – With option 2 you do not set the payment amount when the agreement is made. 
        /// This option allows you to take payments for the duration of the agreement but not on a recurring basis. 
        /// For example, it is an agreement to bill the customer whenever it is needed, rather on a set schedule.
        /// 
        /// </summary>
        [Themeable(false)]
        public string FuturePayOption
        {
            get { return option; }
            set { option = value; }
        }

        private string startDate = string.Empty;
        /// <summary>
        /// yyyy-mm-dd format .ToString("s");
        /// 
        /// Date on which the first payment will be made. 
        /// This can not be set for the day of the agreement it has to be at some point in the future. 
        /// With option 2 it must be at least 2 weeks into the future.
        /// 
        /// </summary>
        [Themeable(false)]
        public string FuturePayStartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        private string startDelayUnit = string.Empty;
        /// <summary>
        /// One digit: 1-day, 2-week, 3-month, 4-year – 
        /// Unit of the delay between when the agreement is created and when the first payment will be made. 
        /// You only use this parameter if you have not set startDate.
        /// </summary>
        [Themeable(false)]
        public string FuturePayStartDelayUnit
        {
            get { return startDelayUnit; }
            set { startDelayUnit = value; }
        }

        private string startDelayMult = string.Empty;
        /// <summary>
        /// A number greater to or equal to 1. 
        /// You must use this parameter and the previous one together in order to set a start delay. 
        /// The numbers are multiplied together. for example. 
        /// The following code would express a start date delay of 3 weeks.
        /// <input type="hidden" name="startDelayUnit" value="2" />
        /// <input type="hidden" name="startDelayMult" value="3" />
        /// </summary>
        [Themeable(false)]
        public string FuturePayStartDelayMult
        {
            get { return startDelayMult; }
            set { startDelayMult = value; }
        }


        private int noOfPayments = 0;
        /// <summary>
        /// Number. The number of payments which will be made under the agreement. 
        /// If you want the agreement to be unlimited, either don’t set it or set it to 0.
        /// 
        /// </summary>
        [Themeable(false)]
        public int FuturePayNoOfPayments
        {
            get { return noOfPayments; }
            set { noOfPayments = value; }
        }

        private string intervalUnit = string.Empty;
        /// <summary>
        /// One Digit: 1-day, 2-week, 3-month, 4-year. The unit of the interval between payments.
        /// </summary>
        [Themeable(false)]
        public string FuturePayIntervalUnit
        {
            get { return intervalUnit; }
            set { intervalUnit = value; }
        }

        private string intervalMult = string.Empty;
        /// <summary>
        /// Number. The interval unit multiplier. 
        /// The actual interval between payments is intervalUnit multiplied by intervalMult. 
        /// The same logic as startDeley unit and mult.
        /// </summary>
        [Themeable(false)]
        public string FuturePayIntervalMult
        {
            get { return intervalMult; }
            set { intervalMult = value; }
        }

        private decimal initialAmount = 0;
        /// <summary>
        /// The amount of the initial payment – if not set first payment will be for the normal amount. 
        /// For example, 100.00.
        /// </summary>
        [Themeable(false)]
        public decimal FuturePayInitialAmount
        {
            get { return initialAmount; }
            set { initialAmount = value; }
        }

        private decimal normalAmount = 0;
        /// <summary>
        /// Amount of normal payments. For example, 20.00
        /// </summary>
        [Themeable(false)]
        public decimal FuturePayNormalAmount 
        {
            get { return normalAmount; }
            set { normalAmount = value; }
        }

        #endregion

        private void RenderFuturePayElements(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write("<input type=\"hidden\" name=\"futurePayType\" value=\"" + HttpUtility.HtmlAttributeEncode(futurePayType) + "\" />");
            writer.Write("<input type=\"hidden\" name=\"option\" value=\"" + HttpUtility.HtmlAttributeEncode(option) + "\" />");
            
            if (startDate.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"startDate\" value=\"" + HttpUtility.HtmlAttributeEncode(startDate) + "\" />");
            }

            if (startDelayUnit.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"startDelayUnit\" value=\"" + HttpUtility.HtmlAttributeEncode(startDelayUnit) + "\" />");
            }

            if (startDelayMult.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"startDelayMult\" value=\"" + HttpUtility.HtmlAttributeEncode(startDelayMult) + "\" />");
            }

            if (noOfPayments > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"noOfPayments\" value=\"" + noOfPayments.ToInvariantString() + "\" />");
            }

            if (intervalUnit.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"intervalUnit\" value=\"" + HttpUtility.HtmlAttributeEncode(intervalUnit) + "\" />");
            }

            if (intervalMult.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"intervalMult\" value=\"" + HttpUtility.HtmlAttributeEncode(intervalMult) + "\" />");
            }

            if (initialAmount > 0)
            {
                amountString = initialAmount.ToString(currencyCulture);
                writer.Write("<input type=\"hidden\" name=\"initialAmount\" value=\"" + amountString + "\" />");
            }


            if (normalAmount > 0)
            {
                amountString = normalAmount.ToString(currencyCulture);
                writer.Write("<input type=\"hidden\" name=\"normalAmount\" value=\"" + amountString + "\" />");
            }


        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (useTestServer)
            {
                PostBackUrl = testServerPostbackUrl;
            }
            else
            {
                PostBackUrl = productionServerPostbackUrl;
            }

            if (Text.Length == 0)
            {
                Text = Resource.WorldPayBuyButton;
            }

        }

        private string GetSignatureFields()
        {
            if (customData.Length > 0)
            {
                return "instId:amount:currency:cartId:M_custom";
            }
            else
            {
                return "instId:amount:currency:cartId";
            }

        }

       

        private string GetHash()
        {
            string valueToHash;

            if (customData.Length > 0)
            {
                valueToHash = md5Secret + ":" + instId + ":" + amountString + ":" + currencyCode + ":" + cartId + ":" + customData;
            }
            else
            {
                valueToHash = md5Secret + ":" + instId + ":" + amountString + ":" + currencyCode + ":" + cartId;
                
            }

            return CryptoHelper.CalculateMD5Hash(valueToHash).ToLowerInvariant();
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (HttpContext.Current == null) { return; }

            if (instId.Length == 0) { return; }
            if (cartId.Length == 0) { return; }
            if (amount <= 0) { return; }

            amount = Math.Round(amount, 2);

            if (useTestServer)
            {
                writer.Write("<input type=\"hidden\" name=\"testMode\" value=\"100\" />");
            }

            writer.Write("<input type=\"hidden\" name=\"instId\" value=\"" + instId + "\" />");

            if (merchantCode.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"accId1\" value=\"" + merchantCode + "\" />");
            }

            amountString = amount.ToString(currencyCulture);

            writer.Write("<input type=\"hidden\" name=\"cartId\" value=\"" + cartId + "\" />");
            writer.Write("<input type=\"hidden\" name=\"amount\" value=\"" + amountString + "\" />");
            writer.Write("<input type=\"hidden\" name=\"currency\" value=\"" + currencyCode + "\" />");

            if (md5Secret.Length > 0)
            {
                //string valueToHash = md5Secret + ":" + instId + ":" + amountString + ":" + currencyCode + ":" + cartId;
                //string hash = CryptoHelper.CalculateMD5Hash(valueToHash);
                //security params
                writer.Write("<input type=\"hidden\" name=\"signatureFields\" value = \"" + GetSignatureFields() + "\" />");
                writer.Write("<input type=\"hidden\" name=signature value=\"" + GetHash() + "\" />");
            }


            //optional params
            if (orderDescription.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"desc\" value=\"" + HttpUtility.HtmlAttributeEncode(orderDescription) + "\" />");
            }

            if (customData.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"M_custom\" value=\"" + customData + "\" />");
            }

            if (customerEmail.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"email\" value=\"" + HttpUtility.HtmlAttributeEncode(customerEmail) + "\" />");
            }

            
            //This tells our server that the test transaction is authorised. Note that the shopper's name
            // parameter is used to specify the test result.
            //<input type=“hidden” name="name" value="AUTHORISED">

            //If you pass the shopper's billing address details to us when you submit order details,
            // we automatically place them into the billing address fields that the shopper would be
            // required to enter in the payment pages. However, the shopper can change these
            // address details in the payment pages unless you specify that they are fixed data

            if (customerName.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"name\" value=\"" + HttpUtility.HtmlAttributeEncode(customerName) + "\"/>");
            }

            if (address1.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"address1\" value=\"" + HttpUtility.HtmlAttributeEncode(address1) + "\"/>");
            }

            if (address2.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"address2\" value=\"" + HttpUtility.HtmlAttributeEncode(address2) + "\"/>");
            }

            if (address3.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"address3\" value=\"" + HttpUtility.HtmlAttributeEncode(address3) + "\"/>");
            }

            if (town.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"town\" value=\"" + HttpUtility.HtmlAttributeEncode(town) + "\"/>");
            }

            if (region.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"region\" value=\"" + HttpUtility.HtmlAttributeEncode(region) + "\"/>");
            }

            if (postalCode.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"postcode\" value=\"" + HttpUtility.HtmlAttributeEncode(postalCode) + "\"/>");
            }

            if (country.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"country\" value=\"" + HttpUtility.HtmlAttributeEncode(country) + "\"/>");
            }

            if (customerPhone.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"tel\" value=\"" + HttpUtility.HtmlAttributeEncode(customerPhone) + "\"/>");
            }

            
            
            

            //To specify that billing address details are fixed data:
            //use an additional parameter in your order details called fixContact to lock
            // the contact information in the payment page.
            if (fixContact)
            {
                writer.Write("<input type=\"hidden\" name=\"fixContact\" />");
            }

            //You can also hide the contact details from the shopper when they reach the payment
            // pages. This is done using the hideContact parameter
    
            if (hideContact)
            {
                writer.Write("<input type=\"hidden\" name=\"hideContact\" />");
            }

            if (futurePayType.Length > 0)
            {
                RenderFuturePayElements(writer);
            }


            base.Render(writer);
        }

        
    }
}