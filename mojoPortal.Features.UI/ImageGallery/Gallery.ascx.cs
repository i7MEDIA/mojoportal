using mojoPortal.Business;
using mojoPortal.FileSystem;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace mojoPortal.Web.GalleryUI
{
	public partial class GalleryControl : SiteModuleControl
	{
		#region Properties 

		protected string EditContentImage = WebConfigSettings.EditContentImage;
		private Literal imageLink;
		private Gallery gallery;
		private GalleryConfiguration config = new GalleryConfiguration();
		private string imageFolderPath;
		private int thumbsPerPage = 999;
		private int itemId = -1;
		private string ViewImagePage = "GalleryBrowse.aspx";
		private int totalRows = 0;
		private string baseUrl = string.Empty;
		protected string thumnailBaseUrl = string.Empty;
		protected string webSizeBaseUrl = string.Empty;
		protected string fullSizeBaseUrl = string.Empty;
		protected string imageBaseUrl = string.Empty;
		private IFileSystem fileSystem = null;
		protected bool useViewState = true;

		protected int PageNumber
		{
			get
			{
				object o = ViewState["PageNumber"];

				if (o != null)
				{
					return Convert.ToInt32(o);
				}

				return 1;
			}
			set
			{
				ViewState["PageNumber"] = value;
			}
		}

		protected int TotalPages
		{
			get
			{
				object o = ViewState["TotalPages"];

				if (o != null)
				{
					return Convert.ToInt32(o);
				}

				return 1;
			}
			set
			{
				ViewState["TotalPages"] = value;
			}
		}

		protected bool UseLightboxMode
		{
			get
			{
				object o = ViewState["UseLightboxMode"];

				if (o != null)
				{
					return Convert.ToBoolean(o);
				}

				return true;
			}
			set
			{
				ViewState["UseLightboxMode"] = value;
			}
		}

		protected bool UseCompactMode
		{
			get
			{
				object o = ViewState["UseCompactMode"];

				if (o != null)
				{
					return Convert.ToBoolean(o);
				}

				return false;
			}
			set
			{
				ViewState["UseCompactMode"] = value;
			}
		}

		#endregion


		protected void Page_Load(object sender, EventArgs e)
		{
			LoadSettings();

			if (!Page.IsPostBack)
			{
				PopulateControls();
			}
		}


		private void PopulateControls()
		{
			if (config.UseNivoSlider)
			{
				SetupNivo();

				return;
			}

			pager.CurrentIndex = 1;

			BindRepeater();
			BindImage();
		}


		private void SetupNivo()
		{
			upGallery.Visible = false;
			pnlNivoWrapper.Visible = true;
			pnlNivoWrapper.CssClass = "slider-wrapper " + displaySettings.NivoTheme;

			using (IDataReader reader = gallery.GetAllImages())
			{
				rptNivo.DataSource = reader;
				rptNivo.DataBind();
			}

			SetupNivoScripts();
		}


		private void SetupNivoScripts()
		{
			if (Page is mojoBasePage)
			{
				mojoBasePage basePage = Page as mojoBasePage;
				basePage.ScriptConfig.IncludeNivoSlider = true;
			}
			else
			{
				ScriptManager.RegisterClientScriptBlock(
					this,
					typeof(Page),
					"nivoslidermain",
					$"\n<script src=\"{Page.ResolveUrl("~/ClientScript/jqmojo/jquery.nivo.slider.pack3-2.js")}\"></script>",
					false
				);
			}

			string nivoInstanceScript = $@"
$(document).ready(function() {{
	$('#{pnlNivoInner.ClientID}').nivoSlider({{{displaySettings.NivoConfig}}});
}});";

			ScriptManager.RegisterStartupScript(
				this,
				GetType(),
				"startSlider" + pnlNivoInner.ClientID,
				nivoInstanceScript,
				true
			);
		}


		protected string FormatNivoImage(string webImageFile, string fullSizeImageFile, string caption, int itemID)
		{
			string imageUrl;

			if (displaySettings.NivoUseFullSizeImages) // false by default
			{
				imageUrl = Page.ResolveUrl(fullSizeBaseUrl + fullSizeImageFile);
			}
			else
			{
				imageUrl = Page.ResolveUrl(webSizeBaseUrl + webImageFile);
			}

			if (displaySettings.NivoLinkToFullSize)
			{
				return $@"
<a class=""cblink"" href=""{Page.ResolveUrl(fullSizeBaseUrl + fullSizeImageFile)}"">
	<img src=""{imageUrl}"" alt=""{caption.HtmlEscapeQuotes()}"" title=""{caption.HtmlEscapeQuotes()}"" />
</a>";
			}

			return $"\n<img src='{imageUrl}' alt='{caption.HtmlEscapeQuotes()}' title='{caption.HtmlEscapeQuotes()}' />";
		}


		private void BindRepeater()
		{
			DataTable dt = gallery.GetThumbsByPage(PageNumber, thumbsPerPage);

			if (dt.Rows.Count > 0)
			{
				TotalPages = Convert.ToInt32(dt.Rows[0]["TotalPages"]);
				itemId = Convert.ToInt32(dt.Rows[0]["ItemID"]);
				totalRows = thumbsPerPage * TotalPages;
			}

			//this handles issue: when redirected back to page from edit page
			//if you deleted the last image on the page an error occurs
			//so decrement the pageNumber
			while (PageNumber > TotalPages)
			{
				PageNumber -= 1;

				dt = gallery.GetThumbsByPage(PageNumber, thumbsPerPage);

				if (dt.Rows.Count > 0)
				{
					TotalPages = Convert.ToInt32(dt.Rows[0]["TotalPages"]);
					itemId = Convert.ToInt32(dt.Rows[0]["ItemID"]);
				}
			}

			if (TotalPages > 1)
			{
				//if (RenderInWebPartMode)
				//{
				//	if (totalRows > thumbsPerPage)
				//	{
				//		Literal moreLink = new Literal
				//		{
				//			Text = $"<a href='{SiteRoot}/{ViewImagePage}?ItemID={itemId.ToInvariantString()}&amp;mid={ModuleId.ToInvariantString()}&amp;pageid={PageId.ToInvariantString()}&amp;pagenumber={PageNumber.ToInvariantString()}'>{GalleryResources.GalleryWebPartMoreLink}</a>"
				//		};

				//		pnlGallery.Controls.Add(moreLink);
				//		pager.Visible = false;
				//	}
				//}
				//else
				//{
					pager.ShowFirstLast = true;
					pager.PageSize = thumbsPerPage;
					pager.PageCount = TotalPages;
				//}
			}
			else
			{
				pager.Visible = false;
			}

			if (UseLightboxMode)
			{
				SetupColorbox();
			}

			rptGallery.DataSource = dt;
			rptGallery.DataBind();
		}


		private void SetupColorbox()
		{
			string ifSlideshow()
			{
				var result = string.Empty;

				if (config.ColorBoxUseSlideshow)
				{
					if (config.ColorBoxSlideShowStartAuto)
					{
						result += "open:true,";
					}

					result += $"slideshowSpeed: '{config.ColorBoxSlideshowSpeed}',";

					if (config.ColorBoxSlideshowAuto != GalleryConfiguration.ColorBoxDefaultUseSlideShow)
					{
						result += $"slideshowAuto: '{config.ColorBoxSlideshowAuto.ToString().ToLower()}',";
					}

					result += $"slideshowStart: '{GalleryResources.StartSlideShow}',";
					result += $"slideshowStop: '{GalleryResources.StopSlideShow}',";
				}

				return result;
			}

			var script = $@"
$(document).ready(function() {{
	$('.cbg{ModuleId.ToInvariantString()}').colorbox({{
		rel: 'cbg{ModuleId.ToInvariantString()}',
		current: '{GalleryResources.ImageCountJsFormat}',
		previous: '{GalleryResources.Previous}',
		next: '{GalleryResources.Next}',
		close: '{GalleryResources.Close}',
		{(config.ColorBoxTransition != GalleryConfiguration.ColorBoxDefaultTransition ? $"transition: '{config.ColorBoxTransition}'," : string.Empty)}
		{(config.ColorBoxTransitionSpeed != GalleryConfiguration.ColorBoxDefaultTransitionSpeed ? $"speed: '{config.ColorBoxTransitionSpeed}'," : string.Empty)}
		opacity: '{config.ColorBoxOpacity}',
		{(config.ColorBoxUseSlideshow != GalleryConfiguration.ColorBoxDefaultUseSlideShow ? $"slideshow: '{config.ColorBoxUseSlideshow.ToString().ToLower()}'," : string.Empty)}
		{ifSlideshow()}
		maxWidth: '95%',
		maxHeight: '95%'
	}});
}});";

			ScriptManager.RegisterStartupScript(
				Page, typeof(Page),
				"img" + ModuleId.ToInvariantString(),
				$"\n<script>{script}\n</script>",
				false
			);

			// make it responsive
			//https://github.com/jackmoore/colorbox/issues/158

			var responsiveScript = @"
(function() {
	let resizeTimer;

	function onResize() {
		if (resizeTimer) {
			clearTimeout(resizeTimer);
		}

		function onTimeout() {
			if ($('#cboxOverlay').is(':visible')) {
				$.colorbox.load(true);
			}
		}

		resizeTimer = setTimeout(onTimeout, 300);
	}

	function onOrientationChange() {
		if ($('#cboxOverlay').is(':visible')) {
			$.colorbox.load(true);
		}
	}

	window.addEventListener('resize', onResize);
	window.addEventListener('orientationchange', onOrientationChange);
})();";

			ScriptManager.RegisterStartupScript(
				Page,
				typeof(Page),
				"colorboxresizer",
				$"\n<script>{responsiveScript}\n</script>",
				false
			);
		}


		private void BindImage()
		{
			if (!UseCompactMode)
			{
				return;
			}

			if (itemId == -1)
			{
				return;
			}

			imageLink = new Literal();

			var galleryImage = new GalleryImage(ModuleId, itemId);

			imageLink.Text = $@"
<a onclick=""window.open(this.href, '_blank'); return false;"" title=""{Server.HtmlEncode(GalleryResources.GalleryWebImageAltText).HtmlEscapeQuotes()}"" href=""{imageBaseUrl + Page.ResolveUrl(fullSizeBaseUrl + galleryImage.ImageFile)}"">
	<img src=""{Page.ResolveUrl(webSizeBaseUrl + galleryImage.WebImageFile)}"" alt=""{Server.HtmlEncode(GalleryResources.GalleryWebImageAltText).HtmlEscapeQuotes()}"" />
</a>";

			pnlGallery.Controls.Clear();
			pnlGallery.Controls.Add(imageLink);
			lblCaption.Text = Page.Server.HtmlEncode(galleryImage.Caption);
			lblDescription.Text = galleryImage.Description;

			if (config.ShowTechnicalData && galleryImage.MetaDataXml.Length > 0)
			{
				xmlMeta.DocumentContent = galleryImage.MetaDataXml;
				xmlMeta.TransformSource = HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot() + "/ImageGallery/GalleryMetaData.xsl");
			}
		}


		void rptGallery_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "setimage")
			{
				itemId = Convert.ToInt32(e.CommandArgument);
				BindImage();
				BindRepeater();
				upGallery.Update();
			}
		}


		protected void pager_Command(object sender, CommandEventArgs e)
		{
			PageNumber = Convert.ToInt32(e.CommandArgument);
			pager.CurrentIndex = PageNumber;

			BindRepeater();
			BindImage();

			upGallery.Update();
		}


		protected string GetThumnailImageLink(
			string itemId,
			string thumbnailFile,
			string webImageFile,
			string caption
		)
		{
			var link = string.Empty;

			if (UseLightboxMode)
			{
				link = $"<a class='cbg{ModuleId.ToInvariantString()}' title=\"{caption.HtmlEscapeQuotes()}\" href=\"{Page.ResolveUrl(webSizeBaseUrl + webImageFile)}\">{GetThumnailMarkup(thumbnailFile, caption)}</a>";
			}

			return link;
		}


		protected string GetThumnailUrl(string thumbnailFile)
		{
			return Page.ResolveUrl(thumnailBaseUrl + thumbnailFile);
		}


		protected string GetThumnailMarkup(string thumbnailFile, string caption)
		{
			if (caption == null)
			{
				caption = string.Empty;
			}

			return $"<img src=\"{Page.ResolveUrl(thumnailBaseUrl + thumbnailFile)}\" alt=\"{caption.HtmlEscapeQuotes()}\" />";
		}


		private void LoadSettings()
		{
			gallery = new Gallery(ModuleId);

			try
			{
				// this keeps the action from changing during ajax postback in folder based sites
				SiteUtils.SetFormAction(Page, Request.RawUrl);
			}
			catch (MissingMethodException)
			{
				//this method was introduced in .NET 3.5 SP1
			}

			Title1.EditUrl = SiteRoot + "/ImageGallery/EditImage.aspx";
			Title1.EditText = GalleryResources.GalleryAddImageLabel;
			Title1.Visible = !RenderInWebPartMode;


			config = new GalleryConfiguration(Settings);

			if (IsEditable)
			{
				Title1.LiteralExtraMarkup = $"&nbsp;<a href='{SiteRoot}/ImageGallery/BulkUpload.aspx?pageid={PageId.ToInvariantString()}&amp;mid={ModuleId.ToInvariantString()}' class='ModuleEditLink' title='{GalleryResources.BulkUploadLink}'>{GalleryResources.BulkUploadLink}</a>";
			}

			if (ModuleConfiguration != null)
			{
				Title = ModuleConfiguration.ModuleTitle;
				Description = ModuleConfiguration.FeatureName;
			}

			if (config.CustomCssClass.Length > 0)
			{
				pnlOuterWrap.SetOrAppendCss(config.CustomCssClass);
			}

			if (WebConfigSettings.ImageGalleryUseMediaFolder)
			{
				baseUrl = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/media/GalleryImages/{ModuleId.ToInvariantString()}/";
			}
			else
			{
				baseUrl = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/GalleryImages/{ModuleId.ToInvariantString()}/";
			}

			thumnailBaseUrl = baseUrl + "Thumbnails/";
			webSizeBaseUrl = baseUrl + "WebImages/";
			fullSizeBaseUrl = baseUrl + "FullSizeImages/";

			imageFolderPath = HttpContext.Current.Server.MapPath(baseUrl);
			thumbsPerPage = config.ThumbsPerPage;
			UseCompactMode = config.UseCompactMode;

			if (UseCompactMode)
			{
				UseLightboxMode = false;
				pnlImageDetails.Visible = false;
			}
			else
			{
				UseLightboxMode = true;
				useViewState = false;
				rptGallery.EnableViewState = false;
			}

			if (UseLightboxMode)
			{
				if (Page is mojoBasePage basePage)
				{
					basePage.ScriptConfig.IncludeColorBox = true;
				}
			}

			imageBaseUrl = ImageSiteRoot;

			FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];

			if (p == null)
			{
				return;
			}

			fileSystem = p.GetFileSystem();

			if (fileSystem != null)
			{
				imageBaseUrl = fileSystem.FileBaseUrl;
			}
		}


		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += new EventHandler(Page_Load);
			pager.Command += new CommandEventHandler(pager_Command);
			rptGallery.ItemCommand += new RepeaterCommandEventHandler(rptGallery_ItemCommand);
		}
	}
}
