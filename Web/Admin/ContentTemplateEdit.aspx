<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ContentTemplateEdit.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ContentTemplateEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkTemplates" runat="server" NavigateUrl="~/Admin/ContentTemplates.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
    </portal:AdminCrumbContainer>
    <mp:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
    <asp:Panel ID="pnl1" runat="server" CssClass="panelwrapper contenttemplates yui-skin-sam">
        <div class="modulecontent">
            <div class="settingrow">
                <mp:SiteLabel ID="lblTitle" runat="server" ForControl="txtTitle" CssClass="settinglabeltight"
                    ConfigKey="ContentTemplateTitleLabel" ResourceFile="Resource" />
                <asp:TextBox ID="txtTitle" CssClass="verywidetextbox forminput" runat="server" />
            </div>
            <div class="settingrow">
                &nbsp;</div>
            <div id="divtabs" class="mojo-tabs">
                <ul>
                    <li class="selected"><a href="#tabTemplate"><em>
                        <asp:Literal ID="litTemplateTab" runat="server" /></em></a></li>
                    <li id="liDescription" runat="server"><a id="lnkDescription" runat="server" href="#tabDescription">
                        <em>
                            <asp:Literal ID="litDescriptionTab" runat="server" /></em></a></li>
                    <li id="liSecurity" runat="server" visible="false"><a id="lnkSecurity" runat="server"
                        href="#tabSecurity"><em>
                            <asp:Literal ID="litSecurityTab" runat="server" /></em></a></li>
                </ul>
                <div id="tabTemplate">
                    <div class="settingrow">
                        <mpe:EditorControl ID="edTemplate" runat="server">
                        </mpe:EditorControl>
                    </div>
                </div>
                <div id="tabDescription">
                    <div class="settingrow">
                        <mpe:EditorControl ID="edDescription" runat="server">
                        </mpe:EditorControl>
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="lblLogo" ForControl="ddImage" runat="server" CssClass="settinglabel"
                            ConfigKey="ContentTemplateImageLabel" EnableViewState="false"></mp:SiteLabel>
                        <asp:DropDownList ID="ddImage" runat="server" TabIndex="10" EnableViewState="true"
                            DataValueField="Name" DataTextField="Name" CssClass="forminput">
                        </asp:DropDownList>
                        <img alt="" src="" id="imgTemplate" runat="server" enableviewstate="false" />
                    </div>
                </div>
                <div id="tabSecurity" runat="server" visible="false">
                    <div class="settingrow">
                        <portal:AllowedRolesSetting ID="arTemplate" runat="server" />
                    </div>
                </div>
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="spacer"
                    ResourceFile="Resource" />
                <portal:mojoButton ID="btnSave" runat="server" />
                <portal:mojoButton ID="btnDelete" runat="server" />
                <asp:HyperLink ID="lnkCancel" runat="server" />
            </div>
        </div>
    </asp:Panel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" 
    runat="server" >
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" 
    runat="server" >
</asp:Content>

