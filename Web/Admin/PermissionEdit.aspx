<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="PermissionEdit.aspx.cs" Inherits="mojoPortal.Web.AdminUI.PermissionEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" />
		<portal:AdminCrumbSeparator ID="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkSiteList" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" />
		<portal:AdminCrumbSeparator ID="litSiteListLinkSeparator" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkPermissionsMenu" runat="server" CssClass="selectedcrumb" />
	</portal:AdminCrumbContainer>

	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper ">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<div class="settingrow">
						<p>
							<asp:CheckBoxList ID="chkAllowedRoles" runat="server" CssClass="forminput" SkinID="Roles" />
							<portal:mojoHelpLink ID="helpLink" runat="server" HelpKey="" />
						</p>
					</div>

					<div class="settingrow">
						<portal:mojoButton ID="btnSave" runat="server" />
					</div>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
			<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared" />
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

