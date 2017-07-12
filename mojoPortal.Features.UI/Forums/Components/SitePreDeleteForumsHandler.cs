using System;
using System.Configuration.Provider;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using log4net;

namespace mojoPortal.Features
{
    /// <summary>
    /// Author:                     
    /// Created:                    2008-11-12
    ///	Last Modified:              2008-11-12
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    ///  
    /// </summary>
    public class SitePreDeleteForumsHandler : SitePreDeleteHandlerProvider
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(SitePreDeleteForumsHandler));

        public SitePreDeleteForumsHandler()
        { }

        public override void DeleteSiteContent(int siteId)
        {
            Forum.DeleteBySite(siteId);

        }
    }
}
