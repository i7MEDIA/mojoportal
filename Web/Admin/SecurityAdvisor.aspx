<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="SecurityAdvisor.aspx.cs" Inherits="mojoPortal.Web.AdminUI.SecurityAdvisorPage" %>

<asp:content contentplaceholderid="leftContent" id="MPLeftPane" runat="server" />
<asp:content contentplaceholderid="mainContent" id="MPContent" runat="server">
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
                    <ul class="simplelist">
                        <li>
                            <mp:SiteLabel ID="SiteLabel5" runat="server" CssClass="" ConfigKey="UsingACustomMachineKey" UseLabelTag="false"></mp:SiteLabel>
                        <asp:image id="imgMachineKeyOk" runat="server" visible="false" />
                        <mp:SiteLabel ID="lblMachineKeyGood" runat="server" CssClass="goodsecurity" ConfigKey="OKLabel"  UseLabelTag="false" Visible="false"></mp:SiteLabel>
                        <asp:image id="imgMachineKeyDanger" runat="server" visible="false" />
                        <mp:SiteLabel ID="lblMachineKeyBad" runat="server" CssClass="txterror verybadsecurity" ConfigKey="SecurityDangerLabel" UseLabelTag="false" Visible="false"></mp:SiteLabel>
                        
                        <p><asp:TextBox id="txtRandomMachineKey" runat="server" TextMode="MultiLine" Rows="5" Columns="70" />
                        <asp:hyperlink id="lnkMachineKeyRefresh" runat="server" visible="false" />
                         </p>
                         <p><mp:SiteLabel ID="lblMachineKeyInstructions" runat="server" CssClass="" ConfigKey="CustomMachineKeyInstructions" UseLabelTag="false" Visible="false"></mp:SiteLabel></p>
                        </li>
                        <li>
                            <asp:Hyperlink id="lnkCheckFolders" runat="server" />
                            <asp:image id="imgFileSystemOk" runat="server" visible="false" />
                        <mp:SiteLabel ID="lblFileSystemOk" runat="server" CssClass="goodsecurity" ConfigKey="OKLabel"  UseLabelTag="false" Visible="false"></mp:SiteLabel>
                        <asp:image id="imgFileSystemWarning" runat="server" visible="false" />
                        <mp:SiteLabel ID="lblFileSystemWarning" runat="server" CssClass="txterror securitywarning" ConfigKey="WritePermissionsNotNeededOnFolders" UseLabelTag="false" Visible="false"></mp:SiteLabel>
                        <asp:hyperlink id="lnkFileSystemHelp" runat="server" visible="false" />
                            <asp:Literal id="litWritableFolderList" runat="server" />
                            </li>
                    </ul>
                    <div class="settingrow">
                        
                    </div>

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
