<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ThankYou.aspx.cs" Inherits="mojoPortal.Web.ELetterUI.ThankYouPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">

<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper ">
<div class="modulecontent">

<asp:Literal ID="litThankYou" runat="server" />
</div>
</asp:Panel> 
	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
