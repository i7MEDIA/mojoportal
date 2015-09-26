<%@ Page Language="c#" CodeBehind="EditThread.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="mojoPortal.Web.ForumUI.ForumThreadEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
   <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper forummodule">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    <asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnUpdate">
            <ol class="formlist">
                <li class="settingrow">
                    <mp:SiteLabel ID="lblThreadSubjectLabel" runat="server" ForControl="txtSubject" CssClass="settinglabel"
                        ConfigKey="ForumThreadEditSubjectLabel" ResourceFile="ForumResources" />
                    <asp:TextBox ID="txtSubject" runat="server" MaxLength="255" CssClass="verywidetextbox forminput"></asp:TextBox>
                </li>
                <li class="settingrow">
                    <mp:SiteLabel ID="SiteLabel1" runat="server" ForControl="txtSortOrder" CssClass="settinglabel"
                        ConfigKey="StickySort" ResourceFile="ForumResources" />
                    <asp:TextBox ID="txtSortOrder" runat="server" MaxLength="10" Text="100" CssClass="smalltextbox forminput"></asp:TextBox>
                    <portal:mojoHelpLink ID="MojoHelpLink2" runat="server" HelpKey="forum-stickysort-help" />
                </li>
                <li class="settingrow">
                    <mp:SiteLabel ID="SiteLabel5" runat="server" ForControl="txtPageTitleOverride" CssClass="settinglabel"
                        ConfigKey="PageTitleOverride" ResourceFile="ForumResources" />
                    <asp:TextBox ID="txtPageTitleOverride" runat="server" MaxLength="255" CssClass="verywidetextbox forminput"></asp:TextBox>
                    <portal:mojoHelpLink ID="MojoHelpLink6" runat="server" HelpKey="forum-thread-PageTitleOverride-help" />
                </li>
                <li class="settingrow">
                    <mp:SiteLabel ID="SiteLabel2" runat="server" ForControl="chkIsLocked" CssClass="settinglabel"
                        ConfigKey="Locked" ResourceFile="ForumResources" />
                    <asp:CheckBox ID="chkIsLocked" runat="server" />
                    <portal:mojoHelpLink ID="MojoHelpLink3" runat="server" HelpKey="forum-thread-islocked-help" />
                </li>
                <li class="settingrow">
                    <mp:SiteLabel ID="SiteLabel3" runat="server" ForControl="chkIncludeInSiteMap" CssClass="settinglabel"
                        ConfigKey="IncludeInSiteMap" ResourceFile="ForumResources" />
                    <asp:CheckBox ID="chkIncludeInSiteMap" runat="server" />
                    <portal:mojoHelpLink ID="MojoHelpLink4" runat="server" HelpKey="forum-thread-chkIncludeInSiteMap-help" />
                </li>
                <li class="settingrow">
                    <mp:SiteLabel ID="SiteLabel4" runat="server" ForControl="chkSetNoIndexMeta" CssClass="settinglabel"
                        ConfigKey="AddNoIndexMeta" ResourceFile="ForumResources" />
                    <asp:CheckBox ID="chkSetNoIndexMeta" runat="server" />
                    <portal:mojoHelpLink ID="MojoHelpLink5" runat="server" HelpKey="forum-thread-AddNoIndexMeta-help" />
                </li>
                <li class="settingrow">
                    <mp:SiteLabel ID="lblThreadForumLabel" runat="server" ForControl="ddForumList" CssClass="settinglabel"
                        ConfigKey="ForumThreadEditForumLabel" ResourceFile="ForumResources" />
                    <asp:DropDownList ID="ddForumList" runat="server" EnableTheming="false" CssClass="forminput"
                        AutoPostBack="False" DataTextField="Title" DataValueField="ItemID">
                    </asp:DropDownList>
                </li>
                <li class="settingrow">
                    <asp:Label ID="lblError" runat="server" CssClass="txterror"></asp:Label>
                </li>
                <li class="settingrow">
                    <mp:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                    <div class="forminput">
                        <portal:mojoButton ID="btnUpdate" runat="server" Text="Update" />&nbsp;
                        <portal:mojoButton ID="btnDelete" runat="server" CausesValidation="false" />&nbsp;
                        <asp:HyperLink ID="lnkCancel" runat="server" />
                        <portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="forumthreadedithelp" />
                    </div>
                </li>
            </ol>
    </asp:Panel>
    <asp:HiddenField ID="hdnReturnUrl" runat="server" />
  </portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
