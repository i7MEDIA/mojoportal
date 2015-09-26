using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace mojoPortal.Web.Controls.FolderGallery
{
    /// <summary>
    /// A template container for the album handler.
    /// </summary>
    public sealed class AlbumTemplateContainer : Control, INamingContainer, IDataItemContainer
    {
        private Album _owner;

        /// <summary>
        /// Constructs a template container for an Album.
        /// </summary>
        /// <param name="owner">The Album that owns this template.</param>
        public AlbumTemplateContainer(Album owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// The Album that owns this template.
        /// </summary>
        public Album Owner
        {
            get
            {
                return _owner;
            }
        }

        /// <summary>
        /// The DataItem is the owner Album.
        /// </summary>
        object IDataItemContainer.DataItem
        {
            get { return Owner; }
        }

        /// <summary>
        /// The templates are single items, so the index is always zero.
        /// </summary>
        int IDataItemContainer.DataItemIndex
        {
            get { return 0; }
        }

        /// <summary>
        /// The templates are single items, so the index is always zero.
        /// </summary>
        int IDataItemContainer.DisplayIndex
        {
            get { return 0; }
        }
    }
}
