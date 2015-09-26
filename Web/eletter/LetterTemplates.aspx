<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="LetterTemplates.aspx.cs" Inherits="mojoPortal.Web.ELetterUI.LetterTemplatesPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
    <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
    <asp:HyperLink ID="lnkLetterAdmin" runat="server" CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
    <asp:HyperLink ID="lnkTemplates" runat="server" CssClass="selectedcrumb" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper letterhtmltemplate">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<mp:mojoGridView ID="grdLetterHtmlTemplate" runat="server"
     AllowPaging="false"
     AutoGenerateColumns="false"
     ShowHeader="false"
     ShowFooter="false" 
     CssClass="editgrid"
     DataKeyNames="Guid"
     SkinID="plain">
     <Columns>
		<asp:TemplateField>
			<ItemTemplate>
                <a href='<%# SiteRoot + "/eletter/LetterTemplateEdit.aspx?t=" + Eval("Guid").ToString() %>' title='<%# Eval("Title").ToString().Coalesce(Resources.Resource.Untitled) %>'><%# Eval("Title").ToString().Coalesce(Resources.Resource.Untitled) %></a>
            </ItemTemplate>
		</asp:TemplateField>
</Columns>
<EmptyDataTemplate>
            <p class="nodata"><asp:Literal id="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" /></p>
    </EmptyDataTemplate>
</mp:mojoGridView>
<div class="settingrow">
	<br /><asp:HyperLink ID="lnkAddNew" runat="server" />
</div>
    <portal:mojoCutePager ID="pgrLetterHtmlTemplate" runat="server" />
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