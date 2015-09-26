<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ProductList.aspx.cs" Inherits="WebStore.UI.ProductListPage" %>

<%@ Register Src="~/WebStore/Controls/CartLink.ascx" TagPrefix="ws" TagName="CartLink" %>
<%@ Register Src="~/WebStore/Controls/ProductListControl.ascx" TagPrefix="ws" TagName="ProductList" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
        <mp:CornerRounderTop ID="ctop1" runat="server" />
        <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper webstore webstoreproductlist">
            <portal:ModuleTitleControl EditText="Edit" runat="server" ID="TitleControl" EnableViewState="false" />
            <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
                <portal:InnerBodyPanel ID="pnlProductList" runat="server" CssClass="modulecontent">
                    <div class="settingrow">
                        <ws:CartLink ID="lnkCart" runat="server" EnableViewState="false" />
                    </div>
                    <h3><asp:Literal ID="litProductListHeading" runat="server" EnableViewState="false"></asp:Literal></h3>
                    <ws:ProductList id="productList1" runat="server" Visible="true" />
                </portal:InnerBodyPanel>
            </portal:OuterBodyPanel>
            <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
        </portal:InnerWrapperPanel>
        <mp:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
