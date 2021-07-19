/// Author:					
/// Created:				2004-11-28
/// Last Modified:			2013-02-18
/// 
/// compact mode = show thumbs with paging and web size of selected thumb in same view
/// normal mode = show all thumbs, no paging, web images shown in Lightbox (iBox)
/// webart mode = show 6 thumbs using lightbox and link to more page
/// 2011-02-16 Jamie Eubanks - implement colorbox
/// 2011-09-05 added changes for ImageBaseUrl to support alternate file systems like Azure Blob Storage

using System;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using mojoPortal.FileSystem;
using Resources;


namespace mojoPortal.Web.GalleryUI
{
	
	public partial class GalleryControl : SiteModuleControl
    {
        #region Properties 

        protected string EditContentImage = WebConfigSettings.EditContentImage;
        private Literal imageLink;
        private mojoPortal.Business.Gallery gallery;
        private GalleryConfiguration config = new GalleryConfiguration();
        private string imageFolderPath;
        private int thumbsPerPage = 999;
        private int itemId = -1;
        private string ViewImagePage = "GalleryBrowse.aspx";
        private int totalRows = 0;
        //private bool UseSilverlightSlideshow = false;
        private string baseUrl = string.Empty;
        protected string thumnailBaseUrl = string.Empty;
        protected string webSizeBaseUrl = string.Empty;
        protected string fullSizeBaseUrl = string.Empty;
        protected string imageBaseUrl = string.Empty;
        private IFileSystem fileSystem = null;
        protected bool useViewState = true;

        //private bool useNivo = true;
        
        
        protected int PageNumber
        {
            get
            {
                object o = ViewState["PageNumber"];
                if (o != null)
                {
                    return Convert.ToInt32(o);
                }
                return 1;
            }
            set
            {
                ViewState["PageNumber"] = value;
                
            }
        }

        protected int TotalPages
        {
            get
            {
                object o = ViewState["TotalPages"];
                if (o != null)
                {
                    return Convert.ToInt32(o);
                }
                return 1;
            }
            set
            {
                ViewState["TotalPages"] = value;

            }
        }

        protected bool UseLightboxMode
        {
            get
            {
                object o = ViewState["UseLightboxMode"];
                if (o != null)
                {
                    return Convert.ToBoolean(o);
                }
                return true;
            }
            set
            {
                ViewState["UseLightboxMode"] = value;
            }
        }




        protected bool UseCompactMode
        {
            get
            {
                object o = ViewState["UseCompactMode"];
                if (o != null)
                {
                    return Convert.ToBoolean(o);
                }
                return false;
            }
            set
            {
                ViewState["UseCompactMode"] = value;

            }
        }

        #endregion

		protected void Page_Load(object sender, EventArgs e)
		{
            LoadSettings();

            //if (config.UseSlideShow)
            //{
            //    SetupSilverlight();
            //}
            //else
            //{

                if (!Page.IsPostBack)
                {
                    PopulateControls();
                }
            //}
		}

        //private void SetupSilverlight()
        //{
        //    pnlSl.Visible = true;
        //    pnlInnerBody.Visible = false;
        //    slideShow.XmlDataUrl = SiteRoot + "/Services/GalleryDataService.ashx?pageid=" + PageId.ToInvariantString()
        //                + "&amp;mid=" + ModuleId.ToInvariantString();

        //    slideShow.Theme = config.SlideShowTheme;
        //    slideShow.Height = config.SlideShowHeight;
        //    slideShow.Width = config.SlideShowWidth;
        //    slideShow.Windowless = config.SlideShowWindowlessMode;
        //}

        private void PopulateControls()
        {
            if (config.UseNivoSlider)
            {
                SetupNivo();
                return;
            }
            pager.CurrentIndex = 1;
            BindRepeater();
            BindImage();
        }

        private void SetupNivo()
        {
            upGallery.Visible = false;
            pnlNivoWrapper.Visible = true;

            pnlNivoWrapper.CssClass = "slider-wrapper " + displaySettings.NivoTheme;
            //pnlNivoWrapper.Attributes.Add("style", "width:" + config.WebSizeWidth.ToInvariantString() + "px;");

            using (IDataReader reader = gallery.GetAllImages())
            {
                rptNivo.DataSource = reader;
                rptNivo.DataBind();

            }

            SetupNivoScripts();

        }

        private void SetupNivoScripts()
        {
            if (Page is mojoBasePage)
            {
                mojoBasePage basePage = Page as mojoBasePage;
                basePage.ScriptConfig.IncludeNivoSlider = true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page),
                    "nivoslidermain", "\n<script src=\""
                    + Page.ResolveUrl("~/ClientScript/jqmojo/jquery.nivo.slider.pack3-2.js") + "\" type=\"text/javascript\"></script>", false);
            }

            string nivoInstanceScript = string.Format(CultureInfo.InvariantCulture, @"$(window).load(function () {{$('#{0}').nivoSlider({{{1}}});}});", pnlNivoInner.ClientID, displaySettings.NivoConfig);

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(), 
                "startSlider" + pnlNivoInner.ClientID, 
                nivoInstanceScript, 
                true);
        
        }

        //private string nivoTheme = "theme-default";

        //private string nivoConfig = string.Empty; //empty means it will use all the defaults

       

        protected string FormatNivoImage(string webImageFile, string fullSizeImageFile, string caption)
        {
            string imageUrl;
            if(displaySettings.NivoUseFullSizeImages) // false by default
            {
                imageUrl = Page.ResolveUrl(fullSizeBaseUrl + fullSizeImageFile);
            }
            else
            {
                imageUrl = Page.ResolveUrl(webSizeBaseUrl + webImageFile);
            }

            if (displaySettings.NivoLinkToFullSize)
            {
                return "\n<a href='" + Page.ResolveUrl(fullSizeBaseUrl + fullSizeImageFile) + "'><img src='" + imageUrl
                + "' alt='" + caption.HtmlEscapeQuotes()
                + "' title='" + caption.HtmlEscapeQuotes()
                + "' /></a>";
            }

            return "\n<img src='" + imageUrl
                + "' alt='" + caption.HtmlEscapeQuotes()
                + "' title='" + caption.HtmlEscapeQuotes()
                + "' />";
                //+ "' style='width:" + config.WebSizeWidth.ToInvariantString() + "px;' />";
        }


        private void BindRepeater()
        {
            DataTable dt = gallery.GetThumbsByPage(PageNumber, thumbsPerPage);

            if (dt.Rows.Count > 0)
            {
                TotalPages = Convert.ToInt32(dt.Rows[0]["TotalPages"]);
                itemId = Convert.ToInt32(dt.Rows[0]["ItemID"]);
                totalRows = thumbsPerPage * TotalPages;
            }

            //this handles issue: when redirected back to page from edit page
            //if you deleted the last image on the page an error occurs
            //so decrement the pageNumber
            while (PageNumber > TotalPages)
            {
                PageNumber -= 1;

                dt = gallery.GetThumbsByPage(PageNumber, thumbsPerPage);
                if (dt.Rows.Count > 0)
                {
                    TotalPages = Convert.ToInt32(dt.Rows[0]["TotalPages"]);
                    itemId = Convert.ToInt32(dt.Rows[0]["ItemID"]);
                }
            }

            if (TotalPages > 1)
            {
                if (this.RenderInWebPartMode)
                {
                    if (totalRows > this.thumbsPerPage)
                    {
                        Literal moreLink = new Literal();
                        moreLink.Text = "<a href='"
                            + SiteRoot
                            + "/" + ViewImagePage
                            + "?ItemID=" + itemId.ToInvariantString()
                            + "&amp;mid=" + ModuleId.ToInvariantString()
                            + "&amp;pageid=" + PageId.ToInvariantString()
                            + "&amp;pagenumber=" + PageNumber.ToInvariantString()
                            + "'>" + GalleryResources.GalleryWebPartMoreLink
                            + "</a>";

                        this.pnlGallery.Controls.Add(moreLink);
                        pager.Visible = false;
                    }

                }
                else
                {
                    pager.ShowFirstLast = true;
                    pager.PageSize = thumbsPerPage;
                    pager.PageCount = TotalPages;
                }
            }
            else
            {
                pager.Visible = false;
            }

            
            if (UseLightboxMode)
            {
                SetupColorbox(); 
            }

            this.rptGallery.DataSource = dt;
            this.rptGallery.DataBind();

        }

        

        private void SetupColorbox()
        {
           // mojoBasePage basePage = Page as mojoBasePage;
           // if (basePage != null) { basePage.ScriptConfig.IncludeColorBox = true; }

            StringBuilder script = new StringBuilder();

            script.Append("$(document).ready(function() {");
            script.Append("$(\".cbg");
            script.Append(ModuleId.ToInvariantString());
            script.Append("\").colorbox(");

            script.Append("{");

            script.Append("rel:'cbg" + ModuleId.ToInvariantString() + "'");

            script.Append(",current: \"" + GalleryResources.ImageCountJsFormat + "\"");
            script.Append(",previous: \"" + GalleryResources.Previous + "\"");
            script.Append(",next: \"" + GalleryResources.Next + "\"");
            script.Append(",close: \"" + GalleryResources.Close + "\"");

            if (config.ColorBoxTransition != GalleryConfiguration.ColorBoxDefaultTransition)
            {
                script.Append(",transition:\"" + config.ColorBoxTransition + "\"");
            }

            if (config.ColorBoxTransitionSpeed != GalleryConfiguration.ColorBoxDefaultTransitionSpeed)
            {
                script.Append(",speed:" + config.ColorBoxTransitionSpeed);
            }

            script.Append(",opacity:" + config.ColorBoxOpacity);
           
            if (config.ColorBoxUseSlideshow != GalleryConfiguration.ColorBoxDefaultUseSlideShow)
            {
                script.Append(",slideshow:" + config.ColorBoxUseSlideshow.ToString().ToLower());
            }

            if (config.ColorBoxUseSlideshow)
            {
                if (config.ColorBoxSlideShowStartAuto)
                {
                    script.Append(",open:true");
                }

                script.Append(",slideshowSpeed: " + config.ColorBoxSlideshowSpeed);
                
                if (config.ColorBoxSlideshowAuto != GalleryConfiguration.ColorBoxDefaultUseSlideShow)
                {
                    script.Append(",slideshowAuto: " + config.ColorBoxSlideshowAuto.ToString().ToLower());
                }

                script.Append(",slideshowStart: \"" + GalleryResources.StartSlideShow + "\"");
                script.Append(",slideshowStop: \"" + GalleryResources.StopSlideShow + "\"");

                
            }

            script.Append(",maxWidth:'95%',maxHeight:'95%'");

            script.Append("}");

            script.Append(");");
            script.Append("});");

            ScriptManager.RegisterStartupScript(Page, typeof(Page),
                    "img" + ModuleId.ToInvariantString(), "\n<script type=\"text/javascript\">\n"
                    + script.ToString()
                    + "\n</script>", false);

            // make it responsive
            //https://github.com/jackmoore/colorbox/issues/158

            script = new StringBuilder();

            script.Append("var resizeTimer; ");
            script.Append("$(window).resize(function(){");
            script.Append("if (resizeTimer) clearTimeout(resizeTimer); ");
            script.Append("resizeTimer = setTimeout(function() {");
            script.Append("if ($('#cboxOverlay').is(':visible')) {");
            script.Append("$.colorbox.load(true); ");
            script.Append("}");
            script.Append("}, 300)");
            script.Append("});");

            script.Append("window.addEventListener('orientationchange', function() {");
            script.Append("if($('#cboxOverlay').is(':visible')){");
            script.Append("$.colorbox.load(true); ");
            script.Append("}");
            script.Append("}, false); ");


            ScriptManager.RegisterStartupScript(Page, typeof(Page),
                   "colorboxresizer", "\n<script type=\"text/javascript\">\n"
                   + script.ToString()
                   + "\n</script>", false);
        }
        

        private void BindImage()
        {
            if (!UseCompactMode) { return; }
            if (itemId == -1) { return; }

            imageLink = new Literal();
           
            GalleryImage galleryImage = new GalleryImage(ModuleId, itemId);

            imageLink.Text = "<a onclick=\"window.open(this.href,'_blank');return false;\"  "
                + " title=\"" + Server.HtmlEncode(GalleryResources.GalleryWebImageAltText).HtmlEscapeQuotes()
                + "\" href=\"" + imageBaseUrl
                + Page.ResolveUrl(fullSizeBaseUrl
                + galleryImage.ImageFile) + "\" ><img src=\""
                + Page.ResolveUrl(webSizeBaseUrl
                + galleryImage.WebImageFile) + "\" alt=\""
                + Server.HtmlEncode(GalleryResources.GalleryWebImageAltText).HtmlEscapeQuotes() + "\" /></a>";

            
            pnlGallery.Controls.Clear();
            pnlGallery.Controls.Add(imageLink);
            lblCaption.Text = Page.Server.HtmlEncode(galleryImage.Caption);
            lblDescription.Text = galleryImage.Description;

            if ((config.ShowTechnicalData) && (galleryImage.MetaDataXml.Length > 0))
            {
                xmlMeta.DocumentContent = galleryImage.MetaDataXml;
                string xslPath = HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot() + "/ImageGallery/GalleryMetaData.xsl");
                xmlMeta.TransformSource = xslPath;

            }
            
        }

        

        void rptGallery_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "setimage":
                    itemId = Convert.ToInt32(e.CommandArgument);
                    BindImage();
                    BindRepeater();
                    upGallery.Update();
                    break;
            }

        }

        protected void pager_Command(object sender, CommandEventArgs e)
        {
            PageNumber = Convert.ToInt32(e.CommandArgument);
            pager.CurrentIndex = PageNumber;
       
            BindRepeater();
            BindImage();

            upGallery.Update();
        }

       
        

        //private int imageArrayIndex = 1;

        

        protected string GetThumnailImageLink(
            string itemId,
            string thumbnailFile,
            string webImageFile,
            string caption)
        {
            
            String link = string.Empty;

            if (UseLightboxMode)
            {
                link = "<a class='cbg" + ModuleId.ToInvariantString() + "' "
                    + "title=\"" + caption.HtmlEscapeQuotes()
                    + "\" href=\""
                    + Page.ResolveUrl(webSizeBaseUrl
                    + webImageFile) + "\">"
                    + GetThumnailMarkup(thumbnailFile, caption)
                    + "</a>";
               
            }

            return link;

        }

        //protected String GetFullSizeUrl(String imageFile)
        //{
        //    //return GetImageRoot() + "/FullSizeImages/" + imageFile;
        //    return GetImageRoot() + "/WebImages/" + imageFile;

        //}

        protected String GetThumnailUrl(string thumbnailFile)
        {
            //return imageBaseUrl + thumnailBaseUrl + thumbnailFile;
            return Page.ResolveUrl(thumnailBaseUrl + thumbnailFile);

        }

        protected String GetThumnailMarkup(string thumbnailFile, string caption)
        {
            if (caption == null) { caption = string.Empty; }

            return "<img  src=\"" + Page.ResolveUrl(thumnailBaseUrl
                + thumbnailFile) + "\" alt=\"" + caption.HtmlEscapeQuotes() + "\" />";

        }

        private void LoadSettings()
        {
            gallery = new mojoPortal.Business.Gallery(ModuleId);
            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            Title1.EditUrl = SiteRoot + "/ImageGallery/EditImage.aspx";
            Title1.EditText = GalleryResources.GalleryAddImageLabel;
            Title1.Visible = !this.RenderInWebPartMode;


            config = new GalleryConfiguration(Settings);

            if (IsEditable)
            {
                Title1.LiteralExtraMarkup = "&nbsp;<a href='"
                        + SiteRoot
                        + "/ImageGallery/BulkUpload.aspx?pageid=" + PageId.ToInvariantString()
                        + "&amp;mid=" + ModuleId.ToInvariantString()
                        + "' class='ModuleEditLink' title='" + GalleryResources.BulkUploadLink + "'>" + GalleryResources.BulkUploadLink + "</a>";
            }

            if (this.ModuleConfiguration != null)
            {
                Title = this.ModuleConfiguration.ModuleTitle;
                Description = this.ModuleConfiguration.FeatureName;
            }

            //UseSilverlightSlideshow = config.UseSlideShow;


            if (config.CustomCssClass.Length > 0) { pnlOuterWrap.SetOrAppendCss(config.CustomCssClass); }

            if (WebConfigSettings.ImageGalleryUseMediaFolder)
            {
                baseUrl = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/GalleryImages/" + ModuleId.ToInvariantString() + "/";
            }
            else
            {
                baseUrl = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/GalleryImages/" + ModuleId.ToInvariantString() + "/";
            }

            thumnailBaseUrl = baseUrl + "Thumbnails/";
            webSizeBaseUrl = baseUrl + "WebImages/";
            fullSizeBaseUrl = baseUrl + "FullSizeImages/";

            imageFolderPath = HttpContext.Current.Server.MapPath(baseUrl);
            thumbsPerPage = config.ThumbsPerPage;
            UseCompactMode = config.UseCompactMode;

            //if (RenderInWebPartMode)
            //{
            //    UseCompactMode = false;
            //    UseSilverlightSlideshow = false;
            //    thumbsPerPage = 6;
            //}

            if (UseCompactMode)
            {
                UseLightboxMode = false;
                pnlImageDetails.Visible = false;
            }
            else
            {
                UseLightboxMode = true;
                useViewState = false;
                rptGallery.EnableViewState = false;
            }

            if (UseLightboxMode)
            {
                mojoBasePage basePage = Page as mojoBasePage;
                if (basePage != null) 
                { 
                    basePage.ScriptConfig.IncludeColorBox = true;
                    basePage.ScriptConfig.IncludeImageFit = false; // this seems needed for win7 phone
                }
            }

            imageBaseUrl = ImageSiteRoot;

            FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
            if (p == null) { return; }

            fileSystem = p.GetFileSystem();
            if (fileSystem != null) { imageBaseUrl = fileSystem.FileBaseUrl; }

        }
       

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            pager.Command += new CommandEventHandler(pager_Command);
            rptGallery.ItemCommand += new RepeaterCommandEventHandler(rptGallery_ItemCommand);
#if NET35
            if (WebConfigSettings.DisablePageViewStateByDefault) {Page.EnableViewState = true; }
#endif
        }

    }
}
