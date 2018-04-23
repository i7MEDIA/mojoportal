<%@ Page CodeBehind="AccessDenied.aspx.cs" Language="c#" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.UI.Pages.AccessDeniedPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">	
<div class="modulecontent accessdenied">
    <mp:SiteLabel id="lblAccessDeniedLabel" runat="server" ConfigKey="AccessDeniedLabel" CssClass="txterror info"></mp:SiteLabel>
    <p>
		<asp:Literal id="lblAccessDenied" runat="server" />
	</p>
	<p>
		<asp:HyperLink ID="lnkHome" runat="server" /><br />
		<asp:HyperLink ID="lnkLogin" runat="server" />
	</p>
</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
