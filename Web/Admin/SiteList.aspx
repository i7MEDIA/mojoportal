<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
	CodeBehind="SiteList.aspx.cs" Inherits="mojoPortal.Web.AdminUI.SiteListPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx"
			CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator ID="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkSiteList" runat="server" CssClass="selectedcrumb" />
	</portal:AdminCrumbContainer>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">

		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper sitelistpage ">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<asp:Repeater ID="rptSites" runat="server">
						<HeaderTemplate>
							<ul class="simplelist sitelist">
								<li class="simplelist">
									<%# CurrentSiteName %>
									<a class="siteitem currentsite" href='<%# $"{WebConfigSettings.AdminDirectoryLocation}/SiteSettings.aspx".ToLinkBuilder() %>'><%# Resources.Resource.AdminMenuSiteSettingsLink %></a>
									<a class="siteitem currentsite" href='<%# $"{WebConfigSettings.AdminDirectoryLocation}/PermissionsMenu.aspx".ToLinkBuilder() %>'><%# Resources.Resource.SiteSettingsPermissionsTab %></a>
								</li>
						</HeaderTemplate>
						<ItemTemplate>
							<li class="simplelist">
								<%# Eval("SiteName") %>
								<asp:Label ID="lblSiteID" runat="server" CssClass="siteidlabel" Visible='<%# showSiteIDInSiteList %>' Text='<%# FormatSiteId(Convert.ToInt32(Eval("SiteID"))) %>' />
								<a class="siteitem" href='<%# $"{WebConfigSettings.AdminDirectoryLocation}/SiteSettings.aspx".ToLinkBuilder().SiteId(Convert.ToInt32(Eval("SiteID"))) %>'><%# Resources.Resource.AdminMenuSiteSettingsLink %></a>
								<a class="siteitem" href='<%# $"{WebConfigSettings.AdminDirectoryLocation}/PermissionsMenu.aspx".ToLinkBuilder().SiteId(Convert.ToInt32(Eval("SiteID"))) %>'><%# Resources.Resource.SiteSettingsPermissionsTab%></a>
							</li>
						</ItemTemplate>
						<FooterTemplate>
							</ul>
						</FooterTemplate>
					</asp:Repeater>
					<portal:mojoCutePager ID="pgr" runat="server" />
					<div class="settingrow">
						<asp:HyperLink ID="lnkNewSite" runat="server" />
					</div>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>

		</portal:InnerWrapperPanel>

	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
