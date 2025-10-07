<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MarkupDefinitionSetting.ascx.cs" Inherits="SuperFlexiUI.MarkupDefinitionSetting" %>
<asp:UpdatePanel ID="pnlDefinitions" runat="server" EnableViewState="true" UpdateMode="Conditional">
	<ContentTemplate>
		<asp:DropDownList ID="ddDefinitions" runat="server" />
		<asp:Button ID="btnEnableChange" runat="server" Visible="false" />
	</ContentTemplate>
</asp:UpdatePanel>
