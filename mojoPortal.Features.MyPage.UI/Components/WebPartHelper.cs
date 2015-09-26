using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using mojoPortal.Business;

namespace mojoPortal.Features.UI
{
    
    public sealed class WebPartHelper
    {
       
        private WebPartHelper()
        { }


        public static Collection<Type> GetWebPartsFromAssemblies(string excludedAssemblies)
        {
            Collection<Type> webPartTypes = new Collection<Type>();

            if (HttpContext.Current != null)
            {
                DirectoryInfo currentDirectory
                    = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/bin"));

                foreach (FileInfo fileInfo in currentDirectory.GetFiles("*.dll"))
                {
                    //if (fileInfo.FullName.IndexOf("sqlite") == -1)
                    if (!excludedAssemblies.Contains(fileInfo.Name))
                    {
                        Assembly assembly = Assembly.LoadFrom(fileInfo.FullName);
                        foreach (Type type in assembly.GetTypes())
                        {
                            if (type.IsSubclassOf(typeof(WebPart)))
                            {
                                webPartTypes.Add(type);

                            }
                        }
                    }
                }
            }

            return webPartTypes;

        }

        public static Collection<Type> GetUninstalledWebPartsFromAssemblies(int siteId, string excludedAssemblies)
        {
            Collection<Type> webPartTypes = new Collection<Type>();

            if (HttpContext.Current != null)
            {
                DirectoryInfo currentDirectory
                    = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/bin"));

                foreach (FileInfo fileInfo in currentDirectory.GetFiles("*.dll"))
                {
                    //if (fileInfo.FullName.IndexOf("sqlite") == -1)
                    if (!excludedAssemblies.Contains(fileInfo.Name))
                    {
                        Assembly assembly = Assembly.LoadFrom(fileInfo.FullName);
                        foreach (Type type in assembly.GetTypes())
                        {
                            if (type.IsSubclassOf(typeof(WebPart)))
                            {
                                String assemblyName = type.Assembly.FullName;
                                if (assemblyName.IndexOf(",") > -1)
                                {
                                    assemblyName = assemblyName.Substring(0, assemblyName.IndexOf(","));
                                }
                                if (!WebPartContent.Exists(siteId, type.FullName, assemblyName))
                                {
                                    webPartTypes.Add(type);
                                }

                            }
                        }
                    }
                }
            }

            return webPartTypes;

        }

    }
}
