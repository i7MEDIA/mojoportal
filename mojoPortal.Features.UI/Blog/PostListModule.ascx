<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="PostListModule.ascx.cs" Inherits="mojoPortal.Web.BlogUI.PostListModule" %>
<%@ Register Namespace="mojoPortal.Web.BlogUI" Assembly="mojoPortal.Features.UI" TagPrefix="blog" %>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper blog-postlist">
        <portal:ModuleTitleControl ID="Title1" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
            <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
            <%--<blog:BlogWrapperPanel id="blogWrapper1" runat="server" CssClass="blogwrapper">--%>
                <blog:BlogDisplaySettings ID="displaySettings" runat="server" />
                <%--<blog:SearchBox id="searchBoxTop" runat="server" />--%>
                <%--<blog:PostList ID="postList" runat="server" />--%>
                <blog:PostListRazor ID="postList" runat="server" />
            <%--</blog:BlogWrapperPanel>--%>
            <portal:EmptyPanel id="divFooter" runat="server" CssClass="modulefooter" SkinID="modulefooter"></portal:EmptyPanel>
            </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
