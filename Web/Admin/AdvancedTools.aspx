<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="AdvnacedTools.aspx.cs" Inherits="mojoPortal.Web.AdminUI.AdvnacedToolsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkCurrentPage" runat="server" CssClass="selectedcrumb" />
    </portal:AdminCrumbContainer>
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper adminmenu">
        <portal:HeadingControl id="heading" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
            <ul class="simplelist">
                <li id="liUrlManager" runat="server">
                    <asp:HyperLink ID="lnkUrlManager" runat="server" CssClass="lnkUrlManager" />
                </li>
                <li id="liRedirectManager" runat="server">
                    <asp:HyperLink ID="lnkRedirectManager" runat="server" CssClass="lnkRedirectManager" />
                </li>
                <li id="liBannedIPs" runat="server">
                    <asp:HyperLink ID="lnkBannedIPs" runat="server" CssClass="lnkBannedIPs" />
                </li>
                <li id="liFeatureAdmin" runat="server">
                    <asp:HyperLink ID="lnkFeatureAdmin" runat="server" CssClass="lnkFeatureAdmin" />
                </li>
                <li id="liWebPartAdmin" runat="server">
                    <asp:HyperLink ID="lnkWebPartAdmin" runat="server" CssClass="lnkWebPartAdmin" />
                </li>
                <li id="liTaskQueue" runat="server">
                    <asp:HyperLink ID="lnkTaskQueue" runat="server" CssClass="lnkTaskQueue" />
                </li>
                <li id="liDesignTools" runat="server">
                    <asp:HyperLink ID="lnkDesignTools" runat="server" CssClass="lnkDesignTools" />
                </li>
                <li id="liDevTools" runat="server">
                    <asp:HyperLink ID="lnkDevTools" runat="server" CssClass="lnkDevTools" />
                </li>
                
                <asp:Literal ID="litSupplementalLinks" runat="server" />
            </ul>
        </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
