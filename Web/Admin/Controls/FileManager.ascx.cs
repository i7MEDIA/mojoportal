// Author:					
// Created:				    2004-12-07
// Last Modified:			2013-04-08
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Brettle.Web.NeatUpload;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class FileManagerControl : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FileManagerControl));
        private SiteSettings siteSettings = null;
        private string imgroot;
        private List<string> iconList;
        private IFileSystem fileSystem = null;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Visible) { return; }

            //if (!WebUser.IsAdminOrContentAdmin)
            //{
            //    SiteUtils.RedirectToAccessDeniedPage(this);
            //    return;
            //}

            LoadSettings();
            PopulateLabels();
            //will be null if user has no permissions
            if (fileSystem == null)
            {
                pnlFile.Visible = false;
                lblDisabledMessage.Text = Resource.FileManagerDisabledMessage;
                return;
            }

            if (WebConfigSettings.DisableFileManager)
            {
                pnlFile.Visible = false;
                lblDisabledMessage.Text = Resource.FileManagerDisabledMessage;
            }
            else
            {
                if (
                    (siteSettings.IsServerAdminSite)
                    || (WebConfigSettings.AllowFileManagerInChildSites)
                    )
                {
                    ShowFileManager();
                }
                else
                {
                    pnlFile.Visible = false;
                    lblDisabledMessage.Text = Resource.FileManagerDisabledMessage;
                }
            }

        }

        private void ShowFileManager()
        {
            lblError.Text = string.Empty;
            iconList = SiteUtils.GetFileIconNames();

            //this is a workaround for a mono bug where progress bars get null reference 2009-12-30
#if MONO
           
            progressBar.AddTrigger(this.btnUpload);
#endif

            if (!Page.IsPostBack)
            {
                UIHelper.AddConfirmationDialog(btnDelete, Resource.FileManagerDeleteConfirm);
                BindData();
            }
        }

        
       
        private String GetCurrentDirectory()
        {
            string currentDirectory = fileSystem.VirtualRoot;

            if (hdnCurDir.Value.Length > 0)
            {
                currentDirectory = hdnCurDir.Value;
            }
          
            lblCurrentDirectory.Text = currentDirectory.Replace("|", "/");

            if (currentDirectory == fileSystem.VirtualRoot)
            {
                btnGoUp.Visible = false;
            }
            else
            {
                btnGoUp.Visible = true;
            }
           
            return currentDirectory = VirtualPathUtility.AppendTrailingSlash(currentDirectory);

        }

        private string GetParentVirtualPath(string virtualPath)
        {
            if (virtualPath == fileSystem.VirtualRoot) { return virtualPath; } // don't allow going up further
            string result = virtualPath;
            if (result.EndsWith("/")) { result = result.Substring(0, result.Length - 1); }
            int i = result.LastIndexOf("/");
            if (i == -1) { return fileSystem.VirtualRoot; }
            result = result.Substring(0, i);


            return result;
        }

        private void SetCurrentDirectory(string dir)
        {
            hdnCurDir.Value = dir;
        }


        private void BindData()
        {
            dgFile.DataSource = GetFiles();
            dgFile.DataBind();
            lblCounter.Text = dgFile.Rows.Count.ToString() + " "
                + Resource.FileManagerObjectsLabel;
        }

        protected DataTable GetFiles()
        {

            IEnumerable<WebFolder> folderList;
            IEnumerable<WebFile> fileList;

            string currentDirectory = GetCurrentDirectory();
            folderList = fileSystem.GetFolderList(currentDirectory);
            fileList = fileSystem.GetFileList(currentDirectory);
            
            DataTable dt = CreateDataSource();
            DataRow dr;
            foreach (WebFolder dir in folderList)
            {
                dr = dt.NewRow();
                dr["filename"] = dir.Name;
                dr["path"] = dir.VirtualPath;
                dr["size"] = "0";
                dr["type"] = "0";
                dr["modified"] = dir.Modified;
                dt.Rows.Add(dr);
            }

            foreach (WebFileInfo file in fileList)
            {
                dr = dt.NewRow();
                dr["filename"] = file.Name;
                dr["path"] = file.VirtualPath;
                dr["size"] = (long)(file.Size / 1024); //(int)file.Length / 1024
                dr["type"] = "1";
                dr["modified"] = file.Modified;
               
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            return dt;
        }

        protected DataTable CreateDataSource()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("filename", typeof(string));
            dt.Columns.Add("path", typeof(string));
            dt.Columns.Add("size", typeof(long));
            dt.Columns.Add("type", typeof(int));
            dt.Columns.Add("modified", typeof(string));
            return dt;
        }


        #region Grid Events


        void dgFile_RowDataBound(object sender, GridViewRowEventArgs e)
        {
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
                        imgType.ImageUrl = imgroot + "folder.png";
                        e.Row.Cells[2].Text = "";
                        e.Row.Cells[3].Text = "";
                    }
                    else
                    {
                        string name = DataBinder.Eval(e.Row.DataItem, "filename", "{0}").Trim().ToLower();
                        string imgFile = System.IO.Path.GetExtension(name).Replace(".", "") + ".png";
                        if (IconExists(imgFile))
                        {

                            imgType.ImageUrl = imgroot + "Icons/" + imgFile;
                        }
                        else
                        {
                            imgType.ImageUrl = imgroot + "Icons/unknown.png";
                        }

                    }

                }
            }
        }


        void dgFile_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ItemClicked")
            {
                string keys = e.CommandArgument.ToString();
                char[] separator = { '#' };
                string[] args = keys.Split(separator);
              
                string name = args[0];
                int type = int.Parse(args[1]);

                dgFile.EditIndex = -1;
                if (type == 0)  
                {
                    //folder 
                    SetCurrentDirectory(VirtualPathUtility.Combine(fileSystem.VirtualRoot, name));
                }
                else
                { 
                    //file
                    string dir = GetCurrentDirectory();
                    string virtualPath = VirtualPathUtility.Combine(dir, name);
                    WebFile file = fileSystem.RetrieveFile(virtualPath);
                    if (file != null)
                    {
                        if (WebConfigSettings.DownloadScriptTimeout > -1)
                        {
                            Page.Server.ScriptTimeout = WebConfigSettings.DownloadScriptTimeout;
                        }

                        Page.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + file.Name + "\"");
                        //Page.Response.WriteFile(file.Path);
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

                if (type == 0)
                {
                    BindData();
                }
            }
        }


        void dgFile_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridView grid = (GridView)sender;
                TextBox txtEditName = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtEditName");

                if (txtEditName.Text.Trim().Length < 1) { return; }

                string dir = GetCurrentDirectory();
                //string path = dir + txtEditName.Text.ToCleanFolderName(WebConfigSettings.ForceLowerCaseForFolderCreation);
                string previousPath = dir + ViewState["lastSelection"].ToString();
               
                int type = int.Parse(grid.DataKeys[e.RowIndex].Value.ToString());
                if (type == 0)
                {
                    // folder
                    string path = dir + txtEditName.Text.ToCleanFolderName(WebConfigSettings.ForceLowerCaseForFolderCreation);
                    fileSystem.MoveFolder(previousPath, path);
                }
                else
                {
                    // file
                    string path = dir + txtEditName.Text.ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
                    fileSystem.MoveFile(previousPath, path, false);
                }
                grid.EditIndex = -1;
                BindData();
            }
            catch (UnauthorizedAccessException ex)
            {
                lblError.Text = ex.Message;
            }
            catch (ArgumentException ex)
            {
                lblError.Text = ex.Message;
            }
        }


        void dgFile_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;
            Button lnkName = (Button)grid.Rows[grid.EditIndex].Cells[1].FindControl("lnkName");
            ViewState["lastSelection"] = lnkName.Text;
            BindData();
        }


        void dgFile_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dgFile.EditIndex = -1;
            BindData();
        }


        void dgFile_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["SortBy"] == null) { ViewState["SortBy"] = "ASC"; }
            else if (ViewState["SortBy"].ToString().Equals("ASC")) { ViewState["SortBy"] = "DESC"; }
            else { ViewState["SortBy"] = "ASC"; }

            dgFile.Columns[1].HeaderText = Resource.FileManagerFileNameLabel;
            dgFile.Columns[2].HeaderText = Resource.FileManagerSizeLabel;
            dgFile.Columns[3].HeaderText = Resource.FileManagerModifiedLabel;
            for (int i = 0; i < dgFile.Columns.Count; i++)
            {
                if (dgFile.Columns[i].SortExpression.Trim().Equals(e.SortExpression.Trim()))
                {
                    if (ViewState["SortBy"].ToString().Trim().Equals("ASC"))
                    {
                        dgFile.Columns[i].HeaderText = dgFile.Columns[i].HeaderText + "<span style='font-family:webdings;'>5</span>";
                    }
                    else
                    {
                        dgFile.Columns[i].HeaderText = dgFile.Columns[i].HeaderText + "<span style='font-family:webdings;'>6</span>";
                    }
                }
            }

            DataView dv = new DataView(GetFiles());
            dv.Sort = e.SortExpression + " " + ViewState["SortBy"];
            dgFile.DataSource = dv;
            dgFile.DataBind();
        }

        #endregion

        private void btnGoUp_Click(object sender, ImageClickEventArgs e)
        {
            MoveUp();

        }
        private void MoveUp()
        {
            if (!CanMoveUp())
            {
                lblError.Text = Resource.FileManagerRootDirectoryReached;
                BindData();
                return;
            }
            try
            {
                string dir = GetParentVirtualPath(GetCurrentDirectory());
                
                SetCurrentDirectory(dir);
                BindData();
            }
            catch (ArgumentException)
            {
                lblError.Text = Resource.FileManagerRootDirectoryReached;
                SetCurrentDirectory(string.Empty);
                BindData();
            }
        }
        protected bool CanMoveUp()
        {
            string dir = GetCurrentDirectory();
            if (dir.Length == 0) { return false; }
            if (dir == fileSystem.VirtualRoot) { return false; }
            return true;
        }
        private void DeleteItem(GridViewRow e)
        {
            Button lnkName = (Button)e.Cells[1].FindControl("lnkName");
            int type = int.Parse(dgFile.DataKeys[e.RowIndex].Value.ToString());

            string dir = GetCurrentDirectory();
            string path;
            if (dir.Length > 0)
            {
                path = dir + lnkName.Text;
            }
            else
            {
                path = lnkName.Text;
            }

            if (type == 0)
            {
                fileSystem.DeleteFolder(path);
            }
            else
            {
                fileSystem.DeleteFile(path);
            }
        }

        private void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            //try
            //{
            bool yes = false;
            foreach (GridViewRow dgi in dgFile.Rows)
            {
                CheckBox chkChecked = (CheckBox)dgi.Cells[0].FindControl("chkChecked");
                if (chkChecked.Checked)
                {
                    yes = true;
                    DeleteItem(dgi);
                }
            }
            if (yes)
            {
                dgFile.PageIndex = 0;
                dgFile.EditIndex = -1;

                //Page.Response.Redirect(Page.Request.Url.ToString(), false);
                BindData();
                return;
            }

            Page.Response.Redirect(Page.Request.Url.ToString(), false);
            //}
            //catch (Exception ex)
            //{
            //    lblError.Text = ex.Message;
            //}
        }


        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string dir = GetCurrentDirectory();
                string path = dir + txtNewDirectory.Text.ToCleanFolderName(WebConfigSettings.ForceLowerCaseForFolderCreation);
               
                //TODO: if not success show message
                OpResult result = fileSystem.CreateFolder(path);

                txtNewDirectory.Text = string.Empty;
                BindData();
               
                //Page.Response.Redirect(Page.Request.Url.ToString(), false);
            }
            catch (UnauthorizedAccessException ex)
            {
                lblError.Text = ex.Message;
            }
            catch (ArgumentException ex)
            {
                lblError.Text = ex.Message;
            }
        }


        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (uploader.HasFile)
                {
                    string dir = GetCurrentDirectory();
                    string path = dir + "/" + uploader.FileName;
                    string ext = System.IO.Path.GetExtension(uploader.FileName);
                    string mimeType = IOHelper.GetMimeType(ext).ToLower();
                    if (fileSystem.Permission.IsExtAllowed(ext))
                    {
                        using (System.IO.Stream s = uploader.FileContent)
                        {
                            fileSystem.SaveFile(path, s, mimeType, true);
                        }
                    }
                      
                }

                BindData();
            }
            catch (UnauthorizedAccessException ex)
            {
                lblError.Text = ex.Message;
            }
            catch (ArgumentException ex)
            {
                lblError.Text = ex.Message;
            }
        }

        void btnRefresh_Click(object sender, ImageClickEventArgs e)
        {
            BindData();
            return;

        }


        //previous implementation with NeatUpload
        //protected void btnUpload_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (multiFile.Files.Length > 0)
        //        {
        //            string dir = GetCurrentDirectory();

        //            foreach (UploadedFile file in multiFile.Files)
        //            {
        //                if (file != null && file.FileName != null && file.FileName.Trim().Length > 0)
        //                {
        //                    string path = dir + "/" + file.FileName;

        //                    using (file)
        //                    {
        //                        using (System.IO.Stream s = file.OpenRead())
        //                        {
        //                            fileSystem.SaveFile(path, s, file.ContentType, true);
        //                        }
        //                    }
                            
        //                }
        //            }
        //        }

        //        BindData();
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        lblError.Text = ex.Message;
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        lblError.Text = ex.Message;
        //    }
        //}


        private void PopulateLabels()
        {
          
            tblUpload.Visible = true;
            btnDelete.Visible = true;

            imgroot = WebUtils.GetSiteRoot() + "/Data/SiteImages/";
            btnDelete.ImageUrl = imgroot + "delete.png";
            btnGoUp.ImageUrl = imgroot + "folder-up-icon.png";

            //btnAddFile.Text = Resource.AddFileButton;

            this.btnUpload.Text = Resource.FileManagerUploadButton;
            btnGoUp.ToolTip = Resource.FileManagerGoUp;
            btnGoUp.AlternateText = Resource.FileManagerGoUp;
            btnNewFolder.Text = Resource.FileManagerNewFolderButton;
            btnDelete.ToolTip = Resource.FileManagerDelete;
            btnDelete.AlternateText = Resource.FileManagerDelete;

            dgFile.Columns[1].HeaderText = Resource.FileManagerFileNameLabel;
            dgFile.Columns[2].HeaderText = Resource.FileManagerSizeLabel;
            dgFile.Columns[3].HeaderText = Resource.FileManagerModifiedLabel;


            UIHelper.AddConfirmationDialog(btnDelete, Resource.FileManagerDeleteConfirm);

            uploader.AddFileText = Resource.SelectFileButton;
            uploader.DropFileText = Resource.DropFile;
            uploader.DropFilesText = Resource.DropFiles;
            uploader.UploadButtonText = Resource.UploadButton;
            uploader.UploadCompleteText = Resource.UploadComplete;
            uploader.UploadingText = Resource.Uploading;

            // this button is clicked by javascript callback from the jquery file uploader
            btnRefresh.ImageUrl = "~/Data/SiteImages/1x1.gif";
            btnRefresh.AlternateText = " ";

        }

        private bool IconExists(String iconFileName)
        {
            bool result = false;
            if (this.iconList != null)
            {
                return iconList.Contains(iconFileName);
                
            }

            return result;

        }


        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();

            FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
            if (p == null)
            {
                log.Error("Could not load file system provider " + WebConfigSettings.FileSystemProvider);
                return;
            }

            fileSystem = p.GetFileSystem();
            if (fileSystem == null)
            {
                log.Error("Could not load file system from provider " + WebConfigSettings.FileSystemProvider);
                return;

            }

            if (!IsPostBack)
            {
                hdnCurDir.Value = fileSystem.VirtualRoot;
            }

            uploader.MaxFilesAllowed = WebConfigSettings.MaxFileManagerFilesToUploadAtOnce;
            // need to get from file system permission based on user
            if ((WebUser.IsAdminOrContentAdmin) || SiteUtils.UserIsSiteEditor())
            {
                uploader.AcceptFileTypes = SecurityHelper.GetRegexValidationForAllowedExtensionsJqueryFileUploader(
                    WebConfigSettings.AllowedUploadFileExtensions);
            }
            else if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                uploader.AcceptFileTypes = SecurityHelper.GetRegexValidationForAllowedExtensionsJqueryFileUploader(
                    WebConfigSettings.AllowedUploadFileExtensions);

            }
            else if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                uploader.AcceptFileTypes = SecurityHelper.GetRegexValidationForAllowedExtensionsJqueryFileUploader(
                    WebConfigSettings.AllowedLessPriveledgedUserUploadFileExtensions);

            }
            
            uploader.UploadButtonClientId = btnUpload.ClientID;
            string fileSystemToken = Global.FileSystemToken.ToString();
            uploader.ServiceUrl = SiteUtils.GetNavigationSiteRoot()
                + "/Services/FileService.ashx?cmd=uploadfm&t=" + fileSystemToken;
            uploader.FormFieldClientId = hdnCurDir.ClientID; // 

            //string refreshFunction = "function refresh () { window.location.reload(true); } ";

            //uploader.UploadCompleteCallback = "refresh";

            string refreshFunction = "function refresh"
                    + " () { $('#" + btnRefresh.ClientID + "').click(); } ";

            uploader.UploadCompleteCallback = "refresh";

            ScriptManager.RegisterClientScriptBlock(
                this,
                this.GetType(), "refresh",
                refreshFunction,
                true);
        }




        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);

            this.btnUpload.Click += new EventHandler(btnUpload_Click);
            this.btnGoUp.Click += new ImageClickEventHandler(this.btnGoUp_Click);
            this.btnDelete.Click += new ImageClickEventHandler(this.btnDelete_Click);

            dgFile.RowCommand += new GridViewCommandEventHandler(dgFile_RowCommand);
            dgFile.RowCancelingEdit += new GridViewCancelEditEventHandler(dgFile_RowCancelingEdit);
            dgFile.RowEditing += new GridViewEditEventHandler(dgFile_RowEditing);
            dgFile.RowUpdating += new GridViewUpdateEventHandler(dgFile_RowUpdating);
            dgFile.RowDataBound += new GridViewRowEventHandler(dgFile_RowDataBound);
            dgFile.Sorting += new GridViewSortEventHandler(dgFile_Sorting);

            this.btnNewFolder.Click += new EventHandler(this.btnNewFolder_Click);
            btnRefresh.Click += btnRefresh_Click;

        }

        


    }
}