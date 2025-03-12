<%@ Page Language="c#" CodeBehind="EditEvent.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.EventCalendarUI.EventCalendarEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />

<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper eventcalendaredit">
			<portal:HeadingControl ID="heading" runat="server" />

			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnUpdate">

						<div class="settingrow">
							<mp:SiteLabel runat="server"
								ID="lblTitle"
								ForControl="txtTitle"
								CssClass="settinglabel"
								ConfigKey="EventCalendarEditTitleLabel"
								ResourceFile="EventCalResources" />

							<asp:TextBox runat="server"
								ID="txtTitle"
								Columns="50"
								MaxLength="100"
								CssClass="forminput widetextbox" />

							<asp:RequiredFieldValidator runat="server"
								ID="rfvTitle"
								ControlToValidate="txtTitle"
							/>
						</div>

						<div class="settingrow">
							<mp:SiteLabel runat="server"
								ID="Sitelabel3"
								ForControl="fckDescription"
								ConfigKey="EventCalendarEditDescriptionLabel"
								ResourceFile="EventCalResources"
								CssClass="settinglabel" />
						</div>

						<div class="settingrow">
							<mpe:EditorControl runat="server" ID="edContent" />
						</div>

						<div class="settingrow">
							<mp:SiteLabel runat="server"
								ID="lblStartDate"
								ForControl="dpEventDate" 
								CssClass="settinglabel" 
								ConfigKey="EventCalendarEditEventDateLabel" 
								ResourceFile="EventCalResources" />

							<mp:DatePickerControl runat="server"
								ID="dpEventDate"
								ShowTime="False" 
								CssClass="forminput" 
								SkinID="eventcalendar" />
						</div>

						<div class="settingrow">
							<mp:SiteLabel runat="server"
								ID="Sitelabel4" 
								ForControl="ddStartTime" 
								CssClass="settinglabel" 
								ConfigKey="EventCalendarEditStartTimeLabel" 
								ResourceFile="EventCalResources" />

							<asp:DropDownList runat="server"
								ID="ddStartTime"
								EnableTheming="false" 
								CssClass="forminput" />
						</div>

						<div class="settingrow">
							<mp:SiteLabel runat="server"
								ID="Sitelabel2" 
								ForControl="ddEndTime" 
								CssClass="settinglabel" 
								ConfigKey="EventCalendarEditEndTimeLabel" 
								ResourceFile="EventCalResources" />

							<asp:DropDownList runat="server"
								ID="ddEndTime" 
								EnableTheming="false" 
								CssClass="forminput" />
						</div>

						<div class="settingrow locationrow">
							<mp:SiteLabel runat="server"
								ID="SiteLabel1" 
								ForControl="txtLocation" 
								CssClass="settinglabel" 
								ConfigKey="LocationLabel" 
								ResourceFile="EventCalResources" />

							<asp:TextBox runat="server"
								ID="txtLocation" 
								Columns="50" 
								MaxLength="100" 
								CssClass="forminput widetextbox" />

							<mp:SiteLabel runat="server" 
								ID="lblShowMap" 
								ForControl="showMap" 
								ConfigKey="ShowMap" 
								ResourceFile="EventCalResources" 
								UseLabelTag="true">
							</mp:SiteLabel>

							<asp:CheckBox runat="server" ID="chkShowMap" />
						</div>

						<div class="settingrow">
							<asp:ValidationSummary runat="server" />
						</div>

						<div class="settingrow">
							<mp:SiteLabel ID="SiteLabel35" runat="server" CssClass="settinglabel" ConfigKey="spacer" />

							<div class="forminput">
								<portal:mojoButton ID="btnUpdate" runat="server" Text="Update" />
								&nbsp;
								<portal:mojoButton ID="btnDelete" runat="server" Text="Delete this item" CausesValidation="false" />
								&nbsp;
								<asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" />&nbsp;	
								<portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="eventedithelp" />
							</div>
						</div>

						<asp:HiddenField ID="hdnReturnUrl" runat="server" />
					</asp:Panel>

				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>

		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>

	<portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
