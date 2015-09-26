using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Resources;
using mojoPortal.Web.Controls;
using SurveyFeature.Business;

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
            Label lblQuestion = new Label();
            _dpAnswer = new DatePickerControl();
            _dpAnswer.SkinID = "Survey";
            _dpAnswer.Text = _answer;
            RequiredFieldValidator valQuestion = new RequiredFieldValidator();

            lblQuestion.ID = "lbl" + _question.QuestionGuid.ToString().Replace("-", String.Empty); ;
            _dpAnswer.ID = "dp" + _question.QuestionGuid.ToString().Replace("-", String.Empty);
            valQuestion.ID = "val" + _question.QuestionGuid.ToString().Replace("-", String.Empty);

            lblQuestion.Text = _question.QuestionText;
            lblQuestion.AssociatedControlID = _dpAnswer.ID;

            valQuestion.ControlToValidate = _dpAnswer.ID;
            valQuestion.Enabled = _question.AnswerIsRequired;

            valQuestion.Text = _question.ValidationMessage;

            Controls.Add(lblQuestion);
            Controls.Add(_dpAnswer);
            Controls.Add(valQuestion);
        }

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

