<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="ContentTemplates.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ContentTemplatesPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
    <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
    <asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
    </portal:AdminCrumbContainer>
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin contenttemplates">
        <portal:HeadingControl id="heading" runat="server" />
  <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
            <div class="settingrow addnewtemplate">
                <asp:HyperLink ID="lnkAddNewTop" runat="server" SkinID="SuccessButton" />
				<asp:HyperLink ID="lnkExportTop" runat="server" SkinID="InfoButton" />
			</div>
            <portal:mojoCutePager ID="pgrTop" runat="server" />
            <asp:Repeater ID="rptTemplates" runat="server">
                <ItemTemplate>
                    <div class="templatewrapper">
                        <h3 class="templatetitle">
                            <%# Eval("Title") %>
                            <asp:HyperLink ID="lnkEdit" runat="server" NavigateUrl='<%# SiteRoot + "/Admin/ContentTemplateEdit.aspx?t=" + Eval("Guid") %>'
                                Text='<%# Resources.Resource.ContentTemplateEditLink %>' CssClass="ModuleEditLink" /></h3>
                        <div id="divBody" runat="server" visible='<%# showBody %>' class="templatebody">
                            <%# Eval("Body") %>
                        </div>
                        <div class="templateinfo">
                            <%# Eval("Description") %>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div class="settingrow addnewtemplate addnewtemplatebottom">
                <br />
                <asp:HyperLink ID="lnkAddNewBottom" runat="server" SkinID="SuccessButton" />
                <asp:HyperLink ID="lnkExportBottom" runat="server" SkinID="InfoButton" />
            </div>
            <portal:mojoCutePager ID="pgrBottom" runat="server" />
            <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
        </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
    </portal:InnerWrapperPanel>
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
