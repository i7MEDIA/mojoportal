using mojoPortal.Web;

namespace SuperFlexiUI;

public class SuperFlexiDisplaySettings : BasePluginDisplaySettings
{

	//public override string FeatureName => "Core";
	//public override string SubFeatureName => GetType().Name.Replace("DisplaySettings", string.Empty);
	public SuperFlexiDisplaySettings() : base() { }

	public bool HideOuterWrapperPanel { get; set; } = false;

	public bool HideInnerWrapperPanel { get; set; } = false;

	public bool HideOuterBodyPanel { get; set; } = false;

	public bool HideInnerBodyPanel { get; set; } = false;

	public string ModuleTitleMarkup { get; set; } = "$_ModuleTitle_$$_ModuleLinks_$";

	public string ModuleTitleFormat { get; set; } = "<h2 class='moduletitle'>{0}</h2>";

	public string ModuleInstanceMarkupTop { get; set; } = string.Empty;

	public string ModuleInstanceMarkupBottom { get; set; } = string.Empty;

	public string InstanceFeaturedImageFormat { get; set; } = string.Empty;

	public string HeaderContentFormat { get; set; } = "<div class='flexi-header'>{0}</div>";

	public string FooterContentFormat { get; set; } = "<div class='flexi-footer'>{0}</div>";

	public string ItemMarkup { get; set; } = string.Empty;

	public string ItemsWrapperFormat { get; set; } = "<div class='flexi-items'>{0}</div>";

	public string ItemsRepeaterMarkup { get; set; } = "$_Items_$";

	public int ItemsPerGroup { get; set; } = -1;

	/// <summary>
	/// {0} = ModuleSettingsLink,
	/// {1} = AddItemLink,
	/// {2} = EditHeaderLink,
	/// {3} = EditFooterLink,
	/// {4} = ImportLink,
	/// {5} = ExportLink
	/// </summary>
	public string ModuleLinksFormat { get; set; } = "<span class=\"modulelinks flexi-module-links\">{0}{1}{2}{3}{4}{5}</span>";

	public string ModuleSettingsLinkFormat { get; set; } = "&nbsp;<a class='ModuleEditLink' href='{0}'>{1}</a>";

	/// <summary>
	/// {0} - url,
	/// {1} - title
	/// </summary>
	public string AddItemLinkFormat { get; set; } = "&nbsp;<a class='ModuleEditLink flexi-item-add' href='{0}'><span class='fa fa-plus'></span>&nbsp;{1}</a>";

	public string EditHeaderLinkFormat { get; set; } = "&nbsp;<a class='ModuleEditLink flexi-header-edit' href='{0}'><span class='fa fa-pencil'></span>&nbsp;{1}</a>";

	public string EditFooterLinkFormat { get; set; } = "&nbsp;<a class='ModuleEditLink flexi-footer-edit' href='{0}'><span class='fa fa-pencil'></span>&nbsp;{1}</a>";

	public string ImportLinkFormat { get; set; } = "&nbsp;<a class='ModuleEditLink flexi-import-link' href='{0}'><span class='fa fa-upload'></span>&nbsp;{1}</a>";

	public string ExportLinkFormat { get; set; } = "&nbsp;<a class='ModuleEditLink flexi-export-link' href='{0}'><span class='fa fa-download'></span>&nbsp;{1}</a>";

	public string ItemEditLinkFormat { get; set; } = "<a class='flexi-item-edit' href='{0}'><span class='fa fa-pencil'></span>&nbsp;Edit</a>";

	public string GlobalViewMarkup { get; set; } = string.Empty;

	public string GlobalViewItemMarkup { get; set; } = string.Empty;
}