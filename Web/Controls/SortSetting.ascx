<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SortSetting.ascx.cs" Inherits="mojoPortal.Web.UI.SortSetting" %>
<asp:DropDownList ID="dd" runat="server" CssClass="forminput" EnableTheming="false">
    <asp:ListItem Value="ASC" Text="<%$ Resources:Resource, SortAscending %>" />
    <asp:ListItem Value="DESC" Text="<%$ Resources:Resource, SortDescending %>" />
</asp:DropDownList>
