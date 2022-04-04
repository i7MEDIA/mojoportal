<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SecurityAdvisor.aspx.cs" Inherits="mojoPortal.Web.AdminUI.SecurityAdvisorPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portalAdmin:AdminDisplaySettings ID="displaySettings" runat="server" />

	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx"
			CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator ID="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
	</portal:AdminCrumbContainer>

	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper securityadvisor">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<p class="sadvisorintro">
						<asp:Literal ID="litInfo" runat="server" />
					</p>

					<portal:FormGroupPanel ID="fgpDefaultAdminAccount" runat="server" SkinID="SecurityDefaultAdminAccount">
						<asp:Literal ID="litDefaultAdminAccountHeading" runat="server" EnableViewState="false" />
						<asp:Literal ID="litDefaultAdminAccountResults" runat="server" EnableViewState="false" />
					</portal:FormGroupPanel>

					<portal:FormGroupPanel ID="fgpMachineKey" runat="server" SkinID="SecurityMachineKey">
						<asp:Literal ID="litMachineKeyHeading" runat="server" EnableViewState="false" />
						<asp:Literal ID="litMachineKeyResults" runat="server" EnableViewState="false" />
					</portal:FormGroupPanel>

					<portal:FormGroupPanel ID="fgpFileSystem" runat="server" SkinID="SecurityFileSystem">
						<asp:Literal ID="litFileSystemHeading" runat="server" EnableViewState="false" />
						<asp:Literal ID="litFileSystemResults" runat="server" EnableViewState="false" />
					</portal:FormGroupPanel>

					<portal:FormGroupPanel ID="fgpSecurityProtocol" runat="server" SkinID="SecurityProtocol">
						<asp:Literal ID="litSecurityProtocolHeading" runat="server" EnableViewState="false" />
						<asp:Literal ID="litSecurityProtocolDescription" runat="server" EnableViewState="false" />
					</portal:FormGroupPanel>

					<div class="settingrow">
						<mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="spacer" UseLabelTag="false"></mp:SiteLabel>
					</div>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>

<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
