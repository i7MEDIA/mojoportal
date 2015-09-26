using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

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
