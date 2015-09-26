<%@ Page Language="c#" CodeBehind="ViewCategory.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.BlogUI.BlogCategoryView" %>
<%@ Register TagPrefix="blog" TagName="NavControl" Src="~/Blog/Controls/BlogNav.ascx" %>
<%@ Register TagPrefix="blog" TagName="PostList" Src="~/Blog/Controls/PostList.ascx" %>
<%@ Register Namespace="mojoPortal.Web.BlogUI" Assembly="mojoPortal.Features.UI" TagPrefix="blog" %>
<asp:content contentplaceholderid="leftContent" id="MPLeftPane" runat="server" />
<asp:content contentplaceholderid="mainContent" id="MPContent" runat="server">
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
        <mp:CornerRounderTop ID="ctop1" runat="server" />
        <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper blogmodule blogcategories ">
            <portal:HeadingControl ID="heading" runat="server" />
            <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
                <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
                    <asp:panel id="pnlBlog" runat="server" cssclass="blogwrapper">
                        <blog:BlogDisplaySettings ID="displaySettings" runat="server" />
                        <blog:PostList ID="postList" runat="server" />
                    </asp:panel> 
                </portal:InnerBodyPanel>
            </portal:OuterBodyPanel>
            <portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared">
            </portal:EmptyPanel>
        </portal:InnerWrapperPanel>
        <mp:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:content>
<asp:content contentplaceholderid="rightContent" id="MPRightPane" runat="server" />
<asp:content contentplaceholderid="pageEditContent" id="MPPageEdit" runat="server" />
