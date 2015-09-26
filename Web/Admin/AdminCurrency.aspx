<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AdminCurrency.aspx.cs" Inherits="mojoPortal.Web.AdminUI.AdminCurrencyPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkCoreData" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkCurrentPage" runat="server" CssClass="selectedcrumb" />
    </portal:AdminCrumbContainer>
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin admincurrency ">
        <portal:HeadingControl id="heading" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
            <div class="settingrow">
            <mp:mojoGridView ID="grdCurrency" runat="server" AllowPaging="false" AllowSorting="false" 
                AutoGenerateColumns="false"  DataKeyNames="Guid">
                <Columns>
                    <asp:TemplateField SortExpression="Title">
                        <ItemTemplate>
                            <asp:Button ID="btnEditCurrency" runat="server" CommandName="Edit" CssClass="buttonlink" Text='<%# Resources.Resource.CurrencyGridEditButton %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Title">
                        <ItemTemplate>
                            <%# Eval("Title") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div class="settingrow">
                                <mp:SiteLabel ID="SiteLabel19" runat="server" CssClass="settinglabel" ConfigKey="CurrencyGridTitleHeading"
                                    ResourceFile="Resource" />
                                <asp:TextBox ID="txtTitle" Columns="20" Text='<%# Eval("Title") %>' runat="server"
                                    MaxLength="50" CssClass="forminput" />
                            </div>
                            <div class="settingrow">
                                <mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="CurrencyGridCodeHeading"
                                    ResourceFile="Resource" />
                                <asp:TextBox ID="txtCode" Columns="20" Text='<%# Eval("Code") %>' runat="server"
                                    MaxLength="3" CssClass="forminput" />
                            </div>
                            
                            <div>
                                <asp:Button ID="btnGridUpdate" runat="server" Text='<%# Resources.Resource.CurrencyGridUpdateButton %>'
                                    CommandName="Update" />
                                <asp:Button ID="btnGridDelete" runat="server" Text='<%# Resources.Resource.CurrencyGridDeleteButton %>'
                                    CommandName="Delete" />
                                <asp:Button ID="btnGridCancel" runat="server" Text='<%# Resources.Resource.CurrencyGridCancelButton %>'
                                    CommandName="Cancel" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </mp:mojoGridView>
            </div>
            <div class="settingrow">
                <portal:mojoButton ID="btnAddNew" runat="server" />
            </div>
        </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
