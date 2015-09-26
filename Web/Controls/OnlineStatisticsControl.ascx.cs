// Author:             Joe Audette
// Created:            12/2/2006
// Last Modified:      12/3/2006
// 
// 
// 
// images used in this module are from DotNetNuke
// http://www.dotnetnuke.com/
// licensed under a BSD style license
// http://www.dotnetnuke.com/Default.aspx?tabid=776

using System;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;


namespace mojoPortal.Web.UI
{
    public partial class OnlineStatisticsControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PopulateControls();
        }

        private void PopulateControls()
        {
            imgPeopleOnline.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/uoGroup2.gif");
            imgPeopleOnline.AlternateText = Resources.Resource.SiteStatisticsPeopleOnlineLabel;

            imgVistitorsOnline.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/uoVisitors.gif");
            imgVistitorsOnline.AlternateText = Resources.Resource.SiteStatisticsVisitorsLabel;

            imgMembersOnline.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/uoMembers.gif");
            imgMembersOnline.AlternateText = Resources.Resource.SiteStatisticsMembersLabel;

            imgTotalOnline.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/uoTotal.gif");
            imgTotalOnline.AlternateText = Resources.Resource.SiteStatisticsTotalOnlineLabel;

            String key = WebUtils.GetHostName() + "_onlineCount";
            int totalUsersOnline = 1;
            int membersOnline = 0;
            int visitorsOnline = 1;
            if (Application[key] != null)
            {
                totalUsersOnline = (int)Application[key];
            }

            if (Session != null)
            {
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                DateTime sessionWindowStart = DateTime.UtcNow.AddMinutes(-Session.Timeout);
                membersOnline = SiteUser.UsersOnlineSinceCount(siteSettings.SiteId, sessionWindowStart);

            }

            if (totalUsersOnline >= membersOnline)
            {
                visitorsOnline = totalUsersOnline - membersOnline;
            }
            else
            {
                totalUsersOnline = membersOnline + visitorsOnline;
            }

            lblVisitorsOnline.Text = visitorsOnline.ToString();
            lblMembersOnline.Text = membersOnline.ToString();
            lblTotalOnline.Text = totalUsersOnline.ToString();

        }

    }
}