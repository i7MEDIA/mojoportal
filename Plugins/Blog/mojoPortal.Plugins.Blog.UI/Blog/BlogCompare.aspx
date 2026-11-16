<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="BlogCompare.aspx.cs" Inherits="mojoPortal.Web.BlogUI.BlogCompare" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
<div  style="height: 100%; width: 48%; border-right: solid thin black; float:<%= currentFloat %>; padding: 5px 5px 5px 5px; overflow:auto;">
<h1 class="dialogheading htmlcontent"><asp:Literal ID="litCurrentHeading" runat="server" /></h1>
<asp:Literal ID="litCurrentVersion" runat="server" />
</div>
<div style="height: 100%; width: 48%; float: <%= historyFloat %>; padding: 5px 5px 5px 5px; overflow:auto;">
<h1 class="dialogheading htmlcontent"><asp:Literal ID="litHistoryHead" runat="server" /></h1>
<asp:Literal ID="litHistoryVersion" runat="server" />
<div class="settingrow">
<asp:Button ID="btnRestore" runat="server" />
</div>
</div>
</asp:Content>
