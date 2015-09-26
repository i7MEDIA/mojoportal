<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SiteStatisticsModule.ascx.cs" Inherits="mojoPortal.Web.StatisticsUI.SiteStatisticsModule" %>

<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper stats">
<portal:ModuleTitleControl id="Title1" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<asp:Panel ID="pnlMembership" runat="server" CssClass="site-statistics floatpanel">
    <portal:MembershipStatistics id="st1" runat="server" />
</asp:Panel>
<asp:Panel ID="pnlUsersOnline" runat="server" CssClass="site-statistics floatpanel">
    <portal:OnlineStatistics ID="ol1" runat="server" />
</asp:Panel>
<asp:Panel ID="pnlOnlineMemberList" runat="server" CssClass="clearpanel onlinemembers">
    <portal:OnlineMemberList ID="olm1" runat="server" />
</asp:Panel>
<asp:Panel ID="pnlUserChart" runat="server" CssClass="clearpanel membergraph">
 <zgw:zedgraphweb id="zgMembershipGrowth" runat="server" RenderMode="ImageTag"
    Width="780" Height="400"></zgw:zedgraphweb>
</asp:Panel>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
