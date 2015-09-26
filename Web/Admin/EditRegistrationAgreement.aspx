<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="EditRegistrationAgreement.aspx.cs" Inherits="mojoPortal.Web.AdminUI.EditRegistrationAgreementPage" %>

<asp:content contentplaceholderid="leftContent" id="MPLeftPane" runat="server" />
<asp:content contentplaceholderid="mainContent" id="MPContent" runat="server">
    <portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
        <asp:hyperlink id="lnkAdminMenu" runat="server" navigateurl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:hyperlink id="lnkCurrentPage" runat="server" cssclass="selectedcrumb" />
    </portal:AdminCrumbContainer>
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
        <mp:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
        <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" cssclass="panelwrapper admin adminregagreement ">
            <portal:HeadingControl id="heading" runat="server" />
            <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
                <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="RegistrationPreamble" />
                    </div>
                    <div class="settingrow">
                        <mpe:EditorControl ID="edPreamble" runat="server"></mpe:EditorControl>
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="RegistrationAgreement" />
                    </div>
                    <div class="settingrow">
                        <mpe:EditorControl ID="edAgreement" runat="server"></mpe:EditorControl>
                    </div>
                    <div class="settingrow">
                    <portal:mojoButton id="btnSave" runat="server" />
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
