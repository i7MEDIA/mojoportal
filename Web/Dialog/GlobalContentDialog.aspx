<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="GlobalContentDialog.aspx.cs" Inherits="mojoPortal.Web.UI.GlobalContentDialog" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
<div style="padding: 5px 5px 5px 5px;" class="globalconentdialog">

<div class="settingrow">
<mp:mojoGridView ID="grid" runat="server"
     AllowPaging="false"
     AllowSorting="false"
     AutoGenerateColumns="false"
     EnableViewState="false" CellPadding="3"
     DataKeyNames="ModuleID"
     UseAccessibleHeader="true">
     <Columns>
        <asp:TemplateField SortExpression="ModuleTitle">
            <ItemTemplate>
                <%# Eval("ModuleTitle").ToString().Coalesce(Resources.Resource.ContentNoTitle)%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="FeatureName">
            <ItemTemplate>
                <%# mojoPortal.Web.Framework.ResourceHelper.GetResourceString(DataBinder.Eval(Container.DataItem, "ResourceFile").ToString(),DataBinder.Eval(Container.DataItem, "FeatureName").ToString()) %>
                <span class="contentusecount">(<%# Eval("UseCount") %>)</span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="CreatedBy" ReadOnly="true" SortExpression="CreatedBy" />
        <asp:TemplateField >
            <ItemTemplate>
                <portal:mojoButton ID="btnSelect" Text='<%# Resources.Resource.GlobalContentDialogSelectButton %>' runat="server" CommandName="select" CommandArgument='<%# Eval("ModuleID") %>'  />
            </ItemTemplate>
        </asp:TemplateField>
        
     </Columns>
</mp:mojoGridView>
</div>
<portal:mojoCutePager ID="pgr" runat="server" />
</div>
</asp:Content>
