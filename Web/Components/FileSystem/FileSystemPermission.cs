/*
 * QtPet Online File Manager v1.0
 * Copyright (c) 2009, Zhifeng Lin (fszlin[at]gmail.com)
 * 
 * Licensed under the MS-PL license.
 * http://qtfile.codeplex.com/license
 * 
 * Last Modified 2009-12-30 
 */

using System.Collections.Generic;
using System.Linq;

namespace mojoPortal.FileSystem
{
	public class FileSystemPermission : IFileSystemPermission
    {
        public bool UserHasUploadPermission { get; set; } 
		public bool UserHasBrowsePermission { get; set; }
        public string VirtualRoot { get; set; }

        //public string RootFolder
        //{
        //    get;
        //    set;
        //}

        //public string DisplayFolder
        //{
        //    get;
        //    set;
        //}

        public bool IsExtAllowed(string extension)
        {
            return AllowedExtensions.Contains(extension.ToLower());
        }

        public IEnumerable<string> AllowedExtensions { get; set; }

        public long Quota { get; set; }

        public long MaxSizePerFile { get; set; }

        public int MaxFolders { get; set; }

        public int MaxFiles { get; set; }

		public string UserFolder { get; set; }
    }
}
