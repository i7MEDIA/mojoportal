/// Author:                     
/// Created:                    2005-11-19
///	Last Modified:              2012-05-08

using System;
using System.Collections;
using System.IO;
using System.Web;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ContentUI
{
	
    public partial class HtmlIncludeEdit : NonCmsBasePage
	{
        
		int moduleID = -1;
        //private String cacheDependencyKey;

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            base.OnInit(e);
        }

        #endregion

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();

            moduleID = WebUtils.ParseInt32FromQueryString("mid", -1);
			
            if (!UserCanEditModule(moduleID))
			{
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            //cacheDependencyKey = "Module-" + moduleID.ToString();

			PopulateLabels();

			if (!IsPostBack) 
			{
				PopulateControls();
                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();

                }
			}
		}

		private void PopulateLabels()
		{
            Title = SiteUtils.FormatPageTitle(siteSettings, HtmlIncludeResources.EditHtmlFragmentIncludeSettingsLabel);
            heading.Text = HtmlIncludeResources.EditHtmlFragmentIncludeSettingsLabel;
            btnUpdate.Text = HtmlIncludeResources.EditHtmlFragmentUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, HtmlIncludeResources.EditHtmlFragmentUpdateButtonAccessKey);

            btnCancel.Text = HtmlIncludeResources.EditHtmlFragmentCancelButton;
            SiteUtils.SetButtonAccessKey(btnCancel, HtmlIncludeResources.EditHtmlFragmentCancelButtonAccessKey);

            
		}

		private void PopulateControls()
		{
			this.ddInclude.DataSource = GetFragmentList();
			this.ddInclude.DataBind();

			if (moduleID > -1) 
			{
				Hashtable settings = ModuleSettings.GetModuleSettings(moduleID);
				string includeFile = string.Empty;
				
				if(settings.Contains("HtmlFragmentSourceSetting"))
				{
					includeFile = settings["HtmlFragmentSourceSetting"].ToString();	
				}

				if(includeFile.Length > 0)
				{
					this.ddInclude.SelectedValue = includeFile;
				}

			}

		}


		protected  FileInfo[] GetFragmentList()
		{
			string filePath = string.Empty;
			
			string p;

            if (WebConfigSettings.HtmlFragmentUseMediaFolder)
            {
                p = WebUtils.GetApplicationRoot() + "/Data/Sites/" + siteSettings.SiteId.ToString() + "/media/htmlfragments";
            }
            else
            {
                p = WebUtils.GetApplicationRoot() + "/Data/Sites/" + siteSettings.SiteId.ToString() + "/htmlfragments";
            }
            

			filePath = HttpContext.Current.Server.MapPath(p);
			
			if(Directory.Exists(filePath))
			{
				DirectoryInfo dir = new DirectoryInfo(filePath);
				
				return dir.GetFiles();
			}

			return null;
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
            Module m = new Module(moduleID);
            
			ModuleSettings.UpdateModuleSetting(m.ModuleGuid, m.ModuleId, "HtmlFragmentSourceSetting", this.ddInclude.SelectedValue);
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

		private void btnCancel_Click(object sender, EventArgs e)
		{
            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

		}


	
	}
}
