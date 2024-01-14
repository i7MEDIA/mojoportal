using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI;

/// <summary>
/// The only purpose of this panel is to easily have a wrapper panel that will automatically have a CSS class
/// based on the ModuleId so that it is possible to easily make a specific module instance have a different style
/// </summary>
[Obsolete()]
public class ModulePanel : Panel
{
	public int ModuleId { get; set; } = -1;

	public bool RenderModulePanel { get; set; } = false;

	protected override void OnPreRender(EventArgs e)
	{
		if (ModuleId > -1)
		{
			if (CssClass.Length == 0)
			{
				CssClass = Invariant($"module{ModuleId}");
			}
			else
			{
				CssClass += Invariant($" module{ModuleId}");
			}
		}

		base.OnPreRender(e);
	}

	protected override void Render(HtmlTextWriter writer)
	{
		if (HttpContext.Current == null)
		{
			writer.Write("[" + this.ID + "]");
			return;
		}

		if ((RenderModulePanel) || (WebConfigSettings.RenderModulePanel))
		{
			base.Render(writer);
		}
		else
		{
			base.RenderContents(writer);
		}
	}
}