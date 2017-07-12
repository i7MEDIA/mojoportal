<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArchiveListControl.ascx.cs" Inherits="mojoPortal.Web.BlogUI.BlogArchiveList" %>
<%@ Register Namespace="mojoPortal.Web.BlogUI" Assembly="mojoPortal.Features.UI" TagPrefix="blog" %>

<blog:BlogDisplaySettings ID="displaySettings" runat="server" />

<asp:Literal ID="litHeadingOpenTag" runat="server" EnableViewState="false" />
<asp:Literal ID="litHeading" runat="server" EnableViewState="false" />
<asp:Literal ID="litHeadingCloseTag" runat="server" EnableViewState="false" />

<asp:Repeater ID="dlArchive" runat="server" EnableViewState="False" SkinID="plain">
	<HeaderTemplate>
		<ul class="<%# displaySettings.CategoryListClass %>">
	</HeaderTemplate>

	<ItemTemplate>
		<li>
			<asp:HyperLink runat="server"
				ID="Hyperlink6"
				EnableViewState="false"
				Text='<%# DateTimeHelper.FormatArchiveLinkText(Convert.ToInt32(Eval("Month")),Convert.ToInt32(Eval("Year")),Convert.ToInt32(Eval("Count"))) %>'
				NavigateUrl='<%# this.SiteRoot +
					"/Blog/ViewArchive.aspx?month=" +
					Eval("Month") + "&amp;year=" +
					Eval("Year").ToString() +
					"&amp;pageid=" +
					PageId.ToString() +
					"&amp;mid=" +
					ModuleId.ToString()
				%>'
			></asp:HyperLink>
		</li>
	</ItemTemplate>

	<FooterTemplate>
		</ul>
	</FooterTemplate>
</asp:Repeater>
