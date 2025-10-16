using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.AdminUI;

public partial class StyleSetsControl : UserControl
{
	private List<string> _selectedStyleSetNames = [];
	public List<StyleSet> StyleSets { get; set; } = [];
	public List<string> SelectedStyleSetNames
	{
		get
		{
			var selected = new List<string>();
			foreach (ListItem item in lbStyleSets.Items)
			{
				if (item.Selected) selected.Add(item.Value);
			}
			return selected;
		}
		set
		{
			if (value == null) return;
			_selectedStyleSetNames = value;
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);

		if (!Page.IsPostBack)
		{
			lbStyleSets.Items.Clear();
			foreach (var styleSet in StyleSets)
			{
				var listItem = new ListItem(styleSet.Name, styleSet.Name);
				listItem.Attributes.Add("title", styleSet.Description);
				if (_selectedStyleSetNames.Contains(styleSet.Name))
				{
					listItem.Selected = true;
				}
				lbStyleSets.Items.Add(listItem);
			}

			SetupStyleSetScript();
		}


	}

	public void SetupStyleSetScript()
	{
		var cacheGuidQueryParam = "?sv=" + CacheHelper.GetSkinCacheGuid();
		var script = $$"""
		<script type="module" data-loader="{{nameof(StyleSetsControl)}}">
		import { mount, MultiSelect } from '{{VirtualPathUtility.ToAbsolute("~/ClientScript/MultiSelect.min.js") + cacheGuidQueryParam}}';

		//const output = document.querySelector('#output');
		//const options = Array.from(Array(10), (_, i) => `Option #${i + 1}`);

		mount(MultiSelect, {
			target: document.getElementById('{{multiSelect.ClientID}}'), // where you want the component to render
			props: {
				// the options you want to choose from
				//options: options,
				// The select element to populate values from (this will overwrite the "options" value)
				selectElement: '#{{lbStyleSets.ClientID}}',
				// the CSS classes on the input in the control
				inputCssClass: 'form-control',
				// the CSS classes on the selected option in the control
				selectedOptionCssClass: 'badge text-bg-secondary',
				// the placeholder text when the control has no items selected
				placeholder: '{{Resources.Resource.SelectButton}}',
				// The callback when the value of the control changes.
				// "e" is a string array.
				//oninput: (e) => {
				//	output.textContent += '\n' + JSON.stringify(e);
				//}
			}
		});
		</script>
		""";

		Page.ClientScript.RegisterStartupScript(typeof(Page), "SelectStyleSetHandler", script);
		Page.Header.Controls.Add(new Literal() { Text = $"""<link href="{VirtualPathUtility.ToAbsolute("~/Content/MultiSelect.min.css") + cacheGuidQueryParam}" rel="stylesheet" />""" });
	}
}