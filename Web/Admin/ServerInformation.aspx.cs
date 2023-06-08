using System;
using System.Data;
using System.IO;
using System.Net;
using System.Web;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Newtonsoft.Json.Linq;
using Resources;

namespace mojoPortal.Web.AdminUI
{
	public partial class ServerInformation : NonCmsBasePage
	{
		private bool shouldAllow = false;
		private static readonly ILog log = LogManager.GetLogger(typeof(ServerInformation));

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();
			if (!shouldAllow)
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

			GetUpdateInfo();

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

			btnRestart.Attributes.Add("class", displaySettings.RestartButtonClass);
		}

		private void GetUpdateInfo()
		{
			if (WebConfigSettings.AllowUpdateCheck)
			{

				var request = WebRequest.CreateHttp(new Uri("https://www.mojoportal.com:443/currentVersion.js"));
				WebResponse response;
				try
				{
					response = request.GetResponse();
				}
				catch (WebException)
				{
					return;
				}

				Stream dataStream = response.GetResponseStream();
				StreamReader reader = new(dataStream);
				string responseFromServer = reader.ReadToEnd();

				reader.Close();
				response.Close();

				JObject jObject = JObject.Parse(responseFromServer);
				string strSiteVersion = DatabaseHelper.DBCodeVersion().ToString();
				string strCurrentVersion = (string)jObject["version"];
				string strCurrentVersionUrl = (string)jObject["url"];

				if (!string.IsNullOrWhiteSpace(strCurrentVersion))
				{
					var currentVersion = Version.Parse(strCurrentVersion);
					var siteVersion = Version.Parse(strSiteVersion);
					if (currentVersion != null && siteVersion != null && currentVersion > siteVersion)
					{
						if (string.IsNullOrWhiteSpace(strCurrentVersionUrl))
						{
							strCurrentVersionUrl = "https://www.mojoportal.com";
						}

						litUpdateInfo.Text = string.Format(displaySettings.UpdateAvailableLinkMarkup, strCurrentVersionUrl, Resource.UpdateAvailable);
					}
				}
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

			if (WebConfigSettings.ShowSystemInformationInChildSiteAdminMenu || siteSettings.IsServerAdminSite && (WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor()))
			{
					shouldAllow = true;
			}
			else
			{
				shouldAllow = false;
			}
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

		protected void btnRestart_ServerClick(object sender, EventArgs e)
		{
			log.Info($"Application Restart triggered by administrator userid={SiteUtils.GetCurrentSiteUser().UserId}, username={SiteUtils.GetCurrentSiteUser().LoginName}");
			HttpRuntime.UnloadAppDomain();
			//WebUtils.SetupRedirect(this, Request.RawUrl);
			return;
		}
	}
}
