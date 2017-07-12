using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mojoPortal.Web
{
    public class mojoViewEngine : RazorViewEngine
    {
        private string skin = "framework";
        private SiteSettings siteSettings;

        public mojoViewEngine()
        {
            List<string> masterLocationFormats = new List<string>(MasterLocationFormats);
            masterLocationFormats.Insert(0, "~/Data/Sites/$SiteId$/skins/$Skin$/Views/Shared/{0}.cshtml");
            MasterLocationFormats = masterLocationFormats.ToArray();

            List<string> partialViewLocationFormats = new List<string>(PartialViewLocationFormats);
            partialViewLocationFormats.Insert(0, "~/Data/Sites/$SiteId$/skins/$Skin$/Views/{1}/{0}.cshtml");
            PartialViewLocationFormats = partialViewLocationFormats.ToArray();


            List<string> viewLocationFormats = new List<string>(ViewLocationFormats);
            viewLocationFormats.Insert(0, "~/Data/Sites/$SiteId$/skins/$Skin$/Views/{1}/{0}.cshtml");
            ViewLocationFormats = viewLocationFormats.ToArray();
        }

        protected override IView CreatePartialView(ControllerContext ctx, string partialPath)
        {
            PopulateVariables();
            return base.CreatePartialView(ctx, OverriddenViewPath(ctx, partialPath));
        }
 
        protected override IView CreateView(ControllerContext ctx, string viewPath, string masterPath)
        {
            PopulateVariables();
            return base.CreateView(ctx, OverriddenViewPath(ctx, viewPath), OverriddenViewPath(ctx, masterPath));
        }

        protected override bool FileExists(ControllerContext ctx, string virtualPath)
        {
            PopulateVariables();
            string overriddenPath = OverriddenViewPath(ctx, virtualPath);
            bool exists = base.FileExists(ctx, overriddenPath);
            return exists;
        }

        //public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        //{
        //    ViewEngineResult veResult = new ViewEngineResult()
        //}

        private string OverriddenViewPath( ControllerContext ctx, string viewPath)
        {
            return viewPath.Replace("$Skin$", skin).Replace("$SiteId$", siteSettings.SiteId.ToString());
        }
        private void PopulateVariables()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings(); 
            if (siteSettings == null)
            {
                //not sure this would ever happen ... 
                return;
            }
            skin = siteSettings.Skin;
            PageSettings currentPage = CacheHelper.GetCurrentPage();
            if (currentPage != null && !String.IsNullOrWhiteSpace(currentPage.Skin))
            {
                skin = currentPage.Skin;
            }
        }

        //public void AddViewLocationFormat(string paths)
        //{
        //    List<string> existingPaths = new List<string>(ViewLocationFormats);
        //    existingPaths.Add(paths);

        //    ViewLocationFormats = existingPaths.ToArray();
        //}

        //public void AddPartialViewLocationFormat(string paths)
        //{
        //    List<string> existingPaths = new List<string>(PartialViewLocationFormats);
        //    existingPaths.Add(paths);

        //    PartialViewLocationFormats = existingPaths.ToArray();
        //}
    }
}