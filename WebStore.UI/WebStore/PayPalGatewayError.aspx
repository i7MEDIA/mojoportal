<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="PayPalGatewayError.aspx.cs" Inherits="WebStore.UI.PayPalGatewayErrorPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server"  />
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper ">
<div class="modulecontent">

<asp:Label ID="lblError" runat="server" CssClass="txterror" />
</div>
</asp:Panel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
