using System;
using System.Collections.Generic;
using System.Text;

namespace mojoPortal.Web.Controls.FolderGallery
{
    /// <summary>
    /// Storing location for the cached preview and thumbnail images.
    /// </summary>
    public enum ImageCacheLocation
    {
        /// <summary>
        /// Cached images are stored on disk in the application's compilation folder.
        /// </summary>
        Disk,
        /// <summary>
        /// Images are cached in memory.
        /// </summary>
        Memory,
        /// <summary>
        /// No caching. Images are redrawn on every request.
        /// </summary>
        None
    }

}
