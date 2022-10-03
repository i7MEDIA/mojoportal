<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="FeedManager.aspx.cs" Inherits="mojoPortal.Web.FeedUI.FeedManagerPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<div class="breadcrumbs">
			<asp:HyperLink ID="lnkBackToPage" runat="server" />&nbsp;&nbsp;
			<asp:HyperLink ID="lnkEditFeeds" runat="server" />
		</div>

		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper feedmanager">
			<portal:HeadingControl ID="heading" runat="server" />

			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<asp:UpdatePanel ID="updPnlRSSA" UpdateMode="Conditional" runat="server">
						<ContentTemplate>
							<asp:Panel ID="divFeedEntries" runat="server" CssClass="rsscenter-rightnav" SkinID="plain">
								<asp:Literal ID="lblFeedHeading" runat="server" Visible="false" />

								<asp:Repeater ID="rptEntries" runat="server" OnItemCommand="rptEntries_ItemCommand">
									<ItemTemplate>
										<asp:ImageButton runat="server"
											CommandName="Confirm"
											CommandArgument='<%#DataBinder.Eval(Container, "DataItem.EntryHash") + "_" + Convert.ToString(DataBinder.Eval(Container, "DataItem.Confirmed")) %>'
											ID="ConfirmBtn"
											ImageUrl='<%# ConfirmImage + DataBinder.Eval(Container, "DataItem.Confirmed") + ".png"%>'
											AlternateText='<%# Convert.ToBoolean(DataBinder.Eval(Container, "DataItem.Confirmed"))?Resources.FeedResources.EntryPublishTrueAlternateText:Resources.FeedResources.EntryPublishFalseAlternateText %>' />

										<div class='<%# "rssfeedentry" + DataBinder.Eval(Container, "DataItem.Confirmed") %>' id="divFeedEntry" runat="server">
											<div class="rsstitle">
												<%# "<" + FeedItemHeadingElement + ">" %>

												<asp:HyperLink ID="Hyperlink4" runat="server" SkinID="BlogTitle" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Link")%>'>
													<%# Server.HtmlEncode(DataBinder.Eval(Container, "DataItem.Title").ToString())%>
												</asp:HyperLink>

												<%# "</" + FeedItemHeadingElement + ">" %>
											</div>

											<div class="rssdate" id="divDate" runat="server" visible='<%# config.ShowDate %>'>
												<%# GetDateHeader((DateTime)DataBinder.Eval(Container, "DataItem.PubDate"))%>
											</div>

											<div runat="server" id="divFeedBody" class="rsstext">
												<NeatHtml:UntrustedContent runat="server"
													ID="UntrustedContent1"
													TrustedImageUrlPattern='<%# allowedImageUrlRegexPattern %>'
													ClientScriptUrl="~/ClientScript/NeatHtml.js">
													<%# DataBinder.Eval(Container, "DataItem.Description").ToString()%>
												</NeatHtml:UntrustedContent>
											</div>

											<div class="rssauthor" id="divAuthor" runat="server">
												<asp:HyperLink ID="Hyperlink1" runat="server" SkinID="BlogTitle" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.BlogUrl")%>'>
													<%# Server.HtmlEncode(DataBinder.Eval(Container, "DataItem.Author").ToString())%>
												</asp:HyperLink>
											</div>
										</div>
									</ItemTemplate>
								</asp:Repeater>
							</asp:Panel>

							<portal:mojoCutePager ID="pgrRptEntries" runat="server" />
						</ContentTemplate>
					</asp:UpdatePanel>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
			<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
