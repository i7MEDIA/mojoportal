<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="OpenIDLoginControl.ascx.cs" Inherits="mojoPortal.Web.OpenIdLoginControl" %>

<portal:mojoOpenIdLogin ID="OpenIdLogin1" runat="server" CssClass="openid_login" />
<br />
<asp:Label ID="lblLoginFailed" runat="server" EnableViewState="False" Visible="False" />
<asp:Label ID="lblLoginCanceled" runat="server" EnableViewState="False" Visible="False" />
<asp:Literal ID="litResult" runat="server" />
<asp:Label ID="lblError" runat="server" CssClass="txterror" />
<div>
<asp:Literal ID="litNotRegisteredYetMessage" runat="server" />
</div>