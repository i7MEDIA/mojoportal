using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mojoPortal.FileSystem
{
    public enum OpResult
    {
        Succeed,

        Error,

        NotFound,

        AlreadyExist,

        FolderNotFound,

        FolderLimitExceed,

        FileLimitExceed,

        FileSizeLimitExceed,

        FileTypeNotAllowed,

        QuotaExceed,

        Denied
    }
}
