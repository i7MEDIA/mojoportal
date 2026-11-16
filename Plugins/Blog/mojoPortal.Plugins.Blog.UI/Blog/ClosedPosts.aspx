<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ClosedPosts.aspx.cs" Inherits="mojoPortal.Web.BlogUI.ClosedPostsPage" %>

<%@ Register Namespace="mojoPortal.Web.BlogUI" Assembly="mojoPortal.Features.UI" TagPrefix="blog" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<blog:BlogDisplaySettings ID="displaySettings" runat="server" />
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper ">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<asp:Repeater ID="rpt" runat="server" EnableViewState="False">
						<HeaderTemplate>
							<ul class="simplelist closedposts">
						</HeaderTemplate>
						<ItemTemplate>
							<li>
								<asp:HyperLink ID="HyperLink1" runat="server" Visible='<%# CanEditPost(Convert.ToInt32(Eval("UserID"))) %>'
									Text='<%# EditLinkText %>'
									NavigateUrl='<%# System.FormattableString.Invariant($"{SiteRoot}/Blog/EditPost.aspx?pageid={pageId}&ItemID={Eval("ItemID")}&mid={moduleId}") %>'>
								</asp:HyperLink>
								<asp:HyperLink ID="Title" runat="server" SkinID="plain"
									Text='<%# Eval("Heading").ToString() %>'
									NavigateUrl='<%# FormatBlogUrl(Eval("ItemUrl").ToString(), Convert.ToInt32(Eval("ItemID")))  %>'>
								</asp:HyperLink>
								<span class="blogdate">
									<span class="blogauthor">
										<%# FormatPostAuthor(Eval("Name").ToString(),Eval("FirstName").ToString(),Eval("LastName").ToString())%></span>
									<span class="bdate">
										<%# FormatBlogDate(Convert.ToDateTime(Eval("StartDate")),Convert.ToDateTime(Eval("EndDate"))) %></span>
								</span>
							</li>
						</ItemTemplate>
						<FooterTemplate></ul></FooterTemplate>
					</asp:Repeater>
					<div class="blogpager">
						<portal:mojoCutePager ID="pgr" runat="server" />
					</div>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
