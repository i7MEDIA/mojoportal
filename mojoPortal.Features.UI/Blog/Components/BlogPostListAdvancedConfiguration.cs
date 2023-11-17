using System;
using System.Collections;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.BlogUI
{
	public class BlogPostListAdvancedConfiguration
	{
		//private static readonly ILog log = LogManager.GetLogger(typeof(BlogPostListAdvancedConfiguration));
		//private Module module;
		private readonly Hashtable settings;
		private readonly int siteId = -1;

		#region contstructors

		public BlogPostListAdvancedConfiguration() { }

		public BlogPostListAdvancedConfiguration(Hashtable settingsHash)
		{
			LoadSettings(settingsHash);
		}

		public BlogPostListAdvancedConfiguration(Module module)
		{
			if (module != null)
			{
				//this.module = module;
				siteId = module.SiteId;
				FeatureGuid = module.FeatureGuid;
				settings = ModuleSettings.GetModuleSettings(module.ModuleId);

				if (siteId < 1)
				{
					siteId = CacheHelper.GetCurrentSiteSettings().SiteId;
				}

				LoadSettings(settings);
			}
		}

		private void LoadSettings(Hashtable settings)
		{
			if (settings == null)
			{
				throw new ArgumentException("must pass in a hashtable of settings");
			}

			if (settings.Contains("PostListLayout"))
			{
				string layoutString = settings["PostListLayout"].ToString();

				if (!string.IsNullOrWhiteSpace(layoutString))
				{
					Layout = settings["PostListLayout"].ToString();
				}
			}

			if (settings.Contains("PostListItemsPerPage"))
			{
				ItemsPerPage = Convert.ToInt32(settings["PostListItemsPerPage"]);
			}

			if (settings.Contains("PostListBlogInstance"))
			{
				string bid = settings["PostListBlogInstance"].ToString();

				if (!string.IsNullOrWhiteSpace(bid))
				{
					BlogModuleId = Convert.ToInt32(settings["PostListBlogInstance"]);
				}
			}

			if (settings.Contains("PostListBlogInstanceCategories"))
			{
				string layoutString = settings["PostListBlogInstanceCategories"].ToString();

				if (!string.IsNullOrWhiteSpace(layoutString))
				{
					Categories = settings["PostListBlogInstanceCategories"].ToString();
				}
			}

			if (settings.Contains("ExtraCssClassSetting"))
			{
				InstanceCssClass = settings["ExtraCssClassSetting"].ToString();
			}
		}

		#endregion

		#region Properties

		public Guid FeatureGuid { get; private set; } = Guid.Parse("031eb6a0-acf5-4559-8356-af6049d57ac1");
		public string Layout { get; set; } = "_BlogPostList";
		public int ItemsPerPage { get; set; } = 4;
		public int BlogModuleId { get; set; } = -1;
		public string Categories { get; set; } = string.Empty; //using string because we'll support multiple categories "soon"
		public string InstanceCssClass { get; set; } = string.Empty;

		#endregion
	}
}