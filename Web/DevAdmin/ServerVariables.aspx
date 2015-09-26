<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ServerVariables.aspx.cs"
    Inherits="mojoPortal.Web.AdminUI.ServerVariablesPage" %>

<%@ Register TagPrefix="dev" TagName="ServerVars" Src="~/DevAdmin/Controls/ServerVars.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Query Tool</title>
    <portal:StyleSheetCombiner ID="StyleSheetCombiner" runat="server" UseIconsForAdminLinks="false" />
    <portal:IEStyleIncludes ID="IEStyleIncludes1" runat="server" IncludeHtml5Script="false" />
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
        <dev:servervars id="sv1" runat="server" />
    </div>
    <portal:mojoGoogleAnalyticsScript ID="mojoGoogleAnalyticsScript1" runat="server" />
    <portal:Woopra ID="woopra11" runat="server" />
    </form>
</body>
</html>
