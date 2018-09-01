// Author:					
// Created:					2009-09-25
// Last Modified:			2013-04-02
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.UI;
//using Brettle.Web.NeatUpload;
using mojoPortal.Business;
using mojoPortal.FileSystem;
using mojoPortal.Web.Framework;
using mojoPortal.Features.UI.ImageGallery;
using Resources;



namespace mojoPortal.Web.GalleryUI
{

    public partial class BulkUploadPage : NonCmsBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private string imageFolderPath;
        private string fullSizeImageFolderPath;
        private GalleryConfiguration config = new GalleryConfiguration();
        private Hashtable moduleSettings;
        private IFileSystem fileSystem = null;
       

        protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadParams();

            if (!UserCanEditModule(moduleId, Gallery.FeatureGuid))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            if (fileSystem == null)
            {
                WebUtils.SetupRedirect(this, SiteRoot);
                return;
            }

            PopulateLabels();

            if (!IsPostBack)
            {
                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                    lnkCancel.NavigateUrl = hdnReturnUrl.Value;
                }
            }

        }

        void btnUpload_Click(object sender, EventArgs e)
        {
            // as long as javascript is available this code should never execute
            // because the standard file input ir replaced by javascript and the file upload happens
            // at the service url /ImageGallery/upload.ashx
            // this is fallback implementation

            Module module = GetModule(moduleId, Gallery.FeatureGuid);

            if (module == null)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            SiteUser siteUser = SiteUtils.GetCurrentSiteUser();

            try
            {

                if (uploader.HasFile)
                {

                    string ext = Path.GetExtension(uploader.FileName);
                    if (SiteUtils.IsAllowedUploadBrowseFile(ext, WebConfigSettings.ImageFileExtensions))
                    {
                        GalleryImage galleryImage = new GalleryImage(this.moduleId);
                        galleryImage.ModuleGuid = module.ModuleGuid;
                        galleryImage.WebImageHeight = config.WebSizeHeight;
                        galleryImage.WebImageWidth = config.WebSizeWidth;
                        galleryImage.ThumbNailHeight = config.ThumbnailHeight;
                        galleryImage.ThumbNailWidth = config.ThumbnailWidth;
                        galleryImage.UploadUser = Context.User.Identity.Name;

                        if (siteUser != null) galleryImage.UserGuid = siteUser.UserGuid;

                        //string newFileName = Path.GetFileName(file.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
                        string newFileName = Path.GetFileName(uploader.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
                        string newImagePath = VirtualPathUtility.Combine(fullSizeImageFolderPath, newFileName);

                        if (galleryImage.ImageFile == newFileName)
                        {
                            // an existing gallery image delete the old one
                            fileSystem.DeleteFile(newImagePath);
                        }
                        else
                        {
                            // this is a new galleryImage instance, make sure we don't use the same file name as any other instance
                            int i = 1;
                            while (fileSystem.FileExists(VirtualPathUtility.Combine(fullSizeImageFolderPath, newFileName)))
                            {
                                newFileName = i.ToInvariantString() + newFileName;
                                i += 1;
                            }

                        }

                        newImagePath = VirtualPathUtility.Combine(fullSizeImageFolderPath, newFileName);

                        
                        using (Stream s = uploader.FileContent)
                        {
                            //fileSystem.SaveFile(newImagePath, s, uploader.FileContentType, true);
                            fileSystem.SaveFile(newImagePath, s, IOHelper.GetMimeType(Path.GetExtension(ext).ToLower()), true);
                        }
                       

                        galleryImage.ImageFile = newFileName;
                        galleryImage.WebImageFile = newFileName;
                        galleryImage.ThumbnailFile = newFileName;
                        galleryImage.Save();
                        GalleryHelper.ProcessImage(galleryImage, fileSystem, imageFolderPath, uploader.FileName, config.ResizeBackgroundColor);
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
        //    Module module = GetModule(moduleId, Gallery.FeatureGuid);

        //    if (module == null)
        //    {
        //        SiteUtils.RedirectToAccessDeniedPage(this);
        //        return;
        //    }

        //    SiteUser siteUser = SiteUtils.GetCurrentSiteUser();

        //    try
        //    {

        //        if (multiFile.Files.Length > 0)
        //        {

        //            foreach (UploadedFile file in multiFile.Files)
        //            {
        //                if (file != null && file.FileName != null && file.FileName.Trim().Length > 0)
        //                {
        //                    string ext = Path.GetExtension(file.FileName);
        //                    if (SiteUtils.IsAllowedUploadBrowseFile(ext, ".jpg|.gif|.png|.jpeg"))
        //                    {
        //                        GalleryImage galleryImage = new GalleryImage(this.moduleId);
        //                        galleryImage.ModuleGuid = module.ModuleGuid;
        //                        galleryImage.WebImageHeight = config.WebSizeHeight;
        //                        galleryImage.WebImageWidth = config.WebSizeWidth;
        //                        galleryImage.ThumbNailHeight = config.ThumbnailHeight;
        //                        galleryImage.ThumbNailWidth = config.ThumbnailWidth;
        //                        galleryImage.UploadUser = Context.User.Identity.Name;

        //                        if (siteUser != null) galleryImage.UserGuid = siteUser.UserGuid;

        //                        //string newFileName = Path.GetFileName(file.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
        //                        string newFileName = Path.GetFileName(file.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
        //                        string newImagePath = VirtualPathUtility.Combine(fullSizeImageFolderPath, newFileName);

        //                        if (galleryImage.ImageFile == newFileName)
        //                        {
        //                            // an existing gallery image delete the old one
        //                            fileSystem.DeleteFile(newImagePath);
        //                        }
        //                        else
        //                        {
        //                            // this is a new galleryImage instance, make sure we don't use the same file name as any other instance
        //                            int i = 1;
        //                            while (fileSystem.FileExists(VirtualPathUtility.Combine(fullSizeImageFolderPath, newFileName)))
        //                            {
        //                                newFileName = i.ToInvariantString() + newFileName;
        //                                i += 1;
        //                            }

        //                        }

        //                        newImagePath = VirtualPathUtility.Combine(fullSizeImageFolderPath, newFileName);

        //                        using (file)
        //                        {
        //                            using (Stream s = file.OpenRead())
        //                            {
        //                                fileSystem.SaveFile(newImagePath, s, file.ContentType, true);
        //                            }
        //                        }

        //                        galleryImage.ImageFile = newFileName;
        //                        galleryImage.WebImageFile = newFileName;
        //                        galleryImage.ThumbnailFile = newFileName;
        //                        galleryImage.Save();
        //                        GalleryHelper.ProcessImage(galleryImage, fileSystem, imageFolderPath, file.FileName, config.ResizeBackgroundColor);
        //                    }

        //                }

        //            }
        //        }
                
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


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, GalleryResources.BulkUploadHeading);
            heading.Text = GalleryResources.BulkUploadHeading;
            //btnAddFile.Text = GalleryResources.AddFileButton;
            btnUpload.Text = GalleryResources.BulkUploadButton;
            lnkCancel.Text = GalleryResources.GalleryEditCancelButton;

            uploader.AddFilesText = GalleryResources.SelectFilesButton;
            uploader.AddFileText = GalleryResources.SelectFileButton;
            uploader.DropFilesText = GalleryResources.DropFiles;
            uploader.DropFileText = GalleryResources.DropFile;
            uploader.UploadButtonText = GalleryResources.BulkUploadButton;
            uploader.UploadCompleteText = GalleryResources.UploadComplete;
            uploader.UploadingText = GalleryResources.Uploading;

            regexUpload.ErrorMessage = GalleryResources.OnlyImageFilesAllowed;
        }

        private void LoadSettings()
        {
            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            config = new GalleryConfiguration(moduleSettings);   
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();

            if (WebConfigSettings.ImageGalleryUseMediaFolder)
            {
                imageFolderPath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/GalleryImages/" + moduleId.ToInvariantString() + "/";
            }
            else
            {
                imageFolderPath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/GalleryImages/" + moduleId.ToInvariantString() + "/";
            }

            fullSizeImageFolderPath = imageFolderPath + "FullSizeImages/";

            AddClassToBody("gallerybulkupload");

            FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
            if (p == null) { return; }

            fileSystem = p.GetFileSystem();

            GalleryHelper.VerifyGalleryFolders(fileSystem, imageFolderPath);

            uploader.MaxFilesAllowed = GalleryConfiguration.MaxFilesToUploadAtOnce;
            uploader.AcceptFileTypes = SecurityHelper.GetRegexValidationForAllowedExtensionsJqueryFileUploader(WebConfigSettings.ImageFileExtensions);
            uploader.UploadButtonClientId = btnUpload.ClientID;
            uploader.ServiceUrl = SiteRoot + "/ImageGallery/upload.ashx?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString() ;
            uploader.FormFieldClientId = hdnState.ClientID; // not really used but prevents submitting all the form 

            string refreshFunction = "function refresh" + moduleId.ToInvariantString()
                    + " () { window.location.href = '" + SiteUtils.GetCurrentPageUrl() + "'; } ";

            uploader.UploadCompleteCallback = "refresh" + moduleId.ToInvariantString();

            ScriptManager.RegisterClientScriptBlock(
                this,
                this.GetType(), "refresh" + moduleId.ToInvariantString(),
                refreshFunction,
                true);


           

        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);

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
            btnUpload.Click += new EventHandler(btnUpload_Click);

        }

        

        #endregion
    }
}
