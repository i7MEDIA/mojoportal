<%@ Page language="c#" Codebehind="ContentValidation.aspx.cs" AutoEventWireup="True" Inherits="mojoPortal.Web.AdminUI.ContentValidation" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Content Validation</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<h3>Content Validation Utility</h3>
			<p>When mojoPortal displays untrusted content, the current cross-site scripting prevention 
			strategy validates it against XML schema that is designed to prevent inclusion of scripts.  
			If content is invalid according to the schema, mojoPortal displays the reason it was 
			considered invalid (in red)	followed by	the content in source form (i.e. with tags displayed).
			Occasionally benign content will be considered invalid as well.  Click below to search for
			invalid content and then, if you are convinced that some content is benign, follow its link 
			to edit the content to make it valid.</p>
			<table border="1">
				<tr>
					<th align="left">
						Database Connection String:
					</th>
					<td colspan="2">
						<asp:TextBox ID="txtConnectionString" Runat="server" Columns="80"></asp:TextBox>
					</td>
				</tr>
			</table>
			<asp:Button ID="validateBtn" Runat="server" Text="Find Invalid Content"></asp:Button>
			<br />
			<asp:Table id="tableResults" runat="server" EnableViewState="False" >
			</asp:Table>
		</form>
	</body>
</HTML>
