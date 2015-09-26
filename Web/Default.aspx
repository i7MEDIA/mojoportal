<%@ Page language="c#" Codebehind="Default.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" StylesheetTheme="" AutoEventWireup="false" Inherits="mojoPortal.Web.UI.CmsPage" %>
<%-- @ OutputCache Duration="120" VaryByParam="*"  --%>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" ></asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server"></asp:Content>
