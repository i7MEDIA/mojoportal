<%@ Control Language="c#" Inherits="mojoPortal.Web.LinksUI.LinksModule" CodeBehind="ListModule.ascx.cs" AutoEventWireup="false" %>
<%@ Register TagPrefix="links" TagName="LinkItems" Src="~/List/Controls/ItemList.ascx" %>
    
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper linksmodule">
        <portal:ModuleTitleControl ID="Title1" runat="server" EnableViewState="false" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
        <links:LinkItems id="theList" runat="server" />
        </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
    </portal:OuterWrapperPanel>

