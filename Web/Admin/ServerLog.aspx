<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ServerLog.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ServerLog" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx"
			CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkServerLog" runat="server" CssClass="selectedcrumb" />
	</portal:AdminCrumbContainer>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin serverlog">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">

					<asp:Panel ID="pnlDbLog" runat="server">
						<asp:Repeater ID="rptSystemLog" runat="server">
							<HeaderTemplate>
								<dl class="errorlog">
							</HeaderTemplate>
							<ItemTemplate>
								<dt class="logmessage <%# Eval("LogLevel").ToString() == "ERROR" ? "text-danger" : Eval("LogLevel").ToString() == "WARN" ? "text-warning" : "text-info" %>">
									<asp:ImageButton ImageUrl='<%# DeleteLinkImage %>' CommandName="deleteitem" CommandArgument='<%# Eval("ID") %>' AlternateText="<%# Resources.Resource.DeleteButton %>" ToolTip="<%# Resources.Resource.DeleteButton %>" runat="server" ID="btnDeleteItem" />
									<%# FormatDate(Convert.ToDateTime(Eval("LogDate"))) %> <%# Eval("LogLevel") %> <%# Eval("Logger") %> <%# FormatIpAddress(Eval("IpAddress").ToString()) %> <%# Eval("Culture") %> <%# Server.HtmlEncode(Eval("ShortUrl").ToString()) %>  </dt>
								<dd class="logmessage <%# Eval("LogLevel").ToString() == "ERROR" ? "text-danger" : Eval("LogLevel").ToString() == "WARN" ? "text-warning" : "text-info" %>">
									<%# Server.HtmlEncode(Eval("Message").ToString()) %>
								</dd>
							</ItemTemplate>
							<FooterTemplate>
								</dl>
							</FooterTemplate>
						</asp:Repeater>
						<portal:mojoCutePager ID="pgr" runat="server" />
						<portal:FormGroupPanel runat="server" SkinID="ButtonGroup">
							<asp:HyperLink ID="lnkRefresh2" runat="server" />
						</portal:FormGroupPanel>
						<portal:FormGroupPanel runat="server" SkinID="ButtonGroup">
							<asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" />

							<portal:mojoButton ID="btnClearDbLOg" runat="server" SkinID="WarningButton" />
						</portal:FormGroupPanel>
					</asp:Panel>

					<portal:BasePanel runat="server" ID="pnlFileLog">
						<portal:FormGroupPanel runat="server">
							<asp:TextBox ID="txtLog" runat="server" Width="100%" Height="600px" TextMode="MultiLine"></asp:TextBox>
						</portal:FormGroupPanel>
						<portal:FormGroupPanel runat="server" SkinID="ButtonGroup">
							<asp:HyperLink ID="lnkRefresh" runat="server" SkinID="SuccessButton" />
						</portal:FormGroupPanel>
						<portal:FormGroupPanel runat="server" SkinID="InputGroup">
							<portal:mojoButton ID="btnClearLog" runat="server" SkinID="WarningButton" />
						</portal:FormGroupPanel>
						<portal:FormGroupPanel runat="server" SkinID="ButtonGroup">
							<portal:mojoButton ID="btnDownloadLog" runat="server" SkinID="InfoButton" />

						</portal:FormGroupPanel>
					</portal:BasePanel>

				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
			<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
