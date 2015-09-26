<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="RoleManager.aspx.cs" Inherits="mojoPortal.Web.AdminUI.RoleManagerPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkRoleAdmin" runat="server" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop ID="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin rolemanager">
<portal:HeadingControl id="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<div class="settingrow">
    <portal:mojoDataList id="rolesList" DataKeyField="RoleID" runat="server">
		<ItemTemplate>
			<asp:ImageButton ImageUrl='<%# EditPropertiesImage %>' CommandName="edit" AlternateText="<%# Resources.Resource.RolesEditLabel %>" ToolTip="<%# Resources.Resource.RolesEditLabel %>" runat="server" ID="btnEdit"  />
			<asp:ImageButton ImageUrl='<%# DeleteLinkImage %>' CommandName="delete" AlternateText="<%# Resources.Resource.RolesDeleteLabel %>" ToolTip="<%# Resources.Resource.RolesDeleteLabel %>" runat="server" ID="btnDelete"  />
			&nbsp;&nbsp;
			<asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "DisplayName") %>' runat="server" ID="Label1"  />
			<asp:HyperLink ID="lnkMembers" runat="server" Text='<%#  FormatMemberLink(Convert.ToInt32(Eval("MemberCount"))) %>' 
			NavigateUrl='<%# SiteRoot + "/Admin/SecurityRoles.aspx?roleid=" + DataBinder.Eval(Container.DataItem, "RoleID") %>' />
		</ItemTemplate>
		<EditItemTemplate>
			<asp:Textbox id="roleName" width="200" Text='<%# DataBinder.Eval(Container.DataItem, "DisplayName") %>' runat="server" />
			&nbsp;
			<asp:Button Text="<%# Resources.Resource.RolesApplyLabel %>" ToolTip="<%# Resources.Resource.RolesApplyLabel %>" CommandName="apply"  runat="server" ID="Button1" />
			&nbsp;
			<asp:Button Text="<%# Resources.Resource.RoleManagerCancelButton %>" ToolTip="<%# Resources.Resource.RoleManagerCancelButton %>" CommandName="cancel"  runat="server" ID="Button2" />
			
		</EditItemTemplate>
	</portal:mojoDataList>
</div>
<div class="settingrow">
    <portal:mojoLabel ID="lblError" Runat="server" CssClass="txterror warning" />
</div>
<asp:Panel ID="pnlAddRole" runat="server" CssClass="settingrow" DefaultButton="btnAddRole">
    <portal:mojoButton  runat="server" id="btnAddRole" />
	<asp:TextBox ID="txtNewRoleName" Runat="server" MaxLength="50" CssClass="mediumtextbox"></asp:TextBox>
	<portal:mojoHelpLink ID="MojoHelpLink4" runat="server" HelpKey="roleadministrationhelp" />	
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
