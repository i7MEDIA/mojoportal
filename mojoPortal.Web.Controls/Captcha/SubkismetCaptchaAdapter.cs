using System.Web;
using System.Web.UI;

namespace mojoPortal.Web.Controls.Captcha;

public class SubkismetCaptchaAdapter : ICaptcha
{
	private readonly Subkismet.Captcha.CaptchaControl captchaControl = new();


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


	public SubkismetCaptchaAdapter() => InitializeAdapter();


	private void InitializeAdapter()
	{
		if (HttpContext.Current == null)
		{
			return;
		}

		captchaControl.InstructionText = "Enter the code shown above:";
		captchaControl.ErrorMessage = "Incorrect, try again";

		try
		{
			var resource = HttpContext.GetGlobalResourceObject("Resource", "SubkismetCaptchaInstructions");

			if (resource is not null)
			{
				captchaControl.InstructionText = "&nbsp;" + resource.ToString();
			}

			resource = HttpContext.GetGlobalResourceObject("Resource", "SubkismetCaptchFailureMessage");

			if (resource is not null)
			{
				captchaControl.ErrorMessage = resource.ToString();
			}
		}
		catch { }
	}


	public Control GetControl() => captchaControl;
}
