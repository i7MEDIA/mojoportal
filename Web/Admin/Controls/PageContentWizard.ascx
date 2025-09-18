<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="PageContentWizard.ascx.cs" Inherits="mojoPortal.Web.UI.PageContentWizard" %>

<div class="padded contentwiz">
	<p>
		<asp:Literal ID="litInstructions" runat="server" EnableViewState="false" />
	</p>
	<div class="settingrow">
		<mp:SiteLabel runat="server" ForControl="moduleType" CssClass="setting" ConfigKey="PageLayoutModuleTypeLabel" />
		<br class="clear" />
		<asp:DropDownList ID="moduleType" runat="server" EnableTheming="false" CssClass="forminput" DataValueField="ModuleDefID" DataTextField="FeatureName" />

	</div>
	<div class="settingrow">
		<mp:SiteLabel runat="server" ForControl="moduleTitle" CssClass="setting" ConfigKey="PageLayoutModuleNameLabel" />
		<br class="clear" />
		<asp:TextBox ID="moduleTitle" runat="server" CssClass="widetextbox forminput" Text="" EnableViewState="false" />
		<asp:RequiredFieldValidator ID="reqModuleTitle" runat="server" ControlToValidate="moduleTitle" ValidationGroup="contentwizard" />
	</div>
	<div class="settingrow">
		<mp:SiteLabel runat="server" ForControl="chkShowTitle" CssClass="setting" ConfigKey="ModuleSettingsShowTitleLabel" />
		<asp:CheckBox ID="chkShowTitle" runat="server" />
	</div>
	<div id="divTitleElement" runat="server" class="settingrow">
		<mp:SiteLabel runat="server" ForControl="txtTitleElement" CssClass="clear" ConfigKey="ModuleSettingsTitleElement" />
		<br class="clear" />
		<asp:DropDownList runat="server" ID="ddlTitleElements" />
	</div>
	<portal:FormGroupPanel ID="fgpModuleViewRoles" runat="server">
		<mp:SiteLabel ID="lblViewRoles" runat="server" ForControl="ddlViewRoles" CssClass="clear" ConfigKey="ModuleSettingsViewRolesLabel" />
		<br class="clear" />
		<asp:DropDownList runat="server" ID="ddlViewRoles" />
	</portal:FormGroupPanel>
	<div class="settingrow">
		<portal:mojoButton ID="btnCreateNewContent" runat="server" CssClass="forminput" ValidationGroup="contentwizard" />
	</div>
</div>
