<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="AdminProductEdit.aspx.cs" Inherits="WebStore.UI.AdminProductEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
        <mp:CornerRounderTop ID="ctop1" runat="server" />
        <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper webstore webstoreproductedit" >
            <portal:HeadingControl ID="heading" runat="server" />
            <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
                <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
                <asp:Panel ID="pnlWrap" runat="server" DefaultButton="btnSave">
                    <div id="divtabs" class="mojo-tabs">
                        <ul>
                            <li class="selected"><a href="#tabSettings">
                                <asp:Literal ID="litSettingsTab" runat="server" /></a></li>
                            <li><a href="#tabAbstract">
                                <asp:Literal ID="litAbstactTab" runat="server" /></a></li>
                            <li><a href="#tabDescription">
                                <asp:Literal ID="litDescriptionTab" runat="server" /></a></li>
                            <li id="liFullfillment" runat="server"><a href="#tabFullfillment" id="lnkFullfillment"
                                runat="server">
                                    <asp:Literal ID="litFullfillmentTab" runat="server" /></a></li>
                            <li><a href="#tabMeta">
                                <asp:Literal ID="litMetaTab" runat="server" /></a></li>
                        </ul>
                        <div id="tabSettings">
                           
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="ProductNameLabel"
                                        ResourceFile="WebStoreResources" />
                                    <asp:TextBox ID="txtName" Columns="70" runat="server" MaxLength="255" CssClass="forminput" />
                                </div>
                                <div class="settingrow" id="divTaxClass" runat="server" visible="false">
                                    <mp:SiteLabel ID="lblTaxClassGuid" runat="server" CssClass="settinglabel" ConfigKey="ProductTaxClassLabel"
                                        ResourceFile="WebStoreResources" />
                                    <asp:DropDownList ID="ddTaxClassGuid" runat="server" DataValueField="Guid" DataTextField="Title"
                                        CssClass="forminput" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="lblModelNumber" runat="server" CssClass="settinglabel" ConfigKey="ProductModelNumberLabel"
                                        ResourceFile="WebStoreResources" />
                                    <asp:TextBox ID="txtModelNumber" Columns="20" runat="server" MaxLength="255" CssClass="forminput" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel8" runat="server" CssClass="settinglabel" ConfigKey="PrimarySortRankLabel"
                                        ResourceFile="WebStoreResources" ForControl="txtSortRank1" />
                                    <asp:TextBox ID="txtSortRank1" runat="server" Text="5000" MaxLength="20" CssClass="forminput smalltextbox" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel9" runat="server" CssClass="settinglabel" ConfigKey="SecondarySortRankLabel"
                                        ResourceFile="WebStoreResources" ForControl="txtSortRank2" />
                                    <asp:TextBox ID="txtSortRank2" runat="server" Text="5000" MaxLength="20" CssClass="forminput smalltextbox" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="lblStatus" runat="server" CssClass="settinglabel" ConfigKey="ProductStatusLabel"
                                        ResourceFile="WebStoreResources" />
                                    <asp:DropDownList ID="ddStatus" runat="server" EnableTheming="false" CssClass="forminput" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="lblWeight" runat="server" CssClass="settinglabel" ConfigKey="ProductWeightLabel"
                                        ResourceFile="WebStoreResources" />
                                    <asp:TextBox ID="txtWeight" Columns="10" runat="server" MaxLength="20" Text="0" CssClass="forminput" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="lblShippingAmount" runat="server" CssClass="settinglabel" ConfigKey="ProductShippingAmountLabel"
                                        ResourceFile="WebStoreResources" />
                                    <asp:TextBox ID="txtShippingAmount" Columns="10" runat="server" MaxLength="20" Text="0" CssClass="forminput" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="lblQuantityOnHand" runat="server" CssClass="settinglabel" ConfigKey="ProductQuantityOnHandLabel"
                                        ResourceFile="WebStoreResources" />
                                    <asp:TextBox ID="txtQuantityOnHand" Columns="10" runat="server" MaxLength="20" Text="0"
                                        CssClass="forminput" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="lblIsVisible" runat="server" CssClass="settinglabel" ConfigKey="ProductShowInListLabel"
                                        ResourceFile="WebStoreResources" ForControl="chkShowInProductList" />
                                    <asp:CheckBox ID="chkShowInProductList" runat="server" CssClass="forminput" />
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="ProductEnableRatingLabel"
                                        ResourceFile="WebStoreResources" ForControl="chkEnableRating" />
                                    <asp:CheckBox ID="chkEnableRating" runat="server" CssClass="forminput" />
                                </div>
                                <div class="settingrow">
                                    &nbsp;</div>
                            
                        </div>
                        <div id="tabAbstract">
                            <div class="settingrow">
                                <mpe:EditorControl ID="edAbstract" runat="server">
                                </mpe:EditorControl>
                            </div>
                        </div>
                        <div id="tabDescription">
                            <div class="settingrow">
                                <mpe:EditorControl ID="edDescription" runat="server">
                                </mpe:EditorControl>
                            </div>
                        </div>
                        <div id="tabFullfillment" runat="server">
                            
                                <div class="settingrow">
                                    <mp:SiteLabel ID="lblFullfillmentType" runat="server" CssClass="settinglabel" ConfigKey="ProductFullfillmentTypeLabel"
                                        ResourceFile="WebStoreResources" />
                                    <asp:DropDownList ID="ddFullfillmentType" EnableTheming="false" runat="server" CssClass="forminput" />
                                    <asp:HyperLink ID="lnkDownload" runat="server" />
                                </div>
                                <asp:Panel ID="pnlUpload" runat="server" Visible="false">
                                    <asp:Panel ID="pnlFileUpload" runat="server"  DefaultButton="btnUpload">
                                        <div class="settingrow">
                                        <mp:SiteLabel ID="Sitelabel3" runat="server" CssClass="settinglabel" ConfigKey="FileUploadLabel"
                                            ResourceFile="WebStoreResources"></mp:SiteLabel>
                                        </div>
                                        <div class="settingrow">
                                       <portal:jQueryFileUpload ID="productUploader" runat="server" />
                                        <portal:mojoButton ID="btnUpload" runat="server" Text="Upload" ValidationGroup="Upload" />
                                        <asp:HiddenField ID="hdnState" Value="images" runat="server" />
                                        </div>
                                    </asp:Panel>
                                    <div class="settingrow">
                                        <mp:SiteLabel ID="SiteLabel10" runat="server" CssClass="settinglabel" ForControl="txtTeaserFileLinkText"
                                            ConfigKey="TeaserFileLinkTextLabel" ResourceFile="WebStoreResources" />
                                        <asp:TextBox ID="txtTeaserFileLinkText" runat="server" CssClass="mediumtextbox forminput" />
                                        <portal:mojoButton ID="btnDeleteTeaser" runat="server" Visible="false" CausesValidation="false" />
                                    </div>
                                    <asp:Panel ID="pnlTeaserUpload" runat="server" DefaultButton="btnUploadTeaser">
                                        <div class="settingrow">
                                        <mp:SiteLabel ID="Sitelabel5" runat="server" CssClass="settinglabel" ConfigKey="TeaserFileUploadLabel"
                                            ResourceFile="WebStoreResources"></mp:SiteLabel>
                                            <asp:HyperLink ID="lnkTeaserDownload" runat="server" Visible="false" />
                                         </div>
                                        <div class="settingrow">
                                        <portal:jQueryFileUpload ID="teaserUploader" runat="server" />
                                        <portal:mojoButton ID="btnUploadTeaser" runat="server" ValidationGroup="Upload" />
                                        </div>
                                        <div>
                                            <mp:SiteLabel ID="Sitelabel11" runat="server" UseLabelTag="false" ConfigKey="TeaserFileInfo"
                                                ResourceFile="WebStoreResources"></mp:SiteLabel>
                                        </div>                                        
                                    </asp:Panel>
                                </asp:Panel>
                                <div class="settingrow">
                                    &nbsp;</div>
                           
                        </div>
                        <div id="tabMeta">
                            
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel6" runat="server" ForControl="txtMetaDescription" CssClass="settinglabel"
                                        ConfigKey="MetaDescriptionLabel" ResourceFile="WebStoreResources"></mp:SiteLabel>
                                    <asp:TextBox ID="txtMetaDescription" runat="server" Columns="50" MaxLength="255"
                                        CssClass="forminput verywidetextbox"></asp:TextBox>
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel7" runat="server" ForControl="txtMetaKeywords" CssClass="settinglabel"
                                        ConfigKey="MetaKeywordsLabel" ResourceFile="WebStoreResources"></mp:SiteLabel>
                                    <asp:TextBox ID="txtMetaKeywords" runat="server" Columns="50" MaxLength="255" CssClass="forminput verywidetextbox"></asp:TextBox>
                                </div>
                                <div class="settingrow">
                                    <mp:SiteLabel ID="lblAdditionalMetaTags" runat="server" CssClass="settinglabel" ConfigKey="MetaAdditionalLabel"
                                        ResourceFile="WebStoreResources"></mp:SiteLabel>
                                    <portal:mojoHelpLink ID="MojoHelpLink25" runat="server" HelpKey="pagesettingsadditionalmetahelp" />
                                </div>
                                <div class="settingrow">
                                    <asp:UpdatePanel ID="updMetaLinks" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <mp:mojoGridView ID="grdMetaLinks" runat="server" CssClass="editgrid" AutoGenerateColumns="false"
                                                DataKeyNames="Guid">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnEditMetaLink" runat="server" CommandName="Edit" Text='<%# Resources.WebStoreResources.ContentMetaGridEditButton %>' />
                                                            <asp:ImageButton ID="btnMoveUpMetaLink" runat="server" ImageUrl="~/Data/SiteImages/up.gif"
                                                                CommandName="MoveUp" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.WebStoreResources.ContentMetaGridMoveUpButton %>'
                                                                Visible='<%# (Convert.ToInt32(Eval("SortRank")) > 3) %>' />
                                                            <asp:ImageButton ID="btnMoveDownMetaLink" runat="server" ImageUrl="~/Data/SiteImages/dn.gif"
                                                                CommandName="MoveDown" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.WebStoreResources.ContentMetaGridMoveDownButton %>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <%# Eval("Rel") %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div class="settingrow">
                                                                <mp:SiteLabel ID="lblNameMetaRel" runat="server" ForControl="txtRel" CssClass="settinglabel"
                                                                    ConfigKey="ContentMetaRelLabel" ResourceFile="WebStoreResources" />
                                                                <asp:TextBox ID="txtRel" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Rel") %>' />
                                                                <asp:RequiredFieldValidator ID="reqMetaName" runat="server" ControlToValidate="txtRel"
                                                                    ErrorMessage='<%# Resources.WebStoreResources.ContentMetaLinkRelRequired %>'
                                                                    ValidationGroup="metalink" />
                                                            </div>
                                                            <div class="settingrow">
                                                                <mp:SiteLabel ID="lblMetaHref" runat="server" ForControl="txtHref" CssClass="settinglabel"
                                                                    ConfigKey="ContentMetaMetaHrefLabel" ResourceFile="WebStoreResources" />
                                                                <asp:TextBox ID="txtHref" CssClass="verywidetextbox forminput" runat="server" Text='<%# Eval("Href") %>' />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtHref"
                                                                    ErrorMessage='<%# Resources.WebStoreResources.ContentMetaLinkHrefRequired %>'
                                                                    ValidationGroup="metalink" />
                                                            </div>
                                                            <div class="settingrow">
                                                                <mp:SiteLabel ID="lblScheme" runat="server" ForControl="txtScheme" CssClass="settinglabel"
                                                                    ConfigKey="ContentMetHrefLangLabel" ResourceFile="Resource" />
                                                                <asp:TextBox ID="txtHrefLang" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("HrefLang") %>' />
                                                            </div>
                                                            <div class="settingrow">
                                                                <asp:Button ID="btnUpdateMetaLink" runat="server" Text='<%# Resources.WebStoreResources.ContentMetaGridUpdateButton %>'
                                                                    CommandName="Update" ValidationGroup="metalink" CausesValidation="true" />
                                                                <asp:Button ID="btnDeleteMetaLink" runat="server" Text='<%# Resources.WebStoreResources.ContentMetaGridDeleteButton %>'
                                                                    CommandName="Delete" CausesValidation="false" />
                                                                <asp:Button ID="btnCancelMetaLink" runat="server" Text='<%# Resources.WebStoreResources.ContentMetaGridCancelButton %>'
                                                                    CommandName="Cancel" CausesValidation="false" />
                                                            </div>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <%# Eval("Href") %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </mp:mojoGridView>
                                            <div class="settingrow">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnAddMetaLink" runat="server" />&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:UpdateProgress ID="prgMetaLinks" runat="server" AssociatedUpdatePanelID="updMetaLinks">
                                                                <ProgressTemplate>
                                                                    <img src='<%= Page.ResolveUrl("~/Data/SiteImages/indicators/indicator1.gif") %>'
                                                                        alt=' ' />
                                                                </ProgressTemplate>
                                                            </asp:UpdateProgress>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="settingrow">
                                    <asp:UpdatePanel ID="upMeta" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <mp:mojoGridView ID="grdContentMeta" runat="server" CssClass="editgrid" AutoGenerateColumns="false"
                                                DataKeyNames="Guid">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnEditMeta" runat="server" CommandName="Edit" Text='<%# Resources.WebStoreResources.ContentMetaGridEditButton %>' />
                                                            <asp:ImageButton ID="btnMoveUpMeta" runat="server" ImageUrl="~/Data/SiteImages/up.gif"
                                                                CommandName="MoveUp" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.WebStoreResources.ContentMetaGridMoveUpButton %>'
                                                                Visible='<%# (Convert.ToInt32(Eval("SortRank")) > 3) %>' />
                                                            <asp:ImageButton ID="btnMoveDownMeta" runat="server" ImageUrl="~/Data/SiteImages/dn.gif"
                                                                CommandName="MoveDown" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.WebStoreResources.ContentMetaGridMoveDownButton %>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <%# Eval("Name") %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div class="settingrow">
                                                                <mp:SiteLabel ID="lblName" runat="server" ForControl="txtName" CssClass="settinglabel"
                                                                    ConfigKey="ContentMetaNameLabel" ResourceFile="WebStoreResources" />
                                                                <asp:TextBox ID="txtName" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Name") %>' />
                                                                <asp:RequiredFieldValidator ID="reqMetaName" runat="server" ControlToValidate="txtName"
                                                                    ErrorMessage='<%# Resources.WebStoreResources.ContentMetaNameRequired %>' ValidationGroup="meta" />
                                                            </div>
                                                            <div class="settingrow">
                                                                <mp:SiteLabel ID="lblMetaContent" runat="server" ForControl="txtMetaContent" CssClass="settinglabel"
                                                                    ConfigKey="ContentMetaMetaContentLabel" ResourceFile="WebStoreResources" />
                                                                <asp:TextBox ID="txtMetaContent" CssClass="verywidetextbox forminput" runat="server"
                                                                    Text='<%# Eval("MetaContent") %>' />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                                                    ErrorMessage='<%# Resources.WebStoreResources.ContentMetaContentRequired %>'
                                                                    ValidationGroup="meta" />
                                                            </div>
                                                            <div class="settingrow">
                                                                <mp:SiteLabel ID="lblScheme" runat="server" ForControl="txtScheme" CssClass="settinglabel"
                                                                    ConfigKey="ContentMetaSchemeLabel" ResourceFile="WebStoreResources" />
                                                                <asp:TextBox ID="txtScheme" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Scheme") %>' />
                                                            </div>
                                                            <div class="settingrow">
                                                                <mp:SiteLabel ID="lblLangCode" runat="server" ForControl="txtLangCode" CssClass="settinglabel"
                                                                    ConfigKey="ContentMetaLangCodeLabel" ResourceFile="WebStoreResources" />
                                                                <asp:TextBox ID="txtLangCode" CssClass="smalltextbox forminput" runat="server" Text='<%# Eval("LangCode") %>' />
                                                            </div>
                                                            <div class="settingrow">
                                                                <mp:SiteLabel ID="lblDir" runat="server" ForControl="ddDirection" CssClass="settinglabel"
                                                                    ConfigKey="ContentMetaDirLabel" ResourceFile="WebStoreResources" />
                                                                <asp:DropDownList ID="ddDirection" runat="server" CssClass="forminput">
                                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                                    <asp:ListItem Text="ltr" Value="ltr"></asp:ListItem>
                                                                    <asp:ListItem Text="rtl" Value="rtl"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="settingrow">
                                                                <asp:Button ID="btnUpdateMeta" runat="server" Text='<%# Resources.WebStoreResources.ContentMetaGridUpdateButton %>'
                                                                    CommandName="Update" ValidationGroup="meta" CausesValidation="true" />
                                                                <asp:Button ID="btnDeleteMeta" runat="server" Text='<%# Resources.WebStoreResources.ContentMetaGridDeleteButton %>'
                                                                    CommandName="Delete" CausesValidation="false" />
                                                                <asp:Button ID="btnCancelMeta" runat="server" Text='<%# Resources.WebStoreResources.ContentMetaGridCancelButton %>'
                                                                    CommandName="Cancel" CausesValidation="false" />
                                                            </div>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <%# Eval("MetaContent") %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </mp:mojoGridView>
                                            <div class="settingrow">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnAddMeta" runat="server" />&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:UpdateProgress ID="prgMeta" runat="server" AssociatedUpdatePanelID="upMeta">
                                                                <ProgressTemplate>
                                                                    <img src='<%= Page.ResolveUrl("~/Data/SiteImages/indicators/indicator1.gif") %>'
                                                                        alt=' ' />
                                                                </ProgressTemplate>
                                                            </asp:UpdateProgress>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="settingrow">
                                    &nbsp;</div>
                            
                        </div>
                    </div>
                    <div>
                        <asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="Product" />
                        <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtName"
                            Display="None" ValidationGroup="Product" />
                        <portal:mojoButton ID="btnSave" runat="server" ValidationGroup="Product" />
                        <portal:mojoButton ID="btnDelete" runat="server" CausesValidation="false" />
                    </div>
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
