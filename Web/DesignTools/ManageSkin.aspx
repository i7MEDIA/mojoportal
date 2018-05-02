<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ManageSkin.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ManageSkinPage" %>

<asp:content contentplaceholderid="leftContent" id="MPLeftPane" runat="server" />
<asp:content contentplaceholderid="mainContent" id="MPContent" runat="server">
        <portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
            <asp:hyperlink id="lnkAdminMenu" runat="server" navigateurl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
            <asp:hyperlink id="lnkDesignerTools" runat="server" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator3" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
            <asp:hyperlink id="lnkSkinList" runat="server" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator4" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
            <asp:hyperlink id="lnkThisPage" runat="server" cssclass="selectedcrumb" />
        </portal:AdminCrumbContainer>
        <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
        <mp:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
        <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" cssclass="panelwrapper ">
            <portal:HeadingControl id="heading" runat="server" />
            <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
                <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
                    <div class="settingrow">
                        <portal:mojoButton id="btnCopy" runat="server" />
                        <asp:textbox id="txtCopyAs" runat="server" cssclass="normaltextbox forminput" />
                        <portal:mojoHelpLink ID="MojoHelpLink2" runat="server" HelpKey="skinmanager-copyas-help" />
                    </div>
					<asp:Literal ID="litCssFiles" runat="server" />
<%--                    <asp:repeater id="rptCss" runat="server">
                        <headertemplate>
                            <ul class="simplelist skinfilelist">
                            
                        </headertemplate>
                        <itemtemplate>
                            <li class="simplelist">
                                <asp:Hyperlink id="lnkEdit" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# SiteRoot + "/DesignTools/CssEditor.aspx?s=" + skinName + "&amp;f=" + Eval("Name")  %>' />
                            </li>
                        </itemtemplate>
                        <footertemplate>
                            </ul>
                        </footertemplate>
                    </asp:repeater>--%>
                </portal:InnerBodyPanel>
            </portal:OuterBodyPanel>
            <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
        </portal:InnerWrapperPanel>
        <mp:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
    </portal:OuterWrapperPanel>
</asp:content>
<asp:content contentplaceholderid="rightContent" id="MPRightPane" runat="server" />
<asp:content contentplaceholderid="pageEditContent" id="MPPageEdit" runat="server" />
