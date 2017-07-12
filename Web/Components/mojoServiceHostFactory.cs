// Author:					
// Created:				    2008-12-15
// Last Modified:		    2009-01-16
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

#if !MONO
using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web.ApplicationServices;

using log4net;

namespace mojoPortal.Web
{
    /// <summary>
    ///
    /// </summary>
    public class mojoServiceHostFactory : ServiceHostFactory
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(mojoServiceHostFactory));

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            mojoServiceHost customServiceHost;

            int preferredHostIndex = -1;
            if (IsAppService(serviceType))
            {
                preferredHostIndex = GetAppServicePreferredHostIndex(baseAddresses);
            }
            else
            {
                preferredHostIndex = GetPreferredHostIndex(baseAddresses);
            }

            if (preferredHostIndex > -1)
            {
                customServiceHost = new mojoServiceHost(serviceType, baseAddresses[preferredHostIndex]);
                return customServiceHost;
            }

            customServiceHost = new mojoServiceHost(serviceType, baseAddresses[0]);
            return customServiceHost;
        }


        private int GetAppServicePreferredHostIndex(Uri[] baseAddresses)
        {
            string siteRoot = string.Empty;
            
            if (SiteUtils.SslIsAvailable())
            {
                siteRoot = SiteUtils.GetSecureNavigationSiteRoot();
            }
            else
            {
                siteRoot = SiteUtils.GetNavigationSiteRoot();
            }

            if (baseAddresses.Length == 0) { return -1; }

            for (int i = 0; i < baseAddresses.Length; i++)
            {
                log.Debug("available endpoint " + baseAddresses[i].ToString());
            }

            for (int i = 0; i < baseAddresses.Length; i++)
            {
                string b = baseAddresses[i].ToString();
                if (b.Contains(siteRoot))
                {
                    log.Debug("matched endpoint " + baseAddresses[i].ToString());
                    return i;
                }
            }

            return -1;
        }

        
        
        /// <summary>
        /// not using this yet and not sure its right
        /// </summary>
        /// <param name="baseAddresses"></param>
        /// <returns></returns>
        private int GetPreferredHostIndex(Uri[] baseAddresses)
        {
            string siteRoot = SiteUtils.GetNavigationSiteRoot();
            string secureSiteRoot = SiteUtils.GetSecureNavigationSiteRoot();

            if (baseAddresses.Length == 0) { return -1; }

            for (int i = 0; i < baseAddresses.Length; i++)
            {
                log.Debug("available endpoint " + baseAddresses[i].ToString());
            }

            //look for a secure endpoint first
            for (int i = 0; i < baseAddresses.Length; i++)
            {
                string b = baseAddresses[i].ToString();
                if (b.Contains(secureSiteRoot))
                {
                    log.Debug("matched endpoint " + baseAddresses[i].ToString());
                    return i;
                } 
            }

            // if not found try http
            for (int i = 0; i < baseAddresses.Length; i++)
            {
                string b = baseAddresses[i].ToString();
                if (b.Contains(siteRoot))
                {
                    log.Debug("matched endpoint " + baseAddresses[i].ToString());
                    return i;
                }
            }

            return -1;
        }

        private bool IsAppService(Type serviceType)
        {
//#if !MONO
            if (serviceType == typeof(AuthenticationService)) { return true; }
            if (serviceType == typeof(RoleService)) { return true; }

            return false;
//#endif
//#if MONO
            //return true;
//#endif  
        }


    }
}
#endif