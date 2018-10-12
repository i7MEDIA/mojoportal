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

		private BetterImageGalleryService bigService = null;
		private BIGModel items = new BIGModel();
		private Module module = null;
		string partialString = string.Empty;

		public int ModuleId { get; set; } = -1;
		public BIGConfig BigConfig { get; set; }


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			LoadSettings();

			if (Page.IsPostBack) return;
		}


		private string GetRazorString(string partialName, object model, bool error = false)
		{
			try
			{
				return RazorBridge.RenderPartialToString(partialName, model, "BetterImageGallery");
			}
			catch (HttpException ex)
			{
				log.ErrorFormat(
					"Chosen layout ({0}) for _BetterImageGallery was not found in skin {1}. perhaps it is in a different skin. Error was: {2}",
					partialName,
					SiteUtils.GetSkinBaseUrl(true, Page),
					ex
				);

				return RazorBridge.RenderPartialToString(error ? "_BetterImageGallery_Error" : "_BetterImageGallery", model, "BetterImageGallery");
			}
		}

		protected virtual void LoadSettings()
		{
			module = new Module(ModuleId);

			if (module == null)
			{
				Visible = false;

				return;
			}

			bigService = new BetterImageGalleryService(ModuleId);
			bigService.Setup();

			if (bigService.Error != null)
			{
				partialString = GetRazorString("_BetterImageGallery_Error", bigService.Error, true);
			}
			else
			{
				items = bigService.GetImages();
				partialString = GetRazorString(BigConfig.Layout, items);
			}
		}

		protected override void RenderContents(HtmlTextWriter output)
		{
			output.Write(partialString);
		}
	}
}