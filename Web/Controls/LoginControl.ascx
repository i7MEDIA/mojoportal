<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="LoginControl.ascx.cs" Inherits="mojoPortal.Web.UI.LoginControl" %>

<portal:SiteLogin ID="LoginCtrl" runat="server" CssClass="logincontrol">
	<LayoutTemplate>
		<asp:Panel ID="pnlLContainer" runat="server" DefaultButton="Login" CssClass="logincontrol">
			<div class="settingrow idrow">
				<mp:SiteLabel ID="lblEmail" runat="server" ForControl="UserName" ConfigKey="SignInEmailLabel" SkinID="settinglabel" />
				<mp:SiteLabel ID="lblUserID" runat="server" ForControl="UserName" ConfigKey="ManageUsersLoginNameLabel" SkinID="settinglabel" />
				<asp:TextBox ID="UserName" runat="server" CssClass="normaltextbox signinbox" MaxLength="100" />
			</div>
			<div class="settingrow passwordrow">
				<mp:SiteLabel ID="lblPassword" runat="server" ForControl="Password" ConfigKey="SignInPasswordLabel" SkinID="settinglabel" />
				<asp:TextBox ID="Password" runat="server" CssClass="normaltextbox passwordbox" TextMode="password" />
			</div>
			<div class="settingrow forgotpasswordrow">
				<asp:HyperLink ID="lnkPasswordRecovery" runat="server" CssClass="lnkpasswordrecovery" SkinID="LoginControlPasswordRecoveryLink" />
			</div>
			<div class="settingrow rememberrow">
				<asp:CheckBox ID="RememberMe" runat="server" />
			</div>
			<asp:Panel class="settingrow" ID="divCaptcha" runat="server">
				<mp:CaptchaControl ID="captcha" runat="server" />
			</asp:Panel>
			<div class="settingrow buttonrow">
				<portal:mojoButton ID="Login" CommandName="Login" runat="server" Text="Login" SkinID="LoginControlLoginButton" />
				<portal:mojoLabel ID="FailureText" runat="server" CssClass="txterror" EnableViewState="false" />
			</div>
			<div class="settingrow registerrow">
				<mp:SiteLabel ID="lblRegisterPrompt" runat="server" CssClass="registerprompt" EnableViewState="false" />
				<asp:HyperLink ID="lnkRegisterExtraLink" runat="server" CssClass="lnkregister" SkinID="LoginControlRegisterLink" />
			</div>
		</asp:Panel>
	</LayoutTemplate>
</portal:SiteLogin>