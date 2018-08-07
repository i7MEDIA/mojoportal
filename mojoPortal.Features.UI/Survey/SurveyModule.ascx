<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="SurveyModule.ascx.cs" Inherits="SurveyFeature.UI.SurveyModule" %>

<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
	<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper survey">
		<portal:ModuleTitleControl EditText="Edit" EditUrl="~/Survey/SurveyAdmin.aspx" runat="server" ID="TitleControl" />
		<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
			<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
				<portal:FormGroupPanel runat="server">
					<asp:Literal runat="server" ID="litSurveyMessage" />
				</portal:FormGroupPanel>

				<portal:FormGroupPanel runat="server" ID="fgpOldResponses" Visible="false">
					<asp:Label runat="server" ID="litOldResponses" />
					<asp:CheckBox runat="server" ID="chkUseOldResponses" />
				</portal:FormGroupPanel>

				<portal:FormGroupPanel runat="server">
					<portal:mojoButton ID="btnStartSurvey" runat="server" CausesValidation="false" SkinID="SaveButton" />
				</portal:FormGroupPanel>
			</portal:InnerBodyPanel>
		</portal:OuterBodyPanel>
		<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared" />
	</portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
