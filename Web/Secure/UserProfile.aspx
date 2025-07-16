<%@ Page Language="c#" MaintainScrollPositionOnPostback="true" CodeBehind="UserProfile.aspx.cs"
	MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.UI.Pages.UserProfile" %>

<%@ Register Src="~/Controls/UserCommerceHistory.ascx" TagPrefix="portal" TagName="PurchaseHistory" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:ProfileDisplaySettings ID="displaySettings" runat="server" />
	<asp:Panel ID="pnlUser" runat="server" DefaultButton="btnUpdate" CssClass="panelwrapper userprofile">
		<div class="modulecontent">
			<fieldset>
				<legend>
					<mp:SiteLabel ID="SiteLabel1" runat="server" ConfigKey="UserProfileMyProfileLabel" />
				</legend>
				<div id="divtabs" class="mojo-tabs">
					<ul>
						<li id="liSecurity" runat="server">
							<asp:Literal ID="litSecurityTab" runat="server" /></li>
						<li id="liNewsletters" runat="server" visible="false">
							<asp:Literal ID="litNewsletterTab" runat="server" /></li>
						<li id="liProfile" runat="server">
							<asp:Literal ID="litProfileTab" runat="server" /></li>
						<li id="liOrderHistory" runat="server">
							<asp:Literal ID="litOrderHistoryTab" runat="server" /></li>
					</ul>
					<div id="tabSecurity">
						<div class="settingrow">
							<mp:SiteLabel ID="lblUserName" runat="server" ForControl="txtName" CssClass="settinglabel" ConfigKey="ManageUsersUserNameLabel" />
							<asp:TextBox ID="txtName" runat="server" TabIndex="10" MaxLength="100" CssClass="widetextbox forminput" />
							<asp:RequiredFieldValidator ID="rfvName" runat="server" ValidationGroup="profile" Display="none" ErrorMessage="" ControlToValidate="txtName" SkinID="Profile" />
							<asp:RegularExpressionValidator ID="regexUserName" runat="server" ControlToValidate="txtName" Display="None" ValidationExpression="" ValidationGroup="profile" Enabled="false" SkinID="Profile" />
							<asp:CustomValidator ID="FailSafeUserNameValidator" runat="server" ControlToValidate="txtName" Display="None" ValidationGroup="profile" EnableClientScript="false" SkinID="Profile" />
							<portal:mojoHelpLink ID="MojoHelpLink11" runat="server" HelpKey="userfullnamehelp" />
						</div>
						<div class="settingrow">
							<mp:SiteLabel ID="SitelabelLoginName" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersLoginNameLabel" />
							<asp:Label ID="lblLoginName" runat="server" CssClass="forminput" />
						</div>
						<div class="settingrow">
							<mp:SiteLabel ID="lblEmail" runat="server" ForControl="txtEmail" CssClass="settinglabel" ConfigKey="ManageUsersEmailLabel" />
							<asp:TextBox ID="txtEmail" runat="server" TabIndex="10" CssClass="widetextbox forminput" />
							<portal:EmailValidator ID="regexEmail" runat="server" ValidationGroup="profile" ErrorMessage="" ControlToValidate="txtEmail" Display="None" SkinID="Profile" />
							<asp:RequiredFieldValidator ID="rfvEmail" runat="server" ValidationGroup="profile" ErrorMessage="" ControlToValidate="txtEmail" Display="none" SkinID="Profile" />
							<portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="useremailhelp" />
						</div>
						<div id="divOpenID" runat="server" class="settingrow">
							<mp:SiteLabel ID="SiteLabel4" runat="server" ForControl="OpenIdLogin1" CssClass="settinglabel" ConfigKey="ManageUsersOpenIDURILabel" />
							<div class="forminput">
								<asp:Label ID="lblOpenID" runat="server" />
								<asp:HyperLink ID="lnkOpenIDUpdate" runat="server" />
								<portal:OpenIdRpxNowLink ID="rpxLink" runat="server" Embed="false" UseOverlay="true" Visible="false" />
							</div>
						</div>
						<asp:Panel ID="pnlSecurityQuestion" runat="server">
							<div class="settingrow">
								<mp:SiteLabel ID="SiteLabel2" runat="server" ForControl="txtPasswordQuestion" CssClass="settinglabel" ConfigKey="UsersSecurityQuestionLabel" />
								<asp:TextBox ID="txtPasswordQuestion" runat="server" TabIndex="10" MaxLength="255" CssClass="widetextbox forminput" />
								<asp:RequiredFieldValidator ControlToValidate="txtPasswordQuestion" ID="QuestionRequired" runat="server" Display="None" ValidationGroup="profile" SkinID="Profile" />
								<portal:mojoHelpLink ID="MojoHelpLink2" runat="server" HelpKey="usersecurityquestionhelp" />
							</div>
							<div class="settingrow">
								<mp:SiteLabel ID="SiteLabel3" runat="server" ForControl="txtPasswordAnswer" CssClass="settinglabel" ConfigKey="UsersSecurityAnswerLabel" />
								<asp:TextBox ID="txtPasswordAnswer" runat="server" TabIndex="10" MaxLength="255" CssClass="widetextbox forminput" />
								<asp:RequiredFieldValidator ControlToValidate="txtPasswordAnswer" ID="AnswerRequired" runat="server" Display="None" ValidationGroup="profile" SkinID="Profile" />
								<portal:mojoHelpLink ID="MojoHelpLink3" runat="server" HelpKey="usersecurityanswerhelp" />
							</div>
						</asp:Panel>
						<div id="divEditorPreference" runat="server" visible="false" class="settingrow">
							<mp:SiteLabel ID="SiteLabel29" runat="server" ForControl="ddEditorProviders" CssClass="settinglabel" ConfigKey="SiteSettingsEditorProviderLabel" EnableViewState="false" />
							<asp:DropDownList ID="ddEditorProviders" DataTextField="name" DataValueField="name" EnableViewState="true" EnableTheming="false" TabIndex="10" runat="server" CssClass="forminput" />
							<portal:mojoHelpLink ID="MojoHelpLink5" runat="server" HelpKey="sitesettingssiteeditorproviderhelp" />
						</div>
					</div>
					<div id="tabNewsletters" runat="server">
						<portal:SubscriberPreferences ID="newsLetterPrefs" runat="server" Visible="false" />
					</div>
					<div id="tabProfile" runat="server">
						<div class="settingrow usercreated">
							<mp:SiteLabel ID="lblCreatedDateLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersCreatedDateLabel" />
							<asp:Label ID="lblCreatedDate" runat="server" CssClass="forminput" />
						</div>
						<div id="divSkin" runat="server" class="settingrow">
							<mp:SiteLabel ID="lblSkin" runat="server" ForControl="ddSkins" CssClass="settinglabel" ConfigKey="SiteSettingsSiteSkinLabel" />
							<portal:SkinList ID="SkinSetting" runat="server" AddSiteDefaultOption="true" />
							<portal:mojoHelpLink ID="MojoHelpLink4" runat="server" HelpKey="userskinhelp" />
						</div>
						<div id="divAvatar" runat="server" class="settingrow">
							<mp:SiteLabel ID="lblAvatar" runat="server" CssClass="settinglabel" ConfigKey="UserProfileAvatarLabel" ShowWarningOnMissingKey="false" />
							<asp:UpdatePanel ID="upAvatar" runat="server" RenderMode="Block" UpdateMode="Conditional" class="forminput">
								<Triggers>
									<asp:AsyncPostBackTrigger ControlID="btnUpdateAvatar" EventName="Click" /> 
								</Triggers>
								<ContentTemplate>
									<portal:Avatar ID="userAvatar" runat="server" CssClass="forminput" />
									<asp:HyperLink ID="lnkAvatarUpld" runat="server" />
									<portal:mojoHelpLink ID="avatarHelp" runat="server" HelpKey="useravatarhelp" />
									<asp:Button ID="btnUpdateAvatar" runat="server" style="visibility:hidden;" />
								</ContentTemplate>
							</asp:UpdatePanel>
						</div>
						<div id="divLiveMessenger" runat="server" class="settingrow">
							<mp:SiteLabel ID="SiteLabel14" runat="server" ForControl="chkEnableLiveMessengerOnProfile" CssClass="settinglabel" ConfigKey="EnableLiveMessengerLabel" />
							<div class="forminput">
								<asp:CheckBox ID="chkEnableLiveMessengerOnProfile" runat="server" />
								<asp:HyperLink ID="lnkAllowLiveMessenger" runat="server" Text="Enable Live Messenger" />
								<portal:mojoHelpLink ID="MojoHelpLink6" runat="server" HelpKey="livemessenger-user-help" />
							</div>
						</div>
						<div id="divTimeZone" runat="server" visible="false" class="settingrow timezone">
							<mp:SiteLabel ID="SiteLabel5" runat="server" CssClass="settinglabel" ConfigKey="TimeZone" />
							<portal:TimeZoneIdSetting ID="timeZoneSetting" runat="server" />
						</div>
						<asp:Panel ID="pnlProfileProperties" runat="server" />
						<div id="divForumPosts" runat="server" class="settingrow">
							<mp:SiteLabel ID="lblTotalPostsLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersTotalPostsLabel" />
							<div class="forminput">
								<asp:Label ID="lblTotalPosts" runat="server" />
								<portal:ForumUserThreadLink ID="lnkUserPosts" runat="server" />
							</div>
						</div>
						<div class="settingrow">
							<asp:HyperLink ID="lnkPubProfile" runat="server" CssClass="publicprofilelink cblink" />
						</div>
					</div>
					<div id="tabOrderHistory" runat="server">
						<portal:PurchaseHistory ID="purchaseHx" runat="server" />
					</div>
				</div>
				<div class="settingrow">
					<asp:ValidationSummary ID="vSummary" runat="server" CssClass="txterror" ValidationGroup="profile" SkinID="Profile" />
				</div>
				<div class="settingrow">
					<mp:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
					<portal:mojoButton ID="btnUpdate" runat="server" Text="" ValidationGroup="profile" />
					<asp:HyperLink ID="lnkChangePassword" runat="server" CssClass="passwordrecovery" />
					<portal:mojoHelpLink ID="MojoHelpLink7" runat="server" HelpKey="userchangepasswordhelp" />
					<br />
					<portal:mojoLabel ID="lblErrorMessage" runat="server" CssClass="txterror" />
				</div>
			</fieldset>
		</div>
	</asp:Panel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
