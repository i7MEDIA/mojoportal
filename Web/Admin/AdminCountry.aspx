<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AdminCountry.aspx.cs" Inherits="mojoPortal.Web.AdminUI.AdminCountryPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkCoreData" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkCurrentPage" runat="server" CssClass="selectedcrumb" />
    </portal:AdminCrumbContainer>
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin admincountry ">
        <portal:HeadingControl id="heading" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
            <mp:mojoGridView ID="grdCountry" runat="server" AllowPaging="false" AllowSorting="true"
                AutoGenerateColumns="false" CssClass="" DataKeyNames="Guid">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="buttonlink" Text='<%# Resources.Resource.CountryGridEditButton %>' /> 
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Button ID="btnGridUpdate" runat="server" Text='<%# Resources.Resource.CountryGridUpdateButton %>'
                                CommandName="Update" />
                            <asp:Button ID="btnGridDelete" runat="server" Text='<%# Resources.Resource.CountryGridDeleteButton %>'
                                CommandName="Delete" />
                            <asp:Button ID="btnGridCancel" runat="server" Text='<%# Resources.Resource.CountryGridCancelButton %>'
                                CommandName="Cancel" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("Name") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtName" Columns="40" Text='<%# Eval("Name") %>' runat="server"
                                MaxLength="255" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("ISOCode2")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtISOCode2" Columns="5" Text='<%# Eval("ISOCode2") %>' runat="server"
                                MaxLength="2" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("ISOCode3")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtISOCode3" Columns="5" Text='<%# Eval("ISOCode3") %>' runat="server"
                                MaxLength="3" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </mp:mojoGridView>
            <div class="settingrow">
               <portal:mojoButton ID="btnAddNew" runat="server" />
            </div>
             <portal:mojoCutePager ID="pgrCountry" runat="server" />
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
