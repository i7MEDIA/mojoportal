// Author:					Joe Audette
// Created:					2010-11-27
// Last Modified:			2010-11-29
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Web.UI;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Flickr.UI
{

    public partial class FlickrGalleryModule : SiteModuleControl
    {
        // FeatureGuid 6d761e31-6378-4621-94c2-4439e714d28b

        private FlickrGalleryConfiguration config = new FlickrGalleryConfiguration();

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);

        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            LoadSettings();
            PopulateLabels();
            PopulateControls();
            if (IsConfigured())
            {
                SetupScript();
            }
            else
            {
                pnlNotConfigured.Visible = true;
            }

        }

        private void PopulateControls()
        {
            TitleControl.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }


        }

        private void SetupScript()
        {
            //http://pupunzi.com/mb.components/mb.gallery/demo/demo_flickr.html

            StringBuilder script = new StringBuilder();

            pnlInnerBody.RenderId = true;
            pnlInnerBody.RenderContentsOnly = false;

            script.Append("$(document).ready(function() { ");
           
            script.Append("$.printOutGallery=function(gallery){ ");
            script.Append("if (gallery.isInit==undefined){ ");
            script.Append("clearTimeout(document.flickr.tracer); ");
            script.Append("$(\"#" + pnlInnerBody.ClientID + " div.fthumbs\").empty(); ");
            
            script.Append("$(gallery).empty(); ");
            script.Append("if(! $(\"#" + pnlInnerBody.ClientID + " div.controls div.galleryinfo\").html()){ ");
            script.Append("var pages=$(\"<div class='pagesIDX'/>\"); ");

            script.Append("for(var i=1;i<=gallery.pages;i++){ ");
            script.Append("var page=$(\"<span class='pages btn' page='\"+i+\"'>\"+i+\"</span>\").click(function(){ ");
            script.Append("if($(this).hasClass(\"sel\")) return; ");
            script.Append("gallery.isInit=undefined; ");
            script.Append("$(\".pagesIDX span\").removeClass(\"sel\"); ");
            script.Append("$(this).addClass(\"sel\"); ");
            script.Append("document.flickr.page=$(this).attr(\"page\"); ");
            script.Append("$('#" + pnlGalleryContainer.ClientID + "').mb_loadFlickrPhotos({page:document.flickr.page}); ");
            script.Append("}); ");

            script.Append("pages.append(page); ");

            script.Append("if(gallery.pages == 1) { $('#gallerynav').children().hide(); }");

            script.Append("} ");

            script.Append("$(\"#" + pnlInnerBody.ClientID + " div.controls div.galleryinfo\").html(pages); ");
            script.Append("$(\".pagesIDX span\").removeClass(\"sel\"); ");
            script.Append("$(\".pagesIDX span[page='1']\").addClass(\"sel\"); ");
            script.Append("} ");

            script.Append("$(gallery.photos).each(function(i){ ");
            script.Append("var photoURL = this.source; ");
            script.Append("var photoW = this.sourceWidth; ");
            script.Append("var photoH = this.sourceHeight; ");
            script.Append("if($.mbFlickr.defaults.size==\"medium\"){ ");
            script.Append("photoURL=this.medium; ");
            script.Append("photoW= this.mediumWidth; ");
            script.Append("photoH= this.mediumHeight; ");
            script.Append("} ");

            script.Append("var t=$(\"<img src='\"+this.square+\"'>\").css({opacity:0,width:this.squareWidth,height:this.squareHeight}).load(function(){ $(this).fadeTo(1000,1);}); ");
            script.Append("t.click(function(){ ");
            script.Append("$(gallery).mbGallery({startFrom:i,autoSlide:false}); ");
            script.Append("}); ");

            script.Append("$(\"#" + pnlInnerBody.ClientID + " div.fthumbs\").append(t); ");

            script.Append("var thumb=$(\"<a href='\"+this.square+\"'>&nbsp;</a>\").addClass(\"imgThumb\").attr({w:photoW,h:photoH}); ");

            script.Append("$(gallery).append(thumb); ");
            script.Append("var full=$(\"<a href='\"+photoURL+\"'>&nbsp;</a>\").addClass(\"imgFull\"); ");
            script.Append("$(gallery).append(full); ");
            script.Append("var flickrUrl=$(\"<a href='\"+this.url+\"' target='_blank'></a>\").html(this.title); ");
            script.Append("var imgTitle=$(\"<div/>\").addClass(\"imgTitle\").html(flickrUrl); ");
            script.Append("$(gallery).append(imgTitle); ");
            script.Append("var desc= typeof this.description ==\"undefined\"?\"\":this.description; ");
            script.Append("var imgDesc=$(\"<div/>\").addClass(\"imgDesc\").html(desc); ");
            script.Append("$(gallery).append(imgDesc); ");
            script.Append("}); ");
            script.Append("}else{ ");
            script.Append("$(gallery).mbGallery(); ");
            script.Append("}");
            script.Append("}; ");


            script.Append("$.loader = function(){ ");
            script.Append("document.flickr.loaded=0; ");
            script.Append("document.flickr.tracer= setInterval(function(){ ");
            
            string loadingMessage = string.Format(FlickrResources.LoadingFormat, "\"+document.flickr.page+\"", "\"+document.flickr.loaded+\"", "\"+document.flickr.total+\"");

            script.Append("$(\"#" + pnlInnerBody.ClientID + " div.fthumbs\").html(\"<span class='msg'>" + loadingMessage + "</span>\"); ");
            
            script.Append("},500); ");
            script.Append("}; ");

            script.Append("$(\"#" + pnlInnerBody.ClientID + " div.controls span.fnext\").click(function(){ ");
            script.Append("document.flickr.page++; ");
            script.Append("$(\".pagesIDX span\").removeClass(\"sel\"); ");
            script.Append("$(\".pagesIDX span[page='\"+document.flickr.page+\"']\").addClass(\"sel\"); ");
            script.Append("$('#" + pnlGalleryContainer.ClientID + "').get(0).isInit=undefined; ");
            script.Append("$('#" + pnlGalleryContainer.ClientID + "').mb_loadFlickrPhotos({page:document.flickr.page}); ");
            script.Append("}); ");


            script.Append("$(\"#" + pnlInnerBody.ClientID + " div.controls span.fprev\").click(function(){ ");
            script.Append("document.flickr.page--; ");
            script.Append("$(\".pagesIDX span\").removeClass(\"sel\"); ");
            script.Append("$(\".pagesIDX span[page='\"+document.flickr.page+\"']\").addClass(\"sel\"); ");
            script.Append("$('#" + pnlGalleryContainer.ClientID + "').get(0).isInit=undefined; ");
            script.Append("$('#" + pnlGalleryContainer.ClientID + "').mb_loadFlickrPhotos({page:document.flickr.page}); ");
            script.Append("}); ");

            script.Append("document.flickr.page=0; ");

            script.Append("$.mbGallery.defaults.galleryTitle=\"" + this.ModuleConfiguration.ModuleTitle.HtmlEscapeQuotes() + "\"; ");
            script.Append("$.mbGallery.defaults.cssURL=\"" + Page.ResolveUrl("~/ClientScript/jqueryflickr/css/") + "\"; ");
            script.Append("$.mbGallery.defaults.skin=\"" + config.Theme + "\"; "); //black or white
            
            script.Append("$.mbFlickr.flickr_api_key=\"" + config.FlickrApiKey + "\"; ");
            //script.Append("$.mbFlickr.defaults.size=\"medium\"; ");
            script.Append("$.mbFlickr.defaults.per_page=" + config.PageSize.ToInvariantString() + "; ");
            script.Append("$.mbFlickr.defaults.onStart=$.loader; ");

            // you can either show a specific set or the user photo stream
            if (config.FlickrSetId.Length > 0)
            {
                script.Append("$.mbFlickr.defaults.flickr_photoset_id='" + config.FlickrSetId + "'; ");
            }
            else
            {
                script.Append("$.mbFlickr.defaults.flickr_user_id=\"" + config.FlickrUserId + "\"; ");
            }
            script.Append("$.mbFlickr.defaults.callback=$.printOutGallery; ");
            script.Append("$(\"#" + pnlInnerBody.ClientID + " div.controls span.fnext\").click(); ");
            script.Append("}); ");
            

            Page.ClientScript.RegisterStartupScript(typeof(Page),
                   "flickrgal" + ModuleId.ToInvariantString(), "\n<script type=\"text/javascript\" >"
                   + script.ToString() + "</script>");

        }


        private void PopulateLabels()
        {
            litPrevious.Text = FlickrResources.Prev;
            litNext.Text = FlickrResources.Next;
            lblNotConfigured.Text = FlickrResources.FlcikrGalleryNotConfigured;
        }

        private bool IsConfigured()
        {
            if (
                (config.FlickrApiKey.Length > 0)
                && ((config.FlickrUserId.Length > 0)||(config.FlickrSetId.Length > 0))
                )
            {
                return true;
            }

            return false;
        }


        private void LoadSettings()
        {
            mojoBasePage basePage = Page as mojoBasePage;
            if (basePage != null)
            {
                //enable the main script
                basePage.ScriptConfig.IncludeFlickrGallery = true;
                basePage.ScriptConfig.IncludeJQueryMigrate = true;
            }

            config = new FlickrGalleryConfiguration(Settings);

        }


    }
}