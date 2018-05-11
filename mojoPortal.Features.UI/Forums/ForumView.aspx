<%@ Page Language="c#" CodeBehind="ForumView.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.ForumUI.ForumView" %>

<%@ Register TagPrefix="forum" TagName="SearchBox" Src="~/Forums/Controls/ForumSearchBox.ascx" %>
<%@ Register TagPrefix="forum" TagName="ThreadList" Src="~/Forums/Controls/ThreadList.ascx" %>
<%@ Register TagPrefix="forum" TagName="ThreadListAlt" Src="~/Forums/Controls/ThreadListAlt.ascx" %>
<%@ Register Namespace="mojoPortal.Web.ForumUI" Assembly="mojoPortal.Features.UI" TagPrefix="forum" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<div class="breadcrumbs forumthreadcrumbs">
		<asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink>
	</div>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<mp:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper forumview" EnableViewState="false">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<forum:ForumDisplaySettings ID="displaySettings" runat="server" />
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<portal:FormGroupPanel runat="server" ID="fgpDescription" ExtraCssClasses="forumdesc">
						<asp:Literal ID="litForumDescription" runat="server" />
					</portal:FormGroupPanel>

					<forum:SearchBox ID="searchBoxTop" runat="server" />
					<forum:ThreadList ID="threadList" runat="server" />
					<forum:ThreadListAlt ID="threadListAlt" runat="server" Visible="false" />
					<forum:SearchBox ID="searchBoxBottom" runat="server" Visible="false" />
					<portal:EmptyPanel ID="divFooter" runat="server" CssClass="modulefooter" SkinID="modulefooter">&nbsp;</portal:EmptyPanel>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
			<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
		</portal:InnerWrapperPanel>
		<mp:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
