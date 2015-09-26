using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Resources;
using SurveyFeature.Business;

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
            Label lblQuestion = new Label();
            CheckBoxListValidator valQuestion = new CheckBoxListValidator();
            _chkAnswer = new CheckBoxList();

            lblQuestion.ID = "lbl" + _question.QuestionGuid.ToString().Replace("-", String.Empty);
            _chkAnswer.ID = "chk" + _question.QuestionGuid.ToString().Replace("-", String.Empty);
            valQuestion.ID = "val" + _question.QuestionGuid.ToString().Replace("-", String.Empty);

            lblQuestion.Text = _question.QuestionText;
            lblQuestion.AssociatedControlID = _chkAnswer.ID;

            valQuestion.ControlToValidate = _chkAnswer.ID;
            valQuestion.Enabled = _question.AnswerIsRequired;

            string[] answers;
            answers = _answer.Split(',');

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

                ListItem li = new ListItem(option.Answer);
                li.Selected = selected;

                _chkAnswer.Items.Add(li);
            }

            valQuestion.Text = _question.ValidationMessage;

            Controls.Add(lblQuestion);
            Controls.Add(_chkAnswer);
            Controls.Add(valQuestion);
        }

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
