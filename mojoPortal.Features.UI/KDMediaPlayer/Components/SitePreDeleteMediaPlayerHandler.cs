// Author:				    
// Created:			        2011-12-06
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// Last Modified: 2011-12-06

using System;
using System.Configuration.Provider;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Features.Business;
using log4net;

namespace mojoPortal.Features
{
    public class SitePreDeleteMediaPlayerHandler : SitePreDeleteHandlerProvider
    {
        public SitePreDeleteMediaPlayerHandler()
        { }

        public override void DeleteSiteContent(int siteId)
        {
            MediaPlayer.RemoveBySite(siteId);

        }
    }
}