<%@ Page ValidateRequest="false" Language="c#" CodeBehind="EditPost.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.BlogUI.BlogEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper editpage blogedit">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server" SkinID="BlogEdit">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<asp:Panel ID="pnlBlog" runat="server" DefaultButton="btnUpdate">
						<div id="divtabs" class="mojo-tabs">
							<ul>
								<li class="selected">
									<a href="#tabContent">
										<asp:Literal ID="litContentTab" runat="server" />
									</a>
								</li>
								<li id="liExcerpt" runat="server">
									<asp:Literal ID="litExcerptTab" runat="server" />
								</li>
								<li>
									<a href="#tabFeaturedImage">
										<asp:Literal runat="server" ID="litFeaturedImageTab" />
									</a>
								</li>
								<li>
									<a href="#tabMeta">
										<asp:Literal ID="litMetaTab" runat="server" />
									</a>
								</li>
								<li>
									<a href="#tabMapSettings">
										<asp:Literal ID="litMapSettingsTab" runat="server" />
									</a>
								</li>
								<li id="liAttachment" runat="server">
									<asp:Literal ID="litAttachmentsTab" runat="server" />
								</li>
								<li id="liGoogleNewsSettigns" runat="server">
									<asp:Literal ID="litGoogleNewsSettingsTab" runat="server" />
								</li>
							</ul>

							<div id="tabContent">
								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="BlogEditTitleLabel"
										CssClass="settinglabel"
										ForControl="txtTitle"
										ID="lblTitle"
										ResourceFile="BlogResources" />
									<asp:TextBox ID="txtTitle" runat="server" CssClass="forminput verywidetextbox"></asp:TextBox>
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="SubTitle"
										CssClass="settinglabel"
										ForControl="txtSubTitle"
										ID="SiteLabel15"
										ResourceFile="BlogResources" />
									<asp:TextBox ID="txtSubTitle" runat="server" MaxLength="500" CssClass="forminput verywidetextbox"></asp:TextBox>
								</div>

								<div class="settingrow">
									<mpe:EditorControl ID="edContent" runat="server"></mpe:EditorControl>
								</div>

								<div id="divUrl" runat="server" class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="BlogEditItemUrlLabel"
										CssClass="settinglabel"
										ForControl="txtItemUrl"
										ID="SiteLabel5"
										ResourceFile="BlogResources" />
									<asp:TextBox ID="txtItemUrl" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
									<span id="spnUrlWarning" runat="server" style="font-weight: normal; display: none;" class="txterror"></span>
									<asp:HiddenField ID="hdnTitle" runat="server" />
								</div>

								<asp:Panel ID="pnlCategories" runat="server" CssClass="settingrow">
									<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
										<ContentTemplate>
											<div class="settingrow">
												<mp:SiteLabel runat="server"
													ConfigKey="BlogEditCategoryLabel"
													CssClass="settinglabel"
													ForControl="txtCategory"
													ID="lblCat"
													ResourceFile="BlogResources" />
												<asp:TextBox ID="txtCategory" runat="server" CssClass="widetextbox forminput"></asp:TextBox>
												<portal:mojoButton ID="btnAddCategory" runat="server" CssClass="forminput" />
											</div>
											<div class="settingrow blogeditcategories">
												<asp:CheckBoxList ID="chkCategories" runat="server" SkinID="Blog" RepeatDirection="Horizontal"></asp:CheckBoxList>
											</div>
										</ContentTemplate>
									</asp:UpdatePanel>
									<asp:HyperLink ID="lnkEditCategories" runat="server"></asp:HyperLink>
								</asp:Panel>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="BlogEditIncludeInFeedLabel"
										CssClass="settinglabel"
										ForControl="chkIncludeInFeed"
										ID="Sitelabel1"
										ResourceFile="BlogResources" />
									<asp:CheckBox ID="chkIncludeInFeed" runat="server" CssClass="forminput"></asp:CheckBox>
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="IncludeInSearchIndex"
										CssClass="settinglabel"
										ForControl="chkIncludeInSearchIndex"
										ID="Sitelabel25"
										ResourceFile="BlogResources" />
									<asp:CheckBox ID="chkIncludeInSearchIndex" runat="server" CssClass="forminput" Checked="true"></asp:CheckBox>
								</div>

								<div id="divExcludeFromRecentContent" runat="server" class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="ExcludeFromRecentContent"
										CssClass="settinglabel"
										ForControl="chkExcludeFromRecentContent"
										ID="Sitelabel14" />
									<asp:CheckBox ID="chkExcludeFromRecentContent" runat="server" CssClass="forminput"></asp:CheckBox>
									<portal:mojoHelpLink ID="MojoHelpLink8" runat="server" HelpKey="ExcludeFromRecentContent-help" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="IncludeInSiteMap"
										CssClass="settinglabel"
										ForControl="chkIncludeInSiteMap"
										ID="Sitelabel31"
										ResourceFile="BlogResources" />
									<asp:CheckBox ID="chkIncludeInSiteMap" runat="server" CssClass="forminput" Checked="true"></asp:CheckBox>
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="IsPublishedLabel"
										CssClass="settinglabel"
										ForControl="chkIsPublished"
										ID="Sitelabel13"
										ResourceFile="BlogResources" />
									<asp:CheckBox ID="chkIsPublished" runat="server" CssClass="forminput" Checked="true"></asp:CheckBox>
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="ShowPostAuthorSetting"
										CssClass="settinglabel"
										ForControl="chkShowAuthorName"
										ID="Sitelabel27"
										ResourceFile="BlogResources" />
									<asp:CheckBox ID="chkShowAuthorName" runat="server" CssClass="forminput" Checked="true"></asp:CheckBox>
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="ShowAuthorPhoto"
										CssClass="settinglabel"
										ForControl="chkShowAuthorAvatar"
										ID="Sitelabel28"
										ResourceFile="BlogResources" />
									<asp:CheckBox ID="chkShowAuthorAvatar" runat="server" CssClass="forminput" Checked="true"></asp:CheckBox>
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="ShowAuthorBio"
										CssClass="settinglabel"
										ForControl="chkShowAuthorBio"
										ID="Sitelabel29"
										ResourceFile="BlogResources" />
									<asp:CheckBox ID="chkShowAuthorBio" runat="server" CssClass="forminput" Checked="true"></asp:CheckBox>
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="BlogEditStartDateLabel"
										CssClass="settinglabel"
										ID="lblStartDate"
										ResourceFile="BlogResources" />
									<mp:DatePickerControl ID="dpBeginDate" runat="server" ShowTime="True" SkinID="blog" CssClass="forminput"></mp:DatePickerControl>
									<mp:SiteLabel ID="SiteLabel3" runat="server" ResourceFile="BlogResources" ConfigKey="BlogDraftInstructions" UseLabelTag="false" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="EndDate"
										CssClass="settinglabel"
										ID="SiteLabel16"
										ResourceFile="BlogResources" />
									<mp:DatePickerControl ID="dpEndDate" runat="server" ShowTime="True" SkinID="blog" CssClass="forminput"></mp:DatePickerControl>
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ID="Sitelabel2"
										ForControl="ddCommentAllowedForDays"
										ConfigKey="BlogEditAllowedCommentsForDaysPrefix"
										ResourceFile="BlogResources"
										CssClass="settinglabel" />
									<asp:DropDownList ID="ddCommentAllowedForDays" runat="server" CssClass="forminput">
										<asp:ListItem Value="-1" Text="<%$ Resources:BlogResources, BlogCommentsNotAllowed %>" />
										<asp:ListItem Value="0" Text="<%$ Resources:BlogResources, BlogCommentsUnlimited %>" />
										<asp:ListItem Value="1" Text="1" />
										<asp:ListItem Value="7" Text="7" />
										<asp:ListItem Value="15" Text="15" />
										<asp:ListItem Value="30" Text="30" />
										<asp:ListItem Value="45" Text="45" />
										<asp:ListItem Value="60" Text="60" />
										<asp:ListItem Value="90" Text="90" Selected="True" />
										<asp:ListItem Value="120" Text="120" />
									</asp:DropDownList>
									<asp:Literal ID="litDays" runat="server" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel ID="Sitelabel44"
										runat="server"
										ForControl="chkFeaturedPost"
										ConfigKey="IsFeaturedPost"
										ResourceFile="BlogResources"
										CssClass="settinglabel" />
									<asp:CheckBox ID="chkFeaturedPost" runat="server" CssClass="forminput" Checked="false"></asp:CheckBox>
								</div>

								<div class="bloghistory">
									<asp:UpdatePanel ID="updHx" UpdateMode="Conditional" runat="server">
										<Triggers>
											<asp:PostBackTrigger ControlID="grdHistory" />
										</Triggers>
										<ContentTemplate>
											<asp:Panel ID="pnlHistory" runat="server" Visible="false">
												<div class="settingrow">
													<mp:SiteLabel runat="server"
														ConfigKey="VersionHistory"
														CssClass="settinglabel"
														ID="SiteLabel10"
														ResourceFile="BlogResources" />
												</div>
												<div class="settingrow">
													<mp:mojoGridView ID="grdHistory" runat="server" CssClass="editgrid" AutoGenerateColumns="false" DataKeyNames="Guid">
														<Columns>
															<asp:TemplateField>
																<ItemTemplate>
																	<span class="blog-history__created-time"><%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("CreatedUtc"), timeOffset)%></span>
																	<span class="blog-history__username"><%# Eval("UserName") %></span>
																</ItemTemplate>
															</asp:TemplateField>

															<asp:TemplateField>
																<ItemTemplate>
																	<span class="blog-history__archived-time"><%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("HistoryUtc"), timeOffset)%></span>
																</ItemTemplate>
															</asp:TemplateField>

															<asp:TemplateField>
																<ItemTemplate>
																	<asp:HyperLink runat="server"
																		CssClass="cblink"
																		DialogCloseText='<%# Resources.BlogResources.DialogCloseLink %>'
																		ID="lnkcompare"
																		NavigateUrl='<%# SiteRoot + "/Blog/BlogCompare.aspx?pageid=" + pageId + "&mid=" + moduleId + "&ItemID=" + itemId + "&h=" + Eval("Guid") %>'
																		SkinID="TextButtonSmall"
																		Text='<%# Resources.BlogResources.CompareHistoryToCurrentLink %>'
																		ToolTip='<%# Resources.BlogResources.CompareHistoryToCurrentLink %>' />
																	<portal:mojoButton runat="server"
																		CommandName="RestoreToEditor" CommandArgument='<%# Eval("Guid") %>'
																		ID="btnRestoreToEditor"
																		SkinID="SaveButtonSmall"
																		Text='<%# Resources.BlogResources.RestoreToEditorButton %>' />
																	<portal:mojoButton runat="server"
																		CommandArgument='<%# Eval("Guid") %>'
																		CommandName="DeleteHistory"
																		ID="btnDelete"
																		SkinID="DeleteButtonSmall"
																		Text='<%# Resources.BlogResources.DeleteHistoryButton %>'
																		Visible='<%# isAdmin %>' />
																</ItemTemplate>
															</asp:TemplateField>
														</Columns>
													</mp:mojoGridView>
												</div>

												<portal:mojoCutePager ID="pgrHistory" runat="server" />

												<div id="divHistoryDelete" runat="server" class="settingrow">
													<portal:mojoButton ID="btnDeleteHistory" runat="server" SkinID="DeleteButton" />
												</div>
											</asp:Panel>
										</ContentTemplate>
									</asp:UpdatePanel>
								</div>
							</div>

							<div id="tabExcerpt" runat="server">
								<div class="settingrow">
									<mpe:EditorControl ID="edExcerpt" runat="server"></mpe:EditorControl>
								</div>

								<div class="settingrow">
									<portal:mojoLabel ID="lblErrorMessage" runat="server" CssClass="txterror" />
								</div>
							</div>

							<div id="tabFeaturedImage">
								<div class="settingrow blog-edit__featured-image">
									<mp:SiteLabel runat="server"
										ConfigKey="FeaturedImage"
										CssClass="settinglabel"
										ForControl="txtHeadlineImage"
										ID="SiteLabel41"
										ResourceFile="BlogResources" />
									<asp:TextBox ID="txtHeadlineImage" runat="server" MaxLength="255" CssClass="forminput verywidetextbox" />
									<portal:FileBrowserTextBoxExtender ID="fbHeadlineImage" runat="server" BrowserType="image" />
									<portal:mojoHelpLink ID="MojoHelpLink9" runat="server" HelpKey="Blog-HeadlineImage-help" />
								</div>

								<div class="settingrow">
									<asp:Image ID="imgPreview" runat="server" ImageUrl="~/Data/SiteImages/1x1.gif" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="IncludeImageInExcerpt"
										CssClass="settinglabel"
										ForControl="chkIncludeImageInExcerpt"
										ID="SiteLabel42"
										ResourceFile="BlogResources" />
									<asp:CheckBox ID="chkIncludeImageInExcerpt" runat="server" Checked="true" CssClass="forminput" />
									<portal:mojoHelpLink ID="MojoHelpLink20" runat="server" HelpKey="Blog-IncludeImageInExcerpt-help" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="IncludeImageInPost"
										CssClass="settinglabel"
										ForControl="chkIncludeImageInPost"
										ID="SiteLabel8"
										ResourceFile="BlogResources" />
									<asp:CheckBox ID="chkIncludeImageInPost" runat="server" Checked="true" CssClass="forminput" />
									<portal:mojoHelpLink ID="MojoHelpLink10" runat="server" HelpKey="Blog-IncludeImageInPost-help" />
								</div>
							</div>

							<div id="tabMeta">
								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="MetaDescriptionLabel"
										CssClass="settinglabel"
										ForControl="txtMetaDescription"
										ID="SiteLabel6"
										ResourceFile="BlogResources" />
									<asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="MetaKeywordsLabel"
										CssClass="settinglabel"
										ForControl="txtMetaKeywords"
										ID="SiteLabel7"
										ResourceFile="BlogResources" />
									<asp:TextBox ID="txtMetaKeywords" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="MetaAdditionalLabel"
										CssClass="settinglabel"
										ID="lblAdditionalMetaTags"
										ResourceFile="BlogResources" />
									<portal:mojoHelpLink ID="MojoHelpLink25" runat="server" HelpKey="pagesettingsadditionalmetahelp" />
								</div>

								<asp:Panel ID="pnlMetaData" runat="server" CssClass="settingrow">
									<asp:UpdatePanel ID="updMetaLinks" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<mp:mojoGridView ID="grdMetaLinks" runat="server" CssClass="editgrid" AutoGenerateColumns="false" DataKeyNames="Guid">
												<Columns>
													<asp:TemplateField>
														<ItemTemplate>
															<portal:mojoButton runat="server"
																CommandName="Edit"
																ID="btnEditMetaLink"
																SkinID="InfoButtonSmall"
																Text='<%# Resources.BlogResources.ContentMetaGridEditButton %>' />
															<asp:ImageButton runat="server"
																AlternateText='<%# Resources.BlogResources.ContentMetaGridMoveUpButton %>'
																CommandArgument='<%# Eval("Guid") %>'
																CommandName="MoveUp"
																ID="btnMoveUpMetaLink"
																ImageUrl='<%# Page.ResolveUrl("~/Data/SiteImages/up.png") %>'
																Visible='<%# (Convert.ToInt32(Eval("SortRank")) > 3) %>' />
															<asp:ImageButton runat="server"
																AlternateText='<%# Resources.BlogResources.ContentMetaGridMoveDownButton %>'
																CommandArgument='<%# Eval("Guid") %>'
																CommandName="MoveDown"
																ID="btnMoveDownMetaLink"
																ImageUrl='<%# Page.ResolveUrl("~/Data/SiteImages/down.png") %>' />
														</ItemTemplate>

														<EditItemTemplate></EditItemTemplate>
													</asp:TemplateField>

													<asp:TemplateField>
														<ItemTemplate><%# Eval("Rel") %></ItemTemplate>

														<EditItemTemplate>
															<div class="settingrow">
																<mp:SiteLabel runat="server"
																	ConfigKey="ContentMetaRelLabel"
																	CssClass="settinglabel"
																	ForControl="txtRel"
																	ID="lblNameMetaRel"
																	ResourceFile="BlogResources" />
																<asp:TextBox ID="txtRel" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Rel") %>' />
																<asp:RequiredFieldValidator runat="server"
																	ControlToValidate="txtRel"
																	ErrorMessage='<%# Resources.BlogResources.ContentMetaLinkRelRequired %>'
																	ID="reqMetaName"
																	ValidationGroup="metalink" />
															</div>

															<div class="settingrow">
																<mp:SiteLabel runat="server"
																	ConfigKey="ContentMetaHrefLabel"
																	CssClass="settinglabel"
																	ForControl="txtHref"
																	ID="lblMetaHref"
																	ResourceFile="BlogResources" />
																<asp:TextBox ID="txtHref" CssClass="verywidetextbox forminput" runat="server" Text='<%# Eval("Href") %>' />
																<asp:RequiredFieldValidator runat="server"
																	ControlToValidate="txtHref"
																	ErrorMessage='<%# Resources.BlogResources.ContentMetaLinkHrefRequired %>'
																	ID="RequiredFieldValidator1"
																	ValidationGroup="metalink" />
															</div>

															<div class="settingrow">
																<mp:SiteLabel runat="server"
																	ConfigKey="ContentMetaHrefLangLabel"
																	CssClass="settinglabel"
																	ForControl="txtScheme"
																	ID="lblScheme"
																	ResourceFile="BlogResources" />
																<asp:TextBox ID="txtHrefLang" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("HrefLang") %>' />
															</div>

															<div class="settingrow">
																<portal:mojoButton runat="server"
																	CausesValidation="true"
																	CommandName="Update"
																	ID="btnUpdateMetaLink"
																	SkinID="SaveButtonSmall"
																	Text='<%# Resources.BlogResources.ContentMetaGridUpdateButton %>'
																	ValidationGroup="metalink" />
																<portal:mojoButton runat="server"
																	CausesValidation="false"
																	CommandName="Delete"
																	ID="btnDeleteMetaLink"
																	SkinID="DeleteButtonSmall"
																	Text='<%# Resources.BlogResources.ContentMetaGridDeleteButton %>' />
																<portal:mojoButton runat="server"
																	CausesValidation="false"
																	CommandName="Cancel"
																	ID="btnCancelMetaLink"
																	SkinID="TextButtonSmall"
																	Text='<%# Resources.BlogResources.ContentMetaGridCancelButton %>' />
															</div>
														</EditItemTemplate>
													</asp:TemplateField>

													<asp:TemplateField>
														<ItemTemplate><%# Eval("Href") %></ItemTemplate>

														<EditItemTemplate></EditItemTemplate>
													</asp:TemplateField>
												</Columns>
											</mp:mojoGridView>

											<div class="settingrow">
												<table>
													<tr>
														<td>
															<portal:mojoButton ID="btnAddMetaLink" runat="server" SkinID="AddButton" /></td>
														<td>
															<asp:UpdateProgress ID="prgMetaLinks" runat="server" AssociatedUpdatePanelID="updMetaLinks">
																<ProgressTemplate>
																	<img src='<%= Page.ResolveUrl("~/Data/SiteImages/indicator1.gif") %>' alt="" />
																</ProgressTemplate>
															</asp:UpdateProgress>
														</td>
													</tr>
												</table>
											</div>
										</ContentTemplate>
									</asp:UpdatePanel>

									<div class="settingrow">
										<asp:UpdatePanel ID="upMeta" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<div class="table-responsive">
													<mp:mojoGridView ID="grdContentMeta" runat="server" CssClass="editgrid" AutoGenerateColumns="false" DataKeyNames="Guid">
														<Columns>
															<asp:TemplateField>
																<ItemTemplate>
																	<portal:mojoButton runat="server"
																		CommandName="Edit"
																		ID="btnEditMeta"
																		SkinID="InfoButtonSmall"
																		Text='<%# Resources.BlogResources.ContentMetaGridEditButton %>' />
																	<asp:ImageButton runat="server"
																		AlternateText='<%# Resources.BlogResources.ContentMetaGridMoveUpButton %>'
																		CommandArgument='<%# Eval("Guid") %>'
																		CommandName="MoveUp"
																		ID="btnMoveUpMeta"
																		ImageUrl='<%# Page.ResolveUrl("~/Data/SiteImages/up.png") %>'
																		Visible='<%# (Convert.ToInt32(Eval("SortRank")) > 3) %>' />
																	<asp:ImageButton runat="server"
																		AlternateText='<%# Resources.BlogResources.ContentMetaGridMoveDownButton %>'
																		CommandArgument='<%# Eval("Guid") %>'
																		CommandName="MoveDown"
																		ID="btnMoveDownMeta"
																		ImageUrl='<%# Page.ResolveUrl("~/Data/SiteImages/down.png") %>' />
																</ItemTemplate>

																<EditItemTemplate>
																	<div class="settingrow">
																		<portal:mojoButton runat="server"
																			CausesValidation="true"
																			CommandName="Update"
																			ID="btnUpdateMeta"
																			SkinID="SaveButtonSmall"
																			Text='<%# Resources.BlogResources.ContentMetaGridUpdateButton %>'
																			ValidationGroup="meta" />
																		<portal:mojoButton runat="server"
																			CausesValidation="false"
																			CommandName="Delete"
																			ID="btnDeleteMeta"
																			SkinID="DeleteButtonSmall"
																			Text='<%# Resources.BlogResources.ContentMetaGridDeleteButton %>' />
																		<portal:mojoButton runat="server"
																			CausesValidation="false"
																			CommandName="Cancel"
																			ID="btnCancelMeta"
																			SkinID="TextButtonSmall"
																			Text='<%# Resources.BlogResources.ContentMetaGridCancelButton %>' />
																	</div>
																</EditItemTemplate>
															</asp:TemplateField>

															<asp:TemplateField>
																<ItemTemplate><%# Eval("NameProperty") %></ItemTemplate>

																<EditItemTemplate>
																	<div class="settingrow">

																		<asp:TextBox runat="server"
																			ID="txtNameProperty"
																			CssClass="widetextbox forminput"
																			Text='<%# Eval("NameProperty") %>' />

																		<asp:RequiredFieldValidator runat="server"
																			ControlToValidate="txtNameProperty"
																			ErrorMessage='<%# Resources.BlogResources.MetaNamePropertyRequired %>'
																			ID="valNameProperty"
																			ValidationGroup="meta" />
																	</div>
																</EditItemTemplate>
															</asp:TemplateField>

															<asp:TemplateField>
																<ItemTemplate><%# Eval("Name") %></ItemTemplate>

																<EditItemTemplate>
																	<asp:TextBox runat="server"
																		ID="txtName"
																		CssClass="widetextbox forminput"
																		Text='<%# Eval("Name") %>' />

																	<asp:RequiredFieldValidator runat="server"
																		ControlToValidate="txtName"
																		ErrorMessage='<%# Resources.BlogResources.ContentMetaNameRequired %>'
																		ID="reqMetaName"
																		ValidationGroup="meta" />
																</EditItemTemplate>
															</asp:TemplateField>

															<asp:TemplateField>
																<ItemTemplate><%# Eval("ContentProperty") %></ItemTemplate>

																<EditItemTemplate>
																	<asp:TextBox runat="server"
																		ID="txtMetaContentProperty"
																		CssClass="verywidetextbox forminput"
																		Text='<%# Eval("ContentProperty") %>' />

																	<asp:RequiredFieldValidator runat="server"
																		ControlToValidate="txtMetaContentProperty"
																		ErrorMessage='<%# Resources.BlogResources.MetaContentPropertyRequired %>'
																		ID="valMetaContentProperty"
																		ValidationGroup="meta" />
																</EditItemTemplate>
															</asp:TemplateField>

															<asp:TemplateField>
																<ItemTemplate><%# Eval("MetaContent") %></ItemTemplate>

																<EditItemTemplate>
																	<asp:TextBox runat="server"
																		ID="txtMetaContent"
																		CssClass="verywidetextbox forminput"
																		Text='<%# Eval("MetaContent") %>' />

																	<asp:RequiredFieldValidator runat="server"
																		ControlToValidate="txtName"
																		ErrorMessage='<%# Resources.BlogResources.ContentMetaContentRequired %>'
																		ID="RequiredFieldValidator1"
																		ValidationGroup="meta" />
																</EditItemTemplate>
															</asp:TemplateField>

															<asp:TemplateField>
																<ItemTemplate><%# Eval("Scheme") %></ItemTemplate>

																<EditItemTemplate>
																	<asp:TextBox runat="server"
																		ID="txtScheme"
																		CssClass="widetextbox forminput"
																		Text='<%# Eval("Scheme") %>' />
																</EditItemTemplate>
															</asp:TemplateField>

															<asp:TemplateField>
																<ItemTemplate><%# Eval("LangCode") %></ItemTemplate>

																<EditItemTemplate>
																	<asp:TextBox runat="server"
																		ID="txtLangCode"
																		CssClass="smalltextbox forminput"
																		Text='<%# Eval("LangCode") %>' />
																</EditItemTemplate>
															</asp:TemplateField>

															<asp:TemplateField>
																<ItemTemplate><%# Eval("Dir") %></ItemTemplate>

																<EditItemTemplate>
																	<asp:DropDownList ID="ddDirection" runat="server" CssClass="forminput">
																		<asp:ListItem Text="" Value=""></asp:ListItem>
																		<asp:ListItem Text="ltr" Value="ltr"></asp:ListItem>
																		<asp:ListItem Text="rtl" Value="rtl"></asp:ListItem>
																	</asp:DropDownList>
																</EditItemTemplate>
															</asp:TemplateField>
														</Columns>
													</mp:mojoGridView>
												</div>

												<div class="settingrow">
													<table>
														<tr>
															<td>
																<portal:mojoButton ID="btnAddMeta" runat="server" SkinID="AddButton" /></td>
															<td>
																<asp:UpdateProgress ID="prgMeta" runat="server" AssociatedUpdatePanelID="upMeta">
																	<ProgressTemplate>
																		<img src='<%= Page.ResolveUrl("~/Data/SiteImages/indicator1.gif") %>' alt="" />
																	</ProgressTemplate>
																</asp:UpdateProgress>
															</td>
														</tr>
													</table>
												</div>
											</ContentTemplate>
										</asp:UpdatePanel>
									</div>
								</asp:Panel>
							</div>

							<div id="tabMapSettings">
								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="BlogEditLocationLabel"
										CssClass="settinglabel"
										ForControl="txtLocation"
										ID="SiteLabel4"
										ResourceFile="BlogResources" />
									<asp:TextBox ID="txtLocation" runat="server" MaxLength="300" CssClass="forminput widetextbox"></asp:TextBox>
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="UseBingMap"
										CssClass="settinglabel"
										ForControl="chkUseBing"
										ID="SiteLabel17"
										ResourceFile="BlogResources" />
									<asp:CheckBox ID="chkUseBing" runat="server" Checked="false" CssClass="forminput" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="GoogleMapInitialMapTypeSetting"
										CssClass="settinglabel"
										ID="SiteLabel18"
										ResourceFile="BlogResources" />
									<portal:GMapTypeSetting ID="MapTypeControl" runat="server" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="GoogleMapInitialZoomSetting"
										CssClass="settinglabel"
										ID="SiteLabel19"
										ResourceFile="BlogResources" />
									<portal:GMapZoomLevelSetting ID="ZoomLevelControl" runat="server" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="GoogleMapHeightSetting"
										CssClass="settinglabel"
										ForControl="txtMapHeight"
										ID="SiteLabel21"
										ResourceFile="BlogResources" />
									<asp:TextBox ID="txtMapHeight" runat="server" Text="300" MaxLength="4" CssClass="forminput smalltextbox"></asp:TextBox>
									<asp:RegularExpressionValidator runat="server"
										ControlToValidate="txtMapHeight"
										Display="None"
										ID="regexMapHeight"
										ValidationExpression="^[1-9][0-9]{0,4}$"
										ValidationGroup="blog" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="GoogleMapWidthSetting"
										CssClass="settinglabel"
										ForControl="txtMapWidth"
										ID="SiteLabel20"
										ResourceFile="BlogResources" />
									<asp:TextBox ID="txtMapWidth" runat="server" Text="500" MaxLength="6" CssClass="forminput smalltextbox"></asp:TextBox>
									<asp:RegularExpressionValidator runat="server"
										ControlToValidate="txtMapWidth"
										Display="None"
										ID="regexMapWidth"
										ValidationExpression="^[1-9][0-9]{0,4}[px|%]?"
										ValidationGroup="blog" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="GoogleMapEnableMapTypeSetting"
										CssClass="settinglabel"
										ForControl="chkShowMapOptions"
										ID="SiteLabel22"
										ResourceFile="BlogResources" />
									<asp:CheckBox ID="chkShowMapOptions" runat="server" Checked="true" CssClass="forminput" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="GoogleMapEnableZoomSetting"
										CssClass="settinglabel"
										ForControl="chkShowMapZoom"
										ID="SiteLabel23"
										ResourceFile="BlogResources" />
									<asp:CheckBox ID="chkShowMapZoom" runat="server" Checked="true" CssClass="forminput" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="GoogleMapShowInfoWindowSetting"
										CssClass="settinglabel"
										ForControl="chkShowMapBalloon"
										ID="SiteLabel24"
										ResourceFile="BlogResources" />
									<asp:CheckBox ID="chkShowMapBalloon" runat="server" Checked="false" CssClass="forminput" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="GoogleMapEnableDirectionsSetting"
										CssClass="settinglabel"
										ForControl="chkShowMapDirections"
										ID="SiteLabel26"
										ResourceFile="BlogResources" />
									<asp:CheckBox ID="chkShowMapDirections" runat="server" Checked="true" CssClass="forminput" />
								</div>
							</div>

							<div id="tabAttachments" runat="server">
								<div class="settingrow">
									<mp:mojoGridView ID="grdAttachments" runat="server" CssClass="" AutoGenerateColumns="false" DataKeyNames="RowGuid">
										<Columns>
											<asp:TemplateField>
												<ItemTemplate>
													<%# Eval("FileName") %>
													<asp:HyperLink runat="server"
														EnableViewState="false"
														ID="lnkDownload"
														ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/arrow_in_down.png" %>'
														NavigateUrl='<%# upLoadPath + Eval("ServerFileName") %>'
														SkinID="TextButtonSmall"
														ToolTip='<%# Resources.BlogResources.Download %>' />
													<portal:mojoButton runat="server"
														CommandArgument='<%# Eval("RowGuid") %>'
														CommandName="delete"
														ID="btnDelete"
														SkinID="DeleteButtonSmall"
														Text='<%# Resources.BlogResources.DeleteImageAltText %>' />
												</ItemTemplate>
											</asp:TemplateField>
										</Columns>
									</mp:mojoGridView>
								</div>

								<asp:Panel ID="pnlUpload" runat="server" CssClass="settingrow" DefaultButton="btnUpload">
									<portal:jQueryFileUpload ID="uploader" runat="server" />
									<portal:mojoButton ID="btnUpload" runat="server" CausesValidation="false" EnableViewState="false" />
									<asp:HiddenField ID="hdnState" Value="images" runat="server" />
									<asp:Literal ID="litAttachmentWarning" runat="server" EnableViewState="false" />
								</asp:Panel>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="IncludeDownloadLinkForMediaAttachments"
										CssClass="settinglabel"
										ForControl="chkShowDownloadLink"
										ID="Sitelabel30"
										ResourceFile="BlogResources" />
									<asp:CheckBox ID="chkShowDownloadLink" runat="server" CssClass="forminput"></asp:CheckBox>
								</div>
							</div>

							<div id="divTabGoogleNews" runat="server">
								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="IncludeInGoogleNews"
										CssClass="settinglabel"
										ForControl="chkIncludeInNews"
										ID="SiteLabel34"
										ResourceFile="BlogResources" />
									<asp:CheckBox ID="chkIncludeInNews" runat="server" Checked="true" CssClass="forminput" />
									<portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="Blog-IncludeInGoogleNews-help" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="PublicationName"
										CssClass="settinglabel"
										ForControl="txtPublicationName"
										ID="SiteLabel33"
										ResourceFile="BlogResources" />
									<asp:TextBox ID="txtPublicationName" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
									<portal:mojoHelpLink ID="MojoHelpLink2" runat="server" HelpKey="Blog-PublicationName-help" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="PublicationLanguage"
										CssClass="settinglabel"
										ForControl="txtPubLanguage"
										ID="SiteLabel36"
										ResourceFile="BlogResources" />
									<asp:TextBox ID="txtPubLanguage" runat="server" MaxLength="7" CssClass="forminput smalltextbox"></asp:TextBox>
									<portal:mojoHelpLink ID="MojoHelpLink3" runat="server" HelpKey="Blog-PublicationLanguage-help" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="PublicationGenres"
										CssClass="settinglabel"
										ForControl="txtPubGenres"
										ID="SiteLabel37"
										ResourceFile="BlogResources" />
									<asp:TextBox ID="txtPubGenres" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
									<portal:mojoHelpLink ID="MojoHelpLink4" runat="server" HelpKey="Blog-PublicationGenres-help" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="PublicationKeyWords"
										CssClass="settinglabel"
										ForControl="txtPubKeyWords"
										ID="SiteLabel38"
										ResourceFile="BlogResources" />
									<asp:TextBox ID="txtPubKeyWords" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
									<portal:mojoHelpLink ID="MojoHelpLink5" runat="server" HelpKey="Blog-PublicationKeyWords-help" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="PublicationGeoLocation"
										CssClass="settinglabel"
										ForControl="txtPubGeoLocations"
										ID="SiteLabel39"
										ResourceFile="BlogResources" />
									<asp:TextBox ID="txtPubGeoLocations" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
									<portal:mojoHelpLink ID="MojoHelpLink6" runat="server" HelpKey="Blog-PublicationGeoLocation-help" />
								</div>

								<div class="settingrow">
									<mp:SiteLabel runat="server"
										ConfigKey="PublicationStockTickers"
										CssClass="settinglabel"
										ForControl="txtPubStockTickers"
										ID="SiteLabel40"
										ResourceFile="BlogResources" />
									<asp:TextBox ID="txtPubStockTickers" runat="server" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
									<portal:mojoHelpLink ID="MojoHelpLink7" runat="server" HelpKey="Blog-PublicationStockTickers-help" />
								</div>
							</div>
						</div>

						<div class="settingrow">
							<mp:SiteLabel runat="server"
								ConfigKey="spacer"
								CssClass="settinglabel"
								ID="SiteLabel35" />
							<div class="forminput">
								<portal:mojoButton ID="btnUpdate" runat="server" ValidationGroup="blog" SkinID="SaveButton" />
								<portal:mojoButton ID="btnSaveAndPreview" runat="server" ValidationGroup="blog" Visible="false" SkinID="InfoButton" />
								<portal:mojoButton ID="btnDelete" runat="server" Text="Delete this item" CausesValidation="false" SkinID="DeleteButton" />
								<asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" SkinID="TextButton" />
							</div>
							<portal:mojoLabel ID="lblError" runat="server" CssClass="txterror" />
							<asp:HiddenField ID="hdnHxToRestore" runat="server" />
							<asp:ImageButton ID="btnRestoreFromGreyBox" runat="server" />
						</div>

						<div class="blogeditor">
							<div class="settingrow">
								<asp:RequiredFieldValidator runat="server"
									ControlToValidate="txtTitle"
									CssClass="txterror"
									Display="None"
									ID="reqTitle"
									ValidationGroup="blog"></asp:RequiredFieldValidator>
								<asp:RequiredFieldValidator runat="server"
									ControlToValidate="dpBeginDate"
									CssClass="txterror"
									Display="None"
									ID="reqStartDate"
									ValidationGroup="blog"></asp:RequiredFieldValidator>
								<asp:RegularExpressionValidator runat="server"
									ControlToValidate="txtItemUrl"
									Display="None"
									ID="regexUrl"
									ValidationExpression="((~/){1}\S+)"
									ValidationGroup="blog" />
								<asp:ValidationSummary runat="server"
									CssClass="txterror"
									ID="vSummary"
									ValidationGroup="blog"></asp:ValidationSummary>
							</div>
						</div>

						<asp:HiddenField ID="hdnReturnUrl" runat="server" />
					</asp:Panel>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
	<portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
