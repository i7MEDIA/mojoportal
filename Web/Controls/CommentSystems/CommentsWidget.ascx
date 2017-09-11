<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="CommentsWidget.ascx.cs" Inherits="mojoPortal.Web.UI.CommentsWidget" %>
<portal:CommentSystemDisplaySettings ID="displaySettings" runat="server" />
<portal:CommentsOuterPanel ID="pnlOuterPanel" runat="server" RenderId="false">
	<portal:HeadingControl ID="commentListHeading" runat="server" SkinID="Comments" HeadingTag="h3" RenderId="false" />
	<portal:CommentsInnerPanel ID="pnlInnerPanel" runat="server" RenderId="false">
		<asp:Repeater ID="rptComments" runat="server" EnableViewState="true">
			<ItemTemplate>
				<portal:CommentItemWrapper ID="pnlItem" runat="server" RenderId="false" CssClass='<%# ItemWrapperCssClass + " modstatus-" + Eval("ModerationStatus").ToString() %>' Visible='<%# UserCanModerate || (Convert.ToInt32(Eval("ModerationStatus")) == Comment.ModerationApproved)   %>'>
						<portal:CommentItemLeftPanel ID="pnlLeft" runat="server" RenderId="false" CssClass='<%# LeftPanelCssClass %>'>
							<portal:CommentItemInnerPanel ID="usernamepanel" runat="server" CssClass='<%# UsernameWrapperCssClass %>' RenderId="false">
								<%# GetProfileManageIcon(Convert.ToInt32(Eval("UserId"))) %>
								<%# GetProfileLinkOrLabel(Convert.ToInt32(Eval("UserId")), Eval("PostAuthor").ToString(), Eval("PostAuthorWebSiteUrl").ToString())%>
							</portal:CommentItemInnerPanel>
							<portal:CommentItemInnerPanel ID="avatarpanel" runat="server" CssClass='<%# AvatarWrapperCssClass %>' RenderId="false">
								<portal:Avatar ID="av1" runat="server"
									UseLink='<%# UseProfileLink() %>'
									MaxAllowedRating='<%# MaxAllowedGravatarRating %>'
									AvatarFile='<%# Eval("PostAuthorAvatar") %>'
									UserName='<%# Eval("PostAuthor") %>'
									UserId='<%# Convert.ToInt32(Eval("UserId")) %>'
									SiteId='<%# SiteId %>'
									SiteRoot='<%# SiteRoot %>'
									Email='<%# Eval("AuthorEmail") %>'
									UserNameTooltipFormat='<%# UserNameTooltipFormat %>'
									Disable='<%# disableAvatars %>'
									UseGravatar='<%# allowGravatars %>' />
							</portal:CommentItemInnerPanel>
							<portal:CommentItemInnerPanel ID="revenuepanel" runat="server" CssClass='<%# RevenueWrapperCssClass %>' RenderId="false" Visible='<%# showUserRevenue %>'>
									<mp:SiteLabel ID="SiteLabel1" runat="server" ConfigKey="UserSalesLabel" ResourceFile="ForumResources" UseLabelTag="false" />
									<%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("UserRevenue"))) %>
							</portal:CommentItemInnerPanel>
							<span class="comment-manage-edit">
								<asp:HyperLink CssClass="commentEdit ceditlink ModuleSettingsLink" Text="<%$ Resources:Resource, EditLink %>"
								ID="editLink" 
								NavigateUrl='<%# EditBaseUrl + "&c=" + Eval("Guid")  %>'
								Visible='<%# UserCanEdit(new Guid(Eval("UserGuid").ToString()), Eval("UserEmail").ToString(), Convert.ToInt32(Eval("ModerationStatus")), Convert.ToDateTime(Eval("CreatedUtc"))) %>' runat="server" />
							</span>
							<span class="comment-manage-approve">
							<portal:mojoButton ID="btnApprove" runat="server" Text='<%$ Resources:Resource, ContentManagerPublishContentLink %>' CommandName="ApproveComment" CommandArgument='<%# Eval("Guid")%>'
								Visible='<%# UserCanModerate && (Convert.ToInt32(Eval("ModerationStatus")) != Comment.ModerationApproved) %>' SkinID="SaveButton" />
							</span>
							<span class="comment-manage-delete">
							<portal:mojoButton ID="btnDelete" runat="server" Text='<%$ Resources:Resource, DeleteButton %>' CommandName="DeleteComment" CommandArgument='<%# Eval("Guid")%>'
								Visible='<%# UserCanModerate %>' SkinID="DeleteButtonSmall" />
							</span>
						</portal:CommentItemLeftPanel>
						<portal:CommentItemRightPanel ID="pnlRight" runat="server" CssClass='<%# RightPanelCssClass %>' RenderId="false">
							<div id='post<%# Eval("Guid") %>'>
								<portal:CommentItemInnerPanel ID="itemheaderpanel" runat="server" CssClass='<%# ItemHeaderCssClass %>' RenderId="false">
									<NeatHtml:UntrustedContent ID="UntrustedContent0" runat="server" TrustedImageUrlPattern='<%# AllowedImageUrlRegexPatern %>' ClientScriptUrl="~/ClientScript/NeatHtml.js">
											<portal:CommentItemTitlePanel ID="pnlTitle" runat="server" RenderId="false"
												CssClass='<%# ItemTitleCssClass %>'
												Visible='<%# UseCommentTitle %>'>
												<%# String.Format(CommentItemHeaderFormat, Server.HtmlEncode(Eval("Title").ToString()))%>
											</portal:CommentItemTitlePanel>
									</NeatHtml:UntrustedContent>
									<portal:CommentDateWrapper ID="dw1" runat="server" RenderId="false" CssClass='<%# DateWrapperCssClass %>'><%# FormatCommentDate(Convert.ToDateTime(Eval("CreatedUtc"))) %></portal:CommentDateWrapper>
								</portal:CommentItemInnerPanel>
								<NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" TrustedImageUrlPattern='<%# AllowedImageUrlRegexPatern %>' ClientScriptUrl="~/ClientScript/NeatHtml.js">
									<portal:CommentItemBodyPanel ID="pnlBody" runat="server" CssClass='<%# ItemBodyCssClass %>' RenderId="false"><%# Eval("UserComment").ToString()%></portal:CommentItemBodyPanel>
								</NeatHtml:UntrustedContent>
							</div>
						</portal:CommentItemRightPanel>
				</portal:CommentItemWrapper>
			</ItemTemplate>
		</asp:Repeater>
	</portal:CommentsInnerPanel>
	<portal:CommentEditor ID="commentEditor" runat="server" />
	<asp:Panel ID="pnlCommentsClosed" runat="server" EnableViewState="false" CssClass="commentsclosed">
		<asp:Literal ID="litCommentsClosed" runat="server" EnableViewState="false" />
	</asp:Panel>
	<asp:Panel ID="pnlCommentsRequireAuthentication" runat="server" Visible="false" EnableViewState="false" CssClass="commentsclosed">
		<portal:SignInOrRegisterPrompt ID="srPrompt" runat="server" />
	</asp:Panel>
</portal:CommentsOuterPanel>
<div id="divCommentService" runat="server" enableviewstate="false" class="blogcommentservice commentservice">
	<portal:IntenseDebateDiscussion ID="intenseDebate" runat="server" EnableViewState="false" />
	<portal:DisqusWidget ID="disqus" runat="server" RenderPoweredBy="false" EnableViewState="false" />
	<portal:FacebookCommentWidget ID="fbComments" runat="server" Visible="false" EnableViewState="false" SkinID="Blog" />
</div>
