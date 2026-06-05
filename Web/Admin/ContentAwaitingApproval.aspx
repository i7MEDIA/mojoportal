<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContentAwaitingApproval.aspx.cs"
	MasterPageFile="~/App_MasterPages/layout.Master" Inherits="mojoPortal.Web.AdminUI.ContentAwaitingApprovalPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:admincrumbcontainer id="pnlAdminCrumbs" runat="server" cssclass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />
		<portal:admincrumbseparator id="litLinkSeparator1" runat="server" text="&nbsp;&gt;" enableviewstate="false" />
		<asp:HyperLink ID="lnkContentWorkFlow" runat="server" NavigateUrl="~/Admin/ContentWorkflow.aspx" />
		<portal:admincrumbseparator id="AdminCrumbSeparator1" runat="server" text="&nbsp;&gt;" enableviewstate="false" />
		<asp:HyperLink ID="lnkCurrentPage" runat="server" CssClass="selectedcrumb" />
	</portal:admincrumbcontainer>

	<portal:outerwrapperpanel id="pnlOuterWrap" runat="server">
		<portal:innerwrapperpanel id="pnlInnerWrap" runat="server" cssclass="panelwrapper admin workflow">
			<portal:headingcontrol id="heading" runat="server" />
			<portal:outerbodypanel id="pnlOuterBody" runat="server">
				<portal:innerbodypanel id="pnlInnerBody" runat="server" cssclass="modulecontent">
					<asp:Literal ID="ltlIntroduction" runat="server"></asp:Literal>
					<mp:mojogridview id="grdContentAwaitingApproval" runat="server" allowpaging="false" allowsorting="false"
						autogeneratecolumns="false" datakeynames="Guid">
						<columns>
							<asp:TemplateField>
								<itemtemplate>
									<%# Eval("ModuleTitle")%><span><a href='<%# SiteRoot + "/Admin/ModuleSettings.aspx?mid=" + Eval("ModuleID") %>'>settings</a></span>
								</itemtemplate>
							</asp:TemplateField>

							<asp:TemplateField>
								<itemtemplate>
									<%# Eval("RecentActionByUserName")%>
								</itemtemplate>
							</asp:TemplateField>

							<asp:TemplateField>
								<itemtemplate>
									<%# DateTimeHelper.Format(Convert.ToDateTime(Eval("RecentActionOn")), timeZone, "g", timeOffset) %>
								</itemtemplate>
							</asp:TemplateField>

							<asp:TemplateField>
								<itemtemplate>
									<asp:Repeater ID="rptPageLinks" runat="server">
										<itemtemplate>
											<a href='<%# SiteRoot + Eval("PageUrl").ToString().Replace("~/","/") + "?vm=WorkInProgess"%>'><%# Eval("PageName")%></a>
										</itemtemplate>
									</asp:Repeater>
								</itemtemplate>
							</asp:TemplateField>
						</columns>

						<emptydatatemplate>
							<p class="nodata">
								<asp:Literal ID="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" /></p>
						</emptydatatemplate>
					</mp:mojogridview>

					<portal:mojocutepager id="pgrContentAwaitingApproval" runat="server" />
				</portal:innerbodypanel>
			</portal:outerbodypanel>
		</portal:innerwrapperpanel>
	</portal:outerwrapperpanel>
	<portal:sessionkeepalivecontrol id="ka1" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

