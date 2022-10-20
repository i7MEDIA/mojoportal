<%@ Page Language="c#" MaintainScrollPositionOnPostback="true" CodeBehind="PageSettings.aspx.cs"
	MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.AdminUI.PageProperties" %>

<%@ Register TagPrefix="portal" TagName="PublishType" Src="~/Controls/PublishTypeSetting.ascx" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin pagesettings">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<portal:PageLayoutDisplaySettings ID="displaySettings" runat="server" />
					<div id="divAdminLinks" runat="server">
						<asp:HyperLink ID="lnkEditContent" EnableViewState="false" runat="server" /><asp:Literal ID="litLinkSpacer1" runat="server" EnableViewState="false" />
						<asp:HyperLink ID="lnkViewPage" runat="server" EnableViewState="false"></asp:HyperLink><asp:Literal ID="litLinkSpacer2" runat="server" EnableViewState="false" />
						<asp:HyperLink ID="lnkPageTree" runat="server" />
					</div>
					<div class="pagetabs">
						<div id="divtabs" class="mojo-tabs">
							<ul>
								<li class="selected"><a href="#tabSettings">
									<asp:Literal ID="litSettingsTab" runat="server" /></a></li>
								<li id="liSecurity" runat="server" enableviewstate="false">
									<asp:Literal ID="litSecurityTab" runat="server" /></li>
								<li><a href="#tabMetaSettings">
									<asp:Literal ID="litMetaSettingsTab" runat="server" /></a></li>
								<li><a href="#tabSEO">
									<asp:Literal ID="litSEOTab" runat="server" /></a></li>
							</ul>

							<div id="tabSettings">
								<div class="settingrow">
									<mp:SiteLabel ID="lblParentPage" runat="server" ForControl="ddPages" CssClass="settinglabel"
										ConfigKey="PageLayoutParentPageLabel">
									</mp:SiteLabel>
									<asp:Label ID="lblParentPageName" runat="server" Visible="false" />
									<asp:DropDownList ID="ddPages" runat="server" DataTextField="PageName"
										DataValueField="PageID" CssClass="forminput">
									</asp:DropDownList>
									<asp:HiddenField ID="hdnParentPageId" runat="server" />
									<asp:HyperLink ID="lnkParentPageEdit" runat="server" CssClass="cblink" Visible="false" />
									<portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="pagesettingsparentpagehelp" />
								</div>
								<div class="settingrow">
									<mp:SiteLabel ID="lblPageNameLabel" runat="server" ForControl="txtPageName" CssClass="settinglabel"
										ConfigKey="PageSettingsPageNameLabel">
									</mp:SiteLabel>
									<asp:TextBox ID="txtPageName" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
									<portal:mojoHelpLink ID="MojoHelpLink2" runat="server" HelpKey="pagesettingspagenamehelp" />
									<asp:HiddenField ID="hdnPageName" runat="server" />
								</div>
								<div class="settingrow">
									<mp:SiteLabel ID="SiteLabel11" runat="server" ForControl="txtPageTitle" CssClass="settinglabel"
										ConfigKey="PageSettingsPageTitleOverrideLabel">
									</mp:SiteLabel>
									<asp:TextBox ID="txtPageTitle" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
									<portal:mojoHelpLink ID="MojoHelpLink3" runat="server" HelpKey="pagesettingspagetitlehelp" />
								</div>
								<div id="divPageHeading" runat="server" class="settingrow">
									<mp:SiteLabel ID="SiteLabel38" runat="server" ForControl="txtPageHeading" CssClass="settinglabel"
										ConfigKey="PageHeading">
									</mp:SiteLabel>
									<asp:TextBox ID="txtPageHeading" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
									<portal:mojoHelpLink ID="MojoHelpLink20" runat="server" HelpKey="page-heading-help" />
								</div>
								<div id="divShowPageHeading" runat="server" class="settingrow">
									<mp:SiteLabel ID="Sitelabel39" runat="server" ForControl="chkShowPageHeading" CssClass="settinglabel"
										ConfigKey="ShowPageHeading">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkShowPageHeading" runat="server" CssClass="forminput"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink21" runat="server" HelpKey="ShowPageHeading-help" />
								</div>
								<div id="divUseUrl" runat="server" class="settingrow">
									<mp:SiteLabel ID="Sitelabel2" runat="server" ForControl="chkUseUrl" CssClass="settinglabel"
										ConfigKey="PageLayoutUseUrlLabel">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkUseUrl" runat="server" CssClass="forminput"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink4" runat="server" HelpKey="pagesettingsuseurlhelp" />
								</div>
								<div class="settingrow">
									<mp:SiteLabel ID="Sitelabel3" runat="server" ForControl="txtUrl" CssClass="settinglabel"
										ConfigKey="PageLayoutUrlLabel">
									</mp:SiteLabel>
									<asp:TextBox ID="txtUrl" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
									<portal:mojoHelpLink ID="MojoHelpLink5" runat="server" HelpKey="pagesettingsurlhelp" />
									<span id="spnUrlWarning" runat="server" style="font-weight: normal; display: none;" class="txterror warning"></span>
								</div>
								<div id="tabSSL" runat="server" class="settingrow">
									<mp:SiteLabel ID="lblRequireSSL" runat="server" ForControl="chkRequireSSL" CssClass="settinglabel"
										ConfigKey="PageLayoutRequireSSLLabel">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkRequireSSL" runat="server"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink18" runat="server" HelpKey="pagesettingsrequiresslhelp" />
								</div>
								<div class="settingrow">
									<mp:SiteLabel ID="SiteLabel30" runat="server" CssClass="settinglabel" ConfigKey="PublishMode"></mp:SiteLabel>
									<portal:PublishType ID="publishType" runat="server" />
									<portal:mojoHelpLink ID="MojoHelpLink38" runat="server" HelpKey="page-settings-publish-mode-help" />
								</div>
								<div id="divSkin" runat="server" class="settingrow">
									<mp:SiteLabel ID="lblSkin" runat="server" ForControl="ddSkins" CssClass="settinglabel"
										ConfigKey="SiteSettingsSiteSkinLabel">
									</mp:SiteLabel>
									<portal:SkinList ID="SkinSetting" runat="server" AddSiteDefaultOption="true" />
									<portal:mojoHelpLink ID="MojoHelpLink7" runat="server" HelpKey="pagesettingsskinhelp" />
								</div>
								<div id="divMenuDesc" runat="server" visible="false" class="settingrow menudesc">
									<mp:SiteLabel ID="Sitelabel36" runat="server" ForControl="txtMenuDesc" CssClass="settinglabel"
										ConfigKey="MenuDescription">
									</mp:SiteLabel>
									<asp:TextBox ID="txtMenuDesc" runat="server" TextMode="MultiLine" CssClass="forminput verywidetextbox"></asp:TextBox>
									<portal:mojoHelpLink ID="MojoHelpLink19" runat="server" HelpKey="pagesettings-menudesc-help" />
								</div>
								<div id="divIsClickable" runat="server" class="settingrow" visible="false">
									<mp:SiteLabel ID="Sitelabel25" runat="server" ForControl="chkIsClickable" CssClass="settinglabel"
										ConfigKey="PageSettingsIsClickableLabel">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkIsClickable" runat="server" CssClass="forminput" Checked="true"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink33" runat="server" HelpKey="pagesettingsisclickablehelp" />
								</div>
								<div class="settingrow">
									<mp:SiteLabel ID="Sitelabel12" runat="server" ForControl="chkAllowBrowserCache" CssClass="settinglabel"
										ConfigKey="PageSettingsAllowBrowserCacheLabel">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkAllowBrowserCache" runat="server" CssClass="forminput"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink8" runat="server" HelpKey="pagesettingsallowbrowsercachehelp" />
								</div>
								<div class="settingrow">
									<mp:SiteLabel ID="Sitelabel10" runat="server" ForControl="chkIncludeInMenu" CssClass="settinglabel"
										ConfigKey="PageSettingsIncludeInMenuLabel">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkIncludeInMenu" runat="server" CssClass="forminput" Checked="true"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink9" runat="server" HelpKey="pagesettingsincludeinmenuhelp" />
								</div>
								<div class="settingrow">
									<mp:SiteLabel ID="Sitelabel16" runat="server" ForControl="chkIncludeInSiteMap" CssClass="settinglabel"
										ConfigKey="PageSettingsIncludeInSiteMapLabel">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkIncludeInSiteMap" runat="server" CssClass="forminput" Checked="true"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink10" runat="server" HelpKey="pagesettingsincludeinsitemaphelp" />
								</div>
								<div class="settingrow expandonsitemap">
									<mp:SiteLabel ID="Sitelabel29" runat="server" ForControl="chkExpandOnSiteMap" CssClass="settinglabel"
										ConfigKey="PageSettingsExpandOnSiteMapLabel">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkExpandOnSiteMap" runat="server" CssClass="forminput" Checked="true"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink37" runat="server" HelpKey="pagesettings-expandonsitemap-help" />
								</div>
								<div class="settingrow">
									<mp:SiteLabel ID="Sitelabel26" runat="server" ForControl="chkIncludeInChildSiteMap" CssClass="settinglabel"
										ConfigKey="PageSettingsIncludeInChildSiteMap">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkIncludeInChildSiteMap" runat="server" CssClass="forminput" Checked="true"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink34" runat="server" HelpKey="pagesettings-includeinchildsitemap-help" />
								</div>
								<div id="divIsPending" runat="server" class="settingrow">
									<mp:SiteLabel ID="Sitelabel19" runat="server" ForControl="chkIsPending" CssClass="settinglabel"
										ConfigKey="PageSettingsIsPendingLabel">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkIsPending" runat="server" CssClass="forminput" Checked="false"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink29" runat="server" HelpKey="pagesettingsisdrafthelp" />
								</div>
								<div class="settingrow">
									<mp:SiteLabel ID="Sitelabel1" runat="server" ForControl="chkShowBreadcrumbs" CssClass="settinglabel"
										ConfigKey="PageLayoutShowBreadcrumbsLabel">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkShowBreadcrumbs" runat="server" CssClass="forminput"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink11" runat="server" HelpKey="pagesettingsbreadcrumbshelp" />
								</div>
								<div class="settingrow">
									<mp:SiteLabel ID="Sitelabel7" runat="server" ForControl="chkShowChildPageBreadcrumbs"
										CssClass="settinglabel" ConfigKey="PageLayoutShowChildBreadcrumbsLabel">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkShowChildPageBreadcrumbs" runat="server" CssClass="forminput"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink12" runat="server" HelpKey="pagesettingschildpagebreadcrumbshelp" />
								</div>
								<div class="settingrow">
									<mp:SiteLabel ID="Sitelabel17" runat="server" ForControl="chkShowHomeCrumb" CssClass="settinglabel"
										ConfigKey="ShowHomePageCrumb">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkShowHomeCrumb" runat="server" CssClass="forminput"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink13" runat="server" HelpKey="pagesettingshomecrumbhelp" />
								</div>
								<div class="settingrow">
									<mp:SiteLabel ID="Sitelabel4" runat="server" ForControl="chkNewWindow" CssClass="settinglabel"
										ConfigKey="PageLayoutOpenInNewWindowLabel">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkNewWindow" runat="server" CssClass="forminput"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink14" runat="server" HelpKey="pagesettingsnewwindowhelp" />
								</div>
								<div class="settingrow ShowChildMenu">
									<mp:SiteLabel ID="Sitelabel5" runat="server" ForControl="chkShowChildMenu" CssClass="settinglabel"
										ConfigKey="PageLayoutShowChildMenuLabel">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkShowChildMenu" runat="server" CssClass="forminput"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink15" runat="server" HelpKey="pagesettingschildpagemenuhelp" />
								</div>
								<div id="divHideMenu" runat="server" class="settingrow">
									<mp:SiteLabel ID="Sitelabel9" runat="server" ForControl="chkHideMainMenu" CssClass="settinglabel"
										ConfigKey="PageLayoutHideMenuLabel">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkHideMainMenu" runat="server" CssClass="forminput"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink16" runat="server" HelpKey="pagesettingshidemenuhelp" />
								</div>
								<%--								<div id="div1" runat="server" class="settingrow">
									<mp:SiteLabel ID="Sitelabel35" runat="server" ForControl="chkHideMainMenu" CssClass="settinglabel"
										ConfigKey="PageLayoutHideMenuLabel"></mp:SiteLabel>
									<asp:CheckBox ID="CheckBox1" runat="server" CssClass="forminput"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink6" runat="server" HelpKey="pagesettingshidemenuhelp" />
								</div>--%>
								<div class="settingrow">
									<mp:SiteLabel ID="Sitelabel13" runat="server" ForControl="chkHideAfterLogin" CssClass="settinglabel"
										ConfigKey="PageSettingstHideAfterLoginLabel">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkHideAfterLogin" runat="server" CssClass="forminput"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink17" runat="server" HelpKey="pagesettingshideafterloginhelp" />
								</div>
								<asp:Panel ID="pnlComments" runat="server" Visible="false" CssClass="settingrow">
									<mp:SiteLabel ID="Sitelabel24" runat="server" ForControl="chkEnableComments" CssClass="settinglabel"
										ConfigKey="PageSettingsEnableComments">
									</mp:SiteLabel>
									<asp:CheckBox ID="chkEnableComments" runat="server" CssClass="forminput"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink32" runat="server" HelpKey="pagesettings-enablecomments-help" />
								</asp:Panel>

								<div id="divBodyCss" runat="server" class="settingrow">
									<mp:SiteLabel ID="SiteLabel27" runat="server" ForControl="txtBodyCssClass" CssClass="settinglabel"
										ConfigKey="PageSettingsBodyCssClass">
									</mp:SiteLabel>
									<asp:TextBox ID="txtBodyCssClass" runat="server" MaxLength="50" CssClass="forminput normaltextbox"></asp:TextBox>
									<portal:mojoHelpLink ID="MojoHelpLink35" runat="server" HelpKey="pagesettings-bodycssclass-help" />
								</div>
								<div id="divMenuCss" runat="server" class="settingrow">
									<mp:SiteLabel ID="SiteLabel28" runat="server" ForControl="txtMenuCssClass" CssClass="settinglabel"
										ConfigKey="PageSettingsMenuCssClass">
									</mp:SiteLabel>
									<asp:TextBox ID="txtMenuCssClass" runat="server" MaxLength="50" CssClass="forminput normaltextbox"></asp:TextBox>
									<portal:mojoHelpLink ID="MojoHelpLink36" runat="server" HelpKey="pagesettings-menucssclass-help" />
								</div>
								<div id="divMenuLinkRelation" runat="server" class="settingrow">
									<mp:SiteLabel ID="SiteLabel40" runat="server" ForControl="txtMenuLinkRelation" CssClass="settinglabel"
										ConfigKey="MenuLinkRelation">
									</mp:SiteLabel>
									<asp:TextBox ID="txtMenuLinkRelation" runat="server" MaxLength="20" CssClass="forminput normaltextbox"></asp:TextBox>
									<portal:mojoHelpLink ID="MojoHelpLink28" runat="server" HelpKey="menu-link-relation-help" />
								</div>
								<asp:Panel ID="pnlModified" runat="server" EnableViewState="false" Visible="false">
									<div class="settingrow pcreateddate">
										<mp:SiteLabel ID="Sitelabel31" runat="server" CssClass="settinglabel"
											ConfigKey="Created">
										</mp:SiteLabel>
										<asp:Label ID="lblCreatedDate" runat="server" CssClass="readonly" />
									</div>
									<div class="settingrow pmodifieddate">
										<mp:SiteLabel ID="Sitelabel32" runat="server" CssClass="settinglabel"
											ConfigKey="LastModified">
										</mp:SiteLabel>
										<asp:Label ID="lblLastModifiedDate" runat="server" CssClass="readonly" />
									</div>
									<div class="settingrow pmodby">
										<mp:SiteLabel ID="Sitelabel33" runat="server" CssClass="settinglabel"
											ConfigKey="LastModifiedBy">
										</mp:SiteLabel>
										<asp:Label ID="lblLastModifiedBy" runat="server" CssClass="readonly" />
									</div>
									<div class="settingrow pmodfromip">
										<mp:SiteLabel ID="Sitelabel34" runat="server" CssClass="settinglabel"
											ConfigKey="LastModifiedFromIpAddress">
										</mp:SiteLabel>
										<asp:Label ID="lblLastModifiedFromIp" runat="server" CssClass="readonly" />
									</div>
								</asp:Panel>
								<div class="settingrow">
									&nbsp;
								</div>
							</div>
							<div id="tabSecurity" runat="server">
								<div id="divRoles" runat="server" class="mojo-accordion">
									<h3 id="h3ViewRoles" runat="server">
										<a href="#">
											<mp:SiteLabel ID="lblAuthorizedRoles" runat="server" ConfigKey="PageLayoutViewRolesLabel"
												UseLabelTag="false" />
										</a>
									</h3>
									<div id="divViewRoles" runat="server">
										<div class="settingrow">
											<asp:RadioButton ID="rbViewAdminOnly" runat="server" GroupName="rdoviewroles" CssClass="rbroles rbadminonly" />
										</div>
										<div class="settingrow">
											<asp:RadioButton ID="rbViewUseRoles" runat="server" GroupName="rdoviewroles" CssClass="rbroles" />
										</div>
										<p>
											<asp:CheckBoxList ID="chkListAuthRoles" runat="server" CssClass="forminput" SkinID="Roles">
											</asp:CheckBoxList>
										</p>
									</div>
									<h3 id="h3EditRoles" runat="server">
										<a href="#">
											<mp:SiteLabel ID="SiteLabel21" runat="server" ConfigKey="PageLayoutEditRolesLabel"
												UseLabelTag="false" />
										</a>
									</h3>
									<div id="divEditRoles" runat="server">
										<div class="settingrow">
											<asp:RadioButton ID="rbEditAdminOnly" runat="server" GroupName="rdoeditroles" CssClass="rbroles rbadminonly" />
										</div>
										<div class="settingrow">
											<asp:RadioButton ID="rbEditUseRoles" runat="server" GroupName="rdoeditroles" CssClass="rbroles" />
										</div>
										<p>
											<asp:CheckBoxList ID="chkListEditRoles" runat="server" CssClass="forminput" SkinID="Roles">
											</asp:CheckBoxList>
										</p>
									</div>
									<h3 id="h3DraftRoles" runat="server">
										<a href="#">
											<mp:SiteLabel ID="SiteLabel6" runat="server" ConfigKey="PageLayoutDraftEditRolesLabel"
												UseLabelTag="false" />
										</a>
									</h3>
									<div id="divDraftRoles" runat="server">
										<p>
											<asp:CheckBoxList ID="chkDraftEditRoles" runat="server" SkinID="Roles">
											</asp:CheckBoxList>
										</p>
									</div>

									<h3 id="h3DraftApprovalRoles" runat="server">
										<a href="#">
											<mp:SiteLabel ID="SiteLabel37" runat="server" ConfigKey="PageLayoutDraftApprovalRolesLabel"
												UseLabelTag="false" />
										</a>
									</h3>
									<div id="divDraftApprovalRoles" runat="server">
										<p>
											<asp:CheckBoxList ID="chkDraftApprovalRoles" runat="server" SkinID="Roles">
											</asp:CheckBoxList>
										</p>
									</div>

									<h3 id="h3ChildEditRoles" runat="server">
										<a href="#">
											<mp:SiteLabel ID="SiteLabel18" runat="server" ConfigKey="PageLayoutCreateChildPageRolesLabel"
												UseLabelTag="false" />
										</a>
									</h3>
									<div id="divChildEditRoles" runat="server">
										<div class="settingrow">
											<asp:RadioButton ID="rbCreateChildAdminOnly" runat="server" GroupName="rdochildpageroles" CssClass="rbroles rbadminonly" />
										</div>
										<div class="settingrow">
											<asp:RadioButton ID="rbCreateChildUseRoles" runat="server" GroupName="rdochildpageroles" CssClass="rbroles" />
										</div>
										<p>
											<asp:CheckBoxList ID="chkListCreateChildPageRoles" runat="server" SkinID="Roles"
												CssClass="forminput">
											</asp:CheckBoxList>
										</p>
									</div>

								</div>
								<mp:SiteLabel ID="lblSavePageBeforeSettingPermissions" Visible="false" runat="server" ConfigKey="PageMustBeSavedBeforeSettingPermissions" UseLabelTag="false" CssClass="info" />
								<div id="divRoleLinks" runat="server" visible="false" enableviewstate="false">
									<ul class="simplelist">
										<li>
											<asp:HyperLink ID="lnkPageViewRoles" runat="server" CssClass="lnkPageViewRoles" />
										</li>
										<li>
											<asp:HyperLink ID="lnkPageEditRoles" runat="server" CssClass="lnkPageEditRoles" />
										</li>
										<li>
											<asp:HyperLink ID="lnkPageDraftRoles" runat="server" CssClass="lnkPageDraftRoles" />
										</li>
										<li id="liDraftApprovalRoles" runat="server">
											<asp:HyperLink ID="lnkPageDraftApprovalRoles" runat="server" CssClass="lnkPageDraftApprovalRoles" />
										</li>
										<li>
											<asp:HyperLink ID="lnkChildPageRoles" runat="server" CssClass="lnkChildPageRoles" />
										</li>
									</ul>
								</div>
							</div>
							<div id="tabMetaSettings">

								<asp:Panel ID="pnlMetaSettings" runat="server" SkinID="plain">
									<div class="settingrow">
										<mp:SiteLabel ID="lblKeywords" runat="server" ForControl="txtPageKeywords" CssClass="settinglabel"
											ConfigKey="PageLayoutMetaKeyWordsLabel">
										</mp:SiteLabel>
										<asp:TextBox ID="txtPageKeywords" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
										<portal:mojoHelpLink ID="MojoHelpLink22" runat="server" HelpKey="pagesettingskeywordshelp" />
									</div>
									<div class="settingrow">
										<mp:SiteLabel ID="lblDescription" runat="server" ForControl="txtPageDescription"
											CssClass="settinglabel" ConfigKey="PageLayoutMetaDescriptionLabel">
										</mp:SiteLabel>
										<asp:TextBox ID="txtPageDescription" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
										<portal:mojoHelpLink ID="MojoHelpLink23" runat="server" HelpKey="pagesettingsmetadescriptionhelp" />
									</div>
<%--									<div id="divPageEncoding" runat="server" visible="false" class="settingrow">
										<mp:SiteLabel ID="lblEncoding" runat="server" ForControl="txtPageEncoding" CssClass="settinglabel"
											ConfigKey="PageLayoutMetaEncodingLabel">
										</mp:SiteLabel>
										<asp:TextBox ID="txtPageEncoding" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
										<portal:mojoHelpLink ID="MojoHelpLink24" runat="server" HelpKey="pagesettingsmetaencodinghelp" />
									</div>--%>
									<asp:Panel ID="pnlMeta" runat="server">
										<div class="settingrow">
											<mp:SiteLabel ID="lblAdditionalMetaTags" runat="server" CssClass="settinglabel" ConfigKey="PageLayoutMetaAdditionalLabel"></mp:SiteLabel>
											<portal:mojoHelpLink ID="MojoHelpLink25" runat="server" HelpKey="pagesettingsadditionalmetahelp" />
										</div>
										<div class="settingrow">
											<asp:UpdatePanel ID="updMetaLinks" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<mp:mojoGridView ID="grdMetaLinks" runat="server" CssClass="editgrid" AutoGenerateColumns="false"
														DataKeyNames="Guid">
														<Columns>
															<asp:TemplateField>
																<ItemTemplate>
																	<asp:Button ID="btnEditMetaLink" runat="server" CommandName="Edit" Text='<%# Resources.Resource.ContentMetaGridEditButton %>' />
																	<asp:ImageButton ID="btnMoveUpMetaLink" runat="server" ImageUrl="~/Data/SiteImages/up.png"
																		CommandName="MoveUp" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.Resource.ContentMetaGridMoveUpButton %>'
																		Visible='<%# (Convert.ToInt32(Eval("SortRank")) > 3) %>' />
																	<asp:ImageButton ID="btnMoveDownMetaLink" runat="server" ImageUrl="~/Data/SiteImages/down.png"
																		CommandName="MoveDown" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.Resource.ContentMetaGridMoveDownButton %>' />
																</ItemTemplate>
																<EditItemTemplate>
																</EditItemTemplate>
															</asp:TemplateField>
															<asp:TemplateField>
																<ItemTemplate>
																	<%# Eval("Rel") %>
																</ItemTemplate>
																<EditItemTemplate>
																	<div class="settingrow">
																		<mp:SiteLabel ID="lblNameMetaRel" runat="server" ForControl="txtRel" CssClass="settinglabel"
																			ConfigKey="ContentMetaRelLabel" ResourceFile="Resource" />
																		<asp:TextBox ID="txtRel" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Rel") %>' />
																		<asp:RequiredFieldValidator ID="reqMetaName" runat="server" ControlToValidate="txtRel"
																			ErrorMessage='<%# Resources.Resource.ContentMetaLinkRelRequired %>' ValidationGroup="metalink" />
																	</div>
																	<div class="settingrow">
																		<mp:SiteLabel ID="lblMetaHref" runat="server" ForControl="txtHref" CssClass="settinglabel"
																			ConfigKey="ContentMetaMetaHrefLabel" ResourceFile="Resource" />
																		<asp:TextBox ID="txtHref" CssClass="verywidetextbox forminput" runat="server" Text='<%# Eval("Href") %>' />
																		<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtHref"
																			ErrorMessage='<%# Resources.Resource.ContentMetaLinkHrefRequired %>' ValidationGroup="metalink" />
																	</div>
																	<div class="settingrow">
																		<mp:SiteLabel ID="lblScheme" runat="server" ForControl="txtScheme" CssClass="settinglabel"
																			ConfigKey="ContentMetHrefLangLabel" ResourceFile="Resource" />
																		<asp:TextBox ID="txtHrefLang" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("HrefLang") %>' />
																	</div>
																	<div class="settingrow">
																		<asp:Button ID="btnUpdateMetaLink" runat="server" Text='<%# Resources.Resource.ContentMetaGridUpdateButton %>'
																			CommandName="Update" ValidationGroup="metalink" CausesValidation="true" />
																		<asp:Button ID="btnDeleteMetaLink" runat="server" Text='<%# Resources.Resource.ContentMetaGridDeleteButton %>'
																			CommandName="Delete" CausesValidation="false" />
																		<asp:Button ID="btnCancelMetaLink" runat="server" Text='<%# Resources.Resource.ContentMetaGridCancelButton %>'
																			CommandName="Cancel" CausesValidation="false" />
																	</div>
																</EditItemTemplate>
															</asp:TemplateField>
															<asp:TemplateField>
																<ItemTemplate>
																	<%# Eval("Href") %>
																</ItemTemplate>
																<EditItemTemplate>
																</EditItemTemplate>
															</asp:TemplateField>
														</Columns>
														<EmptyDataTemplate>
															<p class="nodata">
																<asp:Literal ID="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" />
															</p>
														</EmptyDataTemplate>
													</mp:mojoGridView>
													<div class="settingrow">
														<table>
															<tr>
																<td>
																	<asp:Button ID="btnAddMetaLink" runat="server" />&nbsp;
																</td>
																<td>
																	<asp:UpdateProgress ID="prgMetaLinks" runat="server" AssociatedUpdatePanelID="updMetaLinks">
																		<ProgressTemplate>
																			<img src='<%= Page.ResolveUrl("~/Data/SiteImages/indicator1.gif") %>' alt=' ' />
																		</ProgressTemplate>
																	</asp:UpdateProgress>
																</td>
															</tr>
														</table>
													</div>
												</ContentTemplate>
											</asp:UpdatePanel>
										</div>
										<div class="settingrow">
											<asp:UpdatePanel ID="upMeta" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<mp:mojoGridView ID="grdContentMeta" runat="server" CssClass="editgrid" AutoGenerateColumns="false"
														DataKeyNames="Guid">
														<Columns>
															<asp:TemplateField>
																<ItemTemplate>
																	<asp:Button ID="btnEditMeta" runat="server" CommandName="Edit" Text='<%# Resources.Resource.ContentMetaGridEditButton %>' />
																	<asp:ImageButton ID="btnMoveUpMeta" runat="server" ImageUrl="~/Data/SiteImages/up.png"
																		CommandName="MoveUp" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.Resource.ContentMetaGridMoveUpButton %>'
																		Visible='<%# (Convert.ToInt32(Eval("SortRank")) > 3) %>' />
																	<asp:ImageButton ID="btnMoveDownMeta" runat="server" ImageUrl="~/Data/SiteImages/down.png"
																		CommandName="MoveDown" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.Resource.ContentMetaGridMoveDownButton %>' />
																</ItemTemplate>
																<EditItemTemplate>
																</EditItemTemplate>
															</asp:TemplateField>
															<asp:TemplateField>
																<ItemTemplate>
																	<%# Eval("Name") %>
																</ItemTemplate>
																<EditItemTemplate>
																	<div class="settingrow">
																		<mp:SiteLabel ID="lblNameProperty" runat="server" ForControl="txtNameProperty" CssClass="settinglabel"
																			ConfigKey="MetaNameProperty" ResourceFile="Resource" />
																		<asp:TextBox ID="txtNameProperty" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("NameProperty") %>' />
																		<asp:RequiredFieldValidator ID="reqMetaNameProperty" runat="server" ControlToValidate="txtNameProperty"
																			ErrorMessage='<%# Resources.Resource.MetaNamePropertyRequired %>' ValidationGroup="meta" />
																	</div>
																	<div class="settingrow">
																		<mp:SiteLabel ID="lblName" runat="server" ForControl="txtName" CssClass="settinglabel"
																			ConfigKey="ContentMetaNameLabel" ResourceFile="Resource" />
																		<asp:TextBox ID="txtName" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Name") %>' />
																		<asp:RequiredFieldValidator ID="reqMetaName" runat="server" ControlToValidate="txtName"
																			ErrorMessage='<%# Resources.Resource.ContentMetaNameRequired %>' ValidationGroup="meta" />
																	</div>
																	<div class="settingrow">
																		<mp:SiteLabel ID="lblContentProperty" runat="server" ForControl="txtContentProperty" CssClass="settinglabel"
																			ConfigKey="MetaContentProperty" ResourceFile="Resource" />
																		<asp:TextBox ID="txtContentProperty" CssClass="verywidetextbox forminput" runat="server"
																			Text='<%# Eval("ContentProperty") %>' />
																		<asp:RequiredFieldValidator ID="reqMetaContentProperty" runat="server" ControlToValidate="txtContentProperty"
																			ErrorMessage='<%# Resources.Resource.MetaContentPropertyRequired %>' ValidationGroup="meta" />
																	</div>
																	<div class="settingrow">
																		<mp:SiteLabel ID="lblMetaContent" runat="server" ForControl="txtMetaContent" CssClass="settinglabel"
																			ConfigKey="ContentMetaMetaContentLabel" ResourceFile="Resource" />
																		<asp:TextBox ID="txtMetaContent" CssClass="verywidetextbox forminput" runat="server"
																			Text='<%# Eval("MetaContent") %>' />
																		<asp:RequiredFieldValidator ID="reqMetaContent" runat="server" ControlToValidate="txtMetaContent"
																			ErrorMessage='<%# Resources.Resource.ContentMetaContentRequired %>' ValidationGroup="meta" />
																	</div>
																	<div class="settingrow">
																		<mp:SiteLabel ID="lblScheme" runat="server" ForControl="txtScheme" CssClass="settinglabel"
																			ConfigKey="ContentMetaSchemeLabel" ResourceFile="Resource" />
																		<asp:TextBox ID="txtScheme" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Scheme") %>' />
																	</div>
																	<div class="settingrow">
																		<mp:SiteLabel ID="lblLangCode" runat="server" ForControl="txtLangCode" CssClass="settinglabel"
																			ConfigKey="ContentMetaLangCodeLabel" ResourceFile="Resource" />
																		<asp:TextBox ID="txtLangCode" CssClass="smalltextbox forminput" runat="server" Text='<%# Eval("LangCode") %>' />
																	</div>
																	<div class="settingrow">
																		<mp:SiteLabel ID="lblDir" runat="server" ForControl="ddDirection" CssClass="settinglabel"
																			ConfigKey="ContentMetaDirLabel" ResourceFile="Resource" />
																		<asp:DropDownList ID="ddDirection" runat="server" CssClass="forminput">
																			<asp:ListItem Text="" Value=""></asp:ListItem>
																			<asp:ListItem Text="ltr" Value="ltr"></asp:ListItem>
																			<asp:ListItem Text="rtl" Value="rtl"></asp:ListItem>
																		</asp:DropDownList>
																	</div>
																	<div class="settingrow">
																		<asp:Button ID="btnUpdateMeta" runat="server" Text='<%# Resources.Resource.ContentMetaGridUpdateButton %>'
																			CommandName="Update" ValidationGroup="meta" CausesValidation="true" />
																		<asp:Button ID="btnDeleteMeta" runat="server" Text='<%# Resources.Resource.ContentMetaGridDeleteButton %>'
																			CommandName="Delete" CausesValidation="false" />
																		<asp:Button ID="btnCancelMeta" runat="server" Text='<%# Resources.Resource.ContentMetaGridCancelButton %>'
																			CommandName="Cancel" CausesValidation="false" />
																	</div>
																</EditItemTemplate>
															</asp:TemplateField>
															<asp:TemplateField>
																<ItemTemplate>
																	<%# Eval("MetaContent") %>
																</ItemTemplate>
																<EditItemTemplate>
																</EditItemTemplate>
															</asp:TemplateField>
														</Columns>
														<EmptyDataTemplate>
															<p class="nodata">
																<asp:Literal ID="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" />
															</p>
														</EmptyDataTemplate>
													</mp:mojoGridView>
													<div class="settingrow">
														<table>
															<tr>
																<td>
																	<asp:Button ID="btnAddMeta" runat="server" />&nbsp;
																</td>
																<td>
																	<asp:UpdateProgress ID="prgMeta" runat="server" AssociatedUpdatePanelID="upMeta">
																		<ProgressTemplate>
																			<img src='<%= Page.ResolveUrl("~/Data/SiteImages/indicator1.gif") %>' alt=' ' />
																		</ProgressTemplate>
																	</asp:UpdateProgress>
																</td>
															</tr>
														</table>
													</div>
												</ContentTemplate>
											</asp:UpdatePanel>
										</div>
									</asp:Panel>
									<div class="settingrow">
										<mp:SiteLabel ID="SiteLabel20" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
									</div>
								</asp:Panel>
							</div>
							<div id="tabSEO">
								<asp:Panel ID="pnlSearchEngineOptimization" runat="server" SkinID="plain">
									<div class="settingrow">
										<mp:SiteLabel ID="SiteLabel14" runat="server" ForControl="ddChangeFrequency" CssClass="settinglabel"
											ConfigKey="PageSettingsChangeFrequencyLabel">
										</mp:SiteLabel>
										<asp:DropDownList ID="ddChangeFrequency" runat="server" CssClass="forminput">
										</asp:DropDownList>
										<portal:mojoHelpLink ID="MojoHelpLink26" runat="server" HelpKey="pagesettingsseochangefequencyhelp" />
									</div>
									<div class="settingrow">
										<mp:SiteLabel ID="SiteLabel15" runat="server" ForControl="ddSiteMapPriority" CssClass="settinglabel"
											ConfigKey="PageSettingsPriorityLabel">
										</mp:SiteLabel>
										<asp:DropDownList ID="ddSiteMapPriority" runat="server" CssClass="forminput">
											<asp:ListItem Text="0.0" Value="0.0" />
											<asp:ListItem Text="0.1" Value="0.1" />
											<asp:ListItem Text="0.2" Value="0.2" />
											<asp:ListItem Text="0.3" Value="0.3" />
											<asp:ListItem Text="0.4" Value="0.4" />
											<asp:ListItem Text="0.5" Value="0.5" Selected="true" />
											<asp:ListItem Text="0.6" Value="0.6" />
											<asp:ListItem Text="0.7" Value="0.7" />
											<asp:ListItem Text="0.8" Value="0.8" />
											<asp:ListItem Text="0.9" Value="0.9" />
											<asp:ListItem Text="1.0" Value="1.0" />
										</asp:DropDownList>
										<portal:mojoHelpLink ID="MojoHelpLink27" runat="server" HelpKey="pagesettingssitemappriorityhelp" />
									</div>
								</asp:Panel>
								<div class="settingrow">
									<mp:SiteLabel ID="SiteLabel22" runat="server" ForControl="chkIncludeInSearchEngineSiteMap"
										CssClass="settinglabel" ConfigKey="PageSettingsIncludeInSearchengineSiteMap" />
									<asp:CheckBox ID="chkIncludeInSearchEngineSiteMap" runat="server" Checked="true"
										CssClass="forminput" />
									<portal:mojoHelpLink ID="MojoHelpLink30" runat="server" HelpKey="pagesettings-IncludeInSearchEngineSiteMap-help" />
								</div>
								<div class="settingrow">
									<mp:SiteLabel ID="SiteLabel23" runat="server" CssClass="settinglabel" ConfigKey="PageSettingsCanonicalOverride" />
									<asp:TextBox ID="txtCannonicalOverride" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
									<portal:mojoHelpLink ID="MojoHelpLink31" runat="server" HelpKey="pagesettings-CannonicalOverride-help" />
								</div>
								<div class="settingrow">
									<mp:SiteLabel ID="SiteLabel8" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
								</div>
							</div>
						</div>
					</div>

					<div class="settingrow">
						<asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="pagesettings" />
						<asp:RequiredFieldValidator ID="reqPageName" runat="server" Display="None" ControlToValidate="txtPageName"
							ValidationGroup="pagesettings" />
						<asp:RegularExpressionValidator ID="regexUrl" runat="server" ControlToValidate="txtUrl"
							ValidationExpression="((http\://|https\://|~/){1}(\S+){0,1})" Display="None" ValidationGroup="pagesettings" />
						<asp:RegularExpressionValidator ID="regexBodyCss" runat="server" ControlToValidate="txtBodyCssClass"
							ValidationExpression="^([^\d\s\.#\-][\w\-]*\s?)*$" Display="None" ValidationGroup="pagesettings" />
						<asp:RegularExpressionValidator ID="regexMenuCss" runat="server" ControlToValidate="txtMenuCssClass"
							ValidationExpression="^([^\d\s\.#\-][\w\-]*\s?)*$" Display="None" ValidationGroup="pagesettings" />
						<portal:mojoLabel ID="lblError" runat="server" CssClass="txterror warning" />
					</div>

					<div class="settingrow btn-row text-center">
						<portal:mojoButton ID="applyBtn" runat="server" Text="Apply Changes" SkinID="SuccessButton" />
						<portal:mojoButton ID="btnDelete" runat="server" CausesValidation="false" SkinID="DeleteButton" />
					</div>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
			<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared" />
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
