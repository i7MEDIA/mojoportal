<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="FeedEdit.aspx.cs" Inherits="mojoPortal.Web.FeedUI.FeedEditPage" %>

<%@ Register TagPrefix="mpf" TagName="FeedTypeSetting" Src="~/FeedManager/Controls/FeedTypeSetting.ascx" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper rssmodule">
			<portal:HeadingControl ID="heading" runat="server" />

			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnUpdate">
						<div class="settingrow">
							<mp:SiteLabel ID="lblTitleLabel" runat="server" ForControl="txtAuthor" CssClass="settinglabel" ConfigKey="AuthorLabel" ResourceFile="FeedResources" />
							<asp:TextBox id="txtAuthor" runat="server" maxlength="100" CssClass="forminput widetextbox" />
						</div>

						<div class="settingrow">
							<mp:SiteLabel ID="Sitelabel1" runat="server" ForControl="txtWebSite" CssClass="settinglabel" ConfigKey="WebSiteLabel" ResourceFile="FeedResources" />
							<asp:TextBox id="txtWebSite" runat="server" maxlength="255" CssClass="forminput widetextbox" />
						</div>

						<div class="settingrow">
							<mp:SiteLabel ID="Sitelabel2" runat="server" ForControl="txtRssUrl" CssClass="settinglabel" ConfigKey="FeedUrlLabel" ResourceFile="FeedResources" />
							<asp:TextBox id="txtRssUrl" runat="server" maxlength="1000" CssClass="forminput widetextbox" />
						</div>

						<div class="settingrow">
							<mp:SiteLabel ID="Sitelabel4" runat="server" ForControl="txtSortRank" CssClass="settinglabel" ConfigKey="SortRankLabel" ResourceFile="FeedResources" />
							<asp:TextBox id="txtSortRank" runat="server" maxlength="10" Text="500" CssClass="forminput smalltextbox" />
						</div>

						<div id="divImage" runat="server" visible="false" class="settingrow">
							<mp:SiteLabel ID="Sitelabel3" runat="server" ForControl="txtImageUrl" CssClass="settinglabel" ConfigKey="ImageUrlLabel" ResourceFile="FeedResources" />
							<asp:TextBox id="txtImageUrl" runat="server" maxlength="255" CssClass="forminput widetextbox" />
						</div>

						<div id="divPublish" runat="server" class="settingrow">
							<mp:SiteLabel ID="Sitelabel5" runat="server" ForControl="txtRssUrl" CssClass="settinglabel" ConfigKey="PublishByDefaultLabel" ResourceFile="FeedResources" />
							<asp:CheckBox id="chkPublishByDefault" runat="server" CssClass="forminput" />
						</div>

						<div class="settingrow">
							<asp:Label id="lblError" runat="server" />
						</div>

						<div class="settingrow">
							<mp:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />

							<div class="forminput">
								<portal:mojoButton runat="server" ID="btnUpdate" Text="Update" ValidationGroup="feeds" />
								<portal:mojoButton runat="server" ID="btnDelete" Text="" CausesValidation="false" />
								<asp:HyperLink runat="server" ID="lnkCancel" CssClass="cancellink" />
								<portal:mojoHelpLink runat="server" ID="MojoHelpLink1" HelpKey="rssfeededithelp" />
							</div>

							<asp:ValidationSummary runat="server" ID="vSummary" ValidationGroup="feeds" />
							<asp:RequiredFieldValidator runat="server" ID="reqTitle" ControlToValidate="txtAuthor" Display="None" ValidationGroup="feeds" />
							<asp:RequiredFieldValidator runat="server" ID="reqFeedUrl" ControlToValidate="txtRssUrl" Display="None" ValidationGroup="feeds" />
							<asp:RegularExpressionValidator runat="server" ID="regexWebSiteUrl" ControlToValidate="txtWebSite" Display="None" ValidationGroup="feeds" />
							<asp:RegularExpressionValidator runat="server" ID="regexFeedUrl" ControlToValidate="txtRssUrl" Display="None" ValidationGroup="feeds" />
						</div>

						<asp:HiddenField runat="server" ID="hdnReturnUrl" />
					</asp:Panel>

					<asp:Panel ID="divNav" runat="server" CssClass="rssnavright" SkinID="plain">
						<asp:Label ID="lblFeedListName" Font-Bold="True" runat="server"></asp:Label>
						<br />
						<asp:Hyperlink id="lnkNewFeed" runat="server" />

						<portal:mojoDataList ID="dlstFeedList" runat="server" EnableViewState="false">
							<itemtemplate>
								<asp:HyperLink runat="server"
									ID="editLink"
									Text="<%# Resources.FeedResources.EditImageAltText%>"
									ToolTip="<%# Resources.FeedResources.EditImageAltText%>"
									ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>'
									NavigateUrl='<%#
										this.SiteRoot +
										"/FeedManager/FeedEdit.aspx?pageid=" +
										PageId.ToString() +
										"&amp;ItemID=" +
										DataBinder.Eval(Container.DataItem,"ItemID") +
										"&amp;mid=" +
										ModuleId.ToString()
									%>'
								/>

								<asp:HyperLink runat="server"
									ID="Hyperlink2"
									NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.Url")%>'>
									<%# DataBinder.Eval(Container, "DataItem.Author")%>
								</asp:HyperLink>
					
								<asp:HyperLink runat="server"
									ID="Hyperlink3"
									ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + RssImageFile %>'
									NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.RssUrl")%>'
								/>
							</itemtemplate>
						</portal:mojoDataList>
					</asp:Panel>

					<portal:mojoButton ID="btnClearCache" runat="server" CausesValidation="False" />
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
			<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
