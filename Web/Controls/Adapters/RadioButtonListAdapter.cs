/*
 * Copied from MS CSSFriendly and modified by mojoPortal Team
 * 
 */

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Adapters;

namespace mojoPortal.Web;

public class RadioButtonListAdapter : WebControlAdapter
{
	public bool RenderWrapper { get; set; } = false;

	public string ListWrapperClass { get; set; } = "checkbox-list";

	public string ItemClass { get; set; } = "radio";

	public bool RenderRadioInsideLabel { get; set; } = true;

	protected override void RenderBeginTag(HtmlTextWriter writer)
	{
		writer.AddAttribute(HtmlTextWriterAttribute.Id, base.Control.ClientID);

		if (RenderWrapper)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, ListWrapperClass);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.Indent++;
		}
		if (!string.IsNullOrWhiteSpace(base.Control.CssClass))
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, base.Control.CssClass);
		}

		writer.RenderBeginTag(HtmlTextWriterTag.Ul);
	}

	protected override void RenderEndTag(HtmlTextWriter writer)
	{
		writer.RenderEndTag();

		if (RenderWrapper)
		{
			writer.Indent--;
			writer.RenderEndTag();
		}
	}

	protected override void RenderContents(HtmlTextWriter writer)
	{
		if (base.Control is not RadioButtonList radioButtonList)
		{
			return;
		}

		foreach (ListItem item in radioButtonList.Items)
		{
			AdapterHelpers.GetListItemClientID(radioButtonList, item);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, ItemClass);
			writer.RenderBeginTag(HtmlTextWriterTag.Li);
			if (radioButtonList.TextAlign == TextAlign.Right)
			{
				if (RenderRadioInsideLabel)
				{
					RenderRadioButtonListLabel(writer, radioButtonList, item);
				}
				else
				{
					RenderRadioButtonListInput(writer, radioButtonList, item);
					RenderRadioButtonListLabel(writer, radioButtonList, item);
				}
			}
			else
			{
				RenderRadioButtonListLabel(writer, radioButtonList, item);
				RenderRadioButtonListInput(writer, radioButtonList, item);
			}

			writer.RenderEndTag();
			base.Page?.ClientScript.RegisterForEventValidation(radioButtonList.UniqueID, item.Value);
		}

		base.Page?.ClientScript.RegisterForEventValidation(radioButtonList.UniqueID);
	}

	private void RenderRadioButtonListInput(HtmlTextWriter writer, RadioButtonList buttonList, ListItem li)
	{
		writer.AddAttribute(HtmlTextWriterAttribute.Id, AdapterHelpers.GetListItemClientID(buttonList, li));
		writer.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
		writer.AddAttribute(HtmlTextWriterAttribute.Name, buttonList.UniqueID);
		writer.AddAttribute(HtmlTextWriterAttribute.Value, li.Value);
		if (li.Selected)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
		}

		if (!li.Enabled || !buttonList.Enabled || !this.IsEnabled)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
		}

		if (li.Enabled && buttonList.Enabled && buttonList.AutoPostBack)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Onclick, $"setTimeout('__doPostBack(\\'{AdapterHelpers.GetListItemUniqueID(buttonList, li)}\\',\\'\\')', 0)");
		}

		writer.RenderBeginTag(HtmlTextWriterTag.Input);
		writer.RenderEndTag();
	}

	private void RenderRadioButtonListLabel(HtmlTextWriter writer, RadioButtonList buttonList, ListItem li)
	{
		writer.AddAttribute("for", AdapterHelpers.GetListItemClientID(buttonList, li));
		writer.RenderBeginTag(HtmlTextWriterTag.Label);

		if (RenderRadioInsideLabel)
		{
			if (buttonList.TextAlign == TextAlign.Left)
			{
				writer.Write(li.Text);
				RenderRadioButtonListInput(writer, buttonList, li);
			}
			else
			{
				RenderRadioButtonListInput(writer, buttonList, li);
				writer.Write(li.Text);
			}
		}
		else
		{
			writer.Write(li.Text);
		}
		writer.RenderEndTag();
	}
}
