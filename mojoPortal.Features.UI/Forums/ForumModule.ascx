<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ForumModule.ascx.cs" Inherits="mojoPortal.Web.ForumUI.ForumModule" %>
<%@ Register TagPrefix="forum" TagName="SearchBox" Src="~/Forums/Controls/ForumSearchBox.ascx" %>
<%@ Register TagPrefix="forum" TagName="ForumList" Src="~/Forums/Controls/ForumList.ascx" %>
<%@ Register TagPrefix="forum" TagName="ForumListAlt" Src="~/Forums/Controls/ForumListAlt.ascx" %>
<%@ Register Namespace="mojoPortal.Web.ForumUI" Assembly="mojoPortal.Features.UI" TagPrefix="forum" %>

<forum:ForumDisplaySettings ID="displaySettings" runat="server" />
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper forums">
<portal:ModuleTitleControl id="Title1" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<forum:SearchBox id="searchBoxTop" runat="server" />
<forum:ForumList ID="forumList" runat="server" />
<forum:ForumListAlt ID="forumListAlt" runat="server" Visible="false" />
<forum:SearchBox id="searchBoxBottom" runat="server" Visible="false" />
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared" EnableViewState="false"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
