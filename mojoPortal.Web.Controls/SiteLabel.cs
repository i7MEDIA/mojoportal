using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

		writer.Write(GetControlMarkup());
	}

	private string GetControlMarkup()
	{
		if (string.IsNullOrEmpty(ConfigKey)) return string.Empty;

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

		string forString = string.Empty;
		if (ForControl.Length > 0)
		{
			if (NamingContainer.FindControl(ForControl) is Control c)
			{
				forString = $" for='{c.ClientID}' ";
			}

		}

		var attribs = new List<string>();

		foreach (string key in Attributes.Keys)
		{
			attribs.Add($"{key}=\"{Attributes[key]}\"");
		}

		var attribsString = string.Join(" ", attribs);
		if (!string.IsNullOrWhiteSpace(attribsString))
		{
			attribsString = $" {attribsString}";
		}

		var cssString = string.Empty;
		if (!string.IsNullOrWhiteSpace(CssClass))
		{
			cssString = $" class=\"{CssClass}\"";
		}
		return UseLabelTag
			? $"<label {forString}{cssString}{attribsString}>{text}</label>"
			: string.IsNullOrWhiteSpace(CssClass) ? text : $"<span{cssString}{attribsString}>{text}</span>";
	}
}