using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI;

public partial class CmsPage : mojoBasePage
{
	private static readonly ILog log = LogManager.GetLogger(typeof(CmsPage));

	private HyperLink lnkEditPageSettings = new();
	private HyperLink lnkEditPageContent = new();
	private int countOfIWorkflow = 0;
	private bool forceShowWorkflow = false;

	override protected void OnPreInit(EventArgs e)
	{
		AllowSkinOverride = true;
		base.OnPreInit(e);

		if (WebConfigSettings.SetMaintainScrollPositionOnPostBackTrueOnCmsPages)
		{
			MaintainScrollPositionOnPostBack = true;
		}
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}

	void Page_Load(object sender, EventArgs e)
	{
		EnsurePageAndSite();

		if (CurrentPage == null) return;

		EnforceSecuritySettings();

		bool redirected = RedirectIfNeeded();
		if (redirected) { return; }

		if (CurrentPage.PageId == -1)
		{
			SetupAdminLinks();
			return;
		}

		if (CurrentPage.ShowChildPageMenu
			|| CurrentPage.ShowBreadcrumbs
			|| CurrentPage.ShowChildPageBreadcrumbs
			)
		{
			// this is needed to override some hide logic in
			// layout.Master.cs
			MPContent.Visible = true;
			MPContent.Parent.Visible = true;
		}

		if (CurrentPage.BodyCssClass.Length > 0)
		{
			AddClassToBody(CurrentPage.BodyCssClass);
		}

		// solves background problems with some skin in WLW
		if ((StyleCombiner != null) && (StyleCombiner.AddBodyClassForLiveWriter))
		{
			if (HttpContext.Current != null 
				&& HttpContext.Current.Request != null 
				&& HttpContext.Current.Request.UserAgent != null
				&& HttpContext.Current.Request.UserAgent.ToLower().Contains("windows live writer"))
			{
				AddClassToBody("wysiwygeditor");
			}
		}

		if (CurrentPage.PageTitle.Length > 0)
		{
			if (WebConfigSettings.FormatOverridePageTitle)
			{
				Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageTitle);
			}
			else
			{
				Title = CurrentPage.PageTitle;
			}
		}
		else
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageName);
		}

		MetaContentControl.Title = Title;
		MetaContentControl.Description = CurrentPage.PageMetaDescription;

		var mediaPath = WebConfigSettings.SiteLogoUseMediaFolder ?
				"media/" :
				string.Empty;
		MetaContentControl.Image = string.IsNullOrWhiteSpace(CurrentPage.MenuImage) ?
			$"~/Data/Sites/{siteSettings.SiteId}/{mediaPath}logos/{siteSettings.Logo}".ToLinkBuilder().ToString() :
			string.IsNullOrWhiteSpace(CurrentPage.MenuImage) ?
				string.Empty :
				CurrentPage.MenuImage.ToLinkBuilder().ToString();
		MetaContentControl.KeywordCsv = CurrentPage.PageMetaKeyWords;

		if (CurrentPage.CompiledMeta.Length > 0)
		{
			AdditionalMetaMarkup = CurrentPage.CompiledMeta;
		}


		if (Page.Header != null && CurrentPage.UseUrl && CurrentPage.Url.Length > 0)
		{
			string urlToUse;
			if (CurrentPage.CanonicalOverride.Length > 0)
			{
				urlToUse = CurrentPage.CanonicalOverride;
			}
			else
			{
				if (CurrentPage.Url.StartsWith("http"))
				{
					urlToUse = CurrentPage.Url;
				}
				else
				{
					if (CurrentPage.UrlHasBeenAdjustedForFolderSites)
					{
						urlToUse = WebUtils.GetSiteRoot() + CurrentPage.Url.Replace("~/", "/");
					}
					else
					{
						urlToUse = SiteRoot + CurrentPage.Url.Replace("~/", "/");
					}

					if (WebConfigSettings.ForceHttpForCanonicalUrlsThatDontRequireSsl)
					{
						if (WebHelper.IsSecureRequest() && (!CurrentPage.RequireSsl) && (!siteSettings.UseSslOnAllPages))
						{
							urlToUse = urlToUse.Replace("https:", "http:");
						}
					}
				}
			}

			MetaContentControl.Url = urlToUse;
			
			if (WebConfigSettings.AutomaticallyAddCanonicalUrlToCmsPages)
			{
				Literal link = new Literal
				{
					ID = "pageurl",
					Text = "\n<link rel='canonical' href='"
					+ urlToUse
					+ "' />"
				};

				Page.Header.Controls.Add(link);
			}
		}

		bool isAdmin = WebUser.IsAdmin;
		bool isContentAdmin = false;
		bool isSiteEditor = false;
		if (!isAdmin)
		{
			isContentAdmin = WebUser.IsContentAdmin;
			isSiteEditor = SiteUtils.UserIsSiteEditor();
		}

		if (CurrentPage.Modules.Count == 0)
		{
			if (!CurrentPage.ShowChildPageMenu) // we'll consider Child Page Menu as a feature for our purpose here
			{
				if (
					(WebConfigSettings.UsePageContentWizard)
					&&
					(isAdmin || isContentAdmin || isSiteEditor || WebUser.IsInRoles(CurrentPage.EditRoles))
					)
				{
					// this is to make it a little more intuitive
					// sometimes users create pages but fail to see the link in pagesettings to 
					// add features so if they visit the empty page give them an easy opportunity
					// to add a feature
					Control wiz = Page.LoadControl("~/Admin/Controls/PageContentWizard.ascx");
					MPContent.Controls.Add(wiz);
				}
			}

			return;
		}

		foreach (Module module in CurrentPage.Modules)
		{
			if (!ModuleIsVisible(module)) { continue; }

			if (
				(!WebUser.IsInRoles(module.ViewRoles))
				&& (!isContentAdmin)
				&& (!isSiteEditor)
				)
			{
				continue;
			}

			if ((module.ViewRoles == "Admins;") && (!isAdmin)) { continue; }


			if (module.ControlSource == "Modules/LoginModule.ascx")
			{
				LoginModuleDisplaySettings loginSettings = new LoginModuleDisplaySettings();
				MPContent.Controls.Add(loginSettings); ///theme is not applied until its loaded
				if ((Request.IsAuthenticated) && (loginSettings.HideWhenAuthenticated)) { continue; }
			}

			Control parent = MPContent;

			if (module.PaneName.IsCaseInsensitiveMatch("leftpane"))
			{
				parent = MPLeftPane;
			}

			if (module.PaneName.IsCaseInsensitiveMatch("rightpane"))
			{
				parent = MPRightPane;
			}

			if (module.PaneName.IsCaseInsensitiveMatch("altcontent1"))
			{
				if (AltPane1 != null)
				{
					parent = AltPane1;
				}
				else
				{
					log.Error("Content is assigned to altcontent1 placeholder but it does not exist in layout.master so using center.");
					parent = MPContent;
				}
			}

			if (module.PaneName.IsCaseInsensitiveMatch("altcontent2"))
			{
				if (AltPane2 != null)
				{
					parent = AltPane2;
				}
				else
				{
					log.Error("Content is assigned to altcontent2 placeholder but it does not exist in layout.master so using center.");
					parent = MPContent;
				}

			}

			// this checks if we are using the mobile skin and whether the content is for all web only or mobile only
			if (!ShouldShowModule(module)) { continue; }

			// 2008-10-04 since more an more of our features use postback via ajax
			// its not feasible to use output caching as this breaks postback,
			// so I changed the default the use of WebConfigSettings.DisableContentCache to true
			// this also reduces the memory consumption footprint

			if ((module.CacheTime == 0) || (WebConfigSettings.DisableContentCache))
			{
				//2008-10-16 in ulu's blog post:http://dotfresh.blogspot.com/2008/10/in-search-of-developer-friendly-cms.html
				// he complains about having to inherit from a base class (SiteModuleControl) to make a plugin.
				// he wishes he could just use a UserControl
				// While SiteModuleControl "is a" UserControl that provides additional functionality
				// Its easy enough to support using
				// a plain UserControl so I'm making the needed change here now.
				// The drawback of a plain UserControl is that is not reusable in the same way as SiteModuleControl.
				// If you use a plain UserControl, its going to be exactly the same on any page you use it on.
				// It has no instance specific properties.
				// SiteModuleControl gives you instance specific ids and internal tracking of which instance this is so 
				// that you can have different instances.
				// For example the Blog is instance specific, if you put a blog on one page and then put a blog on another page
				// those are 2 different blogs with different content.
				// However, if you don't need a re-usable feature with instance specific properties
				// you are now free to use a plain old UserControl and I think freedom is a good thing
				// so this was valuable feedback from ulu.
				// Those who do need instance specific features should read my developer Guidelines for building one:
				//http://www.mojoportal.com/addingfeatures.aspx

				try
				{
					Control c = Page.LoadControl("~/" + module.ControlSource);
					if (c == null) { continue; }

					if (c is SiteModuleControl siteModule)
					{
						siteModule.SiteId = siteSettings.SiteId;
						siteModule.ModuleConfiguration = module;

						if (siteModule is IWorkflow)
						{
							if (WebUser.IsInRoles(module.AuthorizedEditRoles)) { forceShowWorkflow = true; }
							if (WebUser.IsInRoles(module.DraftEditRoles)) { forceShowWorkflow = true; }

							countOfIWorkflow += 1;
						}
					}

					parent.Controls.Add(c);
				}
				catch (HttpException ex)
				{
					log.Error("failed to load control " + module.ControlSource, ex);
				}
			}
			else
			{
				CachedSiteModuleControl siteModule = new CachedSiteModuleControl();

				siteModule.SiteId = siteSettings.SiteId;
				siteModule.ModuleConfiguration = module;
				parent.Controls.Add(siteModule);
			}

			parent.Visible = true;
			parent.Parent.Visible = true;


		} //end foreach

		SetupAdminLinks();

		if ((!WebConfigSettings.DisableExternalCommentSystems) && (siteSettings != null) && (CurrentPage != null) && (CurrentPage.EnableComments))
		{
			switch (siteSettings.CommentProvider)
			{
				case "disqus":

					if (siteSettings.DisqusSiteShortName.Length > 0)
					{
						DisqusWidget disqus = new DisqusWidget();
						disqus.SiteShortName = siteSettings.DisqusSiteShortName;
						disqus.WidgetPageUrl = WebUtils.ResolveServerUrl(SiteUtils.GetCurrentPageUrl());
						if (disqus.WidgetPageUrl.StartsWith("https"))
						{
							disqus.WidgetPageUrl = disqus.WidgetPageUrl.Replace("https", "http");
						}
						disqus.RenderWidget = true;
						MPContent.Controls.Add(disqus);
					}
					break;

				case "intensedebate":

					if (siteSettings.IntenseDebateAccountId.Length > 0)
					{
						IntenseDebateDiscussion d = new IntenseDebateDiscussion();
						d.AccountId = siteSettings.IntenseDebateAccountId;
						d.PostUrl = SiteUtils.GetCurrentPageUrl();
						MPContent.Controls.Add(d);
					}
					break;

				case "facebook":
					FacebookCommentWidget fbComments = new FacebookCommentWidget();
					fbComments.AutoDetectUrl = true;
					MPContent.Controls.Add(fbComments);
					break;
			}
		}

		if (WebConfigSettings.HidePageViewModeIfNoWorkflowItems && (countOfIWorkflow == 0))
		{
			HideViewSelector();
		}

		// (to show the last mnodified time of a page we may have this control in layout.master, but I set it invisible by default
		// because we only want to show it on content pages not edit pages
		// since Default.aspx.cs is the handler for content pages, we look for it here and make it visible.
		Control pageLastMod = Master.FindControl("pageLastMod");
		if (pageLastMod != null) { pageLastMod.Visible = true; }
	}

	private void EnsurePageAndSite()
	{
		if (CurrentPage == null)
		{
			int siteCount = SiteSettings.SiteCount();

			if (siteCount == 0)
			{
				// no site data so redirect to setup
				HttpContext.Current.Response.Redirect(WebUtils.GetSiteRoot() + "/Setup/Default.aspx");
			}
		}
	}

	private bool RedirectIfNeeded()
	{
		if (!UserCanViewPage())
		{
			if (!Request.IsAuthenticated)
			{
				if (WebConfigSettings.UseRawUrlForCmsPageLoginRedirects)
				{
					SiteUtils.RedirectToLoginPage(this);
				}
				else
				{
					SiteUtils.RedirectToLoginPage(this, SiteUtils.GetCurrentPageUrl());
				}
				return true;
			}
			else
			{
				SiteUtils.RedirectToAccessDeniedPage(this);
				return true;
			}
		}

		return false;
	}

	private void EnforceSecuritySettings()
	{
		if (CurrentPage.PageId == -1) { return; }

		if (!CurrentPage.AllowBrowserCache)
		{
			SecurityHelper.DisableBrowserCache();
		}

		bool useSsl = false;

		if (SiteUtils.SslIsAvailable())
		{
			if (WebConfigSettings.ForceSslOnAllPages || siteSettings.UseSslOnAllPages || CurrentPage.RequireSsl)
			{
				useSsl = true;
			}
		}

		if (useSsl)
		{
			SiteUtils.ForceSsl();
		}
		else
		{
			SiteUtils.ClearSsl();
		}
	}

	private void SetupAdminLinks()
	{
		// 2010-01-04 made it possible to add these links directly in layout.master so they can be arranged and styled easier
		if (Page.Master.FindControl("lnkPageContent") == null)
		{
			MPPageEdit.Controls.Add(new PageEditFeaturesLink());
		}

		if (Page.Master.FindControl("lnkPageSettings") == null)
		{
			MPPageEdit.Controls.Add(new PageEditSettingsLink());
		}

		SetupWorkflowControls(forceShowWorkflow);
	}
}