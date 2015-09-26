<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ContentStyles.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ContentStylesPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
    <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
    <asp:HyperLink ID="lnkThisPage" runat="server" CssClass="selectedcrumb" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin styletemplates">
 <portal:HeadingControl id="heading" runat="server" />
  <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
           <portal:mojoCutePager ID="pgrTop" runat="server" />
            <mp:mojoGridView ID="grdStyles" runat="server" AllowPaging="false" AllowSorting="false" EnableModelValidation="true"
                AutoGenerateColumns="false"  DataKeyNames="Guid">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="buttonlink" Text='<%# Resources.Resource.TaxClassGridEditButton %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Button ID="btnGridUpdate" runat="server" Text='<%# Resources.Resource.ContentStyleSave %>'
                                CommandName="Update" />
                            <asp:Button ID="btnGridDelete" runat="server" Text='<%# Resources.Resource.ContentStyleDelete %>'
                                CommandName="Delete" />
                             <asp:HyperLink ID="lnkCancel" runat="server" Text='<%# Resources.Resource.ContentStyleCancel %>' NavigateUrl='<%# Request.RawUrl %>'></asp:HyperLink>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("Name") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtName" runat="server" CssClass="mediumtextbox" Text='<%# Eval("Name") %>'  MaxLength="100" />
                            <asp:RequiredFieldValidator ID="reqName" runat="server"  ControlToValidate="txtName"  ErrorMessage='<%# Resources.Resource.StyleNameRequiredMessage %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("Element")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtElement" runat="server" CssClass="smalltextbox" Text='<%# Eval("Element") %>'  MaxLength="50" />
                            <asp:RequiredFieldValidator ID="reqElement" runat="server"  ControlToValidate="txtElement"  ErrorMessage='<%# Resources.Resource.StyleElementRequiredMessage %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("CssClass") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCssClass" runat="server" CssClass="smalltextbox" Text='<%# Eval("CssClass") %>'  MaxLength="50" />
                            <asp:RequiredFieldValidator ID="reqCssClass" runat="server"  ControlToValidate="txtCssClass"  ErrorMessage='<%# Resources.Resource.StyleCssClassRequiredMessage %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Eval("IsActive") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%# Eval("IsActive") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                        <p class="nodata"><asp:Literal id="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" /></p>
                </EmptyDataTemplate>
            </mp:mojoGridView>
            <asp:ValidationSummary ID="valSummary" runat="server"  />
            <div class="settingrow">
                <portal:mojoButton ID="btnAddNew" runat="server" />
            </div>
            <div class="settingrow styleimportexportrow">
                <portal:mojoButton ID="btnExportStyles" runat="server" Text="Export Styles" CssClass="btnexportstyles"/>                

                <span class="importcontrols">
                    <portal:jQueryFileUpload ID="uploader" runat="server" CssClass="fileimportstyles" />
                    <portal:mojoButton ID="btnImportStyles" runat="server" Text="Import Styles" CssClass="btnimportstyles"/>
                </span>
            </div>
                <portal:mojoCutePager ID="pgrBottom" runat="server" />
             <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
       </portal:InnerBodyPanel>
       </portal:OuterBodyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
