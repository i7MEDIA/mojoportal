using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace mojoPortal.Business
{
    public class RegexHelper
    {
        //** Email Validation
        /// <summary>
        /// a regular expression for validating email addresses, efficient but not completely RFC 822 compliant
        /// </summary>
        public const string RegexEmailValidationPattern = @"^([0-9a-zA-Z](['-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w']*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$";

        public static bool IsValidEmailAddressSyntax(string emailAddress)
        {
            Regex emailPattern;
            //emailPattern = new Regex("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
            emailPattern = new Regex(RegexEmailValidationPattern);

            Match emailAddressToValidate = emailPattern.Match(emailAddress);

            if (emailAddressToValidate.Success)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
