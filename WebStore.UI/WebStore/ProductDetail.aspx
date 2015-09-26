<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ProductDetail.aspx.cs" Inherits="WebStore.UI.ProductDetailPage" %>
<%@ Register Src="~/WebStore/Controls/CartLink.ascx" TagPrefix="ws" TagName="CartLink" %>
<%@ Register Namespace="WebStore.UI" Assembly="WebStore.UI" TagPrefix="webstore" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="settingrow">
   <ws:CartLink ID="lnkCart" runat="server" EnableViewState="false" />
</div>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="hproduct hreview  panelwrapper webstore webstoreofferdetail">
        <portal:HeadingControl ID="heading" runat="server" CssClass="fn" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">  
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent productdetail">
            <webstore:WebStoreDisplaySettings id="displaySettings" runat="server" />
            <div class="productratingwrapper">
            <portal:mojoRating runat="server" ID="Rating" ShowPrompt="true" />
            </div>
                <asp:Panel ID="pnlOffers" runat="server" CssClass="clearpanel offerspanel">
                
                    <asp:Repeater ID="rptOffers" runat="server">
                        <HeaderTemplate>
                        <table>
							<tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
								<tr class="offercontainer">
									<td class="productname">
										<%# Eval("ProductListName") %>
										<asp:HyperLink ID="lnkOfferDetail" runat="server" EnableViewState="false" Visible='<%# Convert.ToBoolean(Eval("ShowDetailLink")) %>' NavigateUrl='<%# SiteRoot + Eval("Url") %>' Text='<%# Resources.WebStoreResources.OfferDetailLink %>' />
									</td>
									<td class="price">
									<span class="price"><%# string.Format(currencyCulture, "{0:c}",Convert.ToDecimal(Eval("Price"))) %></span>
									</td>
									<td class="quantity">
									<asp:TextBox ID="txtQuantity" runat="server" Text="1" Columns="3" />
									</td>
									<td class="addtocartbutton">
									<asp:Button ID="btnAddToCart" runat="server" Text='<%# Resources.WebStoreResources.AddToCartLink%>' CommandName="addToCart" CommandArgument='<%# Eval("Guid") %>' CausesValidation="false" CssClass="addtocartbutton jqbutton ui-button ui-widget ui-state-default ui-corner-all" />
									</td>
								</tr>
                        </ItemTemplate>
                        <FooterTemplate>
							</tbody>
                        </table>
                        </FooterTemplate>
                    </asp:Repeater>
                   
                </asp:Panel>
                <div class="description settingrow" id="divOfferDescription" runat="server" EnableViewState="false">
                    <asp:Literal ID="litDescription" runat="server" EnableViewState="false" />
                </div>
                <portal:jPlayerPanel ID="jPlayerPanel" runat="server" SkinID="WebStore" RenderPlayer="false">
                <div class="settingrow preview">
                <asp:HyperLink ID="lnkPreview" CssClass="previewlink" runat="server" Visible='false' />
                </div>
                </portal:jPlayerPanel>
        </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
