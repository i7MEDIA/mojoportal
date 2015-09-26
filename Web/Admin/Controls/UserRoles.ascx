<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="UserRoles.ascx.cs" Inherits="mojoPortal.Web.AdminUI.UserRoles" %>

<asp:HyperLink ID="lnkRolesDialog" runat="server" Visible="false" CssClass="lnkRolesDialog" EnableViewState="false" />
<asp:UpdatePanel ID="upRoles" UpdateMode="Conditional" runat="server">
<ContentTemplate> 
<div id="divRoles" runat="server" class="settingrow">
    <asp:DropDownList ID="allRoles" runat="server" DataValueField="RoleID" DataTextField="DisplayName" CssClass="forminput">
    </asp:DropDownList>
    &nbsp;
    <portal:mojoButton ID="addExisting" runat="server" Text="<%# Resources.Resource.ManageUsersAddToRoleButton %>"
        CausesValidation="false" />
</div>
 <asp:ImageButton ID="btnRefreshRoles" runat="server" />
<div id="divUserRoles" runat="server" class="settingrow">
    <portal:mojoDataList ID="userRoles" runat="server" DataKeyField="RoleID" RepeatColumns="2">
        <ItemTemplate>
            <asp:ImageButton ImageUrl='<%# DeleteLinkImage %>' CommandName="delete" CausesValidation="false"
                AlternateText='<%# Resources.Resource.ManageUsersRemoveFromRoleButton %>' runat="server"
                ToolTip='<%# Resources.Resource.ManageUsersRemoveFromRoleButton %>'
                ID="btnRemoveRole" Visible='<%# CanDeleteUserFromRole(Eval("RoleName").ToString()) %>' />
            <asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "DisplayName") %>' runat="server"
                ID="Label1" />
            &nbsp;&nbsp;&nbsp;
        </ItemTemplate>
    </portal:mojoDataList>
</div>
</ContentTemplate>
</asp:UpdatePanel>
