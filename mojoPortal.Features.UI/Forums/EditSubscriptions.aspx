<%@ Page Language="c#" AutoEventWireup="false" Codebehind="EditSubscriptions.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" Inherits="mojoPortal.Web.ForumUI.ForumModuleEditSubscriptions" %>
<%@ Register TagPrefix="forum" TagName="SearchBox" Src="~/Forums/Controls/ForumSearchBox.ascx" %>
<%@ Register TagPrefix="forum" TagName="ForumList" Src="~/Forums/Controls/ForumList.ascx" %>
<%@ Register TagPrefix="forum" TagName="ForumListAlt" Src="~/Forums/Controls/ForumListAlt.ascx" %>
<%@ Register Namespace="mojoPortal.Web.ForumUI" Assembly="mojoPortal.Features.UI" TagPrefix="forum" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper forumview">
<forum:ForumDisplaySettings ID="displaySettings" runat="server" />
    <portal:HeadingControl ID="heading" runat="server" />
    <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
    <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    <forum:SearchBox id="searchBox1" runat="server" />
	<forum:ForumList ID="forumList" runat="server" ShowSubscribeCheckboxes="true" />
    <forum:ForumListAlt ID="forumListAlt" runat="server" ShowSubscribeCheckboxes="true" Visible="false" />
	</portal:InnerBodyPanel>
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />	
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />


