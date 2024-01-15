<%@ Page Language="c#" CodeBehind="ProfileView.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.UI.Pages.ProfileView" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:ProfileDisplaySettings ID="displaySettings" runat="server" />
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper ">
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<asp:Panel ID="pnlProfile" runat="server" CssClass="profileview">
					<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
						<div class="settingrow usercreated">
							<mp:SiteLabel ID="lblCreatedDateLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersCreatedDateLabel" />
							&nbsp;<asp:Label ID="lblCreatedDate" runat="server" />
						</div>
						<div id="divForumPosts" runat="server" class="settingrow">
							<mp:SiteLabel ID="lblTotalPostsLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersTotalPostsLabel" />
							&nbsp;<asp:Label ID="lblTotalPosts" runat="server" />
							<portal:ForumUserThreadLink ID="lnkUserPosts" runat="server" />
						</div>
						<div class="settingrow">
							<mp:SiteLabel ID="lblUserNameLabel" runat="server" CssClass="settinglabel" ConfigKey="ManageUsersUserNameLabel" />
							&nbsp;<asp:Label ID="lblUserName" runat="server" />
						</div>
						<div id="divAvatar" runat="server" class="settingrow">
							<mp:SiteLabel ID="lblAvatar" runat="server" CssClass="settinglabel" ConfigKey="UserProfileAvatarLabel" ShowWarningOnMissingKey="false" />
							<portal:Avatar ID="userAvatar" runat="server" />
						</div>
						<asp:Panel ID="pnlTimeZone" runat="server" Visible="false" CssClass="settingrow timezone">
							<mp:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="TimeZone" />
							&nbsp;<asp:Label ID="lblTimeZone" runat="server" />
						</asp:Panel>
						<NeatHtml:UntrustedContent ID="UntrustedContent2" runat="server">
							<asp:Panel ID="pnlProfileProperties" runat="server" />
						</NeatHtml:UntrustedContent>
						<div style="clear: left;">
							<portal:mojoLabel ID="lblMessage" runat="server" CssClass="txterror info" />
						</div>
					</portal:InnerBodyPanel>
				</asp:Panel>
			</portal:OuterBodyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
