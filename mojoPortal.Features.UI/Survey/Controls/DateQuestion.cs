using mojoPortal.Web.Controls;
using SurveyFeature.Business;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SurveyFeature.UI
{
	public class DateQuestion : CompositeControl, IQuestion
	{
		private Question _question;
		private DatePickerControl _dpAnswer;
		private string _answer = String.Empty;


		public DateQuestion(Question question)
		{
			_question = question;
		}


		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			_dpAnswer = new DatePickerControl
			{
				ID = "dp" + _question.QuestionGuid.ToString().Replace("-", String.Empty),
				Text = _answer,
				SkinID = "Survey"
			};

			Label lblQuestion = new Label
			{
				ID = "lbl" + _question.QuestionGuid.ToString().Replace("-", String.Empty),
				CssClass = "settinglabel",
				Text = _question.QuestionName,
				AssociatedControlID = _dpAnswer.ID
			};

			Literal litQuestionText = new Literal
			{
				Text = _question.QuestionText
			};

			RequiredFieldValidator valQuestion = new RequiredFieldValidator
			{
				ID = "val" + _question.QuestionGuid.ToString().Replace("-", String.Empty),
				Text = _question.ValidationMessage,
				Enabled = _question.AnswerIsRequired,
				ControlToValidate = _dpAnswer.ID
			};

			Controls.Add(lblQuestion);
			Controls.Add(litQuestionText);
			Controls.Add(_dpAnswer);
			Controls.Add(valQuestion);
		}


		public override void RenderBeginTag(HtmlTextWriter writer)
		{ }


		public override void RenderEndTag(HtmlTextWriter writer)
		{ }


		#region IQuestion Members

		public string Answer
		{
			get
			{
				EnsureChildControls();

				return _dpAnswer.Text;
			}
			set
			{
				_answer = value;
			}
		}

		public Guid QuestionGuid
		{
			get
			{
				return _question.QuestionGuid;
			}
		}

		#endregion
	}
}