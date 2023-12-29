<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="CssEditor.aspx.cs" Inherits="mojoPortal.Web.AdminUI.CssEditorPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkDesignerTools" runat="server" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator3" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkSkinList" runat="server" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator4" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkSkin" runat="server" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator5" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
	</portal:AdminCrumbContainer>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper ">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<div class="settingrow">
						<portal:CodeEditor ID="edCss" runat="server" Syntax="css" CssClass="csseditor" StartHighlighted="false" MinWidth="700" AllowToggle="true" Width="100%" />
						<asp:TextBox ID="txtCss" runat="server" Rows="20" Columns="140" CssClass="csseditor" TextMode="MultiLine" Visible="false" />
					</div>
					<div class="settingrow">
						<portal:mojoButton ID="btnSave" runat="server" />
					</div>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

