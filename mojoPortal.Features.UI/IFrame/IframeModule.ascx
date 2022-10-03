<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="IframeModule.ascx.cs" Inherits="mojoPortal.Features.UI.IframeModule" %>

<asp:Panel ID="pnlWrapper" runat="server" cssclass="panelwrapper IframeModule">
	<portal:ModuleTitleControl  runat="server" id="TitleControl" UseHeading="false" />
	<asp:Panel ID="pnlIframeModule" runat="server" CssClass="modulecontent">
		<asp:Literal ID="litFrame" runat="server" />

	</asp:Panel>
</asp:Panel>

