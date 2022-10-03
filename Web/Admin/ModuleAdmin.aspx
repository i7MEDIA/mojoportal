<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ModuleAdmin.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ModuleAdminPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkAdvancedTools" runat="server" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkFeatureAdmin" runat="server" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin moduleadmin">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<div class="settingrow">
    <portal:mojoDataList id="defsList" DataKeyField="ModuleDefID" runat="server">
		<ItemTemplate>
			<asp:HyperLink ID="HyperLink1" Visible='<%# isServerAdminSite %>' runat="server" Text='<%# Resources.Resource.ModuleAdminEditLink%>' ToolTip='<%# Resources.Resource.ModuleAdminEditLink %>' ImageUrl='<%# GetEditImageUrl() %>' NavigateUrl='<%# SiteRoot + "/Admin/ModuleDefinitions.aspx?defid=" + DataBinder.Eval(Container.DataItem, "ModuleDefID") %>' />
			&emsp;&emsp;
			<asp:Label Text='<%# mojoPortal.Web.Framework.ResourceHelper.GetResourceString(DataBinder.Eval(Container.DataItem, "ResourceFile").ToString(),DataBinder.Eval(Container.DataItem, "FeatureName").ToString()) %>' runat="server" ID="Label1" />&emsp;&emsp;
			<asp:HyperLink ID="lnkSettings" runat="server" Visible='<%# isServerAdminSite %>' Text='<%# Resources.Resource.ModuleAdminSettingsLink %>' ToolTip='<%# Resources.Resource.ModuleAdminSettingsLink %>' NavigateUrl='<%# SiteRoot + "/Admin/ModuleDefinitionSettings.aspx?defid=" + DataBinder.Eval(Container.DataItem, "ModuleDefID") %>' />&emsp;&emsp;
		    <asp:HyperLink ID="lnkPermissions" runat="server" Text='<%# Resources.Resource.FeaturePermissionsLink %>' ToolTip='<%# Resources.Resource.FeaturePermissionsLink %>' NavigateUrl='<%# SiteRoot + "/Admin/FeaturePermissions.aspx?defid=" + DataBinder.Eval(Container.DataItem, "ModuleDefID") %>' />
		
        </ItemTemplate>
	</portal:mojoDataList>
</div>
<div class="settingrow">
    <asp:HyperLink ID="lnkNewModule" runat="server" NavigateUrl="~/Admin/ModuleDefinitions.aspx?defid=-1" />
    <portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="moduledefinitionhelp" />
</div>
</portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
