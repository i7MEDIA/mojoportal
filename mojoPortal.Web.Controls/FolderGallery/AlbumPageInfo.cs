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
    /// Base for classes that describe an album page
    /// </summary>
    public abstract class AlbumPageInfo
    {
        private Album _owner;
        private string _path;

        private string _link;
        private string _permalink;

        /// <summary>
        /// Constructs an AlbumPageInfo.
        /// </summary>
        /// <param name="owner">The Album that owns this info.</param>
        /// <param name="path">The virtual path of the page.</param>
        public AlbumPageInfo(Album owner, string path)
        {
            _owner = owner;
            _path = path;
        }

        /// <summary>
        /// The Album that owns this page.
        /// </summary>
        public Album Owner
        {
            get
            {
                return _owner;
            }
        }

        /// <summary>
        /// The virtual path to the page.
        /// </summary>
        public string Path
        {
            get
            {
                return _path;
            }
        }

        /// <summary>
        /// The character used as a command for this type of album page
        /// </summary>
        protected abstract char CommandCharacter { get; }

        /// <summary>
        /// The AlbumHandlerMode for this type of pages
        /// </summary>
        protected abstract AlbumHandlerMode AlbumMode { get; }

        /// <summary>
        /// A javascript link (callback if the browser supports it, postback otherwise) to this album page.
        /// </summary>
        public string Link
        {
            get
            {
                if (_link == null)
                {
                    if (!_owner.IsControl || _owner.NavigationMode == AlbumNavigationMode.Link)
                    {
                        _link = PermaLink;
                    }
                    else
                    {
                        string dirPrefix = _owner.PathPrefix;
                        string arg = CommandCharacter + _path;
                        if (_owner.NavigationMode == AlbumNavigationMode.Callback &&
                            HttpContext.Current.Request.Browser.SupportsCallback)
                        {

                            _owner.Page.ClientScript.RegisterForEventValidation(_owner.UniqueID, arg);
                            _link = "javascript:" + _owner.Page.ClientScript.GetCallbackEventReference(
                                _owner,
                                '\'' + ImageHelper.JScriptEncode(arg) + '\'',
                                Album.CallbackFunction,
                                '\'' + _owner.ClientID + '\'', false);
                        }
                        else
                        {
                            _link = _owner.Page.ClientScript.GetPostBackClientHyperlink(_owner, arg, true);
                        }
                    }
                }
                return _link;
            }
        }

        /// <summary>
        /// A permanent link to this image's preview page.
        /// </summary>
        public string PermaLink
        {
            get
            {
                if (_permalink == null)
                {
                    //_permalink = ((_owner.Page != null) ?
                    //    _owner.ResolveClientUrl(_owner.Page.AppRelativeVirtualPath) :
                    //    String.Empty) +
                    //    "?albummode=" + AlbumMode + "&albumpath=" + HttpUtility.UrlEncode(_path);

                    _permalink = ((_owner.Page != null) ?
                        _owner.ResolveUrl(_owner.Page.AppRelativeVirtualPath) :
                        String.Empty) +
                        "?albummode=" + AlbumMode + "&albumpath=" + HttpUtility.UrlEncode(_path);
                }
                return _permalink;
            }
        }
    }
}
