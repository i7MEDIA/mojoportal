<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SendProgress.aspx.cs" Inherits="mojoPortal.Web.ELetterUI.SendProgressPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator ID="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkLetterAdmin" runat="server" CssClass="unselectedcrumb" />
	</portal:AdminCrumbContainer>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin eletter">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<div class="settingrow">
						<mp:SiteLabel ID="lbl1" runat="server" CssClass="settinglabel" ConfigKey="NewsletterLabel" ResourceFile="Resource" />
						<asp:Literal ID="litLetterInfoTitle" runat="server" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="NewsletterSubjectEditionLabel" ResourceFile="Resource" />
						<asp:Literal ID="litSubject" runat="server" />
					</div>
					<div class="settingrow">
						<asp:Panel ID="pnlProgress" runat="server"></asp:Panel>
						<asp:Panel ID="pnlStatus" runat="server"></asp:Panel>
					</div>
					<asp:HyperLink ID="lnkSendLog" runat="server" />
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />