<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="LoginControl.ascx.cs"
    Inherits="mojoPortal.Web.UI.LoginControl" %>
<portal:SiteLogin ID="LoginCtrl" runat="server" CssClass="logincontrol">
    <LayoutTemplate>
        <asp:Panel id="pnlLContainer" runat="server" defaultbutton="Login">
            <div class="settingrow idrow">
                <strong>
                    <mp:SiteLabel ID="lblEmail" runat="server" ForControl="UserName" ConfigKey="SignInEmailLabel">
                    </mp:SiteLabel>
                    <mp:SiteLabel ID="lblUserID" runat="server" ForControl="UserName" ConfigKey="ManageUsersLoginNameLabel">
                    </mp:SiteLabel>
                </strong>
                <br />
                <asp:TextBox id="UserName" runat="server" CssClass="normaltextbox signinbox" maxlength="100" />
            </div>
            <div class="settingrow passwordrow">
                <strong>
                    <mp:SiteLabel ID="lblPassword" runat="server" ForControl="Password" ConfigKey="SignInPasswordLabel">
                    </mp:SiteLabel>
                </strong>
                <br />
                <asp:TextBox id="Password" runat="server" CssClass="normaltextbox passwordbox" textmode="password" />
            </div>
            <div class="settingrow rememberrow">
                <asp:CheckBox id="RememberMe" runat="server" />
            </div>
            <asp:Panel class="settingrow" id="divCaptcha" runat="server">
                <mp:CaptchaControl ID="captcha" runat="server" />
            </asp:Panel>
            <div class="settingrow buttonrow">
                <portal:mojoButton ID="Login" CommandName="Login" runat="server" Text="Login" />
                <br />
                <portal:mojoLabel ID="FailureText" runat="server" CssClass="txterror" EnableViewState="false" />
            </div>
            <div class="settingrow registerrow">
                <asp:Hyperlink id="lnkPasswordRecovery" runat="server" CssClass="lnkpasswordrecovery"/>&nbsp;&nbsp;
                <asp:Hyperlink id="lnkRegisterExtraLink" runat="server" CssClass="lnkregister" />
            </div>
        </asp:Panel>
    </LayoutTemplate>
</portal:SiteLogin>

