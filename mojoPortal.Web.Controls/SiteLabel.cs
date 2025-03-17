using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CsvHelper;

namespace mojoPortal.Web.Controls;

[ParseChildren(false)]
[PersistChildren(true)]
public class SiteLabel : WebControl, INamingContainer
{
	public string ResourceFile { get; set; } = "Resource";
	public string ConfigKey { get; set; } = string.Empty;
	public bool UseLabelTag { get; set; } = true;
	public string ForControl { get; set; } = string.Empty;
	public bool ShowWarningOnMissingKey { get; set; } = true;
	/// <summary>
	/// Sets text alignment in relation to any child controls. TextAlign.Left would put the text before child controls. TextAlign.Right puts the text after child controls.
	/// </summary>
	public TextAlign TextAlign { get; set; } = TextAlign.Left;

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		EnableViewState = false;
	}

	protected override void Render(HtmlTextWriter writer)
	{
		if (HttpContext.Current == null)
		{
			writer.Write("[" + ID + "]");
			return;
		}
		if (string.IsNullOrWhiteSpace(ConfigKey))
		{
			return;
		}

		RenderLabel(writer);
	}

	private void RenderLabel(HtmlTextWriter writer)
	{
		string text;

		if (ConfigKey == "EmptyLabel" || ConfigKey == "spacer")
		{
			text = string.Empty;
		}
		else
		{
			text = HttpContext.GetGlobalResourceObject(ResourceFile, ConfigKey) as string;
			text ??= ShowWarningOnMissingKey 
				? string.Format("{0} not found in {1}.resx file", ConfigKey, ResourceFile) 
				: ConfigKey;
		}

		//string forString = string.Empty;
		if (ForControl.Length > 0)
		{
			if (NamingContainer.FindControl(ForControl) is Control c)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.For, c.ClientID);
				//forString = $" for='{c.ClientID}' ";
			}

		}

		//var attribs = new List<string>();

		foreach (string key in Attributes.Keys)
		{
			writer.AddAttribute(key, Attributes[key]); 
			//attribs.Add($"{key}=\"{Attributes[key]}\"");
		}

		//var attribsString = string.Join(" ", attribs);
		//if (!string.IsNullOrWhiteSpace(attribsString))
		//{
		//	attribsString = $" {attribsString}";
		//}

		var cssString = string.Empty;
		if (!string.IsNullOrWhiteSpace(CssClass))
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
			//cssString = $" class=\"{CssClass}\"";
		}

		if (UseLabelTag)
		{
			writer.RenderBeginTag(HtmlTextWriterTag.Label);
			//return $"<label {forString}{cssString}{attribsString}>{text}</label>";
		}
		else
		{
			writer.RenderBeginTag(HtmlTextWriterTag.Span);
			//return string.IsNullOrWhiteSpace(CssClass) ? text : $"<span{cssString}{attribsString}>{text}</span>";
		}
		
		if (TextAlign == TextAlign.Left)
		{
			writer.Write(text);
			foreach (Control i in Controls)
			{
				i.RenderControl(writer);
			}
		}
		else //TextAlign.Right
		{
			foreach (Control i in Controls)
			{
				i.RenderControl(writer);
			}
			writer.Write(text);
		}

		writer.RenderEndTag();
	}
}