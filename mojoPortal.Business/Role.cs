// Author:					
// Created:				    2004-07-19
// Last Modified:		    2018-10-31
//
// You must not remove this notice, or any other, from this software. 

using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using mojoPortal.Data;
using log4net;

namespace mojoPortal.Business
{
	/// <summary>
	/// Represents a user role.
	/// </summary>
	public class Role
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(Role));

        public const string ContentAdministratorsRole = "Content Administrators";
        public const string AdministratorsRole = "Admins";

		#region Constructors


		public Role()
		{}

		public Role(int roleId)
		{
			GetRole(roleId);
		}

		public Role(int siteId, string roleName)
		{
			GetRole(siteId, roleName);
		}

		#endregion
		#region Private Properties

		private static bool UseRelatedSiteMode
        {
            get
            {
                if (
                    (ConfigurationManager.AppSettings["UseRelatedSiteMode"] != null)
                    && (ConfigurationManager.AppSettings["UseRelatedSiteMode"] == "true")
                )
                {
                    return true;
                }

                return false;
            }
        }

        private static int RelatedSiteID
        {
            get
            {
                int result = 1;
                if (ConfigurationManager.AppSettings["RelatedSiteID"] != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings["RelatedSiteID"], out result);
                }

                return result;
            }
        }

		#endregion

		#region Public Properties

		public int RoleId { get; private set; } = -1;

		public Guid RoleGuid { get; private set; } = Guid.Empty;

		public Guid SiteGuid { get; set; } = Guid.Empty;

		public int SiteId { get; set; } = -1;

		public string RoleName { get; set; } = string.Empty;

		public string DisplayName { get; set; } = string.Empty;
		/// <summary>
		/// this is only populated when calling GetbySite
		/// </summary>
		public int MemberCount { get; private set; } = 0;
        public string Description { get; set; } = string.Empty;

        #endregion


        #region Private Methods

        private void GetRole(int roleId)
		{
            using (IDataReader reader = DBRoles.GetById(roleId))
            {
                if (reader.Read())
                {
                    this.RoleId = int.Parse(reader["RoleID"].ToString());

                    this.SiteId = int.Parse(reader["SiteID"].ToString());

                    if (UseRelatedSiteMode) { SiteId = RelatedSiteID; }

                    this.RoleName = reader["RoleName"].ToString();
                    this.DisplayName = reader["DisplayName"].ToString();
                    this.Description = reader["Description"].ToString();
                    this.SiteGuid = new Guid(reader["SiteGuid"].ToString());
                    this.RoleGuid = new Guid(reader["RoleGuid"].ToString());

                }
            }
		}

		private void GetRole(int siteId, string roleName)
		{
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            using (IDataReader reader = DBRoles.GetByName(siteId, roleName))
            {
                if (reader.Read())
                {
                    this.RoleId = int.Parse(reader["RoleID"].ToString());
                    this.SiteId = int.Parse(reader["SiteID"].ToString());
                    this.RoleName = reader["RoleName"].ToString();
                    this.DisplayName = reader["DisplayName"].ToString();
                    this.Description = reader["Description"].ToString();
                    this.SiteGuid = new Guid(reader["SiteGuid"].ToString());
                    this.RoleGuid = new Guid(reader["RoleGuid"].ToString());

                }
            }
		}

		private bool Create()
		{
			int newID = 0;
			// role name never changes after creation
			// only display name is allowed to change
			// otherwise permissions to view pages 
			// and edit modules would be orphaned when
			// a role was re-named

            if (UseRelatedSiteMode) { SiteId = RelatedSiteID; }

			if(Exists(this.SiteId, this.RoleName))
			{
				//string errorMessage = ConfigurationManager.AppSettings["RoleExistsError"];
				//throw new Exception(errorMessage);
                // do nothing instead of  throwing an exception

			}
			else
			{
                this.RoleGuid = Guid.NewGuid();

               
				newID = DBRoles.RoleCreate(
                    RoleGuid,
                    SiteGuid,
                    SiteId,
					RoleName,
                    DisplayName,
                    Description);
			}

			if(newID > 0)
			{
				RoleId = newID;
				//this.roleName = this.displayName;
				return true;
			}
			else
			{
				return false;
			}
		}

		private bool Update()
		{
			return DBRoles.Update(RoleId, DisplayName, Description);
		}

        public bool Equals(string roleName)
        {
            bool result = false;
            if (roleName == RoleName) result = true;

            return result;

        }

		#endregion


		#region Public Methods

		

		public bool Save()
		{
			if(this.RoleId > -1)
			{
				return Update();
			}
			else
			{
				return Create();
			}
		}

        public bool HasUsers()
        {
            return (CountOfUsers() > 0);
        }

        public int CountOfUsers()
        {
            // TODO: implement actual select count from db
            // this is works but is not ideal
            int count = 0;
            using (IDataReader reader = GetRoleMembers(this.RoleId))
            {
                while (reader.Read())
                {
                    count += 1;
                }
            }

            return count;

        }


        public bool CanBeDeleted(List<string> rolesThatCannotBeDeleted)
        {
            if (rolesThatCannotBeDeleted != null)
            {
                foreach (string roleName in rolesThatCannotBeDeleted)
                {
                    //if (this.DisplayName == roleName) { return false; }
                    if (this.RoleName == roleName) { return false; }
                }
            }


            return true;
        }


		#endregion

		#region Static Methods

		public static IDataReader GetSiteRoles(int siteId)
		{
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

			return DBRoles.GetSiteRoles(siteId);
		}

        //public static Collection<Role> GetbySite(int siteId)
        //{
        //    bool enforceRelatedSitesMode = false;
        //    return GetbySite(siteId, enforceRelatedSitesMode);
        //}

        public static Collection<Role> GetBySite(int siteId)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            Collection<Role> roles = new Collection<Role>();
            using (IDataReader reader = DBRoles.GetSiteRoles(siteId))
            {
                while (reader.Read())
                {
                    Role role = new Role();
                    role.RoleId = Convert.ToInt32(reader["RoleID"]);
                    role.SiteId = Convert.ToInt32(reader["SiteID"]);
                    role.DisplayName = reader["DisplayName"].ToString();
                    role.Description = reader["Description"].ToString(); 
                    role.RoleName = reader["RoleName"].ToString();
                    role.RoleGuid = new Guid(reader["RoleGuid"].ToString());
                    role.SiteGuid = new Guid(reader["SiteGuid"].ToString());
                    role.MemberCount = Convert.ToInt32(reader["MemberCount"]);

                    roles.Add(role);
                }
            }

            return roles;

        }

        public static Role GetRoleByName(int siteId, string roleName)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            Role role = null;

            using (IDataReader reader = DBRoles.GetSiteRoles(siteId))
            {
                while (reader.Read())
                {
                    string foundName = reader["RoleName"].ToString();
                    if (foundName == roleName)
                    {
                        role = new Role();
                        role.RoleId = Convert.ToInt32(reader["RoleID"]);
                        role.SiteId = Convert.ToInt32(reader["SiteID"]);
                        role.DisplayName = reader["DisplayName"].ToString();
                        role.Description = reader["Description"].ToString();
                        role.RoleName = reader["RoleName"].ToString();
                        role.RoleGuid = new Guid(reader["RoleGuid"].ToString());
                        role.SiteGuid = new Guid(reader["SiteGuid"].ToString());
                    }
                }
            }

            return role;

        }

        public static List<int> GetRoleIds(int siteId, string roleNamesSeparatedBySemiColons)
        {
            List<int> roleIds = new List<int>();

            List<string> roleNames = GetRolesNames(roleNamesSeparatedBySemiColons);

            foreach (string roleName in roleNames)
            {
                if (string.IsNullOrEmpty(roleName)) { continue; }
                Role r = Role.GetRoleByName(siteId, roleName);
                if (r == null)
                {
                    log.Debug("could not get roleid for role named " + roleName);
                    continue;
                }
                if (r.RoleId > -1) { roleIds.Add(r.RoleId); }
            }

            return roleIds;
        }

        public static List<string> GetRolesNames(string roleNamesSeparatedBySemiColons)
        {
            List<string> roleNames = new List<string>();
            string[] roles = roleNamesSeparatedBySemiColons.Split(';');
            foreach (string r in roles)
            {
                if (!roleNames.Contains(r)) { roleNames.Add(r); }
            }

            return roleNames;

        }


        public static int CountOfRoles(int siteId)
        {
            // TODO: implement actual select count from db
            // this is works but is not ideal
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            //int count = 0;
            //using(IDataReader reader = GetSiteRoles(siteId))
            //{
            //    while (reader.Read())
            //    {
            //        count += 1;
            //    }
                
            //}

            //return count;

            return DBRoles.GetCountOfSiteRoles(siteId);

        }

		public static bool DeleteRole(int roleId)
		{
			return DBRoles.Delete(roleId);
		}

		public static bool Exists(int siteId, String roleName)
		{
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
			return DBRoles.Exists(siteId, roleName);
		}

		public static IDataReader GetRoleMembers(int roleId)
		{
			return DBRoles.GetRoleMembers(roleId);
		}

        public static IDataReader GetUsersNotInRole(int siteId, int roleId, int pageNumber, int pageSize, out int totalPages)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return DBRoles.GetUsersNotInRole(siteId, roleId, pageNumber, pageSize, out totalPages);
        }

        public static IDataReader GetUsersInRole(int siteId, int roleId, int pageNumber, int pageSize, out int totalPages)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return DBRoles.GetUsersInRole(siteId, roleId, pageNumber, pageSize, out totalPages);
        }

        public static IDataReader GetRolesUserIsNotIn(
            int siteId,
            int userId)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return DBRoles.GetRolesUserIsNotIn(siteId, userId);
        }

		public static bool AddUser(
            int roleId, 
            int userId,
            Guid roleGuid,
            Guid userGuid
            )
		{
			return DBRoles.AddUser(roleId,userId,roleGuid,userGuid);
		}

		public static bool RemoveUser(int roleId, int userId)
		{
			return DBRoles.RemoveUser(roleId,userId);
		}

		public static bool DeleteUserRoles(int userId)
		{
			return DBRoles.DeleteUserRoles(userId);
		}

        public static void AddUserToDefaultRoles(SiteUser siteUser)
        {
            
            Role role = new Role(siteUser.SiteId, "Authenticated Users");
            if (role.RoleId > -1)
            {
                Role.AddUser(role.RoleId, siteUser.UserId, role.RoleGuid, siteUser.UserGuid);
            }

            string defaultRoles = string.Empty;

            if (System.Configuration.ConfigurationManager.AppSettings["DefaultRolesForNewUsers"] != null)
            {
                defaultRoles = System.Configuration.ConfigurationManager.AppSettings["DefaultRolesForNewUsers"];
            }

            if (defaultRoles.Length > 0)
            {
                if (defaultRoles.IndexOf(";") == -1)
                {
                    role = new Role(siteUser.SiteId, defaultRoles);
                    if (role.RoleId > -1)
                    {
                        Role.AddUser(role.RoleId, siteUser.UserId, role.RoleGuid, siteUser.UserGuid);
                    }
                }
                else
                {
                    string[] roleArray = defaultRoles.Split(';');
                    foreach (string roleName in roleArray)
                    {
                        if (!string.IsNullOrEmpty(roleName)) 
                        {
                            role = new Role(siteUser.SiteId, roleName);
                            if (role.RoleId > -1)
                            {
                                Role.AddUser(role.RoleId, siteUser.UserId, role.RoleGuid, siteUser.UserGuid);
                            }
                        }
                    }

                }

            }


        }



		#endregion

	}
}
