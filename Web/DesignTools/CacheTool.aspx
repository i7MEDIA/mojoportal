<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="CacheTool.aspx.cs" Inherits="mojoPortal.Web.AdminUI.CacheToolPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkDesignerTools" runat="server" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator3" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
	</portal:AdminCrumbContainer>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper ">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<div class="settingrow">
						<portal:mojoButton ID="btnCssCacheToggle" runat="server" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="lblCacheInfo" runat="server" CssClass="cssinfo" ConfigKey="CssCacheInfo" ResourceFile="Resource" UseLabelTag="false" />
					</div>
					<div class="settingrow">
						<portal:mojoButton ID="btnResetSkinVersionGuid" runat="server" />
						<asp:Label ID="lblSkinGuid" runat="server" CssClass="skinguid" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="cssinfo" ConfigKey="SkinVersionGuidInfo" ResourceFile="Resource" UseLabelTag="false" />
					</div>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />