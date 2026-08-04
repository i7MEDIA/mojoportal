using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.XmlUI;

public partial class EditXml : NonCmsBasePage
{
	private XmlConfiguration config = new();
	private int pageId = -1;
	private int moduleId = -1;
	private string xmlBasePath = string.Empty;
	private string xslBasePath = string.Empty;


	#region OnInit

	protected override void OnPreInit(EventArgs e)
	{
		AllowSkinOverride = true;
		base.OnPreInit(e);
	}


	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		updateButton.Click += new EventHandler(UpdateBtn_Click);
		btnUpload.Click += new EventHandler(btnUpload_Click);
	}

	#endregion


	private void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);

			return;
		}

		SecurityHelper.DisableBrowserCache();

		LoadParams();

		if (!UserCanEditModule(moduleId, XmlConfiguration.FeatureGuid))
		{
			SiteUtils.RedirectToAccessDeniedPage();

			return;
		}

		LoadSettings();
		PopulateLabels();

		if (!IsPostBack)
		{
			PopulateControls();

			if (Request.UrlReferrer != null && hdnReturnUrl.Value.Length == 0)
			{
				hdnReturnUrl.Value = Request.UrlReferrer.ToString();
				lnkCancel.NavigateUrl = hdnReturnUrl.Value;
			}
		}
	}


	private void PopulateControls()
	{
		ddXml.DataSource = GetXmlList();
		ddXml.DataBind();

		var listItem = new ListItem(XmlResources.XmlNoFileSelected, string.Empty);

		ddXml.Items.Insert(0, listItem);

		ddXsl.DataSource = GetXslList();
		ddXsl.DataBind();
		ddXsl.Items.Insert(0, listItem);

		if (config.XmlFileSource.Length > 0)
		{

			listItem = ddXml.Items.FindByValue(config.XmlFileSource);
			if (listItem != null)
			{
				ddXml.ClearSelection();
				listItem.Selected = true;
			}

		}

		if (config.XslFileSource.Length > 0)
		{
			listItem = ddXsl.Items.FindByValue(config.XslFileSource);

			if (listItem != null)
			{
				ddXsl.ClearSelection();
				listItem.Selected = true;
			}
		}

		txtXmlUrl.Text = config.XmlUrl;
		txtXslUrl.Text = config.XslUrl;

	}


	void btnUpload_Click(object sender, EventArgs e)
	{
		if (uploader.HasFile)
		{
			var newFileName = Path.GetFileName(uploader.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
			var ext = Path.GetExtension(uploader.FileName).ToLowerInvariant();

			if (!SiteUtils.IsAllowedUploadBrowseFile(ext, ".xml|.xsl"))
			{
				return;
			}

			string destPath;

			switch (ext)
			{
				case ".xml":
					destPath = Server.MapPath(xmlBasePath + newFileName);

					if (File.Exists(destPath))
					{
						File.Delete(destPath);
					}

					uploader.SaveAs(destPath);

					break;

				case ".xsl":
					destPath = Server.MapPath(xslBasePath + newFileName);

					if (File.Exists(destPath))
					{
						File.Delete(destPath);
					}

					uploader.SaveAs(destPath);

					break;
			}

			if (hdnReturnUrl.Value.Length > 0)
			{
				WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
				return;
			}


		}

		WebUtils.SetupRedirect(this, Request.RawUrl);
	}


	protected FileInfo[] GetXmlList()
	{
		var filePath = HttpContext.Current.Server.MapPath(xmlBasePath);

		if (Directory.Exists(filePath))
		{
			return new DirectoryInfo(filePath).GetFiles("*.xml");
		}

		return null;
	}


	protected FileInfo[] GetXslList()
	{
		var filePath = HttpContext.Current.Server.MapPath(xslBasePath);

		if (Directory.Exists(filePath))
		{
			return new DirectoryInfo(filePath).GetFiles("*.xsl");
		}

		return null;
	}


	private static (bool Success, string ErrorMessage) IsUrlAllowed(string fileUrl, List<string> allowedHostnames)
	{
		if (!Uri.TryCreate(fileUrl, UriKind.Absolute, out Uri targetUri))
		{
			return (false, XmlResources.InvalidFileUrl);
		}

		foreach (var allowedHostname in allowedHostnames)
		{
			if (allowedHostname == null)
			{
				continue;
			}

			var targetHost = targetUri.Host;

			if (targetHost.Equals(allowedHostname, StringComparison.OrdinalIgnoreCase))
			{
				return (true, null);
			}
		}

		return (false, XmlResources.UrlNotFromHostInAllowedHostnames);
	}


	void UpdateBtn_Click(object sender, EventArgs e)
	{
		var m = new Module(moduleId);
		var ok = true;

		if (!string.IsNullOrWhiteSpace(txtXmlUrl.Text))
		{
			var (Success, ErrorMessage) = IsUrlAllowed(txtXmlUrl.Text, config.AllowedHostnames);

			if (!Success)
			{
				ok = false;
				lblXmlUrlValidationSummary.Visible = true;
				lblXmlUrlValidationSummary.Text = ErrorMessage;
			}
		}

		if (!string.IsNullOrWhiteSpace(txtXslUrl.Text))
		{
			var (Success, ErrorMessage) = IsUrlAllowed(txtXslUrl.Text, config.AllowedHostnames);

			if (!Success)
			{
				ok = false;
				lblXslUrlValidationSummary.Visible = true;
				lblXslUrlValidationSummary.Text = ErrorMessage;
			}
		}

		if (ok)
		{
			ModuleSettings.UpdateModuleSetting(
				m.ModuleGuid,
				m.ModuleId,
				"XmlModuleXmlSourceSetting",
				ddXml.SelectedValue);

			ModuleSettings.UpdateModuleSetting(
				m.ModuleGuid,
				m.ModuleId,
				"XmlModuleXslSourceSetting",
				ddXsl.SelectedValue);

			ModuleSettings.UpdateModuleSetting(
				m.ModuleGuid,
				m.ModuleId,
				"XmlUrl",
				txtXmlUrl.Text);

			ModuleSettings.UpdateModuleSetting(
				m.ModuleGuid,
				m.ModuleId,
				"XslUrl",
				txtXslUrl.Text);

			CurrentPage.UpdateLastModifiedTime();
			CacheHelper.ClearModuleCache(m.ModuleId);

			if (!string.IsNullOrWhiteSpace(hdnReturnUrl.Value))
			{
				WebUtils.SetupRedirect(this, hdnReturnUrl.Value);

				return;
			}

			WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}
	}


	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, XmlResources.EditXmlSettingsLabel);

		heading.Text = XmlResources.EditXmlSettingsLabel;

		updateButton.Text = XmlResources.EditXmlUpdateButton;
		SiteUtils.SetButtonAccessKey(updateButton, XmlResources.EditXmlUpdateButtonAccessKey);

		lnkCancel.Text = XmlResources.EditXmlCancelButton;

		btnUpload.Text = XmlResources.Upload;

		regexFile.ErrorMessage = XmlResources.UploadExtensionWarning;

		Control c = Master.FindControl("Breadcrumbs");

		if (c != null)
		{
			BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
			crumbs.ForceShowBreadcrumbs = true;
		}

		// borowing these from Image Gallery feature instead of replicating them

		uploader.AddFileText = GalleryResources.SelectFileButton;
		uploader.DropFileText = XmlResources.DropFile;
		uploader.UploadButtonText = GalleryResources.BulkUploadButton;
		uploader.UploadCompleteText = GalleryResources.UploadComplete;
		uploader.UploadingText = GalleryResources.Uploading;
	}


	private void LoadSettings()
	{
		if (moduleId > -1)
		{
			Hashtable settings = ModuleSettings.GetModuleSettings(moduleId);
			config = new XmlConfiguration(settings);

			if (WebConfigSettings.XmlUseMediaFolder)
			{
				xmlBasePath = Invariant($"~/Data/Sites/{siteSettings.SiteId}/media/xml/");
				xslBasePath = Invariant($"~/Data/Sites/{siteSettings.SiteId}/media/xsl/");
			}
			else
			{
				xmlBasePath = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/xml/";
				xslBasePath = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/xsl/";
			}
		}

		uploader.AcceptFileTypes = SecurityHelper.GetRegexValidationForAllowedExtensionsJqueryFileUploader("xml|xsl");
		uploader.UploadButtonClientId = btnUpload.ClientID;
		uploader.ServiceUrl = $"{SiteRoot}/XmlXsl/uploader.ashx?pageid={pageId.ToInvariantString()}&mid={moduleId.ToInvariantString()}";
		uploader.FormFieldClientId = hdnState.ClientID; // not really used but prevents submitting all the form 

		string refreshFunction = $"function refresh{moduleId.ToInvariantString()} () {{ window.location.reload(true);  }} ";

		uploader.UploadCompleteCallback = "refresh" + moduleId.ToInvariantString();

		ScriptManager.RegisterClientScriptBlock(
			this,
			GetType(), "refresh" + moduleId.ToInvariantString(),
			refreshFunction,
			true);

		AddClassToBody("xmledit");
	}


	private void LoadParams()
	{
		pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
		moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
		lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
	}
}
