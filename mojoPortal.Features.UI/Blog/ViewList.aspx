<%@ Page Language="C#" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" CodeBehind="ViewList.aspx.cs" Inherits="mojoPortal.Web.BlogUI.ViewList" %>

<%@ Register TagPrefix="blog" TagName="PostList" Src="~/Blog/Controls/PostList.ascx" %>
<%@ Register TagPrefix="blog" TagName="SearchBox" Src="~/Blog/Controls/SearchBox.ascx" %>
<%@ Register Namespace="mojoPortal.Web.BlogUI" Assembly="mojoPortal.Features.UI" TagPrefix="blog" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper blogmodule ">
			<portal:ModuleTitleControl ID="moduleTitle" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<blog:BlogWrapperPanel id="blogWrapper1" runat="server" CssClass="blogwrapper">
						<blog:BlogDisplaySettings ID="displaySettings" runat="server" />
						<blog:SearchBox id="searchBoxTop" runat="server" />
						<blog:PostList ID="postList" runat="server" />
					</blog:BlogWrapperPanel>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
			<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
