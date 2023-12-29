<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ManageSkin.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ManageSkinPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkDesignerTools" runat="server" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator3" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkSkinList" runat="server" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator4" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
	</portal:AdminCrumbContainer>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper ">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<div class="settingrow">
						<portal:mojoButton ID="btnCopy" runat="server" />
						<asp:TextBox ID="txtCopyAs" runat="server" CssClass="normaltextbox forminput" />
						<portal:mojoHelpLink ID="MojoHelpLink2" runat="server" HelpKey="skinmanager-copyas-help" />
					</div>
					<asp:Literal ID="litCssFiles" runat="server" />
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
