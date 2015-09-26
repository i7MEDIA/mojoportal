<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductListGroupingSetting.ascx.cs" Inherits="WebStore.UI.ProductListGroupingSetting" %>
<asp:DropDownList ID="ddGrouping" runat="server" >
    <asp:ListItem Value="GroupByProduct" Text="<%$ Resources:WebStoreResources, GroupByProduct %>" />
    <asp:ListItem Value="GroupByOffer" Text="<%$ Resources:WebStoreResources, GroupByOffer %>" />
</asp:DropDownList>
