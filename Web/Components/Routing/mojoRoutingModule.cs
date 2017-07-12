// Author:					
// Created:				    2010-10-18
// Last Modified:			2010-10-18
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Web;
using System.Web.Routing;


namespace mojoPortal.Web.Routing
{
    
    public class RoutingModule : UrlRoutingModule
    {

        //http://msdn.microsoft.com/en-us/library/system.web.routing.urlroutingmodule.aspx

        //http://www.salient6.com/blog/default.aspx

#if!MONO
        public override void PostResolveRequestCache(HttpContextBase context)
        {
            base.PostResolveRequestCache(context);
        }
#endif
    }

}