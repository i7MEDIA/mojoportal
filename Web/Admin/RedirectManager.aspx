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
				<div class="panel-heading">
					<h3 class="panel-title"><%= Resources.Resource.RedirectAddNew %></h3>
				</div>
				<div class="panel-body">
					<div class="form-inline">
						<div class="form-group">
							<div class="input-group">
								<asp:Label ID="lblSiteRoot" runat="server" CssClass="input-group-addon" />
								<asp:TextBox ID="txtOldUrl" runat="server" MaxLength="255" />
								<mp:SiteLabel runat="server" ConfigKey="RedirectsToLabel" CssClass="input-group-addon" UseLabelTag="false" />
							</div>
						</div>
						<div class="form-group">
							<div class="input-group">
								<asp:Label ID="lblSiteRoot2" runat="server" CssClass="input-group-addon" />
								<asp:TextBox ID="txtNewUrl" runat="server" />
							</div>
						</div>
						<portal:mojoButton  runat="server" ID="btnAdd" SkinID="AddButton"/>
					</div>
				</div>
				<div class="panel-footer">
					<portal:mojoLabel ID="lblError" runat="server" CssClass="txterror warning text-danger" />
				</div>
			</div>
		</asp:Panel>
		<asp:Panel ID="pnlSearch" runat="server" CssClass="col-md-4" DefaultButton="btnSearchUrls">
			<div class="panel panel-warning">
				<div class="panel-heading">
					<h3 class="panel-title"><%= Resources.Resource.RedirectSearch %></h3>
				</div>
				<div class="panel-body">
					<div class="input-group">
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
		<portal:mojoDataList id="dlRedirects" DataKeyField="RowGuid" runat="server">
			<ItemTemplate>
				<asp:ImageButton ImageUrl='<%# EditPropertiesImage %>' CommandName="edit" AlternateText="<%# Resources.Resource.EditLink %>" ToolTip="<%# Resources.Resource.EditLink %>"  runat="server" ID="btnEdit"/>
				<asp:ImageButton ImageUrl='<%# DeleteLinkImage %>' CommandName="delete" AlternateText="<%# Resources.Resource.DeleteLink %>" ToolTip="<%# Resources.Resource.DeleteLink %>" runat="server" ID="btnDelete"/>
				<a href='<%# RootUrl + DataBinder.Eval(Container.DataItem, "OldUrl")%>' class="btn btn-link"><%# RootUrl + DataBinder.Eval(Container.DataItem, "OldUrl")%></a>
				<span><%# Resources.Resource.RedirectsToLabel %></span>
				<a href='<%# RootUrl + DataBinder.Eval(Container.DataItem, "NewUrl")%>' class="btn btn-link"><%# RootUrl + DataBinder.Eval(Container.DataItem, "NewUrl")%></a>
			    <hr />
			</ItemTemplate>
			<EditItemTemplate>
				<div class="form-inline">
					<div class="form-group"> 
						<div class="input-group">
							<asp:Label Text='<%# RootUrl %>'  runat="server" ID="Label3" CssClass="input-group-addon"/>
							<asp:Textbox id="txtGridOldUrl" Text='<%# DataBinder.Eval(Container.DataItem, "OldUrl").ToString() %>' runat="server" CssClass="form-control"/>
							<mp:SiteLabel runat="server" ConfigKey="RedirectsToLabel" CssClass="input-group-addon" UseLabelTag="false" />
						</div>
					</div>
					<div class="form-group">
						<div class="input-group">
							<asp:Label Text='<%# RootUrl %>'  runat="server" ID="Label4" CssClass="input-group-addon"/>
							<asp:Textbox id="txtGridNewUrl" Text='<%# DataBinder.Eval(Container.DataItem, "NewUrl").ToString() %>' runat="server" CssClass="form-control"/>
						</div>
					</div>
					<portal:mojoButton Text="<%# Resources.Resource.SaveButton %>" ToolTip="<%# Resources.Resource.SaveButton %>" CommandName="apply" runat="server" ID="button1" SkinID="SaveButton"/>
					<portal:mojoButton Text="<%# Resources.Resource.CancelButton %>" ToolTip="<%# Resources.Resource.CancelButton %>" CommandName="cancel" runat="server" ID="button3" /> 
				</div>
			</EditItemTemplate>
		</portal:mojoDataList>
    <portal:mojoCutePager ID="pgrFriendlyUrls" runat="server" Visible="false" />
</portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
