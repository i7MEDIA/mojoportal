// Created:            12/2/2006
// Last Modified:      2018-01-03

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