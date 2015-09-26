/// Author:					    Joe Audette
/// Created:				    2005-01-09
/// Last Modified:			    2013-10-10
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
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
        //private String cacheDependencyKey;
        private Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        //private string newWindowMarkup = "onclick=\"window.open(this.href,'_blank');return false;\"";
        private SharedFilesConfiguration config = new SharedFilesConfiguration();

        protected Double TimeOffset
        {
            get{return timeOffset;}  
        }
		

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += new EventHandler(this.Page_Load);
            this.btnUpload.Click += new EventHandler(btnUpload_Click);
            this.btnUpdateFile.Click += new EventHandler(btnUpdateFile_Click);
            this.btnDeleteFile.Click += new EventHandler(btnDeleteFile_Click);
            this.btnUpdateFolder.Click += new EventHandler(btnUpdateFolder_Click);
            this.btnDeleteFolder.Click += new EventHandler(btnDeleteFolder_Click);
            
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

			if(!IsPostBack)
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
			if((this.moduleId > 0)&&(this.itemId > 0)&&((this.type == "folder")||(this.type == "file")))
			{
				if(this.type == "folder")
				{
					PopulateFolderControls();
				}

				if(this.type == "file")
				{
					PopulateFileControls();
				}
			}
			else
			{
				this.pnlNotFound.Visible = true;
				this.pnlFolder.Visible = false;
				this.pnlFile.Visible = false;

			}
		}

		private void PopulateFolderControls()
		{
            Title = SiteUtils.FormatPageTitle(siteSettings, SharedFileResources.SharedFilesEditFolderLabel);
            heading.Text = SharedFileResources.SharedFilesEditFolderLabel;
			SharedFileFolder folder = new SharedFileFolder(this.moduleId, this.itemId);
			if((folder.FolderId > 0)&&(folder.ModuleId == this.moduleId))
			{
				this.pnlNotFound.Visible = false;
				this.pnlFile.Visible = false;
				this.pnlFolder.Visible = true;

				this.txtFolderName.Text = folder.FolderName;

                List<SharedFileFolder> allFolders = SharedFileFolder.GetSharedModuleFolderList(folder.ModuleId);
                
                this.ddFolderList.DataSource = allFolders;
                this.ddFolderList.DataBind();

				this.ddFolderList.Items.Insert(0,new ListItem("Root","-1"));
				this.ddFolderList.SelectedValue = folder.ParentId.ToString();

                // prevent a folder from being its own parent
                ListItem item = this.ddFolderList.Items.FindByText(folder.FolderName);
                if(item != null)
                this.ddFolderList.Items.Remove(item);


                //// prevent a children folder from being parent
                //// build list
                List<int> toRemove = new List<int>();
                foreach (ListItem fldItem in this.ddFolderList.Items)
                {
                    //SharedFileFolder currentFolder = new SharedFileFolder(this.moduleId, Convert.ToInt32(fldItem.Value));
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
                    this.ddFolderList.Items.Remove(this.ddFolderList.Items.FindByValue(itemToRemove.ToInvariantString()));
                }
			}
			else
			{
				this.pnlNotFound.Visible = true;
				this.pnlFolder.Visible = false;
				this.pnlFile.Visible = false;
			}
		}

		private void PopulateFileControls()
		{
            Title = SiteUtils.FormatPageTitle(siteSettings, SharedFileResources.SharedFilesEditLabel);
            heading.Text = SharedFileResources.SharedFilesEditLabel;

			SharedFile file = new SharedFile(this.moduleId, this.itemId);
			if((file.ItemId > 0)&&(file.ModuleId == this.moduleId))
			{
				this.pnlNotFound.Visible = false;
				this.pnlFolder.Visible = false;
				this.pnlFile.Visible = true;

                using (IDataReader reader = SharedFileFolder.GetSharedModuleFolders(file.ModuleId))
                {
                    this.ddFolders.DataSource = reader;
                    this.ddFolders.DataBind();
                }
				this.ddFolders.Items.Insert(0,new ListItem("Root","-1"));
				this.ddFolders.SelectedValue = file.FolderId.ToInvariantString();

                hdnCurrentFolderId.Value = file.FolderId.ToInvariantString();

                if (timeZone != null)
                {
                    this.lblUploadDate.Text = file.UploadDate.ToLocalTime(timeZone).ToString();
                }
                else
                {
                    this.lblUploadDate.Text = file.UploadDate.AddHours(timeOffset).ToString();
                }

				SiteUser uploadUser = new SiteUser(siteSettings, file.UploadUserId);
				this.lblUploadBy.Text = uploadUser.Name;
				this.lblFileSize.Text = file.SizeInKB.ToString();
				this.txtFriendlyName.Text = file.FriendlyName;
                edDescription.Text = file.Description;

                if (config.EnableVersioning)
                {
                    using (IDataReader reader = file.GetHistory())
                    {
                        grdHistory.DataSource = reader;
                        grdHistory.DataBind();
                    }
                }

			}
			else
			{
				pnlNotFound.Visible = true;
				pnlFolder.Visible = false;
				pnlFile.Visible = false;
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

            //progressBar.AddTrigger(this.btnUpload);

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

                SharedFile sharedFile = new SharedFile(this.moduleId, this.itemId);
                sharedFile.ContentChanged += new ContentChangedEventHandler(sharedFile_ContentChanged);

                if (config.EnableVersioning)
                {
                    bool historyCreated = SharedFilesHelper.CreateHistory(sharedFile, fileSystem, virtualSourcePath, virtualHistoryPath);
                    if (historyCreated)
                    {
                        sharedFile.ServerFileName = System.Guid.NewGuid().ToString() + ".config";
                    }

                }


                sharedFile.ModuleId = this.moduleId;
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
                        //fileSystem.SaveFile(destPath, file1.FileContent, file1.ContentType, true);
                        fileSystem.SaveFile(destPath, s, IOHelper.GetMimeType(Path.GetExtension(sharedFile.FriendlyName).ToLower()), true);
                    }
                    
                }



                CurrentPage.UpdateLastModifiedTime();
                CacheHelper.ClearModuleCache(moduleId);
                SiteUtils.QueueIndexing();
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);



            
        }


        //previous implementation with NeatUpload

        //private void btnUpload_Click(object sender, EventArgs e)
        //{
        //    SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
        //    if (siteUser == null) return;

        //    SharedFile sharedFile = new SharedFile(this.moduleId, this.itemId);
        //    sharedFile.ContentChanged += new ContentChangedEventHandler(sharedFile_ContentChanged);
        //    if((sharedFile.ItemId > 0)&&(sharedFile.ModuleId == this.moduleId))
        //    {
                
        //            if (file1.HasFile && file1.FileName != null && file1.FileName.Trim().Length > 0)
        //            {
        //                if (config.EnableVersioning)
        //                {
        //                    bool historyCreated = SharedFilesHelper.CreateHistory(sharedFile, fileSystem, virtualSourcePath, virtualHistoryPath);
        //                    if (historyCreated)
        //                    {
        //                        sharedFile.ServerFileName = System.Guid.NewGuid().ToString() + ".config";
        //                    }

        //                }
						
                        
        //                sharedFile.ModuleId = this.moduleId;
        //                if (sharedFile.ModuleGuid == Guid.Empty)
        //                {
        //                    Module m = GetModule(moduleId, SharedFile.FeatureGuid);
        //                    sharedFile.ModuleGuid = m.ModuleGuid;

        //                }

        //                sharedFile.OriginalFileName = file1.FileName;
        //                sharedFile.FriendlyName = Path.GetFileName(file1.FileName);
        //                sharedFile.SizeInKB = (int)(file1.ContentLength/1024);
        //                sharedFile.UploadUserId = siteUser.UserId;
        //                sharedFile.UploadDate = DateTime.UtcNow;
						
        //                if(sharedFile.Save())
        //                {
        //                    string destPath = virtualSourcePath + sharedFile.ServerFileName;
        //                    using (file1)
        //                    {
        //                        using (file1.FileContent)
        //                        {
        //                            fileSystem.SaveFile(destPath, file1.FileContent, file1.ContentType, true);
        //                        }
        //                    }
        //                }
						
        //            }

        //            CurrentPage.UpdateLastModifiedTime();
        //            CacheHelper.ClearModuleCache(moduleId);
        //            SiteUtils.QueueIndexing();
        //            WebUtils.SetupRedirect(this, Request.RawUrl);
                    
					
                
        //    }
        //}

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

			SharedFile file = new SharedFile(this.moduleId, this.itemId);
            file.ContentChanged += new ContentChangedEventHandler(sharedFile_ContentChanged);

            if((file.ItemId > 0)&&(file.ModuleId == this.moduleId))
			{
				file.FriendlyName = this.txtFriendlyName.Text;
                file.Description = edDescription.Text;
				file.FolderId = int.Parse(this.ddFolders.SelectedValue);
                if (file.FolderId > -1)
                {
                    SharedFileFolder folder = new SharedFileFolder(this.moduleId,file.FolderId);
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
			SharedFileFolder folder = new SharedFileFolder(this.moduleId, this.itemId);
			if((folder.FolderId > 0)&&(folder.ModuleId == this.moduleId))
			{
				folder.FolderName = this.txtFolderName.Text;
				folder.ParentId = int.Parse(this.ddFolderList.SelectedValue);
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
                    //SharedFile.RestoreHistoryFile(archiveID, this.upLoadPath, this.historyPath);
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
                            Page.Response.AddHeader("Content-Disposition", "filename=" + historyFile.FriendlyName);
                        }
                        else
                        {
                            // other files just use file save dialog
                            Page.Response.AddHeader("Content-Disposition", "attachment; filename=" + historyFile.FriendlyName);
                        }

                        //Page.Response.AddHeader("Content-Length", documentFile.DocumentImage.LongLength.ToString());

                        Page.Response.ContentType = "application/" + fileType;
                        Page.Response.Buffer = false;
                        Page.Response.BufferOutput = false;
                        //Response.TransmitFile(historyPath + historyFile.ServerFileName);
                        //Response.End();

                        using (System.IO.Stream stream = fileSystem.GetAsStream(virtualHistoryPath + historyFile.ServerFileName))
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

            timeOffset = SiteUtils.GetUserTimeOffset();
        }

        private void LoadSettings()
        {
            timeZone = SiteUtils.GetUserTimeZone();
            virtualSourcePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/SharedFiles/";
            virtualHistoryPath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/SharedFiles/History/";

            lnkCancelFile.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            lnkCancelFolder.NavigateUrl = lnkCancelFile.NavigateUrl;

            //if (BrowserHelper.IsIE())
            //{
            //    //this is a needed hack because IE 8 doesn't work correctly with window.open
            //    // a "security feature" of IE 8
            //    // unfortunately this is not valid xhtml to use target but it works in IE
            //    newWindowMarkup = " target='_blank' ";
            //}

            //this page handles both folders and files
            //expected strItem examples are 23~folder and 13~file
            //the number portion is the ItemID of the folder or file
            if (strItem.IndexOf("~") > -1)
            {
                try
                {
                    char[] separator = { '~' };
                    string[] args = strItem.Split(separator);
                    this.itemId = int.Parse(args[0]);
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

            uploader.UploadCompleteCallback = "refresh" + moduleId.ToInvariantString();

            ScriptManager.RegisterClientScriptBlock(
                this,
                this.GetType(), "refresh" + moduleId.ToInvariantString(),
                refreshFunction,
                true);

            AddClassToBody("sharedfilesedit");

            FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
            if (p == null) { return; }

            fileSystem = p.GetFileSystem();
            
        }


		
	}
}
