<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AdminOrderCheckout.aspx.cs" Inherits="WebStore.UI.AdminOrderCheckoutPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper orderentry admincheckout ">
        <portal:HeadingControl ID="heading" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
            <asp:Panel ID="pnlOrderDetail" runat="server" CssClass="clear orderdetail">
                <asp:Panel ID="pnlCustomer" runat="server" CssClass="floatpanel" DefaultButton="btnSave">
                    <fieldset>
                        <legend>
                            <mp:SiteLabel ID="SiteLabel5" runat="server" ConfigKey="ConfirmOrderCustomerHeader" ResourceFile="WebStoreResources" />
                        </legend>
                        <div id="divUserLink" runat="server" visible="false" class="settingrow">
                            <mp:SiteLabel ID="SiteLabel8" runat="server" CssClass="settinglabel" ConfigKey="SiteUserLabel" ResourceFile="WebStoreResources" />
                            <asp:HyperLink ID="lnkUser" runat="server" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerEmail" runat="server" ForControl="txtCustomerEmail" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerEmailLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerEmail" Columns="40" runat="server" MaxLength="96" />
                            <asp:RequiredFieldValidator ID="reqCustomerEmail" runat="server" ControlToValidate="txtCustomerEmail"
                                ValidationGroup="OrderInfo"></asp:RequiredFieldValidator>
                            <portal:EmailValidator ID="regexCustomerEmail" runat="server" ControlToValidate="txtCustomerEmail"
                                ValidationGroup="OrderInfo"></portal:EmailValidator>
                            <asp:HyperLink ID="lnkUserSearch" runat="server" CssClass="cblink" Visible="false" />
                            <portal:mojoButton ID="btnClearUser" runat="server" />
                            <asp:HiddenField ID="hdnUserGuid" runat="server" />
                            <asp:HiddenField ID="hdnUserID" runat="server" />
                            <asp:ImageButton ID="btnSetUserFromGreyBox" runat="server" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerName" runat="server" ForControl="txtCustomerFirstName"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerFirstNameLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerFirstName" Columns="40" runat="server" MaxLength="255" />
                            <asp:RequiredFieldValidator ID="reqCustomerFirstName" runat="server" ControlToValidate="txtCustomerFirstName"
                                ValidationGroup="OrderInfo"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel4" runat="server" ForControl="txtCustomerLastName" CssClass="settinglabel"
                                ConfigKey="CartOrderInfoCustomerLastNameLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerLastName" Columns="40" runat="server" MaxLength="255" />
                            <asp:RequiredFieldValidator ID="reqCustomerLastName" runat="server" ControlToValidate="txtCustomerLastName"
                                ValidationGroup="OrderInfo"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerCompany" runat="server" ForControl="txtCustomerCompany"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerCompanyLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerCompany" Columns="40" runat="server" MaxLength="255" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerAddressLine1" runat="server" ForControl="txtCustomerAddressLine1"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerAddressLine1Label" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerAddressLine1" Columns="40" runat="server" MaxLength="255" />
                            <asp:RequiredFieldValidator ID="reqCustomerAddress1" runat="server" ControlToValidate="txtCustomerAddressLine1"
                                ValidationGroup="OrderInfo"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerAddressLine2" runat="server" ForControl="txtCustomerAddressLine2"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerAddressLine2Label" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerAddressLine2" Columns="40" runat="server" MaxLength="255" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerCountry" runat="server" ForControl="ddCustomerCountry"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerCountryLabel" ResourceFile="WebStoreResources" />
                            <asp:DropDownList ID="ddCustomerCountry" runat="server" AutoPostBack="true" DataValueField="ISOCode2"
                                DataTextField="Name">
                            </asp:DropDownList>
                        </div>
                        <div class="settingrow" id="divCustomerState" runat="server">
                            <mp:SiteLabel ID="lblCustomerState" runat="server" ForControl="ddCustomerGeoZone"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerStateLabel" ResourceFile="WebStoreResources" />
                            <asp:DropDownList ID="ddCustomerGeoZone" runat="server" DataValueField="Code" DataTextField="Name">
                            </asp:DropDownList>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerSuburb" runat="server" ForControl="txtCustomerSuburb"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerSuburbLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerSuburb" Columns="40" runat="server" MaxLength="255" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerCity" runat="server" ForControl="txtCustomerCity" CssClass="settinglabel"
                                ConfigKey="CartOrderInfoCustomerCityLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerCity" Columns="40" runat="server" MaxLength="255" />
                            <asp:RequiredFieldValidator ID="reqCustomerCity" runat="server" ControlToValidate="txtCustomerCity"
                                ValidationGroup="OrderInfo"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerPostalCode" runat="server" ForControl="txtCustomerPostalCode"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerPostalCodeLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerPostalCode" Columns="20" runat="server" MaxLength="20" />
                            <asp:RequiredFieldValidator ID="reqCustomerPostalCode" runat="server" ControlToValidate="txtCustomerPostalCode"
                                ValidationGroup="OrderInfo"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerTelephoneDay" runat="server" ForControl="txtCustomerTelephoneDay"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerTelephoneDayLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerTelephoneDay" Columns="20" runat="server" MaxLength="32" />
                            <asp:RequiredFieldValidator ID="reqCustomerDayPhone" runat="server" ControlToValidate="txtCustomerTelephoneDay"
                                ValidationGroup="OrderInfo"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblCustomerTelephoneNight" runat="server" ForControl="txtCustomerTelephoneNight"
                                CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerTelephoneNightLabel"
                                ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtCustomerTelephoneNight" Columns="20" runat="server" MaxLength="32" />
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
                            <portal:mojoButton ID="lnkCopyCustomerToShipping" runat="server" CssClass="buttonlink" CausesValidation="false" />&nbsp;&nbsp;
                            <portal:mojoButton ID="lnkCopyBillingToShipping" runat="server" CssClass="buttonlink" CausesValidation="false" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblDeliveryName" runat="server" ForControl="txtDeliveryFirstName"
                                CssClass="storelabel" ConfigKey="CartOrderInfoDeliveryFirstNameLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDeliveryFirstName" Columns="40" runat="server" MaxLength="255" />
                            <asp:RequiredFieldValidator ID="reqDeliveryFirstName" runat="server" ControlToValidate="txtDeliveryFirstName"
                                ValidationGroup="OrderInfo"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel9" runat="server" ForControl="txtDeliveryLastName" CssClass="storelabel"
                                ConfigKey="CartOrderInfoDeliveryLastNameLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDeliveryLastName" Columns="40" runat="server" MaxLength="255" />
                            <asp:RequiredFieldValidator ID="reqDeliveryLastName" runat="server" ControlToValidate="txtDeliveryLastName"
                                ValidationGroup="OrderInfo"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblDeliveryCompany" runat="server" ForControl="txtDeliveryCompany"
                                CssClass="storelabel" ConfigKey="CartOrderInfoDeliveryCompanyLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDeliveryCompany" Columns="40" runat="server" MaxLength="255" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblDeliveryAddress1" runat="server" ForControl="txtDeliveryAddress1"
                                CssClass="storelabel" ConfigKey="CartOrderInfoDeliveryAddress1Label" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDeliveryAddress1" Columns="40" runat="server" MaxLength="255" />
                            <asp:RequiredFieldValidator ID="reqDeliveryAddress1" runat="server" ControlToValidate="txtDeliveryAddress1"
                                ValidationGroup="OrderInfo"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblDeliveryAddress2" runat="server" ForControl="txtDeliveryAddress2"
                                CssClass="storelabel" ConfigKey="CartOrderInfoDeliveryAddress2Label" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDeliveryAddress2" Columns="40" runat="server" MaxLength="255" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblDeliverySuburb" runat="server" ForControl="txtDeliverySuburb"
                                CssClass="storelabel" ConfigKey="CartOrderInfoDeliverySuburbLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDeliverySuburb" Columns="40" runat="server" MaxLength="255" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblDeliveryCity" runat="server" ForControl="txtDeliveryCity" CssClass="storelabel"
                                ConfigKey="CartOrderInfoDeliveryCityLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDeliveryCity" Columns="40" runat="server" MaxLength="255" />
                            <asp:RequiredFieldValidator ID="reqDeliveryCity" runat="server" ControlToValidate="txtDeliveryCity"
                                ValidationGroup="OrderInfo"></asp:RequiredFieldValidator>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblDeliveryCountry" runat="server" ForControl="ddDeliveryCountry"
                                AutoPostBack="true" CssClass="storelabel" ConfigKey="CartOrderInfoDeliveryCountryLabel"
                                ResourceFile="WebStoreResources" />
                            <asp:DropDownList ID="ddDeliveryCountry" runat="server" DataValueField="ISOCode2"
                                DataTextField="Name">
                            </asp:DropDownList>
                        </div>
                        <div class="settingrow" id="divShippingState" runat="server">
                            <mp:SiteLabel ID="lblDeliveryState" runat="server" ForControl="ddDeliveryGeoZone"
                                CssClass="storelabel" ConfigKey="CartOrderInfoDeliveryStateLabel" ResourceFile="WebStoreResources" />
                            <asp:DropDownList ID="ddDeliveryGeoZone" runat="server" DataValueField="Code" DataTextField="Name">
                            </asp:DropDownList>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblDeliveryPostalCode" runat="server" ForControl="txtDeliveryPostalCode"
                                CssClass="storelabel" ConfigKey="CartOrderInfoDeliveryPostalCodeLabel" ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDeliveryPostalCode" Columns="20" runat="server" MaxLength="20" />
                            <asp:RequiredFieldValidator ID="reqDeliveryPostalCode" runat="server" ControlToValidate="txtDeliveryPostalCode"
                                ValidationGroup="OrderInfo"></asp:RequiredFieldValidator>
                        </div>
                    </fieldset>
                </asp:Panel>
                <div class="settingrow">
                    <asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="OrderInfo" />
                    <portal:mojoLabel ID="lblMessage" runat="server" CssClass="txterror" />
                </div>
                <div class="settingrow">
                    <portal:mojoButton ID="btnSave" Text="Update" runat="server" ValidationGroup="OrderInfo" />
                </div>
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
                            <td>
                                <%# Eval("Name")%>
                            </td>
                            <td>
                                <%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("OfferPrice")))%>
                            </td>
                            <td>
                                <%# Eval("Quantity") %>
                            </td>
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
                    <mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartSubTotalLabel" ResourceFile="WebStoreResources" />
                    <asp:Literal ID="litSubTotal" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlDiscount" runat="server" CssClass="settingrowtight storerow">
                    <mp:SiteLabel ID="SiteLabel11" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartDiscountTotalLabel" ResourceFile="WebStoreResources" />
                    <asp:Literal ID="litDiscount" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlShippingTotal" runat="server" CssClass="settingrowtight storerow">
                    <mp:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartShippingTotalLabel" ResourceFile="WebStoreResources" />
                    <asp:Literal ID="litShippingTotal" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlTaxTotal" runat="server" CssClass="settingrowtight storerow">
                        <mp:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabeltight storelabel"  ConfigKey="CartTaxTotalLabel" ResourceFile="WebStoreResources" />
                    <asp:Literal ID="litTaxTotal" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlOrderTotal" runat="server" CssClass="settingrowtight storerow">
                        <mp:SiteLabel ID="SiteLabel7" runat="server" CssClass="settinglabeltight storelabel" ConfigKey="CartOrderTotalLabel" ResourceFile="WebStoreResources" />   
                    <asp:Literal ID="litOrderTotal" runat="server" />
                </asp:Panel>
            </div>
            <asp:Panel ID="pnlCheckout" runat="server" Visible="false">
                <fieldset id="frmCardInput" runat="server" class="clear">
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblCardTypeGuid" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCardTypeLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:DropDownList ID="ddCardType" runat="server">
                            <asp:ListItem Value="Visa" Selected="true">Visa</asp:ListItem>
                            <asp:ListItem Value="MasterCard">MasterCard</asp:ListItem>
                            <asp:ListItem Value="AMEX">AMEX</asp:ListItem>
                            <asp:ListItem Value="Discover">Discover</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblCardOwner" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCardOwnerFirstNameLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtCardOwnerFirstName" Columns="20" runat="server" MaxLength="100" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel10" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCardOwnerLastNameLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtCardOwnerLastName" Columns="20" runat="server" MaxLength="100" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblCardNumber" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCardNumberLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtCardNumber" Columns="20" runat="server" MaxLength="255" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblCardExpires" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCardExpiresLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:DropDownList ID="ddExpireMonth" runat="server">
                            <asp:ListItem Value="01" Text="01"></asp:ListItem>
                            <asp:ListItem Value="02" Text="02"></asp:ListItem>
                            <asp:ListItem Value="03" Text="03"></asp:ListItem>
                            <asp:ListItem Value="04" Text="04"></asp:ListItem>
                            <asp:ListItem Value="05" Text="05"></asp:ListItem>
                            <asp:ListItem Value="06" Text="06"></asp:ListItem>
                            <asp:ListItem Value="07" Text="07"></asp:ListItem>
                            <asp:ListItem Value="08" Text="08"></asp:ListItem>
                            <asp:ListItem Value="09" Text="09"></asp:ListItem>
                            <asp:ListItem Value="10" Text="10"></asp:ListItem>
                            <asp:ListItem Value="11" Text="11"></asp:ListItem>
                            <asp:ListItem Value="12" Text="12"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddExpireYear" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblCardSecurityCode" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCardSecurityCodeLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtCardSecurityCode" Columns="10" runat="server" MaxLength="50" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel13" runat="server" ForControl="chkSendConfirmationEmail"
                            CssClass="settinglabel" ConfigKey="SendConfirmationEmailLabel" ResourceFile="WebStoreResources" />
                        <asp:CheckBox ID="chkSendConfirmationEmail" runat="server" Checked="true" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel12" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                        <portal:mojoButton ID="btnMakePayment" runat="server" />&nbsp;&nbsp;
                    </div>
                </fieldset>
                <fieldset>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel15" runat="server" ForControl='txtAuthCode" ' CssClass="settinglabel"
                            ConfigKey="OrderEntryAuthCodeLabel" ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtAuthCode" runat="server" CssClass="mediumtextbox" MaxLength="50" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel16" runat="server" ForControl='txtTransactionId" ' CssClass="settinglabel"
                            ConfigKey="OrderEntryTransactionIdLabel" ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtTransactionId" runat="server" CssClass="mediumtextbox" MaxLength="50" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel17" runat="server" ForControl="chkOrderEntrySendConfirmationEamil"
                            CssClass="settinglabel" ConfigKey="SendConfirmationEmailLabel" ResourceFile="WebStoreResources" />
                        <asp:CheckBox ID="chkOrderEntrySendConfirmationEamil" runat="server" Checked="true" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel14" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                        <portal:mojoButton ID="btnCreateOrder" runat="server" />&nbsp;&nbsp;
                    </div>
                </fieldset>
            </asp:Panel>
        </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
