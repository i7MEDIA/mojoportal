using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	/// <summary>
	/// A server control that encapsulates the jQuery NivoSlider http://nivo.dev7studios.com/
	/// Set the ImageFolder Property to a folder with images.
	/// Set the ImageWidth to the actual pixel width of the images.
	/// IMPORTANT: All the images need to be the same size.
	/// Example Usage:
	/// <portal:NivoSlider ID="NivoSlider1" runat="server" ImageFolder='~/Data/Sites/1/nivoimages' ImageWidth='618px' />
	/// 
	/// In the style.config of your skin folder add one of these:
	/// <file cssvpath="/Data/style/nivoslider/default/default.css" imagebasevpath="/Data/style/nivoslider/default/">none</file>
	/// or
	/// <file cssvpath="/Data/style/nivoslider/orman/orman.css" imagebasevpath="/Data/style/nivoslider/orman/">none</file>
	/// or create your own css based on one of the above
	/// </summary>
	[ToolboxData("<{0}:NivoSlider runat=server SliderId='slider' ImageWidth='618px' ImageFolder='~/Data/SiteImages'></{0}:NivoSlider>")]
	public class NivoSlider : WebControl
	{
		#region Properties

		/// <summary>
		/// Specifies the path where images are found eg ~/Images
		/// </summary>
		[Bindable(false)]
		[Category("Data")]
		[DefaultValue("")]
		public string ImageFolder { get; set; } = string.Empty;

		/// <summary>
		/// Sets width of images and containing div
		/// </summary>
		[Bindable(false)]
		[Category("Appearance")]
		public Unit ImageWidth { get; set; } = Unit.Pixel(618);

		/// <summary>
		/// this page shows all the javascript options and their defaults
		/// http://nivo.dev7studios.com/support/jquery-plugin-usage/
		/// </summary>
		public string Config { get; set; } = string.Empty;

		public string ScriptPath { get; set; } = "~/ClientScript/jqmojo/jquery.nivo.slider.pack3-2.js";

		#endregion properties


		#region public methods

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

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
					$"\n<script src=\"{Page.ResolveUrl(ScriptPath)}\"></script>",
					false
				);
			}

			EnableViewState = false;
		}



		public override void RenderControl(HtmlTextWriter writer)
		{
			// goal is to render this markup except with dynamic widths and images:
			//<div class="slider-wrapper theme-default">
			//    <div id="slider" class="nivoSlider">
			//        <img src="images/toystory.jpg" alt="" />
			//        <img src="images/walle.jpg" alt="" />
			//        <img src="images/nemo.jpg" alt="" title="#htmlcaption" />
			//    </div>                    
			//</div>            

			// add containing divs

			writer.AddStyleAttribute(HtmlTextWriterStyle.Width, ImageWidth.ToString());

			writer.AddAttribute(HtmlTextWriterAttribute.Class, "slider-wrapper theme-default");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			writer.AddAttribute(HtmlTextWriterAttribute.Class, "nivoSlider");
			writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			if (string.IsNullOrEmpty(ImageFolder))
			{
				writer.Write("No Image Folder Configured"); // TODO: localize?
			}
			else
			{
				// render images

				string folderPath = Context.Server.MapPath(ImageFolder);

				if (!Directory.Exists(folderPath))
				{
					throw new Exception("Invalid image folder path :" + ImageFolder);
				}

				IEnumerable<string> files = Directory.EnumerateFiles(folderPath);

				if (!ImageFolder.EndsWith("/"))
				{
					ImageFolder += "/";
				}

				foreach (string file in files)
				{
					//filter out any files in the folder that are not images
					string ext = Path.GetExtension(file);

					if (!SiteUtils.IsAllowedUploadBrowseFile(ext, ".jpg|.gif|.png|.jpeg"))
					{
						continue;
					}

					Image img = new Image
					{
						EnableViewState = false,
						// make path relative rather than absolute
						ImageUrl = ImageFolder + Path.GetFileName(file),
						AlternateText = " ",
						Width = ImageWidth
					};

					img.RenderControl(writer);
				}
			}

			writer.RenderEndTag();
			writer.RenderEndTag();

			// add jquery onload function to start show
			Page.ClientScript.RegisterStartupScript(GetType(), GetUniqueId("startSlider"), GetPageLoadScript(), true);
		}

		#endregion public methods


		#region private methods

		private string GetPageLoadScript()
		{
			return $@"
$(window).load(function() {{
	$('#{ClientID}').nivoSlider({{{Config}}});
}});";
		}

		/// <summary>
		/// returns a unique id for registering script etc with id unique to control
		/// </summary>
		/// <param name="baseId"></param>
		/// <returns></returns>
		private string GetUniqueId(string baseId)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}{1}", UniqueID, baseId);
		}

		#endregion private methods
	}
}
