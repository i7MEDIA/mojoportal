<%@ Page language="c#" Codebehind="ProfileView.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.UI.Pages.ProfileView" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:ProfileDisplaySettings id="displaySettings" runat="server" />
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
	<mp:CornerRounderTop id="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper ">
    <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
		<asp:Panel id="pnlProfile" runat="server" CssClass="profileview">
		    <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
		        <div class="settingrow usercreated">
		            <mp:SiteLabel id="lblCreatedDateLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersCreatedDateLabel" ></mp:SiteLabel>
		            &nbsp;<asp:Label id="lblCreatedDate" runat="server" ></asp:Label>
		        </div>
		        <div id="divForumPosts" runat="server" class="settingrow">
		            <mp:SiteLabel id="lblTotalPostsLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersTotalPostsLabel" ></mp:SiteLabel>
		            &nbsp;<asp:Label id="lblTotalPosts" runat="server" ></asp:Label>
		            <portal:ForumUserThreadLink id="lnkUserPosts" runat="server"  />
		        </div>
		        <div class="settingrow">
		            <mp:SiteLabel id="lblUserNameLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersUserNameLabel" ></mp:SiteLabel>
		            &nbsp;<asp:Label id="lblUserName" runat="server" ></asp:Label>
		        </div>
		        <div id="divAvatar" runat="server" class="settingrow">
		            <mp:SiteLabel id="lblAvatar" runat="server" CssClass="settinglabel" ConfigKey="UserProfileAvatarLabel" ShowWarningOnMissingKey="false"></mp:SiteLabel>
                    <portal:Avatar id="userAvatar" runat="server" />
		        </div>
                <asp:Panel ID="pnlTimeZone" runat="server" Visible="false" CssClass="settingrow timezone">
                     <mp:SiteLabel id="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="TimeZone" ></mp:SiteLabel>
		            &nbsp;<asp:Label id="lblTimeZone" runat="server" ></asp:Label>
                </asp:Panel>
		        <div id="divLiveMessenger" runat="server" visible="false" class="settingrow messengerpanel">
		            <mp:SiteLabel id="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="spacer" ></mp:SiteLabel>
		            <portal:LiveMessengerControl ID="chat1" runat="server" SkinID="profile"
		                    Width="400"
		                    Height="300"
                            Invitee=""
                            InviteeDisplayName=""
                            OverrideCulture=""
                            UseTheme="false"
                            ThemName=""
                            
                        />
		        </div>
                 <NeatHtml:UntrustedContent ID="UntrustedContent2" runat="server"
                            ClientScriptUrl="~/ClientScript/NeatHtml.js">
		        <asp:Panel ID="pnlProfileProperties" runat="server"></asp:Panel>
                </NeatHtml:UntrustedContent>
		        <div style="clear:left;">&nbsp;
		            <portal:mojoLabel ID="lblMessage" runat="server" CssClass="txterror info" />
		        </div>
		    </portal:InnerBodyPanel>
		</asp:Panel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
        </portal:InnerWrapperPanel>
	<mp:CornerRounderBottom id="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
