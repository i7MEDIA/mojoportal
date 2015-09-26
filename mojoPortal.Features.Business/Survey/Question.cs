/// Author:					Rob Henry
/// Created:				2007-09-18
/// Last Modified:			2009-02-01
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Data;
using SurveyFeature.Data;

namespace SurveyFeature.Business
{
    /// <summary>
    /// Represents a questionin a survey
    /// </summary>
    public class Question
    {

        #region Constructors

        public Question()
        { }


        public Question(
            Guid questionGuid)
        {
            GetQuestion(
                questionGuid);
        }

        #endregion

        #region Private Properties

        private Guid questionGuid = Guid.Empty;
        private Guid pageGuid = Guid.Empty;
        private string questionText;
        private int questionTypeId;
        private bool answerIsRequired;
        private int questionOrder;
        private string validationMessage;

        #endregion

        #region Public Properties

        public Guid QuestionGuid
        {
            get { return questionGuid; }
            set { questionGuid = value; }
        }
        public Guid SurveyPageGuid
        {
            get { return pageGuid; }
            set { pageGuid = value; }
        }
        public string QuestionText
        {
            get { return questionText; }
            set { questionText = value; }
        }
        public int QuestionTypeId
        {
            get { return questionTypeId; }
            set { questionTypeId = value; }
        }
        public bool AnswerIsRequired
        {
            get { return answerIsRequired; }
            set { answerIsRequired = value; }
        }
        public int QuestionOrder
        {
            get { return questionOrder; }
            set { questionOrder = value; }
        }
        public string ValidationMessage
        {
            get { return validationMessage; }
            set { validationMessage = value; }
        }

        #endregion

        #region Private Methods

        private void GetQuestion(Guid guidQuestionGuid)
        {
            using (IDataReader reader = DBQuestion.GetOne(guidQuestionGuid))
            {
                if (reader.Read())
                {
                    this.questionGuid = new Guid(reader["QuestionGuid"].ToString());
                    this.pageGuid = new Guid(reader["PageGuid"].ToString());
                    this.questionText = reader["QuestionText"].ToString();
                    this.questionTypeId = Convert.ToInt32(reader["QuestionTypeId"]);
                    this.answerIsRequired = Convert.ToBoolean(reader["AnswerIsRequired"]);
                    this.questionOrder = Convert.ToInt32(reader["QuestionOrder"]);
                    this.validationMessage = reader["ValidationMessage"].ToString();
                }

            }

        }

        private bool Create()
        {

            this.questionGuid = Guid.NewGuid();

            int rowsAffected = DBQuestion.Add(
                this.questionGuid,
                this.pageGuid,
                this.questionText,
                this.questionTypeId,
                this.answerIsRequired,
                this.validationMessage
                );

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBQuestion.Update(
                this.questionGuid,
                this.pageGuid,
                this.questionText,
                this.questionTypeId,
                this.answerIsRequired,
                this.questionOrder,
                this.validationMessage
                );

        }


        #endregion

        #region Public Methods


        public bool Save()
        {
            if (this.questionGuid != Guid.Empty)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }




        #endregion

        #region Static Methods

        public static bool Delete(Guid questionGuid)
        {
            return DBQuestion.Delete(questionGuid);
        }


        public static List<Question> GetAll(Guid pageGuid)
        {
            List<Question> pageQuestionList
                = new List<Question>();

            using (IDataReader reader = DBQuestion.GetAllByPage(pageGuid))
            {
                while (reader.Read())
                {
                    Question question = new Question();
                    question.questionGuid = new Guid(reader["QuestionGuid"].ToString());
                    question.pageGuid = new Guid(reader["PageGuid"].ToString());
                    question.questionText = reader["QuestionText"].ToString();
                    question.questionTypeId = Convert.ToInt32(reader["QuestionTypeId"]);
                    question.answerIsRequired = Convert.ToBoolean(reader["AnswerIsRequired"]);
                    question.questionOrder = Convert.ToInt32(reader["QuestionOrder"]);
                    question.validationMessage = reader["ValidationMessage"].ToString();
                    pageQuestionList.Add(question);
                }
            }

            return pageQuestionList;

        }


        #endregion

    }

}











