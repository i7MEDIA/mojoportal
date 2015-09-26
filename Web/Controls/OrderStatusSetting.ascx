<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderStatusSetting.ascx.cs" Inherits="mojoPortal.Web.UI.OrderStatusSetting" %>

<asp:DropDownList ID="ddOrderStatus" runat="server" >
    <asp:ListItem Value="00000000-0000-0000-0000-000000000000" Text="<%$ Resources:Resource, OrderStatusNone %>" />
    <asp:ListItem Value="0db28432-d9a9-423e-84f2-8a94db434643" Text="<%$ Resources:Resource, OrderStatusReceived %>" />
    <asp:ListItem Value="70443443-f665-42c9-b69f-48cbf011a14b" Text="<%$ Resources:Resource, OrderStatusFulfillable %>" />
    <asp:ListItem Value="67e92035-e8d0-4700-822b-a4002f2f1a15" Text="<%$ Resources:Resource, OrderStatusFulfilled %>" />
    <asp:ListItem Value="de3b9331-b98f-493f-be5e-926ffe5003bc" Text="<%$ Resources:Resource, OrderStatusCancelled %>" />
</asp:DropDownList>