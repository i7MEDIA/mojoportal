using mojoPortal.Business.WebHelpers;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web;

/// <summary>
/// the main purpose of this class is to be used as a base page for non public pages
/// such as administrative or edit pages. It makes it possible for control developers
/// to add logic to hide a control or not render if the current page is a NonCmsPage
/// like if (Page is NonCmsPage) { Visible = false; return; }
/// this page inherits from mojoBasePage so the functionality is the same as inheriting
/// directly from mojoBasePage
/// </summary>
public class NonCmsBasePage : mojoBasePage
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}


	protected void Page_Load(object sender, EventArgs e)
	{
		if (IsPostBack)
		{
			EncodeTextboxControls(this);
		}
	}


	// Ensure that all entered content is properly handled.
	// HTML Encoding for normal fields.
	// HTML Sanitization for textarea fields if the author isn't an Admin, otherwise anything goes.
	private void EncodeTextboxControls(Control parent)
	{
		foreach (Control control in parent.Controls)
		{
			if (control is TextBox textbox)
			{
				if (
					textbox.TextMode == TextBoxMode.MultiLine &&
					!textbox.ID.Contains("RawScript") // Make an allowance for the Custom Script Module's Raw Script textarea
				)
				{
					if (!WebUser.IsAdmin)
					{
						textbox.Text = textbox.Text.SanitizeMarkup();
					}
				}
				else
				{
					textbox.Text = textbox.Text.RemoveMarkup();
				}
			}

			if (control.HasControls())
			{
				EncodeTextboxControls(control);
			}
		}
	}
}
