<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="PostListAlt.ascx.cs" Inherits="mojoPortal.Web.ForumUI.PostListAlt" %>
<%@ Register Namespace="mojoPortal.Web.ForumUI" Assembly="mojoPortal.Features.UI" TagPrefix="forum" %>

<forum:ForumDisplaySettings ID="displaySettings" runat="server" />

    <asp:HyperLink ID="lnkLogin" runat="server" CssClass="flogin" />
    <portal:mojoLabel ID="lblClosedTop" runat="server" CssClass="closedthreadmessage" Visible="false" EnableViewState="false" />
    <div class="modulepager" id="divPagerTop" runat="server"><portal:mojoCutePager ID="pgrTop" runat="server" /></div>
    <a href="" class="newthread newpost" id="lnkNewPost" runat="server" visible="false"></a>
    <asp:Repeater ID="rptMessages" runat="server" EnableViewState="False">
        <HeaderTemplate><ul class='simplelist  <%= displaySettings.PostListCssClass %>'></HeaderTemplate>
        <ItemTemplate>  
            <li class="forumwrap postwrap">    
                <div class="postright">
                    <div class="posttopic">
                        <h3>
                            <a id='post<%# DataBinder.Eval(Container.DataItem,"PostID") %>'></a>
                            <asp:HyperLink  CssClass="postEdit" ToolTip="<%# Resources.ForumResources.ForumEditPostLink %>"
                                ID="editLink" 
                                NavigateUrl='<%# FormatEditUrl(Convert.ToInt32(Eval("ForumID")),Convert.ToInt32(Eval("ThreadID")), Convert.ToInt32(Eval("PostID")))  %>'
                                Visible='<%# GetPermission(Convert.ToInt32(Eval("UserID")), Convert.ToBoolean(Eval("IsLocked")), Convert.ToDateTime(Eval("PostDate"))) %>' runat="server" />
                            <%# Server.HtmlEncode(Eval("Subject").ToString())%></h3>
                    </div>
                    
                    <div class="postbody" id="divUntrustedPost" runat="server" visible='<%# !(Convert.ToBoolean(Eval("Trusted")) || filterContentFromTrustedUsers) %>'>
                        <NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" TrustedImageUrlPattern='<%# allowedImageUrlRegexPattern %>'
                            ClientScriptUrl="~/ClientScript/NeatHtml.js">
                            <%# Eval("Post").ToString()%>
                        </NeatHtml:UntrustedContent>
                    </div>
                    <div class="postbody" id="divTrustedPost" runat="server" visible='<%# (Convert.ToBoolean(Eval("Trusted")) && !filterContentFromTrustedUsers) %>'>
                            <%# Eval("Post").ToString()%>
                    </div>
                </div>
                <div class="postuser">
                    <div class="forumpostusername">
                        <asp:HyperLink Text="Edit" ID="Hyperlink2" ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/user_edit.png"  %>'
                            NavigateUrl='<%# SiteRoot + "/Admin/ManageUsers.aspx?userid=" + DataBinder.Eval(Container.DataItem,"UserID")   %>'
                            Visible="<%# IsAdmin %>" runat="server" />
                        <%# GetProfileLinkOrLabel(Convert.ToInt32(Eval("UserID")), Eval("PostAuthor").ToString())%>
                    </div>
                    <div class="forumpostdate">
                        <%# FormatDate(Convert.ToDateTime(Eval("PostDate")))%>
                        <div data-tb='<%# Eval("PostID") %>' runat="server" visible='<%# IsModerator && !Convert.ToBoolean(Eval("NotificationSent")) %>' class="cmdbar">
                       <a id="lnkSendNotification" href="#" class="forumcommand sendnotification" runat="server" 
                            data-post='<%# Eval("PostID") %>' data-cmd="sendnotification"
                            ><%# Resources.ForumResources.SendNotification %></a>
                        <a id="lnkMarkAsSent" href="#" class="forumcommand marksent" runat="server" 
                            data-post='<%# Eval("PostID") %>' data-cmd="marksent"
                            ><%# Resources.ForumResources.MarkAsSent %></a>
                            </div>
                    </div>
                    <table class="forumuser">
                        <tr>
                            <td id="tdAvatars" runat="server" visible='<%# !displaySettings.HideAvatars %>'>
                                 <portal:Avatar id="av1" runat="server"
	                                UseLink='<%# UseProfileLink() %>'
	                                MaxAllowedRating='<%# MaxAllowedGravatarRating %>'
                                    AvatarFile='<%# Eval("PostAuthorAvatar") %>'
	                                UserName='<%# Eval("PostAuthor") %>'
                                    UserId='<%# Convert.ToInt32(Eval("UserID")) %>'
                                    SiteId='<%# SiteSettings.SiteId %>'
                                    SiteRoot='<%# SiteRoot %>'
	                                Email='<%# Eval("AuthorEmail") %>'
                                    UserNameTooltipFormat='<%# UserNameTooltipFormat %>'
	                                Disable='<%# disableAvatars %>'
	                                UseGravatar='<%# allowGravatars %>'
                                    GravatarFallbackEmailAddress="noreply@noreply.com"
                                    />
                                </div>
                            </td>
                            <td class="fuserstats">
                                <div class="forumpostuserattribute" id="divUserPostCount" runat="server" visible='<%# !displaySettings.HideUserTotalPosts %>'>
                                    <mp:SiteLabel ID="lblTotalPosts" runat="server" ConfigKey="ManageUsersTotalPostsLabel"
                                        UseLabelTag="false" />
                                    <%# Eval("PostAuthorTotalPosts") %>
                                </div>
                                <div class="forumpostuserattribute" id="divUserPostLink" runat="server" visible='<%# !displaySettings.HideUserTotalPosts %>'>
                                    <portal:ForumUserThreadLink ID="lnkUserPosts" runat="server" UserId='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem,"UserID")) %>'
                                        TotalPosts='<%# Convert.ToInt32(Eval("PostAuthorTotalPosts")) %>' />
                                </div>
                                <div id="divRevenue" runat="server" visible='<%# showUserRevenue %>' class="forumpostuserattribute">
                                    <mp:SiteLabel ID="SiteLabel1" runat="server" ConfigKey="UserSalesLabel" ResourceFile="ForumResources"
                                        UseLabelTag="false" />
                                    <%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("UserRevenue"))) %>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="forumpostuserattribute forumsig" id="divUntrustedSignature" runat="server" visible='<%# !Convert.ToBoolean(Eval("Trusted")) %>'>
                        <NeatHtml:UntrustedContent ID="UntrustedContent2" runat="server" TrustedImageUrlPattern='<%# allowedImageUrlRegexPattern %>'
                            ClientScriptUrl="~/ClientScript/NeatHtml.js">
                            <%# Eval("PostAuthorSignature").ToString()%>
                        </NeatHtml:UntrustedContent>
                    </div>
                    <div class="forumpostuserattribute forumsig" id="divTrustedSignature" runat="server" visible='<%# Convert.ToBoolean(Eval("Trusted")) %>'>
                       <NeatHtml:UntrustedContent ID="UntrustedContent3" runat="server" TrustedImageUrlPattern='<%# allowedImageUrlRegexPattern %>'
                            ClientScriptUrl="~/ClientScript/NeatHtml.js">
                            <%# Eval("PostAuthorSignature").ToString()%>
                        </NeatHtml:UntrustedContent>
                    </div>
                </div>
            </li>
        </ItemTemplate>
        <FooterTemplate></ul></FooterTemplate>
    </asp:Repeater>
    <div class="modulepager" id="divPagerBottom" runat="server"><portal:mojoCutePager ID="pgrBottom" runat="server" /></div>
    <a href="" class="newthread newpost" id="lnkNewPostBottom" runat="server" visible="false"></a>  
    <asp:HyperLink ID="lnkLoginBottom" runat="server" CssClass="flogin" />
    <portal:mojoLabel ID="lblClosedBottom" runat="server" CssClass="closedthreadmessage" Visible="false" EnableViewState="false" />
    <asp:Panel ID="pnlNotify" runat="server" Visible="false" CssClass="forumnotify">
    <asp:HyperLink ID="lnkNotify" runat="server"  />
        &nbsp;<asp:HyperLink ID="lnkNotify2" runat="server" />
    </asp:Panel>
