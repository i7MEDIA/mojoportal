using System;
using System.Data.Common;
using System.Web;

namespace mojoPortal.Business.WebHelpers
{
    public static class VirtualFolderEvaluator
    {
        /// <summary>
        /// find first level folder name after site root
        /// </summary>
        /// <returns></returns>
        public static string VirtualFolderName()
        {
            if (HttpContext.Current == null) return String.Empty;

            string folderName = HttpContext.Current.Items["VirtualFolderName"] as string;

            if (folderName == null)
            {
                folderName = GetVirtualFolderName();
                if (folderName != null)
                    HttpContext.Current.Items["VirtualFolderName"] = folderName;
            }
            return folderName;

            
        }

        private static string GetVirtualFolderName()
        {
            if (HttpContext.Current == null) return String.Empty;

            // find first level folder name
            // after site root
            string folderName = string.Empty;

            string requestPath =
                HttpContext.Current.Request.RawUrl.Replace("https://", string.Empty).Replace("http://", string.Empty);

            if (requestPath == "/") return folderName;

            if (
                (requestPath.IndexOf("/") > -1)
                && (requestPath.LastIndexOf("/") > requestPath.IndexOf("/"))
                )
            {
                requestPath = requestPath.Substring(requestPath.IndexOf("/") + 1, requestPath.Length - 1);

                if (requestPath.IndexOf("/") > -1)
                {
                    folderName = requestPath.Substring(0, requestPath.IndexOf("/"));

                    try
                    {
                        if (!SiteFolder.Exists(folderName))
                        {
                            folderName = string.Empty;
                        }
                    }
                    catch (DbException)
                    {
                        folderName = string.Empty;
                    }
                    catch (InvalidOperationException)
                    {
                        // occurs when db tables and procs haven't been created yet
                        // in MS SQL
                        folderName = string.Empty;
                    }
                }
            }

            return folderName;
        }
    }
}
