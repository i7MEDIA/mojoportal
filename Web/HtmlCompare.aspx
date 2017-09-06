<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="HtmlCompare.aspx.cs" Inherits="mojoPortal.Web.ContentUI.HtmlCompare" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
	<div class="hlinks">
		<asp:HyperLink ID="lnkToggleHighlight" runat="server" EnableViewState="false" />
	</div>
	<div style="
		border-right: solid thin black;
		-webkit-box-sizing: border-box;
		   -moz-box-sizing: border-box;
				box-sizing: border-box;
		float: <%= currentFloat %>;
		height: 100%;
		overflow: auto;
		padding: 5px;
		width: 50%;
	">
		<h1 class="dialogheading htmlcontent">
			<asp:Literal ID="litCurrentHeading" runat="server" /></h1>
		<asp:Literal ID="litCurrentVersion" runat="server" />
	</div>
	<div style="
		-webkit-box-sizing: border-box;
		   -moz-box-sizing: border-box;
				box-sizing: border-box;
		float: <%= historyFloat %>;
		height: 100%;
		overflow: auto;
		padding: 5px;
		width: 50%;
	">
		<h1 class="dialogheading htmlcontent">
			<asp:Literal ID="litHistoryHead" runat="server" /></h1>
		<asp:Literal ID="litHistoryVersion" runat="server" />
		<div class="settingrow">
			<asp:Button ID="btnRestore" runat="server" />
		</div>
	</div>
</asp:Content>