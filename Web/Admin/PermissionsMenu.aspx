<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="PermissionsMenu.aspx.cs" Inherits="mojoPortal.Web.AdminUI.PermissionsMenuPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx"
            CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkSiteList" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx"
            CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator id="litSiteListLinkSeparator" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkPermissionsMenu" runat="server" CssClass="selectedcrumb" />
 </portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper ">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">

<ul class="simplelist">
		    <li id="liSiteEditorRoles" runat="server">
		        <asp:HyperLink ID="lnkSiteEditorRoles" runat="server" CssClass="lnkSiteEditorRoles" />
		    </li>
            <li id="liRolesThatCanViewCommerceReports" runat="server">
		        <asp:HyperLink ID="lnkRolesThatCanViewCommerceReports" runat="server" CssClass="lnkRolesThatCanViewCommerceReports" />
		    </li>
			<li id="liRolesThatCanBrowseFileSystem" runat="server">
				<asp:HyperLink ID="lnkRolesThatCanBrowseFileSystem" runat="server" CssClass="lnkRolesThatCanBrowseFileSystem" />
			</li>
            <li id="liRolesThatCanUploadAndBrowse" runat="server">
		        <asp:HyperLink ID="lnkRolesThatCanUploadAndBrowse" runat="server" CssClass="lnkRolesThatCanUploadAndBrowse" />
		    </li>
		    <li id="liRolesThatCanUploadAndBrowseUserOnly" runat="server">
		        <asp:HyperLink ID="lnkRolesThatCanUploadAndBrowseUserOnly" runat="server" CssClass="lnkRolesThatCanUploadAndBrowseUserOnly" />
		    </li>
		    <li id="liRolesThatCanDeleteFiles" runat="server">
		        <asp:HyperLink ID="lnkRolesThatCanDeleteFiles" runat="server" CssClass="lnkRolesThatCanDeleteFiles" />
		    </li>
		    <li id="liRolesThatCanManageSkins" runat="server">
		        <asp:HyperLink ID="lnkRolesThatCanManageSkins" runat="server" CssClass="lnkRolesThatCanManageSkins" />
		    </li>
            <li id="liRolesThatCanAssignSkinsToPages" runat="server">
		        <asp:HyperLink ID="lnkRolesThatCanAssignSkinsToPages" runat="server" CssClass="lnkRolesThatCanAssignSkinsToPages" />
		    </li>
		    <li id="liRolesThatCanEditContentTemplates" runat="server">
		        <asp:HyperLink ID="lnkRolesThatCanEditContentTemplates" runat="server" CssClass="lnkRolesThatCanEditContentTemplates" />
		    </li>
		    <li id="liRolesNOTAllowedInstanceSettings" runat="server">
		        <asp:HyperLink ID="lnkRolesNOTAllowedInstanceSettings" runat="server" CssClass="lnkRolesNOTAllowedInstanceSettings" />
		    </li>
            <li id="liRolesThatCanViewMemberList" runat="server">
		        <asp:HyperLink ID="lnkRolesThatCanViewMemberList" runat="server" CssClass="lnkRolesThatCanViewMemberList" />
		    </li>
		    <li id="liRolesThatCanLookupUsers" runat="server">
		        <asp:HyperLink ID="lnkRolesThatCanLookupUsers" runat="server" CssClass="lnkRolesThatCanLookupUsers" />
		    </li>
            <li id="liRolesThatCanCreateUsers" runat="server">
		        <asp:HyperLink ID="lnkRolesThatCanCreateUsers" runat="server" CssClass="lnkRolesThatCanCreateUsers" />
		    </li>
		    <li id="liRolesThatCanManageUsers" runat="server">
		        <asp:HyperLink ID="lnkRolesThatCanManageUsers" runat="server" CssClass="lnkRolesThatCanManageUsers" />
		    </li>
            <li id="liRolesThatCanUseMyPage" runat="server">
		        <asp:HyperLink ID="lnkRolesThatCanUseMyPage" runat="server" CssClass="lnkRolesThatCanUseMyPage" />
		    </li>
		    <li id="liRolesThatCanCreateRootLevelPages" runat="server">
		        <asp:HyperLink ID="lnkRolesThatCanCreateRootLevelPages" runat="server" CssClass="lnkRolesThatCanCreateRootLevelPages" />
		    </li>
		    <li id="liDefaultRootLevelViewRoles" runat="server">
		        <asp:HyperLink ID="lnkDefaultRootLevelViewRoles" runat="server" CssClass="lnkDefaultRootLevelViewRoles" />
		    </li>
		    <li id="liDefaultRootLevelEditRoles" runat="server">
		        <asp:HyperLink ID="lnkDefaultRootLevelEditRoles" runat="server" CssClass="lnkDefaultRootLevelEditRoles" />
		    </li>
		    <li id="liDefaultRootLevelCreateChildPageRoles" runat="server">
		        <asp:HyperLink ID="lnkDefaultRootLevelCreateChildPageRoles" runat="server" CssClass="lnkDefaultRootLevelCreateChildPageRoles" />
		    </li>
            
		    
		</ul>


</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel> 
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

