// Author:					
// Created:				    2014-03-18
// Last Modified:			2014-03-18
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
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// Rationale: from my research most regular expression I have tried has some problem.
    /// The best one so far has been @"^([0-9a-zA-Z](['-.\w]*[_0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w']*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$";
    /// this one seems to have a performance problem on server validation for some inputs, a known example input that hangs the registration 
    /// page and spikes cpu usage is patriciarichards5395@yahoo.comgetgoing (which is technically a valid email address format even if it isn't a real email address)
    /// others I tested did not have the performance problem but return invalid for emails like foo+test@foo.com which should be valid.
    /// False negatives like that are a worse problem but performance issues are also a major problem because they could be used for denial of service
    /// or attempts to overload the server.
    /// So the goal of this control is to keep using the regular expression above (or you can override it by config) for client side validation,
    /// but for server side validation skip the regex and just test if the provided string can be used in a mail address as seen below in SecurityHelper.IsValidEmailAddress(...)
    /// this way we can avoid the server side hit for the regex evaluation
    /// </summary>
    public class EmailValidator : RegularExpressionValidator
    {
		/// <summary>
		/// set to true to act like a normal regularexpressionvalidator
		/// </summary>
		public bool UseRegex { get; set; } = false;

		public bool AllowEmpty { get; set; } = false;

		public EmailValidator()
        {

        }

        protected override bool EvaluateIsValid()
        {
            string providedEmail = GetControlValidationValue(ControlToValidate);

            if (AllowEmpty && providedEmail == string.Empty) return true;

            if (UseRegex) { return base.EvaluateIsValid(); }

            return SecurityHelper.IsValidEmailAddress(providedEmail);
        }

        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if(ValidationExpression.Length == 0)
            {
                ValidationExpression = SecurityHelper.GetEmailRegexExpression();
            }

            if (WebConfigSettings.ForceRegexOnEmailValidator) { UseRegex = true; }
        }

        
    }
}