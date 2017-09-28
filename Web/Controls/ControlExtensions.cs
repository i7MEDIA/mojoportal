using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace mojoPortal.Web
{
	public static class ControlExtensions
	{
		/// <summary>
		/// recursively finds a child control of the specified parent.
		/// </summary>
		/// <param name="control"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Control FindControlRecursive(this Control control, string id)
		{
			if (control == null) return null;
			//try to find the control at the current level
			Control ctrl = control.FindControl(id);

			if (ctrl == null)
			{
				//search the children
				foreach (Control child in control.Controls)
				{
					ctrl = FindControlRecursive(child, id);

					if (ctrl != null) break;
				}
			}
			return ctrl;
		}
	}
}