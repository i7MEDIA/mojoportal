<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    Codebehind="AdminDashboard.aspx.cs" Inherits="WebStore.UI.AdminDashboardPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop id="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper webstore webstoreadmin">
        <portal:HeadingControl ID="heading" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
        <ul class="simplelist">
            <li>
                <asp:HyperLink ID="lnkStoreSettings" runat="server" />
            </li>
            <li>
                <asp:HyperLink ID="lnkProductAdmin" runat="server" />
            </li>
            <li>
                <asp:HyperLink ID="lnkOfferAdmin" runat="server" />
            </li>
            <li id="liCategories" runat="server" Visible="false" >
                <asp:HyperLink ID="lnkCategoryAdmin" runat="server" />
            </li>
            <li>
                <asp:HyperLink ID="lnkDownloadTermsAdmin" runat="server" />
            </li>
            <li>
                <asp:HyperLink ID="lnkDiscountAdmin" runat="server" />
            </li>
            <li>
                <asp:HyperLink ID="lnkOrderEntry" runat="server" />
            </li>
            <li>
                <asp:HyperLink ID="lnkOrderHistory" runat="server" />
            </li>
            <li>
                <asp:HyperLink ID="lnkBrowseCarts" runat="server" />
            </li>
            <li id="liReports" runat="server" Visible="false" >
                <asp:HyperLink ID="lnkReports" runat="server" />
                <portal:mojoButton ID="btnRebuildReports" runat="server"  CausesValidation="false" Visible="false" />
                
            </li>
        </ul>
        </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom id="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
