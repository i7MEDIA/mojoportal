<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ContentCatalog.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ContentCatalogPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkContentManager" runat="server" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin contentcatalog">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    <asp:Panel ID="pnlNewContent" runat="server" CssClass="settingrow" DefaultButton="btnCreateNewContent">
        <asp:DropDownList ID="ddModuleType" runat="server" DataValueField="ModuleDefID" DataTextField="FeatureName">
        </asp:DropDownList>
        <asp:TextBox ID="txtModuleTitle" runat="server" Columns="40" Text="" CssClass="mediumtextbox"
            EnableViewState="false"></asp:TextBox>
        <portal:mojoButton ID="btnCreateNewContent" runat="server" ValidationGroup="contentcatalog" />
        <asp:RequiredFieldValidator ID="reqModuleTitle" runat="server" ControlToValidate="txtModuleTitle" ValidationGroup="contentcatalog" />
        <asp:CompareValidator ID="cvModuleTitle" runat="server" Operator="NotEqual" ControlToValidate="txtModuleTitle" ValidationGroup="contentcatalog" />
            
    </asp:Panel>
    <asp:Panel ID="pnlFind" runat="server" CssClass="settingrow" DefaultButton="btnFind">
        <mp:SiteLabel ID="lblTitleFilter" runat="server" ConfigKey="ContentManagerTitleFilterLabel" ForControl="txtTitleFilter" />
        <asp:TextBox ID="txtTitleFilter" runat="server" MaxLength="255" CssClass="mediumtextbox" />
        <portal:mojoButton ID="btnFind" runat="server" />
        <asp:CheckBox ID="chkFilterByFeature" runat="server" />
    </asp:Panel>
<div class="settingrow">
<mp:mojoGridView ID="grdContent" runat="server"
     AllowPaging="false"
     AllowSorting="true"
     AutoGenerateColumns="false"
     EnableViewState="false" CellPadding="3"
     DataKeyNames="ModuleID"
     UseAccessibleHeader="true"
     >
     <Columns>
        <asp:TemplateField SortExpression="ModuleTitle">
            <ItemTemplate>
                <%# Eval("ModuleTitle").ToString().Coalesce(Resources.Resource.ContentNoTitle)%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="FeatureName">
            <ItemTemplate>
                <%# mojoPortal.Web.Framework.ResourceHelper.GetResourceString(DataBinder.Eval(Container.DataItem, "ResourceFile").ToString(),DataBinder.Eval(Container.DataItem, "FeatureName").ToString()) %>
                (<%# Eval("UseCount") %>)
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="CreatedBy" ReadOnly="true" SortExpression="CreatedBy" />
        <asp:TemplateField >
            <ItemTemplate>
                <a href='<%# SiteRoot + "/Admin/ContentManagerPreview.aspx?mid=" + DataBinder.Eval(Container.DataItem,"ModuleID") %>'><%# Resources.Resource.ContentManagerViewEditLabel %></a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField >
            <ItemTemplate>
                <a href='<%# SiteRoot + WebConfigSettings.ContentPublishPageRelativeUrl + "?mid=" + DataBinder.Eval(Container.DataItem,"ModuleID") %>'><%# Resources.Resource.ContentManagerPublishDeleteLabel%></a>
            </ItemTemplate>
        </asp:TemplateField>
     </Columns>
     <EmptyDataTemplate>
            <p class="nodata"><asp:Literal id="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" /></p>
    </EmptyDataTemplate>
</mp:mojoGridView>
</div>
 <portal:mojoCutePager ID="pgrContent" runat="server" />
</portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
