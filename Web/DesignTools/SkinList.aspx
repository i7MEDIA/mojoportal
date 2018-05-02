<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="SkinList.aspx.cs" Inherits="mojoPortal.Web.AdminUI.SkinListPage" %>

<asp:content contentplaceholderid="leftContent" id="MPLeftPane" runat="server" />
<asp:content contentplaceholderid="mainContent" id="MPContent" runat="server">
    <portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
        <asp:hyperlink id="lnkAdminMenu" runat="server" navigateurl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:hyperlink id="lnkDesignerTools" runat="server" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator3" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:hyperlink id="lnkThisPage" runat="server" cssclass="selectedcrumb" />
    </portal:AdminCrumbContainer>
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
        <mp:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
        <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" cssclass="panelwrapper ">
            <portal:HeadingControl id="heading" runat="server" />
            <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
                <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
                    <div id="divUpload" runat="server" class="settingrow">
                        <div class="importskin">
                            <portal:jQueryFileUpload ID="uploader" runat="server" CssClass="forminput normaltextbox" /> 
                            <portal:mojoButton ID="btnUpload" runat="server" ValidationGroup="upload" CssClass="importformbutton" />
                            <asp:HiddenField ID="hdnState" Value="images" runat="server" />
                        </div>
                        <div class="settingrow">
                        <asp:Checkbox id="chkOverwrite" runat="server" Checked="true" />
                        </div>
                        <asp:regularexpressionvalidator id="regexZipFile" controltovalidate="uploader" display="Dynamic" enableclientscript="true" runat="server" validationgroup="upload" />
                        <asp:requiredfieldvalidator id="reqZipFile" runat="server" controltovalidate="uploader" display="Dynamic" validationgroup="upload" />
                    </div>
                    <asp:repeater id="rptSkins" runat="server">
                        <headertemplate>
                            <ul class="simplelist skinlist">
                            
                        </headertemplate>
                        <itemtemplate>
                            <li class="simplelist">
                                <%# Eval("Name") %>
                                <%# BuildDownloadLink(Eval("Name").ToString()) %>
                                <asp:HyperLink ID="lnkSkinPreview" runat="server" CssClass="cblink" Text='<%# PreviewText %>' NavigateUrl='<%# SiteRoot + "/?skin=" + Eval("Name")  %>' />
                                <asp:Hyperlink id="lnkManage" runat="server" Visible='<%# allowEditing %>' Text='<%# ManageText %>' NavigateUrl='<%# SiteRoot + "/DesignTools/ManageSkin.aspx?s=" + Eval("Name")  %>' />
                            </li>
                        </itemtemplate>
                        <footertemplate>
                            </ul>
                        </footertemplate>
                    </asp:repeater>
                </portal:InnerBodyPanel>
            </portal:OuterBodyPanel>
            <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
        </portal:InnerWrapperPanel>
        <mp:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
    </portal:OuterWrapperPanel>
</asp:content>
<asp:content contentplaceholderid="rightContent" id="MPRightPane" runat="server" >
</asp:Content>
<asp:content contentplaceholderid="pageEditContent" id="MPPageEdit" runat="server" >
</asp:Content>

