using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Web.Components;

namespace mojoPortal.Features.UI.BetterImageGallery
{
	public class BetterImageGalleryRazor : WebControl
	{
		public BIGConfig BigConfig { get; set; }
		public int ModuleId { get; set; } = -1;

		private Module module = null;


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			LoadSettings();

			if (Page.IsPostBack) return;
		}


		protected virtual void LoadSettings()
		{
			module = new Module(ModuleId);

			if (module == null)
			{
				Visible = false;

				return;
			}
		}


		protected override void RenderContents(HtmlTextWriter output)
		{
			var partialString = string.Empty;

			var gallery = new BetterImageGalleryService(ModuleId);
			gallery.Setup();

			if (gallery.Error != null)
			{
				partialString = RazorBridge.RenderPartialToString(new RazorBridgePartialModel
				{
					CustomTemplate = "_BetterImageGallery_Error",
					DefaultTemplate = "_BetterImageGallery_Error",
					Controller = "BetterImageGallery",
					Data = gallery.Error,
					Page = Page
				});
			}
			else
			{
				var items = gallery.GetImages();

				partialString = RazorBridge.RenderPartialToString(new RazorBridgePartialModel
				{
					CustomTemplate = BigConfig.Layout,
					DefaultTemplate = "_BetterImageGallery",
					Controller = "BetterImageGallery",
					Data = items,
					Page = Page
				});
			}

			output.Write(partialString);
		}
	}
}