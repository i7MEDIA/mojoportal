// Author:					
// Created:				    2007-09-01
// Last Modified:			2007-09-01
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Web;

namespace mojoPortal.Business.WebHelpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class ResourceFile
    {
        public static string GetResourceString(
            string resourceFile,
            string resourceKey)
        {
            if (HttpContext.Current == null) return resourceKey;
            if (resourceFile.Length == 0) resourceFile = "Resource";

            try
            {
                object resource = HttpContext.GetGlobalResourceObject(
                    resourceFile, resourceKey);

                if (resource != null) return resource.ToString();
            }
            catch (System.Resources.MissingManifestResourceException) { }

            return resourceKey;

        }
    }
}
