<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="AdminMenu.aspx.cs" Inherits="mojoPortal.Web.AdminUI.AdminMenuPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<asp:Literal ID="litMenu" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
