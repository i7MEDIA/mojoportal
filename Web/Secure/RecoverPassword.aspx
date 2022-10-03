<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="RecoverPassword.aspx.cs" Inherits="mojoPortal.Web.UI.Pages.RecoverPassword" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<asp:Panel ID="pnlRecoverPassword" runat="server" CssClass="panelwrapper login recoverpassword">
		<div class="modulecontent">

			<%--<mp:SiteLabel ID="lblHead" runat="server" ConfigKey="ForgotPasswordLabel" Format="<%$ mojoCode:  coreDisplaySettings.DefaultPageHeaderMarkup %>" />--%>
			<%--<asp:Label ID="lbl" runat="server" Text="<%$ mojoCode: string.Format(coreDisplaySettings.DefaultPageHeaderMarkup, Resources.Resource.ForgotPasswordLabel) %>" />--%>
			<asp:Literal ID="litHeading" runat="server" />

			<asp:Literal ID="litMessage" runat="server" />
			<asp:PasswordRecovery ID="PasswordRecovery1" runat="server">
				<UserNameTemplate>
					<asp:Panel ID="pnlRecover" runat="server" DefaultButton="SubmitButton">
						<div class="settingrow">
							<asp:Label ID="lblEnterUserName" AssociatedControlID="UserName" runat="server" Text="" />
							<br />
							<asp:TextBox ID="UserName" runat="server" MaxLength="100" CssClass="widetextbox" />
							<asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
								Display="Dynamic" ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
						</div>
						<div class="settingrow">
							<asp:Button ID="SubmitButton" runat="server" CommandName="Submit" ValidationGroup="PasswordRecovery1" />
						</div>
						<div class="settingrow">
							<asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="PasswordRecovery1" />
							<portal:mojoLabel ID="FailureText" runat="server" />
						</div>
					</asp:Panel>
				</UserNameTemplate>
				<QuestionTemplate>
					<asp:Panel ID="pnlRecover2" runat="server" DefaultButton="SubmitButton">
						<h2>
							<mp:SiteLabel ID="sitelabel2" runat="server" ConfigKey="ForgotPasswordLabel" />
						</h2>
						<div class="settingrow">
							<mp:SiteLabel ID="sitelabel4" runat="server" ConfigKey="HelloLabel" UseLabelTag="false" />
							<asp:Literal ID="UserName" runat="server" />
							<br />
							<mp:SiteLabel ID="sitelabel5" runat="server" ConfigKey="PasswordQuestionLabel" />
							<br />
							<asp:Literal ID="Question" runat="server" />
							<br />
							<asp:TextBox ID="Answer" runat="server" CssClass="verywidetextbox" />
							<asp:RequiredFieldValidator ID="AnswerRequired" runat="server" ControlToValidate="Answer" Display="Dynamic" ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
							<br />
							<asp:Button ID="SubmitButton" runat="server" CommandName="Submit" ValidationGroup="PasswordRecovery1" />
						</div>
						<div class="settingrow">
							<asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="PasswordRecovery1" />
							<portal:mojoLabel ID="FailureText" runat="server" CssClass="txterror warning" />
						</div>
					</asp:Panel>
				</QuestionTemplate>
				<SuccessTemplate>
					<mp:SiteLabel ID="successLabel" runat="server" ConfigKey="PasswordRecoverySuccessMessage" />
					<portal:mojoLabel ID="EmailLabel" runat="server" CssClass="alert alert-success" />
				</SuccessTemplate>
			</asp:PasswordRecovery>
			<portal:mojoLabel ID="lblMailError" runat="server" CssClass="txterror warning alert alert-danger" />
			<asp:Literal ID="litMailError" runat="server" />

		</div>
	</asp:Panel>

</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
