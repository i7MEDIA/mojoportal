using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.AdminUI;

public partial class SecurityRoles : NonCmsBasePage
{
	#region Fields

	private bool _isAdmin = false;
	private int _roleId = -1;
	private Role _role;
	private int _totalPages = 1;
	private int _pageNumber = 1;
	private int _pageSize = 20;

	protected bool CanManageUsers = false;
	protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;

	#endregion


	#region Overrides

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);

		Load += new EventHandler(Page_Load);
		btnSetUserFromGreyBox.Click += new ImageClickEventHandler(BtnSetUserFromGreyBox_Click);
		rptRoleMembers.ItemCommand += new RepeaterCommandEventHandler(RptRoleMembers_ItemCommand);
		rptRoleMembers.ItemDataBound += new RepeaterItemEventHandler(RptRoleMembers_ItemDataBound);

		SuppressMenuSelection();
		SuppressPageMenu();
	}

	#endregion


	private void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);
			return;
		}

		if (!WebUser.IsAdminOrRoleAdmin)
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		if (SiteUtils.IsFishyPost(this))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		SecurityHelper.DisableBrowserCache();

		LoadParams();
		_role = new Role(_roleId);
		EnforceSecurity();
		SetupScript();
		PopulateLabels();

		if (!Page.IsPostBack)
		{
			BindData();
		}
	}


	#region Event Methods

	private void BtnSetUserFromGreyBox_Click(object sender, ImageClickEventArgs e)
	{
		if (
			string.IsNullOrWhiteSpace(hdnUserID.Value) ||
			string.IsNullOrWhiteSpace(hdnUserPassword.Value))
		{
			return;
		}

		try
		{
			var userId = Convert.ToInt32(hdnUserID.Value);
			var user = new SiteUser(siteSettings, userId);
			var membershipProvider = Membership.Provider as mojoMembershipProvider;
			var isValidRequest = membershipProvider.ValidateUser(User.Identity.Name, hdnUserPassword.Value);

			if (!isValidRequest)
			{
				// Provided password did not match logged in user, do not change roles
				WebUtils.SetupRedirect(this, Request.RawUrl);
				return;
			}

			Role.AddUser(_roleId, userId, _role.RoleGuid, user.UserGuid);

			user.RolesChanged = true;

			user.Save();

			WebUtils.SetupRedirect(this, Request.RawUrl);

		}
		catch (FormatException) { }
	}


	private void RptRoleMembers_ItemCommand(object source, RepeaterCommandEventArgs e)
	{
		if (e.CommandName == "delete")
		{
			var userId = Convert.ToInt32(e.CommandArgument);
			var user = new SiteUser(siteSettings, userId);

			Role.RemoveUser(_roleId, userId);

			if (user.UserId > -1)
			{
				user.RolesChanged = true;
				user.Save();
			}
		}

		WebUtils.SetupRedirect(this, Request.RawUrl);
	}


	private void RptRoleMembers_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.FindControl("btnDelete") is ImageButton btnDelete)
		{
			btnDelete.AlternateText = Resource.ManageUsersRemoveFromRoleButton;
			btnDelete.ToolTip = Resource.ManageUsersRemoveFromRoleButton;

			UIHelper.AddConfirmationDialog(btnDelete, Resource.RolesRemoveUserWarning);
		}
	}

	#endregion


	#region Private Methods

	private void BindData()
	{
		heading.Text = $"{Resource.SecurityRolesTitle} {_role.DisplayName}";

		using IDataReader reader = Role.GetUsersInRole(
			siteSettings.SiteId,
			_roleId,
			_pageNumber,
			_pageSize,
			out _totalPages);

		var pageUrl = "~/Admin/SecurityRoles.aspx"
			.ToLinkBuilder()
			.AddParam("roleid", _roleId)
			.AddParam("pagenumber", "{0}")
			.ToString();

		pgr.PageURLFormat = pageUrl;
		pgr.ShowFirstLast = true;
		pgr.CurrentIndex = _pageNumber;
		pgr.PageSize = _pageSize;
		pgr.PageCount = _totalPages;
		pgr.Visible = _totalPages > 1;

		rptRoleMembers.DataSource = reader;

		rptRoleMembers.DataBind();
	}


	private void EnforceSecurity()
	{
		if (_role.RoleId == -1)
		{
			pnlSecurity.Visible = false;
			return;
		}

		if (_role.SiteId != siteSettings.SiteId)
		{
			pnlSecurity.Visible = false;
			return;
		}

		if (!_isAdmin)
		{
			if (_role.Equals("Admins") || _role.Equals("Role Admins"))
			{
				pnlSecurity.Visible = false;
				return;
			}

			if (!WebConfigSettings.AllowRoleAdminsToCreateContentManagers)
			{
				if (_role.Equals("Content Administrators"))
				{
					pnlSecurity.Visible = false;
					return;
				}
			}
		}
	}


	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuRoleAdminLink);

		lnkAdminMenu.Text = Resource.AdminMenuLink;
		lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
		lnkAdminMenu.NavigateUrl = "~/Admin/AdminMenu.aspx".ToLinkBuilder().ToString();
		lnkRoleManager.Text = Resource.AdminMenuRoleAdminLink;
		lnkRoleManager.ToolTip = Resource.AdminMenuRoleAdminLink;
		lnkRoleManager.NavigateUrl = "~/Admin/RoleManager.aspx".ToLinkBuilder().ToString();

		lnkFindUser.Text = Resource.SecurityAddExistingButton;
		lnkFindUser.ToolTip = Resource.SecurityAddExistingButton;
		lnkFindUser.NavigateUrl = "~/Dialog/RoleUserSelectDialog.aspx"
			.ToLinkBuilder()
			.AddParam("r", _roleId)
			.ToString();

		btnSetUserFromGreyBox.ImageUrl = "~/Data/SiteImages/1x1.gif".ToLinkBuilder().ToString();
		btnSetUserFromGreyBox.Attributes.Add("tabIndex", "-1");
		btnSetUserFromGreyBox.AlternateText = " ";
	}


	private void SetupScript()
	{
		var script = $$"""

			<script>
				function SelectUser(userId, password) {
					const hdnUI = document.getElementById('{{hdnUserID.ClientID}}');
					const hdnUserPassword = document.getElementById('{{hdnUserPassword.ClientID}}');

					hdnUI.value = userId;
					hdnUserPassword.value = password;

					const btn = document.getElementById('{{btnSetUserFromGreyBox.ClientID}}');

					btn.click();

					$('.modal').modal('hide');
				}
			</script>
			""";

		Page.ClientScript.RegisterStartupScript(typeof(Page), "SelectUserHandler", script.ToString());
	}


	private void LoadParams()
	{
		_roleId = WebUtils.ParseInt32FromQueryString("roleid", -1);
		_isAdmin = WebUser.IsAdmin;

		if (_isAdmin)
		{
			CanManageUsers = true;
		}
		else
		{
			CanManageUsers = WebUser.IsInRoles(siteSettings.RolesThatCanManageUsers);
		}

		ScriptConfig.IncludeColorBox = true;

		_pageSize = WebConfigSettings.RoleMemberPageSize;
		_pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", _pageNumber);

		AddClassToBody("administration");
		AddClassToBody("securityroles");
	}

	#endregion
}
