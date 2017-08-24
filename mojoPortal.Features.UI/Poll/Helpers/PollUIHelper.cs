// Author:        Christian Fredh
// Created:       2007-09-26
// Last Modified: 2007-09-26
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Web.Framework;
using PollFeature.Business;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PollFeature.UI.Helpers
{
	public static class PollUIHelper
	{
		public static void AddResultBarToControl(PollOption option, string hexColor, Control ctrl)
		{
			Unit imgWidth =  Unit.Percentage(option.VotePercentage == null || option.VotePercentage == 0 ? 1 : option.VotePercentage.Value);

			Color color = UIHelper.HexStringToColor(hexColor);
			Label lbl = new Label();
			lbl.BackColor = color;
			lbl.Height = Unit.Pixel(10);
			lbl.Width = imgWidth;

			ctrl.Controls.Add(lbl);
		}
	}
}