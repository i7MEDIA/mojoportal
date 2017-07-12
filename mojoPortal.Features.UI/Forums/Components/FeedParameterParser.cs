// Author:				
// Created:			    2012-10-27
// Last Modified:	    2012-10-27


using mojoPortal.Business;
using mojoPortal.Web.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace mojoPortal.Web.ForumUI
{
    public class FeedParameterParser
    {
        public FeedParameterParser() { }

        private int pageId = -1;

        public int PageId
        {
            get { return pageId; }
        }

        private int moduleId = -1;

        public int ModuleId
        {
            get { return moduleId; }
        }

        private int itemId = -1;

        public int ItemId
        {
            get { return itemId; }
        }

        private int threadId = -1;

        public int ThreadId
        {
            get { return threadId; }
        }

        public void Parse()
        {
            //if (BasePage == null) { return; }

            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
            threadId = WebUtils.ParseInt32FromQueryString("thread", -1);
            
            string f = WebUtils.ParseStringFromQueryString("m", string.Empty);
            if ((f.Length > 0) && (f.Contains("~")))
            {
                List<string> parms = f.SplitOnCharAndTrim('~');

                if (parms.Count >= 1)
                {
                    int.TryParse(parms[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out moduleId);
                }

                if (parms.Count >= 2)
                {
                    int.TryParse(parms[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out itemId);
                }

                if (parms.Count >= 3)
                {
                    int.TryParse(parms[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out threadId);
                }


            }

            // new option to combine params for better seo
            // we don't need moduleid in the url we can get it from the forum
            // but we still need to validate that the page contains the moduleid of the forum
            // pageid must remain a separate param but we can combine itemid and page number into one param
            // that can be split to get the individual params
            // so we can use ?pageid=x&f=y~z where y is itemid and z is pagenumber
            //string f = WebUtils.ParseStringFromQueryString("f", string.Empty);
            //if ((f.Length > 0) && (f.Contains("~")))
            //{
            //    List<string> parms = f.SplitOnCharAndTrim('~');
            //    if (parms.Count == 2)
            //    {
            //        int.TryParse(parms[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out itemId);
            //        int.TryParse(parms[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out pageNumber);
            //    }
            //}

            //forum = new Forum(ItemId);
            //if (forum.ModuleId == -1)
            //{
            //    forum = null;
            //    return;
            //}

            //Module m = BasePage.GetModule(forum.ModuleId, Forum.FeatureGuid);

            //if (m != null)
            //{ //module exists on the page and matches the forum module id
            //    moduleId = forum.ModuleId;
            //    paramsAreValid = true;
            //}


        }
    }
}