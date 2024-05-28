using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI;

/// <summary>
/// You can modify these settings from the /config/config.json file of your skin
/// See the framework skin for an example skin using this
/// </summary>
public class LayoutDisplaySettings : WebControl
{

	//public override string FeatureName => "Core";
	//private Type type => GetType();
	//public override string SubFeatureName => GetType().Name.Replace("DisplaySettings", string.Empty);

	//public LayoutDisplaySettings() : base() { }

	/// <summary>
	/// css class(es) assigned to divLeft when only left and center are used
	/// replaces default classes on divLeft when only left and center are used
	/// </summary>
	public string LeftSideNoRightSideCss { get; set; } = "left-center";

	/// <summary>
	/// css class(es) assigned to divRight when only right and center are used
	/// replaces default classes on divRight when only right and center are used
	/// </summary>
	public string RightSideNoLeftSideCss { get; set; } = "right-center";

	/// <summary>
	/// css class(es) assigned to divRight and divLeft when both have content
	/// and center has no content if this property is populated from theme.skin
	/// in order to not change the behavior in older skins if this proiperty is empty it is not applied
	/// added 2013-01-06
	/// </summary>
	public string LeftAndRightNoCenterCss { get; set; } = string.Empty;

	/// <summary>
	/// css class(es) assigned to  divLeft when it is the only panel that has content
	/// generally users should just use the center column, there is no good reason to choose a side column if its going
	/// to use the full width, but this property allows you to add css if you want to handle that
	/// in order to not change the behavior in older skins if this proiperty is empty it is not applied
	/// added 2013-01-06
	/// </summary>
	public string LeftOnlyCss { get; set; } = string.Empty;

	/// <summary>
	/// css class(es) assigned to  divRight when it is the only panel that has content
	/// generally users should just use the center column, there is no good reason to choose a side column if its going
	/// to use the full width, but this property allows you to add css if you want to handle that
	/// in order to not change the behavior in older skins if this proiperty is empty it is not applied
	/// added 2013-01-06
	/// </summary>
	public string RightOnlyCss { get; set; } = string.Empty;

	/// <summary>
	/// css class(es) assigned to divCenter when left side has no content and is not used
	/// </summary>
	public string CenterNoLeftSideCss { get; set; } = "col-md-9 center-right";

	/// <summary>
	/// css class(es) assigned to divCenter when right side has no content and is not used
	/// </summary>
	public string CenterNoRightSideCss { get; set; } = "col-md-9 center-left";

	/// <summary>
	/// css class(es) assigned to divCenter when left side and right side has no content and is not used
	/// </summary>
	public string CenterNoLeftOrRightSideCss { get; set; } = "col-md-12 nomargins";

	/// <summary>
	/// css class(es) assigned to divCenter when both left side and right side are used
	/// </summary>
	public string CenterWithLeftAndRightSideCss { get; set; } = "col-md-6 center-left-right";

	/// <summary>
	/// css class(es) assigned to divCenter if the setting is not empty and the center pane has no content but one or both side panels have content
	/// </summary>
	public string EmptyCenterCss { get; set; } = string.Empty;

	/// <summary>
	/// if true and the side columns have content but the center does not this will prevent rendering the div for the center
	/// </summary>
	public bool HideEmptyCenterIfOnlySidesHaveContent { get; set; } = false;


	protected override void Render(HtmlTextWriter writer)
	{
		// nothing to render
	}
}