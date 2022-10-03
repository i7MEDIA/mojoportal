using mojoPortal.Web.UI;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.FeedUI
{
	public partial class FeedTypeSetting : UserControl, ISettingControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{ }


		#region ISettingControl

		public string GetValue()
		{
			return ddFeedType.SelectedValue;
		}


		public void SetValue(string val)
		{
			ListItem item = ddFeedType.Items.FindByValue(val);

			if (item != null)
			{
				ddFeedType.ClearSelection();
				item.Selected = true;
			}
		}

		#endregion
	}
}