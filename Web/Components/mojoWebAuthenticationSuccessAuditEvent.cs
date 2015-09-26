using System;
using System.Text;
using System.Web;
using System.Web.Management;

namespace mojoPortal.Web
{
    public class mojoWebAuthenticationSuccessAuditEvent : WebAuthenticationSuccessAuditEvent
    {
        private string customCreatedMsg, customRaisedMsg;

        public mojoWebAuthenticationSuccessAuditEvent(
            string msg, 
            object eventSource,
            int eventCode, 
            string userName)
            :
        base(msg, eventSource, eventCode, userName)
        {
           
            customCreatedMsg =
                string.Format("Event created at: {0}",
                DateTime.Now.TimeOfDay.ToString());
        }


        public mojoWebAuthenticationSuccessAuditEvent(
            string msg, 
            object eventSource,
            int eventCode, 
            int detailedCode, 
            string userName)
            :
        base(msg, eventSource, eventCode, detailedCode, userName)
        {
            customCreatedMsg =
            string.Format("Event created at: {0}",
                DateTime.Now.TimeOfDay.ToString());
        }

        public override void Raise()
        {
            customRaisedMsg =
                string.Format("Event raised at: {0}",
                DateTime.Now.TimeOfDay.ToString());

            WebBaseEvent.Raise(this);
        }

        public WebRequestInformation GetRequestInformation()
        {
            return RequestInformation;
        }

        public override void FormatCustomEventDetails(WebEventFormatter formatter)
        {
            base.FormatCustomEventDetails(formatter);

            formatter.AppendLine("");

            formatter.IndentationLevel += 1;
            formatter.AppendLine(
                "* mojoWebAuthenticationSuccessAuditEvent Start *");
            formatter.AppendLine(string.Format("Request path: {0}",
                RequestInformation.RequestPath));
            formatter.AppendLine(string.Format("Request Url: {0}",
                RequestInformation.RequestUrl));

            formatter.AppendLine(customCreatedMsg);
            formatter.AppendLine(customRaisedMsg);

            formatter.AppendLine(
                "* mojoWebAuthenticationSuccessAuditEvent End *");

            formatter.IndentationLevel -= 1;

        }
    }

}