<%@ Page Language="c#" CodeBehind="PageLayout.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.AdminUI.PageLayout" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin pagelayout">
			<portal:HeadingControl ID="heading" runat="server" />
			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<asp:Panel ID="pnlContent" runat="server" Visible="False" DefaultButton="btnCreateNewContent">
						<portal:PageLayoutDisplaySettings ID="displaySettings" runat="server" />

						<%-- Inport SVG Sprite --%>
						<portal:EmbedSVGSprite runat="server" FileName="custom-fontawesome-sprite.html" />

						<div id="divAdminLinks" runat="server">
							<asp:HyperLink ID="lnkEditSettings" EnableViewState="false" runat="server" />
							<asp:Literal ID="litLinkSpacer1" runat="server" EnableViewState="false" />
							<asp:HyperLink ID="lnkViewPage" runat="server" EnableViewState="false" />
							<asp:Literal ID="litLinkSpacer2" runat="server" EnableViewState="false" />
							<asp:HyperLink ID="lnkPageTree" runat="server" />
						</div>

						<asp:UpdatePanel ID="upLayout" UpdateMode="Conditional" runat="server">
							<ContentTemplate>
								<div class="settings">
									<div class="addcontent">
										<strong><mp:SiteLabel ID="lblAddModule" runat="server" ConfigKey="PageLayoutAddModuleLabel" UseLabelTag="false" /></strong>

										<div class="settingrow">
											<mp:SiteLabel ID="lblModuleType" runat="server" ForControl="moduleType" CssClass="settinglabel" ConfigKey="PageLayoutModuleTypeLabel" />
											<asp:DropDownList ID="moduleType" runat="server" EnableTheming="false" CssClass="forminput" DataValueField="ModuleDefID" DataTextField="FeatureName" />
											<portal:mojoHelpLink ID="MojoHelpLink1" runat="server" HelpKey="pagelayoutmoduletypehelp" />
										</div>

										<div class="settingrow">
											<mp:SiteLabel ID="lblModuleName" runat="server" ForControl="moduleTitle" CssClass="settinglabel" ConfigKey="PageLayoutModuleNameLabel" />
											<asp:TextBox ID="moduleTitle" runat="server" CssClass="widetextbox forminput" Text="" EnableViewState="false" />
											<portal:mojoHelpLink ID="MojoHelpLink2" runat="server" HelpKey="pagelayoutmodulenamehelp" />
											<asp:RequiredFieldValidator ID="reqModuleTitle" runat="server" ControlToValidate="moduleTitle" ValidationGroup="pagelayout" />
											<asp:CompareValidator ID="cvModuleTitle" runat="server" Operator="NotEqual" ControlToValidate="moduleTitle" ValidationGroup="pagelayout" />
										</div>

										<div class="settingrow">
											<mp:SiteLabel ID="SiteLabel2" runat="server" ForControl="ddPaneNames" CssClass="settinglabel" ConfigKey="PageLayoutLocationLabel" />
											<asp:DropDownList ID="ddPaneNames" runat="server" EnableTheming="false" CssClass="forminput" DataTextField="key" DataValueField="value" />
											<portal:mojoHelpLink ID="MojoHelpLink3" runat="server" HelpKey="pagelayoutmodulelocationhelp" />
											<asp:HyperLink ID="lnkGlobalContent" runat="server" Visible="false"/>
											<asp:HiddenField ID="hdnModuleID" runat="server" />
											<asp:ImageButton ID="btnAddExisting" runat="server" />
										</div>

										<div class="settingrow">
											<mp:SiteLabel ID="lblOrganizeModules" runat="server" CssClass="settinglabel" ConfigKey="EmptyLabel" UseLabelTag="false" />
											<portal:mojoButton ID="btnCreateNewContent" runat="server" CssClass="forminput" ValidationGroup="pagelayout" />
										</div>

										<div class="settingrow pageditnotes">
											<asp:Literal ID="litEditNotes" runat="server" />
										</div>
									</div>

									<div class="panelayout">
										<%-- Alt Content Notice --%>
										<asp:Panel class="altlayoutnotice" ID="divAltLayoutNotice" runat="server" SkinID="notice">
											<asp:Literal ID="litAltLayoutNotice" runat="server" />
										</asp:Panel>

										<%-- Alt Content 1 Pane --%>
										<portal:BasePanel runat="server" id="pnlAlt1LayoutPane" ClientIDMode="Static" RenderId="false">
											<h2 class="pagelayout__panel-title pagelayout__panel-title--alt1"><mp:SiteLabel ID="lblAltPanel1" runat="server" ConfigKey="PageLayoutAltPanel1Label" UseLabelTag="false" /></h2>

											<portal:BasePanel runat="server" id="pnlPaneListBox1" ClientIDMode="Static" RenderId="false">
												<asp:ListBox ID="lbAltContent1" runat="server" DataValueField="ModuleID" DataTextField="ModuleTitle" Rows="7" />

												<portal:BasePanel runat="server" ID="pnlAlt1ItemButtons" SkinID="PageLayoutItemButtons" RenderId="false">
													<button type="button" runat="server" id="btnAlt1MoveUp" ClientIDMode="Static"></button>
													<button type="button" runat="server" id="btnAlt1MoveDown" ClientIDMode="Static"></button>
													<button type="button" runat="server" id="btnMoveAlt1ToCenter" ClientIDMode="Static"></button>
													<asp:Literal runat="server" ID="litButtonSeparator1" />
													<button type="button" runat="server" id="btnEditAlt1" ClientIDMode="Static" data-panel="lbAltContent1"></button>
													<button type="button" runat="server" id="btnDeleteAlt1" ClientIDMode="Static" data-panel="lbAltContent1"></button>
												</portal:BasePanel>
											</portal:BasePanel>
										</portal:BasePanel>

										<portal:BasePanel runat="server" ID="pnlRegularLayoutPanesWrap" ClientIDMode="Static" RenderId="false">
											
											<%-- Left Pane --%>
											<portal:BasePanel runat="server" ID="pnlRegularLayoutPaneLeft" ClientIDMode="Static" RenderId="false">
												<h2 class="pagelayout__panel-title pagelayout__panel-title--left"><mp:SiteLabel ID="lblLeftPane" runat="server" ConfigKey="PageLayoutLeftPaneLabel" UseLabelTag="false" /></h2>

												<portal:BasePanel runat="server" id="pnlPaneListBox2" ClientIDMode="Static" RenderId="false">
													<asp:ListBox ID="leftPane" runat="server" DataValueField="ModuleID" DataTextField="ModuleTitle" Rows="10" />

													<portal:BasePanel ID="pnlLeftItemButtons" runat="server" SkinID="PageLayoutItemButtons" RenderId="false">
														<button type="button" runat="server" id="LeftUpBtn" ClientIDMode="Static"></button>
														<button type="button" runat="server" id="LeftDownBtn" ClientIDMode="Static"></button>
														<button type="button" runat="server" id="LeftRightBtn" ClientIDMode="Static"></button>
														<asp:Literal runat="server" ID="litButtonSeparator2" />
														<button type="button" runat="server" id="LeftEditBtn" ClientIDMode="Static" data-panel="LeftPane"></button>
														<button type="button" runat="server" id="LeftDeleteBtn" ClientIDMode="Static" data-panel="LeftPane"></button>
													</portal:BasePanel>
												</portal:BasePanel>
											</portal:BasePanel>

											<%-- Center Pane --%>
											<portal:BasePanel runat="server" ID="pnlRegularLayoutPaneCenter" ClientIDMode="Static" RenderId="false">
												<h2 class="pagelayout__panel-title pagelayout__panel-title--center"><mp:SiteLabel ID="lblContentPane" runat="server" ConfigKey="PageLayoutContentPaneLabel" UseLabelTag="false" /></h2>

												<portal:BasePanel runat="server" id="pnlPaneListBox3" ClientIDMode="Static" RenderId="false">
													<asp:ListBox ID="contentPane" runat="server" DataValueField="ModuleID" DataTextField="ModuleTitle" Rows="10" />

													<portal:BasePanel ID="pnlCenterItemButtons" runat="server" SkinID="PageLayoutItemButtons" RenderId="false">
														<button type="button" runat="server" id="ContentUpBtn" ClientIDMode="Static"></button>
														<button type="button" runat="server" id="ContentDownBtn" ClientIDMode="Static"></button>
														<button type="button" runat="server" id="ContentLeftBtn" ClientIDMode="Static"></button>
														<button type="button" runat="server" id="ContentRightBtn" ClientIDMode="Static"></button>
														<button type="button" runat="server" id="ContentUpToNextButton" ClientIDMode="Static"></button>
														<button type="button" runat="server" id="ContentDownToNextButton" ClientIDMode="Static"></button>
														<asp:Literal runat="server" ID="litButtonSeparator3" />
														<button type="button" runat="server" id="ContentEditBtn" ClientIDMode="Static" data-panel="ContentPane"></button>
														<button type="button" runat="server" id="ContentDeleteBtn" ClientIDMode="Static" data-panel="ContentPane"></button>
													</portal:BasePanel>
												</portal:BasePanel>
											</portal:BasePanel>

											<%-- Right Pane --%>
											<portal:BasePanel runat="server" ID="pnlRegularLayoutPaneRight" ClientIDMode="Static" RenderId="false">
												<h2 class="pagelayout__panel-title pagelayout__panel-title--right"><mp:SiteLabel ID="lblRightPane" runat="server" ConfigKey="PageLayoutRightPaneLabel" UseLabelTag="false" /></h2>

												<portal:BasePanel runat="server" id="pnlPaneListBox4" ClientIDMode="Static" RenderId="false">
													<asp:ListBox ID="rightPane" runat="server" DataValueField="ModuleID" DataTextField="ModuleTitle" Rows="10" />
													
													<portal:BasePanel ID="pnlRightItemButtons" runat="server" SkinID="PageLayoutItemButtons" RenderId="false">
														<button type="button" runat="server" id="RightUpBtn" ClientIDMode="Static"></button>
														<button type="button" runat="server" id="RightDownBtn" ClientIDMode="Static"></button>
														<button type="button" runat="server" id="RightLeftBtn" ClientIDMode="Static"></button>
														<asp:Literal runat="server" ID="litButtonSeparator4" />
														<button type="button" runat="server" id="RightEditBtn" ClientIDMode="Static" data-panel="RightPane"></button>
														<button type="button" runat="server" id="RightDeleteBtn" ClientIDMode="Static" data-panel="RightPane"></button>
													</portal:BasePanel>
												</portal:BasePanel>
											</portal:BasePanel>

										</portal:BasePanel>

										<%-- Alt Content 2 Pane --%>
										<portal:BasePanel runat="server" id="pnlAlt2LayoutPane" ClientIDMode="Static" RenderId="false">
											<h2 class="pagelayout__panel-title pagelayout__panel-title--alt2"><mp:SiteLabel ID="lblAltLayout2" runat="server" ConfigKey="PageLayoutAltPanel2Label" UseLabelTag="false" /></h2>

											<portal:BasePanel runat="server" id="pnlPaneListBox5" ClientIDMode="Static" RenderId="false">
												<asp:ListBox ID="lbAltContent2" runat="server" DataValueField="ModuleID" DataTextField="ModuleTitle" Rows="7" />

												<portal:BasePanel ID="pnlAlt2ItemButtons" runat="server" SkinID="PageLayoutItemButtons" RenderId="false">
													<button type="button" runat="server" id="btnAlt2MoveUp" ClientIDMode="Static"></button>
													<button type="button" runat="server" id="btnAlt2MoveDown" ClientIDMode="Static"></button>
													<button type="button" runat="server" id="btnMoveAlt2ToCenter" ClientIDMode="Static"></button>
													<asp:Literal runat="server" ID="litButtonSeparator5" />
													<button type="button" runat="server" id="btnEditAlt2" ClientIDMode="Static" data-panel="lbAltContent2"></button>
													<button type="button" runat="server" id="btnDeleteAlt2" ClientIDMode="Static" data-panel="lbAltContent2"></button>
												</portal:BasePanel>
											</portal:BasePanel>
										</portal:BasePanel>
									</div>
								</div>
							</ContentTemplate>
						</asp:UpdatePanel>
					</asp:Panel>
				</portal:InnerBodyPanel>
			</portal:OuterBodyPanel>
			<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
		</portal:InnerWrapperPanel>
	</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
