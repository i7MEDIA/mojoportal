<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="CountryStateSetting.ascx.cs" Inherits="mojoPortal.Web.UI.CountryStateSetting" %>

<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" RenderMode="Inline" class="settingblock">
<ContentTemplate>
<div class="settingrow">
    <asp:DropDownList ID="ddCountry" runat="server" AutoPostBack="true" DataValueField="ISOCode2" DataTextField="Name" />
</div>
<div class="settingrow">
    <asp:DropDownList ID="ddGeoZone" runat="server" DataValueField="Code" DataTextField="Name" AutoPostBack="true" />
</div>
</ContentTemplate>
</asp:UpdatePanel>