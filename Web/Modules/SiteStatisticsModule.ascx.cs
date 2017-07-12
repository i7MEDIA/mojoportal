/// Author:       			
/// Created:      			2006-12-02
/// Last Modified:			2009-04-10

using System;
using System.Data;
using System.Data.Common;
using System.Drawing;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using ZedGraph;
using ZedGraph.Web;
using Resources;

namespace mojoPortal.Web.StatisticsUI
{
    public partial class SiteStatisticsModule : SiteModuleControl
    {
        private bool showMembershipStatistics;
        private bool showOnlineStatistics;
        private bool showOnlineMembersList;
        private bool showMembershipGraph = false;
        private int membershipGraphHeight = 400;
        private int membershipGraphWidth = 780;
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.zgMembershipGrowth.RenderGraph += new ZedGraph.Web.ZedGraphWebControlEventHandler(this.OnRenderUserChart);

            this.zgMembershipGrowth.RenderedImagePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString()
            + "/systemfiles/";
        }

        
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateControls();
        }

        private void PopulateControls()
        {
            pnlMembership.Visible = showMembershipStatistics;
            pnlUsersOnline.Visible = showOnlineStatistics;
            pnlOnlineMemberList.Visible = showOnlineMembersList;
            pnlUserChart.Visible = showMembershipGraph;
            zgMembershipGrowth.Height = membershipGraphHeight;
            zgMembershipGrowth.Width = membershipGraphWidth;
            zgMembershipGrowth.Title = Resource.MemberGraphTitle;
            
            
        }

        private void OnRenderUserChart(ZedGraph.Web.ZedGraphWeb z, System.Drawing.Graphics g, ZedGraph.MasterPane masterPane)
        {

            GraphPane graphPane = masterPane[0];
            graphPane.Title.Text = Resource.MemberGraphTitle;
            graphPane.XAxis.Title.Text = Resource.MemberGraphXAxisLabel;
            graphPane.YAxis.Title.Text = Resource.MemberGraphYAxisLabel;
            
            PointPairList pointList = new PointPairList();

            using (IDataReader reader = SiteUser.GetUserCountByYearMonth(siteSettings.SiteId))
            {
                while (reader.Read())
                {
                    double x = new XDate(Convert.ToInt32(reader["Y"]), Convert.ToInt32(reader["M"]), 1);
                    double y = Convert.ToDouble(reader["Users"]);
                    pointList.Add(x, y);
                }
            }

            LineItem myCurve2 = graphPane.AddCurve(Resource.MemberGraphYAxisLabel, pointList, Color.Blue, SymbolType.Circle);
            // Fill the area under the curve with a white-red gradient at 45 degrees
            myCurve2.Line.Fill = new Fill(Color.White, Color.Green, 45F);
            // Make the symbols opaque by filling them with white
            myCurve2.Symbol.Fill = new Fill(Color.White);

            // Set the XAxis to date type
            graphPane.XAxis.Type = AxisType.Date;
            graphPane.XAxis.CrossAuto = true;

            // Fill the axis background with a color gradient
            graphPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45F);

           

            masterPane.AxisChange(g);
        }

        private void LoadSettings()
        {
            showMembershipStatistics = Settings.Contains("SiteStatistics_ShowMemberStatisitics")
                                           ? bool.Parse(Settings["SiteStatistics_ShowMemberStatisitics"].ToString())
                                           : WebConfigSettings.SiteStatisticsShowMemberStatisticsDefault;

            showOnlineStatistics = Settings.Contains("SiteStatistics_ShowOnlineStatistics")
                                       ? bool.Parse(Settings["SiteStatistics_ShowOnlineStatistics"].ToString())
                                       : WebConfigSettings.SiteStatisticsShowOnlineStatisticsDefault;

            showOnlineMembersList = Settings.Contains("SiteStatistics_ShowOnlineMembers")
                                        ? bool.Parse(Settings["SiteStatistics_ShowOnlineMembers"].ToString())
                                        : WebConfigSettings.SiteStatisticsShowOnlineMembersDefault;

            showMembershipGraph = WebUtils.ParseBoolFromHashtable(
                Settings, "ShowMemberGraphSetting", showMembershipGraph);

            membershipGraphHeight = WebUtils.ParseInt32FromHashtable(
                Settings, "MemberGraphHeightSetting", membershipGraphHeight);

            membershipGraphWidth = WebUtils.ParseInt32FromHashtable(
                Settings, "MemberGraphWidthSetting", membershipGraphWidth);


            //

            
            if(
                (!Request.IsAuthenticated)
                &&(!WebConfigSettings.AllowAnonymousUsersToViewMemberList))
            {
                showOnlineMembersList = false;
            }
        }

    }
}
