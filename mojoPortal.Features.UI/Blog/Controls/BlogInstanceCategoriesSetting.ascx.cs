using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;

namespace mojoPortal.Web.BlogUI
{
	public partial class BlogInstanceCategoriesSetting : UserControl, ISettingControl
	{
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (HttpContext.Current == null) { return; }
			EnsureItems();
		}

		//protected void Page_Load(object sender, EventArgs e) {}

		private void EnsureItems()
		{
			if (dd.Items.Count > 0) { return; }

			var thisModuleId = WebUtils.ParseInt32FromQueryString("mid", -1);

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings == null) { return; }

			dd.Items.Add(new ListItem(Resources.BlogResources.AllCategories, string.Empty));

			var config = new BlogPostListAdvancedConfiguration(ModuleSettings.GetModuleSettings(thisModuleId));

			if (config is not null && config.BlogModuleId != -1)
			{
				using IDataReader reader = Blog.GetCategoriesList(config.BlogModuleId);
				while (reader.Read())
				{
					dd.Items.Add(new ListItem(reader["Category"].ToString(), reader["CategoryID"].ToString()));
				}
			}
			else
			{
				dd.Enabled = false;
				litNote.Text = string.Format(coreDisplaySettings.AlertWarningMarkup, Resources.BlogResources.BlogInstanceNotYetSet);
			}
		}

		#region ISettingControl

		public string GetValue()
		{
			EnsureItems();
			return dd.SelectedValue;
		}

		public void SetValue(string val)
		{
			EnsureItems();
			ListItem item = dd.Items.FindByValue(val);
			if (item != null)
			{
				dd.ClearSelection();
				item.Selected = true;
			}
		}

		#endregion

	}
}