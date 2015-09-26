<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="OfferDetail.aspx.cs" Inherits="WebStore.UI.OfferDetailPage" %>
<%@ Register Namespace="WebStore.UI" Assembly="WebStore.UI" TagPrefix="webstore" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper webstore webstoreofferdetail">
        <portal:HeadingControl ID="heading" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
        <webstore:WebStoreDisplaySettings id="displaySettings" runat="server" />
            <div class="settingrow">
                <strong>
                    <asp:Label ID="lblPrice" runat="server" /></strong>
                <asp:HyperLink ID="lnkAddToCart" runat="server" />
                <portal:mojoGCheckoutButton ID="btnGoogleCheckout" runat="server" Visible="false" /> 
            </div>
            <div class="settingrow" id="divOfferDescription" runat="server">
                <asp:Literal ID="litOfferDescription" runat="server" />
            </div>
            <asp:Panel ID="pnlProducts" runat="server">
            <portal:jPlayerPanel ID="jPlayerPanel" runat="server" SkinID="WebStore" RenderPlayer="false">
                <asp:Repeater ID="rptProducts" runat="server">
                    <ItemTemplate>
                    
                        <div id="divName" runat="server" Visible='<%# offerHasMoreThanOneProduct %>'>
                            <asp:HyperLink ID="lnkPreview" runat="server" Visible='<%# (Eval("TeaserFile").ToString().Length > 0) %>' Text='<%# Eval("Name") %>' NavigateUrl='<%# teaserFileBaseUrl + Eval("TeaserFile") %>' />
                            <asp:Literal ID="litName" runat="server" Visible='<%# (Eval("TeaserFile").ToString().Length == 0) %>' Text='<%# Eval("Name") %>' />
                        </div>
                        <div>
                             <%# Eval("Description") %>
                        </div>
                   
                    </ItemTemplate>
                </asp:Repeater>
                </portal:jPlayerPanel>
            </asp:Panel>
        </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
