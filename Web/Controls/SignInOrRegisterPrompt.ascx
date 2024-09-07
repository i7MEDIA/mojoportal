<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SignInOrRegisterPrompt.ascx.cs"
	Inherits="mojoPortal.Web.UI.SignInOrRegisterPrompt" %>

<div class="floatpanel signinorregister">
	<div class="signinorregisterinstructions">
		<strong>
			<asp:literal id="litLoginInstructions" runat="server" />
		</strong>
	</div>

	<div class="floatpanel signinorregister-signinprompt">
		<asp:literal id="litLoginPrompt" runat="server" />
		<br class="brspacer" />
		<asp:hyperlink id="lnkLogin" runat="server" text="Login" />
	</div>

	<div class="floatpanel signinorregister-registerprompt">
		<asp:literal id="litRegisterPrompt" runat="server" />
		<br class="brspacer" />
		<asp:hyperlink id="lnkRegister" runat="server" text="Register" />
	</div>

	<div id="divAdditionalRegisterOptions" runat="server" visible="false" class="clearpanel signinorregister-additionalregisteroptions">
		<div>
			<asp:literal id="litAdditionalRegisterOptions" runat="server" />
		</div>

		<div id="divOpenId" runat="server" class="floatpanel signinorregister-registeropenid">
			<asp:literal id="litRegisterWithOpenId" runat="server" />
			<br class="brspacer" />
			<asp:hyperlink id="lnkRegisterWithOpenId" runat="server" text="Login" />
		</div>
	</div>

	<asp:panel id="pnlJanrain" runat="server" cssclass="clearpanel signinorregister-janrain" visible="false" enableviewstate="false">
		<portal:openidrpxnowlink id="janrainWidet" runat="server" />
	</asp:panel>
</div>
