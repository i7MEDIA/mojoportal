using System;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using static mojoPortal.Web.UI.Avatar;

namespace mojoPortal.Web.ContentUI;

public partial class HtmlModule : SiteModuleControl, IWorkflow
{

	protected string EditContentImage = WebConfigSettings.EditContentImage;
	protected HtmlConfiguration config = new();
	private bool isAdmin = false;
	private HtmlRepository repository = new();
	private TimeZoneInfo timeZone = null;
	protected RatingType MaxAllowedGravatarRating = RatingType.PG;
	protected bool allowGravatars = false;
	protected bool disableAvatars = true;
	private mojoBasePage basePage;
	private ContentWorkflow workInProgress = null;


	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		basePage = Page as mojoBasePage;
	}


	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		PopulateControls();
	}


	private void PopulateControls()
	{
		string htmlBody = string.Empty;
		bool retrieveHtml = true;
		HtmlContent html = null;

		if (EnableWorkflow)
		{
			//use mojo base page to see if user has toggled draft content:
			if (Page is CmsPage cmsPage)
			{
				if (cmsPage.ViewMode == PageViewMode.WorkInProgress)
				{
					if (workInProgress is not null)
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
			if (html is not null)
			{
				htmlBody = html.Body;
			}
		}

		//see if the literal has already been added:
		var literalHtml = divContent.FindControl("literalHtml") as Literal;

		if (!string.IsNullOrEmpty(htmlBody))
		{
			if (literalHtml is null)
			{
				literalHtml = new Literal
				{
					ID = "literalHtml",
					Text = htmlBody
				};
				divContent.Controls.Add(literalHtml);
			}
		}
		else if (literalHtml is not null)
		{
			literalHtml.Text = string.Empty;
		}

		fbLike.Visible = config.ShowFacebookLikeButton;
		fbLike.ColorScheme = config.FacebookLikeButtonTheme;
		fbLike.ShowFaces = config.FacebookLikeButtonShowFaces;
		fbLike.HeightInPixels = config.FacebookLikeButtonHeight;
		fbLike.WidthInPixels = config.FacebookLikeButtonWidth;
		fbLike.UrlToLike = WebUtils.ResolveServerUrl(SiteUtils.GetCurrentPageUrl());

		if (config.ShowCreatedBy || config.ShowCreatedDate)
		{
			ShowCreated(html, workInProgress);
		}

		if (config.ShowModifiedBy || config.ShowModifiedDate)
		{
			ShowModified(html, workInProgress);
		}

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

		if (!config.ShowAuthorAvatar)
		{
			disableAvatars = true;
		}

		pnlAuthorInfo.Visible = config.ShowAuthorAvatar || config.ShowAuthorBio;

		if (html is not null)
		{
			userAvatar.Email = html.CreatedByEmail;
			userAvatar.UserName = GetCreatedByName(html);
			userAvatar.UserId = html.AuthorUserId;
			userAvatar.AvatarFile = html.AuthorAvatar;
			spnAuthorBio.InnerHtml = html.AuthorBio;
			spnAuthorBio.Visible = config.ShowAuthorBio && (html.AuthorBio.Length > 0);
		}
		else if (workInProgress is not null)
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
		if (!displaySettings.LinkAuthorAvatarToProfile)
		{
			return false;
		}

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
		if (html is null && workInProgress is null)
		{
			return;
		}

		string createdByUserDateFormat = Resource.CreatedByUserDateFormat;
		if (!string.IsNullOrWhiteSpace(displaySettings.OverrideCreatedByUserDateFormat))
		{
			createdByUserDateFormat = displaySettings.OverrideCreatedByUserDateFormat;
		}

		string createdDateFormat = Resource.CreatedDateFormat;
		if (!string.IsNullOrWhiteSpace(displaySettings.OverrideCreatedDateFormat))
		{
			createdDateFormat = displaySettings.OverrideCreatedDateFormat;
		}

		string createdByUserFormat = Resource.CreatedByUserFormat;
		if (!string.IsNullOrWhiteSpace(displaySettings.OverrideCreatedByUserFormat))
		{
			createdByUserFormat = displaySettings.OverrideCreatedByUserFormat;
		}

		if (config.ShowCreatedBy && config.ShowCreatedDate)
		{
			if (html is not null)
			{
				litCreatedBy.Text = string.Format(CultureInfo.InvariantCulture,
					createdByUserDateFormat,
					html.CreatedDate.ToLocalTime(timeZone).ToString(displaySettings.DateFormat),
					GetCreatedByName(html)
					);
			}
			else if (workInProgress is not null)
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
			if (html is not null)
			{
				litCreatedBy.Text = string.Format(CultureInfo.InvariantCulture,
						createdDateFormat,
						html.CreatedDate.ToLocalTime(timeZone).ToString(displaySettings.DateFormat)
						);
			}
			else if (workInProgress is not null)
			{
				litCreatedBy.Text = string.Format(CultureInfo.InvariantCulture,
				   createdDateFormat,
				   workInProgress.CreatedDateUtc.ToLocalTime(timeZone).ToString(displaySettings.DateFormat)
				   );
			}
		}
		else if (config.ShowCreatedBy)
		{
			if (html is not null)
			{
				litCreatedBy.Text = string.Format(CultureInfo.InvariantCulture,
						createdByUserFormat,
						GetCreatedByName(html)
						);
			}
			else if (workInProgress is not null)
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
			if (!string.IsNullOrWhiteSpace(html.CreatedByFirstName)
				&& !string.IsNullOrWhiteSpace(html.CreatedByLastName))
			{
				return $"{html.CreatedByFirstName} {html.CreatedByLastName}";
			}
		}

		if (!string.IsNullOrWhiteSpace(html.CreatedByName))
		{
			return html.CreatedByName;
		}

		return html.LastModByName;
	}


	private string GetCreatedByName(ContentWorkflow workInProgress)
	{
		if (displaySettings.UseAuthorFirstAndLastName)
		{
			if (!string.IsNullOrWhiteSpace(workInProgress.CreatedByUserFirstName)
				&& !string.IsNullOrWhiteSpace(workInProgress.CreatedByUserLastName))
			{
				return $"{workInProgress.CreatedByUserFirstName} {workInProgress.CreatedByUserLastName}";
			}
		}

		if (!string.IsNullOrWhiteSpace(workInProgress.CreatedByUserName))
		{
			return workInProgress.CreatedByUserName;
		}

		return workInProgress.LastModByUserName;
	}


	private void ShowModified(HtmlContent html, ContentWorkflow workInProgress)
	{
		if (html is null && workInProgress is null)
		{
			return;
		}

		string modifiedByUserDateFormat = Resource.ModifiedByUserDateFormat;
		if (!string.IsNullOrWhiteSpace(displaySettings.OverrideModifiedByUserDateFormat))
		{
			modifiedByUserDateFormat = displaySettings.OverrideModifiedByUserDateFormat;
		}

		string modifiedDateFormat = Resource.ModifiedDateFormat;
		if (!string.IsNullOrWhiteSpace(displaySettings.OverrideModifiedDateFormat))
		{
			modifiedDateFormat = displaySettings.OverrideModifiedDateFormat;
		}

		string modifiedByUserFormat = Resource.ModifiedByUserFormat;
		if (!string.IsNullOrWhiteSpace(displaySettings.OverrideModifiedByUserFormat))
		{
			modifiedByUserFormat = displaySettings.OverrideModifiedByUserFormat;
		}

		if (config.ShowModifiedBy && config.ShowModifiedDate)
		{
			if (workInProgress is not null)
			{
				litModifiedBy.Text = string.Format(CultureInfo.InvariantCulture,
				   modifiedByUserDateFormat,
				   workInProgress.CreatedDateUtc.ToLocalTime(timeZone).ToString(displaySettings.DateFormat),
				   GetModifiedByName(workInProgress)
				   );

			}
			else if (html is not null)
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

			if (workInProgress is not null)
			{
				litModifiedBy.Text = string.Format(CultureInfo.InvariantCulture,
				   modifiedDateFormat,
				   workInProgress.LastModUtc.ToLocalTime(timeZone).ToString(displaySettings.DateFormat)
				   );

			}
			else if (html is not null)
			{
				litModifiedBy.Text = string.Format(CultureInfo.InvariantCulture,
						modifiedDateFormat,
						html.LastModUtc.ToLocalTime(timeZone).ToString(displaySettings.DateFormat)
						);
			}
		}
		else if (config.ShowModifiedBy)
		{

			if (workInProgress is not null)
			{
				litModifiedBy.Text = string.Format(CultureInfo.InvariantCulture,
				   modifiedByUserFormat,
				   GetModifiedByName(workInProgress)
				   );

			}
			else if (html is not null)
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
			if (!string.IsNullOrWhiteSpace(html.LastModByFirstName)
				&& !string.IsNullOrWhiteSpace(html.LastModByLastName))
			{
				return $"{html.LastModByFirstName} {html.LastModByLastName}";
			}
		}

		return html.LastModByName;
	}


	private string GetModifiedByName(ContentWorkflow workInProgress)
	{
		if (displaySettings.UseAuthorFirstAndLastName)
		{
			if (!string.IsNullOrWhiteSpace(workInProgress.LastModByUserFirstName) && !string.IsNullOrWhiteSpace(workInProgress.LastModByUserLastName))
			{
				return $"{workInProgress.LastModByUserFirstName} {workInProgress.LastModByUserLastName}";
			}
		}

		return workInProgress.LastModByUserName;
	}



	private void LoadSettings()
	{
		Title1.EditUrl = "HtmlEdit.aspx";
		Title1.EditText = Resource.EditImageAltText;
		Title1.ToolTip = Resource.EditImageAltText;

		timeZone = SiteUtils.GetUserTimeZone();

		if (WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor())
		{
			isAdmin = true;
			Title1.IsAdminEditor = isAdmin;
		}

		if (IsEditable)
		{
			TitleUrl = "HtmlEdit.aspx".ToLinkBuilder().PageId(currentPage.PageId).ModuleId(ModuleId).ToString();
		}

		config = new HtmlConfiguration(Settings);

		pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass);

		if (ModuleConfiguration != null)
		{
			Title = ModuleConfiguration.ModuleTitle;
			Description = ModuleConfiguration.FeatureName;
		}

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

			if (config.SlideContainerHeight > 0) { divContent.ContainerHeight = Invariant($"{config.SlideContainerHeight}px"); }

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
		}

		if (EnableWorkflow)
		{
			//use mojo base page to see if user has toggled draft content:
			if (Page is CmsPage cmsPage)
			{
				if (cmsPage.ViewMode == PageViewMode.WorkInProgress)
				{
					//try to get draft content:
					workInProgress = ContentWorkflow.GetWorkInProgress(ModuleGuid);
				}
			}
		}

		if (IsEditable && WebConfigSettings.EnableInlineEditing)
		{
			if (siteSettings.EditorProviderName == "TinyMCEProvider" && !WebConfigSettings.DisableTinyMceInlineEditing)
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
		if (WebConfigSettings.DisablejQuery)
		{
			return;
		}

		if (siteSettings.EditorProviderName != "CKEditorProvider")
		{
			return;
		}

		bool userCanOnlyEditAsDraft = basePage.UserCanOnlyEditModuleAsDraft(ModuleId, HtmlContent.FeatureGuid);

		if (basePage.ViewMode == PageViewMode.Live)
		{
			if (userCanOnlyEditAsDraft)
			{
				return;
			}
		}

		//http://stackoverflow.com/questions/13922002/ckeditor-inline-edit
		//http://stackoverflow.com/questions/12574664/save-edited-inline-text-from-ckeditor-4-asp-net

		ScriptManager.RegisterClientScriptBlock(
			this,
			GetType(),
			"ckeditormain",
			$"\n<script data-loader=\"HTMLModule\" src=\"{ResolveUrl(WebConfigSettings.CKEditorBasePath + "ckeditor.js")}\"></script>\n"
			, false);

		ScriptManager.RegisterStartupScript(
			this,
			typeof(Page),
			"ckeditordisableautoinline",
			"\n<script data-loader=\"HTMLModule\">CKEDITOR.disableAutoInline = true;</script>\n",
			false);

		// now setup the instance
		string strModuleId = ModuleId.ToInvariantString();
		string strRandomGuid = Guid.NewGuid().ToString().Replace("-", string.Empty);
		string fileManagerUrl = $"{SiteRoot}{WebConfigSettings.FileDialogRelativeUrl}";
		string dropFileUploadUrl = $"{SiteRoot}/Services/FileService.ashx?cmd=uploadfromeditor&rz=true&ko={WebConfigSettings.KeepFullSizeImagesDroppedInEditor.ToString().ToLower()}&t={Global.FileSystemToken}";
		string styleJsonUrl = $"{SiteRoot}/Services/CKeditorStyles.ashx?cb={strRandomGuid}";
		string templatesUrl = $"{SiteRoot}/Services/CKeditorTemplates.ashx?cb={strRandomGuid}";
		var script = new StringBuilder();
		script.Append("\n<script data-loader=\"HTMLModule\">\n");
		script.Append($"var hceditor{strModuleId}; ");
		script.Append($"function SetupCK{strModuleId}(){{");
		script.Append($"$('#{divContent.ClientID}').attr('contenteditable', true); ");
		script.Append($"hceditor{strModuleId} = CKEDITOR.inline( document.getElementById( '{divContent.ClientID}') ");
		script.Append(", {");
		script.Append($"customConfig: '{WebUtils.ResolveServerUrl(WebConfigSettings.CKEditorConfigPath)}' ");
		script.Append(",toolbar:'FullWithTemplates'");
		script.Append($",skin:'{WebConfigSettings.CKEditorSkin}'");
		script.Append(",bodyClass:'wysiwygeditor' ");
		if (WebConfigSettings.CKeditorSuppressTitle)
		{
			script.Append(",title:false");
		}
		script.Append(",filebrowserWindowWidth : ~~((80 / 100) * screen.width)"); // 80% of window width
		script.Append(",filebrowserWindowHeight : ~~((80 / 100) * screen.height)"); // 80% of window height
		script.Append($",filebrowserBrowseUrl:'{fileManagerUrl}?editor=ckeditor&type=file'");
		script.Append($",filebrowserImageBrowseUrl:'{fileManagerUrl}?editor=ckeditor&type=image'");
		script.Append($",filebrowserImageBrowseLinkUrl:'{fileManagerUrl}?editor=ckeditor&type=file'");
		script.Append(",filebrowserWindowFeatures:'location=no,menubar=no,toolbar=no,dependent=yes,minimizable=no,modal=yes,alwaysRaised=yes,resizable=yes,scrollbars=yes'");
		script.Append($",dropFileUploadUrl:'{Page.ResolveUrl(dropFileUploadUrl)}'");
		script.Append($",smiley_path:'{Page.ResolveUrl("~/Data/SiteImages/emoticons/")}'");
		script.Append($",stylesCombo_stylesSet : 'mojo:{styleJsonUrl}'");
		script.Append(",templates: 'mojo'");
		script.Append($",templates_files : ['{templatesUrl}']");
		script.Append(",templates_replaceContent :false");
		script.Append("} "); // end config object
		script.Append("); "); // end CKEDITOR.inline(
		script.Append($"hceditor{strModuleId}.on('blur', function( event ) {{ ");
		script.Append($"if (hceditor{strModuleId}.checkDirty()){{");
		script.Append($"var editorData{strModuleId} = hceditor{strModuleId}.getData(); ");

		script.Append($"var data{strModuleId} = {{ ");
		script.Append($"html: editorData{strModuleId}");
		script.Append("}; ");

		script.Append("$.ajax({ ");
		script.Append("type: 'POST',");
		script.Append($"url: '{SiteRoot}/Services/HtmlEditService.aspx?pageid={PageId.ToInvariantString()}&mid={strModuleId}', ");
		script.Append($"data: data{strModuleId}");

		if (EnableWorkflow
			&& basePage.ViewMode == PageViewMode.WorkInProgress
			&& workInProgress == null)
		{
			script.Append("}).complete(function( html ) {");
			// if user edits it will create a new draft.
			// need to reload the page so it shows the workflow toolbar items after edit
			script.Append("window.location.reload(true); ");
		}

		script.Append("}); ");
		script.Append($"hceditor{strModuleId}.resetDirty(); ");
		script.Append("}"); //end if checkdirty
		script.Append("});"); //end on blur
		script.Append("} "); //end SetupCkModuleId funtion

		script.Append($"function DestroyCK{strModuleId}() {{ ");
		script.Append($"hceditor{strModuleId}.destroy(); ");
		script.Append($"$('#{divContent.ClientID}').attr('contenteditable', false); ");
		script.Append("} "); //end DestroyCkModuleId funtion

		script.Append("function ToggleCk" + strModuleId + "()");
		script.Append("{ ");
		script.Append("if($('#mtogedit" + strModuleId + "').hasClass('ui-icon-unlocked'))");
		script.Append("{ ");
		script.Append(" DestroyCK" + strModuleId + "(); ");
		script.Append("$('#mtogedit" + strModuleId + "').removeClass('ui-icon-unlocked'); ");
		script.Append("$('#mtogedit" + strModuleId + "').addClass('ui-icon-locked'); ");
		script.Append("}else{ ");
		script.Append(" SetupCK" + strModuleId + "(); ");
		script.Append("hceditor" + strModuleId + ".focus(); ");
		script.Append("$('#mtogedit" + strModuleId + "').removeClass('ui-icon-locked'); ");
		script.Append("$('#mtogedit" + strModuleId + "').addClass('ui-icon-unlocked'); ");
		script.Append("} ");
		script.Append("} "); //end ToggleCk funtion

		script.Append("$(document).ready(function() {");

		if (!displaySettings.AddEditToggleToModuleTitle)
		{
			script.Append($"$('#{divContent.ClientID}').after($(\"<a href='#' id='mtogedit{strModuleId}' title='{HttpUtility.HtmlAttributeEncode(Resource.ToggleInlineEditing)}'  class='jqtt inlineedittoggle ui-icon ui-icon-locked'></a>\"));");
			script.Append($" $('#mtogedit{strModuleId}').position({{");
			script.Append($"my:'{displaySettings.EditorTogglePositionMy}', ");
			script.Append($"at:'{displaySettings.EditorTogglePositionAt}',");
			script.Append($"of: '#{divContent.ClientID}'");
			script.Append("}); ");
		}

		script.Append($" $('#mtogedit{strModuleId}').click(function() {{ ToggleCk{strModuleId}(); return false; }});");
		script.Append($"$('#mtogedit{strModuleId}').tooltip(); ");
		script.Append("});");

		if (displaySettings.AddEditToggleToModuleTitle)
		{
			Title1.LiteralExtraMarkup = $" <a href='#' id='mtogedit{strModuleId}' title='{HttpUtility.HtmlAttributeEncode(Resource.ToggleInlineEditing)}' class='jqtt inlineedittoggle ui-icon ui-icon-locked'></a>";
		}

		script.Append("\n</script>\n");

		ScriptManager.RegisterStartupScript(
			this,
			GetType(),
			UniqueID,
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
			GetType(),
			"tinymcemain",
			$"\n<script data-loader=\"HTMLModule\" src=\"{ResolveUrl(WebConfigSettings.TinyMceBasePath + "tinymce.min.js")}\"></script>"
			, false);

		TinyMceConfiguration config = TinyMceConfiguration.GetConfig();
		// need a clone beucause these are chached and we are modifying the object
		TinyMceSettings editorSettings = config.GetEditorSettings("FullWithTemplates").Clone();
		editorSettings.Inline = true;
		editorSettings.EditorAreaCSS = SiteUtils.GetEditorStyleSheetUrl(basePage.AllowSkinOverride, Page);
		editorSettings.TemplatesUrl = $"{SiteUtils.GetNavigationSiteRoot()}/Services/TinyMceTemplates.ashx?v={siteSettings.SkinVersion}";
		editorSettings.StyleFormats = TinyMceEditorAdapter.BuildTinyMceStyleJson();
		editorSettings.EmotionsBaseUrl = Page.ResolveUrl("~/Data/SiteImages/emoticons/tinymce/");
		editorSettings.FileManagerUrl = $"{SiteRoot}{WebConfigSettings.FileDialogRelativeUrl}";
		editorSettings.GlobarVarToAssignEditor = $"hceditor{ModuleId.ToInvariantString()}";

		string strModuleId = ModuleId.ToInvariantString();

		string imagesUploadUrl = Page.ResolveUrl($"{SiteRoot}/Services/FileService.ashx?cmd=uploadfromeditor&rz=true&ko={WebConfigSettings.KeepFullSizeImagesDroppedInEditor.ToString().ToLower()}&t={Global.FileSystemToken}");
		editorSettings.ImagesUploadUrl = imagesUploadUrl;

		if (WebConfigSettings.TinyMceInlineUseSavePlugin) // true by default
		{
			editorSettings.OnSaveCallback = $"saveEditor{strModuleId}";
			if (!editorSettings.Plugins.StartsWith("save,"))
			{
				editorSettings.Plugins = $"save,{editorSettings.Plugins}";
			}
			if (!editorSettings.Toolbar1Buttons.StartsWith("save "))
			{
				editorSettings.Toolbar1Buttons = $"save {editorSettings.Toolbar1Buttons}";
			}

		}
		if (WebConfigSettings.TinyMceInlineRemoveAutoSavePlugin)
		{
			if (editorSettings.Plugins.StartsWith("autosave,"))
			{
				editorSettings.Plugins = editorSettings.Plugins.Replace("autosave,", string.Empty);
			}
		}

		var script = new StringBuilder();
		script.Append("\n<script data-loader=\"HTMLModule\">\n");
		script.Append($"var hceditor{strModuleId}; ");
		script.Append($"function SetupTinyMCE{strModuleId}(){{");
		script.Append($"$('#{divContent.ClientID}').attr('contenteditable', true); ");

		TinyMceEditor.BuildScript(script, editorSettings, divContent.ClientID, Unit.Empty, Unit.Empty);

		if (WebConfigSettings.TinyMceInlineSaveOnBlur) // false by default
		{
			script.Append($" hceditor{strModuleId}.on( 'blur', function( event ) {{ ");
			script.Append($"if(hceditor{strModuleId}.isDirty()){{");
			script.Append($" saveEditor{strModuleId}(); ");
			script.Append("}"); //end if checkdirty
			script.Append("});"); //end on blur
		}

		script.Append("} "); //end SetupCkModuleId funtion
		script.Append($" function saveEditor{strModuleId} (){{ ");
		script.Append($"var editorData{strModuleId} = hceditor{strModuleId}.getContent(); ");

		//http://stackoverflow.com/questions/1078909/jquery-send-html-data-through-post

		script.Append($"var data{strModuleId} = {{ "); ;
		script.Append($"html: editorData{strModuleId}");
		script.Append("}; ");

		script.Append("$.ajax({ ");
		script.Append("type: 'POST',");
		script.Append($"url: '{SiteRoot}/Services/HtmlEditService.aspx?pageid={PageId.ToInvariantString()}&mid={strModuleId}', ");

		script.Append($"data: data{strModuleId}");

		if (EnableWorkflow && basePage.ViewMode == PageViewMode.WorkInProgress && workInProgress == null)
		{
			script.Append("}).complete(function( html ) {");
			// if user edits it will create a new draft.
			// need to reload the page so it shows the workflow toolbar items after edit
			script.Append("window.location.reload(true);  ");
			// script.Append("});");
		}

		script.Append("}); ");
		script.Append($"hceditor{strModuleId}.isNotDirty = true; ");
		script.Append(" }"); // end save

		script.Append($"function DestroyTinyMCE{strModuleId}() {{ ");
		script.Append($"hceditor{strModuleId}.destroy(); ");
		script.Append($"$('#{divContent.ClientID}').attr('contenteditable', false); ");
		script.Append("} "); //end DestroyTinyMCEModuleId function

		script.Append($"function ToggleTinyMCE{strModuleId}()");
		script.Append("{ ");
		script.Append($"if($('#mtogedit{strModuleId}').hasClass('ui-icon-unlocked'))");
		script.Append("{ ");
		script.Append($" DestroyTinyMCE{strModuleId}(); ");
		script.Append($"$('#mtogedit{strModuleId}').removeClass('ui-icon-unlocked'); ");
		script.Append($"$('#mtogedit{strModuleId}').addClass('ui-icon-locked'); ");

		script.Append("}else{ ");

		script.Append($" SetupTinyMCE{strModuleId}(); ");

		script.Append($"$('#mtogedit{strModuleId}').removeClass('ui-icon-locked'); ");
		script.Append($"$('#mtogedit{strModuleId}').addClass('ui-icon-unlocked'); ");
		script.Append("} ");
		script.Append("} "); //end ToggleCk funtion

		script.Append("$(document).ready(function() {");

		if (!displaySettings.AddEditToggleToModuleTitle)
		{
			script.Append($"$('#{divContent.ClientID}').after($(\"<a href='#' id='mtogedit{strModuleId}' title='{HttpUtility.HtmlAttributeEncode(Resource.ToggleInlineEditing)}'  class='jqtt inlineedittoggle ui-icon ui-icon-locked'></a>\"));");

			script.Append($" $('#mtogedit{strModuleId}').position({{");
			script.Append($"my:'{displaySettings.EditorTogglePositionMy}', ");
			script.Append($"at:'{displaySettings.EditorTogglePositionAt}',");
			script.Append($"of: '#{divContent.ClientID}'");
			script.Append("}); ");
		}

		script.Append($" $('#mtogedit{strModuleId}').click(function() {{ ToggleTinyMCE{strModuleId}(); return false; }});");
		script.Append($"$('#mtogedit{strModuleId}').tooltip(); ");
		script.Append("});");

		if (displaySettings.AddEditToggleToModuleTitle)
		{
			Title1.LiteralExtraMarkup = $" <a href='#' id='mtogedit{strModuleId}' title='{HttpUtility.HtmlAttributeEncode(Resource.ToggleInlineEditing)}' class='jqtt inlineedittoggle ui-icon ui-icon-locked'></a>";
		}

		script.Append("\n</script>\n");

		ScriptManager.RegisterStartupScript(
			this,
			GetType(),
			UniqueID,
			script.ToString(),
			false);
	}


	void html_ContentChanged(object sender, ContentChangedEventArgs e)
	{
		IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["HtmlContentIndexBuilderProvider"];
		indexBuilder?.ContentChangedHandler(sender, e);
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
		if (html is not null)
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
		if (workInProgress is not null)
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
