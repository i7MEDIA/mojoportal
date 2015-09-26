<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="CommentsDialog.aspx.cs" Inherits="mojoPortal.Features.UI.Comments.CommentsDialog" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
    <portal:CommentEditor id="commentEditor" runat="server" />
</asp:Content>
