<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ServerLog.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ServerLog" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx"
            CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkServerLog" runat="server" CssClass="selectedcrumb" />
    </portal:AdminCrumbContainer>
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin serverlog">
        <portal:HeadingControl id="heading" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">

        <asp:Panel ID="pnlDbLog" runat="server">
            <asp:repeater id="rptSystemLog" runat="server">
                <headertemplate>
                    <ul class="simplelist errorlog">
                </headertemplate>
                <itemtemplate>
                    <li class="simplelist logitem">
                       <h2 class="logitem <%# Eval("LogLevel") %>"> 
                       <asp:ImageButton ImageUrl='<%# DeleteLinkImage %>' CommandName="deleteitem" CommandArgument='<%# Eval("ID") %>' AlternateText="<%# Resources.Resource.DeleteButton %>" ToolTip="<%# Resources.Resource.DeleteButton %>" runat="server" ID="btnDeleteItem"/>
                       <%# FormatDate(Convert.ToDateTime(Eval("LogDate"))) %> <%# Eval("LogLevel") %> <%# Eval("Logger") %> <%# FormatIpAddress(Eval("IpAddress").ToString()) %> <%# Eval("Culture") %> <%# Server.HtmlEncode(Eval("ShortUrl").ToString()) %>  </h2>
                        <p class="logmessage">
                        <%# Server.HtmlEncode(Eval("Message").ToString()) %>
                       </p>
                    </li>
                </itemtemplate>
                <footertemplate>
                    </ul>
                </footertemplate>
            </asp:repeater>
            <portal:mojoCutePager ID="pgr" runat="server" />
            <asp:HyperLink ID="lnkRefresh2" runat="server" />
            <portal:mojoButton ID="btnClearDbLOg" runat="server" />
        </asp:Panel>

        <asp:Panel ID="pnlFileLog" runat="server">
            <asp:TextBox ID="txtLog" runat="server" Width="100%" Height="300px" TextMode="MultiLine"></asp:TextBox>
            <asp:HyperLink ID="lnkRefresh" runat="server" />
            <portal:mojoButton ID="btnClearLog" runat="server" />
            <portal:mojoButton ID="btnDownloadLog" runat="server" />
        </asp:Panel>
        </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
