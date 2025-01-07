using System.Web.UI;

namespace mojoPortal.Web.Controls.Captcha;

public interface ICaptcha
{
	Control GetControl();
	bool IsValid { get; }
	string ControlID { get; set; }
	string ValidationGroup { get; set; }
	bool Enabled { get; set; }
	short TabIndex { get; set; }
}
