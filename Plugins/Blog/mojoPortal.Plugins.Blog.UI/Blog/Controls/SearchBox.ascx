<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SearchBox.ascx.cs" Inherits="mojoPortal.Web.BlogUI.SearchBox" %>
<asp:Panel ID="pnlBlogSearch" runat="server" CssClass="settingrow searchbox blogsearch" DefaultButton="btnSearch">
	<asp:TextBox ID="txtSearch" runat="server" CssClass="widetextbox blogsearchbox" />
	<portal:mojoButton ID="btnSearch" runat="server" ValidationGroup="blogsearch" />
	<asp:RequiredFieldValidator ID="reqSearchText" runat="server" ControlToValidate="txtSearch" Display="Dynamic" ValidationGroup="blogsearch" />
</asp:Panel>
