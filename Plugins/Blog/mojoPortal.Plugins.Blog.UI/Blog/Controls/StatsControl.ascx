<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StatsControl.ascx.cs" Inherits="mojoPortal.Web.BlogUI.StatsControl" %>
<%@ Register Namespace="mojoPortal.Web.BlogUI" Assembly="mojoPortal.Features.UI" TagPrefix="blog" %>

<blog:BlogDisplaySettings ID="displaySettings" runat="server" />

<asp:Literal ID="litHeadingOpenTag" runat="server" EnableViewState="false" />
	<asp:Literal ID="litHeading" runat="server" EnableViewState="false" />
<asp:Literal ID="litHeadingCloseTag" runat="server" EnableViewState="false" />

<asp:BulletedList runat="server" ID="listStats"></asp:BulletedList>