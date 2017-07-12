/// Author:					
/// Created:				2008-07-28
/// Last Modified:		    2010-06-15
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    
    public partial class PageLastModified : UserControl
    {
        private PageSettings currentPage = null;
        private double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        private bool showTime = true;
        private string labelText = string.Empty;

        public bool ShowTime
        {
            get { return showTime; }
            set { showTime = value; }
        }

        public string LabelText
        {
            get { return labelText; }
            set { labelText = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            LoadSettings();
            PopulateControls();
        }

        private void PopulateControls()
        {
            if (currentPage == null) 
            {
                this.Visible = false;
                return; 
            }

            if (labelText.Length > 0)
            {
                litLabel.Text = labelText;
            }
            else
            {
                litLabel.Text = Resource.PageLastModifiedLabel;
            }

            if (showTime)
            {
                if (timeZone != null)
                {
                    litLastModified.Text = currentPage.LastModifiedUtc.ToLocalTime(timeZone).ToString();
                }
                else
                {
                    litLastModified.Text = currentPage.LastModifiedUtc.AddHours(timeOffset).ToString();
                }
            }
            else
            {
                if (timeZone != null)
                {
                    litLastModified.Text = currentPage.LastModifiedUtc.ToLocalTime(timeZone).ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                }
                else
                {
                    litLastModified.Text = currentPage.LastModifiedUtc.AddHours(timeOffset).ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                }
            }
            


        }

        private void LoadSettings()
        {
            if (Page.IsPostBack) { return; }

            currentPage = CacheHelper.GetCurrentPage();
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

        }
    }
}