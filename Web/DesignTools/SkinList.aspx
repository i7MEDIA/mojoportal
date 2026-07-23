<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SkinList.aspx.cs" Inherits="mojoPortal.Web.AdminUI.SkinListPage" %>

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
					<asp:Repeater ID="rptSkins" runat="server">
						<HeaderTemplate>
							<ul class="simplelist skinlist">
						</HeaderTemplate>
						<ItemTemplate>
							<li class="simplelist">
								<%# Eval("Name") %>
								<%# BuildDownloadLink(Eval("Name").ToString()) %>
								<asp:HyperLink ID="lnkSkinPreview" runat="server" CssClass="cblink" Text='<%# PreviewText %>' NavigateUrl='<%# SiteRoot + "/?skin=" + Eval("Name")  %>' />
								<asp:HyperLink ID="lnkManage" runat="server" Visible='<%# allowEditing %>' Text='<%# ManageText %>' NavigateUrl='<%# SiteRoot + "/DesignTools/ManageSkin.aspx?s=" + Eval("Name")  %>' />
							</li>
						</ItemTemplate>
						<FooterTemplate>
							</ul>
						</FooterTemplate>
					</asp:Repeater>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
