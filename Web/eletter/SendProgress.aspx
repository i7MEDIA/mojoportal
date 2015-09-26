<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SendProgress.aspx.cs" Inherits="mojoPortal.Web.ELetterUI.SendProgressPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
    <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
    <asp:HyperLink ID="lnkLetterAdmin" runat="server" CssClass="unselectedcrumb" />
</portal:AdminCrumbContainer>
<portal:mojoPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnl1" runat="server" CssClass="art-Post-inner panelwrapper sendprogress ">
<h2 class="moduletitle"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:mojoPanel ID="MojoPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent">
<div class="settingrow">
   <mp:SiteLabel id="lbl1" runat="server" CssClass="settinglabel" ConfigKey="NewsletterLabel" ResourceFile="Resource" />
   <asp:Literal ID="litLetterInfoTitle" runat="server" /> 	
</div>
<div class="settingrow">
   <mp:SiteLabel id="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="NewsletterSubjectEditionLabel" ResourceFile="Resource" />
   <asp:Literal ID="litSubject" runat="server" /> 	
</div>
<div class="settingrow">
<asp:Panel ID="pnlProgress" runat="server"></asp:Panel>
<asp:Panel ID="pnlStatus" runat="server"></asp:Panel>
</div>
<asp:HyperLink ID="lnkSendLog" runat="server" />
</div>
</portal:mojoPanel>
<div class="cleared"></div>
</asp:Panel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</portal:mojoPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
