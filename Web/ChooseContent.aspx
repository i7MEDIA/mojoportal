<%@ Page Language="C#" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" CodeBehind="ChooseContent.aspx.cs" Inherits="mojoPortal.Web.UI.Pages.ChooseContent" Title="Untitled Page" %>

<asp:Content ID="MPPageEdit" ContentPlaceHolderID="pageEditContent" runat="server">
</asp:Content>
<asp:Content ID="MPLeftPane" ContentPlaceHolderID="leftContent" runat="server">
</asp:Content>
<asp:Content ID="MPContent" ContentPlaceHolderID="mainContent" runat="server">
<% #if!MONO %>
<portal:mojoDataList ID="dlWebParts" runat="server"
     RepeatColumns="3" 
     RepeatDirection="vertical">
     <ItemTemplate>
        <img alt='<%# Eval("ModuleTitle")%>' src='<%# ImageSiteRoot + "/Data/SiteImages/FeatureIcons/" + GetIcon(Eval("ModuleIcon").ToString(),Eval("FeatureIcon").ToString())%>' />
        <%# Eval("ModuleTitle")%>
        <asp:Button ID="lnkAddToMyPage" runat="server" cssclass="buttonlink"
        CommandName='<%# GetCommandName(Convert.ToInt32(Eval("ModuleID")),Eval("WebPartID").ToString()) %>' 
        CommandArgument='<%# GetCommandArgument(Convert.ToInt32(Eval("ModuleID")),Eval("WebPartID").ToString()) %>' 
        Text='<%# Resources.Resource.ChooseContentAddToMyPageLink %>' ToolTip='<%# Resources.Resource.ChooseContentAddToMyPageLink %>' />
     </ItemTemplate>
 </portal:mojoDataList>
<% #endif %>

</asp:Content>
<asp:Content ID="MPRightPane" ContentPlaceHolderID="rightContent" runat="server">
</asp:Content>
