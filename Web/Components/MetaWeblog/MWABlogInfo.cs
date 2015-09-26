
// borrowed from BlogEngine.NET
// License: Microsoft Reciprocal License (Ms-RL) 
namespace mojoPortal.Core.API.MetaWeblog
{
    /// <summary>
    /// MetaWeblog BlogInfo struct
    ///     returned as an array from getUserBlogs
    /// </summary>
    public struct MWABlogInfo
    {
        #region Constants and Fields

        /// <summary>
        ///     Blog ID (Since BlogEngine.NET is single instance this number is always 10.
        /// </summary>
        public string blogID;

        /// <summary>
        ///     Blog Title
        /// </summary>
        public string blogName;

        /// <summary>
        ///     Blog Url
        /// </summary>
        public string url;

        public string pageEditRoles;

        public string moduleEditRoles;

        public int editUserId;

        public int pageId;

        public string xmlrpcUrl;

        #endregion
    }
}