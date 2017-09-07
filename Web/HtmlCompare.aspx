<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="HtmlCompare.aspx.cs" Inherits="mojoPortal.Web.ContentUI.HtmlCompare" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
	<div class="html-compare container-fluid">
		<div class="html-compare__toggle-highlight form-group">
			<asp:HyperLink ID="lnkToggleHighlight" runat="server" EnableViewState="false" CssClass="html-compare__toggle-highlight-link" />
		</div>
		<div class="html-compare__row row">
			<div class="html-compare__current-content col-md-6">
				<h3 class="html-compare__content-heading m-t-0">
					<asp:Literal ID="litCurrentHeading" runat="server" />
				</h3>
				<asp:Literal ID="litCurrentVersion" runat="server" />
			</div>
			<div class="html-compare__old-content col-md-6">
				<h3 class="html-compare__content-heading m-t-0">
					<asp:Literal ID="litHistoryHead" runat="server" />
				</h3>
				<asp:Literal ID="litHistoryVersion" runat="server" />
				<div class="settingrow">
					<asp:Button ID="btnRestore" runat="server" />
				</div>
			</div>
		</div>
	</div>
</asp:Content>