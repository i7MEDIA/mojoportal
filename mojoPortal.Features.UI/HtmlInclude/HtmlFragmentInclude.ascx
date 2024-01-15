<%@ Control Language="c#" AutoEventWireup="false" Codebehind="HtmlFragmentInclude.ascx.cs" Inherits="mojoPortal.Web.ContentUI.HtmlFragmentInclude" %>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">

<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper htmlfraginclude">
<portal:ModuleTitleControl id="Title1" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<asp:Literal ID="lblInclude" Runat="server" />
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>

</portal:InnerWrapperPanel>

</portal:OuterWrapperPanel>
		