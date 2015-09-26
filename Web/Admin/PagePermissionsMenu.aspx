<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="PagePermissionsMenu.aspx.cs" Inherits="mojoPortal.Web.AdminUI.PagePermissionsMenuPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper ">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<ul class="simplelist">
	<li>
		<asp:HyperLink ID="lnkPageViewRoles" runat="server" CssClass="lnkPageViewRoles" />
	</li>
    <li>
		<asp:HyperLink ID="lnkPageEditRoles" runat="server" CssClass="lnkPageEditRoles" />
	</li>
    <li>
		<asp:HyperLink ID="lnkPageDraftRoles" runat="server" CssClass="lnkPageDraftRoles" />
	</li>
	<li>
		<asp:HyperLink ID="lnkChildPageRoles" runat="server" CssClass="lnkChildPageRoles" />
	</li>
</ul>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

