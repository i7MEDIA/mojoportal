<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="LessUtility.aspx.cs" Inherits="mojoPortal.Web.AdminUI.LessUtilityPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
    <asp:hyperlink id="lnkAdminMenu" runat="server" navigateurl="~/Admin/AdminMenu.aspx" />&nbsp;&gt;
        <asp:hyperlink id="lnkAdvancedTools" runat="server" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:hyperlink id="lnkDesignerTools" runat="server" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:hyperlink id="lnkThisPage" runat="server" cssclass="selectedcrumb" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper ">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    <span class="instructions"><asp:Literal ID="litLessInstructions" runat="server" /> </span>
    <div>
    <asp:TextBox ID="txtInput" runat="server" TextMode="MultiLine" CssClass="texteditor" Rows="30" Columns="90"></asp:TextBox>
    </div>
    <div>
    <asp:TextBox ID="txtOutput" runat="server" TextMode="MultiLine" CssClass="texteditor" Rows="30" Columns="90"></asp:TextBox>
    </div>
    <div>
    <portal:mojoButton ID="btnConvert" runat="server" Text="Generate CSS" />
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

