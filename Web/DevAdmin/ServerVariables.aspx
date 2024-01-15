<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ServerVariables.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ServerVariablesPage" %>

<%@ Register TagPrefix="dev" TagName="ServerVars" Src="~/DevAdmin/Controls/ServerVars.ascx" %>
<!DOCTYPE html>
<html id="Html" runat="server">
<head id="Head1" runat="server">
	<title>Server Variables</title>
	<portal:StyleSheetCombiner ID="StyleSheetCombiner" runat="server" UseIconsForAdminLinks="false" />
	<portal:Favicon ID="Favicon1" runat="server" />
	<portal:ScriptLoader ID="ScriptLoader1" runat="server" />
</head>
<body>
	<form id="form1" runat="server">
		<div class="breadcrumbs">
			<asp:HyperLink ID="lnkHome" runat="server" />&nbsp;&gt;
			<asp:HyperLink ID="lnkAdminMenu" runat="server" />&nbsp;&gt;
			<asp:HyperLink ID="lnkAdvancedTools" runat="server" />&nbsp;&gt;
			<asp:HyperLink ID="lnkDevTools" runat="server" />&nbsp;&gt;
			<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
		</div>
		<h2 class="heading">
			<asp:Literal ID="litHeading" runat="server" /></h2>
		<div class="modulecontent">
			<dev:ServerVars ID="sv1" runat="server" />
		</div>
		<portal:Woopra ID="woopra11" runat="server" />
	</form>
</body>
</html>
