/// Author:             
/// Created:            2006-01-19
/// Last Modified:      2007-12-15

#if !MONO

using System;
using System.Web.UI.WebControls.WebParts;
using System.Collections;
using System.Collections.Specialized;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web
{
    /// <summary>
    ///
    /// </summary>
    public class mojoPersonalizationProvider : PersonalizationProvider
    {
        private String applicationName = String.Empty;
        private String userPageCookie = "userpagecookie";

        public override string ApplicationName
        {
            get { return applicationName; }
            set { applicationName = value; }
        }

        public override string Description
        {
            get { return base.Description; }
        }

        public override string Name
        {
            get { return base.Name; }
        }

        public override PersonalizationScope DetermineInitialScope(
            WebPartManager webPartManager, 
            PersonalizationState loadedState)
        {
            return base.DetermineInitialScope(webPartManager, loadedState);
        }

        public override IDictionary DetermineUserCapabilities(
            WebPartManager webPartManager)
        {
            return base.DetermineUserCapabilities(webPartManager);
        }

        //private PersonalizationStateInfoCollection FindSharedState(
        //    string path,
        //    int pageIndex,
        //    int pageSize,
        //    out int totalRecords)
        //{
            
        //    totalRecords = 0;

          
        //    PersonalizationStateInfoCollection sharedStateInfoCollection 
        //        = new PersonalizationStateInfoCollection();

        //    if (siteSettings != null)
        //    {
        //        IDataReader reader = SitePersonalization.FindState(
        //            siteSettings.SiteID,
        //            path,
        //            true,
        //            Guid.Empty,
        //            DateTime.MaxValue,
        //            pageIndex,
        //            pageSize);

        //        if (reader != null)
        //        {
                    
        //            while (reader.Read())
        //            {
        //                string returnedPath = reader.GetString(reader.GetOrdinal("Path"));

        //                // Data can be null if there is no data associated with the path
        //                DateTime lastUpdatedDate = (reader.IsDBNull(reader.GetOrdinal("LastActivityDate"))) ? DateTime.MinValue :
        //                                                DateTime.SpecifyKind(reader.GetDateTime(reader.GetOrdinal("LastActivityDate")), 
        //                                                DateTimeKind.Utc);
        //                int size = (reader.IsDBNull(2)) ? 0 : reader.GetInt32(2);
        //                int userDataSize = (reader.IsDBNull(3)) ? 0 : reader.GetInt32(3);
        //                int userCount = (reader.IsDBNull(4)) ? 0 : reader.GetInt32(4);

        //                sharedStateInfoCollection.Add(new SharedPersonalizationStateInfo(
        //                    returnedPath, lastUpdatedDate, size, userDataSize, userCount));
        //            }
                    

        //            // The reader needs to be closed so return value can be accessed
        //            // See MSDN doc for SqlParameter.Direction for details.
        //            reader.Close();
        //            reader = null;
        //        }

        //        // Set the total count at the end after all operations pass
        //        //if (returnValue.Value != null && returnValue.Value is int)
        //        //{
        //        //    totalRecords = (int)returnValue.Value;
        //        //}
        //    }

        //    return sharedStateInfoCollection;
                
        //}

        public override PersonalizationStateInfoCollection FindState(
            PersonalizationScope scope, 
            PersonalizationStateQuery query, 
            int pageIndex, 
            int pageSize, 
            out int totalRecords)
        {

            //mojoPersonalizationProviderHelper.CheckPersonalizationScope(scope);
            //mojoPersonalizationProviderHelper.CheckPageIndexAndSize(pageIndex, pageSize);

            //if (scope == PersonalizationScope.Shared)
            //{
            //    string pathToMatch = null;
            //    if (query != null)
            //    {
            //        pathToMatch = CheckAndTrimString(query.PathToMatch, "query.PathToMatch", false, maxStringLength);
            //    }
            //    return FindSharedState(pathToMatch, pageIndex, pageSize, out totalRecords);
            //}
            //else
            //{
            //    string pathToMatch = null;
            //    DateTime inactiveSinceDate = DefaultInactiveSinceDate;
            //    string usernameToMatch = null;
            //    if (query != null)
            //    {
            //        pathToMatch = CheckAndTrimString(query.PathToMatch, "query.PathToMatch", false, maxStringLength);
            //        inactiveSinceDate = query.UserInactiveSinceDate;
            //        usernameToMatch = CheckAndTrimString(query.UsernameToMatch, "query.UsernameToMatch", false, maxStringLength);
            //    }

            //    return FindUserState(pathToMatch, inactiveSinceDate, usernameToMatch,
            //                         pageIndex, pageSize, out totalRecords);
            //}




            throw new Exception("The method or operation is not implemented.");
        }

        
        public override int GetCountOfState(
            PersonalizationScope scope, 
            PersonalizationStateQuery query)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (siteSettings != null)
            {
                return PersonalizationHelper.GetCountOfState(siteSettings, scope, query);
            }

            return 0;
            
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            this.applicationName = siteSettings.SiteName;


            base.Initialize(name, config);
        }

        public override PersonalizationState LoadPersonalizationState(
            WebPartManager webPartManager, 
            bool ignoreCurrentUser)
        {
            return base.LoadPersonalizationState(webPartManager, ignoreCurrentUser);
        }

  
        protected override void ResetPersonalizationBlob(
            WebPartManager webPartManager, 
            string path, 
            string userName)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            PersonalizationHelper.ResetPersonalizationBlob(
                siteSettings, 
                webPartManager,
                GetUserPagePath(path), 
                userName);
         
        }

        

        public override int ResetState(
            PersonalizationScope scope, 
            string[] paths, 
            string[] usernames)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return 0; }
            if (paths.Length != usernames.Length) { return 0; }
            if (paths.Length == 0) { return 0; }
            if (usernames.Length == 0) { return 0; }
            int i = 0;
            while (i < paths.Length)
            {
                PersonalizationHelper.ResetPersonalizationBlob(
                    siteSettings,
                    paths[i],
                    usernames[i]);

                i += 1;
            }

            return (i + 1);
            
        }

        public override int ResetUserState(string path, DateTime userInactiveSinceDate)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SavePersonalizationState(PersonalizationState state)
        {
            base.SavePersonalizationState(state);
        }

        protected override System.Collections.IList CreateSupportedUserCapabilities()
        {
            return base.CreateSupportedUserCapabilities();
        }

     
        protected override void LoadPersonalizationBlobs(
            WebPartManager webPartManager, 
            string path, 
            string userName, 
            ref byte[] sharedDataBlob, 
            ref byte[] userDataBlob)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (siteSettings != null)
            {
                
                PersonalizationHelper.LoadPersonalizationBlobs(
                    siteSettings, 
                    webPartManager, 
                    GetUserPagePath(path), 
                    userName, 
                    ref sharedDataBlob, 
                    ref userDataBlob);

            }

        }


        public override void ResetPersonalizationState(WebPartManager webPartManager)
        {
            base.ResetPersonalizationState(webPartManager);
            
        }

      
        protected override void SavePersonalizationBlob(
            WebPartManager webPartManager, 
            string path, 
            string userName, 
            byte[] dataBlob)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (siteSettings != null)
            {
                PersonalizationHelper.SavePersonalizationBlob(
                    siteSettings, 
                    webPartManager,
                    GetUserPagePath(path), 
                    userName, 
                    dataBlob);

            }

        }

        private String GetUserPagePath(String path)
        {
            String result = path;
            if (CookieHelper.CookieExists(userPageCookie))
            {
                result = CookieHelper.GetCookieValue(userPageCookie);
            }


            return result;

        }



    }
}
#endif
