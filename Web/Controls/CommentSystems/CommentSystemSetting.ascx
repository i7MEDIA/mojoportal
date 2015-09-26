<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentSystemSetting.ascx.cs" Inherits="mojoPortal.Web.UI.CommentSystemSetting" %>
<asp:DropDownList ID="dd" runat="server" >
    <asp:ListItem Value="internal" Text="<%$ Resources:Resource, CommentSystemInternal %>" />
    <asp:ListItem Value="intensedebate" Text="<%$ Resources:Resource, CommentSystemIntenseDebate %>" />
    <asp:ListItem Value="disqus" Text="<%$ Resources:Resource, CommentSystemDisqus %>" />
    <asp:ListItem Value="facebook" Text="<%$ Resources:Resource, CommentSystemFacebook %>" />
</asp:DropDownList>
