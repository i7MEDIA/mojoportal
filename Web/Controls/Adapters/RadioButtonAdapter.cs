using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Adapters;

namespace mojoPortal.Web;

public class RadioButtonAdapter : WebControlAdapter
{
	public bool RenderWrapper { get; set; } = false;

	public string WrapperClass { get; set; } = "radio";

	public bool RenderRadioInsideLabel { get; set; } = true;

	private string _uniqueGroupName;
	private string UniqueGroupName
	{
		get
		{
			if (_uniqueGroupName == null)
			{
				if (base.Control is not RadioButton radioButton)
				{
					return string.Empty;
				}

				string text = radioButton.GroupName;
				string uniqueID = radioButton.UniqueID;
				if (uniqueID != null)
				{
					//int num = uniqueID.LastIndexOf(radioButton.IdSeparator);
					//can't use above because .IdSeparator is inaccessible
					//just don't change the ID separator, who does that anyway?
					int num = uniqueID.LastIndexOf("$");
					if (num >= 0)
					{
						if (text.Length > 0)
						{
							text = uniqueID.Substring(0, num + 1) + text;
						}
						else if (radioButton.NamingContainer is RadioButtonList)
						{
							text = uniqueID.Substring(0, num);
						}
					}

					if (text.Length == 0)
					{
						text = uniqueID;
					}
				}

				_uniqueGroupName = text;
			}

			return _uniqueGroupName;
		}
	}

	private string ValueAttribute
	{
		get
		{
			if (base.Control is not RadioButton radioButton)
			{
				return string.Empty;
			}

			return radioButton.Attributes["value"] ?? radioButton.ID ?? radioButton.UniqueID;
		}
	}

	protected override void RenderBeginTag(HtmlTextWriter writer)
	{
		writer.AddAttribute(HtmlTextWriterAttribute.Id, base.Control.ClientID);

		if (RenderWrapper)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, WrapperClass);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.Indent++;
		}

		if (!string.IsNullOrWhiteSpace(base.Control.CssClass))
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, base.Control.CssClass);
		}
	}

	protected override void RenderEndTag(HtmlTextWriter writer)
	{
		if (RenderWrapper)
		{
			writer.Indent--;
			writer.RenderEndTag();
		}
	}

	protected override void RenderContents(HtmlTextWriter writer)
	{
		if (base.Control is not RadioButton radioButton)
		{
			return;
		}

		writer.RenderBeginTag(HtmlTextWriterTag.Label);

		if (RenderRadioInsideLabel)
		{
			if (radioButton.TextAlign == TextAlign.Left)
			{
				writer.Write(radioButton.Text);
				RenderRadioButtonInput(writer, radioButton);
			}
			else
			{
				RenderRadioButtonInput(writer, radioButton);
				writer.Write(radioButton.Text);
			}
		}
		else
		{
			writer.AddAttribute(HtmlTextWriterAttribute.For, radioButton.ClientID);
			writer.Write(radioButton.Text);
		}
		writer.RenderEndTag();

		base.Page?.ClientScript.RegisterForEventValidation(UniqueGroupName, radioButton.ID);

	}

	private void RenderRadioButtonInput(HtmlTextWriter writer, RadioButton button)
	{
		writer.AddAttribute(HtmlTextWriterAttribute.Id, button.ClientID);
		writer.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
		writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueGroupName);
		writer.AddAttribute(HtmlTextWriterAttribute.Value, ValueAttribute);
		if (button.Checked)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
		}

		if (!button.Enabled || !base.IsEnabled)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
		}

		if (button.Enabled && button.AutoPostBack)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Onclick, $"setTimeout('__doPostBack(\\'{button.UniqueID}\\',\\'\\')', 0)");
		}

		writer.RenderBeginTag(HtmlTextWriterTag.Input);
		writer.RenderEndTag();
	}
}