<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VertigoThemeSetting.ascx.cs" Inherits="mojoPortal.Web.UI.VertigoThemeSetting" %>
<asp:DropDownList ID="dd" runat="server" EnableTheming="false" CssClass="forminput">
    <asp:ListItem Value="SimpleTheme" Text="<%$ Resources:Resource, VertigoSimpleTheme %>" />
    <asp:ListItem Value="LightTheme" Text="<%$ Resources:Resource, VertigoLightTheme %>" />
    <asp:ListItem Value="DarkTheme" Text="<%$ Resources:Resource, VertigoDarkTheme %>" />
</asp:DropDownList>
