<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="LetterTemplateEdit.aspx.cs" Inherits="mojoPortal.Web.ELetterUI.LetterTemplateEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
  <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
    <asp:HyperLink ID="lnkLetterAdmin" runat="server" CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
    <asp:HyperLink ID="lnkTemplateList" runat="server" CssClass="selectedcrumb" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper editpage newsletter">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<div class="settingrow">
    <mp:SiteLabel id="lbl1" runat="server" CssClass="settinglabel" ConfigKey="LetterTemplateTitleLabel"></mp:SiteLabel>
    <asp:Textbox id="txtTitle" runat="server" columns="70"></asp:Textbox>
</div>
<div class="settingrow">
    <mp:SiteLabel id="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
    <portal:mojoButton  id="btnSave" runat="server"  Text="" CausesValidation="false" ValidationGroup="edit" />&nbsp;
    <portal:mojoButton  id="btnDelete" runat="server"  Text="" CausesValidation="False" ValidationGroup="edit" />&nbsp;
    <asp:ValidationSummary ID="editSummary" runat="server" ValidationGroup="edit" />
    <asp:RequiredFieldValidator ID="reqTitle" runat="server" Display="None" ControlToValidate="txtTitle" ValidationGroup="edit" />
</div>
<div class="settingrow">
<mp:SiteLabel id="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
<portal:mojoButton  id="btnSendPreview" runat="server"  Text="" CausesValidation="True" ValidationGroup="preview" />&nbsp;
<asp:TextBox ID="txtPreviewAddress" runat="server" ValidationGroup="preview"></asp:TextBox>
<asp:ValidationSummary ID="previewSummary" runat="server" ValidationGroup="preview" />
<asp:RequiredFieldValidator ID="reqPreviewAddress" runat="server" Display="None" ControlToValidate="txtPreviewAddress" ValidationGroup="preview" />
<portal:EmailValidator ID="regexPreviewAddress" runat="server" Display="None" ControlToValidate="txtPreviewAddress" ValidationGroup="preview" />
</div>
<div class="settingrow">
<mpe:EditorControl id="edContent" runat="server"></mpe:EditorControl>
</div>
</portal:InnerBodyPanel>	
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
