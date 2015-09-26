
// borrowed from BlogEngine.NET
// License: Microsoft Reciprocal License (Ms-RL) 
namespace mojoPortal.Core.API.MetaWeblog
{
    /// <summary>
    /// MetaWeblog Fault struct
    ///     returned when error occurs
    /// </summary>
    public struct MWAFault
    {
        #region Constants and Fields

        /// <summary>
        ///     Error code of Fault Response
        /// </summary>
        public string faultCode;

        /// <summary>
        ///     Message of Fault Response
        /// </summary>
        public string faultString;

        #endregion
    }
}