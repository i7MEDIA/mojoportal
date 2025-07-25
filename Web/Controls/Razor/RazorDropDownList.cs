using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using mojoPortal.Web.Components;

namespace mojoPortal.Web.UI.Razor;

[SupportsEventValidation]
[ValidationProperty("SelectedValue")]
public class RazorDropDownList : Control, IPostBackDataHandler
{
	public List<SelectListItem> Items;

	public int TabIndex { get; set; } = 10;

	public string CssClass { get; set; } = string.Empty;

	public Dictionary<string, object> Attributes { get; set; } = [];

	//public AttributeCollection Attributes { get; set; }

	public string DefaultOption { get; set; }
	public bool Required { get; set; }
	public string SelectedValue
	{
		get
		{
			return (string)ViewState["SelectedValue"];
		}
		set
		{
			ViewState["SelectedValue"] = value;
		}
	}


	public virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
	{

		string presentValue = SelectedValue;
		string postedValue = postCollection[postDataKey];

		if (presentValue == null || !presentValue.Equals(postedValue))
		{
			SelectedValue = postedValue;
			return true;
		}

		return false;
	}

	public virtual void RaisePostDataChangedEvent()
	{
		//OnSelectionChanged(EventArgs.Empty);
	}

	//protected virtual void OnSelectionChanged(EventArgs e)
	//{
	//	SelectionChanged?.Invoke(this, e);
	//}

	//public event EventHandler SelectionChanged;

	protected override void OnPreRender(EventArgs e)
	{
		Page.RegisterRequiresPostBack(this);
		base.OnPreRender(e);
	}

	protected override void Render(HtmlTextWriter writer)
	{
		base.Render(writer);

		Attributes ??= [];

		Attributes.Add("tabindex", TabIndex);

		if (!string.IsNullOrWhiteSpace(CssClass))
		{
			var hasClass = Attributes.ContainsKey("class");

			if (!hasClass)
			{
				Attributes.Add("class", CssClass);
			}
			else
			{
				var classValue = Attributes["class"].ToString().Split([' '], StringSplitOptions.RemoveEmptyEntries).ToList();

				if (!classValue.Contains(CssClass))
				{
					Attributes["class"] = $"{Attributes["class"]} {CssClass}";
				}
			}
		}

		if (Required)
		{
			Attributes.Add("required", Required);
		}

		var model = new DropDownListModel
		{
			ID = UniqueID,
			Items = Items,
			Attributes = Attributes,
			DefaultOption = DefaultOption
		};
		var content = RazorBridge.RenderPartialToString("DropDownList", model, "Controls");

		writer.Write(content);
	}
}


public class DropDownListModel
{
	public string ID { get; set; }
	public List<SelectListItem> Items { get; set; }
	public Dictionary<string, object> Attributes { get; set; }
	public string DefaultOption { get; set; } = string.Empty;
}
