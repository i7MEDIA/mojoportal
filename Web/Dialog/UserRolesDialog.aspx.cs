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

namespace mojoPortal.Web.UI;

public partial class UserRolesDialog : mojoDialogBasePage
{
	private int _userId = -1;
	private SiteUser _siteUser = null;
	private bool _isAdmin = false;

	protected string DeleteLinkImage = $"~/Data/SiteImages/{WebConfigSettings.DeleteLinkImage}";


	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();

		if ((WebUser.IsInRoles(SiteInfo.RolesThatCanManageUsers) || WebUser.IsInRoles(SiteInfo.RolesThatCanCreateUsers)) && !_isAdmin)
		{
			// only admins can edit admins
			if (_siteUser.IsInRoles("Admins"))
			{
				SiteUtils.RedirectToAccessDeniedPage();
				return;
			}
		}
		else
		{
			if (!_isAdmin)
			{
				if (!Request.IsAuthenticated)
				{
					SiteUtils.RedirectToLoginPage(this);
					return;
				}

				SiteUtils.RedirectToAccessDeniedPage();
				return;
			}
		}

		PopulateLabels();
		SetupScripts();

		if (_siteUser is not null && !IsPostBack)
		{
			BindRoles();
		}
	}


	private void BindRoles()
	{
		if (_userId == -1 || SiteInfo is null)
		{
			return;
		}

		using (IDataReader reader = SiteUser.GetRolesByUser(SiteInfo.SiteId, _userId))
		{
			userRoles.DataSource = reader;
			userRoles.DataBind();
		}

		using (IDataReader reader = Role.GetRolesUserIsNotIn(SiteInfo.SiteId, _userId))
		{
			if (WebUser.IsAdmin)
			{
				allRoles.DataSource = reader;
				allRoles.DataBind();
			}
			else
			{
				while (reader.Read())
				{
					var roleName = reader["RoleName"].ToString();

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
		}

		if (allRoles.Items.Count == 0)
		{
			allRoles.Enabled = false;
			addExisting.Enabled = false;
			addExisting.Text = Resource.ManageUsersUserIsInAllRolesMessage;
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

		if (_userId > -1 && SiteInfo is not null)
		{
			var user = new SiteUser(SiteInfo, _userId);
			var membershipProvider = Membership.Provider as mojoMembershipProvider;
			var isValidRequest = membershipProvider.ValidateUser(User.Identity.Name, hdnUserPassword.Value);

			if (!isValidRequest)
			{
				hdnUserPassword.Value = string.Empty;
				lblPasswordError.Text = Resource.SecurityPasswordIncorrect;
				lblPasswordError.Visible = true;

				return;
			}

			var roleID = int.Parse(allRoles.SelectedItem.Value, CultureInfo.InvariantCulture);
			var role = new Role(roleID);

			Role.AddUser(roleID, _userId, role.RoleGuid, user.UserGuid);

			user.RolesChanged = true;

			user.Save();

			BindRoles();
		}

		hdnUserPassword.Value = string.Empty;
		lblPasswordError.Visible = false;
	}


	private void UserRoles_ItemCommand(object sender, DataListCommandEventArgs e)
	{
		var roleID = Convert.ToInt32(userRoles.DataKeys[e.Item.ItemIndex]);
		var user = new SiteUser(SiteInfo, _userId);

		Role.RemoveUser(roleID, _userId);

		userRoles.EditItemIndex = -1;

		if (user.UserId > -1)
		{
			user.RolesChanged = true;
			user.Save();
		}

		BindRoles();
	}


	private void UserRoles_ItemDataBound(object sender, DataListItemEventArgs e)
	{
		ImageButton btnRemoveRole = e.Item.FindControl("btnRemoveRole") as ImageButton;
		UIHelper.AddConfirmationDialog(btnRemoveRole, Resource.ManageUsersRemoveRoleWarning);
	}


	protected bool CanDeleteUserFromRole(string roleName)
	{
		if (WebUser.IsAdmin) { return true; }

		return roleName != "Admins" && roleName != "Role Admins";
	}


	private void PopulateLabels()
	{
		addExisting.Text = Resource.ManageUsersAddToRoleButton;
		addExisting.ToolTip = Resource.ManageUsersAddToRoleButton;
		addExisting.Attributes.Add("onclick", "return EnterPassword();");
		SiteUtils.SetButtonAccessKey(addExisting, AccessKeys.ManageUsersAddToRoleButtonAccessKey);
	}

	private void LoadSettings()

	{
		_userId = WebUtils.ParseInt32FromQueryString("u", true, _userId);
		_isAdmin = WebUser.IsAdmin;

		if (_userId > -1)
		{
			_siteUser = new SiteUser(SiteInfo, _userId);

			if (_siteUser.UserId == -1)
			{
				_siteUser = null;
			}
		}
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
				$"<script src=\"{ResolveUrl($"~/ClientScript/mojo-prompt.js?v={siteSettings.SkinVersion}")}\"></script>",
				false);

			var script = $$"""
				<script>
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
		userRoles.ItemDataBound += new DataListItemEventHandler(UserRoles_ItemDataBound);
		userRoles.ItemCommand += new DataListCommandEventHandler(UserRoles_ItemCommand);
	}
}
