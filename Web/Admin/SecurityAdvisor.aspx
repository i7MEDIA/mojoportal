<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="SecurityAdvisor.aspx.cs" Inherits="mojoPortal.Web.AdminUI.SecurityAdvisorPage" %>

<asp:content contentplaceholderid="leftContent" id="MPLeftPane" runat="server" />
<asp:content contentplaceholderid="mainContent" id="MPContent" runat="server">
	<portalAdmin:AdminDisplaySettings ID="displaySettings" runat="server" />

	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
            <asp:hyperlink id="lnkAdminMenu" runat="server" navigateurl="~/Admin/AdminMenu.aspx"
                cssclass="unselectedcrumb" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
            <asp:hyperlink id="lnkThisPage" runat="server" cssclass="selectedcrumb" />
        </portal:AdminCrumbContainer>
        <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
        <mp:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
        <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" cssclass="panelwrapper securityadvisor">
        <portal:HeadingControl id="heading" runat="server" />
            <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
                <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">  
                    <p class="sadvisorintro">
                        <asp:literal id="litInfo" runat="server" />
                    </p>
					<portal:FormGroupPanel ID="fgpMachineKey" runat="server" SkinID="SecurityMachineKey">
						<asp:Literal ID="litMachineKeyHeading" runat="server" EnableViewState="false" />
						<asp:Literal ID="litMachineKeyResults" runat="server" EnableViewState="false" />
					</portal:FormGroupPanel>
                    <portal:FormGroupPanel ID="fgpFileSystem" runat="server" SkinID="SecurityFileSystem">
						<asp:Literal ID="litFileSystemHeading" runat="server" EnableViewState="false" />
						<asp:Literal ID="litFileSystemResults" runat="server" EnableViewState="false" />
                    </portal:FormGroupPanel>
					<portal:FormGroupPanel ID="fgpSecurityProtocol" runat="server" SkinID="SecurityProtocol">
						<asp:Literal ID="litSecurityProtocolHeading" runat="server" EnableViewState="false" />
						<asp:Literal ID="litSecurityProtocolDescription" runat="server" EnableViewState="false" />
					</portal:FormGroupPanel>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="spacer"  UseLabelTag="false"></mp:SiteLabel>
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
