namespace mojoPortal.Web.AdminUI;


public class AdminDisplaySettings : BaseDisplaySettings
{
	public override string FeatureName => "Core";
	public override string SubFeatureName => GetType().Name.Replace("DisplaySettings", string.Empty);
	public AdminDisplaySettings() : base() { }

	public string PanelHeadingMarkup { get; set; } = "<h3>{0} <small>{1}</small></h3>";
	public string PanelBottomMarkup { get; set; }
	public string PanelHeadingMarkupCollapsible { get; set; } = "<a href='#{2}'><h3>{0} <small>{1}</small></h3></a>";
	public string SubPanelHeadingMarkup { get; set; } = "<h4>{0} <small>{1}</small></h4>";
	public string SubPanelBottomMarkup { get; set; }
	public string SubPanelHeadingMarkupCollapsible { get; set; } = "<a href='#{2}'><h4>{0} <small>{1}</small></h4></a>";
	public string SubPanelBottomMarkupCollapsible { get; set; } = "";
	public string PanelModalMarkupTop { get; set; } =
		@"<div class='modal fade' tabindex='-1' role='dialog'>
				<div class='modal-dialog' role='document'>
					<div class='modal-content'>
						<div class='modal-header'>
							<button type='button' class='close' data-dismiss='modal' aria-label='Close'><span aria-hidden='true'>&times;</span></button>
							<h4 class='modal-title'>{0}</h4>
						</div>
						<div class='modal-body'>{1}</div>
						<div class='modal-footer'>
							<button type='button' class='btn btn-default' data-dismiss='modal'>Close</button>
							<button type='button' class='btn btn-primary'>Save changes</button>
						</div>
					</div>
				</div>
			</div>";
	public string SecurityProtocolCheckResponseMarkup { get; set; } = " <div>{0}</div>";
	public string ModuleSettingsSettingPanelElement { get; set; } = "div";
	public string ModuleSettingsSettingLabelMarkup { get; set; } = "<label class=\"{0}\" for=\"{1}\">{2}</label>";
	public string ModuleSettingsSettingPanelClass { get; set; } = "settingrow";
	public string ModuleSettingsSettingLabelClass { get; set; } = "settinglabel";
	public string ModuleSettingsSettingControlClass { get; set; } = "forminput";
	public string RestartButtonClass { get; set; } = "btn btn-danger btn-sm ";
	public string UpdateAvailableLinkMarkup { get; set; } = "<a href=\"{0}\" target=\"_blank\">{1}</a>";
	public string HelpBlockMarkup { get; set; } = "<span class='help-block'>{0}</span>";
}