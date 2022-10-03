using System;
using System.Collections;
using System.Security.Permissions;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Medium)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.High)]
    public class mojoVirtualPathProvider : VirtualPathProvider
    {
        public mojoVirtualPathProvider() : base()
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


}
