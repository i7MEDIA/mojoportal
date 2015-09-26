<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="SalesCustomerReport.aspx.cs" Inherits="mojoPortal.Web.AdminUI.SalesCustomerReportPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkCommerceReports" runat="server" NavigateUrl="~/Admin/SalesSummary.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
    </portal:AdminCrumbContainer>
   <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" EnableViewState="false" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper salescustomerreport ">
        <portal:HeadingControl ID="heading" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
            <mp:mojoGridView ID="grdUsers" runat="server" AllowPaging="false" AllowSorting="false"
                 AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink Text='<%# Resources.Resource.ManageUserLink %>' ID="Hyperlink2" 
                                NavigateUrl='<%# SiteRoot + "/Admin/ManageUsers.aspx?userid=" + DataBinder.Eval(Container.DataItem,"UserID")   %>'
                                Visible="<%# IsAdmin %>" runat="server" />
                            <%# Eval("Name") %><br />
                            <%# Eval("LoginName") %>
                            -
                            <%# Eval("Email") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href='<%# SiteRoot + "/Admin/SalesCustomerDetail.aspx?u=" + Eval("UserGuid") %>'>
                                <%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("Revenue"))) %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                        <p class="nodata"><asp:Literal id="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" /></p>
                </EmptyDataTemplate>
            </mp:mojoGridView>
            <portal:mojoCutePager ID="pgrUsers" runat="server" />
        </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" EnableViewState="false" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
