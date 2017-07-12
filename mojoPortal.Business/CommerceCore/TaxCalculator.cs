// Author:					
// Created:				    2007-03-11
// Last Modified:			2008-07-01
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.ObjectModel;

namespace mojoPortal.Business
{
    /// <summary>
    ///
    /// </summary>
    public static class TaxCalculator
    {
        

        public static decimal Calculate(
            Guid siteGuid,
            Guid taxZoneGuid,
            Guid taxClassGuid,
            decimal taxableTotal,
            int roundingDecimalPlaces,
            MidpointRounding roundingMode)
        {
            decimal taxTotal = 0;

            if (taxZoneGuid != Guid.Empty)
            {
                Collection<TaxRate> taxRates = TaxRate.GetTaxRates(siteGuid, taxZoneGuid);
                if (taxRates.Count > 0)
                {
                    foreach (TaxRate taxRate in taxRates)
                    {
                        if (taxClassGuid == taxRate.TaxClassGuid)
                        {
                            taxTotal += (taxRate.Rate * taxableTotal);
                            break;
                        }
                    }
                }


            }


            return taxTotal;

        }

    }
}
