<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BetterImageGalleryModule.ascx.cs" Inherits="mojoPortal.Features.UI.BetterImageGalleryModule" %>
<%@ Register Namespace="mojoPortal.Features.UI" Assembly="mojoPortal.Features.UI" TagPrefix="feat" %>

<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper blog-postlist">
        <portal:ModuleTitleControl ID="Title1" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
            <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
                <%--<blog:BlogDisplaySettings ID="displaySettings" runat="server" />--%>
                <feat:BetterImageGalleryRazor ID="postList" runat="server" />
	            <portal:EmptyPanel id="divFooter" runat="server" CssClass="modulefooter" SkinID="modulefooter" />
            </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared" />
    </portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>