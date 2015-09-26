<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ContentWorkflow.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ContentWorkflowPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkCurrentPage" runat="server" CssClass="selectedcrumb" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server"  />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper contentworkflowpage adminmenu">
<portal:HeadingControl id="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
		<ul class="simplelist">		    
		    <li id="liAwaitingApproval" runat="server">
		        <asp:HyperLink ID="lnkAwaitingApproval" runat="server" CssClass="lnkAwaitingApproval" />
		    </li>	
            <li id="liAwaitingPublishing" runat="server">
		        <asp:HyperLink ID="lnkAwaitingPublishing" runat="server" CssClass="lnkAwaitingPublishing" />
		    </li>	
		    <li id="liRejectedContent" runat="server">
		        <asp:HyperLink ID="lnkRejectedContent" runat="server" CssClass="lnkRejectedContent" />
		    </li>
		    <li id="liPendingPages" runat="server">
		        <asp:HyperLink ID="lnkPendingPages" runat="server" CssClass="lnkPendingPages" />
		    </li>		    
		</ul>
	</portal:InnerBodyPanel>	
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" />	
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
