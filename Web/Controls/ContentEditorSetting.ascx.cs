using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI;

public partial class ContentEditorSetting : mojoUserControl, ISettingControl
{
	protected void Page_Load(object sender, EventArgs e)
	{
		SecurityHelper.DisableBrowserCache();
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		if (HttpContext.Current == null) { return; }
		EnsureItems();
	}

	private void EnsureItems()
	{
		if (ddEditorProviders.Items.Count > 0)
		{
			return;
		}

		ddEditorProviders.DataSource = EditorManager.Providers;
		ddEditorProviders.DataBind();

		foreach (ListItem providerItem in ddEditorProviders.Items)
		{
			providerItem.Text = providerItem.Text.Replace("Provider", string.Empty);
		}

		ddEditorProviders.Items.Insert(0, new ListItem { Text = Resources.Resource.SiteDefaultEditor, Value = "" });
	}

	#region ISettingControl

	public string GetValue()
	{
		EnsureItems();
		return ddEditorProviders.SelectedValue;
	}

	public void SetValue(string val)
	{
		EnsureItems();
		var item = ddEditorProviders.Items.FindByValue(val);
		if (item != null)
		{
			ddEditorProviders.ClearSelection();
			item.Selected = true;
		}
	}

	#endregion
}