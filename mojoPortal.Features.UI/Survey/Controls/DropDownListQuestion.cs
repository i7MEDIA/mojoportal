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
            Label lblQuestion = new Label();
            _ddAnswer = new DropDownList();
            RequiredFieldValidator valQuestion = new RequiredFieldValidator();

            lblQuestion.ID = "lbl" + _question.QuestionGuid.ToString().Replace("-", String.Empty);
            _ddAnswer.ID = "dd" + _question.QuestionGuid.ToString().Replace("-", String.Empty);
            valQuestion.ID = "val" + _question.QuestionGuid.ToString().Replace("-", String.Empty);

            lblQuestion.Text = _question.QuestionText;
            lblQuestion.AssociatedControlID = _ddAnswer.ID;

            valQuestion.ControlToValidate = _ddAnswer.ID;
            valQuestion.Enabled = _question.AnswerIsRequired;

            _ddAnswer.Items.Add(new ListItem(Resources.SurveyResources.DropDownPleaseSelectText, String.Empty));

            foreach (QuestionOption option in _options)
            {
                ListItem li = new ListItem(option.Answer);
                if (li.Value == _answer) li.Selected = true;
                _ddAnswer.Items.Add(li);
            }

            valQuestion.Text = _question.ValidationMessage;

            Controls.Add(lblQuestion);
            Controls.Add(_ddAnswer);
            Controls.Add(valQuestion);
        }

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

