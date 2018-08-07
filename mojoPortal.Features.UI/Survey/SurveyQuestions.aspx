<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" EnableEventValidation="false" CodeBehind="SurveyQuestions.aspx.cs" Inherits="SurveyFeature.UI.QuestionsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<div class="breadcrumbs">
		<asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb" /> &gt;
		<asp:HyperLink runat="server" ID="lnkSurveys" CssClass="unselectedcrumb" /> &gt; 
		<asp:HyperLink runat="server" ID="lnkPages" CssClass="unselectedcrumb" /> &gt;
		<asp:HyperLink runat="server" ID="lnkQuestions" CssClass="selectedcrumb" />
	</div>

	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper survey">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<portal:FormGroupPanel runat="server">
						<asp:DropDownList ID="ddQuestionType" runat="server" ToolTip='<%# Resources.SurveyResources.QuestionsQuestionTypeIdLabel %>' />
						<portal:mojoButton ID="btnNewQuestion" runat="server" SkinID="SaveButton" />
					</portal:FormGroupPanel>

					<mp:mojoGridView ID="grdSurveyQuestions" runat="server"
						AllowPaging="false"
						AllowSorting="false"
						AutoGenerateColumns="false"
						CssClass=""
						DataKeyNames="QuestionGuid">
						<Columns>
							<asp:TemplateField>
								<ItemTemplate>
									<asp:HyperLink ID="editLink"
										Text='<%# Resources.SurveyResources.QuestionsGridEditButton %>'
										ToolTip='<%# Resources.SurveyResources.QuestionsGridEditButtonToolTip %>'
										ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>'
										NavigateUrl='<%# SiteRoot + "/Survey/SurveyQuestionEdit.aspx?QuestionGuid=" + Eval("QuestionGuid") + "&pageid=" + PageId + "&mid=" + ModuleId %>'
										runat="server" />
									<asp:ImageButton ID="btnDelete" runat="server"
										CommandName="delete"
										CommandArgument='<%# Eval("QuestionGuid") %>'
										AlternateText='<%# Resources.SurveyResources.QuestionsGridDeleteButtonAlternateText %>'
										ToolTip='<%# Resources.SurveyResources.QuestionsGridDeleteButtonToolTip %>'
										ImageUrl='<%# DeleteLinkImage %>' />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField>
								<ItemTemplate>
									<%# FormatQuestionTextForDisplay(Eval("QuestionName", null)) %>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField>
								<ItemTemplate>
									<%# GetQuestionTypeText(Eval("QuestionTypeId", null)) %>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField>
								<ItemTemplate>
									<%# Eval("AnswerIsRequired") %>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField>
								<ItemTemplate>
									<asp:ImageButton runat="server"
										ID="btnUp"
										ToolTip='<%# Resources.SurveyResources.QuestionsGridMoveUpToolTip %>'
										AlternateText='<%# Resources.SurveyResources.QuestionsGridMoveUpAlternateText %>'
										ImageUrl="~/Data/SiteImages/up.png"
										CommandName="up"
										CausesValidation="False"
										CommandArgument='<%# Eval("QuestionGuid")%>' />
									<asp:ImageButton runat="server"
										ID="btnDown"
										ToolTip='<%# Resources.SurveyResources.QuestionsGridMoveDownToolTip %>'
										AlternateText='<%# Resources.SurveyResources.QuestionsGridMoveDownAlternateText %>'
										ImageUrl="~/Data/SiteImages/down.png"
										CommandName="down"
										CausesValidation="False"
										CommandArgument='<%# Eval("QuestionGuid")%>' />
								</ItemTemplate>
							</asp:TemplateField>
						</Columns>
					</mp:mojoGridView>
					<portal:EmptyPanel ID="divCleared1" runat="server" CssClass="cleared" SkinID="cleared" />
					<asp:Label ID="lblMessages" runat="server" EnableViewState="False" />
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
			<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared" />
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />