/// Author:					Rob Henry
/// Created:				2007-09-15
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
using System.Collections.ObjectModel;
using System.Data;
using SurveyFeature.Data;

namespace SurveyFeature.Business
{
    /// <summary>
    /// Represents an option in a multi option question
    /// </summary>
    public class QuestionOption
    {

        #region Constructors

        public QuestionOption()
        { }


        public QuestionOption(
            Guid questionOptionGuid)
        {
            GetQuestionOption(
                questionOptionGuid);
        }

        #endregion

        #region Private Properties

        private Guid questionOptionGuid = Guid.Empty;
        private Guid questionGuid = Guid.Empty;
        private string answer;
        private int order;

        #endregion

        #region Public Properties

        public Guid QuestionOptionGuid
        {
            get { return questionOptionGuid; }
            set { questionOptionGuid = value; }
        }
        public Guid QuestionGuid
        {
            get { return questionGuid; }
            set { questionGuid = value; }
        }
        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }
        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        #endregion

        #region Private Methods

        private void GetQuestionOption(Guid questionOptionGuid)
        {
            using (IDataReader reader = DBQuestionOption.GetOne(questionOptionGuid))
            {
                if (reader.Read())
                {
                    this.questionOptionGuid = new Guid(reader["QuestionOptionGuid"].ToString());
                    this.questionGuid = new Guid(reader["QuestionGuid"].ToString());
                    this.answer = reader["Answer"].ToString();
                    this.order = Convert.ToInt32(reader["Order"]);

                }

            }

        }

        private bool Create()
        {
            
            this.questionOptionGuid = Guid.NewGuid();

            int rowsAffected = DBQuestionOption.Add(
                this.questionOptionGuid,
                this.questionGuid,
                this.answer,
                this.order);

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBQuestionOption.Update(
                this.questionOptionGuid,
                this.questionGuid,
                this.answer,
                this.order);

        }


        #endregion

        #region Public Methods


        public bool Save()
        {
            if (this.questionOptionGuid != Guid.Empty)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }

        public bool Delete()
        {
            if (QuestionOptionGuid == Guid.Empty) return false;

            return DBQuestionOption.Delete(this.questionOptionGuid);
        }

        #endregion

        #region Static Methods


        public static Collection<QuestionOption> GetAll(Guid questionGuid)
        {
            Collection<QuestionOption> questionOptionList
                = new Collection<QuestionOption>();

            using (IDataReader reader = DBQuestionOption.GetAll(questionGuid))
            {
                while (reader.Read())
                {
                    QuestionOption questionOption = new QuestionOption();
                    questionOption.questionOptionGuid = new Guid(reader["QuestionOptionGuid"].ToString());
                    questionOption.questionGuid = new Guid(reader["QuestionGuid"].ToString());
                    questionOption.answer = reader["Answer"].ToString();
                    questionOption.order = Convert.ToInt32(reader["Order"]);
                    questionOptionList.Add(questionOption);
                }
            }

            return questionOptionList;

        }

        #endregion

    }

}











