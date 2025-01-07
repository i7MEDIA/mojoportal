using System.Configuration.Provider;

namespace mojoPortal.Web.Controls.Captcha;

public abstract class CaptchaProvider : ProviderBase
{
	public abstract ICaptcha GetCaptcha();
}
