<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Confirm.aspx.cs" Inherits="mojoPortal.Web.ELetterUI.ConfirmPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
	</portal:AdminCrumbContainer>
	<asp:Panel ID="pnl1" runat="server" CssClass="panelwrapper ">
		<div class="modulecontent">
			<asp:Panel ID="pnlConfirmed" runat="server" Visible="false">
				<mp:SiteLabel ID="SiteLabel1" runat="server" ConfigKey="NewslettersSubscriptionConfirmed" />
				<br />
				<asp:Literal ID="litConfirmDetails" runat="server" />
			</asp:Panel>
			<asp:Panel ID="pnlNotFound" runat="server" Visible="true">
				<mp:SiteLabel ID="lblWarning" runat="server" CssClass="txterror warning" ConfigKey="NewslettersSubscriptionNotFound" />
			</asp:Panel>
		</div>
	</asp:Panel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
