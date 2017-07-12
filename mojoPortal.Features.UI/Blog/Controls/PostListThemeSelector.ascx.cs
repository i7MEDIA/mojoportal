///	Author:				i7MEDIA
///	Created:			2017-05-11
///	Last Modified:		2017-05-11
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
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Features.UI.Blog
{
    public partial class PostListThemeSelector : UserControl, ISettingControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PostListThemeSelector));
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
            if (ddlThemes == null)
            {

                ddlThemes = new DropDownList();
                if (this.Controls.Count == 0) { this.Controls.Add(ddlThemes); }
            }
            if (ddlThemes.Items.Count > 0) { return; }

            SiteUtils.GetSkinBaseUrl(this.Page);

            themesPath = SiteUtils.GetSkinBaseUrl(this.Page) + "/Themes/PostListAdvanced/";
            globalThemesPath = WebUtils.GetApplicationRoot() + "/Blog/Themes/PostListAdvanced/";

            FileInfo[] themeFiles = GetThemes(themesPath);
            FileInfo[] globalThemeFiles = GetThemes(globalThemesPath);

            List<ListItem> items = new List<ListItem>();



            if (themeFiles != null)
            {
                PopulateDefinitionList(themeFiles, themesPath, false);
            }
            if (globalThemeFiles != null)
            {
                PopulateDefinitionList(globalThemeFiles, globalThemesPath, true);
            }



            //items.Sort()
            //ddlThemes.DataSource = items;
            //ddlThemes.DataBind();
            ddlThemes.Items.Insert(0, new ListItem("Please Select", string.Empty));
        }

        private void PopulateDefinitionList(FileInfo[] files, string path, bool isGlobal)
        {
            string nameAppendage = isGlobal ? " (g)" : "";

            foreach (FileInfo file in files)
            {
                //log.Info("superflexi markup definition found: " + file.FullName);
                if (File.Exists(file.FullName))
                {

                    var jsonFile = file.OpenText();
                    JObject jo = JObject.Parse(jsonFile.ReadToEnd());
                    file.
                    if (jo != null)
                    {
                        if ((string)jo["Name"] != null && (string)jo["Template"] != null)
                        {
                            ddlThemes.Items.Add(new ListItem((string)jo["Name"] + nameAppendage, path + file.Name));
                        }
                        else if ((string)jo["Template"] != null)
                        {
                            ddlThemes.Items.Add(new ListItem(file.Name.ToString().Replace(".json", "") + nameAppendage, path + file.Name));
                        }
                    }
                }
            }
        }
        private FileInfo[] GetThemes(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(path));
            return dir.Exists ? dir.GetFiles("*.json") : null;
        }

        public string GetValue()
        {
            return ddlThemes.SelectedValue;
        }

        public void SetValue(string val)
        {
            EnsureItems();

            if (val != null)
            {
                ListItem item = ddlThemes.Items.FindByValue(val);
                if (item != null)
                {
                    ddlThemes.ClearSelection();
                    item.Selected = true;
                }
            }
        }
    }
}