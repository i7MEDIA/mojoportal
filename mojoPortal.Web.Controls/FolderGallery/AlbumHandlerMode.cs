using System;
using System.Collections.Generic;
using System.Text;

namespace mojoPortal.Web.Controls.FolderGallery
{
    /// <summary>
    /// The display modes or views for the Album Handler.
    /// </summary>
    public enum AlbumHandlerMode
    {
        /// <summary>
        /// Unknown mode.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Displays the contents of a folder.
        /// </summary>
        Folder = 1,
        /// <summary>
        /// Displays the preview page for an image.
        /// </summary>
        Page = 2,
        /// <summary>
        /// Returns the reduced image used in the preview page.
        /// </summary>
        Preview = 3,
        /// <summary>
        /// Returns an image thumbnail.
        /// </summary>
        Thumbnail = 4,
        /// <summary>
        /// Returns a folder thumbnail.
        /// </summary>
        FolderThumbnail = 5,
        /// <summary>
        /// Returns a parent folder thumbnail.
        /// </summary>
        ParentThumbnail = 6,
        /// <summary>
        /// Returns the CSS stylesheet.
        /// </summary>
        Css = 7
    }
}
