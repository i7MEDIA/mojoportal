<%@ Page ValidateRequest="false" Language="c#" CodeBehind="EditPost.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="mojoPortal.Web.BlogUI.BlogEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
    <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper editpage blogedit">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server" SkinID="BlogEdit">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    <asp:Panel ID="pnlBlog" runat="server" DefaultButton="btnUpdate">
            <div id="divtabs" class="mojo-tabs">
                <ul>
                    <li class="selected"><a href="#tabContent">
                        <asp:Literal ID="litContentTab" runat="server" /></a></li>
                    <li id="liExcerpt" runat="server">
                            <asp:Literal ID="litExcerptTab" runat="server" /></li>
                    <li><a href="#tabMeta">
                        <asp:Literal ID="litMetaTab" runat="server" /></a></li>
                    <li><a href="#tabMapSettings">
                        <asp:Literal ID="litMapSettingsTab" runat="server" /></a></li>
                    <li id="liAttachment" runat="server">
                        <asp:Literal ID="litAttachmentsTab" runat="server" /></li>
                    <li id="liGoogleNewsSettigns" runat="server">
                        <asp:Literal ID="litGoogleNewsSettingsTab" runat="server" /></li>
                </ul>
                       
                    <div id="tabContent">
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblTitle" runat="server" ForControl="txtTitle" CssClass="settinglabel"
                                ConfigKey="BlogEditTitleLabel" ResourceFile="BlogResources"></mp:SiteLabel>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="forminput verywidetextbox">
                            </asp:TextBox>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel15" runat="server" ForControl="txtSubTitle" CssClass="settinglabel"
                                ConfigKey="SubTitle" ResourceFile="BlogResources"></mp:SiteLabel>
                            <asp:TextBox ID="txtSubTitle" runat="server" MaxLength="500" CssClass="forminput verywidetextbox">
                            </asp:TextBox>
                        </div>
                        <div class="settingrow">
                            <mpe:EditorControl ID="edContent" runat="server">
                            </mpe:EditorControl>
                        </div>
                        <div id="divUrl" runat="server" class="settingrow">
                            <mp:SiteLabel ID="SiteLabel5" runat="server" ForControl="txtItemUrl" CssClass="settinglabel"
                                ConfigKey="BlogEditItemUrlLabel" ResourceFile="BlogResources"></mp:SiteLabel>
                            <asp:TextBox ID="txtItemUrl" runat="server" MaxLength="255" CssClass="forminput verywidetextbox">
                            </asp:TextBox>
                            <span id="spnUrlWarning" runat="server" style="font-weight: normal; display:none;" class="txterror">
                            </span>
                            <asp:HiddenField ID="hdnTitle" runat="server" />
                        </div>
                        
                        <asp:Panel ID="pnlCategories" runat="server">
                            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div class="settingrow">
                                        <mp:SiteLabel ID="lblCat" runat="server" ForControl="txtCategory" CssClass="settinglabel"
                                            ConfigKey="BlogEditCategoryLabel" ResourceFile="BlogResources"></mp:SiteLabel>
                                        <asp:TextBox ID="txtCategory" runat="server" CssClass="widetextbox forminput"></asp:TextBox>
                                        <portal:mojoButton ID="btnAddCategory" runat="server"  CssClass="forminput" />
                                    </div>
                                    <div class="settingrow blogeditcategories">
                                        <asp:CheckBoxList ID="chkCategories" runat="server" SkinID="Blog"  
                                            RepeatDirection="Horizontal">
                                        </asp:CheckBoxList>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:HyperLink ID="lnkEditCategories" runat="server">
                            </asp:HyperLink>
                            <br />
                        </asp:Panel>
                        <div class="settingrow">
                            <mp:SiteLabel ID="Sitelabel1" runat="server" ForControl="chkIncludeInFeed" ConfigKey="BlogEditIncludeInFeedLabel"
                                ResourceFile="BlogResources" CssClass="settinglabel"></mp:SiteLabel>
                            <asp:CheckBox ID="chkIncludeInFeed" runat="server" CssClass="forminput"></asp:CheckBox>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="Sitelabel25" runat="server" ForControl="chkIncludeInSearchIndex" ConfigKey="IncludeInSearchIndex"
                                ResourceFile="BlogResources" CssClass="settinglabel"></mp:SiteLabel>
                            <asp:CheckBox ID="chkIncludeInSearchIndex" runat="server" CssClass="forminput" Checked="true"></asp:CheckBox>
                        </div>
                        <div id="divExcludeFromRecentContent" runat="server" class="settingrow">
                            <mp:SiteLabel ID="Sitelabel14" runat="server" ForControl="chkExcludeFromRecentContent" CssClass="settinglabel"
                                ConfigKey="ExcludeFromRecentContent"></mp:SiteLabel>
                            <asp:CheckBox ID="chkExcludeFromRecentContent" runat="server" CssClass="forminput"></asp:CheckBox>
                            <portal:mojoHelpLink ID="MojoHelpLink8" runat="server" HelpKey="ExcludeFromRecentContent-help" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="Sitelabel31" runat="server" ForControl="chkIncludeInSiteMap" ConfigKey="IncludeInSiteMap"
                                ResourceFile="BlogResources" CssClass="settinglabel"></mp:SiteLabel>
                            <asp:CheckBox ID="chkIncludeInSiteMap" runat="server" CssClass="forminput" Checked="true"></asp:CheckBox>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="Sitelabel13" runat="server" ForControl="chkIsPublished" ConfigKey="IsPublishedLabel"
                                ResourceFile="BlogResources" CssClass="settinglabel"></mp:SiteLabel>
                            <asp:CheckBox ID="chkIsPublished" runat="server" CssClass="forminput" Checked="true">
                            </asp:CheckBox>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="Sitelabel27" runat="server" ForControl="chkShowAuthorName" ConfigKey="ShowPostAuthorSetting"
                                ResourceFile="BlogResources" CssClass="settinglabel"></mp:SiteLabel>
                            <asp:CheckBox ID="chkShowAuthorName" runat="server" CssClass="forminput" Checked="true">
                            </asp:CheckBox>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="Sitelabel28" runat="server" ForControl="chkShowAuthorAvatar" ConfigKey="ShowAuthorPhoto"
                                ResourceFile="BlogResources" CssClass="settinglabel"></mp:SiteLabel>
                            <asp:CheckBox ID="chkShowAuthorAvatar" runat="server" CssClass="forminput" Checked="true">
                            </asp:CheckBox>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="Sitelabel29" runat="server" ForControl="chkShowAuthorBio" ConfigKey="ShowAuthorBio"
                                ResourceFile="BlogResources" CssClass="settinglabel"></mp:SiteLabel>
                            <asp:CheckBox ID="chkShowAuthorBio" runat="server" CssClass="forminput" Checked="true">
                            </asp:CheckBox>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblStartDate" runat="server"  ConfigKey="BlogEditStartDateLabel"
                                ResourceFile="BlogResources" CssClass="settinglabel"></mp:SiteLabel>
                            <mp:DatePickerControl ID="dpBeginDate" runat="server" ShowTime="True" SkinID="blog"  CssClass="forminput">
                            </mp:DatePickerControl>
                            <mp:SiteLabel ID="SiteLabel3" runat="server" ResourceFile="BlogResources" ConfigKey="BlogDraftInstructions"
                                UseLabelTag="false" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel16" runat="server"  ConfigKey="EndDate"
                                ResourceFile="BlogResources" CssClass="settinglabel"></mp:SiteLabel>
                            <mp:DatePickerControl ID="dpEndDate" runat="server" ShowTime="True" SkinID="blog"  CssClass="forminput">
                            </mp:DatePickerControl>
                            
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="Sitelabel2" runat="server" ForControl="ddCommentAllowedForDays"
                                ConfigKey="BlogEditAllowedCommentsForDaysPrefix" ResourceFile="BlogResources"
                                CssClass="settinglabel"></mp:SiteLabel>
                            <asp:DropDownList ID="ddCommentAllowedForDays" EnableTheming="false" runat="server"
                                CssClass="forminput">
                                <asp:ListItem Value="-1" Text="<%$ Resources:BlogResources, BlogCommentsNotAllowed %>" />
                                <asp:ListItem Value="0" Text="<%$ Resources:BlogResources, BlogCommentsUnlimited %>" />
                                <asp:ListItem Value="1" Text="1" />
                                <asp:ListItem Value="7" Text="7" />
                                <asp:ListItem Value="15" Text="15" />
                                <asp:ListItem Value="30" Text="30" />
                                <asp:ListItem Value="45" Text="45" />
                                <asp:ListItem Value="60" Text="60" />
                                <asp:ListItem Value="90" Text="90" Selected="True" />
                                <asp:ListItem Value="120" Text="120" />
                            </asp:DropDownList>
                            &nbsp;<asp:Literal ID="litDays" runat="server" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                            <div class="forminput">
                                <portal:mojoButton ID="btnUpdate" runat="server" ValidationGroup="blog" />
                                <portal:mojoButton ID="btnSaveAndPreview" runat="server" ValidationGroup="blog" Visible="false" />&nbsp;
                                <portal:mojoButton ID="btnDelete" runat="server" Text="Delete this item" CausesValidation="false" />
                                <asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" />&nbsp;
                            </div>
                            <br />
                            <portal:mojoLabel ID="lblError" runat="server" CssClass="txterror" />
                            <asp:HiddenField ID="hdnHxToRestore" runat="server" />
                            <asp:ImageButton ID="btnRestoreFromGreyBox" runat="server" />
                        </div>
                               
                            <div class="bloghistory">
                                <asp:UpdatePanel ID="updHx" UpdateMode="Conditional" runat="server">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="grdHistory" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlHistory" runat="server" Visible="false">
                                        <div class="settingrow">
                                            <mp:SiteLabel ID="SiteLabel10" runat="server" CssClass="settinglabel" ConfigKey="VersionHistory"
                                                ResourceFile="BlogResources"></mp:SiteLabel>
                                        </div>
                                        <div class="settingrow">
                                        <mp:mojoGridView ID="grdHistory" runat="server" CssClass="editgrid" AutoGenerateColumns="false"
                                            DataKeyNames="Guid">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("CreatedUtc"), timeOffset)%>
                                                        <br />
                                                        <%# Eval("UserName") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("HistoryUtc"), timeOffset)%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:HyperLink id="lnkcompare" runat="server" CssClass="cblink"
                                                            NavigateUrl='<%# SiteRoot + "/Blog/BlogCompare.aspx?pageid=" + pageId + "&mid=" + moduleId + "&ItemID=" + itemId + "&h=" + Eval("Guid") %>'
                                                            Text='<%# Resources.BlogResources.CompareHistoryToCurrentLink %>' ToolTip='<%# Resources.BlogResources.CompareHistoryToCurrentLink %>'
                                                            DialogCloseText='<%# Resources.BlogResources.DialogCloseLink %>' />
                                                        <asp:Button ID="btnRestoreToEditor" runat="server" Text='<%# Resources.BlogResources.RestoreToEditorButton %>'
                                                            CommandName="RestoreToEditor" CommandArgument='<%# Eval("Guid") %>' />
                                                        <asp:Button ID="btnDelete" runat="server" CommandName="DeleteHistory" CommandArgument='<%# Eval("Guid") %>'
                                                            Visible='<%# isAdmin %>' Text='<%# Resources.BlogResources.DeleteHistoryButton %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </mp:mojoGridView>
                                        </div>
                                        <portal:mojoCutePager ID="pgrHistory" runat="server" />
                                        <div id="divHistoryDelete" runat="server" class="settingrow">
                                            <mp:SiteLabel ID="SiteLabel8" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                                            <portal:mojoButton ID="btnDeleteHistory" runat="server" />
                                        </div>
                                            </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                               
                        <div class="settingrow">
                            &nbsp;</div>
                    </div>
                    <div id="tabExcerpt" runat="server">
                        <div class="settingrow">
                            <mpe:EditorControl ID="edExcerpt" runat="server">
                            </mpe:EditorControl>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel11" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                            <div class="forminput">
                                <portal:mojoButton ID="btnUpdate2" runat="server" ValidationGroup="blog" />&nbsp;
                                <portal:mojoButton ID="btnDelete2" runat="server" CausesValidation="false" />
                                <asp:HyperLink ID="lnkCancel2" runat="server" CssClass="cancellink" />&nbsp;
                            </div>
                        </div>
                        <div class="settingrow">
                            <portal:mojoLabel ID="lblErrorMessage" runat="server" CssClass="txterror" /> &nbsp;</div>
                    </div>
                    <div id="tabMeta">
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel6" runat="server" ForControl="txtMetaDescription" CssClass="settinglabel"
                                ConfigKey="MetaDescriptionLabel" ResourceFile="BlogResources"></mp:SiteLabel>
                            <asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="255" CssClass="forminput verywidetextbox">
                            </asp:TextBox>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel7" runat="server" ForControl="txtMetaKeywords" CssClass="settinglabel"
                                ConfigKey="MetaKeywordsLabel" ResourceFile="BlogResources"></mp:SiteLabel>
                            <asp:TextBox ID="txtMetaKeywords" runat="server" MaxLength="255" CssClass="forminput verywidetextbox">
                            </asp:TextBox>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel12" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                            <div class="forminput">
                                <portal:mojoButton ID="btnUpdate3" runat="server" ValidationGroup="blog" />&nbsp;
                                <portal:mojoButton ID="btnDelete3" runat="server" CausesValidation="False" />
                                <asp:HyperLink ID="lnkCancel3" runat="server" CssClass="cancellink" />&nbsp;
                            </div>
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="lblAdditionalMetaTags" runat="server" CssClass="settinglabel" ConfigKey="MetaAdditionalLabel"
                                ResourceFile="BlogResources"></mp:SiteLabel>
                            <portal:mojoHelpLink ID="MojoHelpLink25" runat="server" HelpKey="pagesettingsadditionalmetahelp" />
                        </div>
                        <asp:Panel ID="pnlMetaData" runat="server" CssClass="settingrow">
                            <asp:UpdatePanel ID="updMetaLinks" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <mp:mojoGridView ID="grdMetaLinks" runat="server" CssClass="editgrid" AutoGenerateColumns="false"
                                        DataKeyNames="Guid">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button ID="btnEditMetaLink" runat="server" CommandName="Edit" Text='<%# Resources.BlogResources.ContentMetaGridEditButton %>' />
                                                    <asp:ImageButton ID="btnMoveUpMetaLink" runat="server" ImageUrl='<%= Page.ResolveUrl("~/Data/SiteImages/up.gif") %>'
                                                        CommandName="MoveUp" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.BlogResources.ContentMetaGridMoveUpButton %>'
                                                        Visible='<%# (Convert.ToInt32(Eval("SortRank")) > 3) %>' />
                                                    <asp:ImageButton ID="btnMoveDownMetaLink" runat="server" ImageUrl='<%= Page.ResolveUrl("~/Data/SiteImages/dn.gif") %>'
                                                        CommandName="MoveDown" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.BlogResources.ContentMetaGridMoveDownButton %>' />
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
                                                            ConfigKey="ContentMetaRelLabel" ResourceFile="BlogResources" />
                                                        <asp:TextBox ID="txtRel" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Rel") %>' />
                                                        <asp:RequiredFieldValidator ID="reqMetaName" runat="server" ControlToValidate="txtRel"
                                                            ErrorMessage='<%# Resources.BlogResources.ContentMetaLinkRelRequired %>' ValidationGroup="metalink" />
                                                    </div>
                                                    <div class="settingrow">
                                                        <mp:SiteLabel ID="lblMetaHref" runat="server" ForControl="txtHref" CssClass="settinglabel"
                                                            ConfigKey="ContentMetaMetaHrefLabel" ResourceFile="BlogResources" />
                                                        <asp:TextBox ID="txtHref" CssClass="verywidetextbox forminput" runat="server" Text='<%# Eval("Href") %>' />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtHref"
                                                            ErrorMessage='<%# Resources.BlogResources.ContentMetaLinkHrefRequired %>' ValidationGroup="metalink" />
                                                    </div>
                                                    <div class="settingrow">
                                                        <mp:SiteLabel ID="lblScheme" runat="server" ForControl="txtScheme" CssClass="settinglabel"
                                                            ConfigKey="ContentMetHrefLangLabel" ResourceFile="BlogResources" />
                                                        <asp:TextBox ID="txtHrefLang" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("HrefLang") %>' />
                                                    </div>
                                                    <div class="settingrow">
                                                        <asp:Button ID="btnUpdateMetaLink" runat="server" Text='<%# Resources.BlogResources.ContentMetaGridUpdateButton %>'
                                                            CommandName="Update" ValidationGroup="metalink" CausesValidation="true" />
                                                        <asp:Button ID="btnDeleteMetaLink" runat="server" Text='<%# Resources.BlogResources.ContentMetaGridDeleteButton %>'
                                                            CommandName="Delete" CausesValidation="false" />
                                                        <asp:Button ID="btnCancelMetaLink" runat="server" Text='<%# Resources.BlogResources.ContentMetaGridCancelButton %>'
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
                                                    <portal:mojoButton ID="btnAddMetaLink" runat="server" />&nbsp;
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
                            <div class="settingrow">
                                <asp:UpdatePanel ID="upMeta" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <mp:mojoGridView ID="grdContentMeta" runat="server" CssClass="editgrid" AutoGenerateColumns="false"
                                            DataKeyNames="Guid">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnEditMeta" runat="server" CommandName="Edit" Text='<%# Resources.BlogResources.ContentMetaGridEditButton %>' />
                                                        <asp:ImageButton ID="btnMoveUpMeta" runat="server" ImageUrl="~/Data/SiteImages/up.gif"
                                                            CommandName="MoveUp" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.BlogResources.ContentMetaGridMoveUpButton %>'
                                                            Visible='<%# (Convert.ToInt32(Eval("SortRank")) > 3) %>' />
                                                        <asp:ImageButton ID="btnMoveDownMeta" runat="server" ImageUrl="~/Data/SiteImages/dn.gif"
                                                            CommandName="MoveDown" CommandArgument='<%# Eval("Guid") %>' AlternateText='<%# Resources.BlogResources.ContentMetaGridMoveDownButton %>' />
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
                                                                ConfigKey="ContentMetaNameLabel" ResourceFile="BlogResources" />
                                                            <asp:TextBox ID="txtName" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Name") %>' />
                                                            <asp:RequiredFieldValidator ID="reqMetaName" runat="server" ControlToValidate="txtName"
                                                                ErrorMessage='<%# Resources.BlogResources.ContentMetaNameRequired %>' ValidationGroup="meta" />
                                                        </div>
                                                        <div class="settingrow">
                                                            <mp:SiteLabel ID="lblMetaContent" runat="server" ForControl="txtMetaContent" CssClass="settinglabel"
                                                                ConfigKey="ContentMetaMetaContentLabel" ResourceFile="BlogResources" />
                                                            <asp:TextBox ID="txtMetaContent" CssClass="verywidetextbox forminput" runat="server"
                                                                Text='<%# Eval("MetaContent") %>' />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                                                ErrorMessage='<%# Resources.BlogResources.ContentMetaContentRequired %>' ValidationGroup="meta" />
                                                        </div>
                                                        <div class="settingrow">
                                                            <mp:SiteLabel ID="lblScheme" runat="server" ForControl="txtScheme" CssClass="settinglabel"
                                                                ConfigKey="ContentMetaSchemeLabel" ResourceFile="BlogResources" />
                                                            <asp:TextBox ID="txtScheme" CssClass="widetextbox forminput" runat="server" Text='<%# Eval("Scheme") %>' />
                                                        </div>
                                                        <div class="settingrow">
                                                            <mp:SiteLabel ID="lblLangCode" runat="server" ForControl="txtLangCode" CssClass="settinglabel"
                                                                ConfigKey="ContentMetaLangCodeLabel" ResourceFile="BlogResources" />
                                                            <asp:TextBox ID="txtLangCode" CssClass="smalltextbox forminput" runat="server" Text='<%# Eval("LangCode") %>' />
                                                        </div>
                                                        <div class="settingrow">
                                                            <mp:SiteLabel ID="lblDir" runat="server" ForControl="ddDirection" CssClass="settinglabel"
                                                                ConfigKey="ContentMetaDirLabel" ResourceFile="BlogResources" />
                                                            <asp:DropDownList ID="ddDirection" runat="server" CssClass="forminput">
                                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                                                <asp:ListItem Text="ltr" Value="ltr"></asp:ListItem>
                                                                <asp:ListItem Text="rtl" Value="rtl"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="settingrow">
                                                            <asp:Button ID="btnUpdateMeta" runat="server" Text='<%# Resources.BlogResources.ContentMetaGridUpdateButton %>'
                                                                CommandName="Update" ValidationGroup="meta" CausesValidation="true" />
                                                            <asp:Button ID="btnDeleteMeta" runat="server" Text='<%# Resources.BlogResources.ContentMetaGridDeleteButton %>'
                                                                CommandName="Delete" CausesValidation="false" />
                                                            <asp:Button ID="btnCancelMeta" runat="server" Text='<%# Resources.BlogResources.ContentMetaGridCancelButton %>'
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
                                                        <portal:mojoButton ID="btnAddMeta" runat="server" />&nbsp;
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
                                <div class="settingrow">
                                    <mp:SiteLabel ID="SiteLabel9" runat="server" CssClass="settinglabel" ConfigKey="spacer">
                                    </mp:SiteLabel>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>

                    <div id="tabMapSettings">
                    <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel4" runat="server" ForControl="txtLocation" CssClass="settinglabel"
                                ConfigKey="BlogEditLocationLabel" ResourceFile="BlogResources"></mp:SiteLabel>
                            <asp:TextBox ID="txtLocation" runat="server" MaxLength="300" CssClass="forminput widetextbox">
                            </asp:TextBox>
                        </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel17" runat="server" ForControl="chkUseBing" CssClass="settinglabel"
                            ResourceFile="BlogResources" ConfigKey="UseBingMap"></mp:SiteLabel>
                        <asp:CheckBox ID="chkUseBing" runat="server" Checked="false" CssClass="forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel18" runat="server" CssClass="settinglabel" ConfigKey="GoogleMapInitialMapTypeSetting"
                            ResourceFile="BlogResources"></mp:SiteLabel>
                        <portal:GMapTypeSetting ID="MapTypeControl" runat="server" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel19" runat="server" CssClass="settinglabel" ConfigKey="GoogleMapInitialZoomSetting"
                            ResourceFile="BlogResources"></mp:SiteLabel>
                        <portal:GMapZoomLevelSetting ID="ZoomLevelControl" runat="server" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel21" runat="server" ForControl="txtMapHeight" CssClass="settinglabel"
                            ConfigKey="GoogleMapHeightSetting" ResourceFile="BlogResources"></mp:SiteLabel>
                        <asp:TextBox ID="txtMapHeight" runat="server" Text="300"  MaxLength="4" CssClass="forminput smalltextbox">
                        </asp:TextBox>
                        <asp:RegularExpressionValidator ID="regexMapHeight" runat="server" ControlToValidate="txtMapHeight"
                            Display="None" ValidationExpression="^[1-9][0-9]{0,4}$" ValidationGroup="blog" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel20" runat="server" ForControl="txtMapWidth" CssClass="settinglabel"
                            ConfigKey="GoogleMapWidthSetting" ResourceFile="BlogResources"></mp:SiteLabel>
                        <asp:TextBox ID="txtMapWidth" runat="server" Text="500" MaxLength="6" CssClass="forminput smalltextbox">
                        </asp:TextBox>
                        <asp:RegularExpressionValidator ID="regexMapWidth" runat="server" ControlToValidate="txtMapWidth"
                            Display="None" ValidationExpression="^[1-9][0-9]{0,4}[px|%]?" ValidationGroup="blog" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel22" runat="server" ForControl="chkShowMapOptions" CssClass="settinglabel"
                            ResourceFile="BlogResources" ConfigKey="GoogleMapEnableMapTypeSetting"></mp:SiteLabel>
                        <asp:CheckBox ID="chkShowMapOptions" runat="server" Checked="true" CssClass="forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel23" runat="server" ForControl="chkShowMapZoom" CssClass="settinglabel"
                            ResourceFile="BlogResources" ConfigKey="GoogleMapEnableZoomSetting"></mp:SiteLabel>
                        <asp:CheckBox ID="chkShowMapZoom" runat="server" Checked="true" CssClass="forminput" />
                    </div>
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel24" runat="server" ForControl="chkShowMapBalloon" CssClass="settinglabel"
                            ResourceFile="BlogResources" ConfigKey="GoogleMapShowInfoWindowSetting"></mp:SiteLabel>
                        <asp:CheckBox ID="chkShowMapBalloon" runat="server" Checked="false" CssClass="forminput" />
                    </div>
                   
                    <div class="settingrow">
                        <mp:SiteLabel ID="SiteLabel26" runat="server" ForControl="chkShowMapDirections" CssClass="settinglabel"
                            ResourceFile="BlogResources" ConfigKey="GoogleMapEnableDirectionsSetting"></mp:SiteLabel>
                        <asp:CheckBox ID="chkShowMapDirections" runat="server" Checked="true" CssClass="forminput" />
                    </div>
                    <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel32" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                            <div class="forminput">
                                <portal:mojoButton ID="btnUpdate4" runat="server" ValidationGroup="blog" />&nbsp;
                                <portal:mojoButton ID="btnDelete4" runat="server" CausesValidation="false" />
                                <asp:HyperLink ID="HyperLink1" runat="server" CssClass="cancellink" />&nbsp;
                            </div>
                        </div>
                </div>

                    <div id="tabAttachments" runat="server">
                        <div class="settingrow">
                                <mp:mojoGridView ID="grdAttachments" runat="server" CssClass="" AutoGenerateColumns="false"
                                    DataKeyNames="RowGuid">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <%# Eval("FileName") %>
                                                 <asp:HyperLink ID="lnkDownload" runat="server" EnableViewState="false" 
                                                    ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/Download.gif" %>' 
                                                     NavigateUrl='<%# upLoadPath + Eval("ServerFileName") %>' 
                                                     ToolTip='<%# Resources.BlogResources.Download %>' />
                                                <asp:Button ID="btnDelete" runat="server" CommandName="delete" CommandArgument='<%# Eval("RowGuid") %>'
                                                    Text='<%# Resources.BlogResources.DeleteImageAltText %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </mp:mojoGridView>
                            </div>
                            <asp:Panel ID="pnlUpload" runat="server" CssClass="settingrow" DefaultButton="btnUpload">
                                <portal:jQueryFileUpload ID="uploader" runat="server" />
                                <portal:mojoButton ID="btnUpload" runat="server"  CausesValidation="false" EnableViewState="false" />
                                <asp:HiddenField ID="hdnState" Value="images" runat="server" />
                                <asp:Literal ID="litAttachmentWarning" runat="server" EnableViewState="false" />
                                
                            </asp:Panel>
                        <div class="settingrow">
                            <mp:SiteLabel ID="Sitelabel30" runat="server" ForControl="chkShowDownloadLink" ConfigKey="IncludeDownloadLinkForMediaAttachments"
                                ResourceFile="BlogResources" CssClass="settinglabel"></mp:SiteLabel>
                            <asp:CheckBox ID="chkShowDownloadLink" runat="server" CssClass="forminput">
                            </asp:CheckBox>
                        </div>

                    </div>

                <div id="divTabGoogleNews" runat="server">
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel34" runat="server" ForControl="chkIncludeInNews" CssClass="settinglabel"
                                ResourceFile="BlogResources" ConfigKey="IncludeInGoogleNews"></mp:SiteLabel>
                            <asp:CheckBox ID="chkIncludeInNews" runat="server" Checked="true" CssClass="forminput" />
                            <portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="Blog-IncludeInGoogleNews-help" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel33" runat="server" ForControl="txtPublicationName" CssClass="settinglabel"
                                ConfigKey="PublicationName" ResourceFile="BlogResources"></mp:SiteLabel>
                            <asp:TextBox ID="txtPublicationName" runat="server" MaxLength="255" CssClass="forminput verywidetextbox">
                            </asp:TextBox>
                            <portal:mojoHelpLink ID="MojoHelpLink2" runat="server" HelpKey="Blog-PublicationName-help" />
                        </div>
                         <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel36" runat="server" ForControl="txtPubLanguage" CssClass="settinglabel"
                                ConfigKey="PublicationLanguage" ResourceFile="BlogResources"></mp:SiteLabel>
                            <asp:TextBox ID="txtPubLanguage" runat="server" MaxLength="7" CssClass="forminput smalltextbox">
                            </asp:TextBox>
                             <portal:mojoHelpLink ID="MojoHelpLink3" runat="server" HelpKey="Blog-PublicationLanguage-help" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel37" runat="server" ForControl="txtPubGenres" CssClass="settinglabel"
                                ConfigKey="PublicationGenres" ResourceFile="BlogResources"></mp:SiteLabel>
                            <asp:TextBox ID="txtPubGenres" runat="server" MaxLength="255" CssClass="forminput verywidetextbox">
                            </asp:TextBox>
                            <portal:mojoHelpLink ID="MojoHelpLink4" runat="server" HelpKey="Blog-PublicationGenres-help" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel38" runat="server" ForControl="txtPubKeyWords" CssClass="settinglabel"
                                ConfigKey="PublicationKeyWords" ResourceFile="BlogResources"></mp:SiteLabel>
                            <asp:TextBox ID="txtPubKeyWords" runat="server" MaxLength="255" CssClass="forminput verywidetextbox">
                            </asp:TextBox>
                            <portal:mojoHelpLink ID="MojoHelpLink5" runat="server" HelpKey="Blog-PublicationKeyWords-help" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel39" runat="server" ForControl="txtPubGeoLocations" CssClass="settinglabel"
                                ConfigKey="PublicationGeoLocation" ResourceFile="BlogResources"></mp:SiteLabel>
                            <asp:TextBox ID="txtPubGeoLocations" runat="server" MaxLength="255" CssClass="forminput verywidetextbox">
                            </asp:TextBox>
                            <portal:mojoHelpLink ID="MojoHelpLink6" runat="server" HelpKey="Blog-PublicationGeoLocation-help" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel40" runat="server" ForControl="txtPubStockTickers" CssClass="settinglabel"
                                ConfigKey="PublicationStockTickers" ResourceFile="BlogResources"></mp:SiteLabel>
                            <asp:TextBox ID="txtPubStockTickers" runat="server" MaxLength="255" CssClass="forminput verywidetextbox">
                            </asp:TextBox>
                            <portal:mojoHelpLink ID="MojoHelpLink7" runat="server" HelpKey="Blog-PublicationStockTickers-help" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel42" runat="server" ForControl="chkIncludeImageInExcerpt" CssClass="settinglabel"
                                ResourceFile="BlogResources" ConfigKey="IncludeImageInExcerpt"></mp:SiteLabel>
                            <asp:CheckBox ID="chkIncludeImageInExcerpt" runat="server" Checked="true" CssClass="forminput" />
                             <portal:mojoHelpLink ID="MojoHelpLink20" runat="server" HelpKey="Blog-IncludeImageInExcerpt-help" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel41" runat="server" ForControl="txtHeadlineImage" CssClass="settinglabel"
                                ConfigKey="HeadlineImage" ResourceFile="BlogResources"></mp:SiteLabel>
                            <asp:TextBox ID="txtHeadlineImage" runat="server" MaxLength="255" CssClass="forminput verywidetextbox" />
                            <portal:mojoHelpLink ID="MojoHelpLink9" runat="server" HelpKey="Blog-HeadlineImage-help" />
                            <portal:FileBrowserTextBoxExtender ID="fbHeadlineImage" runat="server" BrowserType="image"  />
                        </div>
                        <div class="settingrow">
                            <asp:Image ID="imgPreview" runat="server" ImageUrl="~/Data/SiteImages/1x1.gif" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel43" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
                            <div class="forminput">
                                <portal:mojoButton ID="btnUpdate5" runat="server" ValidationGroup="blog" />&nbsp;
                                <portal:mojoButton ID="btnDelete5" runat="server" CausesValidation="false" />
                                <asp:HyperLink ID="lnkCancel5" runat="server" CssClass="cancellink" />&nbsp;
                            </div>
                        </div>

                 </div>


                </div>
                    
                
        <div class="blogeditor">
            <div class="settingrow">
                <asp:RequiredFieldValidator ID="reqTitle" runat="server" ControlToValidate="txtTitle"
                    Display="None" CssClass="txterror" ValidationGroup="blog">
                </asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="reqStartDate" runat="server" ControlToValidate="dpBeginDate"
                    Display="None" CssClass="txterror" ValidationGroup="blog">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="regexUrl" runat="server" ControlToValidate="txtItemUrl"
                    ValidationExpression="((~/){1}\S+)" Display="None" ValidationGroup="blog" />
                <asp:ValidationSummary ID="vSummary" runat="server" CssClass="txterror" ValidationGroup="blog">
                </asp:ValidationSummary>
            </div>
        </div>
         
        <asp:HiddenField ID="hdnReturnUrl" runat="server" />
    </asp:Panel>

    </portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
    <portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
