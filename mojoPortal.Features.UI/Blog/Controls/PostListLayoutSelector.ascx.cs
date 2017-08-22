///	Author:				i7MEDIA
///	Created:			2017-05-11
///	Last Modified:		2017-08-22
///		
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace mojoPortal.Web.BlogUI
{
	public partial class PostListLayoutSelector : UserControl, ISettingControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PostListLayoutSelector));
        private static SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
        //private int roleID = -1;
        //private SiteUser siteUser;

        private string themesPath = string.Empty;
        private string globalThemesPath = string.Empty; //these are at a higher level than the current site, can be used by multiple sites


        protected void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.DisableBrowserCache();
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current == null) { return; }
            EnsureItems();
        }

        private void EnsureItems()
        {
            if (ddlLayouts == null)
            {

                ddlLayouts = new DropDownList();
                if (this.Controls.Count == 0) { this.Controls.Add(ddlLayouts); }
            }
            if (ddlLayouts.Items.Count > 0) { return; }

            themesPath = SiteUtils.DetermineSkinBaseUrl(true, false, this.Page) + "/Views/Blog/";
            //globalThemesPath = WebUtils.GetApplicationRoot() + "/Views/Blog/";

            List<FileInfo> themeFiles = GetLayouts(themesPath);
            //List<FileInfo> globalThemeFiles = GetLayouts(globalThemesPath);

            List<ListItem> items = new List<ListItem>();



            if (themeFiles != null)
            {
                PopulateDefinitionList(themeFiles, themesPath);
            }
            //if (globalThemeFiles != null)
            //{
            //    PopulateDefinitionList(globalThemeFiles, globalThemesPath, true);
            //}



            //items.Sort()
            //ddlLayouts.DataSource = items;
            //ddlLayouts.DataBind();
			if (ddlLayouts.Items.Count == 0)
			{
				ddlLayouts.Enabled = false;
				ddlLayouts.Visible = false;
				litNoLayouts.Text = BlogResources.NoPostListLayoutsInSkin;
			}
			else
			{
				ddlLayouts.Items.Insert(0, new ListItem("Please Select", string.Empty));
			}
        }

        private void PopulateDefinitionList(List<FileInfo> files, string path)
        {
            //string nameAppendage = isGlobal ? " (g)" : "";

            foreach (FileInfo file in files)
            {
                //log.Info("superflexi markup definition found: " + file.FullName);
                if (File.Exists(file.FullName))
                {
                    if (file.Name == "_BlogPostList.cshtml")
                    {
                        ddlLayouts.Items.Add(new ListItem("Default", "_BlogPostList"));

                    }
                    else
                    {
                        ddlLayouts.Items.Add(new ListItem(file.Name.Replace(".cshtml", "").Replace("_BlogPostList--", ""), file.Name.Replace(".cshtml", "")));
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
                files.AddRange(dir.GetFiles("_BlogPostList.cshtml"));
                files.AddRange(dir.GetFiles("_BlogPostList--*.cshtml"));
            }
            return files;
        }

        public string GetValue()
        {
            RazorBridge.FindPartialView("_BlogPostList", new { }, "Blog");

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