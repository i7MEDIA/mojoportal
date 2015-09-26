<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="PostList.ascx.cs" Inherits="mojoPortal.Web.BlogUI.PostList" %>
<%@ Register TagPrefix="blog" TagName="NavControl" Src="~/Blog/Controls/BlogNav.ascx" %>
<%@ Register Namespace="mojoPortal.Web.BlogUI" Assembly="mojoPortal.Features.UI" TagPrefix="blog" %>

<blog:BlogDisplaySettings ID="displaySettings" runat="server" />
<blog:NavControl ID="navTop" runat="server" />
<blog:BlogPostListWrapperPanel ID="divblog" runat="server" CssClass="blogcenter-rightnav">
    <asp:Repeater ID="rptBlogs" runat="server" SkinID="Blog" EnableViewState="False">
        <ItemTemplate>
            <blog:BlogPostListItemPanel id="bi1" runat="server" CssClass="blogitem">
                <<%# itemHeadingElement %> class="blogtitle">
                    <asp:HyperLink SkinID="BlogTitle" ID="lnkTitle" runat="server" EnableViewState="false" CssClass="blogitemtitle"
                        Text='<%# Eval("Heading") %>' Visible='<%# Config.UseLinkForHeading %>'
                        NavigateUrl='<%# FormatBlogTitleUrl(Eval("ItemUrl").ToString(), Convert.ToInt32(Eval("ItemID")))  %>'>
                    </asp:HyperLink><asp:Literal ID="litTitle" runat="server" Text='<%# Eval("Heading") %>'
                        Visible='<%#(!Config.UseLinkForHeading) %>' />&nbsp;
                    <asp:HyperLink ID="editLink" runat="server" EnableViewState="false" Text="<%# EditLinkText %>"
                        ToolTip="<%# EditLinkTooltip %>" ImageUrl='<%# EditLinkImageUrl %>' NavigateUrl='<%# this.SiteRoot + "/Blog/EditPost.aspx?pageid=" + PageId.ToString() + "&amp;ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&amp;mid=" + ModuleId.ToString() %>'
                        Visible='<%# CanEditPost(Convert.ToInt32(Eval("UserID"))) %>' CssClass="ModuleEditLink" /></<%# itemHeadingElement %>>
                <asp:Literal ID="litSubtitle" runat="server" EnableViewState="false" Text='<%# FormatSubtitle(Eval("SubTitle").ToString()) %>' />
                <% if (!displaySettings.PostListUseBottomDate && !TitleOnly)
                   { %>
                <div class="blogdate">
                    <span class="blogauthor">
                        <%# FormatPostAuthor(Convert.ToBoolean(Eval("ShowAuthorName")), Eval("Name").ToString(),Eval("FirstName").ToString(),Eval("LastName").ToString())%></span>
                    <span class="bdate" id="spnTopDate" runat="server" enableviewstate="false" visible='<%# !displaySettings.PostListHideDate %>'>
                        <%# FormatBlogDate(Convert.ToDateTime(Eval("StartDate"))) %></span>
                        <asp:Repeater id="rptTopCategories" runat="server" Visible='<%# displaySettings.ShowTagsOnPostList %>'>
                            <HeaderTemplate><span class="blogtags tagslabel"><mp:sitelabel id="lblcatBottom" runat="server" ConfigKey='<%# CategoriesResourceKey %>' ResourceFile="BlogResources" UseLabelTag="false" ShowWarningOnMissingKey="false" /></span><span class="blogtags"> </HeaderTemplate>
                            <ItemTemplate>
                            <asp:HyperLink id="Hyperlink6" runat="server" EnableViewState="false" 
                                    Text='<%# Eval("Category").ToString() %>' data-category='<%# Eval("Category").ToString() %>' 
                                    NavigateUrl='<%# this.SiteRoot + "/Blog/ViewCategory.aspx?cat=" + DataBinder.Eval(Container.DataItem,"CategoryID") + "&amp;pageid=" + PageId.ToString() + "&amp;mid=" + ModuleId.ToString()  %>'>
                                </asp:HyperLink>
                            </ItemTemplate>
                            <FooterTemplate></span></FooterTemplate>
                            </asp:Repeater>
                </div>
                <% } %>
                <asp:Panel ID="pnlPost" runat="server" Visible='<%# !TitleOnly %>'>
                    <portal:mojoRating runat="server" ID="Rating" Enabled='<%# EnableContentRating && !displaySettings.UseBottomContentRating %>'
                        ContentGuid='<%# new Guid(Eval("BlogGuid").ToString()) %>' AllowFeedback='false' />
                    <mp:OdiogoItem ID="od1" runat="server" OdiogoFeedId='<%# Config.OdiogoFeedId %>'
                        ItemId='<%# DataBinder.Eval(Container.DataItem,"ItemID") %>' ItemTitle='<%# Eval("Heading") %>' />
                    <div class="blogtext">
                        <%# FormatBlogEntry(Eval("Description").ToString(), 
                        Eval("Abstract").ToString(), 
                        Eval("ItemUrl").ToString(), 
                        Convert.ToInt32(Eval("ItemID")),
                        Eval("HeadlineImageUrl").ToString(),
                        Convert.ToBoolean(Eval("IncludeImageInExcerpt"))
                        )
                        %></div>
                    <asp:Repeater ID="rptAttachments" runat="server" Visible='<%# !useExcerpt && !TitleOnly %>'>
                        <ItemTemplate>
                            <portal:MediaElement ID="ml1" runat="server" EnableViewState="false" FileUrl='<%# attachmentBaseUrl + Eval("ServerFileName") %>' AddInitScript="true" IncludeDownloadLinkForMedia='<%# Convert.ToBoolean(Eval("ShowDownloadLink")) %>' />
                        </ItemTemplate>
                    </asp:Repeater>
                    <portal:LocationMap ID="gmap" runat="server" Visible='<%# ((Eval("Location").ToString().Length > 0)&&(ShowGoogleMap &&(!Convert.ToBoolean(Eval("UseBingMap"))))) %>'
                        Location='<%# Eval("Location") %>' GMapApiKey='<%# GmapApiKey %>' EnableMapType='<%# Convert.ToBoolean(Eval("ShowMapOptions")) %>'
                        EnableZoom='<%# Convert.ToBoolean(Eval("ShowZoomTool")) %>' ShowInfoWindow='<%# Convert.ToBoolean(Eval("ShowLocationInfo")) %>'
                        EnableLocalSearch='false' EnableDrivingDirections='<%# Convert.ToBoolean(Eval("UseDrivingDirections")) %>'
                        GmapType='<%# (mojoPortal.Web.Controls.google.MapType)Enum.Parse(typeof(mojoPortal.Web.Controls.google.MapType), Eval("MapType").ToString()) %>' ZoomLevel='<%# Convert.ToInt32(Eval("MapZoom")) %>'
                        MapHeight='<%# Convert.ToInt32(Eval("MapHeight").ToString()) %>' MapWidth='<%# Eval("MapWidth").ToString() %>'>
                    </portal:LocationMap>
                    <portal:BingMap id="bmap" runat="server" Visible='<%# ((Eval("Location").ToString().Length > 0)&&(ShowGoogleMap && Convert.ToBoolean(Eval("UseBingMap")))) %>' Location='<%# Eval("Location") %>'
                        MapStyle='<%# BlogConfiguration.GetBingMapType(Eval("MapType").ToString()) %>' Height='<%# Convert.ToInt32(Eval("MapHeight").ToString()) %>' MapWidth='<%# Eval("MapWidth").ToString() %>'
                        ShowGetDirections='<%# Convert.ToBoolean(Eval("UseDrivingDirections")) %>' Zoom='<%# Convert.ToInt32(Eval("MapZoom")) %>' ShowMapControls='<%# Convert.ToBoolean(Eval("ShowMapOptions")) %>'
                        ShowLocationPin='<%# Convert.ToBoolean(Eval("ShowLocationInfo")) %>'  />
                    <asp:Panel ID="pnlAvatar" runat="server" EnableViewState="false" Visible='<%# !disableAvatars && !displaySettings.HideAvatarInPostList && ( (Convert.ToBoolean(Eval("ShowAuthorAvatar"))) || ((Convert.ToBoolean(Eval("ShowAuthorBio")))) ) %>' CssClass="avatarwrap authorinfo">
                    <portal:Avatar id="av1" runat="server"
	                    UseLink='<%# UseProfileLink() %>'
	                    MaxAllowedRating='<%# MaxAllowedGravatarRating %>'
                        AvatarFile='<%# Eval("AvatarUrl") %>'
	                    UserName='<%# Eval("Name") %>'
                        UserId='<%# Convert.ToInt32(Eval("UserID")) %>'
                        SiteId='<%# SiteId %>'
                        SiteRoot='<%# SiteRoot %>'
	                    Email='<%# Eval("Email") %>'
                        UserNameTooltipFormat='<%# UserNameTooltipFormat %>'
	                    Disable='<%# disableAvatars || displaySettings.HideAvatarInPostList || !Convert.ToBoolean(Eval("ShowAuthorAvatar")) %>'
	                    UseGravatar='<%# allowGravatars %>'
                        /> <span id="spnAuthorBio" runat="server" visible='<%# displaySettings.ShowAuthorBioInPostList && Convert.ToBoolean(Eval("ShowAuthorBio")) %>' enableviewstate="false" class="authorbio"><%# Eval("AuthorBio") %></span>
                        </asp:Panel>
                    <% if (displaySettings.PostListUseBottomDate)
                       { %>
                    <div class="blogdate">
                        <span class="blogauthor" id="spnAuthor" runat="server" enableviewstate="false" visible='<%# Convert.ToBoolean(Eval("ShowAuthorName"))  %>'>
                            <%# FormatPostAuthor(Convert.ToBoolean(Eval("ShowAuthorName")),Eval("Name").ToString(),Eval("FirstName").ToString(),Eval("LastName").ToString())%></span>
                        <span class="bdate" id="spnBottomDate" runat="server" enableviewstate="false" visible='<%# !displaySettings.PostListHideDate %>'>
                            <%# FormatBlogDate(Convert.ToDateTime(Eval("StartDate"))) %></span>
                            <asp:Repeater id="rptBottomCategories" runat="server" Visible='<%# displaySettings.ShowTagsOnPostList %>'>
                            <HeaderTemplate><span class="blogtags tagslabel"><mp:sitelabel id="lblcatBottom" runat="server" ConfigKey='<%# CategoriesResourceKey %>' ResourceFile="BlogResources" UseLabelTag="false" ShowWarningOnMissingKey="false" /></span><span class="blogtags"> </HeaderTemplate>
                            <ItemTemplate>
                            <asp:HyperLink id="Hyperlink5" runat="server" EnableViewState="false" 
                                    Text='<%# Eval("Category").ToString() %>' 
                                    NavigateUrl='<%# this.SiteRoot + "/Blog/ViewCategory.aspx?cat=" + DataBinder.Eval(Container.DataItem,"CategoryID") + "&amp;pageid=" + PageId.ToString() + "&amp;mid=" + ModuleId.ToString()  %>'>
                                </asp:HyperLink>
                            </ItemTemplate>
                            <FooterTemplate></span></FooterTemplate>
                            </asp:Repeater>
                    </div>
                    <% } %>
                    <portal:mojoRating runat="server" ID="Rating2" Enabled='<%# EnableContentRating && displaySettings.UseBottomContentRating %>'
                        ContentGuid='<%# new Guid(Eval("BlogGuid").ToString()) %>' AllowFeedback='false' />
                    <div class="bsocial">
                   <portal:AddThisWidget ID="addThisWidget" runat="server" AccountId='<%# addThisAccountId %>' SkinID="BlogList" TitleOfUrlToShare='<%# DataBinder.Eval(Container.DataItem,"Heading") %>' 
                       UrlToShare='<%# FormatBlogTitleUrl(DataBinder.Eval(Container.DataItem,"ItemUrl").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ItemID"))) %>' 
                       Visible='<%# (!Config.HideAddThisButton) %>' EnableViewState="false" />
                        <portal:TweetThisLink id="tt1" runat="server" Visible='<%# ShowTweetThisLink %>' UrlToTweet='<%# FormatBlogTitleUrl(DataBinder.Eval(Container.DataItem,"ItemUrl").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ItemID"))) %>' TitleToTweet='<%# DataBinder.Eval(Container.DataItem,"Heading") %>' />
                    <portal:FacebookLikeButton ID="fbl1" runat="server" Visible='<%# UseFacebookLikeButton %>' 
                        UrlToLike='<%# FormatBlogTitleUrl(DataBinder.Eval(Container.DataItem,"ItemUrl").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ItemID"))) %>'
                            ColorScheme='<%# Config.FacebookLikeButtonTheme %>' ShowFaces='<%# Config.FacebookLikeButtonShowFaces %>'
                            WidthInPixels='<%# Config.FacebookLikeButtonWidth %>' HeightInPixels='<%# Config.FacebookLikeButtonHeight %>' />
                    <portal:PlusOneButton ID="btnPlusOne" runat="server" TargetUrl='<%# FormatBlogTitleUrl(DataBinder.Eval(Container.DataItem,"ItemUrl").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ItemID"))) %>' 
                        Visible='<%# ShowPlusOneButton %>' SkinID="BlogPostList"
                        /> 
                    </div>
                    <div id="blogCommentLink" runat="server" visible='<%# AllowComments %>' class="blogcommentlink">
                        <asp:HyperLink ID="Hyperlink2" runat="server" EnableViewState="false" Text='<%# FeedBackLabel + "(" + DataBinder.Eval(Container.DataItem,"CommentCount") + ")" %>'
                            Visible='<%# AllowComments && ShowCommentCounts %>' NavigateUrl='<%# FormatBlogUrl(DataBinder.Eval(Container.DataItem,"ItemUrl").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ItemID")))  %>'
                            CssClass="blogcommentlink"></asp:HyperLink>
                        <asp:HyperLink ID="Hyperlink1" runat="server" EnableViewState="false" Text='<%# FeedBackLabel %>'
                            Visible='<%# Config.AllowComments && !ShowCommentCounts %>' NavigateUrl='<%# FormatBlogUrl(DataBinder.Eval(Container.DataItem,"ItemUrl").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ItemID")))  %>'
                            CssClass="blogcommentlink"></asp:HyperLink>&#160;
                    </div>
                </asp:Panel>
            </blog:BlogPostListItemPanel>
        </ItemTemplate>
    </asp:Repeater>
    <div class="blogpager">
    <portal:mojoCutePager ID="pgr" runat="server" />
    </div>
</blog:BlogPostListWrapperPanel>
<blog:NavControl ID="navBottom" runat="server" />
<div class="blogcopyright">
    <asp:Label ID="lblCopyright" runat="server" />
</div>
<portal:DisqusWidget ID="disqus" runat="server" />


