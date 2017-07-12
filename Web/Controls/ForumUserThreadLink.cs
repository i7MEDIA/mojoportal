/// Author:					
/// Created:				2008-03-18
/// Last Modified:			2008-11-07
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Resources;

namespace mojoPortal.Web.UI
{
    public class ForumUserThreadLink : NoFollowHyperlink
    {
        private int userId = -1;
        private int totalPosts = -1;


        public int UserId
        {
            get { return userId; }
            set { userId = value;}
                
        }

        public int TotalPosts
        {
            get { return totalPosts; }
            set { totalPosts = value; }
        }



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current == null) { return; }
            this.Text = Resource.ForumUserPostsLink;
            

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (HttpContext.Current == null) { return; }

            this.Visible = (WebConfigSettings.AllowUserThreadBrowsing && (userId > -1) && (totalPosts > 0));

            this.NavigateUrl = SiteUtils.GetNavigationSiteRoot()
                + "/Forums/UserThreads.aspx?"
                + "userid=" + userId.ToString(CultureInfo.InvariantCulture);
        }

    }
}
