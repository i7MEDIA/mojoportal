<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="QueryTool.ascx.cs" Inherits="mojoPortal.Web.DevAdmin.Controls.QueryTool" %>
<h2 class="heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<div class="panelwrapper padded">
    <div>
        <p><em><asp:Literal ID="litWarning" runat="server" /></em></p>
    </div>
    <asp:Panel ID="pnlTables" runat="server" CssClass="settingrow">
        <mp:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" UseLabelTag="false" ResourceFile="DevTools" ConfigKey="Tables" />
        <asp:DropDownList ID="ddTables" runat="server" DataValueField="TableName" DataTextField="TableName" ></asp:DropDownList>
        <asp:Button ID="btnSelectTable" runat="server" />
    </asp:Panel>
    <div class="settingrow">
        <mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" UseLabelTag="false" ResourceFile="DevTools" ConfigKey="SavedQueries" />
        <asp:DropDownList ID="ddQueries" runat="server" DataValueField="Id" DataTextField="Name" ></asp:DropDownList>
        <asp:Button ID="btnLoadQuery" runat="server" />
        <asp:Button ID="btnDelete" runat="server" />
    </div>
    <div class="settingrow">
        <mp:SiteLabel ID="lblQuery" runat="server" CssClass="settinglabel" UseLabelTag="false" ResourceFile="DevTools" ConfigKey="SqlQuery" />
    </div>
    <div class="settingrow">
        <portal:CodeEditor ID="txtQuery" runat="server" Syntax="sql" CssClass="sqleditor" StartHighlighted="true" MinWidth="900" AllowToggle="false" Width="100%" />
    </div>
    <div>
        <asp:HyperLink ID="lnkClear" runat="server"  />
        <asp:Button ID="btnExecuteQuery" runat="server"  />
        <asp:Button ID="btnExport" runat="server"   />
        <asp:Button ID="btnExecuteNonQuery" runat="server"  />
        <asp:Button ID="btnSave" runat="server" />
        <asp:TextBox ID="txtQueryName" runat="server" MaxLength="50" />
    </div>
    <div>
        <asp:Label ID="lblError" runat="server" CssClass="txterror" />
    </div>
    <div>
        <mp:mojoGridView ID="grdResults" runat="server" AutoGenerateColumns="true">
        </mp:mojoGridView>
    </div>
</div>
