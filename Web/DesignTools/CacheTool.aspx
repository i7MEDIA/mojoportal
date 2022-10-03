<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="CacheTool.aspx.cs" Inherits="mojoPortal.Web.AdminUI.CacheToolPage" %>

<asp:content contentplaceholderid="leftContent" id="MPLeftPane" runat="server" />
<asp:content contentplaceholderid="mainContent" id="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
        <asp:hyperlink id="lnkAdminMenu" runat="server" navigateurl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:hyperlink id="lnkDesignerTools" runat="server" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator3" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:hyperlink id="lnkThisPage" runat="server" cssclass="selectedcrumb" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper ">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
        <div class="settingrow">
            <portal:mojoButton id="btnCssCacheToggle" runat="server" />
        </div>
        <div class="settingrow">
            <mp:SiteLabel ID="lblCacheInfo" runat="server" CssClass="cssinfo" ConfigKey="CssCacheInfo"
                ResourceFile="Resource" UseLabelTag="false" />
        </div>
        <div class="settingrow">
            <portal:mojoButton id="btnResetSkinVersionGuid" runat="server" />
            <asp:Label ID="lblSkinGuid" runat="server" CssClass="skinguid" />
        </div>
        <div class="settingrow">
            <mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="cssinfo" ConfigKey="SkinVersionGuidInfo"
                ResourceFile="Resource" UseLabelTag="false" />
        </div>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared" />
</portal:InnerWrapperPanel> 
</portal:OuterWrapperPanel>
</asp:content>
<asp:content contentplaceholderid="rightContent" id="MPRightPane" runat="server" />
<asp:content contentplaceholderid="pageEditContent" id="MPPageEdit" runat="server" />
