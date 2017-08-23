<%@ Page ValidateRequest="false" Language="c#" MaintainScrollPositionOnPostback="true"
	CodeBehind="PollEdit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
	AutoEventWireup="false" Inherits="PollFeature.UI.PollEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<div class="breadcrumbs">
		<asp:HyperLink ID="lnkPageCrumb" runat="server" CssClass="unselectedcrumb"></asp:HyperLink>
		&gt;
		<asp:HyperLink runat="server" ID="lnkPolls" CssClass="unselectedcrumb"></asp:HyperLink>
	</div>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper poll">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<asp:Panel ID="pnlPoll" runat="server" DefaultButton="btnSave">
						<div class="pollwrap">
							<asp:Button ID="btnAddNewPoll" runat="server" CssClass="buttonlink" CausesValidation="false" />
							<asp:Button ID="btnViewPolls" runat="server" CssClass="buttonlink" CausesValidation="false" />
							<hr />
							<div class="settingrow">
								<mp:SiteLabel ID="lblQuestion" runat="server" ForControl="txtQuestion" CssClass="settinglabel"
									ConfigKey="PollEditQuestionLabel" ResourceFile="PollResources"></mp:SiteLabel>
								<asp:TextBox ID="txtQuestion" runat="server" CssClass="forminput verywidetextbox" MaxLength="255"></asp:TextBox>
							</div>
							<div class="settingrow">
								<mp:SiteLabel ID="lblAnonymousVoting" runat="server" ForControl="chkAnonymousVoting"
									CssClass="settinglabel" ConfigKey="PollEditAnonymousVotingLabel" ResourceFile="PollResources"></mp:SiteLabel>
								<asp:CheckBox ID="chkAnonymousVoting" runat="server"></asp:CheckBox>
							</div>
							<div class="settingrow">
								<mp:SiteLabel ID="lblAllowViewingResultsBeforeVoting" runat="server" ForControl="chkAllowViewingResultsBeforeVoting"
									CssClass="settinglabel" ConfigKey="PollEditAllowViewingResultsBeforeVotingLabel"
									ResourceFile="PollResources"></mp:SiteLabel>
								<asp:CheckBox ID="chkAllowViewingResultsBeforeVoting" runat="server"></asp:CheckBox>
							</div>
							<div class="settingrow">
								<mp:SiteLabel ID="lblShowOrderNumbers" runat="server" ForControl="chkShowOrderNumbers"
									CssClass="settinglabel" ConfigKey="PollEditShowOrderNumbersLabel" ResourceFile="PollResources"></mp:SiteLabel>
								<asp:CheckBox ID="chkShowOrderNumbers" runat="server"></asp:CheckBox>
							</div>
							<div class="settingrow">
								<mp:SiteLabel ID="lblShowResultsWhenDeactivated" runat="server" ForControl="chkShowResultsWhenDeactivated"
									CssClass="settinglabel" ConfigKey="PollEditShowResultsWhenDeactivatedLabel" ResourceFile="PollResources"></mp:SiteLabel>
								<asp:CheckBox ID="chkShowResultsWhenDeactivated" runat="server"></asp:CheckBox>
							</div>
							<div class="settingrow">
								<mp:SiteLabel ID="lblPollAddOptions" runat="server" ForControl="tblOptions" CssClass="settinglabel"
									ConfigKey="PollEditOptionsLabel" ResourceFile="PollResources"></mp:SiteLabel>
								<table id="tblOptions" cellpadding="0" cellspacing="0" border="0">
									<tr valign="top">
										<td>
											<asp:ListBox ID="lbOptions" SkinID="PageTree" DataTextField="Answer" DataValueField="OptionGuid"
												Rows="10" runat="server" />
										</td>
										<td>
											<asp:ImageButton ID="btnUp" CommandName="up" runat="server" CausesValidation="False" />
											<br />
											<asp:ImageButton ID="btnDown" CommandName="down" runat="server" CausesValidation="False" />
											<br />
											<asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" />
											<br />
											<asp:ImageButton ID="btnDeleteOption" runat="server" CausesValidation="False" />
											<br />
											<br />
											<portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="addeditpolloptionshelp" />
										</td>
									</tr>
									<tr>
										<td>
											<asp:TextBox ID="txtNewOption" runat="server" Columns="39" MaxLength="100"></asp:TextBox>
											<portal:mojoButton ID="btnAddOption" runat="server" CausesValidation="False" />
										</td>
										<td>&nbsp;</td>
									</tr>
								</table>
							</div>
							<br />
							<div class="settingrow">
								<mp:SiteLabel ID="lblActiveFromTo" runat="server" CssClass="settinglabel" ConfigKey="PollEditActiveFromToLabel"
									ResourceFile="PollResources"></mp:SiteLabel>
								<mp:DatePickerControl ID="dpActiveFrom" runat="server" ShowTime="True" CssClass="forminput" SkinID="Poll" />
								<mp:SiteLabel ID="lblTo" runat="server" ResourceFile="PollResources" ConfigKey="PollEditToLabel" />
								<mp:DatePickerControl ID="dpActiveTo" runat="server" ShowTime="True" CssClass="forminput" SkinID="Poll" />
							</div>
							<div class="settingrow" id="divStartDeactivated" runat="server" visible="false">
								<mp:SiteLabel ID="lblStartDeactivated" runat="server" ForControl="chkStartDeactivated"
									CssClass="settinglabel" ConfigKey="PollEditStartDeactivatedLabel" ResourceFile="PollResources"></mp:SiteLabel>
								<asp:CheckBox ID="chkStartDeactivated" runat="server" />
							</div>
							<div class="settingrow">
								<asp:RequiredFieldValidator ID="reqQuestion" runat="server" ControlToValidate="txtQuestion"
									Display="None" CssClass="txterror" ValidationGroup="poll"></asp:RequiredFieldValidator>
								<asp:CustomValidator ID="cvOptionsLessThanTwo" runat="server" Display="None" CssClass="txterror" ValidationGroup="poll"></asp:CustomValidator>
								<asp:ValidationSummary ID="vSummary" runat="server" CssClass="txterror" ValidationGroup="poll"></asp:ValidationSummary>
							</div>
							<div class="settingrow">
								<mp:SiteLabel ID="lblspacer" runat="server" CssClass="settinglabel" ConfigKey="spacer" />
								<portal:mojoButton ID="btnSave" runat="server" />&nbsp;
								<portal:mojoButton ID="btnDelete" runat="server" CausesValidation="false" />&nbsp;
								<portal:mojoButton ID="btnActivateDeactivate" runat="server" CausesValidation="False" Visible="false" />
							</div>
						</div>
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