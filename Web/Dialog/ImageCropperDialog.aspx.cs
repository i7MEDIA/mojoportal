//  Author:                     
//  Created:                    2009-09-20
//	Last Modified:              2011-09-05
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Web;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using Resources;

namespace mojoPortal.Web.Dialog
{
    /// <summary>
    /// A dialog page for cropping images
    /// </summary>
    public partial class ImageCropperDialog : System.Web.UI.Page
    {
        private SiteSettings siteSettings = null;
        private string rootDirectory = "/Data/";
        private bool canEdit = false;
        private string sourceImageVirtualPath = string.Empty;
        private string destImageVirtualPath = string.Empty;
        private bool sourceExists = false;
        private bool isAllowedPath = false;
        private string returnUrl = string.Empty;
        private IFileSystem fileSystem = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            if (!canEdit)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return; 
            }

            if (isAllowedPath) { SetupCropper(); }

        }

        private void SetupCropper()
        {
            cropper.SourceImagePath = sourceImageVirtualPath;
            cropper.ResultImagePath = destImageVirtualPath;
            cropper.AllowUserToChooseCroppedFileName = true;
            cropper.AllowUserToSetFinalFileSize = true;
            cropper.InitialSelectionX = 100;
            cropper.InitialSelectionY = 100;
            cropper.InitialSelectionX2 = 50;
            cropper.InitialSelectionY2 = 50;
        }

        private void PopulateLabels()
        {
            this.Title = Resource.CropImageLink;
            lnkReturn.Text = Resource.CropperGoBackLink;
        }

        private void LoadSettings()
        {
            if (Request.QueryString["return"] != null)
            {
                returnUrl = Request.QueryString["return"];
                lnkReturn.NavigateUrl = returnUrl;
                lnkReturn.Visible = true;
            }

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
            if (p == null) { return; }

            fileSystem = p.GetFileSystem();
            if (fileSystem == null) { return; }

            rootDirectory = fileSystem.VirtualRoot;

            if (WebUser.IsAdminOrContentAdmin)
            { 
                canEdit = true;
            }
            else if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                canEdit = true;
            }
            else if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                canEdit = true;
            }

            if (Request.QueryString["src"] != null)
            {
                sourceImageVirtualPath = Request.QueryString["src"];

                if (sourceImageVirtualPath.Length > 0)
                {
                    if((fileSystem.FileBaseUrl.Length > 0)&&(sourceImageVirtualPath.StartsWith(fileSystem.FileBaseUrl)))
                    {
                        sourceImageVirtualPath = sourceImageVirtualPath.Substring(fileSystem.FileBaseUrl.Length);
                    }

                    isAllowedPath = WebFolder.IsDecendentVirtualPath(rootDirectory, sourceImageVirtualPath);
                    sourceExists = fileSystem.FileExists(sourceImageVirtualPath);
                    isAllowedPath = WebFolder.IsDecendentVirtualPath(rootDirectory, sourceImageVirtualPath);
                }
            }

            if (sourceImageVirtualPath.Length == 0) 
            {
                cropper.Visible = false;
                return; 
            }

           
            destImageVirtualPath = VirtualPathUtility.Combine(VirtualPathUtility.GetDirectory(sourceImageVirtualPath),
                    Path.GetFileNameWithoutExtension(sourceImageVirtualPath) + "crop" + VirtualPathUtility.GetExtension(sourceImageVirtualPath));

           
            SiteUtils.SetFormAction(Page, Request.RawUrl);


        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);
        }
    }
}
