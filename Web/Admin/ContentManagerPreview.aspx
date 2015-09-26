<%@ Page Language="C#" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" CodeBehind="ContentManagerPreview.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ContentManagerPreview" Title="Untitled Page" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<asp:Panel id="pnlPreview" runat="server" CssClass="contentmanagerpreview">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
    <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
    <asp:HyperLink ID="lnkContentManager" runat="server" NavigateUrl="~/Admin/ContentCatalog.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
	 <mp:SiteLabel id="lbl1" runat="server" ConfigKey="ContentManagerPreviewContentLabel" UseLabelTag="false" ></mp:SiteLabel>
	 <asp:Label ID="lblModuleTitle" runat="server" />
     <small><asp:hyperlink id="lnkPublish" cssclass="ModuleEditLink" EnableViewState="false" runat="server" SkinID="plain" />
     &nbsp;<asp:HyperLink ID="lnkBackToList" runat="server" Visible="false" cssclass="ModuleEditLink" SkinID="plain"></asp:HyperLink></small>
 </portal:AdminCrumbContainer>  
 <asp:Panel ID="pnlWarning" runat="server" Visible="false">
 <mp:SiteLabel id="SiteLabel1" runat="server" CssClass="txterror warning" ConfigKey="ContentManagerNonMultiPageFeatureWarning" UseLabelTag="false" ></mp:SiteLabel>
 </asp:Panel>
<asp:Panel ID="pnlViewModule" runat="server"></asp:Panel>
</asp:Panel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
