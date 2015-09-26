<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BingMapStyleSetting.ascx.cs" Inherits="mojoPortal.Web.UI.BingMapStyleSetting" %>
<asp:DropDownList ID="dd" runat="server" EnableTheming="false" CssClass="forminput">
    <asp:ListItem Value="VEMapStyle.Road" Text="<%$ Resources:Resource, BingMapRoad %>" />
    <asp:ListItem Value="VEMapStyle.Shaded" Text="<%$ Resources:Resource, BingMapShaded %>" />
    <asp:ListItem Value="VEMapStyle.Aerial" Text="<%$ Resources:Resource, BingMapAerial %>" />
    <asp:ListItem Value="VEMapStyle.Hybrid" Text="<%$ Resources:Resource, BingMapHybrid %>" />
    <asp:ListItem Value="VEMapStyle.Birdseye" Text="<%$ Resources:Resource, BingMapBirdseye %>" />
    <asp:ListItem Value="VEMapStyle.BirdseyeHybrid" Text="<%$ Resources:Resource, BingMapBirdseyeHybrid %>" />
</asp:DropDownList>
