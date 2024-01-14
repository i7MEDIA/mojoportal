using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI;

/// <summary>
/// a sub class of BasePanel so it can be configured differently from theme.skin than panels used in other scenarios
/// </summary>
public class AdminCrumbContainer : BasePanel
{
	/// <summary>
	///this can be set from theme.skin and is used to set the text on any <portal:AdminCrumbSeparator inside this panel
	/// 
	/// </summary>
	public string AdminCrumbSeparator { get; set; } = "&nbsp;&gt;";

	protected override void OnLoad(System.EventArgs e)
	{
		base.OnLoad(e);
		foreach (Control c in Controls)
		{
			if (c is AdminCrumbSeparator)
			{
				AdminCrumbSeparator s = c as AdminCrumbSeparator;
				s.Text = AdminCrumbSeparator;
			}
		}
	}
}

public class AdminCrumbSeparator : Literal { }