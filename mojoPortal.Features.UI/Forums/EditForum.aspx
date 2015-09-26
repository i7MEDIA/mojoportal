<%@ Page Language="c#" ValidateRequest="false" CodeBehind="EditForum.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="mojoPortal.Web.ForumUI.ForumEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper forummodule">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    <asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnUpdate">
        <div class="settingrow">
            <mp:SiteLabel ID="lblCreatedDateLabel" runat="server" CssClass="settinglabel" ConfigKey="ForumEditCreatedDateLabel" ResourceFile="ForumResources">
            </mp:SiteLabel>
            <asp:Label ID="lblCreatedDate" runat="server" CssClass="Normal forminput"></asp:Label>
        </div>
        <div class="settingrow">
            <mp:SiteLabel ID="lblTitleLabel" runat="server" ForControl="txtTitle" CssClass="settinglabel"
                ConfigKey="ForumEditTitleLabel" ResourceFile="ForumResources"></mp:SiteLabel>
            <asp:TextBox ID="txtTitle" runat="server" MaxLength="100" CssClass="widetextbox forminput"></asp:TextBox>
        </div>
        <div class="settingrow">
            <mp:SiteLabel ID="lblDescriptionLabel" runat="server" ForControl="fckDescription"
                CssClass="settinglabel" ConfigKey="ForumEditDescriptionLabel" ResourceFile="ForumResources" />
        </div>
        <div class="settingrow">
        <mpe:EditorControl ID="edContent" runat="server">
            </mpe:EditorControl>
        </div>
        
        <div class="settingrow" id="divIsModerated" runat="server">
            <mp:SiteLabel ID="lblIsModeratedLabel" runat="server" ForControl="chkIsModerated"
                CssClass="settinglabel" ConfigKey="ForumEditIsModeratedLabel" ResourceFile="ForumResources"></mp:SiteLabel>
            <asp:CheckBox ID="chkIsModerated" runat="server" CssClass="forminput"></asp:CheckBox>
        </div>
        <div class="settingrow" id="divIsActive" runat="server">
            <mp:SiteLabel ID="lblIsActiveLabel" runat="server" ForControl="chkIsActive" CssClass="settinglabel"
                ConfigKey="ForumEditIsActiveLabel" ResourceFile="ForumResources" />
            <asp:CheckBox ID="chkIsActive" runat="server" CssClass="forminput"></asp:CheckBox>
        </div>
        <div class="settingrow">
            <mp:SiteLabel ID="lblSortOrderLabel" runat="server" ForControl="txtSortOrder" CssClass="settinglabel"
                ConfigKey="ForumEditSortOrderLabel" ResourceFile="ForumResources" />
            <asp:TextBox ID="txtSortOrder" runat="server" MaxLength="5" CssClass="smalltextbox forminput"></asp:TextBox>
        </div>
        <div class="settingrow">
            <mp:SiteLabel ID="lblPostsPerPageLabel" runat="server" ForControl="txtPostsPerPage"
                CssClass="settinglabel" ConfigKey="ForumEditPostsPerPageLabel" ResourceFile="ForumResources" />
            <asp:TextBox ID="txtPostsPerPage" runat="server" MaxLength="5" CssClass="smalltextbox forminput"></asp:TextBox>
        </div>
        <div class="settingrow">
            <mp:SiteLabel ID="lblThreadsPerPageLabel" runat="server" ForControl="txtThreadsPerPage"
                CssClass="settinglabel" ConfigKey="ForumEditThreadsPerPageLabel" ResourceFile="ForumResources" />
            <asp:TextBox ID="txtThreadsPerPage" runat="server" MaxLength="5" CssClass="smalltextbox forminput"></asp:TextBox>
        </div>

        <div class="settingrow">
            <mp:SiteLabel ID="lblAllowAnonymousPosts" runat="server" 
                CssClass="settinglabel" ConfigKey="RolesThatCanPost" ResourceFile="ForumResources"></mp:SiteLabel>
            <portal:AllowedRolesSetting ID="allowedPostRoles" runat="server" />
        </div>
        <div class="settingrow">
            <mp:SiteLabel ID="SiteLabel8" runat="server" 
                CssClass="settinglabel" ConfigKey="RolesThatCanModerate" ResourceFile="ForumResources"></mp:SiteLabel>
            <portal:AllowedRolesSetting ID="moderatorRoles" runat="server" />
        </div>
        <div class="settingrow" >
            <mp:SiteLabel ID="SiteLabel1" runat="server" ForControl="chkRequireModForNotify" CssClass="settinglabel"
                ConfigKey="RequireModForNotify" ResourceFile="ForumResources" />
            <asp:CheckBox ID="chkRequireModForNotify" runat="server" CssClass="forminput"></asp:CheckBox>
            <portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="forum-require-mod-notify-help" />
        </div>
        <div class="settingrow" >
            <mp:SiteLabel ID="SiteLabel2" runat="server" ForControl="chkAllowTrustedDirectNotify" CssClass="settinglabel"
                ConfigKey="AllowTrustedDirectNotify" ResourceFile="ForumResources" />
            <asp:CheckBox ID="chkAllowTrustedDirectNotify" runat="server" CssClass="forminput"></asp:CheckBox>
            <portal:mojoHelpLink ID="MojoHelpLink2" runat="server" HelpKey="forum-allow-trusted-directnotify-help" />
        </div>
        <div class="settingrow">
            <mp:SiteLabel ID="SiteLabel3" runat="server" ForControl="txtTitle" CssClass="settinglabel"
                ConfigKey="ModeratorNotifyEmail" ResourceFile="ForumResources"></mp:SiteLabel>
            <asp:TextBox ID="txtModeratorNotifyEmail" runat="server" CssClass="verywidetextbox forminput"></asp:TextBox>
            <portal:mojoHelpLink ID="MojoHelpLink3" runat="server" HelpKey="forum-moderator-email-help" />
        </div>
        <div class="settingrow" >
            <mp:SiteLabel ID="SiteLabel4" runat="server" ForControl="chkIncludeInGoogleMap" CssClass="settinglabel"
                ConfigKey="IncludeInGoogleMap" ResourceFile="ForumResources" />
            <asp:CheckBox ID="chkIncludeInGoogleMap" runat="server" CssClass="forminput"></asp:CheckBox>
            <portal:mojoHelpLink ID="MojoHelpLink4" runat="server" HelpKey="forum-include-in-sitemap-default-help" />
        </div>
        <div class="settingrow" >
            <mp:SiteLabel ID="SiteLabel5" runat="server" ForControl="chkAddNoIndexMeta" CssClass="settinglabel"
                ConfigKey="DefaultAddNoIndexMeta" ResourceFile="ForumResources" />
            <asp:CheckBox ID="chkAddNoIndexMeta" runat="server" CssClass="forminput"></asp:CheckBox>
            <portal:mojoHelpLink ID="MojoHelpLink5" runat="server" HelpKey="forum-nnoindex-meta-default-help" />
        </div>
        <div class="settingrow" >
            <mp:SiteLabel ID="SiteLabel6" runat="server" ForControl="chkClosed" CssClass="settinglabel"
                ConfigKey="CloseForum" ResourceFile="ForumResources" />
            <asp:CheckBox ID="chkClosed" runat="server" CssClass="forminput"></asp:CheckBox>
        </div>
        <div id="divVisible" runat="server" visible="false" class="settingrow">
            <mp:SiteLabel ID="SiteLabel7" runat="server" ForControl="chkVisible" CssClass="settinglabel"
                ConfigKey="VisibleForum" ResourceFile="ForumResources" />
            <asp:CheckBox ID="chkVisible" runat="server" CssClass="forminput"></asp:CheckBox>
        </div>

        <div class="settingrow">
            <asp:Label ID="lblError" runat="server" CssClass="txterror"></asp:Label>
        </div>
        <div class="settingrow">
            <mp:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
            <div class="forminput">
            <portal:mojoButton ID="btnUpdate" runat="server" />&nbsp;
            <portal:mojoButton ID="btnDelete" runat="server" CausesValidation="false" />&nbsp;
            <asp:HyperLink ID="lnkCancel" runat="server" />
            
            </div>
        </div> 
        <asp:HiddenField ID="hdnReturnUrl" runat="server" />
    </asp:Panel>
  </portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
<portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
