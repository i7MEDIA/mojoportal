<%@ Page CodeBehind="ModuleSettings.aspx.cs" MaintainScrollPositionOnPostback="true"
	Language="c#" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false"
	Inherits="mojoPortal.Web.AdminUI.ModuleSettingsPage" EnableEventValidation="false" %>

<%@ Register TagPrefix="portal" TagName="PublishType" Src="~/Controls/PublishTypeSetting.ascx" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portalAdmin:AdminDisplaySettings ID="displaySettings" runat="server" />
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">

		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin modulesettings">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server" SkinID="admin">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<div id="divtabs" class="mojo-tabs">
						<ul>
							<li class="selected"><a href="#tabFeatureSpecificSettings">
								<asp:Literal ID="litFeatureSpecificSettingsTab" runat="server" EnableViewState="false" /></a></li>
							<li id="liGeneralSettings" runat="server">
								<asp:Literal ID="litGeneralSettingsTabLink" runat="server" EnableViewState="false" /></li>
							<li id="liSecurity" runat="server">
								<asp:Literal ID="litSecurityLink" runat="server" /></li>
						</ul>
						<div id="tabFeatureSpecificSettings">
							<div class="row">
								<portal:FormGroupPanel ID="fgpCustomSettings" runat="server" CssClass="col-md-6">
									<asp:Literal ID="litCustomSettingsHeader" runat="server" />
									<asp:Panel ID="pnlcustomSettings" runat="server" />
								</portal:FormGroupPanel>
								<portal:FormGroupPanel ID="fgpGenericDisplaySettings" runat="server" CssClass="col-md-6">
									<asp:Literal ID="litGenericDisplaySettings" runat="server" />
									<div class="settingrow">
										<mp:SiteLabel ID="lblModuleName" runat="server" ForControl="moduleTitle" CssClass="settinglabel"
											ConfigKey="ModuleSettingsModuleNameLabel" />
										<asp:TextBox ID="moduleTitle" runat="server" EnableViewState="false" CssClass="forminput widetextbox" />
									</div>
									<div class="settingrow">
										<mp:SiteLabel ID="lblShowTitle" runat="server" ForControl="chkShowTitle" CssClass="settinglabel" ConfigKey="ModuleSettingsShowTitleLabel">
										</mp:SiteLabel>
										<asp:CheckBox ID="chkShowTitle" runat="server" EnableViewState="false" CssClass="forminput"></asp:CheckBox>
									</div>
									<div id="divTitleElement" runat="server" class="settingrow">
										<mp:SiteLabel ID="SiteLabel14" runat="server" ForControl="txtTitleElement" CssClass="settinglabel" ConfigKey="ModuleSettingsTitleElement" />
										<asp:DropDownList runat="server" ID="ddlTitleElements" />
									</div>
									<div id="divStyleSets" runat="server" class="settingrow">
										<mp:SiteLabel runat="server" ForControl="styleSetList" CssClass="settinglabel" ConfigKey="StyleSets" />
										<portalAdmin:StyleSetsControl ID="styleSetList" runat="server" />
									</div>
								</portal:FormGroupPanel>
							</div>
						</div>
						<div id="tabGeneralSettings" runat="server">
							<div class="settingrow">
								<mp:SiteLabel ID="SiteLabel5" runat="server" CssClass="settinglabel" ConfigKey="ModuleSettingsFeatureNameLabel"
									UseLabelTag="false">
								</mp:SiteLabel>
								<asp:Label ID="lblFeatureName" runat="server" EnableViewState="false" CssClass="forminput" />
							</div>
							<div class="settingrow">
								<mp:SiteLabel ID="SiteLabel9" runat="server" CssClass="settinglabel" ConfigKey="InstanceIdWithGuid"
									UseLabelTag="false">
								</mp:SiteLabel>
								<asp:Label ID="lblModuleId" runat="server" EnableViewState="false" CssClass="forminput" />
								<asp:Label ID="lblModuleGuid" runat="server" EnableViewState="false" CssClass="forminput instanceguid" />
							</div>
							<div class="settingrow" id="divParentPage" runat="server" visible="false">
								<mp:SiteLabel ID="lblParentPage" runat="server" ForControl="ddPages" CssClass="settinglabel"
									ConfigKey="PageLayoutParentPageLabel">
								</mp:SiteLabel>
								<asp:DropDownList ID="ddPages" runat="server" EnableTheming="false" DataTextField="PageName"
									DataValueField="PageID" CssClass="forminput">
								</asp:DropDownList>
							</div>

							<div id="divCacheTimeout" runat="server" class="settingrow">
								<mp:SiteLabel ID="lblCacheTime" runat="server" ForControl="cacheTime" CssClass="settinglabel"
									ConfigKey="ModuleSettingsCacheTimeLabel" />
								<asp:TextBox ID="cacheTime" runat="server" MaxLength="8" Text="0" EnableViewState="false"
									CssClass="forminput smalltextbox"></asp:TextBox>
							</div>

							<div id="divIncludeInSearch" runat="server" visible="false" class="settingrow">
								<mp:SiteLabel ID="SiteLabel12" runat="server" ForControl="chkIncludeInSearch" CssClass="settinglabel"
									ConfigKey="IncludeInSearchSetting">
								</mp:SiteLabel>
								<asp:CheckBox ID="chkIncludeInSearch" runat="server" Checked="true" EnableViewState="false" CssClass="forminput"></asp:CheckBox>
							</div>
						</div>
						<div id="tabSecurity" runat="server">
							<div id="divIsGlobal" runat="server" visible="false" class="settingrow">
								<mp:SiteLabel ID="SiteLabel13" runat="server" ForControl="chkIsGlobal" CssClass="settinglabel"
									ConfigKey="ModuleSettingsIsGlobal">
								</mp:SiteLabel>
								<asp:CheckBox ID="chkIsGlobal" runat="server" EnableViewState="false" CssClass="forminput"></asp:CheckBox>
								<portal:mojoHelpLink ID="MojoHelpLink2" runat="server" HelpKey="modulesettings-isglobal-help" />
							</div>
							<div class="settingrow">
								<mp:SiteLabel ID="SiteLabel6" runat="server" ForControl="chkHideFromAuth" CssClass="settinglabel"
									ConfigKey="ModuleSettingsHideFromAuthenticatedLabel">
								</mp:SiteLabel>
								<asp:CheckBox ID="chkHideFromAuth" runat="server" EnableViewState="false" CssClass="forminput"></asp:CheckBox>
							</div>
							<div class="settingrow">
								<mp:SiteLabel ID="SiteLabel7" runat="server" ForControl="chkHideFromUnauth" CssClass="settinglabel"
									ConfigKey="ModuleSettingsHideFromUnauthenticatedLabel">
								</mp:SiteLabel>
								<asp:CheckBox ID="chkHideFromUnauth" runat="server" EnableViewState="false" CssClass="forminput"></asp:CheckBox>
							</div>
							<div id="divRoles" runat="server">
								<div class="panel panel-primary">
									<div class="panel-heading">

										<mp:SiteLabel ID="lblAuthorizedRoles" runat="server" ConfigKey="ModuleSettingsViewRolesLabel" UseLabelTag="false" />

									</div>
									<div id="divViewRoles" runat="server" class="panel-body">
										<div class="settingrow">
											<asp:RadioButton ID="rbViewAdminOnly" runat="server" GroupName="rdoviewroles" CssClass="rbroles rbadminonly" />
										</div>
										<div class="settingrow">
											<asp:RadioButton ID="rbViewUseRoles" runat="server" GroupName="rdoviewroles" CssClass="rbroles" />
										</div>
										<p>
											<asp:CheckBoxList ID="cblViewRoles" runat="server"></asp:CheckBoxList>
										</p>
									</div>
									<div class="panel-footer">
										Users in these roles will be able to view this feature's content.
									</div>
								</div>
								<div class="panel panel-warning">
									<div class="panel-heading">

												<mp:SiteLabel ID="SiteLabel8" runat="server" ConfigKey="ModuleSettingsEditRolesLabel"
													UseLabelTag="false" />

									</div>
									
									<div id="div1" runat="server" class="panel-body">
										<div class="settingrow">
											<asp:RadioButton ID="rbEditAdminsOnly" runat="server" GroupName="rdoeditroles" CssClass="rbroles rbadminonly" />
										</div>
										<div class="settingrow">
											<asp:RadioButton ID="rbEditUseRoles" runat="server" GroupName="rdoeditroles" CssClass="rbroles" />
										</div>
										<p>
											<asp:CheckBoxList ID="authEditRoles" runat="server"></asp:CheckBoxList>
										</p>
									</div>
																		<div class="panel-footer">
										Users in these roles will be able to edit this feature's content.
									</div>
								</div>
								<div id="divDraftEditRoles" runat="server" class="panel panel-default">
									<div class="panel-heading">
												<mp:SiteLabel ID="SiteLabel16" runat="server" ConfigKey="ModuleSettingsDraftEditRolesLabel"
													UseLabelTag="false" />
									</div>
									<div class="panel-body">
										<p>
											<asp:CheckBoxList ID="draftEditRoles" runat="server"></asp:CheckBoxList>
										</p>
									</div>
								</div>
								<div id="divDraftApprovalRoles" runat="server" class="panel panel-default">
									<div class="panel-heading">
												<mp:SiteLabel ID="lblDraftApprovalRoles" runat="server" ConfigKey="ModuleSettingsDraftApprovalRolesLabel"
													UseLabelTag="false" />
									</div>
									<div class="panel-body">
										<p>
											<asp:CheckBoxList ID="draftApprovalRoles" runat="server"></asp:CheckBoxList>
										</p>
									</div>
								</div>
							</div>
							<div id="divRoleLinks" runat="server" visible="false" enableviewstate="false" class="panel panel-default">
								<div class="panel-body">
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
							</div>
							<div id="divEditUser" runat="server" class="specificeditor panel panel-default">
								<div class="panel-heading">
									<mp:SiteLabel ID="Sitelabel1" runat="server" ForControl="scUser" CssClass="settinglabel" ConfigKey="ModuleSettingsEditUserLabel" />
								</div>
								<div class="panel-body">
									<portal:jQueryAutoCompleteTextBox ID="acUser" runat="server" CssClass="forminput mediumtextbox" SkinID="modulesecurity" />
									<asp:TextBox ID="txtEditUserId" runat="server" CssClass="smalltextbox" />
									<portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="modulesettings-user-that-can-edit-help" />
								</div>
									<div class="panel-footer">
										This user will be able to edit this feature's content, regardless of the roles selected above.
									</div>
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

		</portal:InnerWrapperPanel>

	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
