// Author:					Joe Audette
// Created:				    2010-11-29
// Last Modified:			2010-11-29
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Web.UI;
using mojoPortal.Web.Framework;

namespace mojoPortal.Flickr.UI
{
    public partial class FlickrThemeSetting : UserControl, ISettingControl
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
            if (dd.Items.Count > 0) { return; }

            List<string> themes = FlickrGalleryConfiguration.AvailableThemes.SplitOnCharAndTrim(',');
            foreach (string theme in themes)
            {
                dd.Items.Add(new ListItem(theme, theme));
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