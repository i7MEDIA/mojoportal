using System.Drawing.Imaging;
using System.Web;

namespace Subkismet.Captcha;

/// <summary>
/// Handles a special request for a CAPTCHA image.  The request must 
/// pass the specs for the image via an encrypted serialized instance 
/// of <see cref="CaptchaInfo" />.
/// </summary>
public class CaptchaImageHandler : IHttpHandler
{
	/// <summary>
	/// Renders the Captcha Image.
	/// </summary>
	/// <param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides 
	/// references to the intrinsic server objects (for example, Request, Response, Session, and Server) 
	/// used to service HTTP requests.</param>
	public void ProcessRequest(HttpContext context)
	{
		HttpApplication application = context.ApplicationInstance;
		string encryptedCaptchaInfo = application.Request.QueryString["spec"];
		CaptchaInfo captcha = CaptchaInfo.FromEncryptedString(encryptedCaptchaInfo);

		string textToRender = captcha.Text;

		if (string.IsNullOrEmpty(textToRender))
		{
			application.Response.StatusCode = 404;
			application.Response.End();
		}
		else
		{
			using (CaptchaImage captchaImage = new CaptchaImage())
			{
				captchaImage.Width = captcha.Width;
				captchaImage.Height = captcha.Height;
				captchaImage.FontWarp = captcha.WarpFactor;
				captchaImage.Font = captcha.FontFamily;
				captchaImage.Text = textToRender;
				captchaImage.Image.Save(application.Context.Response.OutputStream, ImageFormat.Jpeg);
			}
			application.Response.ContentType = "image/jpeg";
			application.Response.StatusCode = 200;
			application.Response.End();
		}
	}


	/// <summary>
	/// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
	/// </summary>
	/// <value></value>
	/// <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
	public bool IsReusable
	{
		get
		{
			return true;
		}
	}
}

