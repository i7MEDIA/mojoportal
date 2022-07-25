<%@ Page Async="true" Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="BannedIPAddresses.aspx.cs" Inherits="mojoPortal.Web.AdminUI.BannedIPAddressesPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:admincrumbcontainer id="pnlAdminCrumbs" runat="server" cssclass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />
		<portal:admincrumbseparator id="AdminCrumbSeparator1" runat="server" text="&nbsp;&gt;" enableviewstate="false" />
		<asp:HyperLink ID="lnkAdvancedTools" runat="server" />
		<portal:admincrumbseparator id="AdminCrumbSeparator2" runat="server" text="&nbsp;&gt;" enableviewstate="false" />
		<asp:HyperLink ID="lnkBannedIPs" runat="server" />
	</portal:admincrumbcontainer>

	<portal:outerwrapperpanel id="pnlOuterWrap" runat="server">
		<portal:innerwrapperpanel id="pnlInnerWrap" runat="server" cssclass="panelwrapper admin bannedips">
			<portal:headingcontrol id="heading" runat="server" />

			<portal:outerbodypanel id="pnlOuterBody" runat="server">
				<portal:innerbodypanel id="pnlInnerBody" runat="server" cssclass="modulecontent">
					<asp:Panel id="pnlBannedIPAddresses" runat="server" DefaultButton="btnAddNew">
						<asp:Panel ID="pnlLookup" runat="server" DefaultButton="btnIPLookup">
							<asp:TextBox ID="txtIPAddress" runat="server" CssClass="mediumtextbox" MaxLength="50" />
							<portal:mojobutton id="btnIPLookup" runat="server" />
						</asp:Panel>

						<mp:mojogridview id="grdBannedIPAddresses" runat="server"
							allowpaging="false"
							allowsorting="false"
							autogeneratecolumns="false"
							cssclass=""
							datakeynames="ID">
							<columns>
								<asp:TemplateField>
									<itemtemplate>
										<asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="buttonlink" Text='<%# Resources.Resource.BannedIPAddressesGridEditButton %>' />
										&nbsp;&nbsp;&nbsp;
									</itemtemplate>

									<edititemtemplate>
										<asp:Button id="btnGridUpdate" runat="server" Text='<%# Resources.Resource.BannedIPAddressesGridUpdateButton %>' CommandName="Update" />
										<asp:Button id="btnGridDelete" runat="server" Text='<%# Resources.Resource.BannedIPAddressesGridDeleteButton %>' CommandName="Delete" />
										<asp:Button id="btnGridCancel" runat="server" Text='<%# Resources.Resource.BannedIPAddressesGridCancelButton %>' CommandName="Cancel" />
									</edititemtemplate>
								</asp:TemplateField>

								<asp:TemplateField>
									<itemtemplate>
										<%# Eval("IPAddress") %>
									</itemtemplate>

									<edititemtemplate>
										<asp:TextBox ID="txtBannedIP" Columns="20" Text='<%# Eval("IPAddress") %>' runat="server" MaxLength="50" />
									</edititemtemplate>
								</asp:TemplateField>

								<asp:TemplateField>
									<itemtemplate>
										<%# Eval("Reason") %>
									</itemtemplate>

									<edititemtemplate>
										<asp:TextBox ID="txtBannedReason" Columns="20" Text='<%# Eval("Reason") %>' runat="server" MaxLength="255" />
									</edititemtemplate>
								</asp:TemplateField>

								<asp:TemplateField>
									<itemtemplate>
										<%# DateTimeHelper.Format(Convert.ToDateTime(Eval("BannedUTC")), timeZone, "g", timeOffset)%>
									</itemtemplate>

									<edititemtemplate>
										<asp:TextBox ID="txtBannedUTC" Columns="20" Text='<%# DateTimeHelper.Format(Convert.ToDateTime(Eval("BannedUTC")), timeZone, "g", timeOffset) %>' runat="server" MaxLength="30" />
									</edititemtemplate>
								</asp:TemplateField>
							</columns>

							<emptydatatemplate>
								<p class="nodata">
									<asp:Literal ID="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" />
								</p>
							</emptydatatemplate>
						</mp:mojogridview>

						<div class="settingrow">
							<portal:mojobutton id="btnAddNew" runat="server" />
						</div>

						<portal:mojocutepager id="pgrBannedIPAddresses" runat="server" />
					</asp:Panel>
				</portal:innerbodypanel>
			</portal:outerbodypanel>
		</portal:innerwrapperpanel>
	</portal:outerwrapperpanel>
</asp:Content>

<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
