using SurveyFeature.Business;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SurveyFeature.UI
{
	public class TextBoxQuestion : CompositeControl, IQuestion
	{
		private Question _question;
		private TextBox _txtAnswer;
		private string _answer;


		public TextBoxQuestion(Question question)
		{
			_question = question;
		}


		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			_txtAnswer = new TextBox
			{
				Text = _answer,
				ID = "txt" + _question.QuestionGuid.ToString().Replace("-", String.Empty)
			};

			Label lblQuestion = new Label
			{
				ID = "lbl" + _question.QuestionGuid.ToString().Replace("-", String.Empty),
				CssClass = "settinglabel",
				Text = _question.QuestionName,
				AssociatedControlID = _txtAnswer.ID
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
				ControlToValidate = _txtAnswer.ID
			};

			Controls.Add(lblQuestion);
			Controls.Add(litQuestionText);
			Controls.Add(_txtAnswer);
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

				return _txtAnswer.Text;
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