<%@ Control Language="c#" Inherits="SuperFlexiUI.SuperFlexiModule" CodeBehind="Module.ascx.cs" AutoEventWireup="false" %>
<%@ Register TagPrefix="flexi" TagName="Widget" Src="~/SuperFlexi/Controls/Widget.ascx" %>
<%--<%@ Register TagPrefix="flexi" TagName="WidgetRazor" Src="~/SuperFlexi/Controls/WidgetRazor.ascx" %>--%>

<%@ Register Namespace="SuperFlexiUI" Assembly="SuperFlexiUI" TagPrefix="flexi" %>
<flexi:SuperFlexiDisplaySettings ID="displaySettings" runat="server" />
<asp:Literal ID="litHead" runat="server" />
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server" EnableViewState="false">
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" EnableViewState="false" CssClass="panelwrapper flexi">
        <asp:Literal ID="litModuleTitle" runat="server" EnableViewState="false" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server" EnableViewState="false">
            <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" EnableViewState="false" CssClass="modulecontent">
                <flexi:Widget id="theWidget" runat="server" EnableViewState="false" />
                <%--<flexi:WidgetRazor id="theWidgetRazor" runat="server" EnableViewState="false" />--%>
            </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" EnableViewState="false" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
<asp:Literal ID="litFoot" runat="server" />
