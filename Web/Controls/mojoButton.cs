//	Created:			    2010-02-20
//	Last Modified:		    2013-04-10
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
        private bool renderArtisteer = false;
        public bool RenderArtisteer
        {
            get { return renderArtisteer; }
            set { renderArtisteer = value; }
        }

        private bool usejQueryButton = false;
        public bool UsejQueryButton
        {
            get { return usejQueryButton; }
            set { usejQueryButton = value; }
        }

        private string artButtonLeftClass = "l"; //changed to art-button-l in Artisteer 3
        public string ArtButtonLeftClass
        {
            get { return artButtonLeftClass; }
            set { artButtonLeftClass = value; }
        }

        private string artButtonRightClass = "r"; // changed to art-button-r in Artisteer 3
        public string ArtButtonRightClass
        {
            get { return artButtonRightClass; }
            set { artButtonRightClass = value; }
        }

        private string literalTopMarkup = string.Empty;

        public string LiteralTopMarkup
        {
            get { return literalTopMarkup; }
            set { literalTopMarkup = value; }
        }

        private string literalBottomMarkup = string.Empty;

        public string LiteralBottomMarkup
        {
            get { return literalBottomMarkup; }
            set { literalBottomMarkup = value; }
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (HttpContext.Current == null) { return; }

            if ((!renderArtisteer) && (usejQueryButton))
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

            if (renderArtisteer)
            {
                writer.Write("<span class=\"art-button-wrapper\">");
                writer.Write("<span class=\"" + artButtonLeftClass + "\"> </span>\n");
                writer.Write("<span class=\"" + artButtonRightClass + "\"> </span>\n");
            }
            else if (literalTopMarkup.Length > 0)
            {
                writer.Write(literalTopMarkup);
            }

            base.Render(writer);

            if (renderArtisteer)
            {
                writer.Write("\n</span>");
            }
            else if (literalBottomMarkup.Length > 0)
            {
                writer.Write(literalBottomMarkup);
            }
        }

    }
}
