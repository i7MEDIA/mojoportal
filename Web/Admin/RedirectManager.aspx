<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="RedirectManager.aspx.cs" Inherits="mojoPortal.Web.AdminUI.RedirectManagerPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkAdvancedTools" runat="server" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkRedirectManager" runat="server" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin urlmanager">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
	<div class="settingrow">
		<mp:SiteLabel ID="Sitelabel5" runat="server" ConfigKey="RedirectHelp" />
	</div>
	<div class="row">
		<asp:Panel ID="pnlAddRedirect" runat="server" DefaultButton="btnAdd" CssClass="col-md-8">
			<div class="panel panel-primary">
				<div class="panel-heading"><h3 class="panel-title"><%= Resources.Resource.RedirectAddNew %></h3></div>
				<div class="panel-body">
					<div class="form-inline">
						<div class="form-group">
							<div class="input-group input-group-sm">
								<asp:Label ID="lblSiteRoot" runat="server" CssClass="input-group-addon" />
								<asp:TextBox ID="txtOldUrl" runat="server" MaxLength="255" />
								<mp:SiteLabel runat="server" ConfigKey="RedirectsToLabel" CssClass="input-group-addon" UseLabelTag="false" />
							</div>
						</div>
						<div class="form-group">
							<div class="input-group input-group-sm">
								<asp:Label ID="lblSiteRoot2" runat="server" CssClass="input-group-addon" />
								<asp:TextBox ID="txtNewUrl" runat="server" />
								<span class="input-group-btn">
									<portal:mojoButton  runat="server" ID="btnAdd" SkinID="AddButton"/>
								</span>
							</div>
						</div>
					</div>
				</div>
				<div class="panel-footer"><portal:mojoLabel ID="lblError" runat="server" CssClass="txterror warning text-danger" /></div>
			</div>
		</asp:Panel>
		<asp:Panel ID="pnlSearch" runat="server" CssClass="col-md-4" DefaultButton="btnSearchUrls">
			<div class="panel panel-warning">
				<div class="panel-heading">
					<h3 class="panel-title"><%= Resources.Resource.RedirectSearch %></h3>
				</div>
				<div class="panel-body">
					<div class="input-group input-group-sm">
						<asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" />
						<span class="input-group-btn">
							<portal:mojoButton ID="btnSearchUrls" runat="server" SkinID="InfoButton" />
							<portal:mojoButton ID="btnClearSearch" runat="server" />
						</span>
					</div>
				</div>
				<div class="panel-footer"></div>
			</div>
		</asp:Panel>
	</div>
	<asp:DataList ID="dlRedirects" DataKeyField="RowGuid" runat="server" CssClass="table table-bordered table-striped table-hover table-responsive" ExtractTemplateRows="true">
		<HeaderTemplate>
			<asp:Table runat="server">
				<asp:TableHeaderRow>
					<asp:TableHeaderCell></asp:TableHeaderCell>
					<asp:TableHeaderCell><%# Resources.Resource.OldUrl %></asp:TableHeaderCell>
					<asp:TableHeaderCell></asp:TableHeaderCell>
					<asp:TableHeaderCell><%# Resources.Resource.NewUrl %></asp:TableHeaderCell>
				</asp:TableHeaderRow>
			</asp:Table>
		</HeaderTemplate>
		<ItemTemplate>
			<asp:Table runat="server">
				<asp:TableRow>
					<asp:TableCell CssClass="text-center input-group-btn">
						<span class="input-group-btn">
							<portal:mojoButton runat="server" CommandName="edit" Text="<%# Resources.Resource.EditLink %>" SkinID="SaveAsNewButton" />
							<portal:mojoButton runat="server" CommandName="delete" Text="<%# Resources.Resource.DeleteLink %>" SkinID="DeleteButton" />
						</span>
					</asp:TableCell>
					<asp:TableCell CssClass="text-left">
						<a href='<%# RootUrl + DataBinder.Eval(Container.DataItem, "OldUrl")%>' title='<%# Resources.Resource.RedirectGoToOld %>'><%# "~/" + DataBinder.Eval(Container.DataItem, "OldUrl")%></a>
					</asp:TableCell>
					<asp:TableCell CssClass="text-center">
						<%# Resources.Resource.RedirectsToLabel %>
					</asp:TableCell>
					<asp:TableCell CssClass="text-left">
						<a href='<%# RootUrl + DataBinder.Eval(Container.DataItem, "NewUrl")%>' title='<%# Resources.Resource.RedirectGoToNew %>'><%# "~/" + DataBinder.Eval(Container.DataItem, "NewUrl")%></a>
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
		</ItemTemplate>
		<EditItemTemplate>
			<asp:Table runat="server">
				<asp:TableRow>
					<asp:TableCell ColumnSpan="3" CssClass="text-left">
						<div class="panel panel-info">
							<div class="panel-heading">
								<h3 class="panel-title"><%# Resources.Resource.RedirectEdit %></h3>
							</div>
							<div class="panel-body">
								<div class="form-inline">
									<div class="form-group">
										<div class="input-group input-group-sm">
											<asp:Label Text='<%# RootUrl %>' runat="server" ID="Label3" CssClass="input-group-addon" />
											<asp:TextBox ID="txtGridOldUrl" Text='<%# DataBinder.Eval(Container.DataItem, "OldUrl").ToString() %>' runat="server" CssClass="form-control" />
											<mp:SiteLabel runat="server" ConfigKey="RedirectsToLabel" CssClass="input-group-addon" UseLabelTag="false" />
										</div>
									</div>
									<div class="form-group">
										<div class="input-group input-group-sm">
											<asp:Label Text='<%# RootUrl %>' runat="server" ID="Label4" CssClass="input-group-addon" />
											<asp:TextBox ID="txtGridNewUrl" Text='<%# DataBinder.Eval(Container.DataItem, "NewUrl").ToString() %>' runat="server" CssClass="form-control" />
											<span class="input-group-btn">
												<portal:mojoButton Text="<%# Resources.Resource.SaveButton %>" ToolTip="<%# Resources.Resource.SaveButton %>" CommandName="apply" runat="server" ID="button1" SkinID="SaveButton" />
												<portal:mojoButton Text="<%# Resources.Resource.CancelButton %>" ToolTip="<%# Resources.Resource.CancelButton %>" CommandName="cancel" runat="server" ID="button3" />
											</span>
										</div>
									</div>
								</div>
							</div>
						</div>
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
		</EditItemTemplate>
	</asp:DataList>
    <portal:mojoCutePager ID="pgrFriendlyUrls" runat="server" Visible="false" />
</portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
