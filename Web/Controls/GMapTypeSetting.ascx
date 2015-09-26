<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GMapTypeSetting.ascx.cs" Inherits="mojoPortal.Web.UI.GMapTypeSetting" %>

<asp:DropDownList ID="ddGMapType" runat="server" EnableTheming="false" CssClass="forminput">
    <asp:ListItem Value="G_NORMAL_MAP" Text="<%$ Resources:Resource, GoogleNormalMap %>" />
    <asp:ListItem Value="G_SATELLITE_MAP" Text="<%$ Resources:Resource, GoogleSatelliteMap %>" />
    <asp:ListItem Value="G_HYBRID_MAP" Text="<%$ Resources:Resource, GoogleHybridMap %>" />
    <asp:ListItem Value="G_PHYSICAL_MAP" Text="<%$ Resources:Resource, GoogleTerrainMap %>" />
</asp:DropDownList>
