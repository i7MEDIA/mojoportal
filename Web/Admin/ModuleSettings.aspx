<%@ Page CodeBehind="ModuleSettings.aspx.cs" MaintainScrollPositionOnPostback="true"
    Language="c#" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false"
    Inherits="mojoPortal.Web.AdminUI.ModuleSettingsPage" EnableEventValidation="false" %>
    <%@ Register TagPrefix="portal" TagName="PublishType" Src="~/Controls/PublishTypeSetting.ascx" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portalAdmin:AdminDisplaySettings ID="displaySettings" runat="server" />
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin modulesettings">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server" SkinID="admin">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    <div id="divtabs" class="mojo-tabs">
        <ul>
            <li class="selected"><a href="#tabFeatureSpecificSettings">
                <asp:Literal ID="litFeatureSpecificSettingsTab" runat="server" EnableViewState="false" /></a></li>
            <li id="liGeneralSettings" runat="server"><asp:Literal ID="litGeneralSettingsTabLink" runat="server" EnableViewState="false" /></li>
            <li id="liSecurity" runat="server"><asp:Literal ID="litSecurityLink" runat="server" /></li>
        </ul>
        <div id="tabFeatureSpecificSettings">
            <div class="settingrow" id="divWebParts" runat="server" visible="false">
                <mp:SiteLabel ID="SiteLabel4" runat="server" ForControl="ddWebParts" CssClass="settinglabel"
                    ConfigKey="WebPartModuleWebPartSetting" EnableViewState="false"></mp:SiteLabel>
                <asp:DropDownList ID="ddWebParts" runat="server" DataValueField="WebPartID" DataTextField="ClassName">
                </asp:DropDownList>
            </div>
            <asp:Panel ID="pnlcustomSettings" runat="server"></asp:Panel>
<%--            <div class="settingrow">
                <mp:SiteLabel ID="SiteLabel10" runat="server" CssClass="settinglabel" ConfigKey="spacer"
                    UseLabelTag="false"></mp:SiteLabel>
            </div>--%>
        </div>
        <div id="tabGeneralSettings" runat="server">
            <div class="settingrow">
                <mp:SiteLabel ID="SiteLabel5" runat="server" CssClass="settinglabel" ConfigKey="ModuleSettingsFeatureNameLabel"
                    UseLabelTag="false"></mp:SiteLabel>
                <asp:Label ID="lblFeatureName" runat="server" EnableViewState="false" CssClass="forminput" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="SiteLabel9" runat="server" CssClass="settinglabel" ConfigKey="InstanceIdWithGuid"
                    UseLabelTag="false"></mp:SiteLabel>
                <asp:Label ID="lblModuleId" runat="server" EnableViewState="false" CssClass="forminput" />
                <asp:Label ID="lblModuleGuid" runat="server" EnableViewState="false" CssClass="forminput instanceguid" />
            </div>
            <div class="settingrow" id="divParentPage" runat="server" visible="false">
                <mp:SiteLabel ID="lblParentPage" runat="server" ForControl="ddPages" CssClass="settinglabel"
                    ConfigKey="PageLayoutParentPageLabel"></mp:SiteLabel>
                <asp:DropDownList ID="ddPages" runat="server" EnableTheming="false" DataTextField="PageName"
                    DataValueField="PageID" CssClass="forminput">
                </asp:DropDownList>
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="lblModuleName" runat="server" ForControl="moduleTitle" CssClass="settinglabel"
                    ConfigKey="ModuleSettingsModuleNameLabel"></mp:SiteLabel>
                <asp:TextBox ID="moduleTitle" runat="server"  EnableViewState="false" CssClass="forminput widetextbox"></asp:TextBox>
            </div>
            <div id="divCacheTimeout" runat="server" class="settingrow">
                <mp:SiteLabel ID="lblCacheTime" runat="server" ForControl="cacheTime" CssClass="settinglabel"
                    ConfigKey="ModuleSettingsCacheTimeLabel" />
                <asp:TextBox ID="cacheTime" runat="server"  MaxLength="8" Text="0" EnableViewState="false"
                    CssClass="forminput smalltextbox"></asp:TextBox>
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="lblShowTitle" runat="server" ForControl="chkShowTitle" CssClass="settinglabel"
                    ConfigKey="ModuleSettingsShowTitleLabel"></mp:SiteLabel>
                <asp:CheckBox ID="chkShowTitle" runat="server" EnableViewState="false" CssClass="forminput">
                </asp:CheckBox>
            </div>
            <div id="divTitleElement" runat="server" class="settingrow">
                <mp:SiteLabel ID="SiteLabel14" runat="server" ForControl="txtTitleElement" CssClass="settinglabel"
                    ConfigKey="ModuleSettingsTitleElement"></mp:SiteLabel>
                <asp:TextBox ID="txtTitleElement" runat="server"  EnableViewState="false" CssClass="forminput smalltextbox"></asp:TextBox>
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="SiteLabel15" runat="server" CssClass="settinglabel" ConfigKey="PublishMode"></mp:SiteLabel>
                <portal:PublishType ID="publishType" runat="server" />
                <portal:mojoHelpLink ID="MojoHelpLink38" runat="server" HelpKey="module-settings-publish-mode-help" />
            </div>
            <div id="divIncludeInSearch" runat="server" visible="false" class="settingrow">
                <mp:SiteLabel ID="SiteLabel12" runat="server" ForControl="chkIncludeInSearch" CssClass="settinglabel"
                    ConfigKey="IncludeInSearchSetting"></mp:SiteLabel>
                <asp:CheckBox ID="chkIncludeInSearch" runat="server" Checked="true" EnableViewState="false" CssClass="forminput">
                </asp:CheckBox>
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="SiteLabel6" runat="server" ForControl="chkHideFromAuth" CssClass="settinglabel"
                    ConfigKey="ModuleSettingsHideFromAuthenticatedLabel"></mp:SiteLabel>
                <asp:CheckBox ID="chkHideFromAuth" runat="server" EnableViewState="false" CssClass="forminput">
                </asp:CheckBox>
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="SiteLabel7" runat="server" ForControl="chkHideFromUnauth" CssClass="settinglabel"
                    ConfigKey="ModuleSettingsHideFromUnauthenticatedLabel"></mp:SiteLabel>
                <asp:CheckBox ID="chkHideFromUnauth" runat="server" EnableViewState="false" CssClass="forminput">
                </asp:CheckBox>
            </div>
                       
            <div id="divMyPage" runat="server" class="settingrow">
                <mp:SiteLabel ID="SiteLabel2" runat="server" ForControl="chkAvailableForMyPage" CssClass="settinglabel"
                    ConfigKey="ModuleSettingsAvailableForMyPageLabel"></mp:SiteLabel>
                <asp:CheckBox ID="chkAvailableForMyPage" runat="server" EnableViewState="false" CssClass="forminput">
                </asp:CheckBox>
            </div>
            <div id="divMyPageMulti" runat="server" class="settingrow">
                <mp:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel" ForControl="chkAllowMultipleInstancesOnMyPage"
                    ConfigKey="ModuleSettingsAllowMultipleInstancesOnMyPageLabel"></mp:SiteLabel>
                <asp:CheckBox ID="chkAllowMultipleInstancesOnMyPage" runat="server" EnableViewState="false"
                    CssClass="forminput"></asp:CheckBox>
            </div>
<%--            <div class="settingrow moduleIconsList">
                <mp:SiteLabel ID="lblIcon" runat="server" ForControl="ddIcons" CssClass="settinglabel"
                    ConfigKey="ModuleSettingsIconLabel"></mp:SiteLabel>
                <asp:DropDownList ID="ddIcons" runat="server" EnableTheming="false" DataValueField="Name"
                    DataTextField="Name" CssClass="forminput">
                </asp:DropDownList>
                <img id="imgIcon" alt="" src="" runat="server" />
            </div>--%>
<%--            <div class="settingrow">
                <mp:SiteLabel ID="SiteLabel11" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                </mp:SiteLabel>
            </div>--%>
        </div>
        <div id="tabSecurity" runat="server">
                <div id="divIsGlobal" runat="server" visible="false" class="settingrow">
                <mp:SiteLabel ID="SiteLabel13" runat="server" ForControl="chkIsGlobal" CssClass="settinglabel"
                    ConfigKey="ModuleSettingsIsGlobal"></mp:SiteLabel>
                <asp:CheckBox ID="chkIsGlobal" runat="server" EnableViewState="false" CssClass="forminput">
                </asp:CheckBox>
                <portal:mojoHelpLink ID="MojoHelpLink2" runat="server" HelpKey="modulesettings-isglobal-help" />
            </div>
            <div id="divRoles" runat="server">
            <div class="mojo-accordion">
                <h3 id="h3ViewRoles" runat="server">
                    <a href="#">
                        <mp:SiteLabel ID="lblAuthorizedRoles" runat="server" ConfigKey="ModuleSettingsViewRolesLabel"
                            UseLabelTag="false" />
                    </a>
                </h3>
                <div id="divViewRoles" runat="server">
                    <div class="settingrow">
                        <asp:RadioButton ID="rbViewAdminOnly" runat="server"  GroupName="rdoviewroles" CssClass="rbroles rbadminonly" />
                    </div>
                    <div class="settingrow">
                        <asp:RadioButton ID="rbViewUseRoles" runat="server"  GroupName="rdoviewroles" CssClass="rbroles" />
                    </div>
                    <p>
                            <asp:CheckBoxList ID="cblViewRoles" runat="server"></asp:CheckBoxList>
                    </p>
                </div>
                <h3 id="h1" runat="server">
                    <a href="#">
                        <mp:SiteLabel ID="SiteLabel8" runat="server" ConfigKey="ModuleSettingsEditRolesLabel"
                            UseLabelTag="false" />
                    </a>
                </h3>
                <div id="div1" runat="server">
                    <div class="settingrow">
                        <asp:RadioButton ID="rbEditAdminsOnly" runat="server"  GroupName="rdoeditroles" CssClass="rbroles rbadminonly" />
                    </div>
                    <div class="settingrow">
                        <asp:RadioButton ID="rbEditUseRoles" runat="server"  GroupName="rdoeditroles" CssClass="rbroles" />
                    </div>
                    <p>
                            <asp:CheckBoxList ID="authEditRoles" runat="server"></asp:CheckBoxList>
                    </p>
                </div>
                <h3 id="h2DraftEditRoles" runat="server">
                    <a href="#">
                        <mp:SiteLabel ID="SiteLabel16" runat="server" ConfigKey="ModuleSettingsDraftEditRolesLabel"
                            UseLabelTag="false" />
                    </a>
                </h3>
                <div id="divDraftEditRoles" runat="server">
 
                    <p>
                            <asp:CheckBoxList ID="draftEditRoles" runat="server"></asp:CheckBoxList>
                    </p>
                </div>
                <h3 id="h2DraftApprovalRoles" runat="server">
                    <a href="#">
                        <mp:SiteLabel ID="lblDraftApprovalRoles" runat="server" ConfigKey="ModuleSettingsDraftApprovalRolesLabel"
                            UseLabelTag="false" />
                    </a>
                </h3>
                <div id="divDraftApprovalRoles" runat="server">
 
                    <p>
                            <asp:CheckBoxList ID="draftApprovalRoles" runat="server"></asp:CheckBoxList>
                    </p>
                </div>
            </div>
            </div>
            <div id="divRoleLinks" runat="server" visible="false" enableviewstate="false">
            <ul class="simplelist">
	            <li>
		            <asp:HyperLink ID="lnkPageViewRoles" runat="server" CssClass="lnkPageViewRoles" EnableViewState="false" />
	            </li>
                <li>
		            <asp:HyperLink ID="lnkPageEditRoles" runat="server" CssClass="lnkPageEditRoles" EnableViewState="false" />
	            </li>
                <li>
		            <asp:HyperLink ID="lnkPageDraftRoles" runat="server" CssClass="lnkPageDraftRoles" EnableViewState="false" />
	            </li>
               <li id="liApproverRoles" runat="server">
		            <asp:HyperLink ID="lnkPageApprovalRoles" runat="server" CssClass="lnkPageApprovalRoles" EnableViewState="false" />
	            </li>
            </ul>
            </div>
            <div id="divEditUser" runat="server" class="settingrow specificeditor">
                <mp:SiteLabel ID="Sitelabel1" runat="server" ForControl="scUser" CssClass="settinglabel"
                    ConfigKey="ModuleSettingsEditUserLabel"></mp:SiteLabel>
                <portal:jQueryAutoCompleteTextBox id="acUser" runat="server" CssClass="forminput mediumtextbox" SkinID="modulesecurity" />
                <asp:TextBox ID="txtEditUserId" runat="server" CssClass="smalltextbox" />
                <portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="modulesettings-user-that-can-edit-help" />
            </div>
        </div>
    </div>
    <div class="modulecontent">
        <div class="settingrow">
            <portal:mojoLabel ID="lblValidationSummary" runat="server" CssClass="txterror errors" EnableViewState="false" />
            <asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="ModuleSettings"
                EnableViewState="false"></asp:ValidationSummary>
            <asp:RequiredFieldValidator ID="reqCacheTime" runat="server" Display="None" ValidationGroup="ModuleSettings"
                ControlToValidate="cacheTime" Enabled="false" EnableViewState="false"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="regexCacheTime" runat="server" Display="None"
                ValidationGroup="ModuleSettings" ControlToValidate="cacheTime" ValidationExpression="^[0-9][0-9]{0,8}$"
                EnableViewState="false" />
        </div>
        <div class="settingrow btn-row text-center">
            <portal:mojoButton ID="btnSave" runat="server" ValidationGroup="ModuleSettings" SkinID="SuccessButton" />
            <portal:mojoButton ID="btnDelete" runat="server" CausesValidation="false" SkinID="DeleteButton" />
            <asp:HyperLink ID="lnkCancel" runat="server" SkinID="TextButton" />
            <asp:HyperLink ID="lnkEditContent" runat="server" Visible="false" EnableViewState="false" SkinID="TextButtonSmall" />
            <asp:HyperLink ID="lnkPublishing" runat="server" Visible="false" EnableViewState="false" SkinID="TextButtonSmall" />
        </div>
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
