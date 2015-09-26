//	Created:			    2010-01-04
//	Last Modified:		    2010-01-04
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// this is useful when you need a div container but you don't want it to render if none of the WebControls inside are visible
    /// </summary>
    public class AutoHidePanel : Panel
    {
        private int countOfVisibleWebControls = 0;

        
        protected override void OnPreRender(EventArgs e)
        {
            if (HttpContext.Current == null) 
            {
                base.OnPreRender(e);
                return; 
            }

            countOfVisibleWebControls = GetCountVisibleChildWebControls();
            Visible = (countOfVisibleWebControls > 0);

            base.OnPreRender(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            countOfVisibleWebControls = GetCountVisibleChildWebControls();
            if (countOfVisibleWebControls > 0)
            {
                base.Render(writer);
            }
        }

        
        private int GetCountVisibleChildWebControls()
        {
            foreach (Control c in Controls)
            {
                if ((c is WebControl) && (c.Visible)) { return 1; }
                if (c is ContentPlaceHolder)
                {
                    foreach (Control child in c.Controls)
                    {
                        if ((child is WebControl) && (child.Visible)) { return 1; }
                    }
                }
            }

            return 0;
        }

    }
}
