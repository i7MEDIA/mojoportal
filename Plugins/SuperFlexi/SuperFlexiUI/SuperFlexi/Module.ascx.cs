using System;
using System.Text;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;

namespace SuperFlexiUI;

public partial class SuperFlexiModule : SiteModuleControl
{
	protected ModuleConfiguration config = new();

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
	}

	private void LoadSettings()
	{
		var module = new Module(ModuleGuid);
		if (module == null)
		{
			return;
		}
		config = new ModuleConfiguration(module);
		var currentPage = CacheHelper.GetCurrentPage();

		if (config.MarkupDefinition != null)
		{
			displaySettings = config.MarkupDefinition;
		}

		// this stuff should always apply, regardless if using razor or markupdef

		pnlOuterWrap.RenderContentsOnly = config.HideOuterWrapperPanel;
		pnlInnerWrap.RenderContentsOnly = config.HideInnerWrapperPanel;
		pnlOuterBody.RenderContentsOnly = config.HideOuterBodyPanel;
		pnlInnerBody.RenderContentsOnly = config.HideInnerBodyPanel;

		if (config.ModuleCssClass.Length > 0 && !config.HideOuterWrapperPanel)
		{
			pnlOuterWrap.SetOrAppendCss(config.ModuleCssClass.Replace("$_ModuleID_$", ModuleId.ToString()));
		}

		if (config.UseRazor)
		{
			var razor = new WidgetRazor
			{
				Config = config,
				PageId = PageId,
				ModuleGuid = ModuleGuid,
				ModuleId = ModuleId,
				IsEditable = IsEditable,
				SiteRoot = SiteRoot,
				ImageSiteRoot = ImageSiteRoot,
				Visible = true,
				Enabled = true
			};

			sflexi.Controls.Add(razor);
		}
		else
		{
			var legacyWidget = new WidgetLegacy
			{
				Config = config,
				ModuleId = ModuleId,
				IsEditable = IsEditable,
				SiteRoot = SiteRoot,
				ImageSiteRoot = ImageSiteRoot,
				PageId = PageId,
				CurrentPage = currentPage ?? CacheHelper.GetCurrentPage()
			};

			sflexi.Controls.Add(legacyWidget);

			if (ModuleConfiguration != null)
			{
				Title = ModuleConfiguration.ModuleTitle;
				Description = ModuleConfiguration.FeatureName;
			}
			StringBuilder moduleTitle = new StringBuilder();

			moduleTitle.Append(displaySettings.ModuleTitleMarkup);
			SuperFlexiHelpers.ReplaceStaticTokens(moduleTitle, config, IsEditable, displaySettings, module, currentPage, siteSettings, out moduleTitle);
			litModuleTitle.Text = moduleTitle.ToString();

			if (config.ModuleCssClass.Length > 0 && !config.HideOuterWrapperPanel)
			{
				pnlOuterWrap.SetOrAppendCss(config.ModuleCssClass.Replace("$_ModuleID_$", ModuleId.ToString()));
			}

			if (SiteUtils.IsMobileDevice() && config.ModuleMobileCssClass.Length > 0 && !config.HideOuterWrapperPanel)
			{
				pnlOuterWrap.SetOrAppendCss(config.ModuleMobileCssClass.Replace("$_ModuleID_$", ModuleId.ToString()));
			}

			if (config.UseHeader && config.HeaderLocation != "InnerBodyPanel" && !string.IsNullOrWhiteSpace(config.HeaderContent) && !string.Equals(config.HeaderContent, "<p>&nbsp;</p>"))
			{
				var headerContent = new StringBuilder();
				headerContent.AppendFormat(displaySettings.HeaderContentFormat, config.HeaderContent);
				SuperFlexiHelpers.ReplaceStaticTokens(headerContent, config, IsEditable, displaySettings, module, currentPage, siteSettings, out headerContent);
				var litHeaderContent = new LiteralControl(headerContent.ToString());
				litHeaderContent.EnableViewState = false;
				//if HeaderLocation is set to a hidden panel the header will be added to the Outside.
				switch (config.HeaderLocation)
				{
					default:
						break;
					case "OuterBodyPanel":
						if (config.HideOuterBodyPanel) goto case "Outside";
						pnlOuterBody.Controls.AddAt(0, litHeaderContent);
						break;

					case "InnerWrapperPanel":
						if (config.HideInnerWrapperPanel) goto case "Outside";
						pnlInnerWrap.Controls.AddAt(0, litHeaderContent);
						break;

					case "OuterWrapperPanel":
						if (config.HideOuterWrapperPanel) goto case "Outside";
						pnlOuterWrap.Controls.AddAt(0, litHeaderContent);
						break;

					case "Outside":
						litHead.Text = headerContent.ToString();
						break;
				}
			}

			if (config.UseFooter && config.FooterLocation != "InnerBodyPanel" && !string.IsNullOrWhiteSpace(config.FooterContent) && !string.Equals(config.FooterContent, "<p>&nbsp;</p>"))
			{
				var footerContent = new StringBuilder();
				footerContent.AppendFormat(displaySettings.FooterContentFormat, config.FooterContent);
				SuperFlexiHelpers.ReplaceStaticTokens(footerContent, config, IsEditable, displaySettings, module, currentPage, siteSettings, out footerContent);
				var litFooterContent = new LiteralControl(footerContent.ToString());
				litFooterContent.EnableViewState = false;
				//if FooterLocation is set to a hidden panel the footer will be added to the Outside.
				switch (config.FooterLocation)
				{
					default:
						break;
					case "OuterBodyPanel":
						if (config.HideOuterBodyPanel) goto case "Outside";
						pnlOuterBody.Controls.Add(litFooterContent);
						break;
					case "InnerWrapperPanel":
						if (config.HideInnerWrapperPanel) goto case "Outside";
						pnlInnerWrap.Controls.Add(litFooterContent);
						break;
					case "OuterWrapperPanel":
						if (config.HideOuterWrapperPanel) goto case "Outside";
						pnlOuterWrap.Controls.Add(litFooterContent);
						break;
					case "Outside":
						litFoot.Text = footerContent.ToString();
						break;
				}
			}
		}
	}
	#region OnInit

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}

	#endregion
}