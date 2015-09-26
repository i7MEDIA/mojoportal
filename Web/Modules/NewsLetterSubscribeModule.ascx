<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="NewsLetterSubscribeModule.ascx.cs" Inherits="mojoPortal.Web.ELetterUI.NewsLetterSubscribeModuleModule" %>

<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" cssclass="panelwrapper NewsLetterSubscribeModule">
<portal:ModuleTitleControl  runat="server" id="TitleControl" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<portal:Subscribe ID="subscribe1" runat="server" />
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />
</portal:OuterWrapperPanel>
