<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="CommentDialog.aspx.cs" Inherits="mojoPortal.Web.BlogUI.CommentDialog" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
    <portal:CommentEditor id="commentEditor" runat="server" />
</asp:Content>
