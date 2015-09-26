<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Results.aspx.cs" Inherits="SurveyFeature.UI.ResultsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">

<div class="breadcrumbs">
    <asp:HyperLink ID="lnkPageCrumb" runat="server"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkSurveys"></asp:HyperLink> &gt;
    <asp:HyperLink runat="server" ID="lnkResults"></asp:HyperLink>
</div>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper survey">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
    <div class="settingrow">
        <strong><mp:SiteLabel ID="lblRespondentLabel" runat="server" ConfigKey="ResponseRespondentLabel"
        ResourceFile="SurveyResources" UseLabelTag="false" /></strong>
        <asp:Label runat="server" id="lblRespondent"></asp:Label>
    </div>
    <div class="settingrow">
        <strong><mp:SiteLabel ID="lblCompletionDateLabel" runat="server" ConfigKey="ResponseCompletionDateLabel"
        ResourceFile="SurveyResources" UseLabelTag="false" /></strong>
        <asp:Label ID="lblCompletionDate" runat="server"></asp:Label>
        <asp:ImageButton ID="btnDelete" runat="server" 
        CommandName="delete" ToolTip='<%$ Resources:SurveyResources, ResultsGridDeleteResponseToolTip %>'
        AlternateText='<%$ Resources:SurveyResources, ResultsGridDeleteResponseButton %>'
        />		
    </div>
    <div class="settingrow">
        <asp:HyperLink ID="lnkPreviousResponse" runat="server"></asp:HyperLink>
        <asp:HyperLink ID="lnkNextResponse" runat="server"></asp:HyperLink>
    </div>
    <asp:Panel id="pnlSurveyPages" runat="server">
     <mp:mojoGridView ID="grdResults" runat="server" 
        AllowPaging="false" 
        AllowSorting="false"
        AutoGenerateColumns="false" 
        CssClass="" 
        DataKeyNames="QuestionGuid">
     <Columns>
		<asp:TemplateField>
        <ItemTemplate>
                 <%# Eval("QuestionText") %>&nbsp;&nbsp;
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                  <%# Eval("Answer") %>&nbsp;&nbsp;
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                  <%# Eval("PageTitle") %>&nbsp;&nbsp;
            </ItemTemplate>
		</asp:TemplateField>	
</Columns>
</mp:mojoGridView>   
        <br /><br />
    </asp:Panel>
    </portal:InnerBodyPanel>
 </portal:OuterBodyPanel>
 <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
   </portal:InnerWrapperPanel>
   <mp:CornerRounderBottom id="cbottom1" runat="server" />
   </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
