<%@ Page Language="c#" CodeBehind="Thread.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.ForumUI.ForumThreadView" %>

<%@ Register TagPrefix="forum" TagName="PostList" Src="~/Forums/Controls/PostList.ascx" %>
<%@ Register TagPrefix="forum" TagName="PostListAlt" Src="~/Forums/Controls/PostListAlt.ascx" %>
<%@ Register Namespace="mojoPortal.Web.ForumUI" Assembly="mojoPortal.Features.UI" TagPrefix="forum" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<forum:ThreadCrumbContainer ID="fcrumbs" runat="server" EnableViewState="false" CssClass="breadcrumbs forumthreadcrumbs">
		<asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink><forum:CrumbSeparatorLiteral ID="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" /><a href="" id="lnkForum" runat="server"></a>
		<forum:CrumbSeparatorLiteral ID="CrumbSeparatorLiteral1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" /><asp:Literal ID="litThreadDescription" runat="server" />
	</forum:ThreadCrumbContainer>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<mp:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper forumthreadview" EnableViewState="false">
			<forum:ForumDisplaySettings ID="displaySettings" runat="server" />
			<portal:HeadingControl ID="heading" runat="server" CssClass="threadheading" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<portal:FormGroupPanel runat="server" ID="fgpDescription" ExtraCssClasses="forumdesc" SkinID="ForumThreadDescription">
						<asp:Literal ID="litForumDescription" runat="server" />
					</portal:FormGroupPanel>

					<forum:PostList ID="postList" runat="server" />
					<forum:PostListAlt ID="postListAlt" runat="server" Visible="false" />
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
			<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
		</portal:InnerWrapperPanel>
		<mp:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
