// Author:					
// Created:				    2010-02-12
// Last Modified:			2011-08-22
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Drawing;
using System.Xml;
using System.IO;
using System.Web;
using log4net;
using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.FileSystem;
using mojoPortal.Web.Framework;

namespace mojoPortal.Features.UI.ImageGallery
{
    public static class GalleryHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GalleryHelper));

        public static void ProcessImage(GalleryImage galleryImage, IFileSystem fileSystem, string virtualRoot, string originalFileName, Color backgroundColor)
        {
            string originalPath = virtualRoot + "FullSizeImages/" + galleryImage.ImageFile;
            string webSizeImagePath = virtualRoot + "WebImages/" + galleryImage.WebImageFile;
            string thumbnailPath = virtualRoot + "Thumbnails/" + galleryImage.ThumbnailFile;

            using (Stream stream = fileSystem.GetAsStream(originalPath))
            {
                using (Bitmap originalImage = new Bitmap(stream))
                {
                    SetExifData(galleryImage, originalImage, originalFileName);
                }
            }

            fileSystem.CopyFile(originalPath, webSizeImagePath, true);

            mojoPortal.Web.ImageHelper.ResizeImage(
                webSizeImagePath, 
                IOHelper.GetMimeType(Path.GetExtension(webSizeImagePath)), 
                galleryImage.WebImageWidth, 
                galleryImage.WebImageHeight, 
                backgroundColor);

            fileSystem.CopyFile(originalPath, thumbnailPath, true);
            
            mojoPortal.Web.ImageHelper.ResizeImage(
                thumbnailPath, 
                IOHelper.GetMimeType(Path.GetExtension(thumbnailPath)), 
                galleryImage.ThumbNailWidth, 
                galleryImage.ThumbNailHeight, 
                backgroundColor);

           
        }

        public static void DeleteImages(GalleryImage image, IFileSystem fileSystem, string virtualRoot)
        {
            string imageVirtualPath = virtualRoot + "FullSizeImages/" + image.ImageFile;

            fileSystem.DeleteFile(imageVirtualPath);

            imageVirtualPath = virtualRoot + "WebImages/" + image.WebImageFile;

            fileSystem.DeleteFile(imageVirtualPath);

            imageVirtualPath = virtualRoot + "Thumbnails/" + image.ThumbnailFile;

            fileSystem.DeleteFile(imageVirtualPath);

        }

        private static void SetExifData(GalleryImage galleryImage, Bitmap originalImage, string filePath)
        {
            XmlDocument metaData = new XmlDocument();
            metaData.XmlResolver = null;
            //if (metaData.DocumentElement == null)
            //{
                metaData.AppendChild(metaData.CreateElement("MetaData"));
            //}


            mojoPortal.Web.ImageHelper.SetMetadata("ImageFile", galleryImage.ImageFile, metaData);
            mojoPortal.Web.ImageHelper.SetMetadata("WebImageFile", galleryImage.WebImageFile, metaData);
            mojoPortal.Web.ImageHelper.SetMetadata("ThumbnailFile", galleryImage.ThumbnailFile, metaData);
            mojoPortal.Web.ImageHelper.SetMetadata("Caption", galleryImage.Caption, metaData);
            mojoPortal.Web.ImageHelper.SetMetadata("Description", galleryImage.Description, metaData);

            mojoPortal.Web.ImageHelper.SetMetadata("OriginalFilename", filePath, metaData);
            mojoPortal.Web.ImageHelper.SetMetadata("WebImageWidth", galleryImage.WebImageWidth.ToInvariantString(), metaData);
            mojoPortal.Web.ImageHelper.SetMetadata("WebImageHeight", galleryImage.WebImageHeight.ToInvariantString(), metaData);
            mojoPortal.Web.ImageHelper.SetMetadata("ThumbNailWidth", galleryImage.ThumbNailWidth.ToInvariantString(), metaData);
            mojoPortal.Web.ImageHelper.SetMetadata("ThumbNailHeight", galleryImage.ThumbNailHeight.ToInvariantString(), metaData);

            try
            {
                mojoPortal.Web.ImageHelper.SetExifInformation(originalImage, metaData);
            }
            catch(Exception ex) 
            {
                log.Error(ex);
            } 

            galleryImage.MetaDataXml = metaData.OuterXml;

            //if (mojoPortal.Web.ImageHelper.GetMetadata("GPSLatitude", metaData).Length > 0)
            //{

            //}

            //if (mojoPortal.Web.ImageHelper.GetMetadata("GPSLongitude", metaData).Length > 0)
            //{

            //}

            //if (mojoPortal.Web.ImageHelper.GetMetadata("GPSAltitude", metaData).Length > 0)
            //{

            //}

            galleryImage.Save();

        }


        
        public static bool VerifyGalleryFolders(IFileSystem fileSystem, string virtualRoot)
        {
            bool result = false;

            string originalPath = virtualRoot + "FullSizeImages/";
            string webSizeImagePath = virtualRoot + "WebImages/";
            string thumbnailPath = virtualRoot + "Thumbnails/";

            try
            {
                if (!fileSystem.FolderExists(originalPath))
                {
                    fileSystem.CreateFolder(originalPath);
                }

                if (!fileSystem.FolderExists(webSizeImagePath))
                {
                    fileSystem.CreateFolder(webSizeImagePath);
                }

                if (!fileSystem.FolderExists(thumbnailPath))
                {
                    fileSystem.CreateFolder(thumbnailPath);
                }

                result = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                log.Error("Error creating directories in GalleryHelper.VerifyGalleryFolders", ex);
            }

            return result;


        }

    }
}
