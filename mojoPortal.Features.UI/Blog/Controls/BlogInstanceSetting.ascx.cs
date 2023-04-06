using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.BlogUI
{
	public partial class BlogInstanceSetting : UserControl, ISettingControl
	{
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (HttpContext.Current == null) { return; }
			EnsureItems();
		}

		protected void Page_Load(object sender, EventArgs e)
		{


		}

		private void EnsureItems()
		{
			// if (dd.Items.Count > 0) { return; }

			// SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			// if (siteSettings == null) { return; }

			// List<Module> modules = Module.GetModuleListForSite(siteSettings.SiteId, Guid.Parse("026cbead-2b80-4491-906d-b83e37179ccf"));

			//foreach (Module module in modules)
			// {
			//     ListItem item = new ListItem(module.ModuleTitle, module.ModuleId.ToString());
			//     dd.Items.Add(item);
			// }

			if (list.Items.Count > 0) { return; }

			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings == null) { return; }

			List<Module> modules = Module.GetModuleListForSite(siteSettings.SiteId, Guid.Parse("026cbead-2b80-4491-906d-b83e37179ccf"));

			foreach (Module module in modules)
			{
				ListItem item = new ListItem(module.ModuleTitle, module.ModuleId.ToString());
				list.Items.Add(item);
			}
		}

		#region ISettingControl

		public string GetValue()
		{
			EnsureItems();

			//return dd.SelectedValue;

			List<int> selectedItems = new List<int>();

			foreach (ListItem item in list.Items)
			{
				if (item.Selected)
				{
					selectedItems.Add(Convert.ToInt32(item.Value));
				}
			}

			return list.SelectedValue;
		}

		public void SetValue(string val)
		{
			EnsureItems();
			//ListItem item = dd.Items.FindByValue(val);
			//if (item != null)
			//{
			//	dd.ClearSelection();
			//	item.Selected = true;
			//}

			foreach (var moduleId in val.SplitOnCharAndTrim(','))
			{
				ListItem item = list.Items.FindByValue(moduleId);
				if (item != null)
				{
					item.Selected = true;
				}
			}
		}
		#endregion
	}
}