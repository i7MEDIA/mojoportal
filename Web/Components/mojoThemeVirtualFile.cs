// Author:             
// Created:            2006-01-19
// Last Modified:      2011-06-26

using System;
using System.Security.Permissions;
using System.Web;
using System.Web.Hosting;
using System.IO;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using log4net;

namespace mojoPortal.Web
{

#if MONO
    public class mojoThemeVirtualFile : VirtualFile
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(mojoThemeVirtualFile));

        private String pathToFile;
        public mojoThemeVirtualFile(String virtualPath)
            : base(virtualPath)
        {
            pathToFile = virtualPath;
        }

        public override Stream Open()
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            PageSettings currentPage = CacheHelper.GetCurrentPage();
            bool reset = false;
            if (siteSettings != null)
            {
                if (
                    (currentPage != null)
                    &&(siteSettings.AllowPageSkins)
                    &&(currentPage.Skin.Length > 0)
                    &&(pathToFile.Contains("App_Themes/pageskin"))
                    )
                {
                    pathToFile = pathToFile.Replace("App_Themes/pageskin", "Data/Sites/"
                       + siteSettings.SiteId.ToString()
                       + "/skins/" + currentPage.Skin);
                }
                else
                {
                    reset = true;
                    pathToFile = pathToFile.Replace("App_Themes/default", "Data/Sites/"
                       + siteSettings.SiteId.ToString()
                       + "/skins/" + siteSettings.Skin);
                }

            }
            if (reset)
            {
                try
                {
                    CacheHelper.ResetThemeCache();
                }
                catch (UnauthorizedAccessException ex)
                {
                    log.Error("Error trying to reset theme cache", ex);
                }
            }

            String filePath = HttpContext.Current.Server.MapPath(pathToFile);

            try
            {
                return File.OpenRead(filePath);
            }
            catch (DirectoryNotFoundException ex)
            {
                log.Error("Error trying to set theme", ex);
            }
            catch (FileNotFoundException ex)
            {
                log.Error("Error trying to set theme", ex);
            }

            if(HttpContext.Current != null)
            return File.OpenRead(HttpContext.Current.Server.MapPath(this.VirtualPath));

            return null;
           
        }

    }

#else
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class mojoThemeVirtualFile : VirtualFile
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(mojoThemeVirtualFile));

        private String pathToFile;
        public mojoThemeVirtualFile(String virtualPath)
            : base(virtualPath)
        {
            pathToFile = virtualPath;
        }

        public override Stream Open()
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            PageSettings currentPage = CacheHelper.GetCurrentPage();
            
            if (
                (siteSettings != null)
                && ((siteSettings.AllowUserSkins) || ((WebConfigSettings.AllowEditingSkins) && (WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins))))
                )
            {
                if (pathToFile.Contains("App_Themes/userpersonal"))
                {
                    SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                    if ((currentUser != null)&&(currentUser.Skin.Length > 0))
                    {
                        pathToFile = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/skins/" + currentUser.Skin + "/theme.skin";
                    }
                }
            }

            if (
               (pathToFile.Contains("App_Themes/mobile"))
                )
            {
                if (siteSettings.MobileSkin.Length > 0)
                {
                    pathToFile = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/skins/" + siteSettings.MobileSkin + "/theme.skin";
                }

                if (WebConfigSettings.MobilePhoneSkin.Length > 0)
                {
                    pathToFile = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/skins/" + WebConfigSettings.MobilePhoneSkin + "/theme.skin";
                }
            }
            else if (
                (currentPage != null)
                &&(siteSettings.AllowPageSkins)
                &&(currentPage.Skin.Length > 0)
                &&(pathToFile.Contains("App_Themes/pageskin"))
                )
            {
                pathToFile = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/skins/" + currentPage.Skin + "/theme.skin";
            }
            else if (pathToFile.Contains("App_Themes/default"))
            {
                
                pathToFile = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/skins/" + siteSettings.Skin + "/theme.skin";
            }
            else if (pathToFile.Contains("App_Themes/mypage"))
            {
                pathToFile = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/skins/" + siteSettings.MyPageSkin + "/theme.skin";
            }
            else if (pathToFile.Contains("App_Themes/preview_"))
            {
                pathToFile = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/skins/" + SiteUtils.GetSkinPreviewParam(siteSettings) + "/theme.skin";
            }

            string filePath = HostingEnvironment.MapPath(pathToFile);

            try
            {
                return File.OpenRead(filePath);
            }
            catch (DirectoryNotFoundException ex)
            {
                log.Error("Error trying to set theme", ex);
            }
            catch (FileNotFoundException ex)
            {
                log.Error("Error trying to set theme", ex);
            }


            log.Error("could not set theme to skin folder theme.skin will use /App_Themes/default/theme.skin");
            
            return File.OpenRead(HostingEnvironment.MapPath("~/App_Themes/default/theme.skin"));

           
           
        }

    }

#endif

}
