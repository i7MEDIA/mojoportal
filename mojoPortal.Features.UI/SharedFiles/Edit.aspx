<%@ Page Language="c#" CodeBehind="Edit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.SharedFilesUI.SharedFilesEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
	<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
		<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper sharedfiles">
			<portal:HeadingControl ID="heading" runat="server" />

			<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
				<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
					<asp:Panel ID="pnlNotFound" runat="server" Visible="true">
						<mp:SiteLabel ID="Sitelabel1" runat="server" ConfigKey="SharedFilesNotFoundMessage" ResourceFile="SharedFileResources" UseLabelTag="false" />
					</asp:Panel>

					<asp:Panel ID="pnlFolder" runat="server" DefaultButton="btnUpdateFolder">
						<div class="settingrow">
							<mp:SiteLabel ID="Sitelabel10" runat="server" ForControl="ddFolderList" CssClass="settinglabel" ConfigKey="SharedFilesFolderParentLabel" ResourceFile="SharedFileResources" />
							<asp:DropDownList ID="ddFolderList" runat="server" EnableTheming="false" DataValueField="FolderID" DataTextField="FolderName" CssClass="forminput" />
						</div>

						<div class="settingrow">
							<mp:SiteLabel ID="Sitelabel8" runat="server" ForControl="txtFolderName" CssClass="settinglabel" ConfigKey="SharedFilesFolderNameLabel" ResourceFile="SharedFileResources" />
							<asp:TextBox ID="txtFolderName" runat="server" Columns="45" MaxLength="255" CssClass="forminput" />
						</div>

						<portal:FormGroupPanel runat="server">
							<mp:SiteLabel runat="server" ID="lblRolesThatCanView1" CssClass="settinglabel" ResourceFile="SharedFileResources" ConfigKey="RolesThatCanViewThisItem" />
							<asp:CheckBoxList runat="server" ID="cblRolesThatCanViewFolder" />
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server">
							<mp:SiteLabel runat="server" ForControl="cbPushRolesToChildren" CssClass="settinglabel" ConfigKey="SetChildrensRolesToParentFolder" ResourceFile="SharedFileResources" />
							<asp:CheckBox runat="server" ID="cbPushRolesToChildren" />
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server">
							<portal:mojoButton ID="btnUpdateFolder" runat="server" SkinID="SaveButton" />
							<portal:mojoButton ID="btnDeleteFolder" runat="server" CausesValidation="false" SkinID="DeleteButton" />
							<asp:HyperLink ID="lnkCancelFolder" runat="server" CssClass="cancellink" SkinID="CancelButton" />
						</portal:FormGroupPanel>
					</asp:Panel>

					<asp:Panel ID="pnlFile" runat="server" Visible="false" DefaultButton="btnUpdateFile">
						<div class="settingrow">
							<mp:SiteLabel ID="lblUploadDateLabel" runat="server" CssClass="settinglabel" ConfigKey="SharedFilesUploadDateLabel" ResourceFile="SharedFileResources" />
							<asp:Label ID="lblUploadDate" runat="server" />
						</div>

						<div class="settingrow">
							<mp:SiteLabel ID="lblUploadByLabel" runat="server" CssClass="settinglabel" ConfigKey="SharedFilesUploadByLabel" ResourceFile="SharedFileResources" />
							<asp:Label ID="lblUploadBy" runat="server" />
						</div>

						<div class="settingrow">
							<mp:SiteLabel ID="lblFileNameLabel" runat="server" ForControl="txtFriendlyName" CssClass="settinglabel" ConfigKey="SharedFilesFileNameLabel" ResourceFile="SharedFileResources" />
							<asp:TextBox ID="txtFriendlyName" runat="server" Columns="45" MaxLength="255" CssClass="forminput widetextbox" />
						</div>

						<div class="settingrow">
							<mp:SiteLabel ID="lblFileSizeLabel" runat="server" CssClass="settinglabel" ConfigKey="SharedFilesFileSizeLabel" ResourceFile="SharedFileResources" />
							<asp:Label ID="lblFileSize" runat="server" CssClass="Normal" />
						</div>

						<div class="settingrow">
							<mp:SiteLabel ID="SiteLabel7" runat="server" CssClass="settinglabel" ConfigKey="FileDescription" ResourceFile="SharedFileResources" UseLabelTag="false" />
						</div>

						<div class="settingrow">
							<mpe:EditorControl ID="edDescription" runat="server" />
						</div>

						<div class="settingrow">
							<mp:SiteLabel ID="Sitelabel2" runat="server" ForControl="ddFolders" CssClass="settinglabel" ConfigKey="SharedFilesFolderLabel" ResourceFile="SharedFileResources" />
							<asp:DropDownList ID="ddFolders" runat="server" EnableTheming="false" DataValueField="FolderID" DataTextField="FolderName" CssClass="forminput" />
						</div>

						<div class="settingrow">
							<asp:HiddenField ID="hdnCurrentFolderId" runat="server" Value="-1" EnableViewState="false" />
							<portal:jQueryFileUpload ID="uploader" runat="server" CssClass="forminput" />
							<portal:mojoButton ID="btnUpload" runat="server" Text="Upload" />
						</div>

						<portal:FormGroupPanel runat="server">
							<mp:SiteLabel runat="server" ID="lblRolesThatCanViewFile" CssClass="settinglabel" ResourceFile="SharedFileResources" ConfigKey="RolesThatCanViewThisItem" />
							<asp:CheckBoxList runat="server" ID="cblRolesThatCanViewFile" />
						</portal:FormGroupPanel>

						<portal:FormGroupPanel runat="server">
							<portal:mojoButton ID="btnUpdateFile" runat="server" SkinID="SaveButton" />
							<portal:mojoButton ID="btnDeleteFile" runat="server" CausesValidation="false" SkinID="DeleteButton" />
							<asp:HyperLink ID="lnkCancelFile" runat="server" CssClass="cancellink" SkinID="CancelButton" />
						</portal:FormGroupPanel>

						<div class="settingrow">
							<portal:mojoLabel ID="lblError" runat="server" CssClass="txterror" />
						</div>

						<div id="divHistory" runat="server">
							<mp:SiteLabel ID="Sitelabel4" runat="server" ConfigKey="SharedFilesHistoryLabel" ResourceFile="SharedFileResources" />
							<mp:mojoGridView ID="grdHistory" runat="server"
								CssClass=""
								AutoGenerateColumns="false"
								DataKeyNames="ID">
								<Columns>
									<asp:TemplateField>
										<ItemTemplate>
											<asp:Button runat="server"
												ID="lnkName"
												CssClass="FileManager buttonlink"
												Text='<%# DataBinder.Eval(Container.DataItem,"FriendlyName") %>'
												CommandName="download"
												CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ID") %>'
												CausesValidation="false" />
										</ItemTemplate>
									</asp:TemplateField>

									<asp:TemplateField>
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem,"SizeInKB") %>
										</ItemTemplate>
									</asp:TemplateField>

									<asp:TemplateField>
										<ItemTemplate>
											<%# FormatDate(Convert.ToDateTime(Eval("UploadDate"))) %>
										</ItemTemplate>
									</asp:TemplateField>

									<asp:TemplateField>
										<ItemTemplate>
											<%# FormatDate(Convert.ToDateTime(Eval("ArchiveDate")))%>
										</ItemTemplate>
									</asp:TemplateField>

									<asp:TemplateField>
										<ItemTemplate>
											<asp:Button runat="server"
												ID="LinkButton1"
												CssClass="buttonlink"
												CommandName="restore"
												CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ID") %>'
												CausesValidation="false"
												Text="<%# Resources.SharedFileResources.SharedFilesRestoreLabel %>"
												SkinID="WarningButtonSmall" />
											<asp:Button runat="server"
												ID="Button1"
												CssClass="buttonlink"
												CommandName="deletehx"
												CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ID") %>'
												CausesValidation="false"
												Text="<%# Resources.SharedFileResources.SharedFilesDeleteButton %>"
												SkinID="DeleteButtonSmall" />
										</ItemTemplate>
									</asp:TemplateField>
								</Columns>
							</mp:mojoGridView>
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
