<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ContactForm.ascx.cs" Inherits="mojoPortal.Web.ContactUI.ContactForm" %>

<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server" CssClass="contactform-wrap">
	<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper contactform">
		<portal:ModuleTitleControl id="Title1" runat="server"  />
		<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
			<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
				<asp:Panel ID="pnlSend" runat="server" SkinID="plain" DefaultButton="btnSend">
					<asp:Panel ID="pnlToAddresses" runat="server" Visible="false" CssClass="settingrow">
						<mp:SiteLabel id="SiteLabel1" runat="server" ForControl="ddToAddresses" ConfigKey="ToLabel" ResourceFile="ContactFormResources" CssClass="settinglabel" />
						<asp:DropDownList ID="ddToAddresses" runat="server" CssClass="forminput" EnableViewState="true" EnableTheming="false" />
					</asp:Panel>
					<div class="settingrow">
						<mp:SiteLabel id="lblEmail" runat="server" ForControl="txtEmail" ConfigKey="ContactFormYourEmailLabel" ResourceFile="ContactFormResources" CssClass="settinglabel" />
						<asp:TextBox id="txtEmail" runat="server" CssClass="forminput widetextbox" maxlength="50" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel id="lblName" runat="server" ForControl="txtName" ConfigKey="ContactFormYourNameLabel" ResourceFile="ContactFormResources" CssClass="settinglabel" />
						<asp:TextBox id="txtName" runat="server" cssclass="forminput widetextbox" maxlength="100" />
					</div>
					<div class="settingrow">
						<mp:SiteLabel id="lblSubject" runat="server" ForControl="txtSubject" ConfigKey="ContactFormSubjectLabel" ResourceFile="ContactFormResources" CssClass="settinglabel" />
						<asp:TextBox id="txtSubject" runat="server" cssclass="forminput widetextbox"  MaxLength="50" />
					</div>
					<div class="settingrow">
					 <mp:SiteLabel id="lblMessageLabel" runat="server" ForControl="edMessage" ConfigKey="ContactFormMessageLabel" ResourceFile="ContactFormResources" CssClass="settinglabel" />
					</div>
					<div class="settingrow">
						<mpe:EditorControl id="edMessage" runat="server" />
					</div>
					<div class="settingrow">
						<asp:ValidationSummary id="vSummary" runat="server" ValidationGroup="Contact" CssClass="txterror" />
						<asp:RequiredFieldValidator id="reqEmail" ValidationGroup="Contact" runat="server" CssClass="txterror" Display="none" ControlToValidate="txtEmail" SetFocusOnError="true" />
						<portal:EmailValidator ID="regexEmail" runat="server" Display="none" ValidationGroup="Contact" ControlToValidate="txtEmail" SetFocusOnError="true" />
					</div>
					<div class="settingrow" id="divCaptcha" runat="server">
					   <mp:CaptchaControl id="captcha" runat="server" />
					 </div>
					<div class="modulebuttonrow">
						<portal:mojoButton ID="btnSend" Runat="server" ValidationGroup="Contact" Text="Send" CausesValidation="true" />
					</div>
				</asp:Panel>
				<portal:mojoLabel ID="lblMessage" Runat="server" CssClass="txterror" SkinID="error" />
			</portal:InnerBodyPanel>
		</portal:OuterBodyPanel>
	</portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>

