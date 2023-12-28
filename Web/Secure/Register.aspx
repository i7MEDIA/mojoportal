<%@ Page Language="c#" CodeBehind="Register.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.UI.Pages.Register" LinePragmas="false" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:RegistrationPageDisplaySettings ID="displaySettings" runat="server" />
	<portal:CoreDisplaySettings ID="coreDisplaySettings" runat="server" />
	<asp:Panel ID="pnlRegister" runat="server" CssClass="panelwrapper register">
		<div class="modulecontent">
			<asp:Literal ID="litHeading" runat="server" />
			<asp:Panel ID="pnlAuthenticated" runat="server" Visible="false">
				<asp:Literal ID="litAlreadyAuthenticated" runat="server" />
			</asp:Panel>
			<asp:Panel ID="pnlRegisterWrapper" runat="server">
				<asp:Panel ID="pnlStandardRegister" runat="server" CssClass="floatpanel">
					<asp:CreateUserWizard ID="RegisterUser" runat="server" NavigationStyle-HorizontalAlign="Center">
						<WizardSteps>
							<asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
								<ContentTemplate>
									<div id="divPreamble" runat="server" class="regpreamble"></div>
									<asp:Panel ID="pnlRequiredProfilePropertiesUpper" runat="server"></asp:Panel>
									<asp:Panel ID="pnlUserName" runat="server" CssClass="settingrow">
										<mp:SiteLabel ID="lblLoginName" runat="server" ForControl="UserName" CssClass="settinglabel" ConfigKey="RegisterLoginNameLabel" />
										<asp:TextBox ID="UserName" runat="server" TabIndex="0" MaxLength="50" CssClass="forminput mediumtextbox" />
										<asp:RequiredFieldValidator ControlToValidate="UserName" ID="UserNameRequired" runat="server" Display="None" ValidationGroup="profile" SkinID="Registration" />
										<asp:RegularExpressionValidator ID="regexUserName" runat="server" ControlToValidate="UserName" Display="None" ValidationExpression="" ValidationGroup="profile" Enabled="false" SkinID="Registration" />
										<asp:CustomValidator ID="FailSafeUserNameValidator" runat="server" ControlToValidate="UserName" Display="None" ValidationGroup="profile" EnableClientScript="false" SkinID="Registration" />
										<asp:Panel ID="pnlUserNameHint" runat="server" CssClass="hint" Visible="false">
											<asp:Label ID="lblUserNameHint" runat="server" />
										</asp:Panel>
									</asp:Panel>
									<div class="settingrow">
										<mp:SiteLabel ID="lblRegisterEmail1" runat="server" ForControl="Email" CssClass="settinglabel" ConfigKey="RegisterEmailLabel" />
										<asp:TextBox ID="Email" runat="server" TabIndex="0" MaxLength="100" CssClass="forminput mediumtextbox" />
										<asp:RequiredFieldValidator ControlToValidate="Email" ID="EmailRequired" runat="server" Display="None" ValidationGroup="profile" SkinID="Registration" />
										<portal:EmailValidator ID="EmailRegex" runat="server" ControlToValidate="Email" Display="None" ValidationGroup="profile" SkinID="Registration" />
										<asp:Panel ID="pnlEmailHint" runat="server" CssClass="hint" Visible="false">
											<mp:SiteLabel ID="SiteLabel4" runat="server" ConfigKey="RegisterEmailHint" UseLabelTag="false" />
										</asp:Panel>
									</div>
									<asp:Panel ID="divConfirmEmail" runat="server" Visible="false" CssClass="settingrow">
										<mp:SiteLabel ID="SiteLabel7" runat="server" ForControl="EmailConfirm" CssClass="settinglabel" ConfigKey="RegisterEmailConfirmLabel" />
										<asp:TextBox ID="ConfirmEmail" runat="server" TabIndex="0" MaxLength="100" CssClass="forminput mediumtextbox" />
										<asp:RequiredFieldValidator ControlToValidate="ConfirmEmail" ID="ConfirmEmailRequired" runat="server" Display="None" ValidationGroup="profile" Enabled="false" SkinID="Registration" />
										<asp:CompareValidator ControlToCompare="Email" ControlToValidate="ConfirmEmail" ID="EmailCompare" runat="server" Display="None" ValidationGroup="profile" Enabled="false" SkinID="Registration" />
										<asp:Panel ID="pnlEmailConfirmHint" runat="server" CssClass="hint" Visible="false">
											<mp:SiteLabel ID="SiteLabel8" runat="server" ConfigKey="RegisterEmailConfirmHint" UseLabelTag="false" />
										</asp:Panel>
									</asp:Panel>
									<div class="settingrow">
										<mp:SiteLabel ID="lblRegisterPassword1" runat="server" ForControl="Password" CssClass="settinglabel" ConfigKey="RegisterPasswordLabel" />
										<asp:TextBox ID="Password" runat="server" TabIndex="0" TextMode="Password" MaxLength="20" CssClass="forminput mediumtextbox" />
										<asp:RequiredFieldValidator ControlToValidate="Password" ID="PasswordRequired" Display="None" runat="server" ValidationGroup="profile" SkinID="Registration" />
										<asp:CustomValidator ID="PasswordRulesValidator" runat="server" ControlToValidate="Password" Display="None" ValidationGroup="profile" EnableClientScript="false" SkinID="Registration" />
										<asp:RegularExpressionValidator ID="PasswordRegex" runat="server" ControlToValidate="Password" Display="None" ValidationGroup="profile" SkinID="Registration" />
										<asp:Panel ID="pnlPasswordHint" runat="server" CssClass="hint" Visible="false">
											<mp:SiteLabel ID="SiteLabel5" runat="server" ConfigKey="RegisterPasswordHint" UseLabelTag="false" />
										</asp:Panel>
									</div>
									<div class="settingrow">
										<mp:SiteLabel ID="lblRegisterConfirmPassword1" runat="server" ForControl="ConfirmPassword" CssClass="settinglabel" ConfigKey="RegisterConfirmPasswordLabel" />
										<asp:TextBox ID="ConfirmPassword" runat="server" TabIndex="0" TextMode="Password" MaxLength="20" CssClass="forminput mediumtextbox" />
										<asp:RequiredFieldValidator ControlToValidate="ConfirmPassword" ID="ConfirmPasswordRequired" runat="server" Display="None" ValidationGroup="profile" SkinID="Registration" />
										<asp:CompareValidator ControlToCompare="Password" ControlToValidate="ConfirmPassword" ID="PasswordCompare" runat="server" Display="None" ValidationGroup="profile" SkinID="Registration" />
										<asp:Panel ID="pnlConfirmPasswordHint" runat="server" CssClass="hint" Visible="false">
											<mp:SiteLabel ID="SiteLabel6" runat="server" ConfigKey="RegisterConfirmPasswordHint" UseLabelTag="false" />
										</asp:Panel>
									</div>
									<div class="settingrow" id="divQuestion" runat="server">
										<mp:SiteLabel ID="SiteLabel2" runat="server" ForControl="Question" CssClass="settinglabel" ConfigKey="RegisterSecurityQuestion" />
										<asp:TextBox ID="Question" runat="server" TabIndex="0" CssClass="forminput widetextbox" />
										<asp:RequiredFieldValidator ControlToValidate="Question" ID="QuestionRequired" runat="server" Display="None" ValidationGroup="profile" SkinID="Registration" />
									</div>
									<div class="settingrow" id="divAnswer" runat="server">
										<mp:SiteLabel ID="SiteLabel1" runat="server" ForControl="Answer" CssClass="settinglabel" ConfigKey="RegisterSecurityAnswer" />
										<asp:TextBox ID="Answer" runat="server" TabIndex="0" CssClass="forminput widetextbox" />
										<asp:RequiredFieldValidator ControlToValidate="Answer" ID="AnswerRequired" runat="server" Display="None" ValidationGroup="profile" SkinID="Registration" />
									</div>
									<asp:Panel ID="pnlRequiredProfileProperties" runat="server" />
									<asp:Panel ID="pnlSubscribe" runat="server" Visible="false">
										<asp:Label ID="lblNewsletterListHeading" runat="server" CssClass="letterlist" />
										<asp:CheckBoxList ID="clNewsletters" runat="server" DataTextField="Title" DataValueField="LetterInfoGuid" SkinID="RegisterNewsletters" TabIndex="0" />

										<span id="spnFormat" class="emailformat">
											<asp:RadioButton ID="rbHtmlFormat" runat="server" Checked="true" GroupName="FormatPreference" />
											<asp:RadioButton ID="rbPlainText" runat="server" GroupName="FormatPreference" />
										</span>
									</asp:Panel>
									<asp:Panel CssClass="settingrow" ID="divCaptcha" runat="server">
										<mp:CaptchaControl ID="captcha" runat="server" TabIndex="0" />
									</asp:Panel>
									<div class="settingrow">
										<asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="profile" SkinID="Registration" />
										<asp:CustomValidator runat="server" ID="MustAgree" EnableClientScript="true" OnClientValidate="CheckBoxRequired_ClientValidate" Enabled="false" Display="None" ValidationGroup="profile" SkinID="Registration" />
									</div>
									<div class="regerror">
										<portal:mojoLabel ID="ErrorMessage" runat="server" CssClass="txterror" />
									</div>
									<div id="divAgreement" runat="server" class="regagree"></div>
									<div class="iagree">
										<asp:CheckBox ID="chkAgree" runat="server" />
									</div>
									<div class="settingrow">
										<mp:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel buttonspacer" ConfigKey="spacer" />
										<portal:mojoRegisterButton ID="StartNextButton" runat="server" CommandName="MoveNext" Text="Next" ValidationGroup="profile" CausesValidation="true" TabIndex="0" />
									</div>

								</ContentTemplate>
							</asp:CreateUserWizardStep>
							<asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
								<ContentTemplate>
									<asp:Panel ID="pnComplete" runat="server" />
									<asp:Literal ID="CompleteMessage" runat="server" />
									<div>
										<asp:Button ID="ContinueButton" runat="server" CausesValidation="False" CommandName="Continue" ValidationGroup="CreateUserWizard1" CssClass="art-button" />
									</div>
								</ContentTemplate>
							</asp:CompleteWizardStep>
						</WizardSteps>
						<StartNavigationTemplate></StartNavigationTemplate>
					</asp:CreateUserWizard>
					<asp:Literal ID="litTest" runat="server" />
				</asp:Panel>
				<asp:Panel ID="pnlThirdPartyAuth" runat="server" Visible="false" CssClass="clearpanel thirdpartyauth">
					<h2>
						<asp:Literal ID="litThirdPartyAuthHeading" runat="server" /></h2>
					<asp:Panel ID="pnlWindowsLiveID" runat="server" CssClass="windowslivepanel" Visible="false">
						<asp:HyperLink ID="lnkWindowsLiveID" runat="server" NavigateUrl="~/Secure/RegisterWithWindowsLiveID.aspx" />
						<br />
					</asp:Panel>
					<asp:Panel ID="divLiteralOr" runat="server" Visible="false" CssClass="clearpanel orpanel">
						<asp:Literal ID="litOr" runat="server" /><br />
						<br />
					</asp:Panel>
					<asp:Panel ID="pnlOpenID" runat="server" CssClass="openidpanel" Visible="false">
						<asp:HyperLink ID="lnkOpenIDRegistration" runat="server" NavigateUrl="~/Secure/RegisterWithOpenID.aspx" />
						<br />
					</asp:Panel>
					<asp:Panel ID="pnlRpx" runat="server" CssClass="openidpanel" Visible="false">
						<portal:OpenIdRpxNowLink ID="rpxLink" runat="server" />
						<br />
					</asp:Panel>
				</asp:Panel>
			</asp:Panel>
		</div>
	</asp:Panel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />