<%@ Control Language="c#" AutoEventWireup="False" CodeBehind="BreadcrumbsControl.ascx.cs" Inherits="mojoPortal.Web.UI.BreadcrumbsControl" %>

<portal:BreadcrumbContainer ID="pnlWrapper" runat="server" EnableViewState="false">
	<portal:SiteMapPath ID="breadCrumbsControl" runat="server" Visible="false" EnableViewState="false">
		<RootNodeTemplate>
			<asp:Literal ID="lit1" runat="server" EnableViewState="false" Text='<%# ItemWrapperTop %>' />
			<asp:HyperLink ID="lnkRoot" runat="server" NavigateUrl='<%# siteRoot %>' Text='<%# rootLinkText %>' CssClass='<%# CssClass %>' EnableViewState="false" />
			<asp:Literal ID="lit2" runat="server" EnableViewState="false" Text='<%# ItemWrapperBottom %>' />
		</RootNodeTemplate>

		<NodeTemplate>
			<asp:Literal ID="lit3" runat="server" EnableViewState="false" Text='<%# ItemWrapperTop %>' />
			<asp:HyperLink ID="lnkNode" runat="server" NavigateUrl='<%#  Page.ResolveUrl(Eval("Url").ToString()) %>' Text='<%# Eval("Title") %>' CssClass='<%# CssClass %>' EnableViewState="false" />
			<asp:Literal ID="lit4" runat="server" EnableViewState="false" Text='<%# ItemWrapperBottom %>' />
		</NodeTemplate>

		<CurrentNodeTemplate>
			<asp:Literal ID="lit5" runat="server" EnableViewState="false" Text='<%# ItemWrapperTop %>' />
			<asp:HyperLink ID="lnkCurrent" runat="server" NavigateUrl='<%#  Page.ResolveUrl(Eval("Url").ToString()) %>' Text='<%# Eval("Title") %>' CssClass='<%# CurrentPageCssClass %>' EnableViewState="false" />
			<asp:Literal ID="lit6" runat="server" EnableViewState="false" Text='<%# ItemWrapperBottom %>' />
		</CurrentNodeTemplate>
	</portal:SiteMapPath>

	<asp:Literal ID="childCrumbs" runat="server" EnableViewState="false" />
</portal:BreadcrumbContainer>
