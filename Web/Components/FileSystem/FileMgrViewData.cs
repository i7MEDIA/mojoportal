/*
 * QtPet Online File Manager v1.0
 * Copyright (c) 2009, Zhifeng Lin (fszlin[at]gmail.com)
 * 
 * Licensed under the MS-PL license.
 * http://qtfile.codeplex.com/license
 */

using System;
using System.Collections.Generic;

namespace mojoPortal.FileSystem
{
    /// <summary>
    /// Value object contains the initial data of a file manager.
    /// </summary>
    public class FileMgrViewData
    {
        public IEnumerable<WebFolder> Folders { get; set; }
        public string CurrentFolder { get; set; }
        public IEnumerable<WebFile> Files { get; set; }
    }
}
