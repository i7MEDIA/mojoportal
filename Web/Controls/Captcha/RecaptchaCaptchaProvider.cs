using System.Collections.Specialized;

namespace mojoPortal.Web.Controls.Captcha
{
	public class RecaptchaCaptchaProvider : CaptchaProvider
	{
		public override ICaptcha GetCaptcha()
		{
			return new RecaptchaAdapter();
		}

		public override void Initialize(string name, NameValueCollection config)
		{
			base.Initialize(name, config);
		}
	}
}
