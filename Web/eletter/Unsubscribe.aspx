<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Unsubscribe.aspx.cs" Inherits="mojoPortal.Web.ELetterUI.UnsubscribePage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">

<asp:Panel id="pnlUnsubscribe" runat="server" CssClass="unsubscribe">
	<p>
	    <asp:Button ID="btnUnsubscribeConfirm" runat="server" Visible="false" />
		<asp:Literal id="lblUnsubscribe" runat="server" />
	</p>
</asp:Panel>

</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
