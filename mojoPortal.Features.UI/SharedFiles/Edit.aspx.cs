// Created:       2005-01-09
// Last Modified: 2018-07-23
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Features.Business.SharedFiles.Models;
using mojoPortal.FileSystem;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.SharedFilesUI
{

	public partial class SharedFilesEdit : NonCmsBasePage
	{
		private IFileSystem fileSystem = null;
		private int pageId = 0;
		private int moduleId = 0;
		private string strItem = string.Empty;
		private int itemId = 0;
		private string type = string.Empty;
		private string virtualSourcePath;
		private string virtualHistoryPath;
		protected TimeZoneInfo timeZone = null;
		private SharedFilesConfiguration config = new SharedFilesConfiguration();

		protected Double TimeOffset { get; private set; } = 0;


		#region OnInit

		protected override void OnPreInit(EventArgs e)
		{
			AllowSkinOverride = true;

			base.OnPreInit(e);
		}

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);

			Load += new EventHandler(Page_Load);
			btnUpload.Click += new EventHandler(btnUpload_Click);
			btnUpdateFile.Click += new EventHandler(btnUpdateFile_Click);
			btnDeleteFile.Click += new EventHandler(btnDeleteFile_Click);
			btnUpdateFolder.Click += new EventHandler(btnUpdateFolder_Click);
			btnDeleteFolder.Click += new EventHandler(btnDeleteFolder_Click);

			grdHistory.RowCommand += new GridViewCommandEventHandler(grdHistory_RowCommand);

			SiteUtils.SetupEditor(edDescription, AllowSkinOverride, this);
			ScriptConfig.IncludeJQTable = true;
		}

		#endregion


		private void Page_Load(object sender, EventArgs e)
		{
			EnableViewState = true;

			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);

				return;
			}

			SecurityHelper.DisableBrowserCache();

			LoadParams();

			if (!UserCanEditModule(moduleId, SharedFile.FeatureGuid))
			{
				SiteUtils.RedirectToAccessDeniedPage(this);

				return;
			}

			if (SiteUtils.IsFishyPost(this))
			{
				SiteUtils.RedirectToAccessDeniedPage(this);

				return;
			}

			LoadSettings();
			PopulateLabels();

			if (!IsPostBack)
			{
				if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
				{
					hdnReturnUrl.Value = Request.UrlReferrer.ToString();
					lnkCancelFile.NavigateUrl = hdnReturnUrl.Value;
					lnkCancelFolder.NavigateUrl = hdnReturnUrl.Value;
				}

				PopulateControls();
			}
		}


		private void PopulateControls()
		{
			if ((moduleId > 0) && (itemId > 0) && ((type == "folder") || (type == "file")))
			{
				if (type == "folder")
				{
					PopulateFolderControls();
				}

				if (type == "file")
				{
					PopulateFileControls();
				}
			}
			else
			{
				pnlNotFound.Visible = true;
				pnlFolder.Visible = false;
				pnlFile.Visible = false;
			}
		}


		private void PopulateFolderControls()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, SharedFileResources.SharedFilesEditFolderLabel);
			heading.Text = SharedFileResources.SharedFilesEditFolderLabel;
			SharedFileFolder folder = new SharedFileFolder(moduleId, itemId);

			if ((folder.FolderId > 0) && (folder.ModuleId == moduleId))
			{
				pnlNotFound.Visible = false;
				pnlFile.Visible = false;
				pnlFolder.Visible = true;

				txtFolderName.Text = folder.FolderName;

				List<SharedFileFolder> allFolders = SharedFileFolder.GetSharedModuleFolderList(folder.ModuleId);

				ddFolderList.DataSource = allFolders;
				ddFolderList.DataBind();

				ddFolderList.Items.Insert(0, new ListItem("Root", "-1"));
				ddFolderList.SelectedValue = folder.ParentId.ToString();


				PopulateAllowedRolesList(cblRolesThatCanViewFolder, folder.ViewRoles);

				// prevent a folder from being its own parent
				ListItem item = ddFolderList.Items.FindByText(folder.FolderName);

				if (item != null) ddFolderList.Items.Remove(item);


				//// prevent a child folder from being parent
				//// build list
				List<int> toRemove = new List<int>();

				foreach (ListItem fldItem in ddFolderList.Items)
				{
					SharedFileFolder currentFolder = SharedFilesHelper.GetFolderFromListById(Convert.ToInt32(fldItem.Value), allFolders);

					if (currentFolder != null)
					{
						if (SharedFilesHelper.GetAllParentsFolderIds(currentFolder, allFolders).Contains(folder.FolderId))
						{
							toRemove.Add(currentFolder.FolderId);
						}
					}
				}

				// remove list
				foreach (int itemToRemove in toRemove)
				{
					ddFolderList.Items.Remove(ddFolderList.Items.FindByValue(itemToRemove.ToInvariantString()));
				}
			}
			else
			{
				pnlNotFound.Visible = true;
				pnlFolder.Visible = false;
				pnlFile.Visible = false;
			}
		}


		private void PopulateFileControls()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, SharedFileResources.SharedFilesEditLabel);
			heading.Text = SharedFileResources.SharedFilesEditLabel;

			SharedFile file = new SharedFile(moduleId, itemId);

			if ((file.ItemId > 0) && (file.ModuleId == moduleId))
			{
				pnlNotFound.Visible = false;
				pnlFolder.Visible = false;
				pnlFile.Visible = true;

				using (IDataReader reader = SharedFileFolder.GetSharedModuleFolders(file.ModuleId))
				{
					ddFolders.DataSource = reader;
					ddFolders.DataBind();
				}

				ddFolders.Items.Insert(0, new ListItem("Root", "-1"));
				ddFolders.SelectedValue = file.FolderId.ToInvariantString();

				hdnCurrentFolderId.Value = file.FolderId.ToInvariantString();

				if (timeZone != null)
				{
					lblUploadDate.Text = file.UploadDate.ToLocalTime(timeZone).ToString();
				}
				else
				{
					lblUploadDate.Text = file.UploadDate.AddHours(TimeOffset).ToString();
				}

				SiteUser uploadUser = new SiteUser(siteSettings, file.UploadUserId);
				lblUploadBy.Text = uploadUser.Name;
				lblFileSize.Text = file.SizeInKB.ToString();
				txtFriendlyName.Text = file.FriendlyName;
				edDescription.Text = file.Description;

				if (config.EnableVersioning)
				{
					using (IDataReader reader = file.GetHistory())
					{
						grdHistory.DataSource = reader;
						grdHistory.DataBind();
					}
				}

				PopulateAllowedRolesList(cblRolesThatCanViewFile, file.ViewRoles);
			}
			else
			{
				pnlNotFound.Visible = true;
				pnlFolder.Visible = false;
				pnlFile.Visible = false;
			}
		}


		private void PopulateAllowedRolesList(CheckBoxList chkAllowedRoles, string selectedRoles)
		{
			if (chkAllowedRoles == null)
			{
				chkAllowedRoles = new CheckBoxList();
			}

			// Start with no checkboxes
			chkAllowedRoles.Items.Clear();

			// Manually create the "All Users" item
			ListItem allUsersItem = new ListItem();
			allUsersItem.Text = SharedFileResources.RolesAllUsersRole;
			allUsersItem.Value = "All Users";

			// Check the "All Users" role if selectedRoles is empty or has the "All Users" role
			if (String.IsNullOrWhiteSpace(selectedRoles) || selectedRoles.Contains("All Users"))
			{
				allUsersItem.Selected = true;
			}

			// Add this role to roles
			chkAllowedRoles.Items.Add(allUsersItem);

			// Get all other roles in the database
			using (IDataReader reader = Role.GetSiteRoles(siteSettings.SiteId))
			{
				// Loop over each role
				while (reader.Read())
				{
					// Create a new role item
					ListItem listItem = new ListItem();
					listItem.Text = reader["DisplayName"].ToString();
					listItem.Value = reader["RoleName"].ToString();

					// Check this role if it's in selectedRoles
					if (selectedRoles.SplitOnCharAndTrim(';').Contains(listItem.Value))
					{
						listItem.Selected = true;
					}

					// Add this role to roles
					chkAllowedRoles.Items.Add(listItem);
				}
			}
		}


		protected string FormatDate(DateTime d)
		{
			if (timeZone != null)
			{
				return d.ToLocalTime(timeZone).ToString();
			}

			return d.AddHours(TimeOffset).ToString();
		}


		private void PopulateLabels()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, SharedFileResources.EditPageTitle);

			btnUpload.Text = SharedFileResources.FileManagerUploadButton;
			btnUpdateFile.Text = SharedFileResources.SharedFilesUpdateButton;
			btnDeleteFile.Text = SharedFileResources.SharedFilesDeleteButton;

			lnkCancelFile.Text = SharedFileResources.SharedFilesCancelButton;
			lnkCancelFolder.Text = SharedFileResources.SharedFilesCancelButton;

			UIHelper.AddConfirmationDialog(btnDeleteFile, SharedFileResources.FileManagerDeleteConfirm);
			btnUpdateFolder.Text = SharedFileResources.SharedFilesUpdateButton;
			btnDeleteFolder.Text = SharedFileResources.SharedFilesDeleteButton;
			UIHelper.AddConfirmationDialog(btnDeleteFolder, SharedFileResources.FileManagerFolderDeleteConfirm);

			grdHistory.Columns[0].HeaderText = SharedFileResources.FileManagerFileNameLabel;
			grdHistory.Columns[1].HeaderText = SharedFileResources.FileManagerSizeLabel;
			grdHistory.Columns[2].HeaderText = SharedFileResources.SharedFilesEditUploadDateLabel;
			grdHistory.Columns[3].HeaderText = SharedFileResources.SharedFilesArchiveDateLabel;

			edDescription.WebEditor.ToolBar = ToolBar.FullWithTemplates;

			uploader.AddFilesText = SharedFileResources.SelectFilesButton;
			uploader.AddFileText = SharedFileResources.SelectFileButton;
			uploader.DropFilesText = SharedFileResources.DropFiles;
			uploader.DropFileText = SharedFileResources.DropFile;
			uploader.UploadButtonText = SharedFileResources.FileManagerUploadButton;
			uploader.UploadCompleteText = SharedFileResources.UploadComplete;
			uploader.UploadingText = SharedFileResources.Uploading;
		}


		private void btnUpload_Click(object sender, EventArgs e)
		{
			if (uploader.HasFile)
			{
				SiteUser siteUser = SiteUtils.GetCurrentSiteUser();

				if (siteUser == null) return;

				SharedFile sharedFile = new SharedFile(moduleId, itemId);
				sharedFile.ContentChanged += new ContentChangedEventHandler(sharedFile_ContentChanged);

				if (config.EnableVersioning)
				{
					bool historyCreated = SharedFilesHelper.CreateHistory(sharedFile, fileSystem, virtualSourcePath, virtualHistoryPath);

					if (historyCreated)
					{
						sharedFile.ServerFileName = $"{Guid.NewGuid().ToString()}.config";
					}
				}

				sharedFile.ModuleId = moduleId;

				if (sharedFile.ModuleGuid == Guid.Empty)
				{
					Module m = GetModule(moduleId, SharedFile.FeatureGuid);
					sharedFile.ModuleGuid = m.ModuleGuid;

				}

				string fileName = Path.GetFileName(uploader.FileName);

				sharedFile.OriginalFileName = fileName;
				sharedFile.FriendlyName = fileName;
				sharedFile.SizeInKB = (int)(uploader.FileBytes.Length / 1024);
				sharedFile.UploadUserId = siteUser.UserId;
				sharedFile.UploadDate = DateTime.UtcNow;

				if (sharedFile.Save())
				{
					string destPath = virtualSourcePath + sharedFile.ServerFileName;

					using (Stream s = uploader.FileContent)
					{
						fileSystem.SaveFile(destPath, s, IOHelper.GetMimeType(Path.GetExtension(sharedFile.FriendlyName).ToLower()), true);
					}
				}

				CurrentPage.UpdateLastModifiedTime();
				CacheHelper.ClearModuleCache(moduleId);
				SiteUtils.QueueIndexing();
			}

			WebUtils.SetupRedirect(this, Request.RawUrl);
		}


		void sharedFile_ContentChanged(object sender, ContentChangedEventArgs e)
		{
			IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["SharedFilesIndexBuilderProvider"];

			if (indexBuilder != null)
			{
				indexBuilder.ContentChangedHandler(sender, e);
				SiteUtils.QueueIndexing();
			}
		}


		private void btnUpdateFile_Click(object sender, EventArgs e)
		{
			SiteUser siteUser = SiteUtils.GetCurrentSiteUser();

			if (siteUser == null) return;

			SharedFile file = new SharedFile(moduleId, itemId);
			file.ContentChanged += new ContentChangedEventHandler(sharedFile_ContentChanged);

			if ((file.ItemId > 0) && (file.ModuleId == moduleId))
			{
				file.FriendlyName = txtFriendlyName.Text;
				file.Description = edDescription.Text;
				file.FolderId = int.Parse(ddFolders.SelectedValue);

				if (file.FolderId > -1)
				{
					SharedFileFolder folder = new SharedFileFolder(moduleId, file.FolderId);
					file.FolderGuid = folder.FolderGuid;
				}

				if (file.ModuleGuid == Guid.Empty)
				{
					Module m = new Module(moduleId);
					file.ModuleGuid = m.ModuleGuid;
				}

				file.UploadUserId = siteUser.UserId;
				// really last mod date
				file.UploadDate = DateTime.UtcNow;

				List<ListItem> selectedRoles = cblRolesThatCanViewFile.Items.Cast<ListItem>()
					.Where(li => li.Selected)
					.ToList();

				string viewRoles = String.Join(";", selectedRoles.Select(x => x.Value.ToString()).ToArray());

				file.ViewRoles = viewRoles;
				file.Save();
			}

			CurrentPage.UpdateLastModifiedTime();
			CacheHelper.ClearModuleCache(moduleId);
			SiteUtils.QueueIndexing();

			if (hdnReturnUrl.Value.Length > 0)
			{
				WebUtils.SetupRedirect(this, hdnReturnUrl.Value);

				return;
			}

			WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}


		private void btnDeleteFile_Click(object sender, EventArgs e)
		{
			SharedFile sharedFile = new SharedFile(moduleId, itemId);

			if (sharedFile.ModuleId != moduleId)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);

				return;
			}

			sharedFile.ContentChanged += new ContentChangedEventHandler(sharedFile_ContentChanged);

			if (config.EnableVersioning)
			{
				SharedFilesHelper.CreateHistory(sharedFile, fileSystem, virtualSourcePath, virtualHistoryPath);
			}

			sharedFile.Delete();
			CurrentPage.UpdateLastModifiedTime();
			CacheHelper.ClearModuleCache(moduleId);
			SiteUtils.QueueIndexing();

			if (hdnReturnUrl.Value.Length > 0)
			{
				WebUtils.SetupRedirect(this, hdnReturnUrl.Value);

				return;
			}

			WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}


		private void btnUpdateFolder_Click(object sender, EventArgs e)
		{
			SharedFileFolder folder = new SharedFileFolder(moduleId, itemId);

			if ((folder.FolderId > 0) && (folder.ModuleId == moduleId))
			{
				List<ListItem> selectedRoles =
					cblRolesThatCanViewFolder.Items.Cast<ListItem>()
						.Where(li => li.Selected)
						.ToList();

				string viewRoles = String.Join(";", selectedRoles.Select(x => x.Value.ToString()).ToArray());

				folder.ViewRoles = viewRoles;

				if (cbPushRolesToChildren.Checked)
				{
					// Current Folder's (can be root) Folders and Files
					FoldersAndFiles foldersAndFiles = SharedFileFolder.GetFoldersAndFilesModel(moduleId, folder.FolderId);

					// Recursively loop through folders and get child items
					void getChildItems(List<Folder> folders)
					{
						foreach (var childFolder in folders)
						{
							FoldersAndFiles childFoldersAndFiles = SharedFileFolder.GetFoldersAndFilesModel(moduleId, childFolder.ID);

							if (childFoldersAndFiles.Folders != null && childFoldersAndFiles.Folders.Count() > 0)
							{
								// Add folders to folders list
								childFoldersAndFiles.Folders.ForEach(f =>
								{
									foldersAndFiles.Folders.Add(f);
								});

								// Recursively call this function again
								getChildItems(childFoldersAndFiles.Folders);
							}

							if (childFoldersAndFiles.Files != null && childFoldersAndFiles.Files.Count() > 0)
							{
								// Add files to files list
								childFoldersAndFiles.Files.ForEach(f =>
								{
									foldersAndFiles.Files.Add(f);
								});
							}
						}
					}

					getChildItems(foldersAndFiles.Folders);

					// Loop through all Folders and set their ViewRoles to the parent folder's
					foldersAndFiles.Folders.ForEach(f =>
					{
						var setFolder = new SharedFileFolder
						{
							FolderId = f.ID,
							ModuleId = f.ModuleID,
							FolderName = f.Name,
							ParentId = f.ParentID,
							ModuleGuid = f.ModuleGuid,
							ParentGuid = f.ParentGuid,
							ViewRoles = folder.ViewRoles
						};

						setFolder.Save();
					});

					// Loop through all Files and set their ViewRoles to the parent folder's
					foldersAndFiles.Files.ForEach(f =>
					{
						var setFile = new SharedFile
						{
							ItemId = f.ID,
							ModuleId = f.ModuleID,
							UploadUserId = f.UploadUserID,
							FriendlyName = f.Name,
							OriginalFileName = f.OriginalFileName,
							ServerFileName = f.ServerFileName,
							SizeInKB = f.SizeInKB,
							UploadDate = f.UploadDate,
							FolderId = f.FolderID,
							ModuleGuid = f.ModuleGuid,
							UserGuid = f.UserGuid,
							FolderGuid = f.FolderGuid,
							Description = f.Description,
							ViewRoles = folder.ViewRoles
						};

						setFile.Save();
					});

					// 1. Get all files for current folder
					// 2. get all folders for current folder (sql should only get folders which contain files or folders)
					// 3. set viewroles on each file/folder in current folder
					// 4. step through all folders in current folder and repeat steps 1-3

					// SQL Method?
					// 1. update all files with parentid = current folder id and moduleid = moduleid
					// 2. update all folders with parent id = current folder id and moduleid = moduleid
					// 3. update all files/folders within each folder
				}

				folder.FolderName = txtFolderName.Text;
				folder.ParentId = int.Parse(ddFolderList.SelectedValue);
				folder.Save();
			}

			CacheHelper.ClearModuleCache(moduleId);

			if (hdnReturnUrl.Value.Length > 0)
			{
				WebUtils.SetupRedirect(this, hdnReturnUrl.Value);

				return;
			}

			WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}


		private void btnDeleteFolder_Click(object sender, EventArgs e)
		{
			SharedFileFolder folder = new SharedFileFolder(this.moduleId, this.itemId);

			if ((folder.FolderId > 0) && (folder.ModuleId == this.moduleId))
			{
				SharedFilesHelper.DeleteAllFiles(folder, fileSystem, virtualSourcePath, config);
				SharedFileFolder.DeleteSharedFileFolder(this.itemId);
				CacheHelper.ClearModuleCache(moduleId);

				if (hdnReturnUrl.Value.Length > 0)
				{
					WebUtils.SetupRedirect(this, hdnReturnUrl.Value);

					return;
				}
			}

			WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}


		void grdHistory_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			int archiveID = Convert.ToInt32(e.CommandArgument);

			switch (e.CommandName)
			{
				case "restore":
					SharedFilesHelper.RestoreHistoryFile(archiveID, fileSystem, virtualSourcePath, virtualHistoryPath);
					WebUtils.SetupRedirect(this, Request.RawUrl);

					break;

				case "deletehx":
					SharedFilesHelper.DeleteHistoryFile(archiveID, fileSystem, virtualHistoryPath);
					SharedFile.DeleteHistory(archiveID);
					WebUtils.SetupRedirect(this, Request.RawUrl);

					break;

				case "download":
					SharedFileHistory historyFile = SharedFile.GetHistoryFile(archiveID);

					if (historyFile != null)
					{
						string fileType = Path.GetExtension(historyFile.FriendlyName).Replace(".", string.Empty);

						if (string.Equals(fileType, "pdf", StringComparison.InvariantCultureIgnoreCase))
						{
							//this will display the pdf right in the browser
							Page.Response.AddHeader("Content-Disposition", $"filename={historyFile.FriendlyName}");
						}
						else
						{
							// other files just use file save dialog
							Page.Response.AddHeader("Content-Disposition", $"attachment; filename={historyFile.FriendlyName}");
						}

						Page.Response.ContentType = $"application/{fileType}";
						Page.Response.Buffer = false;
						Page.Response.BufferOutput = false;

						using (Stream stream = fileSystem.GetAsStream(virtualHistoryPath + historyFile.ServerFileName))
						{
							stream.CopyTo(Page.Response.OutputStream);
						}
						try
						{
							Page.Response.End();
						}
						catch (System.Threading.ThreadAbortException) { }
					}

					break;
			}

		}


		private void LoadParams()
		{
			pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
			moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);

			if (!string.IsNullOrEmpty(HttpContext.Current.Request.Params["ItemID"]))
			{
				strItem = HttpContext.Current.Request.Params["ItemID"];
			}

			TimeOffset = SiteUtils.GetUserTimeOffset();
		}


		private void LoadSettings()
		{
			timeZone = SiteUtils.GetUserTimeZone();
			virtualSourcePath = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/SharedFiles/";
			virtualHistoryPath = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/SharedFiles/History/";

			lnkCancelFile.NavigateUrl = SiteUtils.GetCurrentPageUrl();
			lnkCancelFolder.NavigateUrl = lnkCancelFile.NavigateUrl;


			//this page handles both folders and files
			//expected strItem examples are 23~folder and 13~file
			//the number portion is the ItemID of the folder or file
			if (strItem.IndexOf("~") > -1)
			{
				try
				{
					char[] separator = { '~' };
					string[] args = strItem.Split(separator);
					itemId = int.Parse(args[0]);
					type = args[1];
				}
				catch { };

			}

			Hashtable moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
			config = new SharedFilesConfiguration(moduleSettings);

			divHistory.Visible = config.EnableVersioning;

			uploader.UploadButtonClientId = btnUpload.ClientID;
			uploader.ServiceUrl = SiteRoot + "/SharedFiles/upload.ashx?pageid=" + pageId.ToInvariantString()
				+ "&mid=" + moduleId.ToInvariantString()
				+ "&ItemID=" + itemId.ToInvariantString();

			uploader.FormFieldClientId = hdnCurrentFolderId.ClientID;

			string refreshFunction = "function refresh" + moduleId.ToInvariantString()
					+ " () { window.location.reload(true); } ";

			uploader.UploadCompleteCallback = $"refresh{moduleId.ToInvariantString()}";

			ScriptManager.RegisterClientScriptBlock(
				this,
				GetType(),
				$"refresh{moduleId.ToInvariantString()}",
				refreshFunction,
				true
			);

			AddClassToBody("sharedfilesedit");

			FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
			if (p == null) { return; }

			fileSystem = p.GetFileSystem();
		}
	}
}