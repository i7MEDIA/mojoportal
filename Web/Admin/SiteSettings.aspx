<%@ Page Language="C#" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SiteSettings.aspx.cs" Inherits="mojoPortal.Web.AdminUI.SiteSettingsPage" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portalAdmin:AdminDisplaySettings ID="adminDisplaySettings" runat="server" />
	<portal:CoreDisplaySettings ID="displaySettings" runat="server" />
	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" />
		<portal:AdminCrumbSeparator ID="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkSiteList" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" 
			CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator ID="litLinkSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkSiteSettings" runat="server" CssClass="selectedcrumb" />
	</portal:AdminCrumbContainer>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin sitesettings">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server" SkinID="admin">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<portal:BasePanel ID="pnlSiteSettings" runat="server" DefaultButton="btnSave" RenderId="false">
						<div id="divtabs" class="mojo-tabs">
							<ul>
								<li id="liGeneral" runat="server">
									<a href="#tabSettings">
										<asp:Literal ID="litSettingsTab" runat="server" EnableViewState="false" />
									</a>
								</li>
								<li id="liSecurity" runat="server" enableviewstate="false">
									<asp:Literal ID="litSecurityTabLink" runat="server" EnableViewState="false" />
								</li>
								<li>
									<a href="#tabCompanyInfo">
										<asp:Literal ID="litCompanyInfoTab" runat="server" EnableViewState="false" />
									</a>
								</li>
								<li id="liCommerce" runat="server" enableviewstate="false">
									<asp:Literal ID="litCommerceTabLink" runat="server" EnableViewState="false" />
								</li>
								<li id="liSiteMappings" runat="server" enableviewstate="false">
									<asp:Literal ID="litSiteMappingsTabLink" runat="server" EnableViewState="false" />
								</li>
								<li id="liFeatures" runat="server" visible="false" enableviewstate="false">
									<asp:Literal ID="litFeaturesTabLink" runat="server" EnableViewState="false" />
								</li>
								<li>
									<a href="#tabApiKeys">
										<asp:Literal ID="litAPIKeysTab" runat="server" EnableViewState="false" />
									</a>
								</li>
								<li id="liMailSettings" runat="server" enableviewstate="false">
									<asp:Literal ID="litMailSettingsTabLink" runat="server" EnableViewState="false" />
								</li>
								<li id="liContent" runat="server" enableviewstate="false">
									<asp:Literal ID="litContentTabLink" runat="server" EnableViewState="false" />
								</li>
								<li id="liAdvanced" runat="server" enableviewstate="false">
									<asp:Literal ID="litAdvancedTabLink" runat="server" EnableViewState="false" />
								</li>

							</ul>

							<div id="tabSettings">
								<portal:FormGroupPanel ID="fgpMainSettings" runat="server" SkinID="MainSettings">
									<asp:Literal ID="litMainSettingsHeader" runat="server" EnableViewState="false" />
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel ID="lblSiteTitle" ForControl="txtSiteName" runat="server" CssClass="settinglabel" ConfigKey="SiteSettingsSiteTitleLabel" />
										<asp:TextBox ID="txtSiteName" TabIndex="10" runat="server" CssClass="forminput widetextbox" />
										<%--<portal:Link runat="server" ID="linkNewSite" CssClass="newsitelink" SkinId="NewSiteLink" />--%>
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server" ID="fgpSiteId" Visible="false">
										<mp:SiteLabel runat="server" CssClass="settinglabel" ConfigKey="SiteSettingsSiteIDLabel" EnableViewState="false"></mp:SiteLabel>
										<asp:Label ID="lblSiteId" runat="server" />/<asp:Label ID="lblSiteGuid" runat="server" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server" ID="fgpSiteIsClosed">
										<mp:SiteLabel runat="server" ForControl="chkSiteIsClosed" CssClass="settinglabel" ConfigKey="SiteIsClosed" />
										<asp:CheckBox ID="chkSiteIsClosed" runat="server" CssClass="forminput" />
										<portal:mojoHelpLink runat="server" HelpKey="sitesettings-siteisclosed-help" />
										<asp:HyperLink ID="lnkEditClosedMessage" runat="server" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server" ID="fgpTimeZone" Visible="false">
										<mp:SiteLabel runat="server" CssClass="settinglabel" ConfigKey="TimeZone" />
										<portal:TimeZoneIdSetting ID="timeZone" runat="server" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server" ID="fgpSSL">
										<mp:SiteLabel runat="server" ForControl="chkRequireSSL" CssClass="settinglabel" ConfigKey="SiteSettingsRequireSSLLabel" />
										<asp:CheckBox ID="chkRequireSSL" runat="server" TabIndex="10" CssClass="forminput" />
										<portal:mojoHelpLink runat="server" HelpKey="sitesettingsrequiresslhelp" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server" ID="fgpContentVersioning">
										<mp:SiteLabel runat="server" ForControl="chkForceContentVersioning" CssClass="settinglabel" ConfigKey="ForceContentVersioning" />
										<asp:CheckBox ID="chkForceContentVersioning" runat="server" TabIndex="10" CssClass="forminput" /><mp:SiteLabel
											runat="server" />
										<portal:mojoHelpLink runat="server" HelpKey="sitesettingsforcecontentversioninghelp" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server" ID="fgpApprovalsWorkflow">
										<mp:SiteLabel runat="server" ForControl="chkEnableContentWorkflow" CssClass="settinglabel" ConfigKey="EnableContentWorkflow" />
										<asp:CheckBox ID="chkEnableContentWorkflow" runat="server" TabIndex="10" CssClass="forminput" />
										<mp:SiteLabel runat="server" />
										<portal:mojoHelpLink runat="server" HelpKey="sitesettingsenablecontentworkflowhelp" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" ForControl="txtPrivacyPolicyUrl" CssClass="settinglabel" ConfigKey="SiteSettingsPrivacyUrlLabel" />
										<asp:Label runat="server" ID="lblPrivacySiteRoot" SkinID="PrivacySiteRoot" />
										<asp:TextBox runat="server" ID="txtPrivacyPolicyUrl" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox" SkinID="PrivaryPolicyUrl" />
										<portal:mojoHelpLink runat="server" HelpKey="sitesettingsprivacyhelp" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" ForControl="txtOpenSearchName" CssClass="settinglabel" ConfigKey="SiteSettingsOpenSearchNameLabel" />
										<asp:TextBox runat="server" ID="txtOpenSearchName" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox" />
										<portal:mojoHelpLink runat="server" HelpKey="sitesettings-opensearchname-help" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" ForControl="txtMetaProfile" CssClass="settinglabel" ConfigKey="MetaProfileLabel" />
										<asp:TextBox ID="txtMetaProfile" TabIndex="10" CssClass="forminput verywidetextbox"
											runat="server" />
										<portal:mojoHelpLink runat="server" HelpKey="meta-profile-help" />
									</portal:FormGroupPanel>
								</portal:FormGroupPanel>

								<portal:FormGroupPanel ID="fgpSkinSettings" runat="server" SkinID="SkinSettings">
									<asp:Literal ID="litSkinSettingsHeader" runat="server" EnableViewState="false" />
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel ID="lblSkin" ForControl="ddSkins" runat="server" CssClass="settinglabel" ConfigKey="SiteSettingsSiteSkinLabel" />
										<portal:SkinList ID="SkinSetting" runat="server" />
										<portal:mojoHelpLink runat="server" HelpKey="sitesettingssiteskinhelp" />
										<portal:mojoButton ID="btnRestoreSkins" runat="server" Visible="false" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" CssClass="settinglabel" ConfigKey="MobileSkin"></mp:SiteLabel>
										<asp:DropDownList ID="ddMobileSkin" runat="server" DataValueField="Name" DataTextField="Name" CssClass="forminput skinlist" TabIndex="10">
										</asp:DropDownList>
										<portal:mojoHelpLink runat="server" HelpKey="mobile-skin-help" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server" ID="fgpAllowPageSkins">
										<mp:SiteLabel runat="server" ForControl="chkAllowPageSkins" CssClass="settinglabel" ConfigKey="SiteSettingsAllowPageSkinsLabel" />
										<asp:CheckBox ID="chkAllowPageSkins" runat="server" TabIndex="10" CssClass="forminput" />
										<portal:mojoHelpLink runat="server" HelpKey="sitesettingspageskinhelp" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server" ID="fgpAllowUserSkins">
										<mp:SiteLabel runat="server" ForControl="chkAllowUserSkins" CssClass="settinglabel" ConfigKey="SiteSettingsAllowUserSkinsLabel" />
										<asp:CheckBox ID="chkAllowUserSkins" runat="server" TabIndex="10" CssClass="forminput" />
										<portal:mojoHelpLink runat="server" HelpKey="sitesettingsuserskinhelp" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server" ID="fgpAllowHideMenu">
										<mp:SiteLabel ID="Sitelabel2y" runat="server" ForControl="chkAllowHideMenuOnPages" CssClass="settinglabel" ConfigKey="SiteSettingsAllowHideMainMenuLabel" />
										<asp:CheckBox ID="chkAllowHideMenuOnPages" runat="server" TabIndex="10" CssClass="forminput" />
										<portal:mojoHelpLink runat="server" HelpKey="sitesettingsallowhidemenuhelp" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server" CssClass="logolist">
										<mp:SiteLabel ID="lblLogo" ForControl="ddLogos" runat="server" CssClass="settinglabel" ConfigKey="SiteSettingsSiteLogoLabel" />
										<asp:DropDownList ID="ddLogos" runat="server" TabIndex="10" EnableViewState="true"
											DataValueField="Name" DataTextField="Name" CssClass="forminput">
										</asp:DropDownList>
										<portal:mojoHelpLink runat="server" HelpKey="sitesettingssitelogohelp" />
										<img runat="server" alt="" src="" id="imgLogo" enableviewstate="false" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" ForControl="txtSlogan" CssClass="settinglabel" ConfigKey="SloganLabel" />
										<asp:TextBox ID="txtSlogan" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox" />
										<portal:mojoHelpLink runat="server" HelpKey="site-slogan-help" />
										<mp:SiteLabel runat="server" CssClass="help-block" ConfigKey="SloganLabelDescription" UseLabelTag="false" />
									</portal:FormGroupPanel>
								</portal:FormGroupPanel>
								<portal:FormGroupPanel ID="fgpEditorSettings" runat="server" SkinID="EditorSettings">
									<asp:Literal ID="litContentEditorSettingsHeader" runat="server" EnableViewState="false" />
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" ForControl="ddEditorProviders" CssClass="settinglabel" ConfigKey="SiteSettingsEditorProviderLabel" EnableViewState="false"></mp:SiteLabel>
										<asp:DropDownList ID="ddEditorProviders" DataTextField="name" DataValueField="name"
											EnableViewState="true" TabIndex="10" runat="server" CssClass="forminput">
										</asp:DropDownList>
										<portal:mojoHelpLink runat="server" HelpKey="sitesettingssiteeditorproviderhelp" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" ForControl="ddNewsletterEditor" CssClass="settinglabel" ConfigKey="NewsletterEditorLabel" />
										<asp:DropDownList ID="ddNewsletterEditor" DataTextField="name" DataValueField="name"
											EnableViewState="true" TabIndex="10" runat="server" CssClass="forminput">
										</asp:DropDownList>
										<portal:mojoHelpLink runat="server" HelpKey="newletter-editor-help" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" ForControl="chkAllowUserEditorChoice" CssClass="settinglabel" ConfigKey="AllowUserEditorLabel" />
										<asp:CheckBox ID="chkAllowUserEditorChoice" runat="server" TabIndex="10" CssClass="forminput" />
										<portal:mojoHelpLink runat="server" HelpKey="sitesetting-user-editor-help" />
									</portal:FormGroupPanel>
								</portal:FormGroupPanel>
								
							</div>

							<div id="tabSecurity" runat="server">
								<div id="divSecurityTabs" class="mojo-tabs">
									<ul>
										<li class="selected" id="liGeneralSecurity" runat="server" enableviewstate="false">
											<asp:Literal ID="litGeneralSecurityTabLink" runat="server" EnableViewState="false" />
										</li>
										<li id="liLDAP" runat="server" enableviewstate="false">
											<asp:Literal ID="litLDAPTabLink" runat="server" EnableViewState="false" />
										</li>
										<li id="lithirdpartyauth" runat="server" enableviewstate="false">
											<asp:Literal ID="litthirdpartyauthtabLink" runat="server" EnableViewState="false" />
										</li>
										<%--										<li id="liWindowsLive" runat="server" enableviewstate="false">
											<asp:Literal ID="litWindowsLiveTabLink" runat="server" EnableViewState="false" />
										</li>--%>
										<li id="liCaptcha" runat="server"><a href="#tabAntiSpam">
											<asp:Literal ID="litAntiSpamTab" runat="server" EnableViewState="false" /></a>
										</li>
									</ul>

									<div id="tabGeneralSecurity" runat="server">
										<portal:FormGroupPanel runat="server" ID="fgpRegistrationOptions" SkinID="RegistrationSettings">
											<asp:Literal ID="litRegistrationSettingsHeader" runat="server" EnableViewState="false" />
											<portal:FormGroupPanel runat="server" ID="fgpAllowRegistration">
												<mp:SiteLabel runat="server" ForControl="chkAllowRegistration" CssClass="settinglabel" ConfigKey="SiteSettingsAllowRegistrationLabel" />
												<asp:CheckBox ID="chkAllowRegistration" runat="server" TabIndex="10" CssClass="forminput" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingsallowregistrationhelp" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel runat="server" ForControl="chkRequireEmailTwice" CssClass="settinglabel" ConfigKey="RequireEmailTwiceOnRegistration" />
												<asp:CheckBox ID="chkRequireEmailTwice" runat="server" TabIndex="10" CssClass="forminput" />
												<portal:mojoHelpLink runat="server" HelpKey="RequireEmailTwice-help" />
												<mp:SiteLabel runat="server" CssClass="help-block" UseLabelTag="false" ConfigKey="RequireEmailTwiceOnRegistrationDescription" />

											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server" ID="fgpSecureRegistration">
												<mp:SiteLabel runat="server" ForControl="chkSecureRegistration" CssClass="settinglabel" ConfigKey="SiteSettingsSecureRegistrationLabel" />
												<asp:CheckBox ID="chkSecureRegistration" runat="server" TabIndex="10" CssClass="forminput" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingssecureregistrationhelp" />
												<mp:SiteLabel runat="server" CssClass="help-block" UseLabelTag="false" ConfigKey="SiteSettingsSecureRegistrationDescription" />
												<portal:FormGroupPanel runat="server" ID="fgpShowPasswordStrength">
													<mp:SiteLabel runat="server" ForControl="chkShowPasswordStrength" CssClass="settinglabel" ConfigKey="ShowPasswordStrengthOnRegistrationPage" />
													<asp:CheckBox ID="chkShowPasswordStrength" runat="server" TabIndex="10" CssClass="forminput" />
													<portal:mojoHelpLink runat="server" HelpKey="ShowPasswordStrengthOnRegistrationPage-help" />
												</portal:FormGroupPanel>
												<portal:FormGroupPanel runat="server">
													<mp:SiteLabel runat="server" ForControl="chkRequireCaptcha" CssClass="settinglabel" ConfigKey="RequireCaptchaOnRegistrationPage" />
													<asp:CheckBox ID="chkRequireCaptcha" runat="server" TabIndex="10" CssClass="forminput" />
													<portal:mojoHelpLink runat="server" HelpKey="RequireCaptchaOnRegistrationPage-help" />
												</portal:FormGroupPanel>
											</portal:FormGroupPanel>
											<asp:UpdatePanel runat="server" ID="upLoginApproval" UpdateMode="Conditional" EnableViewState="true">
												<ContentTemplate>
													<portal:FormGroupPanel runat="server">
														<mp:SiteLabel runat="server" ForControl="chkRequireApprovalForLogin" CssClass="settinglabel" ConfigKey="SiteSettingsRequireApprovalForLogin" />
														<asp:CheckBox ID="chkRequireApprovalForLogin" runat="server" TabIndex="10" CssClass="forminput" AutoPostBack="true" />
														<portal:mojoHelpLink runat="server" HelpKey="sitesettings-requireapprovalforlogin-help" />
														<mp:SiteLabel runat="server" CssClass="help-block" UseLabelTag="false" ConfigKey="SiteSettingsRequireApprovalForLoginDescription" />
													</portal:FormGroupPanel>
													<portal:FormGroupPanel runat="server">
														<mp:SiteLabel runat="server" ForControl="txtEmailAdressesForUserApprovalNotification" CssClass="settinglabel" ConfigKey="EmailAddressesForUserApprovalNotification" />
														<portal:mojoHelpLink runat="server" HelpKey="sitesettings-EmailAdressesForUserApprovalNotification-help" />
														<asp:TextBox ID="txtEmailAdressesForUserApprovalNotification" TabIndex="10" CssClass="forminput verywidetextbox" runat="server" Rows="3" TextMode="MultiLine" />
														<mp:SiteLabel runat="server" CssClass="help-block" ConfigKey="EmailAddressForUserApprovalNotificationQuickHelp" UseLabelTag="false" />
													</portal:FormGroupPanel>
												</ContentTemplate>
											</asp:UpdatePanel>
										</portal:FormGroupPanel>
										<portal:FormGroupPanel runat="server" ID="fgpUserAccountSettings" SkinID="UserAccountSettings">
											<asp:Literal ID="litUserAccountSettingsHeader" runat="server" EnableViewState="false" />

											<portal:FormGroupPanel runat="server" ID="fgpUseEmailForLogin">
												<mp:SiteLabel ID="Sitelabelemailforlogin" runat="server" ForControl="chkUseEmailForLogin" CssClass="settinglabel" ConfigKey="SiteSettingsUseEmailForLoginLabel" />
												<asp:CheckBox ID="chkUseEmailForLogin" runat="server" TabIndex="10" CssClass="forminput" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingsuseemailforloginhelp" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server" ID="fgpAllowPersistantLogin">
												<mp:SiteLabel runat="server" ForControl="chkAllowPersistentLogin" CssClass="settinglabel" ConfigKey="AllowPersistentLogin" />
												<asp:CheckBox ID="chkAllowPersistentLogin" runat="server" TabIndex="10" CssClass="forminput" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettings-AllowPersistentLogin-help" />
												<mp:SiteLabel runat="server" CssClass="help-block" ConfigKey="AllowPersistentLoginDescription" UseLabelTag="false"/>

											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel runat="server" ForControl="chkRequireCaptchaOnLogin" CssClass="settinglabel" ConfigKey="RequireCaptchaOnLoginPage" />
												<asp:CheckBox ID="chkRequireCaptchaOnLogin" runat="server" TabIndex="10" CssClass="forminput" />
												<portal:mojoHelpLink runat="server" HelpKey="RequireCaptchaOnLoginPage-help" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server" ID="fgpAllowUserToChangeName">
												<mp:SiteLabel ID="SitelabelAllowUserToChangeName" runat="server" ForControl="chkAllowUserToChangeName" CssClass="settinglabel" ConfigKey="SiteSettingsAllowUsersToChangeNameLabel" />
												<asp:CheckBox ID="chkAllowUserToChangeName" runat="server" TabIndex="10" CssClass="forminput" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingsallowusernamechangehelp" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel runat="server" ForControl="ddAvatarSystem" CssClass="settinglabel" ConfigKey="AvatarSystemLabel" EnableViewState="false"></mp:SiteLabel>
												<asp:DropDownList ID="ddAvatarSystem" DataTextField="name" DataValueField="name"
													EnableViewState="true" TabIndex="10" runat="server" CssClass="forminput">
													<asp:ListItem Value="none" Text="<%$ Resources:Resource, AvatarTypeNone %>"></asp:ListItem>
													<asp:ListItem Value="internal" Text="<%$ Resources:Resource, AvatarTypeInternal %>"></asp:ListItem>
													<asp:ListItem Value="gravatar" Text="<%$ Resources:Resource, AvatarTypeGravatar %>"></asp:ListItem>
												</asp:DropDownList>
												<portal:mojoHelpLink runat="server" HelpKey="avatarsystem-help" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server" ID="fgpReallyDeleteUsers">
												<mp:SiteLabel ID="SitelabelReallyDeleteUsers" runat="server" ForControl="chkReallyDeleteUsers" CssClass="settinglabel" ConfigKey="SiteSettingsReallyDeleteUsersLabel" />
												<asp:CheckBox ID="chkReallyDeleteUsers" runat="server" TabIndex="10" CssClass="forminput" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingsreallydeleteusershelp" />
												<mp:SiteLabel runat="server" CssClass="help-block" ConfigKey="SiteSettingsReallyDeleteUsersExplainLabel" UseLabelTag="false" />
											</portal:FormGroupPanel>
										</portal:FormGroupPanel>
										<portal:FormGroupPanel ID="fgpPasswordSettings" runat="server" SkinID="PasswordSettings">
											<asp:Literal ID="litPasswordSettingsHeader" runat="server" EnableViewState="false" />
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel runat="server" ForControl="ddPasswordFormat" CssClass="settinglabel" ConfigKey="SiteSettingsPasswordFormatLabel" />
												<asp:UpdatePanel runat="server" ID="upPasswordFormat" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:DropDownList ID="ddPasswordFormat" runat="server" TabIndex="10" CssClass="forminput" Enabled="False" />
														<portal:mojoButton runat="server" ID="btnEnablePasswordFormatChange" />
													</ContentTemplate>
												</asp:UpdatePanel>
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingspasswordformathelp" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel runat="server" ForControl="txtMaxInvalidPasswordAttempts" CssClass="settinglabel" ConfigKey="SiteSettingsMaxInvalidPasswordAttemptsLabel" />
												<asp:TextBox ID="txtMaxInvalidPasswordAttempts" TabIndex="10" MaxLength="2" Columns="10" CssClass="forminput smalltextbox" runat="server" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingsmaxincalidpasswordhelp" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel runat="server" ForControl="txtPasswordAttemptWindowMinutes" CssClass="settinglabel" ConfigKey="SiteSettingsPasswordAttemptWindowMinutesLabel" />
												<asp:TextBox ID="txtPasswordAttemptWindowMinutes" TabIndex="10" MaxLength="2" Columns="10" CssClass="forminput smalltextbox" runat="server" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingspasswordattemptwindowhelp" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel runat="server" ForControl="txtMinimumPasswordLength" CssClass="settinglabel" ConfigKey="SiteSettingsMinimumPasswordLengthLabel" />
												<asp:TextBox ID="txtMinimumPasswordLength" TabIndex="10" MaxLength="2" Columns="10" CssClass="forminput smalltextbox" runat="server" Text="7" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingspasswordlengthhelp" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel runat="server" ForControl="txtMinRequiredNonAlphaNumericCharacters" CssClass="settinglabel" ConfigKey="SiteSettingsMinRequiredNonAlphaNumericCharactersLabel" />
												<asp:TextBox ID="txtMinRequiredNonAlphaNumericCharacters" TabIndex="10" MaxLength="2" CssClass="forminput smalltextbox" Columns="10" runat="server" Text="0" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingspasswordnonalphacharactershelp" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel runat="server" ForControl="txtPasswordStrengthRegularExpression" CssClass="settinglabel" ConfigKey="SiteSettingsPasswordStrengthExpressionLabel" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingspasswordstrengthhelp" />
												<asp:TextBox ID="txtPasswordStrengthRegularExpression" TabIndex="10" TextMode="MultiLine" SkinID="regex" Rows="3" runat="server" CssClass="forminput pwdregex" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel runat="server" ForControl="txtPasswordStrengthErrorMessage" CssClass="settinglabel" ConfigKey="SiteSettingsPasswordStrengthErrorMessage" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingspasswordstrength-errormessage-help" />
												<asp:TextBox ID="txtPasswordStrengthErrorMessage" TabIndex="10" runat="server" CssClass="forminput" TextMode="MultiLine" Rows="3" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server" SkinID="PasswordRecoverySettings" ID="fgpPasswordRecovery">
												<asp:Literal ID="litPasswordRecoverySettingsHeader" runat="server" EnableViewState="false" />
												<portal:FormGroupPanel runat="server" ID="fgpAllowPasswordRecovery">
													<mp:SiteLabel ID="lbl1" runat="server" ForControl="chkAllowPasswordRetrieval" CssClass="settinglabel" ConfigKey="SiteSettingsAllowPasswordRetrievalLabel" />
													<asp:CheckBox ID="chkAllowPasswordRetrieval" runat="server" TabIndex="10" CssClass="forminput" />
													<portal:mojoHelpLink runat="server" HelpKey="sitesettingsallowpasswordretrievalhelp" />
												</portal:FormGroupPanel>
												<portal:FormGroupPanel runat="server" ID="fgpAllowPasswordReset">
													<mp:SiteLabel runat="server" ForControl="chkAllowPasswordReset" CssClass="settinglabel" ConfigKey="SiteSettingsAllowPasswordResetLabel" />
													<asp:CheckBox ID="chkAllowPasswordReset" runat="server" TabIndex="10" CssClass="forminput" />
													<portal:mojoHelpLink runat="server" HelpKey="sitesettingsallowpasswordresethelp" />
												</portal:FormGroupPanel>
												<portal:FormGroupPanel runat="server" ID="fgpForcePasswordChangeOnRecovery">
													<mp:SiteLabel runat="server" ForControl="chkRequirePasswordChangeAfterRecovery" CssClass="settinglabel" ConfigKey="SiteSettingsRequirePasswordChangeOnRevovery" />
													<asp:CheckBox ID="chkRequirePasswordChangeAfterRecovery" runat="server" TabIndex="10" CssClass="forminput" />
													<portal:mojoHelpLink runat="server" HelpKey="sitesettings-requirepasswordchangeafterrecovery-help" />
												</portal:FormGroupPanel>
												<portal:FormGroupPanel runat="server">
													<mp:SiteLabel runat="server" ForControl="chkRequiresQuestionAndAnswer" CssClass="settinglabel" ConfigKey="SiteSettingsRequiresQuestionAndAnswerLabel" />
													<asp:CheckBox ID="chkRequiresQuestionAndAnswer" runat="server" TabIndex="10" CssClass="forminput" />
													<portal:mojoHelpLink runat="server" HelpKey="sitesettingsrequirequestionandanswerhelp" />
												</portal:FormGroupPanel>
											</portal:FormGroupPanel>
										</portal:FormGroupPanel>
									</div><!--end tab General Security-->

									<div id="tabLDAP" runat="server">
										<portal:FormGroupPanel runat="server" ID="fgpUseLdap">
											<mp:SiteLabel ID="lblUseLdapAuth" ForControl="chkUseLdapAuth" CssClass="settinglabel" ConfigKey="SiteSettingsUseLdapAuth" runat="server" />
											<asp:CheckBox ID="chkUseLdapAuth" runat="server" TabIndex="10" CssClass="forminput"></asp:CheckBox>
											<portal:mojoHelpLink runat="server" HelpKey="sitesettingsuseldaphelp" />
										</portal:FormGroupPanel>
										<portal:FormGroupPanel runat="server" ID="fgpLdapTestPassword">
											<mp:SiteLabel ForControl="txtLdapTestPassword" ConfigKey="SiteSettingsLdapTestPassword" CssClass="settinglabel" runat="server" />
											<asp:TextBox ID="txtLdapTestPassword" Columns="55" TabIndex="10" runat="server" TextMode="password" CssClass="forminput normaltextbox" MaxLength="255"></asp:TextBox>
											<portal:mojoHelpLink runat="server" HelpKey="sitesettingsldappasswordhelp" />
											<mp:SiteLabel runat="server" ConfigKey="SiteSettingsLdapTestPasswordQuickHelp" CssClass="help-block" UseLabelTag="false" />
										</portal:FormGroupPanel>
										<portal:FormGroupPanel runat="server" ID="fgpAutoCreateLdapUsers">
											<mp:SiteLabel ID="lblAutoCreateLdapUser" ForControl="chkAutoCreateLdapUserOnFirstLogin" CssClass="settinglabel" ConfigKey="SiteSettingsAutoCreateLdapUserOnFirstLoginLabel"
												runat="server" />
											<asp:CheckBox ID="chkAutoCreateLdapUserOnFirstLogin" runat="server" TabIndex="10" CssClass="forminput"></asp:CheckBox>
											<portal:mojoHelpLink runat="server" HelpKey="sitesettingsautocreateldapuserhelp" />
										</portal:FormGroupPanel>
										<portal:FormGroupPanel runat="server" ID="fgpLdapServer">
											<mp:SiteLabel ID="lblLdapServer" ForControl="txtLdapServer" CssClass="settinglabel" ConfigKey="SiteSettingsLdapServer" runat="server"></mp:SiteLabel>
											<asp:TextBox ID="txtLdapServer" Columns="55" runat="server" TabIndex="10" MaxLength="255" CssClass="forminput widetextbox"></asp:TextBox>
											<portal:mojoHelpLink runat="server" HelpKey="sitesettingsldapserverhelp" />
										</portal:FormGroupPanel>
										<portal:FormGroupPanel runat="server" ID="fgpLdapPort">
											<mp:SiteLabel ID="lblLdapPort" ForControl="txtLdapPort" CssClass="settinglabel" ConfigKey="SiteSettingsLdapPort"
												runat="server"></mp:SiteLabel>
											<asp:TextBox ID="txtLdapPort" Columns="55" runat="server" TabIndex="10" MaxLength="255" CssClass="forminput smalltextbox"></asp:TextBox>
											<portal:mojoHelpLink runat="server" HelpKey="sitesettingsldapporthelp" />
										</portal:FormGroupPanel>
										<portal:FormGroupPanel runat="server" ID="fgpLdapDomain">
											<mp:SiteLabel ForControl="txtLdapDomain" CssClass="settinglabel" ConfigKey="SiteSettingsLdapDomain" runat="server"></mp:SiteLabel>
											<asp:TextBox ID="txtLdapDomain" Columns="55" runat="server" TabIndex="10" MaxLength="255" CssClass="forminput widetextbox"></asp:TextBox>
											<portal:mojoHelpLink runat="server" HelpKey="sitesettingsldapdomainhelp" />
										</portal:FormGroupPanel>
										<portal:FormGroupPanel runat="server" ID="fgpLdapRootDn">
											<mp:SiteLabel ID="lblLdapRootDN" ForControl="txtLdapRootDN" CssClass="settinglabel" ConfigKey="SiteSettingsLdapRootDN" runat="server"></mp:SiteLabel>
											<asp:TextBox ID="txtLdapRootDN" Columns="55" runat="server" TabIndex="10" MaxLength="255" CssClass="forminput widetextbox"></asp:TextBox>
											<portal:mojoHelpLink runat="server" HelpKey="sitesettingsldaprootdnhelp" />
										</portal:FormGroupPanel>
										<portal:FormGroupPanel runat="server" ID="fgpLdapUserDNKey">
											<mp:SiteLabel ForControl="ddLdapUserDNKey" CssClass="settinglabel" ConfigKey="SiteSettingsLdapUserDNKey" runat="server"></mp:SiteLabel>
											<asp:DropDownList ID="ddLdapUserDNKey" runat="server" TabIndex="10" CssClass="forminput">
												<asp:ListItem Value="uid">uid (OpenLDAP)</asp:ListItem>
												<asp:ListItem Value="CN">CN (Active Directory)</asp:ListItem>
											</asp:DropDownList>
											<portal:mojoHelpLink runat="server" HelpKey="sitesettingsldapuserdnkeyhelp" />
										</portal:FormGroupPanel>
										<portal:FormGroupPanel runat="server">
											<mp:SiteLabel ForControl="chkAllowDbFallbackWithLdap" CssClass="settinglabel" ConfigKey="AllowDbFallbackWithLdap" runat="server"></mp:SiteLabel>
											<asp:CheckBox ID="chkAllowDbFallbackWithLdap" runat="server" TabIndex="10" CssClass="forminput"></asp:CheckBox>
											<portal:mojoHelpLink runat="server" HelpKey="sitesetting-AllowDbFallbackWithLdap-help" />
										</portal:FormGroupPanel>
										<portal:FormGroupPanel runat="server">
											<mp:SiteLabel ForControl="chkAllowEmailLoginWithLdapDbFallback" CssClass="settinglabel" ConfigKey="AllowEmailLoginWithLdapDbFallback" runat="server"></mp:SiteLabel>
											<asp:CheckBox ID="chkAllowEmailLoginWithLdapDbFallback" runat="server" TabIndex="10" CssClass="forminput"></asp:CheckBox>
											<portal:mojoHelpLink runat="server" HelpKey="sitesetting-AllowEmailLoginWithLdapDbFallback-help" />
										</portal:FormGroupPanel>
									</div><!--end tab LDAP-->

									<div id="tabthirdpartyauth" runat="server">
										<portal:FormGroupPanel runat="server" ID="fgpOpenIDSettings" SkinID="OpenIDSettings">
											<asp:Literal ID="litOpenIDSettingsHeader" runat="server" EnableViewState="false" />
											<portal:FormGroupPanel runat="server" ID="fgpOpenID">
												<mp:SiteLabel runat="server" ForControl="chkAllowOpenIDAuth" CssClass="settinglabel" ConfigKey="SiteSettingsAllowOpenIDAuthenticationLabel" />
												<asp:CheckBox ID="chkAllowOpenIDAuth" runat="server" TabIndex="10" CssClass="forminput" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingsopenidhelp" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server" ID="fgpOpenIDSelector">
												<mp:SiteLabel ForControl="txtOpenIdSelectorCode" CssClass="settinglabel" ConfigKey="SiteSettingsOpenIdSelectorLabel" runat="server"></mp:SiteLabel>
												<asp:TextBox ID="txtOpenIdSelectorCode" Columns="55" runat="server" TabIndex="10"
													MaxLength="255" CssClass="forminput widetextbox"></asp:TextBox>
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingsopenidselectorhelp" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel ForControl="txtRpxNowApiKey" CssClass="settinglabel" ConfigKey="RpxNowApiKeyLabel" runat="server"></mp:SiteLabel>
												<asp:TextBox ID="txtRpxNowApiKey" Columns="55" runat="server" MaxLength="255" CssClass="forminput widetextbox"></asp:TextBox>
												<portal:mojoHelpLink runat="server" HelpKey="rpxnow-apikey-help" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel ForControl="txtRpxNowApplicationName" CssClass="settinglabel" ConfigKey="RpxNowApplicationNameLabel" runat="server"></mp:SiteLabel>
												<asp:TextBox ID="txtRpxNowApplicationName" Columns="55" runat="server" MaxLength="255" CssClass="forminput widetextbox"></asp:TextBox>
												<portal:mojoHelpLink runat="server" HelpKey="rpxnow-applicationname-help" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server" SkinID="ButtonPanel">
												<asp:HyperLink ID="lnkRpxAdmin" runat="server" Visible="false" />
												<portal:mojoButton ID="btnSetupRpx" runat="server" />
											</portal:FormGroupPanel>
											
										</portal:FormGroupPanel>
										<portal:FormGroupPanel runat="server" ID="fgpWinLiveID" SkinID="WindowsLiveIDSettings">
											<asp:Literal ID="litWindowsLiveIDSettingsHeader" runat="server" EnableViewState="false" />

											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel runat="server" ForControl="chkAllowWindowsLiveAuth" CssClass="settinglabel" ConfigKey="SiteSettingsAllowWindowsLiveAuthLabel" />
												<asp:CheckBox ID="chkAllowWindowsLiveAuth" runat="server" TabIndex="10" CssClass="forminput" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingswindowslivehelp" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server" ID="fgpLiveMessenger">
												<mp:SiteLabel runat="server" ForControl="chkAllowWindowsLiveMessengerForMembers" CssClass="settinglabel" ConfigKey="AllowLiveMessengerOnProfilesLabel" />
												<asp:CheckBox ID="chkAllowWindowsLiveMessengerForMembers" runat="server" TabIndex="10" CssClass="forminput" />
												<portal:mojoHelpLink runat="server" HelpKey="livemessenger-admin-help" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel runat="server" ForControl="txtWindowsLiveAppID" CssClass="settinglabel" ConfigKey="SiteSettingsWindowsLiveAppIDLabel" />
												<asp:TextBox ID="txtWindowsLiveAppID" TabIndex="10" MaxLength="100" Columns="45" CssClass="forminput widetextbox" runat="server" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingswindowslivehelp" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel runat="server" ForControl="txtWindowsLiveKey" CssClass="settinglabel" ConfigKey="SiteSettingsWindowsLiveKeyLabel" />
												<asp:TextBox ID="txtWindowsLiveKey" TabIndex="10" MaxLength="100" Columns="45" runat="server" CssClass="forminput widetextbox" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingswindowslivehelp" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel runat="server" ForControl="txtAppLogoForWindowsLive" CssClass="settinglabel" ConfigKey="WindowsLiveAppLogoLabel" />
												<asp:Label ID="lblSiteRoot" runat="server" />
												<asp:TextBox ID="txtAppLogoForWindowsLive" TabIndex="10" MaxLength="100" Columns="45"
													runat="server" CssClass="forminput widetextbox" />
												<portal:FileBrowserTextBoxExtender runat="server" ID="pickAppLogoForWindowsLive"></portal:FileBrowserTextBoxExtender>
												<portal:mojoHelpLink runat="server" HelpKey="windowslive-applogo-help" />
											</portal:FormGroupPanel>
										</portal:FormGroupPanel>
										<portal:FormGroupPanel runat="server" ID="fgpDisableDbAuthentication">
											<mp:SiteLabel runat="server" ForControl="chkDisableDbAuthentication" CssClass="settinglabel" ConfigKey="DisableDbAuthentication" />
											<asp:CheckBox ID="chkDisableDbAuthentication" runat="server" TabIndex="10" CssClass="forminput" />
											<portal:mojoHelpLink runat="server" HelpKey="sitesettings-DisableDbAuthentication-help" />
										</portal:FormGroupPanel>
									</div><!--end tab 3rd party-->

									<div id="tabAntiSpam">
										<asp:UpdatePanel ID="updCaptcha" runat="server" UpdateMode="Conditional" RenderMode="Block" EnableViewState="true">
											<ContentTemplate>
												<portal:FormGroupPanel runat="server">
													<mp:SiteLabel runat="server" ForControl="ddCaptchaProviders" CssClass="settinglabel" ConfigKey="SiteSettingsCaptchaProviderLabel" />
													<asp:DropDownList ID="ddCaptchaProviders" DataTextField="name" DataValueField="name"
														EnableViewState="true" TabIndex="10" runat="server" CssClass="forminput" AutoPostBack="true">
													</asp:DropDownList>
													<portal:mojoHelpLink runat="server" HelpKey="sitesettingscaptchaproviderhelp" />
												</portal:FormGroupPanel>
												<portal:FormGroupPanel ID="pnlRecaptchaSettings" runat="server">
													<asp:Literal ID="litRecaptchaSettingsHeader" runat="server" EnableViewState="false" />
													<portal:FormGroupPanel runat="server">
														<mp:SiteLabel runat="server" ForControl="rbRecaptchaHcaptcha" CssClass="settinglabel" ConfigKey="SiteSettingsSiteRecaptchaHCaptchaChoiceLabel" />
														<asp:RadioButtonList ID="rbRecaptchaHcaptcha" runat="server" EnableViewState="true" AutoPostBack="true">
															<asp:ListItem Text="reCAPTCHA" Value="recaptcha" />
															<asp:ListItem Text="hCaptcha" Value="hcaptcha" />
														</asp:RadioButtonList>
													</portal:FormGroupPanel>
													<portal:FormGroupPanel runat="server">
														<mp:SiteLabel runat="server" ForControl="txtRecaptchaPublicKey" CssClass="settinglabel" ConfigKey="SiteSettingsSiteRecaptchaPublicKeyLabel" />
														<asp:TextBox ID="txtRecaptchaPublicKey" TabIndex="10" MaxLength="100" Columns="45" CssClass="forminput verywidetextbox" runat="server" />
														<portal:mojoHelpLink runat="server" HelpKey="sitesettingsrecaptchahelp" />
													</portal:FormGroupPanel>
													<portal:FormGroupPanel runat="server">
														<mp:SiteLabel runat="server" ForControl="txtRecaptchPrivateKey" CssClass="settinglabel" ConfigKey="SiteSettingsSiteRecaptchaPrivateKeyLabel" />
														<asp:TextBox ID="txtRecaptchaPrivateKey" TabIndex="10" MaxLength="100" Columns="45" CssClass="forminput verywidetextbox" runat="server" />
													</portal:FormGroupPanel>
													<portal:FormGroupPanel runat="server" ID="pnlRecaptchaAdvancedSettings" SkinID="SettingsWarningPanel">
														<asp:Literal ID="litRecaptchaAdvancedSettingsHeader" runat="server" EnableViewState="false" />
														<portal:FormGroupPanel runat="server">
															<mp:SiteLabel runat="server" ForControl="txtCaptchaVerifyUrl" CssClass="settinglabel" ConfigKey="SiteSettingsSiteCaptchaVerifyUrlLabel" />
															<asp:TextBox ID="txtCaptchaVerifyUrl" TabIndex="10" MaxLength="100" Columns="45" CssClass="forminput verywidetextbox" runat="server" />
															<asp:Literal runat="server" ID="litCaptchaVerifyDefault" EnableViewState="false" />
														</portal:FormGroupPanel>
														<portal:FormGroupPanel runat="server">
															<mp:SiteLabel runat="server" ForControl="txtCaptchaClientScriptUrl" CssClass="settinglabel" ConfigKey="SiteSettingsSiteCaptchaClientScriptUrlLabel" />
															<asp:TextBox ID="txtCaptchaClientScriptUrl" TabIndex="10" MaxLength="100" Columns="45" CssClass="forminput verywidetextbox" runat="server" />
															<asp:Literal runat="server" ID="litCaptchaScriptDefault" EnableViewState="false" />
														</portal:FormGroupPanel>
														<portal:FormGroupPanel runat="server">
															<mp:SiteLabel runat="server" ForControl="txtCaptchaTheme" CssClass="settinglabel" ConfigKey="SiteSettingsSiteCaptchaThemeLabel" />
															<asp:TextBox ID="txtCaptchaTheme" TabIndex="10" MaxLength="100" Columns="45" CssClass="forminput verywidetextbox" runat="server" />
															<asp:Literal runat="server" ID="litCaptchaThemeDefault" EnableViewState="false" />
														</portal:FormGroupPanel>
														<portal:FormGroupPanel runat="server">
															<mp:SiteLabel runat="server" ForControl="txtCaptchaParam" CssClass="settinglabel" ConfigKey="SiteSettingsSiteCaptchaParamLabel" />
															<asp:TextBox ID="txtCaptchaParam" TabIndex="10" MaxLength="100" Columns="45" CssClass="forminput verywidetextbox" runat="server" />
															<asp:Literal runat="server" ID="litCaptchaParamDefault" EnableViewState="false" />
														</portal:FormGroupPanel>
														<portal:FormGroupPanel runat="server">
															<mp:SiteLabel runat="server" ForControl="txtCaptchaResponseField" CssClass="settinglabel" ConfigKey="SiteSettingsSiteCaptchaResponseFieldLabel" />
															<asp:TextBox ID="txtCaptchaResponseField" TabIndex="10" MaxLength="100" Columns="45" CssClass="forminput verywidetextbox" runat="server" />
															<asp:Literal runat="server" ID="litCaptchaResponseDefault" EnableViewState="false" />
														</portal:FormGroupPanel>
														<portal:FormGroupPanel runat="server">
															<portal:mojoButton runat="server" ID="btnResetRecaptchaHcaptchaDefaults" />
														</portal:FormGroupPanel>
													</portal:FormGroupPanel>
												</portal:FormGroupPanel>
											</ContentTemplate>
										</asp:UpdatePanel>
										<portal:FormGroupPanel runat="server" ID="fgpBadWordSettings" SkinID="SettingsPanel">
											<asp:Literal ID="litBadWordHeader" runat="server" EnableViewState="false" />
											<asp:Literal ID="litBadWordQuickHelp" runat="server" EnableViewState="false" />
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel runat="server" ForControl="txtBadWordList" CssClass="settinglabel" ConfigKey="SiteSettingsBadWordList" />
												<mp:SiteLabel runat="server" ConfigKey="SiteSettingsBadWordListQuickHelp" CssClass="help-block" UseLabelTag="false" />
												<asp:TextBox runat="server" ID="txtBadWordList" CssClass="forminput badwordlist" TextMode="MultiLine" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel runat="server" ForControl="chkForceBadWordChecking" CssClass="settinglabel" ConfigKey="SiteSettingsForceBadWordChecking" />
												<asp:CheckBox runat="server" ID="chkForceBadWordChecking" CssClass="forminput" Checked="true" />
												<mp:SiteLabel runat="server" ConfigKey="SiteSettingsForceBadWordCheckingQuickHelp" CssClass="help-block" UseLabelTag="false" />
											</portal:FormGroupPanel>
										</portal:FormGroupPanel>
									</div><!--end tab SPAM-->
								</div><!-- end Security Tabs Wrapper-->
							</div><!-- end tab Security -->
							<div id="tabCompanyInfo">
								<portal:FormGroupPanel runat="server" ID="fgpCompanyInfo" SkinID="SettingsPanel">
									<asp:Literal ID="litCompanyInfoHeader" runat="server" EnableViewState="false" />
									<asp:Literal ID="litCompanyInfoQuickHelp" runat="server" EnableViewState="false" />
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" ForControl="txtCompanyName" CssClass="settinglabel" ConfigKey="SiteSettingsCompanyNameLabel" />
										<asp:TextBox ID="txtCompanyName" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput verywidetextbox" />
										<portal:mojoHelpLink runat="server" HelpKey="sitesettingscompanynamehelp" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" ForControl="txtStreetAddress" CssClass="settinglabel" ConfigKey="StreetAddress" />
										<asp:TextBox ID="txtStreetAddress" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput verywidetextbox" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" ForControl="txtStreetAddress2" CssClass="settinglabel" ConfigKey="StreetAddress2" />
										<asp:TextBox ID="txtStreetAddress2" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput verywidetextbox" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" ForControl="txtLocality" CssClass="settinglabel" ConfigKey="Locality" />
										<asp:TextBox ID="txtLocality" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" ForControl="txtRegion" CssClass="settinglabel" ConfigKey="Region" />
										<asp:TextBox ID="txtRegion" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" ForControl="txtPostalCode" CssClass="settinglabel" ConfigKey="PostalCode" />
										<asp:TextBox ID="txtPostalCode" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput normaltextbox" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" ForControl="txtCountry" CssClass="settinglabel" ConfigKey="Country" />
										<asp:TextBox ID="txtCountry" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" ForControl="txtPhone" CssClass="settinglabel" ConfigKey="Phone" />
										<asp:TextBox ID="txtPhone" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput normaltextbox" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" ForControl="txtFax" CssClass="settinglabel" ConfigKey="Fax" />
										<asp:TextBox ID="txtFax" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput normaltextbox" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server">
										<mp:SiteLabel runat="server" ForControl="txtPublicEmail" CssClass="settinglabel" ConfigKey="PublicEmail" />
										<asp:TextBox ID="txtPublicEmail" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox" TextMode="Email" />
									</portal:FormGroupPanel>
								</portal:FormGroupPanel>
							</div><!--end tab Company -->

							<div id="tabCommerce" runat="server">
								<portal:FormGroupPanel runat="server" ID="fgpDefaultCountry" SkinID="SettingsPanel">
									<asp:Literal ID="litDefaultCountryHeader" runat="server" EnableViewState="false" />
									<asp:UpdatePanel ID="upCountryState" UpdateMode="Conditional" runat="server" EnableViewState="true" RenderMode="Block">
										<ContentTemplate>
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel ID="lblDefaultCountry" runat="server" CssClass="settinglabel" ConfigKey="Country" ForControl="ddDefaultCountry" />
												<asp:DropDownList ID="ddDefaultCountry" runat="server" AutoPostBack="true" CssClass="countrylistdd" DataValueField="Guid" DataTextField="Name" EnableViewState="true" />
												<asp:HyperLink ID="lnkCountryAdmin" runat="server" />
											</portal:FormGroupPanel>
											<portal:FormGroupPanel runat="server">
												<mp:SiteLabel ID="lblDefaultState" runat="server" CssClass="settinglabel" ConfigKey="Region" ForControl="ddDefaultGeoZone" />
												<asp:DropDownList ID="ddDefaultGeoZone" runat="server" DataValueField="Guid" CssClass="statelistdd" DataTextField="Name" EnableViewState="true" />
												<asp:HyperLink ID="lnkStateAdmin" runat="server" />
											</portal:FormGroupPanel>
										</ContentTemplate>
									</asp:UpdatePanel>
								</portal:FormGroupPanel>
								<portal:FormGroupPanel runat="server" ID="fgpDefaultCurrency" SkinID="SettingsPanel">
									<asp:Literal ID="litDefaultCurrencyHeader" runat="server" EnableViewState="false" />
									<mp:SiteLabel runat="server" ForControl="SiteCurrencySetting" CssClass="settinglabel" ConfigKey="CurrencyLabel" />
									<portal:CurrencySetting ID="SiteCurrencySetting" runat="server" />
									<asp:HyperLink ID="lnkCurrencyAdmin" runat="server" />
								</portal:FormGroupPanel>
							</div>

							<div id="tabSiteMappings" runat="server">
								<asp:UpdatePanel ID="upHosts" UpdateMode="Conditional" runat="server">
									<ContentTemplate>
										<portal:FormGroupPanel ID="fgpHostNames" runat="server" SkinID="HostNames">
											<asp:Literal ID="litHostListHeader" runat="server" EnableViewState="false" />
											<asp:Panel ID="pnlAddHostName" runat="server" DefaultButton="btnAddHost" CssClass="add-mapping">
												<asp:TextBox ID="txtHostName" MaxLength="255" runat="server" CssClass="mediumtextbox" />
												<portal:mojoButton ID="btnAddHost" runat="server" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingshostnamehelp" /><br />
											</asp:Panel>
											<asp:Literal ID="litHostMessage" runat="server" EnableViewState="false" />
											<asp:Repeater ID="rptHosts" runat="server">
												<HeaderTemplate>
													<ul class="simplelist hostslist">
												</HeaderTemplate>
												<ItemTemplate>
													<li>
														<asp:ImageButton ImageUrl='<%# DeleteLinkImage %>'
															CommandName="delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "HostID") %>'
															AlternateText="<%# Resources.Resource.SiteSettingsDeleteHostLabel %>" runat="server"
															ID="btnDeleteHost" />&nbsp;
														<a href='http://<%# DataBinder.Eval(Container.DataItem, "HostName") %>' class="hostlink"><%# DataBinder.Eval(Container.DataItem, "HostName") %></a>
													</li>
												</ItemTemplate>
												<FooterTemplate>
													</ul>
												</FooterTemplate>
											</asp:Repeater>
										</portal:FormGroupPanel>
									</ContentTemplate>
								</asp:UpdatePanel>
								<asp:UpdatePanel ID="upFolderNames" UpdateMode="Conditional" runat="server" >
									<ContentTemplate>
										<portal:FormGroupPanel ID="fgpFolderNames" runat="server" SkinID="FolderNames">
											<asp:Literal ID="litFolderNamesListHeader" runat="server" EnableViewState="false" />
											<asp:Panel ID="pnlAddFolder" runat="server" DefaultButton="btnAddFolder" CssClass="add-mapping">
												<asp:TextBox ID="txtFolderName" MaxLength="255" runat="server" CssClass="mediumtextbox" />
												<portal:mojoButton ID="btnAddFolder" runat="server" />
												<portal:mojoHelpLink runat="server" HelpKey="sitesettingsfoldernamehelp" />
											</asp:Panel>

											<asp:Literal ID="litFolderMessage" runat="server" EnableViewState="false" />
											<asp:Repeater ID="rptFolderNames" runat="server">
												<HeaderTemplate>
													<ul class="simplelist foldernamelist">
												</HeaderTemplate>
												<ItemTemplate>
													<li>
														<asp:ImageButton ImageUrl='<%# DeleteLinkImage %>'
															CommandName="delete" ToolTip="<%# Resources.Resource.SiteSettingsDeleteFolderMapping %>"
															CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Guid") %>' AlternateText="<%# Resources.Resource.SiteSettingsDeleteHostLabel %>"
															runat="server" ID="btnDeleteFolder" />&nbsp;
														<a href='<%# WebUtils.GetSiteRoot() + "/" + DataBinder.Eval(Container.DataItem, "FolderName")%>' class="hostlink"><%# DataBinder.Eval(Container.DataItem, "FolderName") %></a>
													</li>
												</ItemTemplate>
												<FooterTemplate>
													</ul>
												</FooterTemplate>
											</asp:Repeater>
										</portal:FormGroupPanel>
									</ContentTemplate>
								</asp:UpdatePanel>
							</div>
							<div id="tabSiteFeatures" runat="server" visible="false">
								<portal:FormGroupPanel runat="server" SkinID="SiteFeatures">
									<asp:UpdatePanel ID="upFeatures" UpdateMode="Conditional" runat="server">
										<ContentTemplate>
											<portal:FormGroupPanel runat="server" SkinID="SiteFeaturesLeft">
												<h3>
													<mp:SiteLabel runat="server" ConfigKey="SiteSettingsSiteAvailableFeaturesLabel" UseLabelTag="false" />
													<portal:mojoHelpLink runat="server" HelpKey="sitesettingschildsitefeatureshelp" />
												</h3>
												<asp:ListBox ID="lstAllFeatures" runat="Server" SelectionMode="Multiple" />
											</portal:FormGroupPanel>

											<portal:FormGroupPanel runat="server" SkinID="SiteFeaturesCenter">
												<asp:Button Text="<" runat="server" ID="btnRemoveFeature" CausesValidation="false" SkinID="SiteFeaturesRemove" />
												<asp:Button Text=">" runat="server" ID="btnAddFeature" CausesValidation="false" SkinID="SiteFeaturesAdd" />
											</portal:FormGroupPanel>

											<portal:FormGroupPanel runat="server" SkinID="SiteFeaturesRight">
												<h3>
													<mp:SiteLabel runat="server" ConfigKey="SiteSettingsSiteSelectedFeaturesLabel" UseLabelTag="false" />
												</h3>
												<asp:ListBox ID="lstSelectedFeatures" runat="Server" SelectionMode="Multiple" />
											</portal:FormGroupPanel>

											<asp:Literal ID="litFeatureMessage" runat="server" EnableViewState="false" />
										</ContentTemplate>
									</asp:UpdatePanel>
									<%-- This script determines how many items the selects have and sets the size to that many items --%>
<%--									<script>
										(function () {
											Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(updateSelectsHeight);

											function updateSelectsHeight() {
												var select1 = document.getElementById('<%= lstAllFeatures.ClientID %>');
												var select2 = document.getElementById('<%= lstSelectedFeatures.ClientID %>');
												var height = Math.max(select1.length, select2.length) <= 1 ? 4 : Math.max(select1.length, select2.length);

												select1.size = height;
												select1.style.overflow = 'hidden';
												select2.size = height;
												select2.style.overflow = 'hidden';
											};
										})();
									</script>--%>
								</portal:FormGroupPanel>
							</div>

						

							<div id="tabApiKeys">
								<portal:FormGroupPanel runat="server">
									<mp:SiteLabel runat="server" ForControl="ddSearchEngine" CssClass="settinglabel" ConfigKey="DefaultSiteSearch" />
									<asp:DropDownList ID="ddSearchEngine" runat="server" CssClass="forminput">
										<asp:ListItem Value="internal" Text="<%$ Resources:Resource, InternalSearchEngine %>" />
										<asp:ListItem Value="bing" Text="<%$ Resources:Resource, BingSiteSearch %>" />
										<asp:ListItem Value="google" Text="<%$ Resources:Resource, GoogleSiteSearch %>" />
									</asp:DropDownList>
									<portal:mojoHelpLink runat="server" HelpKey="default-search-engine-help" />
								</portal:FormGroupPanel>
								<portal:FormGroupPanel runat="server">
									<mp:SiteLabel runat="server" ForControl="txtBingSearchAPIKey" CssClass="settinglabel" ConfigKey="BingSearchApiKey" />
									<asp:TextBox ID="txtBingSearchAPIKey" TabIndex="10" MaxLength="100" Columns="45" runat="server" CssClass="forminput widetextbox" />
									<portal:mojoHelpLink runat="server" HelpKey="bing-search-apikey-help" />
								</portal:FormGroupPanel>
								<portal:FormGroupPanel runat="server">
									<mp:SiteLabel runat="server" ForControl="txtGoogleCustomSearchId" CssClass="settinglabel" ConfigKey="GoogleCustomSearchId" />
									<asp:TextBox ID="txtGoogleCustomSearchId" TabIndex="10" MaxLength="100" Columns="45" runat="server" CssClass="forminput widetextbox" />
									<portal:mojoHelpLink runat="server" HelpKey="google-custom-searchid-help" />
								</portal:FormGroupPanel>
								<portal:FormGroupPanel runat="server">
									<mp:SiteLabel runat="server" ForControl="chkShowAlternateSearchIfConfigured" CssClass="settinglabel" ConfigKey="ShowAlternateSearchIfConfigured" />
									<asp:CheckBox ID="chkShowAlternateSearchIfConfigured" runat="server" CssClass="forminput" />
									<portal:mojoHelpLink runat="server" HelpKey="show-alternate-search-help" />
								</portal:FormGroupPanel>
								<portal:FormGroupPanel runat="server" ID="fgpGAnalytics">
									<mp:SiteLabel runat="server" ForControl="txtGoogleAnayticsAccountCode" CssClass="settinglabel" ConfigKey="GoogleAnalyticsAccountCodeLabel" />
									<asp:TextBox ID="txtGoogleAnayticsAccountCode" TabIndex="10" MaxLength="100" Columns="45"
										runat="server" CssClass="forminput widetextbox" />
									<portal:mojoHelpLink runat="server" HelpKey="googleanalyticsaccountcodehelp" />
								</portal:FormGroupPanel>
								<portal:FormGroupPanel runat="server" ID="fgpWoopra">
									<mp:SiteLabel runat="server" ForControl="chkEnableWoopra" CssClass="settinglabel" ConfigKey="EnableWoopraLabel" />
									<asp:CheckBox ID="chkEnableWoopra" runat="server" CssClass="forminput" />
									<portal:mojoHelpLink runat="server" HelpKey="wooprahelp" />
								</portal:FormGroupPanel>
								<portal:FormGroupPanel runat="server">
									<mp:SiteLabel runat="server" ForControl="txtGmapApiKey" CssClass="settinglabel" ConfigKey="SiteSettingsGmapApiKeyLabel" />
									<asp:TextBox ID="txtGmapApiKey" TabIndex="10" MaxLength="100" Columns="45" runat="server" CssClass="forminput widetextbox" />
									<portal:mojoHelpLink runat="server" HelpKey="sitesettingsgmapapikeyhelp" />
								</portal:FormGroupPanel>
								<portal:FormGroupPanel runat="server">
									<mp:SiteLabel runat="server" ForControl="ddCommentSystem" CssClass="settinglabel" ConfigKey="SiteSettingsCommentSystem" />
									<asp:DropDownList ID="ddCommentSystem" runat="server" CssClass="forminput">
										<asp:ListItem Value="intensedebate" Text="<%$ Resources:Resource, CommentSystemIntenseDebate %>" />
										<asp:ListItem Value="disqus" Text="<%$ Resources:Resource, CommentSystemDisqus %>" />
										<asp:ListItem Value="facebook" Text="<%$ Resources:Resource, CommentSystemFacebook %>" />
									</asp:DropDownList>
									<portal:mojoHelpLink runat="server" HelpKey="comment-system-help" />
								</portal:FormGroupPanel>
								<portal:FormGroupPanel runat="server">
									<mp:SiteLabel runat="server" ForControl="txtFacebookAppId" CssClass="settinglabel" ConfigKey="FacebookAppId" />
									<asp:TextBox ID="txtFacebookAppId" TabIndex="10" MaxLength="255" runat="server" CssClass="forminput widetextbox" />
									<portal:mojoHelpLink runat="server" HelpKey="FacebookAppId-help" />
								</portal:FormGroupPanel>
								<portal:FormGroupPanel runat="server">
									<mp:SiteLabel runat="server" ForControl="txtIntenseDebateAccountId" CssClass="settinglabel" ConfigKey="IntenseDebateAccountId" />
									<asp:TextBox ID="txtIntenseDebateAccountId" TabIndex="10" MaxLength="255" runat="server" CssClass="forminput widetextbox" />
									<portal:mojoHelpLink runat="server" HelpKey="intensedebate-accoutid-help" />
								</portal:FormGroupPanel>
								<portal:FormGroupPanel runat="server">
									<mp:SiteLabel runat="server" ForControl="txtDisqusSiteShortName" CssClass="settinglabel" ConfigKey="DisqusSiteShortName" />
									<asp:TextBox ID="txtDisqusSiteShortName" TabIndex="10" MaxLength="255" runat="server" CssClass="forminput widetextbox" />
									<portal:mojoHelpLink runat="server" HelpKey="disqus-siteshortname-help" />
								</portal:FormGroupPanel>
								<portal:FormGroupPanel runat="server">
									<mp:SiteLabel runat="server" ForControl="txtAddThisUserId" CssClass="settinglabel" ConfigKey="SiteSettingsAddThisAccountIdLabel" />
									<asp:TextBox ID="txtAddThisUserId" TabIndex="10" MaxLength="100" Columns="45" runat="server" CssClass="forminput widetextbox" />
									<portal:mojoHelpLink runat="server" HelpKey="sitesettingsaddthisuseridhelp" />
								</portal:FormGroupPanel>
							</div>

							<div id="tabMailSettings" runat="server">
								<portal:FormGroupPanel runat="server">
									<mp:SiteLabel runat="server" ForControl="txtSiteEmailFromAddress" CssClass="settinglabel" ConfigKey="SiteSettingsSiteEmailFromAddressLabel" />
									<asp:TextBox ID="txtSiteEmailFromAddress" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox" />
									<portal:mojoHelpLink runat="server" HelpKey="sitesettingssiteemailfromhelp" />
								</portal:FormGroupPanel>
								<portal:FormGroupPanel runat="server">
									<mp:SiteLabel runat="server" ForControl="txtSiteEmailFromAlias" CssClass="settinglabel" ConfigKey="SiteSettingsSiteEmailFromAliasLabel" />
									<asp:TextBox ID="txtSiteEmailFromAlias" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox" />
									<portal:mojoHelpLink runat="server" HelpKey="sitesettingssiteemailfromaliashelp" />
								</portal:FormGroupPanel>

								<portal:FormGroupPanel runat="server" ID="fgpSMTPSettings" SkinID="SMTPSettings">
									<asp:Literal runat="server" ID="litSMTPSettingsHeader" EnableViewState="false" />
									<portal:mojoHelpLink runat="server" HelpKey="smtphelp" />
									<portal:BasePanel runat="server" ID="pnlSMTPSettingsWrapper" RenderContentsOnly="true">
										<portal:FormGroupPanel runat="server">
											<mp:SiteLabel runat="server" ForControl="txtSMTPServer" CssClass="settinglabel" ConfigKey="SMTPServer" />
											<asp:TextBox ID="txtSMTPServer" MaxLength="100" Columns="45" runat="server" CssClass="forminput widetextbox" TabIndex="10" />
										</portal:FormGroupPanel>
										<portal:FormGroupPanel runat="server">
											<mp:SiteLabel runat="server" ForControl="txtSMTPPort" CssClass="settinglabel" ConfigKey="SMTPPort" />
											<asp:TextBox ID="txtSMTPPort" MaxLength="100" Columns="45" runat="server" CssClass="forminput widetextbox" TabIndex="10" />
										</portal:FormGroupPanel>
										<portal:FormGroupPanel runat="server">
											<mp:SiteLabel runat="server" ForControl="chkSMTPUseSsl" CssClass="settinglabel" ConfigKey="SMTPUseSsl" />
											<asp:CheckBox ID="chkSMTPUseSsl" runat="server" CssClass="forminput" TabIndex="10" />
										</portal:FormGroupPanel>
										<asp:UpdatePanel ID="upSMTPAuth" UpdateMode="Conditional" runat="server" EnableViewState="true" RenderMode="Inline">
											<ContentTemplate>
												<portal:FormGroupPanel runat="server">
													<mp:SiteLabel runat="server" ForControl="chkSMTPRequiresAuthentication" CssClass="settinglabel" ConfigKey="SMTPRequiresAuthentication" />
													<asp:CheckBox ID="chkSMTPRequiresAuthentication" runat="server" CssClass="forminput" AutoPostBack="true" TabIndex="10" />
												</portal:FormGroupPanel>
												<portal:FormGroupPanel runat="server">
													<mp:SiteLabel runat="server" ForControl="txtSMTPUser" CssClass="settinglabel" ConfigKey="SMTPUser" />
													<asp:TextBox ID="txtSMTPUser" MaxLength="100" Columns="45" runat="server" CssClass="forminput widetextbox" TabIndex="10" />
												</portal:FormGroupPanel>
												<portal:FormGroupPanel runat="server">
													<mp:SiteLabel runat="server" ForControl="txtSMTPPassword" CssClass="settinglabel" ConfigKey="SMTPPassword" />
													<asp:TextBox ID="txtSMTPPassword" TextMode="Password" MaxLength="100" Columns="45" runat="server" CssClass="forminput widetextbox" TabIndex="10" />
												</portal:FormGroupPanel>										
											</ContentTemplate>
										</asp:UpdatePanel>
										<portal:FormGroupPanel runat="server" ID="fgpSMTPEncoding">
											<mp:SiteLabel runat="server" ForControl="txtSMTPPreferredEncoding" CssClass="settinglabel" ConfigKey="SMTPPreferredEncoding" />
											<asp:TextBox ID="txtSMTPPreferredEncoding" MaxLength="100" Columns="45" runat="server" CssClass="forminput widetextbox" TabIndex="10" />
										</portal:FormGroupPanel>
									</portal:BasePanel>
								</portal:FormGroupPanel>
								<portal:FormGroupPanel runat="server" ID="fgpSMTPHeaders">
									<asp:Literal runat="server" ID="litSMTPHeadersHeading" EnableViewState="false" />
									<mp:SiteLabel runat="server" ForControl="" CssClass="settinglabel" ConfigKey="SiteSettingsSMTPHeaders" />
									<asp:TextBox runat="server" ID="txtSMTPHeaders" TextMode="MultiLine" />
									<portal:mojoHelpLink runat="server" HelpKey="smtpheaders" />
								</portal:FormGroupPanel>
								<portal:FormGroupPanel runat="server" ID="fgpTestSMTPSettings" SkinID="SMTPSettings">
									<asp:Literal runat="server" ID="litTestSMTPSettingsHeader" EnableViewState="false" />
									<portal:mojoHelpLink runat="server" HelpKey="smtptesthelp" />
									<portal:BasePanel runat="server" ID="pnlTestSMTPSettingsWrapper" RenderContentsOnly="true">
										<portal:FormGroupPanel runat="server" DefaultButton="btnTestSMTPSettings">
											<asp:UpdatePanel runat="server" ID="updTestSMTPSettings" UpdateMode="Always">
												<ContentTemplate>
													<mp:SiteLabel runat="server" ForControl="txtTestSMTPEmailAddress" CssClass="settinglabel" ConfigKey="SiteSettingsTestSMTPEmailAddress" />
													<asp:TextBox ID="txtTestSMTPEmailAddress" MaxLength="100" Columns="45" runat="server" CssClass="forminput" TabIndex="10" />
													<asp:Button ID="btnTestSMTPSettings" runat="server" />

													<portal:FormGroupPanel runat="server" ID="fgpTestSMTPSettingsResult" SkinID="SMTPTestResult">
														<asp:Literal ID="litTestSMTPResultHeader" runat="server" EnableViewState="false" />
														<asp:Literal ID="litTestSMTPResult" runat="server" EnableViewState="false" />

													</portal:FormGroupPanel>
												</ContentTemplate>
												<Triggers>
													<asp:AsyncPostBackTrigger ControlID="btnTestSMTPSettings" />
												</Triggers>
											</asp:UpdatePanel>
										</portal:FormGroupPanel>
									</portal:BasePanel>
									<script>
										<!-- 
										var prm = Sys.WebForms.PageRequestManager.getInstance();
										prm.add_initializeRequest(InitializeRequest);
										prm.add_endRequest(EndRequest);
										var postBackElement;
										function InitializeRequest(sender, args) {
											if (prm.get_isInAsyncPostBack()) {
												args.set_cancel(true);
											}
											postBackElement = args.get_postBackElement();
											if (postBackElement.id == 'btnTestSMTPSettings') {
												$get('btnTestSMTPSettings').value = 'Attempting to send ...';
												$get('btnTestSMTPSettings').disabled = true;
											}
										}
										function EndRequest(sender, args) {
											if (postBackElement.id == 'btnTestSMTPSettings') {
												$get('btnTestSMTPSettings').value = 'Send Test Message';
												$get('btnTestSMTPSettings').disabled = false;
											}
										}
										// -->
									</script>
								</portal:FormGroupPanel>
							</div>

							<div id="tabContent" runat="server" enableviewstate="false">
								<portal:FormGroupPanel runat="server" ID="fgpScripts" SkinID="SettingsPanel">
									<asp:Literal ID="litScriptsHeader" runat="server" EnableViewState="false" />

									<portal:FormGroupPanel runat="server" ID="fgpHeaderContent">
										<mp:SiteLabel runat="server" ForControl="txtHeaderContent" CssClass="settinglabel" ConfigKey="SiteSettingsContentHeaderContentHeading" EnableViewState="false" />
										<asp:TextBox runat="server" ID="txtHeaderContent" TextMode="MultiLine" />
										<asp:Literal ID="litHeaderContentQuickHelp" runat="server" EnableViewState="false" />
									</portal:FormGroupPanel>

									<portal:FormGroupPanel runat="server" ID="fgpFooterContent">
										<mp:SiteLabel runat="server" ForControl="txtFooterContent" CssClass="settinglabel" ConfigKey="SiteSettingsContentFooterContentHeading" EnableViewState="false" />
										<asp:TextBox runat="server" ID="txtFooterContent" TextMode="MultiLine" />
										<asp:Literal ID="litFooterContentQuickHelp" runat="server" EnableViewState="false" />
									</portal:FormGroupPanel>
								</portal:FormGroupPanel>

								<portal:FormGroupPanel runat="server" ID="fgpAdminContent" SkinID="SettingsPanel">
									<asp:Literal ID="litAdminScriptsHeader" runat="server" EnableViewState="false" />

									<portal:FormGroupPanel runat="server" ID="fgpHeaderContentAdmin">
										<mp:SiteLabel runat="server" ForControl="txtHeaderAdminContent" CssClass="settinglabel" ConfigKey="SiteSettingsContentHeaderContentHeading" EnableViewState="false" />
										<asp:TextBox runat="server" ID="txtHeaderAdminContent" TextMode="MultiLine" />
										<asp:Literal ID="litHeaderAdminContentQuickHelp" runat="server" EnableViewState="false" />
									</portal:FormGroupPanel>

									<portal:FormGroupPanel runat="server" ID="fgpFooterAdminContent">
										<mp:SiteLabel runat="server" ForControl="txtFooterAdminContent" CssClass="settinglabel" ConfigKey="SiteSettingsContentFooterContentHeading" EnableViewState="false" />
										<asp:TextBox runat="server" ID="txtFooterAdminContent" TextMode="MultiLine" />
										<asp:Literal ID="litFooterAdminContentQuickHelp" runat="server" EnableViewState="false" />
									</portal:FormGroupPanel>
								</portal:FormGroupPanel>
							</div>

							<div id="tabAdvanced" runat="server" enableviewstate="false">
								<portal:FormGroupPanel runat="server" ID="fgpAdvancedSettings">
									<asp:Literal ID="litAdvSettingsHeader" runat="server" EnableViewState="false" />
									<portal:FormGroupPanel runat="server" ID="fgpFriendlyUrlPattern">
										<mp:SiteLabel ID="lblDefaultFriendlyUrlPatten" runat="server" ForControl="ddDefaultFriendlyUrlPattern" CssClass="settinglabel" ConfigKey="SiteSettingsDefaultFriendlyUrlPatternLabel"
											EnableViewState="false" />
										<asp:DropDownList ID="ddDefaultFriendlyUrlPattern" runat="server"
											TabIndex="10" CssClass="forminput">
											<asp:ListItem Value="PageNameWithDotASPX" Text="<%$ Resources:Resource, UrlFormatAspx %>"></asp:ListItem>
											<asp:ListItem Value="PageName" Text="<%$ Resources:Resource, UrlFormatExtensionless %>" Selected></asp:ListItem>
										</asp:DropDownList>
										<portal:mojoHelpLink runat="server" HelpKey="sitesettingsdefaultfriendlyurlpatternhelp" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server" ID="fgpPreferredHostName">
										<mp:SiteLabel runat="server" ForControl="txtPreferredHostName" CssClass="settinglabel" ConfigKey="SiteSettingsPreferredHostNameLabel" />
										<asp:TextBox runat="server" ID="txtPreferredHostName" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox" />
										<portal:mojoHelpLink runat="server" HelpKey="sitesettingspreferredhostnamehelp" />
									</portal:FormGroupPanel>
									<portal:FormGroupPanel runat="server" ID="fgpHomePage">
										<mp:SiteLabel ForControl="pageSelector" runat="server" CssClass="settinglabel" ConfigKey="SiteSettingsHomePageOverride" />
										<portal:PageSelectorSetting ID="pageSelector" runat="server" />
									</portal:FormGroupPanel>
								</portal:FormGroupPanel>
							</div>
						</div>

						<portal:FormGroupPanel runat="server">
							<asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="sitesettings" />
							<asp:RegularExpressionValidator ID="regexMaxInvalidPasswordAttempts" runat="server"
								ControlToValidate="txtMaxInvalidPasswordAttempts" ValidationGroup="sitesettings"
								Display="None" ValidationExpression="^-{0,1}\d+$"></asp:RegularExpressionValidator>
							<asp:RegularExpressionValidator ID="regexPasswordAttemptWindow" runat="server" ControlToValidate="txtPasswordAttemptWindowMinutes"
								ValidationGroup="sitesettings" Display="None" ValidationExpression="^-{0,1}\d+$"></asp:RegularExpressionValidator>
							<asp:RegularExpressionValidator ID="regexMinPasswordLength" runat="server" ControlToValidate="txtMinimumPasswordLength"
								ValidationGroup="sitesettings" Display="None" ValidationExpression="^-{0,1}\d+$"></asp:RegularExpressionValidator>
							<asp:RegularExpressionValidator ID="regexPasswordMinNonAlphaNumeric" runat="server"
								ControlToValidate="txtMinRequiredNonAlphaNumericCharacters" ValidationGroup="sitesettings"
								Display="None" ValidationExpression="^-{0,1}\d+$"></asp:RegularExpressionValidator>
							<asp:RegularExpressionValidator ID="regexWinLiveSecret" runat="server" ControlToValidate="txtWindowsLiveKey"
								ValidationGroup="sitesettings" Display="None" ValidationExpression=".{16}.*"></asp:RegularExpressionValidator>
							<portal:mojoLabel ID="lblErrorMessage" runat="server" CssClass="txterror warning" EnableViewState="false"></portal:mojoLabel>
							<asp:HiddenField ID="hdnCurrentSkin" runat="server" />
						</portal:FormGroupPanel>
						<portal:FormGroupPanel runat="server" SkinID="ButtonPanel">
							<portal:mojoButton ID="btnSave" Text="Apply Changes" runat="server" SkinID="SaveButton" />
							<portal:mojoButton ID="btnDelete" runat="server" Visible="false" SkinID="DeleteButton" />
						</portal:FormGroupPanel>
					</portal:BasePanel>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server"></asp:Content>
