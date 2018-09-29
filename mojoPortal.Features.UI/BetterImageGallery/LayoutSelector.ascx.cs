using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Features.UI.BetterImageGallery
{
	public partial class LayoutSelector : UserControl, ISettingControl
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(LayoutSelector));
		private static SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

		private string themesPath = string.Empty;
		private string globalThemesPath = string.Empty; //these are at a higher level than the current site, can be used by multiple sites


		protected void Page_Load(object sender, EventArgs e)
		{
			SecurityHelper.DisableBrowserCache();
		}


		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			if (HttpContext.Current == null) return;

			EnsureItems();
		}

		private void EnsureItems()
		{
			if (ddlLayouts == null)
			{
				ddlLayouts = new DropDownList();

				if (Controls.Count == 0) Controls.Add(ddlLayouts);
			}

			if (ddlLayouts.Items.Count > 0) return;

			themesPath = SiteUtils.DetermineSkinBaseUrl(true, false, Page) + "/Views/BetterImageGallery/";

			List<FileInfo> themeFiles = GetLayouts(themesPath);
			List<ListItem> items = new List<ListItem>();

			if (themeFiles != null)
			{
				PopulateDefinitionList(themeFiles, themesPath);
			}

			if (ddlLayouts.Items.Count == 0)
			{
				ddlLayouts.Enabled = false;
				ddlLayouts.Visible = false;
				litNoLayouts.Text = BetterImageGalleryResources.NoLayoutsInSkin;
			}
			else
			{
				ddlLayouts.Items.Insert(0, new ListItem("Please Select", string.Empty));
			}
		}


		private void PopulateDefinitionList(List<FileInfo> files, string path)
		{
			foreach (FileInfo file in files)
			{
				if (File.Exists(file.FullName))
				{
					if (file.Name == "_BetterImageGallery.cshtml")
					{
						ddlLayouts.Items.Add(new ListItem("Default", "_BetterImageGallery"));
					}
					else
					{
						ddlLayouts.Items.Add(new ListItem(file.Name.Replace(".cshtml", "").Replace("_BetterImageGallery--", ""), file.Name.Replace(".cshtml", "")));
					}
				}
			}
		}


		private List<FileInfo> GetLayouts(string path)
		{
			DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(path));
			List<FileInfo> files = new List<FileInfo>();

			if (dir.Exists)
			{
				files.AddRange(dir.GetFiles("_BetterImageGallery.cshtml"));
				files.AddRange(dir.GetFiles("_BetterImageGallery--*.cshtml"));
			}

			return files;
		}


		public string GetValue()
		{
			RazorBridge.FindPartialView("_BetterImageGallery", new { }, "BetterImageGallery");

			return ddlLayouts.SelectedValue;
		}


		public void SetValue(string val)
		{
			EnsureItems();

			if (val != null)
			{
				ListItem item = ddlLayouts.Items.FindByValue(val);

				if (item != null)
				{
					ddlLayouts.ClearSelection();
					item.Selected = true;
				}
			}
		}
	}
}