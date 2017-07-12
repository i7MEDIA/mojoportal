// Author:					
// Created:				    2011-03-23
// Last Modified:			2012-06-11
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Hosting;
using mojoPortal.Business;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web
{
    public class HtmlContentInstaller : IContentInstaller
    {

        public void InstallContent(Module module, string configInfo)
        {
            HtmlContent htmlContent = new HtmlContent();
            htmlContent.ModuleId = module.ModuleId;
            if (configInfo.StartsWith("~/"))
            {
                if (File.Exists(HostingEnvironment.MapPath(configInfo)))
                {
                    htmlContent.Body = File.ReadAllText(HostingEnvironment.MapPath(configInfo), Encoding.UTF8);
                }
            }
            else
            {
                htmlContent.Body = ResourceHelper.GetMessageTemplate(CultureInfo.CurrentUICulture, configInfo);
            }

            htmlContent.ModuleGuid = module.ModuleGuid;

            SiteSettings siteSettings = new SiteSettings(module.SiteId);
            SiteUser adminUser = null;
            if (siteSettings.UseEmailForLogin)
            {
                adminUser = new SiteUser(siteSettings, "admin@admin.com");
                if (adminUser.UserId == -1) { adminUser = null; }
            }
            else
            {
                adminUser = new SiteUser(siteSettings, "admin");
                if (adminUser.UserId == -1) { adminUser = null; }
            }

            if (adminUser != null)
            {
                htmlContent.UserGuid = adminUser.UserGuid;
                htmlContent.LastModUserGuid = adminUser.UserGuid;
            }

            HtmlRepository repository = new HtmlRepository();
            repository.Save(htmlContent);

        }


    }
}