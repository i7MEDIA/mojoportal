// Author:		        
// Created:             2009-05-16
// Last Modified:       2009-05-17
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Runtime.Serialization;

namespace mojoPortal.Web
{
    /// <summary>
    /// A class for deserializing the rpxacountinfo from json result of a server side web call.
    /// </summary>
#if !MONO
    [DataContract]
#endif
    public class OpenIdRpxAccountInfo
    {
#if!MONO
        [DataMember(Name = "apiKey", IsRequired = true)]
#endif
        public string ApiKey { get; set; }
#if!MONO
        [DataMember(Name = "realm", IsRequired = true)]
#endif
        public string Realm { get; set; }
#if!MONO
        [DataMember(Name = "adminUrl", IsRequired = true)]
#endif
        public string AdminUrl { get; set; }

        private string requestId = string.Empty;
#if!MONO
        [DataMember(Name = "requestId", IsRequired = false)]
#endif
        public string RequestId 
        {
            get { return requestId; }
            set { requestId = value; }
        }

    }
}
