<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryListControl.ascx.cs" Inherits="mojoPortal.Web.BlogUI.BlogCategories" %>
<%@ Register Namespace="mojoPortal.Web.BlogUI" Assembly="mojoPortal.Features.UI" TagPrefix="blog" %>

<blog:BlogDisplaySettings ID="displaySettings" runat="server" />

<asp:Literal ID="litHeadingOpenTag" runat="server" EnableViewState="false" />
	<asp:Literal ID="litHeading" runat="server" EnableViewState="false" />
<asp:Literal ID="litHeadingCloseTag" runat="server" EnableViewState="false" />

<asp:Repeater ID="dlCategories" runat="server" EnableViewState="False" SkinID="plain">
	<HeaderTemplate>
		<ul class="<%# displaySettings.CategoryListClass %>">
	</HeaderTemplate>

	<ItemTemplate>
		<li>
			<asp:HyperLink runat="server"
				ID="Hyperlink5"
				EnableViewState="false"
				Text='<%# ResourceHelper.FormatCategoryLinkText(DataBinder.Eval(Container.DataItem,"Category").ToString(),Convert.ToInt32(DataBinder.Eval(Container.DataItem,"PostCount"))) %>'
				NavigateUrl='<%# this.SiteRoot +
					"/Blog/ViewCategory.aspx?cat=" +
					DataBinder.Eval(Container.DataItem,"CategoryID") +
					"&amp;mid=" +
					ModuleId.ToString() +
					"&amp;pageid=" +
					PageId.ToString()
				%>'
			></asp:HyperLink>
		</li>
	</ItemTemplate>

	<FooterTemplate>
		</ul>
	</FooterTemplate>
</asp:Repeater>

<portal:TagCloudControl ID="cloud" runat="server" />