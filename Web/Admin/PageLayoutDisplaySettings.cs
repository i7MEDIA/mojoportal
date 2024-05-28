namespace mojoPortal.Web.UI;

public class PageLayoutDisplaySettings : BaseDisplaySettings
{
	public override string FeatureName => "Core";
	public override string SubFeatureName => "PageLayoutPage";
	public PageLayoutDisplaySettings() : base() { }

	public string AdminLinksContainerDivCssClass { get; set; } = "breadcrumbs pageditlinks";

	public string AdminLinkSeparator { get; set; } = "&nbsp;";

	public string AdminLinkCssClass { get; set; } = string.Empty;

	/// <summary>
	/// most skins don't use menu descriptions so by default it is not shown in pagesettings.aspx
	/// set this to true if you want to show this field and populate it for use in the menu
	/// </summary>
	public bool ShowMenuDescription { get; set; } = false;

	/// <summary>
	/// most skins don't use menu images so by default it is not shown in pagesettings.aspx
	/// set this to true if you want to show this field and populate it for use in the menu
	/// </summary>
	public bool ShowMenuImage { get; set; } = false;
	public string PageLayoutUpButtonCssClass { get; set; } = "pagelayout__item-btn pagelayout__item-btn--up btn btn-sm btn-default";

	public string PageLayoutDownButtonCssClass { get; set; } = "pagelayout__item-btn pagelayout__item-btn--down btn btn-sm btn-default";

	public string PageLayoutAlt1ToCenterButtonCssClass { get; set; } = "pagelayout__item-btn pagelayout__item-btn--alt1-center btn btn-sm btn-default";

	public string PageLayoutAlt2ToCenterButtonCssClass { get; set; } = "pagelayout__item-btn pagelayout__item-btn--alt2-center btn btn-sm btn-default";

	public string PageLayoutCenterToAlt1ButtonCssClass { get; set; } = "pagelayout__item-btn pagelayout__item-btn--center-alt1 btn btn-sm btn-default";

	public string PageLayoutCenterToAlt2ButtonCssClass { get; set; } = "pagelayout__item-btn pagelayout__item-btn--center-alt2 btn btn-sm btn-default";

	public string PageLayoutLeftToRightButtonCssClass { get; set; } = "pagelayout__item-btn pagelayout__item-btn--left-right btn btn-sm btn-default";

	public string PageLayoutRightToLeftButtonCssClass { get; set; } = "pagelayout__item-btn pagelayout__item-btn--right-left btn btn-sm btn-default";

	public string PageLayoutEditButtonCssClass { get; set; } = "pagelayout__item-btn pagelayout__item-btn--edit btn btn-sm btn-default";

	public string PageLayoutDeleteButtonCssClass { get; set; } = "pagelayout__item-btn pagelayout__item-btn--delete btn btn-sm btn-default";

	public string PageLayoutUpButtonInnerHtml { get; set; } = "<svg class='mp-svg-icon'><use href='#mp-angle-up'></use></svg>";

	public string PageLayoutDownButtonInnerHtml { get; set; } = "<svg class='mp-svg-icon'><use href='#mp-angle-down'></use></svg>";

	public string PageLayoutAlt1ToCenterButtonInnerHtml { get; set; } = "<svg class='mp-svg-icon'><use href='#mp-angle-double-down'></use></svg>";

	public string PageLayoutAlt2ToCenterButtonInnerHtml { get; set; } = "<svg class='mp-svg-icon'><use href='#mp-angle-double-up'></use></svg>";

	public string PageLayoutCenterToAlt1ButtonInnerHtml { get; set; } = "<svg class='mp-svg-icon'><use href='#mp-angle-double-up'></use></svg>";

	public string PageLayoutCenterToAlt2ButtonInnerHtml { get; set; } = "<svg class='mp-svg-icon'><use href='#mp-angle-double-down'></use></svg>";

	public string PageLayoutLeftToRightButtonInnerHtml { get; set; } = "<svg class='mp-svg-icon'><use href='#mp-angle-double-right'></use></svg>";

	public string PageLayoutRightToLeftButtonInnerHtml { get; set; } = "<svg class='mp-svg-icon'><use href='#mp-angle-double-left'></use></svg>";

	public string PageLayoutEditButtonInnerHtml { get; set; } = "<svg class='mp-svg-icon'><use href='#mp-cog'></use></svg>";

	public string PageLayoutDeleteButtonInnerHtml { get; set; } = "<svg class='mp-svg-icon'><use href='#mp-trash-alt'></use></svg>";

	public string Alt1PaneCssClass { get; set; } = "pane layoutalt1";

	public string RegularLayoutPanesWrapCssClass { get; set; } = "regularpanes";

	public string RegularLayoutPaneLeftCssClass { get; set; } = "pane layoutleft";

	public string RegularLayoutPaneCenterCssClass { get; set; } = "pane layoutcenter";

	public string RegularLayoutPaneRightCssClass { get; set; } = "pane layoutright";

	public string Alt2PaneCssClass { get; set; } = "pane layoutalt2";

	public string PaneListBoxCssClass { get; set; } = "panelistbox";

	public string PageLayoutButtonGroupCssClass { get; set; } = "pagelayout-item-btns btn-group-vertical";

	public string PageLayoutButtonGroupSeparatorMarkup { get; set; } = "";
}