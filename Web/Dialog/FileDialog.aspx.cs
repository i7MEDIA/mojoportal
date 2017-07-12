//  Author:                     
//  Created:                    2009-08-16
//	Last Modified:              2014-01-09
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.IO;
using System.Text;
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


namespace mojoPortal.Web.Dialog
{
    /// <summary>
    /// A dialog page for file browse and upload in WYSIWYG Editors
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Index
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Custom_filebrowser
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Configuration
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Plugins/template
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Configuration/theme_advanced_blockformats 
    /// http://abeautifulsite.net/notebook/58
    /// http://plugins.jquery.com/project/filetree
    /// </summary>
    public partial class FileDialog : Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FileDialog));

        private SiteSettings siteSettings = null;
        private SiteUser currentUser = null;
        private string rootDirectory = "/Data/";
        private string navigationRoot = string.Empty;
        private string browserType = "image";
        private string editorType = string.Empty;
        private string currentDir = string.Empty;
        private bool canEdit = false;
        private bool userCanDeleteFiles = false;
        private string allowedExtensions = string.Empty;
        private int resizeWidth = 550;
        private int resizeHeight = 550;
        private string imageCropperUrl = string.Empty;
        private string CKEditor = string.Empty;
        private string CKEditorFuncNum = string.Empty;
        private string langCode = string.Empty;
        private string clientTextBoxId = string.Empty;
        private IFileSystem fileSystem = null;
        
       
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            if (fileSystem == null) { return; }
            PopulateLabels();
            SetupScripts();

        }


        void btnNewFolder_Click(object sender, EventArgs e)
        {
            if(!canEdit){ return; }

            if ((hdnFolder.Value.Length > 0) && (hdnFolder.Value != rootDirectory))
            {
                currentDir = hdnFolder.Value;
            }

            if (fileSystem.CountFolders() <= fileSystem.Permission.MaxFolders)
            {
                try
                {
                    fileSystem.CreateFolder(VirtualPathUtility.Combine(GetCurrentDirectory(), Path.GetFileName(txtNewDirectory.Text).ToCleanFolderName(WebConfigSettings.ForceLowerCaseForFolderCreation)));

                    txtNewDirectory.Text = "";
                    WebUtils.SetupRedirect(this, GetRedirectUrl());
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
            else
            {
                lblError.Text = Resource.FileSystemFolderLimitReached;
            }
        }

        void btnUpload_Click(object sender, EventArgs e)
        {
            // if javascript is available this method will not be called
            // the file upload will happen by ajax post to /Services/FileService.ashx
            // from jquery file uploaded
            // this is fallback implementation

            if ((hdnFolder.Value.Length > 0) && (hdnFolder.Value != rootDirectory))
            {
                currentDir = hdnFolder.Value;
            }
            if (!canEdit)
            {
                WebUtils.SetupRedirect(this, navigationRoot + "/Dialog/FileDialog.aspx?ed=" + editorType + "&type=" + browserType + "&dir=" + currentDir);
                return;
            }

            if (uploader.HasFile)
            {
                //bool doUpload = true;

                long contentLength = uploader.FileBytes.Length;

                if (contentLength > fileSystem.Permission.MaxSizePerFile)
                {
                    //doUpload = false;
                    lblError.Text = Resource.FileSystemFileTooLargeError;
                    return;
                }

                if (fileSystem.CountAllFiles() >= fileSystem.Permission.MaxFiles)
                {
                    //doUpload = false;
                    lblError.Text = Resource.FileSystemFileCountQuotaReachedError;
                    return;

                }

                if (fileSystem.GetTotalSize() + contentLength >= fileSystem.Permission.Quota)
                {
                    //doUpload = false;
                    lblError.Text = Resource.FileSystemStorageQuotaError;
                    return;
                }


                string currentDirectory = GetCurrentDirectory();
                if (!fileSystem.FolderExists(currentDirectory))
                {
                    fileSystem.CreateFolder(currentDirectory);
                }

                string destPath = VirtualPathUtility.Combine(
                    currentDirectory, 
                    Path.GetFileName(uploader.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles));

                string ext = Path.GetExtension(uploader.FileName);

                
                    if (SiteUtils.IsAllowedUploadBrowseFile(ext, allowedExtensions))
                    {
                        using (Stream s = uploader.FileContent)
                        {
                            fileSystem.SaveFile(destPath, s, IOHelper.GetMimeType(ext), true);
                        }

                        if (SiteUtils.IsImageFileExtension(ext))
                        {
                            if (chkConstrainImageSize.Checked)
                            {
                                mojoPortal.Web.ImageHelper.ResizeImage(
                                    destPath, 
                                    IOHelper.GetMimeType(ext), 
                                    resizeWidth, 
                                    resizeHeight, 
                                    WebConfigSettings.DefaultResizeBackgroundColor);
                            }
                        }
                    }

                
            }

            WebUtils.SetupRedirect(this, GetRedirectUrl());
        }


       

        void btnDelete_Click(object sender, EventArgs e)
        {
            // this is using a LinkButton which I normally never use for accessibility reasons 
            // because linkbuttons don't work if javascript is disabled
            // but in this case this dialog can't work any if javascript is disabled 
            // so I'm using one
            if (userCanDeleteFiles)
            {
                string fileToDelete = string.Empty;
                if (hdnFileUrl.Value.Length > 0) { fileToDelete = hdnFileUrl.Value; }

                bool canDelete = WebFolder.IsDecendentVirtualPath(rootDirectory, fileToDelete);
                if (canDelete)
                {
                    //File.Delete(Server.MapPath(fileToDelete));
                    fileSystem.DeleteFile(fileToDelete);
                } 
            }

            if ((hdnFolder.Value.Length > 0) && (hdnFolder.Value != rootDirectory))
            {
                currentDir = hdnFolder.Value;
            }

            WebUtils.SetupRedirect(this, GetRedirectUrl());

        }

        private string GetRedirectUrl()
        {
            if(editorType == "ck")
            {
                return navigationRoot + "/Dialog/FileDialog.aspx?"
                    + "type=" + browserType 
                    + "&CKEditor=" + CKEditor
                    + "&CKEditorFuncNum=" + CKEditorFuncNum
                    + "&langCode=" + langCode
                    + "&ed=" + editorType
                    + "&dir=" + currentDir;

            }
            return navigationRoot + "/Dialog/FileDialog.aspx?ed=" + editorType 
                + "&type=" + browserType 
                + "&dir=" + currentDir
                + "&tbi=" + clientTextBoxId;

        }

        private string GetCurrentDirectory()
        {
            string hiddenCurrentFolder = hdnFolder.Value;

            if (string.IsNullOrEmpty(hiddenCurrentFolder)) { return rootDirectory; }

            if (hiddenCurrentFolder.StartsWith("/")) { hiddenCurrentFolder = "~" + hiddenCurrentFolder; }
           
            if (!fileSystem.FolderExists(hiddenCurrentFolder)) { return rootDirectory; }


            if (WebFolder.IsDecendentVirtualPath(rootDirectory, hiddenCurrentFolder))
            {
                return hiddenCurrentFolder;
            }

            return rootDirectory;
        }

        

        private void PopulateLabels()
        {
            this.Title = Resource.FileBrowser;
            litHeading.Text = Server.HtmlEncode(Resource.FileBrowseDialogHeading);
            btnSubmit.Text = Resource.SelectButton;
            btnNewFolder.Text = Resource.FileBrowserCreateFolderButton;
            btnUpload.Text = Resource.FileManagerUploadButton;
            regexFile.ErrorMessage = Resource.FileTypeNotAllowed;
            reqFile.ErrorMessage = Resource.NoFileSelectedWarning;
            requireFolder.ErrorMessage = Resource.FolderNameRequired;
            regexFolder.ValidationExpression = SecurityHelper.GetMaxLengthRegexValidationExpression(150);
            regexFolder.ErrorMessage = Resource.FolderName150Limit;
            litCreateFolder.Text = Server.HtmlEncode(Resource.FileBrowserCreateFolderHeading);
            litUpload.Text = Server.HtmlEncode(Resource.FileBrowserUploadHeading);
            litFolderInstructions.Text = Server.HtmlEncode(Resource.FileBrowserCreateFolderInstructions);
            litUploadInstructions.Text = Server.HtmlEncode(Resource.FileBrowserUploadInstructions);
            if(browserType == "folder")
            {
                litFileSelectInstructions.Text = Server.HtmlEncode(Resource.FolderBrowserSelectionInstructions);
                litFileBrowser.Text = string.Format(CultureInfo.InvariantCulture, "<span class='operationheading'>{0}</span>", Resource.FolderBrowser);
               
            }
            else
            {
                litFileSelectInstructions.Text = Server.HtmlEncode(Resource.FileBrowserSelectFileInstructions);
                litFileBrowser.Text = string.Format(CultureInfo.InvariantCulture, "<span class='operationheading'>{0}</span>", Resource.FileBrowser);
                litPreview.Text = string.Format(CultureInfo.InvariantCulture, "<span class='operationheading'>{0}</span>", Resource.Preview);
            }

            
            

            chkConstrainImageSize.Text = Resource.FileBrowserResizeForWeb;

            lnkImageCropper.Text = Resource.CropImageLink;
            btnDelete.Text = "Delete";
            UIHelper.AddConfirmationDialog(btnDelete, "Are you sure you want to delete this file?");
            btnDelete.Visible = userCanDeleteFiles;

            if (Head1.FindControl("treecss") == null)
            {
                Literal cssLink = new Literal();
                cssLink.ID = "forumcss";
                cssLink.Text = "\n<link href='"
                + Page.ResolveUrl("~/ClientScript/jqueryFileTree/jqueryFileTree.css")
                + "' type='text/css' rel='stylesheet' media='screen' />";

                Head1.Controls.Add(cssLink);
            }

            uploader.AddFilesText = Resource.SelectFilesButton;
            uploader.AddFileText = Resource.SelectFileButton;
            ///uploader.DropFilesText = Resource.DropFiles;
            uploader.DropFileText = Resource.DropFile;
            uploader.UploadButtonText = Resource.UploadButton;
            uploader.UploadCompleteText = Resource.UploadComplete;
            uploader.UploadingText = Resource.Uploading;

        }

        private void LoadSettings()
        {
            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            userCanDeleteFiles = WebUser.IsInRoles(siteSettings.RolesThatCanDeleteFilesInEditor);

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

            rootDirectory = fileSystem.VirtualRoot;


            if ((WebUser.IsAdminOrContentAdmin)||(SiteUtils.UserIsSiteEditor()))
            {
                allowedExtensions = WebConfigSettings.AllowedUploadFileExtensions;
                regexFile.ValidationExpression = SecurityHelper.GetRegexValidationForAllowedExtensions(allowedExtensions);
                uploader.AcceptFileTypes = SecurityHelper.GetRegexValidationForAllowedExtensionsJqueryFileUploader(allowedExtensions);
                canEdit = true;
                userCanDeleteFiles = true;
            }
            else if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                allowedExtensions = WebConfigSettings.AllowedUploadFileExtensions;
                regexFile.ValidationExpression = SecurityHelper.GetRegexValidationForAllowedExtensions(allowedExtensions);
                uploader.AcceptFileTypes = SecurityHelper.GetRegexValidationForAllowedExtensionsJqueryFileUploader(allowedExtensions);
                canEdit = true;

            }
            else if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser == null) { return; }

                allowedExtensions = WebConfigSettings.AllowedLessPriveledgedUserUploadFileExtensions;
                regexFile.ValidationExpression = SecurityHelper.GetRegexValidationForAllowedExtensions(allowedExtensions);
                uploader.AcceptFileTypes = SecurityHelper.GetRegexValidationForAllowedExtensionsJqueryFileUploader(allowedExtensions);
                canEdit = true;
                if(!userCanDeleteFiles)
                {
                    // user is not in a role that can delete files but config setting alows delete from user specific folder anyway
                    userCanDeleteFiles = WebConfigSettings.AllowDeletingFilesFromUserFolderWithoutDeleteRole;
                }
            }


            resizeWidth = WebConfigSettings.ResizeImageDefaultMaxWidth;
            resizeHeight = WebConfigSettings.ResizeImageDefaultMaxHeight;
            if (!IsPostBack) { chkConstrainImageSize.Checked = WebConfigSettings.ResizeEditorUploadedImages; }


            pnlUpload.Visible = canEdit;

            if (Request.QueryString["ed"] != null)
            {
                editorType = Request.QueryString["ed"];
            }

            string requestedType = "image";
            if (Request.QueryString["type"] != null)
            {
                requestedType = Request.QueryString["type"];
            }

            if (Request.QueryString["dir"] != null)
            {
                currentDir = Request.QueryString["dir"];
                if (!WebFolder.IsDecendentVirtualPath(rootDirectory, currentDir)) { currentDir = string.Empty; }
            }


            if (Request.QueryString["CKEditor"] != null)
            {
                CKEditor = Request.QueryString["CKEditor"];
            }

            if (Request.QueryString["CKEditorFuncNum"] != null)
            {
                CKEditorFuncNum = Request.QueryString["CKEditorFuncNum"];
            }

            if (Request.QueryString["langCode"] != null)
            {
                langCode = Request.QueryString["langCode"];
            }

            if (Request.QueryString["tbi"] != null)
            {
                clientTextBoxId = Request.QueryString["tbi"];
            }

            
            switch (requestedType)
            {
                case "media":
                    browserType = "media";
                    break;

                case "audio":
                    browserType = "audio";
                    break;

                case "video":
                    browserType = "video";
                    break;

                case "file":
                    browserType = "file";
                    break;

                case "folder":
                    browserType = "folder";
                    divFileUpload.Visible = false;
                    //divFilePreview.Visible = false;
                    break;

                case "image":
                default:
                    browserType = "image";
                    break;

            }

            navigationRoot = SiteUtils.GetNavigationSiteRoot();

            lnkRoot.Text = rootDirectory.Replace("~", string.Empty); 
            lnkRoot.NavigateUrl = navigationRoot + "/Dialog/FileDialog.aspx?type=" + browserType;

            if (!Page.IsPostBack)
            {
                hdnFolder.Value = rootDirectory;
                if (currentDir.Length > 0)
                {
                    hdnFolder.Value = currentDir;
                }

                txtMaxWidth.Text = resizeWidth.ToInvariantString();
                txtMaxHeight.Text = resizeHeight.ToInvariantString();
            }
            else
            {
                int.TryParse(txtMaxWidth.Text, out resizeWidth);
                int.TryParse(txtMaxHeight.Text, out resizeHeight);
            }

            imageCropperUrl = navigationRoot + "/Dialog/ImageCropperDialog.aspx";
            lnkImageCropper.NavigateUrl = imageCropperUrl;


            if ((canEdit)&&(browserType != "folder"))
            {
                string fileSystemToken = Global.FileSystemToken.ToString();

                uploader.UseDropZone = WebConfigSettings.FileDialogEnableDragDrop;

                uploader.UploadButtonClientId = btnUpload.ClientID;
                uploader.ServiceUrl = navigationRoot
                    + "/Services/FileService.ashx?cmd=uploadfromeditor&q="
                    + Server.UrlEncode(hdnFolder.ClientID)
                    + "&t=" + fileSystemToken;


                StringBuilder refreshScript = new StringBuilder();

                refreshScript.Append("function refresh() {");    
                refreshScript.Append("var selDir = document.getElementById('" + hdnFolder.ClientID + "').value; ");
                refreshScript.Append("window.location.href = updateQueryStringParameter(window.location.href,'dir',selDir); ");
                refreshScript.Append("} ");

                //string refreshFunction = "function refresh"
                //        + " () {  window.location.reload(true)'; } ";

                uploader.UploadCompleteCallback = "refresh";

                ScriptManager.RegisterClientScriptBlock(
                    this,
                    this.GetType(), "refresh",
                    refreshScript.ToString(),
                    true);
            }
            
        }

        private void SetupScripts()
        {
            SetupMainScript();
            SetupjQueryFileTreeScript();
            SetupClearFileInputScript();

        }

        private void SetupMainScript()
        {
            switch (editorType)
            {
                case "tmc":
                    SetupTinyMce();
                    break;

                case "ck":
                    SetupCKeditor();
                    break;

                case "fck":
                    SetupFCKeditor();
                    break;

                default:
                    SetupDefaultScript();
                    break;
            }
        }

        private void SetupClearFileInputScript()
        {
            // 2013-04-04 this may not be needed now since upload is done via ajax to serivce url
            // without this if the user selects a file for upload but then decides to create a folder instead
            // the file would be uploaded even though it is not processed or used on the server
            // this script clears the file selection before submitting the form for folder creation

            btnNewFolder.Attributes.Add("onclick", "clearFile(); return true; ");

            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("function clearFile () {");

            script.Append("document.getElementById('" + uploader.ClientID + "').value = ''; ");

            script.Append("}");

            script.Append("\n</script>");

            this.Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "clearfile",
                script.ToString());


        }

        //this is used by /Controls/FileBrowserTextBoxExtender.cs
        private void SetupDefaultScript()
        {
            btnSubmit.Attributes.Add("onclick", "fbSubmit(); return false; ");

            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("function fbSubmit () {");

            if(browserType == "folder")
            {
                script.Append("var URL = document.getElementById('" + hdnFolder.ClientID + "').value; ");
            }
            else
            {
                script.Append("var URL = document.getElementById('" + hdnFileUrl.ClientID + "').value; ");
            }
            
            //script.Append("alert(URL);");

            script.Append("top.window.SetUrl(URL, '" + clientTextBoxId + "');");
            //script.Append("window.close();");
            //script.Append("window.opener.focus();");

            script.Append("}");

            script.Append("\n</script>");

            this.Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "fbsubmit",
                script.ToString());

        }

        private void SetupCKeditor()
        {
         
            btnSubmit.Attributes.Add("onclick", "ckSubmit(); return false; ");

            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("function ckSubmit () {");

            script.Append("var URL = document.getElementById('" + hdnFileUrl.ClientID + "').value; ");
            //script.Append("alert(URL);");
            script.Append("var CKEditorFuncNum = window.location.href.replace(/.*CKEditorFuncNum=(\\d+).*/,\"$1\")||alert('Error: lost CKEditorFuncNum param from url'+window.location.href)||1;");

            //script.Append("alert(CKEditorFuncNum);");
            //script.Append("var CKEditorFuncNum = " + CKEditorFuncNum + ";");
            // not sure why need to call this 2x but otherwise after an upload it fails to preview the selected image
            script.Append("window.opener.CKEDITOR.tools.callFunction(CKEditorFuncNum, URL);");
            script.Append("window.opener.CKEDITOR.tools.callFunction(CKEditorFuncNum, URL);");
            
            script.Append("window.close();");

            script.Append("}");

           
            script.Append("\n</script>");

            this.Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "cksubmit",
                script.ToString());

            //SetupScrollFix();

        }

        private void SetupFCKeditor()
        {
            
            btnSubmit.Attributes.Add("onclick", "fckSubmit(); return false; ");

            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("function fckSubmit () {");

            script.Append("var URL = document.getElementById('" + hdnFileUrl.ClientID + "').value; ");
            //script.Append("alert(URL);");

            script.Append("window.opener.SetUrl(URL);");
            script.Append("window.close();");
            script.Append("window.opener.focus();");

            script.Append("}");

            script.Append("\n</script>");

            this.Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "fcksubmit",
                script.ToString());

            //SetupScrollFix();

        }

        /// <summary>
        /// fixes an issue in CKeditor and FCKeditor where it was not possible to scroll the page and folderlist could be clipped at the bottom
        /// not needed for TinyMCE so added with script instead of style declaration
        /// </summary>
        //private void SetupScrollFix()
        //{
        //    StringBuilder script = new StringBuilder();
        //    script.Append("\n<script type=\"text/javascript\">");

           
        //    script.Append("$(document).ready(function () {");
        //    script.Append("$('#filewrapper').attr({ 'style': 'height:595px; overflow:auto;  padding: 10px;' });");
        //    script.Append(" });");

        //    script.Append("\n</script>");

        //    this.Page.ClientScript.RegisterStartupScript(
        //        typeof(Page),
        //        "scrollfix",
        //        script.ToString());
        //}

        private void SetupTinyMce()
        {
            btnSubmit.Attributes.Add("onclick", "FileBrowserDialogue.mySubmit(); return false; ");

            if (WebConfigSettings.TinyMceUseV4)
            {
                SetupTinyMce4x();

            }
            else
            {
                SetupTinyMce3x();

            }
            

        }

        private void SetupTinyMce4x()
        {
            btnSubmit.Attributes.Add("onclick", "FileBrowserDialogue.mySubmit(); return false; ");

            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("var FileBrowserDialogue = { ");
            script.Append("init : function () {");

          
            script.Append("},");

            script.Append("mySubmit : function () { ");

            // Here goes your code to insert the retrieved URL value into the original dialogue window.
            //script.Append("alert('hey');");
            //script.Append("var url = '" + WebUtils.ResolveServerUrl(WebUtils.GetSiteRoot()) + "' + document.getElementById('" + hdnFileUrl.ClientID + "').value; ");

            script.Append("var url = document.getElementById('" + hdnFileUrl.ClientID + "').value; ");
            script.Append("top.tinymce.activeEditor.windowManager.getParams().oninsert(url);");
            
           
            // are we an image browser
            //script.Append("if (typeof(win.ImageDialog) != \"undefined\") {");
            //// we are, so update image dimensions
            //script.Append("if (win.ImageDialog.getImageData){ ");
            //script.Append("win.ImageDialog.getImageData(); }");

            //// and preview if necessary
            //script.Append("if (win.ImageDialog.showPreviewImage) {");
            //script.Append("win.ImageDialog.showPreviewImage(URL); }");

            //script.Append("}");

            // close popup window
            script.Append("top.tinymce.activeEditor.windowManager.close();");
            

            script.Append("}");


            script.Append("};");

            //script.Append("tinyMCEPopup.onInit.add(FileBrowserDialogue.init, FileBrowserDialogue);");

            script.Append("\n</script>");

            this.Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "tmcsubmit",
                script.ToString());

        }

        private void SetupTinyMce3x()
        {
            btnSubmit.Attributes.Add("onclick", "FileBrowserDialogue.mySubmit(); return false; ");

            if (WebConfigSettings.TinyMceUseV4)
            {
                Page.ClientScript.RegisterClientScriptBlock(
                this.GetType(),
                "tinymcemain",
                "<script type=\"text/javascript\" src=\""
                + ResolveUrl(WebConfigSettings.TinyMceBasePath + "plugins/compat3x/tiny_mce_popup.js") + "\"></script>");

            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(
                this.GetType(),
                "tinymcemain",
                "<script type=\"text/javascript\" src=\""
                + ResolveUrl(WebConfigSettings.TinyMceBasePath + "tiny_mce_popup.js") + "\"></script>");

            }



            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("var FileBrowserDialogue = { ");
            script.Append("init : function () {");

            //Remove TinyMCE's default popup CSS
            script.Append("var allLinks = document.getElementsByTagName('link');");
            script.Append("allLinks[allLinks.length-1].parentNode.removeChild(allLinks[allLinks.length-1]);");

            script.Append("},");

            script.Append("mySubmit : function () { ");

            // Here goes your code to insert the retrieved URL value into the original dialogue window.
            //script.Append("alert('hey');");

            script.Append("var URL = document.getElementById('" + hdnFileUrl.ClientID + "').value; ");
            script.Append("var win = tinyMCEPopup.getWindowArg('window');");

            // insert information now
            script.Append("win.document.getElementById(tinyMCEPopup.getWindowArg('input')).value = URL;");

            // are we an image browser
            script.Append("if (typeof(win.ImageDialog) != \"undefined\") {");
            // we are, so update image dimensions
            script.Append("if (win.ImageDialog.getImageData){ ");
            script.Append("win.ImageDialog.getImageData(); }");

            // and preview if necessary
            script.Append("if (win.ImageDialog.showPreviewImage) {");
            script.Append("win.ImageDialog.showPreviewImage(URL); }");

            script.Append("}");

            // close popup window
            script.Append("tinyMCEPopup.close();");

            script.Append("}");


            script.Append("};");

            script.Append("tinyMCEPopup.onInit.add(FileBrowserDialogue.init, FileBrowserDialogue);");

            script.Append("\n</script>");

            this.Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "tmcsubmit",
                script.ToString());

        }

        private void SetupjQueryFileTreeScript()
        {
            //http://abeautifulsite.net/notebook/58
            //http://plugins.jquery.com/project/filetree

            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("function updateQueryStringParameter(uri, key, value) {");
            script.Append("var re = new RegExp(\"([?|&])\" + key + \"=.*?(&|$)\", \"i\"); ");
            script.Append("separator = uri.indexOf('?') !== -1 ? \"&\" : \"?\"; ");
            script.Append("if (uri.match(re)) {");
            script.Append("return uri.replace(re, '$1' + key + \"=\" + value + '$2'); ");
            script.Append("} else { ");
            script.Append("return uri + separator + key + \"=\" + value; ");
            script.Append("}} ");

            script.Append("$(document).ready(function() {");

            script.Append("$('#" + lnkImageCropper.ClientID + "').hide(); ");

            if (userCanDeleteFiles)
            {
                script.Append("$('#" + btnDelete.ClientID + "').hide(); ");
            }

            script.Append("$('#" + pnlFileTree.ClientID + "').fileTree({");
            script.Append("root: '" + rootDirectory + "'");

            if (currentDir.Length > 0)
            {
                script.Append(",currentDir : '" + currentDir + "'");
            }

            script.Append(",loadMessage:'" + Resource.AjaxLoadingMessage.HtmlEscapeQuotes() + "'");
            script.Append(",multiFolder: false");


            //http://www.mojoportal.com/Forums/Thread.aspx?thread=6809&mid=34&pageid=5&ItemID=2
            //script.Append(", script: '" + navigationRoot + "/Services/jqueryFileTreeMediaBrowser.ashx?type=" + browserType + "&amp;dir=" + currentDir + "'");
            script.Append(", script: '" + navigationRoot + "/Services/jqueryFileTreeMediaBrowser.ashx?type=" + browserType + "'");

            script.Append("}, function(file) {");

            //script.Append("alert(file);");
            script.Append("document.getElementById('" + hdnFileUrl.ClientID + "').value = file; ");
            script.Append("document.getElementById('" + txtSelection.ClientID + "').value = file; ");
            if (userCanDeleteFiles)
            {
                script.Append("$('#" + btnDelete.ClientID + "').show(); ");
            }

            if (browserType == "image")
            {
                script.Append("document.getElementById('" + imgPreview.ClientID + "').src = file; ");
                script.Append("var imageCropperUrl = '" + imageCropperUrl + "'; ");
                script.Append("var selDir = document.getElementById('" + hdnFolder.ClientID + "').value; ");
                script.Append("var returnUrl = encodeURIComponent('" + navigationRoot + "/Dialog/FileDialog.aspx?ed=" + editorType + "&type=" + browserType + "&dir=' + selDir) ; ");
                //script.Append("alert(returnUrl);");
                script.Append("$('#" + lnkImageCropper.ClientID + "').attr('href',imageCropperUrl + '?src=' + file + '&return=' + returnUrl); ");
                script.Append("$('#" + lnkImageCropper.ClientID + "').show(); ");
            }
            else
            {
                script.Append("$('#" + lnkImageCropper.ClientID + "').hide(); ");
            }

            script.Append("}, function(folder) {");
            
            
            
            script.Append("document.getElementById('" + hdnFolder.ClientID + "').value = folder; ");
            script.Append("if(folder == 'root'){");
            script.Append("document.getElementById('" + hdnFolder.ClientID + "').value = '" + rootDirectory + "'; ");
            script.Append("}");

            if (browserType == "folder")
            {
                script.Append("document.getElementById('" + txtSelection.ClientID + "').value = document.getElementById('" + hdnFolder.ClientID + "').value; ");
                //script.Append("alert(folder);");
            }

            //script.Append("document.getElementById('" + imgPreview.ClientID + "').src = file; ");
            if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                script.Append("if(folder == '/Pages/'){");
                script.Append("$('#" + pnlUpload.ClientID + "').hide();");
                script.Append("}else{");
                script.Append("$('#" + pnlUpload.ClientID + "').show();");
                script.Append("}");
            }

            script.Append("}");

            script.Append(");");
            script.Append("});");

           
            script.Append("$('#pnlFiles" + uploader.ClientID + "').bind('fileuploadsubmit', function (e, data) {");
            script.Append("var fld = $('#" + hdnFolder.ClientID + "'); ");
            script.Append("var maxW = $('#" + txtMaxWidth.ClientID + "'); ");
            script.Append("var maxH = $('#" + txtMaxHeight.ClientID + "'); ");
            script.Append("var rz = $('#" + chkConstrainImageSize.ClientID + "'); ");
            //script.Append(" alert(rz.is(':checked')); ");
            script.Append("data.formData = {fld: fld.val(),maxW: maxW.val(),maxH: maxH.val(),rz: rz.is(':checked')}; ");
            script.Append("}); ");

            script.Append("\n</script>");

            this.Page.ClientScript.RegisterStartupScript(
                typeof(Page),
                "jqftinstance",
                script.ToString());

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            btnNewFolder.Click += new EventHandler(btnNewFolder_Click);
            btnUpload.Click += new EventHandler(btnUpload_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
        }

        

        

        


    }
}
