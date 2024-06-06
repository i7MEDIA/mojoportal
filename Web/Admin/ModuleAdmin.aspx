<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ModuleAdmin.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ModuleAdminPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkAdvancedTools" runat="server" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkFeatureAdmin" runat="server" />
	</portal:AdminCrumbContainer>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin moduleadmin">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<div class="settingrow">
						<asp:Repeater ID="featuresList" runat="server">
							<HeaderTemplate><table border="0" class="table table-bordered table-striped table-hover">
									<thead>
										<tr>
											<th style="width:25%;">
												<mp:SiteLabel runat="server" ConfigKey="ModuleSettingsFeatureNameLabel" UseLabelTag="false" />
											</th>
											<th>
												<mp:SiteLabel runat="server" ConfigKey="AdminIndexBrowserActionsHeading" UseLabelTag="false" />
											</th>
										</tr>
									</thead></HeaderTemplate>
							<ItemTemplate><tr><td><asp:HyperLink ID="HyperLink1" Visible='<%# isServerAdminSite %>' runat="server" Text='<%# mojoPortal.Web.Framework.ResourceHelper.GetResourceString(DataBinder.Eval(Container.DataItem, "ResourceFile").ToString(),DataBinder.Eval(Container.DataItem, "FeatureName").ToString()) %>' ToolTip='<%# Resources.Resource.ModuleAdminEditLink %>' NavigateUrl='<%# "Admin/ModuleDefinitions.aspx".ToLinkBuilder().AddParam("defid", DataBinder.Eval(Container.DataItem, "ModuleDefID")) %>' />
								<td>
									<asp:HyperLink ID="lnkSettings" runat="server" Visible='<%# isServerAdminSite %>' Text='<%# Resources.Resource.ModuleAdminSettingsLink %>' ToolTip='<%# Resources.Resource.ModuleAdminSettingsLink %>' ImageUrl='<%# $"Data/SiteImages/{WebConfigSettings.EditPropertiesImage}".ToLinkBuilder() %>' NavigateUrl='<%# "Admin/ModuleDefinitionSettings.aspx".ToLinkBuilder().AddParam("defid", DataBinder.Eval(Container.DataItem, "ModuleDefID")) %>' />&emsp;&emsp;
									<asp:HyperLink ID="lnkPermissions" runat="server" Text='<%# Resources.Resource.FeaturePermissionsLink %>' ToolTip='<%# Resources.Resource.FeaturePermissionsLink %>' ImageUrl='<%# $"Data/SiteImages/{WebConfigSettings.AdminImage}".ToLinkBuilder() %>' NavigateUrl='<%# "Admin/FeaturePermissions.aspx".ToLinkBuilder().AddParam("defid", DataBinder.Eval(Container.DataItem, "ModuleDefID")) %>' /></td>
								</tr></ItemTemplate>
							<FooterTemplate></table></FooterTemplate>
						</asp:Repeater>
					</div>
					<div class="settingrow">
						<asp:HyperLink ID="lnkNewModule" runat="server" NavigateUrl="~/Admin/ModuleDefinitions.aspx" />
						<portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="moduledefinitionhelp" />
					</div>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
