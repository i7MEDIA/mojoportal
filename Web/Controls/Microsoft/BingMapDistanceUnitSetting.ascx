<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BingMapDistanceUnitSetting.ascx.cs" Inherits="mojoPortal.Web.UI.BingMapDistanceUnitSetting" %>
<asp:DropDownList ID="dd" runat="server" EnableTheming="false" CssClass="forminput">
    <asp:ListItem Value="VERouteDistanceUnit.Mile" Text="<%$ Resources:Resource, Mile %>" />
    <asp:ListItem Value="VERouteDistanceUnit.Kilometer" Text="<%$ Resources:Resource, Kilometer %>" />
</asp:DropDownList>
