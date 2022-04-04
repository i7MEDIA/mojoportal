using log4net;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace mojoPortal.Web.AdminUI
{
	public partial class SecurityAdvisorPage : NonCmsBasePage
	{
		private readonly SecurityAdvisor securityAdvisor = new SecurityAdvisor();
		private static readonly ILog log = LogManager.GetLogger(typeof(SecurityAdvisorPage));

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);

				return;
			}

			if (!WebUser.IsAdmin)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);

				return;
			}

			if (!siteSettings.IsServerAdminSite)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);

				return;
			}

			SecurityHelper.DisableBrowserCache();

			LoadSettings();
			PopulateLabels();
			PopulateControls();
		}


		private void PopulateControls()
		{
			// Default Admin Account

			var (userExists, passwordIsDefault) = securityAdvisor.DefaultAdmin();

			if (userExists)
			{
				litDefaultAdminAccountResults.Text = $"<div class='alert alert-danger'><strong>{Resource.Attention}</strong> {(passwordIsDefault ? Resource.SecurityAdvisorDefaultAdminAccountExistsAndPasswordIsDefault : Resource.SecurityAdvisorDefaultAdminAccountExists)}</div>";
			}
			else
			{
				litDefaultAdminAccountResults.Text = $"<div class='alert alert-success'><strong>{Resource.Congratulations}</strong> {Resource.SecurityAdvisorDefaultAdminAccountDoesNotExists}</div>";
			}

			// Machine Key
			if (securityAdvisor.UsingCustomMachineKey())
			{
				litMachineKeyResults.Text = $"<div class='alert alert-success'><strong>{Resource.Congratulations}</strong> {Resource.SecurityAdvisorMachineKeyCorrect}</div>";
			}
			else
			{
				litMachineKeyResults.Text = $@"<div class='alert alert-danger'><strong>{Resource.Attention}</strong> {Resource.SecurityAdvisorMachineKeyWrong}</div>
					<pre class='language language-xml'><code>{Server.HtmlEncode(SiteUtils.GenerateRandomMachineKeyXml())}</code></pre>
					<div class=''>{Resource.CustomMachineKeyInstructions}</div>
					<div class='alert alert-info'>{Resource.GenerateMachineKey}.</div>";
			}

			// File System Permissions
			if (WebUtils.ParseBoolFromQueryString("fc", false))
			{
				List<string> writableFolders = securityAdvisor.GetWritableFolders();

				if (writableFolders.Count > 0)
				{
					var sb = new StringBuilder();

					sb.Append($@"<div class='alert alert-danger'><strong>{Resource.Attention}</strong> {Resource.SecurityAdvisorFileSystemPermissionsWrong}</div>");
					sb.Append("<div><ul class='simplelist writablefolders'>");

					foreach (string f in writableFolders)
					{
						sb.Append("<li>" + f + "</li>");
					}

					sb.Append("</ul></div>");

					litFileSystemResults.Text = sb.ToString();
				}
				else
				{
					litFileSystemResults.Text = $"<div class='alert alert-success'><strong>{Resource.Congratulations}</strong> {Resource.SecurityAdvisorFileSystemPermissionsCorrect}</div>";
				}
			}
			else
			{
				litFileSystemResults.Text = $"<a href='{SiteRoot}/Admin/SecurityAdvisor.aspx?fc=true' class='btn btn-warning'>{Resource.CheckIfTooManyWritableFolders}</a>";
			}

			// SSL Connection Test
			SslTest_HowsMySsl();

			litDefaultAdminAccountResults.Text += displaySettings.PanelBottomMarkup;
			litMachineKeyResults.Text += displaySettings.PanelBottomMarkup;
			litFileSystemResults.Text += displaySettings.PanelBottomMarkup;
			litSecurityProtocolDescription.Text += displaySettings.PanelBottomMarkup;
		}


		private void PopulateLabels()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, Resource.SecurityAdvisor);

			heading.Text = Resource.SecurityAdvisor;
			litInfo.Text = Resource.SecurityAdvisorInfo;

			lnkAdminMenu.Text = Resource.AdminMenuLink;
			lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
			lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

			lnkThisPage.Text = Resource.SecurityAdvisor;
			lnkThisPage.ToolTip = Resource.SecurityAdvisor;
			lnkThisPage.NavigateUrl = SiteRoot + "/Admin/SecurityAdvisor.aspx";

			litDefaultAdminAccountHeading.Text = string.Format(displaySettings.PanelHeadingMarkup, Resource.SecurityAdvisorDefaultAdminAccountHeading, Resource.SecurityAdvisorDefaultAdminAccountDescription);
			litMachineKeyHeading.Text = string.Format(displaySettings.PanelHeadingMarkup, Resource.SecurityAdvisorMachineKeyHeading, Resource.SecurityAdvisorMachineKeyDescription);
			litFileSystemHeading.Text = string.Format(displaySettings.PanelHeadingMarkup, Resource.SecurityAdvisorFileSystemHeading, Resource.SecurityAdvisorFileSystemDescription);
			litSecurityProtocolHeading.Text = string.Format(displaySettings.PanelHeadingMarkup, Resource.SecurityAdvisorSecurityProtocolHeading, Resource.SecurityAdvisorSecurityProtocolDescription);
		}

		public void SslTest_HowsMySsl()
		{
			try
			{
				var request = WebRequest.CreateHttp(new Uri("https://howsmyssl.com:443/a/check"));

				ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

				WebResponse response = request.GetResponse();

				Stream dataStream = response.GetResponseStream();
				StreamReader reader = new StreamReader(dataStream);
				string responseFromServer = reader.ReadToEnd();

				reader.Close();
				response.Close();

				if (WebConfigSettings.SecurityAdvisorLogTLSCheckResponse)
				{
					log.Info($"SecurityAdvisorTLSCheckResponse:\r\n{responseFromServer}");
				}

				var jObject = JObject.Parse(responseFromServer);
				var ciphers = ((JArray)jObject["given_cipher_suites"]).Select(c => (string)c).ToList();
				var tlsver = (string)jObject["tls_version"];
				var rating = (string)jObject["rating"];
				var ekeys = (string)jObject["ephemeral_keys_supported"];
				var sticket = (string)jObject["session_ticket_supported"];
				var tlscompr = (string)jObject["tls_compression_supported"];
				var unknownCiphers = (string)jObject["unknown_cipher_suite_supported"];
				var beast = (string)jObject["beast_vuln"];
				var n_minus_one_splitting = (string)jObject["able_to_detect_n_minus_one_splitting"];
				var insecureCiphers = (JObject)jObject["insecure_cipher_suites"];

				if (rating == "Bad")
				{
					rating = "<span class=\"text-danger\">Bad <i class=\"fa fa-exclamation-triangle\" aria-hidden=\"true\"></i></span>";
				}

				var sb = new StringBuilder();

				sb.Append($"<strong>{Resource.SecurityAdvisorSecurityProtocolVersion}:</strong> {tlsver}<br/>");
				sb.Append($"<strong>{Resource.SecurityAdvisorSecurityProtocolRating}:</strong> {rating}<br/>");
				sb.Append($"<strong>{Resource.SecurityAdvisorSecurityProtocolEphemeralKeys}:</strong> {ekeys}<br/>");
				sb.Append($"<strong>{Resource.SecurityAdvisorSecurityProtocolTLSCompression}:</strong> {tlscompr}<br/>");
				sb.Append($"<strong>{Resource.SecurityAdvisorSecurityProtocolUnknownCiphers}:</strong> {unknownCiphers}<br/>");
				sb.Append($"<strong>{Resource.SecurityAdvisorSecurityProtocolBeastVuln}:</strong> {beast}<br/>");
				sb.Append($"<strong>{Resource.SecurityAdvisorSecurityProtocolNMinusOneSplitting}:</strong> {n_minus_one_splitting}<br/>");

				if (insecureCiphers.Count > 0)
				{
					sb.Append($"<h4 class=\"text-danger\">{Resource.SecurityAdvisorSecurityProtocolInsecureCiphers} <i class=\"fa fa-exclamation-triangle\" aria-hidden=\"true\"></i></h4><ul>");

					foreach (var cipher in insecureCiphers)
					{
						sb.Append($"<li><strong>{cipher.Key}</strong><ul>");

						foreach (var cipherWarning in cipher.Value)
						{
							sb.Append($"<li>{(string)cipherWarning}</li>");
						}

						sb.Append("</ul></li>");
					}

					sb.Append("</ul>");
				}

				sb.Append($"<h4>{Resource.SecurityAdvisorSecurityProtocolCiphers}</h4><ul>");

				foreach (string cipher in ciphers)
				{
					sb.Append($"<li>{cipher}</li>");
				}

				sb.Append("</ul>");
				sb.Append($"<h4>{Resource.SecurityAdvisorSecurityProtocolFullCheckResponse}</h4><pre class='language language-js'><code>{JsonConvert.SerializeObject(jObject, Formatting.Indented)}</code></pre>");

				litSecurityProtocolDescription.Text = string.Format(displaySettings.SecurityProtocolCheckResponseMarkup, sb.ToString());
			}
			catch (WebException)
			{ }
		}


		private void LoadSettings()
		{
			AddClassToBody("administration");
			AddClassToBody("securityadvisor");
		}


		#region OnInit

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += new EventHandler(Page_Load);

			SuppressMenuSelection();
			SuppressPageMenu();
		}

		#endregion
	}
}