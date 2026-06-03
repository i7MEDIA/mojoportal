<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="UserRoles.ascx.cs" Inherits="mojoPortal.Web.AdminUI.UserRoles" %>

<asp:HyperLink runat="server" ID="lnkRolesDialog"
	Visible="false"
	EnableViewState="false"
	CssClass="lnkRolesDialog"
	data-modal=""
	data-size="fluid-xlarge"
	data-close-text="Close"
	data-modal-type="iframe"
	data-height="full" />

<asp:UpdatePanel ID="upRoles" UpdateMode="Conditional" runat="server">
	<ContentTemplate>
		<div id="divRoles" runat="server" class="settingrow">
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

		<asp:ImageButton ID="btnRefreshRoles" runat="server" />

		<div id="divUserRoles" runat="server" class="settingrow">
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
	</ContentTemplate>
</asp:UpdatePanel>
