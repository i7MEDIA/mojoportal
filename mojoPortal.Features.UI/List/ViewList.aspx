<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ViewList.aspx.cs" Inherits="mojoPortal.Web.LinksUI.ViewListPage" %>
<%@ Register TagPrefix="links" TagName="LinkItems" Src="~/List/Controls/ItemList.ascx" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper linksmodule">
<portal:ModuleTitleControl ID="Title1" runat="server" EnableViewState="false" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<links:LinkItems id="theList" runat="server" />
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
 <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
