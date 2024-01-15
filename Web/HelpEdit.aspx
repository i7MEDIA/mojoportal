<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HelpEdit.aspx.cs" Inherits="mojoPortal.Web.UI.Pages.HelpEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Untitled Page</title>
	<portal:StyleSheetCombiner ID="StyleSheetCombiner" runat="server" />
</head>
<body class="help-page">
	<form id="form1" runat="server">
		<asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" />
		<div id="divEditor" runat="server">
			<mpe:EditorControl ID="edContent" runat="server"></mpe:EditorControl>
			<asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
			<asp:HyperLink ID="lnkCancel" runat="server" />
			<portal:SessionKeepAliveControl ID="ka1" runat="server" />
		</div>
	</form>
</body>
</html>
