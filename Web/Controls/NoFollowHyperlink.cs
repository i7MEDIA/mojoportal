// Author:					
// Created:				    2012-06-04
// Last Modified:			2012-06-04
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
//

using System;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// Inherits from asp:Hyperlink, automatically adds rel="nofollow"
    /// </summary>
    public class NoFollowHyperlink : HyperLink
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Attributes.Add("rel", "nofollow");
        }
    }
}