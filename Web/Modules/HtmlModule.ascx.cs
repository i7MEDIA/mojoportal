// Author:					
// Created:				    2004-10-21
// Last Modified:		    2014-01-07
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
//  2011-02-18 Joe Davis added supprtfor the pager in the jquery cycle sideshow

using System;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using mojoPortal.SearchIndex;
using Resources;

namespace mojoPortal.Web.ContentUI
{

    public partial class HtmlModule : SiteModuleControl, IWorkflow
    {
       
        protected string EditContentImage = WebConfigSettings.EditContentImage;
        protected HtmlConfiguration config = new HtmlConfiguration();
        private bool isAdmin = false;
        private HtmlRepository repository = new HtmlRepository();
        private TimeZoneInfo timeZone = null;
        protected mojoPortal.Web.UI.Avatar.RatingType MaxAllowedGravatarRating = UI.Avatar.RatingType.PG;
        protected bool allowGravatars = false;
        protected bool disableAvatars = true;
        private mojoBasePage basePage;
        private ContentWorkflow workInProgress = null;


        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            basePage = Page as mojoBasePage;
            
        }

        
        #endregion

        protected void Page_Load(object sender, EventArgs e) 
		{
            LoadSettings();
            PopulateControls();
			
        }

        
		private void PopulateControls()
		{
            string htmlBody = String.Empty;
            bool retrieveHtml = true;
            HtmlContent html = null;
            
            if (EnableWorkflow)
            {
                //use mojo base page to see if user has toggled draft content:
                CmsPage cmsPage = this.Page as CmsPage;
                if (cmsPage != null)
                {
                    if (cmsPage.ViewMode == PageViewMode.WorkInProgress)
                    {
                        if (workInProgress != null)
                        {
                            Title1.WorkflowStatus = workInProgress.Status;
                            htmlBody = workInProgress.ContentText;
                            retrieveHtml = false;
                        }
                    }
                }
            }

            

            if (retrieveHtml)
            {
                //html not yet retrieved:
                
                html = repository.Fetch(ModuleId);
                if (html != null)
                {
                    htmlBody = html.Body;
                }
            }

            //see if the literal has already been added:
            Literal literalHtml = divContent.FindControl("literalHtml") as Literal;

            if (!String.IsNullOrEmpty(htmlBody))
            {
                if (literalHtml == null)
                {
                    literalHtml = new Literal();
                    literalHtml.ID = "literalHtml";
                    divContent.Controls.Add(literalHtml);
                }
                literalHtml.Text = htmlBody;
            }
            else if (literalHtml != null)
            {
                literalHtml.Text = String.Empty;
            }

            fbLike.Visible = (config.ShowFacebookLikeButton && !RenderInWebPartMode);
            fbLike.ColorScheme = config.FacebookLikeButtonTheme;
            fbLike.ShowFaces = config.FacebookLikeButtonShowFaces;
            fbLike.HeightInPixels = config.FacebookLikeButtonHeight;
            fbLike.WidthInPixels = config.FacebookLikeButtonWidth;
            fbLike.UrlToLike = WebUtils.ResolveServerUrl(SiteUtils.GetCurrentPageUrl());

            if (config.ShowCreatedBy || config.ShowCreatedDate) { ShowCreated(html, workInProgress); }
            if (config.ShowModifiedBy || config.ShowModifiedDate) { ShowModified(html, workInProgress); }

            

            switch (basePage.SiteInfo.AvatarSystem)
            {
                case "gravatar":
                    allowGravatars = true;
                    disableAvatars = false;
                    break;

                case "internal":
                    allowGravatars = false;
                    disableAvatars = false;
                    break;

                case "none":
                default:
                    allowGravatars = false;
                    disableAvatars = true;
                    break;

            }

            if (!config.ShowAuthorAvatar) { disableAvatars = true; }

            pnlAuthorInfo.Visible = config.ShowAuthorAvatar || config.ShowAuthorBio;

            if (html != null)
            {
                userAvatar.Email = html.CreatedByEmail;
                userAvatar.UserName = GetCreatedByName(html);
                userAvatar.UserId = html.AuthorUserId;
                userAvatar.AvatarFile = html.AuthorAvatar;
                spnAuthorBio.InnerHtml = html.AuthorBio;
                spnAuthorBio.Visible = config.ShowAuthorBio && (html.AuthorBio.Length > 0);
            }
            else if (workInProgress != null)
            {
                userAvatar.Email = workInProgress.AuthorEmail;
                userAvatar.UserName = GetCreatedByName(workInProgress);
                userAvatar.UserId = workInProgress.AuthorUserId;
                userAvatar.AvatarFile = workInProgress.AuthorAvatar;
                spnAuthorBio.InnerHtml = workInProgress.AuthorBio;
                spnAuthorBio.Visible = config.ShowAuthorBio && (workInProgress.AuthorBio.Length > 0);
            }

            userAvatar.MaxAllowedRating = MaxAllowedGravatarRating;
            userAvatar.Disable = disableAvatars;
            userAvatar.UseGravatar = allowGravatars;
            userAvatar.SiteId = basePage.SiteInfo.SiteId;
            userAvatar.UserNameTooltipFormat = displaySettings.AvatarUserNameTooltipFormat;
            userAvatar.UseLink = UseProfileLinkForAvatar();
            userAvatar.SiteRoot = SiteRoot;

		}

        protected bool UseProfileLinkForAvatar()
        {
            if (!displaySettings.LinkAuthorAvatarToProfile) { return false; }

            if (Request.IsAuthenticated)
            {
                // if we know the user is signed in and not in a role allowed then return username without a profile link
                if (!WebUser.IsInRoles(basePage.SiteInfo.RolesThatCanViewMemberList))
                {
                    return false;
                }
            }

            // if user is not authenticated we don't know if he will be allowed but he will be prompted to login first so its ok to show the link
            return true;
        }

        private void ShowCreated(HtmlContent html, ContentWorkflow workInProgress)
        {
            if((html == null) && (workInProgress == null)) { return;}

            string createdByUserDateFormat = Resource.CreatedByUserDateFormat;
            if (displaySettings.OverrideCreatedByUserDateFormat.Length > 0)
            {
                createdByUserDateFormat = displaySettings.OverrideCreatedByUserDateFormat;
            }

            string createdDateFormat = Resource.CreatedDateFormat;
            if (displaySettings.OverrideCreatedDateFormat.Length > 0)
            {
                createdDateFormat = displaySettings.OverrideCreatedDateFormat;
            }

            string createdByUserFormat = Resource.CreatedByUserFormat;
            if (displaySettings.OverrideCreatedByUserFormat.Length > 0)
            {
                createdByUserFormat = displaySettings.OverrideCreatedByUserFormat;
            }

            if (config.ShowCreatedBy && config.ShowCreatedDate)
            {
                if (html != null)
                {
                    litCreatedBy.Text = string.Format(CultureInfo.InvariantCulture,
                        createdByUserDateFormat,
                        html.CreatedDate.ToLocalTime(timeZone).ToString(displaySettings.DateFormat),
                        GetCreatedByName(html)
                        );
                }
                else if (workInProgress != null)
                {
                    litCreatedBy.Text = string.Format(CultureInfo.InvariantCulture,
                       createdByUserDateFormat,
                       workInProgress.CreatedDateUtc.ToLocalTime(timeZone).ToString(displaySettings.DateFormat),
                        GetCreatedByName(workInProgress)
                       );

                }
            }
            else if (config.ShowCreatedDate)
            {
                if (html != null)
                {
                    litCreatedBy.Text = string.Format(CultureInfo.InvariantCulture,
                            createdDateFormat,
                            html.CreatedDate.ToLocalTime(timeZone).ToString(displaySettings.DateFormat)
                            );
                }
                else if (workInProgress != null)
                {
                    litCreatedBy.Text = string.Format(CultureInfo.InvariantCulture,
                       createdDateFormat,
                       workInProgress.CreatedDateUtc.ToLocalTime(timeZone).ToString(displaySettings.DateFormat)
                       );

                }
            }
            else if (config.ShowCreatedBy)
            {
                if (html != null)
                {
                    litCreatedBy.Text = string.Format(CultureInfo.InvariantCulture,
                            createdByUserFormat,
                            GetCreatedByName(html)
                            );
                }
                else if (workInProgress != null)
                {
                    litCreatedBy.Text = string.Format(CultureInfo.InvariantCulture,
                       createdByUserFormat,
                       GetCreatedByName(workInProgress)
                       );

                }
            }

            pnlCreatedBy.Visible = true;

        }

        private string GetCreatedByName(HtmlContent html)
        {
            if (displaySettings.UseAuthorFirstAndLastName)
            {
                if ((html.CreatedByFirstName.Length > 0) && (html.CreatedByLastName.Length > 0))
                {
                    return html.CreatedByFirstName + " " + html.CreatedByLastName;
                }
            }

            if (html.CreatedByName.Length > 0)
            {
                return html.CreatedByName;
            }

            return html.LastModByName;
        }

        private string GetCreatedByName(ContentWorkflow workInProgress)
        {
            if (displaySettings.UseAuthorFirstAndLastName)
            {
                if ((workInProgress.CreatedByUserFirstName.Length > 0) && (workInProgress.CreatedByUserLastName.Length > 0))
                {
                    return workInProgress.CreatedByUserFirstName + " " + workInProgress.CreatedByUserLastName;
                }
            }

            if (workInProgress.CreatedByUserName.Length > 0)
            {
                return workInProgress.CreatedByUserName;
            }

            return workInProgress.LastModByUserName;
        }

        private void ShowModified(HtmlContent html, ContentWorkflow workInProgress)
        {
            if ((html == null) && (workInProgress == null)) { return; }

            string modifiedByUserDateFormat = Resource.ModifiedByUserDateFormat;
            if (displaySettings.OverrideModifiedByUserDateFormat.Length > 0)
            {
                modifiedByUserDateFormat = displaySettings.OverrideModifiedByUserDateFormat;
            }

            string modifiedDateFormat = Resource.ModifiedDateFormat;
            if (displaySettings.OverrideModifiedDateFormat.Length > 0)
            {
                modifiedDateFormat = displaySettings.OverrideModifiedDateFormat;
            }

            string modifiedByUserFormat = Resource.ModifiedByUserFormat;
            if (displaySettings.OverrideModifiedByUserFormat.Length > 0)
            {
                modifiedByUserFormat = displaySettings.OverrideModifiedByUserFormat;
            }

            if (config.ShowModifiedBy && config.ShowModifiedDate)
            {
                
                if (workInProgress != null)
                {
                    litModifiedBy.Text = string.Format(CultureInfo.InvariantCulture,
                       modifiedByUserDateFormat,
                       workInProgress.CreatedDateUtc.ToLocalTime(timeZone).ToString(displaySettings.DateFormat),
                       GetModifiedByName(workInProgress)
                       );

                }
                else if (html != null)
                {
                    litModifiedBy.Text = string.Format(CultureInfo.InvariantCulture,
                        modifiedByUserDateFormat,
                        html.LastModUtc.ToLocalTime(timeZone).ToString(displaySettings.DateFormat),
                        GetModifiedByName(html)
                        );
                }
            }
            else if (config.ShowModifiedDate)
            {
                
                if (workInProgress != null)
                {
                    litModifiedBy.Text = string.Format(CultureInfo.InvariantCulture,
                       modifiedDateFormat,
                       workInProgress.LastModUtc.ToLocalTime(timeZone).ToString(displaySettings.DateFormat)
                       );

                }
                else if (html != null)
                {
                    litModifiedBy.Text = string.Format(CultureInfo.InvariantCulture,
                            modifiedDateFormat,
                            html.LastModUtc.ToLocalTime(timeZone).ToString(displaySettings.DateFormat)
                            );
                }
            }
            else if (config.ShowModifiedBy)
            {
                
                if (workInProgress != null)
                {
                    litModifiedBy.Text = string.Format(CultureInfo.InvariantCulture,
                       modifiedByUserFormat,
                       GetModifiedByName(workInProgress)
                       );

                }
                else if (html != null)
                {
                    litModifiedBy.Text = string.Format(CultureInfo.InvariantCulture,
                            modifiedByUserFormat,
                            GetModifiedByName(html)
                            );
                }
            }

            pnlModifiedBy.Visible = true;

        }

        private string GetModifiedByName(HtmlContent html)
        {
            if (displaySettings.UseAuthorFirstAndLastName)
            {
                if ((html.LastModByFirstName.Length > 0) && (html.LastModByLastName.Length > 0))
                {
                    return html.LastModByFirstName + " " + html.LastModByLastName;
                }
            }

            return html.LastModByName;
        }

        private string GetModifiedByName(ContentWorkflow workInProgress)
        {
            if (displaySettings.UseAuthorFirstAndLastName)
            {
                if ((workInProgress.LastModByUserFirstName.Length > 0) && (workInProgress.LastModByUserLastName.Length > 0))
                {
                    return workInProgress.LastModByUserFirstName + " " + workInProgress.LastModByUserLastName;
                }
            }

            return workInProgress.LastModByUserName;
        }


        private void LoadSettings()
        {
            Title1.EditUrl = SiteRoot + "/HtmlEdit.aspx";
            Title1.EditText = Resource.EditImageAltText;
            Title1.ToolTip = Resource.EditImageAltText;
            
            Title1.Visible = !this.RenderInWebPartMode;

            timeZone = SiteUtils.GetUserTimeZone();

            if ((WebUser.IsAdminOrContentAdmin) || (SiteUtils.UserIsSiteEditor()))
            { 
                isAdmin = true;
                Title1.IsAdminEditor = isAdmin;
            }

            if (IsEditable)
            {
                TitleUrl = SiteRoot + "/HtmlEdit.aspx" + "?mid=" + ModuleId.ToInvariantString()
                        + "&pageid=" + currentPage.PageId.ToInvariantString();
            }

            config = new HtmlConfiguration(Settings);

            if (config.InstanceCssClass.Length > 0)  {
                pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass); 
            }
            
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            //pnlContainer.ModuleId = this.ModuleId;

            if (config.EnableSlideShow)
            {
                divContent.EnableSlideShow = true;
                divContent.EnableSlideClick = config.EnableSlideClick;
                divContent.Random = config.RandomizeSlides;
                divContent.CleartypeNoBg = !config.UseSlideClearTypeCorrections;
                divContent.SlideContainerClass = config.SlideContainerClass;
                divContent.PauseOnHover = config.PauseSlideOnHover;
                divContent.TransitionEffect = config.SlideTransitions;
                divContent.TransitionInterval = config.SlideDuration;
                divContent.Speed = config.TransitionSpeed;

                if (config.SlideContainerHeight > 0) { divContent.ContainerHeight = config.SlideContainerHeight.ToInvariantString() + "px"; }

                if (config.EnableSlideShowPager)
                {
                    divContent.Pager = "cyclenav";
                    if (config.SlideShowPagerBefore) { divContent.PagerBefore = true; }
                }

            }

            if (displaySettings.UseHtml5Elements)
            {
                if (displaySettings.UseOuterBodyForHtml5Article)
                {
                    pnlOuterBody.Element = "article";
                }
                else
                {
                    pnlInnerWrap.Element = "article";
                }
                //if (!IsPostBack)
                //{
                //    Title1.LiteralExtraTopContent += "<header>";
                //    Title1.LiteralExtraBottomContent += "</header>"; // this was a bug just use the theme value
                //}
            }

            if (EnableWorkflow)
            {
                //use mojo base page to see if user has toggled draft content:
                CmsPage cmsPage = this.Page as CmsPage;
                if (cmsPage != null)
                {
                    if (cmsPage.ViewMode == PageViewMode.WorkInProgress)
                    {
                        //try to get draft content:
                        workInProgress = ContentWorkflow.GetWorkInProgress(this.ModuleGuid);
                        
                    }
                }
            }

            if (IsEditable && WebConfigSettings.EnableInlineEditing)
            {
                if(siteSettings.EditorProviderName == "TinyMCEProvider" && !WebConfigSettings.DisableTinyMceInlineEditing)
                {
                    SetupTinyMceInline();
                }
                else if (siteSettings.EditorProviderName == "CKEditorProvider")
                {
                    SetupCKEditorInline();
                }
            }
        }

        private void SetupCKEditorInline()
        {
            if (WebConfigSettings.DisablejQuery) { return; }

            if (siteSettings.EditorProviderName != "CKEditorProvider") { return; }

            bool userCanOnlyEditAsDraft = basePage.UserCanOnlyEditModuleAsDraft(ModuleId, HtmlContent.FeatureGuid);

            if (basePage.ViewMode == PageViewMode.Live)
            {
                if (userCanOnlyEditAsDraft) { return; }
            }

            //http://stackoverflow.com/questions/13922002/ckeditor-inline-edit
            //http://stackoverflow.com/questions/12574664/save-edited-inline-text-from-ckeditor-4-asp-net

            //divContent.Attributes.Add("contenteditable", "true");

            ScriptManager.RegisterClientScriptBlock(
                this,
                this.GetType(),
                "ckeditormain",
				"\n<script data-loader=\"HTMLModule\" src=\""
				+ ResolveUrl(WebConfigSettings.CKEditorBasePath + "ckeditor.js") + "\"></script>", false);

            StringBuilder script = new StringBuilder();
            script.Append("\n<script data-loader=\"HTMLModule\">");
            script.Append("\n CKEDITOR.disableAutoInline = true; ");
            script.Append("\n</script>");

            ScriptManager.RegisterStartupScript(
                this,
                typeof(Page),
                "ckeditordisableautoinline",
                script.ToString(),
                false);

            // now setup the instance
            script = new StringBuilder();

            script.Append("\n<script data-loader=\"HTMLModule\">\n");

            script.Append("var hceditor" + ModuleId.ToInvariantString() + "; ");

            script.Append("function SetupCK" + ModuleId.ToInvariantString() + "(){");

            script.Append("$('#" + divContent.ClientID + "').attr('contenteditable', true); ");

            script.Append("hceditor" + ModuleId.ToInvariantString() + " = CKEDITOR.inline( document.getElementById( '" + divContent.ClientID + "') ");

            script.Append(", {");

            script.Append("customConfig : '" + WebUtils.ResolveServerUrl(WebConfigSettings.CKEditorConfigPath) + "' ");

            script.Append(",toolbar:'FullWithTemplates'");
            ////script.Append(",startupFocus : true");

            script.Append(",skin:'" + WebConfigSettings.CKEditorSkin + "'");
            script.Append(",bodyClass:'wysiwygeditor' ");
            if (WebConfigSettings.CKeditorSuppressTitle)
            {
                script.Append(",title:false");
            }
            

            string fileManagerUrl = SiteRoot + WebConfigSettings.FileDialogRelativeUrl;

            script.Append(",filebrowserWindowWidth : ~~((80 / 100) * screen.width)"); // 80% of window width
            script.Append(",filebrowserWindowHeight : ~~((80 / 100) * screen.height)"); // 80% of window height
            script.Append(",filebrowserBrowseUrl:'" + fileManagerUrl + "?editor=ckeditor&type=file'");
            script.Append(",filebrowserImageBrowseUrl:'" + fileManagerUrl + "?editor=ckeditor&type=image'");
            //script.Append(",filebrowserFlashBrowseUrl:'" + fileManagerUrl + "?editor=ckeditorck&type=media'");
            script.Append(",filebrowserImageBrowseLinkUrl:'" + fileManagerUrl + "?editor=ckeditor&type=file'");
            script.Append(",filebrowserWindowFeatures:'location=no,menubar=no,toolbar=no,dependent=yes,minimizable=no,modal=yes,alwaysRaised=yes,resizable=yes,scrollbars=yes'");

            string dropFileUploadUrl = SiteRoot + "/Services/FileService.ashx?cmd=uploadfromeditor&rz=true&ko=" 
                + WebConfigSettings.KeepFullSizeImagesDroppedInEditor.ToString().ToLower()
                    + "&t=" + Global.FileSystemToken.ToString(); 

            script.Append(",dropFileUploadUrl:'" + Page.ResolveUrl(dropFileUploadUrl) + "'");

            // commented out 2014-10-06 - should come from the config js
            //script.Append(",extraPlugins:'oembed,templates,mojofiledrop' ");

            script.Append(",smiley_path:'" + Page.ResolveUrl("~/Data/SiteImages/emoticons/") + "'");

            string styleJsonUrl = SiteRoot + "/Services/CKeditorStyles.ashx?cb=" + Guid.NewGuid().ToString().Replace("-", string.Empty);
            script.Append(",stylesCombo_stylesSet : 'mojo:" + styleJsonUrl + "'");

            string templatesUrl = SiteRoot + "/Services/CKeditorTemplates.ashx?cb=" + Guid.NewGuid().ToString();
            script.Append(",templates: 'mojo'");
            script.Append(",templates_files : ['" + templatesUrl + "']");
            script.Append(",templates_replaceContent :false");
         
            script.Append("} "); // end config object

            script.Append("); "); // end CKEDITOR.inline(

            script.Append("hceditor" + ModuleId.ToInvariantString() + ".on( 'blur', function( event ) { ");
            //script.Append("alert('blurr'); ");
            script.Append("if(hceditor" + ModuleId.ToInvariantString() + ".checkDirty()){");
            //script.Append("alert('dirty');");
            script.Append("var editorData" + ModuleId.ToInvariantString() + " = hceditor" + ModuleId.ToInvariantString() + ".getData(); ");
            //script.Append("alert(editorData" + ModuleId.ToInvariantString() + ");");

            //http://stackoverflow.com/questions/1078909/jquery-send-html-data-through-post

            script.Append("var data" + ModuleId.ToInvariantString() + " = { ");
            //script.Append("id: 'currid', ");
            script.Append("html: editorData" + ModuleId.ToInvariantString());
            script.Append("}; ");


            script.Append("$.ajax({ ");
            script.Append("type: 'POST',");

            //script.Append("processData:false,");

            script.Append("url: '" + SiteRoot + "/Services/HtmlEditService.aspx?pageid="
                + PageId.ToInvariantString()
                + "&mid=" + ModuleId.ToInvariantString() + "', ");

            //script.Append("data: editorData" + ModuleId.ToInvariantString());
            script.Append("data: data" + ModuleId.ToInvariantString());

            if ((EnableWorkflow) && (basePage.ViewMode == PageViewMode.WorkInProgress) && (workInProgress == null))
            {
                script.Append("}).complete(function( html ) {");
                // if user edits it will create a new draft.
                // need to reload the page so it shows the workflow toolbar items after edit
                script.Append("window.location.reload(true); ");
            }

            script.Append("}); ");

            script.Append("hceditor" + ModuleId.ToInvariantString() + ".resetDirty(); ");

            
            script.Append("}"); //end if checkdirty

            script.Append("});"); //end on blur

            script.Append("} "); //end SetupCkModuleId funtion


            script.Append("function DestroyCK" + ModuleId.ToInvariantString() + "() { ");

            script.Append("hceditor" + ModuleId.ToInvariantString() + ".destroy(); ");
            script.Append("$('#" + divContent.ClientID + "').attr('contenteditable', false); ");

            script.Append("} "); //end DestroyCkModuleId funtion


            script.Append("function ToggleCk" + ModuleId.ToInvariantString() + "()");
            script.Append("{ ");

            script.Append("if($('#mtogedit" + ModuleId.ToInvariantString() + "').hasClass('ui-icon-unlocked'))");
            script.Append("{ ");
            script.Append(" DestroyCK" + ModuleId.ToInvariantString() + "(); ");
            script.Append("$('#mtogedit" + ModuleId.ToInvariantString() + "').removeClass('ui-icon-unlocked'); ");
            script.Append("$('#mtogedit" + ModuleId.ToInvariantString() + "').addClass('ui-icon-locked'); ");

            script.Append("}else{ ");

            script.Append(" SetupCK" + ModuleId.ToInvariantString() + "(); ");
            script.Append("hceditor" + ModuleId.ToInvariantString() + ".focus(); ");

            script.Append("$('#mtogedit" + ModuleId.ToInvariantString() + "').removeClass('ui-icon-locked'); ");
            script.Append("$('#mtogedit" + ModuleId.ToInvariantString() + "').addClass('ui-icon-unlocked'); ");

            script.Append("} ");

            script.Append("} "); //end ToggleCk funtion



            script.Append("$(document).ready(function() {");

            if (!displaySettings.AddEditToggleToModuleTitle)
            {
                script.Append("$('#" + divContent.ClientID + "').after($(\"<a href='#' id='mtogedit"
                    + ModuleId.ToInvariantString() + "' "
                    + "title='" + HttpUtility.HtmlAttributeEncode(Resource.ToggleInlineEditing) + "' "
                    + " class='jqtt inlineedittoggle ui-icon ui-icon-locked'></a>\"));");

                script.Append(" $('#mtogedit" + ModuleId.ToInvariantString() + "').position({");
                script.Append("my:'" + displaySettings.EditorTogglePositionMy + "', ");
                script.Append("at:'" + displaySettings.EditorTogglePositionAt + "',");
                script.Append("of: '#" + divContent.ClientID + "'");
                script.Append("}); ");
            }

            script.Append(" $('#mtogedit" + ModuleId.ToInvariantString() + "').click(function() { ToggleCk" 
                + ModuleId.ToInvariantString() + "(); return false; });");

            script.Append("$('#mtogedit" + ModuleId.ToInvariantString() + "').tooltip(); ");

            script.Append("});"); 


           

            if (displaySettings.AddEditToggleToModuleTitle)
            {
                Title1.LiteralExtraMarkup =
                        " <a href='#' id='mtogedit" + ModuleId.ToInvariantString()
                        + "' title='" + HttpUtility.HtmlAttributeEncode(Resource.ToggleInlineEditing) 
                        + "' class='jqtt inlineedittoggle ui-icon ui-icon-locked'></a>"
                        ;
            }
            
            script.Append("\n</script>");

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                this.UniqueID,
                script.ToString(),
                false);

        

        }

        private void SetupTinyMceInline()
        {
            // this is work in progress not completed or functioning yet

            if (WebConfigSettings.DisablejQuery) { return; }
            if (WebConfigSettings.DisableTinyMceInlineEditing) { return; }

            //if (siteSettings.EditorProviderName != "CKEditorProvider") { return; }

            bool userCanOnlyEditAsDraft = basePage.UserCanOnlyEditModuleAsDraft(ModuleId, HtmlContent.FeatureGuid);

            if (basePage.ViewMode == PageViewMode.Live)
            {
                if (userCanOnlyEditAsDraft) { return; }
            }

            //http://stackoverflow.com/questions/13922002/ckeditor-inline-edit
            //http://stackoverflow.com/questions/12574664/save-edited-inline-text-from-ckeditor-4-asp-net

            //divContent.Attributes.Add("contenteditable", "true");

            ScriptManager.RegisterClientScriptBlock(
                this, 
                this.GetType(),
                "tinymcemain",
				$"\n<script data-loader=\"HTMLModule\" src=\"{ResolveUrl(WebConfigSettings.TinyMceBasePath + "tinymce.min.js")}\"></script>", false);

            TinyMceConfiguration config = TinyMceConfiguration.GetConfig();
            // need a clone beucause these are chached and we are modifying the object
            TinyMceSettings editorSettings = config.GetEditorSettings("FullWithTemplates").Clone();
            editorSettings.Inline = true;
            editorSettings.EditorAreaCSS = SiteUtils.GetEditorStyleSheetUrl(basePage.AllowSkinOverride, true, Page);
            editorSettings.TemplatesUrl = $"{SiteUtils.GetNavigationSiteRoot()}/Services/TinyMceTemplates.ashx?v={siteSettings.SkinVersion}";
            editorSettings.StyleFormats = TinyMceEditorAdapter.BuildTinyMceStyleJson();
			editorSettings.EmotionsBaseUrl = Page.ResolveUrl("~/Data/SiteImages/emoticons/tinymce/");
            editorSettings.FileManagerUrl = SiteRoot + WebConfigSettings.FileDialogRelativeUrl;
            editorSettings.GlobarVarToAssignEditor = "hceditor" + ModuleId.ToInvariantString();

            string imagesUploadUrl = Page.ResolveUrl($"{SiteRoot}/Services/FileService.ashx?cmd=uploadfromeditor&rz=true&ko={WebConfigSettings.KeepFullSizeImagesDroppedInEditor.ToString().ToLower()}&t={Global.FileSystemToken}");

            editorSettings.ImagesUploadUrl = imagesUploadUrl;

            if (WebConfigSettings.TinyMceInlineUseSavePlugin) // true by default
            {
                editorSettings.OnSaveCallback = "saveEditor" + ModuleId.ToInvariantString();
                if (!editorSettings.Plugins.StartsWith("save,"))
                {
                    editorSettings.Plugins = "save," + editorSettings.Plugins;
                }
                if (!editorSettings.Toolbar1Buttons.StartsWith("save "))
                {
                    editorSettings.Toolbar1Buttons = "save " + editorSettings.Toolbar1Buttons;
                }
                
            }
            if(WebConfigSettings.TinyMceInlineRemoveAutoSavePlugin)
            {
                if (editorSettings.Plugins.StartsWith("autosave,"))
                {
                    editorSettings.Plugins = editorSettings.Plugins.Replace("autosave,", string.Empty);
                }
                
            }
            

            StringBuilder script = new();
            script.Append("\n<script data-loader=\"HTMLModule\">\n");
            script.Append("var hceditor" + ModuleId.ToInvariantString() + "; ");
            script.Append("function SetupTinyMCE" + ModuleId.ToInvariantString() + "(){");
            script.Append("$('#" + divContent.ClientID + "').attr('contenteditable', true); ");

            TinyMceEditor.BuildScript(script, editorSettings, divContent.ClientID, Unit.Empty, Unit.Empty);

            if (WebConfigSettings.TinyMceInlineSaveOnBlur) // false by default
            {
                script.Append(" hceditor" + ModuleId.ToInvariantString() + ".on( 'blur', function( event ) { ");
                //script.Append("alert('blurr'); ");
                script.Append("if(hceditor" + ModuleId.ToInvariantString() + ".isDirty()){");
                //script.Append("alert('dirty');");
                script.Append(" saveEditor" + ModuleId.ToInvariantString() + "(); ");
                //script.Append("hceditor" + ModuleId.ToInvariantString() + ".isNotDirty = true; ");
                script.Append("}"); //end if checkdirty
                script.Append("});"); //end on blur
            }
            
            script.Append("} "); //end SetupCkModuleId funtion
            script.Append(" function saveEditor" + ModuleId.ToInvariantString() +" (){ ");
            script.Append("var editorData" + ModuleId.ToInvariantString() + " = hceditor" + ModuleId.ToInvariantString() + ".getContent(); ");

            //http://stackoverflow.com/questions/1078909/jquery-send-html-data-through-post

            script.Append("var data" + ModuleId.ToInvariantString() + " = { "); ;
            script.Append("html: editorData" + ModuleId.ToInvariantString());
            script.Append("}; ");


            script.Append("$.ajax({ ");
            script.Append("type: 'POST',");
            script.Append("url: '" + SiteRoot + "/Services/HtmlEditService.aspx?pageid="
                + PageId.ToInvariantString()
                + "&mid=" + ModuleId.ToInvariantString() + "', ");

            script.Append("data: data" + ModuleId.ToInvariantString());

            if ((EnableWorkflow) && (basePage.ViewMode == PageViewMode.WorkInProgress) && (workInProgress == null))
            {
                script.Append("}).complete(function( html ) {");
                // if user edits it will create a new draft.
                // need to reload the page so it shows the workflow toolbar items after edit
                script.Append("window.location.reload(true);  ");
               // script.Append("});");
            }

            script.Append("}); ");
            script.Append("hceditor" + ModuleId.ToInvariantString() + ".isNotDirty = true; ");
            script.Append(" }"); // end save


            script.Append("function DestroyTinyMCE" + ModuleId.ToInvariantString() + "() { ");

            script.Append("hceditor" + ModuleId.ToInvariantString() + ".destroy(); ");
            script.Append("$('#" + divContent.ClientID + "').attr('contenteditable', false); ");

            script.Append("} "); //end DestroyTinyMCEModuleId function


            script.Append("function ToggleTinyMCE" + ModuleId.ToInvariantString() + "()");
            script.Append("{ ");

            script.Append("if($('#mtogedit" + ModuleId.ToInvariantString() + "').hasClass('ui-icon-unlocked'))");
            script.Append("{ ");
            script.Append(" DestroyTinyMCE" + ModuleId.ToInvariantString() + "(); ");
            script.Append("$('#mtogedit" + ModuleId.ToInvariantString() + "').removeClass('ui-icon-unlocked'); ");
            script.Append("$('#mtogedit" + ModuleId.ToInvariantString() + "').addClass('ui-icon-locked'); ");

            script.Append("}else{ ");

            //script.Append("try{");
            script.Append(" SetupTinyMCE" + ModuleId.ToInvariantString() + "(); ");
            
            //script.Append("}catch(err){");
            //script.Append(" alert(err); ");
            //script.Append("}");
            

            script.Append("$('#mtogedit" + ModuleId.ToInvariantString() + "').removeClass('ui-icon-locked'); ");
            script.Append("$('#mtogedit" + ModuleId.ToInvariantString() + "').addClass('ui-icon-unlocked'); ");

            script.Append("} ");

            script.Append("} "); //end ToggleCk funtion



            script.Append("$(document).ready(function() {");

            if (!displaySettings.AddEditToggleToModuleTitle)
            {
                script.Append("$('#" + divContent.ClientID + "').after($(\"<a href='#' id='mtogedit"
                    + ModuleId.ToInvariantString() + "' "
                    + "title='" + HttpUtility.HtmlAttributeEncode(Resource.ToggleInlineEditing) + "' "
                    + " class='jqtt inlineedittoggle ui-icon ui-icon-locked'></a>\"));");

                script.Append(" $('#mtogedit" + ModuleId.ToInvariantString() + "').position({");
                script.Append("my:'" + displaySettings.EditorTogglePositionMy + "', ");
                script.Append("at:'" + displaySettings.EditorTogglePositionAt + "',");
                script.Append("of: '#" + divContent.ClientID + "'");
                script.Append("}); ");
            }

            script.Append(" $('#mtogedit" + ModuleId.ToInvariantString() + "').click(function() { ToggleTinyMCE"
                + ModuleId.ToInvariantString() + "(); return false; });");

            script.Append("$('#mtogedit" + ModuleId.ToInvariantString() + "').tooltip(); ");

            script.Append("});");




            if (displaySettings.AddEditToggleToModuleTitle)
            {
                Title1.LiteralExtraMarkup =
                        " <a href='#' id='mtogedit" + ModuleId.ToInvariantString()
                        + "' title='" + HttpUtility.HtmlAttributeEncode(Resource.ToggleInlineEditing)
                        + "' class='jqtt inlineedittoggle ui-icon ui-icon-locked'></a>"
                        ;
            }

            script.Append("\n</script>");

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                this.UniqueID,
                script.ToString(),
                false);



        }

        void html_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["HtmlContentIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        #region IWorkflow Members

        public void SubmitForApproval()
        {
            if (currentPage == null) { return; }

            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser == null) { return; }

            ContentWorkflow workInProgress = ContentWorkflow.GetWorkInProgress(ModuleGuid);
            if (workInProgress == null) { return; }

            //Module module = new Module(workInProgress.ModuleId);

            workInProgress.Status = ContentWorkflowStatus.AwaitingApproval;
            if (WebConfigSettings.Use3LevelContentWorkflow)
            {
                workInProgress.LastModUserGuid = currentUser.UserGuid;
                workInProgress.LastModUtc = DateTime.UtcNow;
            }
            workInProgress.Save();

            if (!WebConfigSettings.DisableWorkflowNotification)
            {
                //string approverRoles = currentPage.EditRoles + ModuleConfiguration.AuthorizedEditRoles;
                string approverRoles = WebConfigSettings.Use3LevelContentWorkflow //joe davis
                    ? currentPage.DraftApprovalRoles + ModuleConfiguration.DraftApprovalRoles
                    : currentPage.EditRoles + ModuleConfiguration.AuthorizedEditRoles;

                WorkflowHelper.SendApprovalRequestNotification(
                    SiteUtils.GetSmtpSettings(),
                    siteSettings,
                    currentUser,
                    workInProgress,
                    approverRoles,
                    SiteUtils.GetCurrentPageUrl());
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);

            
        }

        public void CancelChanges()
        {
            
            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser == null) { return; }

            ContentWorkflow workInProgress = ContentWorkflow.GetWorkInProgress(ModuleGuid);
            if (workInProgress == null) { return; }

            workInProgress.Status = ContentWorkflowStatus.Cancelled;
            workInProgress.LastModUserGuid = currentUser.UserGuid;
            workInProgress.LastModUtc = DateTime.UtcNow;
            workInProgress.Save();

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        public void Approve()
        {
            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser == null) { return; }

            HtmlContent html = repository.Fetch(ModuleId);
            if (html != null)
            {
                if (!WebConfigSettings.Use3LevelContentWorkflow)
                {
                    html.ContentChanged += new ContentChangedEventHandler(html_ContentChanged);
                    html.ApproveContent(siteSettings.SiteGuid, currentUser.UserGuid, WebConfigSettings.WorkflowShowPublishForUnSubmittedDraft);
                }
                else
                {
                    Do3LevelApproval(html, currentUser);
                }
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        private void Do3LevelApproval(HtmlContent html, SiteUser currentUser)
        {
            ContentWorkflow workInProgress = ContentWorkflow.GetWorkInProgress(ModuleGuid);
            if (workInProgress != null)
            {
                if (workInProgress.Status == ContentWorkflowStatus.AwaitingPublishing)
                {
                    html.ContentChanged += new ContentChangedEventHandler(html_ContentChanged);

                    if (workInProgress.Status == ContentWorkflowStatus.AwaitingPublishing)
                    {
                        html.PublishApprovedContent(siteSettings.SiteGuid, currentUser.UserGuid);
                    }
                    else
                    {
                        html.ApproveContent(siteSettings.SiteGuid, currentUser.UserGuid, false);
                    }
                }
                else
                {
                    SiteUser draftSubmitter = new SiteUser(siteSettings, workInProgress.RecentActionByUserLogin);
                    html.ApproveContentForPublishing(siteSettings.SiteGuid, currentUser.UserGuid);
                    //Module module = new Module(workInProgress.ModuleId);
                    //string publisherRoles = currentPage.EditRoles + module.AuthorizedEditRoles;
                    string publisherRoles = currentPage.EditRoles + ModuleConfiguration.AuthorizedEditRoles;

                    if (!WebConfigSettings.DisableWorkflowNotification)
                    {
                        WorkflowHelper.SendPublishRequestNotification(
                            SiteUtils.GetSmtpSettings(),
                            siteSettings,
                            draftSubmitter,
                            currentUser,
                            workInProgress,
                            publisherRoles,
                            SiteUtils.GetCurrentPageUrl());
                    }
                }
            }
        }

        #endregion

    }


    public class HtmlDisplaySettings : WebControl
    {
        private string editorTogglePositionMy = "left bottom";

        public string EditorTogglePositionMy
        {
            get { return editorTogglePositionMy; }
            set { editorTogglePositionMy = value; }
        }

        private string editorTogglePositionAt = "left top";

        public string EditorTogglePositionAt
        {
            get { return editorTogglePositionAt; }
            set { editorTogglePositionAt = value; }
        }

        private string editToggleCommonCssClass = "inlineedittoggle ui-icon";

        public string EditToggleCommonCssClass
        {
            get { return editToggleCommonCssClass; }
            set { editToggleCommonCssClass = value; }
        }

        private string editToggleLockCssClass = "ui-icon-lock";

        public string EditToggleLockCssClass
        {
            get { return editToggleLockCssClass; }
            set { editToggleLockCssClass = value; }
        }

        private string editToggleUnLockCssClass = "ui-icon-unlock";

        public string EditToggleUnLockCssClass
        {
            get { return editToggleUnLockCssClass; }
            set { editToggleUnLockCssClass = value; }
        }

        private bool addEditToggleToModuleTitle = true;

        public bool AddEditToggleToModuleTitle
        {
            get { return addEditToggleToModuleTitle; }
            set { addEditToggleToModuleTitle = value; }
        }

        private string overrideCreatedByUserDateFormat = string.Empty;
        /// <summary>
        /// example: Created {0} by {1}
        /// </summary>
        public string OverrideCreatedByUserDateFormat
        {
            get { return overrideCreatedByUserDateFormat; }
            set { overrideCreatedByUserDateFormat = value; }
        }

        private string overrideCreatedDateFormat = string.Empty;
        /// <summary>
        /// example: Created {0}
        /// </summary>
        public string OverrideCreatedDateFormat
        {
            get { return overrideCreatedDateFormat; }
            set { overrideCreatedDateFormat = value; }
        }

        private string overrideCreatedByUserFormat = string.Empty;
        /// <summary>
        /// example: Created by {0}
        /// </summary>
        public string OverrideCreatedByUserFormat
        {
            get { return overrideCreatedByUserFormat; }
            set { overrideCreatedByUserFormat = value; }
        }

        private string overrideModifiedByUserDateFormat = string.Empty;
        /// <summary>
        /// example: Modified {0} by {1}
        /// </summary>
        public string OverrideModifiedByUserDateFormat
        {
            get { return overrideModifiedByUserDateFormat; }
            set { overrideModifiedByUserDateFormat = value; }
        }

        private string overrideModifiedDateFormat = string.Empty;
        /// <summary>
        /// example: Modified {0}
        /// </summary>
        public string OverrideModifiedDateFormat
        {
            get { return overrideModifiedDateFormat; }
            set { overrideModifiedDateFormat = value; }
        }

        private string overrideModifiedByUserFormat = string.Empty;
        /// <summary>
        /// example: Modified by {0}
        /// </summary>
        public string OverrideModifiedByUserFormat
        {
            get { return overrideModifiedByUserFormat; }
            set { overrideModifiedByUserFormat = value; }
        }

        private string dateFormat = "d";

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/az4se3k1.aspx
        /// </summary>
        public string DateFormat
        {
            get { return dateFormat; }
            set { dateFormat = value; }
        }

        private bool useAuthorFirstAndLastName = false;
        /// <summary>
        /// if true will try to use the first and last name of created by and last mod user
        /// will fallback to disoplayname if those are empty
        /// </summary>
        public bool UseAuthorFirstAndLastName
        {
            get { return useAuthorFirstAndLastName; }
            set { useAuthorFirstAndLastName = value; }
        }

        private bool disableContentRating = false;

        public bool DisableContentRating
        {
            get { return disableContentRating; }
            set { disableContentRating = value; }
        }

        
        private bool useBottomContentRating = false;

        public bool UseBottomContentRating
        {
            get { return useBottomContentRating; }
            set { useBottomContentRating = value; }
        }


        private bool useHtml5Elements = false;

        public bool UseHtml5Elements
        {
            get { return useHtml5Elements; }
            set { useHtml5Elements = value; }
        }

        private bool useOuterBodyForHtml5Article = false;

        public bool UseOuterBodyForHtml5Article
        {
            get { return useOuterBodyForHtml5Article; }
            set { useOuterBodyForHtml5Article = value; }
        }

        private bool linkAuthorAvatarToProfile = false;

        public bool LinkAuthorAvatarToProfile
        {
            get { return linkAuthorAvatarToProfile; }
            set { linkAuthorAvatarToProfile = value; }
        }

        private string avatarUserNameTooltipFormat = "View User Profile for {0}";

        public string AvatarUserNameTooltipFormat
        {
            get { return avatarUserNameTooltipFormat; }
            set { avatarUserNameTooltipFormat = value; }
        }

        //private bool showAuthorAvatar = false;

        //public bool ShowAuthorAvatar
        //{
        //    get { return showAuthorAvatar; }
        //    set { showAuthorAvatar = value; }
        //}

        //private bool showAuthorBio = false;

        //public bool ShowAuthorBio
        //{
        //    get { return showAuthorBio; }
        //    set { showAuthorBio = value; }
        //}

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            // nothing to render
        }
    }


}
