using SurveyFeature.Business;
using System;
using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SurveyFeature.UI
{
	public class RadioButtonListQuestion : CompositeControl, IQuestion
	{
		private Question _question;
		private Collection<QuestionOption> _options;
		private RadioButtonList _rdoAnswer;
		private string _answer;


		public RadioButtonListQuestion(Question question, Collection<QuestionOption> options)
		{
			_question = question;
			_options = options;
		}


		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			_rdoAnswer = new RadioButtonList
			{
				ID = "rdo" + _question.QuestionGuid.ToString().Replace("-", String.Empty)
			};

			Label lblQuestion = new Label
			{
				ID = "lbl" + _question.QuestionGuid.ToString().Replace("-", String.Empty),
				CssClass = "settinglabel",
				Text = _question.QuestionName,
				AssociatedControlID = _rdoAnswer.ID
			};

			Literal litQuestionText = new Literal
			{
				Text = _question.QuestionText
			};

			RequiredFieldValidator valQuestion = new RequiredFieldValidator
			{
				ID = "val" + _question.QuestionGuid.ToString().Replace("-", String.Empty),
				Text = _question.ValidationMessage,
				ControlToValidate = _rdoAnswer.ID,
				Enabled = _question.AnswerIsRequired
			};

			foreach (QuestionOption option in _options)
			{
				ListItem li = new ListItem(option.Answer);

				if (li.Value == _answer)
				{
					li.Selected = true;
				}

				_rdoAnswer.Items.Add(li);
			}

			Controls.Add(lblQuestion);
			Controls.Add(litQuestionText);
			Controls.Add(_rdoAnswer);
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

				if (_rdoAnswer.SelectedItem == null)
				{
					return string.Empty;
				}

				return _rdoAnswer.SelectedValue;
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