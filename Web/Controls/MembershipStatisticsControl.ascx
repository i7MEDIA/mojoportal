<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MembershipStatisticsControl.ascx.cs" Inherits="mojoPortal.Web.MembershipStatisticsControl" %>
<h3>
    <mp:sitelabel id="Sitelabel4" runat="server" ConfigKey="SiteStatisticsMembershipLabel" />
</h3>
<ul class="userstats">
    <li class="newtoday">
        <mp:sitelabel id="Sitelabel2" runat="server" ConfigKey="SiteStatisticsNewTodayLabel" />
        <asp:Label ID="lblNewUsersToday" runat="server" EnableViewState="false" />
    </li>
	<li class="newyesterday">
		<mp:sitelabel id="Sitelabel3" runat="server" ConfigKey="SiteStatisticsNewYesterdayLabel" />
        <asp:Label ID="lblNewUsersYesterday" runat="server" EnableViewState="false" />
    </li>
	<li class="total">
		<mp:sitelabel id="Sitelabel5" runat="server" ConfigKey="SiteStatisticsTotalUsersLabel" />
        <asp:Label ID="lblTotalUsers" runat="server" EnableViewState="false" />
    </li>
	<li class="newestuser">
		<mp:sitelabel id="Sitelabel1" runat="server" ConfigKey="SiteStatisticsNewestMemberLabel" />
        <asp:Label ID="lblNewestUser" runat="server" EnableViewState="false" CssClass="username" />
    </li>
</ul>