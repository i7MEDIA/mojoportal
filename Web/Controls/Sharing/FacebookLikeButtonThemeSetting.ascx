<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FacebookLikeButtonThemeSetting.ascx.cs" Inherits="mojoPortal.Web.UI.FacebookLikeButtonThemeSetting" %>

<asp:DropDownList ID="dd" runat="server" EnableTheming="false" CssClass="forminput">
    <asp:ListItem Value="light" Text="<%$ Resources:Resource, FBLikeLightTheme %>" />
    <asp:ListItem Value="dark" Text="<%$ Resources:Resource, FBLikeDarkTheme %>" />
   
</asp:DropDownList>