using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

	public Dictionary<string, object> Attributes { get; set; }

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

		Attributes ??= new Dictionary<string, object>();

		Attributes.Add("tabindex", TabIndex);

		if (!string.IsNullOrWhiteSpace(CssClass))
		{
			Attributes.Add("class", CssClass);
		}

		if (Required)
		{
			Attributes.Add("required", Required);
		}

		var model = new DropDownlistModel
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
public class DropDownlistModel
{
	public string ID;
	public List<SelectListItem> Items;
	public Dictionary<string, object> Attributes { get; set; }
	public string DefaultOption { get; set; } = string.Empty;
}