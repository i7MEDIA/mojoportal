using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Configuration;
using mojoPortal.Core.Helpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	public class ScriptLoader : WebControl
	{

		#region Properties

		ScriptManager scriptManager = null;
		private string protocol = "http";
		private SiteSettings siteSettings;
		/// <summary>
		/// if you set this to false then you need this in layout.master ScriptManagerDeclaration:
		/// <asp:ScriptManager runat="server">
		///    <Scripts>
		///        <asp:ScriptReference Name="MsAjaxBundle" />
		///        ...
		///    </Scripts>
		///</asp:ScriptManager>
		///with this as true we are just doing this programatically
		///because most existing skins won't have the above
		/// </summary>
		public bool AddMSAjaxScriptReference { get; set; } = true;

		public string LanguageCode { get; set; } = "en";

		/// <summary>
		/// this is used to set a javascript variable to enable prompting a user if they try to navigate away from a page with unsaved content
		/// it does not do anything by itself it still requires a call to the hookupGoodbyePrompt method which is included in mojocombined.js
		/// TinyMCE and CKeditor will call that method when content in the editor has changed
		/// Note that the save button or any button that does postback will also trigger the prompt so we have to add extra javascript to the save buttons
		/// in features to set the variable to false in order to prevent the prompt when the user does click the button to save the content
		/// So edit pages should set ScriptConfig.EnableExitPromptForUnsavedContent = true; and should also add script to their save buttons to implement this feature
		/// we have a helper mthod for that: UIHelper.AddClearPageExitCode(btnUpdate);
		/// study the edit pages for existing features to learn more
		/// </summary>
		public bool EnableExitPromptForUnsavedContent { get; set; } = false;

		private bool includeGoogleGeoLocator = false;
		public bool IncludeGoogleGeoLocator
		{
			get { return includeGoogleGeoLocator; }
			set
			{
				includeGoogleGeoLocator = value;

				if (includeGoogleGeoLocator)
				{
					IncludeGoogleMaps = true;
				}
			}
		}

		public bool IncludeGoogleMaps { get; set; } = false;
		public bool IncludeGoogleSearch { get; set; } = false;
		public bool IncludeGoogleSearchV2 { get; set; } = false;
		public string GoogleSearchV2Id { get; set; } = string.Empty;

		public bool IncludeGoogleAnalytics { get; set; } = true;

		public string GoogleAnalyticsAccountCode { get; set; }

		private bool includeSimpleFaq = true;

		public bool IncludeSimpleFaq
		{
			get
			{
				return includeSimpleFaq;
			}
			set
			{
				includeSimpleFaq = value;

				if (includeSimpleFaq)
				{
					IncludeJQuery = true;
				}
			}
		}

		public bool IncludeJQuery { get; set; } = true;

		private bool includejPlayer = false;
		public bool IncludejPlayer
		{
			get
			{
				return includejPlayer;
			}
			set
			{
				includejPlayer = value;

				if (includejPlayer)
				{
					IncludeJQuery = true;
				}
			}
		}

		private bool includejQueryCycle = false;
		public bool IncludejQueryCycle
		{
			get
			{
				return includejQueryCycle;
			}
			set
			{
				includejQueryCycle = value;

				if (includejQueryCycle)
				{
					IncludeJQuery = true;
				}
			}
		}

		private bool includeNivoSlider = false;
		public bool IncludeNivoSlider
		{
			get
			{
				return includeNivoSlider;
			}
			set
			{
				includeNivoSlider = value;

				if (includeNivoSlider)
				{
					IncludeJQuery = true;
				}
			}
		}

		private bool includejQueryValidator = false;
		public bool IncludejQueryValidator
		{
			get
			{
				return includejQueryValidator;
			}
			set
			{
				includejQueryValidator = value;

				if (includejQueryValidator)
				{
					IncludeJQuery = true;
				}
			}
		}

		public bool IncludejQueryUICore { get; set; } = true;

		private bool includejQueryAccordion = true;

		public bool IncludejQueryAccordion
		{
			get
			{
				return includejQueryAccordion;
			}
			set
			{
				includejQueryAccordion = value;

				if (includejQueryAccordion)
				{
					IncludejQueryUICore = true;
				}
			}
		}

		/// <summary>
		/// can be an empty string or a json string with configuration for the jqueryui tabs example: { fx: { opacity: 'toggle', duration: 'fast'} }
		/// </summary>
		public string JQueryTabConfig { get; set; } = "{}";

		/// <summary>
		/// can be an empty string or a json string with configuration for the jqueryui tabs example: { fx: { opacity: 'toggle', duration: 'fast'} }
		/// </summary>
		public string JQueryAccordionConfig { get; set; } = "{}";

		/// <summary>
		/// can be an empty string or a json string with configuration for the jqueryui tabs example: { fx: { opacity: 'toggle', duration: 'fast'} }
		/// </summary>
		public string JQueryAccordionNoHeightConfig { get; set; } = "{heightStyle:'content',animate:{opacity:'toggle',duration:'400'}}";

		private bool includeJqueryScroller = false;
		public bool IncludeJqueryScroller
		{
			get
			{
				return includeJqueryScroller;
			}
			set
			{
				includeJqueryScroller = value;

				if (includeJqueryScroller)
				{
					IncludeJQuery = true;
				}
			}
		}

		public bool UseMobileVersionOfFlickrGallery { get; set; } = false;

		private bool includeColorBox = false;
		public bool IncludeColorBox
		{
			get
			{
				return includeColorBox;
			}
			set
			{
				includeColorBox = value;

				if (includeColorBox)
				{
					IncludeJQuery = true;
				}
			}
		}

		public bool IncludeGreyBox { get; set; } = false;
		public bool IncludeSizzle { get; set; } = false;


		private bool includeColorPicker = false;
		public bool IncludeColorPicker
		{
			get { return includeColorPicker; }
			set
			{
				includeColorPicker = value;
			}
		}


		public bool RenderjQueryInHead { get; set; } = true;
		public bool AssumejQueryIsLoaded { get; set; } = false;
		public bool AssumejQueryUiIsLoaded { get; set; } = false;
		public bool AssumeMarkitupIsLoaded { get; set; } = false;
		public bool AssumeMojoCombinedIsLoaded { get; set; } = false;
		public bool IncludeAspTreeView { get; set; } = false;
		public bool IncludeAspMenu { get; set; } = false;

		/// <summary>
		/// if either includeAjaxToolkit or alwaysIncludeAjaxToolkit is true
		/// a script reference will be added to script manager
		/// equivalent to this in the layout.master file:
		/// <asp:ScriptReference Name="MsAjaxBundle" />
		/// 
		/// includeAjaxToolkit is meant to be set programatically to true per page if ajaxcontroltoolkit is needed
		/// alwaysIncludeAjaxToolkit is false by default but if you want to force it to load the scripts for ajaxtoolkit 
		/// you can set it true ie if your custom code that depends on ajaxtoolkit is not setting includeAjaxToolkit 
		/// programtically for you. Typically you would get a reference to mojoBasePage.ScriptConfig.IncludeAjaxToolkit = true;
		/// </summary>
		public bool IncludeAjaxToolkit { get; set; } = false;

		/// <summary>
		/// if you set this as false then you would need to add this in the <Scripts></Scripts>
		/// element inside <ScriptManager></ScriptManager> in the layout.master file:
		/// <asp:ScriptReference Name="MsAjaxBundle" />
		/// with this true we are just adding it programatically because most existing skins don't have that
		/// </summary>
		public bool AlwaysIncludeAjaxToolkit { get; set; } = false;

		/// <summary>
		/// if this is true in addition to either includeAjaxToolkit or alwaysIncludeAjaxToolkit
		/// then the css for the toolkit will be rendered in the render method of this control
		/// </summary>
		public bool AutoAddAjaxToolkitCss { get; set; } = true;


		private bool includeJqueryTmpl = false;
		public bool IncludeJqueryTmpl
		{
			get
			{
				return includeJqueryTmpl;
			}
			set
			{
				includeJqueryTmpl = value;

				if (includeJqueryTmpl)
				{
					IncludeJQuery = true;
				}
			}
		}

		public bool IncludeKnockoutJs { get; set; } = false;

		/// <summary>
		///  in case you need to override with a different script file to include more things
		/// </summary>
		public string MojoCombinedFullScript { get; set; } = "/mojocombined/mojocombinedfull.js";
		public bool RenderCombinedInHead { get; set; } = false;
		public bool CombineScriptsWithScriptManager { get; set; } = true;
		public bool IncludeColorboxHelp { get; set; } = true;
		public string ColorBoxConfig { get; set; } = "{width:'85%', height:'85%', iframe:true}";
		public bool IncludeModernizr { get; set; } = false;
		public string ModernizrFileName { get; set; } = "modernizr-2.5.3.min.js";
		public bool IncludeJQueryMigrate { get; set; } = false;
		public string JQueryMigrateUrl { get; set; } = "~/ClientScript/jqmojo/jquery-migrate1-0-0.js";

		public bool AutoWireJQueryUITooltip { get; set; } = true;
		public bool IncludeJQTable { get; set; } = false;
		public bool DisableJQTable { get; set; } = true;
		public string MediaElementScriptPath { get; set; } = "~/ClientScript/mediaelement2-13-1/mediaelement-and-player.min.js";

		public string KnockouJsPath { get; set; } = "~/ClientScript/jqmojo/knockout-1.2.0.js";

		private bool includejPlayerPlaylist = false;
		public bool IncludejPlayerPlaylist
		{
			get { return includejPlayerPlaylist; }
			set
			{
				includejPlayerPlaylist = value;
				if (includejPlayerPlaylist)
				{
					IncludeJQuery = true;
					includejPlayer = true;
					IncludeJQueryMigrate = true;
				}
			}
		}

		public string ScriptFormatRef { get; set; } = "\n<script src=\"{0}\" data-loader=\"scriptloader\"></script>";
		public string ScriptFormatRefAsync { get; set; } = "\n<script async src=\"{0}\" data-loader=\"scriptloader\"></script>";
		public string ScriptFormatInline { get; set; } = "\n<script data-loader=\"scriptloader\">{0}</script>";
		#endregion

		protected override void Render(HtmlTextWriter writer)
		{

			// Modernizr needs to be the first script in the head
			if (IncludeModernizr)
			{
				writer.WriteLine(string.Format(ScriptFormatRef, Page.ResolveUrl("~/ClientScript/" + ModernizrFileName)));
			}

			if (IncludeGoogleAnalytics)
			{
				writer.WriteLine(string.Format(ScriptFormatRefAsync, $"{AppConfig.GoogleAnalyticsScript}{GoogleAnalyticsAccountCode}"));
				writer.WriteLine(string.Format(ScriptFormatRef, Page.ResolveUrl($"{AppConfig.GoogleAnalyticsInitScript}?id={GoogleAnalyticsAccountCode}")));
			}

			if (RenderjQueryInHead)
			{
				if (WebConfigSettings.DisablejQuery)
				{
					return;
				}

				if (!AssumejQueryIsLoaded && IncludeJQuery)
				{
					string jqueryBasePath = GetJQueryBasePath();

					writer.Write(string.Format(ScriptFormatRef, $"{jqueryBasePath}jquery.min.js"));

					if (IncludeJQueryMigrate)
					{
						writer.Write(string.Format(ScriptFormatRef, Page.ResolveUrl(JQueryMigrateUrl)));
					}
				}

				if (!AssumejQueryUiIsLoaded && IncludejQueryUICore)
				{
					string jqueryUIBasePath = GetJQueryUIBasePath();

					writer.Write(string.Format(ScriptFormatRef, $"{jqueryUIBasePath}jquery-ui.min.js"));

				}
			}

			if (!AssumeMojoCombinedIsLoaded && RenderCombinedInHead)
			{
				writer.Write(string.Format(ScriptFormatRef, Page.ResolveUrl($"~/ClientScript/{MojoCombinedFullScript}?{WebConfigSettings.mojoCombinedScriptVersionParam}")));
			}

			if (IncludeGoogleSearchV2 && GoogleSearchV2Id.Length > 0)
			{
				writer.Write(string.Format(ScriptFormatInline, BuildGoogleSearchV2Script()));
			}

			if (IncludeAjaxToolkit || AlwaysIncludeAjaxToolkit)
			{
				if (!WebConfigSettings.DisableAjaxToolkitBundlesAndScriptReferences)
				{
					if (AutoAddAjaxToolkitCss)
					{
						writer.Write(Styles.Render("~/Content/AjaxControlToolkit/Styles/Bundle").ToHtmlString());
					}
				}
			}
		}


		private string BuildGoogleSearchV2Script()
		{
			var script = $@"(function() {{
var cx = '{GoogleSearchV2Id}';
var gcse = document.createElement('script');	
gcse.type = 'text/javascript';
gcse.async = true;
gcse.src = (document.location.protocol == 'https:' ? 'https:' : 'http:') + '//www.google.com/cse/cse.js?cx=' + cx; 
var s = document.getElementsByTagName('script')[0];
s.parentNode.insertBefore(gcse, s);
}})();";

			return script.RemoveLineBreaks(Replacement: " ").RemoveMultipleSpaces();
		}


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			CombineScriptsWithScriptManager = false;

			if (Page.Master != null)
			{
				scriptManager = Page.Master.FindControl("ScriptManager1") as ScriptManager;

				if (scriptManager == null)
				{
					scriptManager = Page.Form.FindControl("ScriptManager1") as ScriptManager;
				}
			}

			//setup these in page load to ensure they come in before other scripts that may depend on them
			SetupCoreScripts();
		}


		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			if (HttpContext.Current == null || HttpContext.Current.Request == null)
			{
				return;
			}

			if (WebHelper.IsSecureRequest())
			{
				protocol = "https";
			}

			siteSettings = CacheHelper.GetCurrentSiteSettings();


			if (WebConfigSettings.GoogleAnalyticsRolesToExclude.Length > 0)
			{
				if (WebUser.IsInRoles(WebConfigSettings.GoogleAnalyticsRolesToExclude))
				{
					IncludeGoogleAnalytics = false;
				}
			}

			GoogleAnalyticsAccountCode = siteSettings.GoogleAnalyticsAccountCode;

			// webconfig overrides site settings, this is helpful with demo site where public has access to change the value in site settings
			if (ConfigurationManager.AppSettings["GoogleAnalyticsProfileId"] != null)
			{
				GoogleAnalyticsAccountCode = ConfigurationManager.AppSettings["GoogleAnalyticsProfileId"].ToString();
			}

			if (string.IsNullOrWhiteSpace(GoogleAnalyticsAccountCode) || GoogleAnalyticsAccountCode.StartsWith("UA"))
			{
				IncludeGoogleAnalytics = false;
			}

			SetupScripts();
		}


		private void SetupCoreScripts()
		{
			SetupmojoCombined();

			if (!WebConfigSettings.DisablejQuery)
			{
				SetupJQuery();

				if (!WebConfigSettings.DisablejQueryUI)
				{
					SetupJQueryUICore();
				}
			}
		}

		private void SetupScripts()
		{
			if (AddMSAjaxScriptReference)
			{
				SetupMSAjaxScripts(scriptManager);
			}

			if (IncludeAjaxToolkit || AlwaysIncludeAjaxToolkit)
			{
				SetupAjaxControlToolkitScripts(scriptManager);
			}

			if (IncludeAspTreeView)
			{
				SetupAspTreeView();
			}

			if (IncludeAspMenu)
			{
				SetupAspMenu();
			}

			if (!WebConfigSettings.DisablejQuery)
			{
				if (WebConfigSettings.DisablejQueryUI)
				{
					return;
				}

				SetupInitScript();

				if (includejQueryValidator)
				{
					SetupJQueryValidate();
				}

				if (includeColorBox)
				{
					SetupColorBox();
				}

				if (includejPlayer)
				{
					SetupjPlayer();
				}

				if (includeJqueryScroller)
				{
					SetupScroller();
				}

				if (includejQueryCycle)
				{
					SetupjQueryCycle();
				}

				if (includeNivoSlider)
				{
					SetupNivoSlider();
				}
			}

			if (IncludeKnockoutJs)
			{
				SetupKnockoutJs();
			}

			SetupBrowserSpecificScripts();

			if (IncludeGreyBox)
			{
				SetupGreyBox();
			}

			SetupGoogleAjax();

			if (IncludeSizzle)
			{
				SetupSizzle();
			}

			if (EnableExitPromptForUnsavedContent)
			{
				ScriptManager.RegisterStartupScript(
					Page,
					typeof(Page),
					"requireExitPrompt",
					string.Format(ScriptFormatInline, "requireExitPrompt = true;"),
					false
				);
			}
		}


		private void SetupInitScript()
		{
			if (includejQueryAccordion)
			{
				// this also includes jqueryui tabs
				var initAutoScript = new StringBuilder();

				initAutoScript.Append($"$('div.mojo-accordion').accordion({JQueryAccordionConfig});");
				initAutoScript.Append($"$('div.mojo-accordion-nh').accordion({JQueryAccordionNoHeightConfig});");
				initAutoScript.Append($"$('div.mojo-tabs').tabs({JQueryTabConfig}); $('input.jqbutton').button();");

				if (AutoWireJQueryUITooltip)
				{
					initAutoScript.Append("$('.jqtt').tooltip(); ");
				}

				if (includeColorBox && IncludeColorboxHelp)
				{
					initAutoScript.Append($"$('a.cblink').colorbox({ColorBoxConfig});");
				}

				if (includeSimpleFaq)
				{
					initAutoScript.Append("$('.faqs dd').hide();"); // Hide all DDs inside .faqs
					initAutoScript.Append("$('.faqs dt').hover(function(){$(this).addClass('hover')},function(){$(this).removeClass('hover')}).click(function(){ "); // Add class "hover" on dt when hover
					initAutoScript.Append("$(this).next().slideToggle('normal'); });"); // Toggle dd when the respective dt is clicked
				}

				if (IncludeJQTable && !DisableJQTable)
				{
					initAutoScript.Append(@"
$('table.jqtable th').each(function() {
	$(this).addClass('ui-state-default');
});

$('table.jqtable td').each(function() {
	$(this).addClass('ui-widget-content');
});

$('table.jqtable tr').hover(function() {
	$(this).children('td').addClass('ui-state-hover');
}, function () {
	$(this).children('td').removeClass('ui-state-hover');
});

$('table.jqtable tr').click(function() {
	$(this).children('td').toggleClass('ui-state-highlight');
});");
				}

				if (Page is mojoBasePage)
				{
					mojoBasePage basePage = Page as mojoBasePage;

					if (basePage.StyleCombiner.EnableNonClickablePageLinks)
					{
						initAutoScript.Append("$(\"a.unclickable\").click(function() { return false; });");
					}
				}

				ScriptManager.RegisterStartupScript(
					this,
					typeof(Page),
					"jui-init",
					string.Format(ScriptFormatInline, initAutoScript),
					false
				);
			}
		}


		private void SetupWebFormsScripts(ScriptManager scriptManager)
		{
			if (scriptManager == null)
			{
				return;
			}

			var script = new ScriptReference("WebForms.js", "System.Web")
			{
				Path = "~/Scripts/WebForms/WebForms.js"
			};

			scriptManager.Scripts.Add(script);

			script = new ScriptReference("WebUIValidation.js", "System.Web")
			{
				Path = "~/Scripts/WebForms/WebUIValidation.js"
			};

			scriptManager.Scripts.Add(script);

			script = new ScriptReference("MenuStandards.js", "System.Web")
			{
				Path = "~/Scripts/WebForms/MenuStandards.js"
			};

			scriptManager.Scripts.Add(script);

			script = new ScriptReference("GridView.js", "System.Web")
			{
				Path = "~/Scripts/WebForms/GridView.js"
			};

			scriptManager.Scripts.Add(script);

			script = new ScriptReference("DetailsView.js", "System.Web")
			{
				Path = "~/Scripts/WebForms/DetailsView.js"
			};

			scriptManager.Scripts.Add(script);

			script = new ScriptReference("TreeView.js", "System.Web")
			{
				Path = "~/Scripts/WebForms/TreeView.js"
			};

			scriptManager.Scripts.Add(script);

			script = new ScriptReference("WebParts.js", "System.Web")
			{
				Path = "~/Scripts/WebForms/WebParts.js"
			};

			scriptManager.Scripts.Add(script);

			script = new ScriptReference("Focus.js", "System.Web")
			{
				Path = "~/Scripts/WebForms/Focus.js"
			};

			scriptManager.Scripts.Add(script);
		}


		private void SetupMSAjaxScripts(ScriptManager scriptManager)
		{
			if (scriptManager == null)
			{
				return;
			}

			SetupWebFormsScripts(scriptManager);

			ScriptReference scriptReference = new ScriptReference("~/bundles/WebFormsJs")
			{
				Name = "WebFormsBundle"
			};

			scriptManager.Scripts.Add(scriptReference);

			scriptReference = new ScriptReference("~/bundles/MsAjaxJs")
			{
				Name = "MsAjaxBundle"
			};

			scriptManager.Scripts.Add(scriptReference);

			if (WebConfigSettings.BundlesUseCdn)
			{
				scriptManager.EnableCdn = true;
				// for some reason with true it was using the cdn url as the fallback url
				// which is redundant
				scriptManager.EnableCdnFallback = false;
			}
		}


		private void SetupAjaxControlToolkitScripts(ScriptManager scriptManager)
		{
			if (WebConfigSettings.DisableAjaxToolkitBundlesAndScriptReferences || scriptManager == null)
			{
				return;
			}

			var scriptReference = new ScriptReference("~/Scripts/AjaxControlToolkit/Bundle");

			scriptManager.Scripts.Add(scriptReference);
		}


		private void AddPathScriptReference(ScriptManager scriptManager, ScriptReference scriptReference)
		{
			if (scriptManager == null || scriptReference == null)
			{
				return;
			}

			foreach (ScriptReference s in scriptManager.CompositeScript.Scripts)
			{
				if (s.Path == scriptReference.Path)
				{
					return;
				}
			}

			scriptManager.CompositeScript.Scripts.Add(scriptReference);
		}


		/// <summary>
		/// should be in the format ~/path/to/yourscript.js
		/// </summary>
		/// <param name="scriptRelativeUrl"></param>
		public void AddPathScriptReference(string scriptRelativeUrl)
		{
			ScriptReference script = new ScriptReference
			{
				Path = scriptRelativeUrl
			};

			AddPathScriptReference(scriptManager, script);
		}


		#region jQuery

		private string GetJQueryBasePath()
		{
			if (WebConfigSettings.UseGoogleCDN)
			{
				var baseUrl = WebConfigSettings.GoogleCDNJQueryBaseUrl;
				if (baseUrl.StartsWith("//"))
				{
					baseUrl = $"{protocol}:{baseUrl}";
				}
				return $"{baseUrl + WebConfigSettings.GoogleCDNjQueryVersion}/";
			}
			return Page.ResolveUrl(WebConfigSettings.jQueryBasePath);
		}


		private void SetupJQuery()
		{
			if (RenderjQueryInHead)
			{
				return;
			}

			if (AssumejQueryIsLoaded)
			{
				return;
			}

			string jqueryBasePath = GetJQueryBasePath();

			if (WebConfigSettings.UseGoogleCDN)
			{
				Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "jquery", string.Format(ScriptFormatRef, $"{jqueryBasePath}jquery.min.js"));
			}
			else
			{
				if (CombineScriptsWithScriptManager)
				{
					ScriptReference script = new ScriptReference
					{
						Path = jqueryBasePath + "jquery.min.js"
					};

					AddPathScriptReference(scriptManager, script);
				}
				else
				{
					Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "jquery", string.Format(ScriptFormatRef, $"{jqueryBasePath}jquery.min.js"));
				}
			}
		}


		private string GetJQueryUIBasePath()
		{
			if (WebConfigSettings.UseGoogleCDN)
			{
				var baseUrl = WebConfigSettings.GoogleCDNJQueryUIBaseUrl;
				if (baseUrl.StartsWith("//"))
				{
					baseUrl = $"{protocol}:{baseUrl}";
				}
				return $"{baseUrl + WebConfigSettings.GoogleCDNjQueryUIVersion}/";
			}

			return Page.ResolveUrl(WebConfigSettings.jQueryUIBasePath);
		}


		private void SetupJQueryUICore()
		{
			if (RenderjQueryInHead)
			{
				return;
			}

			string jqueryUIBasePath = GetJQueryUIBasePath();

			if (IncludejQueryUICore && !AssumejQueryUiIsLoaded)
			{
				if (WebConfigSettings.UseGoogleCDN)
				{
					Page.ClientScript.RegisterClientScriptBlock(
						typeof(Page),
						"jqueryui-core",
						string.Format(ScriptFormatRef, Page.ResolveUrl("~/ClientScript/" + ModernizrFileName))
					);
				}
				else
				{
					if (CombineScriptsWithScriptManager)
					{
						ScriptReference script = new ScriptReference
						{
							Path = jqueryUIBasePath + "jquery-ui.min.js"
						};

						AddPathScriptReference(scriptManager, script);
					}
					else
					{
						Page.ClientScript.RegisterClientScriptBlock(
							typeof(Page),
							"jqueryui-core",
							string.Format(ScriptFormatRef, Page.ResolveUrl("~/ClientScript/" + ModernizrFileName))
						);
					}
				}
			}
		}


		private void SetupAspTreeView()
		{

			if (CombineScriptsWithScriptManager)
			{
				ScriptReference script = new ScriptReference
				{
					Path = "~/ClientScript/CSSFriendly/AdapterUtils.min.js"
				};

				AddPathScriptReference(scriptManager, script);

				script = new ScriptReference
				{
					Path = "~/ClientScript/CSSFriendly/TreeViewAdapter.min.js"
				};

				AddPathScriptReference(scriptManager, script);
			}
			else
			{
				Page.ClientScript.RegisterClientScriptBlock(
					typeof(Page),
					"cssadapterutils",
					string.Format(ScriptFormatRef, $"{Page.ResolveUrl("~/ClientScript/CSSFriendly/AdapterUtils.min.js")}")
				);
				Page.ClientScript.RegisterClientScriptBlock(
					typeof(Page),
					"treeviewadapter",
					string.Format(ScriptFormatRef, $"{Page.ResolveUrl("~/ClientScript/CSSFriendly/TreeViewAdapter.min.js")}")
				);
			}
		}


		private void SetupAspMenu()
		{
			if (CombineScriptsWithScriptManager)
			{
				ScriptReference script = new ScriptReference
				{
					Path = "~/ClientScript/CSSFriendly/AdapterUtils.min.js"
				};

				AddPathScriptReference(scriptManager, script);

				script = new ScriptReference
				{
					Path = "~/ClientScript/CSSFriendly/MenuAdapter.min.js"
				};

				AddPathScriptReference(scriptManager, script);
			}
			else
			{
				Page.ClientScript.RegisterClientScriptBlock(
					typeof(Page),
					"cssadapterutils",
					string.Format(ScriptFormatRef, $"{Page.ResolveUrl("~/ClientScript/CSSFriendly/AdapterUtils.min.js")}")
				);
				Page.ClientScript.RegisterClientScriptBlock(
					typeof(Page),
					"aspmenuadapter",
					string.Format(ScriptFormatRef, $"{Page.ResolveUrl("~/ClientScript/CSSFriendly/MenuAdapter.min.js")}")
				);
			}
		}


		private void SetupjQueryCycle()
		{
			if (WebConfigSettings.DisablejQueryUI)
			{
				return;
			}

			Page.ClientScript.RegisterClientScriptBlock(
				typeof(Page),
				"jqcycle",
				string.Format(ScriptFormatRef, $"{Page.ResolveUrl("~/ClientScript/jqmojo/jquery.cycle.all.min.js")}")
			);
		}


		private void SetupNivoSlider()
		{
			if (WebConfigSettings.DisablejQueryUI)
			{
				return;
			}

			ScriptManager.RegisterClientScriptBlock(
				this,
				typeof(Page),
				"nivoslidermain",
				string.Format(ScriptFormatRef, $"{Page.ResolveUrl("~/ClientScript/jqmojo/jquery.nivo.slider.pack3-2.js")}"),
				false
			);
		}


		//http://mediaelementjs.com/
		private void SetupMediaElement()
		{
			if (WebConfigSettings.DisablejQuery)
			{
				return;
			}

			ScriptManager.RegisterClientScriptBlock(
				this,
				typeof(Page),
				"mediaelementmain",
				string.Format(ScriptFormatRef, Page.ResolveUrl(MediaElementScriptPath)),
				false
			);
		}

		private void SetupKnockoutJs()
		{
			if (WebConfigSettings.DisablejQueryUI) { return; }

			if (CombineScriptsWithScriptManager)
			{
				ScriptReference script = new ScriptReference
				{
					Path = KnockouJsPath
				};

				AddPathScriptReference(scriptManager, script);
			}
			else
			{
				Page.ClientScript.RegisterClientScriptBlock(
					typeof(Page),
					"knockoutjs",
					string.Format(ScriptFormatRef, Page.ResolveUrl(KnockouJsPath))
				);
			}
		}

		private void SetupJQueryValidate()
		{
			if (CombineScriptsWithScriptManager)
			{
				ScriptReference script = new ScriptReference
				{
					Path = "~/ClientScript/jqmojo/jquery.validate.min.js"
				};

				AddPathScriptReference(scriptManager, script);
			}
			else
			{
				Page.ClientScript.RegisterClientScriptBlock(
					typeof(Page),
					"jqvalidate",
					string.Format(ScriptFormatRef, Page.ResolveUrl("~/ClientScript/jqmojo/jquery.validate.min.js"))
				);
			}
		}

		private void SetupSizzle()
		{
			if (CombineScriptsWithScriptManager)
			{
				ScriptReference script = new ScriptReference
				{
					Path = "~/ClientScript/jqmojo/sizzle.js"
				};

				AddPathScriptReference(scriptManager, script);
			}
			else
			{
				Page.ClientScript.RegisterClientScriptBlock(
					typeof(Page),
					"sizzle",
					string.Format(ScriptFormatRef, Page.ResolveUrl("~/ClientScript/jqmojo/sizzle.js"))
				);
			}
		}


		private void SetupImpromtu()
		{
			Page.ClientScript.RegisterClientScriptBlock(
				typeof(Page),
				"jqprompt",
				string.Format(ScriptFormatRef, Page.ResolveUrl("~/ClientScript/jqmojo/jquery-impromptu42.min.js"))
			);
		}


		private void SetupColorBox()
		{
			if (CombineScriptsWithScriptManager)
			{
				ScriptReference script = new ScriptReference
				{
					Path = "~/ClientScript/colorbox/jquery.colorbox-min.js"
				};

				AddPathScriptReference(scriptManager, script);
			}
			else
			{
				Page.ClientScript.RegisterClientScriptBlock(
					typeof(Page),
					"jqcolorbox",
					string.Format(ScriptFormatRef, Page.ResolveUrl("~/ClientScript/colorbox/jquery.colorbox-min.js"))
				);
			}
		}


		private void SetupScroller()
		{
			if (CombineScriptsWithScriptManager)
			{
				ScriptReference script = new ScriptReference
				{
					Path = "~/ClientScript/jqmojo/jquery.scroller.min.js"
				};

				AddPathScriptReference(scriptManager, script);
			}
			else
			{
				Page.ClientScript.RegisterClientScriptBlock(
					typeof(Page),
					"jqscroller",
					string.Format(ScriptFormatRef, Page.ResolveUrl("~/ClientScript/jqmojo/jquery.scroller.min.js"))
				);
			}
		}


		private void SetupjPlayer()
		{
			Page.ClientScript.RegisterClientScriptBlock(
				typeof(Page),
				"jplayer",
				string.Format(ScriptFormatRef, Page.ResolveUrl(WebConfigSettings.JPlayerBasePath + "jquery.jplayer.min.js"))
			);

			if (includejPlayerPlaylist)
			{
				Page.ClientScript.RegisterClientScriptBlock(
					typeof(Page),
					"jplayer-playlist",
					string.Format(ScriptFormatRef, $"{Page.ResolveUrl(WebConfigSettings.JPlayerBasePath + "add-on/jplayer.playlist.min.js")}")
				);
			}
		}

		#endregion

		private void SetupGreyBox()
		{
			Page.ClientScript.RegisterClientScriptBlock(
				typeof(Page),
				"gbVar",
				string.Format(ScriptFormatInline, $"var GB_ROOT_DIR = '{Page.ResolveUrl("~/ClientScript/greybox/")}'; var GBCloseText = '{Resource.CloseDialogButton}';")
			);

			//Page.ClientScript.RegisterClientScriptBlock(
			//	typeof(Page),
			//	"GreyBoxJs",
			//	$"\n<script src=\"{Page.ResolveUrl(scriptBaseUrl + "gbcombined.js")}\" ></script>"
			//);

			// The commented version above is the preferred syntax, the uncommented one below is the deprecated version.
			// There is a reason we are using the deprecated version, greybox is registered also by NeatUpload using the deprecated
			// syntax, the reason they use the older syntax is because they also support .NET v1.1
			// We use the old syntax here for compatibility with NeatUpload so that it does not get registered more than once
			// on pages that use NeatUpload. Otherwise we would have to always modify our copy of NeatUpload.
			Page.ClientScript.RegisterClientScriptBlock(
				typeof(Page),
				"GreyBoxJs",
				string.Format(ScriptFormatRef, Page.ResolveUrl("~/ClientScript/greybox/gbcombined.js"))
			);
		}


		private void SetupmojoCombined()
		{
			if (RenderCombinedInHead)
			{
				return;
			}

			if (!AssumeMojoCombinedIsLoaded)
			{
				if (CombineScriptsWithScriptManager)
				{
					ScriptReference script = new ScriptReference
					{
						Path = "~/ClientScript" + MojoCombinedFullScript
					};

					AddPathScriptReference(scriptManager, script);
				}
				else
				{
					Page.ClientScript.RegisterClientScriptBlock(
						typeof(Page),
						"mojocombined",
						string.Format(ScriptFormatRef, Page.ResolveUrl($"~/ClientScript{MojoCombinedFullScript}?{WebConfigSettings.mojoCombinedScriptVersionParam}"))
					);
				}
			}
		}


		private void SetupGoogleAjax()
		{
			if (!IncludeGoogleMaps && !IncludeGoogleSearch)
			{
				return;
			}

			string googleApiKey = SiteUtils.GetGmapApiKey();

			if (googleApiKey.Length > 0)
			{
				Page.ClientScript.RegisterClientScriptBlock(
					typeof(Page),
					"gajaxmain",
					String.Format(ScriptFormatRef, $"{protocol}://www.google.com/jsapi?key={googleApiKey}")
				);
			}
			else
			{
				Page.ClientScript.RegisterClientScriptBlock(
					typeof(Page),
					"gajaxmain",
					String.Format(ScriptFormatRef, $"{protocol}://www.google.com/jsapi")
				); ;
			}

			var script = string.Format(ScriptFormatInline, $@"
{(IncludeGoogleMaps ? "google.load('maps', '2');" : string.Empty)}
{(IncludeGoogleSearch ? $"google.load('search', '1', {{ language: '{LanguageCode}' }});" : string.Empty)}
{(includeGoogleGeoLocator && Page.Request.IsAuthenticated ? $@"
function trackLocation() {{
	var location = google.loader.ClientLocation;

	if (location != null) {{
		trackUserLocation(location);
	}}
}}

google.setOnLoadCallback(trackLocation);" : string.Empty)}");

			Page.ClientScript.RegisterClientScriptBlock(
				typeof(Page),
				"gloader",
				script
			);
		}


		private void SetupBrowserSpecificScripts()
		{
			var loweredBrowser = string.Empty;

			if (HttpContext.Current.Request.UserAgent != null)
			{
				loweredBrowser = HttpContext.Current.Request.UserAgent.ToLower();
			}

			if (WebConfigSettings.UseSafariWebKitHack)
			{
				if (loweredBrowser.Contains("webkit"))
				{
					//this fixes some ajax updatepanel issues in webkit
					//http://forums.asp.net/p/1252014/2392110.aspx
					//try
					//{
						ScriptReference scriptReference = new ScriptReference
						{
							Path = Page.ResolveUrl("~/ClientScript/AjaxWebKitFix.js")
						};

						ScriptManager ajax = ScriptManager.GetCurrent(Page);

						if (ajax != null)
						{
							ajax.Scripts.Add(scriptReference);
						}
					//}
					// this can happen if SP1 is not installed for .NET 3.5
					//catch (TypeLoadException)
					//{ }
				}
			}
		}
	}
}
