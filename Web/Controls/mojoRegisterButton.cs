//	Created:			    2011-06-12
//	Last Modified:		    2011-06-12
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// the purpose of this button is to get better control over the display on the register page.
    /// The problem is that we can get access to the button the CreateUserWizard adds, so the strategy is to add our own button
    /// and hide the other button by CSS:
    /// input.createuserbutton { display:none;}
    /// since this is not already done in existing skins this button is not rendered by default because then there would be 2 buttons
    /// so to use it you first add the css to hide the other button then you enable this button from theme.skin with
    /// <portal:mojoRegisterButton runat="server" UseThisButton="true" />
    /// 
    /// 
    /// </summary>
    public class mojoRegisterButton : mojoButton
    {
        private bool useThisButton = false;

        public bool UseThisButton
        {
            get { return useThisButton; }
            set { useThisButton = value; }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!useThisButton) { return; }

            base.Render(writer);
        }
    }
}