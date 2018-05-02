<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="CssEditor.aspx.cs" Inherits="mojoPortal.Web.AdminUI.CssEditorPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server"> 
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
            <asp:hyperlink id="lnkAdminMenu" runat="server" navigateurl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
            <asp:hyperlink id="lnkDesignerTools" runat="server" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator3" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
            <asp:hyperlink id="lnkSkinList" runat="server" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator4" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
            <asp:hyperlink id="lnkSkin" runat="server" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator5" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
            <asp:hyperlink id="lnkThisPage" runat="server" cssclass="selectedcrumb" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper ">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
 <div class="settingrow">
        <portal:CodeEditor ID="edCss" runat="server" Syntax="css" CssClass="csseditor" StartHighlighted="false"  MinWidth="700" AllowToggle="true" Width="100%" />
        <asp:TextBox id="txtCss" runat="server" Rows="20" Columns="140" CssClass="csseditor" TextMode="MultiLine" Visible="false" />
    </div>
    <div class="settingrow">
    <portal:mojoButton id="btnSave" runat="server" />
    </div>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" 
    runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" 
    runat="server" >
</asp:Content>

