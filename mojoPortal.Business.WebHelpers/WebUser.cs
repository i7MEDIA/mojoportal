using System;
using System.Collections;
using System.Configuration;
using System.Web;

namespace mojoPortal.Business.WebHelpers
{
	public static class WebUser
	{
		public static bool IsInRole(string role)
		{
			if (HttpContext.Current == null || HttpContext.Current.User == null) { return false; }
			//if (role == null) { return false; }
			if (string.IsNullOrWhiteSpace(role)) { return false; }
			if (role.Contains("All Users")) { return true; }
			if (!HttpContext.Current.Request.IsAuthenticated) { return false; }
			if (HttpContext.Current.User.IsInRole("Admins")) { return true; }
			//if (String.IsNullOrEmpty(role)) { return false; }

			return HttpContext.Current.User.IsInRole(role);
		}


		public static bool IsInRoles(string roles)
		{
			if (IsInRole("Admins")) return true;
			if (String.IsNullOrEmpty(roles)) return false;
			if (roles.Contains("All Users;")) return true;
			if (!HttpContext.Current.Request.IsAuthenticated) return false;

			foreach (string role in roles.Split(new char[] { ';' }))
			{
				if (role.IndexOf("All Users") > -1) return true;
				if (IsInRole(role)) return true;
			}
			return false;
		}


		public static bool IsInRoles(IList roles)
		{
			if (IsInRole("Admins")) return true;

			if (roles == null) return false;

			if (roles.Contains("All Users")) return true;

			if (!HttpContext.Current.Request.IsAuthenticated) return false;



			foreach (string role in roles)
			{
				if (role.Contains("All Users")) return true;
				if (IsInRole(role)) return true;
			}
			return false;
		}


		public static bool IsAdmin
		{
			get
			{
				try
				{
					if (!HttpContext.Current.Request.IsAuthenticated) return false;
					return IsInRole("Admins");
				}
				catch (System.NullReferenceException)
				{
					return false;
				}
			}
		}


		public static bool IsContentAdmin
		{
			get
			{
				try
				{
					if (!HttpContext.Current.Request.IsAuthenticated) return false;
					return IsInRole("Content Administrators");
				}
				catch (System.NullReferenceException)
				{
					return false;
				}
			}
		}

		public static bool IsContentPublisher
		{
			get
			{
				try
				{
					if (!HttpContext.Current.Request.IsAuthenticated) return false;
					return IsInRole("Content Publishers");
				}
				catch (System.NullReferenceException)
				{
					return false;
				}
			}
		}

		public static bool IsContentAuthor
		{
			get
			{
				try
				{
					if (!HttpContext.Current.Request.IsAuthenticated) return false;
					return IsInRole("Content Authors");
				}
				catch (System.NullReferenceException)
				{
					return false;
				}
			}
		}

		public static bool IsRoleAdmin
		{
			get
			{
				try
				{
					if (!HttpContext.Current.Request.IsAuthenticated) return false;
					return IsInRole("Role Admins");
				}
				catch (System.NullReferenceException)
				{
					return false;
				}
			}
		}

		public static bool IsNewsletterAdmin
		{
			get
			{
				try
				{
					if (!HttpContext.Current.Request.IsAuthenticated) return false;
					return IsInRole("Newsletter Administrators");
				}
				catch (System.NullReferenceException)
				{
					return false;
				}
			}
		}

		public static bool IsSkinManager
		{
			get
			{
				try
				{
					if (!HttpContext.Current.Request.IsAuthenticated) return false;
					SiteSettings siteSettings = (SiteSettings)HttpContext.Current.Items["SiteSettings"];
					if (siteSettings == null) return false;
					return IsInRoles(siteSettings.RolesThatCanManageSkins);
				}
				catch (System.NullReferenceException)
				{
					return false;
				}
			}
		}

		public static bool IsAdminOrContentAdmin
		{
			get { return IsAdmin || IsContentAdmin; }
		}

		public static bool IsAdminOrContentAdminOrContentAuthor
		{
			get { return IsAdmin || IsContentAdmin || IsContentAuthor; }
		}

		public static bool IsAdminOrContentAdminOrContentPublisher
		{
			get { return IsAdmin || IsContentAdmin || IsContentPublisher; }
		}

		public static bool IsAdminOrContentAdminOrContentPublisherOrContentAuthor
		{
			get { return IsAdmin || IsContentAdmin || IsContentPublisher || IsContentAuthor; }
		}


		public static bool IsAdminOrContentAdminOrRoleAdmin
		{
			get { return IsAdmin || IsContentAdmin || IsRoleAdmin; }
		}

		public static bool IsAdminOrRoleAdmin
		{
			get { return IsAdmin || IsRoleAdmin; }
		}

		public static bool IsAdminOrContentAdminOrRoleAdminOrNewsletterAdmin
		{
			get { return IsAdmin || IsContentAdmin || IsRoleAdmin || IsNewsletterAdmin; }
		}

		public static bool IsAdminOrNewsletterAdmin
		{
			get { return IsAdmin || IsNewsletterAdmin; }
		}

		//public static bool IsNotAllowedToEditModuleSettings
		//{
		//    get 
		//    {
		//        if (!HttpContext.Current.Request.IsAuthenticated) return true;
		//        if (IsAdmin) { return false; }
		//        if (IsContentAdmin) { return false; }

		//        if (ConfigurationManager.AppSettings["RolesNotAllowedToEditModuleSettings"] != null)
		//        {
		//            string forbiddenRoles = ConfigurationManager.AppSettings["RolesNotAllowedToEditModuleSettings"];
		//            if (!string.IsNullOrEmpty(forbiddenRoles))
		//            {
		//                return IsInRoles(forbiddenRoles);
		//            }
		//        }

		//        return true;
		//    }
		//}


		public static bool HasEditPermissions(int siteId, int moduleId, int pageId)
		{
			if (HttpContext.Current == null || HttpContext.Current.User == null) return false;

			if (!HttpContext.Current.Request.IsAuthenticated) return false;

			if (IsAdmin || IsContentAdmin) return true;

			Module module = new Module(moduleId, pageId);
			PageSettings pageSettings = new PageSettings(siteId, module.PageId);

			if (pageSettings == null) return false;
			if (pageSettings.PageId < 0) return false;



			if (IsInRoles(pageSettings.EditRoles) || IsInRoles(module.AuthorizedEditRoles))
			{
				return true;
			}

			if (module.EditUserId > 0)
			{
				SiteSettings siteSettings = (SiteSettings)HttpContext.Current.Items["SiteSettings"];
				SiteUser siteUser = new SiteUser(siteSettings, HttpContext.Current.User.Identity.Name);
				if (module.EditUserId == siteUser.UserId)
				{
					return true;
				}
			}

			return false;
		}




		//public static bool HasPageEditPermissions(int siteID, int pageID)
		//{
		//    if (IsAdmin || IsContentAdmin) return true;
		//    PageSettings pageSettings = new PageSettings(siteID, pageID);
		//    return IsInRoles(pageSettings.EditRoles);
		//}

	}
}
