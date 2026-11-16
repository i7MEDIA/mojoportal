<%@ Page CodeBehind="UnsubscribeForum.aspx.cs" Language="c#" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.ForumUI.UnsubscribeForum" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	
<asp:Panel id="pnlUnsubscribe" runat="server" CssClass="unsubscribe">
	<p>
		<asp:Literal id="lblUnsubscribe" runat="server" />
	</p>
</asp:Panel>
		
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
