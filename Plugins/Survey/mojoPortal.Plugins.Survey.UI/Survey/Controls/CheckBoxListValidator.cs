using System;
using System.ComponentModel;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Taken from Scott Mitchell article http://scottonwriting.net/sowblog/posts/10039.aspx

namespace SurveyFeature.UI
{
	public class CheckBoxListValidator : BaseValidator
	{
		[Description("The minimum number of CheckBoxes that must be checked to be considered valid.")]
		public int MinimumNumberOfSelectedCheckBoxes
		{
			get
			{
				object o = ViewState["MinimumNumberOfSelectedCheckBoxes"];

				if (o == null)
				{
					return 1;
				}
				else
				{
					return (int)o;
				}
			}
			set
			{
				ViewState["MinimumNumberOfSelectedCheckBoxes"] = value;
			}
		}


		private CheckBoxList _ctrlToValidate;


		protected CheckBoxList CheckBoxListToValidate
		{
			get
			{
				if (_ctrlToValidate == null)
				{
					_ctrlToValidate = FindControl(this.ControlToValidate) as CheckBoxList;
				}

				return _ctrlToValidate;
			}
		}


		protected override bool ControlPropertiesValid()
		{
			// Make sure ControlToValidate is set
			if (ControlToValidate.Length == 0)
			{
				throw new HttpException(string.Format(CultureInfo.CurrentCulture, "The ControlToValidate property of '{0}' cannot be blank.", this.ID));
			}

			// Ensure that the control being validated is a CheckBoxList
			if (CheckBoxListToValidate == null)
			{
				throw new HttpException(string.Format(CultureInfo.CurrentCulture, "The CheckBoxListValidator can only validate controls of type CheckBoxList."));
			}

			// ... and that it has at least MinimumNumberOfSelectedCheckBoxes ListItems
			if (CheckBoxListToValidate.Items.Count < MinimumNumberOfSelectedCheckBoxes)
			{
				throw new HttpException(string.Format(CultureInfo.CurrentCulture, "MinimumNumberOfSelectedCheckBoxes must be set to a value greater than or equal to the number of ListItems; MinimumNumberOfSelectedCheckBoxes is set to {0}, but there are only {1} ListItems in '{2}'", MinimumNumberOfSelectedCheckBoxes, CheckBoxListToValidate.Items.Count, CheckBoxListToValidate.ID));
			}

			return true;    // if we reach here, everything checks out
		}


		protected override bool EvaluateIsValid()
		{
			// Make sure that the CheckBoxList has at least MinimumNumberOfSelectedCheckBoxes ListItems selected
			int selectedItemCount = 0;

			foreach (ListItem cb in CheckBoxListToValidate.Items)
			{
				if (cb.Selected) selectedItemCount++;
			}

			return selectedItemCount >= MinimumNumberOfSelectedCheckBoxes;
		}


		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);

			// Add the client-side code (if needed)
			if (RenderUplevel)
			{
				// Indicate the mustBeChecked value and the client-side function to used for evaluation
				// Use AddAttribute if Helpers.EnableLegacyRendering is true; otherwise, use expando attributes
				if (EnableLegacyRendering())
				{
					writer.AddAttribute("evaluationfunction", "CheckBoxListValidatorEvaluateIsValid", false);
					writer.AddAttribute("minimumNumberOfSelectedCheckBoxes", MinimumNumberOfSelectedCheckBoxes.ToString(CultureInfo.CurrentCulture), false);
				}
				else
				{
					Page.ClientScript.RegisterExpandoAttribute(ClientID, "evaluationfunction", "CheckBoxListValidatorEvaluateIsValid", false);
					Page.ClientScript.RegisterExpandoAttribute(ClientID, "minimumNumberOfSelectedCheckBoxes", MinimumNumberOfSelectedCheckBoxes.ToString(CultureInfo.CurrentCulture), false);
				}
			}
		}


		private bool EnableLegacyRendering()
		{
			return false;
		}


		public void RegisterScripts()
		{
			string filePath = "~/ClientScript/skmValidators.js";
			Page.ClientScript.RegisterClientScriptInclude(GetType(), GetType().ToString(), Page.ResolveUrl(filePath));
		}


		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			RegisterScripts();
		}
	}
}