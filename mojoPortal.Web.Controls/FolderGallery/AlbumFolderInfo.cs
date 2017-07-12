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
    /// Describes an album folder.
    /// </summary>
    public sealed class AlbumFolderInfo : AlbumPageInfo
    {
        private bool _isParent;

        private string _name;

        /// <summary>
        /// Constructs an AlbumFolderInfo.
        /// </summary>
        /// <param name="owner">The Album that owns this info.</param>
        /// <param name="path">The virtual path of the folder.</param>
        /// <param name="isParent">True if the folder decribes the parent of the current Album view.</param>
        public AlbumFolderInfo(Album owner, string path, bool isParent)
            : this(owner, path)
        {
            _isParent = isParent;
        }

        /// <summary>
        /// Constructs an AlbumFolderInfo.
        /// </summary>
        /// <param name="owner">The Album that owns this info.</param>
        /// <param name="path">The virtual path of the folder.</param>
        public AlbumFolderInfo(Album owner, string path)
            : base(owner, path) { }

        protected override char CommandCharacter
        {
            get
            {
                return Album.FolderCommand;
            }
        }

        protected override AlbumHandlerMode AlbumMode
        {
            get
            {
                return AlbumHandlerMode.Folder;
            }
        }

        /// <summary>
        /// The icon URL for the folder.
        /// </summary>
        public string IconUrl
        {
            get
            {
                //return Owner.FilePath +
                //    (_isParent ? "?albummode=parentthumbnail&albumpath=" : "?albummode=FolderThumbnail&albumpath=") +
                //    HttpUtility.UrlEncode(Path);
                return Owner.ServiceUrl +
                    (_isParent ? "?albummode=parentthumbnail&albumpath=" : "?albummode=FolderThumbnail&albumpath=") +
                    HttpUtility.UrlEncode(Path);
            }
        }

        /// <summary>
        /// True if the folder is the parent of the current view.
        /// </summary>
        public bool IsParent
        {
            get
            {
                return _isParent;
            }
        }

        /// <summary>
        /// The name of this folder.
        /// </summary>
        public string Name
        {
            get
            {
                if (_name == null)
                {
                    _name = Path.Substring(Path.LastIndexOf('/') + 1);
                }

                
                return _name;
            }
        }
    }
}
