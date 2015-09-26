<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" EnableEventValidation="false" CodeBehind="SurveyQuestions.aspx.cs" Inherits="SurveyFeature.UI.QuestionsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
    <asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkSurveys" CssClass="unselectedcrumb"></asp:HyperLink> &gt; 
    <asp:HyperLink runat="server" ID="lnkPages" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkQuestions" CssClass="selectedcrumb"></asp:HyperLink>     
</div>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper survey">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<div class="settingrow">
 <asp:DropDownList ID="ddQuestionType" runat="server" ToolTip='<%# Resources.SurveyResources.QuestionsQuestionTypeIdLabel %>' />
<asp:Button ID="btnNewQuestion" runat="server" />
</div>
<mp:mojoGridView ID="grdSurveyQuestions" runat="server" 
        AllowPaging="false" 
        AllowSorting="false"
        AutoGenerateColumns="false" 
        CssClass="" 
        DataKeyNames="QuestionGuid">
     <Columns>
		<asp:TemplateField>
        <ItemTemplate>
                 <asp:HyperLink id="editLink" 
						    Text='<%# Resources.SurveyResources.QuestionsGridEditButton %>'
						    Tooltip='<%# Resources.SurveyResources.QuestionsGridEditButtonToolTip %>'
						    ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>' 
						    NavigateUrl='<%# SiteRoot + "/Survey/SurveyQuestionEdit.aspx?QuestionGuid=" + Eval("QuestionGuid") + "&pageid=" + PageId + "&mid=" + ModuleId%>' 
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
                  <%# FormatQuestionTextForDisplay(Eval("QuestionText", null)) %>
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
               <asp:ImageButton id="btnUp"
						    Tooltip='<%# Resources.SurveyResources.QuestionsGridMoveUpToolTip %>'
						    AlternateText='<%# Resources.SurveyResources.QuestionsGridMoveUpAlternateText %>'
						    ImageUrl="~/Data/SiteImages/up.gif"
                            CommandName="up"
                            runat="server"
                            CausesValidation="False" 
                            CommandArgument='<%# Eval("QuestionGuid")%>' />
				        <asp:ImageButton id="btnDown"
						    Tooltip='<%# Resources.SurveyResources.QuestionsGridMoveDownToolTip %>'
						    AlternateText='<%# Resources.SurveyResources.QuestionsGridMoveDownAlternateText %>'
						    ImageUrl="~/Data/SiteImages/dn.gif"
                            CommandName="down" 
                            runat="server"
                            CausesValidation="False" 
                            CommandArgument='<%# Eval("QuestionGuid")%>' />
            </ItemTemplate>	
		</asp:TemplateField>	
</Columns>
</mp:mojoGridView>
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
