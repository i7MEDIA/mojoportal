<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="ServerInformation.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ServerInformation" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx"
			CssClass="unselectedcrumb" /><portal:AdminCrumbSeparator ID="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkServerInfo" runat="server" CssClass="selectedcrumb" />
	</portal:AdminCrumbContainer>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portalAdmin:AdminDisplaySettings ID="displaySettings" runat="server" />
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin serverinformation">
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:HeadingControl ID="heading" runat="server" />
				<div class="settingrow">
					<span class="settinglabel">
						<a href="https://www.mojoportal.com?utm_source=amojosite&utm_medium=referral&utm_campaign=SystemInfo" title="mojoPortal Web Site" target="_blank">mojoPortal</a>
						<mp:SiteLabel ID="SiteLabel1" runat="server" ConfigKey="AdminMenuVersionLabel" UseLabelTag="false" />
					</span>
					<asp:Literal ID="litCodeVersion" runat="server" />
					<asp:Literal ID="litPlatform" runat="server" />
					<asp:Literal ID="litUpdateInfo" runat="server" />
				</div>
				<div class="settingrow">
					<mp:SiteLabel ID="SiteLabel5" runat="server" CssClass="settinglabel" ConfigKey="OperatingSystemLabel" UseLabelTag="false" />
					<asp:Literal ID="litOperatingSystem" runat="server" />
				</div>
				<div class="settingrow" id="divDotNetVersion" runat="server">
					<mp:SiteLabel ID="SiteLabel7" runat="server" CssClass="settinglabel" ConfigKey="DotNetVersionLabel" UseLabelTag="false" />
					<asp:Literal ID="litDotNetVersion" runat="server" />
				</div>
				<div class="settingrow">
					<mp:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="AdminMenuServerTimeZoneLabel" UseLabelTag="false" />
					<asp:Literal ID="litServerTimeZone" runat="server" />
				</div>
				<div class="settingrow">
					<mp:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="AdminMenuServerLocalTimeLabel" UseLabelTag="false" />
					(<mp:SiteLabel ID="SiteLabel4" runat="server" ConfigKey="GMT" UseLabelTag="false" />
					<asp:Literal ID="litServerGMTOffset" runat="server" />)
					<asp:Literal ID="litServerLocalTime" runat="server" />
				</div>
				<div class="settingrow">
					<mp:SiteLabel ID="SiteLabel6" runat="server" CssClass="settinglabel" ConfigKey="AdminMenuServerCurrentGMTTimeLabel" UseLabelTag="false" />
					<asp:Literal ID="litCurrentGMT" runat="server" />
				</div>
				<div class="settingrow restartbuttonrow">
					<button id="btnRestart" runat="server" onserverclick="btnRestart_ServerClick"><%= Resources.Resource.RestartWebApp %></button>
				</div>
				<asp:Panel ID="pnlFeatureVersions" runat="server" CssClass="settingrow">
					<h2 class="heading">
						<asp:Literal ID="litFeaturesHeading" runat="server" />
						<portal:mojoHelpLink ID="MojoHelpLink2" runat="server" HelpKey="featureversion-help" /></h2>
					<mp:mojoGridView ID="grdSchemaVersion" runat="server"
						CssClass=""
						AutoGenerateColumns="false"
						DataKeyNames="ApplicationID">
						<Columns>
							<asp:TemplateField>
								<ItemTemplate><%# Eval("ApplicationName") %></ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField>
								<ItemTemplate><%# Eval("Major") %>.<%# Eval("Minor") %>.<%# Eval("Build") %>.<%# Eval("Revision") %></ItemTemplate>
							</asp:TemplateField>
						</Columns>
					</mp:mojoGridView>
				</asp:Panel>
			</portal:OuterBodyPanel>
			<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
