<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="OptInUsersDialog.aspx.cs" Inherits="mojoPortal.Web.UI.OptInUsersDialog" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server">
<asp:Literal id="litStyleLink" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">

<div  style="padding: 5px 5px 5px 5px;" class="parentpagedialog">
<portal:HeadingControl id="heading" runat="server" />
<div class="settingrow">

<asp:Panel ID="pnlProgress" runat="server"></asp:Panel>
<asp:Panel ID="pnlStatus" runat="server"></asp:Panel>

</div>
<asp:Panel ID="pnlButton" runat="server" CssClass="settingrow">
<portal:mojoButton ID="btnOptInUsers" runat="server" />
</asp:Panel>

</div>
</asp:Content>
