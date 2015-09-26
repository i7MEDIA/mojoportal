<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="CoreData.aspx.cs" Inherits="mojoPortal.Web.AdminUI.CoreDataPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkCurrentPage" runat="server" CssClass="selectedcrumb" />
    </portal:AdminCrumbContainer>
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper adminmenu">
        <portal:HeadingControl id="heading" runat="server" />
    <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
            <ul class="simplelist">
                <li id="liLanguageAdmin" runat="server">
                    <asp:HyperLink ID="lnkLanguageAdmin" runat="server" CssClass="lnkLanguageAdmin" />
                </li>
                <li id="liCurrencyAdmin" runat="server">
                    <asp:HyperLink ID="lnkCurrencyAdmin" runat="server" CssClass="lnkCurrencyAdmin" />
                </li>
                <li id="liCountryAdmin" runat="server">
                    <asp:HyperLink ID="lnkCountryAdmin" runat="server" CssClass="lnkCountryAdmin" />
                </li>
                <li id="liGeoZoneAdmin" runat="server">
                    <asp:HyperLink ID="lnkGeoZone" runat="server" CssClass="lnkGeoZone" />
                </li>
                <li id="liTaxClassAdmin" runat="server">
                    <asp:HyperLink ID="lnkTaxClassAdmin" runat="server" CssClass="lnkTaxClassAdmin" />
                </li>
                <li id="liTaxRateAdmin" runat="server">
                    <asp:HyperLink ID="lnkTaxRateAdmin" runat="server" CssClass="lnkTaxRateAdmin" />
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
