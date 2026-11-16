<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="BlogModule.ascx.cs" Inherits="mojoPortal.Web.BlogUI.BlogModule" %>
<%@ Register TagPrefix="blog" TagName="PostList" Src="~/Blog/Controls/PostList.ascx" %>
<%@ Register Namespace="mojoPortal.Web.BlogUI" Assembly="mojoPortal.Features.UI" TagPrefix="blog" %>
<%@ Register TagPrefix="blog" TagName="SearchBox" Src="~/Blog/Controls/SearchBox.ascx" %>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
	<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper blogmodule">
		<portal:ModuleTitleControl ID="Title1" runat="server" />
		<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
			<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
				<blog:BlogWrapperPanel id="blogWrapper1" runat="server" CssClass="blogwrapper">
					<blog:BlogDisplaySettings ID="displaySettings" runat="server" />
					<blog:SearchBox id="searchBoxTop" runat="server" />
					<blog:PostList ID="postList" runat="server" />
				</blog:BlogWrapperPanel>
			</portal:InnerBodyPanel>
		</portal:OuterBodyPanel>
	</portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
