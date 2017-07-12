// Author:					
// Created:				2008-03-27
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
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;


namespace mojoPortal.Web.Controls
{
    /// <summary>
    /// See http://www.addthis.com
    /// </summary>
    public class AddThisButton : HyperLink
    {
        #region Private Properties

        private string accountId = string.Empty;
        private bool useMouseOverWidget = true;
        private string customLogoUrl = string.Empty;
        private string customLogoBackgroundColor = string.Empty;
        private string customLogoColor = string.Empty;
        private string customBrand = string.Empty;
        private string customOptions = string.Empty;
        private int customOffsetTop = -999;
        private int customOffsetLeft = -999;
        private string buttonImageUrl = "~/Data/SiteImages/addthissharebutton.gif";
        private string protocol = "http";

        private string urlToShare = string.Empty;
        private string titleOfUrlToShare = string.Empty;

        #endregion


        #region Public Properties


        /// <summary>
        /// Your addthis.com username.
        /// If this is not set the control will not render.
        /// </summary>
        public string AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }

        /// <summary>
        /// if true will show widget in the page
        /// </summary>
        public bool UseMouseOverWidget
        {
            get { return useMouseOverWidget; }
            set { useMouseOverWidget = value; }
        }

        /// <summary>
        /// The logo to display on the popup window (about 200x50 pixels). 
        /// The popup window is show when the user selects the 'More' choice
        /// </summary>
        public string CustomLogoUrl
        {
            get { return customLogoUrl; }
            set { customLogoUrl = value; }
        }

        /// <summary>
        /// The color to use as a background around the logo in the popup 
        /// </summary>
        public string CustomLogoBackgroundColor
        {
            get { return customLogoBackgroundColor; }
            set { customLogoBackgroundColor = value; }
        }


        /// <summary>
        /// The color to use for the text next to the logo in the popup 
        /// </summary>
        public string CustomLogoColor
        {
            get { return customLogoColor; }
            set { customLogoColor = value; }
        }


        /// <summary>
        /// The brand name to display in the drop-down (top right)
        /// </summary>
        public string CustomBrand
        {
            get { return customBrand; }
            set { customBrand = value; }
        }


        /// <summary>
        /// A comma-separated ordered list of options to include in the drop-down
        /// Example: addthis_options = 'favorites, email, digg, delicious, more'; 
        /// Currently supported options:
        /// delicious, digg, email, favorites, facebook, fark, furl, google, live, myweb, myspace, 
        /// newsvine, reddit, slashdot, stumbleupon, technorati, twitter, more 
        /// (the default is currently 'favorites, digg, delicious, google, myspace, facebook, 
        /// reddit, newsvine, 
        /// live, more', in that order).
        /// </summary>
        public string CustomOptions
        {
            get { return customOptions; }
            set { customOptions = value; }
        }

        /// <summary>
        /// Vertical offset for the drop-down window widget (in pixels) 
        /// thiscontrol defaults to -999 which means unsepcified
        /// this will result in the defaults from addthis.com
        /// not sure what the defsault is
        /// </summary>
        public int CustomOffsetTop
        {
            get { return customOffsetTop; }
            set { customOffsetTop = value; }
        }

        /// <summary>
        /// Horizontal offset for the drop-down window widget (in pixels) 
        /// thiscontrol defaults to -999 which means unsepcified
        /// this will result in the defaults from addthis.com
        /// not sure what the defsault is
        /// </summary>
        public int CustomOffsetLeft
        {
            get { return customOffsetLeft; }
            set { customOffsetLeft = value; }
        }

        public string ButtonImageUrl
        {
            get { return buttonImageUrl; }
            set { buttonImageUrl = value; }
        }

        public string UrlToShare
        {
            get { return urlToShare; }
            set { urlToShare = value; }
        }

        public string TitleOfUrlToShare
        {
            get { return titleOfUrlToShare; }
            set { titleOfUrlToShare = value; }
        }


        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (HttpContext.Current == null) { return; }

            if (accountId.Length == 0)
            {
                this.Visible = false;
                return;
            }

            if (Page.Request.IsSecureConnection)
                protocol = "https";

            SetupScripts();

            this.ImageUrl = Page.ResolveUrl(buttonImageUrl);
            this.NavigateUrl = "http://www.addthis.com/bookmark.php";

            if (useMouseOverWidget)
                SetupWidget();
            else
                SetupNormalLink();

        }

        private void SetupNormalLink()
        {
            StringBuilder onClickAttribute = new StringBuilder();

            if (urlToShare.Length > 0)
            {
                onClickAttribute.Append("addthis_url = '" + urlToShare + "'; ");
            }
            else
            {
                onClickAttribute.Append("addthis_url = location.href; ");
            }

            if (titleOfUrlToShare.Length > 0)
            {
                onClickAttribute.Append("addthis_title ='" + titleOfUrlToShare.HtmlEscapeQuotes() + "'; ");
            }
            else
            {
                onClickAttribute.Append("addthis_title = document.title; ");

            }

            onClickAttribute.Append("return addthis_click(this); ");

            this.Attributes.Add("onclick", onClickAttribute.ToString());

            //this.Attributes.Add("onclick", "return addthis_click(this); ");

        }

        private void SetupWidget()
        {
            StringBuilder mouseOverAttribute = new StringBuilder();

            mouseOverAttribute.Append("try {return addthis_open(this, '',");

            if (urlToShare.Length > 0)
            {
                mouseOverAttribute.Append("'" + urlToShare + "', ");
            }
            else
            {
                mouseOverAttribute.Append("'[URL]', ");
            }

            if (titleOfUrlToShare.Length > 0)
            {
                mouseOverAttribute.Append("'" + titleOfUrlToShare.HtmlEscapeQuotes() + "' ");
            }
            else
            {
                mouseOverAttribute.Append("'[TITLE]' ");

            }

            mouseOverAttribute.Append(");}catch(ex){} ");

            
            this.Attributes.Add("onmouseover", mouseOverAttribute.ToString());

            this.Attributes.Add("onmouseout", "try { addthis_close(); }catch(ex){}");

        }

        private void SetupScripts()
        {
            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\"> ");
            script.Append("\n<!-- \n");

            script.Append("var addthis_pub = '" + accountId + "';");

            if(customLogoUrl.Length > 0)
                script.Append("var addthis_logo = '" + customLogoUrl + "';");

            if (customLogoBackgroundColor.Length > 0)
                script.Append("var addthis_logo_background = '" + customLogoBackgroundColor + "';");

            if (customLogoColor.Length > 0)
                script.Append("var addthis_logo_color = '" + customLogoColor + "';");

            if (customBrand.Length > 0)
                script.Append("var addthis_brand = '" + customBrand + "';");

            if (customOptions.Length > 0)
                script.Append("var addthis_options = '" + customOptions + "';");

            if (customOffsetTop != -999)
                script.Append("var addthis_offset_top = " + customOffsetTop.ToString(CultureInfo.InvariantCulture) + ";");

            if (customOffsetLeft != -999)
                script.Append("var addthis_offset_left = " + customOffsetLeft.ToString(CultureInfo.InvariantCulture) + ";");


            script.Append("\n//--> ");
            script.Append(" </script>");


            Page.ClientScript.RegisterClientScriptBlock(
                typeof(AddThisButton),
                "addthisbutton",
                script.ToString());

            if(useMouseOverWidget)
                Page.ClientScript.RegisterStartupScript(
                    typeof(AddThisButton),
                    "addthisbuttonsetup", "\n<script type=\"text/javascript\" src=\""
                    + protocol + "://s7.addthis.com/js/152/addthis_widget.js"
                    + "\" ></script>");
            else
                Page.ClientScript.RegisterStartupScript(
                    typeof(AddThisButton),
                    "addthisbuttonsetup", "\n<script type=\"text/javascript\" src=\""
                    + protocol + "://s9.addthis.com/js/widget.php?v=10"
                    + "\" ></script>");

        }


        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            base.Render(writer);


        }


    }
}
