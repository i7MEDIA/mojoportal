///	Author:				i7MEDIA
///	Created:			2017-05-11
///	Last Modified:		2017-08-23
///		
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.
/// 
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using System;
using System.Collections;

namespace mojoPortal.Web.BlogUI
{
	public class BlogPostListAdvancedConfiguration
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(BlogPostListAdvancedConfiguration));
		private Module module;
		private Hashtable settings;
		private int siteId = -1;

		#region contstructors

		public BlogPostListAdvancedConfiguration()
		{ }

		public BlogPostListAdvancedConfiguration(Hashtable settingsHash)
		{
			LoadSettings(settingsHash);
		}

		public BlogPostListAdvancedConfiguration(Module module)
		{
			if (module != null)
			{
				this.module = module;
				siteId = module.SiteId;
				featureGuid = module.FeatureGuid;
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

				if (!String.IsNullOrWhiteSpace(layoutString))
				{
					layout = settings["PostListLayout"].ToString();
				}
			}

			if (settings.Contains("PostListItemsPerPage"))
			{
				itemsPerPage = Convert.ToInt32(settings["PostListItemsPerPage"]);
			}

			if (settings.Contains("PostListBlogInstance"))
			{
				string bid = settings["PostListBlogInstance"].ToString();

				if (!string.IsNullOrWhiteSpace(bid))
				{
					blogModuleId = Convert.ToInt32(settings["PostListBlogInstance"]);
				}
			}

			if (settings.Contains("ExtraCssClassSetting"))
			{
				instanceCssClass = settings["ExtraCssClassSetting"].ToString();
			}
		}

		#endregion

		#region Properties

		private Guid featureGuid = Guid.Parse("031eb6a0-acf5-4559-8356-af6049d57ac1");
		public Guid FeatureGuid
		{
			get { return featureGuid; }
		}

		private string layout = "_BlogPostList";
		public string Layout
		{
			get { return layout; }
			set { layout = value; }
		}

		private int itemsPerPage = 4;
		public int ItemsPerPage
		{
			get => itemsPerPage;
			set => itemsPerPage = value;
		}

		private int blogModuleId = -1;
		public int BlogModuleId
		{
			get { return blogModuleId; }
			set { blogModuleId = value; }
		}

		private string instanceCssClass = string.Empty;
		public string InstanceCssClass
		{
			get { return instanceCssClass; }
			set { instanceCssClass = value; }
		}

		#endregion
	}
}