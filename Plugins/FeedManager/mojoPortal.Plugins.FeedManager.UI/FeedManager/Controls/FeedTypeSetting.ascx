<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeedTypeSetting.ascx.cs" Inherits="mojoPortal.FeedUI.FeedTypeSetting" %>

<asp:DropDownList ID="ddFeedType" runat="server">
	<asp:ListItem Value="Rss" Text="<%$ Resources:FeedResources, RssFeedType %>" />
	<asp:ListItem Value="Atom" Text="<%$ Resources:FeedResources, AtomFeedType %>" />
</asp:DropDownList>