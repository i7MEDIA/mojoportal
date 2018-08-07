using System;

namespace SurveyFeature.UI
{
	public interface IQuestion
    {
        string Answer
        {
            get;
            set;
        }

        Guid QuestionGuid
        {
            get;
        }
    }
}
