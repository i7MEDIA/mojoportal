using System.Collections.Specialized;

namespace mojoPortal.Web.Controls.Captcha;

public class SubkismetInvisibleCaptchaProvider : CaptchaProvider
{
	public override ICaptcha GetCaptcha() => new SubkismetInvisibleCaptchaAdapter();


	public override void Initialize(string name, NameValueCollection config)
	{
		base.Initialize(name, config);
		// don't read anything from config
		// here as this would raise an error under Medium Trust
	}
}
