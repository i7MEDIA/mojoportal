using Subkismet.Captcha;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls.Captcha;

public class SubkismetInvisibleCaptchaAdapter : ICaptcha
{
	private readonly InvisibleCaptcha captchaControl = new();


	#region Properties

	public bool IsValid
	{
		get
		{
			captchaControl.Validate();
			return captchaControl.IsValid;
		}
	}

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

	#endregion


	public SubkismetInvisibleCaptchaAdapter() => InitializeAdapter();


	private void InitializeAdapter()
	{
		captchaControl.Display = ValidatorDisplay.Dynamic;
		captchaControl.ValidationGroup = "Blog";
	}


	public Control GetControl() => captchaControl;
}
