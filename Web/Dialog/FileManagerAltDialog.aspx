<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="FileManagerAltDialog.aspx.cs" Inherits="mojoPortal.Web.Dialog.FileManagerAltDialog" %>
<%@ Register TagPrefix="admin" TagName="FileManager" Src="~/Admin/Controls/FileManager.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <portal:StyleSheetCombiner ID="StyleSheetCombiner" runat="server" />
    <portal:ScriptLoader ID="ScriptInclude" runat="server" IncludeJQTable="true" IncludeQtFile="true" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="breadcrumbs">
        <asp:HyperLink ID="lnkAltFileManager" runat="server" NavigateUrl="FileManagerDialog.aspx" CssClass="altfile" />
    </div>
    <div>
        <admin:FileManager ID="fm1" runat="server" />
    </div>
    </form>
</body>
</html>
