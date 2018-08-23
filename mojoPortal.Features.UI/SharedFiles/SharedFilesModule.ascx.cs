// Created:       2005-01-05
// Last Modified: 2018-07-20
// 2013-04-01 replaced file upload implementation
// removed NeatUpload and integrated new jqueryfileupload

using mojoPortal.Business;
using mojoPortal.FileSystem;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.SharedFilesUI
{
	public partial class SharedFilesModule : SiteModuleControl
	{
		private string imgroot;
		protected string RootLabel = string.Empty;
		private String fileVirtualBasePath;
		private FileInfo[] iconList;
		protected Double TimeOffset = 0;
		protected TimeZoneInfo timeZone = null;
		protected SharedFilesConfiguration config = null;
		protected string EditContentImage = string.Empty;
		private string newWindowMarkup = "onclick=\"window.open(this.href,'_blank');return false;\"";
		private IFileSystem fileSystem = null;
		SiteUser siteUser = null;

		protected int CurrentFolderId
		{
			get
			{
				int i = -1;
				int.TryParse(hdnCurrentFolderId.Value, out i);

				return i;
			}
			set
			{
				hdnCurrentFolderId.Value = value.ToInvariantString();
			}
		}


		protected void Page_Load(object sender, EventArgs e)
		{
			LoadSettings();

			if (fileSystem == null) { return; }

			PopulateLabels();

			iconList = SiteUtils.GetFileIconList();

			if ((!Page.IsPostBack) && (!Page.IsAsync))
			{
				BindData();
			}
		}


		private void BindData()
		{
			if (CurrentFolderId > -1)
			{
				SharedFileFolder folder = new SharedFileFolder(ModuleId, CurrentFolderId);

				btnGoUp.Visible = true;
				rptFoldersLinks.Visible = true;

				if (displaySettings.ShowClickableFolderPathCrumbs)
				{
					lblCurrentDirectory.Visible = false;

					// by Thomas N
					List<SharedFileFolder> allFolders = SharedFileFolder.GetSharedModuleFolderList(folder.ModuleId);
					rptFoldersLinks.DataSource = SharedFilesHelper.GetAllParentsFolder(folder, allFolders);

					IEnumerable<SharedFileFolder> fullPathList = SharedFilesHelper.GetAllParentsFolder(folder, allFolders).Concat(Enumerable.Repeat(folder, 1));
					rptFoldersLinks.DataSource = fullPathList;
					rptFoldersLinks.DataBind();
				}
				else
				{
					lblCurrentDirectory.Text = folder.FolderName;
				}
			}
			else
			{
				lblCurrentDirectory.Visible = false;
				btnGoUp.Visible = false;
				rptFoldersLinks.Visible = false;
			}

			DataView dv = new DataView(SharedFileFolder.GetFoldersAndFiles(ModuleId, CurrentFolderId));

			EnumerableRowCollection<DataRow> query =
				from row in dv.Table.AsEnumerable()
				where CheckRoles(row.Field<string>("ViewRoles"))
				select row;

			DataView view = query.AsDataView();

			view.Sort = $"type ASC, filename {config.DefaultSort}";
			dgFile.DataSource = view;
			dgFile.DataBind();

			lblCounter.Text = $"{dgFile.Rows.Count.ToString()} {SharedFileResources.FileManagerObjectsLabel}";
		}


		protected bool CheckRoles(string roles)
		{
			if (roles.Contains("All Users")) return true;

			if (siteUser != null)
			{
				if (siteUser.IsInRoles("Admins")) return true;
				if (siteUser.IsInRoles(roles)) return true;
			}

			return false;
		}


		protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
		{
			BindData();
			upFiles.Update();
		}


		protected void lbFolderItem_Command(object sender, CommandEventArgs e)
		{
			CurrentFolderId = int.Parse(e.CommandArgument.ToString());
			BindData();
			upFiles.Update();
		}

		#region Grid Events


		protected void dgFile_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (sender == null) return;
			if (e == null) return;

			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				Image imgType;
				if (e.Row.RowIndex == dgFile.EditIndex)
				{
					imgType = (Image)e.Row.Cells[1].FindControl("imgEditType");
				}
				else
				{
					imgType = (Image)e.Row.Cells[1].FindControl("imgType");

				}

				if (imgType != null)
				{
					int type = int.Parse(DataBinder.Eval(e.Row.DataItem, "type", "{0}"));
					if (type == 0)
					{
						//type is folder
						imgType.ImageUrl = imgroot + "folder.png";
						imgType.AlternateText = SharedFileResources.SharedFilesFolderLabel;
					}
					else
					{
						// type is file
						string name = DataBinder.Eval(e.Row.DataItem, "OriginalFileName", "{0}").Trim();
						string imgFile = Path.GetExtension(name).ToLower().Replace(".", "") + ".png";

						if (IconExists(imgFile))
						{
							imgType.ImageUrl = imgroot + "Icons/" + imgFile;
							imgType.AlternateText = SharedFileResources.ImageFileLabel;
						}
						else
						{
							imgType.ImageUrl = imgroot + "Icons/unknown.png";
							imgType.AlternateText = SharedFileResources.FileLabel;
						}
					}
				}
			}

		}


		protected void dgFile_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName == "ItemClicked")
			{

				string keys = e.CommandArgument.ToString();
				char[] separator = { '~' };
				string[] args = keys.Split(separator);
				string type = args[1];
				dgFile.EditIndex = -1;

				if (type == "folder")
				{
					CurrentFolderId = int.Parse(args[0]);
					BindData();
					upFiles.Update();
					return;

				}

				// this isn't used since we changed to a link to download.aspx
				if (type == "file")
				{
					int fileID = int.Parse(args[0]);
					SharedFile sharedFile = new SharedFile(this.ModuleId, fileID);

					sharedFile.ContentChanged += new ContentChangedEventHandler(SharedFile_ContentChanged);

					string virtualPath = "~/Data/Sites/" + this.SiteId.ToInvariantString()
						+ "/SharedFiles/" + sharedFile.ServerFileName;

					string fileType = Path.GetExtension(sharedFile.OriginalFileName).Replace(".", string.Empty);
					string mimeType = SiteUtils.GetMimeType(fileType);
					Page.Response.ContentType = mimeType;

					if (SiteUtils.IsNonAttacmentFileType(fileType))
					{
						//this will display the pdf right in the browser
						Page.Response.AddHeader("Content-Disposition", "filename=\"" + HttpUtility.UrlEncode(sharedFile.FriendlyName.Replace(" ", string.Empty), Encoding.UTF8) + "\"");
					}
					else
					{
						// other files just use file save dialog
						Page.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(sharedFile.FriendlyName.Replace(" ", string.Empty), Encoding.UTF8) + "\"");
					}


					Page.Response.Buffer = false;
					Page.Response.BufferOutput = false;
					//Page.Response.TransmitFile(downloadPath);
					using (System.IO.Stream stream = fileSystem.GetAsStream(virtualPath))
					{
						stream.CopyTo(Page.Response.OutputStream);
					}
					try
					{
						Page.Response.End();
					}
					catch (System.Threading.ThreadAbortException) { }
				}


			}


		}


		protected void dgFile_RowUpdating(object sender, GridViewUpdateEventArgs e)
		{
			if (siteUser == null) return;

			try
			{
				GridView grid = (GridView)sender;
				TextBox txtEditName = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtEditName");
				if (txtEditName.Text.Trim().Length < 1)
					return;


				string keys = grid.DataKeys[e.RowIndex].Value.ToString();
				char[] separator = { '~' };
				string[] args = keys.Split(separator);
				string type = args[1];

				if (type == "folder")
				{
					int folderID = int.Parse(args[0]);
					SharedFileFolder folder = new SharedFileFolder(this.ModuleId, folderID);
					folder.FolderName = Path.GetFileName(txtEditName.Text);
					folder.Save();

				}

				if (type == "file")
				{
					int fileID = int.Parse(args[0]);
					SharedFile sharedFile = new SharedFile(this.ModuleId, fileID);
					sharedFile.ContentChanged += new ContentChangedEventHandler(SharedFile_ContentChanged);
					sharedFile.FriendlyName = Path.GetFileName(txtEditName.Text);
					sharedFile.UploadUserId = siteUser.UserId;
					sharedFile.UploadDate = DateTime.UtcNow; //lastModDate
					sharedFile.Save();

				}

				dgFile.EditIndex = -1;
				BindData();

			}
			catch (Exception ex)
			{
				lblError.Text = ex.Message;
			}
			upFiles.Update();
		}


		protected void dgFile_RowEditing(object sender, GridViewEditEventArgs e)
		{
			dgFile.EditIndex = e.NewEditIndex;
			BindData();
			upFiles.Update();
		}


		protected void dgFile_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
		{
			dgFile.EditIndex = -1;
			BindData();
			upFiles.Update();
		}

		protected void dgFile_Sorting(object sender, GridViewSortEventArgs e)
		{
			if (ViewState["SortBy"] == null)
			{
				ViewState["SortBy"] = "ASC";
			}
			else if (ViewState["SortBy"].ToString().Equals("ASC"))
			{
				ViewState["SortBy"] = "DESC";
			}
			else
			{
				ViewState["SortBy"] = "ASC";
			}

			DataView dv = new DataView(SharedFileFolder.GetFoldersAndFiles(ModuleId, CurrentFolderId));

			dv.Sort = e.SortExpression + " " + ViewState["SortBy"];
			dgFile.DataSource = dv;
			dgFile.DataBind();
			upFiles.Update();

		}


		protected string BuildDownloadLink(string id, string name, string fileType, bool includeImage)
		{
			if (fileType != "1") { return string.Empty; }
			string innerMarkup = name;
			if (includeImage)
			{
				innerMarkup = "<img src='" + ImageSiteRoot + "/Data/SiteImages/arrow_in_down.png' alt='" + SharedFileResources.SharedFilesDownloadLink + "' />";
			}

			return "<a href='" + SiteRoot + "/SharedFiles/Download.aspx?pageid=" + PageId.ToInvariantString()
				+ "&amp;mid=" + ModuleId.ToInvariantString()
				+ "&amp;fileid=" + id.Replace("~file", string.Empty) + "' "
				+ "title='" + (includeImage ? SharedFileResources.SharedFilesDownloadLink + " " : "") + name + "' "
				+ newWindowMarkup
				+ ">"
				+ innerMarkup
				+ "</a>";
		}

		#endregion


		protected void btnGoUp_Click(object sender, ImageClickEventArgs e)
		{
			MoveUp();
			upFiles.Update();
		}


		private void MoveUp()
		{
			if (CurrentFolderId > 0)
			{
				SharedFileFolder folder = new SharedFileFolder(ModuleId, CurrentFolderId);
				CurrentFolderId = folder.ParentId;
				BindData();
			}
			else
			{
				lblError.Text = SharedFileResources.RootDirectoryReached;
			}
		}


		protected void btnDelete_Click(object sender, ImageClickEventArgs e)
		{
			bool yes = false;

			foreach (GridViewRow dgi in dgFile.Rows)
			{
				CheckBox chkChecked = (CheckBox)dgi.Cells[0].FindControl("chkChecked");
				if ((chkChecked != null) && (chkChecked.Checked))
				{
					yes = true;
					DeleteItem(dgi);
				}
			}

			if (yes)
			{
				dgFile.PageIndex = 0;
				dgFile.EditIndex = -1;
				BindData();
			}

			upFiles.Update();
		}


		private void DeleteItem(GridViewRow e)
		{
			string keys = dgFile.DataKeys[e.RowIndex].Value.ToString();
			char[] separator = { '~' };
			string[] args = keys.Split(separator);
			string type = args[1];

			if (type == "folder")
			{
				int folderID = int.Parse(args[0]);
				SharedFileFolder folder = new SharedFileFolder(this.ModuleId, folderID);
				//folder.DeleteAllFiles(this.filePath);
				SharedFilesHelper.DeleteAllFiles(folder, fileSystem, fileVirtualBasePath, config);
				SharedFileFolder.DeleteSharedFileFolder(folderID);

				//TODO: file content changed event so re-index

			}

			if (type == "file")
			{
				int fileID = int.Parse(args[0]);
				SharedFile sharedFile = new SharedFile(this.ModuleId, fileID);

				if (!config.EnableVersioning)
				{
					fileSystem.DeleteFile(VirtualPathUtility.Combine(fileVirtualBasePath, sharedFile.ServerFileName));
				}

				sharedFile.Delete();

				sharedFile.ContentChanged += new ContentChangedEventHandler(SharedFile_ContentChanged);
			}
		}


		protected void btnNewFolder_Click(object sender, EventArgs e)
		{
			try
			{
				if (txtNewDirectory.Text.Length > 0)
				{
					SharedFileFolder folder = new SharedFileFolder();

					folder.ParentId = CurrentFolderId;
					folder.ModuleId = ModuleId;

					Module m = new Module(ModuleId);

					folder.ModuleGuid = m.ModuleGuid;
					folder.FolderName = Path.GetFileName(txtNewDirectory.Text);

					if (CurrentFolderId == -1)
					{
						folder.ViewRoles = "All Users";
					}
					else
					{
						SharedFileFolder parentFolder = new SharedFileFolder(ModuleId, CurrentFolderId);

						if (parentFolder != null)
						{
							folder.ViewRoles = parentFolder.ViewRoles;
						}
					}

					if (folder.Save())
					{
						BindData();
					}
				}
			}
			catch (Exception ex)
			{
				lblError.Text = ex.Message;
			}

			upFiles.Update();
		}


		protected void btnUpload_Click(object sender, EventArgs e)
		{
			// as long as javascript is available this code should never execute
			// because the standard file input ir replaced by javascript and the file upload happens
			// at the service url /SharedFiles/upload.ashx
			// this is fallback implementation

			if (!fileSystem.FolderExists(fileVirtualBasePath))
			{
				fileSystem.CreateFolder(fileVirtualBasePath);
			}

			SiteUser siteUser = SiteUtils.GetCurrentSiteUser();

			if (siteUser == null) { WebUtils.SetupRedirect(this, Request.RawUrl); return; }


			if (uploader.HasFile)
			{
				SharedFile sharedFile = new SharedFile();

				string fileName = Path.GetFileName(uploader.FileName);

				sharedFile.ModuleId = ModuleId;
				sharedFile.ModuleGuid = ModuleConfiguration.ModuleGuid;
				sharedFile.OriginalFileName = fileName;
				sharedFile.FriendlyName = fileName;
				sharedFile.SizeInKB = (int)(uploader.FileContent.Length / 1024);
				sharedFile.FolderId = CurrentFolderId;

				if (CurrentFolderId > -1)
				{
					SharedFileFolder folder = new SharedFileFolder(ModuleId, CurrentFolderId);
					sharedFile.FolderGuid = folder.FolderGuid;
				}

				sharedFile.UploadUserId = siteUser.UserId;
				sharedFile.UserGuid = siteUser.UserGuid;

				sharedFile.ContentChanged += new ContentChangedEventHandler(SharedFile_ContentChanged);

				if (sharedFile.Save())
				{
					string destPath = VirtualPathUtility.Combine(fileVirtualBasePath, sharedFile.ServerFileName);

					using (Stream s = uploader.FileContent)
					{
						fileSystem.SaveFile(destPath, s, IOHelper.GetMimeType(Path.GetExtension(sharedFile.FriendlyName).ToLower()), true);
					}
				}
			}

			WebUtils.SetupRedirect(this, Request.RawUrl);
		}


		void SharedFile_ContentChanged(object sender, ContentChangedEventArgs e)
		{
			IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["SharedFilesIndexBuilderProvider"];
			if (indexBuilder != null)
			{
				indexBuilder.ContentChangedHandler(sender, e);
			}
		}


		private bool IconExists(String iconFileName)
		{
			bool result = false;
			if (this.iconList != null)
			{
				foreach (FileInfo f in this.iconList)
				{
					if (f.Name == iconFileName)
					{
						result = true;
					}
				}
			}

			return result;
		}


		private void PopulateLabels()
		{
			UIHelper.AddConfirmationDialog(btnDelete, SharedFileResources.FileManagerDeleteConfirm);

			dgFile.Columns[1].HeaderText = SharedFileResources.FileManagerFileNameLabel;
			dgFile.Columns[2].HeaderText = SharedFileResources.FileDescription;
			dgFile.Columns[3].HeaderText = SharedFileResources.FileManagerSizeLabel;
			dgFile.Columns[4].HeaderText = SharedFileResources.DownloadCountLabel;
			dgFile.Columns[5].HeaderText = SharedFileResources.FileManagerModifiedLabel;
			dgFile.Columns[6].HeaderText = SharedFileResources.SharedFilesUploadedByLabel;

			dgFile.Columns[0].Visible = !displaySettings.HideFirstColumnIfNotEditable || IsEditable;
			dgFile.Columns[2].Visible = config.ShowDescription && !displaySettings.HideDescription;
			dgFile.Columns[3].Visible = config.ShowSize && !displaySettings.HideSize;
			dgFile.Columns[4].Visible = (IsEditable || config.ShowDownloadCountToAllUsers) && !displaySettings.HideDownloadCount;
			dgFile.Columns[5].Visible = config.ShowModified && !displaySettings.HideModified;
			dgFile.Columns[6].Visible = config.ShowUploadedBy && !displaySettings.HideUploadedBy;
			dgFile.Columns[7].Visible = IsEditable;

			fgpNewFolder.Visible = IsEditable;
			imgroot = ImageSiteRoot + "/Data/SiteImages/";
			btnDelete.ImageUrl = imgroot + "delete.png";
			btnGoUp.ImageUrl = imgroot + "folder-up-icon.png";

			btnUpload2.Text = SharedFileResources.FileManagerUploadButton;
			btnGoUp.ToolTip = SharedFileResources.FileManagerGoUp;
			btnGoUp.AlternateText = SharedFileResources.FileManagerGoUp;
			btnNewFolder.Text = SharedFileResources.FileManagerNewFolderButton;
			btnDelete.ToolTip = SharedFileResources.FileManagerDelete;
			btnDelete.AlternateText = SharedFileResources.FileManagerDelete;
			btnDelete.Visible = IsEditable;

			// this button is clicked by javascript callback from the jquery file uploader
			btnRefresh.ImageUrl = "~/Data/SiteImages/1x1.gif";
			btnRefresh.AlternateText = SharedFileResources.RefreshButtonText; //we really don't want any text here but without it accessibility checks fail... smh...

			RootLabel = SharedFileResources.Root;

			uploader.AddFilesText = SharedFileResources.SelectFilesButton;
			uploader.AddFileText = SharedFileResources.SelectFileButton;
			uploader.DropFilesText = SharedFileResources.DropFiles;
			uploader.DropFileText = SharedFileResources.DropFile;
			uploader.UploadButtonText = SharedFileResources.FileManagerUploadButton;
			uploader.UploadCompleteText = SharedFileResources.UploadComplete;
			uploader.UploadingText = SharedFileResources.Uploading;


			if (ModuleConfiguration != null)
			{
				Title = this.ModuleConfiguration.ModuleTitle;
				Description = this.ModuleConfiguration.FeatureName;
			}
		}


		private void LoadSettings()
		{
			config = new SharedFilesConfiguration(Settings);
			EditContentImage = WebConfigSettings.EditContentImage;
			lblError.Text = String.Empty;

			FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];

			if (p == null) return;

			fileSystem = p.GetFileSystem();

			if (fileSystem == null) return;

			siteUser = SiteUtils.GetCurrentSiteUser();

			newWindowMarkup = displaySettings.NewWindowLinkMarkup;

			if (BrowserHelper.IsIE())
			{
				//this is a needed hack because IE 8 doesn't work correctly with window.open
				// a "security feature" of IE 8
				// unfortunately this is not valid xhtml to use target but it works in IE
				newWindowMarkup = displaySettings.IeNewWindowLinkMarkup;
			}

			if (!SharedFilesConfiguration.DownloadLinksOpenNewWindow)
			{
				newWindowMarkup = string.Empty;
			}

			TimeOffset = SiteUtils.GetUserTimeOffset();
			timeZone = SiteUtils.GetUserTimeZone();
			fileVirtualBasePath = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/SharedFiles/";

			try
			{
				// this keeps the action from changing during ajax postback in folder based sites
				SiteUtils.SetFormAction(Page, Request.RawUrl);
			}
			catch (MissingMethodException)
			{
				//this method was introduced in .NET 3.5 SP1
			}

			btnUpload2.Visible = IsEditable;
			uploader.Visible = IsEditable;
			uploader.MaxFilesAllowed = SharedFilesConfiguration.MaxFilesToUploadAtOnce;
			uploader.ServiceUrl = $"{SiteRoot}/SharedFiles/upload.ashx?pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}";
			uploader.FormFieldClientId = hdnCurrentFolderId.ClientID;
			uploader.UploadButtonClientId = btnUpload2.ClientID;

			if (IsEditable)
			{
				string refreshFunction = $"function refresh{ModuleId.ToInvariantString()}() {{ $('#{btnRefresh.ClientID}').click(); }};";

				uploader.UploadCompleteCallback = $"refresh{ModuleId.ToInvariantString()}";

				ScriptManager.RegisterClientScriptBlock(
					this,
					GetType(),
					$"refresh{ModuleId.ToInvariantString()}",
					refreshFunction,
					true
				);
			}


			if (dgFile.TableCssClass.Contains("jqtable") && !WebConfigSettings.DisablejQuery)
			{

				string script = $@"
function setupJTable{ModuleId.ToInvariantString()}() {{
	$('#{dgFile.ClientID} th').each(function() {{
		$(this).addClass('ui-state-default');
	}});

	$('table.jqtable td').each(function() {{
		$(this).addClass('ui-widget-content');
	}});

	$('table.jqtable tr').hover(
		function() {{
			$(this).children('td').addClass('ui-state-hover');
		}},
		function() {{
			$(this).children('td').removeClass('ui-state-hover');
		}}
	);

	$('table.jqtable tr').on('click', function() {{
		$(this).children('td').toggleClass('ui-state-highlight');
	}});
}};

Sys.WebForms.PageRequestManager.getInstance().add_endRequest(setupJTable{ModuleId.ToInvariantString()});";

				ScriptManager.RegisterStartupScript(
					this,
					GetType(),
					$"jTable{ModuleId.ToInvariantString()}",
					script,
					true
				);
			}


			trObjectCount.Visible = config.ShowObjectCount;

			if (config.InstanceCssClass.Length > 0) { pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass); }

			if (WebConfigSettings.ForceLegacyFileUpload)
			{
				ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnUpload2);
			}
		}


		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			if (Page is mojoBasePage)
			{
				mojoBasePage basePage = Page as mojoBasePage;
				basePage.ScriptConfig.IncludeJQTable = true;
			}
		}
	}
}