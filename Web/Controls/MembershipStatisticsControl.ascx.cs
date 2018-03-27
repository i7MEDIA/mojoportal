// Created:            2006-12-02
// Last Modified:      2018-01-03
// 

using System;
using System.Web.UI;
using mojoPortal.Business.Statistics;
using mojoPortal.Business.WebHelpers;


namespace mojoPortal.Web
{
    public partial class MembershipStatisticsControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PopulateControls();
        }

        private void PopulateControls()
        {
            MembershipStatistics membershipStatistics = CacheHelper.GetCurrentMembershipStatistics();
            if (membershipStatistics != null)
            {
                lblNewestUser.Text = membershipStatistics.NewestUser;
                lblNewUsersToday.Text = membershipStatistics.NewUsersToday.ToString();
                lblNewUsersYesterday.Text = membershipStatistics.NewUsersYesterday.ToString();
                lblTotalUsers.Text = membershipStatistics.TotalUsers.ToString();
            }
        }
    }
}