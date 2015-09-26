<%@ Page language="c#" CodeBehind="SecurityRoles.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.AdminUI.SecurityRoles" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkRoleManager" runat="server" NavigateUrl="~/Admin/RoleManager.aspx" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop ID="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin securityroles ">
<portal:HeadingControl id="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<asp:Panel id="pnlSecurity" runat="server"  CssClass="securityroles">
    <div class="settingrow">
        <asp:HyperLink id="lnkFindUser" runat="server" CssClass="cblink" />
        <asp:HiddenField ID="hdnUserID" runat="server" />
        <asp:ImageButton ID="btnSetUserFromGreyBox" runat="server" />
    </div>
    <div class="settingrow">
        <asp:Repeater ID="rptRoleMembers" runat="server">
            <HeaderTemplate>
                <ul class="simplelist">
            </HeaderTemplate>
            <ItemTemplate>
                <li class="simplelist">
                    <asp:ImageButton ImageUrl='<%# DeleteLinkImage %>' 
                    AlternateText='<%# Resources.Resource.ManageUsersRemoveFromRoleButton %>'
                     ToolTip='<%# Resources.Resource.ManageUsersRemoveFromRoleButton %>'
                    CommandName="delete" 
                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "UserID") %>' runat="server" ID="btnDelete"  />
				<asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' runat="server" ID="Label1" EnableViewState="false" />
                <asp:HyperLink ID="lnkManageUser" runat="server" EnableViewState="false" Visible='<%# CanManageUsers %>'
                    NavigateUrl='<%# SiteRoot + "/Admin/ManageUsers.aspx?userid=" + Eval("UserID") %>' Text='<%# Resources.Resource.ManageUsersPageTitle %>' />
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div class="settingrow">
        <portal:mojoLabel id="Message" runat="server" CssClass="txterror warning" />
    </div>
    <portal:mojoCutePager ID="pgr" runat="server" />
</asp:Panel>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom ID="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
