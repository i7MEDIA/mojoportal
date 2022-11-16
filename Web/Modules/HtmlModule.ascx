<%@ Control language="c#" Inherits="mojoPortal.Web.ContentUI.HtmlModule" CodeBehind="HtmlModule.ascx.cs" AutoEventWireup="false" %>
<%@ Register Namespace="mojoPortal.Web.ContentUI" Assembly="mojoPortal.Web" TagPrefix="html" %>
<html:HtmlDisplaySettings ID="displaySettings" runat="server" />
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">

<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server"  CssClass="panelwrapper htmlmodule">
<portal:ModuleTitleControl id="Title1" runat="server" EditUrl="/Modules/HtmlEdit.aspx" EnableViewState="false" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<portal:SlidePanel id="divContent" runat="server" EnableViewState="false" EnableSlideShow="false" class="slidecontainer"></portal:SlidePanel>
<asp:HiddenField ID="hdnIsDirty" runat="server" />
<asp:Panel ID="pnlAuthorInfo" runat="server" EnableViewState="false" CssClass="authorinfo">
<portal:Avatar id="userAvatar" runat="server" />
<span id="spnAuthorBio" runat="server" visible="false" enableviewstate="false" class="authorbio"></span>
</asp:Panel>
<asp:Panel ID="pnlCreatedBy" runat="server" CssClass="authorinfo createdby" Visible="false">
<asp:Literal ID="litCreatedBy" runat="server" />
</asp:Panel>
<asp:Panel ID="pnlModifiedBy" runat="server" CssClass="authorinfo modifiedby" Visible="false">
<asp:Literal ID="litModifiedBy" runat="server" />
</asp:Panel>
<portal:FacebookLikeButton ID="fbLike" runat="server" Visible="false" />
</portal:InnerBodyPanel>
<portal:EmptyPanel id="divFooter" runat="server" CssClass="modulefooter" SkinID="modulefooter"></portal:EmptyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
