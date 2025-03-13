<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="BannedIPAddresses.aspx.cs" Inherits="mojoPortal.Web.AdminUI.BannedIPAddressesPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkAdvancedTools" runat="server" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkBannedIPs" runat="server" />
	</portal:AdminCrumbContainer>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">

		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin bannedips">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<asp:Panel ID="pnlBannedIPAddresses" runat="server" DefaultButton="btnAddNew">
						<asp:Panel ID="pnlLookup" runat="server" DefaultButton="btnIPLookup">
							<asp:TextBox ID="txtIPAddress" runat="server" CssClass="mediumtextbox" MaxLength="50" />
							<portal:mojoButton ID="btnIPLookup" runat="server" />
						</asp:Panel>
						<mp:mojoGridView ID="grdBannedIPAddresses" runat="server"
							AllowPaging="false"
							AllowSorting="false"
							AutoGenerateColumns="false"
							CssClass=""
							DataKeyNames="RowID">
							<Columns>
								<asp:TemplateField>
									<ItemTemplate>
										<asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="buttonlink" Text='<%# Resources.Resource.BannedIPAddressesGridEditButton %>' />&nbsp;&nbsp;&nbsp;
									</ItemTemplate>
									<EditItemTemplate>
										<asp:Button ID="btnGridUpdate" runat="server" Text='<%# Resources.Resource.BannedIPAddressesGridUpdateButton %>' CommandName="Update" SkinID="UpdateButton" />
										<asp:Button ID="btnGridDelete" runat="server" Text='<%# Resources.Resource.BannedIPAddressesGridDeleteButton %>' CommandName="Delete" SkinID="DeleteButton" />
										<asp:Button ID="btnGridCancel" runat="server" Text='<%# Resources.Resource.BannedIPAddressesGridCancelButton %>' CommandName="Cancel" SkinID="CancelButton" />
									</EditItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField>
									<ItemTemplate>
										<%# Eval("BannedIP") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox ID="txtBannedIP" Columns="20" Text='<%# Eval("BannedIP") %>' runat="server" MaxLength="50" />
										<asp:RequiredFieldValidator runat="server" ControlToValidate="txtBannedIP" ErrorMessage="*" />
										<asp:RegularExpressionValidator runat="server" ID="valIP" ControlToValidate="txtBannedIP" ValidationExpression="^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$" ErrorMessage="Invalid IP" />
									</EditItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField>
									<ItemTemplate>
										<%# Eval("BannedReason") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox ID="txtBannedReason" Columns="20" Text='<%# Eval("BannedReason") %>' runat="server" MaxLength="255" />
									</EditItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField>
									<ItemTemplate>
										<%# DateTimeHelper.Format(Convert.ToDateTime(Eval("BannedUTC")), timeZone, "g", timeOffset)%>
									</ItemTemplate>
									<EditItemTemplate></EditItemTemplate>
								</asp:TemplateField>
							</Columns>
							<EmptyDataTemplate>
								<p class="nodata">
									<asp:Literal ID="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" /></p>
							</EmptyDataTemplate>
						</mp:mojoGridView>
						<div class="settingrow">
							<portal:mojoButton ID="btnAddNew" runat="server" />
						</div>
						<portal:mojoCutePager ID="pgrBannedIPAddresses" runat="server" />
					</asp:Panel>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>

		</portal:InnerWrapperPanel>

	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane"
	runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
