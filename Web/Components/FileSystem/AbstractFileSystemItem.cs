using System;

namespace mojoPortal.FileSystem
{
    public abstract class AbstractFileSystemItem
    {
        public string Name { get; set; }
        public string VirtualPath { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}