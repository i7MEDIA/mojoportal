<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="Default.aspx.cs" Inherits="mojoPortal.Web.AdminUI.DesignerToolsPage" %>

<asp:content contentplaceholderid="leftContent" id="MPLeftPane" runat="server" />
<asp:content contentplaceholderid="mainContent" id="MPContent" runat="server">
    <portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
        <asp:hyperlink id="lnkAdminMenu" runat="server" navigateurl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:hyperlink id="lnkAdvancedTools" runat="server" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:hyperlink id="lnkThisPage" runat="server" cssclass="selectedcrumb" />
    </portal:AdminCrumbContainer>
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
        <mp:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
        <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" cssclass="panelwrapper adminmenu ">
            <portal:HeadingControl id="heading" runat="server" />
            <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
                <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
                    <ul class="simplelist">
                        <li>
                            <asp:hyperlink id="lnkSkinList" runat="server" cssclass="lnkSkinList" />
                        </li>
                        <li>
                            <asp:hyperlink id="lnkCacheTool" runat="server" cssclass="lnkCacheTool" />
                        </li>
                        <li id="liLessUtility" runat="server">
                            <asp:hyperlink id="lnkLessUtility" runat="server" cssclass="lnkLessUtility" />
                        </li>
                        <asp:literal id="litSupplementalLinks" runat="server" />
                    </ul>
                </portal:InnerBodyPanel>
            </portal:OuterBodyPanel>
            <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
        </portal:InnerWrapperPanel>
        <mp:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
    </portal:OuterWrapperPanel>
</asp:content>
<asp:content contentplaceholderid="rightContent" id="MPRightPane" runat="server" />
<asp:content contentplaceholderid="pageEditContent" id="MPPageEdit" runat="server" />
