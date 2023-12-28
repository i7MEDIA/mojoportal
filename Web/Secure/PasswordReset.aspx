<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="PasswordReset.aspx.cs" Inherits="mojoPortal.Web.UI.Pages.PasswordResetPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper ">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<fieldset>
						<legend>
							<mp:SiteLabel ID="lblRegisterLabel" runat="server" ConfigKey="ChangePasswordRequired" UseLabelTag="false" />
						</legend>
						<asp:Panel ID="pnlResetPassword" runat="server" DefaultButton="btnChangePassword">
							<div class="settingrow">
								<strong>
									<mp:SiteLabel ID="SiteLabel1" runat="server" ForControl="NewPassword" ConfigKey="ChangePasswordNewPasswordLabel" />
								</strong>
								<br />
								<asp:TextBox ID="txtNewPassword" runat="server" TextMode="password" />
							</div>
							<div class="settingrow">
								<strong>
									<mp:SiteLabel ID="SiteLabel2" runat="server" ForControl="ConfirmNewPassword" ConfigKey="ChangePasswordConfirmNewPasswordLabel" />
								</strong>
								<br />
								<asp:TextBox ID="txtConfirmNewPassword" runat="server" TextMode="password" />
							</div>
							<div class="settingrow">
								<portal:mojoButton ID="btnChangePassword" CommandName="ChangePassword" Text="Change Password" runat="server" ValidationGroup="ChangePassword1" />
							</div>
							<div class="settingrow">
								<asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="ChangePassword1" />
								<asp:RequiredFieldValidator ControlToValidate="txtNewPassword" ID="NewPasswordRequired" runat="server" Display="None" ValidationGroup="ChangePassword1" />
								<asp:RequiredFieldValidator ControlToValidate="txtConfirmNewPassword" ID="ConfirmNewPasswordRequired" runat="server" Display="None" ValidationGroup="ChangePassword1" />
								<asp:CompareValidator ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmNewPassword" ID="NewPasswordCompare" runat="server" Display="None" ValidationGroup="ChangePassword1" />
								<asp:RegularExpressionValidator ID="NewPasswordRegex" runat="server" ControlToValidate="txtNewPassword" Display="None" ValidationGroup="ChangePassword1" />
								<asp:CustomValidator ID="NewPasswordRulesValidator" runat="server" ControlToValidate="txtNewPassword" Display="None" ValidationGroup="ChangePassword1" />
								<asp:Literal ID="FailureText" runat="server" EnableViewState="false" />
							</div>
						</asp:Panel>
					</fieldset>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
