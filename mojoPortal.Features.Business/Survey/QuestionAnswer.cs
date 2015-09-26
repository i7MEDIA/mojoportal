/// Author:					Rob Henry
/// Created:				2007-10-17
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
using System.Data;
using SurveyFeature.Data;

namespace SurveyFeature.Business
{
    /// <summary>
    /// Represents an answer to a srvey question
    /// </summary>
    public class QuestionAnswer
    {

        #region Constructors

        public QuestionAnswer()
        { }


        public QuestionAnswer(
            Guid questionGuid, Guid responseGuid)
        {
            GetSurveyQuestionAnswer(questionGuid, responseGuid);
        }

        #endregion

        #region Private Properties

        private Guid answerGuid = Guid.Empty;
        private Guid questionGuid = Guid.Empty;
        private Guid responseGuid = Guid.Empty;
        private string answer;
        private DateTime answeredDate;

        #endregion

        #region Public Properties

        public Guid AnswerGuid
        {
            get { return answerGuid; }
            set { answerGuid = value; }
        }
        public Guid QuestionGuid
        {
            get { return questionGuid; }
            set { questionGuid = value; }
        }
        public Guid ResponseGuid
        {
            get { return responseGuid; }
            set { responseGuid = value; }
        }
        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }
        public DateTime AnsweredDate
        {
            get { return answeredDate; }
            set { answeredDate = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of SurveyQuestionAnswer.
        /// </summary>
        /// <param name="answerGuid"> answerGuid </param>
        private void GetSurveyQuestionAnswer(
            Guid questionGuid, Guid responseGuid)
        {
            bool found = false;
            using (IDataReader reader = DBQuestionAnswer.GetOne(responseGuid, questionGuid))
            {
                if (reader.Read())
                {
                    this.answerGuid = new Guid(reader["AnswerGuid"].ToString());
                    this.questionGuid = new Guid(reader["QuestionGuid"].ToString());
                    this.responseGuid = new Guid(reader["ResponseGuid"].ToString());
                    this.answer = reader["Answer"].ToString();
                    this.answeredDate = Convert.ToDateTime(reader["AnsweredDate"]);
                    found = true;
                }
            }
            
            if(!found)
            {
                this.answerGuid = new Guid();
                this.questionGuid = questionGuid;
                this.responseGuid = responseGuid;
            }

            

        }

        /// <summary>
        /// Persists a new instance of SurveyQuestionAnswer. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            
            this.answerGuid = Guid.NewGuid();

            int rowsAffected = DBQuestionAnswer.Add(
                this.answerGuid,
                this.questionGuid,
                this.responseGuid,
                this.answer);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of SurveyQuestionAnswer. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBQuestionAnswer.Update(
                this.answerGuid,
                this.questionGuid,
                this.responseGuid,
                this.answer
                );

        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of SurveyQuestionAnswer. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.answerGuid != Guid.Empty)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }

        #endregion

    }

}





