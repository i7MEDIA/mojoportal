<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="PageContentWizard.ascx.cs" Inherits="mojoPortal.Web.UI.PageContentWizard" %>

<div class="padded contentwiz">
<p><asp:Literal ID="litInstructions" runat="server" EnableViewState="false" /></p>
<div class="settingrow">
    <mp:SiteLabel ID="lblModuleType" runat="server" ForControl="moduleType" CssClass="setting"
        ConfigKey="PageLayoutModuleTypeLabel"></mp:SiteLabel><br class='clear' />
    <asp:DropDownList ID="moduleType" runat="server" EnableTheming="false" CssClass="forminput"
        DataValueField="ModuleDefID" DataTextField="FeatureName">
    </asp:DropDownList>
    
</div>
<div class="settingrow">
    <mp:SiteLabel ID="lblModuleName" runat="server" ForControl="moduleTitle" CssClass="setting"
        ConfigKey="PageLayoutModuleNameLabel"></mp:SiteLabel><br class='clear' />
    <asp:TextBox ID="moduleTitle" runat="server" CssClass="widetextbox forminput" Text="" EnableViewState="false"></asp:TextBox>
     <asp:RequiredFieldValidator ID="reqModuleTitle" runat="server" ControlToValidate="moduleTitle" ValidationGroup="contentwizard" />
</div>
	<div class="settingrow">
		<mp:SiteLabel ID="lblShowTitle" runat="server" ForControl="chkShowTitle" CssClass="setting" ConfigKey="ModuleSettingsShowTitleLabel" />
		<asp:CheckBox ID="chkShowTitle" runat="server" />
	</div>
<div class="settingrow">
    <portal:mojoButton ID="btnCreateNewContent" runat="server" CssClass="forminput" ValidationGroup="contentwizard" />
</div>
</div>
