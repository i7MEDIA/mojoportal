<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="NewsLetterSubscribeModule.ascx.cs" Inherits="mojoPortal.Web.ELetterUI.NewsLetterSubscribeModuleModule" %>

<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
	<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper NewsLetterSubscribeModule">
		<portal:ModuleTitleControl runat="server" ID="TitleControl" />
		<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
			<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
				<portal:Subscribe ID="subscribe1" runat="server" />
			</portal:InnerBodyPanel>
		</portal:OuterBodyPanel>
	</portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
