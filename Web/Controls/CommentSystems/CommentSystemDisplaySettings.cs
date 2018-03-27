// Author:					
// Created:				    2012-08-16
// Last Modified:			2016-01-06
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
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// this control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
/// </summary>
namespace mojoPortal.Web.UI
{
    public class CommentSystemDisplaySettings : WebControl
    {
        private string deleteLinkImage = "~/Data/SiteImages/delete.png";

        public string DeleteLinkImage
        {
            get { return deleteLinkImage; }
            set { deleteLinkImage = value; }
        }

        private bool showNameInputWhenAuthenticated = false;

        public bool ShowNameInputWhenAuthenticated
        {
            get { return showNameInputWhenAuthenticated; }
            set { showNameInputWhenAuthenticated = value; }
        }

        private bool showEmailInputWhenAuthenticated = false;

        public bool ShowEmailInputWhenAuthenticated
        {
            get { return showEmailInputWhenAuthenticated; }
            set { showEmailInputWhenAuthenticated = value; }
        }

        private bool showUrlInputWhenAuthenticated = false;

        public bool ShowUrlInputWhenAuthenticated
        {
            get { return showUrlInputWhenAuthenticated; }
            set { showUrlInputWhenAuthenticated = value; }
        }

        private string preferredEditor = "";
        public string PreferredEditor
        {
            get { return preferredEditor; }
            set { preferredEditor = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            // nothing to render
        }
    }
}