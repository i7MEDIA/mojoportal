<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Help.aspx.cs" Inherits="mojoPortal.Web.UI.Pages.Help" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Untitled Page</title>
    <portal:StyleSheetCombiner id="StyleSheetCombiner" runat="server" />
</head>
<body class="help-page">
    <form id="form1" runat="server">
    <asp:Panel ID="pnlHelp" runat="server">
        <asp:Literal ID="litEditLink" runat="server" />
        <asp:Literal ID="litHelp" runat="server" />
    </asp:Panel>
    <portal:mojoGoogleAnalyticsScript ID="mojoGoogleAnalyticsScript1" runat="server" />
    </form>
</body>
</html>
