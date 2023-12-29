using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI;

public partial class AllowedRolesSetting : UserControl, ISettingControl
{
	private string selectedRoles = string.Empty;
	private SiteSettings siteSettings = null;

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		if (HttpContext.Current == null) { return; }
		Load += new EventHandler(Page_Load);
		EnsureItems();
	}

	protected void Page_Load(object sender, EventArgs e)
	{
	}

	private void EnsureItems()
	{
		siteSettings ??= CacheHelper.GetCurrentSiteSettings();
		
		//why is this null here, its declared
		if (chkAllowedRoles == null)
		{
			chkAllowedRoles = new CheckBoxList();
			if (Controls.Count == 0) { Controls.Add(chkAllowedRoles); }
		}

		if (chkAllowedRoles.Items.Count > 0) { return; }

		chkAllowedRoles.Items.Add(new ListItem
		{
			Text = Resource.RolesAllUsersRole,
			Value = "All Users"
		});

		using IDataReader reader = Role.GetSiteRoles(siteSettings.SiteId);
		while (reader.Read())
		{
			chkAllowedRoles.Items.Add(new ListItem
			{
				Text = reader["DisplayName"].ToString(),
				Value = reader["RoleName"].ToString()
			});
		}
	}

	private void GetSelectedItems()
	{
		selectedRoles = string.Empty;
		foreach (ListItem item in chkAllowedRoles.Items)
		{
			if (item.Selected)
			{
				selectedRoles = $"{selectedRoles}{item.Value};";
			}
		}

	}

	private void BindSelection()
	{
		foreach (ListItem item in chkAllowedRoles.Items)
		{
			if (selectedRoles.LastIndexOf($"{item.Value};") > -1)
			{
				item.Selected = true;
			}
		}
	}

	#region ISettingControl

	public string GetValue()
	{
		EnsureItems();
		GetSelectedItems();
		return selectedRoles;
	}

	public void SetValue(string val)
	{
		EnsureItems();
		selectedRoles = val;
		BindSelection();
	}

	#endregion
}