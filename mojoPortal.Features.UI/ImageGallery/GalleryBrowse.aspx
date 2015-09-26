<%@ Page language="c#" Codebehind="GalleryBrowse.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.GalleryUI.GalleryBrowse" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper gallerymodule">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
	<div class="modulepager">
			<span id="spnTopPager" runat="server"></span>
	</div>
	<div class="divgalleryimage">
    <asp:Panel id="pnlGallery" runat="server" SkinID="plain" ></asp:Panel>
    <asp:Label ID="lblCaption" Runat="server"></asp:Label>
    <asp:Label ID="lblDescription" Runat="server"></asp:Label>
    </div>
    <div class="divgalleryimagemeta">
    <asp:Panel ID="pnlImageDetails" runat="server" SkinID="plain" >
	    <asp:xml ID="xmlMeta" runat="server" />
    </asp:Panel>
    </div>	
</portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

