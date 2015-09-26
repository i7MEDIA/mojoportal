<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="UserRolesDialog.aspx.cs" Inherits="mojoPortal.Web.UI.UserRolesDialog" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server">
<asp:Literal id="litStyleLink" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">

<div id="divRoles" runat="server" class="settingrow">
    <asp:DropDownList ID="allRoles" runat="server" DataValueField="RoleID" DataTextField="DisplayName" CssClass="forminput">
    </asp:DropDownList>
    &nbsp;
    <portal:mojoButton ID="addExisting" runat="server" Text="<%# Resources.Resource.ManageUsersAddToRoleButton %>"
        CausesValidation="false" />
    
</div>
<asp:HyperLink ID="lnkRolesDialog" runat="server" Visible="false" CssClass="cblink lnkRolesDialog" EnableViewState="false" />
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

</asp:Content>
