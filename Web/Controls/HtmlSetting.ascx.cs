// Author:					
// Created:				    2010-08-01
// Last Modified:			2014-10-15
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 2011-03-05 changed to use CKeditor which now can work inside updatepanel. The ajaxtoolkit editor does not work in medium trust so this solves a medium trust problem as well

using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Editor;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI
{
    public partial class HtmlSetting : UserControl, ISettingControl
    {
        //private bool runningInMediumTrust = false;

        

        protected void Page_Load(object sender, EventArgs e)
        {

            ConfigurEditor();
        }

        private void ConfigurEditor()
        {
            //editor1.Visible = false;
            //editor1.Enabled = false;
            //txtPlain.Visible = false;

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            
            string siteRoot = SiteUtils.GetNavigationSiteRoot(); ;
            //ed1.SiteRoot = siteRoot;
            if(WebConfigSettings.UseSkinCssInEditor)
            {
                ed1.EditorCSSUrl = SiteUtils.GetEditorStyleSheetUrl(true, true, Page);
            }
            
            ed1.ToolBar = ToolBar.FullWithTemplates;
            ed1.Height = Unit.Pixel(300);
            ed1.Skin = WebConfigSettings.CKEditorSkin;

            if (siteSettings != null)
            {
                if (
                    (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
                    || (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
                    )
                {
                    ed1.FileManagerUrl = siteRoot + WebConfigSettings.FileDialogRelativeUrl;
                    ed1.EnableFileBrowser = true;
                }
            }

            ed1.TemplatesJsonUrl = siteRoot + "/Services/CKeditorTemplates.ashx?cb=" + Guid.NewGuid().ToString(); //prevent caching with a guid param
            ed1.StylesJsonUrl = siteRoot + "/Services/CKeditorStyles.ashx?cb=" + Guid.NewGuid().ToString().Replace("-", string.Empty);


            CultureInfo defaultCulture = SiteUtils.GetDefaultUICulture();
            if (defaultCulture.TextInfo.IsRightToLeft)
            {
                ed1.TextDirection = Direction.RightToLeft;
            }
        }


        #region ISettingControl
#if MONO
        //this is broken under Mono null reference exception 
        // try catch just prevents the whole page from crashing
        public string GetValue()
        {
            try
            {
                return ed1.Text;
            }
            catch{}
            return string.Empty;
        }

        public void SetValue(string val)
        {
            try
            {
                ed1.Text = val;
            }
            catch{}
        }

#else
        public string GetValue()
        {
            //if (runningInMediumTrust)
            //{
            //    return txtPlain.Text;
            //}
            //return editor1.Content;
            return ed1.Text;
        }

        public void SetValue(string val)
        {
            //if (runningInMediumTrust)
            //{
             //   txtPlain.Text = val;
            //}
            //else
            //{
              //  editor1.Content = val;
            //}

            ed1.Text = val;
        }

#endif

        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //http://ajaxcontroltoolkit.codeplex.com/workitem/26772

            //runningInMediumTrust = !mojoSetup.RunningInFullTrust();

            //if (runningInMediumTrust)
            //{
            //    editor1.Visible = false;
            //    editor1.Enabled = false;
            //    txtPlain.Visible = true;
                
            //}
        }

    }
}