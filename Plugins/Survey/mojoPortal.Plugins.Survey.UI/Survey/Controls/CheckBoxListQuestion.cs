using SurveyFeature.Business;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SurveyFeature.UI
{
	public class CheckBoxListQuestion : CompositeControl, IQuestion
	{
		private Question _question;
		private Collection<QuestionOption> _options;
		private CheckBoxList _chkAnswer;
		private string _answer = String.Empty;


		public CheckBoxListQuestion(Question question, Collection<QuestionOption> options)
		{
			_question = question;
			_options = options;
		}


		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			_chkAnswer = new CheckBoxList
			{
				ID = "chk" + _question.QuestionGuid.ToString().Replace("-", String.Empty)
			};

			Label lblQuestion = new Label
			{
				ID = "lbl" + _question.QuestionGuid.ToString().Replace("-", String.Empty),
				CssClass = "settinglabel",
				Text = _question.QuestionName,
				AssociatedControlID = _chkAnswer.ID
			};

			Literal litQuestionText = new Literal
			{
				Text = _question.QuestionText
			};

			CheckBoxListValidator valQuestion = new CheckBoxListValidator
			{
				ID = "val" + _question.QuestionGuid.ToString().Replace("-", String.Empty),
				ControlToValidate = _chkAnswer.ID,
				Enabled = _question.AnswerIsRequired
			};

			string[] answers = _answer.Split(',');

			foreach (QuestionOption option in _options)
			{
				bool selected = false;

				foreach (string item in answers)
				{
					if (option.Answer == item)
					{
						selected = true;
						break;
					}
				}

				ListItem li = new ListItem(option.Answer)
				{
					Selected = selected
				};

				_chkAnswer.Items.Add(li);
			}

			valQuestion.Text = _question.ValidationMessage;

			Controls.Add(lblQuestion);
			Controls.Add(litQuestionText);
			Controls.Add(_chkAnswer);
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
				StringBuilder answer = new StringBuilder();

				foreach (ListItem li in _chkAnswer.Items)
				{
					if (li.Selected) answer.Append(li.Value + ",");
				}

				return answer.ToString().TrimEnd(',');
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