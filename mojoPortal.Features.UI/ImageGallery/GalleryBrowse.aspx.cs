/// Author:				        
/// Created:			        2004-11-28
/// Last Modified:		        2012-05-08

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.GalleryUI
{
    public partial class GalleryBrowse : mojoBasePage
	{
        private Literal imageLink = new Literal();
        private string imageFolderPath;
        private int pageNumber = 1;
        private int totalPages = 1;
        private int pageId = -1;
        private int moduleId = -1;
        private int itemId = -1;
        private Hashtable moduleSettings;
        //private string pageNumberParam;
        private bool showTechnicalData = false;
        protected string webSizeBaseUrl = string.Empty;
        protected string fullSizeBaseUrl = string.Empty;

        private static readonly ILog log = LogManager.GetLogger(typeof(GalleryBrowse));

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new System.EventHandler(this.Page_Load);
            base.OnInit(e);
            SuppressPageMenu();
            SuppressMenuSelection();
        }

        #endregion

        private void Page_Load(object sender, System.EventArgs e)
		{
            GetRequestParams();

            if (!UserCanViewPage(moduleId, Gallery.FeatureGuid))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, GalleryResources.BrowseGalleryPageTitle);

            LoadSettings();
			ShowImage();

		}

		private void ShowImage()
		{
            if (moduleId == -1) { return; }
            
            Gallery gallery = new Gallery(moduleId);
            DataTable dt = gallery.GetWebImageByPage(pageNumber);

            if (dt.Rows.Count > 0)
            {
                itemId = Convert.ToInt32(dt.Rows[0]["ItemID"]);
                totalPages = Convert.ToInt32(dt.Rows[0]["TotalPages"]);
            }

            
            showTechnicalData = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "GalleryShowTechnicalDataSetting", false);

            if (itemId == -1) { return; }
                
			Literal topPageLinks = new Literal();
			string pageUrl = SiteRoot
				+ "/ImageGallery/GalleryBrowse.aspx?"
                + "pageid=" + pageId.ToInvariantString()
                + "&amp;mid=" + moduleId.ToInvariantString()
				+ "&amp;pagenumber=";

			topPageLinks.Text = UIHelper.GetPagerLinksWithPrevNext(
                pageUrl,1, 
                this.totalPages, 
                this.pageNumber, 
                "modulepager", 
                "SelectedPage");

			this.spnTopPager.Controls.Add(topPageLinks);
			
			GalleryImage galleryImage = new GalleryImage(moduleId, itemId);

			
            imageLink.Text = "<a onclick=\"window.open(this.href,'_blank');return false;\"  href='" + ImageSiteRoot 
				+ fullSizeBaseUrl + galleryImage.ImageFile + "' ><img  src='"
                + ImageSiteRoot + webSizeBaseUrl
				+ galleryImage.WebImageFile + "' alt='"
                + Resources.GalleryResources.GalleryWebImageAltText + "' /></a>";

			
			
			this.pnlGallery.Controls.Add(imageLink);
            this.lblCaption.Text = Server.HtmlEncode(galleryImage.Caption);
			this.lblDescription.Text = galleryImage.Description;

			if(showTechnicalData)
			{
				if(galleryImage.MetaDataXml.Length > 0)
				{
                    xmlMeta.DocumentContent = galleryImage.MetaDataXml;
                    string xslPath = System.Web.HttpContext.Current.Server.MapPath(SiteRoot + "/ImageGallery/GalleryMetaData.xsl");
                    xmlMeta.TransformSource = xslPath;
				}
			}

				

		}

        private void LoadSettings()
        {
            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

            string baseUrl;
            if (WebConfigSettings.ImageGalleryUseMediaFolder)
            {
                baseUrl = "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/GalleryImages/" + moduleId.ToInvariantString() + "/";
            }
            else
            {
                baseUrl = "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/GalleryImages/" + moduleId.ToInvariantString() + "/";
            }

            
            webSizeBaseUrl = baseUrl + "WebImages/";
            fullSizeBaseUrl = baseUrl + "FullSizeImages/";

            imageFolderPath = HttpContext.Current.Server.MapPath("~" + baseUrl);

            AddClassToBody("gallerybrowse");
           
        }

        private void GetRequestParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", itemId);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);

        }

	}
}
