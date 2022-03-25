<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="Gallery.ascx.cs" Inherits="mojoPortal.Web.GalleryUI.GalleryControl" %>
<%@ Register Namespace="mojoPortal.Web.GalleryUI" Assembly="mojoPortal.Features.UI" TagPrefix="gallery" %>
<gallery:GalleryDisplaySettings ID="displaySettings" runat="server" />
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
	<mp:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
	<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper gallerymodule">
		<portal:ModuleTitleControl ID="Title1" runat="server" />
		<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
			<portal:InnerBodyPanel ID="pnlSl" runat="server" Visible="false" CssClass="modulecontent">
				<portal:VertigoSlideshow ID="slideShow" runat="server" />
			</portal:InnerBodyPanel>
			<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
				<asp:UpdatePanel ID="upGallery" UpdateMode="Conditional" runat="server">
					<ContentTemplate>
						<portal:mojoCutePager ID="pager" runat="server" />
						<div class="mojogallery">
							<asp:Repeater ID="rptGallery" runat="server">
								<HeaderTemplate>
									<ul class="simplehorizontalmenu gallerylist">
								</HeaderTemplate>
								<ItemTemplate>
									<li class="galleryitem">
										<%# GetThumnailImageLink(Eval("ItemID").ToString(), Eval("ThumbnailFile").ToString(), Eval("WebImageFile").ToString(),Eval("Caption").ToString()) %>
										<asp:ImageButton ID="btnThumb" runat="server" EnableViewState='<%# useViewState %>' Visible='<%# (!UseLightboxMode) %>' ImageUrl='<%# GetThumnailUrl(Eval("ThumbnailFile").ToString()) %>' CommandName="setimage" CommandArgument='<%# Eval("ItemID") %>' />
										<span class="txtmed galleryedit" id="spnEdit" runat="server" visible="<%# IsEditable %>">
											<asp:HyperLink ID="editLink" runat="server" EnableViewState="false"
												Text="<%# Resources.GalleryResources.GalleryEditImageLink %>"
												ToolTip="<%# Resources.GalleryResources.GalleryEditImageLink %>"
												ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>'
												NavigateUrl='<%# this.SiteRoot + "/ImageGallery/EditImage.aspx?ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&amp;mid=" + ModuleId.ToString() + "&amp;pageid=" + this.PageId.ToString() %>'
												Visible="<%# IsEditable %>" />
										</span></li>
								</ItemTemplate>
								<FooterTemplate></ul></FooterTemplate>
							</asp:Repeater>
						</div>
						<div class="divgalleryimage">
							<asp:Panel ID="pnlGallery" runat="server" SkinID="plain" EnableViewState="false"></asp:Panel>
							<asp:Label ID="lblCaption" runat="server" EnableViewState="false"></asp:Label><br />
							<asp:Label ID="lblDescription" runat="server" EnableViewState="false"></asp:Label><br />
							<br />
						</div>
						<div class="divgalleryimagemeta">
							<asp:Panel ID="pnlImageDetails" runat="server" SkinID="plain" EnableViewState="false">
								<asp:Xml ID="xmlMeta" runat="server" />
							</asp:Panel>
						</div>
					</ContentTemplate>
				</asp:UpdatePanel>
				<asp:Panel ID="pnlNivoWrapper" runat="server" Visible="false" EnableViewState="false">
					<asp:Panel ID="pnlNivoInner" runat="server" EnableViewState="false" CssClass="nivoSlider">
						<asp:Repeater ID="rptNivo" runat="server">
							<ItemTemplate>
								<asp:Literal ID="litImage" runat="server" EnableViewState="false" Text='<%# FormatNivoImage(Eval("WebImageFile").ToString(), Eval("ImageFile").ToString(), Eval("Caption").ToString(), Convert.ToInt32(Eval("ItemID"))) %>' />
							</ItemTemplate>
						</asp:Repeater>
					</asp:Panel>
					<div id="htmlcaption" class="nivo-html-caption"></div>
				</asp:Panel>
			</portal:InnerBodyPanel>
		</portal:OuterBodyPanel>
		<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
	</portal:InnerWrapperPanel>
	<mp:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
</portal:OuterWrapperPanel>
