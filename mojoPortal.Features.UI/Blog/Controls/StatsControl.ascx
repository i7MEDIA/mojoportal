<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StatsControl.ascx.cs" Inherits="mojoPortal.Web.BlogUI.StatsControl" %>
<asp:Literal ID="litHeadingOpenTag" runat="server" EnableViewState="false" />
<asp:Literal ID="litHeading" runat="server" EnableViewState="false" />
<asp:Literal ID="litHeadingCloseTag" runat="server" EnableViewState="false" />
<ul class="blognav">
<li><asp:Literal id="litEntryCount" Runat="server" /></li>
<li id="liComments" runat="server"><asp:Literal id="litCommentCount" Runat="server" /></li>
</ul>
