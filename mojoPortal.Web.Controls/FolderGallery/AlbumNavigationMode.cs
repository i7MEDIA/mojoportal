using System;
using System.Collections.Generic;
using System.Text;

namespace mojoPortal.Web.Controls.FolderGallery
{
    /// <summary>
    /// Navigation mode when the album is used as a control.
    /// </summary>
    public enum AlbumNavigationMode
    {
        /// <summary>
        /// The default, this mode uses callbacks to refresh the album
        /// without navigating away from the page or posting back.
        /// Ifthe browser does not support callbacks, the control will post back.
        /// </summary>
        Callback,
        /// <summary>
        /// This mode uses form post backs to navigate in the album.
        /// </summary>
        Postback,
        /// <summary>
        /// Uses regular links to navigate in the album.
        /// May have side-effects on the rest of the page,
        /// which may have its own state management.
        /// </summary>
        Link
    }

}
