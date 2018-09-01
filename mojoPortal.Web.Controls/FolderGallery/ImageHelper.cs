using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Caching;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.drew.metadata.exif;
using com.drew.metadata.jpeg;
using com.drew.metadata.iptc;

using MetadataDirectory = com.drew.metadata.AbstractDirectory;
using Metadata = com.drew.metadata.Metadata;
using Tag = com.drew.metadata.Tag;

// Photo Album handler
// Originally created by Dmitry Robsman
// Modified by Bertrand Le Roy
// with the participation of David Ebbo
// http://www.codeplex.com/PhotoHandler
//
// Uses the Metadata public domain library by
// Drew Noakes (drew@drewnoakes.com)
// and adapted for .NET by Renaud Ferret (renaud91@free.fr)
// The library can be downloaded from
// http://renaud91.free.fr/MetaDataExtractor/
//
// further modifications and integration with mojoPortal by 
//  Last Modified: 2008-02-08
//  2008-05-19 changes to support Medium Trust

namespace mojoPortal.Web.Controls.FolderGallery
{
    public static class ImageHelper
    {
        // the following constants constitute the handler's configuration:

        /// <summary>
        /// Size in pixels of the thumbnails.
        /// </summary>
        public const int ThumbnailSize = 120;

        /// <summary>
        /// Size in pixels of the preview images.
        /// </summary>
        public static int PreviewSize()
        {
            
                int previewSize = 700;
                if (ConfigurationManager.AppSettings["FolderGalleryPreviewWidth"] != null)
                {
                    try
                    {
                        int.TryParse(ConfigurationManager.AppSettings["FolderGalleryPreviewWidth"], NumberStyles.Integer, CultureInfo.InvariantCulture, out previewSize);
                    }
                    catch (ArgumentException) { }

                }

                return previewSize;
            
        }

        /// <summary>
        /// The background color of the image thumbnails and previews.
        /// </summary>
        private static readonly Color BackGround = Color.Transparent;

        /// <summary>
        /// The color used to draw borders around image thumbnails and previews.
        /// </summary>
        private static readonly Color BorderColor = Color.Black;

        /// <summary>
        /// The width in pixels of the thumbnail border.
        /// </summary>
        private static readonly float ThumbnailBorderWidth = 2;

        /// <summary>
        /// The width in pixels of the image previews.
        /// </summary>
        private static readonly float PreviewBorderWidth = 3;

        /// <summary>
        /// The color of the shadow that's drawn around up folder stacked image thumbnails.
        /// </summary>
        private static readonly Color UpFolderBorderColor = Color.FromArgb(90, Color.Black);

        /// <summary>
        /// The width in pixels of the shadow that's drawn around up folder stacked image thumbnails.
        /// </summary>
        private static readonly float UpFolderBorderWidth = 2;

        /// <summary>
        /// The color of the arrow that's drawn on up folder thumbnails.
        /// </summary>
        private static readonly Color UpArrowColor = Color.FromArgb(200, Color.Beige);

        /// <summary>
        /// The color of the arrow that's drawn on up folder thumbnails.
        /// </summary>
        private static readonly float UpArrowWidth = 4;

        /// <summary>
        /// The relative size of the arrow that's drawn on up folder thumbnails.
        /// </summary>
        private static readonly float UpArrowSize = 0.125F;

        /// <summary>
        /// The number of images on the up folder thumbnail stack.
        /// </summary>
        private static readonly int UpFolderStackHeight = 3;

        /// <summary>
        /// The quality (between 0 and 100) of the thumbnail JPEGs.
        /// </summary>
        private static readonly long ThumbnailJpegQuality = 75L;

        /// <summary>
        /// Location of the cache.
        /// Can be Disk (recommended), Memory or None.
        /// </summary>
        private const ImageCacheLocation Caching = ImageCacheLocation.Disk;

        /// <summary>
        /// If using memory cache, the duration in minutes of the sliding expiration.
        /// </summary>
        private const int MemoryCacheSlidingDuration = 10;

        /// <summary>
        /// The default CSS that's requested by the header.
        /// </summary>
        private const string Css = @"
body, td
{
	font-family: Verdana, Arial, Helvetica;
	font-size: xx-small;
	color: Gray;
	background-color: Black;
}

a 
{
	color: White;
}

img 
{
	border: none;
}

.album 
{
}

.albumFloat 
{
    float: left;
    text-align: center;
    margin-right: 8px;
    margin-bottom: 4px;
}

.albumDetailsLink 
{
}

.albumMetaSectionHead 
{
	background-color: Gray;
	color: White;
	font-weight: bold;
}

.albumMetaName 
{
	font-weight: bold;
}

.albumMetaValue 
{
}
";

        private static string _imageCacheDir;
        private static string _appPath;

        /// <summary>
        /// Static constructor that sets up the caching directory
        /// </summary>
        static ImageHelper()
        {
#pragma warning disable 162
            if (Caching == ImageCacheLocation.Disk)
            {
                if (
                    (HttpContext.Current != null)
                    &&(ConfigurationManager.AppSettings["FolderGalleryCachePath"] != null)
                    )
                {
                    string baseDir = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FolderGalleryCachePath"]);

                    _imageCacheDir = Path.Combine(baseDir, "FolderGalleryCache");


                }
                else
                {

                    _imageCacheDir = Path.Combine(HttpRuntime.CodegenDir, "Album");
                }

                //  added this check 2009-04-05
                if (!Directory.Exists(_imageCacheDir))
                {
                    Directory.CreateDirectory(_imageCacheDir);
                }

                _appPath = HttpRuntime.AppDomainAppPath;
                if (_appPath[_appPath.Length - 1] == '\\')
                {
                    _appPath = _appPath.Substring(0, _appPath.Length - 1);
                }
            }
#pragma warning restore 162
        }

        /// <summary>
        /// Gets the path for a cached image and its status.
        /// </summary>
        /// <param name="path">The path to the image.</param>
        /// <param name="imageTypeModifier">Type of image.</param>
        /// <param name="cachedPath">The physical path of the cached image (out parameter).</param>
        /// <returns>True if the cached image exists and is not outdated.</returns>
        public static bool GetCachedPathAndCheckCacheStatus(
            string path,
            string imageTypeModifier,
            out string cachedPath)
        {

            string cacheKey = imageTypeModifier + "_" + path.Substring(_appPath.Length);
            cacheKey = cacheKey.Replace('\\', '_').Replace(':', '_');
            cachedPath = Path.Combine(_imageCacheDir, cacheKey);

            return File.Exists(cachedPath) && File.GetLastWriteTime(cachedPath) > File.GetLastWriteTime(path);
        }

        /// <summary>
        /// Sends the default CSS to the response.
        /// </summary>
        /// <param name="response">The response where to write the CSS.</param>
        public static void GenerateCssResponse(HttpResponse response)
        {
            //response.Write(Css);
        }

        /// <summary>
        /// Outputs a resized image to the HttpResponse object
        /// </summary>
        public static void GenerateResizedImageResponse(
            Color backgroundColor, 
            Color borderColor,
            string imageFile,
            int size,
            bool thumbnail,
            HttpResponse response)
        {

            string buildPath = null;

#pragma warning disable 162
            switch (Caching)
            {
                case ImageCacheLocation.Disk:
                    if (GetCachedPathAndCheckCacheStatus(imageFile, (thumbnail ? "thumbnail" : "preview"), out buildPath))
                    {
                        WriteNewImage(buildPath, response);
                        return;
                    }
                    break;
                case ImageCacheLocation.Memory:
                    if (thumbnail)
                    {
                        buildPath = (thumbnail ? "thumbnail_" : "preview_") + imageFile.Replace('\\', '_').Replace(':', '_');
                        byte[] cachedBytes = (byte[])HttpRuntime.Cache.Get(buildPath);

                        if (cachedBytes != null)
                        {
                            WriteNewImage(cachedBytes, response);
                            return;
                        }
                    }
                    break;
            }
#pragma warning restore 162

            using (Bitmap originalImage = LoadOriginalImage(imageFile))
            {
                using (Bitmap newImage = CreateNewImage(backgroundColor, borderColor, originalImage, size, thumbnail))
                {

                    WriteNewImage(newImage, response);

#pragma warning disable 162
                    switch (Caching)
                    {
                        case ImageCacheLocation.Disk:
                            File.WriteAllBytes(buildPath, GetImageBytes(newImage));
                            break;
                        case ImageCacheLocation.Memory:
                            if (thumbnail)
                            {
                                HttpRuntime.Cache.Insert(buildPath, GetImageBytes(newImage),
                                    new CacheDependency(imageFile), DateTime.MaxValue, TimeSpan.FromMinutes(MemoryCacheSlidingDuration));
                            }
                            break;
                    }
#pragma warning restore 162
                }
            }
        }

        /// <summary>
        /// Outputs a folder thumbnail to the HttpResponse.
        /// </summary>
        public static void GenerateFolderImageResponse(
            Color backColorToUse,
            Color borderColorToUse, 
            bool isParentFolder,
            string folder,
            int size,
            HttpResponse response)
        {

            string buildPath = null;

#pragma warning disable 162
            switch (Caching)
            {
                case ImageCacheLocation.Disk:
                    if (GetCachedPathAndCheckCacheStatus(folder, (isParentFolder ? "parent" : "folder"), out buildPath))
                    {
                        WriteNewImage(buildPath, response);
                        return;
                    }
                    break;
                case ImageCacheLocation.Memory:
                    {
                        buildPath = (isParentFolder ? "parent_" : "folder_") + folder.Replace('\\', '_').Replace(':', '_');
                        byte[] cachedBytes = (byte[])HttpRuntime.Cache.Get(buildPath);

                        if (cachedBytes != null)
                        {
                            WriteNewImage(cachedBytes, response);
                            return;
                        }
                    }
                    break;
            }
#pragma warning restore 162

            Bitmap folderImage = CreateFolderImage(
                backColorToUse,
                borderColorToUse, 
                isParentFolder, 
                folder, 
                size);

#pragma warning disable 162
            switch (Caching)
            {
                case ImageCacheLocation.Disk:
                    File.WriteAllBytes(buildPath, GetImageBytes(folderImage));
                    break;
                case ImageCacheLocation.Memory:
                    HttpRuntime.Cache.Insert(buildPath, GetImageBytes(folderImage),
                        null, DateTime.MaxValue, TimeSpan.FromMinutes(MemoryCacheSlidingDuration));
                    break;
            }

            WriteNewImage(folderImage, response);
            folderImage.Dispose();
        }
#pragma warning restore 162

        /// <summary>
        /// Prepares a string to be used in JScript by escaping characters.
        /// </summary>
        /// <remarks>This is not a general purpose escaping function:
        /// we do not encode characters that can't be in Windows paths.
        /// It should not be used in general to prevent injection attacks.</remarks>
        /// <param name="unencoded">The unencoded string.</param>
        /// <returns>The encoded string.</returns>
        public static string JScriptEncode(string unencoded)
        {
            System.Text.StringBuilder sb = null;
            int checkedIndex = 0;
            int len = unencoded.Length;
            for (int i = 0; i < len; i++)
            {
                char c = unencoded[i];
                if ((c == '\'') || (c == '\"'))
                {
                    if (sb == null)
                    {
                        sb = new System.Text.StringBuilder(len + 1);
                    }
                    sb.Append(unencoded.Substring(checkedIndex, i - checkedIndex));
                    sb.Append('\\');
                    sb.Append(unencoded[i]);
                    checkedIndex = i + 1;
                }
            }
            if (sb == null)
            {
                return unencoded;
            }
            if (checkedIndex < len)
            {
                sb.Append(unencoded.Substring(checkedIndex));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates a Bitmap object from a file.
        /// </summary>
        /// <param name="imageFile">The path ofthe image.</param>
        /// <returns>The bitmap object.</returns>
        static Bitmap LoadOriginalImage(string imageFile)
        {
            return (Bitmap)Bitmap.FromFile(imageFile, false);
        }

        /// <summary>
        /// Creates a reduced Bitmap from a full-size Bitmap.
        /// </summary>
        static Bitmap CreateNewImage(Color backgroundColor, Color borderColor, Bitmap originalImage, int size, bool thumbnail)
        {
            int originalWidth = originalImage.Width;
            int originalHeight = originalImage.Height;
            int width, height;
            int drawXOffset, drawYOffset, drawWidth, drawHeight;

            if (size > 0 && originalWidth >= originalHeight && originalWidth > size)
            {
                width = size;
                height = Convert.ToInt32((double)size * (double)originalHeight / (double)originalWidth);
            }
            else if (size > 0 && originalHeight >= originalWidth && originalHeight > size)
            {
                width = Convert.ToInt32((double)size * (double)originalWidth / (double)originalHeight);
                height = size;
            }
            else
            {
                width = originalWidth;
                height = originalHeight;
            }

            drawXOffset = 0;
            drawYOffset = 0;
            drawWidth = width;
            drawHeight = height;

            if (thumbnail)
            {
                width = Math.Max(width, size);
                height = Math.Max(height, size);
                drawXOffset = (width - drawWidth) / 2;
                drawYOffset = (height - drawHeight) / 2;
            }

            Bitmap newImage = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(newImage);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float borderWidth = thumbnail ? ThumbnailBorderWidth : PreviewBorderWidth;
            g.FillRectangle(new SolidBrush(backgroundColor), 0, 0, width, height);
            Pen BorderPen = new Pen(borderColor);
            BorderPen.Width = borderWidth;
            BorderPen.LineJoin = LineJoin.Round;
            BorderPen.StartCap = LineCap.Round;
            BorderPen.EndCap = LineCap.Round;
            g.DrawRectangle(BorderPen,
                drawXOffset + borderWidth / 2,
                drawYOffset + borderWidth / 2,
                drawWidth - borderWidth,
                drawHeight - borderWidth);

            g.DrawImage(originalImage,
                drawXOffset + borderWidth,
                drawYOffset + borderWidth,
                drawWidth - 2 * borderWidth,
                drawHeight - 2 * borderWidth);
            g.Dispose();
            return newImage;
        }

		
	    /// <summary>
		/// Gets image file extensions from web.config and adds * to each.
		/// </summary>
		/// <returns></returns>
	    public static string[] GetImageExtensions()
	    {
		    var imgExtensions = ConfigurationManager.AppSettings["ImageFileExtensions"] ?? ".gif|.jpg|.jpeg|.png|.svg|.webp";

		    // Split on pipe and append asterisk on each file extension
		    return imgExtensions.Split('|').Select(i => $"*{i.Trim()}").ToArray();
		}


	    /// <summary>
        /// Creates the thumbnail Bitmap for a folder.
        /// </summary>
        static Bitmap CreateFolderImage(
            Color backgroundColor,
            Color borderColor,
            bool isParentFolder, 
            string folder, 
            int size)
        {
            Bitmap newImage = new Bitmap(size, size);
            Graphics g = Graphics.FromImage(newImage);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillRectangle(new SolidBrush(backgroundColor), 0, 0, size, size);

            Random rnd = new Random();
            List<string> imagesToDraw = new List<string>();
            int nbFound;
            string[] images = Directory.GetFiles(folder, "default.jpg", SearchOption.AllDirectories);

	        for (nbFound = 0; nbFound < Math.Min(UpFolderStackHeight, images.Length); nbFound++)
            {
                imagesToDraw.Insert(0, images[nbFound]);
            }

	        if (nbFound < UpFolderStackHeight)
            {
	            var imgExt = GetImageExtensions();
                images = imgExt.SelectMany(ext => Directory.GetFiles(
	                folder,
	                ext,
	                SearchOption.AllDirectories
	            )).ToArray();

	            for (int i = 0; i < Math.Min(UpFolderStackHeight - nbFound, images.Length); i++)
                {
                    imagesToDraw.Insert(0, images[rnd.Next(images.Length)]);
                }
            }

	        int drawXOffset = size / 2;
            int drawYOffset = size / 2;
            double angleAmplitude = Math.PI / 10;
            int imageFolderSize = (int)(size / (Math.Cos(angleAmplitude) + Math.Sin(angleAmplitude)));

            foreach (string folderImagePath in imagesToDraw)
            {
                Bitmap folderImage = new Bitmap(folderImagePath);

                int width = folderImage.Width;
                int height = folderImage.Height;
                if (imageFolderSize > 0 && folderImage.Width >= folderImage.Height && folderImage.Width > imageFolderSize)
                {
                    width = imageFolderSize;
                    height = imageFolderSize * folderImage.Height / folderImage.Width;
                }
                else if (imageFolderSize > 0 && folderImage.Height >= folderImage.Width && folderImage.Height > imageFolderSize)
                {
                    width = imageFolderSize * folderImage.Width / folderImage.Height;
                    height = imageFolderSize;
                }

                Pen UpFolderBorderPen = new Pen(new SolidBrush(borderColor), UpFolderBorderWidth);
                UpFolderBorderPen.LineJoin = LineJoin.Round;
                UpFolderBorderPen.StartCap = LineCap.Round;
                UpFolderBorderPen.EndCap = LineCap.Round;

                double angle = (0.5 - rnd.NextDouble()) * angleAmplitude;
                float sin = (float)Math.Sin(angle);
                float cos = (float)Math.Cos(angle);
                float sh = sin * height / 2;
                float ch = cos * height / 2;
                float sw = sin * width / 2;
                float cw = cos * width / 2;
                float shb = sin * (height + UpFolderBorderPen.Width) / 2;
                float chb = cos * (height + UpFolderBorderPen.Width) / 2;
                float swb = sin * (width + UpFolderBorderPen.Width) / 2;
                float cwb = cos * (width + UpFolderBorderPen.Width) / 2;

                g.DrawPolygon(UpFolderBorderPen, new PointF[] {
                new PointF(
                    (float)drawXOffset - cwb - shb,
                    (float)drawYOffset + chb - swb),
                new PointF(
                    (float)drawXOffset - cwb + shb,
                    (float)drawYOffset - swb - chb),
                new PointF(
                    (float)drawXOffset + cwb + shb,
                    (float)drawYOffset + swb - chb),
                new PointF(
                    (float)drawXOffset + cwb - shb,
                    (float)drawYOffset + swb + chb)
            });

                g.DrawImage(folderImage, new PointF[] {
                new PointF(
                    (float)drawXOffset - cw + sh,
                    (float)drawYOffset - sw - ch),
                new PointF(
                    (float)drawXOffset + cw + sh,
                    (float)drawYOffset + sw - ch),
                new PointF(
                    (float)drawXOffset - cw - sh,
                    (float)drawYOffset + ch - sw)
            });
                folderImage.Dispose();
            }

            if (isParentFolder)
            {
                Pen UpArrowPen = new Pen(new SolidBrush(UpArrowColor), UpArrowWidth);
                UpArrowPen.LineJoin = LineJoin.Round;
                UpArrowPen.StartCap = LineCap.Flat;
                UpArrowPen.EndCap = LineCap.Round;

                g.DrawLines(UpArrowPen, new PointF[] {
                new PointF((float)drawXOffset - UpArrowSize * size, (float)drawYOffset + UpArrowSize * size),
                new PointF((float)drawXOffset + UpArrowSize * size, (float)drawYOffset + UpArrowSize * size),
                new PointF((float)drawXOffset + UpArrowSize * size, (float)drawYOffset - UpArrowSize * size),
                new PointF((float)drawXOffset + UpArrowSize * size * 2 / 3, (float)drawYOffset - UpArrowSize * size * 2 / 3),
                new PointF((float)drawXOffset + UpArrowSize * size, (float)drawYOffset - UpArrowSize * size),
                new PointF((float)drawXOffset + UpArrowSize * size * 4 / 3, (float)drawYOffset - UpArrowSize * size * 2 / 3)
            });
            }

            g.Dispose();
            return newImage;
        }

        /// <summary>
        /// Gets the JPEG codec.
        /// </summary>
        /// <returns>The JPEG codec.</returns>
        static ImageCodecInfo GetImageCodec()
        {
            ImageCodecInfo codec = null;
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo e in encoders)
            {
                if (e.MimeType == "image/jpeg")
                {
                    codec = e;
                    break;
                }
            }

            return codec;
        }

        /// <summary>
        /// Returns the encoder parameters.
        /// </summary>
        /// <returns></returns>
        static EncoderParameters GetImageEncoderParams()
        {
            EncoderParameters encoderParams = new EncoderParameters();
            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, ThumbnailJpegQuality);
            return encoderParams;
        }

        /// <summary>
        /// Writes an image on the response.
        /// </summary>
        /// <param name="newImage">The Bitmap to write.</param>
        /// <param name="response">The HttpResponse to write to.</param>
        static void WriteNewImage(Bitmap newImage, HttpResponse response)
        {
            ImageCodecInfo codec = GetImageCodec();
            EncoderParameters encoderParams = GetImageEncoderParams();

            response.ContentType = "image/jpeg";
            newImage.Save(response.OutputStream, codec, encoderParams);

            encoderParams.Dispose();
        }

        /// <summary>
        /// Writes an image to the response.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="response">The HttpResponse to write to.</param>
        static void WriteNewImage(string path, HttpResponse response)
        {
            response.ContentType = "image/jpeg";
            response.TransmitFile(path);
        }

        /// <summary>
        /// Writes a byte array to the response.
        /// </summary>
        /// <param name="imageBytes">The bytes to write</param>
        /// <param name="response">The HttpResponse to write to.</param>
        static void WriteNewImage(byte[] imageBytes, HttpResponse response)
        {
            response.ContentType = "image/jpeg";
            response.OutputStream.Write(imageBytes, 0, imageBytes.Length);
        }

        /// <summary>
        /// Gets an array of bytes from a Bitmap.
        /// </summary>
        /// <param name="image">The Bitmap.</param>
        /// <returns>The byte array.</returns>
        static byte[] GetImageBytes(Bitmap image)
        {
            ImageCodecInfo codec = GetImageCodec();
            EncoderParameters encoderParams = GetImageEncoderParams();

            MemoryStream ms = new MemoryStream();
            image.Save(ms, codec, encoderParams);
            ms.Close();

            encoderParams.Dispose();
            return ms.ToArray();
        }
    }
}
