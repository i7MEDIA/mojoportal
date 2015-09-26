<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" enableEventValidation="false" CodeBehind="SurveyPages.aspx.cs" Inherits="SurveyFeature.UI.SurveyPagesPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
    <asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkSurveys" CssClass="unselectedcrumb"></asp:HyperLink> &gt; 
    <asp:HyperLink runat="server" ID="lnkPages" CssClass="selectedcrumb"></asp:HyperLink>
</div>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper survey">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<mp:mojoGridView ID="grdSurveyPages" runat="server" 
        AllowPaging="false" 
        AllowSorting="false"
        AutoGenerateColumns="false" 
        CssClass="" 
        DataKeyNames="SurveyPageGuid">
     <Columns>
		<asp:TemplateField>
        <ItemTemplate>
                 <asp:HyperLink id="editLink" 
						Text="<%# Resources.SurveyResources.PagesGridEditButton %>" 
						Tooltip="<%# Resources.SurveyResources.PagesGridEditButtonToolTip %>"
						ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>' 
						NavigateUrl='<%# SiteRoot + "/Survey/SurveyPageEdit.aspx?SurveyPageGuid=" + Eval("SurveyPageGuid") + "&pageid=" + PageId + "&mid=" + ModuleId%>' 
						runat="server" />
                    <asp:ImageButton ID="btnDelete" runat="server" 
                        CommandName="delete" ToolTip='<%# Resources.SurveyResources.PagesGridDeleteButtonToolTip %>'
                        CommandArgument='<%# Eval("SurveyPageGuid") %>'
                        AlternateText='<%# Resources.SurveyResources.PagesGridDeleteButton %>'
                        ImageUrl='<%# DeleteLinkImage %>' />		
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                 <%# Eval("PageTitle") %>&nbsp;&nbsp;
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                 <%# Eval("PageEnabled") %>&nbsp;&nbsp;
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
               <asp:HyperLink id="questionsLink"
                        Text='<%# Eval("QuestionCount") %>'
						Tooltip="<%# Resources.SurveyResources.PagesGridEditButtonToolTip %>"
						NavigateUrl='<%# SiteRoot + "/Survey/SurveyQuestions.aspx?SurveyGuid=" + Eval("SurveyGuid") + "&SurveyPageGuid=" + Eval("SurveyPageGuid") + "&pageid=" + PageId + "&mid=" + ModuleId%>' 
						runat="server" />
						
						<asp:HyperLink id="HyperLink1"
						Text="<%# Resources.SurveyResources.AddEditGridLink %>"
						Tooltip="<%# Resources.SurveyResources.AddEditGridLink %>"
						NavigateUrl='<%# SiteRoot + "/Survey/SurveyQuestions.aspx?SurveyGuid=" + Eval("SurveyGuid") + "&SurveyPageGuid=" + Eval("SurveyPageGuid") + "&pageid=" + PageId + "&mid=" + ModuleId%>' 
						runat="server" />
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
               <asp:ImageButton id="btnUp"
						Tooltip='<%# Resources.SurveyResources.PagesGridMoveUpToolTip %>'
						AlternateText='<%# Resources.SurveyResources.PagesGridMoveUpAlternateText %>'
						ImageUrl="~/Data/SiteImages/up.gif"
                        CommandName="up"
                        runat="server"
                        CausesValidation="False" 
                        CommandArgument='<%# Eval("SurveyPageGuid")%>' />
				    <asp:ImageButton id="btnDown"
						Tooltip='<%# Resources.SurveyResources.PagesGridMoveDownToolTip %>'
						AlternateText='<%# Resources.SurveyResources.PagesGridMoveDownAlternateText %>'
						ImageUrl="~/Data/SiteImages/dn.gif"
                        CommandName="down" 
                        runat="server"
                        CausesValidation="False" 
                        CommandArgument='<%# Eval("SurveyPageGuid")%>' />
            </ItemTemplate>
		</asp:TemplateField>
</Columns>
</mp:mojoGridView>
<div class="modulepager" >
    <asp:HyperLink ID="lnkAddNew" runat="server" />
    </div>
<portal:EmptyPanel id="divCleared1" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
<asp:Label ID="lblMessages" runat="server" EnableViewState="False"></asp:Label>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
