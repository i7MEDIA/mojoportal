using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
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
using mojoPortal.Web.Controls.FolderGallery;
using System.Linq;

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

namespace mojoPortal.Web.Controls
{
    /// <summary>
    /// The photo album handler.
    /// This class can act as an HttpHandler or as a Control. IHttpHandler,
    /// </summary>
    public sealed class Album :  WebControl, IHttpHandler, IPostBackEventHandler, ICallbackEventHandler
    {

        #region Constants

        private const string AlbumScript = @"
function photoAlbumDetails(id) {
    var details = document.getElementById(id + '_details');
    if (details.style.display && details.style.display == 'none') {
        details.style.display = 'block';
    }
    else {
        details.style.display = 'none';
    }
}
";

        private const string CallbackScript = @"
function photoAlbumCallback(result, context) {
    var album = document.getElementById(context);
    if (album) {
        album.innerHTML = result;
    }            
}
";

        internal const string CallbackFunction = "photoAlbumCallback";

        internal const char FolderCommand = 'f';
        internal const char PageCommand = 'p';

        #endregion

        



        #region Properties

        #region Private Properties

        /// <summary>
        /// The background color of the image thumbnails and previews.
        /// </summary>
        private static readonly Color backGroundColor = Color.Transparent;

        /// <summary>
        /// The color used to draw borders around image thumbnails and previews.
        /// </summary>
        private static readonly Color borderColor = Color.Black;

        private HttpRequest _request;
        private HttpResponse _response;

        private string _requestPathPrefix;
        private string _requestDir;

        private AlbumHandlerMode _mode;
        private bool _isControl;
        private string _filePath;
        private string _rawPath;
        private string _rootPhysicalPath;
        private string _physicalPath;
        private string _title;
        private string _description;

        private ITemplate _folderModeTemplate;
        private ITemplate _pageModeTemplate;

        private ImageInfo _image;
        private AlbumFolderInfo _parentFolder;
        private ImageInfo _previousImage;
        private ImageInfo _nextImage;

        #endregion

        #region UI Properties

        /// <summary>
        /// The tooltip for going back to the folder view from the image preview.
        /// </summary>
        [Localizable(true), DefaultValue("Go back to folder view")]
        public string BackToFolderViewTooltip
        {
            get
            {
                string s = ViewState["BackToFolderViewTooltip"] as string;
                return (s == null) ? "Go back to folder view" : s;
            }
            set
            {
                ViewState["BackToFolderViewTooltip"] = value;
            }
        }

        /// <summary>
        /// The text of the back to parent link.
        /// </summary>
        [Localizable(true), DefaultValue("Up")]
        public string BackToParentText
        {
            get
            {
                string s = ViewState["BackToParentText"] as string;
                return (s == null) ? "Up" : s;
            }
            set
            {
                ViewState["BackToParentText"] = value;
            }
        }

        /// <summary>
        /// Tooltip for going back to the parent folder.
        /// </summary>
        [Localizable(true), DefaultValue("Click to go back to the parent folder")]
        public string BackToParentTooltip
        {
            get
            {
                string s = ViewState["BackToParentTooltip"] as string;
                return (s == null) ? "Click to go back to the parent folder" : s;
            }
            set
            {
                ViewState["BackToParentTooltip"] = value;
            }
        }

        /// <summary>
        /// The top CSS class for the control.
        /// </summary>
        [DefaultValue("album")]
        public override string CssClass
        {
            get
            {
                string s = ViewState["CssClass"] as string;
                return (s == null) ? "album" : s;
            }
            set
            {
                ViewState["CssClass"] = value;
            }
        }

        /// <summary>
        /// The CSS class for the details (meta-data) link in the preview pages.
        /// </summary>
        [DefaultValue("albumDetailsLink")]
        public string DetailsLinkCssClass
        {
            get
            {
                string s = ViewState["DetailsLinkCssClass"] as string;
                return (s == null) ? "albumDetailsLink" : s;
            }
            set
            {
                ViewState["DetailsLinkCssClass"] = value;
            }
        }

        /// <summary>
        /// The text for the details (meta-data) link in the preview pages.
        /// </summary>
        [Localizable(true), DefaultValue("Details")]
        public string DetailsText
        {
            get
            {
                string s = ViewState["DetailsText"] as string;
                return (s == null) ? "Details" : s;
            }
            set
            {
                ViewState["DetailsText"] = value;
            }
        }

        /// <summary>
        /// The tooltip for displaying the preview page.
        /// </summary>
        [Localizable(true), DefaultValue("Click to display")]
        public string DisplayImageTooltip
        {
            get
            {
                string s = ViewState["DisplayImageTooltip"] as string;
                return (s == null) ? "Click to display" : s;
            }
            set
            {
                ViewState["DisplayImageTooltip"] = value;
            }
        }

        /// <summary>
        /// Tooltip for seeing the image at full resolution.
        /// </summary>
        [Localizable(true), DefaultValue("Click to view picture at full resolution")]
        public string DisplayFullResolutionTooltip
        {
            get
            {
                string s = ViewState["DisplayFullResolutionTooltip"] as string;
                return (s == null) ? "Click to view picture at full resolution" : s;
            }
            set
            {
                ViewState["DisplayFullResolutionTooltip"] = value;
            }
        }

        /// <summary>
        /// The template for the folder mode.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(AlbumTemplateContainer))]
        public ITemplate FolderModeTemplate
        {
            get
            {
                return _folderModeTemplate;
            }
            set
            {
                _folderModeTemplate = value;
            }
        }

        /// <summary>
        /// The CSS class for a thumbnail.
        /// </summary>
        [DefaultValue("albumFloat")]
        public string ImageDivCssClass
        {
            get
            {
                string s = ViewState["ImageDivCssClass"] as string;
                return (s == null) ? "albumFloat" : s;
            }
            set
            {
                ViewState["ImageDivCssClass"] = value;
            }
        }

        /// <summary>
        /// CSS class for metadata field names.
        /// </summary>
        [DefaultValue("albumMetaName")]
        public string MetaNameCssClass
        {
            get
            {
                string s = ViewState["MetaNameCssClass"] as string;
                return (s == null) ? "albumMetaName" : s;
            }
            set
            {
                ViewState["MetaNameCssClass"] = value;
            }
        }

        /// <summary>
        /// CSS class for metadata section heads.
        /// </summary>
        [DefaultValue("albumMetaSectionHead")]
        public string MetaSectionHeadCssClass
        {
            get
            {
                string s = ViewState["MetaSectionHeadCssClass"] as string;
                return (s == null) ? "albumMetaSectionHead" : s;
            }
            set
            {
                ViewState["MetaSectionHeadCssClass"] = value;
            }
        }

        /// <summary>
        /// CSS class for metadata values.
        /// </summary>
        [DefaultValue("albumMetaValue")]
        public string MetaValueCssClass
        {
            get
            {
                string s = ViewState["MetaValueCssClass"] as string;
                return (s == null) ? "albumMetaValue" : s;
            }
            set
            {
                ViewState["MetaValueCssClass"] = value;
            }
        }


        /// <summary>
        /// Tooltip for the link to the next image.
        /// </summary>
        [Localizable(true), DefaultValue("Click to view the next picture")]
        public string NextImageTooltip
        {
            get
            {
                string s = ViewState["NextImageTooltip"] as string;
                return (s == null) ? "Click to view the next picture" : s;
            }
            set
            {
                ViewState["NextImageTooltip"] = value;
            }
        }

        /// <summary>
        /// Format string for the open folder tooltip.
        /// </summary>
        [Localizable(true), DefaultValue(@"Click to open ""{0}""")]
        public string OpenFolderTooltipFormatString
        {
            get
            {
                string s = ViewState["OpenFolderTooltipFormatString"] as string;
                return (s == null) ? @"Click to open ""{0}""" : s;
            }
            set
            {
                ViewState["OpenFolderTooltipFormatString"] = value;
            }
        }

        /// <summary>
        /// Template for the control in image preview mode.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(AlbumTemplateContainer))]
        public ITemplate PageModeTemplate
        {
            get
            {
                return _pageModeTemplate;
            }
            set
            {
                _pageModeTemplate = value;
            }
        }

        /// <summary>
        /// A permanent link to the current page with the Album in its current state.
        /// </summary>
        public string PermaLink
        {
            get
            {
                return ResolveClientUrl("~/thumbnailservice.ashx") +
                        "?albummode=" + _mode.ToString() +
                        "&albumpath=" + HttpUtility.UrlEncode(_rawPath);
            }
        }

        /// <summary>
        /// The tooltip for the link to the previous image.
        /// </summary>
        [Localizable(true), DefaultValue("Click to view the previous picture")]
        public string PreviousImageTooltip
        {
            get
            {
                string s = ViewState["PreviousImageTooltip"] as string;
                return (s == null) ? "Click to view the previous picture" : s;
            }
            set
            {
                ViewState["PreviousImageTooltip"] = value;
            }
        }



        #endregion

        /// <summary>
        /// The path of the current file.
        /// </summary>
        internal string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        

        ///// <summary>
        ///// The URL of the handler.
        ///// </summary>
        //[DefaultValue("~/album.ashx"), UrlProperty]
        //public string HandlerUrl
        //{
        //    get
        //    {
        //        string s = ViewState["HandlerUrl"] as string;
        //        return (s == null) ? "~/album.ashx" : s;
        //    }
        //    set
        //    {
        //        ViewState["HandlerUrl"] = value;
        //    }
        //}

        /// <summary>
        /// The URL of the handler.
        /// </summary>
        [DefaultValue("~/thumbnailservice.ashx"), UrlProperty]
        public string ServiceUrl
        {
            get
            {
                string s = ViewState["ServiceUrl"] as string;
                return (s == null) ? "~/thumbnailservice.ashx" : s;
            }
            set
            {
                ViewState["ServiceUrl"] = value;
            }
        }

        /// <summary>
        /// The info for the current image.
        /// </summary>
        public ImageInfo Image
        {
            get
            {
                if (_mode == AlbumHandlerMode.Page)
                {
                    if (_image == null)
                    {
                        _image = new ImageInfo(this, Path, _physicalPath);
                    }
                    return _image;
                }
                return null;
            }
        }

        

        /// <summary>
        /// The list of image infos for the current folder.
        /// </summary>
        public List<ImageInfo> Images
        {
            get
            {
                if (_mode == AlbumHandlerMode.Folder)
                {
                    FileInfo[] pics = GetImages(_physicalPath);
                    List<ImageInfo> images = null;
                    if (pics != null && pics.Length > 0)
                    {
                        string dirPrefix = Path;
                        if (!dirPrefix.EndsWith("/"))
                        {
                            dirPrefix += "/";
                        }
                        images = new List<ImageInfo>(pics.Length);
                        foreach (FileInfo f in pics)
                        {
                            string picName = f.Name;
                            images.Add(new ImageInfo(this, dirPrefix + picName, f.FullName));
                        }
                    }
                    return images;
                }
                return null;
            }
        }

        /// <summary>
        /// True if the class is used in Control mode (as opposed to handler mode).
        /// </summary>
        internal bool IsControl
        {
            get
            {
                return _isControl;
            }
        }

        

        /// <summary>
        /// Defines how the Album navigation links work.
        /// </summary>
        [DefaultValue(AlbumNavigationMode.Callback)]
        public AlbumNavigationMode NavigationMode
        {
            get
            {
                object o = ViewState["NavigationMode"];
                return (o == null) ? AlbumNavigationMode.Callback : (AlbumNavigationMode)o;
            }
            set
            {
                ViewState["NavigationMode"] = value;
            }
        }

        /// <summary>
        /// Info for the next image.
        /// </summary>
        public ImageInfo NextImage
        {
            get
            {
                if (_mode == AlbumHandlerMode.Page)
                {
                    if (_nextImage == null)
                    {
                        EnsureNextPrevious();
                    }
                    return _nextImage;
                }
                return null;
            }
        }

        
        /// <summary>
        /// Information for the parent folder.
        /// </summary>
        public AlbumFolderInfo ParentFolder
        {
            get
            {
                if (
                    (_parentFolder == null)
                    &&(Path != GalleryRootPath)
                    )
                {
                    if (Path != "/")
                    {
                        int i = Path.LastIndexOf('/');
                        string parentDirPath;

                        if (i == 0)
                        {
                            parentDirPath = "/";
                        }
                        else
                        {
                            parentDirPath = Path.Substring(0, i);
                        }

                        if (parentDirPath == _requestDir || parentDirPath.StartsWith(_requestPathPrefix))
                        {
                            _parentFolder = new AlbumFolderInfo(this, parentDirPath, true);
                        }
                    }
                }
                return _parentFolder;
            }
        }

        /// <summary>
        /// The current virtual path.
        /// </summary>
        [DefaultValue("")]
        public string GalleryRootPath
        {
            get
            {
                string s = ViewState["GalleryRootPath"] as string;
                return (s == null) ? String.Empty : s;
            }
            set
            {
                _rawPath = value;
                string path = null;
                if (value != null)
                {
                    // JA 2008-03023 don't lower the path as paths are case sensitive on *nix
                    //path = value.ToLower().Replace("\\", "/").Trim();
                    path = value.Replace("\\", "/").Trim();

                    if (path != "/" && path.EndsWith("/"))
                    {
                        path = path.Substring(0, path.Length - 1);
                    }

                    //if (path != _requestDir && !path.StartsWith(_requestPathPrefix)) {
                    //    ReportError(path + " invalid path - not in the handler scope");
                    //}

                    if (path.IndexOf("/.") >= 0)
                    {
                        ReportError("invalid path");
                    }
                }
                else
                {
                    path = _requestDir;
                    _rawPath = _requestDir;
                }
                ViewState["GalleryRootPath"] = path;


                //_physicalPath = _request.MapPath(path, "/", false);
                if (HttpContext.Current != null)
                {
                    _rootPhysicalPath = HttpContext.Current.Server.MapPath(path);

                }
            }
        }

        /// <summary>
        /// The current virtual path.
        /// </summary>
        [DefaultValue("")]
        public string Path
        {
            get
            {
                string s = ViewState["Path"] as string;
                return (s == null) ? String.Empty : s;
            }
            set
            {
                _rawPath = value;
                string path = null;
                if (value != null)
                {
                    //path = value.ToLower().Replace("\\", "/").Trim();
                    path = value.Replace("\\", "/").Trim();

                    if (path != "/" && path.EndsWith("/"))
                    {
                        path = path.Substring(0, path.Length - 1);
                    }

                    //if (path != _requestDir && !path.StartsWith(_requestPathPrefix)) {
                    //    ReportError(path + " invalid path - not in the handler scope");
                    //}

                    if (path.IndexOf("/.") >= 0)
                    {
                        ReportError("invalid path");
                    }
                }
                else
                {
                    path = _requestDir;
                    _rawPath = _requestDir;
                }
                ViewState["Path"] = path;


                //_physicalPath = _request.MapPath(path, "/", false);
                if (HttpContext.Current != null)
                {
                    _physicalPath = HttpContext.Current.Server.MapPath(path);

                }
            }
        }

        


        /// <summary>
        /// The fixed part of the path.
        /// </summary>
        internal string PathPrefix
        {
            get
            {
                return _requestPathPrefix;
            }
        }

        

        /// <summary>
        /// The information for the previous image.
        /// </summary>
        public ImageInfo PreviousImage
        {
            get
            {
                if (_mode == AlbumHandlerMode.Page)
                {
                    if (_previousImage == null)
                    {
                        EnsureNextPrevious();
                    }
                    return _previousImage;
                }
                return null;
            }
        }

        
        /// <summary>
        /// The subfolders of the current folder.
        /// </summary>
        public List<AlbumFolderInfo> SubFolders
        {
            get
            {
                if (_mode == AlbumHandlerMode.Folder)
                {
                    string[] dirs = Directory.GetDirectories(_physicalPath);
                    List<AlbumFolderInfo> subFolders = null;
                    if (dirs != null && dirs.Length > 0)
                    {
                        subFolders = new List<AlbumFolderInfo>(dirs.Length);
                        string dirPrefix = Path;
                        if (!dirPrefix.EndsWith("/"))
                        {
                            dirPrefix += "/";
                        }

                        foreach (string d in dirs)
                        {
                            DirectoryInfo dirInfo = new DirectoryInfo(d);
                            //string dirName = (new FileInfo(d)).Name;
                            string dirName = dirInfo.Name;
                            
                            string dir = dirName.ToLower();
                            if (dir.StartsWith("_vti_") ||
                                dir.StartsWith("app_") ||
                                (dir == "bin") ||
                                (dir == ".svn") ||
                                (dir == "systemfiles") ||
                                (dir == "index") ||
                                (dir == "webstoreproductfiles") ||
                                (dir == "sharedfiles") ||
                                (dir == "style") ||
                                (dir == "htmlfragments") ||
                                (dir == "avatars") ||
                                (dir == "emoticons") ||
                                (dir == "flash") ||
                                (dir == "mathml") ||
                                (dir == "xml") ||
                                (dir == "xsl") ||
                                (dir == "logos") ||
                                (dir == "skins") ||
                                (dir == "banners") ||
                                (dir == "aspnet_client")
                                )
                            {
                                continue;
                            }
                            subFolders.Add(new AlbumFolderInfo(this, dirPrefix + dirName));
                        }
                    }
                    return subFolders;
                }
                return null;
            }
        }

        

        /// <summary>
        /// The title for the current Album view.
        /// </summary>
        public string Title
        {
            get
            {
                if (_title == null)
                {
                    if (_mode == AlbumHandlerMode.Folder)
                    {
                        _title = _rawPath.Substring(_rawPath.LastIndexOf('/') + 1);
                    }
                    else
                    {
                        FileInfo pictureFileInfo = new FileInfo(_physicalPath);
                        Metadata data = ImageInfo.GetImageData(pictureFileInfo);

                        // First, check for the IPTC Title tag
                        IptcDirectory iptcDir = (IptcDirectory)data.GetDirectory(typeof(IptcDirectory));
                        if (iptcDir.ContainsTag(IptcDirectory.TAG_OBJECT_NAME))
                        {
                            _title = iptcDir.GetString(IptcDirectory.TAG_OBJECT_NAME);
                        }
                        else
                        {
                            // Then, try the Exif Title tag used by XP (in Property / Summary dialog)
                            ExifDirectory exifDir = (ExifDirectory)data.GetDirectory(typeof(ExifDirectory));
                            if (exifDir.ContainsTag(ExifDirectory.TAG_XP_TITLE))
                            {
                                _title = exifDir.GetDescription(ExifDirectory.TAG_XP_TITLE);
                            }
                            else
                            {
                                // Default the title to teh file name
                                _title = System.IO.Path.GetFileNameWithoutExtension(_physicalPath);
                            }
                        }
                    }
                }
                return _title;
            }
        }

        /// <summary>
        /// The description for the current Album view.
        /// </summary>
        public string Description
        {
            get
            {
                if (_description == null)
                {
                    _description = String.Empty;
                    if (_mode == AlbumHandlerMode.Page)
                    {
                        FileInfo pictureFileInfo = new FileInfo(_physicalPath);
                        Metadata data = ImageInfo.GetImageData(pictureFileInfo);

                        // Try the Exif Description tag used by XP (in Property / Summary dialog)
                        ExifDirectory exifDir = (ExifDirectory)data.GetDirectory(typeof(ExifDirectory));
                        if (exifDir.ContainsTag(ExifDirectory.TAG_XP_COMMENTS))
                        {
                            _description = exifDir.GetDescription(ExifDirectory.TAG_XP_COMMENTS);
                        }
                    }
                }

                return _description;
            }
        }

        #endregion

        protected override void CreateChildControls()
        {
            if ((FolderModeTemplate != null) && (_mode == AlbumHandlerMode.Folder))
            {
                Controls.Clear();
                _parentFolder = null;
                AlbumTemplateContainer container = new AlbumTemplateContainer(this);
                container.EnableViewState = false;
                FolderModeTemplate.InstantiateIn(container);
                Controls.Add(container);
            }
            if ((PageModeTemplate != null) && (_mode == AlbumHandlerMode.Page))
            {
                Controls.Clear();
                _image = null;
                AlbumTemplateContainer container = new AlbumTemplateContainer(this);
                container.EnableViewState = false;
                PageModeTemplate.InstantiateIn(container);
                Controls.Add(container);
            }
        }

        /// <summary>
        /// added by  2008-05-02
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns></returns>
        public static int CompareFileNames(FileInfo f1, FileInfo f2)
        {
            return f1.FullName.CompareTo(f2.FullName);
        }

        /// <summary>
        /// Ensures that the next and previous image infos have been computed.
        /// </summary>
        private void EnsureNextPrevious()
        {
            bool pictureFound = false;
            string dirPath = System.IO.Path.GetDirectoryName(_physicalPath);
            FileInfo[] pics = GetImages(dirPath);
            Array.Sort(pics, CompareFileNames);

            string prev = null;
            string next = null;
            string prevPhysPath = null;
            string nextPhysPath = null;

            if (ParentFolder == null) return;
            if (ParentFolder.Path == null) return;

            string parentPath = ParentFolder.Path;

            if (pics != null && pics.Length > 0)
            {
                foreach (FileInfo p in pics)
                {
                    // don't lower - JA 2008-03-23
                    //string picture = p.Name.ToLower();
                    string picture = p.Name;

                    if (String.Equals(p.FullName, _physicalPath, StringComparison.InvariantCultureIgnoreCase))
                    {
                        pictureFound = true;
                    }
                    else if (pictureFound)
                    {
                        nextPhysPath = p.FullName;
                        next = parentPath + '/' + picture;
                        break;
                    }
                    else
                    {
                        prevPhysPath = p.FullName;
                        prev = parentPath + '/' + picture;
                    }
                }
            }

            if (!pictureFound)
            {
                prevPhysPath = null;
                nextPhysPath = null;
            }
            if (prev != null)
            {
                _previousImage = new ImageInfo(this, prev, prevPhysPath);
            }
            if (next != null)
            {
                _nextImage = new ImageInfo(this, next, nextPhysPath);
            }
        }

        #region IHttpHandler 

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            _request = context.Request;
            _response = context.Response;

            RenderPrivate(new HtmlTextWriter(_response.Output));
        }

        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }


        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _isControl = true;
            //_filePath = Page.ResolveUrl(HandlerUrl);
           

            if(GalleryRootPath.Length > 0)
            DoSetup();
            
            _request = Page.Request;
            _response = Page.Response;

            ParseParams();
            ChildControlsCreated = false;
        }

        public void DoSetup()
        {
            if (GalleryRootPath.Length == 0) return;

            _filePath = Page.ResolveUrl(GalleryRootPath);
            _requestDir = GalleryRootPath;

            Path = GalleryRootPath;


            //_requestPathPrefix = _filePath.Substring(0, _filePath.LastIndexOf('/') + 1).ToLower();

            _requestPathPrefix = string.Empty;

            //_requestDir = (_requestPathPrefix == "/") ?
            //    "/" :
            //    _requestPathPrefix.Substring(0, _requestPathPrefix.Length - 1);


        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Album), "AlbumScript", AlbumScript, true);
            if (Page.Request.Browser.SupportsCallback)
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Album), "CallbackScript", CallbackScript, true);
            }
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            if (!Visible) { return; }
            if (((FolderModeTemplate != null) && (_mode == AlbumHandlerMode.Folder)) ||
                ((PageModeTemplate != null) && (_mode == AlbumHandlerMode.Page)))
            {
                Controls[0].DataBind();
            }
            RenderPrivate(writer);
        }

        

        /// <summary>
        /// Directs to the right rendering methos according to the mode.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        private void RenderPrivate(HtmlTextWriter writer)
        {
            if (!Visible) { return; }
            if (!_isControl)
            {
                
                _filePath = _request.FilePath;

                // JA 2008-03-23 don't lower
                //_requestPathPrefix = _filePath.Substring(0, _filePath.LastIndexOf('/') + 1).ToLower();
                _requestPathPrefix = _filePath.Substring(0, _filePath.LastIndexOf('/') + 1);

                _requestDir = (_requestPathPrefix == "/") ?
                    "/" :
                    _requestPathPrefix.Substring(0, _requestPathPrefix.Length - 1);

                ParseParams();

                if (ConfigurationManager.AppSettings["FolderGalleryRequiredPath"] != null)
                {
                    string requiredPathFragment = ConfigurationManager.AppSettings["FolderGalleryRequiredPath"];
                    if (!Path.Contains(requiredPathFragment))
                    {
                        //ReportError("invalid path");
                        this._response.Redirect("~/AccessDenied.aspx");
                        return;

                    }


                }
            }

            if (_mode == AlbumHandlerMode.Unknown) { return; }

            if ((_mode == AlbumHandlerMode.Folder) ||
                (_mode == AlbumHandlerMode.FolderThumbnail) ||
                (_mode == AlbumHandlerMode.ParentThumbnail))
            {

                if (!Directory.Exists(_physicalPath))
                {
                    throw new HttpException(404, "Directory Not Found");
                }
            }
            else if ((_mode != AlbumHandlerMode.Css) && !File.Exists(_physicalPath))
            {
                throw new HttpException(404, "File Not Found");
            }

            Color backColorToUse = backGroundColor;
            Color borderColorToUse = borderColor;

            if (ConfigurationManager.AppSettings["FolderGalleryImageBackColor"] != null)
            {
                try
                {
                    backColorToUse = Color.FromName(ConfigurationManager.AppSettings["FolderGalleryImageBackColor"]);
                }
                catch { }
            }

            if (ConfigurationManager.AppSettings["FolderGalleryImageBorderColor"] != null)
            {
                try
                {
                    borderColorToUse = Color.FromName(ConfigurationManager.AppSettings["FolderGalleryImageBorderColor"]);
                }
                catch { }
            }

            switch (_mode)
            {
                case AlbumHandlerMode.Folder:
                    GenerateFolderPage(writer, Path, _physicalPath);
                    break;

                case AlbumHandlerMode.Page:
                    string dir = Path.Substring(0, Path.LastIndexOf('/') + 1);

                    if (dir != "/")
                    {
                        dir = dir.Substring(0, dir.Length - 1);
                    }

                    // JA 2008-03-23 don't lower
                    //GeneratePreviewPage(
                    //    writer,
                    //    dir,
                    //    _request.MapPath(dir),
                    //    Path.Substring(Path.LastIndexOf('/') + 1).ToLower());

                    GeneratePreviewPage(
                        writer,
                        dir,
                        _request.MapPath(dir),
                        Path.Substring(Path.LastIndexOf('/') + 1));

                    break;

                case AlbumHandlerMode.Preview:
                    ImageHelper.GenerateResizedImageResponse(
                        backColorToUse,
                        Color.Transparent,
                        _physicalPath, 
                        ImageHelper.PreviewSize(), 
                        false, 
                        _response);
                    break;

                case AlbumHandlerMode.Thumbnail:
                    ImageHelper.GenerateResizedImageResponse(
                        backColorToUse,
                        borderColorToUse, 
                        _physicalPath, 
                        ImageHelper.ThumbnailSize, 
                        true, _response);
                    break;

                case AlbumHandlerMode.FolderThumbnail:
                    ImageHelper.GenerateFolderImageResponse(
                        backColorToUse,
                        borderColorToUse, 
                        false, 
                        _physicalPath, 
                        ImageHelper.ThumbnailSize, 
                        _response);
                    break;

                case AlbumHandlerMode.ParentThumbnail:
                    ImageHelper.GenerateFolderImageResponse(
                        backColorToUse,
                        borderColorToUse, 
                        true, 
                        _physicalPath, 
                        ImageHelper.ThumbnailSize, 
                        _response);
                    break;

                case AlbumHandlerMode.Css:
                    ImageHelper.GenerateCssResponse(_response);
                    break;

                default:
                    ReportError("invalid mode");
                    break;
            }
        }

        

        /// <summary>
        /// Does the actual rendering for the folder mode.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="dirPath">The virtual path to the folder.</param>
        /// <param name="dirPhysicalPath">The physical for the folder.</param>
        void GenerateFolderPage(HtmlTextWriter writer, string dirPath, string dirPhysicalPath)
        {
            string dirPrefix = dirPath;
            if (!dirPrefix.EndsWith("/"))
            {
                dirPrefix += "/";
            }

            if (_isControl)
            {
                if (!Page.IsCallback)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                }
            }
            else
            {
                writer.Write(@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.1//EN"" ""http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"">");
                writer.AddAttribute("xmlns", "http://www.w3.org/1999/xhtml");
                writer.RenderBeginTag(HtmlTextWriterTag.Html);
                writer.RenderBeginTag(HtmlTextWriterTag.Head);
                writer.AddAttribute(HtmlTextWriterAttribute.Rel, "Stylesheet");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "?albummode=css");
                writer.RenderBeginTag(HtmlTextWriterTag.Link);
                writer.RenderEndTag(); // link
                writer.RenderBeginTag(HtmlTextWriterTag.Title);
                writer.WriteEncodedText(Title);
                writer.RenderEndTag(); // title
                writer.RenderEndTag(); // head
                writer.RenderBeginTag(HtmlTextWriterTag.Body);
            }

            if (FolderModeTemplate != null)
            {
                Controls[0].RenderControl(writer);
            }
            else
            {

                if (dirPath != "/")
                {
                    int i = dirPath.LastIndexOf('/');
                    string parentDirPath;

                    if (i == 0)
                    {
                        parentDirPath = "/";
                    }
                    else
                    {
                        parentDirPath = dirPath.Substring(0, i);
                    }

                    if (parentDirPath == _requestDir || parentDirPath.StartsWith(_requestPathPrefix))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, ImageDivCssClass);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, ParentFolder.Link, true);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, ParentFolder.IconUrl, true);
                        writer.AddAttribute(HtmlTextWriterAttribute.Alt, BackToParentTooltip, true);
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag(); // img
                        writer.WriteBreak();
                        writer.Write(BackToParentText);
                        writer.RenderEndTag(); // a
                        writer.RenderEndTag(); // div
                    }
                }

                List<AlbumFolderInfo> folders = SubFolders;
                if (folders != null && folders.Count > 0)
                {
                    foreach (AlbumFolderInfo folder in folders)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, ImageDivCssClass);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, folder.Link, true);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, folder.IconUrl, true);
                        writer.AddAttribute(HtmlTextWriterAttribute.Alt,
                            String.Format(OpenFolderTooltipFormatString, folder.Name), true);
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag(); // img
                        writer.WriteBreak();
                        writer.Write(folder.Name);
                        writer.RenderEndTag(); // a
                        writer.RenderEndTag(); // div
                    }
                }

                List<ImageInfo> images = Images;
                if (images != null && images.Count > 0)
                {
                    foreach (ImageInfo image in images)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, ImageDivCssClass);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, image.Link, true);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, image.IconUrl, true);
                        writer.AddAttribute(HtmlTextWriterAttribute.Alt, DisplayImageTooltip, true);
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag(); // img
                        writer.RenderEndTag(); // a
                        writer.WriteBreak();
                        writer.Write("&nbsp;");
                        writer.RenderEndTag(); // div
                    }
                }
            }

            if (_isControl)
            {
                if (!Page.IsCallback)
                {
                    writer.RenderEndTag(); // div
                }
            }
            else
            {
                writer.RenderEndTag(); // body
                writer.RenderEndTag(); // html
            }
        }

        /// <summary>
        /// Gets an array of file infos for the images in a folder, sorted by date.
        /// </summary>
        /// <param name="path">The folder's physical path.</param>
        /// <returns>The list of file infos, sorted by date.</returns>
        private static FileInfo[] GetImages(string path)
        {
            return GetImages(path, false);
        }

        /// <summary>
        /// Gets an array of file infos for the images in a folder, sorted by date.
        /// </summary>
        /// <param name="path">The folder's physical path.</param>
        /// <param name="includeSubfolders">True if subfolders should be included.</param>
        /// <returns>The list of file infos, sorted by date.</returns>
        private static FileInfo[] GetImages(string path, bool includeSubfolders)
        {
	        var di = new DirectoryInfo(path);
			//var pics = di.GetFiles("*.jpg",
			//	includeSubfolders
			//		? SearchOption.AllDirectories
			//		: SearchOption.TopDirectoryOnly
			//);

	        var imgExt = ImageHelper.GetImageExtensions();
			var pics = imgExt.SelectMany(ext => di.GetFiles(ext,
		        includeSubfolders
			        ? SearchOption.AllDirectories
			        : SearchOption.TopDirectoryOnly
	        )).ToArray();

	        Array.Sort(pics, CompareFileNames);

	        return pics;
        }

        /// <summary>
        /// Gets the top n images in a folder, sorted by date descending.
        /// </summary>
        /// <param name="numberOfImages">Maximum number of images to return.</param>
        /// <param name="includeSubFolders">True if subfolders should be included.</param>
        /// <returns>The image infos.</returns>
        public List<ImageInfo> GetImages(int numberOfImages, bool includeSubFolders)
        {
            if (_mode == AlbumHandlerMode.Folder)
            {
                FileInfo[] pics = GetImages(_physicalPath, includeSubFolders);
                List<ImageInfo> images = null;
                if (pics != null && pics.Length > 0)
                {
                    string dirPrefix = Path;
                    if (!dirPrefix.EndsWith("/"))
                    {
                        dirPrefix += "/";
                    }
                    images = new List<ImageInfo>(numberOfImages);
                    int n = 1;
                    foreach (FileInfo f in pics)
                    {
                        string picName = f.Name;
                        images.Add(new ImageInfo(this, dirPrefix + picName, f.FullName));
                        if (n++ >= numberOfImages)
                        {
                            break;
                        }
                    }
                }
                return images;
            }
            return null;
        }

        /// <summary>
        /// Renders an image preview page.
        /// </summary>
        /// <param name="writer">The writer to render to.</param>
        /// <param name="dirPath">The virtual path of the directory.</param>
        /// <param name="dirPhysicalPath">The physical path of the directory.</param>
        /// <param name="page">The name of the image.</param>
        void GeneratePreviewPage(HtmlTextWriter writer, string dirPath, string dirPhysicalPath, string page)
        {
            string dirPrefix = dirPath;
            if (!dirPrefix.EndsWith("/"))
            {
                dirPrefix += "/";
            }

            string pictPath = dirPrefix + page;

            if (_isControl)
            {
                if (!Page.IsCallback)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                }
            }
            else
            {
                writer.Write(@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.1//EN"" ""http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"">");
                writer.AddAttribute("xmlns", "http://www.w3.org/1999/xhtml");
                writer.RenderBeginTag(HtmlTextWriterTag.Html);
                writer.RenderBeginTag(HtmlTextWriterTag.Head);
                writer.AddAttribute(HtmlTextWriterAttribute.Rel, "Stylesheet");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "?albummode=css");
                writer.RenderBeginTag(HtmlTextWriterTag.Link);
                writer.RenderEndTag(); // link
                writer.RenderBeginTag(HtmlTextWriterTag.Title);
                writer.WriteEncodedText(Title);
                writer.RenderEndTag(); // title
                writer.RenderBeginTag(HtmlTextWriterTag.Script);
                writer.Write(AlbumScript);
                writer.RenderEndTag(); // script
                writer.RenderEndTag(); // head
                writer.RenderBeginTag(HtmlTextWriterTag.Body);
            }

            if (PageModeTemplate != null)
            {
                Controls[0].RenderControl(writer);
            }
            else
            {
                EnsureNextPrevious();

                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                if (ParentFolder != null)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, ParentFolder.Link, true);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, ParentFolder.IconUrl, true);
                    writer.AddAttribute(HtmlTextWriterAttribute.Alt, BackToFolderViewTooltip, true);
                    writer.RenderBeginTag(HtmlTextWriterTag.Img);
                    writer.RenderEndTag(); // img
                    writer.RenderEndTag(); // a
                }

                if (PreviousImage != null)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, PreviousImage.Link, true);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, PreviousImage.IconUrl, true);
                    writer.AddAttribute(HtmlTextWriterAttribute.Alt, PreviousImageTooltip, true);
                    writer.RenderBeginTag(HtmlTextWriterTag.Img);
                    writer.RenderEndTag(); // img
                    writer.RenderEndTag(); // a
                }
                else
                {
                    writer.Write("&nbsp;");
                }
                writer.WriteBreak();

                writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick,
                    @"photoAlbumDetails(""" +
                    (_isControl ? ClientID : String.Empty) +
                    @""")", true);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, DetailsLinkCssClass);
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(DetailsText);
                writer.RenderEndTag(); // a

                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Id,
                    (_isControl ? ClientID : String.Empty) + "_details", true);
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                writer.RenderBeginTag(HtmlTextWriterTag.Table);

                Dictionary<string, List<KeyValuePair<string, string>>> metadata = Image.MetaData;
                foreach (KeyValuePair<string, List<KeyValuePair<string, string>>> dir in metadata)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, MetaSectionHeadCssClass);
                    writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteEncodedText(dir.Key);
                    writer.RenderEndTag(); // td
                    writer.RenderEndTag(); // tr

                    foreach (KeyValuePair<string, string> data in dir.Value)
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                        writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, MetaNameCssClass);
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.WriteEncodedText(data.Key);
                        writer.RenderEndTag(); // td
                        writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, MetaValueCssClass);
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.WriteEncodedText(data.Value);
                        writer.RenderEndTag(); // td
                        writer.RenderEndTag(); // tr
                    }
                }

                writer.RenderEndTag(); // table
                writer.RenderEndTag(); // td

                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                //writer.AddAttribute(HtmlTextWriterAttribute.Href, pictPath, true);

                writer.AddAttribute(HtmlTextWriterAttribute.Href, mojoPortal.Web.Framework.WebUtils.ResolveUrl(pictPath), true);

                writer.AddAttribute(HtmlTextWriterAttribute.Target, "_blank");
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.AddAttribute(HtmlTextWriterAttribute.Src, Image.PreviewUrl, true);
                writer.AddAttribute(HtmlTextWriterAttribute.Alt, DisplayFullResolutionTooltip, true);
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag(); // img
                writer.RenderEndTag(); // a

                writer.RenderEndTag(); // td

                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);

                if (NextImage != null)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, NextImage.Link, true);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, NextImage.IconUrl, true);
                    writer.AddAttribute(HtmlTextWriterAttribute.Alt, NextImageTooltip, true);
                    writer.RenderBeginTag(HtmlTextWriterTag.Img);
                    writer.RenderEndTag(); // img
                    writer.RenderEndTag(); // a
                }
                else
                {
                    writer.Write("&nbsp;");
                }

                writer.RenderEndTag(); // td
                writer.RenderEndTag(); // tr
                writer.RenderEndTag(); // table
            }

            if (_isControl)
            {
                if (!Page.IsCallback)
                {
                    writer.RenderEndTag(); // div
                }
            }
            else
            {
                writer.RenderEndTag(); // body
                writer.RenderEndTag(); // html
            }
        }

        /// <summary>
        /// Sends a 500 error to the client.
        /// </summary>
        /// <param name="msg">The error message.</param>
        void ReportError(string msg)
        {
            throw new HttpException(500, msg);
        }

        /// <summary>
        /// Parses the parameters from the querystring.
        /// </summary>
        void ParseParams()
        {
            ParseParams(_request.QueryString);
        }

        /// <summary>
        /// Parses the parameters.
        /// </summary>
        /// <param name="paramsCollection">The parameter collection to parse from.</param>
        void ParseParams(NameValueCollection paramsCollection)
        {
            string s;

            s = paramsCollection["albummode"];

            if (s != null)
            {
                try
                {
                    _mode = (AlbumHandlerMode)Enum.Parse(typeof(AlbumHandlerMode), s, true);
                }
                catch
                {
                }

                if (_mode == AlbumHandlerMode.Unknown)
                {
                    ReportError("invalid mode");
                }
            }
            else
            {
                _mode = AlbumHandlerMode.Folder;
            }

            s = paramsCollection["albumpath"];

            Path = s;
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            Page.ClientScript.ValidateEvent(UniqueID, eventArgument);
            char command = eventArgument[0];
            string arg = eventArgument.Substring(1);
            switch (command)
            {
                case FolderCommand:
                    _mode = AlbumHandlerMode.Folder;
                    Path = arg;
                    break;
                case PageCommand:
                    _mode = AlbumHandlerMode.Page;
                    Path = arg;
                    break;
            }
        }

        private string _callbackEventArg;

        string ICallbackEventHandler.GetCallbackResult()
        {
            ((IPostBackEventHandler)this).RaisePostBackEvent(_callbackEventArg);
            ChildControlsCreated = false;
            using (StringWriter swriter = new StringWriter())
            {
                using (HtmlTextWriter writer = new HtmlTextWriter(swriter))
                {
                    EnsureChildControls();
                    RenderControl(writer);
                }
                return swriter.ToString();
            }
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            _callbackEventArg = eventArgument;
        }
    }

}
