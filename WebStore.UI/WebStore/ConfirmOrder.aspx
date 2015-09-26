<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ConfirmOrder.aspx.cs" Inherits="WebStore.UI.ConfirmOrderPage" MaintainScrollPositionOnPostback="true" %>
<%@ Register Src="~/WebStore/Controls/CartLink.ascx" TagPrefix="ws" TagName="CartLink" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
   <ws:CartLink ID="lnkCart" runat="server" EnableViewState="false" />
</div>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper webstore webstoreconfirmorder">
        <portal:HeadingControl ID="heading" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
            <asp:Panel ID="pnlRequireLogin" runat="server">
                <portal:SignInOrRegisterPrompt ID="srPrompt" runat="server" ShowJanrainWidget="true" />
            </asp:Panel>
            <asp:Panel ID="pnlCartItems" runat="server" CssClass="clearpanel">
                <h3 class="heading cartheading">
                    <asp:Literal ID="litCartHeader" runat="server" /></h3>
                    <asp:Repeater ID="rptCartItems" runat="server">
                        <HeaderTemplate>
                            <table class="cartgrid">
                            <tr>
                                <th>
                                    <%# Resources.WebStoreResources.CartItemsHeading%>
                                </th>
                                <th>
                                    <%# Resources.WebStoreResources.CartPriceHeading%>
                                </th>
                                <th>
                                    <%# Resources.WebStoreResources.CartQuantityHeading%>
                                </th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("Name")%></td>
                                <td><%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("OfferPrice")))%></td>
                                <td><%# Eval("Quantity") %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                        </table>
                        </FooterTemplate>
                    </asp:Repeater>
            </asp:Panel>
            <hr />
            <div class="carttotalwrapper ">
                <asp:Panel ID="pnlSubTotal" runat="server" CssClass="settingrowtight storerow">
                        <mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartSubTotalLabel"
                            ResourceFile="WebStoreResources" />
                    <asp:Literal ID="litSubTotal" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlDiscount" runat="server" CssClass="settingrowtight storerow">
                        <mp:SiteLabel ID="SiteLabel11" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartDiscountTotalLabel"
                            ResourceFile="WebStoreResources" />
                    <asp:Literal ID="litDiscount" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlShippingTotal" runat="server" CssClass="settingrowtight storerow">
                        <mp:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartShippingTotalLabel"
                            ResourceFile="WebStoreResources" />
                    <asp:Literal ID="litShippingTotal" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlTaxTotal" runat="server" CssClass="settingrowtight storerow">
                        <mp:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabeltight storelabel taxtotal" ConfigKey="CartTaxTotalLabel"
                            ResourceFile="WebStoreResources" />
                    <asp:Literal ID="litTaxTotal" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlOrderTotal" runat="server" CssClass="settingrowtight storerow">
                        <mp:SiteLabel ID="SiteLabel7" runat="server" CssClass="settinglabeltight storelabel ordertotal" ConfigKey="CartOrderTotalLabel"
                            ResourceFile="WebStoreResources" />
                    <asp:Literal ID="litOrderTotal" runat="server" />
                </asp:Panel>
            </div>
            <asp:Panel ID="pnlOrderDetail" runat="server" CssClass="clear orderdetail" Visible="false">
                <asp:Panel ID="pnlCustomer" runat="server" CssClass="floatpanel" DefaultButton="btnSaveAndValidate">
                    <fieldset>
                        <legend>
                            <mp:SiteLabel ID="SiteLabel5" runat="server" ConfigKey="ConfirmOrderCustomerHeader"
                                ResourceFile="WebStoreResources" />
                        </legend>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerName" runat="server" ForControl="txtCustomerFirstName"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerFirstNameLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerFirstName" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                            <asp:RequiredFieldValidator ID="reqCustomerFirstName" runat="server" ControlToValidate="txtCustomerFirstName"
                                ValidationGroup="OrderInfo" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel4" runat="server" ForControl="txtCustomerLastName" CssClass="settinglabel"
                                ConfigKey="CartOrderInfoCustomerLastNameLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerLastName" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                            <asp:RequiredFieldValidator ID="reqCustomerLastName" runat="server" ControlToValidate="txtCustomerLastName"
                                ValidationGroup="OrderInfo" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerCompany" runat="server" ForControl="txtCustomerCompany"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerCompanyLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerCompany" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerAddressLine1" runat="server" ForControl="txtCustomerAddressLine1"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerAddressLine1Label" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerAddressLine1" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                            <asp:RequiredFieldValidator ID="reqCustomerAddress1" runat="server" ControlToValidate="txtCustomerAddressLine1"
                                ValidationGroup="OrderInfo" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerAddressLine2" runat="server" ForControl="txtCustomerAddressLine2"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerAddressLine2Label" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerAddressLine2" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerCountry" runat="server" ForControl="ddCustomerCountry"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerCountryLabel" ResourceFile="WebStoreResources" />
                            <asp:DropDownList ID="ddCustomerCountry" runat="server" AutoPostBack="true" DataValueField="ISOCode2"
                                DataTextField="Name" CssClass="forminput countrylistdd">
                            </asp:DropDownList>
                        </div>
                        <div class="settingrow" id="divCustomerState" runat="server">
                            <mp:SiteLabel ID="lblCustomerState" runat="server" ForControl="ddCustomerGeoZone"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerStateLabel" ResourceFile="WebStoreResources" />
                            <asp:DropDownList ID="ddCustomerGeoZone" runat="server" DataValueField="Code" DataTextField="Name" CssClass="forminput statelistdd">
                            </asp:DropDownList>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerSuburb" runat="server" ForControl="txtCustomerSuburb"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerSuburbLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerSuburb" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerCity" runat="server" ForControl="txtCustomerCity" CssClass="settinglabel"
                                ConfigKey="CartOrderInfoCustomerCityLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerCity" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                            <asp:RequiredFieldValidator ID="reqCustomerCity" runat="server" ControlToValidate="txtCustomerCity"
                                ValidationGroup="OrderInfo" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerPostalCode" runat="server" ForControl="txtCustomerPostalCode"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerPostalCodeLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerPostalCode" runat="server" MaxLength="20" CssClass="forminput normaltextbox" />
                            <asp:RequiredFieldValidator ID="reqCustomerPostalCode" runat="server" ControlToValidate="txtCustomerPostalCode"
                                ValidationGroup="OrderInfo" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerTelephoneDay" runat="server" ForControl="txtCustomerTelephoneDay"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerTelephoneDayLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerTelephoneDay" runat="server" MaxLength="32" CssClass="forminput normaltextbox" />
                            <asp:RequiredFieldValidator ID="reqCustomerDayPhone" runat="server" ControlToValidate="txtCustomerTelephoneDay"
                                ValidationGroup="OrderInfo" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerTelephoneNight" runat="server" ForControl="txtCustomerTelephoneNight"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerTelephoneNightLabel"
                                ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerTelephoneNight" runat="server" MaxLength="32" CssClass="forminput normaltextbox" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerEmail" runat="server" ForControl="txtCustomerEmail"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerEmailLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerEmail" runat="server" MaxLength="96" CssClass="forminput widetextbox" />
                            <asp:RequiredFieldValidator ID="reqCustomerEmail" runat="server" ControlToValidate="txtCustomerEmail"
                                ValidationGroup="OrderInfo" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <portal:EmailValidator ID="regexCustomerEmail" runat="server" ControlToValidate="txtCustomerEmail"
                                ValidationGroup="OrderInfo" SetFocusOnError="true"></portal:EmailValidator>
                        </div>
                    </fieldset>
                </asp:Panel>
                <asp:Panel ID="pnlBillingInfo" runat="server" CssClass="floatpanel ">
                    <fieldset>
                        <legend>
                            <mp:SiteLabel ID="SiteLabel10" runat="server" ConfigKey="ConfirmOrderBillingHeader"
                                ResourceFile="WebStoreResources" />
                        </legend>
                        <div class="settingrow">
                            <asp:Button ID="lnkCopyCustomerToBilling" runat="server" CssClass="buttonlink" CausesValidation="false" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblBillingName" runat="server" ForControl="txtBillingFirstName"
                                CssClass="storelabel" ConfigKey="CartOrderInfoBillingFirstNameLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtBillingFirstName" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                            <asp:RequiredFieldValidator ID="reqBillingFirstName" runat="server" ControlToValidate="txtBillingFirstName"
                                ValidationGroup="OrderInfo" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel8" runat="server" ForControl="txtBillingLastName" CssClass="storelabel"
                                ConfigKey="CartOrderInfoBillingLastNameLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtBillingLastName" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                            <asp:RequiredFieldValidator ID="reqBillingLastName" runat="server" ControlToValidate="txtBillingLastName"
                                ValidationGroup="OrderInfo" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblBillingCompany" runat="server" ForControl="txtBillingCompany"
                                CssClass="storelabel" ConfigKey="CartOrderInfoBillingCompanyLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtBillingCompany" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblBillingAddress1" runat="server" ForControl="txtBillingAddress1"
                                CssClass="storelabel" ConfigKey="CartOrderInfoBillingAddress1Label" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtBillingAddress1" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                            <asp:RequiredFieldValidator ID="reqBillingAddress1" runat="server" ControlToValidate="txtBillingAddress1"
                                ValidationGroup="OrderInfo" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblBillingAddress2" runat="server" ForControl="txtBillingAddress2"
                                CssClass="storelabel" ConfigKey="CartOrderInfoBillingAddress2Label" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtBillingAddress2" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblBillingSuburb" runat="server" ForControl="txtBillingSuburb"
                                CssClass="storelabel" ConfigKey="CartOrderInfoBillingSuburbLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtBillingSuburb" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblBillingCity" runat="server" ForControl="txtBillingCity" CssClass="storelabel"
                                ConfigKey="CartOrderInfoBillingCityLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtBillingCity" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                            <asp:RequiredFieldValidator ID="reqBillingCity" runat="server" ControlToValidate="txtBillingCity"
                                ValidationGroup="OrderInfo" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblBillingCountry" runat="server" ForControl="ddBillingCountry"
                                CssClass="storelabel" ConfigKey="CartOrderInfoBillingCountryLabel" ResourceFile="WebStoreResources" />
                            <asp:DropDownList ID="ddBillingCountry" runat="server" AutoPostBack="true" DataValueField="ISOCode2"
                                DataTextField="Name" CssClass="forminput countrylistdd">
                            </asp:DropDownList>
                        </div>
                        <div class="settingrow" id="divBillingState" runat="server">
                            <mp:SiteLabel ID="lblBillingState" runat="server" ForControl="ddBillingGeoZone" CssClass="storelabel"
                                ConfigKey="CartOrderInfoBillingStateLabel" ResourceFile="WebStoreResources" />
                            <asp:DropDownList ID="ddBillingGeoZone" runat="server" DataValueField="Code" DataTextField="Name" CssClass="forminput statelistdd">
                            </asp:DropDownList>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblBillingPostalCode" runat="server" ForControl="txtBillingPostalCode"
                                CssClass="storelabel" ConfigKey="CartOrderInfoBillingPostalCodeLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtBillingPostalCode" Columns="20" runat="server" MaxLength="20" />
                            <asp:RequiredFieldValidator ID="reqBillingPostalCode" runat="server" ControlToValidate="txtBillingPostalCode"
                                ValidationGroup="OrderInfo" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                    </fieldset>
                </asp:Panel>
                <asp:Panel ID="pnlShipping" runat="server" CssClass="floatpanel ">
                    <fieldset>
                        <legend>
                            <mp:SiteLabel ID="SiteLabel6" runat="server" ConfigKey="ConfirmOrderShippingHeader"
                                ResourceFile="WebStoreResources" />
                        </legend>
                        <div class="settingrow">
                            <asp:Button ID="lnkCopyCustomerToShipping" runat="server" CssClass="buttonlink" CausesValidation="false" />&nbsp;&nbsp;
                            <asp:Button ID="lnkCopyBillingToShipping" runat="server" CssClass="buttonlink" CausesValidation="false" Visible="false"/>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblDeliveryName" runat="server" ForControl="txtDeliveryFirstName"
                                CssClass="storelabel" ConfigKey="CartOrderInfoDeliveryFirstNameLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDeliveryFirstName" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                            <asp:RequiredFieldValidator ID="reqDeliveryFirstName" runat="server" ControlToValidate="txtDeliveryFirstName"
                                ValidationGroup="OrderInfo"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel9" runat="server" ForControl="txtDeliveryLastName" CssClass="storelabel"
                                ConfigKey="CartOrderInfoDeliveryLastNameLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDeliveryLastName" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                            <asp:RequiredFieldValidator ID="reqDeliveryLastName" runat="server" ControlToValidate="txtDeliveryLastName"
                                ValidationGroup="OrderInfo"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblDeliveryCompany" runat="server" ForControl="txtDeliveryCompany"
                                CssClass="storelabel" ConfigKey="CartOrderInfoDeliveryCompanyLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDeliveryCompany" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblDeliveryAddress1" runat="server" ForControl="txtDeliveryAddress1"
                                CssClass="storelabel" ConfigKey="CartOrderInfoDeliveryAddress1Label" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDeliveryAddress1" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                            <asp:RequiredFieldValidator ID="reqDeliveryAddress1" runat="server" ControlToValidate="txtDeliveryAddress1"
                                ValidationGroup="OrderInfo" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblDeliveryAddress2" runat="server" ForControl="txtDeliveryAddress2"
                                CssClass="storelabel" ConfigKey="CartOrderInfoDeliveryAddress2Label" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDeliveryAddress2" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblDeliverySuburb" runat="server" ForControl="txtDeliverySuburb"
                                CssClass="storelabel" ConfigKey="CartOrderInfoDeliverySuburbLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDeliverySuburb" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblDeliveryCity" runat="server" ForControl="txtDeliveryCity" CssClass="storelabel"
                                ConfigKey="CartOrderInfoDeliveryCityLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDeliveryCity" runat="server" MaxLength="255" CssClass="forminput widetextbox" />
                            <asp:RequiredFieldValidator ID="reqDeliveryCity" runat="server" ControlToValidate="txtDeliveryCity"
                                ValidationGroup="OrderInfo" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblDeliveryCountry" runat="server" ForControl="ddDeliveryCountry"
                                AutoPostBack="true" CssClass="storelabel" ConfigKey="CartOrderInfoDeliveryCountryLabel"
                                ResourceFile="WebStoreResources" />
                            <asp:DropDownList ID="ddDeliveryCountry" runat="server" DataValueField="ISOCode2"
                                DataTextField="Name" CssClass="forminput countrylistdd">
                            </asp:DropDownList>
                        </div>
                        <div class="settingrow" id="divShippingState" runat="server">
                            <mp:SiteLabel ID="lblDeliveryState" runat="server" ForControl="ddDeliveryGeoZone"
                                CssClass="storelabel" ConfigKey="CartOrderInfoDeliveryStateLabel" ResourceFile="WebStoreResources" />
                            <asp:DropDownList ID="ddDeliveryGeoZone" runat="server" DataValueField="Code" DataTextField="Name" CssClass="forminput statelistdd">
                            </asp:DropDownList>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblDeliveryPostalCode" runat="server" ForControl="txtDeliveryPostalCode"
                                CssClass="storelabel" ConfigKey="CartOrderInfoDeliveryPostalCodeLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDeliveryPostalCode" runat="server" MaxLength="20" CssClass="forminput normaltextbox" />
                            <asp:RequiredFieldValidator ID="reqDeliveryPostalCode" runat="server" ControlToValidate="txtDeliveryPostalCode"
                                ValidationGroup="OrderInfo" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                    </fieldset>
                </asp:Panel>
                <div class="settingrow">
                    <asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="OrderInfo" />
                    <portal:mojoLabel ID="lblMessage" runat="server" CssClass="txterror info"></portal:mojoLabel>
                </div>
                <div class="settingrow">
                    <div class="floatpanel" style="padding-top: 15px; margin-left:0px;">
                        <portal:mojoButton ID="btnSaveAndValidate" Text="Update" runat="server" ValidationGroup="OrderInfo" />
                        <portal:mojoButton ID="btnContinue" Text="Continue" runat="server" Enabled="false" />&nbsp;&nbsp;
                    </div>
                    <portal:PaymentAcceptanceMark ID="pam1" runat="server" />
                    
                </div>
                <div class="settingrow">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Literal
                        ID="litOr" runat="server" />
                </div>
            </asp:Panel>
            <div class="settingrow">
                &nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="btnPayPal" runat="server" ImageUrl="https://www.paypal.com/en_US/i/btn/btn_xpressCheckout.gif"
                    AlternateText="Checkout with PayPal" />
                    <portal:mojoGCheckoutButton ID="btnGoogleCheckout" runat="server" /> 
                <asp:Literal ID="litPayPalFormVariables" runat="server" />
            </div>
            <div class="settingrow">
            <portal:mojoLabel ID="lblGoogleMessage" runat="server" CssClass="txterror" Visible="false" />
            </div>
            <portal:CommerceTestModeWarning ID="commerceWarning" runat="server" />
        </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
