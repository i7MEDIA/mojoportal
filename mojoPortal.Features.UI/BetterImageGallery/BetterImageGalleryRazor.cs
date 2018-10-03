using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.Components;

namespace mojoPortal.Features.UI.BetterImageGallery
{
	public class BetterImageGalleryRazor : WebControl
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(BetterImageGalleryRazor));
		protected string EditContentImage = WebConfigSettings.EditContentImage;

		private Module module = null;
		public int ModuleId { get; set; } = -1;
		public BIGConfig BigConfig { get; set; }

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			LoadSettings();

			if (module == null)
			{
				Visible = false;

				return;
			}

			if (Page.IsPostBack) return;
		}


		protected override void RenderContents(HtmlTextWriter output)
		{
			var gallery = new GalleryCore(BigConfig);
			gallery.Setup();
			var items = gallery.GetImages();

			string partialString = string.Empty;

			try
			{
				partialString = RazorBridge.RenderPartialToString(BigConfig.Layout, items, "BetterImageGallery");
			}
			catch (HttpException ex)
			{
				log.ErrorFormat(
					"Chosen layout ({0}) for _BetterImageGallery was not found in skin {1}. perhaps it is in a different skin. Error was: {2}",
					BigConfig.Layout,
					SiteUtils.GetSkinBaseUrl(true, Page),
					ex
				);

				partialString = RazorBridge.RenderPartialToString("_BetterImageGallery", items, "BetterImageGallery");
			}

			output.Write(partialString);
		}


		protected virtual void LoadSettings()
		{
			module = new Module(ModuleId);
		}
	}
}