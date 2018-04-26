using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	[ToolboxData("<{0}:EmbedSVGSprite runat=server></{0}:EmbedSVGSprite>")]
	public class EmbedSVGSprite : WebControl
	{
		public string FileName { get; set; } = null;
		public string FilePath { get; set; } = "~/Content/ui-icons/";

		protected override void RenderContents(HtmlTextWriter output)
		{
			string svgFilePath = null;
			string svgFileBody = null;			

			if (FileName != null)
			{
				svgFilePath = HttpContext.Current.Server.MapPath(FilePath + FileName);
				svgFileBody = File.ReadAllText(svgFilePath);

				output.Write(svgFileBody);
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			RenderContents(writer);
		}
	}
}
