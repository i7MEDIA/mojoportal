<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="FlickrSlideShowModule.ascx.cs" Inherits="mojoPortal.Features.UI.FlickrSlideShowModule" %>

<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" cssclass="panelwrapper FlickrSlideShow">
<portal:ModuleTitleControl  runat="server" id="TitleControl" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<portal:VertigoSlideshow ID="slideShow" runat="server" />
</portal:InnerBodyPanel>
<portal:InnerBodyPanel ID="pnlNotConfig" runat="server" CssClass="modulecontent" Visible="false">
<asp:Label ID="lblNotConfigured" Runat="server" EnableViewState="false" CssClass="txterror"></asp:Label>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
