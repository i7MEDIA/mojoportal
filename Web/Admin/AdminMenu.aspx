<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="AdminMenu.aspx.cs" Inherits="mojoPortal.Web.AdminUI.AdminMenuPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper adminmenu">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
		<ul class="simplelist">
		    <li id="liSiteSettings" runat="server">
		        <asp:HyperLink ID="lnkSiteSettings" runat="server" CssClass="lnkSiteSettings" />
		    </li>
            <li id="liSiteList" runat="server">
		        <asp:HyperLink ID="lnkSiteList" runat="server" CssClass="lnkSiteList" />
		    </li>
            <li id="liSecurityAdvisor" runat="server" visible="false" class="liSecurityAdvisor">
		        <asp:HyperLink ID="lnkSecurityAdvisor" runat="server" CssClass="lnkSecurityAdvisor" />
                <span class="secwarning">
                <asp:image id="imgMachineKeyDanger" runat="server" Visible="false" CssClass="securitywarning"  />
                <mp:SiteLabel ID="lblNeedsAttantion" runat="server" CssClass="txterror needsattention" ConfigKey="NeedsAttention" UseLabelTag="false" Visible="false"></mp:SiteLabel>
                </span>
		    </li>
            <li id="liRoleAdmin" runat="server">
		        <asp:HyperLink ID="lnkRoleAdmin" runat="server" CssClass="lnkRoleAdmin" />
		    </li>
            <li id="liPermissions" runat="server">
		        <asp:HyperLink ID="lnkPermissionAdmin" runat="server" CssClass="lnkPermissionAdmin" />
		    </li>
		    <li id="liMemberList" runat="server">
		        <asp:HyperLink ID="lnkMemberList" runat="server" CssClass="lnkMemberList" />
		    </li>
		    <li id="liAddUser" runat="server">
		        <asp:HyperLink ID="lnkAddUser" runat="server" CssClass="lnkAddUser" />
		    </li>
            <li id="liPageTree" runat="server">
		        <asp:HyperLink ID="lnkPageTree" runat="server" CssClass="lnkPageTree" />
		    </li>
		    <li id="liContentManager" runat="server">
		        <asp:HyperLink ID="lnkContentManager" runat="server" CssClass="lnkContentManager" />
		    </li>
		    <li id="liContentWorkFlow" runat="server">
		        <asp:HyperLink ID="lnkContentWorkFlow" runat="server" CssClass="lnkContentWorkFlow" />
		    </li>
		    <li id="liContentTemplates" runat="server">
		        <asp:HyperLink ID="lnkContentTemplates" runat="server" CssClass="lnkContentTemplates" />
		    </li>
		    <li id="liStyleTemplates" runat="server">
		        <asp:HyperLink ID="lnkStyleTemplates" runat="server" CssClass="lnkStyleTemplates" />
		    </li>
		    <li id="liFileManager" runat="server">
		        <asp:HyperLink ID="lnkFileManager" runat="server" CssClass="lnkFileManager" />
		    </li>
		    <li id="liNewsletter" runat="server">
		        <asp:HyperLink ID="lnkNewsletter" runat="server" CssClass="lnkNewsletter" />
		    </li>
            <li id="liRegistrationAgreement" runat="server">
                    <asp:HyperLink ID="lnkRegistrationAgreement" runat="server" CssClass="lnkRegistrationAgreement" />
                </li>
            <li id="liLoginInfo" runat="server">
                    <asp:HyperLink ID="lnkLoginInfo" runat="server" CssClass="lnkLoginInfo" />
                </li>
		    <li id="liCoreData" runat="server">
		        <asp:HyperLink ID="lnkCoreData" runat="server" CssClass="lnkCoreData" />
		    </li>
		    <li id="liAdvancedTools" runat="server">
		        <asp:HyperLink ID="lnkAdvancedTools" runat="server" CssClass="lnkAdvancedTools" />
		    </li>
		    <li id="liLogViewer" runat="server">
		        <asp:HyperLink ID="lnkLogViewer" runat="server" CssClass="lnkLogViewer" />
		    </li>
		    <li id="liServerInfo" runat="server">
		        <asp:HyperLink ID="lnkServerInfo" runat="server" CssClass="lnkServerInfo" />
		    </li>
            <li id="liCommerceReports" runat="server">
		        <asp:HyperLink ID="lnkCommerceReports" runat="server" CssClass="lnkCommerceReports" />
		    </li>
		    <asp:Literal ID="litSupplementalLinks" runat="server" />
		</ul>
</portal:InnerBodyPanel>	
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
