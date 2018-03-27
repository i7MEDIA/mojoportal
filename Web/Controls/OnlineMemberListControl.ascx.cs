/// Created:				2006-12-03
/// Last Modified:		    2018-01-03
/// 
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
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Controls;
using Resources;

namespace mojoPortal.Web.UI
{
   
    public partial class OnlineMemberListControl : UserControl
    {
        protected SiteSettings siteSettings;
        private bool isAdmin;

        //Gravatar public enum RatingType { G, PG, R, X }
        protected Avatar.RatingType MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();
        protected bool allowGravatars = false;
        protected bool disableAvatars = true;

        protected bool IsAdmin
        {
            get{return isAdmin;}  
        }

        private bool siteMailEnabled;
        private String siteRoot; 

        protected String SiteRoot
        {
            get{return siteRoot;} 
        }
        private String toolTipPrefix;
        
        // TODO: add link for site mail when authenticated
        // set to use top 30 here, link to OnlineMembers.aspx
        // for page able list of online members
        // 

        protected void Page_Load(object sender, EventArgs e)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (siteSettings == null) { this.Visible = false; return; }

            siteRoot = SiteUtils.GetNavigationSiteRoot();

            switch (siteSettings.AvatarSystem)
            {
                case "gravatar":
                    allowGravatars = true;
                    disableAvatars = false;
                    break;

                case "internal":
                    allowGravatars = false;
                    disableAvatars = false;
                    break;

                case "none":
                default:
                    allowGravatars = false;
                    disableAvatars = true;
                    break;

            }

            toolTipPrefix = Resource.OnlineMemberListProfileLinkPrefix;

            isAdmin = WebUser.IsAdmin;

            //if (Context.Request.IsAuthenticated)
            //{
            //    isAuthenticated = true;
            //}

            siteMailEnabled = WebConfigSettings.UseSiteMailFeature;

            PopulateControls();
        }


        private void PopulateControls()
        {
            if (Session != null)
            {
                if (siteSettings != null)
                {
                    DateTime sessionWindowStart = DateTime.UtcNow.AddMinutes(-Session.Timeout);
                    using (IDataReader reader = SiteUser.GetUsersOnlineSince(siteSettings.SiteId, sessionWindowStart))
                    {
                        rptOnlineMembers.DataSource = reader;
                        rptOnlineMembers.DataBind();
                    }

                    if (rptOnlineMembers.Items.Count == 0)
                    {
                        this.Visible = false;
                    }

                }
            }
        }


        //public String GetAvatarUrl(object userId, String userName, String avatar)
        //{
        //    if (allowGravatars) { return string.Empty; }
        //    if (disableAvatars) { return string.Empty; }

        //    String toolTip = toolTipPrefix + " " + userName;
        //    if ((avatar != null) 
        //        && (
        //        (avatar == "blank.gif")||(avatar == String.Empty)
        //            )
        //        )
        //    {
        //        return String.Empty;
        //    }
        //    else
        //    {
        //        return "<br />" 
        //            + SiteUtils.GetProfileAvatarLink(Page, userId, siteSettings.SiteId, avatar, toolTip) 
        //            + "<br />";
        //    }
        //}

    }
}
