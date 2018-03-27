<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OnlineStatisticsControl.ascx.cs" Inherits="mojoPortal.Web.UI.OnlineStatisticsControl" %>
<h3>
	<mp:sitelabel id="Sitelabel6" runat="server" ConfigKey="SiteStatisticsPeopleOnlineLabel" />
</h3>
<ul class="userstats">
	<li class="visitors">
		<mp:sitelabel id="Sitelabel7" runat="server" ConfigKey="SiteStatisticsVisitorsLabel" />
		<asp:Label ID="lblVisitorsOnline" runat="server" EnableViewState="false" />
	</li>
	<li class="members">
		<mp:sitelabel id="lbl2" runat="server" ConfigKey="SiteStatisticsMembersLabel" />
		<asp:Label ID="lblMembersOnline" runat="server" EnableViewState="false" />
	</li>
	<li class="total">
		<mp:sitelabel id="lbl3" runat="server" ConfigKey="SiteStatisticsTotalOnlineLabel" />
		<asp:Label ID="lblTotalOnline" runat="server" EnableViewState="false" />
	</li>
</ul>    