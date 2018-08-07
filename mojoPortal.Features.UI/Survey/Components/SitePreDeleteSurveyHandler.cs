// Created:       2008-11-12
// Last Modified: 2018-07-31
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
//  

using log4net;
using mojoPortal.Business.WebHelpers;
using SurveyFeature.Business;

namespace mojoPortal.Features
{
	public class SitePreDeleteSurveyHandler : SitePreDeleteHandlerProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SitePreDeleteSurveyHandler));


		public SitePreDeleteSurveyHandler()
        { }


		public override void DeleteSiteContent(int siteId)
        {
            Survey.DeleteBySite(siteId);
        }
    }
}