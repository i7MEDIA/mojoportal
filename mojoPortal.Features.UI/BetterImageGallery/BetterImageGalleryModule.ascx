<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BetterImageGalleryModule.ascx.cs" Inherits="mojoPortal.Features.UI.BetterImageGallery.BetterImageGalleryModule" %>
<%@ Register Namespace="mojoPortal.Features.UI.BetterImageGallery" Assembly="mojoPortal.Features.UI" TagPrefix="feat" %>

<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
	<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper big-module">
		<portal:ModuleTitleControl ID="Title1" runat="server" />
		<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
			<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
				<feat:BetterImageGalleryRazor ID="gallery1" runat="server" />
				<portal:EmptyPanel ID="divFooter" runat="server" CssClass="modulefooter" SkinID="modulefooter" />
			</portal:InnerBodyPanel>
		</portal:OuterBodyPanel>
		<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared" />
	</portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>