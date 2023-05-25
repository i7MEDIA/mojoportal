<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomMenu.ascx.cs" Inherits="mojoPortal.Web.UI.CustomMenu" %>
    
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper custommenu">
        <portal:ModuleTitleControl ID="Title1" runat="server" EnableViewState="false" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
            <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
                <asp:Literal ID="lit1" runat="server" />
            </portal:InnerBodyPanel>
            <portal:EmptyPanel ID="divFooter" runat="server" CssClass="modulefooter" SkinID="modulefooter">
            </portal:EmptyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared">
        </portal:EmptyPanel>
    </portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
