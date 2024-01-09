<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SiteSearchModule.ascx.cs" Inherits="mojoPortal.Web.SearchUI.SiteSearchModule" %>
<portal:SearchModuleDisplaySettings ID="displaySettings" runat="server" />
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
	<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper SiteSearch">
		<portal:ModuleTitleControl EditText="Edit" runat="server" ID="TitleControl" />
		<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
			<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
				<portal:SearchInput ID="Search" LinkOnly="false" RenderAsListItem="false" runat="server" UseHeading="false" />
				<asp:Panel ID="pnlSearch" runat="server" Visible="false" DefaultButton="btnSearch">
					<asp:UpdatePanel ID="updPnl" UpdateMode="Conditional" runat="server">
						<ContentTemplate>
							<portal:SearchPanel ID="pnlS" runat="server">
								<%--<mp:WatermarkTextBox ID="txtSearch" runat="server" CssClass="watermarktextbox" />--%>
								<asp:TextBox ID="txtSearch" runat="server" />
								<portal:mojoButton ID="btnSearch" runat="server" CausesValidation="false" SkinID="searchinput" />
							</portal:SearchPanel>
							<asp:Repeater ID="rptResults" runat="server" EnableViewState="False">
								<HeaderTemplate>
									<ol class="searchresultlist">
								</HeaderTemplate>
								<ItemTemplate>
									<li class="searchresult">
										<NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" TrustedImageUrlPattern='<%# mojoPortal.Web.Framework.SecurityHelper.RegexRelativeImageUrlPatern %>'>
											<<%# displaySettings.ItemHeadingElement %>>
												<asp:HyperLink ID="Hyperlink1" runat="server"
													NavigateUrl='<%# BuildUrl((mojoPortal.SearchIndex.IndexItem)Container.DataItem) %>'
													Text='<%# FormatLinkText(Eval("PageName").ToString(), Eval("ModuleTitle").ToString(), Eval("Title").ToString())  %>' />
											</<%# displaySettings.ItemHeadingElement %>>
											<div id="divExcerpt" runat="server" visible='<%# config.ShowExcerpt && displaySettings.ShowExcerpt %>' class="searchbutton">
												<%# Eval("Intro").ToString() %>
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
							<portal:mojoCutePager ID="pgr" runat="server" Visible="false" />
							<portal:mojoLabel ID="lblError" runat="server" CssClass="txterror info" />
						</ContentTemplate>
					</asp:UpdatePanel>
				</asp:Panel>
			</portal:InnerBodyPanel>
		</portal:OuterBodyPanel>
	</portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
