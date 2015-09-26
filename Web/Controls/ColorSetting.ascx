<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ColorSetting.ascx.cs" Inherits="mojoPortal.Web.UI.ColorSetting" %>
<asp:TextBox ID="txtHexColor" runat="server" MaxLength="7" />
<span id="spnButton" runat="server" style="border: 1px solid black;padding: 1px 10px;cursor: hand;">&nbsp;</span>
<asp:Panel ID="pnlPickerContainer" runat="server">
<asp:Panel ID="pnlPicker" runat="server"></asp:Panel>
</asp:Panel>
