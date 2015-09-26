<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="BlogViewControl.ascx.cs"
    Inherits="mojoPortal.Web.BlogUI.BlogViewControl" %>
<%@ Register TagPrefix="blog" TagName="NavControl" Src="~/Blog/Controls/BlogNav.ascx" %>
<%@ Register Namespace="mojoPortal.Web.BlogUI" Assembly="mojoPortal.Features.UI" TagPrefix="blog" %>
<%@ Register TagPrefix="blog" TagName="RelatedPostsList" Src="~/Blog/Controls/RelatedPosts.ascx" %>
<%@ Register TagPrefix="blog" TagName="SearchBox" Src="~/Blog/Controls/SearchBox.ascx" %>
<portal:ContentExpiredLabel ID="expired" runat="server" EnableViewState="false" Visible="false" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper blogwrapper blogview">
    <blog:BlogDisplaySettings ID="displaySettings" runat="server" />
    <blog:SearchBox id="searchBoxTop" runat="server" />
    <portal:HeadingControl ID="heading" runat="server" />
    <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
            <blog:NavControl ID="navTop" runat="server"  />
            <asp:Panel ID="divblog" runat="server" CssClass="blogcenter-rightnav" SkinID="plain"
                DefaultButton="btnPostComment">
                <blog:BlogDatePanel ID="pnlDateTop" runat="server" CssClass="blogdate">
                    <span class="blogauthor">
                        <asp:Literal ID="litAuthor" runat="server" EnableViewState="false" Visible="false" /></span>
                    <span class="bdate">
                        <asp:Literal ID="litStartDate" runat="server" EnableViewState="false" /></span>
                    <asp:Repeater ID="rptTopCategories" runat="server" EnableViewState="false" Visible='false'>
                        <HeaderTemplate>
                            <span class="blogtags tagslabel">
                                <mp:SiteLabel ID="lblcatTop" runat="server" ConfigKey='<%# CategoriesResourceKey %>'
                                    ResourceFile="BlogResources" UseLabelTag="false" ShowWarningOnMissingKey="false" />
                            </span><span class="blogtags">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:HyperLink ID="Hyperlink5" runat="server" EnableViewState="false" Text='<%# Eval("Category").ToString() %>' data-category='<%# Eval("Category").ToString() %>'
                                NavigateUrl='<%# this.SiteRoot + "/Blog/ViewCategory.aspx?cat=" + DataBinder.Eval(Container.DataItem,"CategoryID") + "&amp;mid=" + ModuleId.ToString() + "&amp;pageid=" + PageId.ToString() %>'>
                            </asp:HyperLink>
                        </ItemTemplate>
                        <FooterTemplate>
                            </span></FooterTemplate>
                    </asp:Repeater>
                </blog:BlogDatePanel>
                <blog:BlogPagerPanel ID="divTopPager" runat="server" CssClass="blogpager">
                    <asp:HyperLink ID="lnkPreviousPostTop" runat="server" Visible="false" CssClass="postlink prevpost"
                        EnableViewState="false">
                    </asp:HyperLink>
                    <asp:HyperLink ID="lnkNextPostTop" runat="server" Visible="false" CssClass="postlink nextpost"
                        EnableViewState="false">
                    </asp:HyperLink>
                </blog:BlogPagerPanel>
                <asp:Literal ID="litSubtitle" runat="server" EnableViewState="false" />
                <asp:Panel ID="pnlDetails" runat="server">
                    <portal:mojoRating runat="server" ID="Rating" Enabled="false" />
                    <mp:OdiogoItem ID="odiogoPlayer" runat="server" EnableViewState="false" />
                    <div class="blogtext">
                        <asp:Literal ID="litDescription" runat="server" EnableViewState="false" />
                    </div>
                    <asp:Repeater ID="rptAttachments" runat="server" EnableViewState="false">
                        <ItemTemplate>
                            <portal:MediaElement ID="ml1" runat="server" IncludeDownloadLinkForMedia='<%# blog.ShowDownloadLink %>' EnableViewState="false" FileUrl='<%# attachmentBaseUrl + Eval("ServerFileName") %>' AddInitScript="true" />
                        </ItemTemplate>
                    </asp:Repeater>
                    <portal:LocationMap ID="gmap" runat="server" EnableViewState="false" Visible="false">
                    </portal:LocationMap>
                    <portal:BingMap ID="bmap" runat="server" Visible="false" EnableViewState="false" />
                    <asp:Panel ID="divDirections" runat="server" Visible="false" CssClass="settingrow directionsrow"
                        DefaultButton="btnGetBingDirections">
                        <portal:mojoButton ID="btnGetBingDirections" runat="server" />
                        <asp:TextBox ID="txtFromLocation" runat="server" CssClass="widetextbox fromlocationtb" />
                    </asp:Panel>
                    <asp:Panel ID="pnlBingDirections" runat="server" Visible="false" CssClass="drivingdirections">
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="pnlExcerpt" runat="server" Visible="false">
                    <div class="blogtext">
                        <asp:Literal ID="litExcerpt" runat="server" EnableViewState="false" />   
                    </div>
                     <portal:SignInOrRegisterPrompt ID="srPrompt" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlAuthorInfo" runat="server" EnableViewState="false" CssClass="authorinfo">
                <portal:Avatar id="userAvatar" runat="server" />
                <span id="spnAuthorBio" runat="server" visible="false" enableviewstate="false" class="authorbio"></span>
                </asp:Panel>
                <blog:BlogDatePanel ID="pnlBottomDate" runat="server" Visible="false" CssClass="clear blogdate">
                    <span class="blogauthor">
                        <asp:Literal ID="litAuthorBottom" runat="server" EnableViewState="false" Visible="false" /></span>
                    <span class="bdate">
                        <asp:Literal ID="litStartDateBottom" runat="server" EnableViewState="false" /></span>
                    <asp:Repeater ID="rptBottomCategories" runat="server" EnableViewState="false" Visible='false'>
                        <HeaderTemplate>
                            <span class="blogtags tagslabel">
                                <mp:SiteLabel ID="lblcatBottom" runat="server" ConfigKey='<%# CategoriesResourceKey %>'
                                    ResourceFile="BlogResources" UseLabelTag="false" ShowWarningOnMissingKey="false" />
                            </span><span class="blogtags">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:HyperLink ID="Hyperlink5" runat="server" EnableViewState="false" Text='<%# Eval("Category").ToString() %>'
                                NavigateUrl='<%# this.SiteRoot + "/Blog/ViewCategory.aspx?cat=" + DataBinder.Eval(Container.DataItem,"CategoryID") + "&amp;mid=" + ModuleId.ToString() + "&amp;pageid=" + PageId.ToString() %>'>
                            </asp:HyperLink>
                        </ItemTemplate>
                        <FooterTemplate>
                            </span></FooterTemplate>
                    </asp:Repeater>
                </blog:BlogDatePanel>
                <div class="blogcopyright">
                    <asp:Label ID="lblCopyright" runat="server" EnableViewState="false" />
                </div>
                <portal:mojoRating runat="server" ID="RatingBottom" Enabled="false" />
                <div id="bsocial" runat="server" class="bsocial">
                    <div id="divAddThis" runat="server" class="blogaddthis" enableviewstate="false">
                        <portal:AddThisWidget ID="addThisWidget" runat="server" EnableViewState="false" SkinID="BlogPostDetail" />
                    </div>
                    <portal:TweetThisLink ID="tweetThis1" runat="server" EnableViewState="false" />
                    <portal:FacebookLikeButton ID="fblike" runat="server" Visible="false" EnableViewState="false" />
                    <portal:PlusOneButton ID="btnPlusOne" runat="server" Visible="false" SkinID="BlogDetail"
                        EnableViewState="false" />
                </div>
                <blog:BlogPagerPanel ID="divBottomPager" runat="server" EnableViewState="false" CssClass="blogpager blogpagerbottom">
                    <asp:HyperLink ID="lnkPreviousPost" runat="server" CssClass="postlink prevpost" Visible="false"
                        EnableViewState="false">
                    </asp:HyperLink>
                    <asp:HyperLink ID="lnkNextPost" runat="server" Visible="false" CssClass="postlink nextpost"
                        EnableViewState="false">
                    </asp:HyperLink>
                </blog:BlogPagerPanel>
                
                <portal:CommentsWidget ID="InternalCommentSystem" runat="server" Visible="false" />
                <blog:BlogCommentPanel ID="pnlFeedback" runat="server" CssClass="bcommentpanel">
                <portal:HeadingControl ID="commentListHeading" runat="server" SkinID="BlogComments" HeadingTag="h3" />
                    <div class="blogcomments">
                        <asp:Repeater ID="dlComments" runat="server" EnableViewState="true" OnItemCommand="dlComments_ItemCommand">
                            <ItemTemplate>
                            
                                <<%# CommentItemHeaderElement %> class="blogtitle">
                                    <asp:ImageButton ID="btnDelete" runat="server" AlternateText="<%# Resources.BlogResources.DeleteImageAltText %>"
                                        ToolTip="<%# Resources.BlogResources.DeleteImageAltText %>" ImageUrl='<%# DeleteLinkImage %>'
                                        CommandName="DeleteComment" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"BlogCommentID")%>'
                                        Visible="<%# IsEditable%>" />
                                    <asp:Literal ID="litTitle" runat="server" EnableViewState="false" Text='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Title").ToString()) %>' />
                                </<%# CommentItemHeaderElement %>>
                                <div>
                                    <asp:Label ID="Label2" Visible="True" runat="server" EnableViewState="false" CssClass="blogdate"
                                        Text='<%# FormatCommentDate(Convert.ToDateTime(Eval("DateCreated"))) %>' />
                                    <asp:Label ID="Label3" runat="server" EnableViewState="false" Visible='<%# (bool) (DataBinder.Eval(Container.DataItem, "URL").ToString().Length == 0) %>'
                                        CssClass="blogcommentposter">
					        <%#  Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Name").ToString()) %>
                                    </asp:Label>
                                    <NeatHtml:UntrustedContent ID="UntrustedContent2" runat="server" EnableViewState="false"
                                        TrustedImageUrlPattern='<%# RegexRelativeImageUrlPatern %>' ClientScriptUrl="~/ClientScript/NeatHtml.js">
                                        <asp:HyperLink ID="Hyperlink2" runat="server" EnableViewState="false" Visible='<%# (bool) (DataBinder.Eval(Container.DataItem, "URL").ToString().Length != 0) %>'
                                            Text='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Name").ToString()) %>'
                                            NavigateUrl='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"URL").ToString())%>'
                                            CssClass="blogcommentposter">
                                        </asp:HyperLink>
                                    </NeatHtml:UntrustedContent>
                                </div>
                                <div class="blogcommenttext">
                                    <NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" EnableViewState="false"
                                        TrustedImageUrlPattern='<%# RegexRelativeImageUrlPatern %>' ClientScriptUrl="~/ClientScript/NeatHtml.js">
                                        <asp:Literal ID="litComment" runat="server" EnableViewState="false" Text='<%# DataBinder.Eval(Container.DataItem, "Comment").ToString() %>' />
                                    </NeatHtml:UntrustedContent>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <fieldset id="fldEnterComments" runat="server" visible="false">
                        <legend>
                            <mp:SiteLabel ID="lblFeedback" runat="server" ConfigKey="NewComment" ResourceFile="BlogResources"
                                EnableViewState="false"></mp:SiteLabel>
                        </legend>
                        <asp:Panel ID="pnlNewComment" runat="server">
                            <div class="settingrow">
                                <mp:SiteLabel ID="lblCommentTitle" runat="server" ForControl="txtCommentTitle" CssClass="settinglabel"
                                    ConfigKey="BlogCommentTitleLabel" ResourceFile="BlogResources" EnableViewState="false">
                                </mp:SiteLabel>
                                <asp:TextBox ID="txtCommentTitle" runat="server" MaxLength="100" EnableViewState="false"
                                    CssClass="forminput widetextbox">
                                </asp:TextBox>
                            </div>
                            <div class="settingrow">
                                <mp:SiteLabel ID="lblCommentUserName" runat="server" ForControl="txtName" CssClass="settinglabel"
                                    ConfigKey="BlogCommentUserNameLabel" ResourceFile="BlogResources" EnableViewState="false">
                                </mp:SiteLabel>
                                <asp:TextBox ID="txtName" runat="server" MaxLength="100" EnableViewState="false"
                                    CssClass="forminput widetextbox">
                                </asp:TextBox>
                            </div>
                            <div id="divCommentUrl" runat="server" class="settingrow">
                                <mp:SiteLabel ID="lblCommentURL" runat="server" ForControl="txtURL" CssClass="settinglabel"
                                    ConfigKey="BlogCommentUrlLabel" ResourceFile="BlogResources" EnableViewState="false">
                                </mp:SiteLabel>
                                <asp:TextBox ID="txtURL" runat="server" MaxLength="200" EnableViewState="true" CssClass="forminput widetextbox">
                                </asp:TextBox>
                                <asp:RegularExpressionValidator ID="regexUrl" runat="server" ControlToValidate="txtURL"
                                    Display="Dynamic" ValidationGroup="blogcomments" ValidationExpression="(((http(s?))\://){1}\S+)">
                                </asp:RegularExpressionValidator>
                            </div>
                            <div class="settingrow">
                                <mp:SiteLabel ID="lblRememberMe" runat="server" ForControl="chkRememberMe" CssClass="settinglabel"
                                    ConfigKey="BlogCommentRemeberMeLabel" ResourceFile="BlogResources" EnableViewState="false">
                                </mp:SiteLabel>
                                <asp:CheckBox ID="chkRememberMe" runat="server" EnableViewState="false" CssClass="forminput">
                                </asp:CheckBox>
                            </div>
                            <div class="settingrow">
                                <mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="BlogCommentCommentLabel"
                                    ResourceFile="BlogResources" EnableViewState="false"></mp:SiteLabel>
                            </div>
                            <div class="settingrow">
                                <mpe:EditorControl ID="edComment" runat="server">
                                </mpe:EditorControl>
                            </div>
                            <asp:Panel ID="pnlAntiSpam" runat="server" Visible="true">
                                <mp:CaptchaControl ID="captcha" runat="server"  />
                            </asp:Panel>
                            <div class="modulebuttonrow">
                                <portal:mojoButton ID="btnPostComment" runat="server" Text="Submit" ValidationGroup="blogcomments" />
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlCommentsClosed" runat="server" EnableViewState="false">
                            <asp:Literal ID="litCommentsClosed" runat="server" EnableViewState="false" />
                        </asp:Panel>
                        <asp:Panel ID="pnlCommentsRequireAuthentication" runat="server" Visible="false" EnableViewState="false">
                            <asp:Literal ID="litCommentsRequireAuthentication" runat="server" EnableViewState="false" />
                        </asp:Panel>
                    </fieldset>
                </blog:BlogCommentPanel>
                <blog:RelatedPostsList ID="relatedPosts" runat="server" />
            </asp:Panel>
            
            <blog:NavControl ID="navBottom" runat="server" />
            
        </portal:InnerBodyPanel>
    </portal:OuterBodyPanel>
    <portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared">
    </portal:EmptyPanel>
</portal:InnerWrapperPanel>
