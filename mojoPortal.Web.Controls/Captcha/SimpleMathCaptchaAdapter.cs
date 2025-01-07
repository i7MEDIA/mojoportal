using System.Web.UI;

namespace mojoPortal.Web.Controls.Captcha;

public class SimpleMathCaptchaAdapter : ICaptcha
{
	private readonly SimpleMathCaptchaControl captchaControl = new();

	public bool IsValid => captchaControl.IsValid;

	public bool Enabled
	{
		get => captchaControl.Enabled;
		set => captchaControl.Enabled = value;
	}

	public string ControlID
	{
		get => captchaControl.ID;
		set => captchaControl.ID = value;
	}

	public string ValidationGroup
	{
		get => captchaControl.ValidationGroup;
		set => captchaControl.ValidationGroup = value;
	}

	public short TabIndex
	{
		get => captchaControl.TabIndex;
		set => captchaControl.TabIndex = value;
	}


	public SimpleMathCaptchaAdapter()
	{ }


	public Control GetControl() => captchaControl;
}
