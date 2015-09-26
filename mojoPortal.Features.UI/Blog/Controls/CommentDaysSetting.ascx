<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentDaysSetting.ascx.cs" Inherits="mojoPortal.Web.BlogUI.CommentDaysSetting" %>
<asp:DropDownList ID="ddCommentDays" runat="server" >
    <asp:ListItem Value="-1" Text="<%$ Resources:BlogResources, BlogCommentsNotAllowed %>" />
    <asp:ListItem Value="0" Text="<%$ Resources:BlogResources, BlogCommentsUnlimited %>" />
    <asp:ListItem Value="1" Text="1" />
    <asp:ListItem Value="7" Text="7" />
    <asp:ListItem Value="15" Text="15" />
    <asp:ListItem Value="30" Text="30" />
    <asp:ListItem Value="45" Text="45" />
    <asp:ListItem Value="60" Text="60" />
    <asp:ListItem Value="90" Text="90" Selected="True" />
    <asp:ListItem Value="120" Text="120" />
</asp:DropDownList>
