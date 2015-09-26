<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AdminTaxRate.aspx.cs" Inherits="mojoPortal.Web.AdminUI.AdminTaxRatePage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkCoreData" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator2" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkCurrentPage" runat="server" CssClass="selectedcrumb" />
    </portal:AdminCrumbContainer>
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin admintaxrate">
        <portal:HeadingControl id="heading" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
            <div class="settingrow">
                <mp:SiteLabel ID="SiteLabel19" runat="server" CssClass="settinglabel" ConfigKey="TaxRateGridCountryHeader" />
                <asp:DropDownList ID="ddCountry" runat="server" DataValueField="Guid" DataTextField="Name"
                    AutoPostBack="true" />
            </div>
            <div class="settingrow">
                <mp:SiteLabel ID="SiteLabel5" runat="server" CssClass="settinglabel" ConfigKey="TaxRateGridGeoZoneHeader" />
                <asp:DropDownList ID="ddGeoZones" runat="server" DataValueField="Guid" DataTextField="Name"
                    AutoPostBack="true" />
            </div>
            <mp:mojoGridView ID="grdTaxRate" runat="server" AllowPaging="false" AllowSorting="false"
                AutoGenerateColumns="false"  DataKeyNames="Guid">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="buttonlink" Text='<%# Resources.Resource.TaxRateGridEditButton %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("Description") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div class="settingrow">
                                <mp:SiteLabel ID="SiteLabel19" runat="server" CssClass="settinglabel" ConfigKey="TaxRateGridDescriptionHeader"
                                    ResourceFile="Resource" />
                                <asp:TextBox ID="txtDescription" Columns="20" Text='<%# Eval("Description") %>' runat="server"
                                    MaxLength="255" CssClass="forminput" />
                            </div>
                            <div class="settingrow">
                                <mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="TaxRateGridTaxClassHeader"
                                    ResourceFile="Resource" />
                                <asp:DropDownList ID="ddTaxClass" runat="server" DataValueField="Guid" DataTextField="Title"
                                    CssClass="forminput" />
                            </div>
                            <div class="settingrow">
                                <mp:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="TaxRateGridPriorityHeader"
                                    ResourceFile="Resource" />
                                <asp:TextBox ID="txtPriority" Columns="20" Text='<%# Eval("Priority") %>' runat="server"
                                    MaxLength="4" CssClass="forminput" />
                            </div>
                            <div class="settingrow">
                                <mp:SiteLabel ID="SiteLabel4" runat="server" CssClass="settinglabel" ConfigKey="TaxRateGridRateHeader"
                                    ResourceFile="Resource" />
                                <asp:TextBox ID="txtRate" Columns="20" Text='<%# Eval("Rate") %>' runat="server"
                                    MaxLength="9" CssClass="forminput" />
                            </div>
                            <div>
                                <asp:Button ID="btnGridUpdate" runat="server" Text='<%# Resources.Resource.TaxRateGridUpdateButton %>'
                                    CommandName="Update" />
                                <asp:Button ID="btnGridDelete" runat="server" Text='<%# Resources.Resource.TaxRateGridDeleteButton %>'
                                    CommandName="Delete" />
                                <asp:Button ID="btnGridCancel" runat="server" Text='<%# Resources.Resource.TaxRateGridCancelButton %>'
                                    CommandName="Cancel" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("Rate") %>
                        </ItemTemplate>
                        <EditItemTemplate>
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
