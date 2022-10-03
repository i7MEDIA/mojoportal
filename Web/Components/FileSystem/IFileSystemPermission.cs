/*
 * QtPet Online File Manager v1.0
 * Copyright (c) 2009, Zhifeng Lin (fszlin[at]gmail.com)
 * 
 * Licensed under the MS-PL license.
 * http://qtfile.codeplex.com/license
 * 
 * Last Modifed by  2011-08-18
 */

using System;

namespace mojoPortal.FileSystem
{
    /// <summary>
    /// Contract of user setting.
    /// </summary>
    public interface IFileSystemPermission
    {
        bool UserHasUploadPermission { get; } 
        bool UserHasBrowsePermission { get; }
		/// <summary>
		/// The root folder allowed 
		/// </summary>
		string VirtualRoot { get; }


        /// <summary>
        /// The root folder allowed 
        /// </summary>
        //string RootFolder { get; }

        /// <summary>
        /// an alias for presenting an alternate path in the UI
        /// </summary>
        //string DisplayFolder { get; }

        /// <summary>
        /// Whether the extension is allowed for the user.
        /// </summary>
        /// <param name="extension">
        /// The extension of the file.
        /// </param>
        /// <returns></returns>
        bool IsExtAllowed(string extension);

        /// <summary>
        /// Gets the disk space quota of the user.
        /// </summary>
        /// <value>
        /// The disk space quota of the user.
        /// </value>
        long Quota { get; }

        /// <summary>
        /// Gets the max allowed size for a single file in bytes of the user.
        /// </summary>
        /// <value>
        /// The max allowed size for a single file in bytes of the user.
        /// </value>
        long MaxSizePerFile { get; }

        /// <summary>
        /// Gets the max number of folders allowed for the user.
        /// </summary>
        /// <value>
        /// The max number of folders allowed for the user.
        /// </value>
        int MaxFolders { get; }

        /// <summary>
        /// Gets the max number of files allowed for the user.
        /// </summary>
        /// <value>
        /// The max number of files allowed for the user.
        /// </value>
        int MaxFiles { get; }

		string UserFolder { get; }
    }
}
