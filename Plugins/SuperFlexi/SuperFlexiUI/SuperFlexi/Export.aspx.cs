using log4net;
using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using System;
using System.Collections;
using System.Web;

namespace SuperFlexiUI
{
	public partial class Export : NonCmsBasePage
	{

		private static readonly ILog log = LogManager.GetLogger(typeof(Import));
		private Hashtable moduleSettings;
		private Module module;
		private int moduleId = -1;
		private int pageId = -1;
		protected ModuleConfiguration config = new ModuleConfiguration();


		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);

				return;
			}

			SecurityHelper.DisableBrowserCache();

			LoadParams();
			LoadSettings();

			if (!UserCanEditModule(moduleId, config.FeatureGuid) || !config.AllowExport)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);

				return;
			}

			if (SiteUtils.IsFishyPost(this))
			{
				SiteUtils.RedirectToAccessDeniedPage(this);

				return;
			}

			PopulateLabels();

			if (!IsPostBack)
			{
				txtExportName.Text = module.ModuleTitle;

				if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
				{
					hdnReturnUrl.Value = Request.UrlReferrer.ToString();
					lnkCancel.NavigateUrl = Request.UrlReferrer.ToString();
				}
			}
		}


		private void ExportBtn_Click(object sender, EventArgs e)
		{
			dynamic expando = SuperFlexiHelpers.GetExpandoForModuleItems(module, config, false);
			ExportHelper.ExportDynamicListToCSV(HttpContext.Current, expando.Items, $"export-{txtExportName.Text}.csv");
		}


		private void PopulateLabels()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, config.ExportPageTitle);
			heading.Text = config.ExportPageTitle;
			lnkCancel.Text = config.ImportPageCancelLinkText;
			litInstructions.Text = config.ExportInstructions;
		}


		private void LoadParams()
		{
			moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
			pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
		}


		private void LoadSettings()
		{
			moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

			//we want to get the module using this method because it will let the module be editable when placed on the page with a ModuleWrapper
			module = SuperFlexiHelpers.GetSuperFlexiModule(moduleId);

			if (module == null)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);

				return;
			}

			config = new ModuleConfiguration(module);
			lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
			AddClassToBody("flexi-export " + config.EditPageCssClass);
		}


		protected override void OnPreInit(EventArgs e)
		{
			AllowSkinOverride = true;
			base.OnPreInit(e);
		}


		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += new EventHandler(Page_Load);
			exportButton.Click += new EventHandler(ExportBtn_Click);
			SuppressPageMenu();
		}
	}
}
