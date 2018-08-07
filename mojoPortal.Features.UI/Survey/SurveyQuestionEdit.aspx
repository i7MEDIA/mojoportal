<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SurveyQuestionEdit.aspx.cs" Inherits="SurveyFeature.UI.QuestionEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<div class="breadcrumbs">
		<asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb" /> &gt;
		<asp:HyperLink runat="server" ID="lnkSurveys" CssClass="unselectedcrumb" /> &gt;
		<asp:HyperLink runat="server" ID="lnkPages" CssClass="unselectedcrumb" /> &gt;
	    <asp:HyperLink runat="server" ID="lnkQuestions" CssClass="unselectedcrumb" />
	</div>

	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper survey">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<asp:Panel ID="pnlQuestionEdit" runat="server" DefaultButton="btnSave">
						<portal:FormGroupPanel runat="server" CssClass="settingrow">
							<mp:SiteLabel runat="server"
								ID="SiteLabel1"
								ForControl="txtQuestionName"
								ConfigKey="QuestionsGridNameHeader"
								ResourceFile="SurveyResources"
								CssClass="settinglabel"
							/>
							<asp:TextBox runat="server" ID="txtQuestionName" />
							<asp:RequiredFieldValidator runat="server"
								ID="rfvQuestionName"
								ControlToValidate="txtQuestionName"
							/>
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server">
							<mpe:EditorControl ID="edMessage" runat="server" />
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server">
							<mp:SiteLabel runat="server"
								ID="lblQuestionRequiredLabel"
								ForControl="chkAnswerRequired"
								ConfigKey="QuestionRequiredLabel"
								ResourceFile="SurveyResources"
								CssClass="settinglabel"
							/>
							<asp:CheckBox runat="server" ID="chkAnswerRequired" />
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server">
							<mp:SiteLabel runat="server"
								ID="lblQuestionTypeLabel"
								ConfigKey="QuestionTypeLabel"
								ResourceFile="SurveyResources"
								CssClass="settinglabel"
							/>
							<asp:Label runat="server" ID="lblQuestionType" />
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server">
							<mp:SiteLabel runat="server"
								ID="lblValidationMessage"
								ForControl="txtValidationMessage"
								ConfigKey="QuestionValidationMessageLabel"
								ResourceFile="SurveyResources"
								CssClass="settinglabel"
							/>
							<asp:TextBox ID="txtValidationMessage" runat="server" Columns="39" MaxLength="100" />
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server" ID="fgpItemsRow">
							<div id="questionItems" class="floatpanel">
								<asp:ListBox ID="lbOptions" SkinID="PageTree" DataTextField="Answer" DataValueField="OptionGuid" Rows="10" runat="server" />
							</div>

							<div id="questionItemsMove" class="floatpanel">
								<asp:ImageButton ID="btnUp" CommandName="up" runat="server" CausesValidation="False" />
								<br />
								<asp:ImageButton ID="btnDown" CommandName="down" runat="server" CausesValidation="False" />
								<br />
								<asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" />
								<br />
								<asp:ImageButton ID="btnDeleteOption" runat="server" CausesValidation="False" />
								<br />
								<portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="addeditsurveypageshelp" />
							</div>
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server" ID="fgpAddOptionRow">
							<asp:TextBox ID="txtNewOption" runat="server" Columns="39" MaxLength="255" />
							<portal:mojoButton ID="btnAddOption" runat="server" CausesValidation="false" SkinID="SaveButton" />
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server">
							<portal:mojoButton ID="btnSave" runat="server" SkinID="SaveButton" />
							<portal:mojoButton ID="btnCancel" runat="server" SkinID="TextButton" />
						</portal:FormGroupPanel>
					</asp:Panel>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
			<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
	<portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
