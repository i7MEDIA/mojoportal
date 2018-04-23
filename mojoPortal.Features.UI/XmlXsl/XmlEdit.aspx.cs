///	Last Modified:              2013-04-05  
/// 
/// 

using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Brettle.Web.NeatUpload;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.XmlUI 
{
     
    public partial class EditXml : NonCmsBasePage 
	{
        private int pageId = -1;
        private int moduleId = -1;
        //private String cacheDependencyKey;
        private XmlConfiguration config = new XmlConfiguration();

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
            this.Load += new EventHandler(this.Page_Load);
            this.updateButton.Click += new EventHandler(this.UpdateBtn_Click);
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
                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                    lnkCancel.NavigateUrl = hdnReturnUrl.Value;

                }
            }
        }



		

		private void PopulateControls()
		{
			this.ddXml.DataSource = GetXmlList();
			this.ddXml.DataBind();

            ListItem listItem = new ListItem(XmlResources.XmlNoFileSelected, string.Empty);
            ddXml.Items.Insert(0, listItem);

			this.ddXsl.DataSource = GetXslList();
			this.ddXsl.DataBind();
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
                //this.ddXsl.SelectedValue = xsl;
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
                string newFileName = Path.GetFileName(uploader.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);

                string ext = Path.GetExtension(uploader.FileName).ToLowerInvariant();
                if (!SiteUtils.IsAllowedUploadBrowseFile(ext, ".xml|.xsl"))
                {
                    //lblMessage.Text = GalleryResources.InvalidFile;

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

        // previous implementation with NeatUpload
        //void btnUpload_Click(object sender, EventArgs e)
        //{

        //    if (fileToUpload.HasFile && fileToUpload.FileName != null && fileToUpload.FileName.Trim().Length > 0)
        //    {
        //        string newFileName = Path.GetFileName(fileToUpload.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);

        //        string ext = Path.GetExtension(fileToUpload.FileName).ToLowerInvariant();
        //        if (!SiteUtils.IsAllowedUploadBrowseFile(ext, ".xml|.xsl"))
        //        {
        //            //lblMessage.Text = GalleryResources.InvalidFile;

        //            return;
        //        }

        //        string destPath;

        //        switch (ext)
        //        {
        //            case ".xml":
        //                destPath = Server.MapPath(xmlBasePath + newFileName);
        //                fileToUpload.MoveTo(destPath, MoveToOptions.Overwrite);
        //                break;

        //            case ".xsl":
        //                destPath = Server.MapPath(xslBasePath + newFileName);
        //                fileToUpload.MoveTo(destPath, MoveToOptions.Overwrite);
        //                break;
        //        }

        //        if (hdnReturnUrl.Value.Length > 0)
        //        {
        //            WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
        //            return;
        //        }
                

        //    }

        //    WebUtils.SetupRedirect(this, Request.RawUrl);
        //}

        protected FileInfo[] GetXmlList()
        {
            
            string filePath = HttpContext.Current.Server.MapPath(xmlBasePath);

            if (Directory.Exists(filePath))
            {
                return new DirectoryInfo(filePath).GetFiles("*.xml");
            }

            return null;
        }

        protected FileInfo[] GetXslList()
        {
           
            string filePath = HttpContext.Current.Server.MapPath(xslBasePath);

            if (Directory.Exists(filePath))
            {
                return new DirectoryInfo(filePath).GetFiles("*.xsl");
            }

            return null;
        }


	    void UpdateBtn_Click(Object sender, EventArgs e)
		{
            Module m = new Module(moduleId);

            ModuleSettings.UpdateModuleSetting(
                m.ModuleGuid,
                m.ModuleId, 
                "XmlModuleXmlSourceSetting", 
                this.ddXml.SelectedValue);

            ModuleSettings.UpdateModuleSetting(
                m.ModuleGuid,
                m.ModuleId, 
                "XmlModuleXslSourceSetting", 
                this.ddXsl.SelectedValue);

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
            //CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
            CacheHelper.ClearModuleCache(m.ModuleId);
            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
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
                    xmlBasePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/xml/";
                    xslBasePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/xsl/";
                }
                else
                {
                    xmlBasePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/xml/";
                    xslBasePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/xsl/";
                }


                

            }

            uploader.AcceptFileTypes = SecurityHelper.GetRegexValidationForAllowedExtensionsJqueryFileUploader("xml|xsl");
            uploader.UploadButtonClientId = btnUpload.ClientID;
            uploader.ServiceUrl = SiteRoot + "/XmlXsl/uploader.ashx?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString();
            uploader.FormFieldClientId = hdnState.ClientID; // not really used but prevents submitting all the form 

            string refreshFunction = "function refresh" + moduleId.ToInvariantString()
                    + " () { window.location.reload(true);  } ";

            uploader.UploadCompleteCallback = "refresh" + moduleId.ToInvariantString();

            ScriptManager.RegisterClientScriptBlock(
                this,
                this.GetType(), "refresh" + moduleId.ToInvariantString(),
                refreshFunction,
                true);

            AddClassToBody("xmledit");
        }


        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            //cacheDependencyKey = "Module-" + moduleId.ToString();
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();

        }
        
    }
}
