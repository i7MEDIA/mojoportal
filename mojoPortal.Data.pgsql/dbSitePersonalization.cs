/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2012-08-11
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.


using System;
using System.Data;
using System.Text;
using log4net;
using Npgsql;

namespace mojoPortal.Data
{
   
    public static class DBSitePersonalization
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DBSitePersonalization));

        
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

        public static int GetCountOfStateByUser(
            Guid userGuid,
            Guid pathId)
        {
            int result = 0;

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter(":userid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter(":pathid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pathId.ToString();

            result = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationperuser_countbyuserpath(:userid,:pathid)",
                    arParams));

            return result;

        }

        public static int GetCountOfStateByUser(
            Guid userGuid,
            Guid pathId,
            DateTime inactiveSinceTime)
        {
            int result = 0;

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];
            
            arParams[0] = new NpgsqlParameter(":userid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter(":pathid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pathId.ToString();

            arParams[2] = new NpgsqlParameter(":sincetime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = inactiveSinceTime;

            result = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationperuser_countbyuserpathsince(:userid,:pathid,:sincetime)",
                    arParams));

            return result;
        }

        public static int GetCountOfStateByUser(
            Guid pathId,
            DateTime inactiveSinceTime)
        {
            int result = 0;

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter(":pathid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pathId.ToString();

            arParams[1] = new NpgsqlParameter(":sincetime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = inactiveSinceTime;

            result = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationperuser_countbypathsince(:pathid,:sincetime)",
                    arParams));

            return result;
        }

        public static int GetCountOfStateByUserAllPaths(
            Guid userGuid,
            DateTime inactiveSinceTime)
        {
            int result = 0;

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter(":userid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter(":sincetime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = inactiveSinceTime;

            result = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationperuser_countbyusersince(:userid,:sincetime)",
                    arParams));

            return result;

        }

        public static int GetCountOfStateByUserAllPaths(Guid userGuid)
        {
            int result = 0;

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":userid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            result = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationperuser_countbyuser(:userid)",
                    arParams));

            return result;

        }

        public static int GetCountOfStateByUser(DateTime inactiveSinceTime)
        {
            int result = 0;

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":sincetime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = inactiveSinceTime;

            result = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationperuser_countinactivesince(:sincetime)",
                    arParams));

            return result;

        }

        public static int GetCountOfStateByUser()
        {
            int result = 0;

            result = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationperuser_countall()"));

            return result;

        }

        public static int GetCountOfStateAllUsers(Guid pathId)
        {
            int result = 0;

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":pathid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pathId.ToString();

            result = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationperuser_countbypath(:pathid)",
                    arParams));

            return result;

        }

        public static int GetCountOfStateAllUsers()
        {
            int result = 0;

            result = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationallusers_countall()"));

            return result;

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
            Guid pathID = GetOrCreatePathId(siteId, path);

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter(":userid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter(":pathid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pathID.ToString();


            byte[] result = null;

            try
            {
                result = (byte[])NpgsqlHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationperuser_getpersonalizationblob(:userid,:pathid)",
                    arParams);
            }
            catch (System.InvalidCastException ex)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("dbPortal.SitePersonalization_GetPersonalizationBlob", ex);
                }
            }

            return result;



        }

        public static void ResetPersonalizationBlob(
            int siteId,
            String path,
            Guid userGuid)
        {
            Guid pathID = GetOrCreatePathId(siteId, path);

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter(":userid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter(":pathid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pathID.ToString();
            
            NpgsqlHelper.ExecuteNonQuery(
                    ConnectionString.GetWriteConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationperuser_deletebypath(:userid,:pathid)",
                    arParams);


        }

        public static void ResetPersonalizationBlob(
            int siteId,
            String path)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE  ");
            sqlCommand.Append("FROM	mp_sitepersonalizationallusers ");
            sqlCommand.Append("WHERE pathid IN (   ");
            sqlCommand.Append("SELECT pathid    ");
            sqlCommand.Append("FROM mp_sitepaths    ");
            sqlCommand.Append("WHERE siteid = " + siteId.ToString() + "  ");
            sqlCommand.Append(" )  ");
            sqlCommand.Append("  ; ");

            //NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            //arParams[0] = new NpgsqlParameter(":siteid", NpgsqlDbType.Integer);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = siteID;


            NpgsqlHelper.ExecuteNonQuery(
                    ConnectionString.GetWriteConnectionString(),
                    CommandType.Text,
                    sqlCommand.ToString(),
                    null);

        }

        public static byte[] GetPersonalizationBlobAllUsers(
            int siteId,
            String path)
        {
            Guid pathID = GetOrCreatePathId(siteId, path);

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":pathid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pathID.ToString();

            byte[] result = null;

            try
            {
                result = (byte[])NpgsqlHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationallusers_getpersonalizationblob(:pathid)",
                    arParams);
            }
            catch (System.InvalidCastException ex)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("dbPortal.SitePersonalization_GetPersonalizationBlobAllUsers", ex);
                }
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

        public static void CreatePersonalizationBlob(
            Guid userGuid,
            Guid pathId,
            byte[] dataBlob,
            DateTime lastUpdateTime)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[5];
            
            arParams[0] = new NpgsqlParameter(":id", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = Guid.NewGuid().ToString();

            arParams[1] = new NpgsqlParameter(":userid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new NpgsqlParameter(":pathid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pathId.ToString();

            arParams[3] = new NpgsqlParameter(":pagesettings", NpgsqlTypes.NpgsqlDbType.Bytea);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = dataBlob;

            arParams[4] = new NpgsqlParameter(":lastupdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = lastUpdateTime;
            
            NpgsqlHelper.ExecuteNonQuery(
                    ConnectionString.GetWriteConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationperuser_insert(:id,:userid,:pathid,:pagesettings,:lastupdate)",
                    arParams);


        }

        public static void CreatePersonalizationBlob(
            Guid pathId,
            byte[] dataBlob,
            DateTime lastUpdateTime)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[3];
            
            arParams[0] = new NpgsqlParameter(":pathid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pathId.ToString();

            arParams[1] = new NpgsqlParameter(":pagesettings", NpgsqlTypes.NpgsqlDbType.Bytea);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = dataBlob;

            arParams[2] = new NpgsqlParameter(":lastupdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = lastUpdateTime;
            
            NpgsqlHelper.ExecuteNonQuery(
                    ConnectionString.GetWriteConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationallusers_insert(:pathid,:pagesettings,:lastupdate)",
                    arParams);


        }

        public static void UpdatePersonalizationBlob(
            Guid userGuid,
            Guid pathId,
            byte[] dataBlob,
            DateTime lastUpdateTime)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[4];
            
            arParams[0] = new NpgsqlParameter(":userid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new NpgsqlParameter(":pathid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pathId.ToString();

            arParams[2] = new NpgsqlParameter(":pagesettings", NpgsqlTypes.NpgsqlDbType.Bytea);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = dataBlob;

            arParams[3] = new NpgsqlParameter(":lastupdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = lastUpdateTime;
            
            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                    ConnectionString.GetWriteConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationperuser_updatepersonalizationblob(:userid,:pathid,:pagesettings,:lastupdate)",
                    arParams));

           


        }

        public static void UpdatePersonalizationBlob(
            Guid pathId,
            byte[] dataBlob,
            DateTime lastUpdateTime)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[3];
            
            arParams[0] = new NpgsqlParameter(":pathid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pathId.ToString();

            arParams[1] = new NpgsqlParameter(":pagesettings", NpgsqlTypes.NpgsqlDbType.Bytea);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = dataBlob;

            arParams[2] = new NpgsqlParameter(":lastupdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = lastUpdateTime;
            
            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                    ConnectionString.GetWriteConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationallusers_updatepersonalizationblob(:pathid,:pagesettings,:lastupdate)",
                    arParams));

            //return (rowsAffected > -1);

        }

        public static bool PersonalizationBlobExists(Guid pathId, Guid userGuid)
        {
            bool result = false;
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter(":pathid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pathId.ToString();

            arParams[1] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            int count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationperuser_countbypath(:pathid,:userguid)",
                    arParams));

            result = (count > 0);

            return result;

        }

        public static bool PersonalizationBlobExists(Guid pathId)
        {
            bool result = false;

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":pathid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pathId.ToString();
           

            int count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepersonalizationallusers_countbypath(:pathid)",
                    arParams));

            result = (count > 0);

            return result;
        }

        public static Guid GetOrCreatePathId(int siteId, String path)
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

        public static Guid GetPathId(int siteId, String path)
        {
            Guid result = Guid.Empty;

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":loweredpath", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = path.ToLower();

            String guidString = NpgsqlHelper.ExecuteScalar(
                    ConnectionString.GetReadConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_sitepaths_getpathid(:siteid,:loweredpath)",
                    arParams).ToString();

            result = new Guid(guidString);

            return result;
        }

        public static bool PathExists(int siteId, String path)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":loweredpath", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = path.ToLower();


            int count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_sitepaths_count(:siteid,:loweredpath)",
                arParams));

            return (count > 0);
        }

        public static Guid CreatePath(int siteId, String path)
        {
            Guid newPathID = Guid.NewGuid();

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter(":pathid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = newPathID;

            arParams[1] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteId;

            arParams[2] = new NpgsqlParameter(":path", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = path;

            arParams[3] = new NpgsqlParameter(":loweredpath", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = path.ToLower();

            NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                 CommandType.StoredProcedure,
                 "mp_sitepaths_insert(:pathid,:siteid,:path,:loweredpath)",
                 arParams);

            return newPathID;

        }

    }
}
