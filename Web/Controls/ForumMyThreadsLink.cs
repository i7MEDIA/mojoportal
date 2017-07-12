/// Author:					
/// Created:				2008-04-01
/// Last Modified:			2008-04-01
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Web.UI;
using Resources;
using mojoPortal.Business;

namespace mojoPortal.Web.UI
{
    public class ForumMyThreadsLink : ForumUserThreadLink
    {
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


        protected override void OnPreRender(EventArgs e)
        {
            if (Page.Request.IsAuthenticated)
            {
                SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                if (siteUser != null)
                {
                    this.UserId = siteUser.UserId;
                    this.TotalPosts = siteUser.TotalPosts;

                }

            }

            
            this.Text = Resource.ForumMyPostsLink;

            if (renderAsListItem)
                if (CssClass.Length == 0) CssClass = "sitelink";

            base.OnPreRender(e);

        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (renderAsListItem)
            {
  
                writer.WriteBeginTag("li");
                writer.WriteAttribute("class", listItemCSS);
                writer.Write(HtmlTextWriter.TagRightChar);

            }

            base.Render(writer);

            if (renderAsListItem) writer.WriteEndTag("li");
        }

    }
}
