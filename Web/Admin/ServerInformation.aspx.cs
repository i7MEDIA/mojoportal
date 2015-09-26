/// Author:					Joe Audette
/// Created:				2007-08-08
/// Last Modified:			2011-03-15
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class ServerInformation : NonCmsBasePage
    {
        private bool shouldAllow = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            if (!shouldAllow)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
            
            if (
                (!siteSettings.IsServerAdminSite)
                && (!WebConfigSettings.ShowSystemInformationInChildSiteAdminMenu)
                )
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            
            litPlatform.Text = DatabaseHelper.DBPlatform();
            litCodeVersion.Text = DatabaseHelper.DBCodeVersion().ToString();
            if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now))
            {
                litServerTimeZone.Text = TimeZone.CurrentTimeZone.DaylightName;
            }
            else
            {
                litServerTimeZone.Text = TimeZone.CurrentTimeZone.StandardName;
            }

            double preferredOffset = DateTimeHelper.GetPreferredGmtOffset();

            litServerLocalTime.Text = DateTime.Now.ToString();
            litCurrentGMT.Text = DateTime.UtcNow.ToString();
            litServerGMTOffset.Text = DateTimeHelper.GetServerGmtOffset().ToString();
            //litPreferredGMTOffset.Text = preferredOffset.ToString();
            //litPreferredTime.Text = DateTime.UtcNow.AddHours(preferredOffset).ToString();

            try
            {
                litOperatingSystem.Text = Environment.OSVersion.VersionString;
            }
            catch (InvalidOperationException) { }

            string space = " ";

            try
            {
                //http://stackoverflow.com/questions/13748055/could-not-load-type-system-runtime-compilerservices-extensionattribute-from-as

                //http://en.wikipedia.org/wiki/.NET_Framework_version_history

                //http://en.wikipedia.org/wiki/List_of_.NET_Framework_versions

                //http://stackoverflow.com/questions/16137658/system-environment-version-providing-an-inaccurate-value

                //http://stackoverflow.com/questions/8517159/how-to-detect-at-runtime-that-net-version-4-5-currently-running-your-code

                //http://blog.marcgravell.com/2012/09/iterator-blocks-missing-methods-and-net.html

                //litDotNetVersion.Text = System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion();
                //litDotNetVersion.Text = typeof(int).Assembly.ImageRuntimeVersion;
                //litDotNetVersion.Text = System.Runtime.InteropServices.RuntimeEnvironment.
                litDotNetVersion.Text = Environment.Version.ToString();
            }
            catch (System.Security.SecurityException) //happens under medium trust at elast in 3.5 .NET
            {
                space = string.Empty;
            }

            if (mojoSetup.RunningInFullTrust())
            {
                litDotNetVersion.Text += space + Resource.RunningInFullTrust; 
            }
            else
            {
                litDotNetVersion.Text += space + Resource.RunningInPartialTrust; 
            }

            using (IDataReader reader = DatabaseHelper.SchemaVersionGetNonCore())
            {
                grdSchemaVersion.DataSource = reader;
                grdSchemaVersion.DataBind();
            }

        }

        private void PopulateLabels()
        {
            litFeaturesHeading.Text = Resource.FeatureVersions;
            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkServerInfo.Text = Resource.AdminMenuServerInfoLabel;
            lnkServerInfo.ToolTip = Resource.AdminMenuServerInfoLabel;
            lnkServerInfo.NavigateUrl = SiteRoot + "/Admin/ServerInformation.aspx";

            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuServerInfoLabel);
            heading.Text = Resource.AdminMenuServerInfoLabel;

             grdSchemaVersion.Columns[0].HeaderText = Resource.Feature;
             grdSchemaVersion.Columns[1].HeaderText = Resource.SchemaVersion;

             AddClassToBody("administration");
             AddClassToBody("serverinfo");

        }

        private void LoadSettings()
        {
            if (WebUser.IsAdminOrContentAdmin)  {  shouldAllow = true;  }
            if (SiteUtils.UserIsSiteEditor()) { shouldAllow = true; }

            if ((!WebConfigSettings.ShowSystemInformationInChildSiteAdminMenu) && (!siteSettings.IsServerAdminSite)) { shouldAllow = false; }

        }

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            SuppressMenuSelection();
            SuppressPageMenu();
        }


        #endregion

    }
}
