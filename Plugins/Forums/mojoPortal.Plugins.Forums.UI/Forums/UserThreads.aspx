<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="UserThreads.aspx.cs" Inherits="mojoPortal.Web.ForumUI.ForumUserThreadsPage" %>
<%@ Register TagPrefix="forum" TagName="UserThreadList" Src="~/Forums/Controls/UserThreadList.ascx" %>
<%@ Register TagPrefix="forum" TagName="UserThreadListAlt" Src="~/Forums/Controls/UserThreadListAlt.ascx" %>
<%@ Register Namespace="mojoPortal.Web.ForumUI" Assembly="mojoPortal.Features.UI" TagPrefix="forum" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">

		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" cssclass="panelwrapper forumview">
		     <portal:HeadingControl ID="heading" runat="server" />
             <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
		     <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
		    <forum:ForumDisplaySettings ID="displaySettings" runat="server" />
		    <forum:UserThreadList id="threadList" runat="server" />
            <forum:UserThreadListAlt id="threadListAlt" runat="server" Visible="false" />
	</portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        
		</portal:InnerWrapperPanel>
		
        </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
