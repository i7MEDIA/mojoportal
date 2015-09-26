
// borrowed from BlogEngine.NET
// License: Microsoft Reciprocal License (Ms-RL) 
namespace mojoPortal.Core.API.MetaWeblog
{
    /// <summary>
    /// MetaWeblog MediaInfo struct
    ///     returned from NewMediaObject call
    /// </summary>
    public struct MWAMediaInfo
    {
        #region Constants and Fields

        /// <summary>
        ///     Url that points to Saved MediaObejct
        /// </summary>
        public string url;

        public string file;

        /// <summary>
        ///     Type of file
        /// </summary>
        public string type;

        #endregion
    }
}