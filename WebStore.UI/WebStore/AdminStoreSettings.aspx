<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AdminStoreSettings.aspx.cs" Inherits="WebStore.UI.AdminStoreSettingsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper webstore webstoresettings">
        <portal:HeadingControl ID="heading" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
            <div id="divtabs" class="mojo-tabs">
            <ul>
                <li class="selected"><a href="#tab1"><em><asp:Literal ID="litSettingsTab" runat="server" /></em></a></li>
                <li><a href="#tab2"><em><asp:Literal ID="litDescriptionTab" runat="server" /></em></a></li>
                <li><a href="#tab3"><em><asp:Literal ID="litClosedTab" runat="server" /></em></a></li>
            </ul>
            
                <div id="tab1">
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblName" runat="server" CssClass="settinglabel" ConfigKey="StoreNameLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtName" runat="server" MaxLength="255" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblOwnerName" runat="server" CssClass="settinglabel" ConfigKey="StoreOwnerNameLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtOwnerName" runat="server" MaxLength="255" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblOwnerEmail" runat="server" CssClass="settinglabel" ConfigKey="StoreOwnerEmailLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtOwnerEmail" runat="server" MaxLength="100" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblCountryGuid" runat="server" CssClass="settinglabel" ConfigKey="StoreCountryLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:DropDownList ID="ddCountryGuid" runat="server" EnableTheming="false" CssClass="forminput" DataTextField="Name" DataValueField="Guid"
                            AutoPostBack="true" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblAddress" runat="server" CssClass="settinglabel" ConfigKey="StoreAddressLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtAddress" runat="server" MaxLength="255" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblCity" runat="server" CssClass="settinglabel" ConfigKey="StoreCityLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtCity"  runat="server" MaxLength="255" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblZoneGuid" runat="server" CssClass="settinglabel" ConfigKey="StoreZoneLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:DropDownList ID="ddZoneGuid" runat="server" EnableTheming="false" CssClass="forminput" DataTextField="Name" DataValueField="Guid" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblPostalCode" runat="server" CssClass="settinglabel" ConfigKey="StorePostalCodeLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtPostalCode"  runat="server" MaxLength="50" CssClass="mediumtextbox  forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblPhone" runat="server" CssClass="settinglabel" ConfigKey="StorePhoneLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="32" CssClass="mediumtextbox  forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblFax" runat="server" CssClass="settinglabel" ConfigKey="StoreFaxLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtFax" runat="server" MaxLength="32" CssClass="mediumtextbox  forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblSalesEmail" runat="server" CssClass="settinglabel" ConfigKey="StoreSalesEmailLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtSalesEmail" runat="server" MaxLength="100" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblSupportEmail" runat="server" CssClass="settinglabel" ConfigKey="StoreSupportEmailLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtSupportEmail" runat="server" MaxLength="100" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblEmailFrom" runat="server" CssClass="settinglabel" ConfigKey="StoreEmailFromLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtEmailFrom" runat="server" MaxLength="100" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblOrderBCCEmail" runat="server" CssClass="settinglabel" ConfigKey="StoreOrderBCCEmailLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:TextBox ID="txtOrderBCCEmail"  runat="server" MaxLength="100" CssClass="verywidetextbox forminput" />
                    </div>
                    <div class="settingrow">&nbsp;</div>
                </div>
                <div id="tab2">
                    <div class="settingrow">
                        <mpe:EditorControl ID="edDescription" runat="server">
                        </mpe:EditorControl>
                    </div>
                </div>
                <div id="tab3">
                     <div class="settingrow">
                     <mp:SiteLabel ID="lblClosedMessage" runat="server" CssClass="settinglabel" ConfigKey="StoreClosedMessageLabel"
                            ResourceFile="WebStoreResources" />
                     </div>
                    <div class="settingrow">
                        <mpe:EditorControl ID="edClosedMessage" runat="server">
                        </mpe:EditorControl>
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblIsClosed" runat="server" CssClass="settinglabel" ConfigKey="StoreIsClosedLabel"
                            ResourceFile="WebStoreResources" />
                        <asp:CheckBox ID="chkIsClosed" runat="server" CssClass="forminput" />
                    </div>
                    <div class="settingrow">
                         <mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                    </div>
                </div>
            
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="lblspacer1" runat="server" ConfigKey="spacer" />
                <portal:mojoButton ID="btnSave" runat="server" Text="Update" />&nbsp;
            </div>
        </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
