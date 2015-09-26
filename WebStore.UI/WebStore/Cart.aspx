<%@ Page Language="C#" AutoEventWireup="false"  MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Cart.aspx.cs" Inherits="WebStore.UI.CartPage" %>
<%@ Register TagPrefix="webstore" TagName="CartList" Src="~/WebStore/Controls/CartList.ascx" %>
<%@ Register TagPrefix="webstore" TagName="CartListAlt" Src="~/WebStore/Controls/CartListAlt.ascx" %>
<%@ Register Namespace="WebStore.UI" Assembly="WebStore.UI" TagPrefix="webstore" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper webstore webstorecart">
        <portal:HeadingControl ID="heading" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
        <webstore:WebStoreDisplaySettings id="displaySettings" runat="server" />
            <asp:Panel ID="pnlCartItems" runat="server" CssClass="cart">
                <webstore:CartList ID="cartList" runat="server" />
                <webstore:CartListAlt ID="cartListAlt" runat="server" Visible="false" />
            </asp:Panel>
            <hr class="subtotalbreak" />
            <asp:Panel ID="pnlSubTotal" runat="server" CssClass="settingrowtight  carttotalwrapper storerow subtotal">
                <mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartSubTotalLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Literal ID="litSubTotal" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlDiscountAmount" runat="server" CssClass="settingrowtight carttotalwrapper storerow discount">
                <mp:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartDiscountTotalLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Literal ID="litDiscount" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlTotal" runat="server" CssClass="settingrowtight carttotalwrapper storerow total">
                <mp:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartTotalLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Literal ID="litTotal" runat="server" />
            </asp:Panel>
           
            <asp:Panel ID="pnlDiscountCode" runat="server" CssClass="settingrow discountcode">   
                <mp:SiteLabel ID="SiteLabel4" runat="server" CssClass="storelabel" ConfigKey="CartDiscountCodeLabel" ResourceFile="WebStoreResources" />
                <asp:TextBox ID="txtDiscountCode" runat="server" CssClass="discountcode" />
                <portal:mojoButton ID="btnApplyDiscount" runat="server"  />
                <portal:mojoLabel ID="lblDiscountError" runat="server" CssClass="txterror warning" />        
             </asp:Panel>
            
            <div class="settingrow checkoutlinks">
                &nbsp;<asp:HyperLink ID="lnkCheckout" runat="server" CssClass="checkoutlink" />
                &nbsp;<asp:HyperLink ID="lnkKeepShopping" runat="server" CssClass="keepshopping" />
            </div>
            <div class="settingrow ortext">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Literal ID="litOr" runat="server" Visible="false" />
            </div>
            <div class="settingrow">
                <asp:ImageButton ID="btnPayPal" runat="server" ImageUrl="https://www.paypal.com/en_US/i/btn/btn_xpressCheckout.gif"
                    AlternateText="Checkout with PayPal" Visible="false" />
                    <portal:mojoGCheckoutButton ID="btnGoogleCheckout" runat="server" Visible="false" />
                <br />
                    <portal:mojoLabel ID="lblMessage" runat="server" CssClass="txterror info"></portal:mojoLabel>
                <asp:Literal ID="litPayPalFormVariables" runat="server" />
            </div>
            <div class="settingrow">
            <portal:mojoLabel ID="lblGoogleMessage" runat="server" CssClass="txterror info" Visible="false"></portal:mojoLabel> 
            </div>
            <portal:CommerceTestModeWarning ID="commerceWarning" runat="server" />
            <div class="clearpanel">
                <portal:PaymentAcceptanceMark ID="pam1" runat="server" />
            </div>
            <asp:Literal ID="litCartFooter" runat="server" EnableViewState="false" />
        </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
