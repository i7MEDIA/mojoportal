using System.Web;

namespace mojoPortal.Business.WebHelpers
{
    public static class ResourceFile
    {
        public static string GetResourceString(string resourceFile, string resourceKey)
        {
            if (HttpContext.Current == null)
            {
                return resourceKey;
            }

            if (resourceFile.Length == 0)
            {
                resourceFile = "Resource";
            }

            try
            {
                object resource = HttpContext.GetGlobalResourceObject(resourceFile, resourceKey);

                if (resource != null)
                {
                    return resource.ToString();
                }
            }
            catch (System.Resources.MissingManifestResourceException) { }

            return resourceKey;

        }
    }
}
