using mojoPortal.Core.Models;
using Resources;
namespace SuperFlexiUI;

public class SuperDuperNewConfigStuff
{
	public string MarkupDefinitionFile { get; set; } = string.Empty;

	public bool IsGlobalView { get; set; }

	public int GlobalViewSortOrder { get; set; } = 0;

	public string ModuleFriendlyName { get; set; } = string.Empty;

	public string InstanceFeaturedImage { get; set; } = string.Empty;

	public bool DescendingSortOrder { get; set; }

	public bool ExtraCssClass { get; set; }

	public int MaxItems { get; set; } = -1;

	public string CustomizableSettings { get; set; } = string.Empty;

	public bool IncludeInSearch { get; set; } = true;

	public int PageSize { get; set; } = 0;

	#region Hidden Settings
	public string MarkupDefinitionContent { get; set; } = string.Empty;
	public string HeaderContent { get; set; } = string.Empty;
	public string FooterContent { get; set; } = string.Empty;

	#endregion
}

public class SuperDuperNewConfigStuffDisplay
{
	public SettingFieldGroup DefinitionSettings => new()
	{
		Label = SuperFlexiResources.DefinitionSettings,
		Fields = [
			new SettingField<string>
			{
				Name = nameof(SuperDuperNewConfigStuff.MarkupDefinitionFile),
				Label = SuperFlexiResources.MarkupDefinitionFile,
				Options = SuperFlexiHelpers.GetSolutionPaths(),
				HelpKey = "sflexi-MarkupDefinitionSetting-help"
			},

			new SettingField<bool>
			{
				Name = nameof(SuperDuperNewConfigStuff.IsGlobalView),
				Label = SuperFlexiResources.IsGlobalView,
				HelpKey = "sflexi-IsGlobalView-help"
			},

			new SettingField<int>
			{
				Name = nameof(SuperDuperNewConfigStuff.GlobalViewSortOrder),
				Label = SuperFlexiResources.GlobalViewSortOrder,
				HelpKey = "sflexi-GlobalViewSortOrder-help"
			},

			new SettingField<string>
			{
				Name = nameof(SuperDuperNewConfigStuff.ModuleFriendlyName),
				Label = SuperFlexiResources.ModuleFriendlyName,
				HelpKey = "sflexi-ModuleFriendlyName-help"
			},

			new SettingField<string>
			{
				Name = nameof(SuperDuperNewConfigStuff.InstanceFeaturedImage),
				Label = SuperFlexiResources.InstanceFeaturedImage,
				HelpKey = "sflexi-InstanceFeaturedImageSetting-help",
				FieldType = "image"
			},

			new SettingField<bool>
			{
				Name = nameof(SuperDuperNewConfigStuff.DescendingSortOrder),
				Label = SuperFlexiResources.DescendingSortOrder,
				HelpKey = "sflexi-DescendingSortOrder-help"
			}
		]
	};

	public SettingFieldGroup AdvancedSettings => new()
	{
		Label = SuperFlexiResources.AdvancedSettings,
		Fields = [
			new SettingField<string>
			{
				Name = nameof(SuperDuperNewConfigStuff.ExtraCssClass),
				Label = SuperFlexiResources.ExtraCssClassSetting,
				HelpKey = "sflexi-ExtraCssClassSetting-help",
				FieldType = "CssClass"
			},

			new SettingField<int>
			{
				Name = nameof(SuperDuperNewConfigStuff.MaxItems),
				Label = SuperFlexiResources.MaxItems,
				HelpKey = "sflexi-MaxItems-help"
			},

			new SettingField<string>
			{
				Name = nameof(SuperDuperNewConfigStuff.CustomizableSettings),
				Label = SuperFlexiResources.CustomizableSettings,
				HelpKey = "sflexi-CustomizableSettings-help"
			},

			new SettingField<bool>
			{
				Name = nameof(SuperDuperNewConfigStuff.IncludeInSearch),
				Label = SuperFlexiResources.IncludeInSearch,
				HelpKey = "sflexi-IncludeInSearch-help"
			},

			new SettingField<int>
			{
				Name = nameof(SuperDuperNewConfigStuff.PageSize),
				Label = SuperFlexiResources.PageSize,
				HelpKey = "sflexi-PageSize-help"
			}
		]
	};

}