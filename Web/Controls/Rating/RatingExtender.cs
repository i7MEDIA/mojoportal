// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using AjaxControlToolkit;

#region Assembly Resource Attribute
//[assembly: System.Web.UI.WebResource("mojoPortal.Web.Controls.Rating.AjaxRatingBehavior.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("mojoPortal.Web.Controls.Rating.AjaxRatingBehavior.js", "text/javascript")]

#endregion

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// Forked from the original AjaxControlToolkit by  to add support for using a service url and to fix issues in the original logic
    /// </summary>
    [ToolboxItem(false)]
    [TargetControlType(typeof(AjaxRating))]
    [ClientScriptResource("mojoAjaxControlToolkit.AjaxRatingBehavior", "mojoPortal.Web.Controls.Rating.AjaxRatingBehavior")]
    public class AjaxRatingExtender : ExtenderControlBase
    {
        public AjaxRatingExtender()
        {
            EnableClientState = true;
        }

        [ExtenderControlProperty]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]

        public bool AutoPostBack
        {
            get
            {
                return GetPropertyValue("AutoPostback", false);
            }
            set
            {
                SetPropertyValue("AutoPostback", value);
            }
        }

        [DefaultValue(0)]
        [ExtenderControlProperty]
        public int Rating
        {
            get
            {
                string value = ClientState;
                if (value == null)
                {
                    value = "0";
                }
                return Int32.Parse(value, CultureInfo.InvariantCulture);
            }
            set
            {
                ClientState = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        [DefaultValue("")]
        [ExtenderControlProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Justification = "Following ASP.NET AJAX pattern")]
        public string CallbackID
        {
            get { return GetPropertyValue("CallbackID", string.Empty); }
            set { SetPropertyValue("CallbackID", value); }
        }

        [DefaultValue("")]
        [ExtenderControlProperty]
        public string Tag
        {
            get { return GetPropertyValue("Tag", string.Empty); }
            set { SetPropertyValue("Tag", value); }
        }

        // property added by 
        [DefaultValue("")]
        [ExtenderControlProperty]
        public string JsonUrl
        {
            get { return GetPropertyValue("JsonUrl", string.Empty); }
            set { SetPropertyValue("JsonUrl", value); }
        }

        // property added by 
        [DefaultValue("")]
        [ExtenderControlProperty]
        public string ContentId
        {
            get { return GetPropertyValue("ContentId", string.Empty); }
            set { SetPropertyValue("ContentId", value); }
        }

        // property added by 
        [DefaultValue("")]
        [ExtenderControlProperty]
        public string TotalVotesElementId
        {
            get { return GetPropertyValue("TotalVotesElementId", string.Empty); }
            set { SetPropertyValue("TotalVotesElementId", value); }
        }

        // property added by 
        [DefaultValue(false)]
        [ExtenderControlProperty]
        public bool CommentsEnabled
        {
            get { return GetPropertyValue("CommentsEnabled", false); }
            set { SetPropertyValue("CommentsEnabled", value); }
        }
        

        [DefaultValue(0)]
        [ExtenderControlProperty]
        public int RatingDirection
        {
            get { return GetPropertyValue("RatingDirection", 0); }
            set { SetPropertyValue("RatingDirection", value); }

        }

        [DefaultValue(5)]
        [ExtenderControlProperty]
        public int MaxRating
        {
            get { return GetPropertyValue("MaxRating", 5); }
            set { SetPropertyValue("MaxRating", value); }
        }

        //// property added by  2010-02-16
        //[DefaultValue(0)]
        //[ExtenderControlProperty]
        //public int AvgRating
        //{
        //    get { return GetPropertyValue("AvgRating", 0); }
        //    set { SetPropertyValue("AvgRating", value); }
        //}

        [DefaultValue("")]
        [ExtenderControlProperty]
        [RequiredProperty]
        public string StarCssClass
        {
            get { return GetPropertyValue("StarCssClass", String.Empty); }
            set { SetPropertyValue("StarCssClass", value); }
        }

        [DefaultValue(false)]
        [ExtenderControlProperty]
        public bool ReadOnly
        {
            get { return GetPropertyValue("ReadOnly", false); }
            set { SetPropertyValue("ReadOnly", value); }
        }

        [DefaultValue("")]
        [ExtenderControlProperty]
        [RequiredProperty]
        public string FilledStarCssClass
        {
            get { return GetPropertyValue("FilledStarCssClass", string.Empty); }
            set { SetPropertyValue("FilledStarCssClass", value); }
        }

        [DefaultValue("")]
        [ExtenderControlProperty]
        [RequiredProperty]
        public string EmptyStarCssClass
        {
            get { return GetPropertyValue("EmptyStarCssClass", string.Empty); }
            set { SetPropertyValue("EmptyStarCssClass", value); }
        }

        [DefaultValue("")]
        [ExtenderControlProperty]
        [RequiredProperty]
        public string WaitingStarCssClass
        {
            get { return GetPropertyValue("WaitingStarCssClass", string.Empty); }
            set { SetPropertyValue("WaitingStarCssClass", value); }
        }
    }
}


