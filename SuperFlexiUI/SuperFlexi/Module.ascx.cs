/// Author:             Joe Davis (i7MEDIA)
/// Created:			2014-12-20
///	Last Modified:		2022-03-11
///
/// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using System;
using System.Text;
using System.Web.UI;

namespace SuperFlexiUI
{
	public partial class SuperFlexiModule : SiteModuleControl
	{
		protected ModuleConfiguration config = new ModuleConfiguration();


		protected void Page_Load(object sender, EventArgs e)
		{
			LoadSettings();
		}

		private void LoadSettings()
		{
			Module module = new Module(ModuleGuid);
			if (module == null)
			{
				return;
			}
			config = new ModuleConfiguration(module);
			PageSettings currentPage = CacheHelper.GetCurrentPage();

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

				WidgetRazor razor = new WidgetRazor
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

				//widgetRazor.Config = config;
				//widgetRazor.PageId = PageId;
				//widgetRazor.ModuleGuid = ModuleGuid;
				//widgetRazor.ModuleId = ModuleId;
				//widgetRazor.IsEditable = IsEditable;
				//widgetRazor.SiteRoot = SiteRoot;
				//widgetRazor.ImageSiteRoot = ImageSiteRoot;

				//widgetRazor.Visible = widgetRazor.Enabled = true;
				//theWidget.Visible = false;

			}
			else
			{
				//widgetRazor.Visible = widgetRazor.Enabled = false;

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

				//theWidget.Config = config;
				//if (currentPage != null)
				//{
				//	theWidget.PageId = currentPage.PageId;
				//	theWidget.CurrentPage = currentPage;
				//}
				//theWidget.ModuleId = ModuleId;
				//theWidget.IsEditable = IsEditable;
				//theWidget.SiteRoot = SiteRoot;
				//theWidget.ImageSiteRoot = ImageSiteRoot;

				//theWidget.Visible = true;

				if (config.UseHeader && config.HeaderLocation != "InnerBodyPanel" && !String.IsNullOrWhiteSpace(config.HeaderContent) && !String.Equals(config.HeaderContent, "<p>&nbsp;</p>"))
				{
					StringBuilder headerContent = new StringBuilder();
					headerContent.AppendFormat(displaySettings.HeaderContentFormat, config.HeaderContent);
					SuperFlexiHelpers.ReplaceStaticTokens(headerContent, config, IsEditable, displaySettings, module, currentPage, siteSettings, out headerContent);
					LiteralControl litHeaderContent = new LiteralControl(headerContent.ToString());
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

				if (config.UseFooter && config.FooterLocation != "InnerBodyPanel" && !String.IsNullOrWhiteSpace(config.FooterContent) && !String.Equals(config.FooterContent, "<p>&nbsp;</p>"))
				{
					StringBuilder footerContent = new StringBuilder();
					footerContent.AppendFormat(displaySettings.FooterContentFormat, config.FooterContent);
					SuperFlexiHelpers.ReplaceStaticTokens(footerContent, config, IsEditable, displaySettings, module, currentPage, siteSettings, out footerContent);
					LiteralControl litFooterContent = new LiteralControl(footerContent.ToString());
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

				//pnlOuterWrap.RenderContentsOnly = config.HideOuterWrapperPanel;
				//pnlInnerWrap.RenderContentsOnly = config.HideInnerWrapperPanel;
				//pnlOuterBody.RenderContentsOnly = config.HideOuterBodyPanel;
				//pnlInnerBody.RenderContentsOnly = config.HideInnerBodyPanel;
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
}