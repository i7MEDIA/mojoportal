//  Author:                     
//  Created:                    2013-10-24
//	Last Modified:              2013-10-25
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web.Script.Services;
using System.Web.Services;

namespace mojoPortal.Web.Services
{
    /// <summary>
    /// Summary description for UserNameIdAutoComplete
    /// currently has one method that provides data for jqueryui autocomplete to lookup a user and userid
    /// </summary>
    [WebService(Namespace = "http://www.mojoportal.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // allow this Web Service to be called from script, using ASP.NET AJAX or jquery. 
    [ScriptService]
    public class UserLookupService : WebService
    {
        /// <summary>
        /// returns data for jquery ui autocomplete
        /// [{"label":"Client example","value":1},{"label":"Lorem Ipsum","value":2},{"label":"Microsoft","value":3}]
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<AutoCompleteResult> AutoComplete(string term)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return ReturnAccessDenied(); }

            // enforce security 
            if (!WebUser.IsInRoles(siteSettings.RolesThatCanLookupUsers)) { return ReturnAccessDenied(); }

            List<AutoCompleteResult> result = new List<AutoCompleteResult>();

            using (IDataReader reader = SiteUser.GetSmartDropDownData(siteSettings.SiteId, term, WebConfigSettings.UserAutoCompleteRowsToGet))
            {
                while (reader.Read())
                {
                    result.Add(new AutoCompleteResult(reader["SiteUser"].ToString().Trim(), reader["UserID"].ToString()));

                }

            }

            return result;
        }

        /// <summary>
        /// same as AutoComplete but returns UserGuid as value instead of UserID
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<AutoCompleteResult> AutoCompleteUserGuid(string term)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return ReturnAccessDenied(); }

            // enforce security 
            if (!WebUser.IsInRoles(siteSettings.RolesThatCanLookupUsers)) { return ReturnAccessDenied(); }

            List<AutoCompleteResult> result = new List<AutoCompleteResult>();

            using (IDataReader reader = SiteUser.GetSmartDropDownData(siteSettings.SiteId, term, WebConfigSettings.UserAutoCompleteRowsToGet))
            {
                while (reader.Read())
                {
                    result.Add(new AutoCompleteResult(reader["SiteUser"].ToString().Trim(), reader["UserGuid"].ToString()));

                }

            }

            return result;
        }

        /// <summary>
        /// returns Email as label and UserGuid as value
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<AutoCompleteResult> AutoCompleteEmailUserGuid(string term)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return ReturnAccessDenied(); }

            // enforce security 
            if (!WebUser.IsInRoles(siteSettings.RolesThatCanLookupUsers)) { return ReturnAccessDenied(); }

            List<AutoCompleteResult> result = new List<AutoCompleteResult>();

            using (IDataReader reader = SiteUser.EmailLookup(siteSettings.SiteId, term, WebConfigSettings.UserAutoCompleteRowsToGet))
            {
                while (reader.Read())
                {
                    result.Add(new AutoCompleteResult(reader["Email"].ToString().Trim(), reader["UserGuid"].ToString()));

                }

            }

            return result;
        }

        
        private List<AutoCompleteResult> ReturnAccessDenied()
        {
            List<AutoCompleteResult> result = new List<AutoCompleteResult>();
            result.Add(new AutoCompleteResult("Access Denied", "-1"));
            return result;
        }

    }



    public class AutoCompleteResult
    {

        public AutoCompleteResult()
        {
        }
        public AutoCompleteResult(string Label, string Value)
        {
            this.Label = Label;
            this.Value = Value;
        }
        public string Label { get; set; }
        public string Value { get; set; }
    }


}

