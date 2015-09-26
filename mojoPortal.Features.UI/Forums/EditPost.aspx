<%@ Page Language="c#" CodeBehind="EditPost.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.ForumUI.ForumPostEdit" %>
<%@ Register TagPrefix="forum" TagName="PostList" Src="~/Forums/Controls/PostList.ascx" %>
<%@ Register TagPrefix="forum" TagName="PostListAlt" Src="~/Forums/Controls/PostListAlt.ascx" %>
<%@ Register Namespace="mojoPortal.Web.ForumUI" Assembly="mojoPortal.Features.UI" TagPrefix="forum" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <div class="breadcrumbs forumthreadcrumbs">
        <asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink>
        &gt; <a href="" id="lnkForum" runat="server"></a>&nbsp;&gt;
        <asp:Label ID="lblThreadDescription" runat="server"></asp:Label>
    </div>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper forummodule forumeditpost">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    <asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnUpdate">
            <div id="divDescription" runat="server" class="settingrow forumdesc">
                <asp:Literal ID="litForumDescription" runat="server" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="lblSubjectLabel" runat="server" ForControl="txtSubject" CssClass="settinglabel"
                    ConfigKey="ForumPostEditSubjectLabel" ResourceFile="ForumResources"></mp:SiteLabel>
                <asp:TextBox ID="txtSubject" runat="server" MaxLength="255" CssClass="verywidetextbox forminput"></asp:TextBox>
            </div>
            <div class="settingrow">
                <mpe:EditorControl ID="edMessage" runat="server">
                </mpe:EditorControl>
            </div>
            <div class="settingrow" id="divSortOrder" runat="server" visible="false">
                <mp:SiteLabel ID="SiteLabel2" runat="server" ForControl="txtSortOrder" CssClass="settinglabel"
                    ConfigKey="StickySort" ResourceFile="ForumResources" />
                <asp:TextBox ID="txtSortOrder" runat="server" MaxLength="10" Text="100" CssClass="smalltextbox forminput"></asp:TextBox>
                <portal:mojoHelpLink ID="MojoHelpLink2" runat="server" HelpKey="forum--post-stickysort-help" />
            </div>
            <asp:Panel ID="pnlNotify" runat="server">
                <div class="settingrow">
                    <mp:SiteLabel ID="lblNotifyOnReply" runat="server" ForControl="chkNotifyOnReply"
                        CssClass="settinglabel" ConfigKey="ForumPostEditNotifyLabel" ResourceFile="ForumResources">
                    </mp:SiteLabel>
                    <asp:CheckBox ID="chkNotifyOnReply" runat="server" CssClass="forminput"></asp:CheckBox>
                </div>
                <div class="settingrow">
                    <mp:SiteLabel ID="SiteLabel1" runat="server" ForControl="chkSubscribeToForum" CssClass="settinglabel"
                        ConfigKey="SubscribeToAllOfThisForum" ResourceFile="ForumResources"></mp:SiteLabel>
                    <asp:CheckBox ID="chkSubscribeToForum" runat="server" CssClass="forminput"></asp:CheckBox>
                </div>
            </asp:Panel>
            <div class="settingrow">
                <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
                <asp:RequiredFieldValidator ID="reqSubject" runat="server" ControlToValidate="txtSubject"
                    ValidationGroup="Forum" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            <asp:Panel ID="pnlAntiSpam" runat="server">
                <mp:CaptchaControl ID="captcha" runat="server" />
            </asp:Panel>
            <div class="settingrow">
                <mp:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                <div class="forminput">
                    <portal:mojoButton ID="btnUpdate" runat="server" ValidationGroup="Forum" />&nbsp;
                    <portal:mojoButton ID="btnDelete" runat="server" CausesValidation="false" />&nbsp;
                    <asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" />&nbsp;
                </div>
            </div>
        <div class="settingrow forumspacer">&nbsp;</div>
        <forum:ForumDisplaySettings ID="displaySettings" runat="server" />
        <forum:PostList id="postList" runat="server" UseReverseSort="true" />
        <forum:PostListAlt id="postListAlt" runat="server" Visible="false" UseReverseSort="true" />           
    </asp:Panel>
    <asp:HiddenField ID="hdnReturnUrl" runat="server" />  
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
