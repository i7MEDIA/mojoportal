// Author:             
// Created:            2006-04-10
// Last Modified:      2009-01-09

using System;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// A utility for managing user personalization via the MyPage feature aka WebParts
    /// </summary>
    public sealed class SitePersonalization
    {

        #region Most Important Personalization methods

        public static void ResetPersonalizationBlob(int siteId, String path, Guid userId)
        {
            DBSitePersonalization.ResetPersonalizationBlob(
                    siteId,
                    path,
                    userId);
        }

        public static void ResetPersonalizationBlob(
            int siteId,
            String path)
        {
            DBSitePersonalization.ResetPersonalizationBlob(siteId, path);
        }

        public static byte[] GetPersonalizationBlob(int siteId, String path, Guid userId)
        {
            return DBSitePersonalization.GetPersonalizationBlob(
                            siteId,
                            path,
                            userId);

        }

        public static byte[] GetPersonalizationBlobAllUsers(int siteId, String path)
        {
            return DBSitePersonalization.GetPersonalizationBlobAllUsers(siteId, path);

        }

        public static void SavePersonalizationBlob(
               int siteId,
               String path,
               Guid userId,
               byte[] dataBlob,
               DateTime dateTime)
        {
            DBSitePersonalization.SavePersonalizationBlob(
                            siteId,
                            path,
                            userId,
                            dataBlob,
                            dateTime);


        }

        public static void SavePersonalizationBlobAllUsers(
             int siteId,
             String path,
             byte[] dataBlob,
             DateTime dateTime)
        {
            DBSitePersonalization.SavePersonalizationBlobAllUsers(
                            siteId,
                            path,
                            dataBlob,
                            dateTime);

        }


        public static int GetCountOfState(
           int siteId,
           String pathToMatch,
           bool allUsersScope,
           Guid userGuid,
           DateTime userInactiveSinceDate)
        {
            return DBSitePersonalization.GetCountOfState(
                    siteId,
                    pathToMatch,
                    allUsersScope,
                    userGuid,
                    userInactiveSinceDate);


        }

        


        


        


        //public static IDataReader FindState(
        //    int siteID,
        //    String path,
        //    bool allUserScope,
        //    Guid userGuid,
        //    DateTime inactiveSince,
        //    int pageIndex,
        //    int pageSize)
        //{

        //    return dbSitePersonalization.FindState(
        //        siteID,
        //        path,
        //        allUserScope,
        //        userGuid,
        //        inactiveSince,
        //        pageIndex,
        //        pageSize);

        //}


        #endregion


    }
}
