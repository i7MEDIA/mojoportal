//	Created:			    2010-02-20
//	Last Modified:		    2018-11-14
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
    /// inherits button and adds extra markup for Artisteer if RenderArtisteer is true
    /// </summary>
    public class mojoButton : Button
    {
		public bool RenderArtisteer { get; set; } = false;
		public bool UsejQueryButton { get; set; } = false;
		public string ArtButtonLeftClass { get; set; } = "l"; // changed to art-button-l in Artisteer 3
		public string ArtButtonRightClass { get; set; } = "r"; // changed to art-button-r in Artisteer 3

		public string LiteralTopMarkup { get; set; } = string.Empty;

		public string LiteralBottomMarkup { get; set; } = string.Empty;


		protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (HttpContext.Current == null) { return; }

            if ((!RenderArtisteer) && (UsejQueryButton))
            {
                CssClass += " jqbutton ui-button ui-widget ui-state-default ui-corner-all";

            }
        }

        //jqbutton

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            if (RenderArtisteer)
            {
                writer.Write("<span class=\"art-button-wrapper\">");
                writer.Write("<span class=\"" + ArtButtonLeftClass + "\"> </span>\n");
                writer.Write("<span class=\"" + ArtButtonRightClass + "\"> </span>\n");
            }
            else if (LiteralTopMarkup.Length > 0)
            {
                writer.Write(LiteralTopMarkup);
            }

            base.Render(writer);

            if (RenderArtisteer)
            {
                writer.Write("\n</span>");
            }
            else if (LiteralBottomMarkup.Length > 0)
            {
                writer.Write(LiteralBottomMarkup);
            }
        }

    }
}
