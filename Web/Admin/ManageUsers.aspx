<%@ Page Language="c#" MaintainScrollPositionOnPostback="true" CodeBehind="ManageUsers.aspx.cs"
    MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.AdminUI.ManageUsers" %>
<%@ Register Src="~/Controls/UserCommerceHistory.ascx" TagPrefix="portal" TagName="PurchaseHistory" %>
<%@ Register Src="~/Admin/Controls/UserRoles.ascx" TagPrefix="portal" TagName="UserRoles" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:ProfileDisplaySettings id="displaySettings" runat="server" />
    <portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" EnableViewState="false" />
        <asp:HyperLink ID="lnkMemberList" runat="server" CssClass="unselectedcrumb" EnableViewState="false" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" EnableViewState="false" />
        <asp:HyperLink ID="lnkManageUser" runat="server" CssClass="selectedcrumb" EnableViewState="false" />
    </portal:AdminCrumbContainer>
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin manageusers">
        <portal:HeadingControl id="heading" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server" SkinID="admin">
            <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    <asp:Panel ID="pnlUserProfile" runat="server" DefaultButton="btnUpdate">
            <fieldset>
                <%-- <legend><span id="spnTitle" runat="server"></span></legend>
                <br /> --%>
                <div id="divtabs" class="mojo-tabs">
                    <ul>
                        <li class="selected">
                            <asp:Literal ID="litSecurityTab" runat="server" />
                         </li>
                        <li id="liProfile" runat="server">
                            <asp:Literal ID="litProfileTab" runat="server" />
                        </li>
                        <li id="liOrderHistory" runat="server">
                           <asp:Literal ID="litOrderHistoryTab" runat="server" />
                           </li>
                        <li id="liNewsletters" runat="server" visible="false">
                                <asp:Literal ID="litNewsletterTab" runat="server" />
                                </li>
                        <li id="liRoles" runat="server">
                            <asp:Literal ID="litRolesTab" runat="server" />
                            </li>
                        <li id="liActivity" runat="server">
                            <asp:Literal ID="litActivityTab" runat="server" />
                            </li>
                        <li id="liLocation" runat="server">
                            <asp:Literal ID="litLocationTab" runat="server" />
                            </li>
                    </ul>
                    
                        <div id="tabSecurity">
                            <div class="settingrow">
                                <mp:SiteLabel ID="lblUserName" runat="server" ForControl="txtName" CssClass="settinglabel"
                                    ConfigKey="ManageUsersUserNameLabel"></mp:SiteLabel>
                                <asp:TextBox ID="txtName" runat="server" TabIndex="10" Columns="45" MaxLength="100"
                                    CssClass="forminput widetextbox"></asp:TextBox>
                                <portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="userfullnamehelp" />
                            </div>
                            <div class="settingrow">
                                <mp:SiteLabel ID="lblFirstName" runat="server" ForControl="txtFirstName" CssClass="settinglabel"
                                    ResourceFile="ProfileResource" ConfigKey="FirstName" />
                                <asp:TextBox ID="txtFirstName" runat="server" TabIndex="10" Columns="45" MaxLength="100"
                                    CssClass="forminput widetextbox" />
                                <%--<portal:mojoHelpLink ID="MojoHelpLink18" runat="server" HelpKey="userfirstnamehelp" />--%>
                            </div>
                            <div class="settingrow">
                                <mp:SiteLabel ID="lblLastName" runat="server" ForControl="txtLastName" CssClass="settinglabel"
                                    ResourceFile="ProfileResource" ConfigKey="LastName" />
                                <asp:TextBox ID="txtLastName" runat="server" TabIndex="10" Columns="45" MaxLength="100"
                                    CssClass="forminput widetextbox" />
                                <%--<portal:mojoHelpLink ID="MojoHelpLink18" runat="server" HelpKey="userfirstnamehelp" />--%>
                            </div>
                            <div class="settingrow">
                                <mp:SiteLabel ID="SitelabelLoginName" runat="server" ForControl="txtLoginName" CssClass="settinglabel"
                                    ConfigKey="ManageUsersLoginNameLabel"></mp:SiteLabel>
                                <asp:TextBox ID="txtLoginName" runat="server" TabIndex="10" Columns="45" MaxLength="50"
                                    CssClass="forminput mediumtextbox"></asp:TextBox>
                                <portal:mojoHelpLink ID="MojoHelpLink2" runat="server" HelpKey="userloginnamehelp" />
                            </div>
                            <div class="settingrow">
                                <mp:SiteLabel ID="lblEmail" runat="server" ForControl="txtEmail" CssClass="settinglabel"
                                    ConfigKey="ManageUsersEmailLabel"></mp:SiteLabel>
                                <asp:TextBox ID="txtEmail" runat="server" Columns="45" TabIndex="10" MaxLength="100"
                                    CssClass="forminput widetextbox"></asp:TextBox>
                                <portal:mojoHelpLink ID="MojoHelpLink3" runat="server" HelpKey="useremailhelp" />
                            </div>
                            <div id="divOpenID" runat="server" class="settingrow">
                                <mp:SiteLabel ID="SiteLabel12" runat="server" ForControl="txtOpenIDURI" CssClass="settinglabel"
                                    ConfigKey="ManageUsersOpenIDURILabel"></mp:SiteLabel>
                                <asp:TextBox ID="txtOpenIDURI" runat="server" TabIndex="10" Columns="45" MaxLength="100"
                                    CssClass="forminput widetextbox"></asp:TextBox>
                                <portal:mojoHelpLink ID="MojoHelpLink4" runat="server" HelpKey="useropenidhelp" />
                            </div>
                            <div id="divWindowsLiveID" runat="server" class="settingrow">
                                <mp:SiteLabel ID="SiteLabel13" runat="server" ForControl="txtWindowsLiveID" CssClass="settinglabel"
                                    ConfigKey="ManageUsersWindowsLiveIDLabel"></mp:SiteLabel>
                                <asp:TextBox ID="txtWindowsLiveID" runat="server" TabIndex="10" Columns="45" MaxLength="100"
                                    CssClass="forminput widetextbox"></asp:TextBox>
                                <portal:mojoHelpLink ID="MojoHelpLink5" runat="server" HelpKey="manageuserwindowsliveidhelp" />
                            </div>
                            <div id="divLiveMessenger" runat="server">
                            <div  class="settingrow">
                                <mp:SiteLabel ID="SiteLabel14" runat="server" ForControl="txtLiveMessengerCID" CssClass="settinglabel"
                                    ConfigKey="LiveMessengerCIDLabel"></mp:SiteLabel>
                                <asp:TextBox ID="txtLiveMessengerCID" runat="server" TabIndex="10" Columns="45" MaxLength="255"
                                    CssClass="forminput widetextbox"></asp:TextBox>
                                <portal:mojoHelpLink ID="MojoHelpLink6" runat="server" HelpKey="livemessenger-cid-help" />
                            </div>
                            <div id="div1" runat="server" class="settingrow">
                                <mp:SiteLabel ID="SiteLabel15" runat="server" ForControl="chkEnableLiveMessengerOnProfile" CssClass="settinglabel"
                                    ConfigKey="EnableLiveMessengerLabel"></mp:SiteLabel>
                                <asp:CheckBox ID="chkEnableLiveMessengerOnProfile" runat="server" CssClass="forminput" />
                                <portal:mojoHelpLink ID="MojoHelpLink7" runat="server" HelpKey="livemessenger-admin-help" />
                            </div>
                            
                            </div>
                            <div id="divPassword" runat="server" class="settingrow">
                                <mp:SiteLabel ID="lblPassword" runat="server" ForControl="txtPassword" CssClass="settinglabel"
                                    ConfigKey="ManageUsersPasswordLabel"></mp:SiteLabel>
                                <asp:TextBox ID="txtPassword" runat="server" Columns="45" TabIndex="10" MaxLength="50"
                                    CssClass="forminput mediumtextbox"></asp:TextBox>
                                <portal:mojoHelpLink ID="MojoHelpLink8" runat="server" HelpKey="userpasswordhelp" />
                            </div>
                            <div id="divReqPasswordChange" runat="server" class="settingrow">
                                <mp:SiteLabel ID="SiteLabel17" runat="server" ForControl="chkRequirePasswordChange"
                                    CssClass="settinglabel" ConfigKey="UserMustChangePassword"></mp:SiteLabel>
                                <asp:CheckBox ID="chkRequirePasswordChange" runat="server" TabIndex="10" CssClass="forminput"></asp:CheckBox>
                                <portal:mojoHelpLink ID="MojoHelpLink17" runat="server" HelpKey="manageuser-mustchangepassword-help" />
                            </div>
                            <div class="settingrow" id="divSecurityQuestion" runat="server">
                                <mp:SiteLabel ID="SiteLabel1" runat="server" ForControl="txtPasswordQuestion" CssClass="settinglabel"
                                    ConfigKey="UsersSecurityQuestionLabel"></mp:SiteLabel>
                                <asp:TextBox ID="txtPasswordQuestion" runat="server" TabIndex="10" Columns="45" MaxLength="255"
                                    CssClass="forminput widetextbox"></asp:TextBox>
                                <portal:mojoHelpLink ID="MojoHelpLink9" runat="server" HelpKey="usersecurityquestionhelp" />
                            </div>
                            <div class="settingrow" id="divSecurityAnswer" runat="server">
                                <mp:SiteLabel ID="SiteLabel2" runat="server" ForControl="txtPasswordAnswer" CssClass="settinglabel"
                                    ConfigKey="UsersSecurityAnswerLabel"></mp:SiteLabel>
                                <asp:TextBox ID="txtPasswordAnswer" runat="server" TabIndex="10" Columns="45" MaxLength="255"
                                    CssClass="forminput widetextbox"></asp:TextBox>
                                <portal:mojoHelpLink ID="MojoHelpLink10" runat="server" HelpKey="usersecurityanswerhelp" />
                            </div>
                            <div id="divProfileApproved" runat="server" class="settingrow">
                                <mp:SiteLabel ID="lblProfileApproved" runat="server" ForControl="chkProfileApproved"
                                    CssClass="settinglabel" ConfigKey="ManageUsersProfileApprovedLabel"></mp:SiteLabel>
                                <asp:CheckBox ID="chkProfileApproved" runat="server" TabIndex="10" CssClass="forminput"></asp:CheckBox>
                                <portal:mojoHelpLink ID="MojoHelpLink11" runat="server" HelpKey="userprofileapprovedhelp" />
                            </div>
                            <div id="divApprovedForLogin" runat="server" class="settingrow">
                                <mp:SiteLabel ID="lblApprovedForLogin" runat="server" 
                                    CssClass="settinglabel" ConfigKey="ApprovedForLogin"></mp:SiteLabel>
                                <portal:mojoButton ID="btnApprove" runat="server" CausesValidation="false" />
                                <portal:mojoHelpLink ID="MojoHelpLink12" runat="server" HelpKey="manageuser-approvedforlogin-help" />
                            </div>
                            <div id="divTrusted" runat="server" class="settingrow">
                                <mp:SiteLabel ID="lblTrusted" runat="server" ForControl="chkTrusted" CssClass="settinglabel"
                                    ConfigKey="ManageUsersTrustedLabel"></mp:SiteLabel>
                                <asp:CheckBox ID="chkTrusted" runat="server" TabIndex="10" CssClass="forminput"></asp:CheckBox>
                                <portal:mojoHelpLink ID="MojoHelpLink13" runat="server" HelpKey="usertrustedhelp" />
                            </div>
                            <div id="divLockout" runat="server" class="settingrow">
                                <mp:SiteLabel ID="SiteLabel9" runat="server" ForControl="chkIsLockedOut" CssClass="settinglabel"
                                    ConfigKey="UserIsLockedOutLabel"></mp:SiteLabel>
                                <asp:CheckBox ID="chkIsLockedOut" runat="server" Enabled="false" CssClass="forminput" />
                                <portal:mojoButton ID="btnUnlockUser" runat="server" CausesValidation="false" />
                                <portal:mojoButton ID="btnLockUser" runat="server" CausesValidation="false" />
                                <portal:mojoHelpLink ID="MojoHelpLink14" runat="server" HelpKey="useradminunlockhelp" />
                            </div>
                            <div id="divEmailConfirm" runat="server" class="settingrow">
                                <mp:SiteLabel ID="SiteLabel11" runat="server" ForControl="chkEmailIsConfirmed" CssClass="settinglabel"
                                    ConfigKey="UserEmailIsConfirmedLabel"></mp:SiteLabel>
                                <asp:CheckBox ID="chkEmailIsConfirmed" runat="server" Enabled="false" CssClass="forminput" />
                                <portal:mojoButton ID="btnConfirmEmail" runat="server" CausesValidation="false" />
                                <portal:mojoButton ID="btnResendConfirmationEmail" runat="server" CausesValidation="false" />
                                <portal:mojoHelpLink ID="MojoHelpLink15" runat="server" HelpKey="useradminconfirmemailhelp" />
                            </div>
                            <div id="divDisplayInMemberList" runat="server" class="settingrow">
                                <mp:SiteLabel ID="lblDisplayInMemberList" runat="server" ForControl="chkDisplayInMemberList"
                                    CssClass="settinglabel" ConfigKey="ManageUsersDisplayInMemberListLabel"></mp:SiteLabel>
                                <asp:CheckBox ID="chkDisplayInMemberList" runat="server" TabIndex="10" CssClass="forminput"></asp:CheckBox>
                                <portal:mojoHelpLink ID="MojoHelpLink16" runat="server" HelpKey="userdisplayinmemberlisthelp" />
                            </div>
                            <div id="divEditorPreference" runat="server" visible="false" class="settingrow">
                                <mp:SiteLabel ID="SiteLabel29" runat="server" ForControl="ddEditorProviders" CssClass="settinglabel"
                                    ConfigKey="SiteSettingsEditorProviderLabel" EnableViewState="false"></mp:SiteLabel>
                                <asp:DropDownList ID="ddEditorProviders" DataTextField="name" DataValueField="name"
                                    EnableViewState="true" TabIndex="10" runat="server" CssClass="forminput">
                                </asp:DropDownList>
                                <portal:mojoHelpLink ID="MojoHelpLink20" runat="server" HelpKey="sitesettingssiteeditorproviderhelp" />
                            </div>
                            <div class="settingrow">&nbsp;</div>
                        </div>
                        <div id="tabProfile" runat="server">
                            <div id="divAvatarUrl" runat="server" >
                                <div class="settingrow">
                                <mp:SiteLabel ID="lblAvatar" runat="server" ForControl="ddAvatars" CssClass="settinglabel"
                                    ConfigKey="UserProfileAvatarLabel" ShowWarningOnMissingKey="false"></mp:SiteLabel>
                               <portal:Avatar id="userAvatar" runat="server" CssClass="forminput" />
                                </div>
                                <div class="settingrow">
                                <asp:HyperLink id="lnkAvatarUpld" runat="server" />
                                <portal:mojoHelpLink ID="avatarHelp" runat="server" HelpKey="useravatarhelp" />
                                </div>
                            </div>
                            <div id="divTimeZone" runat="server" visible="false" class="settingrow">
                                <mp:SiteLabel ID="SiteLabel16" runat="server" CssClass="settinglabel"
                                    ConfigKey="TimeZone"></mp:SiteLabel>
                                <portal:TimeZoneIdSetting ID="timeZoneSetting" runat="server" />
                            </div>
                            <asp:Panel ID="pnlProfileProperties" runat="server">
                            </asp:Panel>
                            <div class="settingrow">&nbsp;</div>
                        </div>
                        <div id="tabOrderHistory" runat="server">
                            <portal:PurchaseHistory id="purchaseHx" runat="server"></portal:PurchaseHistory>
                        </div>
                        <div id="tabNewsletters" runat="server">
                            <portal:SubscriberPreferences ID="newsLetterPrefs" runat="server" Visible="false">
                            </portal:SubscriberPreferences>
                        </div>
                        <div id="tabRoles" runat="server">
                            <portal:UserRoles id="UserRolesControl" runat="server" />
                        </div>
                        <div id="tabActivity" runat="server">
                            <div id="divCreatedDate" runat="server" class="settingrow">
                                <mp:SiteLabel ID="lblCreatedDateLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersCreatedDateLabel">
                                </mp:SiteLabel>
                                <asp:Label ID="lblCreatedDate" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div id="divTotalPosts" runat="server" class="settingrow">
                                <mp:SiteLabel ID="lblTotalPostsLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersTotalPostsLabel">
                                </mp:SiteLabel>
                                <asp:Label ID="lblTotalPosts" runat="server" CssClass="forminput"></asp:Label>
                                <portal:ForumUserThreadLink ID="lnkUserPosts" runat="server" />
                                <asp:HyperLink ID="lnkUnsubscribeFromForums" runat="server" />
                            </div>
                            <div id="divUserGuid" runat="server" class="settingrow">
                                <mp:SiteLabel ID="lblUserGuidLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersUserGuidLabel">
                                </mp:SiteLabel>
                                <asp:Label ID="lblUserGuid" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div id="divLastActivity" runat="server" class="settingrow">
                                <mp:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="UserLastActivityDateLabel">
                                </mp:SiteLabel>
                                <asp:Label ID="lblLastActivityDate" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div id="divLastLogin" runat="server" class="settingrow">
                                <mp:SiteLabel ID="SiteLabel4" runat="server" CssClass="settinglabel" ConfigKey="UserLastLoginDateLabel">
                                </mp:SiteLabel>
                                <asp:Label ID="lblLastLoginDate" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div id="divPasswordChanged" runat="server" class="settingrow">
                                <mp:SiteLabel ID="SiteLabel5" runat="server" CssClass="settinglabel" ConfigKey="UserLastPasswordChangeDateLabel">
                                </mp:SiteLabel>
                                <asp:Label ID="lblLastPasswordChangeDate" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div id="divLockoutDate" runat="server" class="settingrow">
                                <mp:SiteLabel ID="SiteLabel6" runat="server" CssClass="settinglabel" ConfigKey="UserLastLockoutDateLabel">
                                </mp:SiteLabel>
                                <asp:Label ID="lblLastLockoutDate" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div id="divFailedPasswordAttempt" runat="server" class="settingrow">
                                <mp:SiteLabel ID="SiteLabel7" runat="server" CssClass="settinglabel" ConfigKey="UserFailedPasswordAttemptCountLabel">
                                </mp:SiteLabel>
                                <asp:Label ID="lblFailedPasswordAttemptCount" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div id="divFailedPasswordAnswerAttempt" runat="server" class="settingrow">
                                <mp:SiteLabel ID="SiteLabel8" runat="server" CssClass="settinglabel" ConfigKey="UserFailedPasswordAnswerAttemptCountLabel">
                                </mp:SiteLabel>
                                <asp:Label ID="lblFailedPasswordAnswerAttemptCount" runat="server" CssClass="forminput"></asp:Label>
                            </div>
                            <div id="divComment" runat="server" class="settingrow">
                                <mp:SiteLabel ID="SiteLabel10" runat="server" ForControl="txtComment" CssClass="settinglabel"
                                    ConfigKey="UserCommentsLabel"></mp:SiteLabel>
                                <asp:TextBox ID="txtComment" runat="server" TabIndex="10" MaxLength="255" TextMode="MultiLine"
                                    Rows="15" Columns="55" CssClass="forminput"></asp:TextBox>
                                <portal:mojoHelpLink ID="MojoHelpLink19" runat="server" HelpKey="useradmincommenthelp" />
                            </div>
                            <div class="settingrow">&nbsp;</div>
                        </div>
                        <div id="tabLocation" runat="server">
                            <mp:mojoGridView ID="grdUserLocation" runat="server" AllowPaging="false" AutoGenerateColumns="false"
                                CssClass="editgrid" DataKeyNames="RowID" SkinID="plain">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                        <a class='cblink' title='<%# Eval("IPAddress") %>' href='http://whois.arin.net/rest/ip/<%# Eval("IPAddress") %>.txt'><%# Eval("IPAddress") %></a>         
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("Hostname") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("ISP") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("Continent") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("Country") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("Region") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("City") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("TimeZone") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Eval("CaptureCount") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("FirstCaptureUTC"), TimeOffset)%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("LastCaptureUTC"), TimeOffset)%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </mp:mojoGridView>
                            <portal:mojoButton ID="btnPurgeUserLocations" runat="server" CausesValidation="false" />
                        </div>
                    </div>
                    <div class="settingrow">
                        <asp:ValidationSummary ID="vSummary" runat="server" CssClass="txterror errors" ValidationGroup="profile"></asp:ValidationSummary>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" Display="none"
                            ControlToValidate="txtName" ValidationGroup="profile"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvLoginName" runat="server" Display="none"
                            ControlToValidate="txtLoginName" ValidationGroup="profile"></asp:RequiredFieldValidator>
                        <portal:EmailValidator ID="regexEmail" runat="server" ControlToValidate="txtEmail"
                            Display="None" ValidationGroup="profile"></portal:EmailValidator>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" Display="none"
                            ControlToValidate="txtEmail" ValidationGroup="profile"></asp:RequiredFieldValidator>
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                        <portal:mojoButton ID="btnUpdate" runat="server"  ValidationGroup="profile" />
                        <portal:mojoButton ID="btnDelete" runat="server" CausesValidation="false" /><br />
                        <portal:mojoLabel ID="lblErrorMessage" runat="server" CssClass="txterror warning" />
                    </div>
                
            </fieldset>
    </asp:Panel>
                </portal:InnerBodyPanel>
            </portal:OuterBodyPanel>
            <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
        </portal:InnerWrapperPanel>
        <mp:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
