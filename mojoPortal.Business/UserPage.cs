// Author:					
// Created:				    2006-06-12
// Last Modified:			2011-01-19
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// Represents persoanlized pages created by the user within the MyPage feature aka WebParts
    /// </summary>
    public class UserPage
    {

        #region Constructors

        public UserPage()
        { }


        public UserPage(Guid userPageId)
        {
            GetUserPage(userPageId);
        }

        #endregion

        #region Private Properties

        private Guid userPageID = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private int siteID = -1;
        private Guid userGuid = Guid.Empty;
        private string pageName = string.Empty;
        private String pagePath = String.Empty;
        private int pageOrder = 1;

        #endregion

        #region Public Properties

        public Guid UserPageId
        {
            get { return userPageID; }
            set { userPageID = value; }
        }

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }
        public string PageName
        {
            get { return pageName; }
            set { pageName = value; }
        }
        public string PagePath
        {
            get { return pagePath; }
            set { pagePath = value; }
        }
        public int PageOrder
        {
            get { return pageOrder; }
            set { pageOrder = value; }
        }

        #endregion

        #region Private Methods

        private void GetUserPage(Guid userPageId)
        {
            using (IDataReader reader = DBUserPage.GetUserPage(userPageId))
            {
                if (reader.Read())
                {
                    this.userPageID = new Guid(reader["UserPageID"].ToString());
                    this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    this.siteID = Convert.ToInt32(reader["SiteID"]);
                    this.userGuid = new Guid(reader["UserGuid"].ToString());
                    this.pageName = reader["PageName"].ToString();
                    this.pagePath = reader["PagePath"].ToString();
                    this.pageOrder = Convert.ToInt32(reader["PageOrder"]);

                }

            }

        }

        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.userPageID = newID;

            int rowsAffected = DBUserPage.AddUserPage(
                this.userPageID,
                this.siteGuid,
                this.siteID,
                this.userGuid,
                this.pageName,
                this.pagePath,
                this.pageOrder);

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBUserPage.UpdateUserPage(
                this.userPageID,
                this.pageName,
                this.pageOrder);

        }


        #endregion

        #region Public Methods


        public bool Save()
        {
            if (this.userPageID != Guid.Empty)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }

        // aka left
        public void MoveUp()
        {
            
            //no need to look up whther pages above me, assume yes if my sort is > 1
            if (this.pageOrder > 1)
            {
                //pages exist above me so update my page order
                this.pageOrder -= 3;
                UserPage.UpdatePageOrder(this.userPageID, this.pageOrder);
               
                ResortPages();

            }
            else
            {
                if (this.pageOrder < 1)
                {
                    this.pageOrder = 1;
                    UserPage.UpdatePageOrder(this.userPageID, this.pageOrder);
                    ResortPages();

                }
                

            }

        }

        // aka down
        public void MoveDown()
        {
            this.pageOrder += 3;
            UserPage.UpdatePageOrder(this.userPageID, this.pageOrder);
            //now get all children of my parent id and reset sort using current order
            ResortPages();

        }

        public void ResortPages()
        {
            int i = 1;
            IDataReader reader = UserPage.SelectByUser(this.userGuid);
            while (reader.Read())
            {
                Guid pageID = new Guid(reader["UserPageID"].ToString());
                UserPage.UpdatePageOrder(pageID, i);
                i += 2;


            }

            reader.Close();
        }




        #endregion


        #region Static Methods

        public static IDataReader SelectByUser(Guid userGuid)
        {
            return DBUserPage.SelectByUser(userGuid);
        }

        public static DataTable GetUserPageMenu(Guid userGuid)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("UserPageID", typeof(String));
            dataTable.Columns.Add("SiteID", typeof(int));
            dataTable.Columns.Add("UserGuid", typeof(String));
            dataTable.Columns.Add("PageName", typeof(String));
            dataTable.Columns.Add("PagePath", typeof(String));
            dataTable.Columns.Add("PageOrder", typeof(int));

            using (IDataReader reader = SelectByUser(userGuid))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["UserPageID"] = reader["UserPageID"].ToString();
                    row["SiteID"] = Convert.ToInt32(reader["SiteID"]);
                    row["UserGuid"] = reader["UserGuid"].ToString();
                    row["PageName"] = reader["PageName"].ToString();
                    row["PagePath"] = reader["PagePath"].ToString();
                    row["PageOrder"] = Convert.ToInt32(reader["PageOrder"]);
                    dataTable.Rows.Add(row);

                }

            }

            return dataTable;

        }

        public static int GetCountByUser(Guid userGuid)
        {
            int result = 0;

            using (IDataReader reader = DBUserPage.SelectByUser(userGuid))
            {
                while (reader.Read())
                {
                    result += 1;
                }
            }

            return result;

        }

        public static String GetDefaultPagePath(
            Guid userGuid, 
            SiteSettings siteSettings, 
            String defaultFirstPageName, 
            String deafaultFirstPagePath)
        {
            String result = String.Empty;
            int countOfPages = 0;

            using (IDataReader reader = DBUserPage.SelectByUser(userGuid))
            {
                while ((reader.Read()) && (countOfPages < 1))
                {
                    result = reader["PagePath"].ToString();
                    result += 1;
                }
            }

            if (result == String.Empty)
            {
                UserPage userPage = new UserPage();
                userPage.SiteId = siteSettings.SiteId;
                userPage.SiteGuid = siteSettings.SiteGuid;
                userPage.UserGuid = userGuid;
                userPage.PageName = defaultFirstPageName;
                userPage.PagePath = deafaultFirstPagePath;
                userPage.PageOrder = 1;
                userPage.Save();

                result = deafaultFirstPagePath;

            }


            return result;
        }

        public static int GetNextPageOrder(Guid userGuid)
        {
            return DBUserPage.GetNextPageOrder(userGuid);
        }

        public static bool UpdatePageOrder(Guid userPageId, int pageOrder)
        {
            return DBUserPage.UpdatePageOrder(userPageId, pageOrder);
        }

        public static bool DeleteUserPage(Guid userPageId)
        {
            return DBUserPage.DeleteUserPage(userPageId);
        }

        public static bool DeleteByUser(Guid userGuid)
        {
            return DBUserPage.DeleteByUser(userGuid);
        }

        #endregion


    }

}
