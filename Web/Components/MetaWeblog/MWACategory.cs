
// borrowed from BlogEngine.NET
// License: Microsoft Reciprocal License (Ms-RL) 
namespace mojoPortal.Core.API.MetaWeblog
{
    /// <summary>
    /// MetaWeblog Category struct
    ///     returned as an array from GetCategories
    /// </summary>
    public struct MWACategory
    {
        #region Constants and Fields

        /// <summary>
        ///     Category title
        /// </summary>
        public string description;

        /// <summary>
        ///     Url to thml display of category
        /// </summary>
        public string htmlUrl;

        /// <summary>
        ///     The guid of the category
        /// </summary>
        public string id;

        public string parentId;

        /// <summary>
        ///     Url to RSS for category
        /// </summary>
        public string rssUrl;

        /// <summary>
        ///     The title/name of the category
        /// </summary>
        public string title;

        #endregion
    }
}