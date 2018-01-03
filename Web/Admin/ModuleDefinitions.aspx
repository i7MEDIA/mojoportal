<%@ Page language="c#" CodeBehind="ModuleDefinitions.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.AdminUI.ModuleDefinitions" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkModuleAdmin" runat="server" NavigateUrl="~/Admin/ModuleAdmin.aspx" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin moduledefinitions">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    <div class="settingrow">
        <mp:SiteLabel id="lblFeatureName" runat="server" ForControl="txtFeatureName" CssClass="settinglabel" ConfigKey="ModuleDefinitionsResourceKeyLabel"></mp:SiteLabel>
        <asp:TextBox id="txtFeatureName" runat="server" Columns="50" maxlength="255" CssClass="forminput"></asp:TextBox>
    </div>
    <div class="settingrow">
        <mp:SiteLabel id="SiteLabel4" runat="server" ForControl="txtResourceFile" CssClass="settinglabel" ConfigKey="ModuleDefinitionsResourceFileLabel"></mp:SiteLabel>
        <asp:TextBox id="txtResourceFile" runat="server" Columns="50" maxlength="255" CssClass="forminput"></asp:TextBox>
    </div>
    <div class="settingrow">
        <mp:SiteLabel id="lblControlSource" runat="server" ForControl="txtControlSource" CssClass="settinglabel" ConfigKey="ModuleDefinitionsDesktopSourceLabel"></mp:SiteLabel>
        <asp:TextBox id="txtControlSource" runat="server" Columns="50" maxlength="150" CssClass="forminput"></asp:TextBox>
    </div>
    <div class="settingrow">
        <mp:SiteLabel id="SiteLabel3" runat="server" ForControl="txtFeatureGuid" CssClass="settinglabel" ConfigKey="ModuleDefinitionsFeatureGuidLabel"></mp:SiteLabel>
        <asp:TextBox id="txtFeatureGuid" runat="server" Columns="50" maxlength="36" CssClass="forminput"></asp:TextBox>
    </div>
    <div class="settingrow">
        <mp:SiteLabel id="lblSortOrder" runat="server" ForControl="txtSortOrder" CssClass="settinglabel" ConfigKey="ModuleDefinitionsSortOrderLabel"></mp:SiteLabel>
        <asp:TextBox id="txtSortOrder" runat="server" Columns="20" maxlength="3" CssClass="forminput"></asp:TextBox>
    </div>
     <div class="settingrow">
        <mp:SiteLabel id="Sitelabel5" runat="server" ForControl="chkIsCacheable" CssClass="settinglabel" ConfigKey="ModuleDefinitionsIsCacheableLabel"></mp:SiteLabel>
        <asp:CheckBox ID="chkIsCacheable" runat="server" CssClass="forminput"></asp:CheckBox>
    </div>
    <div class="settingrow">
        <mp:SiteLabel id="SiteLabel2" runat="server" ForControl="txtDefaultCacheDuration" CssClass="settinglabel" ConfigKey="ModuleDefinitionsDefaultCacheDurationLabel"></mp:SiteLabel>
        <asp:TextBox id="txtDefaultCacheDuration" runat="server" Columns="20" maxlength="8" CssClass="forminput"></asp:TextBox>
    </div>
    <div class="settingrow">
        <mp:SiteLabel id="Sitelabel6" runat="server" ForControl="chkIsSearchable" CssClass="settinglabel" ConfigKey="ModuleDefinitionsIsSearchableLabel"></mp:SiteLabel>
        <asp:CheckBox ID="chkIsSearchable" runat="server" CssClass="forminput"></asp:CheckBox>
    </div>
    <div class="settingrow">
        <mp:SiteLabel id="SiteLabel7" runat="server" ForControl="txtSearchListName" CssClass="settinglabel" ConfigKey="ModuleDefinitionsSearchListNameLabel"></mp:SiteLabel>
        <asp:TextBox id="txtSearchListName" runat="server" Columns="50" maxlength="255" CssClass="forminput"></asp:TextBox>
    </div>
    <div class="settingrow">
        <mp:SiteLabel id="Sitelabel1" runat="server" ForControl="chkIsAdmin" CssClass="settinglabel" ConfigKey="ModuleDefinitionsIsAdminLabel"></mp:SiteLabel>
        <asp:CheckBox ID="chkIsAdmin" runat="server" CssClass="forminput"></asp:CheckBox>
    </div>
<%--    <div  class="settingrow">
	    <mp:SiteLabel id="lblIcon" runat="server" ForControl="ddIcons" CssClass="settinglabel" ConfigKey="ModuleSettingsIconLabel" ></mp:SiteLabel>
	    <asp:DropDownList id="ddIcons" runat="server" DataValueField="Name" DataTextField="Name" CssClass="forminput"></asp:DropDownList>
	    <img id="imgIcon" alt="" src=""  runat="server" />
	</div>--%>
    <div class="settingrow">
        <asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="DefinitionSettings"></asp:ValidationSummary>
        <asp:RequiredFieldValidator id="reqFeatureName" runat="server" Display="None" ValidationGroup="DefinitionSettings" 
        ControlToValidate="txtFeatureName"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator id="reqControlSource" runat="server" Display="None" ValidationGroup="DefinitionSettings"
						ControlToValidate="txtControlSource"></asp:RequiredFieldValidator>
		<asp:RequiredFieldValidator id="reqSortOrder" runat="server" Display="None" ValidationGroup="DefinitionSettings"
						ControlToValidate="txtSortOrder"></asp:RequiredFieldValidator>
		<asp:RequiredFieldValidator id="reqDefaultCache" runat="server" Display="None" ValidationGroup="DefinitionSettings"
						ControlToValidate="txtDefaultCacheDuration"></asp:RequiredFieldValidator>
		<asp:RegularExpressionValidator ID="regexSortOrder" runat="server" Display="None" ValidationGroup="DefinitionSettings"
		    ControlToValidate="txtSortOrder" ValidationExpression="^[0-9][0-9]{0,4}$" />
		<asp:RegularExpressionValidator ID="regexCacheDuration" runat="server" Display="None" ValidationGroup="DefinitionSettings"
		    ControlToValidate="txtDefaultCacheDuration" ValidationExpression="^[0-9][0-9]{0,8}$" />
    </div>
    <div class="settingrow">
        <mp:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
        <portal:mojoButton  id="updateButton" runat="server" Text="" ValidationGroup="DefinitionSettings" />&nbsp;
	    <portal:mojoButton  id="cancelButton" runat="server" Text="" CausesValidation="false" />&nbsp;
	    <portal:mojoButton id="deleteButton" runat="server" Text="" CausesValidation="false" />&nbsp;
	    <asp:HyperLink ID="lnkConfigureSettings" runat="server" />
	    <portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="moduledefinitionedithelp" />	
    </div>
    <portal:mojoLabel ID="lblErrorMessage" runat="server" CssClass="txterror warning" /> 
</portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
