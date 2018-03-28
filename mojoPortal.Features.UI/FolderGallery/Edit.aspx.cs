/// Author:					
/// Created:				2008-02-08
/// Last Modified:			2018-03-28
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using mojoPortal.Web.UI;
using mojoPortal.Web.Framework;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business;
//using Brettle.Web.NeatUpload;
using Resources;
using log4net;

namespace mojoPortal.Web.GalleryUI
{
   
    public partial class FolderGalleryEditPage : NonCmsBasePage
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(FolderGalleryEditPage));

        private int pageId = -1;
        private int moduleId = -1;
        private FolderGalleryConfiguration config = new FolderGalleryConfiguration();
        private Hashtable moduleSettings = null;
        private string basePath = string.Empty;
        //private bool userIdIsEditUserId = false;
        //private bool allowEditUsersToChangeFolderPath = true;
        //private bool allowEditUsersToUpload = true;
        private Guid featureGuid = new Guid("9e58fcda-90de-4ed7-abc7-12f096f5c58f");

        

        protected void Page_Load(object sender, EventArgs e)
        {
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();

            if (!UserCanEditModule(moduleId, featureGuid))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            
            lblBasePath.Text = basePath;
            

            if (Page.IsPostBack) return;

            if (config.GalleryRootFolder.Length > 0)
            {
                txtFolderName.Text = config.GalleryRootFolder.Replace(basePath, string.Empty);
            }

        }

        

        void btnSave_Click(object sender, EventArgs e)
        {
            Module m = new Module(moduleId);

            string newPath = basePath + txtFolderName.Text;
            try
            {
                if (!Directory.Exists(Server.MapPath(newPath)))
                {
                    lblError.Text = FolderGalleryResources.FolderGalleryFolderNotExistsMessage;
                    return;
                }
            }
            catch (HttpException)
            {
                //thrown at Server.MapPath if the path is not a valid virtual path
                txtFolderName.Text = string.Empty;
                lblError.Text = FolderGalleryResources.FolderGalleryFolderNotExistsMessage;
                return;

            }


            ModuleSettings.UpdateModuleSetting(
                m.ModuleGuid,
                m.ModuleId,
                "FolderGalleryRootFolder",
                newPath);

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

        }

        void btnUpload_Click(object sender, EventArgs e)
        {
            // as long as javascript is available this code should never execute
            // because the standard file input ir replaced by javascript and the file upload happens
            // at the service url /FolderGallery/upload.ashx
            // this is fallback implementation

            string pathToGallery = basePath;

            if (config.GalleryRootFolder.Length > 0)
            {
                pathToGallery = config.GalleryRootFolder;

            }




            try
            {

                
                if (uploader.HasFile)
                {
                    string ext = Path.GetExtension(uploader.FileName);
                    if (SiteUtils.IsAllowedUploadBrowseFile(ext, ".jpg|.jpeg|.gif|.png"))
                    {
                        string destPath = Path.Combine(Server.MapPath(pathToGallery), Path.GetFileName(uploader.FileName));
                        if (File.Exists(destPath))
                        {
                            File.Delete(destPath);
                        }
                        uploader.SaveAs(destPath);
                    }
                }

                    
                

                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

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


        // previous implementation with NeatUpload
        //void btnUpload_Click(object sender, EventArgs e)
        //{
        //    string pathToGallery = basePath;

        //    if (moduleSettings.Contains("FolderGalleryRootFolder"))
        //    {
        //        pathToGallery = moduleSettings["FolderGalleryRootFolder"].ToString();

        //    }


            

        //    try
        //    {

        //        if (multiFile.Files.Length > 0)
        //        {
                    
        //            foreach (UploadedFile file in multiFile.Files)
        //            {
        //                if (file != null && file.FileName != null && file.FileName.Trim().Length > 0)
        //                {
        //                    string ext = Path.GetExtension(file.FileName);
        //                    if (SiteUtils.IsAllowedUploadBrowseFile(ext, ".jpg|.gif|.png"))
        //                    {
        //                        string destPath = Path.Combine(Server.MapPath(pathToGallery), Path.GetFileName(file.FileName));
        //                        if (File.Exists(destPath))
        //                        {
        //                            File.Delete(destPath);
        //                        }
        //                        file.MoveTo(destPath, MoveToOptions.Overwrite);
        //                    }
        //                }
                       
        //            }
        //        }

        //        //InputFile[] files = new InputFile[] { file1, file2, file3, file4, file5, file6, file7, file8, file9, file10 };
        //        //foreach (InputFile file in files)
        //        //{
        //        //    if (file != null && file.FileName != null && file.FileName.Trim().Length > 0 && IsImageFile(file))
        //        //    {
        //        //        string destPath = Path.Combine(Server.MapPath(pathToGallery), Path.GetFileName(file.FileName));
        //        //        if (File.Exists(destPath))
        //        //        {
        //        //            File.Delete(destPath);
        //        //        }
        //        //        file.MoveTo(destPath, MoveToOptions.Overwrite);
        //        //    }
        //        //}

        //        WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                
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

        //private bool IsImageFile(InputFile file)
        //{
        //    string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        //    switch (fileExtension)
        //    {
        //        case ".jpg":
        //        case ".jpeg":
        //        case ".gif":
        //        case ".png":
        //            return true;

        //        default:
        //            return false;

        //    }


        //}

        //void btnCancel_Click(object sender, EventArgs e)
        //{
        //    WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        //}


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, FolderGalleryResources.EditPageTitle);

            btnSave.Text = FolderGalleryResources.FolderGallerySaveButton;
            lnkCancel.Text = FolderGalleryResources.FolderGalleryCancelButton;
            //btnCancel.Text = FolderGalleryResources.FolderGalleryCancelButton;
            lblError.Text = string.Empty;

            btnUpload.Text = FolderGalleryResources.FolderGalleryUploadImagesButton;
            regexUpload.ErrorMessage = FolderGalleryResources.AllowedExtensionsMessage;
            //btnAddFile.Text = FolderGalleryResources.AddFileButton;

            // borowing these from Image Gallery feature instead of replicating them
            uploader.AddFilesText = GalleryResources.SelectFilesButton;
            uploader.AddFileText = GalleryResources.SelectFileButton;
            uploader.DropFilesText = GalleryResources.DropFiles;
            uploader.DropFileText = GalleryResources.DropFile;
            uploader.UploadButtonText = GalleryResources.BulkUploadButton;
            uploader.UploadCompleteText = GalleryResources.UploadComplete;
            uploader.UploadingText = GalleryResources.Uploading;

            

            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;

            }

        }

        private void LoadSettings()
        {
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            mojoSetup.EnsureFolderGalleryFolder(siteSettings);

            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);

            // previous value
            //~/Data/Sites/{0}/FolderGalleries/
            // changed to
            //~/Data/Sites/{0}/media/FolderGalleries/
            // 2013-04-04
            // did this to make more useful since that allows browsing the images from the wysiwyg editor

            //basePath = "~/Data/Sites/"
            //    + siteSettings.SiteId.ToInvariantString()
            //    + "/FolderGalleries/";
            basePath = string.Format(CultureInfo.InvariantCulture,
                FolderGalleryConfiguration.BasePathFormat,
                siteSettings.SiteId);

            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            config = new FolderGalleryConfiguration(moduleSettings);

            // this check is for backward compat with galleries previously created below ~/Data/Sites/{0}/FolderGalleries/
            if (config.GalleryRootFolder.Length > 0)
            {
                if (!config.GalleryRootFolder.StartsWith(basePath))
                {
                    // legacy path
                    basePath = "~/Data/Sites/"
                        + siteSettings.SiteId.ToInvariantString()
                        + "/FolderGalleries/";

                }
            }

            try
            {
                // ensure directory
                if (!Directory.Exists(Server.MapPath(basePath)))
                {
                    Directory.CreateDirectory(Server.MapPath(basePath));
                }
            }
            catch (IOException ex)
            {
                log.Error(ex);
            }

            //allowEditUsersToChangeFolderPath = WebUtils.ParseBoolFromHashtable(
            //    moduleSettings, "AllowEditUsersToChangeFolderPath", allowEditUsersToChangeFolderPath);

            //allowEditUsersToUpload = WebUtils.ParseBoolFromHashtable(
            //    moduleSettings, "AllowEditUsersToUpload", allowEditUsersToUpload);

            if (!WebUser.IsAdminOrContentAdmin)
            {
                pnlUpload.Visible = config.AllowEditUsersToUpload;
                pnlEdit.Visible = config.AllowEditUsersToChangeFolderPath;
            }

            uploader.MaxFilesAllowed = FolderGalleryConfiguration.MaxFilesToUploadAtOnce;
            uploader.AcceptFileTypes = SecurityHelper.GetRegexValidationForAllowedExtensionsJqueryFileUploader(WebConfigSettings.ImageFileExtensions);
            uploader.UploadButtonClientId = btnUpload.ClientID;
            uploader.ServiceUrl = SiteRoot + "/FolderGallery/upload.ashx?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString();
            uploader.FormFieldClientId = hdnState.ClientID; // not really used but prevents submitting all the form 

            string refreshFunction = "function refresh" + moduleId.ToInvariantString()
                    + " () { window.location.href = '" + SiteUtils.GetCurrentPageUrl() + "'; } ";

            uploader.UploadCompleteCallback = "refresh" + moduleId.ToInvariantString();

            ScriptManager.RegisterClientScriptBlock(
                this,
                this.GetType(), "refresh" + moduleId.ToInvariantString(),
                refreshFunction,
                true);


            AddClassToBody("foldergalleryedit");

           
           
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
            this.btnSave.Click += new EventHandler(btnSave_Click);
            //this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnUpload.Click += new EventHandler(btnUpload_Click);


        }

        

        

        #endregion
    }
}
