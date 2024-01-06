using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace mojoPortal.Web.Services;

/// <summary>
/// Returns styles JS for CKeditor
/// </summary>
public class CkEditorStyles : IHttpHandler
{
	public bool IsReusable { get { return false; } }

	public void ProcessRequest(HttpContext context)
	{
		context.Response.ContentType = "text/javascript";
		context.Response.ContentEncoding = new UTF8Encoding();
		var json = JsonConvert.SerializeObject(Global.SkinConfig.EditorStyles, Formatting.None);

		context.Response.Write($"try{{CKEDITOR.addStylesSet('mojo',{json});}}catch(err){{}}");
	}
}
