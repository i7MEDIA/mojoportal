<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="UrlManager.aspx.cs" Inherits="mojoPortal.Web.AdminUI.UrlManagerPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" />
		<portal:AdminCrumbSeparator ID="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkAdvancedTools" runat="server" />
		<portal:AdminCrumbSeparator ID="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkUrlManager" runat="server" />
	</portal:AdminCrumbContainer>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin urlmanager">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<div class="row">
						<asp:Panel ID="pnlAddUrl" runat="server" DefaultButton="btnAddFriendlyUrl" CssClass="col-md-8">
							<div class="panel panel-primary">
								<div class="panel-heading">
									<h3 class="panel-title"><%= Resources.Resource.FriendlyUrlAddNewLabel %></h3>
								</div>
								<div class="panel-body">
									<div class="form-group">
										<div class="input-group input-group-sm">
											<asp:Label ID="lblFriendlyUrlRoot" runat="server" CssClass="input-group-addon" />
											<asp:TextBox ID="txtFriendlyUrl" runat="server" Columns="60" MaxLength="255" />
											<mp:SiteLabel runat="server" ConfigKey="RedirectsToLabel" CssClass="input-group-addon" UseLabelTag="false" />
										</div>
									</div>
									<div class="row form-group">
										<div class="col-md-5">
											<mp:SiteLabel runat="server" ConfigKey="FriendlyUrlSelectFromDropdownLabel" CssClass="control-label" />
											<asp:DropDownList ID="ddPages" runat="server" DataTextField="key" DataValueField="value" Style="display: block;width:100%;" />
										</div>
										<div class="col-md-2 text-center">
											<mp:SiteLabel runat="server" ConfigKey="LiteralOr" CssClass="control-label" />
											<br />
											|
										</div>
										<div class="col-md-5">
											<mp:SiteLabel ID="Sitelabel6" runat="server" ConfigKey="FriendlyUrlExpertEntryLabel" CssClass="control-label" />
											<asp:TextBox ID="txtRealUrl" Columns="60" runat="server" style="display:block;width:100%;" />
										</div>
									</div>
									<div class="text-center">
										<portal:mojoButton runat="server" ID="btnAddFriendlyUrl" SkinID="SaveButton" />
									</div>
								</div>
								<div class="panel-footer">
									<portal:mojoLabel ID="lblError" runat="server" CssClass="txterror warning" />
								</div>
							</div>
						</asp:Panel>


						<asp:Panel ID="pnlSearch" runat="server" CssClass="col-md-4 urlsearch" DefaultButton="btnSearchUrls">
							<div class="panel panel-warning">
								<div class="panel-heading">
									<h3 class="panel-title"><%= Resources.Resource.FriendlyUrlSearch %></h3>
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
							</div>
						</asp:Panel>
					</div>

					<asp:DataList ID="dlUrlMap" DataKeyField="UrlID" runat="server" CssClass="table table-bordered table-striped table-hover table-responsive"  ExtractTemplateRows="true">
						<HeaderTemplate>
							<asp:Table runat="server">
								<asp:TableHeaderRow>
									<asp:TableHeaderCell></asp:TableHeaderCell>
									<asp:TableHeaderCell><%# Resources.Resource.FriendlyUrlLabel %></asp:TableHeaderCell>
									<asp:TableHeaderCell></asp:TableHeaderCell>
									<asp:TableHeaderCell><%# Resources.Resource.FriendlyUrlRealUrlLabel %></asp:TableHeaderCell>
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
										<a href='<%# RootUrl + DataBinder.Eval(Container.DataItem, "FriendlyUrl")%>' title='<%# Resources.Resource.FriendlyUrlViewLink %>'><%# "~/" + DataBinder.Eval(Container.DataItem, "FriendlyUrl")%></a>
									</asp:TableCell>
									<asp:TableCell CssClass="text-center">
										<%# Resources.Resource.RedirectsToLabel %>
									</asp:TableCell>
									<asp:TableCell CssClass="text-left">
										<a href='<%# RootUrl + DataBinder.Eval(Container.DataItem, "RealUrl").ToString().Replace("~/", "")%>' title='<%# Resources.Resource.FriendlyUrlViewRealLink %>'><%# DataBinder.Eval(Container.DataItem, "RealUrl")%></a>
									</asp:TableCell>
								</asp:TableRow>
							</asp:Table>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:Table runat="server">
								<asp:TableRow>
									<asp:TableCell ColumnSpan="4" CssClass="text-left">
										<div class="panel panel-info">
											<div class="panel-heading">
												<h3 class="panel-title"><%# Resources.Resource.FriendlyUrlEdit %></h3>
											</div>
											<div class="panel-body">
												<div class="form-group">
													<div class="input-group input-group-sm">
														<asp:Label Text='<%# RootUrl %>' runat="server" ID="Label4" CssClass="input-group-addon" />
														<asp:TextBox ID="txtItemFriendlyUrl" Columns="60" Text='<%# DataBinder.Eval(Container.DataItem, "FriendlyUrl") %>' runat="server" />
														<mp:SiteLabel ID="Sitelabel2" runat="server" ConfigKey="RedirectsToLabel" UseLabelTag="false" CssClass="input-group-addon"/>
													</div>
												</div>
												<div class="row form-group">
													<div class="col-md-5">
														<mp:SiteLabel ID="Sitelabel7" runat="server" ConfigKey="FriendlyUrlSelectFromDropdownLabel" UseLabelTag="true" CssClass="control-label" />
														<asp:DropDownList ID="ddPagesEdit" runat="server" DataSource='<%# PageList %>' DataValueField="value" DataTextField="key" 
															SelectedValue='<%# GetSelectedPage(DataBinder.Eval(Container.DataItem, "PageGuid").ToString()) %>'
															style="display:block;width:100%;"/>
					

													</div>
													<div class="col-md-2 text-center">
														<mp:SiteLabel runat="server" ConfigKey="LiteralOr" CssClass="control-label" />
														<br />
														|
													</div>
													<div class="col-md-5">
														<mp:SiteLabel ID="Sitelabel6" runat="server" ConfigKey="FriendlyUrlExpertEntryLabel" UseLabelTag="true" CssClass="control-label"/>
														<asp:TextBox ID="txtItemRealUrl" Columns="60" Text='<%# GetRealUrl(DataBinder.Eval(Container.DataItem, "RealUrl").ToString(), DataBinder.Eval(Container.DataItem, "PageGuid").ToString())%>' runat="server" style="display:block;width:100%;"/>
													</div>
				
												</div>
												<div class="text-center ">
													<portal:mojoButton Text="<%# Resources.Resource.FriendlyUrlSaveLabel %>" ToolTip="<%# Resources.Resource.FriendlyUrlSaveLabel %>" CommandName="apply" runat="server" ID="MojoButton1" SkinID="SaveButton" />
													&nbsp;<portal:mojoButton Text="<%# Resources.Resource.CancelButton %>" ToolTip="<%# Resources.Resource.CancelButton %>" CommandName="cancel" runat="server" ID="MojoButton2" />
												</div>
											</div>
											<div class="panel-footer">
												<portal:mojoLabel ID="lblEditError" runat="server" CssClass="txterror warning" />
											</div>
										</div>
									</asp:TableCell>
								</asp:TableRow>
							</asp:Table>								
						</EditItemTemplate>
					</asp:DataList>
					<portal:mojoCutePager ID="pgrFriendlyUrls" runat="server" />
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

