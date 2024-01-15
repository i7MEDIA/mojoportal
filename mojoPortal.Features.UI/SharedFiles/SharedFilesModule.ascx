<%@ Control Language="c#" AutoEventWireup="true" CodeBehind="SharedFilesModule.ascx.cs" Inherits="mojoPortal.Web.SharedFilesUI.SharedFilesModule" %>
<%@ Register Namespace="mojoPortal.Web.SharedFilesUI" Assembly="mojoPortal.Features.UI" TagPrefix="sf" %>

<sf:SharedFilesDisplaySettings ID="displaySettings" runat="server" />

<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
	<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper sharedfiles">
		<portal:ModuleTitleControl ID="Title1" runat="server" />
		<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
			<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
				<asp:UpdatePanel ID="upFiles" UpdateMode="Conditional" runat="server">
					<ContentTemplate>
						<asp:Panel ID="pnlFile" runat="server">
							<table class="FileManager_table" border="0">
								<tr>
									<td>
										<asp:ImageButton runat="server"
											ID="btnDelete"
											EnableViewState="false"
											OnClick="btnDelete_Click"
											CssClass="deleteitem"
											AlternateText="Delete"
											ImageUrl="~/Data/SiteImages/delete.png"
											ToolTip="<%# Resources.SharedFileResources.SharedFilesDeleteButton %>" />

										<asp:ImageButton runat="server"
											ID="btnGoUp"
											EnableViewState="false"
											CssClass="folderup"
											OnClick="btnGoUp_Click"
											AlternateText=""
											ImageUrl="~/Data/SiteImages/folder-up-icon.png"
											Visible="false" />

										<asp:Label runat="server"
											ID="lblCurrentDirectory"
											EnableViewState="false"
											CssClass="foldername" />

										<asp:Repeater runat="server" ID="rptFoldersLinks" EnableViewState="true">
											<HeaderTemplate>
												<asp:LinkButton runat="server"
													ID="lbFolderItem"
													Text='<%# RootLabel %>'
													CommandArgument='-1'
													CommandName="FolderCliked"
													OnCommand="lbFolderItem_Command" />
											</HeaderTemplate>
											<ItemTemplate>
												<asp:Literal runat="server" ID="ltSeparator" Text='<%# displaySettings.PathSeparator %>' />
												<asp:LinkButton runat="server"
													ID="lbFolderItem"
													Text='<%# Eval("FolderName") %>'
													CommandArgument='<%# Eval("FolderId") %>'
													CommandName="FolderCliked"
													OnCommand="lbFolderItem_Command" />
											</ItemTemplate>
										</asp:Repeater>

										<asp:Label ID="lblError" runat="server" EnableViewState="false" CssClass="txterror" />
									</td>
								</tr>
								<tr>
									<td>
										<mp:mojoGridView runat="server"
											ID="dgFile"
											DataKeyNames="ID"
											SkinID="SharedFiles"
											AllowSorting="True"
											OnRowCancelingEdit="dgFile_RowCancelingEdit"
											OnRowCommand="dgFile_RowCommand"
											OnRowDataBound="dgFile_RowDataBound"
											OnRowEditing="dgFile_RowEditing"
											OnRowUpdating="dgFile_RowUpdating"
											OnSorting="dgFile_Sorting"
											AutoGenerateColumns="False">
											<Columns>
												<asp:TemplateField ItemStyle-CssClass="col-edit">
													<ItemTemplate>
														<asp:CheckBox runat="server"
															ID="chkChecked"
															EnableViewState="false"
															Visible='<%# IsEditable%>' />

														<asp:HyperLink runat="server"
															EnableViewState="false"
															Text="<%# Resources.SharedFileResources.SharedFilesEditLink %>"
															ToolTip="<%# Resources.SharedFileResources.SharedFilesEditLink %>"
															ID="editLink"
															ImageUrl='<%# this.ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>'
															NavigateUrl='<%# this.SiteRoot + "/SharedFiles/Edit.aspx?pageid=" + PageId.ToString() + "&ItemID=" + DataBinder.Eval(Container.DataItem,"ID") + "&mid=" + ModuleId.ToString()  %>'
															Visible="<%# IsEditable %>" />

														<asp:Literal runat="server"
															ID="litDownloadLink1"
															EnableViewState="false"
															Text='<%# BuildDownloadLink(Eval("ID").ToString(),Eval("filename").ToString(), Eval("type").ToString(), true )%>' />
													</ItemTemplate>
												</asp:TemplateField>

												<asp:TemplateField SortExpression="filename" ItemStyle-CssClass="col-filename">
													<ItemTemplate>
														<asp:PlaceHolder ID="plhImgEdit" runat="server" />

														<asp:Image runat="server"
															ID="imgType"
															EnableViewState="false"
															AlternateText=" "
															ImageUrl="~/Data/SiteImages/Icons/unknown.gif" />

														<asp:Button runat="server"
															ID="lnkName"
															CssClass="buttonlink"
															Text='<%# DataBinder.Eval(Container.DataItem,"filename") %>'
															CommandName="ItemClicked"
															CommandArgument='<%# Eval("ID") %>'
															CausesValidation="false"
															Visible='<%# (DataBinder.Eval(Container.DataItem,"type").ToString().ToLower() != "1") %>' />

														<asp:Literal runat="server"
															ID="litDownloadLink"
															EnableViewState="false"
															Text='<%# BuildDownloadLink(Eval("ID").ToString(),Eval("filename").ToString(), Eval("type").ToString(), false )%>' />
													</ItemTemplate>

													<EditItemTemplate>
														<asp:Panel ID="PnlRename" runat="server" DefaultButton="btnRename">
															<asp:PlaceHolder ID="Placeholder1" runat="server" />
															<asp:Image runat="server"
																ID="imgEditType"
																EnableViewState="false"
																ImageUrl="~/Data/SiteImages/Icons/unknown.gif" />

															<asp:TextBox runat="server"
																ID="txtEditName"
																EnableViewState="false"
																Columns="50"
																Text='<%# DataBinder.Eval(Container.DataItem,"filename") %>' />
														</asp:Panel>
													</EditItemTemplate>
												</asp:TemplateField>

												<asp:TemplateField SortExpression="Description" ItemStyle-CssClass="col-description">
													<ItemTemplate>
														<asp:Literal runat="server"
															ID="litDescription"
															EnableViewState="false"
															Text='<%# DataBinder.Eval(Container.DataItem,"Description") %>' />
													</ItemTemplate>
												</asp:TemplateField>

												<asp:TemplateField SortExpression="size" ItemStyle-CssClass="col-filesize">
													<ItemTemplate>
														<asp:Literal runat="server"
															ID="litSize"
															EnableViewState="false"
															Text='<%# DataBinder.Eval(Container.DataItem,"size") %>' />
													</ItemTemplate>
												</asp:TemplateField>

												<asp:TemplateField SortExpression="DownloadCount" ItemStyle-CssClass="col-downloadcount">
													<ItemTemplate>
														<asp:Literal runat="server"
															ID="litDownloadCount"
															EnableViewState="false"
															Text='<%# DataBinder.Eval(Container.DataItem, "DownloadCount")%>' />
													</ItemTemplate>
												</asp:TemplateField>

												<asp:TemplateField SortExpression="modified" ItemStyle-CssClass="col-modifieddate">
													<ItemTemplate>
														<asp:Literal runat="server"
															ID="litMod"
															EnableViewState="false"
															Text='<%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(((System.Data.DataRowView)Container.DataItem),"modified", TimeOffset, timeZone)%>' />
													</ItemTemplate>
												</asp:TemplateField>

												<asp:TemplateField SortExpression="username" ItemStyle-CssClass="col-uploadedby">
													<ItemTemplate>
														<asp:Literal runat="server"
															ID="litUser"
															EnableViewState="false"
															Text='<%# DataBinder.Eval(Container.DataItem, "username")%>' />
													</ItemTemplate>
												</asp:TemplateField>

												<asp:TemplateField ItemStyle-CssClass="col-rename">
													<ItemTemplate>
														<asp:Button runat="server"
															ID="LinkButton1"
															EnableViewState="false"
															Visible='<%# IsEditable%>'
															CssClass="buttonlink"
															CommandName="Edit"
															CommandArgument='<%# Eval("ID") %>'
															CausesValidation="false"
															Text="<%# Resources.SharedFileResources.FileManagerRename %>" />
													</ItemTemplate>

													<EditItemTemplate>
														<asp:Button runat="server"
															ID="btnRename"
															EnableViewState="false"
															CommandName="Update"
															CommandArgument='<%# Eval("ID") %>'
															Text="<%# Resources.SharedFileResources.SharedFilesUpdateButton %>" />
														<asp:Button runat="server"
															ID="LinkButton2"
															CommandName="Cancel"
															CausesValidation="false"
															Text="<%# Resources.SharedFileResources.SharedFilesCancelButton %>" />
													</EditItemTemplate>
												</asp:TemplateField>
											</Columns>
										</mp:mojoGridView>
										<asp:HiddenField ID="hdnCurrentFolderId" runat="server" Value="-1" EnableViewState="false" />
									</td>
								</tr>
								<tr id="trObjectCount" runat="server" enableviewstate="false" class="trfilecount">
									<td>
										<asp:Label ID="lblCounter" runat="server" EnableViewState="false" CssClass="sfcount" />
									</td>
								</tr>
							</table>
						</asp:Panel>

						<portal:FormGroupPanel runat="server"
							ID="fgpNewFolder"
							DefaultButton="btnNewFolder"
							CssClass="newfolderpanel"
							SkinID="SharedFilesNewFolderPanel">
							<asp:TextBox runat="server"
								ID="txtNewDirectory"
								SkinID="SharedFilesNewFolderInput" />

							<portal:mojoButton runat="server"
								ID="btnNewFolder"
								Text=""
								OnClick="btnNewFolder_Click"
								SkinID="SharedFilesNewFolderButton" />
						</portal:FormGroupPanel>

						<asp:ImageButton runat="server"
							ID="btnRefresh"
							OnClick="btnRefresh_Click"
							TabIndex="-1" />

						<portal:jQueryFileUpload runat="server" ID="uploader" />

						<portal:mojoButton runat="server"
							ID="btnUpload2"
							Text="Upload"
							OnClick="btnUpload_Click"
							EnableViewState="false" />
					</ContentTemplate>
				</asp:UpdatePanel>
			</portal:InnerBodyPanel>
		</portal:OuterBodyPanel>
	</portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>