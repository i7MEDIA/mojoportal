using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using mojoPortal.Business;

namespace mojoPortal.Business.WebHelpers
{
    public sealed class PersonalizationHelper
    {


        public static void ResetPersonalizationBlob(
            SiteSettings siteSettings,
            WebPartManager webPartManager,
            string path,
            string userName)
        {
            //if (userName == null) return;
            //if (userName.Length == 0) return;
            if (siteSettings == null) return;
            if (path == null) return;
            if (path.Length == 0) return;
            if (webPartManager == null) return;

            // delete personlization settings for this user for this path
            if (!String.IsNullOrEmpty(userName))
            {
                SiteUser siteUser = new SiteUser(siteSettings, userName);
                Guid userID = Guid.Empty;
                if (siteUser.UserId > -1)
                {
                    userID = siteUser.UserGuid;
                }

                if (userID != Guid.Empty)
                {
                    SitePersonalization.ResetPersonalizationBlob(
                        siteSettings.SiteId,
                        path,
                        userID);

                    siteUser.UpdateLastActivityTime();

                }
            }
            else
            {
                SitePersonalization.ResetPersonalizationBlob(
                        siteSettings.SiteId,
                        path);
            }

        }

        public static void ResetPersonalizationBlob(
            SiteSettings siteSettings,
            string path,
            string userName)
        {
            //if (userName == null) return;
            //if (userName.Length == 0) return;
            if (siteSettings == null) return;
            if (path == null) return;
            if (path.Length == 0) return;
   

            // delete personlization settings for this user for this path
            if (!String.IsNullOrEmpty(userName))
            {
                SiteUser siteUser = new SiteUser(siteSettings, userName);
                Guid userID = Guid.Empty;
                if (siteUser.UserId > -1)
                {
                    userID = siteUser.UserGuid;
                }

                if (userID != Guid.Empty)
                {
                    SitePersonalization.ResetPersonalizationBlob(
                        siteSettings.SiteId,
                        path,
                        userID);

                    siteUser.UpdateLastActivityTime();

                }
            }
            else
            {
                SitePersonalization.ResetPersonalizationBlob(
                        siteSettings.SiteId,
                        path);
            }

        }


        public static void LoadPersonalizationBlobs(
            SiteSettings siteSettings,
            WebPartManager webPartManager,
            string path,
            string userName,
            ref byte[] sharedDataBlob,
            ref byte[] userDataBlob)
        {
            if (siteSettings != null)
            {
                if ((userName != null) && (userName.Length > 0))
                {
                    SiteUser siteUser = new SiteUser(siteSettings, userName);
                    Guid userID = Guid.Empty;
                    if (siteUser.UserId > 0)
                    {
                        userID = siteUser.UserGuid;
                    }

                    if (userID != Guid.Empty)
                    {
                        userDataBlob = SitePersonalization.GetPersonalizationBlob(
                            siteSettings.SiteId,
                            path,
                            userID);

                        siteUser.UpdateLastActivityTime();


                        sharedDataBlob = SitePersonalization.GetPersonalizationBlobAllUsers(
                            siteSettings.SiteId,
                            path);

                    }
                }
                else
                {
                    //TODO: tracking/personalization for unauthenticated users?

                    sharedDataBlob = SitePersonalization.GetPersonalizationBlobAllUsers(
                            siteSettings.SiteId,
                            path);


                }



            }



        }


        public static void SavePersonalizationBlob(
            SiteSettings siteSettings,
            WebPartManager webPartManager,
            string path,
            string userName,
            byte[] dataBlob)
        {
            if (siteSettings != null)
            {
                if ((userName != null) && (userName.Length > 0))
                {
                    SiteUser siteUser = new SiteUser(siteSettings, userName);
                    Guid userID = Guid.Empty;
                    if (siteUser.UserId > 0)
                    {
                        userID = siteUser.UserGuid;
                    }

                    if (userID != Guid.Empty)
                    {

                        SitePersonalization.SavePersonalizationBlob(
                            siteSettings.SiteId,
                            path,
                            userID,
                            dataBlob,
                            DateTime.Now);

                        siteUser.UpdateLastActivityTime();

                    }
                }
                else
                {
                    SitePersonalization.SavePersonalizationBlobAllUsers(
                        siteSettings.SiteId,
                        path,
                        dataBlob,
                        DateTime.Now);

                }
            }

        }


        public static int GetCountOfState(
            SiteSettings siteSettings,
            PersonalizationScope scope,
            PersonalizationStateQuery query)
        {
            int result = 0;

            if (siteSettings != null)
            {
                Guid userGuid = Guid.Empty;
                if ((query.UsernameToMatch != null) && (query.UsernameToMatch.Length > 0))
                {
                    SiteUser siteUser = new SiteUser(siteSettings, query.UsernameToMatch);
                    if (siteUser.UserId > 0)
                    {
                        userGuid = siteUser.UserGuid;
                    }

                }

                bool allUsersScope = (scope == PersonalizationScope.Shared);
                result = SitePersonalization.GetCountOfState(
                    siteSettings.SiteId,
                    query.PathToMatch,
                    allUsersScope,
                    userGuid,
                    query.UserInactiveSinceDate);


            }


            return result;
        }



    }
}
