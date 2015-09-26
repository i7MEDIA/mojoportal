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
            _questionGuid = questionGuid;
            _responseGuid = responseGuid;
            _answer = answer;
            _pageTitle = pageTitle;
            _questionText = questionText;
        }

        #endregion

        #region Private Properties

        private Guid _questionGuid = Guid.Empty;
        private Guid _responseGuid = Guid.Empty;
        private string _answer;
        private string _pageTitle;
        private string _questionText;

        #endregion

        #region Public Properties

        public Guid QuestionGuid
        {
            get { return _questionGuid; }
            set { _questionGuid = value; }
        }
        public Guid ResponseGuid
        {
            get { return _responseGuid; }
            set { _responseGuid = value; }
        }
        public string QuestionText
        {
            get { return _questionText; }
            set { _questionText = value; }
        }
        public string Answer
        {
            get { return _answer; }
            set { _answer = value; }
        }
        public string PageTitle
        {
            get { return _pageTitle; }
            set { _pageTitle = value; }
        }

        #endregion

    }

}
