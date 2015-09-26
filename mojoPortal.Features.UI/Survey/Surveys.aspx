<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Surveys.aspx.cs" Inherits="SurveyFeature.UI.SurveysPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
    <asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkSurveys" CssClass="selectedcrumb"></asp:HyperLink>
</div>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper survey">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    <mp:mojoGridView ID="grdSurveys" runat="server" 
        AllowPaging="false" 
        AllowSorting="false"
        AutoGenerateColumns="false" 
        CssClass="" 
        DataKeyNames="SurveyGuid">
     <Columns>
		<asp:TemplateField>
        <ItemTemplate>
                <asp:HyperLink id="editLink" 
						    Text="<%# Resources.SurveyResources.SurveysGridEditButton %>" 
						    Tooltip="<%# Resources.SurveyResources.SurveysGridEditButtonToolTip %>"
						    ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>' 
						    NavigateUrl='<%# SiteRoot + "/Survey/SurveyEdit.aspx?SurveyGuid=" + Eval("SurveyGuid") + "&pageid=" + PageId + "&mid=" + ModuleId%>' 
						    runat="server" />
                        <asp:ImageButton ID="btnDelete" runat="server" 
                            CommandName="delete" ToolTip='<%# Resources.SurveyResources.SurveysGridDeleteButtonToolTip %>'
                            CommandArgument='<%# Eval("SurveyGuid") %>'
                            AlternateText='<%# Resources.SurveyResources.SurveysGridDeleteButton %>'
                            ImageUrl='<%# DeleteLinkImage %>' />	
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                 <%# Eval("SurveyName") %>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                 <%# Eval("CreationDate") %>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
               <asp:HyperLink id="pagesLink"
                            Text='<%# Eval("PageCount") %>'
						    Tooltip="<%# Resources.SurveyResources.SurveysGridPageCountToolTip %>"
						    NavigateUrl='<%# SiteRoot + "/Survey/SurveyPages.aspx?SurveyGuid=" + Eval("SurveyGuid") + "&pageid=" + PageId + "&mid=" + ModuleId %>' 
						    runat="server" />
						    <asp:HyperLink id="HyperLink1"
						    Text="<%# Resources.SurveyResources.AddEditGridLink %>"
						    Tooltip="<%# Resources.SurveyResources.AddEditGridLink %>"
						    NavigateUrl='<%# SiteRoot + "/Survey/SurveyPages.aspx?SurveyGuid=" + Eval("SurveyGuid") + "&pageid=" + PageId + "&mid=" + ModuleId %>' 
						    runat="server" />&nbsp;&nbsp;
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
               <asp:Button ID="lnkAddRemoveFromModule" runat="server" CssClass="buttonlink" CommandArgument='<%# Eval("SurveyGuid") %>' />
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                 <asp:Label ID="lblResponseCount" runat="server" Text='<%# Eval("ResponseCount") %>' Visible='<%# (Convert.ToInt32(Eval("ResponseCount")) == 0) %>'></asp:Label>
                        <asp:HyperLink
                            ID="lnkResults"
                            Text='<%# Eval("ResponseCount") %>' 
                            Visible='<%# (Convert.ToInt32(Eval("ResponseCount")) > 0) %>' 
                            CommandArgument='<%# Eval("SurveyGuid") %>'
                            runat="server" 
                            ToolTip='<%# Resources.SurveyResources.SurveyGridResponseCountLinkToolTip %>'
                            NavigateUrl='<%# SiteRoot + "/Survey/Results.aspx?SurveyGuid=" + Eval("SurveyGuid") + "&pageid=" + PageId + "&mid=" + ModuleId %>'/>
                            &nbsp;&nbsp;
                        <asp:HyperLink
                            ID="HyperLink2" 
                            Visible='<%# (Convert.ToInt32(Eval("ResponseCount")) > 0) %>' 
                            CommandArgument='<%# Eval("SurveyGuid") %>'
                            runat="server" 
                            Text='<%# Resources.SurveyResources.ViewResponsesLink %>'
                            NavigateUrl='<%# SiteRoot + "/Survey/Results.aspx?SurveyGuid=" + Eval("SurveyGuid") + "&pageid=" + PageId + "&mid=" + ModuleId %>'/>
                        &nbsp;&nbsp;
                        <asp:ImageButton ID="btnExport" runat="server" Visible='<%# (Convert.ToInt32(Eval("ResponseCount")) > 0) %>'
                            CommandName="export" ToolTip='<%# Resources.SurveyResources.SurveyGridExportResponsesToolTip %>'
                            CommandArgument='<%# Eval("SurveyGuid") %>'
                            AlternateText='<%# Resources.SurveyResources.SurveyGridExportResponsesAlternateText %>'
                            ImageUrl="~/Data/SiteImages/download.gif"/>	 
                        &nbsp;&nbsp;
            </ItemTemplate>
		</asp:TemplateField>
</Columns>
</mp:mojoGridView>
<div class="modulepager" >
<asp:HyperLink ID="lnkAddNew" runat="server" />
</div>
<portal:EmptyPanel id="divCleared1" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
<asp:Label id="lblMessages" runat="server" EnableViewState="False"></asp:Label>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
