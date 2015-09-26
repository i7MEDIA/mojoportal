<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ProductListControl.ascx.cs" Inherits="WebStore.UI.ProductListControl" %>
<%@ Register Namespace="WebStore.UI" Assembly="WebStore.UI" TagPrefix="webstore" %>
<webstore:WebStoreDisplaySettings id="displaySettings" runat="server" />
<asp:Repeater ID="rptProducts" runat="server" >
        <ItemTemplate>
            <div class="hproduct hreview productcontainer">
                <h4><a class="fn url productlink" href='<%# FormatProductUrl(Eval("Guid").ToString(), Eval("Url").ToString()) %>'><%# Eval("Name") %></a></h4>
                <portal:mojoRating runat="server" ID="Rating" ContentGuid='<%# Eval("Guid") %>' Visible='<%# (EnableRatings && Convert.ToBoolean(Eval("EnableRating"))) %>' AllowFeedback='<%# EnableRatingComments %>' ShowPrompt="true" PromptText='<%# Resources.WebStoreResources.RatingPrompt %>'  />
                <div class="description"><%# Eval("Abstract") %></div>
                <a class="productdetaillink" href='<%# FormatProductUrl(Eval("Guid").ToString(), Eval("Url").ToString()) %>'><%# Resources.WebStoreResources.ProductDetailsLink%></a>
                <asp:HyperLink ID="lnkPreview" CssClass="previewlink" runat="server" Visible='<%# (Eval("TeaserFile").ToString().Length > 0) %>' Text='<%# Eval("TeaserFileLink") %>' NavigateUrl='<%# teaserFileBaseUrl + Eval("TeaserFile") %>' />
                <div class="productoffers">
                <asp:Repeater id="rptOffers" runat="server">
                <ItemTemplate>
                <div class="offercontainer">
                <span class="productname"><%# Eval("ProductListName") %></span>
                <asp:HyperLink ID="lnkOfferDetail" CssClass="offerdetaillink" runat="server" Visible='<%# Convert.ToBoolean(Eval("ShowDetailLink")) %>' 
                NavigateUrl='<%# FormatOfferUrl(Eval("Guid").ToString(), Eval("Url").ToString()) %>' Text='<%# Resources.WebStoreResources.OfferDetailLink %>' EnableViewState="false" />
                <span class="price"><%# string.Format(CurrencyCulture, "{0:c}",Convert.ToDecimal(Eval("Price"))) %></span>
                <a class="addtocartlink" rel="nofollow" href='<%# SiteRoot + "/WebStore/CartAdd.aspx?offer=" + Eval("Guid") + "&pageid=" + PageId.ToString() + "&mid=" + ModuleId.ToString() %>'>
                    <span class="linktext"><%# Resources.WebStoreResources.AddToCartLink%></span></a>
                </div>
                </ItemTemplate>
                </asp:Repeater>
                </div>
            </div>
        </ItemTemplate>
		<alternatingItemTemplate>
            <div class="hproduct hreview productcontainer altproductcontainer">
                <h4><a class="fn url productlink" href='<%# FormatProductUrl(Eval("Guid").ToString(), Eval("Url").ToString()) %>'><%# Eval("Name") %></a></h4>
                <portal:mojoRating runat="server" ID="Rating" ContentGuid='<%# Eval("Guid") %>' Visible='<%# (EnableRatings && Convert.ToBoolean(Eval("EnableRating"))) %>' AllowFeedback='<%# EnableRatingComments %>' ShowPrompt="true" PromptText='<%# Resources.WebStoreResources.RatingPrompt %>'  />
                <div class="description"><%# Eval("Abstract") %></div>
                <a class="productdetaillink" href='<%# FormatProductUrl(Eval("Guid").ToString(), Eval("Url").ToString()) %>'><%# Resources.WebStoreResources.ProductDetailsLink%></a>
                <asp:HyperLink ID="lnkPreview" CssClass="previewlink" runat="server" Visible='<%# (Eval("TeaserFile").ToString().Length > 0) %>' Text='<%# Eval("TeaserFileLink") %>' NavigateUrl='<%# teaserFileBaseUrl + Eval("TeaserFile") %>' />
                <div class="productoffers">
                <asp:Repeater id="rptOffers" runat="server">
                <ItemTemplate>
                <div class="offercontainer">
                <span class="productname"><%# Eval("ProductListName") %></span>
                <asp:HyperLink ID="lnkOfferDetail" CssClass="offerdetaillink" runat="server" Visible='<%# Convert.ToBoolean(Eval("ShowDetailLink")) %>' 
                NavigateUrl='<%# FormatOfferUrl(Eval("Guid").ToString(), Eval("Url").ToString()) %>' Text='<%# Resources.WebStoreResources.OfferDetailLink %>' EnableViewState="false" />
                <span class="price"><%# string.Format(CurrencyCulture, "{0:c}",Convert.ToDecimal(Eval("Price"))) %></span>
                <a class="addtocartlink" rel="nofollow" href='<%# SiteRoot + "/WebStore/CartAdd.aspx?offer=" + Eval("Guid") + "&pageid=" + PageId.ToString() + "&mid=" + ModuleId.ToString() %>'>
                    <span class="linktext"><%# Resources.WebStoreResources.AddToCartLink%></span></a>
                </div>
                </ItemTemplate>
                </asp:Repeater>
                </div>
            </div>
		</alternatingItemTemplate>
    </asp:Repeater>
    <portal:mojoCutePager ID="pgr" runat="server" />
    