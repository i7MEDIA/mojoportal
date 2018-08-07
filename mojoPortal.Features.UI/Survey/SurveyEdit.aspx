<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="SurveyEdit.aspx.cs" Inherits="SurveyFeature.UI.SurveyEditPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<div class="breadcrumbs">
		<asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb" /> &gt;
		<asp:HyperLink runat="server" ID="lnkSurveys" CssClass="unselectedcrumb" /> &gt;
		<asp:HyperLink runat="server" ID="lnkSurveyEdit" CssClass="selectedcrumb" />
	</div>

	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper survey">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<asp:Panel ID="pnlSurvey" runat="server" DefaultButton="btnSave">
						<portal:FormGroupPanel runat="server">
							<mp:SiteLabel runat="server"
								ID="lblSurveyName"
								ForControl="txtSurveyName"
								CssClass="settinglabel"
								ConfigKey="SurveyEditSurveyNameLabel"
								ResourceFile="SurveyResources" />
							<asp:TextBox ID="txtSurveyName" runat="server" Columns="50" MaxLength="100" />
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server">
							<mp:SiteLabel runat="server"
								ID="lblLimitSubmissions"
								CssClass="settinglabel"
								ForControl="cbLimitSubmissions"
								ConfigKey="SurveyEditLimitSubmissions"
								ResourceFile="SurveyResources" />
							<asp:CheckBox runat="server" ID="cbLimitSubmissions" />
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server" ID="fgpSubmissionLimit" ExtraCssClasses="hide" RenderId="true">
							<mp:SiteLabel runat="server"
								ID="lblSubmissionLimit"
								CssClass="settinglabel"
								ForControl="cbLimitSubmissionLimit"
								ConfigKey="SurveyEditSubmissionLimit"
								ResourceFile="SurveyResources" />
							<asp:TextBox runat="server" ID="txtSubmissionLimit" TextMode="Number" />
						</portal:FormGroupPanel>

						<script>
							(function () {
								var checkbox = document.getElementById('<%= cbLimitSubmissions.ClientID %>');
								var limitGroup = document.getElementById('<%= fgpSubmissionLimit.ClientID %>');

								function toggleVisibility() {
									if (checkbox.checked) {
										limitGroup.classList.remove('hide');
									} else {
										limitGroup.classList.add('hide');
									}
								}

								toggleVisibility();

								checkbox.addEventListener('click', function (e) {
									toggleVisibility();
								});
							})();
						</script>

						<portal:FormGroupPanel runat="server">
							<mp:SiteLabel runat="server"
								ID="lblWelcomeMessage"
								ForControl="edWelcomeMessage"
								CssClass="settinglabel"
								ConfigKey="SurveyWelcomeMessageLabel"
								ResourceFile="SurveyResources" />
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server">
							<mpe:EditorControl ID="edWelcomeMessage" runat="server" />
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server">
							<mp:SiteLabel runat="server"
								ID="lblThankyouMessage"
								ForControl="edThankyouMessage"
								CssClass="settinglabel"
								ConfigKey="SurveyThankyouMessageLabel"
								ResourceFile="SurveyResources" />
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server">
							<mpe:EditorControl ID="edThankyouMessage" runat="server" />
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server" ExtraCssClasses="btn-row">
							<portal:mojoButton runat="server" ID="btnSave" SkinID="SaveButton" />
							<portal:mojoButton runat="server" ID="btnCancel" CausesValidation="false" SkinID="TextButton" />
						</portal:FormGroupPanel>
					</asp:Panel>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
			<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared" />
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
	<portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
