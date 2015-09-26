using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
namespace mojoPortal.Web.Framework
{
    
    /// <summary>
    /// Created by Michael Morozov
    /// michael.morozov@neudesic.com
    /// </summary>
    public class CurrencyHelper
    {
        // Lookup dictionary of CultureInfo by currency ISO code (e.g  USD, GBP, JPY)
        static Dictionary<string, CultureInfo> _currencyCultureInfo;

        static CurrencyHelper()
        {
            _currencyCultureInfo = new Dictionary<string, CultureInfo>();

            // get the list of cultures. We are not interested in neutral cultures, since
            // currency and RegionInfo is only applicable to specific cultures
            CultureInfo[] _cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo ci in _cultures)
            {
                // Create a RegionInfo from culture id. 
                // RegionInfo holds the currency ISO code
                try
                {
                    RegionInfo ri = new RegionInfo(ci.LCID);

                    // multiple cultures can have the same currency code
                    if (!_currencyCultureInfo.ContainsKey(ri.ISOCurrencySymbol))
                        _currencyCultureInfo.Add(ri.ISOCurrencySymbol, ci);
                }
                catch (ArgumentException) { }
            }
        }

        /// <summary>
        /// Lookup CultureInfo by currency ISO code
        /// </summary>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        static public CultureInfo CultureInfoFromCurrencyISO(string isoCode)
        {
            if (_currencyCultureInfo.ContainsKey(isoCode))
                return _currencyCultureInfo[isoCode];
            else
                return null;
        }

        /// <summary>
        /// Convert currency to a string using the specified currency format
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currencyISO"></param>
        /// <returns></returns>
        static public string FormatCurrency(decimal amount, string currencyISO)
        {
            CultureInfo c = CultureInfoFromCurrencyISO(currencyISO);
            if (c == null)
            {
                // if currency ISO code doesn't match any culture
                // create a new culture without currency symbol
                // and use the ISO code as a prefix (e.g. YEN 123,123.00)
                c = CultureInfo.CreateSpecificCulture("us-EN");
                c.NumberFormat.CurrencySymbol = "";
                c.NumberFormat.CurrencyDecimalDigits = 2;
                c.NumberFormat.CurrencyDecimalSeparator = ".";
                c.NumberFormat.CurrencyGroupSeparator = ",";

                return String.Format("{0} {1}", currencyISO, amount.ToString("C", c.NumberFormat));
            }
            else
                return amount.ToString("C", c.NumberFormat);
        }


    }
}
