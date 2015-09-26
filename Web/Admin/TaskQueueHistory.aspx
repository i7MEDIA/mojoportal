<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="TaskQueueHistory.aspx.cs" Inherits="mojoPortal.Web.AdminUI.TaskQueueHistoryPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkAdvancedTools" runat="server" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkTaskQueueMonitor" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator3" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin taskqueue">
<portal:HeadingControl id="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<mp:mojoGridView ID="grdTaskQueue" runat="server" 
    AllowPaging="false" 
    AllowSorting="false"
    AutoGenerateColumns="false" 
    CssClass="" 
    DataKeyNames="Guid">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:ImageButton ID="btnDeletet" runat="server" 
                    CommandName="Delete" ToolTip='<%# Resources.Resource.TaskQueueGridDeleteButton %>'
                    AlternateText='<%# Resources.Resource.TaskQueueGridDeleteButton %>'
                    ImageUrl='<%# DeleteLinkImage %>' />&nbsp;&nbsp;&nbsp;&nbsp;
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# Eval("TaskName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# FormatDate(Eval("QueuedUTC")) %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
			<ItemTemplate>
                <%# FormatDate(Eval("StartUTC")) %>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <%# FormatDate(Eval("LastStatusUpdateUTC")) %>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <%# FormatDate(Eval("CompleteUTC")) %>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <%# GetPercentComplete(Eval("CompleteRatio"))%>
            </ItemTemplate>
		</asp:TemplateField>
		
    </Columns>
    <EmptyDataTemplate>
            <p class="nodata"><asp:Literal id="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" /></p>
    </EmptyDataTemplate>
</mp:mojoGridView>
<portal:mojoCutePager ID="pgrTaskQueue" runat="server" />
<div class="settingrow">
	<asp:Label ID="lblStatus" runat="server" />
</div>
<div class="settingrow">
	<br /><asp:HyperLink ID="lnkRefresh" runat="server" />
	<portal:mojoButton ID="btnClearHistory" runat="server" />
</div>
<portal:EmptyPanel id="divCleared1" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
