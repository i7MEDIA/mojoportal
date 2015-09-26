<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SurveyQuestionEdit.aspx.cs" Inherits="SurveyFeature.UI.QuestionEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<div class="breadcrumbs">
    <asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkSurveys" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkPages" CssClass="unselectedcrumb"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkQuestions" CssClass="unselectedcrumb"></asp:HyperLink> 
   
</div>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper survey">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<asp:Panel id="pnlQuestionEdit" runat="server" DefaultButton="btnSave">  
    <div class="settingrow">
        <mpe:EditorControl id="edMessage" runat="server"></mpe:EditorControl>
    </div>
    <div class="settingrow">
        <mp:SiteLabel ID="lblQuestionRequiredLabel" runat="server" ForControl="chkAnswerRequired" ConfigKey="QuestionRequiredLabel" ResourceFile="SurveyResources" CssClass="settinglabel" />
        <asp:CheckBox runat="server" ID="chkAnswerRequired" />
    </div>
    <div class="settingrow">
        <mp:SiteLabel ID="lblQuestionTypeLabel" runat="server" ConfigKey="QuestionTypeLabel" ResourceFile="SurveyResources" CssClass="settinglabel" />
        <asp:Label ID="lblQuestionType" runat="server" />
    </div>
    <div class="settingrow" runat="server">
        <mp:SiteLabel ID="lblValidationMessage" runat="server" ForControl="txtValidationMessage" ConfigKey="QuestionValidationMessageLabel" ResourceFile="SurveyResources" CssClass="settinglabel" />
        <asp:TextBox ID="txtValidationMessage" runat="server" Columns="39" MaxLength="100"></asp:TextBox>
    </div>
    <div class="settingrow" runat="server" id="itemsRow">
            <div id="questionItems" class="floatpanel">
                <asp:ListBox ID="lbOptions" SkinID="PageTree" DataTextField="Answer" DataValueField="OptionGuid" Rows="10"  runat="server" />
            </div>
            <div id="questionItemsMove" class="floatpanel">
            	    <asp:ImageButton ID="btnUp" CommandName="up"  runat="server" CausesValidation="False" />
		            <br />
		            <asp:ImageButton ID="btnDown" CommandName="down" runat="server" CausesValidation="False" />
	                <br />
			        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" />
		            <br />
			        <asp:ImageButton ID="btnDeleteOption" runat="server" CausesValidation="False" />	
		            <br /><br />
		            <portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="addeditsurveypageshelp" />	
            </div>
    </div>
    <div class="settingrow" runat="server" id="addOptionRow">
        <asp:TextBox ID="txtNewOption" runat="server" Columns="39" MaxLength="255"></asp:TextBox>
        <portal:mojoButton ID="btnAddOption" runat="server" CausesValidation="false" /><br />
    </div>
    <div class="settingrow">
        <br />
        <portal:mojoButton ID="btnSave" runat="server" />
        <portal:mojoButton ID="btnCancel" runat="server" />
    </div>
    </asp:Panel>
</portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
<portal:SessionKeepAliveControl id="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
