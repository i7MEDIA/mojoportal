<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="BingMapModule.ascx.cs" Inherits="mojoPortal.Web.MapUI.BingMapModule" %>

<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" cssclass="panelwrapper bingmap">
<portal:ModuleTitleControl runat="server" id="TitleControl" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<portal:BingMap id="bmap" runat="server" />
<asp:Panel id="divDirections" runat="server" visible="false" CssClass="settingrow directionsrow" DefaultButton="btnGetDirections">
<portal:mojoButton ID="btnGetDirections" runat="server" />
<asp:TextBox ID="txtFromLocation" runat="server" CssClass="widetextbox fromlocationtb" />
</asp:Panel>
<asp:Panel ID="pnlDirections" runat="server" CssClass="drivingdirections"></asp:Panel>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
