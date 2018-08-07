using SurveyFeature.Business;
using System;
using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SurveyFeature.UI
{
	public class DropDownListQuestion : CompositeControl, IQuestion
	{
		private Question _question;
		private Collection<QuestionOption> _options;
		private DropDownList _ddAnswer;
		private string _answer;


		public DropDownListQuestion(Question question, Collection<QuestionOption> options)
		{
			_question = question;
			_options = options;
		}


		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			_ddAnswer = new DropDownList
			{
				ID = "dd" + _question.QuestionGuid.ToString().Replace("-", String.Empty)
			};

			Label lblQuestion = new Label
			{
				ID = "lbl" + _question.QuestionGuid.ToString().Replace("-", String.Empty),
				CssClass = "settinglabel",
				Text = _question.QuestionName,
				AssociatedControlID = _ddAnswer.ID
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
				ControlToValidate = _ddAnswer.ID
			};

			_ddAnswer.Items.Add(new ListItem(Resources.SurveyResources.DropDownPleaseSelectText, String.Empty));

			foreach (QuestionOption option in _options)
			{
				ListItem li = new ListItem(option.Answer);

				if (li.Value == _answer) li.Selected = true;

				_ddAnswer.Items.Add(li);
			}

			Controls.Add(lblQuestion);
			Controls.Add(litQuestionText);
			Controls.Add(_ddAnswer);
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
				return _ddAnswer.SelectedValue;
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

