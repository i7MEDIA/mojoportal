<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="HtmlFragmentInclude.ascx.cs" Inherits="mojoPortal.Plugins.HtmlInclude.HtmlIncludeModule" %>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
	<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper htmlfraginclude">
		<portal:ModuleTitleControl ID="Title1" runat="server" />
		<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
			<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
				<asp:Literal ID="litInclude" runat="server" />
			</portal:InnerBodyPanel>
		</portal:OuterBodyPanel>
	</portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
