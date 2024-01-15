using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Editor;

public class TextAreaEditorControl : Control, IPostBackDataHandler
{
	public TextAreaEditorControl() { }

	public string Text
	{
		get { object o = ViewState["Text"]; return (o == null ? string.Empty : (string)o); }
		set { ViewState["Text"] = value; }
	}

	public Unit Width
	{
		get { object o = ViewState["Width"]; return (o == null ? Unit.Percentage(100) : (Unit)o); }
		set { ViewState["Width"] = value; }
	}

	public Unit Height
	{
		get { object o = ViewState["Height"]; return (o == null ? Unit.Pixel(200) : (Unit)o); }
		set { ViewState["Height"] = value; }
	}

	protected override void Render(HtmlTextWriter writer)
	{
		writer.Write(
			$"<textarea class=\"markituphtml\" id=\"{ClientID}\" name=\"{UniqueID}\" rows=\"20\" cols=\"80\">{System.Web.HttpUtility.HtmlEncode(Text)}</textarea>");
	}

	#region Postback Handling

	public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
	{
		if (postCollection[postDataKey] is not null
			&& postCollection[postDataKey] != Text)
		{
			Text = postCollection[postDataKey];
			return true;
		}

		return false;
	}

	public void RaisePostDataChangedEvent()
	{
		// Do nothing
	}

	#endregion
}