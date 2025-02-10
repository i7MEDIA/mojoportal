<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HelpEdit.aspx.cs" Inherits="mojoPortal.Web.UI.Pages.HelpEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title>Untitled Page</title>
		<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous" />
	</head>

	<body class="help-page">
		<form id="form1" runat="server">
			<asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" />
			<div id="divEditor" runat="server">
				<mpe:EditorControl ID="edContent" runat="server"></mpe:EditorControl>
			
				<div class="mt-3 text-center">
					<asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="btn btn-secondary" />
					<asp:HyperLink ID="lnkCancel" runat="server" CssClass="btn btn-link" />
				</div>

				<portal:SessionKeepAliveControl ID="ka1" runat="server" />
			</div>
		</form>

		<script>
			const resizeObserver = new ResizeObserver(entries => window.frameElement.height = document.body.scrollHeight + 'px');

			resizeObserver.observe(document.body);
		</script>
	</body>
</html>
