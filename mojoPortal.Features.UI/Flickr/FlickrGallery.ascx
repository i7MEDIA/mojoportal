<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="FlickrGallery.ascx.cs" Inherits="mojoPortal.Flickr.UI.FlickrGalleryModule" %>

<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" cssclass="panelwrapper FlickrGallery">
<portal:ModuleTitleControl runat="server" id="TitleControl" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<div id="gallerynav" class="controls">
  <span class="fprev btn"><asp:Literal id="litPrevious" runat="server" /></span>
  <div class="galleryinfo"></div>
  <span class="fnext btn"><asp:Literal id="litNext" runat="server" /></span>
</div>
<div class="fthumbs"></div>
<asp:Panel id="pnlGalleryContainer" runat="server" CssClass="hidden"></asp:Panel>
</asp:Panel>
<asp:Panel ID="pnlNotConfigured" runat="server" Visible="false">
<asp:Label ID="lblNotConfigured" Runat="server" EnableViewState="false" CssClass="txterror"></asp:Label>
</asp:Panel>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
