using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls;

public class CornerRounderBottom : WebControl
{
	public bool DoRounding { get; set; } = false;
	public string RoundingMarkup { get; set; } = "";
}

public class CornerRounderTop : WebControl
{
	public bool DoRounding { get; set; } = false;
	public string RoundingMarkup { get; set; } = "";
}