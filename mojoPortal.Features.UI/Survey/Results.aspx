<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Results.aspx.cs" Inherits="SurveyFeature.UI.ResultsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<div class="breadcrumbs">
		<asp:HyperLink ID="lnkPageCrumb" runat="server" /> &gt;
		<asp:HyperLink runat="server" ID="lnkSurveys" /> &gt;
		<asp:HyperLink runat="server" ID="lnkResults" CssClass="selectedcrumb" />
	</div>

	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper survey">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<portal:FormGroupPanel runat="server">
						<strong>
							<mp:SiteLabel runat="server"
								ID="lblRespondentLabel"
								ConfigKey="ResponseRespondentLabel"
								ResourceFile="SurveyResources"
								UseLabelTag="false" />
						</strong>
						<asp:Label runat="server" ID="lblRespondent" />
					</portal:FormGroupPanel>

					<portal:FormGroupPanel runat="server">
						<strong>
							<mp:SiteLabel runat="server"
								ID="lblCompletionDateLabel"
								ConfigKey="ResponseCompletionDateLabel"
								ResourceFile="SurveyResources"
								UseLabelTag="false" />
						</strong>
						<asp:Label ID="lblCompletionDate" runat="server" />
						<asp:ImageButton runat="server"
							ID="btnDelete"
							CommandName="delete"
							ToolTip='<%$ Resources:SurveyResources, ResultsGridDeleteResponseToolTip %>'
							AlternateText='<%$ Resources:SurveyResources, ResultsGridDeleteResponseButton %>' />
					</portal:FormGroupPanel>

					<nav role="navigation" aria-label="Response Pager">
						<ul class="pager">
							<li runat="server" class="previous" ID="liPrevResponse"><asp:HyperLink ID="lnkPreviousResponse" runat="server" /></li>
							<li runat="server" class="next" ID="liNextResponse"><asp:HyperLink ID="lnkNextResponse" runat="server" /></li>
						</ul>
					</nav>

					<asp:Panel ID="pnlSurveyPages" runat="server">
						<mp:mojoGridView ID="grdResults" runat="server"
							AllowPaging="false"
							AllowSorting="false"
							AutoGenerateColumns="false"
							CssClass=""
							DataKeyNames="QuestionGuid">
							<Columns>
								<asp:TemplateField>
									<ItemTemplate>
										<b><%# Eval("QuestionName") %></b><br />
										<%# Eval("QuestionText") %>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField>
									<ItemTemplate>
										<%# Eval("Answer") %>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField>
									<ItemTemplate>
										<%# Eval("PageTitle") %>
									</ItemTemplate>
								</asp:TemplateField>
							</Columns>
						</mp:mojoGridView>
					</asp:Panel>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
			<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared" />
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
