<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ModulePermissions.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ModulePermissionsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper ">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<div id="divRadioButtons" runat="server">
<div class="settingrow">
    <asp:RadioButton ID="rbAdminsOnly" runat="server"  GroupName="rdouseroles" CssClass="rbroles rbadminonly" />
</div>
<div class="settingrow">
    <asp:RadioButton ID="rbUseRoles" runat="server"  GroupName="rdouseroles" CssClass="rbroles" />
</div>
</div>
<p>
    <asp:CheckBoxList ID="chkAllowedRoles" runat="server" SkinID="Roles">
    </asp:CheckBoxList>
</p>
<div class="settingrow">
    <portal:mojoButton ID="btnSave" runat="server"  />
</div>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

