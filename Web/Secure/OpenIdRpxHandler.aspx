<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="OpenIdRpxHandler.aspx.cs" Inherits="mojoPortal.Web.UI.OpenIdRpxHandlerPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
    <portal:RegistrationPageDisplaySettings id="displaySettings" runat="server" />
    <asp:Panel ID="pnlRegister" runat="server" CssClass="panelwrapper register">
        <div class="modulecontent">
            <fieldset>
                <legend>
                    <asp:Literal ID="litHeading" runat="server" />
                </legend>
                <asp:Panel ID="pnlOpenID" runat="server" CssClass="floatpanel" Visible="true">
                    <asp:Label ID="lblLoginFailed" runat="server" EnableViewState="False" Visible="False" />
                    <asp:Label ID="lblLoginCanceled" runat="server" EnableViewState="False" Visible="False" />
                    <asp:Literal ID="litResult" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlNeededProfileProperties" runat="server" Visible="false">
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="OpenIDLabel">
                        </mp:SiteLabel>
                        <asp:Literal ID="litOpenIDURI" runat="server" />
                    </div>
                    <div id="divEmailDisplay" runat="server" class="settingrow">
                        <mp:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="RegisterEmailLabel">
                        </mp:SiteLabel>
                        <asp:Literal ID="litEmail" runat="server" />
                    </div>
                    <div id="divEmailInput" runat="server" class="settingrow">
                        <mp:SiteLabel ID="lblRegisterEmail1" runat="server" ForControl="txtEmail" CssClass="settinglabel"
                            ConfigKey="RegisterEmailLabel"></mp:SiteLabel>
                        <asp:TextBox ID="txtEmail" runat="server" Columns="30" MaxLength="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="txtEmail" ID="EmailRequired" runat="server"
                            Display="Dynamic" ValidationGroup="profile"></asp:RequiredFieldValidator>
                        <portal:EmailValidator ID="regexEmail" runat="server" ControlToValidate="txtEmail"
                            Display="Dynamic" ValidationGroup="profile"></portal:EmailValidator>
                    </div>
                    <asp:Panel ID="pnlRequiredProfileProperties" runat="server" Visible="false">
                    </asp:Panel>
                    <asp:Panel ID="pnlSubscribe" runat="server" Visible="false">
                        <asp:Label ID="lblNewsletterListHeading" runat="server" CssClass="letterlist" />
                        <asp:CheckBoxList ID="clNewsletters" runat="server" DataTextField="Title" DataValueField="LetterInfoGuid" SkinID="RegisterNewsletters" RepeatLayout="UnorderedList"></asp:CheckBoxList>
                        <span id="spnFormat" class="emailformat">
                            <asp:RadioButton ID="rbHtmlFormat" runat="server" Checked="true" GroupName="FormatPreference" />
                            <asp:RadioButton ID="rbPlainText" runat="server" GroupName="FormatPreference" />
                        </span>
                    </asp:Panel>
                    <div class="modulebuttonrow">
                        <asp:HiddenField ID="hdnIdentifier" runat="server" />
                        <asp:HiddenField ID="hdnPreferredUsername" runat="server" />
                        <asp:HiddenField ID="hdnDisplayName" runat="server" />
                        <asp:HiddenField ID="hdnEmail" runat="server" />
                        <asp:Button ID="btnCreateUser" runat="server" Text="Register" />
                        <asp:CustomValidator runat="server" ID="MustAgree" EnableClientScript="true"
                                                 OnClientValidate="CheckBoxRequired_ClientValidate" Enabled="false" Display="Dynamic" ValidationGroup="profile"></asp:CustomValidator>
                    </div>
                    <div id="divAgreement" runat="server">
                    </div>
                    <div class="iagree">
                        <asp:CheckBox ID="chkAgree" runat="server" />
                    </div>
                    <div>
                        <asp:Literal ID="litInfoNeededMessage" runat="server" />
                    </div>
                </asp:Panel>
                
                <div class="clearpanel">
                    <portal:mojoLabel ID="lblError" runat="server" CssClass="txterror warning" />
                </div>
            </fieldset>
        </div>
    </asp:Panel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
