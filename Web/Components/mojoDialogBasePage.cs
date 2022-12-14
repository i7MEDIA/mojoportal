using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web
{
	public class mojoDialogBasePage : mojoBasePage
	{
		//private static readonly ILog log = LogManager.GetLogger(typeof(mojoDialogBasePage));

		public mojoDialogBasePage()
		{
			MasterPageName = "dialogMaster.Master";
		}

		//private PageSettings currentPage = null;
		//protected SiteSettings siteSettings = null;
		//private string siteRoot = string.Empty;
		//private ScriptLoader scriptLoader = null;
		//protected StyleSheet StyleSheetControl;
		//private StyleSheetCombiner styleCombiner = null;

		//private bool allowSkinOverride = false;
		//private bool setMasterInBasePage = true;
		//public SiteSettings SiteInfo
		//{
		//	get
		//	{
		//		EnsureSiteSettings();
		//		return siteSettings;
		//	}
		//}




		/// <summary>
		/// Returns true if the module exists on the page and the user has permission to edit the page or the module.
		/// </summary>
		/// <param name="moduleId"></param>
		/// <returns></returns>
		//public bool UserCanEditModule(int moduleId)
		//{
		//	return UserCanEditModule(moduleId, Guid.Empty);

		//}

		/// <summary>
		/// this overload is preferred because it checks if the module represents an instance of the feature
		/// </summary>
		/// <param name="moduleId"></param>
		/// <param name="featureGuid"></param>
		/// <returns></returns>
		//public bool UserCanEditModule(int moduleId, Guid featureGuid)
		//{
		//	if (!Request.IsAuthenticated) return false;

		//	if (WebUser.IsAdminOrContentAdmin) return true;

		//	if (SiteUtils.UserIsSiteEditor()) { return true; }

		//	if (CurrentPage == null) return false;

		//	bool moduleFoundOnPage = false;
		//	foreach (Module m in CurrentPage.Modules)
		//	{
		//		if (
		//			(m.ModuleId == moduleId)
		//			&& ((featureGuid == Guid.Empty) || (m.FeatureGuid == featureGuid))
		//			)
		//		{
		//			moduleFoundOnPage = true;
		//		}
		//	}

		//	if (!moduleFoundOnPage) return false;

		//	if (WebUser.IsInRoles(CurrentPage.EditRoles)) return true;

		//	SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
		//	if (currentUser == null) return false;

		//	foreach (Module m in CurrentPage.Modules)
		//	{
		//		if (m.ModuleId == moduleId)
		//		{
		//			if (m.EditUserId == currentUser.UserId) return true;
		//			if (WebUser.IsInRoles(m.AuthorizedEditRoles)) return true;
		//		}
		//	}

		//	return false;

		//}

		//public Module GetModule(int moduleId)
		//{
		//	return GetModule(moduleId, Guid.Empty);
		//}

		//public Module GetModule(int moduleId, Guid featureGuid)
		//{
		//	if (CurrentPage == null) { return null; }

		//	foreach (Module m in CurrentPage.Modules)
		//	{
		//		if (
		//			(m.ModuleId == moduleId)
		//			&& ((featureGuid == Guid.Empty) || (m.FeatureGuid == featureGuid))
		//			)
		//		{ return m; }
		//	}

		//	return null;
		//}

		//public bool UserCanOnlyEditModuleAsDraft(int moduleId)
		//{
		//	return UserCanOnlyEditModuleAsDraft(moduleId, Guid.Empty);

		//}

		//public bool UserCanOnlyEditModuleAsDraft(int moduleId, Guid featureGuid)
		//{
		//	if (!Request.IsAuthenticated) return false;

		//	if (WebUser.IsAdminOrContentAdmin) return false;

		//	if (SiteUtils.UserIsSiteEditor()) { return false; }

		//	if (!WebConfigSettings.EnableContentWorkflow) { return false; }
		//	if (SiteInfo == null) { return false; }
		//	if (!SiteInfo.EnableContentWorkflow) { return false; }

		//	if (CurrentPage == null) return false;

		//	bool moduleFoundOnPage = false;
		//	foreach (Module m in CurrentPage.Modules)
		//	{
		//		if (
		//			(m.ModuleId == moduleId)
		//			&& ((featureGuid == Guid.Empty) || (m.FeatureGuid == featureGuid))
		//			)
		//		{
		//			moduleFoundOnPage = true;
		//		}
		//	}

		//	if (!moduleFoundOnPage) return false;

		//	if (WebUser.IsInRoles(CurrentPage.DraftEditOnlyRoles)) return true;

		//	SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
		//	if (currentUser == null) return false;

		//	foreach (Module m in CurrentPage.Modules)
		//	{
		//		if (m.ModuleId == moduleId)
		//		{
		//			if (WebUser.IsInRoles(m.DraftEditRoles)) return true;
		//		}
		//	}

		//	return false;

		//}

		//protected override void OnPreInit(EventArgs e)
		//{
		//	base.OnPreInit(e);
		//}

		//protected void SetupFailsafeMasterPage()
		//{
		//	if (!WebConfigSettings.UseFailSafeMasterPageOnError)
		//	{
		//		return;
		//	}

		//	MasterPageFile = "~/App_MasterPages/DialogMaster.Master";

		//	StyleSheetControl = (StyleSheet)Master.FindControl("StyleSheet");
		//}

	}
}
