<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Confirm.aspx.cs" Inherits="mojoPortal.Web.ELetterUI.ConfirmPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
<asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
</portal:AdminCrumbContainer>
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnl1" runat="server" CssClass="panelwrapper ">
<div class="modulecontent">
<asp:Panel ID="pnlConfirmed" runat="server" Visible="false">
<mp:SiteLabel id="SiteLabel1" runat="server"  ConfigKey="NewslettersSubscriptionConfirmed"></mp:SiteLabel><br />
<asp:Literal ID="litConfrimDetails" runat="server" />
</asp:Panel>
<asp:Panel ID="pnlNotFound" runat="server" Visible="true">
<mp:SiteLabel id="lblWarning" runat="server" CssClass="txterror warning" ConfigKey="NewslettersSubscriptionNotFound"></mp:SiteLabel>
</asp:Panel>
</div>
</asp:Panel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
