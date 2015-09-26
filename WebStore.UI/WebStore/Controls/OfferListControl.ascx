<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OfferListControl.ascx.cs" Inherits="WebStore.UI.OfferListControl" %>
<%@ Register Namespace="WebStore.UI" Assembly="WebStore.UI" TagPrefix="webstore" %>
<webstore:WebStoreDisplaySettings id="displaySettings" runat="server" />
<asp:Repeater ID="rptOffers" runat="server" >
        <ItemTemplate>
            <div class="hproduct productcontainer">
            <portal:jPlayerPanel ID="jp1" runat="server" SkinID="WebStore" RenderPlayer="false">
                <h4><a class="fn url productlink" href='<%# FormatOfferUrl(Eval("Guid").ToString(), Eval("Url").ToString()) %>'><%# Eval("Name") %></a></h4>
                <span class="price"><%# string.Format(CurrencyCulture, "{0:c}",Convert.ToDecimal(Eval("Price"))) %></span>
                <a class="addtocartlink" rel="nofollow" href='<%# SiteRoot + "/WebStore/CartAdd.aspx?offer=" + Eval("Guid") + "&pageid=" + PageId.ToString() + "&mid=" + ModuleId.ToString() %>'>
                    <span class="linktext"><%# Resources.WebStoreResources.AddToCartLink%></span></a>
                <div class="description"><%# Eval("Abstract") %></div>
                <div class="productoffers">
                <asp:Repeater id="rptProducts" runat="server">
                <ItemTemplate>
                <div class="offercontainer">
                <span class="offername"><asp:Literal ID="litName" runat="server" Text='<%# Eval("Name") %>' Visible='<%# (Eval("TeaserFile").ToString().Length == 0) %>' /></span>
                <asp:HyperLink ID="lnkPreview" runat="server" Visible='<%# (Eval("TeaserFile").ToString().Length > 0) %>' Text='<%# Eval("Name") %>' NavigateUrl='<%# teaserFileBaseUrl + Eval("TeaserFile") %>' />
                <portal:mojoRating runat="server" ID="Rating" ContentGuid='<%# Eval("Guid") %>' Visible='<%# (EnableRatings && Convert.ToBoolean(Eval("EnableRating"))) %>' AllowFeedback='<%# EnableRatingComments %>' ShowPrompt="true" PromptText='<%# Resources.WebStoreResources.RatingPrompt %>'  />
                <a class="productdetaillink" href='<%# FormatProductUrl(Eval("Guid").ToString(), Eval("Url").ToString()) %>'><span class="linktext"><%# Resources.WebStoreResources.ProductDetailsLink%></span></a>
                </div>
                </ItemTemplate>
                </asp:Repeater>
                </div>
                </portal:jPlayerPanel>
            </div>
        </ItemTemplate>
		<alternatingItemTemplate>
            <div class="hproduct productcontainer altproductcontainer">
                <portal:jPlayerPanel ID="jp1" runat="server" SkinID="WebStore" RenderPlayer="false">
                <h4><a class="fn url productlink" href='<%# FormatOfferUrl(Eval("Guid").ToString(), Eval("Url").ToString()) %>'><%# Eval("Name") %></a></h4>
                <span class="price"><%# string.Format(CurrencyCulture, "{0:c}",Convert.ToDecimal(Eval("Price"))) %></span>
                <a class="addtocartlink" rel="nofollow" href='<%# SiteRoot + "/WebStore/CartAdd.aspx?offer=" + Eval("Guid") + "&pageid=" + PageId.ToString() + "&mid=" + ModuleId.ToString() %>'>
                    <span class="linktext"><%# Resources.WebStoreResources.AddToCartLink%></span></a>
                <div class="description"><%# Eval("Abstract") %></div>
                <div class="productoffers">
                <asp:Repeater id="rptProducts" runat="server">
                <ItemTemplate>
                <div class="offercontainer">
                <span class="offername"><asp:Literal ID="litName" runat="server" Text='<%# Eval("Name") %>' Visible='<%# (Eval("TeaserFile").ToString().Length == 0) %>' /></span>
                <asp:HyperLink ID="lnkPreview" runat="server" CssClass="teaserfile" Visible='<%# (Eval("TeaserFile").ToString().Length > 0) %>' Text='<%# Eval("Name") %>' NavigateUrl='<%# teaserFileBaseUrl + Eval("TeaserFile") %>' />
                <portal:mojoRating runat="server" ID="Rating" ContentGuid='<%# Eval("Guid") %>' Visible='<%# (EnableRatings && Convert.ToBoolean(Eval("EnableRating"))) %>' AllowFeedback='<%# EnableRatingComments %>' ShowPrompt="true" PromptText='<%# Resources.WebStoreResources.RatingPrompt %>'  />
                <a class="productdetaillink" href='<%# FormatProductUrl(Eval("Guid").ToString(), Eval("Url").ToString()) %>'><span class="linktext"><%# Resources.WebStoreResources.ProductDetailsLink%></span></a>
                </div>
                </ItemTemplate>
                </asp:Repeater>
                </div>
                </portal:jPlayerPanel>
            </div>		
		</alternatingItemTemplate>
    </asp:Repeater>
    <portal:mojoCutePager ID="pgr" runat="server" />
