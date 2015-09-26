<%@ Page Language="C#" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SiteSettings.aspx.cs"
    Inherits="mojoPortal.Web.AdminUI.SiteSettingsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
 <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkSiteList" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx"
            CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator id="litLinkSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkSiteSettings" runat="server" CssClass="selectedcrumb" />
</portal:AdminCrumbContainer>
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin sitesettings">
        <portal:HeadingControl id="heading" runat="server" />
    <portal:OuterBodyPanel ID="pnlOuterBody" runat="server" SkinID="admin">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    <asp:Panel ID="pnlSiteSettings" runat="server" DefaultButton="btnSave">
        <div class="settingrow">
            <mp:SiteLabel ID="lblSiteTitle" ForControl="txtSiteName" runat="server" CssClass="settinglabel "
                ConfigKey="SiteSettingsSiteTitleLabel" EnableViewState="false"></mp:SiteLabel>
            <asp:TextBox ID="txtSiteName" TabIndex="10" runat="server" CssClass="forminput widetextbox" />  
            <asp:Hyperlink id="lnkNewSite" runat="server" CssClass="newsitelink" />
        </div>
        <div id="divtabs" class="mojo-tabs">
            <ul>
                <li id="liGeneral" runat="server"><a href="#tabSettings">
                    <asp:Literal ID="litSettingsTab" runat="server" EnableViewState="false" /></a></li>
                <li id="liSecurity" runat="server" enableviewstate="false"><asp:Literal ID="litSecurityTabLink" runat="server" EnableViewState="false" /></li>
                    <li><a href="#tabCompanyInfo"><asp:Literal ID="litCompanyInfoTab" runat="server" EnableViewState="false" /></a></li>
                <li id="liCommerce" runat="server" enableviewstate="false"><asp:Literal ID="litCommerceTabLink" runat="server" EnableViewState="false" /></li>
                <li id="liHosts" runat="server" visible="false" enableviewstate="false"><asp:Literal ID="litHostsTabLink" runat="server" EnableViewState="false" /></li>
                <li id="liFolderNames" runat="server" visible="false" enableviewstate="false"><asp:Literal ID="litFolderNamesTabLink" runat="server" EnableViewState="false" /></li>
                <li id="liFeatures" runat="server" visible="false" enableviewstate="false"><asp:Literal ID="litFeaturesTabLink" runat="server" EnableViewState="false" /></li>
                <li id="liWebParts" runat="server" visible="false" enableviewstate="false"><asp:Literal ID="litWebPartsTabLink" runat="server" EnableViewState="false" /></li>
                <li><a href="#tabApiKeys"><asp:Literal ID="litAPIKeysTab" runat="server" EnableViewState="false" /></a></li>
                <li id="liMailSettings" runat="server" enableviewstate="false"><asp:Literal ID="litMailSettingsTabLink" runat="server" EnableViewState="false" /></li>
            </ul>
                    
                <div id="tabSettings">
                    <div id="divSiteId" runat="server" class="settingrow" visible="false">
                        <mp:SiteLabel ID="SiteLabel52" ForControl="ddSkins" runat="server" CssClass="settinglabel"
                            ConfigKey="SiteSettingsSiteIDLabel" EnableViewState="false"></mp:SiteLabel>
                        <asp:Label ID="lblSiteId" runat="server" />/<asp:Label ID="lblSiteGuid" runat="server" />
                    </div>
                    <div id="divSiteIsClosed" runat="server" class="settingrow">
                        <mp:SiteLabel ID="Sitelabel102" runat="server" ForControl="chkSiteIsClosed" CssClass="settinglabel"
                            ConfigKey="SiteIsClosed" />
                        <asp:CheckBox ID="chkSiteIsClosed" runat="server" CssClass="forminput" />
                        <portal:mojoHelpLink ID="MojoHelpLink97" runat="server" HelpKey="sitesettings-siteisclosed-help" />
                        <asp:HyperLink ID="lnkEditClosedMessage" runat="server" />
                    </div>
                    <div id="divTimeZone" runat="server" visible="false" class="settingrow">
                        <mp:SiteLabel ID="SiteLabel84" runat="server"  CssClass="settinglabel"
                            ConfigKey="TimeZone"></mp:SiteLabel>
                        <portal:TimeZoneIdSetting ID="timeZone" runat="server" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblSkin" ForControl="ddSkins" runat="server" CssClass="settinglabel"
                            ConfigKey="SiteSettingsSiteSkinLabel" EnableViewState="false"></mp:SiteLabel> 
                        <portal:SkinList id="SkinSetting" runat="server" />
                        <portal:mojoHelpLink ID="MojoHelpLink2" runat="server" HelpKey="sitesettingssiteskinhelp" />
                        <portal:mojoButton ID="btnRestoreSkins" runat="server" Visible="false" />
                    </div>
                    <div id="divMobileSkin" runat="server" class="settingrow">
                        <mp:SiteLabel ID="SiteLabel101" runat="server" CssClass="settinglabel" ConfigKey="MobileSkin">
                        </mp:SiteLabel>
                        <asp:DropDownList ID="ddMobileSkin" runat="server" DataValueField="Name"
                            DataTextField="Name" CssClass="forminput skinlist" TabIndex="10">
                        </asp:DropDownList>
                        <portal:mojoHelpLink ID="MojoHelpLink96" runat="server" HelpKey="mobile-skin-help" />
                    </div>
                    <div class="settingrow logolist">
                        <mp:SiteLabel ID="lblLogo" ForControl="ddLogos" runat="server" CssClass="settinglabel"
                            ConfigKey="SiteSettingsSiteLogoLabel" EnableViewState="false"></mp:SiteLabel>
                        <asp:DropDownList ID="ddLogos" runat="server" TabIndex="10" EnableViewState="true"
                                DataValueField="Name" DataTextField="Name" CssClass="forminput">
                        </asp:DropDownList>
                        <img alt="" src="" id="imgLogo" runat="server" enableviewstate="false" />
                        <portal:mojoHelpLink ID="MojoHelpLink3" runat="server" HelpKey="sitesettingssitelogohelp" />
                    </div>
                    <div id="divFriendlyUrlPattern" runat="server" class="settingrow">
                        <mp:SiteLabel ID="lblDefaultFriendlyUrlPatten" runat="server" ForControl="ddDefaultFriendlyUrlPattern"
                            CssClass="settinglabel" ConfigKey="SiteSettingsDefaultFriendlyUrlPatternLabel"
                            EnableViewState="false"></mp:SiteLabel>
                        <asp:DropDownList ID="ddDefaultFriendlyUrlPattern" runat="server"
                            TabIndex="10" CssClass="forminput">
                            <asp:ListItem Value="PageNameWithDotASPX" Text="<%$ Resources:Resource, UrlFormatAspx %>"></asp:ListItem>
                            <asp:ListItem Value="PageName" Text="<%$ Resources:Resource, UrlFormatExtensionless %>"></asp:ListItem>
                        </asp:DropDownList>
                        <portal:mojoHelpLink ID="MojoHelpLink4" runat="server" HelpKey="sitesettingsdefaultfriendlyurlpatternhelp" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel83" runat="server" ForControl="txtSlogan" CssClass="settinglabel"
                            ConfigKey="SloganLabel" EnableViewState="false" />
                        <asp:TextBox ID="txtSlogan" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox" />
                        <portal:mojoHelpLink ID="MojoHelpLink79" runat="server" HelpKey="site-slogan-help" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel19" runat="server" ForControl="txtSiteEmailFromAddress"
                            CssClass="settinglabel" ConfigKey="SiteSettingsSiteEmailFromAddressLabel" EnableViewState="false" />
                        <asp:TextBox ID="txtSiteEmailFromAddress" runat="server" TabIndex="10" MaxLength="100"
                            CssClass="forminput widetextbox" />
                        <portal:mojoHelpLink ID="MojoHelpLink6" runat="server" HelpKey="sitesettingssiteemailfromhelp" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel100" runat="server" ForControl="txtSiteEmailFromAlias"
                            CssClass="settinglabel" ConfigKey="SiteSettingsSiteEmailFromAliasLabel" EnableViewState="false" />
                        <asp:TextBox ID="txtSiteEmailFromAlias" runat="server" TabIndex="10" MaxLength="100"
                            CssClass="forminput widetextbox" />
                        <portal:mojoHelpLink ID="MojoHelpLink95" runat="server" HelpKey="sitesettingssiteemailfromaliashelp" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel29" runat="server" ForControl="ddEditorProviders" CssClass="settinglabel"
                            ConfigKey="SiteSettingsEditorProviderLabel" EnableViewState="false"></mp:SiteLabel>
                        <asp:DropDownList ID="ddEditorProviders" DataTextField="name" DataValueField="name"
                            EnableViewState="true" TabIndex="10" runat="server" CssClass="forminput">
                        </asp:DropDownList>
                        <portal:mojoHelpLink ID="MojoHelpLink7" runat="server" HelpKey="sitesettingssiteeditorproviderhelp" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel78" runat="server" ForControl="ddNewsletterEditor" CssClass="settinglabel"
                            ConfigKey="NewsletterEditorLabel" EnableViewState="false"></mp:SiteLabel>
                        <asp:DropDownList ID="ddNewsletterEditor" DataTextField="name" DataValueField="name"
                            EnableViewState="true" TabIndex="10" runat="server" CssClass="forminput">
                        </asp:DropDownList>
                        <portal:mojoHelpLink ID="MojoHelpLink49" runat="server" HelpKey="newletter-editor-help" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel74" runat="server" ForControl="ddAvatarSystem" CssClass="settinglabel"
                            ConfigKey="AvatarSystemLabel" EnableViewState="false"></mp:SiteLabel>
                        <asp:DropDownList ID="ddAvatarSystem" DataTextField="name" DataValueField="name"
                            EnableViewState="true"  TabIndex="10" runat="server" CssClass="forminput">
                            <asp:ListItem Value="none" Text="<%$ Resources:Resource, AvatarTypeNone %>"></asp:ListItem>
                            <asp:ListItem Value="internal" Text="<%$ Resources:Resource, AvatarTypeInternal %>"></asp:ListItem>
                            <asp:ListItem Value="gravatar" Text="<%$ Resources:Resource, AvatarTypeGravatar %>"></asp:ListItem>    
                        </asp:DropDownList>
                        <portal:mojoHelpLink ID="MojoHelpLink8" runat="server" HelpKey="avatarsystem-help" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="Sitelabel85" runat="server" ForControl="chkAllowUserEditorChoice" CssClass="settinglabel"
                            ConfigKey="AllowUserEditorLabel" EnableViewState="false"></mp:SiteLabel>
                        <asp:CheckBox ID="chkAllowUserEditorChoice" runat="server" TabIndex="10" CssClass="forminput" />
                        <portal:mojoHelpLink ID="MojoHelpLink80" runat="server" HelpKey="sitesetting-user-editor-help" />
                    </div>
                    <div id="divAllowUserSkins" runat="server" class="settingrow">
                        <mp:SiteLabel ID="Sitelabel2" runat="server" ForControl="chkAllowUserSkins" CssClass="settinglabel"
                            ConfigKey="SiteSettingsAllowUserSkinsLabel" EnableViewState="false"></mp:SiteLabel>
                        <asp:CheckBox ID="chkAllowUserSkins" runat="server" TabIndex="10" CssClass="forminput" />
                        <portal:mojoHelpLink ID="MojoHelpLink9" runat="server" HelpKey="sitesettingsuserskinhelp" />
                    </div>
                    <div id="divAllowPageSkins" runat="server" class="settingrow">
                        <mp:SiteLabel ID="Sitelabel2x" runat="server" ForControl="chkAllowPageSkins" CssClass="settinglabel"
                            ConfigKey="SiteSettingsAllowPageSkinsLabel" EnableViewState="false"></mp:SiteLabel>
                        <asp:CheckBox ID="chkAllowPageSkins" runat="server" TabIndex="10" CssClass="forminput" />
                        <portal:mojoHelpLink ID="MojoHelpLink10" runat="server" HelpKey="sitesettingspageskinhelp" />
                    </div>
                    <div id="divAllowHideMenu" runat="server" class="settingrow">
                        <mp:SiteLabel ID="Sitelabel2y" runat="server" ForControl="chkAllowHideMenuOnPages"
                            CssClass="settinglabel" ConfigKey="SiteSettingsAllowHideMainMenuLabel"></mp:SiteLabel>
                        <asp:CheckBox ID="chkAllowHideMenuOnPages" runat="server" TabIndex="10" CssClass="forminput" />
                        <portal:mojoHelpLink ID="MojoHelpLink11" runat="server" HelpKey="sitesettingsallowhidemenuhelp" />
                    </div>
                    <div id="divMyPage" runat="server" class="settingrow">
                        <mp:SiteLabel ID="Sitelabel20" ForControl="chkEnableMyPageFeature" runat="server"
                            CssClass="settinglabel" ConfigKey="SiteSettingsEnableMyPageFeatureLabel" EnableViewState="false">
                        </mp:SiteLabel>
                        <asp:CheckBox ID="chkEnableMyPageFeature" runat="server" TabIndex="10" CssClass="forminput" />
                        <portal:mojoHelpLink ID="MojoHelpLink12" runat="server" HelpKey="sitesettingsmypagehelp" />
                    </div>
                    <div id="divMyPageSkin" runat="server" class="settingrow">
                        <mp:SiteLabel ID="SiteLabel45" runat="server" CssClass="settinglabel" ConfigKey="MyPageSkinLabel">
                        </mp:SiteLabel>
                        <asp:DropDownList ID="ddMyPageSkin" runat="server" DataValueField="Name"
                            DataTextField="Name" CssClass="forminput" TabIndex="10">
                        </asp:DropDownList>
                        <portal:mojoHelpLink ID="MojoHelpLink13" runat="server" HelpKey="mypageskinhelp" />
                    </div>
                    <div id="divSSL" runat="server" class="settingrow">
                        <mp:SiteLabel ID="Sitelabel3" runat="server" ForControl="chkRequireSSL" CssClass="settinglabel"
                            ConfigKey="SiteSettingsRequireSSLLabel" />
                        <asp:CheckBox ID="chkRequireSSL" runat="server" TabIndex="10" CssClass="forminput" />
                        <portal:mojoHelpLink ID="MojoHelpLink14" runat="server" HelpKey="sitesettingsrequiresslhelp" />
                    </div>
                    <div id="divReallyDeleteUsers" runat="server" class="settingrow">
                        <mp:SiteLabel ID="SitelabelReallyDeleteUsers" runat="server" ForControl="chkReallyDeleteUsers"
                            CssClass="settinglabel" ConfigKey="SiteSettingsReallyDeleteUsersLabel" />
                        <asp:CheckBox ID="chkReallyDeleteUsers" runat="server" TabIndex="10" CssClass="forminput" /><mp:SiteLabel
                            ID="SitelabelReallyDeleteUsersExplain" runat="server" ConfigKey="SiteSettingsReallyDeleteUsersExplainLabel" />
                        <portal:mojoHelpLink ID="MojoHelpLink15" runat="server" HelpKey="sitesettingsreallydeleteusershelp" />
                    </div>
                    <div id="divContentVersioning" runat="server" class="settingrow">
                        <mp:SiteLabel ID="Sitelabel48" runat="server" ForControl="chkForceContentVersioning"
                            CssClass="settinglabel" ConfigKey="ForceContentVersioning" />
                        <asp:CheckBox ID="chkForceContentVersioning" runat="server" TabIndex="10" CssClass="forminput" /><mp:SiteLabel
                            ID="Sitelabel49" runat="server" />
                        <portal:mojoHelpLink ID="MojoHelpLink16" runat="server" HelpKey="sitesettingsforcecontentversioninghelp" />
                    </div>
                    <div id="divApprovalsWorkflow" runat="server" class="settingrow">
                        <mp:SiteLabel ID="Sitelabel59" runat="server" ForControl="chkEnableContentWorkflow"
                            CssClass="settinglabel" ConfigKey="EnableContentWorkflow" />
                        <asp:CheckBox ID="chkEnableContentWorkflow" runat="server" TabIndex="10" CssClass="forminput" /><mp:SiteLabel
                            ID="Sitelabel57" runat="server" />
                        <portal:mojoHelpLink ID="MojoHelpLink67" runat="server" HelpKey="sitesettingsenablecontentworkflowhelp" />
                    </div>
                    <div class="settingrow" id="divPreferredHostName" runat="server">
                        <mp:SiteLabel ID="SiteLabel24" runat="server" ForControl="txtPreferredHostName" CssClass="settinglabel"
                            ConfigKey="SiteSettingsPreferredHostNameLabel" />
                        <asp:TextBox ID="txtPreferredHostName" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox"
                            runat="server" />
                        <portal:mojoHelpLink ID="MojoHelpLink17" runat="server" HelpKey="sitesettingspreferredhostnamehelp" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel53" runat="server" ForControl="txtPrivacyPolicyUrl" CssClass="settinglabel"
                            ConfigKey="SiteSettingsPrivacyUrlLabel" EnableViewState="false" />
                        <asp:Label ID="lblPrivacySiteRoot" runat="server" CssClass="forminput" />
                        <asp:TextBox ID="txtPrivacyPolicyUrl" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox"
                            runat="server" />
                        <portal:mojoHelpLink ID="MojoHelpLink62" runat="server" HelpKey="sitesettingsprivacyhelp" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel56" runat="server" ForControl="txtOpenSearchName" CssClass="settinglabel"
                            ConfigKey="SiteSettingsOpenSearchNameLabel" EnableViewState="false" />
                        <asp:TextBox ID="txtOpenSearchName" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox"
                            runat="server" />
                        <portal:mojoHelpLink ID="MojoHelpLink65" runat="server" HelpKey="sitesettings-opensearchname-help" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel79" runat="server" ForControl="txtMetaProfile" CssClass="settinglabel"
                            ConfigKey="MetaProfileLabel" EnableViewState="false" />
                        <asp:TextBox ID="txtMetaProfile" TabIndex="10" CssClass="forminput verywidetextbox"
                            runat="server" />
                        <portal:mojoHelpLink ID="MojoHelpLink50" runat="server" HelpKey="meta-profile-help" />
                    </div>
                    <div class="settingrow" id="div2" runat="server">
                        &nbsp;<br />
                    </div>
                </div>
                <div id="tabSecurity" runat="server">
                    <div id="divSecurityTabs" class="mojo-tabs">
                        <ul>
                            <li class="selected" id="liGeneralSecurity" runat="server" enableviewstate="false"><asp:Literal ID="litGeneralSecurityTabLink" runat="server" EnableViewState="false" /></li>
                            <li id="liLDAP" runat="server" enableviewstate="false"><asp:Literal ID="litLDAPTabLink" runat="server" EnableViewState="false" /></li>
                            <li id="liOpenID" runat="server" enableviewstate="false"><asp:Literal ID="litOpenIDTabLink" runat="server" EnableViewState="false" /></li>
                            <li id="liWindowsLive" runat="server" enableviewstate="false"><asp:Literal ID="litWindowsLiveTabLink" runat="server" EnableViewState="false" /></li>
                            <li id="liCaptcha" runat="server"><a href="#tabAntiSpam"><asp:Literal ID="litAntiSpamTab" runat="server" EnableViewState="false" /></a></li>
                        </ul>
                                
                            <div id="tabGeneralSecurity" runat="server">
                                <asp:Panel ID="pnlUserSecurity" runat="server" SkinID="plain">
                                    
                                    <div id="divAllowRegistration" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="Sitelabel1" runat="server" ForControl="chkAllowRegistration" CssClass="settinglabel"
                                            ConfigKey="SiteSettingsAllowRegistrationLabel" />
                                        <asp:CheckBox ID="chkAllowRegistration" runat="server" TabIndex="10" CssClass="forminput" />
                                        <portal:mojoHelpLink ID="MojoHelpLink18" runat="server" HelpKey="sitesettingsallowregistrationhelp" />
                                    </div>
                                    <div id="divUseEmailForLogin" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="Sitelabelemailforlogin" runat="server" ForControl="chkUseEmailForLogin"
                                            CssClass="settinglabel" ConfigKey="SiteSettingsUseEmailForLoginLabel" />
                                        <asp:CheckBox ID="chkUseEmailForLogin" runat="server" TabIndex="10" CssClass="forminput" />
                                        <portal:mojoHelpLink ID="MojoHelpLink19" runat="server" HelpKey="sitesettingsuseemailforloginhelp" />
                                    </div>
                                    <div id="divAllowPersistentLogin" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="Sitelabel103" runat="server" ForControl="chkAllowPersistentLogin"
                                            CssClass="settinglabel" ConfigKey="AllowPersistentLogin" />
                                        <asp:CheckBox ID="chkAllowPersistentLogin" runat="server" TabIndex="10" CssClass="forminput" />
                                        <portal:mojoHelpLink ID="MojoHelpLink98" runat="server" HelpKey="sitesettings-AllowPersistentLogin-help" />
                                    </div>
                                    <div id="div3" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="Sitelabel92" runat="server" ForControl="chkRequireEmailTwice"
                                            CssClass="settinglabel" ConfigKey="RequireEmailTwiceOnRegistration" />
                                        <asp:CheckBox ID="chkRequireEmailTwice" runat="server" TabIndex="10" CssClass="forminput" />
                                        <portal:mojoHelpLink ID="MojoHelpLink87" runat="server" HelpKey="RequireEmailTwice-help" />
                                    </div>
                                    <div id="divSecureRegistration" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="lblSecureRegistration" runat="server" ForControl="chkSecureRegistration"
                                            CssClass="settinglabel" ConfigKey="SiteSettingsSecureRegistrationLabel" />
                                        <asp:CheckBox ID="chkSecureRegistration" runat="server" TabIndex="10" CssClass="forminput" />
                                        <portal:mojoHelpLink ID="MojoHelpLink20" runat="server" HelpKey="sitesettingssecureregistrationhelp" />
                                    </div>
                                    <div  class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel96" runat="server" ForControl="chkRequireApprovalForLogin"
                                            CssClass="settinglabel" ConfigKey="SiteSettingsRequireApprovalForLogin" />
                                        <asp:CheckBox ID="chkRequireApprovalForLogin" runat="server" TabIndex="10" CssClass="forminput" />
                                        <portal:mojoHelpLink ID="MojoHelpLink91" runat="server" HelpKey="sitesettings-requireapprovalforlogin-help" />
                                    </div>
                                    <div class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel97" runat="server" ForControl="txtEmailAdressesForUserApprovalNotification"
                                            CssClass="settinglabel" ConfigKey="EmailAddressesForUserApprovalNotification" />
                                        <asp:TextBox ID="txtEmailAdressesForUserApprovalNotification" TabIndex="10" 
                                            CssClass="forminput verywidetextbox" runat="server" />
                                        <portal:mojoHelpLink ID="MojoHelpLink92" runat="server" HelpKey="sitesettings-EmailAdressesForUserApprovalNotification-help" />
                                    </div>
                                    <div id="divAllowUserToChangeName" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="SitelabelAllowUserToChangeName" runat="server" ForControl="chkAllowUserToChangeName"
                                            CssClass="settinglabel" ConfigKey="SiteSettingsAllowUsersToChangeNameLabel" />
                                        <asp:CheckBox ID="chkAllowUserToChangeName" runat="server" TabIndex="10" CssClass="forminput" />
                                        <portal:mojoHelpLink ID="MojoHelpLink21" runat="server" HelpKey="sitesettingsallowusernamechangehelp" />
                                    </div>
                                    <div id="divDisableDbAuthentication" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="Sitelabel76" runat="server" ForControl="chkDisableDbAuthentication"
                                            CssClass="settinglabel" ConfigKey="DisableDbAuthentication" />
                                        <asp:CheckBox ID="chkDisableDbAuthentication" runat="server" TabIndex="10" CssClass="forminput" />
                                        <portal:mojoHelpLink ID="MojoHelpLink76" runat="server" HelpKey="sitesettings-DisableDbAuthentication-help" />
                                    </div>
                                    <div class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel11" runat="server" ForControl="ddPasswordFormat" CssClass="settinglabel"
                                            ConfigKey="SiteSettingsPasswordFormatLabel"></mp:SiteLabel>
                                        <asp:DropDownList ID="ddPasswordFormat"  runat="server" TabIndex="10"
                                            CssClass="forminput">
                                        </asp:DropDownList>
                                        <portal:mojoHelpLink ID="MojoHelpLink22" runat="server" HelpKey="sitesettingspasswordformathelp" />
                                    </div>
                                    <div id="divPasswordRecovery" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="lbl1" runat="server" ForControl="chkAllowPasswordRetrieval" CssClass="settinglabel"
                                            ConfigKey="SiteSettingsAllowPasswordRetrievalLabel" />
                                        <asp:CheckBox ID="chkAllowPasswordRetrieval" runat="server" TabIndex="10" CssClass="forminput" />
                                        <portal:mojoHelpLink ID="MojoHelpLink23" runat="server" HelpKey="sitesettingsallowpasswordretrievalhelp" />
                                    </div>
                                    <div id="divAllowPasswordReset" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel12" runat="server" ForControl="chkAllowPasswordReset"
                                            CssClass="settinglabel" ConfigKey="SiteSettingsAllowPasswordResetLabel" />
                                        <asp:CheckBox ID="chkAllowPasswordReset" runat="server" TabIndex="10" CssClass="forminput" />
                                        <portal:mojoHelpLink ID="MojoHelpLink25" runat="server" HelpKey="sitesettingsallowpasswordresethelp" />
                                    </div>
                                    <div id="divForcePasswordChangeOnRecovery"  runat="server" class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel95" runat="server" ForControl="chkRequirePasswordChangeAfterRecovery"
                                            CssClass="settinglabel" ConfigKey="SiteSettingsRequirePasswordChangeOnRevovery" />
                                        <asp:CheckBox ID="chkRequirePasswordChangeAfterRecovery" runat="server" TabIndex="10" CssClass="forminput" />
                                        <portal:mojoHelpLink ID="MojoHelpLink90" runat="server" HelpKey="sitesettings-requirepasswordchangeafterrecovery-help" />
                                    </div>
                                    <div id="div1" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="Sitelabel16" runat="server" ForControl="chkRequiresQuestionAndAnswer"
                                            CssClass="settinglabel" ConfigKey="SiteSettingsRequiresQuestionAndAnswerLabel" />
                                        <asp:CheckBox ID="chkRequiresQuestionAndAnswer" runat="server" TabIndex="10" CssClass="forminput" />
                                        <portal:mojoHelpLink ID="MojoHelpLink24" runat="server" HelpKey="sitesettingsrequirequestionandanswerhelp" />
                                    </div>
                                            
                                    <div class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel17" runat="server" ForControl="txtMaxInvalidPasswordAttempts"
                                            CssClass="settinglabel" ConfigKey="SiteSettingsMaxInvalidPasswordAttemptsLabel" />
                                        <asp:TextBox ID="txtMaxInvalidPasswordAttempts" TabIndex="10" MaxLength="2" Columns="10"
                                            CssClass="forminput smalltextbox" runat="server" />
                                        <portal:mojoHelpLink ID="MojoHelpLink26" runat="server" HelpKey="sitesettingsmaxincalidpasswordhelp" />
                                    </div>
                                    <div class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel18" runat="server" ForControl="txtPasswordAttemptWindowMinutes"
                                            CssClass="settinglabel" ConfigKey="SiteSettingsPasswordAttemptWindowMinutesLabel" />
                                        <asp:TextBox ID="txtPasswordAttemptWindowMinutes" TabIndex="10" MaxLength="2" Columns="10"
                                            CssClass="forminput smalltextbox" runat="server" />
                                        <portal:mojoHelpLink ID="MojoHelpLink27" runat="server" HelpKey="sitesettingspasswordattemptwindowhelp" />
                                    </div>
                                    <div class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel13" runat="server" ForControl="txtMinimumPasswordLength"
                                            CssClass="settinglabel" ConfigKey="SiteSettingsMinimumPasswordLengthLabel" />
                                        <asp:TextBox ID="txtMinimumPasswordLength" TabIndex="10" MaxLength="2" Columns="10"
                                            CssClass="forminput smalltextbox" runat="server" Text="7" />
                                        <portal:mojoHelpLink ID="MojoHelpLink28" runat="server" HelpKey="sitesettingspasswordlengthhelp" />
                                    </div>
                                    <div class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel14" runat="server" ForControl="txtMinRequiredNonAlphaNumericCharacters"
                                            CssClass="settinglabel" ConfigKey="SiteSettingsMinRequiredNonAlphaNumericCharactersLabel" />
                                        <asp:TextBox ID="txtMinRequiredNonAlphaNumericCharacters" TabIndex="10" MaxLength="2"
                                            CssClass="forminput smalltextbox" Columns="10" runat="server" Text="0" />
                                        <portal:mojoHelpLink ID="MojoHelpLink29" runat="server" HelpKey="sitesettingspasswordnonalphacharactershelp" />
                                    </div>
                                    <div id="divShowPasswordStrength" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel90" runat="server" ForControl="chkShowPasswordStrength"
                                            CssClass="settinglabel" ConfigKey="ShowPasswordStrengthOnRegistrationPage" />
                                        <asp:CheckBox ID="chkShowPasswordStrength" runat="server" TabIndex="10" CssClass="forminput" />
                                        <portal:mojoHelpLink ID="MojoHelpLink85" runat="server" HelpKey="ShowPasswordStrengthOnRegistrationPage-help" />
                                    </div>
                                    <div class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel15" runat="server" ForControl="txtPasswordStrengthRegularExpression"
                                            CssClass="settinglabel" ConfigKey="SiteSettingsPasswordStrengthExpressionLabel" />
                                        <asp:TextBox ID="txtPasswordStrengthRegularExpression" TabIndex="10" TextMode="MultiLine" SkinID="regex"
                                            Rows="3"  runat="server" CssClass="forminput pwdregex" />
                                        <portal:mojoHelpLink ID="MojoHelpLink30" runat="server" HelpKey="sitesettingspasswordstrengthhelp" />
                                    </div>
                                    <div class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel94" runat="server" ForControl="txtPasswordStrengthErrorMessage"
                                            CssClass="settinglabel" ConfigKey="SiteSettingsPasswordStrengthErrorMessage" />
                                        <asp:TextBox ID="txtPasswordStrengthErrorMessage" TabIndex="10" runat="server" CssClass="forminput verywidetextbox" />
                                        <portal:mojoHelpLink ID="MojoHelpLink89" runat="server" HelpKey="sitesettingspasswordstrength-errormessage-help" />
                                    </div>
                                    <div class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel91" runat="server" ForControl="chkRequireCaptcha"
                                            CssClass="settinglabel" ConfigKey="RequireCaptchaOnRegistrationPage" />
                                        <asp:CheckBox ID="chkRequireCaptcha" runat="server" TabIndex="10" CssClass="forminput" />
                                        <portal:mojoHelpLink ID="MojoHelpLink86" runat="server" HelpKey="RequireCaptchaOnRegistrationPage-help" />
                                    </div>
                                    <div class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel93" runat="server" ForControl="chkRequireCaptchaOnLogin"
                                            CssClass="settinglabel" ConfigKey="RequireCaptchaOnLoginPage" />
                                        <asp:CheckBox ID="chkRequireCaptchaOnLogin" runat="server" TabIndex="10" CssClass="forminput" />
                                        <portal:mojoHelpLink ID="MojoHelpLink88" runat="server" HelpKey="RequireCaptchaOnLoginPage-help" />
                                    </div>
                                    <div class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel63" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                                        </mp:SiteLabel>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div id="tabLDAP" runat="server">
                                    
                                <asp:Panel ID="pnlLdapSettings" runat="server" SkinID="plain">
                                    <div id="divUseLdap" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="lblUseLdapAuth" ForControl="chkUseLdapAuth" CssClass="settinglabel"
                                            ConfigKey="SiteSettingsUseLdapAuth" runat="server"></mp:SiteLabel>
                                        <asp:CheckBox ID="chkUseLdapAuth" runat="server" TabIndex="10" CssClass="forminput">
                                        </asp:CheckBox>
                                        <portal:mojoHelpLink ID="MojoHelpLink31" runat="server" HelpKey="sitesettingsuseldaphelp" />
                                    </div>
                                    <div id="divLdapTestPassword" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="Sitelabel9" ForControl="txtLdapTestPassword" ConfigKey="SiteSettingsLdapTestPassword"
                                            CssClass="settinglabel" runat="server"></mp:SiteLabel>
                                        <asp:TextBox ID="txtLdapTestPassword" Columns="55" TabIndex="10" runat="server" TextMode="password"
                                            CssClass="forminput normaltextbox" MaxLength="255"></asp:TextBox>
                                        <portal:mojoHelpLink ID="MojoHelpLink32" runat="server" HelpKey="sitesettingsldappasswordhelp" />
                                        <br />
                                        <br />
                                    </div>
                                    <div id="divAutoCreateLdapUsers" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="lblAutoCreateLdapUser" ForControl="chkAutoCreateLdapUserOnFirstLogin"
                                            CssClass="settinglabel" ConfigKey="SiteSettingsAutoCreateLdapUserOnFirstLoginLabel"
                                            runat="server"></mp:SiteLabel>
                                        <asp:CheckBox ID="chkAutoCreateLdapUserOnFirstLogin" runat="server" TabIndex="10"
                                            CssClass="forminput"></asp:CheckBox>
                                        <portal:mojoHelpLink ID="MojoHelpLink33" runat="server" HelpKey="sitesettingsautocreateldapuserhelp" />
                                    </div>
                                    <div id="divLdapServer" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="lblLdapServer" ForControl="txtLdapServer" CssClass="settinglabel"
                                            ConfigKey="SiteSettingsLdapServer" runat="server"></mp:SiteLabel>
                                        <asp:TextBox ID="txtLdapServer" Columns="55" runat="server" TabIndex="10" MaxLength="255"
                                            CssClass="forminput widetextbox"></asp:TextBox>
                                        <portal:mojoHelpLink ID="MojoHelpLink34" runat="server" HelpKey="sitesettingsldapserverhelp" />
                                    </div>
                                    <div id="divLdapPort" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="lblLdapPort" ForControl="txtLdapPort" CssClass="settinglabel" ConfigKey="SiteSettingsLdapPort"
                                            runat="server"></mp:SiteLabel>
                                        <asp:TextBox ID="txtLdapPort" Columns="55" runat="server" TabIndex="10" MaxLength="255"
                                            CssClass="forminput smalltextbox"></asp:TextBox>
                                        <portal:mojoHelpLink ID="MojoHelpLink35" runat="server" HelpKey="sitesettingsldapporthelp" />
                                    </div>
                                    <div id="divLdapDomain" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="Sitelabel26" ForControl="txtLdapDomain" CssClass="settinglabel"
                                            ConfigKey="SiteSettingsLdapDomain" runat="server"></mp:SiteLabel>
                                        <asp:TextBox ID="txtLdapDomain" Columns="55" runat="server" TabIndex="10" MaxLength="255"
                                            CssClass="forminput widetextbox"></asp:TextBox>
                                        <portal:mojoHelpLink ID="MojoHelpLink36" runat="server" HelpKey="sitesettingsldapdomainhelp" />
                                    </div>
                                    <div id="divLdapRootDn" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="lblLdapRootDN" ForControl="txtLdapRootDN" CssClass="settinglabel"
                                            ConfigKey="SiteSettingsLdapRootDN" runat="server"></mp:SiteLabel>
                                        <asp:TextBox ID="txtLdapRootDN" Columns="55" runat="server" TabIndex="10" MaxLength="255"
                                            CssClass="forminput widetextbox"></asp:TextBox>
                                        <portal:mojoHelpLink ID="MojoHelpLink37" runat="server" HelpKey="sitesettingsldaprootdnhelp" />
                                    </div>
                                    <div id="divLdapUserDNKey" runat="server" class="settingrow">
                                        <mp:SiteLabel ID="Sitelabel8" ForControl="ddLdapUserDNKey" CssClass="settinglabel"
                                            ConfigKey="SiteSettingsLdapUserDNKey" runat="server"></mp:SiteLabel>
                                        <asp:DropDownList ID="ddLdapUserDNKey" runat="server" TabIndex="10"
                                            CssClass="forminput">
                                            <asp:ListItem Value="uid">uid (OpenLDAP)</asp:ListItem>
                                            <asp:ListItem Value="CN">CN (Active Directory)</asp:ListItem>
                                        </asp:DropDownList>
                                        <portal:mojoHelpLink ID="MojoHelpLink38" runat="server" HelpKey="sitesettingsldapuserdnkeyhelp" />
                                    </div>
                                    <div class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel72" ForControl="chkAllowDbFallbackWithLdap" CssClass="settinglabel"
                                            ConfigKey="AllowDbFallbackWithLdap" runat="server"></mp:SiteLabel>
                                        <asp:CheckBox ID="chkAllowDbFallbackWithLdap" runat="server" TabIndex="10" CssClass="forminput">
                                        </asp:CheckBox>
                                        <portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="sitesetting-AllowDbFallbackWithLdap-help" />
                                    </div>
                                    <div class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel73" ForControl="chkAllowEmailLoginWithLdapDbFallback" CssClass="settinglabel"
                                            ConfigKey="AllowEmailLoginWithLdapDbFallback" runat="server"></mp:SiteLabel>
                                        <asp:CheckBox ID="chkAllowEmailLoginWithLdapDbFallback" runat="server" TabIndex="10" CssClass="forminput">
                                        </asp:CheckBox>
                                        <portal:mojoHelpLink ID="MojoHelpLink66" runat="server" HelpKey="sitesetting-AllowEmailLoginWithLdapDbFallback-help" />
                                    </div>
                                    <div class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel62" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                                        </mp:SiteLabel>
                                    </div>
                                </asp:Panel>
                                       
                            </div>
                            <div id="tabOpenID" runat="server">
                                    
                                <div id="divOpenID" runat="server" class="settingrow">
                                    <mp:SiteLabel ID="Sitelabel31" runat="server" ForControl="chkAllowOpenIDAuth" CssClass="settinglabel"
                                        ConfigKey="SiteSettingsAllowOpenIDAuthenticationLabel" />
                                    <asp:CheckBox ID="chkAllowOpenIDAuth" runat="server" TabIndex="10" CssClass="forminput" />
                                    <portal:mojoHelpLink ID="MojoHelpLink39" runat="server" HelpKey="sitesettingsopenidhelp" />
                                </div>
                                <div id="divOpenIDSelector" runat="server" visible="false" class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel27" ForControl="txtOpenIdSelectorCode" CssClass="settinglabel"
                                        ConfigKey="SiteSettingsOpenIdSelectorLabel" runat="server"></mp:SiteLabel>
                                    <asp:TextBox ID="txtOpenIdSelectorCode" Columns="55" runat="server" TabIndex="10"
                                        MaxLength="255" CssClass="forminput widetextbox"></asp:TextBox>
                                    <portal:mojoHelpLink ID="MojoHelpLink40" runat="server" HelpKey="sitesettingsopenidselectorhelp" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel54" ForControl="txtRpxNowApiKey" CssClass="settinglabel"
                                        ConfigKey="RpxNowApiKeyLabel" runat="server"></mp:SiteLabel>
                                    <asp:TextBox ID="txtRpxNowApiKey" Columns="55" runat="server" MaxLength="255" CssClass="forminput widetextbox"></asp:TextBox>
                                    <portal:mojoHelpLink ID="MojoHelpLink63" runat="server" HelpKey="rpxnow-apikey-help" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel55" ForControl="txtRpxNowApplicationName" CssClass="settinglabel"
                                        ConfigKey="RpxNowApplicationNameLabel" runat="server"></mp:SiteLabel>
                                    <asp:TextBox ID="txtRpxNowApplicationName" Columns="55" runat="server" MaxLength="255"
                                        CssClass="forminput widetextbox"></asp:TextBox>
                                    <portal:mojoHelpLink ID="MojoHelpLink64" runat="server" HelpKey="rpxnow-applicationname-help" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel61" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                                    </mp:SiteLabel>
                                </div>
                                <div class="settingrow">
                                    <asp:HyperLink ID="lnkRpxAdmin" runat="server" Visible="false" />
                                    <portal:mojoButton ID="btnSetupRpx" runat="server" />
                                </div>
                                       
                            </div>
                            <div id="tabWindowsLiveID" runat="server">
                                    
                                <div class="settingrow">
                                    <mp:SiteLabel ID="Sitelabel32" runat="server" ForControl="chkAllowWindowsLiveAuth"
                                        CssClass="settinglabel" ConfigKey="SiteSettingsAllowWindowsLiveAuthLabel"></mp:SiteLabel>
                                    <asp:CheckBox ID="chkAllowWindowsLiveAuth" runat="server" TabIndex="10" CssClass="forminput" />
                                    <portal:mojoHelpLink ID="MojoHelpLink41" runat="server" HelpKey="sitesettingswindowslivehelp" />
                                </div>
                                <div id="divLiveMessenger" runat="server" class="settingrow">
                                    <mp:SiteLabel ID="Sitelabel50" runat="server" ForControl="chkAllowWindowsLiveMessengerForMembers"
                                        CssClass="settinglabel" ConfigKey="AllowLiveMessengerOnProfilesLabel"></mp:SiteLabel>
                                    <asp:CheckBox ID="chkAllowWindowsLiveMessengerForMembers" runat="server" TabIndex="10"
                                        CssClass="forminput" />
                                    <portal:mojoHelpLink ID="MojoHelpLink42" runat="server" HelpKey="livemessenger-admin-help" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel33" runat="server" ForControl="txtWindowsLiveAppID" CssClass="settinglabel"
                                        ConfigKey="SiteSettingsWindowsLiveAppIDLabel" />
                                    <asp:TextBox ID="txtWindowsLiveAppID" TabIndex="10" MaxLength="100" Columns="45"
                                        CssClass="forminput widetextbox" runat="server" />
                                    <portal:mojoHelpLink ID="MojoHelpLink43" runat="server" HelpKey="sitesettingswindowslivehelp" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel34" runat="server" ForControl="txtWindowsLiveKey" CssClass="settinglabel"
                                        ConfigKey="SiteSettingsWindowsLiveKeyLabel" />
                                    <asp:TextBox ID="txtWindowsLiveKey" TabIndex="10" MaxLength="100" Columns="45" runat="server"
                                        CssClass="forminput widetextbox" />
                                    <portal:mojoHelpLink ID="MojoHelpLink44" runat="server" HelpKey="sitesettingswindowslivehelp" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel51" runat="server" ForControl="txtAppLogoForWindowsLive"
                                        CssClass="settinglabel" ConfigKey="WindowsLiveAppLogoLabel" />
                                    <asp:Label ID="lblSiteRoot" runat="server" />
                                    <asp:TextBox ID="txtAppLogoForWindowsLive" TabIndex="10" MaxLength="100" Columns="45"
                                        runat="server" CssClass="forminput widetextbox" />
                                    <portal:mojoHelpLink ID="MojoHelpLink45" runat="server" HelpKey="windowslive-applogo-help" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel60" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                                    </mp:SiteLabel>
                                </div>
                                       
                            </div>
                            <div id="tabAntiSpam">
                                    
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel6" runat="server" ForControl="ddCaptchaProviders" CssClass="settinglabel"
                                        ConfigKey="SiteSettingsCaptchaProviderLabel"></mp:SiteLabel>
                                    <asp:DropDownList ID="ddCaptchaProviders" DataTextField="name" DataValueField="name"
                                        EnableViewState="true" TabIndex="10" runat="server" CssClass="forminput">
                                    </asp:DropDownList>
                                    <portal:mojoHelpLink ID="MojoHelpLink46" runat="server" HelpKey="sitesettingscaptchaproviderhelp" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel7" runat="server" ForControl="txtRecaptchPrivateKey" CssClass="settinglabel"
                                        ConfigKey="SiteSettingsSiteRecaptchaPrivateKeyLabel" />
                                    <asp:TextBox ID="txtRecaptchaPrivateKey" TabIndex="10" MaxLength="100" Columns="45"
                                        CssClass="forminput verywidetextbox" runat="server" />
                                    <portal:mojoHelpLink ID="MojoHelpLink47" runat="server" HelpKey="sitesettingsrecaptchahelp" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel30" runat="server" ForControl="txtRecaptchaPublicKey"
                                        CssClass="settinglabel" ConfigKey="SiteSettingsSiteRecaptchaPublicKeyLabel" />
                                    <asp:TextBox ID="txtRecaptchaPublicKey" TabIndex="10" MaxLength="100" Columns="45"
                                        CssClass="forminput verywidetextbox" runat="server" />
                                    <portal:mojoHelpLink ID="MojoHelpLink48" runat="server" HelpKey="sitesettingsrecaptchahelp" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel58" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                                    </mp:SiteLabel>
                                </div>
                                       
                            </div>
                        </div>
                    </div>
                <div id="tabCompanyInfo">
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel47" runat="server" ForControl="txtCompanyName" CssClass="settinglabel"
                            ConfigKey="SiteSettingsCompanyNameLabel" EnableViewState="false" />
                        <asp:TextBox ID="txtCompanyName" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput verywidetextbox" />
                        <portal:mojoHelpLink ID="MojoHelpLink5" runat="server" HelpKey="sitesettingscompanynamehelp" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel42" runat="server" ForControl="txtStreetAddress" CssClass="settinglabel"
                            ConfigKey="StreetAddress" EnableViewState="false" />
                        <asp:TextBox ID="txtStreetAddress" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput verywidetextbox" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel77" runat="server" ForControl="txtStreetAddress2" CssClass="settinglabel"
                            ConfigKey="StreetAddress2" EnableViewState="false" />
                        <asp:TextBox ID="txtStreetAddress2" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput verywidetextbox" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel43" runat="server" ForControl="txtLocality" CssClass="settinglabel"
                            ConfigKey="Locality" EnableViewState="false" />
                        <asp:TextBox ID="txtLocality" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel66" runat="server" ForControl="txtRegion" CssClass="settinglabel"
                            ConfigKey="Region" EnableViewState="false" />
                        <asp:TextBox ID="txtRegion" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel67" runat="server" ForControl="txtPostalCode" CssClass="settinglabel"
                            ConfigKey="PostalCode" EnableViewState="false" />
                        <asp:TextBox ID="txtPostalCode" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput normaltextbox" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel68" runat="server" ForControl="txtCountry" CssClass="settinglabel"
                            ConfigKey="Country" EnableViewState="false" />
                        <asp:TextBox ID="txtCountry" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel69" runat="server" ForControl="txtPhone" CssClass="settinglabel"
                            ConfigKey="Phone" EnableViewState="false" />
                        <asp:TextBox ID="txtPhone" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput normaltextbox" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel70" runat="server" ForControl="txtFax" CssClass="settinglabel"
                            ConfigKey="Fax" EnableViewState="false" />
                        <asp:TextBox ID="txtFax" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput normaltextbox" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel71" runat="server" ForControl="txtPublicEmail" CssClass="settinglabel"
                            ConfigKey="PublicEmail" EnableViewState="false" />
                        <asp:TextBox ID="txtPublicEmail" runat="server" TabIndex="10" MaxLength="100" CssClass="forminput widetextbox" />
                    </div>
                </div>
                <div id="tabCommerce" runat="server">
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblDefaultCountry" runat="server" CssClass="settinglabel" ConfigKey="DefaultCountryStateLabel" />
                        <div>
                            <asp:UpdatePanel ID="upCountryState" UpdateMode="Conditional" runat="server" EnableViewState="true">
                                <ContentTemplate>
                                    <div>
                                        <asp:DropDownList ID="ddDefaultCountry" runat="server" AutoPostBack="true" CssClass="countrylistdd"
                                            DataValueField="Guid" DataTextField="Name" EnableViewState="true">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddDefaultGeoZone" runat="server" DataValueField="Guid" CssClass="statelistdd"
                                            DataTextField="Name" EnableViewState="true">
                                        </asp:DropDownList>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel44" runat="server" ForControl="txtDefaultPageKeywords"
                            CssClass="settinglabel" ConfigKey="CurrencyLabel" />
                        <portal:CurrencySetting ID="SiteCurrencySetting" runat="server" />
                    </div>
                </div>
                        
                <div id="tabHosts" runat="server" visible="false">
                    <asp:UpdatePanel ID="upHosts" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlHostNames" runat="server" DefaultButton="btnAddHost">
                                <asp:TextBox ID="txtHostName" MaxLength="255" runat="server" CssClass="mediumtextbox" />
                                <portal:mojoButton ID="btnAddHost" runat="server"></portal:mojoButton>
                                <portal:mojoHelpLink ID="MojoHelpLink53" runat="server" HelpKey="sitesettingshostnamehelp" />
                                <br />
                                <br />
                                <portal:mojoLabel ID="lblHostMessage" runat="server" CssClass="txterror info" EnableViewState="false" />
                            </asp:Panel>
                            <h3>
                                <asp:Literal ID="litHostListHeader" runat="server" /></h3>
                            <asp:Repeater ID="rptHosts" runat="server">
                                <HeaderTemplate>
                                    <ul class="simplelist">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li>
                                        <asp:ImageButton ImageUrl='<%# DeleteLinkImage %>'
                                            CommandName="delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "HostID") %>'
                                            AlternateText="<%# Resources.Resource.SiteSettingsDeleteHostLabel %>" runat="server"
                                            ID="btnDeleteHost" />&nbsp;
                                        <%# DataBinder.Eval(Container.DataItem, "HostName") %>
                                    </li>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <li>
                                        <asp:ImageButton ImageUrl='<%# DeleteLinkImage %>'
                                            CommandName="delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "HostID") %>'
                                            AlternateText="<%# Resources.Resource.SiteSettingsDeleteHostLabel %>" runat="server"
                                            ID="btnDeleteHost" />&nbsp;
                                        <%# DataBinder.Eval(Container.DataItem, "HostName") %>
                                    </li>
                                    </ItemTemplate>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="tabFolderNames" runat="server" visible="false">
                    <asp:UpdatePanel ID="upFolderNames" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlAddFolder" runat="server" DefaultButton="btnAddFolder">
                                <asp:TextBox ID="txtFolderName" MaxLength="255" runat="server" CssClass="mediumtextbox" />
                                <portal:mojoButton ID="btnAddFolder" runat="server"></portal:mojoButton>
                                <portal:mojoHelpLink ID="MojoHelpLink54" runat="server" HelpKey="sitesettingsfoldernamehelp" />
                                <br />
                                <br />
                                <portal:mojoLabel ID="lblFolderMessage" runat="server" CssClass="txterror info" />
                            </asp:Panel>
                            <h3>
                                <asp:Literal ID="litFolderNamesListHeading" runat="server" EnableViewState="false" /></h3>
                            <asp:Repeater ID="rptFolderNames" runat="server">
                                <HeaderTemplate>
                                    <ul class="simplelist">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li>
                                        <asp:ImageButton ImageUrl='<%# DeleteLinkImage %>'
                                            CommandName="delete" ToolTip="<%# Resources.Resource.SiteSettingsDeleteFolderMapping %>"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Guid") %>' AlternateText="<%# Resources.Resource.SiteSettingsDeleteHostLabel %>"
                                            runat="server" ID="btnDeleteFolder" />&nbsp;
                                        <%# DataBinder.Eval(Container.DataItem, "FolderName") %>
                                    </li>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <li>
                                        <asp:ImageButton ImageUrl='<%# DeleteLinkImage %>'
                                            CommandName="delete" ToolTip="<%# Resources.Resource.SiteSettingsDeleteFolderMapping %>"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Guid") %>' AlternateText="<%# Resources.Resource.SiteSettingsDeleteHostLabel %>"
                                            runat="server" ID="btnDeleteFolder" />&nbsp;
                                        <%# DataBinder.Eval(Container.DataItem, "FolderName") %>
                                    </li>
                                    </ItemTemplate>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="tabSiteFeatures" runat="server" visible="false" class="minheightpanel">
                    <div style="height: 250px;">
                        <asp:UpdatePanel ID="upFeatures" UpdateMode="Conditional" runat="server" >
                            <ContentTemplate>
                                <portal:mojoHelpLink ID="MojoHelpLink55" runat="server" HelpKey="sitesettingschildsitefeatureshelp" />
                                <div class="floatpanel">
                                    <div>
                                        <h3>
                                            <mp:SiteLabel ID="Sitelabel4" runat="server" ConfigKey="SiteSettingsSiteAvailableFeaturesLabel"
                                                UseLabelTag="false" />
                                        </h3>
                                    </div>
                                    <div>
                                        <asp:ListBox ID="lstAllFeatures" runat="Server" Width="175" Height="175" SelectionMode="Multiple"  />
                                    </div>
                                </div>
                                <div class="floatpanel">
                                    <div>
                                        <asp:Button Text="<-" runat="server" ID="btnRemoveFeature" CausesValidation="false" />
                                        <asp:Button Text="->" runat="server" ID="btnAddFeature" CausesValidation="false" />
                                    </div>
                                </div>
                                <div class="floatpanel">
                                    <div>
                                        <h3>
                                            <mp:SiteLabel ID="Sitelabel5" runat="server" ConfigKey="SiteSettingsSiteSelectedFeaturesLabel"
                                                UseLabelTag="false" />
                                        </h3>
                                    </div>
                                    <div>
                                        <asp:ListBox ID="lstSelectedFeatures" runat="Server" Width="175" Height="175" SelectionMode="Multiple"  />
                                    </div>
                                    <div class="clearpanel">
                                        <portal:mojoLabel ID="lblFeatureMessage" runat="server" CssClass="txterror info" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div id="tabWebParts" runat="server" visible="false" class="minheightpanel">
                    <asp:UpdatePanel ID="upWebParts" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <portal:mojoHelpLink ID="MojoHelpLink56" runat="server" HelpKey="sitesettingschildsitefeatureshelp" />
                            <div class="floatpanel">
                                <div>
                                    <h3>
                                        <mp:SiteLabel ID="Sitelabel21" runat="server" ConfigKey="WebPartAdminAllWebParts"
                                            UseLabelTag="false" />
                                    </h3>
                                </div>
                                <div>
                                    <asp:ListBox ID="lstAllWebParts" runat="Server" Width="150" />
                                </div>
                            </div>
                            <div class="floatpanel">
                                <div>
                                    <asp:Button Text="<-" runat="server" ID="btnRemoveWebPart" />
                                    <asp:Button Text="->" runat="server" ID="btnAddWebPart" />
                                </div>
                            </div>
                            <div class="floatpanel">
                                <div>
                                    <h3>
                                        <mp:SiteLabel ID="Sitelabel22" runat="server" ConfigKey="WebPartAdminSelectedWebParts"
                                            UseLabelTag="false" />
                                    </h3>
                                </div>
                                <div>
                                    <asp:ListBox ID="lstSelectedWebParts" runat="Server" Width="150" />
                                </div>
                                <div>
                                    <portal:mojoLabel ID="lblWebPartMessage" runat="server" CssClass="txterror info" EnableViewState="false" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="tabApiKeys">

                    <div class="settingrow">
                    <mp:SiteLabel ID="SiteLabel86" runat="server" ForControl="ddSearchEngine" CssClass="settinglabel"
                            ConfigKey="DefaultSiteSearch" />
                        <asp:DropDownList ID="ddSearchEngine" runat="server" CssClass="forminput">
                            <asp:ListItem Value="internal" Text="<%$ Resources:Resource, InternalSearchEngine %>" />
                            <asp:ListItem Value="bing" Text="<%$ Resources:Resource, BingSiteSearch %>" />
                            <asp:ListItem Value="google" Text="<%$ Resources:Resource, GoogleSiteSearch %>" />
                        </asp:DropDownList>
                        <portal:mojoHelpLink ID="MojoHelpLink81" runat="server" HelpKey="default-search-engine-help" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel87" runat="server" ForControl="txtBingSearchAPIKey" CssClass="settinglabel"
                            ConfigKey="BingSearchApiKey" />
                        <asp:TextBox ID="txtBingSearchAPIKey" TabIndex="10" MaxLength="100" Columns="45" runat="server"
                            CssClass="forminput widetextbox" />
                        <portal:mojoHelpLink ID="MojoHelpLink82" runat="server" HelpKey="bing-search-apikey-help" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel88" runat="server" ForControl="txtGoogleCustomSearchId" CssClass="settinglabel"
                            ConfigKey="GoogleCustomSearchId" />
                        <asp:TextBox ID="txtGoogleCustomSearchId" TabIndex="10" MaxLength="100" Columns="45" runat="server"
                            CssClass="forminput widetextbox" />
                        <portal:mojoHelpLink ID="MojoHelpLink83" runat="server" HelpKey="google-custom-searchid-help" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel89" runat="server" ForControl="chkShowAlternateSearchIfConfigured" CssClass="settinglabel"
                            ConfigKey="ShowAlternateSearchIfConfigured" />
                        <asp:CheckBox ID="chkShowAlternateSearchIfConfigured" runat="server" CssClass="forminput" />
                        <portal:mojoHelpLink ID="MojoHelpLink84" runat="server" HelpKey="show-alternate-search-help" />
                    </div>
                            

                    <div id="divGAnalytics" runat="server" class="settingrow">
                        <mp:SiteLabel ID="SiteLabel25" runat="server" ForControl="txtGoogleAnayticsAccountCode"
                            CssClass="settinglabel" ConfigKey="GoogleAnalyticsAccountCodeLabel" />
                        <asp:TextBox ID="txtGoogleAnayticsAccountCode" TabIndex="10" MaxLength="100" Columns="45"
                            runat="server" CssClass="forminput widetextbox" />
                        <portal:mojoHelpLink ID="MojoHelpLink57" runat="server" HelpKey="googleanalyticsaccountcodehelp" />
                    </div>
                    <div id="divWoopra" runat="server" class="settingrow">
                        <mp:SiteLabel ID="SiteLabel46" runat="server" ForControl="chkEnableWoopra" CssClass="settinglabel"
                            ConfigKey="EnableWoopraLabel" />
                        <asp:CheckBox ID="chkEnableWoopra" runat="server" CssClass="forminput" />
                        <portal:mojoHelpLink ID="MojoHelpLink58" runat="server" HelpKey="wooprahelp" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel10" runat="server" ForControl="txtGmapApiKey" CssClass="settinglabel"
                            ConfigKey="SiteSettingsGmapApiKeyLabel" />
                        <asp:TextBox ID="txtGmapApiKey" TabIndex="10" MaxLength="100" Columns="45" runat="server"
                            CssClass="forminput widetextbox" />
                        <portal:mojoHelpLink ID="MojoHelpLink59" runat="server" HelpKey="sitesettingsgmapapikeyhelp" />
                    </div>
                    <div class="settingrow">
                    <mp:SiteLabel ID="SiteLabel80" runat="server" ForControl="ddCommentSystem" CssClass="settinglabel"
                            ConfigKey="SiteSettingsCommentSystem" />
                        <asp:DropDownList ID="ddCommentSystem" runat="server" CssClass="forminput">
                           
                            <asp:ListItem Value="intensedebate" Text="<%$ Resources:Resource, CommentSystemIntenseDebate %>" />
                            <asp:ListItem Value="disqus" Text="<%$ Resources:Resource, CommentSystemDisqus %>" />
                            <asp:ListItem Value="facebook" Text="<%$ Resources:Resource, CommentSystemFacebook %>" />
                        </asp:DropDownList>
                        <portal:mojoHelpLink ID="MojoHelpLink51" runat="server" HelpKey="comment-system-help" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel75" runat="server" ForControl="txtFacebookAppId" CssClass="settinglabel"
                            ConfigKey="FacebookAppId" />
                        <asp:TextBox ID="txtFacebookAppId" TabIndex="10" MaxLength="255"  runat="server"
                            CssClass="forminput widetextbox" />
                        <portal:mojoHelpLink ID="MojoHelpLink68" runat="server" HelpKey="FacebookAppId-help" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel81" runat="server" ForControl="txtIntenseDebateAccountId" CssClass="settinglabel"
                            ConfigKey="IntenseDebateAccountId" />
                        <asp:TextBox ID="txtIntenseDebateAccountId" TabIndex="10" MaxLength="255"  runat="server"
                            CssClass="forminput widetextbox" />
                        <portal:mojoHelpLink ID="MojoHelpLink52" runat="server" HelpKey="intensedebate-accoutid-help" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel82" runat="server" ForControl="txtDisqusSiteShortName" CssClass="settinglabel"
                            ConfigKey="DisqusSiteShortName" />
                        <asp:TextBox ID="txtDisqusSiteShortName" TabIndex="10" MaxLength="255"  runat="server"
                            CssClass="forminput widetextbox" />
                        <portal:mojoHelpLink ID="MojoHelpLink78" runat="server" HelpKey="disqus-siteshortname-help" />
                    </div>
                            
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel23" runat="server" ForControl="txtAddThisUserId" CssClass="settinglabel"
                            ConfigKey="SiteSettingsAddThisAccountIdLabel" />
                        <asp:TextBox ID="txtAddThisUserId" TabIndex="10" MaxLength="100" Columns="45" runat="server"
                            CssClass="forminput widetextbox" />
                        <portal:mojoHelpLink ID="MojoHelpLink60" runat="server" HelpKey="sitesettingsaddthisuseridhelp" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel64" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                        </mp:SiteLabel>
                    </div>
                </div>
                <div id="tabMailSettings" runat="server" class="minheightpanel" visible="false">
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel28" runat="server" ForControl="txtSMTPUser" CssClass="settinglabel"
                            ConfigKey="SMTPUser" />
                        <asp:TextBox ID="txtSMTPUser" TabIndex="10" MaxLength="100" Columns="45" runat="server"
                            CssClass="forminput widetextbox" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel36" runat="server" ForControl="txtSMTPPassword" CssClass="settinglabel"
                            ConfigKey="SMTPPassword" />
                        <asp:TextBox ID="txtSMTPPassword" TabIndex="10" TextMode="Password" MaxLength="100"
                            Columns="45" runat="server" CssClass="forminput widetextbox" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel37" runat="server" ForControl="txtSMTPServer" CssClass="settinglabel"
                            ConfigKey="SMTPServer" />
                        <asp:TextBox ID="txtSMTPServer" TabIndex="10" MaxLength="100" Columns="45" runat="server"
                            CssClass="forminput widetextbox" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel38" runat="server" ForControl="txtSMTPPort" CssClass="settinglabel"
                            ConfigKey="SMTPPort" />
                        <asp:TextBox ID="txtSMTPPort" TabIndex="10" MaxLength="100" Columns="45" runat="server"
                            CssClass="forminput widetextbox" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel41" runat="server" ForControl="chkSMTPRequiresAuthentication"
                            CssClass="settinglabel" ConfigKey="SMTPRequiresAuthentication" />
                        <asp:CheckBox ID="chkSMTPRequiresAuthentication" runat="server" CssClass="forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel40" runat="server" ForControl="chkSMTPUseSsl" CssClass="settinglabel"
                            ConfigKey="SMTPUseSsl" />
                        <asp:CheckBox ID="chkSMTPUseSsl" runat="server" CssClass="forminput" />
                    </div>
                    <div class="settingrow" id="divSMTPEncoding" runat="server">
                        <mp:SiteLabel ID="SiteLabel39" runat="server" ForControl="txtSMTPPreferredEncoding"
                            CssClass="settinglabel" ConfigKey="SMTPPreferredEncoding" />
                        <asp:TextBox ID="txtSMTPPreferredEncoding" TabIndex="10" MaxLength="100" Columns="45"
                            runat="server" CssClass="forminput widetextbox" />
                    </div>
                    <div class="settingrow">
                        <portal:mojoHelpLink ID="MojoHelpLink61" runat="server" HelpKey="smtphelp" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel65" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                        </mp:SiteLabel>
                    </div>
                </div>
            </div>
               
        <div class="settingrow">
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
        </div>
        <div class="settingrow">
            <mp:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
            <portal:mojoButton ID="btnSave" Text="Apply Changes" runat="server" />&nbsp;&nbsp;
            <portal:mojoButton ID="btnDelete" runat="server" Visible="false" />&nbsp;&nbsp;
        </div>
    </asp:Panel>
    </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" 
    runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" 
    runat="server" >
</asp:Content>

