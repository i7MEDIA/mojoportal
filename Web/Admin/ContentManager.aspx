<%@ Page Language="C#" MaintainScrollPositionOnPostback="true"  MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" CodeBehind="ContentManager.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ContentManagerPage" Title="Untitled Page" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
<asp:HyperLink ID="lnkAdminMenu" runat="server" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
<asp:HyperLink ID="lnkContentManager" runat="server" />
</portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin contentmanager">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<asp:Panel ID="pnlWarning" runat="server" Visible="false">
 <mp:SiteLabel id="SiteLabel1" runat="server" CssClass="txterror warning" ConfigKey="ContentManagerNoReuseWarning" UseLabelTag="false" ></mp:SiteLabel>
 </asp:Panel>
 <div class="settingrow">
 <asp:HyperLink Text="Settings" id="lnkModuleSettings" Visible="False" runat="server" />
	 <portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="contentmanagerpublishpagehelp" />
     <asp:hyperlink id="lnkEdit" cssclass="ModuleEditLink" EnableViewState="false" runat="server" SkinID="plain" />
     &nbsp;<asp:HyperLink ID="lnkBackToList" runat="server" Visible="false" cssclass="ModuleEditLink" SkinID="plain"></asp:HyperLink>
 </div>
<mp:mojoGridView ID="grdPages" runat="server"
     AllowPaging="false"
     AllowSorting="false"
     AutoGenerateColumns="false"
     EnableViewState="true"
     DataKeyNames="PageID">
     <Columns>
        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="left">
            <ItemTemplate>
                <%# Eval("DepthIndicator")%><a href=<%# WebUtils.ResolveUrl(CacheHelper.GetPage(Convert.ToInt32(Eval("PageId"))).Url) %> class="pagelink"><%# Eval("PageName")%></a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="center">
            <ItemTemplate>
                <asp:ImageButton ID="btnEditPublish" runat="server" 
                    Visible='<%# !useDialogForEditing %>'
                    CommandName="Edit" 
                    ToolTip='<%# GetIsPublishedImageAltText(Eval("IsPublished"))%>'
                    AlternateText='<%# GetIsPublishedImageAltText(Eval("IsPublished"))%>'
                    ImageUrl='<%# GetIsPublishedImageUrl(Eval("IsPublished"))%>' />
                <asp:HyperLink ID="lnkPubDialog" runat="server" Visible='<%# useDialogForEditing %>' ImageUrl='<%# GetIsPublishedImageUrl(Eval("IsPublished"))%>'
                    NavigateUrl='<%# SiteRoot + "/Admin/ContentPublishDialog.aspx?pageid=" + Eval("PageId") + "&mid=" + Eval("ModuleId") + "&ia=" + includeAltPanes %>' 
                    CssClass="publink" ToolTip='<%# publishLinkTitle  %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:CheckBox ID="chkPublished" runat="server" Checked='<%# WebUtils.NullToFalse(Eval("IsPublished")) %>' />
                <br />
                <asp:Button id="btnGridUpdate" runat="server" Text='<%# GetUpdateButtonText() %>' CommandName="Update" EnableViewState='<%# !useDialogForEditing %>' />
                <asp:Button id="btnGridCancel" runat="server" Text='<%# GetCancelButtonText() %>' CommandName="Cancel" EnableViewState='<%# !useDialogForEditing %>' />
            
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="center">
            <ItemTemplate>
                <%# GetPaneAlias(Eval("PaneName"))%>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList  ID="ddPaneNames" runat="server" DataTextField="key" DataValueField="value" EnableViewState='<%# !useDialogForEditing %>' 
                DataSource='<%# PaneList() %>' SelectedValue='<%# GetPaneName(Eval("PaneName")) %>'>
                </asp:DropDownList>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="center">
            <ItemTemplate>
                <%# Eval("ModuleOrder")%>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtModuleOrder" Columns="4" Text='<%# GetModuleOrder(Eval("ModuleOrder")) %>' runat="server" EnableViewState='<%# !useDialogForEditing %>' />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="">
            <ItemTemplate>
                <%# GetDisplayBeginDate(Eval("PublishBeginDate"))%>
            </ItemTemplate>
            <EditItemTemplate>
                <mp:DatePickerControl id="dpBeginDate" runat="server" Text='<%# GetBeginDate(Eval("PublishBeginDate")) %>' ShowTime="True" SkinID="ContentManager" EnableViewState='<%# !useDialogForEditing %>'></mp:DatePickerControl>
                <asp:RequiredFieldValidator ID="reqElement" runat="server" ControlToValidate="dpBeginDate" ErrorMessage='<%# Resources.Resource.ContentPublishBeginDateRequiredMessage %>' Display="Dynamic" EnableViewState='<%# !useDialogForEditing %>' />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="">
            <ItemTemplate>
                <%# GetEndDate(Eval("PublishEndDate"))%>
            </ItemTemplate>
            <EditItemTemplate>
                <mp:DatePickerControl id="dpEndDate" runat="server" Text='<%# GetEndDate(Eval("PublishEndDate")) %>' ShowTime="True" SkinID="ContentManager" EnableViewState='<%# !useDialogForEditing %>'></mp:DatePickerControl>
            </EditItemTemplate>
        </asp:TemplateField>
      
     </Columns>
</mp:mojoGridView><br />
<portal:mojoButton  id="btnDelete" runat="server" Text="" CausesValidation="false" />
<asp:HiddenField ID="hdnReturnUrl" runat="server" />
</portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
