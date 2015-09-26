<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HelpEdit.aspx.cs" Inherits="mojoPortal.Web.UI.Pages.HelpEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <portal:StyleSheetCombiner id="StyleSheetCombiner" runat="server" />
</head>
<body class="help-page">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" />
    <div id="divEditor" runat="server">
        <mpe:EditorControl id="edContent" runat="server"></mpe:EditorControl>
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
        <asp:HyperLink ID="lnkCancel" runat="server" />
        <portal:SessionKeepAliveControl id="ka1" runat="server" />
    </div>
    
    <portal:mojoGoogleAnalyticsScript ID="mojoGoogleAnalyticsScript1" runat="server" />
    </form>
</body>
</html>
