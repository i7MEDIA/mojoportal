/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	
///
///		Author:				Joe Audette
///		Created:			2005-03-24
///		Last Modified:		2010-07-30
/// 
/// 03/13/2007   Alexander Yushchenko: moved all the control logic to Render() to simplify it.
/// 2011-02-08 made it possible to customize the welcome message, by default it comes from resource file
/// 2012-09-03 made it possible to use first and last name

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using Resources;

namespace mojoPortal.Web.UI
{
    public partial class WelcomeMessage : WebControl
    {
        
        private bool useRightSeparator = false;
        /// <summary>
        /// This property is deprecated. Instead to use mojoPortal.Web.Controls.SeparatorControl
        /// </summary>
        public bool UseRightSeparator
        {
            get { return useRightSeparator; }
            set { useRightSeparator = value; }
        }

        private bool renderAsListItem = false;
        public bool RenderAsListItem
        {
            get { return renderAsListItem; }
            set { renderAsListItem = value; }
        }

        private string listItemCSS = "topnavitem";
        public string ListItemCss
        {
            get { return listItemCSS; }
            set { listItemCSS = value; }
        }

        private bool useFirstLast = false;
        public bool UseFirstLast
        {
            get { return useFirstLast; }
            set { useFirstLast = value; }
        }

        private string firstLastFormat = string.Empty;
        /// <summary>
        /// allows using first and last name in the welcome message, the default value is "Signed in as: {0} {1}"
        /// the {0} is required and will be replaced by the first name and {1} will be replaced by the last name
        /// However this is only useful if first and last name are actually populated
        /// which it may not be if you have not required it on registration and there are existing users
        /// also requires setting UseFirstLast to true
        /// </summary>
        public string FirstLastFormat
        {
            get { return firstLastFormat; }
            set { firstLastFormat = value; }
        }

         

        private string overrideFormat = string.Empty;
        /// <summary>
        /// allows overriding the welcome message, the default value is "Signed in as: {0}"
        /// the {0} is required and will be replaced by the user name
        /// </summary>
        public string OverrideFormat
        {
            get { return overrideFormat; }
            set { overrideFormat = value; }
        }

        private bool wrapInAnchor = false;
        public bool WrapInAnchor
        {
            get { return wrapInAnchor; }
            set { wrapInAnchor = value; }
        }

        private bool wrapInProfileLink = false;
        public bool WrapInProfileLink
        {
            get { return wrapInProfileLink; }
            set { wrapInProfileLink = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            DoRender(writer);
            
            
        }

        private void DoRender(HtmlTextWriter writer)
        {
            if (!HttpContext.Current.Request.IsAuthenticated) { return; }

            SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
            if ((siteUser == null) || (siteUser.UserId <= -1)) { return; }

            if (CssClass.Length == 0) CssClass = "sitelink";

            if (renderAsListItem) writer.Write("<li class='" + listItemCSS + "'>");

            if ((useFirstLast)&&(siteUser.FirstName.Length > 0)&&(siteUser.LastName.Length > 0))
            {
                if (firstLastFormat.Length == 0) { firstLastFormat = Resource.FirstLastFormat; }

                if (wrapInProfileLink)
                {
                    writer.Write("<a class='" + CssClass + "' href='" + SiteUtils.GetNavigationSiteRoot() + "/Secure/UserProfile.aspx" + "'>" + string.Format(firstLastFormat,
                        HttpUtility.HtmlEncode(siteUser.FirstName), HttpUtility.HtmlEncode(siteUser.LastName)) + "</a>");
                }
                else if (wrapInAnchor)
                {
                    writer.Write("<a class='" + CssClass + "' name='welcome'>" + string.Format(firstLastFormat,
                        HttpUtility.HtmlEncode(siteUser.FirstName), HttpUtility.HtmlEncode(siteUser.LastName)) + "</a>");
                }
                else
                {

                    writer.Write("<span class='" + CssClass + "'>" + string.Format(firstLastFormat,
                        HttpUtility.HtmlEncode(siteUser.FirstName), HttpUtility.HtmlEncode(siteUser.LastName)) + "</span>");
                }

            }
            else
            {
                string format = Resource.WelcomeMessageFormat;

                if (overrideFormat.Length > 0) { format = overrideFormat; }

                if (wrapInProfileLink)
                {
                    writer.Write("<a class='" + CssClass + "' href='" + SiteUtils.GetNavigationSiteRoot() + "/Secure/UserProfile.aspx" + "'>" + string.Format(format, HttpUtility.HtmlEncode(siteUser.Name)) + "</a>");
                }
                else if (wrapInAnchor)
                {
                    writer.Write("<a class='" + CssClass + "' name='welcome'>" + string.Format(format, HttpUtility.HtmlEncode(siteUser.Name)) + "</a>");
                }
                else
                {

                    writer.Write("<span class='" + CssClass + "'>" + string.Format(format, HttpUtility.HtmlEncode(siteUser.Name)) + "</span>");
                }
            }

            if (UseRightSeparator) writer.Write(" <span class='Accent'>|</span>");

            if (renderAsListItem) writer.Write("</li>");
        }

    }
}