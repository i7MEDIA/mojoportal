<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SurveyModule.ascx.cs" Inherits="SurveyFeature.UI.SurveyModule" %>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server"  CssClass="panelwrapper survey">
<portal:ModuleTitleControl EditText="Edit" EditUrl="~/Survey/SurveyAdmin.aspx" runat="server" id="TitleControl" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    <asp:Literal runat="server" ID="litSurveyMessage"></asp:Literal>
    <asp:Label runat="server" ID="litOldResponses"></asp:Label>
    <asp:CheckBox runat="server" ID="chkUseOldResponses" Visible="false"/>
<div class="settingrow">
    <portal:mojoButton ID="btnStartSurvey" runat="server" CausesValidation="false" />
</div>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
