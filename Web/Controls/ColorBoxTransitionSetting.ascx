<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CBTransitionSetting.ascx.cs" Inherits="mojoPortal.Web.UI.CBTransitionSetting" %>

<asp:DropDownList ID="cbTransition" runat="server" EnableTheming="false" CssClass="forminput">
	<asp:ListItem Value="elastic" Text="<%$ Resources:Resource, ColorBoxTransitionElastic %>" />
	<asp:ListItem Value="fade" Text="<%$ Resources:Resource, ColorBoxTransitionFade %>" />
	<asp:ListItem Value="none" Text="<%$ Resources:Resource, ColorBoxTransitionNone %>" />
</asp:DropDownList>