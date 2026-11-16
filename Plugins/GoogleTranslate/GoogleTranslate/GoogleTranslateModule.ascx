<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="GoogleTranslateModule.ascx.cs" Inherits="mojoPortal.Features.UI.GoogleTranslateModule" %>


<asp:Panel ID="pnlWrapper" runat="server" cssclass="panelwrapper GoogleTranslateModule">
<portal:ModuleTitleControl runat="server" id="TitleControl" UseHeading="false" />
<asp:Panel ID="pnlGoogleTranslateModule" runat="server" CssClass="modulecontent">
<portal:GoogleTranslatePanel id="gt1" runat="server" />
</asp:Panel>
</asp:Panel>

