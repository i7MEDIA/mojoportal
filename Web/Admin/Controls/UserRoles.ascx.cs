using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Data;
using System.Globalization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.AdminUI;

public partial class UserRoles : UserControl
{
	private SiteSettings _siteSettings = null;
	private bool _useSeparatePagesForRoles = false;

	protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;

	public string SiteRoot { get; set; } = string.Empty;
	public int UserId { get; set; } = -1;


	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		PopulateLabels();

		if (_useSeparatePagesForRoles)
		{
			divRoles.Visible = false;
			lnkRolesDialog.Visible = true;

			SetupDialogScript();
		}
		else
		{
			SetupScripts();
		}

		if (!IsPostBack)
		{
			BindRoles();
		}
	}


	private void BindRoles()
	{
		if (UserId == -1 || _siteSettings is null)
		{
			return;
		}

		using (IDataReader reader = SiteUser.GetRolesByUser(_siteSettings.SiteId, UserId))
		{
			userRoles.DataSource = reader;
			userRoles.DataBind();
		}

		if (!_useSeparatePagesForRoles)
		{
			using IDataReader reader = Role.GetRolesUserIsNotIn(_siteSettings.SiteId, UserId);

			if (WebUser.IsAdmin)
			{
				allRoles.DataSource = reader;
				allRoles.DataBind();
			}
			else
			{
				while (reader.Read())
				{
					string roleName = reader["RoleName"].ToString();

					// only admins can add users to Admins role or Role Admins role
					if (
						roleName != "Admins" &&
						roleName != "Role Admins" &&
						(
							roleName != "Content Administrators" ||
							WebConfigSettings.AllowRoleAdminsToCreateContentManagers
						)
					)
					{
						var item = new ListItem(
							reader["DisplayName"].ToString(),
							reader["RoleID"].ToString()
						);

						allRoles.Items.Add(item);
					}
				}
			}

			if (allRoles.Items.Count == 0)
			{
				allRoles.Enabled = false;
				addExisting.Enabled = false;
				addExisting.Text = Resource.ManageUsersUserIsInAllRolesMessage;
			}
		}
	}


	private void AddRole_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(hdnUserPassword.Value))
		{
			lblPasswordError.Text = Resource.SecurityPasswordRequired;
			lblPasswordError.Visible = true;

			return;
		}

		if (UserId > -1 && _siteSettings != null)
		{
			var user = new SiteUser(_siteSettings, UserId);
			var membershipProvider = Membership.Provider as mojoMembershipProvider;
			var isValidRequest = membershipProvider.ValidateUser(Page.User.Identity.Name, hdnUserPassword.Value);

			if (!isValidRequest)
			{
				hdnUserPassword.Value = string.Empty;
				lblPasswordError.Text = Resource.SecurityPasswordIncorrect;
				lblPasswordError.Visible = true;

				return;
			}

			var roleID = int.Parse(allRoles.SelectedItem.Value, CultureInfo.InvariantCulture);
			var role = new Role(roleID);

			Role.AddUser(roleID, UserId, role.RoleGuid, user.UserGuid);

			user.RolesChanged = true;

			user.Save();

			BindRoles();

			upRoles.Update();
		}

		hdnUserPassword.Value = string.Empty;
		lblPasswordError.Visible = false;
	}


	private void UserRoles_ItemCommand(object sender, DataListCommandEventArgs e)
	{
		var roleID = Convert.ToInt32(userRoles.DataKeys[e.Item.ItemIndex]);
		var user = new SiteUser(_siteSettings, UserId);

		Role.RemoveUser(roleID, UserId);

		userRoles.EditItemIndex = -1;

		if (user.UserId > -1)
		{
			user.RolesChanged = true;
			user.Save();
		}

		BindRoles();

		upRoles.Update();
	}


	void UserRoles_ItemDataBound(object sender, DataListItemEventArgs e)
	{
		ImageButton btnRemoveRole = e.Item.FindControl("btnRemoveRole") as ImageButton;
		UIHelper.AddConfirmationDialog(btnRemoveRole, Resource.ManageUsersRemoveRoleWarning);
	}

	void BtnRefreshRoles_Click(object sender, ImageClickEventArgs e)
	{
		BindRoles();
		upRoles.Update();
	}


	protected bool CanDeleteUserFromRole(string roleName)
	{
		if (WebUser.IsAdmin) { return true; }

		if (roleName is "Admins" or "Role Admins") { return false; }

		return true;
	}


	private void PopulateLabels()
	{
		addExisting.Text = Resource.ManageUsersAddToRoleButton;
		addExisting.ToolTip = Resource.ManageUsersAddToRoleButton;

		SiteUtils.SetButtonAccessKey(addExisting, AccessKeys.ManageUsersAddToRoleButtonAccessKey);

		lnkRolesDialog.Text = Resource.ManageUserRoles;
		lnkRolesDialog.ToolTip = Resource.ManageUserRoles;
		lnkRolesDialog.NavigateUrl = "~/Dialog/UserRolesDialog.aspx"
			.ToLinkBuilder()
			.AddParam("u", UserId)
			.ToString();
		btnRefreshRoles.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/1x1.gif");
		btnRefreshRoles.Attributes.Add("tabIndex", "-1");
	}


	private void SetupDialogScript()
	{
		var script = $$"""
			<script>
				function RefreshRoles(){
					document.getElementById('{{btnRefreshRoles.ClientID}}').click();
				}
			</script>
			""";

		ScriptManager.RegisterClientScriptBlock(
			this,
			typeof(Page),
			"refreshroles",
			script,
			false);
	}


	private void LoadSettings()
	{
		_siteSettings = CacheHelper.GetCurrentSiteSettings();
		_useSeparatePagesForRoles = (Role.CountOfRoles(_siteSettings.SiteId) >= WebConfigSettings.TooManyRolesForModuleSettings);
	}


	private void SetupScripts()
	{
		var mojoPromptScriptName = "mojoDialog";

		if (!Page.ClientScript.IsStartupScriptRegistered(mojoPromptScriptName))
		{
			ScriptManager.RegisterStartupScript(
				Page,
				typeof(Page),
				mojoPromptScriptName,
				$"<script src=\"{ResolveUrl($"~/ClientScript/mojo-prompt.js?v={_siteSettings.SkinVersion}")}\"></script>",
				false);

			var script = $$"""
				<script>
					const enterPasswordBtn = document.getElementById('enterPasswordBtn');

					enterPasswordBtn.addEventListener('keydown', (e) => {
						if (e.key === 'Enter') {
							e.preventDefault();
							e.target.click();
						}
					});

					function EnterPassword() {
						mojoPrompt(
							'{{Resource.AddRolesToUserApprovalPromptMessage}}',
							(userPassword) => addRole(userPassword),
							'{{Resource.AddRoleToUseApprovalPromptTitle}}',
							'password'
						);
				
						function addRole(password) {
							if (password === null) {
								return;
							}
				
							document.getElementById('{{hdnUserPassword.ClientID}}').value = password;
							document.getElementById('{{addExisting.ClientID}}').click();
						}
					}
				</script>
				""";

			ScriptManager.RegisterStartupScript(
				Page,
				typeof(Page),
				"EnterPasswordForRoleChange",
				script,
				false);
		}
	}


	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		addExisting.Click += new EventHandler(AddRole_Click);
		btnRefreshRoles.Click += new ImageClickEventHandler(BtnRefreshRoles_Click);
		userRoles.ItemDataBound += new DataListItemEventHandler(UserRoles_ItemDataBound);
		userRoles.ItemCommand += new DataListCommandEventHandler(UserRoles_ItemCommand);
	}
}
