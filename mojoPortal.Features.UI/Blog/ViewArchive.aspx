<%@ Page language="c#" Codebehind="ViewArchive.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.BlogUI.BlogArchiveView" %>
<%@ Register TagPrefix="blog" TagName="NavControl" Src="~/Blog/Controls/BlogNav.ascx" %>
<%@ Register Namespace="mojoPortal.Web.BlogUI" Assembly="mojoPortal.Features.UI" TagPrefix="blog" %>
<%@ Register TagPrefix="blog" TagName="PostList" Src="~/Blog/Controls/PostList.ascx" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" cssclass="panelwrapper blogmodule blogcategories blogarchive ">
    <portal:HeadingControl ID="heading" runat="server" />
    <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
            <asp:panel id="pnlBlog" runat="server" cssclass="blogwrapper">
            <blog:BlogDisplaySettings ID="displaySettings" runat="server" />
            <blog:PostList ID="postList" runat="server" />
            </asp:panel>
        </portal:InnerBodyPanel>
    </portal:OuterBodyPanel>
    <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />	
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
