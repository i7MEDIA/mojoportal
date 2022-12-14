// ApplicationGuid:		    9e58fcda-90de-4ed7-abc7-12f096f5c58f
using System;
using System.Globalization;
using System.IO;
using System.Web;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using Resources;
using log4net;


namespace mojoPortal.Web.GalleryUI
{
	public partial class FolderGalleryModule : SiteModuleControl
	{
		protected static readonly ILog log = LogManager.GetLogger(typeof(FolderGalleryModule));

		protected bool ShowPermaLinksSetting = false;
		protected bool ShowMetaDetailsSetting = false;
		private FolderGalleryConfiguration config = new FolderGalleryConfiguration();

		private mojoBasePage basePage = null;
		private bool useMobileTemplate = false;

		#region OnInit



		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Load += new EventHandler(Page_Load);

			config = new FolderGalleryConfiguration(Settings);

			string pathToGallery = string.Empty;

			if (config.GalleryRootFolder.Length > 0)
			{
				pathToGallery = config.GalleryRootFolder;
			}
			else
			{
				pathToGallery = GetDefaultGalleryPath();

				if (pathToGallery.Length > 0)
				{
					CreateDefaultFolderSetting(pathToGallery);
				}
			}

			try
			{
				if (!Directory.Exists(Server.MapPath(pathToGallery)))
				{
					Directory.CreateDirectory(Server.MapPath(pathToGallery));
				}
			}
			catch (IOException ex)
			{
				log.Error(ex);
			}
			catch (HttpException ex)
			{
				log.Error(ex);
				//thrown at Server.MapPath if the path is not a valid virtual path
				pathToGallery = GetDefaultGalleryPath();
			}

			ShowPermaLinksSetting = config.ShowPermaLinks;
			ShowMetaDetailsSetting = config.ShowMetaDetails;

			if (Page is mojoBasePage)
			{
				basePage = Page as mojoBasePage;
				useMobileTemplate = basePage.UseMobileSkin;
			}

			if (useMobileTemplate)
			{
				this.Album1.Visible = false;
				this.Album2.Visible = true;
				this.Album2.GalleryRootPath = pathToGallery;
				this.Album2.DoSetup();
			}
			else
			{
				this.Album1.GalleryRootPath = pathToGallery;
				this.Album1.DoSetup();
			}
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			LoadSettings();
			PopulateLabels();
			PopulateControls();
		}

		private void PopulateControls()
		{
			TitleControl.EditUrl = SiteRoot + "/FolderGallery/Edit.aspx";

			TitleControl.Visible = !this.RenderInWebPartMode;
			if (this.ModuleConfiguration != null)
			{
				this.Title = this.ModuleConfiguration.ModuleTitle;
				this.Description = this.ModuleConfiguration.FeatureName;
			}
		}

		void btnMakeActive_Click(object sender, EventArgs e)
		{
			if (this.ModuleConfiguration == null)
			{
				return;
			}

			ModuleSettings.UpdateModuleSetting(
				this.ModuleConfiguration.ModuleGuid,
				this.ModuleConfiguration.ModuleId,
				"FolderGalleryRootFolder",
				Album1.Path);
		}

		private void CreateDefaultFolderSetting(string pathToGallery)
		{
			if (this.ModuleConfiguration == null)
			{
				return;
			}

			ModuleSettings.CreateModuleSetting(
				this.ModuleConfiguration.ModuleGuid,
				this.ModuleConfiguration.ModuleId,
				"FolderGalleryRootFolder",
				pathToGallery,
				"TextBox",
				string.Empty,
				string.Empty,
				string.Empty,
				100);
		}

		private string GetDefaultGalleryPath()
		{
			if (siteSettings == null)
			{
				return string.Empty;
			}

			string basePath = string.Format(CultureInfo.InvariantCulture,
				FolderGalleryConfiguration.BasePathFormat,
				siteSettings.SiteId);

			if (config.GalleryRootFolder.Length > 0)
			{
				if (!config.GalleryRootFolder.StartsWith(basePath))
				{
					// legacy path
					basePath = "~/Data/Sites/"
						+ siteSettings.SiteId.ToInvariantString()
						+ "/FolderGalleries/";

				}
			}

			return basePath;
		}


		private void PopulateLabels()
		{
			TitleControl.EditText = FolderGalleryResources.FolderGalleryEditLink;
		}

		private void LoadSettings()
		{
			if (config.CustomCssClass.Length > 0)
			{
				pnlOuterWrap.SetOrAppendCss(config.CustomCssClass);
			}
		}
	}
}
