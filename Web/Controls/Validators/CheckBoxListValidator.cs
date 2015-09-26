using System.Web;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

//Taken from Scott Mitchell article http://scottonwriting.net/sowblog/posts/10039.aspx

namespace mojoPortal.Web.UI
{
    public class CheckBoxListValidator : BaseValidator
    {
        protected int MinimumNumberOfSelectedCheckBoxes = 1;

        private CheckBoxList _ctrlToValidate;

        protected CheckBoxList CheckBoxListToValidate
        {
            get
            {
                if (_ctrlToValidate == null)
                    _ctrlToValidate = FindControl(this.ControlToValidate) as CheckBoxList;

                return _ctrlToValidate;
            }
        }

        protected override bool ControlPropertiesValid()
        {
            // Make sure ControlToValidate is set
            if (this.ControlToValidate.Length == 0)
                throw new HttpException(string.Format(CultureInfo.CurrentCulture, "The ControlToValidate property of '{0}' cannot be blank.", this.ID));

            // Ensure that the control being validated is a CheckBoxList
            if (CheckBoxListToValidate == null)
                throw new HttpException(string.Format(CultureInfo.CurrentCulture, "The CheckBoxListValidator can only validate controls of type CheckBoxList."));

            // ... and that it has at least MinimumNumberOfSelectedCheckBoxes ListItems
            //if (CheckBoxListToValidate.Items.Count < MinimumNumberOfSelectedCheckBoxes)
            //    throw new HttpException(string.Format(CultureInfo.CurrentCulture, "MinimumNumberOfSelectedCheckBoxes must be set to a value greater than or equal to the number of ListItems; MinimumNumberOfSelectedCheckBoxes is set to {0}, but there are only {1} ListItems in '{2}'", MinimumNumberOfSelectedCheckBoxes, CheckBoxListToValidate.Items.Count, CheckBoxListToValidate.ID));

            return true;    // if we reach here, everything checks out
        }

        protected override bool EvaluateIsValid()
        {
            // Make sure that the CheckBoxList has at least MinimumNumberOfSelectedCheckBoxes ListItems selected
            int selectedItemCount = 0;
            foreach (ListItem cb in CheckBoxListToValidate.Items)
                if (cb.Selected) selectedItemCount++;

            return selectedItemCount >= MinimumNumberOfSelectedCheckBoxes;
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            // Add the client-side code (if needed)
            //if (this.RenderUplevel)
            //{
            //    // Indicate the mustBeChecked value and the client-side function to used for evaluation
            //    // Use AddAttribute if Helpers.EnableLegacyRendering is true; otherwise, use expando attributes
            //    if (this.EnableLegacyRendering())
            //    {
            //        writer.AddAttribute("evaluationfunction", "CheckBoxListValidatorEvaluateIsValid", false);
            //        writer.AddAttribute("minimumNumberOfSelectedCheckBoxes", MinimumNumberOfSelectedCheckBoxes.ToString(CultureInfo.CurrentCulture), false);
            //    }
            //    else
            //    {
            //        this.Page.ClientScript.RegisterExpandoAttribute(this.ClientID, "evaluationfunction", "CheckBoxListValidatorEvaluateIsValid", false);
            //        this.Page.ClientScript.RegisterExpandoAttribute(this.ClientID, "minimumNumberOfSelectedCheckBoxes", MinimumNumberOfSelectedCheckBoxes.ToString(CultureInfo.CurrentCulture), false);
            //    }
            //}
        }

        private bool EnableLegacyRendering()
        {
            return false;

            //bool result;

            //try
            //{
            //    string webConfigFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "web.config");
            //    XmlTextReader webConfigReader = new XmlTextReader(new StreamReader(webConfigFile));
            //    result = ((webConfigReader.ReadToFollowing("xhtmlConformance")) && (webConfigReader.GetAttribute("mode") == "Legacy"));
            //    webConfigReader.Close();
            //}
            //catch(ArgumentOutOfRangeException)
            //{
            //    result = false;
            //}
            //return result;
        }

        //public void RegisterScripts()
        //{
        //    string folderPath = WebConfigurationManager.AppSettings.Get("CSSFriendly-JavaScript-Path");
        //    if (String.IsNullOrEmpty(folderPath))
        //    {
        //        folderPath = "~/ClientScript";
        //    }
        //    string filePath = folderPath.EndsWith("/") ? folderPath + "skmValidators.js" : folderPath + "/skmValidators.js";
        //    //this.Page.ClientScript.RegisterClientScriptInclude(GetType(), GetType().ToString(), this.Page.ResolveUrl(filePath));
        //    this.Page.ClientScript.RegisterClientScriptInclude(typeof(System.Web.UI.Page), "skmValidators", this.Page.ResolveUrl(filePath));
        //}

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //RegisterScripts();

            // Register the client-side function using WebResource.axd (if needed)
            // see: http://aspnet.4guysfromrolla.com/articles/080906-1.aspx
            //if (this.RenderUplevel && this.Page != null && !this.Page.ClientScript.IsClientScriptIncludeRegistered(this.GetType(), "skmValidators"))
            //    this.Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "skmValidators", this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "skmValidators.skmValidators.js"));
        }

    }
}