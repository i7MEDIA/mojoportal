using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Resources;
using SurveyFeature.Business;

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
            //EnsureChildControls();
            base.CreateChildControls();
            Label lblQuestion = new Label();
            //TextBox txtQuestion = new TextBox();
            RequiredFieldValidator valQuestion = new RequiredFieldValidator();
            _txtAnswer = new TextBox();
            _txtAnswer.Text = _answer;

            lblQuestion.ID = "lbl" + _question.QuestionGuid.ToString().Replace("-", String.Empty);
            _txtAnswer.ID = "txt" + _question.QuestionGuid.ToString().Replace("-", String.Empty);
            valQuestion.ID = "val" + _question.QuestionGuid.ToString().Replace("-", String.Empty);

            lblQuestion.Text = _question.QuestionText;
            lblQuestion.AssociatedControlID = _txtAnswer.ID;

            valQuestion.ControlToValidate = _txtAnswer.ID;
            valQuestion.Enabled = _question.AnswerIsRequired;

            valQuestion.Text = _question.ValidationMessage;
            
            Controls.Add(lblQuestion);
            Controls.Add(_txtAnswer);
            Controls.Add(valQuestion);
        }

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
