using Microsoft.Web.Preview.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Configuration;
using mojoPortal.Web.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;

namespace mojoPortal.Web.Services
{
	/// <summary>
	/// Returns content templates in a json format consumable by CKeditor
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CKeditorTemplates : IHttpHandler
	{

		private SiteSettings siteSettings = null;
		private string siteRoot = string.Empty;
		private string skinRootFolder = string.Empty;
		private string currentSkin = string.Empty;
		private FileInfo skinTemplatesFile = null;
		private FileInfo systemTemplatesFile = null;

		public void ProcessRequest(HttpContext context)
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings == null)
			{

				return;
			}

			var export = context.Request.QueryString["e"] != null && context.Request.QueryString["e"].ToLower() == "true";

			siteRoot = SiteUtils.GetNavigationSiteRoot();
			skinRootFolder = SiteUtils.GetSiteSkinFolderPath();
			currentSkin = siteSettings.Skin;

			if (HttpContext.Current.Request.Params.Get("skin") != null)
			{
				currentSkin = SiteUtils.SanitizeSkinParam(HttpContext.Current.Request.Params.Get("skin"));
			}

			//var currentPage = CacheHelper.GetCurrentPage();

			//if (currentPage != null && !string.IsNullOrEmpty(currentPage.Skin))
			//{
			//	currentSkin = currentPage.Skin;
			//}

			skinTemplatesFile = new FileInfo($"{skinRootFolder + currentSkin}\\config\\editortemplates.json");
			systemTemplatesFile = new FileInfo(HttpContext.Current.Server.MapPath("~/data/style/editortemplates.json"));

			RenderJsonList(context, export);

		}

		private void RenderJsonList(HttpContext context, bool export = false)
		{
			context.Response.ContentEncoding = new UTF8Encoding();
			context.Response.ContentType = "text/javascript";

			StringBuilder output = new StringBuilder();

			var templatesOrder = AppConfig.EditorTemplatesOrder.SplitOnCharAndTrim(',');
			List<string> templateNames = new List<string>();

			foreach (var i in templatesOrder)
			{
				var collection = new CkEditorTemplateCollection();

				switch (i)
				{
					case "site":

						collection.ImagesPath = $"{siteRoot}/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/htmltemplateimages/";

						//add site content templates to list
						foreach (var t in ContentTemplate.GetAll(siteSettings.SiteGuid))
						{
							if (!WebUser.IsInRoles(t.AllowedRoles)) { continue; }

							collection.Templates.Add(new CkEditorTemplate
							{
								Title = t.Title,
								Image = t.ImageFileName,
								Description = t.Description,
								Html = t.Body.RemoveLineBreaks()
							});
						}
						if (collection.Templates.Count > 0)
						{

							var siteJson = JsonConvert.SerializeObject(collection, Formatting.None);

							if (export)
							{
								context.Response.ContentType = "application/octet-stream";
								context.Response.AddHeader("Content-Disposition", $"attachment; filename={siteSettings.SiteName} Content Templates.json");
								output.AppendLine(siteJson);
							}
							else
							{
								templateNames.Add("site");
								output.AppendLine($"try{{CKEDITOR.addTemplates('site',{siteJson});}}catch(err){{}}");
							}
						}
						break;
					case "system":
						if (export || !systemTemplatesFile.Exists) break;

						collection = GetEditorTemplateCollection(systemTemplatesFile);

						if (collection.Templates.Count > 0)
						{
							templateNames.Add("system");
							var sysJson = JsonConvert.SerializeObject(collection, Formatting.None);
							output.AppendLine($"try{{CKEDITOR.addTemplates('system',{sysJson});}}catch(err){{}}");
						}
						break;
					case "skin":
						if (export || !skinTemplatesFile.Exists) break;

						collection = GetEditorTemplateCollection(skinTemplatesFile);

						if (collection.ImagesPath.Contains("$skinpath$"))
						{
							collection.ImagesPath = collection.ImagesPath.Replace("$skinpath$", SiteUtils.DetermineSkinBaseUrl(currentSkin));
						}

						if (collection.Templates.Count > 0)
						{
							templateNames.Add("skin");
							var skinJson = JsonConvert.SerializeObject(collection, Formatting.None);
							output.AppendLine($"try{{CKEDITOR.addTemplates('skin',{skinJson});}}catch(err){{}}");
						}
						break;
					default:
						break;
				}
			}

			if (templateNames.Count > 0)
			{
				context.Response.Write($"CKEDITOR.config.templates = '{string.Join(",", templateNames)}';\r\n{output}");
			}
			else if (export)
			{
				context.Response.Write(output.ToString());
			}
		}

		public CkEditorTemplateCollection GetEditorTemplateCollection(FileInfo file)
		{
			var collection = new CkEditorTemplateCollection();
			if (file.Exists)
			{
				var contents = File.ReadAllText(file.FullName);
				collection = JsonConvert.DeserializeObject<CkEditorTemplateCollection>(contents);
			}
			return collection;
		}

		public bool IsReusable { get { return false; } }

	}

	public class CkEditorTemplateCollection
	{
		[JsonProperty(PropertyName = "imagesPath")]
		public string ImagesPath { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "templates")]
		public List<CkEditorTemplate> Templates { get; set; }

		public CkEditorTemplateCollection()
		{
			Templates = new List<CkEditorTemplate>();
		}
	}

	public class CkEditorTemplate
	{
		[JsonProperty(PropertyName = "title")]
		public string Title { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "image")]
		public string Image { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "html")]
		public string Html { get; set; } = string.Empty;

		public bool ShouldSerializeDescription()
		{
			return Description != null;
		}
		public bool ShouldSerializeImage()
		{
			return Image != null;
		}
	}
}
