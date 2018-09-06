// Author:        Rob Henry
// Created:       2007-09-18
// Last Modified: 2018-09-06
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;

namespace SurveyFeature.Business
{
	/// <summary>
	/// Represents the set of answers for a survey
	/// </summary>
	public class Result
	{
		#region Constructors

		public Result()
		{ }

		public Result(
			Guid questionGuid, Guid responseGuid, string answer, string pageTitle, string questionText)
		{
			QuestionGuid = questionGuid;
			ResponseGuid = responseGuid;
			Answer = answer;
			PageTitle = pageTitle;
			QuestionName = questionText;
		}

		#endregion


		#region Public Properties

		public Guid QuestionGuid { get; set; } = Guid.Empty;
		public Guid ResponseGuid { get; set; } = Guid.Empty;
		public string QuestionName { get; set; }
		public string QuestionText { get; set; }
		public string Answer { get; set; }
		public string PageTitle { get; set; }

		#endregion
	}
}