using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
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

namespace mojoPortal.Web.Controls.FolderGallery
{
    /// <summary>
    /// Describes an Album image.
    /// </summary>
    public sealed class ImageInfo : AlbumPageInfo
    {
        private string _physicalPath;

        private string _name;

        /// <summary>
        /// Constructs an ImageInfo.
        /// </summary>
        /// <param name="owner">The Album that owns this image.</param>
        /// <param name="path">The virtual path of the image.</param>
        /// <param name="physicalPath">The physical path of the image.</param>
        public ImageInfo(Album owner, string path, string physicalPath)
            : base(owner, path)
        {
            _physicalPath = physicalPath;
        }

        protected override char CommandCharacter
        {
            get
            {
                return Album.PageCommand;
            }
        }

        protected override AlbumHandlerMode AlbumMode
        {
            get
            {
                return AlbumHandlerMode.Page;
            }
        }

        /// <summary>
        /// The image caption.
        /// It is the name if available from the ITPC meta-data, or the file name.
        /// </summary>
        public string Caption
        {
            get
            {
                if (_name == null)
                {
                    FileInfo pictureFileInfo = new FileInfo(_physicalPath);
                    Metadata data = GetImageData(pictureFileInfo);
                    IptcDirectory iptcDir = (IptcDirectory)data.GetDirectory(typeof(IptcDirectory));
                    if (iptcDir.ContainsTag(IptcDirectory.TAG_OBJECT_NAME))
                    {
                        _name = iptcDir.GetString(IptcDirectory.TAG_OBJECT_NAME);
                    }
                    else
                    {
                        _name = System.IO.Path.GetFileNameWithoutExtension(_physicalPath);
                    }
                }
                return _name;
            }
        }

        /// <summary>
        /// The date the image was created.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return GetImageDate(new FileInfo(_physicalPath));
            }
        }

        /// <summary>
        /// The URL of the thumbnail for this image.
        /// </summary>
        public string IconUrl
        {
            get
            {
                //return Owner.FilePath +
                //    "?albummode=Thumbnail&albumpath=" +
                //    HttpUtility.UrlEncode(Path);

                return Owner.ServiceUrl +
                    "?albummode=Thumbnail&albumpath=" +
                    HttpUtility.UrlEncode(Path);
            }
        }

        /// <summary>
        /// The metadata for this image.
        /// </summary>
        public Dictionary<string, List<KeyValuePair<string, string>>> MetaData
        {
            get
            {
                Metadata data = GetImageData(new FileInfo(_physicalPath));
                Dictionary<string, List<KeyValuePair<string, string>>> dict =
                    new Dictionary<string, List<KeyValuePair<string, string>>>(data.GetDirectoryCount());
                IEnumerator dirs = data.GetDirectoryIterator();
                while (dirs.MoveNext())
                {
                    MetadataDirectory dir = (MetadataDirectory)dirs.Current;
                    List<KeyValuePair<string, string>> properties =
                        new List<KeyValuePair<string, string>>(dir.GetTagCount());
                    dict.Add(dir.GetName(), properties);
                    IEnumerator tags = dir.GetTagIterator();
                    while (tags.MoveNext())
                    {
                        string name = String.Empty;
                        string description = String.Empty;
                        try
                        {
                            Tag tag = (Tag)tags.Current;
                            name = tag.GetTagName();
                            description = tag.GetDescription();
                        }
                        catch { }
                        if (!String.IsNullOrEmpty(description) &&
                            !String.IsNullOrEmpty(name) &&
                            !name.StartsWith("Unknown ") &&
                            !name.StartsWith("Makernote Unknown ") &&
                            !description.StartsWith("Unknown (\""))
                        {

                            properties.Add(new KeyValuePair<string, string>(name, description));
                        }
                    }
                }
                return dict;
            }
        }

        /// <summary>
        /// The URL of the preview image for this image.
        /// </summary>
        public string PreviewUrl
        {
            get
            {
                //return Owner.FilePath +
                //    "?albummode=Preview&albumpath=" +
                //    HttpUtility.UrlEncode(Path);

                return Owner.ServiceUrl +
                    "?albummode=Preview&albumpath=" +
                    HttpUtility.UrlEncode(Path);
            }
        }

        /// <summary>
        /// The virtual path of the full resolution image.
        /// </summary>
        public string Url
        {
            get
            {
                return Path;
            }
        }

        /// <summary>
        /// Extracts and caches the metadata for an image.
        /// </summary>
        /// <param name="pictureFileInfo">The FileInfo to the image.</param>
        /// <returns>The MetaData for the image.</returns>
        internal static Metadata GetImageData(FileInfo pictureFileInfo)
        {
            try
            {
                string cacheKey = "data(" + pictureFileInfo.FullName + ")";
                Cache cache = HttpContext.Current.Cache;
                object cached = cache[cacheKey];
                if (cached == null)
                {
                    Metadata data = new Metadata();
                    ExifReader exif = new ExifReader(pictureFileInfo);
                    exif.Extract(data);
                    IptcReader iptc = new IptcReader(pictureFileInfo);
                    iptc.Extract(data);
                    JpegReader jpeg = new JpegReader(pictureFileInfo);
                    jpeg.Extract(data);
                    cache.Insert(cacheKey, data, new CacheDependency(pictureFileInfo.FullName));
                    return data;
                }
                return (Metadata)cached;
            }
            catch
            {
                return new Metadata();
            }
        }

        /// <summary>
        /// Gets and caches the most relevant date available for an image.
        /// It looks first at the EXIF date, then at the creation date from ITPC data,
        /// and then at the file's creation date if everything else failed.
        /// </summary>
        /// <param name="pictureFileInfo">The FileInfo for the image.</param>
        /// <returns>The date.</returns>
        internal static DateTime GetImageDate(FileInfo pictureFileInfo)
        {
            string cacheKey = "date(" + pictureFileInfo.FullName + ")";
            Cache cache = HttpContext.Current.Cache;
            DateTime result = DateTime.MinValue;
            object cached = cache[cacheKey];
            if (cached == null)
            {
                Metadata data = GetImageData(pictureFileInfo);
                ExifDirectory directory = (ExifDirectory)data.GetDirectory(typeof(ExifDirectory));
                if (directory.ContainsTag(ExifDirectory.TAG_DATETIME))
                {
                    try
                    {
                        result = directory.GetDate(ExifDirectory.TAG_DATETIME);
                    }
                    catch { }
                }
                else
                {
                    IptcDirectory iptcDir = (IptcDirectory)data.GetDirectory(typeof(IptcDirectory));
                    if (iptcDir.ContainsTag(IptcDirectory.TAG_DATE_CREATED))
                    {
                        try
                        {
                            result = iptcDir.GetDate(IptcDirectory.TAG_DATE_CREATED);
                        }
                        catch { }
                    }
                    else
                    {
                        result = pictureFileInfo.CreationTime;
                    }
                }
                cache.Insert(cacheKey, result, new CacheDependency(pictureFileInfo.FullName));
            }
            else
            {
                result = (DateTime)cached;
            }
            return result;
        }
    }

}
