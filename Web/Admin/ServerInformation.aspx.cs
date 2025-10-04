using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;

namespace mojoPortal.Web.AdminUI;

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
		litCodeVersion.Text = DatabaseHelper.AppCodeVersion().ToString();

		Task.Run(GetUpdateInfo).Wait();

		if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now))
		{
			litServerTimeZone.Text = TimeZone.CurrentTimeZone.DaylightName;
		}
		else
		{
			litServerTimeZone.Text = TimeZone.CurrentTimeZone.StandardName;
		}

		//double preferredOffset = DateTimeHelper.GetPreferredGmtOffset();

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

		litDotNetVersion.Text = Environment.Version.ToString();


		//Medium Trust doesn't exist anymore so we're always in Full Trust
		//string space = " ";
		//try
		//{
		//	//http://stackoverflow.com/questions/13748055/could-not-load-type-system-runtime-compilerservices-extensionattribute-from-as

		//	//http://en.wikipedia.org/wiki/.NET_Framework_version_history

		//	//http://en.wikipedia.org/wiki/List_of_.NET_Framework_versions

		//	//http://stackoverflow.com/questions/16137658/system-environment-version-providing-an-inaccurate-value

		//	//http://stackoverflow.com/questions/8517159/how-to-detect-at-runtime-that-net-version-4-5-currently-running-your-code

		//	//http://blog.marcgravell.com/2012/09/iterator-blocks-missing-methods-and-net.html

		//	//litDotNetVersion.Text = System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion();
		//	//litDotNetVersion.Text = typeof(int).Assembly.ImageRuntimeVersion;
		//	//litDotNetVersion.Text = System.Runtime.InteropServices.RuntimeEnvironment.
		//	litDotNetVersion.Text = Environment.Version.ToString();
		//}
		//catch (System.Security.SecurityException) //happens under medium trust at elast in 3.5 .NET
		//{
		//	space = string.Empty;
		//}


		//if (mojoSetup.RunningInFullTrust())
		//{
		//	litDotNetVersion.Text += space + Resource.RunningInFullTrust;
		//}
		//else
		//{
		//	litDotNetVersion.Text += space + Resource.RunningInPartialTrust;
		//}

		using (IDataReader reader = DatabaseHelper.SchemaVersionGetNonCore())
		{
			grdSchemaVersion.DataSource = reader;
			grdSchemaVersion.DataBind();
		}

		btnRestart.Attributes.Add("class", displaySettings.RestartButtonClass);
	}


	private async Task GetUpdateInfo()
	{
		if (!WebConfigSettings.AllowUpdateCheck)
		{
			return;
		}

		try
		{
			var client = new HttpClient();
			var response = await client.GetAsync("https://www.mojoportal.com:443/currentVersion.json");

			if (!response.IsSuccessStatusCode)
			{
				return;
			}

			List<MediaTypeFormatter> formatters = [new JsonMediaTypeFormatter()];

			var currentVersionResponse = await response.Content.ReadAsAsync<CurrentVersion>(formatters);
			var currentVersionUrl = currentVersionResponse.Url;

			if (string.IsNullOrWhiteSpace(currentVersionResponse.Version))
			{
				return;
			}

			var currentVersion = Version.Parse(currentVersionResponse.Version);
			var siteVersion = Version.Parse(DatabaseHelper.AppCodeVersion().ToString());

			if (currentVersion == null || siteVersion == null || currentVersion <= siteVersion)
			{
				return;
			}

			if (string.IsNullOrWhiteSpace(currentVersionUrl))
			{
				currentVersionUrl = "https://www.mojoportal.com";
			}

			litUpdateInfo.Text = string.Format(displaySettings.UpdateAvailableLinkMarkup, currentVersionUrl, Resource.UpdateAvailable);
		}
		catch (Exception e)
		{
			log.Error("There was an issue fetching the currentVersion file from mojoportal.com", e);
			return;
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

		if (AppConfig.MultiTenancy.ShowSystemInfo || siteSettings.IsServerAdminSite && (WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor()))
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
		WebUtils.SetupRedirect(this, Request.RawUrl);
		return;
	}


	public record CurrentVersion
	{
		public string? Version { get; set; }
		public string? Url { get; set; }
	}
}
