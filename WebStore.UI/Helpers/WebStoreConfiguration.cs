// Author:				    Joe Audette
// Created:			        2010-05-31
// Last Modified:		    2014-06-12
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Globalization;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace WebStore.UI
{
    public class WebStoreConfiguration
    {
        public WebStoreConfiguration()
        { }

        public WebStoreConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

            enableRatingsInProductList = WebUtils.ParseBoolFromHashtable(settings, "EnableContentRatingInProductListSetting", enableRatingsInProductList);

            enableRatingComments = WebUtils.ParseBoolFromHashtable(settings, "EnableRatingCommentsSetting", enableRatingComments);

            indexOffersInSearch = WebUtils.ParseBoolFromHashtable(settings, "IndexOffersSetting", indexOffersInSearch);


            if (settings.Contains("ProductListGroupingSetting"))
            {
                groupingMode = settings["ProductListGroupingSetting"].ToString();
            }

            if (settings.Contains("CartPageFooter"))
            {
                cartPageFooter = settings["CartPageFooter"].ToString();
            }

        }

        private string cartPageFooter = string.Empty;

        public string CartPageFooter
        {
            get { return cartPageFooter; }
        }

        private bool enableRatingsInProductList = false;

        public bool EnableRatingsInProductList
        {
            get { return enableRatingsInProductList; }
        }

        private bool enableRatingComments = false;

        public bool EnableRatingComments
        {
            get { return enableRatingComments; }
        }

        private bool indexOffersInSearch = false;

        public bool IndexOffersInSearch
        {
            get { return indexOffersInSearch; }
        }

        private string groupingMode = "GroupByProduct";

        public string GroupingMode
        {
            get { return groupingMode; }
        }

        public static bool IsDemo
        {
            get { return ConfigHelper.GetBoolProperty("WebStore:IsDemo", false); }
        }

        public static bool LogDoNothingOrderHandler
        {
            get { return ConfigHelper.GetBoolProperty("WebStore:LogDoNothingOrderHandler", false); }
        }

        public static bool UseNoIndexFollowMetaOnLists
        {
            get { return ConfigHelper.GetBoolProperty("WebStore:UseNoIndexFollowMetaOnLists", true); }
        }

        public static bool LogCardTransactionStatus
        {
            get { return ConfigHelper.GetBoolProperty("WebStore:LogCardTransactionStatus", false); }
        }

        public static bool LogCardFailedTransactionStatus
        {
            get { return ConfigHelper.GetBoolProperty("WebStore:LogCardFailedTransactionStatus", false); }
        }

        public static bool DisableOrderConfirmationEmail
        {
            get { return ConfigHelper.GetBoolProperty("WebStore:DisableOrderConfirmationEmail", false); }
        }

        


    }
}