<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Manage.aspx.cs" Inherits="mojoPortal.Web.BlogUI.ManagePage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">

<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper ">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<ul class="simplelist blogmenu"> 
    <li class="lnkNewPost">
        <asp:HyperLink ID="lnkNewPost" runat="server" />
    </li>
    <li class="lnkDrafts">
        <asp:HyperLink ID="lnkDrafts" runat="server" />
    </li>
    <li class="lnkCategories">
        <asp:HyperLink ID="lnkCategories" runat="server" />
    </li>
    <li class="lnkClosedPosts">
        <asp:HyperLink ID="lnkClosedPosts" runat="server" />
    </li> 
</ul>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>

</portal:InnerWrapperPanel> 
	
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
