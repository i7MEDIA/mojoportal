<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Help.aspx.cs" Inherits="mojoPortal.Web.UI.Pages.Help" %>

<!DOCTYPE html>
<html>
	<head runat="server">
		<title>Untitled Page</title>
		<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous">
	</head>

	<body class="help-page">
		<form id="form1" runat="server">
			<asp:Panel ID="pnlHelp" runat="server">
				<asp:Literal ID="litEditLink" runat="server" />
				<asp:Literal ID="litHelp" runat="server" />
			</asp:Panel>
		</form>
	</body>
</html>
