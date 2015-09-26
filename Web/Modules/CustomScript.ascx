<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomScript.ascx.cs" Inherits="mojoPortal.Web.UI.CustomScriptModule" %>
<portal:ModuleTitleControl id="TitleTop" runat="server" EnableViewState="false" Visible="false" />
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper customscriptmodule">
        <portal:ModuleTitleControl id="Title1" runat="server" EnableViewState="false" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
            <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
                <asp:Literal ID="litScriptUrl" runat="server" EnableViewState="false" />
                <asp:Literal ID="litScript" runat="server" EnableViewState="false" />
            </portal:InnerBodyPanel>
        <portal:EmptyPanel id="divFooter" runat="server" CssClass="modulefooter" SkinID="modulefooter"></portal:EmptyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />
</portal:OuterWrapperPanel>
