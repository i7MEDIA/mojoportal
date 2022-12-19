<%@ Control Language="c#" Inherits="SuperFlexiUI.SuperFlexiModule" CodeBehind="Module.ascx.cs" AutoEventWireup="false" %>
<%@ Register Namespace="SuperFlexiUI" Assembly="SuperFlexiUI" TagPrefix="flexi" %>
<flexi:SuperFlexiDisplaySettings ID="displaySettings" runat="server" />

<asp:Literal ID="litHead" runat="server" />
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server" EnableViewState="false">
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" EnableViewState="false" CssClass="panelwrapper flexi">
        <asp:Literal ID="litModuleTitle" runat="server" EnableViewState="false" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server" EnableViewState="false">
            <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" EnableViewState="false" CssClass="modulecontent">
                <asp:PlaceHolder ID="aboveMarkupDefinitionScripts" runat="server" EnableViewState="false" />
                <asp:PlaceHolder ID="sflexi" runat="server" EnableViewState="false" />
                <asp:PlaceHolder ID="belowMarkupDefinitionScripts" runat="server" EnableViewState="false" />
            </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" EnableViewState="false" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
<asp:Literal ID="litFoot" runat="server" />
