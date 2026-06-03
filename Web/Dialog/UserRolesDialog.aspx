<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="UserRolesDialog.aspx.cs" Inherits="mojoPortal.Web.UI.UserRolesDialog" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server">
	<asp:Literal ID="litStyleLink" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
	<div runat="server" id="divRoles" class="settingrow">
		<asp:HiddenField runat="server" ID="hdnUserPassword" />

		<asp:DropDownList runat="server"
			ID="allRoles"
			DataValueField="RoleID"
			DataTextField="DisplayName"
			CssClass="forminput" />

		<portal:mojoButton runat="server"
			ID="addExisting"
			Text="<%# Resources.Resource.ManageUsersAddToRoleButton %>"
			CausesValidation="false"
			hidden="hidden"
			style="display: none;" />

			<button
				id="enterPasswordBtn"
				onclick="EnterPassword();"
				type="button"
				class="btn btn-default">
				<%= Resources.Resource.ManageUsersAddToRoleButton %>
			</button>
			<asp:Label runat="server" ID="lblPasswordError" Visible="false" CssClass="text-danger" />
	</div>

	<asp:HyperLink ID="lnkRolesDialog" runat="server" Visible="false" CssClass="cblink lnkRolesDialog" EnableViewState="false" />

	<div runat="server" id="divUserRoles" class="settingrow">
		<portal:mojoDataList runat="server"
			ID="userRoles"
			DataKeyField="RoleID"
			RepeatColumns="2">
			
			<ItemTemplate>
				<asp:ImageButton runat="server"
					ImageUrl='<%# DeleteLinkImage %>'
					CommandName="delete"
					CausesValidation="false"
					AlternateText='<%# Resources.Resource.ManageUsersRemoveFromRoleButton %>'
					ToolTip='<%# Resources.Resource.ManageUsersRemoveFromRoleButton %>'
					ID="btnRemoveRole"
					Visible='<%# CanDeleteUserFromRole(Eval("RoleName").ToString()) %>' />

				<asp:Label runat="server"
					ID="Label1"
					Text='<%# DataBinder.Eval(Container.DataItem, "DisplayName") %>' />
				&nbsp;&nbsp;&nbsp;
			</ItemTemplate>

		</portal:mojoDataList>
	</div>
</asp:Content>
