using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web.Framework;
//using Brettle.Web.NeatUpload;
using Resources;

namespace mojoPortal.Web.Dialog
{
	/// <summary>
	/// a page to upload and crop user avatars
	/// </summary>
	public partial class AvatarUploadDialog : mojoDialogBasePage
	{
        private int userId = -1;
        private bool disableAvatars = true;
        private bool canEdit = false;
        private string avatarBasePath = string.Empty;
        private SiteUser currentUser = null;
        private SiteUser selectedUser = null;
        private SiteSettings siteSettings = null;
        private IFileSystem fileSystem = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if(disableAvatars)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            if (!canEdit)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            PopulateLabels();

            //if (!Page.IsPostBack)
            //{
                PopulateControls();
            //}

        }

        private void PopulateControls()
        {
            if (selectedUser == null) { return; }

            if (selectedUser.AvatarUrl.Length > 0)
            {
                cropper.ResultImagePath = avatarBasePath + selectedUser.AvatarUrl;
                string fullSizeFileName = GetFullSizeImageName();
                if (fullSizeFileName.Length > 0)
                {
                    cropper.SourceImagePath = avatarBasePath + fullSizeFileName;
                }
            }

            if (WebConfigSettings.ForceSquareAvatars) { cropper.AspectRatio = 1; }

            cropper.MinWidth = WebConfigSettings.AvatarMaxWidth;
            cropper.MinHeight = WebConfigSettings.AvatarMaxHeight;

            cropper.FinalMaxWidth = WebConfigSettings.AvatarMaxWidth;
            cropper.FinalMaxHeight = WebConfigSettings.AvatarMaxHeight;

            cropper.InitialSelectionX = 100;
            cropper.InitialSelectionY = 100;
            cropper.InitialSelectionX2 = cropper.InitialSelectionX + cropper.FinalMaxWidth;
            cropper.InitialSelectionY2 = cropper.InitialSelectionY + cropper.FinalMaxHeight;
        }

        private string GetFullSizeImageName()
        {
            string fullSizeFileName = string.Empty;
            if (selectedUser.AvatarUrl.Length > 0)
            {
                fullSizeFileName = "user" + selectedUser.UserId.ToInvariantString() + "fullsize" + Path.GetExtension(selectedUser.AvatarUrl);
                if (fileSystem.FileExists(avatarBasePath + fullSizeFileName)) { return fullSizeFileName; }
            }

            fullSizeFileName = "user" + selectedUser.UserId.ToInvariantString() + "fullsize.jpg";
            if (fileSystem.FileExists(avatarBasePath + fullSizeFileName)) { return fullSizeFileName; }

            fullSizeFileName = "user" + selectedUser.UserId.ToInvariantString() + "fullsize.gif";
            if (fileSystem.FileExists(avatarBasePath + fullSizeFileName)) { return fullSizeFileName; }

            fullSizeFileName = "user" + selectedUser.UserId.ToInvariantString() + "fullsize.png";
            if (fileSystem.FileExists(avatarBasePath + fullSizeFileName)) { return fullSizeFileName; }

            
            return string.Empty;

        }

        private string GetBaseImageName()
        {
            //return "user" + selectedUser.UserId.ToInvariantString() + "-" + selectedUser.Name.ToCleanFileName() + "-" + DateTime.UtcNow.Millisecond.ToInvariantString();
            return "user" + selectedUser.UserId.ToInvariantString() + "-" + selectedUser.Name.ToCleanFileName();
        }

        void btnUploadAvatar_Click(object sender, EventArgs e)
        {
            if (selectedUser == null) { return; }

            if (uploader.HasFile)
            {
                if (!fileSystem.FolderExists(avatarBasePath)) { fileSystem.CreateFolder(avatarBasePath); }
                string newFileName = "user" + selectedUser.UserId.ToInvariantString() + "fullsize" + Path.GetExtension(uploader.FileName).ToLower();

                string destPath = avatarBasePath + newFileName;
                string ext = Path.GetExtension(uploader.FileName);
                string mimeType = IOHelper.GetMimeType(ext).ToLower();

                if (!fileSystem.FolderExists(avatarBasePath)) { fileSystem.CreateFolder(avatarBasePath); }

                if (SiteUtils.IsAllowedUploadBrowseFile(ext, WebConfigSettings.ImageFileExtensions))
                {
                    fileSystem.DeleteFile(destPath);
                    fileSystem.SaveFile(destPath, uploader.FileContent, mimeType, true);
                    

                    // limit the size of the full size image to something reasonable
                    mojoPortal.Web.ImageHelper.ResizeImage(
                        destPath, 
                        IOHelper.GetMimeType(ext), 
                        WebConfigSettings.AvatarMaxOriginalWidth, 
                        WebConfigSettings.AvatarMaxOriginalHeight, 
                        WebConfigSettings.DefaultResizeBackgroundColor);

                    //create initial crop
                    string croppedFileName = GetBaseImageName() + Path.GetExtension(uploader.FileName).ToLower();
                    string destCropFile = avatarBasePath + croppedFileName;
                    fileSystem.CopyFile(destPath, destCropFile, true);

                    if (WebConfigSettings.ForceSquareAvatars)
                    {
                        mojoPortal.Web.ImageHelper.ResizeAndSquareImage(
                            destCropFile, 
                            IOHelper.GetMimeType(ext), 
                            WebConfigSettings.AvatarMaxWidth, 
                            WebConfigSettings.DefaultResizeBackgroundColor);
                    }
                    else
                    {
                        mojoPortal.Web.ImageHelper.ResizeImage(
                            destCropFile, 
                            IOHelper.GetMimeType(ext), 
                            WebConfigSettings.AvatarMaxWidth, 
                            WebConfigSettings.AvatarMaxHeight, 
                            WebConfigSettings.DefaultResizeBackgroundColor);
                    }

                    selectedUser.AvatarUrl = croppedFileName;
                    selectedUser.Save();


                }
                
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        private void PopulateLabels()
        {
            btnUploadAvatar.Text = Resource.UploadAvatarButton;
            regexAvatarFile.ErrorMessage = Resource.FileTypeNotAllowed;
            regexAvatarFile.ValidationExpression = SecurityHelper.GetRegexValidationForAllowedExtensions(WebConfigSettings.ImageFileExtensions);
            lblUploadNewAvatar.Text = Resource.UploadAvatarLink;
        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            avatarBasePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/useravatars/";
            userId = WebUtils.ParseInt32FromQueryString("u", true, userId);
            currentUser = SiteUtils.GetCurrentSiteUser();

			//if (Page is mojoDialogBasePage)
			//{
			//	mojoDialogBasePage basePage = Page as mojoDialogBasePage;
   //             PlaceHolder phHead = basePage.FindControl("phHead") as PlaceHolder;
   //             if (phHead != null)
   //             {
   //                 phHead.Controls.Add(new Literal { Text = $"<script src=\"{WebUtils.ResolveUrl($"~/ClientScript/jcrop3/jcrop.js")}\" data-loader=\"AvatarUploadDialog\"></script>" });
   //                 phHead.Controls.Add(new Literal { Text = $"<link rel=\"stylesheet\" href=\"{WebUtils.ResolveUrl($"~/ClientScript/jcrop3/jcrop.css")}\" data-loader=\"AvatarUploadDialog\"/>" });
			//	}
			//}

			if ((currentUser != null) && (currentUser.UserId == userId) && (userId != -1)) 
            {
                selectedUser = currentUser;
                canEdit = true; 
            }

            if (WebUser.IsAdmin) 
            { 
                canEdit = true;
                if ((selectedUser == null) && (userId != -1))
                {
                    selectedUser = new SiteUser(siteSettings, userId);
                    if (selectedUser.UserId != userId) { selectedUser = null; }
                }
            }
           

            switch (siteSettings.AvatarSystem)
            {
                case "gravatar":
                    
                    disableAvatars = true;
                    break;

                case "internal":
                    
                    disableAvatars = false;
                   
                    if ((WebConfigSettings.AvatarsCanOnlyBeUploadedByAdmin)&&(!WebUser.IsAdmin))
                    {
                        canEdit = false;  
                    }

                    break;

                case "none":
                default:
                    
                    disableAvatars = true;
                    break;

            }

            FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
            if (p == null) { return; }

            fileSystem = p.GetFileSystem();
            

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);
            btnUploadAvatar.Click += new EventHandler(btnUploadAvatar_Click);
        }

    }
}
