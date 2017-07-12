//  Author:				
//  Created:			2006-05-16
//	Last Modified:		2009-04-10
//	
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.IO;
using System.Data;
using System.Collections.ObjectModel;
using System.Web;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;

namespace mojoPortal.Features.UI
{
    /// <summary>
    /// 	
    /// </summary>
    public class mojoUserCatalogPart : CatalogPart
    {
        public override WebPartDescriptionCollection GetAvailableWebPartDescriptions()
        {
            Collection<WebPartDescription> colDescriptions = new Collection<WebPartDescription>();

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings != null)
            {
                using (IDataReader reader = WebPartContent.GetMostPopular
                    (siteSettings.SiteId, WebConfigSettings.NumberOfWebPartsToShowInMiniCatalog))
                {
                    while (reader.Read())
                    {
                        bool allowMultipleInstances = Convert.ToBoolean(reader["AllowMultipleInstancesOnMyPage"]);
                        bool isAssembly = Convert.ToBoolean(reader["IsAssembly"]);

                        String moduleIcon = reader["ModuleIcon"].ToString();
                        String featureIcon = reader["FeatureIcon"].ToString();
                        String imageUrl = featureIcon;
                        if (moduleIcon.Length > 0)
                        {
                            imageUrl = moduleIcon;
                        }

                        if (imageUrl.Length > 0)
                        {
                            imageUrl = Page.ResolveUrl("~/Data/SiteImages/FeatureIcons/" + imageUrl);
                        }

                        WebPartDescription wpDescription;

                        if (isAssembly)
                        {
                            wpDescription
                            = new WebPartDescription(
                                reader["WebPartID"].ToString(),
                                reader["ModuleTitle"].ToString(),
                                ResourceHelper.GetResourceString(reader["ResourceFile"].ToString(), reader["FeatureName"].ToString()),
                                imageUrl);

                        }
                        else
                        {
                            wpDescription
                            = new WebPartDescription(
                                reader["ModuleID"].ToString(),
                                reader["ModuleTitle"].ToString(),
                                ResourceHelper.GetResourceString(reader["ResourceFile"].ToString(), reader["FeatureName"].ToString()),
                                imageUrl);

                        }



                        if (allowMultipleInstances)
                        {
                            colDescriptions.Add(wpDescription);
                        }
                        else
                        {
                            if (!PageHasPart(wpDescription.Title, wpDescription.Description))
                            {
                                colDescriptions.Add(wpDescription);
                            }
                        }
                    }

                }

            }

            WebPartDescriptionCollection wpdCollection
                = new WebPartDescriptionCollection(colDescriptions);

            return wpdCollection;
        }

        

        public override WebPart GetWebPart(WebPartDescription description)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) return null;

            WebPart webPart = null;
            if (description.ID.Length == 36)
            {
                Guid webPartID = new Guid(description.ID);
                WebPartContent webPartContent = new WebPartContent(webPartID);
                if (webPartContent.WebPartId != Guid.Empty)
                {
                    if (HttpContext.Current != null)
                    {
                        String path = HttpContext.Current.Server.MapPath("~/bin")
                                      + Path.DirectorySeparatorChar + webPartContent.AssemblyName + ".dll";
                        Assembly assembly = Assembly.LoadFrom(path);
                        Type type = assembly.GetType(webPartContent.ClassName, true, true);
                        object obj = Activator.CreateInstance(type);
                        if (obj != null)
                        {
                            webPart = (WebPart)obj;
                            WebPartContent.UpdateCountOfUseOnMyPage(webPartContent.WebPartId, 1);

                        }
                    }
                }
            }
            else
            {
                mojoPortal.Business.Module module = new mojoPortal.Business.Module(int.Parse(description.ID));

                SiteModuleControl siteModule = Page.LoadControl("~/" + module.ControlSource) as SiteModuleControl;
                if (siteModule != null)
                {
                    siteModule.SiteId = siteSettings.SiteId;
                    siteModule.ID = "module" + module.ModuleId.ToString(CultureInfo.InvariantCulture);

                    siteModule.ModuleConfiguration = module;
                    siteModule.RenderInWebPartMode = true;

                    webPart = WebPartManager.CreateWebPart(siteModule);
                    

                    siteModule.ModuleId = module.ModuleId;
                    mojoPortal.Business.Module.UpdateCountOfUseOnMyPage(module.ModuleId, 1);
                }
            }

            return webPart;
        }


        public override string Title
        {
            get
            {
                return Resources.MyPageResources.mojoUserCatalogPartTitle;
                //return SiteUtils.GetLocalizedValue("mojoUserCatalogPartTitle");
            }
            set
            {
                base.Title = value;
            }
        }


        private bool PageHasPart(String title, String description)
        {
            foreach (WebPart part in WebPartManager.WebParts)
            {
                if ((part.Title == title) && (part.Description == description)) return true;
            }
            return false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) return;

            int countOfWebParts = WebPartContent.Count(siteSettings.SiteId);
            if (countOfWebParts > WebConfigSettings.NumberOfWebPartsToShowInMiniCatalog)
            {
                string text = "<a href='" + WebUtils.GetSiteRoot()
                              + "/ChooseContent.aspx' "
                              + " class='webpartcatalogmorelink'>"
                              + Resources.MyPageResources.WebPartCatalogMoreLink
                              + "</a><br /><br />";

                this.Controls.Add(new LiteralControl(text));
            }
        }

    }
}
