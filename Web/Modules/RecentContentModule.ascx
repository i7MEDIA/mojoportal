<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="RecentContentModule.ascx.cs" Inherits="mojoPortal.Web.ContentUI.RecentContentModule" %>

<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
	<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper recentcontent">
		<portal:ModuleTitleControl EditText="Edit" runat="server" ID="TitleControl" />
		<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
			<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
				<portal:RecentContentDisplaySettings ID="displaySettings" runat="server" />
				<portal:NoFollowHyperlink ID="lnkFeedTop" runat="server" SkinID="RecentContent" CssClass="feedlink rssfeed" Visible="false" EnableViewState="false" />
				<asp:Repeater ID="rptResults" runat="server" EnableViewState="False">
					<HeaderTemplate>
						<ol class="searchresultlist">
					</HeaderTemplate>
					<ItemTemplate>
						<li class="searchresult">
							<NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" TrustedImageUrlPattern='<%# mojoPortal.Web.Framework.SecurityHelper.RegexRelativeImageUrlPatern %>'>
								<<%# displaySettings.ItemHeadingElement %>>
									<asp:HyperLink ID="Hyperlink1" runat="server"
										NavigateUrl='<%# ((mojoPortal.SearchIndex.IndexItem)Container.DataItem).Url %>'
										Text='<%#((mojoPortal.SearchIndex.IndexItem)Container.DataItem).LinkText %>' />
								</<%# displaySettings.ItemHeadingElement %>>
								<div id="divExcerpt" runat="server" visible='<%# config.ShowExcerpt && displaySettings.ShowExcerpt %>' class="searchresultdesc">
									<%# Eval("ContentAbstract").ToString() %>
								</div>
								<%# FormatAuthor(Eval("Author").ToString()) %>
								<%# FormatCreatedDate((mojoPortal.SearchIndex.IndexItem)Container.DataItem) %>
								<%# FormatModifiedDate((mojoPortal.SearchIndex.IndexItem)Container.DataItem) %>
							</NeatHtml:UntrustedContent>
						</li>
					</ItemTemplate>
					<FooterTemplate>
						</ol>
					</FooterTemplate>
				</asp:Repeater>
				<portal:NoFollowHyperlink ID="lnkFeedBottom" runat="server" SkinID="RecentContent" CssClass="feedlink rssfeed" Visible="false" EnableViewState="false" />
				<portal:mojoLabel ID="lblError" runat="server" CssClass="txterror info" />
			</portal:InnerBodyPanel>
		</portal:OuterBodyPanel>
	</portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
