using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Resources;
using SurveyFeature.Business;

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
            Label lblQuestion = new Label();
            _rdoAnswer = new RadioButtonList();
            RequiredFieldValidator valQuestion = new RequiredFieldValidator();

            lblQuestion.ID = "lbl" + _question.QuestionGuid.ToString().Replace("-", String.Empty);
            _rdoAnswer.ID = "rdo" + _question.QuestionGuid.ToString().Replace("-", String.Empty);
            valQuestion.ID = "val" + _question.QuestionGuid.ToString().Replace("-", String.Empty);

            lblQuestion.Text = _question.QuestionText;
            lblQuestion.AssociatedControlID = _rdoAnswer.ID;

            valQuestion.ControlToValidate = _rdoAnswer.ID;
            valQuestion.Enabled = _question.AnswerIsRequired;

            foreach (QuestionOption option in _options)
            {
                ListItem li = new ListItem(option.Answer);
                if (li.Value == _answer) li.Selected = true;
                _rdoAnswer.Items.Add(li);
            }

            valQuestion.Text = _question.ValidationMessage;

            Controls.Add(lblQuestion);
            Controls.Add(_rdoAnswer);
            Controls.Add(valQuestion);
        }

        #region IQuestion Members

        public string Answer
        {
            get
            {
                EnsureChildControls();
                if (_rdoAnswer.SelectedItem == null) return string.Empty;
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
