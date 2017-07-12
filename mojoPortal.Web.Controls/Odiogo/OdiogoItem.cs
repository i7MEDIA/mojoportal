// Author:					
// Created:				2008-03-16
// Last Modified:			2009-06-26
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
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;


namespace mojoPortal.Web.Controls
{
    
    public class OdiogoItem : Panel
    {
        const string ItemScriptTemplate = "\n<script type=\"text/javascript\">\n<!--\nshowOdiogoReadNowButton ('{0}', '{1}', '{2}', 290, 55);\n//-->\n</script>\n<script type=\"text/javascript\">\nshowInitialOdiogoReadNowFrame ('{0}', '{2}', 290, 0);\n//-->\n</script>\n";

        private string odiogoFeedId = string.Empty;
        private string itemTitle = string.Empty;
        private string itemId = string.Empty;

        public string OdiogoFeedId
        {
            get { return odiogoFeedId; }
            set { odiogoFeedId = value; }
        }

        public string ItemTitle
        {
            get { return itemTitle; }
            set { itemTitle = value; }
        }

        public string ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }


        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.CssClass = "odiogo";
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetupMainScript();
            SetupItem();

            this.Visible = (odiogoFeedId.Length > 0);

        }

        private void SetupItem()
        {
            if (odiogoFeedId.Length == 0) return;
            if (itemTitle.Length == 0) return;
            if (itemId.Length == 0) return;

            Literal litItem = new Literal();
            litItem.Text = string.Format(CultureInfo.InvariantCulture,
                ItemScriptTemplate,
                odiogoFeedId,
                itemTitle.HtmlEscapeQuotes(),
                itemId);

            this.Controls.Add(litItem);

        }

        private void SetupMainScript()
        {
            if (odiogoFeedId.Length == 0) return;

            Page.ClientScript.RegisterClientScriptBlock(
                typeof(OdiogoItem),
                "odiogofeed" + odiogoFeedId, "\n<script type=\"text/javascript\" src=\""
                + "http://podcasts.odiogo.com/odiogo_js.php?feed_id=" + odiogoFeedId + "&amp;platform=mp" + "\" ></script>");

        }

    }
}
