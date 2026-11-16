<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PublicationAccessSetting.ascx.cs" Inherits="mojoPortal.BlogUI.PublicationAccessSetting" %>
<asp:DropDownList ID="dd" runat="server"  CssClass="forminput">
    <asp:ListItem Value="" Text="<%$ Resources:BlogResources, OpenAccess %>" />
    <asp:ListItem Value="Subscription" Text="<%$ Resources:BlogResources, PaidSubscription %>" />
    <asp:ListItem Value="Registration" Text="<%$ Resources:BlogResources, FreeSiteRegistration %>" />
</asp:DropDownList>
