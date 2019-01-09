<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="LoginModule.ascx.cs" Inherits="mojoPortal.Web.Modules.LoginModule" %>
<%@ Register TagPrefix="mp" TagName="Login" Src="~/Controls/LoginControl.ascx" %>

<portal:LoginModuleDisplaySettings ID="displaySettings" runat="server" />

<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
	<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper loginmodule">
		<portal:ModuleTitleControl EditText="Edit" runat="server" ID="TitleControl" />

		<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
			<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
				<div id="sslWarning" runat="server" class="sslwarning">
					<mp:SiteLabel ID="lblSslWarning" runat="server" CssClass="txterror warning" ConfigKey="UseSslWarning" ResourceFile="Resource" UseLabelTag="false" />
				</div>
	
				<asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
					<ContentTemplate>
						<mp:Login ID="login1" runat="server" SetRedirectUrl="false" />
					</ContentTemplate>
				</asp:UpdatePanel>

				<portal:OpenIdRpxNowLink ID="janrainWidet" runat="server" />
				<portal:WelcomeMessage ID="WelcomeMessage" runat="server" UseFirstLast="true" RenderAsListItem="false" SkinID="LoginModule" />
				<portal:Avatar ID="avatar1" runat="server" AutoConfigure="true" />
				<asp:Literal ID="litBreak" runat="server" EnableViewState="false" />
				<portal:LogoutLink ID="LogoutLink" runat="server" RenderAsListItem="false" CssClass="logoutlink" />
			</portal:InnerBodyPanel>
		</portal:OuterBodyPanel>
		<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
	</portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
