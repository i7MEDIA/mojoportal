<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UrlSelector.ascx.cs" Inherits="SuperFlexiUI.UrlSelector" %>
<%@ Register Assembly="SuperFlexiUI" Namespace="SuperFlexiUI" TagPrefix="SuperFlexi" %>
<div class="url-selector">
    <asp:TextBox ID="txtUrl" runat="server" EnableTheming="false"/>    
    <SuperFlexi:FileBrowserTextBoxExtender ID="browse" runat="server" CssClass="btn btn-link" TabIndex="10" />
</div>