// Author:        Rob Henry
// Created:       2007-10-17
// Last Modified: 2018-08-03
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
using System.Data;

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


		public QuestionAnswer(Guid questionGuid, Guid responseGuid)
		{
			GetSurveyQuestionAnswer(questionGuid, responseGuid);
		}

		#endregion


		#region Public Properties

		public Guid AnswerGuid { get; set; } = Guid.Empty;
		public Guid QuestionGuid { get; set; } = Guid.Empty;
		public Guid ResponseGuid { get; set; } = Guid.Empty;
		public string Answer { get; set; }
		public DateTime AnsweredDate { get; set; }

		#endregion


		#region Private Methods

		/// <summary>
		/// Gets an instance of SurveyQuestionAnswer.
		/// </summary>
		/// <param name="answerGuid"> answerGuid </param>
		private void GetSurveyQuestionAnswer(Guid questionGuid, Guid responseGuid)
		{
			bool found = false;
			using (IDataReader reader = DBQuestionAnswer.GetOne(responseGuid, questionGuid))
			{
				if (reader.Read())
				{
					AnswerGuid = new Guid(reader["AnswerGuid"].ToString());
					QuestionGuid = new Guid(reader["QuestionGuid"].ToString());
					ResponseGuid = new Guid(reader["ResponseGuid"].ToString());
					Answer = reader["Answer"].ToString();
					AnsweredDate = Convert.ToDateTime(reader["AnsweredDate"]);
					found = true;
				}
			}

			if (!found)
			{
				AnswerGuid = new Guid();
				QuestionGuid = questionGuid;
				ResponseGuid = responseGuid;
			}
		}


		/// <summary>
		/// Persists a new instance of SurveyQuestionAnswer. Returns true on success.
		/// </summary>
		/// <returns></returns>
		private bool Create()
		{
			AnswerGuid = Guid.NewGuid();

			int rowsAffected = DBQuestionAnswer.Add(
				AnswerGuid,
				QuestionGuid,
				ResponseGuid,
				Answer
			);

			return (rowsAffected > 0);
		}


		/// <summary>
		/// Updates this instance of SurveyQuestionAnswer. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		private bool Update()
		{
			return DBQuestionAnswer.Update(
				AnswerGuid,
				QuestionGuid,
				ResponseGuid,
				Answer
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
			if (AnswerGuid != Guid.Empty)
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