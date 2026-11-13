#nullable enable
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI;

public partial class CmsPage : mojoBasePage
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(CmsPage));

	private int _countOfIWorkflow = 0;
	private bool _forceShowWorkflow = false;


	protected override void OnPreInit(EventArgs e)
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

		if (CurrentPage is null)
		{
			return;
		}

		EnforceSecuritySettings();

		var redirected = RedirectIfNeeded();

		if (redirected)
		{
			return;
		}

		if (CurrentPage.PageId == -1)
		{
			SetupAdminLinks();

			return;
		}

		if (CurrentPage.ShowChildPageMenu ||
			CurrentPage.ShowBreadcrumbs ||
			CurrentPage.ShowChildPageBreadcrumbs)
		{
			// this is needed to override some hide logic in
			// layout.Master.cs
			MPContent.Visible = true;
			MPContent.Parent.Visible = true;
		}

		if (!string.IsNullOrWhiteSpace(CurrentPage.BodyCssClass))
		{
			AddClassToBody(CurrentPage.BodyCssClass);
		}

		// solves background problems with some skin in WLW
		if (StyleCombiner is not null && StyleCombiner.AddBodyClassForLiveWriter)
		{
			if (HttpContext.Current is not null &&
				HttpContext.Current.Request is not null &&
				HttpContext.Current.Request.UserAgent is not null &&
				HttpContext.Current.Request.UserAgent.ToLower().Contains("windows live writer"))
			{
				AddClassToBody("wysiwygeditor");
			}
		}

		if (!string.IsNullOrWhiteSpace(CurrentPage.PageTitle))
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

		if (Page.Header is not null && CurrentPage.UseUrl && !string.IsNullOrWhiteSpace(CurrentPage.Url))
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
						if (WebHelper.IsSecureRequest() && !CurrentPage.RequireSsl && !siteSettings.UseSslOnAllPages)
						{
							urlToUse = urlToUse.Replace("https:", "http:");
						}
					}
				}
			}

			MetaContentControl.Url = urlToUse;

			if (WebConfigSettings.AutomaticallyAddCanonicalUrlToCmsPages)
			{
				var link = new Literal
				{
					ID = "pageurl",
					Text = $"\n<link rel='canonical' href='{urlToUse}' />"
				};

				Page.Header.Controls.Add(link);
			}
		}

		var isAdmin = WebUser.IsAdmin;
		var isContentAdmin = false;
		var isSiteEditor = false;

		if (!isAdmin)
		{
			isContentAdmin = WebUser.IsContentAdmin;
			isSiteEditor = SiteUtils.UserIsSiteEditor();
		}

		if (CurrentPage.Modules.Count == 0)
		{
			if (!CurrentPage.ShowChildPageMenu) // we'll consider Child Page Menu as a feature for our purpose here
			{
				if (WebConfigSettings.UsePageContentWizard &&
					(isAdmin ||
						isContentAdmin ||
						isSiteEditor ||
						WebUser.IsInRoles(CurrentPage.EditRoles)))
				{
					// this is to make it a little more intuitive
					// sometimes users create pages but fail to see the link in pagesettings to 
					// add features so if they visit the empty page give them an easy opportunity
					// to add a feature
					var wiz = Page.LoadControl("~/Admin/Controls/PageContentWizard.ascx");

					MPContent.Controls.Add(wiz);
				}
			}

			return;
		}

		foreach (Module module in CurrentPage.Modules)
		{
			if (!ModuleIsVisible(module) ||
				!WebUser.IsInRoles(module.ViewRoles) &&
				!isContentAdmin &&
				!isSiteEditor ||
				module.ViewRoles == "Admins;" &&
				!isAdmin)
			{
				continue;
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
				if (AltPane1 is not null)
				{
					parent = AltPane1;
				}
				else
				{
					_log.Error("Content is assigned to altcontent1 placeholder but it does not exist in layout.master so using center.");

					parent = MPContent;
				}
			}

			if (module.PaneName.IsCaseInsensitiveMatch("altcontent2"))
			{
				if (AltPane2 is not null)
				{
					parent = AltPane2;
				}
				else
				{
					_log.Error("Content is assigned to altcontent2 placeholder but it does not exist in layout.master so using center.");

					parent = MPContent;
				}
			}

			// 2008-10-04 since more an more of our features use postback via ajax
			// its not feasible to use output caching as this breaks postback,
			// so I changed the default the use of WebConfigSettings.DisableContentCache to true
			// this also reduces the memory consumption footprint
			if (module.CacheTime == 0 || WebConfigSettings.DisableContentCache)
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
					var controlVirtualPath = "~/" + module.ControlSource;

					if (!Plugins.PluginFileResolver.FileExists(controlVirtualPath))
					{
						_log.Warn($"Skipping module '{module.ModuleTitle}' because control '{module.ControlSource}' was not found (plugin may be unloaded or disabled).");
						continue;
					}

					var control = Page.LoadControl(Plugins.PluginFileResolver.ResolveVirtualPath(controlVirtualPath));

					if (control is null)
					{
						continue;
					}

					if (control is SiteModuleControl siteModule)
					{
						siteModule.SiteId = siteSettings.SiteId;
						siteModule.ModuleConfiguration = module;

						if (siteModule is IWorkflow)
						{
							if (WebUser.IsInRoles(module.AuthorizedEditRoles))
							{
								_forceShowWorkflow = true;
							}

							if (WebUser.IsInRoles(module.DraftEditRoles))
							{
								_forceShowWorkflow = true;
							}

							_countOfIWorkflow += 1;
						}
					}

					parent.Controls.Add(control);
				}
				catch (HttpException ex)
				{
					_log.Error("failed to load control " + module.ControlSource, ex);
				}
			}
			else
			{
				var siteModule = new CachedSiteModuleControl(module)
				{
					SiteId = siteSettings.SiteId,
				};

				parent.Controls.Add(siteModule);
			}

			parent.Visible = true;
			parent.Parent.Visible = true;
		}

		SetupAdminLinks();

		if (!WebConfigSettings.DisableExternalCommentSystems &&
			siteSettings is not null &&
			CurrentPage is not null &&
			CurrentPage.EnableComments)
		{
			switch (siteSettings.CommentProvider)
			{
				case "disqus":
					if (siteSettings.DisqusSiteShortName.Length > 0)
					{
						var disqus = new DisqusWidget
						{
							SiteShortName = siteSettings.DisqusSiteShortName,
							WidgetPageUrl = WebUtils.ResolveServerUrl(SiteUtils.GetCurrentPageUrl()),
							RenderWidget = true
						};

						if (disqus.WidgetPageUrl.StartsWith("https"))
						{
							disqus.WidgetPageUrl = disqus.WidgetPageUrl.Replace("https", "http");
						}

						MPContent.Controls.Add(disqus);
					}

					break;

				case "intensedebate":
					if (siteSettings.IntenseDebateAccountId.Length > 0)
					{
						var d = new IntenseDebateDiscussion
						{
							AccountId = siteSettings.IntenseDebateAccountId,
							PostUrl = SiteUtils.GetCurrentPageUrl()
						};

						MPContent.Controls.Add(d);
					}

					break;

				case "facebook":
					FacebookCommentWidget fbComments = new FacebookCommentWidget
					{
						AutoDetectUrl = true
					};

					MPContent.Controls.Add(fbComments);

					break;
			}
		}

		if (WebConfigSettings.HidePageViewModeIfNoWorkflowItems && _countOfIWorkflow == 0)
		{
			HideViewSelector();
		}

		// (to show the last mnodified time of a page we may have this control in layout.master, but I set it invisible by default
		// because we only want to show it on content pages not edit pages
		// since Default.aspx.cs is the handler for content pages, we look for it here and make it visible.
		var pageLastMod = Master.FindControl("pageLastMod");

		pageLastMod?.Visible = true;
	}


	private void EnsurePageAndSite()
	{
		if (CurrentPage is null)
		{
			var siteCount = SiteSettings.SiteCount();

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
		if (CurrentPage.PageId == -1)
		{
			return;
		}

		if (!CurrentPage.AllowBrowserCache)
		{
			SecurityHelper.DisableBrowserCache();
		}
	}


	private void SetupAdminLinks()
	{
		if (Page.Master.FindControl("lnkPageContent") is null)
		{
			MPPageEdit.Controls.Add(new PageEditFeaturesLink());
		}

		if (Page.Master.FindControl("lnkPageSettings") is null)
		{
			MPPageEdit.Controls.Add(new PageEditSettingsLink());
		}

		SetupWorkflowControls(_forceShowWorkflow);
	}
}
