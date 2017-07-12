// Author:					
// Created:				    2010-07-27
// Last Modified:			2010-08-06
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
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI
{
    public class JQueryUIThemePicker : DropDownList
    {
        private SiteSettings siteSettings = null;
        private string themeCookieName = "jqThemeCookie";
        private string themeCookieValue = string.Empty;
        private bool usePersistentCookie = true;
        private mojoBasePage basePage = null;
        private bool renderAsListItem = false;
        private string listItemCssClass = "topnavitem";
        private string allowedRoles = string.Empty;

        public bool RenderAsListItem
        {
            get { return renderAsListItem; }
            set { renderAsListItem = value; }
        }

        public bool UsePersistentCookie
        {
            get { return usePersistentCookie; }
            set { usePersistentCookie = value; }
        }

        /// <summary>
        /// if blank all users are allowed, otherwise specify semi colon separated role names like Admins;Content Administrators;
        /// </summary>
        public string AllowedRoles
        {
            get { return allowedRoles; }
            set { allowedRoles = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Visible) { return; }
            if (!Enabled) { Visible = false; return; }

            if (allowedRoles.Length > 0)
            {
                if (!WebUser.IsInRoles(allowedRoles)) { Visible = false; return; }
            }

            LoadSettings();
            if((siteSettings == null)||(basePage == null)) { Visible = false; return; }
            
#if NET35
            if (WebConfigSettings.DisablePageViewStateByDefault) {Page.EnableViewState = true; }
#endif
            EnsureDropDownData();
            DoInitialSelection();

            
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (renderAsListItem)
            {
                writer.Write("<li class='" + listItemCssClass + "'>");
            }
            base.Render(writer);
            if (renderAsListItem)
            {
                writer.Write("</li>");
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            SetCookieAndRedirect();
        }

        private void SetCookieAndRedirect()
        {
            if (SelectedIndex > -1)
            {
                CookieHelper.SetCookie(themeCookieName, SelectedValue, usePersistentCookie);

            }

            WebUtils.SetupRedirect(this, Page.Request.RawUrl);

        }
        

        private void EnsureDropDownData()
        {
            if (this.Items.Count > 0) { return; }

            string availableThemesCsv = WebConfigSettings.jQueryUIAvailableThemes;
            List<string> themes = availableThemesCsv.SplitOnChar(',');
            themes.Sort();

            foreach (string theme in themes)
            {
                ListItem item = new ListItem(theme, theme);
                this.Items.Add(item);

            }

        }

        private void DoInitialSelection()
        {
           // if (Page.IsPostBack) { return; }

            string defaultTheme = basePage.StyleCombiner.JQueryUIThemeName;

            if (string.IsNullOrEmpty(themeCookieValue))
            {
                themeCookieValue = defaultTheme; 
            }

            if (!Page.IsPostBack)
            {
                ListItem item = Items.FindByValue(themeCookieValue);
                if (item != null)
                {
                    ClearSelection();
                    item.Selected = true;
                }
            }

            basePage.StyleCombiner.JQueryUIThemeName = themeCookieValue;

        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            basePage = Page as mojoBasePage;

            themeCookieName += siteSettings.SiteId.ToInvariantString();
            themeCookieValue = CookieHelper.GetCookieValue(themeCookieName);

        }



    }
}