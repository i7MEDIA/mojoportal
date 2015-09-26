<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AdminTaxClass.aspx.cs" Inherits="mojoPortal.Web.AdminUI.AdminTaxClassPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkCoreData" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkCurrentPage" runat="server" CssClass="selectedcrumb" />
    </portal:AdminCrumbContainer>
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin admintaxclass">
       <portal:HeadingControl id="heading" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
            <mp:mojoGridView ID="grdTaxClass" runat="server" AllowPaging="false" AllowSorting="false"
                AutoGenerateColumns="false"  DataKeyNames="Guid">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="buttonlink" Text='<%# Resources.Resource.TaxClassGridEditButton %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Button ID="btnGridUpdate" runat="server" Text='<%# Resources.Resource.TaxClassGridUpdateButton %>'
                                CommandName="Update" />
                            <asp:Button ID="btnGridDelete" runat="server" Text='<%# Resources.Resource.TaxClassGridDeleteButton %>'
                                CommandName="Delete" />
                            <asp:Button ID="btnGridCancel" runat="server" Text='<%# Resources.Resource.TaxClassGridCancelButton %>'
                                CommandName="Cancel" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("Title") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtTitle" Columns="20" Text='<%# Eval("Title") %>' runat="server"
                                MaxLength="255" />
                            <asp:TextBox ID="txtDescription" Columns="20" Text='<%# Eval("Description") %>' runat="server" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                        <p class="nodata"><asp:Literal id="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" /></p>
                </EmptyDataTemplate>
            </mp:mojoGridView>
            <div class="settingrow">
                <portal:mojoButton ID="btnAddNew" runat="server" />
            </div>
                <portal:mojoCutePager ID="pgrTaxClass" runat="server" />
            <portal:EmptyPanel id="divCleared1" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
        </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
