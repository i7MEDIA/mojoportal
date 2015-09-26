using System;
using System.Collections.Generic;
using System.Text;
using SurveyFeature.Data;

namespace SurveyFeature.Business
{
    public class SurveyManager
    {
        private Guid _surveyGuid;

        public SurveyManager(Guid surveyGuid)
        {
            _surveyGuid = surveyGuid; 
        }

        public Guid GetNextSurveyPageGuid(Guid pageGuid)
        {
            return DBSurvey.GetNextPageGuid(pageGuid);
        }

        public Guid GetPreviousSurveyPageGuid(Guid pageGuid)
        {
            return DBSurvey.GetPreviousPageGuid(pageGuid);
        }

        public Guid GetSurveyFirstSurveyPageGuid(Guid surveyGuid)
        {
            return DBSurvey.GetFirstPageGuid(surveyGuid);
        }
    }
}
