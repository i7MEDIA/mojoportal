//  Author:                     
//  Created:                    2012-09-20
//	Last Modified:              2012-09-20
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    public class WorldPayAcceptanceMark : WebControl
    {
        private string instId = string.Empty;
        [Themeable(false)]
        public string InstId
        {
            get { return instId; }
            set { instId = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null) { return; }

            if (instId.Length == 0) { return; }

            writer.Write("<script language=\"JavaScript\" src=\"https://secure.worldpay.com/wcc/logo?instId=" + instId + "\"></script>");

            //base.Render(writer);
        }

    }
}