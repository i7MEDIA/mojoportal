<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FilePathSetting.ascx.cs" Inherits="mojoPortal.Web.Controls.FilePathSetting" %>
<asp:TextBox ID="txtPath" runat="server" />
<portal:FileBrowserTextBoxExtender ID="browse" runat="server" BrowserType="image" CssClass="btn btn-link" TabIndex="10" />
