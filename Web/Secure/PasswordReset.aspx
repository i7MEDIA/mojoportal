<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="PasswordReset.aspx.cs" Inherits="mojoPortal.Web.UI.Pages.PasswordResetPage" %>

<asp:content contentplaceholderid="leftContent" id="MPLeftPane" runat="server" />
<asp:content contentplaceholderid="mainContent" id="MPContent" runat="server">
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper ">
    <portal:HeadingControl ID="heading" runat="server" />
    <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
    <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
            <fieldset>
                <legend>
                    <mp:SiteLabel ID="lblRegisterLabel" runat="server" ConfigKey="ChangePasswordRequired"
                        UseLabelTag="false"></mp:SiteLabel>
                </legend>
                <asp:Panel ID="pnlResetPassword" runat="server" DefaultButton="btnChangePassword" >
                <div class="settingrow">
                    <strong>
                        <mp:SiteLabel ID="SiteLabel1" runat="server" ForControl="NewPassword" ConfigKey="ChangePasswordNewPasswordLabel">
                        </mp:SiteLabel>
                    </strong>
                    <br />
                    <asp:textbox id="txtNewPassword" runat="server" textmode="password" />
<%--                    <ajaxtoolkit:passwordstrength id="passwordStrengthChecker" runat="server" enabled="false"
                        targetcontrolid="txtNewPassword" displayposition="RightSide" strengthindicatortype="Text"
                        prefixtext="Strength:" textcssclass="pwdstrength" barbordercssclass="pwdstrengthbarborder"
                        barindicatorcssclass="pwdstrengthbar" textstrengthdescriptionstyles="pwspoor;pwsweak;pwsaverage;pwsstrong;pwsexcellent" />--%>
                </div>
                <div class="settingrow">
                    <strong>
                        <mp:SiteLabel ID="SiteLabel2" runat="server" ForControl="ConfirmNewPassword" ConfigKey="ChangePasswordConfirmNewPasswordLabel">
                        </mp:SiteLabel>
                    </strong>
                    <br />
                    <asp:textbox id="txtConfirmNewPassword" runat="server" textmode="password" />
                </div>
                <div class="settingrow">
                    <portal:mojoButton ID="btnChangePassword" CommandName="ChangePassword" Text="Change Password"
                        runat="server" ValidationGroup="ChangePassword1" />
                </div>
                <div class="settingrow">
                    <asp:validationsummary id="vSummary" runat="server" validationgroup="ChangePassword1" />
                    <asp:requiredfieldvalidator controltovalidate="txtNewPassword" id="NewPasswordRequired"
                        runat="server" display="None" validationgroup="ChangePassword1">
                    </asp:requiredfieldvalidator>
                    <asp:requiredfieldvalidator controltovalidate="txtConfirmNewPassword" id="ConfirmNewPasswordRequired"
                        runat="server" display="None" validationgroup="ChangePassword1">
                    </asp:requiredfieldvalidator>
                    <asp:comparevalidator controltocompare="txtNewPassword" controltovalidate="txtConfirmNewPassword"
                        id="NewPasswordCompare" runat="server" display="None" validationgroup="ChangePassword1">
                    </asp:comparevalidator>
                    <asp:regularexpressionvalidator id="NewPasswordRegex" runat="server" controltovalidate="txtNewPassword"
                        display="None" validationgroup="ChangePassword1">
                    </asp:regularexpressionvalidator>
                    <asp:customvalidator id="NewPasswordRulesValidator" runat="server" controltovalidate="txtNewPassword"
                        display="None" validationgroup="ChangePassword1">
                    </asp:customvalidator>
                    <asp:literal id="FailureText" runat="server" enableviewstate="false" />
                </div>
                </asp:Panel>
            </fieldset>
               
           
 </portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</portal:OuterWrapperPanel>
</asp:content>
<asp:content contentplaceholderid="rightContent" id="MPRightPane" runat="server" />
<asp:content contentplaceholderid="pageEditContent" id="MPPageEdit" runat="server" />
