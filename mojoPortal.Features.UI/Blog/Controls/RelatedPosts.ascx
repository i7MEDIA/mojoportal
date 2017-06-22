<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedPosts.ascx.cs" Inherits="mojoPortal.Web.BlogUI.RelatedPosts" %>
<%@ Register Namespace="mojoPortal.Web.BlogUI" Assembly="mojoPortal.Features.UI" TagPrefix="blog" %>

<blog:BlogDisplaySettings ID="displaySettings" runat="server" />

<div class="bsidelist brelatedposts">
	<asp:Literal ID="litHeadingOpenTag" runat="server" EnableViewState="false" />
	<asp:Literal ID="litHeading" runat="server" EnableViewState="false" />
	<asp:Literal ID="litHeadingCloseTag" runat="server" EnableViewState="false" />

	<asp:Repeater ID="rptRelatedPosts" runat="server" EnableViewState="False">
		<HeaderTemplate>
			<ul class="<%# displaySettings.CategoryListClass %> relatedposts">
		</HeaderTemplate>

		<ItemTemplate>
			<li>
				<asp:HyperLink runat="server"
					SkinID="BlogTitle"
					ID="lnkTitle"
					EnableViewState="false"
					CssClass="blogitemtitle"
					Text='<%# Eval("Heading") %>'
					NavigateUrl='<%# FormatBlogUrl(Eval("ItemUrl").ToString(), Convert.ToInt32(Eval("ItemID")))  %>'
				></asp:HyperLink>
			</li>
		</ItemTemplate>

		<FooterTemplate>
			</ul>
		</FooterTemplate>
	</asp:Repeater>
</div>
