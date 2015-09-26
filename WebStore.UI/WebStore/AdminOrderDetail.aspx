<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AdminOrderDetail.aspx.cs" Inherits="WebStore.UI.AdminOrderDetailPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper webstore webstorecart ">
<portal:HeadingControl id="heading" runat="server" />
  <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
  <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    
            <div class="settingrow">
                <mp:SiteLabel ID="SiteLabel8" runat="server" CssClass="settinglabel" ConfigKey="SiteUserLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblSiteUser" runat="server" />
                <asp:HyperLink ID="lnkUser" runat="server" />
            </div>
            <div id="divMoveOrder" runat="server" visible="false" class="settingrow">
                <asp:TextBox ID="txtNewUser" runat="server" CssClass="widetextbox" Enabled="false" />
                <asp:HyperLink ID="lnkUserSearch" runat="server" CssClass="cblink" />
                <portal:mojoButton ID="btnMoveOrderToUser" runat="server" />
                <asp:HiddenField ID="hdnUserGuid" runat="server" />
                <asp:HiddenField ID="hdnUserID" runat="server" />
                <asp:ImageButton ID="btnSetUserFromGreyBox" runat="server" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="OrderIdLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblOrderId" runat="server" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="SiteLabel4" runat="server" CssClass="settinglabel" ConfigKey="OrderStatusLabel"
                    ResourceFile="WebStoreResources" />
                <portal:OrderStatusSetting ID="orderStatusControl" runat="server" />
                <asp:Button ID="btnSaveStatusChange" runat="server" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="LabelCustomerName" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerNameLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerName" runat="server" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="LabelCustomerCompany" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerCompanyLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerCompany" runat="server" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="LabelCustomerAddressLine1" runat="server" CssClass="settinglabel"
                    ConfigKey="CartOrderInfoCustomerAddressLine1Label" ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerAddressLine1" runat="server" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="LabelCustomerAddressLine2" runat="server" CssClass="settinglabel"
                    ConfigKey="CartOrderInfoCustomerAddressLine2Label" ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerAddressLine2" runat="server" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="LabelCustomerCountry" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerCountryLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerCountry" runat="server" />
            </div>
            <div class="settingrow" id="divCustomerState" runat="server">
                <mp:SiteLabel ID="LabelCustomerState" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerStateLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerGeoZone" runat="server" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="LabellCustomerSuburb" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerSuburbLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerSuburb" runat="server" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="LabelCustomerCity" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerCityLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerCity" runat="server" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="LabelCustomerPostalCode" runat="server" CssClass="settinglabel"
                    ConfigKey="CartOrderInfoCustomerPostalCodeLabel" ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerPostalCode" runat="server" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="LabelCustomerTelephoneDay" runat="server" CssClass="settinglabel"
                    ConfigKey="CartOrderInfoCustomerTelephoneDayLabel" ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerTelephoneDay" runat="server" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="LabelCustomerTelephoneNight" runat="server" CssClass="settinglabel"
                    ConfigKey="CartOrderInfoCustomerTelephoneNightLabel" ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerTelephoneNight" runat="server" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="LabelCustomerEmail" runat="server" CssClass="settinglabel" ConfigKey="CartOrderInfoCustomerEmailLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomerEmail" runat="server" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="SiteLabel10" runat="server" CssClass="settinglabel" ConfigKey="OrderDiscountCodesLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Label ID="lblDiscountCodes" runat="server" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="SiteLabel12" runat="server" CssClass="settinglabel" ConfigKey="OrderCustomData" ResourceFile="WebStoreResources" />
                <asp:Label ID="lblCustomData" runat="server" />
            </div>
            <h3 class="itemsheader"><asp:Literal ID="litItemsHeader" runat="server" /></h3>
            <div class="settingrow">
                <asp:Repeater ID="rptOrderItems" runat="server">
                    <ItemTemplate>
                        <div>
                            <%# Eval("Name") %>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <asp:Panel ID="pnlDownloadTickets" runat="server" CssClass="yui-skin-sam">
            <h3 class="itemsheader"><asp:Literal ID="litDownloadTicketsHeading" runat="server" /></h3>
            <div class="settingrow">
                <mp:mojoGridView ID="grdDownloadTickets" runat="server" 
                    AllowPaging="false" 
                    AllowSorting="false"
                    CssClass="" AutoGenerateColumns="false"
                    DataKeyNames="Guid">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%# Eval("ProductName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%# Eval("DownloadsAllowed")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%# Eval("DownloadedCount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                 <asp:HyperLink ID="lnkDownloadhx" runat="server" CssClass="cblink" NavigateUrl='<%# SiteRoot + "/WebStore/AdminDownloadHistory.aspx?pageid=" + pageId + "&amp;mid=" + moduleId + "&amp;order=" + orderGuid.ToString() + "&amp;t=" + Eval("Guid")%>' Text='<%# Resources.WebStoreResources.DownloadHistoryViewLink %>' Tooltip='<%# Resources.WebStoreResources.DownloadHistoryHeading %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                </mp:mojoGridView>
            </div>
            </asp:Panel>
            <asp:Panel ID="pnlSubTotal" runat="server" CssClass="settingrow">
                <mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="CartSubTotalLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Literal ID="litSubTotal" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlDiscount" runat="server" CssClass="storerow">
                    <mp:SiteLabel ID="SiteLabel11" runat="server" CssClass="settinglabel" ConfigKey="CartDiscountTotalLabel" ResourceFile="WebStoreResources" />
                    <asp:Literal ID="litDiscount" runat="server" />
                </asp:Panel>
            <asp:Panel ID="pnlShippingTotal" runat="server" CssClass="settingrow">
                <mp:SiteLabel ID="SiteLabel5" runat="server" CssClass="settinglabel" ConfigKey="OrderDetailShippingTotalLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Literal ID="litShippingTotal" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlTaxTotal" runat="server" CssClass="settingrow">
                <mp:SiteLabel ID="SiteLabel6" runat="server" CssClass="settinglabel" ConfigKey="OrderDetailTaxTotalLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Literal ID="litTaxTotal" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlOrderTotal" runat="server" CssClass="settingrow">
                <mp:SiteLabel ID="SiteLabel7" runat="server" CssClass="settinglabel" ConfigKey="OrderDetailOrderTotalLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Literal ID="litOrderTotal" runat="server" />
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server" CssClass="settingrow">
                <mp:SiteLabel ID="SiteLabel9" runat="server" CssClass="settinglabel" ConfigKey="OrderDetailPaymentMethodLabel"
                    ResourceFile="WebStoreResources" />
                <asp:Literal ID="litPaymentMethod" runat="server" />
            </asp:Panel>
            <div class="settingrow">
                <mp:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                <portal:mojoButton ID="btnDelete" runat="server" Visible="false" />
            </div>
            <asp:Panel ID="pnlCheckoutLog" runat="server">
            </asp:Panel>
            
        
    </portal:InnerBodyPanel>
    </portal:OuterBodyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
