<%@ Page Language="c#" CodeBehind="ModuleDefinitions.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.AdminUI.ModuleDefinitions" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkAdvancedTools" runat="server" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkModuleAdmin" runat="server" NavigateUrl="~/Admin/ModuleAdmin.aspx" />
	</portal:AdminCrumbContainer>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">

		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin moduledefinitions">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<div class="settingrow">
						<mp:SiteLabel ID="lblFeatureName" runat="server" ForControl="txtFeatureName" CssClass="settinglabel" ConfigKey="ModuleDefinitionsResourceKeyLabel" />
						<asp:TextBox ID="txtFeatureName" runat="server" Columns="50" MaxLength="255" CssClass="forminput" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="SiteLabel4" runat="server" ForControl="txtResourceFile" CssClass="settinglabel" ConfigKey="ModuleDefinitionsResourceFileLabel" />
						<asp:TextBox ID="txtResourceFile" runat="server" Columns="50" MaxLength="255" CssClass="forminput" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="lblControlSource" runat="server" ForControl="txtControlSource" CssClass="settinglabel" ConfigKey="ModuleDefinitionsDesktopSourceLabel" />
						<asp:TextBox ID="txtControlSource" runat="server" Columns="50" MaxLength="150" CssClass="forminput" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="SiteLabel3" runat="server" ForControl="txtFeatureGuid" CssClass="settinglabel" ConfigKey="ModuleDefinitionsFeatureGuidLabel" />
						<asp:TextBox ID="txtFeatureGuid" runat="server" Columns="50" MaxLength="36" CssClass="forminput" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="lblSortOrder" runat="server" ForControl="txtSortOrder" CssClass="settinglabel" ConfigKey="ModuleDefinitionsSortOrderLabel" />
						<asp:TextBox ID="txtSortOrder" runat="server" Columns="20" MaxLength="3" CssClass="forminput" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="Sitelabel5" runat="server" ForControl="chkIsCacheable" CssClass="settinglabel" ConfigKey="ModuleDefinitionsIsCacheableLabel" />
						<asp:CheckBox ID="chkIsCacheable" runat="server" CssClass="forminput" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="SiteLabel2" runat="server" ForControl="txtDefaultCacheDuration" CssClass="settinglabel" ConfigKey="ModuleDefinitionsDefaultCacheDurationLabel" />
						<asp:TextBox ID="txtDefaultCacheDuration" runat="server" Columns="20" MaxLength="8" CssClass="forminput" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="Sitelabel6" runat="server" ForControl="chkIsSearchable" CssClass="settinglabel" ConfigKey="ModuleDefinitionsIsSearchableLabel" />
						<asp:CheckBox ID="chkIsSearchable" runat="server" CssClass="forminput" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="SiteLabel7" runat="server" ForControl="txtSearchListName" CssClass="settinglabel" ConfigKey="ModuleDefinitionsSearchListNameLabel" />
						<asp:TextBox ID="txtSearchListName" runat="server" Columns="50" MaxLength="255" CssClass="forminput" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel ID="Sitelabel1" runat="server" ForControl="chkIsAdmin" CssClass="settinglabel" ConfigKey="ModuleDefinitionsIsAdminLabel" />
						<asp:CheckBox ID="chkIsAdmin" runat="server" CssClass="forminput" />
					</div>
					<div class="settingrow">
						<asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="DefinitionSettings" />
						<asp:RequiredFieldValidator ID="reqFeatureName" runat="server" Display="None" ValidationGroup="DefinitionSettings" ControlToValidate="txtFeatureName" />
						<asp:RequiredFieldValidator ID="reqControlSource" runat="server" Display="None" ValidationGroup="DefinitionSettings" ControlToValidate="txtControlSource" />
						<asp:RequiredFieldValidator ID="reqSortOrder" runat="server" Display="None" ValidationGroup="DefinitionSettings" ControlToValidate="txtSortOrder" />
						<asp:RequiredFieldValidator ID="reqDefaultCache" runat="server" Display="None" ValidationGroup="DefinitionSettings" ControlToValidate="txtDefaultCacheDuration" />
						<asp:RegularExpressionValidator ID="regexSortOrder" runat="server" Display="None" ValidationGroup="DefinitionSettings" ControlToValidate="txtSortOrder" ValidationExpression="^[0-9][0-9]{0,4}$" />
						<asp:RegularExpressionValidator ID="regexCacheDuration" runat="server" Display="None" ValidationGroup="DefinitionSettings" ControlToValidate="txtDefaultCacheDuration" ValidationExpression="^[0-9][0-9]{0,8}$" />
					</div>
					<div class="settingrow">
						<portal:mojoButton ID="updateButton" runat="server" Text="" ValidationGroup="DefinitionSettings" />&nbsp;
						<portal:mojoButton ID="cancelButton" runat="server" Text="" CausesValidation="false" />&nbsp;
						<portal:mojoButton ID="deleteButton" runat="server" Text="" CausesValidation="false" />&nbsp;
						<asp:HyperLink ID="lnkConfigureSettings" runat="server" />
						<portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="moduledefinitionedithelp" />
					</div>
					<portal:mojoLabel ID="lblErrorMessage" runat="server" CssClass="txterror warning" />
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
