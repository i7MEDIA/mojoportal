using System;
using System.Web.UI;

namespace mojoPortal.Web.UI;

/// <summary>
/// Control is no longer needed because TextMode=Color works well. Leaving here for backwards compatibility for modules which used this.
/// </summary>

public partial class ColorSetting : UserControl, ISettingControl
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}

	protected void Page_Load(object sender, EventArgs e)
	{
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);		
	}

	#region ISettingControl

	public string GetValue()
	{
		return txtHexColor.Text;
	}

	public void SetValue(string val)
	{
		txtHexColor.Text = val;
	}

	#endregion
}