<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Default.aspx.cs" Inherits="mojoPortal.Web.ELetterUI.DefaultPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">

<div class="breadcrumbs">
<span id="spnAdmin" runat="server"><asp:HyperLink ID="lnkLetterAdmin" runat="server" CssClass="unselectedcrumb" />&nbsp;&gt;</span>
<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
</div>

<portal:SubscriberPreferences id="newsLetterPrefs" runat="server"></portal:SubscriberPreferences>
<asp:Panel ID="pnlAnonymousSubscriber" runat="server">
<fieldset>
<legend><asp:Literal ID="litAnonymousHeader" runat="server"></asp:Literal></legend>
<div class="modulecontent">
<portal:Subscribe ID="anonymousSubscribe" runat="server" />
</div>
</fieldset>
</asp:Panel>

</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
