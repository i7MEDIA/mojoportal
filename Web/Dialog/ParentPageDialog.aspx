<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master" CodeBehind="ParentPageDialog.aspx.cs" Inherits="mojoPortal.Web.UI.ParentPageDialog" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server">
<asp:Literal id="litStyleLink" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
<asp:SiteMapDataSource ID="SiteMapData" runat="server" ShowStartingNode="false" />
<div  style="padding: 5px 5px 5px 5px;" class="parentpagedialog">
<div class="settingrow">
<asp:Hyperlink id="lnkRoot" runat="server" Visible="false" CssClass="rootlink" />
</div>
<portal:mojoTreeView ID="tree" runat="server" SkinID="ParentPageDialog"
    ContainerCssClass="treecontainer"
    RenderLiCssClasses="true"
	RenderAnchorCss="false"
	LiCssClass="leaf"
	LiRootExpandableCssClass="root"
	LiRootNonExpandableCssClass="root leaf"
	LiNonRootExpnadableCssClass="parent"
	LiSelectedCssClass="selected"
	LiChildSelectedCssClass="childselected"
	LiParentSelectedCssClass="parentselected"
	AnchorCssClass="inactive"
	AnchorSelectedCssClass="current"
 />
</div>
</asp:Content>
