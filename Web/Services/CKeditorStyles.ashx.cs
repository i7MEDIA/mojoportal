using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Serializers.Newtonsoft;
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
	public class CKeditorStyles : IHttpHandler
    {

        private SiteSettings siteSettings = null;
        private string siteRoot = string.Empty;
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
			
            siteRoot = SiteUtils.GetNavigationSiteRoot();
			skinRootFolder = SiteUtils.GetSiteSkinFolderPath();
			currentSkin = siteSettings.Skin;
            
			var currentPage = CacheHelper.GetCurrentPage();

			if (currentPage != null && !string.IsNullOrEmpty(currentPage.Skin))
			{
				currentSkin = currentPage.Skin;
			}

			skinStylesFile = new FileInfo($"{skinRootFolder + currentSkin}\\config\\editorstyles.json");
			systemStylesFile = new FileInfo(HttpContext.Current.Server.MapPath("~/data/style/editorstyles.json"));

			RenderJsonList(context);
        }

        private void RenderJsonList(HttpContext context)
        {
			context.Response.ContentType = "text/javascript";
            context.Response.ContentEncoding = new UTF8Encoding();

			List<CkEditorStyle> styles = new List<CkEditorStyle>();

			if (WebConfigSettings.AddSystemStyleTemplatesAboveSiteTemplates && systemStylesFile.Exists)
            {
				styles.AddRange(GetEditorStyles(systemStylesFile));
			}

            using (IDataReader reader = ContentStyle.GetAllActive(siteSettings.SiteGuid))
            {
                while (reader.Read())
                {
					styles.Add(new CkEditorStyle
					{
						Name = reader["Name"].ToString(),
						Element = new List<string> { reader["Element"].ToString() },
						Attributes = new Dictionary<string, string>() { { "class", reader["CssClass"].ToString() } }
					});
                }
            }

            if (skinStylesFile.Exists)
            {
				styles.AddRange(GetEditorStyles(skinStylesFile));
			}

			if (WebConfigSettings.AddSystemStyleTemplatesBelowSiteTemplates && systemStylesFile.Exists)
            {
				styles.AddRange(GetEditorStyles(systemStylesFile));
			}

			var json = JsonConvert.SerializeObject(styles, Formatting.None);

			context.Response.Write($"try{{CKEDITOR.addStylesSet('mojo',{json});}}catch(err){{}}");
        }

		public List<CkEditorStyle> GetEditorStyles(FileInfo file)
		{
			var styles = new List<CkEditorStyle>();
			if (file.Exists)
			{
				var bar = File.ReadAllText(file.FullName);
				
				styles = JsonConvert.DeserializeObject<List<CkEditorStyle>>(bar);
				
			}
			return styles;
		}

        public bool IsReusable { get { return false; } }
    }



	public class CkEditorStyle
	{
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "element")]
		[JsonConverter(typeof(SingleOrArrayConverter<string>))] 
		public List<string> Element { get; set; }
		
		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		[JsonProperty(PropertyName = "widget")]
		public string Widget { get; set; }

		[JsonProperty(PropertyName = "group")]

		public List<string> Group { get; set; }

		[JsonProperty(PropertyName = "attributes")]
		public Dictionary<string, string> Attributes { get; set; }

		public bool ShouldSerializeAttributes()
		{
			return Attributes != null;
		}

		public bool ShouldSerializeElement()
		{
			return Element != null;
		}
		
		public bool ShouldSerializeType()
		{
			return Type != null;
		}

		public bool ShouldSerializeWidget()
		{
			return Widget != null;
		}

		public bool ShouldSerializeGroup()
		{
			return Group != null;
		}
	}
}
