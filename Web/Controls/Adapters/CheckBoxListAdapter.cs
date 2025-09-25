/*
 * Copied from MS CSSFriendly and modified by mojoPortal Team
 * 
 */

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Adapters;

namespace mojoPortal.Web;

public class CheckBoxListAdapter : WebControlAdapter
{
	public bool RenderWrapper { get; set; } = false;

	public string ListWrapperClass { get; set; } = "checkbox-list";

	public string ItemClass { get; set; } = "checkbox";

	public bool RenderCheckboxInsideLabel { get; set; } = true;

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
			base.Control.Attributes.AddAttributes(writer);
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
		if (base.Control is not CheckBoxList checkBoxList)
		{
			return;
		}

		foreach (ListItem item in checkBoxList.Items)
		{
			AdapterHelpers.GetListItemClientID(checkBoxList, item);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, ItemClass);
			writer.RenderBeginTag(HtmlTextWriterTag.Li);
			if (checkBoxList.TextAlign == TextAlign.Right)
			{
				if (RenderCheckboxInsideLabel)
				{
					RenderCheckBoxListLabel(writer, checkBoxList, item);
				}
				else
				{
					RenderCheckBoxListInput(writer, checkBoxList, item);
					RenderCheckBoxListLabel(writer, checkBoxList, item);
				}
			}
			else
			{
				RenderCheckBoxListLabel(writer, checkBoxList, item);
				RenderCheckBoxListInput(writer, checkBoxList, item);
			}

			writer.RenderEndTag();
			base.Page?.ClientScript.RegisterForEventValidation(checkBoxList.UniqueID, item.Value);
		}

		base.Page?.ClientScript.RegisterForEventValidation(checkBoxList.UniqueID);
	}

	private void RenderCheckBoxListInput(HtmlTextWriter writer, CheckBoxList checkList, ListItem li)
	{
		writer.AddAttribute(HtmlTextWriterAttribute.Id, AdapterHelpers.GetListItemClientID(checkList, li));
		writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
		writer.AddAttribute(HtmlTextWriterAttribute.Name, AdapterHelpers.GetListItemUniqueID(checkList, li));
		writer.AddAttribute(HtmlTextWriterAttribute.Value, li.Value);

		if (li.Selected)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
		}

		if (!li.Enabled || !checkList.Enabled)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
		}

		if (li.Enabled && checkList.Enabled && checkList.AutoPostBack)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Onclick, $"setTimeout('__doPostBack(\\'{AdapterHelpers.GetListItemUniqueID(checkList, li)}\\',\\'\\')', 0)");
		}

		writer.RenderBeginTag(HtmlTextWriterTag.Input);
		writer.RenderEndTag();
	}

	private void RenderCheckBoxListLabel(HtmlTextWriter writer, CheckBoxList checkList, ListItem li)
	{
		writer.AddAttribute("for", AdapterHelpers.GetListItemClientID(checkList, li));
		li.Attributes.AddAttributes(writer);
		writer.RenderBeginTag(HtmlTextWriterTag.Label);

		if (RenderCheckboxInsideLabel)
		{
			if (checkList.TextAlign == TextAlign.Left)
			{
				writer.Write(li.Text);
				RenderCheckBoxListInput(writer, checkList, li);
			}
			else
			{
				RenderCheckBoxListInput(writer, checkList, li);
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
