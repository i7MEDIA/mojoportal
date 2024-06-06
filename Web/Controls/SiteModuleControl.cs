using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web;

public abstract class SiteModuleControl : mojoUserControl
{
	private Hashtable settings;
	private string imageSiteRoot = null;
	private bool isSiteEditor = false;
	private bool use3LevelWorkflow = false; //joe davis
	private bool IsOnInitExecuted = false;

	protected PageSettings currentPage;
	protected SiteSettings siteSettings;
	protected ScriptManager ScriptController;

	protected override void OnInit(EventArgs e)
	{
		// Alexander Yushchenko: workaround to make old custom modules work
		// Before 03.19.2007 this method was "new" and called from descendant classes
		// To avoid multiple self-calls a boolean flag is used
		if (IsOnInitExecuted) return;
		IsOnInitExecuted = true;

		base.OnInit(e);

		if (HttpContext.Current == null) { return; }

		siteSettings = CacheHelper.GetCurrentSiteSettings();

		currentPage = CacheHelper.GetCurrentPage();
		ScriptController = (ScriptManager)Page.Master.FindControl("ScriptManager1");

		if (siteSettings != null)
		{
			this.SiteId = siteSettings.SiteId;
			if (!WebUser.IsAdminOrContentAdmin)
			{
				ForbidModuleSettings = WebUser.IsInRoles(siteSettings.RolesNotAllowedToEditModuleSettings);
			}
		}

		if (Page.Request.IsAuthenticated)
		{
			isSiteEditor = SiteUtils.UserIsSiteEditor();

			//if (WebUser.IsAdminOrContentAdmin || isSiteEditor || WebUser.IsInRoles(currentPage.EditRoles)
			//    || ((moduleConfiguration != null)
			//           && (WebUser.IsInRoles(moduleConfiguration.AuthorizedEditRoles))
			//       )
			//   )
			//{
			//    isEditable = true;
			//}

			IsEditable = ShouldAllowEdit();

			if ((ModuleConfiguration != null) && (!ModuleConfiguration.IsGlobal))
			{
				if (WebConfigSettings.EnableContentWorkflow && siteSettings.EnableContentWorkflow && (this is IWorkflow))
				{
					EnableWorkflow = true;

					use3LevelWorkflow = WebConfigSettings.Use3LevelContentWorkflow; //joe davis

					if (!IsEditable)
					{
						if (
							(WebUser.IsInRoles(currentPage.DraftEditOnlyRoles))
							|| (WebUser.IsInRoles(ModuleConfiguration.DraftEditRoles))
							|| (use3LevelWorkflow && WebUser.IsInRoles(ModuleConfiguration.DraftApprovalRoles))
							)
						{
							IsEditable = true;

						}

					}
				}
			}

			if (!IsEditable && (ModuleConfiguration != null) && (ModuleConfiguration.EditUserId > 0))
			{
				SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
				if (
					(siteUser != null)
					&& (ModuleConfiguration.EditUserId == siteUser.UserId)
					)
				{
					IsEditable = true;
				}
			}
		}

		if (ModuleConfiguration != null)
		{
			this.Title = ModuleConfiguration.ModuleTitle;
			this.Description = ModuleConfiguration.FeatureName;
		}
	}

	private bool ShouldAllowEdit()
	{
		if (WebUser.IsAdmin)
		{
			return true;
		}

		if (ModuleConfiguration != null)
		{
			if (ModuleConfiguration.AuthorizedEditRoles == "Admins;") { return false; }
			if (currentPage.EditRoles == "Admins;") { return false; }

			if (WebUser.IsContentAdmin) { return true; }

			if (isSiteEditor) { return true; }

			if (WebUser.IsInRoles(ModuleConfiguration.AuthorizedEditRoles)) { return true; }

			if ((!ModuleConfiguration.IsGlobal) && WebUser.IsInRoles(currentPage.EditRoles)) { return true; }

		}

		return false;
	}

	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string TitleUrl { get; set; } = string.Empty;

	public int ModuleId
	{
		get { return ModuleConfiguration == null ? 0 : ModuleConfiguration.ModuleId; }
		set
		{
			ModuleConfiguration ??= new Module(value);
			ModuleConfiguration.ModuleId = value;
		}
	}

	public Guid ModuleGuid
	{
		get { return ModuleConfiguration == null ? Guid.Empty : ModuleConfiguration.ModuleGuid; }
		set
		{
			ModuleConfiguration ??= new Module(value);
			ModuleConfiguration.ModuleGuid = value;
		}
	}


	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int PageId => ModuleConfiguration == null ? 0 : ModuleConfiguration.PageId;


	public string SiteRoot => SiteUtils.GetNavigationSiteRoot();

	public string ImageSiteRoot
	{
		get
		{
			//imageSiteRoot = WebUtils.GetSiteRoot();
			imageSiteRoot ??= SiteUtils.GetImageSiteRoot(Page);
			return imageSiteRoot;
		}
	}


	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SiteId { get; set; } = -1;

	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsEditable { get; private set; } = false;

	public bool ForbidModuleSettings { get; private set; } = false;

	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool EnableWorkflow { get; private set; } = false;


	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Module ModuleConfiguration { get; set; }


	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Hashtable Settings
	{
		get
		{
			settings ??= ModuleSettings.GetModuleSettings(ModuleId);
			return settings;
		}
	}
}