// Author:             
// Created:            2005-01-17
// Last Modified:      2010-08-30

using System;
using System.Collections;
using System.Security.Permissions;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web
{
#if MONO

    public class mojoVirtualPathProvider : VirtualPathProvider
    {
       
        /// <summary>
        ///  AppInitialize() is not used in mojoportal. It would be called on startup
        ///  if this file was in the App_Code folder but who wants a folder with a generic 
        ///  name like App_code and besides we can just as easily register our 
        ///  VirtualPathProvider in the Global.asax.cs Application_OnStart as we do
        ///
        /// </summary>
        public static void AppInitialize()
        {
            HostingEnvironment.RegisterVirtualPathProvider(new mojoVirtualPathProvider());
        }

        public override string CombineVirtualPaths(string basePath, string relativePath)
        {
            return base.CombineVirtualPaths(basePath, relativePath);
          
        }

        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            return base.GetDirectory(virtualDir);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (virtualPath == null) return null;

            // don't handle requests for css, its not needed
            // under IIS requests for .css are not handled by
            // the asp.net runtime anyway but under the VS web server
            // all files are so this is to prevent side effects of that
            // our StyleSheet user control correctly handles css
            // so we don't need to re-map files here
            String loweredPath = virtualPath.ToLower();
            if (
                (
                (virtualPath.Contains("App_Themes/default"))
                || (virtualPath.Contains("App_Themes/pageskin"))
                )
                && (!loweredPath.EndsWith(".css"))
                && (!loweredPath.EndsWith(".gif"))
                && (!loweredPath.EndsWith(".jpg"))
                && (!loweredPath.EndsWith(".png"))
                && (!loweredPath.EndsWith(".js"))
                && (!loweredPath.EndsWith(".ico"))
                && (!loweredPath.EndsWith(".axd"))
                )
            {
                try
                {
                    return new mojoThemeVirtualFile(virtualPath);
                }
                catch (System.UnauthorizedAccessException)
                { }
                
            }
            return base.GetFile(virtualPath);
        }

        public override CacheDependency GetCacheDependency(
            string virtualPath,
            IEnumerable virtualPathDependencies, 
            DateTime utcStart)
        {
            if (virtualPath.Contains("App_Themes/default"))
            {
                String pathToDependencyFile = CacheHelper.GetPathToThemeCacheDependencyFile();
                String pathToThemeFile = SiteUtils.GetFullPathToThemeFile();
                if(pathToDependencyFile != null)
                {
                    AggregateCacheDependency dependency = new AggregateCacheDependency();
                    dependency.Add(new CacheDependency(pathToDependencyFile));
                    try
                    {
                        dependency.Add(new CacheDependency(pathToThemeFile));
                    }
                    catch (HttpException)
                    { // this can happen if the site is configured for a skin that doesn't exist

                    }
                    return dependency;

                }
            }

            return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

    }

#else

    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Medium)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.High)]
    public class mojoVirtualPathProvider : VirtualPathProvider
    {
        public mojoVirtualPathProvider()
            : base()
        {

        }
       
        /// <summary>
        ///  AppInitialize() is not used in mojoportal. It would be called on startup
        ///  if this file was in the App_Code folder 
        ///
        /// </summary>
        public static void AppInitialize()
        {
            HostingEnvironment.RegisterVirtualPathProvider(new mojoVirtualPathProvider());
        }

        private bool IsThemePath(string virtualPath)
        {
            return virtualPath.Contains("App_Themes");
        }

        public override bool FileExists(string virtualPath)
        {
            if (IsThemePath(virtualPath))
            {
                // just pretend it exists
                return true;
            }
            else
            { 
               return Previous.FileExists(virtualPath);
                    
            }
                
        }

        public override bool DirectoryExists(string virtualDir)
        {
            if (IsThemePath(virtualDir))
            {
                return (!virtualDir.Contains("Resources"));
            }
            else
                return Previous.DirectoryExists(virtualDir);
        }


        public override string CombineVirtualPaths(string basePath, string relativePath)
        {
            return base.CombineVirtualPaths(basePath, relativePath);
        }

        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            if (IsThemePath(virtualDir))
            {
                return new mojoThemeVirtualDirectory(Previous.GetDirectory(virtualDir));
            }
            return Previous.GetDirectory(virtualDir);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (virtualPath == null) return null;

            string loweredPath = virtualPath.ToLower();
           
            if (loweredPath.EndsWith(".skin"))
            {
                try
                {
                    return new mojoThemeVirtualFile(virtualPath);
                }
                catch (System.UnauthorizedAccessException)
                { }
                
            }
            return Previous.GetFile(virtualPath);
        }

        public override CacheDependency GetCacheDependency(
            string virtualPath,
            IEnumerable virtualPathDependencies, 
            DateTime utcStart)
        {

            if (IsThemePath(virtualPath))
            {
                AggregateCacheDependency dependency = new AggregateCacheDependency();

                string pathToWebConfig = CacheHelper.GetPathToWebConfigFile();
                dependency.Add(new CacheDependency(pathToWebConfig));

                if (WebConfigSettings.UseCacheDependencyFiles)
                {
                    string pathToDependencyFile = CacheHelper.GetPathToThemeCacheDependencyFile();
                    if (!string.IsNullOrEmpty(pathToDependencyFile)) { dependency.Add(new CacheDependency(pathToDependencyFile)); }
                }

                string pathToThemeFile = SiteUtils.GetFullPathToThemeFile();
                if (!string.IsNullOrEmpty(pathToThemeFile))
                {
                    try
                    {
                        dependency.Add(new CacheDependency(pathToThemeFile));
                    }
                    catch (HttpException)
                    { // this can happen if the site is configured for a skin that doesn't exist
                    }
                }

                return dependency;
               
            }

            return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

    }

#endif
}
