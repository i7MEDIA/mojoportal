///	Author:				i7MEDIA
///	Created:			2017-05-12
///	Last Modified:		2017-05-12
///		
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using System;

namespace mojoPortal.Web.BlogUI
{

	public partial class PostListModule : SiteModuleControl
	{
		protected BlogConfiguration blogConfig = new BlogConfiguration();
		protected BlogPostListAdvancedConfiguration config = new BlogPostListAdvancedConfiguration();

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += new EventHandler(Page_Load);
			EnableViewState = false;
		}

		protected virtual void Page_Load(object sender, EventArgs e)
		{
			LoadSettings();
			PopulateControls();
		}

		private void PopulateControls()
		{
			postList.ModuleId = ModuleId;
			postList.PageId = PageId;
			postList.IsEditable = IsEditable;
			postList.BlogConfig = blogConfig;
			postList.Config = config;
			postList.SiteRoot = SiteRoot;
			postList.ImageSiteRoot = ImageSiteRoot;
		}

		protected virtual void LoadSettings()
		{
			config = new BlogPostListAdvancedConfiguration(Settings);

			blogConfig = new BlogConfiguration(ModuleSettings.GetModuleSettings(config.BlogModuleId));

			if (config.InstanceCssClass.Length > 0)
			{
				pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass);
			}

			if (ModuleConfiguration != null)
			{
				Title = ModuleConfiguration.ModuleTitle;
				Description = ModuleConfiguration.FeatureName;
			}
		}
	}
}
