// Author:        Rob Henry
// Created:       2007-09-18
// Last Modified: 2018-07-31
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using SurveyFeature.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace SurveyFeature.Business
{
	/// <summary>
	/// Represents a question in a survey
	/// </summary>
	public class Question
	{
		#region Constructors

		public Question()
		{ }


		public Question(Guid questionGuid)
		{
			GetQuestion(questionGuid);
		}

		#endregion


		#region Private Properties

		private Guid questionGuid = Guid.Empty;
		private Guid pageGuid = Guid.Empty;
		private string questionName;
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
		public string QuestionName
		{
			get { return questionName; }
			set { questionName = value; }
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
					questionGuid = new Guid(reader["QuestionGuid"].ToString());
					pageGuid = new Guid(reader["PageGuid"].ToString());
					questionName = reader["QuestionName"].ToString();
					questionText = reader["QuestionText"].ToString();
					questionTypeId = Convert.ToInt32(reader["QuestionTypeId"]);
					answerIsRequired = Convert.ToBoolean(reader["AnswerIsRequired"]);
					questionOrder = Convert.ToInt32(reader["QuestionOrder"]);
					validationMessage = reader["ValidationMessage"].ToString();
				}
			}
		}


		private bool Create()
		{
			questionGuid = Guid.NewGuid();

			int rowsAffected = DBQuestion.Add(
				questionGuid,
				pageGuid,
				questionName,
				questionText,
				questionTypeId,
				answerIsRequired,
				validationMessage
			);

			return (rowsAffected > 0);
		}


		private bool Update()
		{
			return DBQuestion.Update(
				questionGuid,
				pageGuid,
				questionName,
				questionText,
				questionTypeId,
				answerIsRequired,
				questionOrder,
				validationMessage
			);
		}

		#endregion


		#region Public Methods


		public bool Save()
		{
			if (questionGuid != Guid.Empty)
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
			List<Question> pageQuestionList = new List<Question>();

			using (IDataReader reader = DBQuestion.GetAllByPage(pageGuid))
			{
				while (reader.Read())
				{
					Question question = new Question
					{
						questionGuid = new Guid(reader["QuestionGuid"].ToString()),
						pageGuid = new Guid(reader["PageGuid"].ToString()),
						questionName = reader["QuestionName"].ToString(),
						questionText = reader["QuestionText"].ToString(),
						questionTypeId = Convert.ToInt32(reader["QuestionTypeId"]),
						answerIsRequired = Convert.ToBoolean(reader["AnswerIsRequired"]),
						questionOrder = Convert.ToInt32(reader["QuestionOrder"]),
						validationMessage = reader["ValidationMessage"].ToString()
					};

					pageQuestionList.Add(question);
				}
			}

			return pageQuestionList;
		}

		#endregion
	}
}