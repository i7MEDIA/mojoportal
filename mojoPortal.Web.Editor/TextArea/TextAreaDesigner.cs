// Author:					
// Created:				    2009-05-06
// Last Modified:			2009-05-07
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web.UI.Design;

namespace mojoPortal.Web.Editor
{
    public class TextAreaDesigner : ControlDesigner
    {
        public TextAreaDesigner()
        {
        }

        public override string GetDesignTimeHtml()
        {
            TinyMCE control = (TinyMCE)Component;
            return String.Format(CultureInfo.InvariantCulture,
                "<div><table width=\"{0}\" height=\"{1}\" bgcolor=\"#f5f5f5\" bordercolor=\"#c7c7c7\" cellpadding=\"0\" cellspacing=\"0\" border=\"1\"><tr><td valign=\"middle\" align=\"center\">TinyMCE<b>{2}</b></td></tr></table></div>",
                    control.Width,
                    control.Height,
                    control.ID);
        }
    }
}
