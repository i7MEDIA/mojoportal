<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="QueryTool.aspx.cs" Inherits="mojoPortal.Web.AdminUI.QueryToolPage" %>

<%@ Register TagPrefix="dev" TagName="QueryTool" Src="~/DevAdmin/Controls/QueryTool.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Query Tool</title>
    <portal:StyleSheetCombiner ID="StyleSheetCombiner" runat="server" UseIconsForAdminLinks="false" />
    <portal:IEStyleIncludes ID="IEStyleIncludes1" runat="server" IncludeHtml5Script="false"  />
    <portal:Favicon id="Favicon1"  runat="server" />
	<portal:ScriptLoader id="ScriptLoader1" runat="server" />
</head>
<body class="querytool">
    <form id="form1" runat="server">
    <div class="breadcrumbs">
        <asp:HyperLink ID="lnkHome" runat="server"  />&nbsp;&gt;
        <asp:HyperLink ID="lnkAdminMenu" runat="server"  />&nbsp;&gt;
        <asp:HyperLink ID="lnkAdvancedTools" runat="server" />&nbsp;&gt;
        <asp:HyperLink ID="lnkDevTools" runat="server" />&nbsp;&gt;
        <asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
    </div>
    <div>
        <dev:QueryTool id="qt1" runat="server" />
    </div>
    <portal:mojoGoogleAnalyticsScript ID="mojoGoogleAnalyticsScript1" runat="server" />
    <portal:Woopra ID="woopra11" runat="server" />
    </form>
</body>
</html>
