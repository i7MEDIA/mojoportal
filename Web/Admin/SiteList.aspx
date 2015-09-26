<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="SiteList.aspx.cs" Inherits="mojoPortal.Web.AdminUI.SiteListPage" %>

<asp:content contentplaceholderid="leftContent" id="MPLeftPane" runat="server" />
<asp:content contentplaceholderid="mainContent" id="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx"
            CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkSiteList" runat="server" CssClass="selectedcrumb" />
</portal:AdminCrumbContainer>
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
        <mp:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
        <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" cssclass="panelwrapper sitelistpage ">
        <portal:HeadingControl id="heading" runat="server" />
            <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
                <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
                    <asp:repeater id="rptSites" runat="server">
                        <headertemplate>
                            <ul class="simplelist sitelist">
                            <li class="simplelist">
                                <%# CurrentSiteName %>
                                <a class="siteitem currentsite" href='<%# SiteRoot + "/Admin/SiteSettings.aspx" %>'><%# Resources.Resource.AdminMenuSiteSettingsLink %></a>
                                <a class="siteitem currentsite" href='<%# SiteRoot + "/Admin/PermissionsMenu.aspx" %>'><%# Resources.Resource.SiteSettingsPermissionsTab %></a>
                            </li>
                        </headertemplate>
                        <itemtemplate>
                            <li class="simplelist">
                                <%# Eval("SiteName") %>
                                <asp:Label id="lblSiteID" runat="server" CssClass="siteidlabel" Visible='<%# showSiteIDInSiteList %>' Text='<%# FormatSiteId(Convert.ToInt32(Eval("SiteID"))) %>' />
                                <a class="siteitem" href='<%# SiteRoot + "/Admin/SiteSettings.aspx?SiteID=" + Eval("SiteID") %>'><%# Resources.Resource.AdminMenuSiteSettingsLink %></a>
                                <a class="siteitem" href='<%# SiteRoot + "/Admin/PermissionsMenu.aspx?SiteID=" + Eval("SiteID") %>'><%# Resources.Resource.SiteSettingsPermissionsTab%></a>
                            </li>
                        </itemtemplate>
                        <footertemplate>
                            </ul>
                        </footertemplate>
                    </asp:repeater>
                    <portal:mojoCutePager ID="pgr" runat="server" />
                    <div class="settingrow">
                        <asp:Hyperlink id="lnkNewSite" runat="server" />
                    </div>
                </portal:InnerBodyPanel>
            </portal:OuterBodyPanel>
            <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
        </portal:InnerWrapperPanel>
        <mp:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
    </portal:OuterWrapperPanel>
</asp:content>
<asp:content contentplaceholderid="rightContent" id="MPRightPane" runat="server" />
<asp:content contentplaceholderid="pageEditContent" id="MPPageEdit" runat="server" />
