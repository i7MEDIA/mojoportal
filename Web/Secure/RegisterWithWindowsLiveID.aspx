<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="RegisterWithWindowsLiveID.aspx.cs" Inherits="mojoPortal.Web.UI.Pages.RegisterWithWindowsLiveId" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<asp:Panel ID="pnlRegister" runat="server" CssClass="panelwrapper register">
		<div class="modulecontent">
			<fieldset>
				<legend>
					<mp:SiteLabel ID="lblRegisterLabel" runat="server" ConfigKey="WindowsLiveIDRegistrationHeading" UseLabelTag="false" />
				</legend>
				<asp:Panel ID="pnlAuthenticated" runat="server" Visible="false">
					<asp:Literal ID="litAlreadyAuthenticated" runat="server" />
				</asp:Panel>
				<asp:Panel ID="pnlRegisterWrapper" runat="server">
					<asp:Panel ID="pnlWindowsLiveLogin" runat="server">
						<div id="divAgreement" runat="server"></div>
						<div style="background: white; color: black; font-size: small; width: 350px; padding: 3px 3px 3px 3px; border: dashed thin gray;">
							<div>
								<asp:Literal ID="litInstructions" runat="server" />
							</div>
							<iframe
								id="WebAuthControl"
								name="WebAuthControl"
								src="<%=Protocol%>login.live.com/controls/WebAuth.htm?appid=<%=WindowsLiveAppId%>&style=font-size%3A+small%3B+font-family%3A+verdana%3B+background%3A+white%3B"
								width="97"
								height="25"
								marginwidth="0"
								marginheight="0"
								frameborder="0"
								scrolling="no" style="display: inline; padding: 0px 0px 0px 0px;"></iframe>
							<span style="vertical-align: top;"></span>
						</div>
					</asp:Panel>
					<asp:Panel ID="pnlWindowsLiveRegister" runat="server">
						<div class="settingrow">
							<mp:SiteLabel ID="lblLoginName" runat="server" ForControl="txtUserName" CssClass="settinglabel" ConfigKey="RegisterLoginNameLabel" />
							<asp:TextBox ID="txtUserName" runat="server" TabIndex="10" Columns="30" MaxLength="50" />
						</div>
						<div class="settingrow">
							<mp:SiteLabel ID="lblRegisterEmail1" runat="server" ForControl="txtEmail" CssClass="settinglabel" ConfigKey="RegisterEmailLabel" />
							<asp:TextBox ID="txtEmail" runat="server" TabIndex="10" Columns="30" MaxLength="100" />
						</div>
						<asp:Panel ID="pnlRequiredProfileProperties" runat="server">
						</asp:Panel>
						<div class="settingrow">
							<asp:ValidationSummary ID="vSummary" runat="server" ValidationGroup="profile" />

							<asp:RequiredFieldValidator
								ControlToValidate="txtUserName"
								ID="UserNameRequired"
								runat="server"
								Display="None"
								ValidationGroup="profile" />

							<asp:RequiredFieldValidator
								ControlToValidate="txtEmail"
								ID="EmailRequired"
								runat="server"
								Display="None"
								ValidationGroup="profile" />

							<asp:RegularExpressionValidator
								ID="EmailRegex" runat="server"
								ControlToValidate="txtEmail"
								Display="None"
								ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$"
								ValidationGroup="profile" />

						</div>
						<div>
							<portal:mojoLabel ID="lblMessage" runat="server" CssClass="txterror info" />
						</div>
						<div class="settingrow">
							<mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="EmptyLabel" />
							<asp:Button ID="btnCreateUser" runat="server" />
						</div>
						<div>
							<asp:Literal ID="litInfoNeededMessage" runat="server" />
						</div>
					</asp:Panel>
					<portal:mojoLabel ID="lblError" runat="server" CssClass="txterror warning" />
				</asp:Panel>
			</fieldset>
		</div>
	</asp:Panel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
