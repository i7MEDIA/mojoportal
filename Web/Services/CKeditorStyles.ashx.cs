using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Serializers.Newtonsoft;
using mojoPortal.Web.Controls.Editors;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;

namespace mojoPortal.Web.Services
{
	/// <summary>
	/// Returns styles JS for CKeditor
	/// </summary>
	public class CkEditorStyles : IHttpHandler
    {

        private SiteSettings siteSettings = null;
        //private string siteRoot = string.Empty;
        private string skinRootFolder = string.Empty;
		private string currentSkin = string.Empty;
        private FileInfo skinStylesFile = null;
        private FileInfo systemStylesFile = null;

		public void ProcessRequest(HttpContext context)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
			
            if (siteSettings == null)
			{
				return;
			}
			
            //siteRoot = SiteUtils.GetNavigationSiteRoot();
			skinRootFolder = SiteUtils.GetSiteSkinFolderPath();
			currentSkin = siteSettings.Skin;
            
			var currentPage = CacheHelper.GetCurrentPage();

			if (currentPage != null && !string.IsNullOrEmpty(currentPage.Skin))
			{
				currentSkin = currentPage.Skin;
			}

			skinStylesFile = new($"{skinRootFolder + currentSkin}\\config\\editorstyles.json");
			systemStylesFile = new(HttpContext.Current.Server.MapPath("~/data/style/editorstyles.json"));

			RenderJsonList(context);
        }

        private void RenderJsonList(HttpContext context)
        {
			context.Response.ContentType = "text/javascript";
            context.Response.ContentEncoding = new UTF8Encoding();

			List<EditorStyle> styles = new();

			if (WebConfigSettings.AddSystemStyleTemplatesAboveSiteTemplates && systemStylesFile.Exists)
            {
				styles.AddRange(EditorStyle.GetEditorStyles(systemStylesFile));
			}

            using (IDataReader reader = ContentStyle.GetAllActive(siteSettings.SiteGuid))
            {
                while (reader.Read())
                {
					styles.Add(new EditorStyle
					{
						Name = reader["Name"].ToString(),
						Element = new List<string> { reader["Element"].ToString() },
						Attributes = new Dictionary<string, string>() { { "class", reader["CssClass"].ToString() } }
					});
                }
            }

            if (skinStylesFile.Exists)
            {
				styles.AddRange(EditorStyle.GetEditorStyles(skinStylesFile));
			}

			if (WebConfigSettings.AddSystemStyleTemplatesBelowSiteTemplates && systemStylesFile.Exists)
            {
				styles.AddRange(EditorStyle.GetEditorStyles(systemStylesFile));
			}

			var json = JsonConvert.SerializeObject(styles, Formatting.None);

			context.Response.Write($"try{{CKEDITOR.addStylesSet('mojo',{json});}}catch(err){{}}");
        }

        public bool IsReusable { get { return false; } }
    }
}
