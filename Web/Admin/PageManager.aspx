<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="PageManager.aspx.cs" Inherits="mojoPortal.Web.AdminUI.PageManager" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:AdminCrumbContainer ID="divAdminLinks" runat="server" CssClass="breadcrumbs">
		<asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator ID="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
		<asp:HyperLink ID="lnkPageTree" runat="server" /><portal:AdminCrumbSeparator ID="altPmSeparator" runat="server" Text="&nbsp;&gt;" EnableViewState="false" Visible="false" />
		<asp:HyperLink ID="lnkAltPageManager" runat="server" Visible="false" />
	</portal:AdminCrumbContainer>
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin pagetree">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<div class="settingrow">
						<a id="lnkNewPage" runat="server"></a>&nbsp;<portal:mojoLabel ID="litWarning" runat="server" CssClass="txterror warning" />
					</div>
					<asp:Panel ID="pnlInstructions" runat="server" EnableViewState="false" CssClass="settingrow pagemanhelp">
						<asp:Literal ID="litInstructions" runat="server" />
					</asp:Panel>
					<div class="settingrow">
						<div id="tree1" class="treecontainer"></div>
						<ul id="ulCommands" class="treecommands" style="display: none;">
							<li id="liInfo" class="pageinfo"></li>
							<li id="liEdit" class="editcontent">
								<a id="lnkEdit" class="" href="#">
									<asp:Literal ID="litEdit" runat="server" />
								</a>
							</li>
							<li id="liSettings" class="editsettings">
								<a id="lnkSettings" class="" href="#">
									<asp:Literal ID="litSettings" runat="server" />
								</a>
							</li>
							<li id="liPermissions" class="editpermissions">
								<a id="lnkPermissions" class="" href="#">
									<asp:Literal ID="litPermissions" runat="server" />
								</a>
								<span class="permissions" id="spnPermissions"></span>
							</li>
							<li id="liView" class="viewpage">
								<a id="lnkView" class="" href="#">
									<asp:Literal ID="litView" runat="server" />
								</a>
							</li>
							<li id="liSort" class="sortpages">
								<a id="lnkSort" class="" href="#">
									<asp:Literal ID="litSort" runat="server" />
								</a>
							</li>
							<li id="liNewChild" class="newchild">
								<a id="lnkNewChild" class="" href="#">
									<asp:Literal ID="litNewChild" runat="server" />
								</a>
							</li>
							<li id="liDeletePage" class="deletepage">
								<a id="lnkDeletePage" class="" href="#">
									<asp:Literal ID="litDeletePage" runat="server" />
								</a>
							</li>
						</ul>
						<input id="hdnSelPage" type="hidden" value="-1" />
					</div>
					<div class="settingrow inforow">
						<portal:mojoHelpLink ID="MojoHelpLink4" runat="server" HelpKey="sts-page-menager-help" />
						<asp:Literal ID="litDemoInfo" runat="server" Visible="false" EnableViewState="false" />
					</div>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
			
		</portal:InnerWrapperPanel>
		
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

