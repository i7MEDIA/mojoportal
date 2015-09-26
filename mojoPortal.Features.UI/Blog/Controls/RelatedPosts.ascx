<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedPosts.ascx.cs" Inherits="mojoPortal.Web.BlogUI.RelatedPosts" %>
<div class="bsidelist brelatedosts">
<asp:Literal ID="litHeadingOpenTag" runat="server" EnableViewState="false" />
<asp:Literal ID="litHeading" runat="server" EnableViewState="false" />
<asp:Literal ID="litHeadingCloseTag" runat="server" EnableViewState="false" />
<asp:repeater id="rptRelatedPosts" runat="server" EnableViewState="False">
    <HeaderTemplate><ul class="simplelist blognav relatedposts"></HeaderTemplate>
    <ItemTemplate>
        <li>
        <asp:HyperLink SkinID="BlogTitle" ID="lnkTitle" runat="server" EnableViewState="false" CssClass="blogitemtitle"
                        Text='<%# Eval("Heading") %>' 
                        NavigateUrl='<%# FormatBlogUrl(Eval("ItemUrl").ToString(), Convert.ToInt32(Eval("ItemID")))  %>'>
                    </asp:HyperLink>
        </li>
    </ItemTemplate>
    <FooterTemplate></ul></FooterTemplate>
</asp:repeater>
</div>
