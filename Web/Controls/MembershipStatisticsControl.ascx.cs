// Author:             Joe Audette
// Created:            2006-12-02
// Last Modified:      2007-11-12
// 
// 
// 
// images used in this module are from DotNetNuke
// http://www.dotnetnuke.com/
// licensed under a BSD style license
// http://www.dotnetnuke.com/Default.aspx?tabid=776

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
            imgMembership.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/uoGroup1.gif");
            imgMembership.AlternateText = Resources.Resource.SiteStatisticsMembershipLabel;

            imgNewestMember.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/uoLatest.gif");
            imgNewestMember.AlternateText = Resources.Resource.SiteStatisticsNewestMemberLabel;

            imgNewToday.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/uoNewToday.gif");
            imgNewToday.AlternateText = Resources.Resource.SiteStatisticsNewTodayLabel;

            imgNewYesterday.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/uoNewYesterday.gif");
            imgNewYesterday.AlternateText = Resources.Resource.SiteStatisticsNewYesterdayLabel;

            imgTotalUsers.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/uoOverall.gif");
            imgTotalUsers.AlternateText = Resources.Resource.SiteStatisticsTotalUsersLabel;

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