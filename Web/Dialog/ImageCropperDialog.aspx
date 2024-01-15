<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ImageCropperDialog.aspx.cs" Inherits="mojoPortal.Web.Dialog.ImageCropperDialog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title></title>
	<portal:StyleSheetCombiner ID="StyleSheetCombiner" runat="server" IncludejCrop="true" />
	<portal:ScriptLoader ID="ScriptInclude" runat="server" IncludeJQuery="true" />
</head>
<body class="filedialog">
	<form id="form1" runat="server">

		<div id="filewrapper" style="padding: 10px; overflow: auto;">
			<asp:HyperLink ID="lnkReturn" runat="server" Visible="false" />
			<portal:ImageCropper ID="cropper" runat="server" />
		</div>
	</form>
</body>
</html>