<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OnlineStatisticsControl.ascx.cs" Inherits="mojoPortal.Web.UI.OnlineStatisticsControl" %>
<ul class="userstats">
        <li>
            <asp:Image ID="imgPeopleOnline" runat="server" EnableViewState="false" />
            <mp:sitelabel id="Sitelabel6" runat="server" ConfigKey="SiteStatisticsPeopleOnlineLabel" UseLabelTag="false" />
        </li>
        <li>
            <ul class="userstats">
                <li>
                    <asp:Image ID="imgVistitorsOnline" runat="server" EnableViewState="false" />
                    <mp:sitelabel id="Sitelabel7" runat="server" ConfigKey="SiteStatisticsVisitorsLabel" UseLabelTag="false" />
                    <asp:Label ID="lblVisitorsOnline" runat="server" EnableViewState="false" />
                </li>
                <li>
                    <asp:Image ID="imgMembersOnline" runat="server" EnableViewState="false" />
                    <mp:sitelabel id="lbl2" runat="server" ConfigKey="SiteStatisticsMembersLabel" UseLabelTag="false" />
                    <asp:Label ID="lblMembersOnline" runat="server" EnableViewState="false" />
                </li>
                <li>
                    <asp:Image ID="imgTotalOnline" runat="server" EnableViewState="false" />
                    <mp:sitelabel id="lbl3" runat="server" ConfigKey="SiteStatisticsTotalOnlineLabel" UseLabelTag="false" />
                    <asp:Label ID="lblTotalOnline" runat="server" EnableViewState="false" />
                </li>
            </ul>
        </li>
    </ul>
    