
// borrowed from BlogEngine.NET
// License: Microsoft Reciprocal License (Ms-RL) 
namespace mojoPortal.Core.API.MetaWeblog
{
    using System;

    /// <summary>
    /// wp Page Struct
    /// For use in metaweblogapi.ashx (in the blog project but it will also handle the html feature).
    /// 
    /// Theory - not yet sure whether we will be able to make this work. 2011-11-13
    /// We will use this struct to simulate a "page" in the word press sense of the word page.
    /// In wordpress a page has a one to one correspondence to the html content on the page
    /// represented here by the description.
    /// In mojoPortal a page is just a container for feature instances of which there can be muliple on any given page
    /// so it is not a one to one correspndence. We will simulate it by defining the page content as the firs tinstance 
    /// of the Html content feature in the center pane of the page for which the user has edit permissions.
    /// We will store the module id of the html instance in the struct in a custom field for ease of updating the instance.
    /// When creating a new page we will create it with a single html content instance in the center pane.
    /// </summary>
    public struct MWAPage
    {
        #region Constants and Fields

        /// <summary>
        ///     Content of Blog Post
        /// </summary>
        public string description;

        /// <summary>
        ///     Link to Blog Post
        /// </summary>
        public string link;

        /// <summary>
        ///     Convert Breaks
        /// </summary>
        public string mt_convert_breaks;

        /// <summary>
        ///     Page keywords
        /// </summary>
        public string mt_keywords;

        /// <summary>
        ///     Display date of Blog Post (DateCreated)
        /// </summary>
        public DateTime pageDate;

        public DateTime pageUtcDate;

        /// <summary>
        ///     PostID Guid in string format
        /// </summary>
        public string pageID;

        /// <summary>
        ///     Page Parent ID
        /// </summary>
        public string pageParentID;

        public string parentTitle;

        //string page_status

        /// <summary>
        ///     PageOrder
        /// </summary>
        public string pageOrder;

        /// <summary>
        ///     Title of Blog Post
        /// </summary>
        public string title;

        /// <summary>
        ///     CommentPolicy (Allow/Deny)
        /// </summary>
        public string commentPolicy;

        
        public string published; //publish or draft

        //http://codex.wordpress.org/XML-RPC_wp
        //TODO: implement support for custom fields
        // ? will live writer round trip these?
        // we need a place to store the module id of the hmtl item
        //array custom_fields : struct string id string key string value

        //public string moduleId;

        //public string itemId;

        public string pageEditRoles;

        public string moduleEditRoles;

        

        #endregion
    }
}