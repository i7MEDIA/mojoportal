<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="BlogViewControl.ascx.cs" Inherits="mojoPortal.Web.BlogUI.BlogViewControl" %>
<%@ Register TagPrefix="blog" TagName="NavControl" Src="~/Blog/Controls/BlogNav.ascx" %>
<%@ Register Namespace="mojoPortal.Web.BlogUI" Assembly="mojoPortal.Features.UI" TagPrefix="blog" %>
<%@ Register TagPrefix="blog" TagName="RelatedPostsList" Src="~/Blog/Controls/RelatedPosts.ascx" %>
<%@ Register TagPrefix="blog" TagName="SearchBox" Src="~/Blog/Controls/SearchBox.ascx" %>

<portal:ContentExpiredLabel ID="expired" runat="server" EnableViewState="false" Visible="false" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper blogwrapper blogview">
	<blog:BlogDisplaySettings ID="displaySettings" runat="server" />
	<blog:SearchBox ID="searchBoxTop" runat="server" />
	<portal:HeadingControl ID="heading" runat="server" />
	<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
		<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">

			<portal:BasePanel runat="server" ID="pnlLayoutRow">
				<asp:PlaceHolder runat="server" ID="phNavLeft" />

				<portal:BasePanel ID="divBlog" runat="server" DefaultButton="btnPostComment">
					<blog:BlogDatePanel ID="pnlDateTop" runat="server" CssClass="blogdate">
						<span class="blogauthor">
							<asp:Literal ID="litAuthor" runat="server" EnableViewState="false" Visible="false" />
						</span>
						<span class="bdate">
							<asp:Literal ID="litStartDate" runat="server" EnableViewState="false" />
						</span>
						<asp:Repeater ID="rptTopCategories" runat="server" EnableViewState="false" Visible='false'>
							<HeaderTemplate>
								<span class="blogtags tagslabel">
									<mp:SiteLabel runat="server"
										ID="lblcatTop"
										ConfigKey='<%# CategoriesResourceKey %>'
										ResourceFile="BlogResources"
										UseLabelTag="false"
										ShowWarningOnMissingKey="false" />
								</span>
								<span class="blogtags">
							</HeaderTemplate>

							<ItemTemplate>
								<asp:HyperLink runat="server"
									ID="Hyperlink5"
									EnableViewState="false"
									Text='<%# Eval("Category").ToString() %>'
									data-category='<%# Eval("Category").ToString() %>'
									NavigateUrl='<%# this.SiteRoot +
										"/Blog/ViewCategory.aspx?cat=" +
										DataBinder.Eval(Container.DataItem,"CategoryID") +
										"&amp;mid=" +
										ModuleId.ToString() +
										"&amp;pageid=" +
										PageId.ToString()
									%>'>
								</asp:HyperLink>
							</ItemTemplate>

							<FooterTemplate>
								</span>
							</FooterTemplate>
						</asp:Repeater>
					</blog:BlogDatePanel>

					<blog:BlogPagerPanel ID="divTopPager" runat="server" CssClass="blogpager">
						<asp:HyperLink ID="lnkPreviousPostTop" runat="server" Visible="false" CssClass="postlink prevpost" EnableViewState="false"></asp:HyperLink>
						<asp:HyperLink ID="lnkNextPostTop" runat="server" Visible="false" CssClass="postlink nextpost" EnableViewState="false"></asp:HyperLink>
					</blog:BlogPagerPanel>

					<asp:Literal ID="litSubtitle" runat="server" EnableViewState="false" />

					<portal:BasePanel runat="server" ID="pnlDetails">
						<mp:OdiogoItem ID="odiogoPlayer" runat="server" EnableViewState="false" />

						<asp:Literal runat="server" ID="featuredImagePostTop" Visible="false" />

						<portal:BasePanel runat="server" ID="pnlBlogText">
							<asp:Literal ID="litDescription" runat="server" EnableViewState="false" />
						</portal:BasePanel>

						<asp:Literal runat="server" ID="featuredImagePostBottom" Visible="false" />

						<asp:Repeater ID="rptAttachments" runat="server" EnableViewState="false">
							<ItemTemplate>
								<portal:MediaElement ID="ml1" runat="server" AllowDownload='<%# blog.ShowDownloadLink %>' EnableViewState="false" FileUrl='<%# attachmentBaseUrl + Eval("ServerFileName") %>'/>
							</ItemTemplate>
						</asp:Repeater>

						<portal:LocationMap ID="gmap" runat="server" EnableViewState="false" Visible="false"></portal:LocationMap>

						<portal:BingMap ID="bmap" runat="server" Visible="false" EnableViewState="false" />

						<asp:Panel ID="divDirections" runat="server" Visible="false" CssClass="settingrow directionsrow" DefaultButton="btnGetBingDirections">
							<portal:mojoButton ID="btnGetBingDirections" runat="server" />
							<asp:TextBox ID="txtFromLocation" runat="server" CssClass="widetextbox fromlocationtb" />
						</asp:Panel>

						<asp:Panel ID="pnlBingDirections" runat="server" Visible="false" CssClass="drivingdirections"></asp:Panel>
					</portal:BasePanel>

					<portal:BasePanel runat="server" ID="pnlExcerpt" Visible="false">
						<asp:Literal runat="server" ID="featuredImageExcerptTop" Visible="false" />

						<portal:BasePanel runat="server" ID="pnlBlogTextExpt">
							<asp:Literal ID="litExcerpt" runat="server" EnableViewState="false" />
						</portal:BasePanel>

						<asp:Literal runat="server" ID="featuredImageExcerptBottom" Visible="false" />

						<portal:SignInOrRegisterPrompt ID="srPrompt" runat="server" />
					</portal:BasePanel>

					<portal:BasePanel runat="server" ID="pnlAuthor" EnableViewState="false" RenderId="false">
						<portal:Avatar runat="server" ID="av1" />
						<asp:Label runat="server" ID="lblAuthorBio" />
					</portal:BasePanel>

					<blog:BlogDatePanel ID="pnlDateBottom" runat="server" CssClass="clear blogdate">
						<span class="blogauthor">
							<asp:Literal ID="litAuthorBottom" runat="server" EnableViewState="false" Visible="false" />
						</span>

						<span class="bdate">
							<asp:Literal ID="litStartDateBottom" runat="server" EnableViewState="false" />
						</span>

						<asp:Repeater ID="rptBottomCategories" runat="server" EnableViewState="false" Visible='false'>
							<HeaderTemplate>
								<span class="blogtags tagslabel">
									<mp:SiteLabel runat="server"
										ID="lblcatBottom"
										ConfigKey='<%# CategoriesResourceKey %>'
										ResourceFile="BlogResources"
										UseLabelTag="false"
										ShowWarningOnMissingKey="false" />
								</span>
								<span class="blogtags">
							</HeaderTemplate>

							<ItemTemplate>
								<asp:HyperLink runat="server"
									ID="Hyperlink5"
									EnableViewState="false"
									Text='<%# Eval("Category").ToString() %>'
									NavigateUrl='<%# this.SiteRoot +
										"/Blog/ViewCategory.aspx?cat=" +
										DataBinder.Eval(Container.DataItem,"CategoryID") +
										"&amp;mid=" +
										ModuleId.ToString() +
										"&amp;pageid=" +
										PageId.ToString()
									%>'>
								</asp:HyperLink>
							</ItemTemplate>

							<FooterTemplate>
								</span>
							</FooterTemplate>
						</asp:Repeater>
					</blog:BlogDatePanel>

					<portal:BasePanel runat="server" ID="pnlCopyright" RenderId="false">
						<asp:Literal runat="server" ID="litCopyright" />
					</portal:BasePanel>

					<div id="bsocial" runat="server" class="bsocial">
						<div id="divAddThis" runat="server" class="blogaddthis" enableviewstate="false">
							<portal:AddThisWidget ID="addThisWidget" runat="server" EnableViewState="false" SkinID="BlogPostDetail" />
						</div>

						<portal:TweetThisLink ID="tweetThis1" runat="server" EnableViewState="false" />
						<portal:FacebookLikeButton ID="fblike" runat="server" Visible="false" EnableViewState="false" />
						<portal:PlusOneButton ID="btnPlusOne" runat="server" Visible="false" SkinID="BlogDetail" EnableViewState="false" />
					</div>

					<blog:BlogPagerPanel ID="divBottomPager" runat="server" EnableViewState="false" CssClass="blogpager blogpagerbottom">
						<asp:HyperLink ID="lnkPreviousPost" runat="server" CssClass="postlink prevpost" Visible="false" EnableViewState="false"></asp:HyperLink>
						<asp:HyperLink ID="lnkNextPost" runat="server" Visible="false" CssClass="postlink nextpost" EnableViewState="false"></asp:HyperLink>
					</blog:BlogPagerPanel>

					<portal:CommentsWidget ID="InternalCommentSystem" runat="server" Visible="false" SkinID="Blog" />

					<blog:BlogCommentPanel ID="pnlFeedback" runat="server" CssClass="bcommentpanel">
						<portal:HeadingControl ID="commentListHeading" runat="server" SkinID="BlogComments" HeadingTag="h3" />

						<div class="blogcomments">
							<asp:Repeater ID="dlComments" runat="server" EnableViewState="true" OnItemCommand="dlComments_ItemCommand">
								<ItemTemplate>
									<<%# CommentItemHeaderElement %> class="blogtitle">
										<asp:ImageButton runat="server"
											ID="btnDelete"
											AlternateText="<%# Resources.BlogResources.DeleteImageAltText %>"
											ToolTip="<%# Resources.BlogResources.DeleteImageAltText %>"
											ImageUrl='<%# DeleteLinkImage %>'
											CommandName="DeleteComment"
											CommandArgument='<%# DataBinder.Eval(Container.DataItem,"BlogCommentID")%>'
											Visible="<%# IsEditable%>"
										/>
										<asp:Literal runat="server"
											ID="litTitle"
											EnableViewState="false"
											Text='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Title").ToString()) %>'
										/>
									</<%# CommentItemHeaderElement %>>

									<div>
										<asp:Label runat="server"
											Visible="True"
											ID="Label2"
											EnableViewState="false"
											CssClass="blogdate"
											Text='<%# FormatCommentDate(Convert.ToDateTime(Eval("DateCreated"))) %>' />
										<asp:Label runat="server"
											ID="Label3"
											EnableViewState="false"
											Visible='<%# (bool) (DataBinder.Eval(Container.DataItem, "URL").ToString().Length == 0) %>'
											CssClass="blogcommentposter">
											<%#  Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Name").ToString()) %>
										</asp:Label>
										<NeatHtml:UntrustedContent runat="server"
											ID="UntrustedContent2"
											EnableViewState="false"
											TrustedImageUrlPattern='<%# RegexRelativeImageUrlPatern %>'
											ClientScriptUrl="~/ClientScript/NeatHtml.js">
											<asp:HyperLink runat="server"
												ID="Hyperlink2"
												EnableViewState="false"
												Visible='<%# (bool) (DataBinder.Eval(Container.DataItem, "URL").ToString().Length != 0) %>'
												Text='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Name").ToString()) %>'
												NavigateUrl='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"URL").ToString())%>'
												CssClass="blogcommentposter"></asp:HyperLink>
										</NeatHtml:UntrustedContent>
									</div>

									<div class="blogcommenttext">
										<NeatHtml:UntrustedContent runat="server"
											ID="UntrustedContent1"
											EnableViewState="false"
											TrustedImageUrlPattern='<%# RegexRelativeImageUrlPatern %>'
											ClientScriptUrl="~/ClientScript/NeatHtml.js">
											<asp:Literal runat="server"
												ID="litComment"
												EnableViewState="false"
												Text='<%# DataBinder.Eval(Container.DataItem, "Comment").ToString() %>' />
										</NeatHtml:UntrustedContent>
									</div>
								</ItemTemplate>
							</asp:Repeater>
						</div>

						<fieldset id="fldEnterComments" runat="server" visible="false">
							<legend>
								<mp:SiteLabel runat="server"
									ID="lblFeedback"
									ConfigKey="NewComment"
									ResourceFile="BlogResources"
									EnableViewState="false" />
							</legend>

							<asp:Panel ID="pnlNewComment" runat="server">
								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ID="lblCommentTitle"
										ForControl="txtCommentTitle"
										CssClass="settinglabel"
										ConfigKey="BlogCommentTitleLabel"
										ResourceFile="BlogResources"
										EnableViewState="false" />
									<asp:TextBox ID="txtCommentTitle" runat="server" MaxLength="100" EnableViewState="false" CssClass="forminput widetextbox"></asp:TextBox>
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ID="lblCommentUserName"
										ForControl="txtName"
										CssClass="settinglabel"
										ConfigKey="BlogCommentUserNameLabel"
										ResourceFile="BlogResources"
										EnableViewState="false" />
									<asp:TextBox ID="txtName" runat="server" MaxLength="100" EnableViewState="false" CssClass="forminput widetextbox"></asp:TextBox>
								</div>

								<div id="divCommentUrl" runat="server" class="settingrow">
									<mp:SiteLabel runat="server"
										ID="lblCommentURL"
										ForControl="txtURL"
										CssClass="settinglabel"
										ConfigKey="BlogCommentUrlLabel"
										ResourceFile="BlogResources"
										EnableViewState="false" />
									<asp:TextBox ID="txtURL" runat="server" MaxLength="200" EnableViewState="true" CssClass="forminput widetextbox"> </asp:TextBox>
									<asp:RegularExpressionValidator runat="server"
										ID="regexUrl"
										ControlToValidate="txtURL"
										Display="Dynamic"
										ValidationGroup="blogcomments"
										ValidationExpression="(((http(s?))\://){1}\S+)"></asp:RegularExpressionValidator>
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ID="lblRememberMe"
										ForControl="chkRememberMe"
										CssClass="settinglabel"
										ConfigKey="BlogCommentRemeberMeLabel"
										ResourceFile="BlogResources"
										EnableViewState="false" />
									<asp:CheckBox ID="chkRememberMe" runat="server" EnableViewState="false" CssClass="forminput"></asp:CheckBox>
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ID="SiteLabel1"
										CssClass="settinglabel"
										ConfigKey="BlogCommentCommentLabel"
										ResourceFile="BlogResources"
										EnableViewState="false" />
								</div>

								<div class="settingrow">
									<mpe:EditorControl ID="edComment" runat="server"></mpe:EditorControl>
								</div>

								<asp:Panel ID="pnlAntiSpam" runat="server" Visible="true">
									<mp:CaptchaControl ID="captcha" runat="server" />
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
				</portal:BasePanel>

				<%--<blog:NavControl ID="navBottom" runat="server" />--%>
				<asp:PlaceHolder runat="server" ID="phNavRight" />
			</portal:BasePanel>

		</portal:InnerBodyPanel>
	</portal:OuterBodyPanel>
</portal:InnerWrapperPanel>
