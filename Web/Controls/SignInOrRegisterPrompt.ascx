<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SignInOrRegisterPrompt.ascx.cs"
    Inherits="mojoPortal.Web.UI.SignInOrRegisterPrompt" %>
    
<div class="floatpanel signinorregister">
<div class="signinorregisterinstructions">
    <strong><asp:Literal ID="litLoginInstructions" runat="server" /></strong>
</div>
<div class="floatpanel signinorregister-signinprompt">
    <asp:Literal ID="litLoginPrompt" runat="server" /><br class="brspacer" />
    <asp:HyperLink ID="lnkLogin" runat="server" Text="Login" />
</div>
<div class="floatpanel signinorregister-registerprompt">
    <asp:Literal ID="litRegisterPrompt" runat="server" /><br class="brspacer" />
    <asp:HyperLink ID="lnkRegister" runat="server" Text="Register" />
</div>
<div id="divAdditionalRegisterOptions" runat="server" visible="false" class="clearpanel signinorregister-additionalregisteroptions">
<div>
    <asp:Literal ID="litAdditionalRegisterOptions" runat="server" />
</div>
<div id="divOpenId" runat="server" class="floatpanel signinorregister-registeropenid">
    <asp:Literal ID="litRegisterWithOpenId" runat="server" /><br class="brspacer" />
    <asp:HyperLink ID="lnkRegisterWithOpenId" runat="server" Text="Login" />
</div>
<div id="divWindowsLive" runat="server" class="floatpanel signinorregister-registerwindowslive">
    <asp:Literal ID="litRegisterWithWindowsLive" runat="server" /><br class="brspacer" />
    <asp:HyperLink ID="lnkRegisterWithWindowsLive" runat="server" Text="Register" />
</div>

</div>
<asp:Panel ID="pnlJanrain" runat="server" CssClass="clearpanel signinorregister-janrain" Visible="false" EnableViewState="false">
    <portal:OpenIdRpxNowLink id="janrainWidet" runat="server" />
</asp:Panel>
</div>
