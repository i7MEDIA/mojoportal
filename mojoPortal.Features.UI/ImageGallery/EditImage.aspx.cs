/// Author:				        
/// Created:			        2004-11-28
///	Last Modified:              2013-04-03

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Features.UI.ImageGallery;
using mojoPortal.FileSystem;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.UI;

namespace mojoPortal.Web.GalleryUI
{
    public partial class GalleryImageEdit : NonCmsBasePage
	{
	
		private string imageFolderPath;
        private string fullSizeImageFolderPath;
        private int pageId = -1;
        private int moduleId = -1;
        private int itemId = -1;
        private GalleryConfiguration config = new GalleryConfiguration();
        private Hashtable moduleSettings;
        private string thumbnailBaseUrl = string.Empty;
        private string appRoot;
        private IFileSystem fileSystem = null;
        private bool newItem = false;

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);

            SiteUtils.SetupEditor(edDescription, AllowSkinOverride, this);
            
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
           
            
        }

        #endregion

        private void Page_Load(object sender, EventArgs e)
		{
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();

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

				if (itemId > -1)
				{
					PopulateControls();
					
				}
				else
				{
					this.btnDelete.Visible = false;
				}
			}

		}

		
		private void PopulateControls()
		{

			if(moduleId > -1)
			{
				if(itemId > -1)
				{
					GalleryImage galleryImage = new GalleryImage(moduleId, itemId);

                    if (galleryImage.ModuleId != moduleId)
                    {
                        SiteUtils.RedirectToAccessDeniedPage(this);
                        return;
                    }

					txtCaption.Text = Server.HtmlDecode(galleryImage.Caption);
					edDescription.Text = galleryImage.Description;
					txtDisplayOrder.Text = galleryImage.DisplayOrder.ToString();
                    imgThumb.Src = fileSystem.FileBaseUrl + thumbnailBaseUrl + galleryImage.ThumbnailFile;

				}

			}

		}

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            GalleryImage galleryImage;
            if (moduleId > -1)
            {
                if (itemId > -1)
                {
                    galleryImage = new GalleryImage(moduleId, itemId);
                }
                else
                {
                    galleryImage = new GalleryImage(moduleId);
                }

                if (galleryImage.ModuleId != moduleId)
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                    return;
                }

                Module module = GetModule(moduleId, Gallery.FeatureGuid);
                galleryImage.ModuleGuid = module.ModuleGuid;

                galleryImage.ContentChanged += new ContentChangedEventHandler(galleryImage_ContentChanged);

                int displayOrder;
                if (!Int32.TryParse(txtDisplayOrder.Text, out displayOrder))
                {
                    displayOrder = -1;
                }

                if (displayOrder > -1)
                {
                    galleryImage.DisplayOrder = displayOrder;
                }

                galleryImage.WebImageHeight = config.WebSizeHeight;
                galleryImage.WebImageWidth = config.WebSizeWidth;
                galleryImage.ThumbNailHeight = config.ThumbnailHeight;
                galleryImage.ThumbNailWidth = config.ThumbnailWidth;
                galleryImage.Description = edDescription.Text;
                galleryImage.Caption = txtCaption.Text;
                galleryImage.UploadUser = Context.User.Identity.Name;
                SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                if (siteUser != null) galleryImage.UserGuid = siteUser.UserGuid;

                // as long as javascript is available this code should never execute
                // because the standard file input ir replaced by javascript and the file upload happens
                // at the service url /ImageGallery/upload.ashx
                // this is fallback implementation

                if (uploader.HasFile)
                {
                    string ext = Path.GetExtension(uploader.FileName);
                    if (!SiteUtils.IsAllowedUploadBrowseFile(ext, WebConfigSettings.ImageFileExtensions))
                    {
                        lblMessage.Text = GalleryResources.InvalidFile;
                        
                        return;
                    }

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

                    if (galleryImage.ItemId > -1)
                    {
                        //updating with a new image so delete the previous version
                        GalleryHelper.DeleteImages(galleryImage, fileSystem, imageFolderPath);

                    }

                    
                    //using (Stream s = flImage.FileContent)
                    //{
                    //    fileSystem.SaveFile(newImagePath, s, flImage.ContentType, true);
                    //}
                    using (Stream s = uploader.FileContent)
                    {
                        
                        fileSystem.SaveFile(newImagePath, s, IOHelper.GetMimeType(Path.GetExtension(ext).ToLower()), true);
                    }

                    

                    galleryImage.ImageFile = newFileName;
                    galleryImage.WebImageFile = newFileName;
                    galleryImage.ThumbnailFile = newFileName;
                    galleryImage.Save();
                    GalleryHelper.ProcessImage(galleryImage, fileSystem, imageFolderPath, uploader.FileName, config.ResizeBackgroundColor);

                    CurrentPage.UpdateLastModifiedTime();
                    CacheHelper.ClearModuleCache(moduleId);

                    SiteUtils.QueueIndexing();
                    if (hdnReturnUrl.Value.Length > 0)
                    {
                        WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                        return;
                    }

                }
                else // not hasfile
                {	//updating a previously uploaded image
                    if (itemId > -1)
                    {
                        if (galleryImage.Save())
                        {
                            CurrentPage.UpdateLastModifiedTime();
                            CacheHelper.ClearModuleCache(moduleId);
                            SiteUtils.QueueIndexing();
                            if (newItem)
                            {
                                string thisUrl = SiteRoot + "/ImageGallery/EditImage.aspx?pageid="
                                    + pageId.ToInvariantString()
                                    + "&mid=" + moduleId.ToInvariantString()
                                    + "&ItemID=" + galleryImage.ItemId.ToInvariantString();

                                WebUtils.SetupRedirect(this, thisUrl);
                                return;

                            }
                            else
                            {
                                if (hdnReturnUrl.Value.Length > 0)
                                {
                                    WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                                    return;
                                }

                                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                            }

                        }
                    }
                }
            }
        }

        //private void btnUpdate_Click(object sender, EventArgs e)
        //{
        //    if (!Page.IsValid) return;
           
        //    GalleryImage galleryImage;
        //    if(moduleId > -1)
        //    {
        //        if(itemId > -1)
        //        {
        //            galleryImage = new GalleryImage(moduleId, itemId);
        //        }
        //        else
        //        {
        //            galleryImage = new GalleryImage(moduleId);
        //        }

        //        if (galleryImage.ModuleId != moduleId)
        //        {
        //            SiteUtils.RedirectToAccessDeniedPage(this);
        //            return;
        //        }

        //        Module module = GetModule(moduleId, Gallery.FeatureGuid);
        //        galleryImage.ModuleGuid = module.ModuleGuid;

        //        galleryImage.ContentChanged += new ContentChangedEventHandler(galleryImage_ContentChanged);

        //        int displayOrder;
        //        if (!Int32.TryParse(txtDisplayOrder.Text, out displayOrder))
        //        {
        //            displayOrder = -1;
        //        }

        //        if (displayOrder > -1)
        //        {
        //            galleryImage.DisplayOrder = displayOrder;
        //        }

        //        galleryImage.WebImageHeight = config.WebSizeHeight;
        //        galleryImage.WebImageWidth = config.WebSizeWidth;
        //        galleryImage.ThumbNailHeight = config.ThumbnailHeight;
        //        galleryImage.ThumbNailWidth = config.ThumbnailWidth;
        //        galleryImage.Description = edDescription.Text;
        //        galleryImage.Caption = txtCaption.Text;
        //        galleryImage.UploadUser = Context.User.Identity.Name;
        //        SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
        //        if (siteUser != null) galleryImage.UserGuid = siteUser.UserGuid;

        //        if (flImage.HasFile && flImage.FileName != null && flImage.FileName.Trim().Length > 0)
        //        {
        //            string ext = Path.GetExtension(flImage.FileName);
        //            if (!SiteUtils.IsAllowedUploadBrowseFile(ext, ".jpg|.gif|.png|.jpeg"))
        //            {
        //                lblMessage.Text = GalleryResources.InvalidFile;
        //                flImage.Dispose();
        //                return;
        //            }
         
        //            string newFileName = Path.GetFileName(flImage.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
        //            string newImagePath = VirtualPathUtility.Combine(fullSizeImageFolderPath, newFileName);
        //            if (galleryImage.ImageFile == newFileName)
        //            {
        //                // an existing gallery image delete the old one
        //                fileSystem.DeleteFile(newImagePath);
        //            }
        //            else
        //            {
        //                // this is a new galleryImage instance, make sure we don't use the same file name as any other instance
        //                int i = 1;
        //                while (fileSystem.FileExists(VirtualPathUtility.Combine(fullSizeImageFolderPath, newFileName)))
        //                {
        //                    newFileName = i.ToInvariantString() + newFileName;
        //                    i += 1;
        //                }

        //            }
        //            newImagePath = VirtualPathUtility.Combine(fullSizeImageFolderPath, newFileName);

        //            if (galleryImage.ItemId > -1)
        //            {
        //                //updating with a new image so delete the previous version
        //                GalleryHelper.DeleteImages(galleryImage, fileSystem, imageFolderPath);

        //            }

        //            using (flImage)
        //            {
        //                using (Stream s = flImage.FileContent)
        //                {
        //                    fileSystem.SaveFile(newImagePath, s, flImage.ContentType, true);
        //                }

        //            }  

        //            galleryImage.ImageFile = newFileName;
        //            galleryImage.WebImageFile = newFileName;
        //            galleryImage.ThumbnailFile = newFileName;
        //            galleryImage.Save();
        //            GalleryHelper.ProcessImage(galleryImage, fileSystem, imageFolderPath, flImage.FileName, config.ResizeBackgroundColor);

        //            CurrentPage.UpdateLastModifiedTime();
        //            CacheHelper.ClearModuleCache(moduleId);
                    
        //            SiteUtils.QueueIndexing();
        //            if (hdnReturnUrl.Value.Length > 0)
        //            {
        //                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
        //                return;
        //            }
                        
        //        }
        //        else
        //        {	//updating a previously uploaded image
        //            if(itemId > -1)
        //            {
        //                if(galleryImage.Save())
        //                {
        //                    CurrentPage.UpdateLastModifiedTime();
        //                    CacheHelper.ClearModuleCache(moduleId);
        //                    SiteUtils.QueueIndexing();
        //                    if (hdnReturnUrl.Value.Length > 0)
        //                    {
        //                        WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
        //                        return;
        //                    }

        //                    WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                                
        //                }
        //            }
        //        }	
        //    }
        //}

        

        void galleryImage_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["GalleryImageIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if(moduleId > -1)
			{
				if(itemId > -1)
				{
					GalleryImage galleryImage = new GalleryImage(moduleId, itemId);
                    if (galleryImage.ModuleId != moduleId)
                    {
                        SiteUtils.RedirectToAccessDeniedPage(this);
                        return;
                    }

                    galleryImage.ContentChanged += new ContentChangedEventHandler(galleryImage_ContentChanged);
                   
                    GalleryHelper.DeleteImages(galleryImage, fileSystem, imageFolderPath);
                    galleryImage.Delete();
                    CurrentPage.UpdateLastModifiedTime();
                    CacheHelper.ClearModuleCache(moduleId);
                    SiteUtils.QueueIndexing();

				}
			}

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}

        

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, GalleryResources.EditImagePageTitle);

            heading.Text = GalleryResources.GalleryEditImageLabel;

            btnUpdate.Text = GalleryResources.GalleryEditUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, GalleryResources.GalleryEditUpdateButtonAccessKey);

            ScriptConfig.EnableExitPromptForUnsavedContent = true;
            UIHelper.AddClearPageExitCode(btnUpdate);

            lnkCancel.Text = GalleryResources.GalleryEditCancelButton;
            
            btnDelete.Text = GalleryResources.GalleryEditDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, GalleryResources.GalleryEditDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialogWithClearExitCode(btnDelete, GalleryResources.GalleryDeleteImageWarning);

            imgThumb.Src = Page.ResolveUrl("~/Data/SiteImages/1x1.gif");

            regexFile.ErrorMessage = GalleryResources.OnlyImageFilesAllowed;

            uploader.AddFilesText = GalleryResources.SelectFilesButton;
            uploader.AddFileText = GalleryResources.SelectFileButton;
            uploader.DropFilesText = GalleryResources.DropFiles;
            uploader.DropFileText = GalleryResources.DropFile;
            uploader.UploadButtonText = GalleryResources.BulkUploadButton;
            uploader.UploadCompleteText = GalleryResources.UploadComplete;
            uploader.UploadingText = GalleryResources.Uploading;

            
        }

        private void LoadSettings()
        {
            appRoot = WebUtils.GetApplicationRoot();

            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            config = new GalleryConfiguration(moduleSettings);
            
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();

            //if (WebConfigSettings.ImageGalleryUseMediaFolder)
            //{
                imageFolderPath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/GalleryImages/" + moduleId.ToInvariantString() + "/";

                thumbnailBaseUrl = ImageSiteRoot + "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/GalleryImages/" + moduleId.ToInvariantString() + "/Thumbnails/"; 
            //}
            //else
            //{
            //    imageFolderPath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/GalleryImages/" + moduleId.ToInvariantString() + "/";

            //    thumbnailBaseUrl = ImageSiteRoot + "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/GalleryImages/" + moduleId.ToInvariantString() + "/Thumbnails/"; 
            //}

            fullSizeImageFolderPath = VirtualPathUtility.Combine(imageFolderPath, "FullSizeImages/");

            edDescription.WebEditor.ToolBar = ToolBar.Full;

            AddClassToBody("galleryeditimage");

            FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
            if (p == null) { return; }

            fileSystem = p.GetFileSystem();

            GalleryHelper.VerifyGalleryFolders(fileSystem, imageFolderPath);

            uploader.AcceptFileTypes = SecurityHelper.GetRegexValidationForAllowedExtensionsJqueryFileUploader(WebConfigSettings.ImageFileExtensions);
            
            uploader.ServiceUrl = SiteRoot + "/ImageGallery/upload.ashx?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString()
                + "&ItemID=" + itemId.ToInvariantString();
            // itemid will be returned into this field
            uploader.ReturnValueFormFieldClientId = hdnState.ClientID;

            uploader.FormFieldClientId = hdnState.ClientID; // not really used but prevents submitting all the form 

            string refreshFunction = "function refresh" + moduleId.ToInvariantString()
                    + " () { $('#" + btnUpdate.ClientID + "').click(); } ";

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
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", itemId);

            if ((itemId == -1) && (IsPostBack))
            {
                // itemid returned from upload service when new image uploaded
                if (hdnState.Value.Length > 0)
                {
                    int.TryParse(hdnState.Value, out itemId);
                    newItem = (itemId > -1);
                }

            }

        }

	}
}
