// Author:					Joe Audette
// Created:					2010-04-06
// Last Modified:			2010-04-20
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
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Data.SqlServerCe;

namespace mojoPortal.Data
{
    public static class DBSitePersonalization
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

        public static void SavePersonalizationBlob(
            int siteId,
            String path,
            Guid userGuid,
            byte[] dataBlob,
            DateTime lastUpdateTime)
        {
            Guid pathID = GetOrCreatePathId(siteId, path);
            if (PersonalizationBlobExists(pathID, userGuid))
            {
                UpdatePersonalizationBlob(userGuid, pathID, dataBlob, lastUpdateTime);
            }
            else
            {
                CreatePersonalizationBlob(userGuid, pathID, dataBlob, lastUpdateTime);
            }

        }

        public static byte[] GetPersonalizationBlob(
            int siteId,
            String path,
            Guid userGuid)
        {
            Guid pathId = GetOrCreatePathId(siteId, path);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT PageSettings ");
            sqlCommand.Append("FROM	mp_SitePersonalizationPerUser ");
            sqlCommand.Append("WHERE PathID = @PathID AND UserID = @UserID; ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid;

            arParams[1] = new SqlCeParameter("@PathID", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pathId;

            byte[] result = null;

            try
            {
                //result = (byte[])SqlHelper.ExecuteScalar(
                //    GetConnectionString(),
                //    CommandType.Text,
                //    sqlCommand.ToString(),
                //    arParams);

                using (IDataReader reader = SqlHelper.ExecuteReader(
                     GetConnectionString(),
                     CommandType.Text,
                     sqlCommand.ToString(),
                     arParams))
                {
                    if (reader.Read())
                    {
                        result = (byte[])reader["PageSettings"];
                    }

                }

            }
            catch (System.InvalidCastException)
            {
                //if (log.IsErrorEnabled)
                //{
                //    log.Error("dbPortal.SitePersonalization_GetPersonalizationBlob", ex);
                //}
            }

            return result;

           
        }

        public static void ResetPersonalizationBlob(
            int siteId,
            String path,
            Guid userGuid)
        {
            Guid pathId = GetOrCreatePathId(siteId, path);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE  ");
            sqlCommand.Append("FROM	mp_SitePersonalizationPerUser ");
            sqlCommand.Append("WHERE PathID = @PathID AND UserID = @UserID  ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@PathID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pathId;

            arParams[1] = new SqlCeParameter("@UserID", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid;

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

           
        }

        public static void ResetPersonalizationBlob(
            int siteId,
            String path)
        {
           
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE  ");
            sqlCommand.Append("FROM	mp_SitePersonalizationAllUsers ");
            sqlCommand.Append("WHERE PathID IN (   ");
            sqlCommand.Append("SELECT PathID    ");
            sqlCommand.Append("FROM mp_SitePaths    ");
            sqlCommand.Append("WHERE SiteID = @SiteID  ");
            sqlCommand.Append(" )  ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

           
        }

        public static byte[] GetPersonalizationBlobAllUsers(
            int siteId,
            String path)
        {
            
            Guid pathId = GetOrCreatePathId(siteId, path);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT PageSettings ");
            sqlCommand.Append("FROM	mp_SitePersonalizationAllUsers ");
            sqlCommand.Append("WHERE PathID = @PathID   ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PathID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pathId;


            byte[] result = null;

            try
            {
                //result = (byte[])SqlHelper.ExecuteScalar(
                //    GetConnectionString(),
                //    CommandType.Text,
                //    sqlCommand.ToString(),
                //    arParams);

                using (IDataReader reader = SqlHelper.ExecuteReader(
                     GetConnectionString(),
                     CommandType.Text,
                     sqlCommand.ToString(),
                     arParams))
                {
                    if (reader.Read())
                    {
                        result = (byte[])reader["PageSettings"];
                    }

                }
            }
            catch (System.InvalidCastException)
            {
                //if (log.IsErrorEnabled)
                //{
                //    log.Error("dbPortal.SitePersonalization_GetPersonalizationBlobAllUsers", ex);
                //}
            }

            return result;
        }

        public static void SavePersonalizationBlobAllUsers(
            int siteId,
            String path,
            byte[] dataBlob,
            DateTime lastUpdateTime)
        {
 
            Guid pathID = GetOrCreatePathId(siteId, path);

            if (PersonalizationBlobExists(pathID))
            {
                UpdatePersonalizationBlob(pathID, dataBlob, lastUpdateTime);
            }
            else
            {
                CreatePersonalizationBlob(pathID, dataBlob, lastUpdateTime);
            }

        }

        public static int GetCountOfState(
            int siteId,
            String path,
            bool allUserScope,
            Guid userGuid,
            DateTime inactiveSince)
        {
            int result = 0;

            Guid pathID = Guid.Empty;
            if ((path != null) && (path.Length > 0))
            {
                pathID = GetOrCreatePathId(siteId, path);
            }


            if (allUserScope)
            {
                if (pathID != Guid.Empty)
                {
                    result = GetCountOfStateAllUsers(pathID);
                }
                else
                {
                    result = GetCountOfStateAllUsers();
                }

            }
            else
            {

                if (userGuid != Guid.Empty)
                {
                    if (pathID != Guid.Empty)
                    {
                        if (inactiveSince > DateTime.MinValue)
                        {
                            result = GetCountOfStateByUser(userGuid, pathID, inactiveSince);

                        }
                        else
                        {
                            result = GetCountOfStateByUser(userGuid, pathID);
                        }
                    }
                    else
                    {
                        if (inactiveSince > DateTime.MinValue)
                        {
                            result = GetCountOfStateByUserAllPaths(userGuid, inactiveSince);

                        }
                        else
                        {
                            result = GetCountOfStateByUserAllPaths(userGuid);
                        }

                    }

                }
                else
                {
                    // not a specific user
                    if (pathID != Guid.Empty)
                    {
                        if (inactiveSince > DateTime.MinValue)
                        {
                            result = GetCountOfStateByUser(pathID, inactiveSince);

                        }
                        else
                        {
                            result = GetCountOfStateByUser(inactiveSince);
                        }
                    }
                    else
                    {
                        // not a specific path
                        if (inactiveSince > DateTime.MinValue)
                        {
                            result = GetCountOfStateByUser(inactiveSince);

                        }
                        else
                        {
                            result = GetCountOfStateByUser();
                        }

                    }

                }

            }

            return result;
        }


        /******** private *********************/

        public static void CreatePersonalizationBlob(
            Guid pathId,
            byte[] dataBlob,
            DateTime lastUpdateTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SitePersonalizationAllUsers ");
            sqlCommand.Append("(");
            sqlCommand.Append("PathID, ");
            sqlCommand.Append("PageSettings, ");
            sqlCommand.Append("LastUpdate ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@PathID, ");
            sqlCommand.Append("@PageSettings, ");
            sqlCommand.Append("@LastUpdate ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@PathID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pathId;

            arParams[1] = new SqlCeParameter("@PageSettings", SqlDbType.Image);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = dataBlob;

            arParams[2] = new SqlCeParameter("@LastUpdate", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = lastUpdateTime;

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static void UpdatePersonalizationBlob(
            Guid pathId,
            byte[] dataBlob,
            DateTime lastUpdateTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_SitePersonalizationAllUsers ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("PageSettings = @PageSettings, ");
            sqlCommand.Append("LastUpdate = @LastUpdate ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("PathID = @PathID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@PathID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pathId;

            arParams[1] = new SqlCeParameter("@PageSettings", SqlDbType.Image);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = dataBlob;

            arParams[2] = new SqlCeParameter("@LastUpdate", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = lastUpdateTime;

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        private static void CreatePersonalizationBlob(
            Guid userGuid,
            Guid pathId,
            byte[] dataBlob,
            DateTime lastUpdateTime)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SitePersonalizationPerUser ");
            sqlCommand.Append("(");
            sqlCommand.Append("ID, ");
            sqlCommand.Append("PathID, ");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("PageSettings, ");
            sqlCommand.Append("LastUpdate ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ID, ");
            sqlCommand.Append("@PathID, ");
            sqlCommand.Append("@UserID, ");
            sqlCommand.Append("@PageSettings, ");
            sqlCommand.Append("@LastUpdate ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[5];

            arParams[0] = new SqlCeParameter("@ID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = Guid.NewGuid();

            arParams[1] = new SqlCeParameter("@PathID", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pathId;

            arParams[2] = new SqlCeParameter("@UserID", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid;

            arParams[3] = new SqlCeParameter("@PageSettings", SqlDbType.Image);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = dataBlob;

            arParams[4] = new SqlCeParameter("@LastUpdate", SqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = lastUpdateTime;

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        private static void UpdatePersonalizationBlob(
            Guid userGuid,
            Guid pathId,
            byte[] dataBlob,
            DateTime lastUpdateTime)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_SitePersonalizationPerUser ");
            sqlCommand.Append("SET ");
            sqlCommand.Append("PageSettings = @PageSettings, ");
            sqlCommand.Append("LastUpdate = @LastUpdate ");

            sqlCommand.Append("WHERE UserID = @UserID AND PathID = @PathID  ;");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@PathID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pathId;

            arParams[1] = new SqlCeParameter("@UserID", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid;

            arParams[2] = new SqlCeParameter("@PageSettings", SqlDbType.Image);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = dataBlob;

            arParams[3] = new SqlCeParameter("@LastUpdate", SqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = lastUpdateTime;

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static bool PersonalizationBlobExists(Guid pathId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_SitePersonalizationAllUsers ");
            sqlCommand.Append("WHERE PathID = @PathID  ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PathID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pathId;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static bool PersonalizationBlobExists(Guid pathId, Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COALESCE(Count(*),0) ");
            sqlCommand.Append("FROM	mp_SitePersonalizationPerUser ");
            sqlCommand.Append("WHERE PathID = @PathID AND UserID = @UserID ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@PathID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pathId;

            arParams[1] = new SqlCeParameter("@UserID", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }


        private static Guid GetOrCreatePathId(int siteId, String path)
        {
            Guid result = Guid.Empty;

            if (PathExists(siteId, path))
            {
                result = GetPathId(siteId, path);
            }
            else
            {
                result = CreatePath(siteId, path);
            }

            return result;
        }

        private static Guid CreatePath(int siteId, String path)
        {
            Guid newPathID = Guid.NewGuid();
            Guid siteGuid = DBSiteSettings.GetSiteGuidFromID(siteId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SitePaths (PathID, SiteID, SiteGuid, Path, LoweredPath)");
            sqlCommand.Append("VALUES (");
            sqlCommand.Append(" @PathID , ");
            sqlCommand.Append(" @SiteID , ");
            sqlCommand.Append(" @SiteGuid , ");
            sqlCommand.Append(" @Path , ");
            sqlCommand.Append(" @LoweredPath  ");
            sqlCommand.Append(");");


            SqlCeParameter[] arParams = new SqlCeParameter[5];

            arParams[0] = new SqlCeParameter("@PathID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = newPathID;

            arParams[1] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteId;

            arParams[2] = new SqlCeParameter("@Path", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = path;

            arParams[3] = new SqlCeParameter("@LoweredPath", SqlDbType.NVarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = path.ToLower();
            
            arParams[4] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = siteGuid;

            int rowsAffected = Convert.ToInt32(SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams).ToString());

            return newPathID;

        }


        private static Guid GetPathId(int siteId, String path)
        {
            Guid result = Guid.Empty;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(1) PathID ");
            sqlCommand.Append("FROM	mp_SitePaths ");
            sqlCommand.Append("WHERE SiteID = @SiteID AND LoweredPath = @LoweredPath  ; ");

            
            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@LoweredPath", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = path.ToLower();

            string guidString = SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams).ToString();

            if ((guidString != null) && (guidString.Length == 36))
            {
                result = new Guid(guidString);
            }


            return result;
        }

        private static bool PathExists(int siteId, String path)
        {
           
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COALESCE(Count(*), 0) ");
            sqlCommand.Append("FROM	mp_SitePaths ");
            sqlCommand.Append("WHERE SiteID = @SiteID AND LoweredPath = @LoweredPath ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@LoweredPath", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = path.ToLower();

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }


        private static int GetCountOfStateByUser(
            Guid userGuid,
            Guid pathId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_SitePersonalizationPerUser ");
            sqlCommand.Append("WHERE PathID = @PathID AND UserID = @UserID  ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid;

            arParams[1] = new SqlCeParameter("@PathID", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pathId;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        private static int GetCountOfStateByUser(
            Guid userGuid,
            Guid pathId,
            DateTime inactiveSinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(pu.*) ");
            sqlCommand.Append("FROM	mp_SitePersonalizationPerUser pu ");
            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON	pu.UserID = u.UserGuid ");
            sqlCommand.Append("WHERE pu.PathID = @PathID AND pu.UserID = @UserID  ");
            sqlCommand.Append("AND u.LastActivityDate <= @LastActivityDate  ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid;

            arParams[1] = new SqlCeParameter("@PathID", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pathId;

            arParams[2] = new SqlCeParameter("@LastActivityDate", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = inactiveSinceTime;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        private static int GetCountOfStateByUser(
            Guid pathId,
            DateTime inactiveSinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(pu.*) ");
            sqlCommand.Append("FROM	mp_SitePersonalizationPerUser pu ");
            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON	pu.UserID = u.UserGuid ");
            sqlCommand.Append("WHERE pu.PathID = @PathID   ");
            sqlCommand.Append("AND u.LastActivityDate <= @LastActivityDate  ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@PathID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pathId;

            arParams[1] = new SqlCeParameter("@LastActivityDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = inactiveSinceTime;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        private static int GetCountOfStateByUserAllPaths(
            Guid userGuid,
            DateTime inactiveSinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(pu.*) ");
            sqlCommand.Append("FROM	mp_SitePersonalizationPerUser pu ");
            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON	pu.UserID = u.UserGuid ");
            sqlCommand.Append("WHERE pu.UserID = @UserID   ");
            sqlCommand.Append("AND u.LastActivityDate <= @LastActivityDate  ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid;

            arParams[1] = new SqlCeParameter("@LastActivityDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = inactiveSinceTime;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        private static int GetCountOfStateByUserAllPaths(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(pu.*) ");
            sqlCommand.Append("FROM	mp_SitePersonalizationPerUser pu ");
            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON	pu.UserID = u.UserGuid ");
            sqlCommand.Append("WHERE pu.UserID = @UserID   ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        private static int GetCountOfStateByUser(DateTime inactiveSinceTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(pu.*) ");
            sqlCommand.Append("FROM	mp_SitePersonalizationPerUser pu ");
            sqlCommand.Append("JOIN	mp_Users u ");
            sqlCommand.Append("ON pu.UserID = u.UserGuid ");
            sqlCommand.Append("WHERE    ");
            sqlCommand.Append(" u.LastActivityDate <= @LastActivityDate ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@LastActivityDate", SqlDbType.DateTime);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = inactiveSinceTime;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        private static int GetCountOfStateByUser()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(pu.*) ");
            sqlCommand.Append("FROM	mp_SitePersonalizationPerUser pu ");
            sqlCommand.Append(";");

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null));

        }

        private static int GetCountOfStateAllUsers(Guid pathId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_SitePersonalizationAllUsers ");
            sqlCommand.Append("WHERE PathID = @PathID  ; ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PathID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pathId;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        private static int GetCountOfStateAllUsers()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_SitePersonalizationAllUsers ");
            sqlCommand.Append(";");

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null));

        }


    }
}
