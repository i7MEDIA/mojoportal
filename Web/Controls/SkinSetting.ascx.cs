// Author:					
// Created:				    2011-05-18
// Last Modified:			2011-06-18
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.UI
{
    public partial class SkinSetting : UserControl, ISettingControl
    {
        private SiteSettings selectedSite = null;

        public SiteSettings SelectedSite
        {
            get { return selectedSite; }
            set { 
                selectedSite = value;
                catalogChanged = true;
                EnsureItems();
                catalogChanged = false;
            }
        }

        private bool useCatalog = false;
        private bool catalogChanged = false;

        public bool UseCatalog
        {
            get { return useCatalog; }
            set 
            {

                if (useCatalog != value) 
                { 
                    catalogChanged = true;
                    EnsureItems();
                }
                useCatalog = value; 
            }
        }

        private bool showPreviewLink = true;

        public bool ShowPreviewLink
        {
            get { return showPreviewLink; }
            set { showPreviewLink = value; }
        }

        private bool addSiteDefaultOption = false;

        public bool AddSiteDefaultOption
        {
            get { return addSiteDefaultOption; }
            set { addSiteDefaultOption = value; }
        }

        private string colorBoxConfig = string.Empty;

        public string ColorBoxConfig
        {
            get { return colorBoxConfig; }
            set { colorBoxConfig = value; }
        }

        private string pageUrl = string.Empty;

        public string PageUrl
        {
            get { return pageUrl; }
            set { pageUrl = value; }
        }

        

        public bool Enabled
        {
            get { return dd.Enabled; }
            set { dd.Enabled = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current == null) { return; }
            EnsureItems();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            


        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            lnkPreview.Visible = (showPreviewLink && !useCatalog); //can't preview the catalog skins
            if (showPreviewLink)
            {  
                lnkPreview.Text = Resource.SkinPreviewLink;
                lnkPreview.NavigateUrl = Request.RawUrl;
                SetupScript();
            }
        }

        private void SetupScript()
        {
            //EnsureItems();
            if (dd.Items.Count == 0) { return; }

            if (pageUrl.Length == 0)
            {
                pageUrl = SiteUtils.GetNavigationSiteRoot() + "/Default.aspx";
            }
            
            StringBuilder sb = new StringBuilder();
            sb.Append("<div style='display:none;'>");
            foreach (ListItem item in dd.Items)
            {
                if (item.Value.Length == 0) { continue; }

                sb.Append("<a rel='previewitem' href='");
                sb.Append(pageUrl + "?skin=");
                sb.Append(item.Value);
                sb.Append("' title='");
                sb.Append(item.Value);
                sb.Append("'>");
                sb.Append(item.Value);
                sb.Append("</a>");
            }
            sb.Append("</div>");
            litHiddenPreviewLinks.Text = sb.ToString();

            if (colorBoxConfig.Length == 0)
            {
                colorBoxConfig = "{width:'95%', height:'98%', iframe:true, current:\"" + Server.HtmlEncode(Resource.SkinCountJsFormat) + "\"}";
            }

            mojoBasePage basePage = Page as mojoBasePage;
            if (basePage != null) { basePage.ScriptConfig.IncludeColorBox = true; }

            sb = new StringBuilder();

            sb.Append("$('#" + lnkPreview.ClientID + "').click(function(e){ ");
           // sb.Append("e.preventDefault(); ");

            sb.Append("$(\"a[rel='previewitem']\").colorbox(" + colorBoxConfig + ");");

            sb.Append("$(\"a[rel='previewitem']\").first().click();");

            sb.Append("e.preventDefault(); ");
            
            sb.Append("});");

            string script = "$(\"a[rel='previewitem']\").colorbox(" + colorBoxConfig + ");";

            ScriptManager.RegisterStartupScript(
                this, 
                typeof(Page),
                "skinpreviewlink", "\n<script type=\"text/javascript\">\n" + sb.ToString() + "\n</script>", 
                false);

        }

        private void EnsureItems()
        {
            if (!Visible) { return; } //don't populate unless visible

            if (dd == null)
            {
                dd = new DropDownList();
                dd.DataValueField = "Name";
                dd.DataTextField = "Name";

                if (this.Controls.Count == 0) { this.Controls.Add(dd); }
            }

            if ((dd.Items.Count > 0)&&(!catalogChanged)) { return; }

            DirectoryInfo[] skins;

            if (selectedSite != null)
            {
                skins = SiteUtils.GetSkinList(selectedSite);
            }
            else
            {
                if (useCatalog)
                {
                    skins = SiteUtils.GetSkinCatalogList();
                }
                else
                {
                    skins = SiteUtils.GetSkinList(CacheHelper.GetCurrentSiteSettings());
                }
            }

            if (skins == null) { return; }

            dd.DataSource = skins;
            dd.DataBind();

            ListItem listItem;

            List<string> skinsToExclude = WebConfigSettings.SkinsToExcludeFromSkinList.SplitOnCharAndTrim(',');
            foreach (string s in skinsToExclude)
            {
                listItem = dd.Items.FindByValue(s);
                if (listItem != null)
                {
                    dd.Items.Remove(listItem);
                }
            }

            if (addSiteDefaultOption)
            {
                listItem = new ListItem();
                listItem.Value = "";
                listItem.Text = Resource.PageLayoutDefaultSkinLabel;
                dd.Items.Insert(0, listItem);
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