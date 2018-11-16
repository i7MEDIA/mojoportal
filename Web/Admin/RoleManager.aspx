<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="RoleManager.aspx.cs" Inherits="mojoPortal.Web.AdminUI.RoleManagerPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkRoleAdmin" runat="server" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin rolemanager">
<portal:HeadingControl id="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<div class="settingrow">
    <portal:mojoDataList id="rolesList" DataKeyField="RoleID" runat="server" RepeatLayout="Flow">
		<ItemTemplate>
			<div>
				<asp:ImageButton ImageUrl='<%# EditPropertiesImage %>' CommandName="edit" AlternateText="<%# Resources.Resource.RolesEditLabel %>" ToolTip="<%# Resources.Resource.RolesEditLabel %>" runat="server" ID="btnEdit" CausesValidation="false" />
				<asp:ImageButton ImageUrl='<%# DeleteLinkImage %>' CommandName="delete" AlternateText="<%# Resources.Resource.RolesDeleteLabel %>" ToolTip="<%# Resources.Resource.RolesDeleteLabel %>" runat="server" ID="btnDelete"  />
				&nbsp;&nbsp;
				<asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "DisplayName") %>' runat="server" ID="Label1"  />
				<asp:HyperLink ID="lnkMembers" runat="server" Text='<%#  FormatMemberLink(Convert.ToInt32(Eval("MemberCount"))) %>' 
				NavigateUrl='<%# SiteRoot + "/Admin/SecurityRoles.aspx?roleid=" + DataBinder.Eval(Container.DataItem, "RoleID") %>' />
			</div>
		</ItemTemplate>
		<EditItemTemplate>
			<div class="form-inline">
				<div class="form-group col-md-3">
					<asp:Label Text="<%# Resources.Resource.RoleSystemName %>" runat="server" AssociatedControlID="roleName" />
					<asp:Textbox ID="roleName" Text='<%# DataBinder.Eval(Container.DataItem, "RoleName") %>' runat="server" Enabled="false" ClientIDMode="Static" />
					<p id="roleNameHelp" class="help-block"><%# Resources.Resource.RoleSystemNameCantChange %></p>
				</div>
				<div class="form-group col-md-3">
					<asp:Label Text="<%# Resources.Resource.RoleDisplayName %>" runat="server" AssociatedControlID="displayName" />
					<asp:Textbox ID="displayName" Text='<%# DataBinder.Eval(Container.DataItem, "DisplayName") %>' runat="server" ClientIDMode="Static" ValidationGroup="edit" />
					<asp:RequiredFieldValidator ID="rfvDisplayName" runat="server" ValidationGroup="edit" ControlToValidate="displayName" Display="Dynamic" CssClass="text-danger" ErrorMessage="*" />

				</div>
				<div class="form-group col-md-3">
					<portal:mojoButton Text="<%# Resources.Resource.RolesApplyLabel %>" ToolTip="<%# Resources.Resource.RolesApplyLabel %>" CommandName="apply" ValidationGroup="edit" runat="server" ID="Button1" SkinID="SaveButton" />&nbsp;
					<portal:mojoButton Text="<%# Resources.Resource.RoleManagerCancelButton %>" ToolTip="<%# Resources.Resource.RoleManagerCancelButton %>" CommandName="cancel"  runat="server" ID="Button2" SkinID="LinkButton"/>
				</div>
			</div>
		</EditItemTemplate>
	</portal:mojoDataList>
</div>
<div class="settingrow">
    <portal:mojoLabel ID="lblError" Runat="server" CssClass="txterror warning" />
</div>
<asp:Panel ID="pnlAddRole" runat="server" CssClass="form-inline" DefaultButton="btnAddRole">
	<h3><%= Resources.Resource.RoleAddRoleHeader %> <portal:mojoHelpLink ID="MojoHelpLink4" runat="server" HelpKey="roleadministrationhelp" /></h3>
	<div class="form-group">
		<asp:TextBox ID="txtNewRoleName" runat="server" MaxLength="50" ValidationGroup="create" />
		<asp:RequiredFieldValidator ID="rfvNewRoleName" runat="server" ValidationGroup="create" ControlToValidate="txtNewRoleName" Display="Dynamic" CssClass="text-danger" ErrorMessage='*'/>
		<%--<p id="roleNameHelp" class="help-block"><%# Resources.Resource.RoleSystemNameCantChange %></p>--%>
	</div>
	<div class="form-group">
		<asp:TextBox ID="txtNewDisplayName" runat="server" MaxLength="50" ValidationGroup="create" />
		<asp:RequiredFieldValidator ID="rfvNewDisplayName" runat="server" ValidationGroup="create" ControlToValidate="txtNewDisplayName" Display="Dynamic" CssClass="text-danger" ErrorMessage='*' />
	</div>
	<div class="form-group">
		<portal:mojoButton runat="server" id="btnAddRole" SkinID="AddButton" ValidationGroup="create" />
	</div>
	<asp:ValidationSummary runat="server" CssClass="alert alert-danger" ValidationGroup="create" ShowMessageBox="false" ShowSummary="false" />
</asp:Panel>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
